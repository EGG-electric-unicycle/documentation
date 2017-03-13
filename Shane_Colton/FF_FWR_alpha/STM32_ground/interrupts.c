/*
Copyright (C) 2011 by Shane W. Colton

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

#include <ST\stm32f10x.h>
#include "definitions.h"
#include "interrupts.h"

extern const unsigned char SIN8LUT[256];

extern volatile int32_t ib_adc, ic_adc, vdc_adc, thr_adc;

extern volatile int32_t ib_zero, ic_zero;
extern volatile int32_t ia_raw, ib_raw, ic_raw, idc_raw;
extern volatile int32_t ia_int, ib_int, ic_int, idc_int;

extern volatile unsigned int vdc_raw;
extern volatile signed int vdc_int;

extern volatile unsigned char hallstate, old_hallstate;
extern volatile unsigned char fluxstate, old_fluxstate;

extern volatile unsigned char faultstate;
extern volatile unsigned char drivestate;

extern volatile unsigned char mag, phase;
extern volatile unsigned char v_idx;

extern volatile unsigned char loop_go;

// Data protocol variables.
extern volatile unsigned char tx_buffer[TX_LEN];
extern volatile unsigned char rx_buffer[RX_LEN];
extern volatile unsigned char tx_i;
extern volatile unsigned char rx_i;

// position and speed variables for flux observer
extern volatile unsigned int v_idx_abs_f, v_idx_inc_f, v_idx_int_f;
extern volatile unsigned int speed_f, inv_speed_f;
extern volatile unsigned int inv_speed_timer_f;

// position and speed variables for hall effect sensors
extern volatile unsigned int v_idx_abs_h, v_idx_inc_h, v_idx_int_h;
extern volatile unsigned int speed_h, inv_speed_h;
extern volatile unsigned int inv_speed_timer_h;

// position and speed variables for open-loop startup
extern volatile unsigned int v_idx_int_s;
extern volatile unsigned int speed_s;

extern volatile unsigned int atemp, btemp, ctemp, ntemp;
extern volatile signed int vir_a, vir_b, vir_c, vir_temp;
extern volatile signed int fa_int, fb_int, fc_int;

extern volatile unsigned char ADC1_ch[2];
extern volatile unsigned char ADC2_ch[2];
extern volatile unsigned char ADC_idx;

extern volatile signed int pwm_in;             // PWM input [us]
extern volatile unsigned int sliderp;

extern volatile unsigned int ae;

extern volatile unsigned int gd_config_flag;

volatile unsigned int loop_timer = 0;
volatile unsigned int fault_timer = 0;

volatile unsigned char revcount = 0;

volatile unsigned int oc_fault_counter = 0;

// SysTick interrupt handler runs at 10kHz.
// void SysTick_Handler()
// {
// }

void fastloop()
{
  signed int va_int, vb_int, vc_int;

  // Reload the watchdog timer.
  IWDG->KR = 0xAAAA;

  // Divider for the slow loop flag.
  loop_timer = (loop_timer + 1) % LOOP_INTERVAL;
  if(loop_timer == 0) { loop_go = 1; }

  // ADC converting.
  // --------------------------------------------------------------------------
  ntemp = (255 * mag) >> 7; // neutral voltage [0-1023]

  ib_zero = 2028;       // [lsb] (?)
  ic_zero = 2030;       // [lsb] (?)

  // Sample current sensor ADC channels.                  MAX BITS: UNITS:
  ib_raw = ib_adc - ib_zero;                              // [+-12] [lsb]
  ic_raw = ic_adc - ic_zero;                              // [+-12] [lsb]
  ia_raw = -(ib_raw + ic_raw);                            // [+-13] [lsb]

  // Convert to mA.
  ia_int = ia_raw * KI;                                   // [+-18] [mA]
  ib_int = ib_raw * KI;                                   // [+-19] [mA]
  ic_int = ic_raw * KI;                                   // [+-18] [mA]

  // Sample DC voltage ADC channel.
  vdc_raw = vdc_adc;                                      // [+12]  [lsb]

  // Convert to mV.
  vdc_int = vdc_raw * KV;                                 // [+17]  [mV]
  va_int = ((signed int) atemp - (signed int) ntemp);     // [+-10] [lsb]
  vb_int = ((signed int) btemp - (signed int) ntemp);     // [+-10] [lsb]
  vc_int = ((signed int) ctemp - (signed int) ntemp);     // [+-10] [lsb]
  va_int = va_int * vdc_int / 1023;                       // [+-17] [mV]
  vb_int = vb_int * vdc_int / 1023;                       // [+-17] [mV]
  vc_int = vc_int * vdc_int / 1023;                       // [+-17] [mV]
  // Intermediate result is +-27 bits!
  // --------------------------------------------------------------------------

  // Flux observer.
  // --------------------------------------------------------------------------
  // Integral (low-pass filter) of (V - IR).
  vir_temp = ia_int * R;                                  // [+-15] [mV]
  vir_temp = va_int - vir_temp;                           // [+-18] [mV]
  vir_temp = vir_temp * TVIR;                             // [+-16] [uWb]
  vir_a -= vir_a * AVIR;
  vir_a += vir_temp * AVIR;
  if(vir_a > VIR_SAT) { vir_a = VIR_SAT; }
  if(vir_a < -VIR_SAT) { vir_a = -VIR_SAT; }

  vir_temp = ib_int * R;                                  // [+-16] [mV]
  vir_temp = vb_int - vir_temp;                           // [+-18] [mV]
  vir_temp = vir_temp * TVIR;                             // [+-16] [uWb]
  vir_b -= vir_b * AVIR;
  vir_b += vir_temp * AVIR;
  if(vir_b > VIR_SAT) { vir_b = VIR_SAT; }
  if(vir_b < -VIR_SAT) { vir_b = -VIR_SAT; }

  vir_temp = ic_int * R;                                  // [+-15] [mV]
  vir_temp = vc_int - vir_temp;                           // [+-18] [mV]
  vir_temp = vir_temp * TVIR;                             // [+-16] [uWb]
  vir_c -= vir_c * AVIR;
  vir_c += vir_temp * AVIR;
  if(vir_c > VIR_SAT) { vir_c = VIR_SAT; }
  if(vir_c < -VIR_SAT) { vir_c = -VIR_SAT; }

  // Flux.
  fa_int = vir_a - ia_int * L;                            // [?]    [uWb]
  if(fa_int > F_SAT) { fa_int = F_SAT; }
  if(fa_int < -F_SAT) { fa_int = -F_SAT; }
  fb_int = vir_b - ib_int * L;                            // [?]    [uWb]
  if(fb_int > F_SAT) { fb_int = F_SAT; }
  if(fb_int < -F_SAT) { fb_int = -F_SAT; }
  fc_int = vir_c - ic_int * L;                            // [?]    [uWb]
  if(fc_int > F_SAT) { fc_int = F_SAT; }
  if(fc_int < -F_SAT) { fc_int = -F_SAT; }
  // --------------------------------------------------------------------------

  // Virtual Hall effect sensors.
  // --------------------------------------------------------------------------
  if((fa_int > FLUX_THRESHOLD) && ((fluxstate & PHASE_A) == 0))
  { fluxstate |= PHASE_A; do_flux_encoder(); }
  else if((fa_int < -FLUX_THRESHOLD) && ((fluxstate & PHASE_A) != 0))
  { fluxstate &= ~PHASE_A; do_flux_encoder(); }

  if((fb_int > FLUX_THRESHOLD) && ((fluxstate & PHASE_B) == 0))
  { fluxstate |= PHASE_B; do_flux_encoder(); }
  else if((fb_int < -FLUX_THRESHOLD) && ((fluxstate & PHASE_B) != 0))
  { fluxstate &= ~PHASE_B; do_flux_encoder(); }

  if((fc_int > FLUX_THRESHOLD) && ((fluxstate & PHASE_C) == 0))
  { fluxstate |= PHASE_C; do_flux_encoder(); }
  else if((fc_int < -FLUX_THRESHOLD) && ((fluxstate & PHASE_C) != 0))
  { fluxstate &= ~PHASE_C; do_flux_encoder(); }
  // --------------------------------------------------------------------------

  // Sensor-based FOC fast loop.
  // --------------------------------------------------------------------------
  // Increment motor speed timer.
  if(inv_speed_timer_h < 7813) { inv_speed_timer_h++; }
  else { inv_speed_h = inv_speed_timer_h; speed_h = 0; }

  if(inv_speed_timer_f < 7813) { inv_speed_timer_f++; }
  else { inv_speed_f = inv_speed_timer_f; speed_f = 0; }

  // Check the state of the Hall-effect sensors.
  // do_hall_encoder();

  // Intepolation based on estimated speed.
  // Do not allow more than 60deg of interpolation between sensor updates.
  if(v_idx_inc_h < INT_60_DEG) {v_idx_inc_h += speed_h; }
  if(v_idx_inc_f < INT_90_DEG) {v_idx_inc_f += speed_f; }

  // Sine table index is the absolute position
  // plus the incremental interpolation.
  v_idx_int_h = v_idx_abs_h + v_idx_inc_h;
  v_idx_int_f = (v_idx_abs_f + v_idx_inc_f) % 65536;

  // Update the open-loop startup position estimate.
  v_idx_int_s = (v_idx_int_s + speed_s) % 65536;

  // Shift down to 8-bit for look-up table.
  if(RUN == RUN)
  {
    if(1)
    { v_idx = (unsigned char)(v_idx_int_f >> 8) + phase; }
    else
    { v_idx = (unsigned char)(v_idx_int_h >> 8) + phase; }
  }
  else
  {
    v_idx = (unsigned char)(v_idx_int_s >> 8);
  }

  // Start by looking up the 8-bit sine values for each phase.
  // Range: 8-bit, 0-255
  atemp = SIN8LUT[v_idx];
  btemp = SIN8LUT[(unsigned char)(v_idx - CHAR_120_DEG)];
  ctemp = SIN8LUT[(unsigned char)(v_idx + CHAR_120_DEG)];

  // Scale by the mag value.
  // Final range: 10-bit, 0-1023
  atemp = (atemp * mag) >> 6;
  btemp = (btemp * mag) >> 6;
  ctemp = (ctemp * mag) >> 6;

  // Load the new PWM values into timer compare buffers.
  // They will be latched at the next timer reset.
  APWM = atemp;
  BPWM = btemp;
  CPWM = ctemp;
  // --------------------------------------------------------------------------

  // Fast fault checking state machine.
  // --------------------------------------------------------------------------
  if((faultstate & TEST_FLAG) != 0)
  {
    // Test mode: Activate gate driver for test period and ignore faults.
    // (The amplifiers need time to warm up.)
    NOT_KILL;
    fault_timer++;
    if(fault_timer >= 2*TEST_PERIOD)
    {
      faultstate = 0;
      gd_config_flag = 0;
      fault_timer = 0;
    }
    else if((fault_timer >= TEST_PERIOD) && (gd_config_flag == 0))
    {
      gd_config_flag = 1;
    }
  }
  else
  {
    // If not in test/warm-up period, check for faults.
    if((ia_int > OC_ABC) || (ia_int < -OC_ABC)) { oc_fault_counter += 10; }
    if((ib_int > OC_ABC) || (ib_int < -OC_ABC)) { oc_fault_counter += 10; }
    if((ic_int > OC_ABC) || (ic_int < -OC_ABC)) { oc_fault_counter += 10; }
    if(oc_fault_counter >= 750)
    {
      faultstate |= OC_ABC_FLAG;
      oc_fault_counter = 0;
    }
    else if(oc_fault_counter > 0)
    {
      oc_fault_counter--;
    }
    // if(idc_int > OC_DCP) { faultstate |= OC_DCP_FLAG; }
    // if(idc_int < -OC_DCN) { faultstate |= OC_DCN_FLAG; }
    if(vdc_int > OV_DC) { faultstate |= OV_DC_FLAG; }
    if(vdc_int < UV_DC) { faultstate |= UV_DC_FLAG; }

    if((faultstate != 0) && (fault_timer == 0))
    {
      // If a new faults comes in, KILL gate driver and start timeout period.
      KILL;
      fault_timer++;
    }
    else if((faultstate != 0) && (fault_timer < FAULT_TIMEOUT))
    {
      // Kill time for timeout period.
      fault_timer++;
    }
    else if(faultstate != 0)
    {
      // After timeout period, enter test mode.
      fault_timer = 0;
      faultstate |= TEST_FLAG;
    }
  }
  // --------------------------------------------------------------------------
}

// USART1 Tx and Rx interrupt handler.
void USART1_IRQHandler()
{
  // The interrupt may be from Tx, Rx, or both.

  // If there is a pending Tx interrupt...
  if((USART1->SR & USART_SR_TXE) != 0 )
  {
    // If the Tx index is less than the Tx packet length...
    if(tx_i < (TX_LEN - 1))
    {
      // ...increment and transmit the next byte.
      tx_i++;
      USART1->DR = tx_buffer[tx_i];
    }
    else
    {
      // ...disable Tx interrupt until next packet.
      USART1->CR1 &= ~USART_CR1_TXEIE;
    }
  }

  // If there is a pending Rx interrupt...
  if((USART1->SR & USART_SR_RXNE) != 0)
  {
    // Triggers EVERY time a byte is received on the UART
    unsigned char rx_byte;
    rx_byte = USART1->DR;    // read byte

    // If START byte is received...
    if(rx_byte == START)
    {
      // Start a new packet buffer.
      rx_buffer[0] = START;
      rx_i = 1;
    }
    // If the packet is not full...
    else if(rx_i < RX_LEN)
    {
      rx_buffer[rx_i] = rx_byte;
      rx_i++;
    }
    // If the packets IS full (rx_i == RX_LEN),
    // main() will trigger rx().
  }
}

void TIM1_CC_IRQHandler()
{
  pwm_in = ((signed int) TIM1->CCR4 - (signed int) TIM1->CCR3);
  if(pwm_in < 0)
  {
    // handles the rollover condition for TIM1
    pwm_in = 30000 + pwm_in;
  }
}


void ADC1_2_IRQHandler()
{
  DEBUG_HIGH;
  switch(ADC_idx)
  {
  case 0:
    ib_adc = (ADC1->DR) & 0x0FFF;
    ic_adc = (ADC2->DR) & 0x0FFF;
    break;
  case 1:
    vdc_adc = (ADC1->DR) & 0x0FFF;
    thr_adc = (ADC2->DR) & 0x0FFF;
    break;
  }

  if(ADC_idx < 1)
  {
    ADC_idx++;                                // increment index
    ADC1->SQR3 = ADC1_ch[ADC_idx];            // set channel
    ADC2->SQR3 = ADC2_ch[ADC_idx];
    ADC1->CR2 |= ADC_CR2_ADON;                // start next conversion
    ADC2->CR2 |= ADC_CR2_ADON;
  }
  else
  {
    ADC_idx = 0;
    ADC1->SQR3 = ADC1_ch[ADC_idx];            // set channel
    ADC2->SQR3 = ADC2_ch[ADC_idx];
    fastloop();
  }
  DEBUG_LOW;
}

// Flux observer state processing.
void do_flux_encoder()
{
  // If a transition has occurred.
  if(fluxstate != old_fluxstate)
  {
    /* Encoder Table (Gray Code)

    State #   ABC     Decimal Value
      1       001           1
      2       011           3
      3       010           2
      4       110           6
      5       100           4
      6       101           5

    */

    // Set the absolute index for sine wave commutation.
    switch(fluxstate)
    {
    case 5:
      if(old_fluxstate == 4)
      {
        // v_idx_abs_f = 16384 ;
        v_idx_abs_f = safe_filter(16384, v_idx_int_f, ae);
      }
      break;
    case 1:
      if(old_fluxstate == 5)
      {
        // v_idx_abs_f = 27307;
        v_idx_abs_f = safe_filter(27307, v_idx_int_f, ae);
      }
      break;
    case 3:
      if(old_fluxstate == 1)
      {
        // v_idx_abs_f = 38229;
        v_idx_abs_f = safe_filter(38229, v_idx_int_f, ae);
      }
      break;
    case 2:
      if(old_fluxstate == 3)
      {
        // v_idx_abs_f = 49152;
        v_idx_abs_f = safe_filter(49152, v_idx_int_f, ae);
      }
      break;
    case 6:
      if(old_fluxstate == 2)
      {
        // v_idx_abs_f = 60075;
        v_idx_abs_f = safe_filter(60075, v_idx_int_f, ae);
      }
      break;
    case 4:
      if(old_fluxstate == 6)
      {
        // v_idx_abs_f = 5461;
        v_idx_abs_f = safe_filter(5461, v_idx_int_f, ae);
        revcount = (revcount + 1) % 2;
        if(revcount == 0)
        {
          if(inv_speed_timer_f >= 45)
          { inv_speed_f = inv_speed_timer_f; }
          inv_speed_timer_f = 0;
          speed_f = 2*0xFFFF / inv_speed_f;   // 7*0xFFFF = 458745
          if(speed_f > 500)
          { ae = 10000; }
          else
          { ae = 10000; }
        }
      }
      break;
    default:
      break;
    }

    // Add in the phase advance. Obsolete
    // v_idx_abs_f += ((unsigned int) phase << 8);
    v_idx_abs_f += 0;

    // Reset the interpolation incrementer.
    v_idx_inc_f = 0;
  }

  old_fluxstate =  fluxstate;
}

// Hall effect sensor processing.
void do_hall_encoder()
{
  // New snapshot of the Hall effect sensor inputs.
  // (PB13 = C, PB14 = B, PB15 = A)
  hallstate = (unsigned char)((GPIOB->IDR >> 13) & 0x7);

  // If a transition has occurred.
  if(hallstate != old_hallstate)
  {
    /* Encoder Table (Gray Code)

    State #   ABC     Decimal Value
      1       001           1
      2       011           3
      3       010           2
      4       110           6
      5       100           4
      6       101           5

    */

    // Set the absolute index for sine wave commutation.
    switch(hallstate)
    {
    case 4:
      if(old_hallstate == 5)
      { v_idx_abs_h = 31292; }
      break;
    case 6:
      if(old_hallstate == 4)
      { v_idx_abs_h = 42215; }
      break;
    case 2:
      if(old_hallstate == 6)
      { v_idx_abs_h = 53137; }
      break;
    case 3:
      if(old_hallstate== 2)
      { v_idx_abs_h = 64073; }
      break;
    case 1:
      if(old_hallstate == 3)
      { v_idx_abs_h = 9447; }
      break;
    case 5:
      if(old_hallstate == 1)
      {
        v_idx_abs_h = 20369;
        if(inv_speed_timer_h >= 40)
        { inv_speed_h = inv_speed_timer_h; }
        inv_speed_timer_h = 0;
        speed_h = 0xFFFF / inv_speed_h;
      }
      break;
    default:
      break;
    }

    // Add in the phase advance.
    v_idx_abs_h += ((unsigned int) phase << 8);
    // v_idx_abs_h -= sliderp;

    // Reset the interpolation incrementer.
    v_idx_inc_h = 0;
  }

  old_hallstate = hallstate;
}

unsigned int safe_filter(unsigned int x, unsigned int y, unsigned int a)
{
  // Rollover-safe filter for unsigned 16-bit numbers x and y.
  // y = (1 - a / 10000) * y + (a / 10000) * x

  signed int x_s, y_s;
  unsigned int y_ret;

  x_s = (signed int) x;
  y_s = (signed int) y;

  if((y_s - x_s) > 32768)
  {
    x_s += 65536;
    y_s -= y_s * a / 10000;
    y_s += x_s * a / 10000;
  }
  else if((x_s - y_s) > 32768)
  {
    y_s += 65536;
    y_s -= y_s * a / 10000;
    y_s += x_s * a / 10000;
  }
  else
  {
    y_s -= y_s * a / 10000;
    y_s += x_s * a / 10000;
  }

  y_ret = (unsigned int) y_s;
  y_ret = y_ret % 65536;
  return y_ret;
}
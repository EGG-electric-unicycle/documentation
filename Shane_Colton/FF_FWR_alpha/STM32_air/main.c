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
#include "lookups.h"

extern void config();
void tx();
int rx();
unsigned int adc_read(unsigned char channel);

// Data protocol variables.
volatile unsigned char tx_buffer[TX_LEN];
volatile unsigned char rx_buffer[RX_LEN];
volatile unsigned char tx_i = 0;
volatile unsigned char rx_i = 0;

volatile unsigned int tx_timer = 0;
volatile unsigned char tx_go = 0;

volatile int32_t ib_zero, ic_zero;
volatile int32_t ia_raw, ib_raw, ic_raw, idc_raw;
volatile int32_t ia_int, ib_int, ic_int, idc_int;

volatile unsigned int vdc_raw;
volatile unsigned int vdc_int;

volatile unsigned char faultstate = 0;
volatile unsigned char drivestate = IDLE;

volatile unsigned char hallstate, old_hallstate;
volatile unsigned char fluxstate, old_fluxstate;

volatile unsigned char mag = 0;             // PWM Magnitude 0-255
volatile unsigned char phase = 0;           // PWM Phase Advance 0-255

volatile unsigned char v_idx;

volatile unsigned char loop_go;

volatile unsigned char ADC1_ch[2] = {IB_CH, VDC_CH};
volatile unsigned char ADC2_ch[2] = {IC_CH, THR_CH};
volatile unsigned char ADC_idx = 0;

volatile int32_t ib_adc, ic_adc, vdc_adc, thr_adc;

volatile unsigned int atemp, btemp, ctemp, ntemp;
volatile signed int vir_a, vir_b, vir_c, vir_temp;
volatile signed int fa_int, fb_int, fc_int;

// position and speed variables for flux observer
volatile unsigned int v_idx_abs_f, v_idx_inc_f, v_idx_int_f;
volatile unsigned int inv_speed_timer_f = 0;  // proportional to 1/speed
volatile unsigned int speed_f = 0;            // proportional to speed
volatile unsigned int inv_speed_f = 65535;    // interval/inverse speed

// position and speed variables for hall effect sensors
volatile unsigned int v_idx_abs_h, v_idx_inc_h, v_idx_int_h;
volatile unsigned int inv_speed_timer_h = 0;  // proportional to 1/speed
volatile unsigned int speed_h = 0;            // proportional to speed
volatile unsigned int inv_speed_h = 65535;    // interval/inverse speed

// position and speed variables for open-loop startup
volatile unsigned int v_idx_int_s;
volatile unsigned int speed_s = 0;            // proportional to speed
float speed_sf = 0.0;                         // floating point speed

volatile unsigned int accel = 0;            // acceleration command

volatile signed int pwm_in = 0;             // PWM input [us]
volatile unsigned int fail_timer = 0;     // radio loss timeout
volatile unsigned int sliderp = 0;          // test slider phase
volatile unsigned int sliderm = 0;          // test slider magnitude

volatile unsigned int ae = 10000;

volatile unsigned int gd_config_flag;       // gate driver needs to be
                                            // reconfigured
// startup routine variables
float park_counter = 0.0;                   // Amp second counter for parking

// Closed-Loop Speed Control Variables
volatile float rpm_command_raw = 0.0;       // rpm
volatile float rpm_command_lim = 0.0;       // rpm
volatile float rpm_measured = 0.0;          // rpm
volatile float rpm_error = 0.0;             // rpm
volatile float rpm_error_int = 0.0;         // rpm*s
volatile float rpm_p_out = 0.0;             // mA
volatile float rpm_i_out = 0.0;             // mA
volatile float rpm_ff_out = 0.0;            // mA

float Iqf = 0.0;
float Idf = 0.0;
float Iqr = 0.0;
float Idr = 0.0;

int main()
{
  unsigned char idx_snapshot = 0;
  signed int ia_int_snapshot, ib_int_snapshot, ic_int_snapshot;

  float magf = 0.0;
  float magfi = 0.0;
  float phasef = 0.0;
  float temp = 0.0;

  config();

  // send intitial setup commands to DRV8301 via SPI

  NOT_KILL;

  // kill time for SPI to start up
  for(int x = 0; x <= 1000000; x++)
  {
    DEBUG_HIGH;
    DEBUG_LOW;
  }

  GPIOA->BRR |= GPIO_BRR_BR15;
  SPI1->DR = (0x2 << 11) | (0x028);
  while((SPI1->SR & SPI_SR_TXE) == 0);
  while((SPI1->SR & SPI_SR_BSY) != 0);
  GPIOA->BSRR |= GPIO_BSRR_BS15;

  for(int x = 0; x <= 1000000; x++)
  {
    DEBUG_HIGH;
    DEBUG_LOW;
  }

  while((SPI1->SR & SPI_SR_TXE) == 0);
  GPIOA->BRR |= GPIO_BRR_BR15;
  SPI1->DR = (0x3 << 11) | (0x008);
  while((SPI1->SR & SPI_SR_BSY) != 0);
  GPIOA->BSRR |= GPIO_BSRR_BS15;

  while(1==1)
  {
    // Wait for the 1kHz flag to begin the slow loop.
    while(loop_go == 0);
    loop_go = 0;

    if(gd_config_flag == 1)
    {
      GPIOA->BRR |= GPIO_BRR_BR15;
      SPI1->DR = (0x2 << 11) | (0x02C);
      while((SPI1->SR & SPI_SR_TXE) == 0);
      while((SPI1->SR & SPI_SR_BSY) != 0);
      GPIOA->BSRR |= GPIO_BSRR_BS15;
      gd_config_flag = 2;
    }
    else
    {
      while((SPI1->SR & SPI_SR_TXE) == 0);
      GPIOA->BRR |= GPIO_BRR_BR15;
      SPI1->DR = (0x3 << 11) | (0x008);
      while((SPI1->SR & SPI_SR_BSY) != 0);
      GPIOA->BSRR |= GPIO_BSRR_BS15;
    }

    // If a complete Rx packet is waiting,
    if(rx_i == RX_LEN)
    {
      // Try to parse it. If successful...
      if(rx())
      {
        // rx_timeout = 0;
      }
    }
    // Start a new transmission at fixed intervals.
    tx_timer = (tx_timer + 1) % TX_INTERVAL;
    if(tx_timer == 0) { tx(); }

    Iqr = 0.0; // q-axis duty cycle directly controlled

    if(fail_timer < FAILTIME)
    {
      fail_timer++;
    }
    else
    {
      pwm_in = PWM_IN_MIN - 1;
    }

    // PWM input to RPM command mapping.
    if((pwm_in > PWM_IN_MIN) && (pwm_in < PWM_IN_INVALID))
    {
      // PWM input is in the proper range
      // scale to range of magnitudes
      temp = (float)(pwm_in - PWM_IN_MIN);
      temp *= (RPM_MAX - RPM_MIN);
      temp /= (float)(PWM_IN_MAX - PWM_IN_MIN);
      temp += RPM_MIN;
      // limit
      rpm_command_raw = temp;
      if(rpm_command_raw < RPM_MIN) { rpm_command_raw = 0.0; }
      if(rpm_command_raw > RPM_MAX) { rpm_command_raw = RPM_MAX; }
      // low-pass filter
      if(rpm_command_raw >= (rpm_command_lim + RPM_SLEW * DT_LOOP))
      {
        // positive-going slew rate limit
        rpm_command_lim += RPM_SLEW * DT_LOOP;
      }
      else if(rpm_command_raw < (rpm_command_lim - RPM_SLEW * DT_LOOP))
      {
        // negative-going slew ate limit
        rpm_command_lim -= RPM_SLEW * DT_LOOP;
      }
      else
      {
        rpm_command_lim = rpm_command_raw;
      }
    }
    else
    {
      rpm_command_lim -= RPM_SLEW * DT_LOOP;
    }

    if(rpm_command_lim < 0.0) { rpm_command_lim = 0.0; }
    if(rpm_command_lim > RPM_MAX) { rpm_command_lim = RPM_MAX; }

    // RPM Control Outer Loop
    rpm_measured = KS * (float) speed_f;

    rpm_error = (rpm_command_lim - rpm_measured);

    if(rpm_error > 0.0)
    {
      rpm_p_out = KP_RPM_UP * (rpm_measured / RPM_MAX) * rpm_error;
    }
    else
    {
      rpm_p_out = KP_RPM_DN * (rpm_measured / RPM_MAX) * rpm_error;
    }

    if((rpm_error > 0.0) && (rpm_i_out < I_SAT_RPM))
    {
      rpm_i_out += KI_RPM * rpm_error * DT_LOOP;
    }
    if((rpm_error < 0.0) && (rpm_i_out > -I_SAT_RPM))
    {
      rpm_i_out += KI_RPM * rpm_error * DT_LOOP;
    }
    rpm_ff_out = KFF_I * rpm_command_lim * rpm_command_lim;

    Iqr = rpm_p_out + rpm_i_out + rpm_ff_out;

    if(Iqr > AMAX) { Iqr = AMAX; }
    if(Iqr < -BMAX) { Iqr = -BMAX; }

    Idr = 0.0; // d-axis current command fixed to zero (MTPA)

    // If the ADC is sampling phase current, wait for it to finish so that
    // the phase current measurements are synchronoized!!!

    ia_int_snapshot = ia_int;
    ib_int_snapshot = ib_int;
    ic_int_snapshot = ic_int;
    idx_snapshot = (v_idx - phase);

    // ABC->dq Park transform
    // Keep types explicit!
    // ------------------------------------------------------------------------
    temp = (float)(ia_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot)] - 127);
    temp += (float)(ib_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot - CHAR_120_DEG)] - 127);
    temp += (float)(ic_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot + CHAR_120_DEG)] - 127);
    temp *= 0.00525; // = * (2/3) / 127
    Iqf = AIDQP * Iqf + AIDQN * temp; // filter

    temp = (float)(ia_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot - CHAR_90_DEG)] - 127);
    temp += (float)(ib_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot - CHAR_120_DEG - CHAR_90_DEG)] - 127);
    temp += (float)(ic_int_snapshot) * (float)((signed int)SIN8LUT[(unsigned char)(idx_snapshot + CHAR_120_DEG - CHAR_90_DEG)] - 127);
    temp *= 0.00525; // = * (2/3) / 127
    Idf = AIDQP * Idf + AIDQN * temp; // filter
    // ------------------------------------------------------------------------

    // q-axis/torque control (active in all states but IDLE)

    if(faultstate == 0)
    {
      switch(drivestate)
      {
      case IDLE:
        magf = 0;
        if(rpm_command_lim > RPM_MIN)
        {
          park_counter = 0;
          drivestate = PARK;
        }
        break;
      case PARK:
        magf = magf + KPQ * (START_CURRENT - Iqf);
        v_idx_int_s = 0;
        speed_sf = 0;
        speed_s = 0;
        if((park_counter < 0) || (rpm_command_lim < RPM_MIN))
        {
          drivestate = IDLE;
        }
        else if(park_counter < PARK_THRESHOLD)
        {
          park_counter += START_CURRENT * DT_LOOP;
        }
        else
        {
          drivestate = RAMP;
        }
        break;
      case RAMP:
        magf = magf + KPQ * (START_CURRENT - Iqf);
        if((speed_sf < 0) || (rpm_command_lim < RPM_MIN))
        {
          drivestate = IDLE;
        }
        else if((speed_sf > SPEED_CL) && (speed_f > SPEED_CL))
        {
          drivestate = RUN;
        }
        else
        {
          speed_sf += START_CURRENT * RAMP_RATE * DT_LOOP;
          speed_s = (unsigned int) speed_sf;
        }
        break;
      case RUN:
        magfi = magfi + KPQ * (Iqr - Iqf);
        magf = magfi + KFF_V * rpm_command_lim;
        if(speed_f < SPEED_OL)
        {
          speed_sf = (float) speed_f;
          speed_s = speed_f;
          drivestate = RAMP;
        }
        break;
      }

      // Restrict to 0-245 to maintain 4% minimum off-time.
      if(magf > 245.0) { magf = 245.0; }
      if(magf < 0.0) { magf = 0.0; }
      mag = (unsigned char) magf;

    }
    else
    {
      mag = 0;
    }

    // Open-loop speed control:
    // if((Iqr + 10000.0) * 255.0 / 50000.0 < 0.0) { mag = 0; }
    // else if((Iqr + 10000.0) * 255.0 / 50000.0 > 255.0) { mag = 255; }
    // else {mag = (unsigned char) ((Iqr + 10000.0) * 255.0 / 50000.0); }

    // Slider control:
    // mag = sliderm;

    // d-axis/phase control
    if((mag > 0) && (drivestate == RUN))
    {
      phasef = phasef + KPD * (Idf + Idr);
      if(phasef > (float)(CHAR_90_DEG)) { phasef = (float)(CHAR_90_DEG); }
      if(phasef < 0.0) { phasef = 0.0; }
      phase = (unsigned char) phasef;
    }
    else
    {
      phasef = 0.0;
      phase = 0;
    }
  }
}

// Read a single ADC channel.
unsigned int adc_read(unsigned char channel)
{
  unsigned int result = 0;

  ADC1->SQR3 = channel;                     // set channel
  ADC1->CR2 |= ADC_CR2_ADON;                // start conversion
  while((ADC1->SR & ADC_SR_EOC) == 0);      // wait for EOC
  result = ADC1->DR;

  return result;
}

// Try to parse a received data packet.
int rx()
{
  unsigned char i, j;
  unsigned char rflags;
  unsigned char rx_crc = CRC_SEED;

  // First, un-escape any byte that was flagged as 0xFF.
  // There is 1 sets of 7 escape flag bits.
  for(j = 0; j <= 0; j++)
  {
    // They are stored in rx_buffer[8:8].
    rflags = rx_buffer[8+j];
    // Each bit encodes whether tx_buffer[7*j+i+1] was escaped.
    for(i = 0; i <= 6; i++)
    {
      // If the escape flag bit is set...
      if((rflags & (1 << i)) != 0)
      {
        // ...change its byte in rx_buffer[] back to 0xFF.
        rx_buffer[7*j+i+1] = START;
      }
    }
  }

  // Next, calculate and verify the CRC on bytes 1-6.
  for(i = 1; i <= 6; i++)
  {
    rx_crc = CRC8LUT[rx_buffer[i] ^ rx_crc];
  }
  // The expected CRC result is stored in byte 7.
  // If the CRC does not match...
  if(rx_buffer[7] != rx_crc)
  {
    // ...return 0 to indicated failure.
    return 0;
  }

  // Otherwise, process the entire packet:

  // Grab the RPM command from the UART buffer.
  sliderm = ((unsigned int)(rx_buffer[1])) << 8;
  sliderm += (unsigned int)(rx_buffer[2]);

  // Grab the auxiliary command from the UART buffer.
  sliderp = ((unsigned int)(rx_buffer[3])) << 8;
  sliderp += (unsigned int)(rx_buffer[4]);

  // Return 1 to indicate success.
  return 1;
}

// Transmit a new data packet.
void tx()
{
  unsigned char i, j = 0;
  unsigned char rflags = 0x00;
  unsigned char tx_crc = CRC_SEED;
  unsigned int temp_int;

  // Pack up the Tx buffer.
  tx_buffer[0] = START;

  temp_int = (unsigned int) (ia_int + 100000);
  tx_buffer[1] = (temp_int >> 24) & 0xFF;
  tx_buffer[2] = (temp_int >> 16) & 0xFF;
  tx_buffer[3] = (temp_int >> 8) & 0xFF;
  tx_buffer[4] = (temp_int) & 0xFF;
  temp_int = (unsigned int) (ib_int + 100000);
  tx_buffer[5] = (temp_int >> 24) & 0xFF;
  tx_buffer[6] = (temp_int >> 16) & 0xFF;
  tx_buffer[7] = (temp_int >> 8) & 0xFF;

  tx_buffer[8] = (temp_int) & 0xFF;
  temp_int = (unsigned int) (ic_int + 100000);
  tx_buffer[9] = (temp_int >> 24) & 0xFF;
  tx_buffer[10] = (temp_int >> 16) & 0xFF;
  tx_buffer[11] = (temp_int >> 8) & 0xFF;
  tx_buffer[12] = (temp_int) & 0xFF;
  temp_int = (unsigned int) (idc_int + 100000);
  tx_buffer[13] = (temp_int >> 24) & 0xFF;
  tx_buffer[14] = (temp_int >> 16) & 0xFF;

  tx_buffer[15] = (temp_int >> 8) & 0xFF;
  tx_buffer[16] = (temp_int) & 0xFF;
  temp_int = (unsigned int) (Iqf + 100000.0);
  tx_buffer[17] = (temp_int >> 24) & 0xFF;
  tx_buffer[18] = (temp_int >> 16) & 0xFF;
  tx_buffer[19] = (temp_int >> 8) & 0xFF;
  tx_buffer[20] = (temp_int) & 0xFF;
  temp_int = (unsigned int) (Idf + 100000.0);
  tx_buffer[21] = (temp_int >> 24) & 0xFF;

  tx_buffer[22] = (temp_int >> 16) & 0xFF;
  tx_buffer[23] = (temp_int >> 8) & 0xFF;
  tx_buffer[24] = (temp_int) & 0xFF;
  tx_buffer[25] = (vdc_int >> 24) & 0xFF;
  tx_buffer[26] = (vdc_int >> 16) & 0xFF;
  tx_buffer[27] = (vdc_int >> 8) & 0xFF;
  tx_buffer[28] = (vdc_int) & 0xFF;

  tx_buffer[29] = ( (int)rpm_command_lim >> 8) & 0xFF;
  tx_buffer[30] = (int)rpm_command_lim & 0xFF;
  tx_buffer[31] = mag;
  tx_buffer[32] = phase;
  tx_buffer[33] = ((drivestate & 0x07) << 3) + (fluxstate & 0x07);
  tx_buffer[34] = (v_idx_int_h >> 8) & 0xFF;
  tx_buffer[35] = (v_idx_int_h) & 0xFF;

  tx_buffer[36] = (v_idx_int_f >> 8) & 0xFF;
  tx_buffer[37] = (v_idx_int_f) & 0xFF;
  tx_buffer[38] = (speed_h >> 8) & 0xFF;
  tx_buffer[39] = (speed_h) & 0xFF;
  tx_buffer[40] = (speed_f >> 8) & 0xFF;
  tx_buffer[41] = (speed_f) & 0xFF;
  tx_buffer[42] = (faultstate) & 0xFF;

  temp_int = (unsigned int) (fa_int + 100000);
  tx_buffer[43] = (temp_int >> 24) & 0xFF;
  tx_buffer[44] = (temp_int >> 16) & 0xFF;
  tx_buffer[45] = (temp_int >> 8) & 0xFF;
  tx_buffer[46] = (temp_int) & 0xFF;
  tx_buffer[47] = 0;
  tx_buffer[48] = 0;

  // Calculate a CRC value on bytes 1-48.
  for(i = 1; i <= 48; i++)
  {
    tx_crc = CRC8LUT[tx_buffer[i] ^ tx_crc];
  }
  // Put the CRC value in byte 49.
  tx_buffer[49] = tx_crc;

  // Check bytes 1-49 in 7 groups of 7.
  // If the byte is START, change to ESC and set a flag in bytes 50-56.
  // They will be changed back by the receiving program.
  for(j = 0; j <= 6; j++)
  {
    rflags = 0x00;
    for(i = 0; i <= 6; i++)
    {
      if(tx_buffer[7*j+i+1] == START)
      {
        tx_buffer[7*j+i+1] = ESC;
        rflags |= 0x01 << i;
      }
    }
    tx_buffer[50+j] = rflags;
  }

  // Start transmitting, interrupts will take over after the first byte.
  tx_i = 0;
  // Enable Tx interrupt.
  USART1->CR1 |= USART_CR1_TXEIE;
  // Start transmission.
  USART1->DR = tx_buffer[0];
}
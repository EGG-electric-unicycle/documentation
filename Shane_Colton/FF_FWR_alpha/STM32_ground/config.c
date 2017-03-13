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
#include "config.h"

extern volatile unsigned char ADC1_ch[2];
extern volatile unsigned char ADC2_ch[2];
extern volatile unsigned char ADC_idx;


// General purpose config routine, runs all others
void config()
{
  configClock();
  configRCC();
  configIO();
  configTIM1();
  configTIM2();
  configTIM3();
  configUSART();
  configSPI();
  configADC();
  configNVIC();
  configIWDG();
}

// Configure the various clocks
void configClock()
{
  RCC->CR &= ~RCC_CR_CSSON;              // Clock security system disabled
  RCC->CR &= ~RCC_CR_HSEBYP;             // High speed clock bypassing disabled
  RCC->CR |= RCC_CR_HSEON;               // External crystal enabled

  while( !(RCC->CR & RCC_CR_HSERDY) );   // Wait until the clock is stable

  // PLL clock multipler:
  // RCC->CFGR |= RCC_CFGR_PLLMULL_2;    // PLLMULL = [000]
  // RCC->CFGR |= RCC_CFGR_PLLMULL_1;    // --> x2 PLL
  // RCC->CFGR |= RCC_CFGR_PLLMULL_0;    //
  RCC->CFGR |= RCC_CFGR_PLLSRC;          // PLL fed by HSE (16MHz)
  // RCC->CFGR |= RCC_CFGR_PLLXTPRE;     // PLL fed by HSE/2 (8MHz)
  RCC->CR |= RCC_CR_PLLON;               // PLL on
  // RCC->CR &= ~RCC_CR_PLLON;           // PLL disabled
  while( !(RCC->CR & RCC_CR_PLLRDY) );   // Wait until PLL is locked

  // RCC->CFGR &= ~RCC_CFGR_SW_1;        // HSE used as SYSCLOCK
  // RCC->CFGR |= RCC_CFGR_SW_0;         // SW[1:0] = [01]
  RCC->CFGR |= RCC_CFGR_SW_1;            // PLL used as SYSCLOCK
  RCC->CFGR &= ~RCC_CFGR_SW_0;           // SW[1:0] = [10]

  RCC->CFGR |= RCC_CFGR_MCO_NOCLOCK;     // Disable clock output on IO pin
  RCC->CFGR |= RCC_CFGR_ADCPRE_DIV4;     // APB2/4 for ADC (8MHz)
  RCC->CFGR |= RCC_CFGR_PPRE1_DIV1;      // HCLK/1 for APB1 (32MHz)
  RCC->CFGR |= RCC_CFGR_PPRE2_DIV1;      // HCLK/1 for APB2 (32MHz)
  RCC->CFGR |= RCC_CFGR_HPRE_DIV1;       // SYSCLOCK/1 for AHB (32MHZ)
  RCC->CR &= ~RCC_CR_HSION;              // Disable internal RC oscillator
}

// Set up internal clock distribution
void configRCC()
{
  RCC->APB2ENR |= RCC_APB2ENR_AFIOEN;     // enable AFIO clock

  RCC->APB2ENR |= RCC_APB2ENR_IOPAEN;     // enable GPIOA clock
  RCC->APB2ENR |= RCC_APB2ENR_IOPBEN;     // enable GPIOB clock
  RCC->APB2ENR |= RCC_APB2ENR_IOPCEN;     // enable GPIOC clock

  RCC->APB2ENR |= RCC_APB2ENR_TIM1EN;     // enable TIM1 clock
  RCC->APB1ENR |= RCC_APB1ENR_TIM2EN;     // enable TIM2 clock
  RCC->APB1ENR |= RCC_APB1ENR_TIM3EN;     // enable TIM3 clock

  RCC->APB2ENR |= RCC_APB2ENR_ADC1EN;     // enable ADC1 clock
  RCC->APB2ENR |= RCC_APB2ENR_ADC2EN;     // enable ADC2 clock

  RCC->APB2ENR |= RCC_APB2ENR_USART1EN;   // enable USART1 clock
  RCC->APB2ENR |= RCC_APB2ENR_SPI1EN;     // enable SPI1 clock

  RCC->AHBENR |= RCC_AHBENR_DMA1EN;       // enable DMA1 clock
  RCC->AHBENR |= RCC_AHBENR_DMA2EN;       // enable DMA2 clock
}

// Configure the IO map
void configIO()
{
  // PA3 - Analog input for DC voltage (0)
  // PA4 - Analog input for Phase C current (0)
  // PA5 - Analog input for Phase B current (0)
  // PA6 - TIM3.1 PWM output for Phase C (A)
  // PA7 - TIM3.2 PWM output for Phase B (A)

  // PA9 - USART1 TX AF output (A)
  // PA10 - USART1 RX floating input (4)

  // PA11 - TIM1.4 PWM floating input (4)

  // PA15 - SPI /SCS AF output (A)

  // PB0 - TIM3.3 PWM output for phase A (A)

  // PB1 - Analog input for Throttle (0)

  // PB3 - SPI SCLK output (A)
  // PB4 - SPI MISO floating input (4)
  // PB5 - SPI MOSI output (A)

  // PB8 - digital output for status LED (2)

  // PB13 - digital output to control EN (2)
  // PB14 - digital input w/ pull-up for /FAULT (8)
  // PB15 - digital input w/ pull-up for /OCTW (8)

  // All unused pins to GND.

  // set I/O map
  GPIOA->CRL = 0xAA000222;
  GPIOA->CRH = 0x222244A2;
  GPIOB->CRL = 0x22A4A20A;
  GPIOB->CRH = 0x88222222;
  GPIOC->CRL = 0x22222222;
  GPIOC->CRH = 0x22222222;

  // turn on pull-up resistors
  GPIOB->BSRR |= GPIO_BSRR_BS14;
  GPIOB->BSRR |= GPIO_BSRR_BS15;

  GPIOA->BSRR |= GPIO_BSRR_BS15;

  // re-map SPI
  AFIO->MAPR |= AFIO_MAPR_SWJ_CFG_2; // disable JTAG
  AFIO->MAPR |= AFIO_MAPR_SPI1_REMAP;
}

// Configure Phase A PWM on Timer 1
void configTIM1()
{
  TIM1->CCMR2 |= TIM_CCMR2_CC3S_1;    // map TIM1.4 to IC3
  TIM1->CCMR2 |= TIM_CCMR2_CC4S_0;    // map TIM1.4 to IC4

  TIM1->CCER &= ~(TIM_CCER_CC3P);     // IC3 on rising edge
  TIM1->CCER |= TIM_CCER_CC4P;        // IC4 on falling edge
  TIM1->CCER |= TIM_CCER_CC3E;        // enable IC3
  TIM1->CCER |= TIM_CCER_CC4E;        // enable IC4

  TIM1->PSC = 31;                     // 1us tick with 32MHz clock
  TIM1->ARR = 30000;                  // 30ms period

  TIM1->DIER |= TIM_DIER_CC4IE;       // enable IC4 interrupt
  TIM1->CR1 |= TIM_CR1_CEN;           // start timer
}

// Congigure Phase B PWM on Timer 2
void configTIM2()
{
  TIM2->CCMR1 |= TIM_CCMR1_OC2M_2;    // OC2M[2:0] = 110
  TIM2->CCMR1 |= TIM_CCMR1_OC2M_1;    // PWM Mode 1
  TIM2->CCMR1 &= ~TIM_CCMR1_OC2M_0;   // duty cycle reflects high time
  TIM2->CCMR1 |= TIM_CCMR1_OC2PE;     // new duty cycle latched at update

  TIM2->CR1 |= TIM_CR1_ARPE;          // new max (freq) latched at update
  TIM2->CR1 |= TIM_CR1_CMS_0;         // center-aligned mode

  TIM2->ARR = 1023;                   // initial max
  TIM2->CCR2 = 996;                   // trigger ADC at -859ns from center

  TIM2->EGR |= TIM_EGR_UG;            // latch initial values

  TIM2->BDTR |= TIM_BDTR_MOE;         // main output enable
  TIM2->BDTR |= TIM_BDTR_AOE;         // something about enabling
  TIM2->BDTR |= TIM_BDTR_OSSR;        // something about enabling
  TIM2->CCER |= TIM_CCER_CC2E;        // channel 2 output enable

  TIM2->CR2 |= TIM_CR2_MMS_1;         // master mode: UPDATE
  TIM2->RCR |= 1;                     // wtf
  TIM2->EGR |= TIM_EGR_UG;            // why do i have to do this?

  TIM2->CR1 |= TIM_CR1_CEN;           // start timer
}

// Configure PWMs on Timer 3
void configTIM3()
{
  TIM3->CCMR1 |= TIM_CCMR1_OC1M_2;    // OC1M[2:0] = 110
  TIM3->CCMR1 |= TIM_CCMR1_OC1M_1;    // PWM Mode 1
  TIM3->CCMR1 &= ~TIM_CCMR1_OC1M_0;   // duty cycle reflects high time
  TIM3->CCMR1 |= TIM_CCMR1_OC1PE;     // new duty cycle latched at update

  TIM3->CCMR1 |= TIM_CCMR1_OC2M_2;    // OC1M[2:0] = 110
  TIM3->CCMR1 |= TIM_CCMR1_OC2M_1;    // PWM Mode 1
  TIM3->CCMR1 &= ~TIM_CCMR1_OC2M_0;   // duty cycle reflects high time
  TIM3->CCMR1 |= TIM_CCMR1_OC2PE;     // new duty cycle latched at update

  TIM3->CCMR2 |= TIM_CCMR2_OC3M_2;    // OC1M[2:0] = 110
  TIM3->CCMR2 |= TIM_CCMR2_OC3M_1;    // PWM Mode 1
  TIM3->CCMR2 &= ~TIM_CCMR2_OC3M_0;   // duty cycle reflects high time
  TIM3->CCMR2 |= TIM_CCMR2_OC3PE;     // new duty cycle latched at update

  TIM3->CR1 |= TIM_CR1_ARPE;          // new max (freq) latched at update
  TIM3->CR1 |= TIM_CR1_CMS_0;         // center-aligned mode

  TIM3->ARR = 1023;                   // initial max
  TIM3->CCR1 = 0;                     // 0% duty cycle for all 3 PWMs
  TIM3->CCR2 = 0;
  TIM3->CCR3 = 0;

  TIM3->EGR |= TIM_EGR_UG;            // latch initial values

  TIM3->BDTR |= TIM_BDTR_MOE;         // main output enable
  TIM3->BDTR |= TIM_BDTR_AOE;         // something about enabling
  TIM3->BDTR |= TIM_BDTR_OSSR;        // something about enabling
  TIM3->CCER |= TIM_CCER_CC1E;        // channel 1 output enable
  TIM3->CCER |= TIM_CCER_CC2E;        // channel 2 output enable
  TIM3->CCER |= TIM_CCER_CC3E;        // channel 3 output enable

  TIM3->SMCR |= TIM_SMCR_SMS_2;       // slave mode: RESET with Timer 1

  TIM3->CR1 |= TIM_CR1_CEN;           // start timer
}

// Configure USART1
void configUSART()
{
  USART1->CR1 |= USART_CR1_UE;        // enable USART1
  USART1->BRR |= (34 << 4) + 12;      // 34+12/16 for 57.6k @ 32MHz RTFM
  USART1->CR1 |= USART_CR1_M;         // 9-bit word length (8-E-1)
  USART1->CR1 |= USART_CR1_PCE;       // parity enabled (default EVEN)
  USART1->CR1 |= USART_CR1_TE;        // enable TX
  USART1->CR1 |= USART_CR1_RE;        // enable RX
  USART1->CR1 |= USART_CR1_RXNEIE;    // enable RX interrupts
  USART1->CR1 |= USART_CR1_TXEIE;     // enable TX interrupts
}

// Configure SPI1
void configSPI()
{
  SPI1->CR1 |= SPI_CR1_DFF;            // 16-bit data frame
  SPI1->CR1 |= SPI_CR1_BR_2;           // BR: [100] f_PCLK/32
  SPI1->CR1 &= ~SPI_CR1_BR_1;          // --> 1MHz bit rate
  SPI1->CR1 &= ~SPI_CR1_BR_0;          // with 32MHz clock
  SPI1->CR1 |= SPI_CR1_MSTR;           // master mode
  SPI1->CR1 |= SPI_CR1_CPHA;           // data on clock falling edge

  SPI1->CR2 |= SPI_CR2_SSOE;           // slave select output enable
  SPI1->CR1 |= SPI_CR1_SPE;            // enable SPI
}

// Configure ADC
void configADC()
{
  unsigned char i = 0;

  // Start the hardware calibration
  ADC1->CR2 |= ADC_CR2_CAL;
  ADC2->CR2 |= ADC_CR2_CAL;
  // Wait for the calibration to end.
  // while((ADC1->CR2 & ADC_CR2_CAL) != 0);

  // Set all sample times to 13.5 cycles, ~2us.
  for(i = 0; i <= 9; i++)
  {
    ADC1->SMPR2 |= (0x2 << (3 * i));
    ADC2->SMPR2 |= (0x2 << (3 * i));
  }
  for(i = 0; i <= 7; i++)
  {
    ADC1->SMPR1 |= (0x2 << (3 * i));
    ADC2->SMPR1 |= (0x2 << (3 * i));
  }

  // ADC1->CR1 |= ADC_CR1_DUALMOD_2;     // Dual ADC Mode:
  // ADC1->CR1 |= ADC_CR1_DUALMOD_1;     // Regular Sequence Simultaneous
  ADC1->CR2 |= ADC_CR2_EXTTRIG;       // ADC1 External Trigger:
  ADC1->CR2 |= ADC_CR2_EXTSEL_1;      // TIM2 CC2
  ADC1->CR2 |= ADC_CR2_EXTSEL_0;
  ADC2->CR2 |= ADC_CR2_EXTTRIG;       // ADC2 External Trigger:
  ADC2->CR2 |= ADC_CR2_EXTSEL_1;      // TIM2 CC2
  ADC2->CR2 |= ADC_CR2_EXTSEL_0;
  ADC1->SQR3 = ADC1_ch[ADC_idx];      // initialize ADC1 first channel
  ADC2->SQR3 = ADC2_ch[ADC_idx];      // initialize ADC2 first channel
  ADC1->CR1 |= ADC_CR1_EOCIE;         // enable end-of-conversion interrupt
  // Power-up the ADC converter.
  ADC1->CR2 |= ADC_CR2_ADON;          // turn on ADC1
  ADC2->CR2 |= ADC_CR2_ADON;          // turn on ADC2
}

// Configure the Interrupt Controller
void configNVIC()
{
  // Don't use sub-priorities.
  NVIC_SetPriorityGrouping(0);

  // Set up SysTick for 10kHz interrupts @ 16MHz.
  // SysTick_Config(1600);
  // NVIC_SetPriority(SysTick_IRQn, 1);

  // Enable ADC interrupt.
  NVIC_EnableIRQ(ADC1_2_IRQn);
  NVIC_SetPriority(ADC1_2_IRQn, 2);

  // Enable Timer1 Capture/Compare interrupt for PWM input.
  NVIC_EnableIRQ(TIM1_CC_IRQn);
  NVIC_SetPriority(TIM1_CC_IRQn, 3);

  // Turn on USART1 interrupt and set its priority.
  NVIC_EnableIRQ(USART1_IRQn);
  NVIC_SetPriority(USART1_IRQn, 4);
}

// Configure the watchdog timer.
void configIWDG()
{
  IWDG->KR = 0x5555;  // Key to unlock watchdog registers.
  IWDG->RLR = 20;      // Set 0.2ms watchdog interval.
  IWDG->KR = 0xCCCC;  // Start the watchdog timer.
}
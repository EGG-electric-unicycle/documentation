// Interrupt service routines.
// void SysTick_Handler();
void USART1_IRQHandler();
void ADC1_2_IRQHandler();
void TIM1_CC_IRQHandler();

// User routines.
void fastloop();
void do_flux_encoder();
void do_hall_encoder();
unsigned int safe_filter(unsigned int x, unsigned int y, unsigned int a);
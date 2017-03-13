// Timing constants.
// FAST LOOP:  15625     Hz, triggered by PWM
// SLOW LOOP:    976.56  Hz, in main()
// TX LOOP:       19.531 Hz
#define LOOP_INTERVAL 16           // number of fast loops per slow loop
#define TX_INTERVAL 20             // number of slow loops per Tx

#define DTLOOP 0.001024            // seconds per slow loop

// Data protocol constants.
#define START 0xFF                 // start character
#define ESC 0xFE                   // escape character
#define TX_LEN 56                  // length of the Tx packet
#define RX_LEN 9                   // length of the Rx packet

// Motor properties.
#define R 17 / 1000                // phase resistance in Ohm
#define L 15 / 1000                // synchronous inductance in mH

// Fast loop filter time constants:
// T = A/(1-A)*64.00us
#define TVIR 16                     // T in ms
#define AVIR 40 / 10000             // A
#define VIR_SAT 30000              // [uWb]
#define F_SAT 20000                  // [uWb]
#define FLUX_THRESHOLD 100          // [uWb]

// Slow loop filter time constants:
// T = A/(1-A)*1.024ms = 19.46ms
#define AIDQP 0.95                  // A
#define AIDQN 0.05                  // (1-A)

// Overcurrent and Undervoltage Limits.
#define OC_ABC 150000                // [mA]
// #define OC_DCP 20000                // [mA], unused
// #define OC_DCN 10000                // [mA], unused
#define OV_DC 50000                 // [mV]
#define UV_DC 25000                 // [mV]

// Fault Flags
#define OC_ABC_FLAG 0x01            // Phase overcurrent
#define OC_DCP_FLAG 0x02            // DC overcurrent positive
#define OC_DCN_FLAG 0x04            // DC overcurrent negative
#define OV_DC_FLAG 0x08             // DC overvoltage
#define UV_DC_FLAG 0x10             // DC undervoltage
#define TEST_FLAG 0x80              // Fault recovery test.

#define FAULT_TIMEOUT 78125       // [fast loops]
#define TEST_PERIOD 7813

// State variable scaling constants.
// These should also be consistent with the VB program.
#define KV 123 / 10                 // [mV/lsb] (3/17/12)
#define KI 1620 / 10                 // [mA/lsb] (3/17/12)
#define KS 2.044                    // [rpm/lsb] (4/14/12)

#define PHASE_A 0x1
#define PHASE_B 0x2
#define PHASE_C 0x4

// Throttle range, needs to be calibrated for a new throttle.
#define ACCEL_LO 640               // ~1V, max braking
#define ACCEL_MID 1500             // ~2V, neutral
#define ACCEL_HI 3920              // ~4V, max accel

// Peak q-axis current values.
// Note: maximum current may be higher if phase advance is used.
#define AMAX 50000.0                  // max accel current [mA]
#define BMAX 20000.0                  // max braking current [mA]

// Proportional control gains for q-axis (mag) and d-axis (phase).
#define KPQ 0.008 / 1000.0            // [LSB/A/loop] = [100%/20A/0.5s]
#define KPD 0.0064 / 1000.0           // [LSB/A/loop] = [90deg/20A/0.5s]

// Drive States:
#define IDLE 0
#define PARK 1
#define RAMP 2
#define RUN 3

#define SPEED_CL 84                 // 500rpm
#define SPEED_OL 56                 // 250rpm
#define PARK_THRESHOLD 20000.0      // 10.0As = 1.0emu
#define RAMP_RATE 0.0010            // speed units per mAs

// Maths.
#define INT_60_DEG 10923
#define INT_90_DEG 16384
#define CHAR_120_DEG 85
#define CHAR_90_DEG 64

// PWM channel definitions.
#define APWM TIM3->CCR3
#define BPWM TIM3->CCR2
#define CPWM TIM3->CCR1

// ADC channel definitions.
#define IB_CH 5
#define IC_CH 4
#define VDC_CH 3
#define THR_CH 9

// Kill (disable) commands.
#define NOT_KILL GPIOB->BSRR |= GPIO_BSRR_BS13
#define KILL GPIOB->BRR |= GPIO_BRR_BR13

// Status pin commands.
#define DEBUG_HIGH GPIOB->BSRR |= GPIO_BSRR_BS8
#define DEBUG_LOW GPIOB->BRR |= GPIO_BRR_BR8
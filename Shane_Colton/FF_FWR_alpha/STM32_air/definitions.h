// Timing constants.
// FAST LOOP:  15625     Hz, triggered by PWM
// SLOW LOOP:    976.56  Hz, in main()
// TX LOOP;       19.531 Hz
#define LOOP_INTERVAL 16           // number of fast loops per slow loop
#define TX_INTERVAL 20             // number of slow loops per Tx

#define DT_LOOP 0.001024            // seconds per slow loop

// Data protocol constants.
#define START 0xFF                 // start character
#define ESC 0xFE                   // escape character
#define TX_LEN 56                  // length of the Tx packet
#define RX_LEN 9                   // length of the Rx packet

// Motor properties.
#define R 93 / 1000                // phase resistance in Ohm
#define L 10 / 1000                // synchronous inductance in mH

// Fast loop filter time constants:
// T = A/(1-A)*64.00us
#define TVIR 6                      // T in ms
#define AVIR 105 / 10000            // A

#define VIR_SAT 2000                // [uWb]
#define F_SAT 1500                  // [uWb]
#define FLUX_THRESHOLD 50          // [uWb]

// Slow loop filter time constants:
// Synchronous current tracking time constant:
// T = A/(1-A)*1.024ms = 9.22ms
#define AIDQP 0.90                  // A
#define AIDQN 0.10                  // (1-A)

// Command input tracking slew rate limit:
// #define RATEUP 0.086                // = 25%/s
// #define RATEDN 0.086                // = 25%/s
#define FAILTIME 100                // slow loop ticks

// High speed filter
#define SPEED_HS 1000               // 2044rpm
#define AEN 10000                    // 10000*A

// Overcurrent and Undervoltage Limits.
#define OC_ABC 50000                // [mA]
// #define OC_DCP 20000                // [mA], unused
// #define OC_DCN 10000                // [mA], unused
#define OV_DC 18000                 // [mV]
#define UV_DC 12000                 // [mV]

// Fault Flags
#define OC_ABC_FLAG 0x01            // Phase overcurrent
#define OC_DCP_FLAG 0x02            // DC overcurrent positive
#define OC_DCN_FLAG 0x04            // DC overcurrent negative
#define OV_DC_FLAG 0x08             // DC overvoltage
#define UV_DC_FLAG 0x10             // DC undervoltage
#define TEST_FLAG 0x80              // Fault recovery test.

#define FAULT_TIMEOUT 78125         // [fast loops] ~5s
#define TEST_PERIOD 15625           // [fast loops] ~1s

// State variable scaling constants.
// These should also be consistent with the VB program.
#define KV 123 / 10                  // [mV/lsb] (3/17/12)
#define KI 201 / 10                 // [mA/lsb] (8/15/12 - FFv1.2s)
#define KS 2.044                     // [rpm/lsb] (4/14/12)

#define PHASE_A 0x1
#define PHASE_B 0x2
#define PHASE_C 0x4

// Peak q-axis current values.
// Note: maximum current may be higher if phase advance is used.
#define AMAX 15000.0                  // max accelerating current [mA]
#define BMAX 5000.0                  // max braking current [mA]

// Proportional control gains for q-axis (mag) and d-axis (phase).
#define KPQ 0.1000 / 1000.0           // [LSB/mA/loop] ~= [1V/A/s at 24VDC]
#define KPD 0.0128 / 1000.0           // [LSB/mA/loop] ~= ??? calculate

// PWM Input Settings
#define PWM_IN_MIN 1200
#define PWM_IN_MAX 1800
#define PWM_IN_INVALID 2200
#define MAG_OUT_MIN 20
#define MAG_OUT_MAX 245

// Drive States:
#define IDLE 0
#define PARK 1
#define RAMP 2
#define RUN 3

// Start-Up Parameters
#define SPEED_CL 400                 // ~800rpm
#define SPEED_OL 200                 // ~400rpm
#define PARK_THRESHOLD 1000.0        // 10.0mAs = 1.0emu
#define RAMP_RATE 0.030              // speed units per mAs
#define START_CURRENT 5000.0         // mA

// Closed-Loop Speed Control Parameters:
#define RPM_MAX 5000.0               // rpm
#define RPM_MIN 800.0                // rpm
#define RPM_SLEW 20000.0             // rpm/s
#define KP_RPM_UP 30.0               // mA/rpm
#define KP_RPM_DN 10.0               // mA/rpm
#define KI_RPM 0.0                   // mA/rpm/s
#define I_SAT_RPM 20000.0            // mA
#define KFF_I 4.000e-4               // mA/rpm^2
#define KFF_V 0.038                  // (0-255)/rpm

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
Analysis:

* The controller is very simple and cheap, seems to be near the cheap electric bicycles controllers with the exception that have the IMU ic
* The LEDs for charging indication are controlled by the main board and not by the battery BMS
The charging cable go directly to battery pack and not to the controller board. I believe there is a BMS on the battery pack (just like on electric bicycles)
* Seems there is a big inductor that can be for a step down voltage converter from the 60V from to something like 12V. After there is a 7805 to provide 5V and a LM1117 provides the 3.3V.
* The 2 equal ICs with 8 pins each, are dual opamps (SGM8632XS) each one. 3 opamps for the circuit of reading BEMF and 1 opamp for the circuit of reading total current (there is 2 big resistors in parallel that seems to be for the current measurement)
* Motors drivers are done using transistors
* The commuting mosfets are P75NF758 (STP75NF75)


STM32 PIN out (see also [original document](https://github.com/qjayjayp/myewheel.org/blob/master/docu/STM32_Generic_Controller_PINout.odg)):

![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/STM32_Generic_Controller_PINout.png)


Images of the controller from a generic wheel:

![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_1009861.jpg)
![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_1009872.jpg)
![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_1009862.jpg)
![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_1009863.jpg)
![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_1009868.jpg)
![](https://github.com/qjayjayp/myewheel.org/blob/master/docu/images/controller/generic_a/img_2129808_e_markup.jpg)
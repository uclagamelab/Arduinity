Arduinity
=========

WARNING: This code appears to be working with Leonardo Arduinos only. Work is being done on compatibility with previous generations. My apologies if this has caused you any pain, it sure has for me. - JC

Need an Arudino to talk to Unity game engine? or Unity to speak with an Arduino? Arduinity is a flexible, lightweight set of Unity scripts and Arduino code.

## Preamble
Arduinity is a UCLA Game Lab project that provides simple and easy to use communication between applications made with Unity 3d and Arduino hardware. Arduinity currently supports any USB equipped Arduino and version 3.5 of Unity 3d on the windows platform.

This document will walk you through the process of running the Arduinity examples to communicate to your Arduino from a Unity application.


## Features
Works with standard Arduino serial communication
Unity reads and writes pin values to and from Arduino
JS or C# examples included.
Multiple Arduino support
Dynamically change pin modes on Arduino


## Caveats
Windows only(for now).
Windows COM port error when numbered greater than COM10, occasionally. See Troubleshooting section.
It isn't our intention to break your hardware. This works for us, so we put it out for others to use. We don't take responsibility for what goes wrong with your hardware.
Digital write from analog pins not yet supported.


## Getting the files
Arduinity is a group of Unity scripts and Arduino programs that are available for download from Github at https://github.com/uclagamelab/Arduinity. The download link is https://github.com/uclagamelab/Arduinity/archive/master.zip

You need both the Arduino project files and the Unity project files. Unpack to a local directory.


## Setup Arduino
A simple Arduino program handles communication over serial from Unity. A USB cable must be attached to the computer running your Unity application for this to work. Open the ArudinitySerial.ino file in the Arduino software(available from http://www.arduino.cc/) and upload the program to your Arduino. This will overwrite any program currently residing on the Arduino.


## Example Files
The example files are intended to show the various features supported by Arduinity, as well as demonstrate how the interface from Unity to Arduino functions. Running and tinkering with the examples is highly recommended.


## First Example - Blink LED
The first example, in a scene called BlinkLED, demonstrates the structure of the Arduinity interface and how to send a digital high signal to an Arduino pin from Unity. Open the scene in by double clicking the BlinkLED.unity file.

To run this example, you need to make a simple circuit for your Arduino. This example is modeled on the Blink example which comes with the Arduino IDE, so it uses the same circuit. Attach an LED with the anode in pin 13 and the cathode in GND.

With the BlinkLED scene open in Unity, press the triangle play button at the top of the window. You should see a red capsule in the game window and the LED on the Arduino blinking in unison. If that's not the case, look for a error in the console. Press the play button again when you're done looking at the pretty lights.

In this example, all the logic for the behaviour on the screen and on the Arduino happens in Unity. Communication with the Arduino is separated from the logic of the blinking. There are three scripts in the scene controlling the action. 

The first is the Arduinity.cs script which is found in the Project tab in StandardAssets->ArduinityScripts->Arduinity, and in the scene Hierarchy as a component on CubeArduinoBlue->BlinkArduinoInterface. This script handles communication to and from the Arduino, and has variables for changing the behaviour of that communication.

That same game object also has the second script controlling the action, the BlinkLEDOnArduino.js component. This script controls the blinking on the Arduino of the LED in pin 13. It does this by setting alternating values or 0 or 1 on pin 13 of the Arduinity component. The rate of blinking is controlled by the blinkInterval component.

The third script is located as a component on the LED game object in the project hierarchy. This script watches the value of pin 13 on the Arduinity component and switches the in-scene light on or off depending on whether the current value is 0 or 1.


## Second Example - Fade LED
The second example, in a scene called FadeLED, demonstrates how to send a pulse width modulation(PWM) signal to the Arduino from Unity. This example is based on the Fade example found the Arduino IDE, but uses the same circuit as found in the Blink example.

Open the FadeLED scene and press play, the same Arduino program as the last is used for this example. The LED should fade on and off rather than blink.

The structure of game objects in the Hierarchy is identical to the BlinkLED scene, but the scripts are slightly different. Instead of sending a high or low (0 or 1) value to the Arduinity digital pin values array, a value between 0 and 255 is sent from the FadLEDOnScreen script. Play the scene again with the ArduinoInterface game object selected, and you should see DigitalPins Element13(that's pin 13 on the Arduino) changing in value from 0 to 255.

The Arduinity component is where all setup and communication happens with the Arduino. Notice that in the DigitalPinModes, Element 13 now has a value of OUTPUT_PWM. The Arduino's pin modes can be set in the Arduinity Digital Pin Modes array. This value indicated to the script that it should set pin 13 to OUTPUT_PWM during the scene setup.

For the last example, this value was set to OUTPUT, which corresponds to the standard Arduino OUTPUT pin mode. Other possible values are self explanatory, with the exception of IGNORE. It is used to avoid sending and receiving data from the Arduino.

The other values in the Arduinity component are worth a reviewing:

The arduinityName in your Arduino sketch must match the arduinoName in the Arduinity component in your scene. This allows multiple Arduinos to be used in a scene by giving each a separate name.

The ConnectOnStart flag tells Arduinity to attempt to pole devices when the scene starts. If this is not set, you are responsible for connecting to Arduino yourself using the Arudinity.ConnectToArduino() function.

The ContinuousUpdate flag tells Arduinity whether to read and write values during every Unity frame or not.

The SerialSpeed value must match the value specified in your Arduino program.


## Third Example - Read Analog Voltage
The third example, in a scene called ReadAnalogVoltage, demonstrates how read an analog pin value from the Arduino to Unity. This example is based on the ReadAnalogVoltage example found in the Arduino IDE and uses the same circuit. The center pin of a potentiometer should be connected to pin A0, and the two ends of the pot to 5V and GND.

Open the ReadAnalogVoltage scene and press the play button. Turning the potentiometer should change the orientation of the red lever in the Game view. You can see in this example that the AnalogPinModes Element 0 has been set to INPUT.

The component used for logic in this example, ReadVoltageOnScreen, is coded in C# rather than javascript. 


## Fourth Example - Servo Knob
The fourth example, in a scene called ServoKnob, demonstrates how to send arbitrary messages to the Arduino, and how to modify the Arduino program to execute more complex code. This example is based on the ServoKnob example from the Arduino IDE, however, there is a difference. The servo is controlled from a lever in the game screen rather than from a physical potentiometer.

The process of moving a servo is more complicated than blinking a light, so we need to use an Arduino library for control. That process is more complex than simply writing to a pin, we need to do servo setup and write an angle value to the created software servo object. To do this, the base Arduino sketch has been modified to allow two custom messages to be sent from Unity. One tells the Arduino which pin the servo is attached to, the other tells what angle the servo should be set to.

To run this scene, open the ArduinitySerialWithServoFunction program and upload it to your Arduino. The circuit for the servo is just connecting power to 5V on the Arduino, ground to GND on the Arduino, and the control wire to pin 9 on the Arduino.

After the Arduino and the circuit are setup, press play with the ServoKnob scene open in Unity. The lever should move servo between 0 and 180 degrees.


## Troubleshooting
How do I get more information about what's wrong?
There are two commented defines at the top of the Arduinity.cs file. Change the line 

//#define debugSerial

to

\#define debugSerial

You should see messages in the Unity console now. Greater detail can be see by uncommenting the line below with debugSerialMessages.

My Arduino is connected with USB, but Arduinity can't connect in Unity. What can I do?
One explanation is that the serial port is being used by another application, a likely candidate is the Arduino IDE. Close other applications which may be addressing the same serial port that you want to use, and try reopening the project in Unity.

If that doesn't work, you may be the victim of a Microsoft serial numbering bug; name COM1 - COM9 work properly, but sometimes COM10 and up aren't read correctly. Try plugging into another USB port and restarting Unity. See here for more information: http://support.microsoft.com/default.aspx?scid=kb;EN-US;q115831


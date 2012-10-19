/*
 * 
 * The Arduinity component can function as a dispatcher to
 * and from an Arduino.
 * 
 * 
 * 
 * 
 */

// the following two defines can be uncommented for debugging
// serial messages sent to and received from the Arduino
// via the Arduinity Communicator
//#define debugSerial
//#define debugSerialMessages // greater detail

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;

public enum ArduinoDigitalPinMode
{
    IGNORE,
    INPUT,
    INPUT_PULLUP,
    OUTPUT,
    OUTPUT_PWM // don't really need this, but oh well
}

/*
 * Analog In pins can act as digital I/O
 * but that's not supported yet
 */
public enum ArduinoAnalogPinMode
{
    IGNORE,
    INPUT,
    //INPUT_DIGITAL,
    //OUTPUT_DIGITAL
}

public class Arduinity : MonoBehaviour
{
    // this is the name uploaded in Arduino sketch
    // default name is: arduinity1
    // change yours accordingly
    public string arduinoName = "arduinity1";

    // digital pin modes for setting up and reading from arduino
    public ArduinoDigitalPinMode[] digitalPinModes = new ArduinoDigitalPinMode[14];

    // digital pin values
    // when updated continuously, each frame will write or read pins
    // indicated above
    public int[] digitalPins = new int[14];

    // analog pin modes for reading from arduino
    // length of array should match analogPins below
    public ArduinoAnalogPinMode[] analogPinModes = new ArduinoAnalogPinMode[6];

    // analog pin values
    // when updated continuously, each frame will write or read pins
    // indicated above
    public int[] analogPins = new int[6];

    // attempt to connect to arduino when the scene starts?
    public bool connectOnStart = true;

    // continuously update input and output pins each frame?
    public bool continuousUpdate = false;

    // serial speed in Unity needs to match that set in Arduino code
    public int serialSpeed = 57600;

    private ArduinityCommunicator arduinoCommunicator;

    public bool IsSetup()
    {
        if (arduinoCommunicator == null
            || !arduinoCommunicator.IsSetup())
        {
            return false;
        }
        return true;
    }

    void Start()
    {
        if (connectOnStart)
        {
            if (!ConnectToArduino(arduinoName, serialSpeed))
            {
                #if debugSerial
                Debug.Log("ArduinitySerialInterface::Start(): could not connect to Arduino named " + arduinoName);
                #endif
            }
        }
    }

    void LateUpdate()
    {
        if (continuousUpdate && IsSetup())
        {
            // TODO: wrap this all in an updatepins function
            string message;
            // TODO: switch this to batch mode
            for (int i = 0; i < digitalPins.Length; i++)
            {
                if (digitalPinModes[i] == ArduinoDigitalPinMode.INPUT 
                    || digitalPinModes[i] == ArduinoDigitalPinMode.INPUT_PULLUP)
                {
                    // response should be something like 'arduinity_dvalue 6 1'
                    // which is pin six value high
                    string response = arduinoCommunicator.SendReceiveMessage("arduinity_dread " + i);

                    // read the last number in the response string
                    digitalPins[i] = int.Parse(response.Substring(response.LastIndexOf(' ') + 1));
                }
                else if (digitalPinModes[i] == ArduinoDigitalPinMode.OUTPUT)
                {
                    message = "arduinity_dwrite " + i + " " + digitalPins[i];
                    arduinoCommunicator.SendMessage(message);
                    #if debugSerialMessage
                    Debug.Log("ArduinitySerialInterface::LateUpdate(): sent message: " + message);
                    #endif
                }
                else if (digitalPinModes[i] == ArduinoDigitalPinMode.OUTPUT_PWM)
                {
                    message = "arduinity_pwrite " + i + " " + digitalPins[i];
                    arduinoCommunicator.SendMessage(message);
                    #if debugSerialMessage
                    Debug.Log("ArduinitySerialInterface::LateUpdate(): sent message: " + message);
                    #endif
                }
            }

            for (int i = 0; i < analogPins.Length; i++)
            {
                if (analogPinModes[i] == ArduinoAnalogPinMode.INPUT)
                {
                    string response = arduinoCommunicator.SendReceiveMessage("arduinity_aread " + i);
                    // this will protect from trying to parse empty strings
                    if (response == "" || response == null) continue;
                    analogPins[i] = int.Parse(response.Substring(response.LastIndexOf(' ') + 1));
                }
                /*else if (analogPinModes[i] == ArduinoAnalogPinMode.OUTPUT_DIGITAL)
                {
                    message = "arduinity_dwrite A" + i + analogPins[i];
                    arduinoCommunicator.SendMessage(message);
                }*/
            }
            #if debugSerial
            Debug.Log(arduinoCommunicator.ReceiveMessage());
            #endif
        }
    }
	/*
	 *  Handles connecting to a named Arduino and setting pin modes.
	 */
    public bool ConnectToArduino(string name, int _serialSpeed)
    {
        arduinoCommunicator = ArduinityCommunicator.GetArduinoByName(arduinoName, _serialSpeed);
        if (arduinoCommunicator == null) return false;
        // now set the pinmodes
        for (int i = 0; i < digitalPinModes.Length; i++)
        {
            if (digitalPinModes[i] != ArduinoDigitalPinMode.IGNORE)
            {
                string pinModeString = "";
                switch(digitalPinModes[i])
                {
                    case ArduinoDigitalPinMode.INPUT:
                        pinModeString = "INPUT";
                        break;
                    case ArduinoDigitalPinMode.OUTPUT:
                        pinModeString = "OUTPUT";
                        break;
                    case ArduinoDigitalPinMode.INPUT_PULLUP:
                        pinModeString = "INPUT_PULLUP";
                        break;
                    default: // shouldn't get here, very bad....
                        pinModeString = "INPUT";
                        break;
                }

                string message = "arduinity_dpinmode " + i + " " + pinModeString;

                arduinoCommunicator.SendMessage(message);
            }
        }
        return true;
    }

    public bool SendMessageToArduino(string message)
    {
        if (!IsSetup()) return false;
        arduinoCommunicator.SendMessage(message);
        return true;
    }
}
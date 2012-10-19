//#define debugSerial
//#define debugSerialMessages // greater detail

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;

public class ArduinityCommunicator
{
    // raw serial port name, probably something like 'COM4'
    string portName;

    // speed of serial communication
    // needs to match Arduino program
    int serialSpeed = 57600;

    // serial port for communicating with the Arduino
    SerialPort serialPort;

    // each arduino needs a name
    string arduinoName;

    public ArduinityCommunicator(string _portName, int _serialSpeed)
    {
        portName = _portName;
        serialSpeed = _serialSpeed;
    }

    public bool OpenConnection()
    {
        try
        {
            serialPort = new SerialPort("\\\\.\\" + portName, serialSpeed);
            serialPort.Open();
            if (serialPort.IsOpen)
            {
                serialPort.ReadTimeout = 1000 / 120; // this is half a unity frame
                serialPort.WriteTimeout = 1000 / 120;
                serialPort.WriteLine("arduinity_check"); // send message to arduino

                string reply = "";

                while (reply == "")
                {
                    reply = serialPort.ReadLine();
                    if (reply != null && reply != "")
                    {
                        if (reply.StartsWith("arduinity_present"))
                        {
                            #if debugSerial
                            Debug.Log("ArduinityCommunicator::OpenConnection(): found arduinity on " + portName);
                            #endif

                            // parse the name 
                            arduinoName = reply.Substring(reply.LastIndexOf('_') + 1);

                            return true;
                        }
                        else
                        {
                            #if debugSerial
                            Debug.Log("ArduinityCommunicator::OpenConnection(): got wrong response on " + portName + " " + reply);
                            #endif

                            return false;
                        }
                    }
                }
            }
            else
            {
                #if debugSerial
                Debug.Log("ArduinityCommunicator::OpenConnection(): " + portName + " is not open");
                #endif
                return false;
            }
        }
        #if debugSerial
        catch(Exception e)
        {
            Debug.Log("ArduinityCommunicator::OpenConnection(): exception trying to read from " + portName + " " + e.Message);
            return false;
        }
        #else
        catch
        {   
            return false;
        }
        #endif
        return false;
    }

    public bool IsSetup()
    {
        if (serialPort != null && serialPort.IsOpen && arduinoName != "") return true;
        return false;
    }

    public bool SendMessage(string messageToArduino)
    {
        try // this carries a ton of overhead, remove if concerned about speed
        {
            // send message
            serialPort.WriteLine(messageToArduino);
        }
        #if debugSerialMessages
        catch (Exception e)
        {
            
            Debug.Log("ArduinityCommunicator::SendMessage(): " + e);
            
            return false;
        }
        #else
        catch
        {
            return false;
        }
        #endif
        return true;
    }

    public string ReceiveMessage()
    {
        string response = "";

        if (IsSetup())
        {
            try
            {
                while (response == "" || response == null)
                {
                    response = serialPort.ReadLine();
                    if (response != null && response != "")
                    {
                        #if debugSerial
                        Debug.Log("ArduinityCommunicator::RecieveMessage(): got response " + response);
                        #endif
                        return response;
                    }
                }
            }
            #if debugSerial
            catch (Exception e)
            {
                Debug.Log("ArduinityCommunicator::RecieveMessage(): exception trying to read " + e.Message);
            }
            #else
            catch
            {

            }
            #endif
        }
        else
        {
#if debugSerial
            Debug.Log("ArduinityCommunicator::RecieveMessage(): serial is not open for reading");
#endif
        }

        return response;
    }

    public string SendReceiveMessage(string messageToArduino)
    {
        string response = "";

        // keeps the arduino from sending too quickly
        //arduinity.WriteLine("arduinity_begin_instructions");

        SendMessage(messageToArduino);

        // tell arduino message is done
        //arduinity.WriteLine("arduinity_end_instructions");

        response = ReceiveMessage();

        return response;
    }

    static public ArduinityCommunicator GetArduinoByPortname(string _portName, int _serialSpeed)
    {
        ArduinityCommunicator arduinoCommunicator = new ArduinityCommunicator(_portName, _serialSpeed);
        if (arduinoCommunicator.OpenConnection())
            return arduinoCommunicator;

        return null;
    }

    static public ArduinityCommunicator GetArduinoByName(string name, int _serialSpeed)
    {
        ArduinityCommunicator arduinoCommunicator = null;

        // get a list of portnames 
        string[] theSerialPortNames = System.IO.Ports.SerialPort.GetPortNames();

        // portName 'COM13' appears to not work for some reason....
        // it's a bug in .Net, com ports over 10 do not work.
        foreach (string portName in theSerialPortNames)
        {
            arduinoCommunicator = GetArduinoByPortname(portName, _serialSpeed);
            //if ( TestForArduinityByPortName(portName, _serialSpeed) == name)
            if (arduinoCommunicator != null
                && arduinoCommunicator.arduinoName == name)
            {
                return arduinoCommunicator;
                // wait for serial to open back up
                //System.Threading.Thread.Sleep(500);

                //arduinoCommunicator = new ArduinityCommunicator(portName, _serialSpeed);
            }
        }
        return arduinoCommunicator;
    }
}
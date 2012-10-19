/*
*   BlinkLED
*       This is the Arduinity version of the Blink example found
*       in the Arduino IDE in Examples->01.Basics->Blink
*
*       This script expects the ArduinitySerialInterface 
*       component has been added to this game object has 
*       digitalPinModes[13] set to OUTPUT, connectOnStart set
*       to true, and continuousUpdate set to true.
*
*       The attached Arduino is expected to be a 
*       standard 14 digital pin Arduino(ie. Leonardo).
*
*       The circuit is an LED attached to pin 13 and GND.
*
*/

#pragma strict

// this script only works on gameObjects with ArduinitySerialInterface

var blinkPin : int = 13;
var blinkInterval : float = 1.0f;

private var lastBlink : float = 0.0f;

private var arduinity : Arduinity;


function Start () {
    arduinity = gameObject.GetComponent("Arduinity") as Arduinity;
}

function Update()
{
    if (Time.time - lastBlink > blinkInterval) 
    {
        arduinity.digitalPins[blinkPin] = arduinity.digitalPins[blinkPin] == 0 ? 1 : 0;
        lastBlink = Time.time;
    }
}
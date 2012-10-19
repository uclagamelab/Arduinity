/*
*   FadeLEDOnArduino
*       This is the Arduinity version of the Fade example found
*       in the Arduino IDE in Examples->01.Basics->Fade
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
*/

#pragma strict

// this script only works on gameObjects with ArduinitySerialInterface

var fadePin : int = 13;
var fadeInterval : float = 0.03f; // set this really low so we notice the effect
var fadeAmount : int = 5;
var brightness : int = 0;

private var lastFade : float = 0.0f;

private var arduinity : Arduinity;


function Start () {
    arduinity = gameObject.GetComponent("Arduinity") as Arduinity;
}

function Update()
{
    if (Time.time - lastFade > fadeInterval) 
    {
        arduinity.digitalPins[fadePin] = brightness;
        brightness += fadeAmount;
        if ( brightness <= 0 || brightness >= 255)
        {
            fadeAmount = -fadeAmount;
        }
        lastFade = Time.time;
    }
}
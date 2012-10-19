/*
*   ReadVoltageOnArduino
*       This is the Arduinity version of the ReadAnalogVoltage example found
*       in the Arduino IDE in Examples->01.Basics->ReadAnalogVoltage
*
*       This will simply read a value off the Arduinity
*       component in your scene, and adjust the meter on screen.
*
*       This script expects the Arduinity
*       component has been added to your scene and has 
*       analogPinModes[0] set to INPUT, connectOnStart set
*       to true, and continuousUpdate set to true.
*
*       The circuit is the same as the original Arduino example:
*       "Attach the center pin of a potentiometer to pin A0, 
*       and the outside pins to +5V and ground."
*
*/

using UnityEngine;
using System.Collections;

public class ReadVoltageOnScreen : MonoBehaviour {

    public Arduinity arduinityToWatch;
    public int analogPinToWatch;

    // change these if you want the needle to move more or less
    public float minRotationXZ = -45.0f;
    public float maxRotationXZ = 45.0f;

    // max min values for pot
    public int minPotValue = 0;
    public int maxPotValue = 1024;

    // lerp between these two values
    public Quaternion minRotation;
    public Quaternion maxRotation;

    public float pinVal;
    public float needleAmount;

	// Use this for initialization
	void Start () {
        // set the max and min rotations based on the initial
        // object orientation
        minRotation = transform.rotation;
        // obsolete
        //minRotation.SetEulerAngles(minRotationXZ, 0.0f, minRotationXZ);
        minRotation = Quaternion.Euler(minRotationXZ, 0.0f, minRotationXZ);
        
        maxRotation = transform.rotation;
        // obsolete
        //maxRotation.SetEulerAngles(maxRotationXZ, 0.0f, maxRotationXZ);
        maxRotation = Quaternion.Euler(maxRotationXZ, 0.0f, maxRotationXZ);
	}
	
	// Update is called once per frame
	void Update () {
	    pinVal = arduinityToWatch.analogPins[analogPinToWatch];
        needleAmount = (pinVal - minPotValue) / maxPotValue;
        transform.rotation = 
            Quaternion.Lerp(minRotation, maxRotation, needleAmount);
	}
}

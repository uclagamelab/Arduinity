/*
*   ControlServoOnScreen
*       This is the Arduinity version of the Knob example found
*       in the Arduino IDE in Examples->Servo->Knob
 *       
 *      Instead of reading a potentimeter to control the position
 *      of the servo, this script controls a on-screen lever
 *      which can be pulled back and forth.
*
*       Unlike some of the other Arduinity examples, this script
 *       does not require a pin setup in the Arduinity component.
 *       This script requires a special version of the Arduinity sketch
 *       loaded on the Arduino: it has an extra bit of code to interface
 *       with the Servo Arduino library.
*
 *      This script uses the general messaging capability of 
 *      Arduinity to pass a servo attach command, and then a
 *      servo position during each frame.
*
*       The circuit is the same as the original Arduino example:
*       Pin 9 is attached to the control wire on a 180 degree servo.
 *      Ground wire connected to a GND pin on the Arduino, and 
 *      power wire connected to the +5V pin on the Arduino.
*
*/

using UnityEngine;
using System.Collections;

public class ControlServoOnScreen : MonoBehaviour {

    public Arduinity arduinityToDispatchServo;
    public Transform objectToRotate;
    public int servoPin;

    // change these if you want the needle to move more or less
    public float minRotationXZ = -89.0f;
    public float maxRotationXZ = 89.0f;

    // max min angle values for servo
    public int minServoAngle = 0;
    public int maxServoAngle = 179;

    // lerp between these two values
    public Quaternion minRotation;
    public Quaternion maxRotation;

    public float needleAmount = 0.0f; // range between 0 and 1.0f
    public bool servoAttached = false;

    public int mouseStartX;
    public float startNeedleAmount = 0.0f;

    public int servoDegrees;

	// Use this for initialization
	void Start () {
        // set the max and min rotations based on the initial
        // object orientation
        minRotation = objectToRotate.rotation;
        minRotation = Quaternion.Euler(minRotationXZ, 0.0f, minRotationXZ);
        maxRotation = objectToRotate.rotation;
        maxRotation = Quaternion.Euler(maxRotationXZ, 0.0f, maxRotationXZ);
	}
	
	// Update is called once per frame
	void Update () {
        // set up the servo if necessary
        if (!servoAttached)
        {
            // the servo library on the Arduino requires a function to be called to
            // "attach" a servo to a pin.
            servoAttached = arduinityToDispatchServo.SendMessageToArduino("arduinity_attachservo " + servoPin);
            if (!servoAttached)
            {
                Debug.Log("unable to attach servo pin");
            }
        }
	    else
        {
           // rotate the needle
            objectToRotate.rotation = 
                Quaternion.Lerp(minRotation, maxRotation, needleAmount);
            servoDegrees = (int)(minServoAngle + (needleAmount * maxServoAngle));
            string servoInstructions =
                "arduinity_setservo " + servoDegrees;
            arduinityToDispatchServo.SendMessageToArduino(servoInstructions);
           
        }
	}

    void OnMouseDown()
    {
        mouseStartX = (int) Input.mousePosition.x;
        startNeedleAmount = needleAmount;
    }

    void OnMouseDrag()
    {
        if (Input.mousePosition.x < 0)
        {
            needleAmount = 0.0f;
        }
        else if (Input.mousePosition.x > Screen.width)
        {
            needleAmount = 1.0f;
        }
        else if (Input.mousePosition.x > mouseStartX)
        {
            float mouseRange = Screen.width - mouseStartX;
            float positionPercent = (Input.mousePosition.x - mouseStartX) / mouseRange;
            float needleRange = 1.0f - startNeedleAmount;
            float amountToAdd = Mathf.Abs(positionPercent * needleRange);
            needleAmount = startNeedleAmount + amountToAdd;
            if (needleAmount > 1.0f)
            {
                needleAmount = 1.0f;
            }
        }
        else
        {
            float mouseRange = mouseStartX;
            float positionPercent = (float)(Input.mousePosition.x) / mouseRange;
            float needleRange = startNeedleAmount;
            float amountToAdd = Mathf.Abs(positionPercent * needleRange);
            needleAmount = amountToAdd;
            if (needleAmount < 0.0f)
            {
                needleAmount = 0.0f;
            }
        }
    }
}

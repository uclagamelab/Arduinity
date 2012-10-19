#include <Servo.h>

// communication
int serialSpeed = 57600; // needs to match arduinity in unity

// name for this arduinity, should be unique
String arduinityName = "arduinity1";

// serial buffer
String   serialInputString = ""; // stores incoming chars
int      serialCharBuffer = 200; // maximum message length
boolean  serialStringComplete = false; // store chars until message complete

Servo myServo;

void setup()
{
  Serial.begin(serialSpeed);
  serialInputString.reserve(serialCharBuffer);
}

void loop()
{ 
  // get one serial input and put into a string
  if (Serial.available() > 0 && !serialStringComplete)
  {
    char inChar = (char)Serial.read(); 
    
    if (inChar == '\n') {
      serialStringComplete = true;
    } 
    else if (inChar == '\r') // asci 13, windows line endings suck
    {
      // do nothing
    }
    else serialInputString += inChar;
  }
  
  // parse the command received
  if (serialStringComplete)
  {
    if (serialInputString == "arduinity_check")
    {
      // TODO: this is slightly inconsistent, last _ should be space
      Serial.println("arduinity_present_" + arduinityName);
    }
    // dpinmode message should be:
    //  arduinity_dpinmode PINNUM MODE
    else if (serialInputString.startsWith("arduinity_dpinmode"))
    {
      int i = 19; // char 18 should be SPACE
      String pinNumberString = "";
      String modeString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      for (i++ ; i < serialInputString.length(); i++)// new char is a SPACE
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else modeString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      
      int pinNumber = atoi(pinNumberCharArray);
      if (modeString == "INPUT")
      {
        pinMode(pinNumber, INPUT);
      }
      else if (modeString == "OUTPUT" || modeString == "OUTPUT_PWM")
      {
        pinMode(pinNumber, OUTPUT);
      }
      else if (modeString == "INPUT_PULLUP")
      {
        pinMode(pinNumber, INPUT_PULLUP);
      }
    }
    /*
    else if (serialInputString.startsWith("arduinity_apinmode"))
    {
      int i = 19; // char 18 should be SPACE
      String pinNumberString = "";
      String modeString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      for (i++ ; i < serialInputString.length(); i++)// new char is a SPACE
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else modeString += newChar;
      }
      int pinNumber = -9876;
      if (pinNumberString == "A0") pinNumber = A0;
      else if (pinNumberString == "A1") pinNumber = A1;
      else if (pinNumberString == "A2") pinNumber = A2;
      else if (pinNumberString == "A3") pinNumber = A3;
      else if (pinNumberString == "A4") pinNumber = A4;
      else if (pinNumberString == "A5") pinNumber = A5;
      else if (pinNumberString == "A6") pinNumber = A6;
      else if (pinNumberString == "A7") pinNumber = A7;
      else if (pinNumberString == "A8") pinNumber = A8;
      else if (pinNumberString == "A9") pinNumber = A9;
      else if (pinNumberString == "A10") pinNumber = A10;
      else if (pinNumberString == "A11") pinNumber = A11;
      // uncomment the lines below to set analog pins on Mega
      //else if (pinNumberString == "A12") pinNumber = A12;
      //else if (pinNumberString == "A13") pinNumber = A13;
      //else if (pinNumberString == "A14") pinNumber = A14;
      //else if (pinNumberString == "A15") pinNumber = A15;
      if (pinNumber = -9876) return;
      
      if (modeString == "INPUT")
      {
        pinMode(pinNumber, INPUT);
      }
      else if (modeString == "OUTPUT_DIGITAL")
      {
        pinMode(pinNumber, OUTPUT);
      }
      
    }*/
    else if (serialInputString.startsWith("arduinity_dwrite"))
    {
      int i = 17; // char 16 should be SPACE
      String pinNumberString = "";
      String writeValueString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      for (i++ ; i < serialInputString.length(); i++)// new char is a SPACE
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else writeValueString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      
      char writeValueCharArray[writeValueString.length()+1];
      writeValueString.toCharArray(writeValueCharArray, writeValueString.length()+1);
      
      int pinNumber = atoi(pinNumberCharArray);
      int writeValue = atoi(writeValueCharArray);
      
      digitalWrite(pinNumber, writeValue);
    }
    else if (serialInputString.startsWith("arduinity_pwrite"))
    {
      int i = 17; // char 16 should be SPACE
      String pinNumberString = "";
      String writeValueString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      for (i++ ; i < serialInputString.length(); i++)// new char is a SPACE
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else writeValueString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      
      char writeValueCharArray[writeValueString.length()+1];
      writeValueString.toCharArray(writeValueCharArray, writeValueString.length()+1);
      
      int pinNumber = atoi(pinNumberCharArray);
      int writeValue = atoi(writeValueCharArray);
      
      analogWrite(pinNumber, writeValue);
    }
    else if (serialInputString.startsWith("arduinity_aread"))
    {
      int i = 16; // char 15 should be a SPACE
      String pinNumberString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      int pinNumber = atoi(pinNumberCharArray);
      int pinValue = analogRead(pinNumber);
      
      // return the value to Unity
      Serial.println("arduinity_avalue " + pinNumberString + " " + pinValue);
    }
    else if (serialInputString.startsWith("arduinity_dread"))
    {
      int i = 16; // char 15 should be a SPACE
      String pinNumberString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      int pinNumber = atoi(pinNumberCharArray);
      int pinValue = digitalRead(pinNumber);
      
      Serial.println("arduinity_dvalue " + pinNumberString + " " + pinValue);
    }
    else if (serialInputString.startsWith("arduinity_attachservo"))
    {
      int i = 22; // char 21 should be a SPACE
      String pinNumberString = "";
      for (i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else pinNumberString += newChar;
      }
      char pinNumberCharArray[pinNumberString.length()+1];
      pinNumberString.toCharArray(pinNumberCharArray, pinNumberString.length()+1);
      int pinNumber = atoi(pinNumberCharArray);
      myServo.attach(pinNumber);
    }
    else if (serialInputString.startsWith("arduinity_setservo"))
    {
      int i = 19; // char 18 should be a SPACE
      String angleString = "";
      for(i ; i < serialInputString.length(); i++)
      {
        char newChar = serialInputString.charAt(i);
        if ( newChar == ' ') break;
        else angleString += newChar;
      }
      char angleCharArray[angleString.length()+1];
      angleString.toCharArray(angleCharArray, angleString.length()+1);
      int angle = atoi(angleCharArray);
      myServo.write(angle);
    }
    else
    {
      Serial.println("arduino serial error: got " + serialInputString);
    }
    
    serialStringComplete = false;
    serialInputString = "";
  }
  
  
}



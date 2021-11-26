// Includes 
#include <IRremote.h>        

// Global notes 
// Code taken from here 
// https://www.makerguides.com/ir-receiver-remote-arduino-tutorial/
// Old remote is RC5
// Roku is NEC as is the new little one that came with board 

// Defines 
#define RECEIVER_PIN          2             // define the IR receiver pin
#define DELAY_BETWEEN_CLICKS  25

#define REPEAT_VALUE          0xFFFFFFFF
#define VOLUME_UP             0x20DF40BF
#define VOLUME_DOWN           0x20DFC03F
#define INPUT_BUTTON          0x20DFD02F
#define OK_BUTTON             0x20DF22DD
#define POWER_BUTTON          0x20DF10EF
#define MUTE_BUTTON           0x20DF906F

// Globals
IRrecv receiver(RECEIVER_PIN);  // create a receiver object of the IRrecv class
decode_results results;         // create a results object of the decode_results class
unsigned long lastValue = 0;    // variable to store the last pressed key value

// Canned set up method 
void setup() 
{
  Serial.begin(9600);     // begin serial communication with a baud rate of 9600
  receiver.enableIRIn();  // enable the receiver
  receiver.blink13(true); // enable blinking of the built-in LED when an IR signal is received
}

// Main loop
void loop() 
{
  // If needed uncomment 
  // printRemoteProtocolType();

  // Show the button values 
  // printButtonCodes();
  handleButtonClicks();
}

void handleButtonClicks()
{
  // Delay for a bit
  delay(250);
  
  // decode the received signal and store it in results
  if (receiver.decode(&results)) 
  { 
    // if repeat value set to last 
    if (results.value == REPEAT_VALUE) 
    { 
      // if the value is equal to REPEAT_VALUE
      results.value = lastValue; 
    }

    // compare the value to the following cases
    switch (results.value) 
    { 
      case VOLUME_UP: 
        Serial.println("VOLUME_UP"); 
        break;
      case VOLUME_DOWN:
        Serial.println("VOLUME_DOWN");
        break;
      case INPUT_BUTTON:
        Serial.println("INPUT_BUTTON");
        break;
      case OK_BUTTON:
        Serial.println("OK_BUTTON");
        break;
      case POWER_BUTTON:
        Serial.println("POWER_BUTTON");
        break;
      case MUTE_BUTTON:
        Serial.println("MUTE_BUTTON");
        break;
    }

    // Store the last value 
    lastValue = results.value;

    // Start the receiver back up 
    receiver.resume(); 
  }
}

void printButtonCodes()
{
    if (receiver.decode(&results)) 
    { 
      // decode the received signal and store it in results
      Serial.println(results.value, HEX); // print the values in the Serial Monitor
      receiver.resume();                  // reset the receiver for the next code
    }
}

void printRemoteProtocolType()
{
    // Read out if data is available 
  receiver.decode();
  
  if (receiver.decode(&results)) 
  //if(receiver.decodedIRData.results != null)
  {
    if (results.value == 0XFFFFFFFF) 
    {
      results.value = lastValue;
    }
    Serial.println(results.value, HEX);
    switch (results.decode_type) {
      case NEC:
        Serial.println("NEC");
        break;
      case SONY:
        Serial.println("SONY");
        break;
      case RC5:
        Serial.println("RC5");
        break;
      case RC6:
        Serial.println("RC6");
        break;
      case DISH:
        Serial.println("DISH");
        break;
      case SHARP:
        Serial.println("SHARP");
        break;
      case JVC:
        Serial.println("JVC");
        break;
//      case SANYO:
//        Serial.println("SANYO");
//        break;
//      case MITSUBISHI:
//        Serial.println("MITSUBISHI");
//        break;
      case SAMSUNG:
        Serial.println("SAMSUNG");
        break;
      case LG:
        Serial.println("LG");
        break ;
      case WHYNTER:
        Serial.println("WHYNTER");
        break;
//      case AIWA_RC_T501:
//        Serial.println("AIWA_RC_T501");
//        break;
      case PANASONIC:
        Serial.println("PANASONIC");
        break;
      case DENON:
        Serial.println("DENON");
        break;
      case BOSEWAVE:
        Serial.println("BOSEWAVE");
        break;
      case LEGO_PF:
        Serial.println("LEGO_PF");
        break;
      case MAGIQUEST:
        Serial.println("MAGIQUEST");
        break;
      default:
      case UNKNOWN:
        Serial.println("UNKNOWN");
        break ;
    }
    lastValue = results.value;
    receiver.resume();
  }
}

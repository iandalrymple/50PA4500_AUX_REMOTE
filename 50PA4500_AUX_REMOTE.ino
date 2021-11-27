// Includes 
#include <IRremote.h>        

// Global notes 
// Code taken from here 
// https://www.makerguides.com/ir-receiver-remote-arduino-tutorial/
// Old remote is RC5
// Roku is NEC as is the new little one that came with board 

// Defines 
#define IR_RECEIVER_PIN       2           
#define DELAY_BETWEEN_CLICKS  100
#define SOFT_SERIAL_RX_PIN    2
#define SOFT_SERIAL_TX_PIN    3

#define REPEAT_VALUE          0xFFFFFFFF
#define VOLUME_UP             0x20DF40BF
#define VOLUME_DOWN           0x20DFC03F
#define INPUT_BUTTON          0x20DFD02F
#define OK_BUTTON             0x20DF22DD
#define POWER_BUTTON          0x20DF10EF
#define MUTE_BUTTON           0x20DF906F

#define SET_ID_0                0x00
#define SET_ID_1                0x00
#define SPACE                   0x20
#define CR                      0x0D
#define K_COMMAND_1             0x6B
#define F_VOL_COMMAND_2         0x66
#define F_QUERY                 0x66

#define SIZE_RX_BUFFER          250
#define WDG_FOR_RESP            500

byte RX_BUFFER[SIZE_RX_BUFFER];
byte GET_VOLUME_REQ[] = { K_COMMAND_1, F_VOL_COMMAND_2, SPACE, SET_ID_0, SET_ID_1, SPACE, F_QUERY, F_QUERY, CR };

// Globals
byte arVOLUME_UP[4]     = {0x20, 0xDF, 0x40, 0xBF};
byte arVOLUME_DOWN[4]   = {0x20, 0xDF, 0xC0, 0x3F};
byte arINPUT_BUTTON[4]  = {0x20, 0xDF, 0xD0, 0x2F};
byte arOK_BUTTON[4]     = {0x20, 0xDF, 0x22, 0xDD};
byte arPOWER_BUTTON[4]  = {0x20, 0xDF, 0x10, 0xEF};
byte arMUTE_BUTTON[4]   = {0x20, 0xDF, 0x90, 0x6F};

IRrecv receiver(IR_RECEIVER_PIN);   // create a receiver object of the IRrecv class
decode_results results;             // create a results object of the decode_results class
unsigned long lastValue = 0;        // variable to store the last pressed key value

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

  // Handle the button clicks 
  handleButtonClicks();
}

int readFromSerialBuffer()
{
  // Locals 
  int incomingByte = 0; 
  int totalBytes = 0;
  unsigned long startTime = millis();

  // Spin for up to one second 
  while((totalBytes < SIZE_RX_BUFFER) && (millis() - startTime < WDG_FOR_RESP))
  {
    // Pull off bytes from hardware 
    if (Serial.available() > 0) 
    {
      // Read the bytes 
      incomingByte = Serial.read();
  
      // Insert to the buffer 
      RX_BUFFER[totalBytes] = (byte)incomingByte;
  
      // Increment the total bytes
      totalBytes++;
    }
  }

  // Bounce back the result 
  return totalBytes;
}

void handleVOLUME_UP()
{
  // Locals 
  int respLength = 0;
  
  // First we need to get the current volume 
  Serial.write(GET_VOLUME_REQ, sizeof(GET_VOLUME_REQ));

  // Get the response 
  respLength = readFromSerialBuffer();
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
        Serial.write(arVOLUME_UP, 4); 
        // handleVOLUME_UP();
        break;
      case VOLUME_DOWN:
        Serial.write(arVOLUME_DOWN, 4);
        break;
      case INPUT_BUTTON:
        Serial.write(arINPUT_BUTTON, 4);
        break;
      case OK_BUTTON:
        Serial.write(arOK_BUTTON, 4);
        break;
      case POWER_BUTTON:
        Serial.write(arPOWER_BUTTON, 4);
        break;
      case MUTE_BUTTON:
        Serial.write(arMUTE_BUTTON, 4);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

namespace VS_50PA4500_AUX_REMOTE
{
    public class Sender
    {
        Thread senderThread;
        bool stopFlag;
        SerialPort sp;
        string comport;

        SharedMemory sharedMem;
        InputButton inputButton;

        byte[] rxBuffer;

        public Sender(ref SharedMemory inMem, ref InputButton inButton)
        {
            senderThread = null;
            sp = null;
            stopFlag = false;
            comport = "";
            sharedMem = inMem;
            inputButton = inButton;
            rxBuffer = new byte[1000];
        }

        public void startSender(string port)
        {
            senderThread = new Thread(send);
            senderThread.Start();
            StopFlag = false;
            comport = port;
        }

        public bool StopFlag { get => stopFlag; set => stopFlag = value; }

        public bool isThreadRunning()
        {
            return senderThread.IsAlive;
        }

        private void setUpSerialPort()
        {
            // Set up the serial port 
            sp = new SerialPort();
            sp.PortName = comport;
            sp.BaudRate = 9600;
        }

        private void send()
        {
            // Locals 
            setUpSerialPort();

            // Turn on the serial port 
            if (!sp.IsOpen)
                sp.Open();

            // Create the new thread to listen for messages 
            while (!StopFlag)
            {
                // Grab the last message 
                if (sharedMem.getCountQueue() > 0)
                {
                    // Send the response 
                    msgHandler(sharedMem.getLastValue());
                }
            }

            // Clean up the comport 
            sp.Close();
            sp = null;
        }

        private void msgHandler(UInt32 buttonPress)
        {
            // Switch on the message type 
            switch (buttonPress)
            {
                case ButtonValues.VOLUME_UP:
                    handleVOLUME_UP();
                    break;
                case ButtonValues.VOLUME_DOWN:
                    handleVOLUME_DOWN();
                    break;
                case ButtonValues.INPUT_BUTTON:
                    handleINPUT_BUTTON();
                    break;
                case ButtonValues.OK_BUTTON:
                    
                    break;
                case ButtonValues.POWER_BUTTON:
                    
                    break;
                case ButtonValues.MUTE_BUTTON:
                    
                    break;
            }
        }

        private int readBytesFromPort()
        {
            // Locals 
            int totalBytes = 0;
            int tempBytes = 0;
            Stopwatch stopWatch = new Stopwatch();

            // Start the watch 
            stopWatch.Start();

            // Spin for up to one second 
            while (((totalBytes + sp.BytesToRead) < rxBuffer.Length) && (stopWatch.ElapsedMilliseconds < 500))
            {
                // Pull off bytes from hardware 
                if (sp.BytesToRead > 0)
                {
                    // Grab temp holder 
                    tempBytes = sp.BytesToRead;

                    // Read the bytes 
                    sp.Read(rxBuffer, totalBytes, sp.BytesToRead);

                    // Increment the total bytes
                    totalBytes += tempBytes;
                }
            }

            // Close the stop watch 
            stopWatch.Stop();

            // Bounce back the result 
            return totalBytes;
        }

        private void handleVOLUME_UP()
        {
            // Locals 
            int responseStartIndex = -1;
            byte currentVolume = 0;
            byte newVolume = 0;
            byte[] sendArray = new byte[ButtonMessages.sizeSET_VOLUME_REQ];

            // Put the bytes on the wire 
            sp.Write(ButtonMessages.msgGET_VOLUME_REQ, 0, ButtonMessages.sizeGET_VOLUME_REQ);

            // Read the bytes from the port 
            if(readBytesFromPort() >= ButtonMessages.sizeGET_VOLUME_RES)
            {
                // Find the response
                responseStartIndex = GeneralHelpers.findResponseInBuffer(rxBuffer, ButtonMessages.msgGET_VOLUME_RES);

                // Check if response found 
                if (responseStartIndex >= 0)
                {
                    // Parse out the volume now 
                    currentVolume = Convert.ToByte(Encoding.ASCII.GetString(rxBuffer, ButtonMessages.volByteMsbGET_VOLUME_RES, 2), 16);

                    // Get the new volume 
                    newVolume = (byte)(currentVolume + 1);

                    // Get the send message ready 
                    ButtonMessages.msgSET_VOLUME_REQ.CopyTo(sendArray, 0);

                    // Updat the volume bytes 
                    sendArray[ButtonMessages.volByteMsbSET_VOLUME_REQ] = (byte)Encoding.ASCII.GetBytes(newVolume.ToString("X2").ToLower())[0];
                    sendArray[ButtonMessages.volByteLsbSET_VOLUME_REQ] = (byte)Encoding.ASCII.GetBytes(newVolume.ToString("X2").ToLower())[1];

                    // Now send the message 
                    sp.Write(sendArray, 0, sendArray.Length);
                }
            }
        }

        private void handleVOLUME_DOWN()
        {
            // Locals 
            int responseStartIndex = -1;
            byte currentVolume = 0;
            byte newVolume = 0;
            byte[] sendArray = new byte[ButtonMessages.sizeSET_VOLUME_REQ];

            // Put the bytes on the wire 
            sp.Write(ButtonMessages.msgGET_VOLUME_REQ, 0, ButtonMessages.sizeGET_VOLUME_REQ);

            // Read the bytes from the port 
            if (readBytesFromPort() >= ButtonMessages.sizeGET_VOLUME_RES)
            {
                // Find the response
                responseStartIndex = GeneralHelpers.findResponseInBuffer(rxBuffer, ButtonMessages.msgGET_VOLUME_RES);

                // Check if response found 
                if (responseStartIndex >= 0)
                {
                    // Parse out the volume now 
                    currentVolume = Convert.ToByte(Encoding.ASCII.GetString(rxBuffer, ButtonMessages.volByteMsbGET_VOLUME_RES, 2), 16);

                    // Get the new volume 
                    if (currentVolume > 0)
                        newVolume = (byte)(currentVolume - 1);
                    else
                        newVolume = 0;

                    // Get the send message ready 
                    ButtonMessages.msgSET_VOLUME_REQ.CopyTo(sendArray, 0);

                    // Updat the volume bytes 
                    sendArray[ButtonMessages.volByteMsbSET_VOLUME_REQ] = (byte)Encoding.ASCII.GetBytes(newVolume.ToString("X2").ToLower())[0];
                    sendArray[ButtonMessages.volByteLsbSET_VOLUME_REQ] = (byte)Encoding.ASCII.GetBytes(newVolume.ToString("X2").ToLower())[1];

                    // Now send the message 
                    sp.Write(sendArray, 0, sendArray.Length);
                }
            }
        }

        private void handleINPUT_BUTTON()
        {
            // Locals 
            byte nextInput = inputButton.getNextDataField();
            byte[] sendArray = new byte[ButtonMessages.sizeINPUT_BUTTON_REQ];

            // Get the send message ready 
            ButtonMessages.msgINPUT_BUTTON_REQ.CopyTo(sendArray, 0);

            // Updat the volume bytes 
            sendArray[ButtonMessages.inputByteMsbINPUT_BUTTON_REQ] = (byte)Encoding.ASCII.GetBytes(nextInput.ToString("X2").ToLower())[0];
            sendArray[ButtonMessages.inputByteLsbINPUT_BUTTON_REQ] = (byte)Encoding.ASCII.GetBytes(nextInput.ToString("X2").ToLower())[1];

            // Now send the message 
            sp.Write(sendArray, 0, sendArray.Length);
        }
    }
}

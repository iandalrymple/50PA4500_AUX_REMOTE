using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace VS_50PA4500_AUX_REMOTE
{
    class Listener
    {
        Thread listenerThread;
        bool stopFlag;
        SerialPort sp;
        string comport;

        SharedMemory sharedMem;

        public Listener(ref SharedMemory inMem)
        {
            listenerThread = null;
            sp = null;
            stopFlag = false;
            comport = "";
            sharedMem = inMem;
        }

        public void startListener(string port)
        {
            listenerThread = new Thread(listen);
            listenerThread.Start();
            StopFlag = false;
            comport = port;
        }

        public bool StopFlag { get => stopFlag; set => stopFlag = value; }

        public bool isThreadRunning()
        {
            return listenerThread.IsAlive;
        }

        private void setUpSerialPort()
        {
            // Set up the serial port 
            sp = new SerialPort();
            sp.PortName = comport;
            sp.BaudRate = 9600;
        }

        private void listen()
        {
            // Locals 
            byte[] rxBuffer = new byte[4];
            setUpSerialPort();
            int reuseByteCount = 0;

            // Turn on the serial port 
            if (!sp.IsOpen)
                sp.Open();

            // Create the new thread to listen for messages 
            while(!StopFlag)
            {
                //  Reset the reuse byte count 
                reuseByteCount = 0;
                for (int i = 0; i < 4; i++)
                    rxBuffer[i] = 0;

                // Read bytes if available
                while(reuseByteCount < 4)
                {
                    //  Read all the bytes out 
                    if (sp.BytesToRead > 0)
                    {
                        // Read out one byte 
                        sp.Read(rxBuffer, reuseByteCount, 1);

                        // Increment count 
                        reuseByteCount++;
                    }
                }

                // Switch on the message type 
                switch(BitConverter.ToUInt32(rxBuffer.Reverse().ToArray(), 0))
                {
                    case ButtonValues.VOLUME_UP:
                        sharedMem.insertButtonValue(ButtonValues.VOLUME_UP);
                        break;
                    case ButtonValues.VOLUME_DOWN:
                        sharedMem.insertButtonValue(ButtonValues.VOLUME_DOWN);
                        break;
                    case ButtonValues.INPUT_BUTTON:
                        sharedMem.insertButtonValue(ButtonValues.INPUT_BUTTON);
                        break;
                    case ButtonValues.OK_BUTTON:
                        sharedMem.insertButtonValue(ButtonValues.OK_BUTTON);
                        break;
                    case ButtonValues.POWER_BUTTON:
                        sharedMem.insertButtonValue(ButtonValues.POWER_BUTTON);
                        break;
                    case ButtonValues.MUTE_BUTTON:
                        sharedMem.insertButtonValue(ButtonValues.MUTE_BUTTON);
                        break;
                }     
            }

            // Clean up the comport 
            sp.Close();
            sp = null;
        }
    }
}

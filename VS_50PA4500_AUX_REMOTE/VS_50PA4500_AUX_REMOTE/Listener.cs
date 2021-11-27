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

        public Listener(string port)
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

        void setUpSerialPort()
        {
            // Set up the serial port 
            sp = new SerialPort();
            sp.PortName = comport;
            sp.BaudRate = 9600;
        }

        private void listen()
        {
            // Locals 
            byte[] rxBuffer = new byte[1000];
            setUpSerialPort();
            int reuseByteCount = 0;
            int tempBytesToRead = 0;

            // Turn on the serial port 
            if (!sp.IsOpen)
                sp.Open();

            // Create the new thread to listen for messages 
            while(!StopFlag)
            {
                //  Reset the reuse byte count 
                reuseByteCount = 0;

                // Read bytes if available
                while(reuseByteCount < 4)
                {
                    //  Read all the bytes out 
                    if (sp.BytesToRead > 0)
                    {
                        // Temp bytes 
                        tempBytesToRead = sp.BytesToRead;

                        // Read in the bytes
                        sp.Read(rxBuffer, reuseByteCount, sp.BytesToRead);
                        reuseByteCount += tempBytesToRead;
                    }
                }



                // Switch on the message type 
                switch(BitConverter.ToUInt32(rxBuffer, 0))
                {
                    case ButtonValues.VOLUME_UP:
                        break;
                    case ButtonValues.VOLUME_DOWN:
                        break;
                    case ButtonValues.INPUT_BUTTON:
                        break;
                    case ButtonValues.OK_BUTTON:
                        break;
                    case ButtonValues.POWER_BUTTON:
                        break;
                    case ButtonValues.MUTE_BUTTON:
                        break;
                }     
            }

            // Clean up the comport 
            sp.Close();
            sp = null;
        }
    }
}

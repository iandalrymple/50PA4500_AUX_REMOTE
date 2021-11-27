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
            Thread listenerThread = new Thread(new ThreadStart(listen));
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
            sp = new SerialPort();
            sp.PortName = comport;
            sp.BaudRate = 9600;
        }

        private void listen()
        {
            byte[] rxBuffer = new byte[1000];
            setUpSerialPort();

            // Create the new thread to listen for messages 
            while(!StopFlag)
            {
                
            }
        }
    }
}

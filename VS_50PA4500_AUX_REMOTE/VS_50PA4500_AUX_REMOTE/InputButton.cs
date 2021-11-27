using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_50PA4500_AUX_REMOTE
{
    public class InputButton
    {
        private int currentIndex;
        public readonly byte[] dataFields = { 0x00, 0x01, 0x10, 0x11, 0x20, 0x40, 0x41, 0x60, 0x90, 0x91, 0x92 };

        public InputButton()
        {
            currentIndex = 0;
        }

        public byte getNextDataField()
        {
            // Save the return value 
            byte returnValue = dataFields[currentIndex];

            // Deal with the current index 
            currentIndex++;

            // Check for over flow 
            if (currentIndex >= dataFields.Length)
                currentIndex = 0;

            // Bounce back the result 
            return returnValue;
        }
    }
}

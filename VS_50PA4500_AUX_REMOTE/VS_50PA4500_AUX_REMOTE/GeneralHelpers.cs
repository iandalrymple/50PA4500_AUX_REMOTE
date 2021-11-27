using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_50PA4500_AUX_REMOTE
{
    public static class GeneralHelpers
    {

        public static int findResponseInBuffer(byte[] haystack, byte[] needle)
        {
            if (needle.Length == 0)
                return -1;

            for (int i = 0; i <= (haystack.Length - needle.Length); i++)
            {
                if (matchMsg(haystack, needle, i))
                {
                    return i;
                }
            }

           return -1;
        }

        public static bool matchMsg(byte[] haystack, byte[] needle, int start)
        {
            if ((needle.Length + start) > haystack.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i <= needle.Length - 1; i++)
                {
                    if (needle[i] != haystack[i + start])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_50PA4500_AUX_REMOTE
{
    public class ButtonValues
    {
        const UInt32 VOLUME_UP      = 0x20DF40BF;
        const UInt32 VOLUME_DOWN    = 0x20DFC03F;
        const UInt32 INPUT_BUTTON   = 0x20DFD02F;
        const UInt32 OK_BUTTON      = 0x20DF22DD;
        const UInt32 POWER_BUTTON   = 0x20DF10EF;
        const UInt32 MUTE_BUTTON    = 0x20DF906F;
    }
}

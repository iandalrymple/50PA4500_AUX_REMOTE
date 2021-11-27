using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_50PA4500_AUX_REMOTE
{
    public static class ButtonMessages
    {
        private const byte SET_ID_0 = 0x30;
        private const byte SET_ID_1 = 0x31;
        private const byte SPACE = 0x20;
        private const byte CR = 0x0D;
        private const byte K_COMMAND_1 = 0x6B;
        private const byte F_VOL_COMMAND_2 = 0x66;
        private const byte F_QUERY = 0x66;
        private const byte O_OK = 0x4F;
        private const byte K_OK = 0x4B;

        private const byte X_COMMAND_1 = 0x78;
        private const byte B_COMMAND_2 = 0x62;

        public static readonly byte[] msgGET_VOLUME_REQ = { K_COMMAND_1, F_VOL_COMMAND_2, SPACE, SET_ID_0, SET_ID_1, SPACE, F_QUERY, F_QUERY, CR };
        public static readonly int sizeGET_VOLUME_REQ = 9;
        public static readonly byte[] msgGET_VOLUME_RES = { F_VOL_COMMAND_2, SPACE, SET_ID_0, SET_ID_1, SPACE, O_OK, K_OK };
        public static readonly int sizeGET_VOLUME_RES = 10;
        public static readonly int volByteMsbGET_VOLUME_RES = 7;
        public static readonly int volByteLsbGET_VOLUME_RES = 8;

        public static readonly byte[] msgSET_VOLUME_REQ = { K_COMMAND_1, F_VOL_COMMAND_2, SPACE, SET_ID_0, SET_ID_1, SPACE, F_QUERY, F_QUERY, CR };
        public static readonly int sizeSET_VOLUME_REQ = 9;
        public static readonly int volByteMsbSET_VOLUME_REQ = 6;
        public static readonly int volByteLsbSET_VOLUME_REQ = 7;

        public static readonly byte[] msgINPUT_BUTTON_REQ = { X_COMMAND_1, B_COMMAND_2, SPACE, SET_ID_0, SET_ID_1, SPACE, F_QUERY, F_QUERY, CR };
        public static readonly int sizeINPUT_BUTTON_REQ = 9;
        public static readonly int inputByteMsbINPUT_BUTTON_REQ = 6;
        public static readonly int inputByteLsbINPUT_BUTTON_REQ = 7;
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.11.11
// Author       : jemoon
// Description  : ASCII Code
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public enum Asciis
    { 
        NULL = 0x00,
        SOH = 0x01,
        STX = 0x02,
        ETX = 0x03,
        EOT = 0x04,
        ENQ = 0x05,
        ACK = 0x06,
        BEL = 0x07,
        BS = 0x08,
        TAB = 0x09,
        LF = 0x0A,
        VT = 0x0B,
        FF = 0x0C,
        CR = 0x0D,
        SO = 0x0E,
        SI = 0x0F,
        DLE = 0x10,
        DC1 = 0x11,
        DC2 = 0x12,
        DC3 = 0x13,
        DC4 = 0x14,
        NAK = 0x15,
        SYN = 0x16,
        ETB = 0x17,
        CAN = 0x18,
        EM = 0x19,
        SUB = 0x1A,
        ESC = 0x1B,
        FS = 0x1C,
        GS = 0x1D,
        RS = 0x1E,
        US = 0x1F,
        SPACE = 0x20,
        PERCENT = 0x25,
        MINUS = 0x2D,
        DOT = 0x2E,
        DEL = 0x7F
    }

    public class AsciiChar
    {
        public static readonly char NULL = (char)Asciis.NULL;
        public static readonly char SOH = (char)Asciis.SOH;
        public static readonly char STX = (char)Asciis.STX;
        public static readonly char ETX = (char)Asciis.ETX;
        public static readonly char EOT = (char)Asciis.EOT;
        public static readonly char ENQ = (char)Asciis.ENQ;
        public static readonly char ACK = (char)Asciis.ACK;
        public static readonly char BEL = (char)Asciis.BEL;
        public static readonly char BS = (char)Asciis.BS;
        public static readonly char TAB = (char)Asciis.TAB;
        public static readonly char LF = (char)Asciis.LF;
        public static readonly char VT = (char)Asciis.VT;
        public static readonly char FF = (char)Asciis.FF;
        public static readonly char CR = (char)Asciis.CR;
        public static readonly char SO = (char)Asciis.SO;
        public static readonly char SI = (char)Asciis.SI;
        public static readonly char DLE = (char)Asciis.DLE;
        public static readonly char DC1 = (char)Asciis.DC1;
        public static readonly char DC2 = (char)Asciis.DC2;
        public static readonly char DC3 = (char)Asciis.DC3;
        public static readonly char DC4 = (char)Asciis.DC4;
        public static readonly char NAK = (char)Asciis.NAK;
        public static readonly char SYN = (char)Asciis.SYN;
        public static readonly char ETB = (char)Asciis.ETB;
        public static readonly char CAN = (char)Asciis.CAN;
        public static readonly char EM = (char)Asciis.EM;
        public static readonly char SUB = (char)Asciis.SUB;
        public static readonly char ESC = (char)Asciis.ESC;
        public static readonly char FS = (char)Asciis.FS;
        public static readonly char GS = (char)Asciis.GS;
        public static readonly char RS = (char)Asciis.RS;
        public static readonly char US = (char)Asciis.US;
        public static readonly char SPACE = (char)Asciis.SPACE;
        public static readonly char PERCENT = (char)Asciis.PERCENT;
        public static readonly char MINUS = (char)Asciis.MINUS;
        public static readonly char DOT = (char)Asciis.DOT;
        public static readonly char DEL = (char)Asciis.DEL;
    }
}

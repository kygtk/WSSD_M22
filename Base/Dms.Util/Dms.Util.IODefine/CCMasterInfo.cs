using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
	public enum CclinkIoType
	{
		DI,
		DO,
		AI,
		AO,
        NA,
	};

	public enum CclinkTerminalType
	{
		DI,
		DO,
		DIO,
		AI,
		AO,
		BLDC,
		Inverter,
		NA,
	}

	public enum CclinkStationType
	{
		Reserved,
		RemoteIo,
		RemoteDevice,
	};

	public enum CclinkRangeType
	{
		_n10_p10V = 0,
		_0_5V = 1,
		_1_5V = 2,
		_0_20mA = 3,
		_4_20mA = 4
	};

	public enum CclinkMovingAvgType
	{
		_4Times = 0,
		_8Times = 1,
		_16Times = 2,
		_32Times = 3
	};

	public enum CclinkOutputType
	{
		Clear = 0,
		Hold = 1
	};

    [Serializable()]
    public class CClinkMasterInfo
    {
        public devTYPE DevTypeRx = devTYPE.devX;
        public devTYPE DevTypeRy = devTYPE.devY;
        public devTYPE DevTypeRWr = devTYPE.devWr;
        public devTYPE DevTypeRWw = devTYPE.devWw;
        public devTYPE DevTypeSb = devTYPE.devSM;
        public devTYPE DevTypeSw = devTYPE.devSD;
        public int AddressBase = 0;
        public int AddressRx = 0;
        public int AddressRy = 0;
        public int AddressRWr = 0;
        public int AddressRWw = 0;
        public int AddressSb = 0;
        public int AddressSw = 0;
        public int[] ReadyBit; // 11.02.24 minhan Crevis
        public bool ReadyBitUse; // 11.03.07 minhan
        public bool HasTrouble = false;
        public bool[] SationLinkError = new bool[64];
        public bool StationInitReq;
        public bool StationInitFail;
        public bool StationInitComp;
    }
}

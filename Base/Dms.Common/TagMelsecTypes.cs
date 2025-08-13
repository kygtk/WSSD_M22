///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.18
// Author       : jemoon
// Description  : Melsec Common Types
//-------------------------------------------------------------------------
// Revison History
// * Dms.Ctl.Melsec 에서 위치 이동함
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Common
{
    public enum channel
    {
        melsecnet10_Slot1 = 51,
        melsecnet10_Slot2 = 52,
        melsecnet10_Slot3 = 53,
        melsecnet10_Slot4 = 54,
        cclink_Slot1 = 81,
        cclink_Slot2 = 82,
        cclink_Slot3 = 83,
        cclink_Slot4 = 84,
        melsecnetG_Slot1 = 151,
        melsecnetG_Slot2 = 152,
        melsecnetG_Slot3 = 153,
        melsecnetG_Slot4 = 154,
        melsecEthernet = 901
    }

    public enum devTYPE
    {
        devX = 1,		// 1	DevX	X
        devY = 2,		// 2	DevY	Y
        devL = 3,		// 3	DevL	L
        devM = 4,		// 4	DevM	M
        devSM = 5,		// 5	DevSM	Special MSB(link special B for MELSECNET/10 or CC-Link)
        devF = 6,		// 6	DevF	F
        //devTT = 7,		// 7	DevTT	T(contact)
        //devTC = 8,		// 8	DevTC	T(coil)
        //devCT = 9,		// 9	DevCT	C(contact)
        //devCC = 10,		// 10	DevCC	C(coil)
        //devTN = 11,		// 11	DevTN	T(current value)
        //devCN = 12,		// 12	DevCN	C(current value)
        devD = 13,		// 13	DevD	D
        devSD = 14,		// 14	DevSD	SpecialDSW(link special W for MELSECNET/10 or CC-Link)
        //devTM_T = 15,		// 15	DevTM	T(setting value main)
        //devTS_T = 16,		// 16	DevTS	T(setting value sub1)
        //devTS2_T = 16002,	// 16002	DevTS2	T(setting value sub2)
        //devTS3_T = 16003,	// 16003	DevTS3	T(setting value sub3)
        //devCM_C = 17,		// 17	DevCM	C(setting value main)
        //devTS_C = 18,		// 18	DevTS	C(setting value sub1)
        //devTS2_C = 18002,	// 18002	DevTS2	C(setting value sub2)
        //devTS3_C = 18003,	// 18003	DevTS3	C(setting value sub3)
        //devA = 19,		// 19	DevA	A
        //devZ = 20,		// 20	DevZ	Z
        //devV = 21,		// 21	DevV	V(index register)
        devR = 22,		// 22	DevR	R
        // 22001~22256	DevER1~256	Extend R
        devB = 23,		// 23	DevB	B
        devW = 24,		// 24	DevW	W
		devZR = 99,		// 99   DevZR   ZR, 본 아이템은 MelsecNET H/G에서는 지원안함, 임의로 99로 지정
        //devQSB = 25,	// 25	DevQSB	Q/QnA link special relay(Q/QnACPUup)
        //devSTT = 26,	// 26	DevSTT	Retentive timer
        //devSTC = 27,	// 27	DevSTC	Retentive timer
        //devQSW = 28,	// 28	DevQSW	Q/QnA link special register(Q/QnACPUup)
        //devQV = 30,		// 30	DevQV	Q/QnA edge relay(Q/QnACPUup)
        //devSTN = 35,	// 35	DevSTN	Retentive timer
        //devFS = 40,		// 40	DevFS	S(FxCPU)
        // *** // 101	DevMAIL	Q/QnA SEND function(arrival acknowledgement  required) or RECV function
        // *** // 102	DevMAILNC	Q/QnA SEND function(arrival acknowledgement not required)
        // 1001~1255	DevLX1~255	Direct link input
        // 2001~2255	DevLY1~255	Direct link output
        // 23001~23255	DevLB1~255	Direct link relay
        // 24001~24255	DevLW1~255	Direct link register
        // 25001~25255	DevLSB1~255	Direct link special relay(network unit side)
        // 28001~28255	DevLSW1~255	Direct link special register(network unit side)
        // 29000~29255	DevSPG0~255	Special direct buffer register
        // 31000~31255	DevEM0~255	EM(shared device)
        // 32000~32255	DevED0~255	ED(shared device)
        // 8000h	DevRBM	Buffer memory*1
        // 8020h	DevRAB	Random access buffer*1
        // 8021h	DevRX	link input*1
        // 8022h	DevRY	link output*1
        // 8024h	DevRW	link register*1
        // 8063h	DevSB	link special relay*1
        // 8064h	DevSW	link spcial register*1
        // 33	DevMRB	Own station random access buffer*2
        devWw = 36,		//Own station link register(send)*2
        devWr = 37,		//Own station link register(receive)*2
        // 50	DevSPB	Own station buffer memory*3
    };

    public enum LinkMemoryDivision
    {
        NONE,
        LOADER,
        EQP,
        CIM
    }
}

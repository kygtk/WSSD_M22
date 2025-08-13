using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Device
{
    public enum HomeTypeMP2300 : long
    {
        orgDEC_C    = 0,
        orgZERO     = 1,
        orgDEC_ZERO = 2,
        orgC        = 3,
        orgC_ONLY   = 11,
        orgPOT_C    = 12,
        orgPOT      = 13,
        orgLS_C     = 14,
        orgLS       = 15,
        orgNOT_C    = 16,
        orgNOT      = 17,
        orgIN_C     = 18,
        orgIN       = 19,
        orgLS_PC    = 20,	//Fishbone(Z상사용)
		orgEXT_PC	= 21	//TR, R/B(Z상사용하지않음)
    }

    public struct HomeInfoMP2300
    {
        public HomeTypeMP2300 Type;
        public double Vel;          // Unit : mm/sec, Speed when move to final distance
        public double AppVel;       // Unit : mm/sec, Approach Speed
        public double CrpVel;       // Unit : mm/sec, Creep Speed
        public int Acc;				// Unit : msec, ACC/DEC
        public double Dist;         // Unit : mm, Final Distance

		public static HomeInfoMP2300 CreateDefault()
		{
			//Refernce : G4.5 HDC LD/UL Hand Servo case를 적용함
			HomeInfoMP2300 homeInfo;
			homeInfo.Type = HomeTypeMP2300.orgLS_PC;
			homeInfo.Vel = 5;
			homeInfo.AppVel = -20;
			homeInfo.CrpVel = 2;
			homeInfo.Acc = 300;
			homeInfo.Dist = -5;
			return homeInfo;
		}
    }

    public struct AxisInfoMP2300
    {
        public AxisType Type;
        public short Acc;		    // 단위 : msec - 가속 시간
        public double Ratio;	    // 단위 : mm (The distance of 1 rev)
        public uint Encoder;	    // The pulse number of 1 rev
        public double Vel;		    // 단위 : 지령 단위
	    public double Theta;		// 최상점과 Home Sensor의 각도 편차.(RB Servo Use only)
	    public MP2300CmdUnit Unit;  // 지령 단위 : mm, deg, pulse
	    public int Decimal;		    // 지령 최소 단위 : ( ex : 1000 --> 1.000 )
        public double DefaultVel;   // reference velocity
        public short Dec;           //단위 : msec - 감속시간

		public static AxisInfoMP2300 CreateDefault()
		{
			//Refernce : G4.5 HDC LD/UL Hand Servo case를 적용함
			AxisInfoMP2300 axisInfo;
			axisInfo.Type = AxisType.Normal;
			axisInfo.Acc = 400;
			axisInfo.Ratio = 24;
			axisInfo.Encoder = 131072;
			axisInfo.Vel = 0.0;
			axisInfo.Theta = 0;
			axisInfo.Unit = MP2300CmdUnit.mm;
			axisInfo.Decimal = 1000;
			axisInfo.DefaultVel = 1.0;
			axisInfo.Dec = 100;
			return axisInfo;
		}
    }

    public enum MP2300CmdUnit
    {
        mm, pulse, degree
    }

    public enum MP2300Error
    {
        errLink         = 1000,
        errTimeOver     = 1001,
        errGetPosition  = 1002,
        errSetPosition  = 1003,
		errCmdReject  = 1004,
        errServoReady   = 1010,
        errServoOn      = 1011,
        errServoOff     = 1012,
        errHomeStop     = 1020,
        errHomeComp     = 1021,
        errSetHomeComp  = 1022,
        errMovePosNo    = 1100,
        errMoveRbtState = 1101,
        errMoveSensor   = 1102,
        errMoveInPos    = 1103
    }
    //public class MP2300UnitInfo
    //{
    //    //public short    nStartAxis;     // start axis 
    //    //public short    nMaxAxes;       // max_axes
    //    //public short    nMaxPositonNo;  // max. teaching point no
    //    //public short[]  nXyzoToAxis;    // xyz0_to_axis
    //    //public short Mask;              // mask (x, y axis)
    //    public MoveType MoveType;       // robot moving type
    //    public short[] HomeOrder;       // home order
    //    public string[] PointName;      // teaching pointName
    //    public RbtPos[] TeachPoint;     // position : NULL
    //    public ServoSyncInfo SyncInfo;  // Sync Control information (master : 0, slave : 1)
    //    public double VelRatio;         // velocity ratio
    //    //public RbtPos CurPos;
    //    //public AxisStatus[] CurStatus;

    //    public MP2300UnitInfo()
    //    {
    //        SyncInfo = new ServoSyncInfo();
    //    }
    //}
}

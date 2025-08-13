using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Device
{    
    public enum MaxRetry
    { 
        Cnt = 3
    }

    public enum InPosition
    { 
        Margin = 1
    }

    public enum HomeType : short
    { 
        HomeSensorAndIndex = 0,
        HomeSensorOnly = 1,
        HomeMechanicalHardStop = 2
    }

    public enum MoveType : short
    {
        S1Move = 0, // S-move (no use interpolation)
        S2Move = 1, // S-move (use interpolation)
        T1Move = 2, // Trapezoidal move (no use interpolation)
        T2Move = 3, // Trapezoidal move (use interpolation)
        CMove  = 4,	// Coordinated move
        AT1Move = 5   // asymmetry Trapezoidal move (no use interpolation)
    }

    public enum AxisType : short
    {
        Normal,
        RbGap,
    };

    public enum MotorType
    {
        Servo,
        Stepper,
    }

    #region Structure for Axis Control
    public struct HomeInfo
    {
        public double Vel;          // Unit : Pulse
        public short Acc;           // Unit : 4 msec
        public double Dist;         // distance to recheck home sensor
        public HomeType Type;       // Homing type

		public static HomeInfo CreateDefault()
		{
			//Refernce : G7.5 HDC LD/UL UP HAND Z
			HomeInfo homeInfo;
			homeInfo.Vel = 5000.0;
			homeInfo.Acc = 300;
			homeInfo.Dist = 10.0;
			homeInfo.Type = HomeType.HomeSensorOnly;
			return homeInfo;
		}
    }

    public struct AxisInfo
    {
        public AxisType Type;       // kind of Axis : General, R/B Gap(Cam)
        public short Acc;           // Unit : 4 msec - 가속시간
        public double Ratio;        // Unit : mm (the distance of 1 rev.)
        public uint Encoder;        // the pulse number of 1 rev.
        public double Vel;          // current velocity
        public double Theta;		// 최상점과 Home Sensor의 각도 편차.(RB Servo Use only)//2010.03.27 kimgun
        public double DefaultVel;   // reference velocity
        public short Dec;           // Unit : 4 msec (for AT1 move) - 감속시간
        public short EStopRate;     // Unit : 10 msec (for E-Stop Delay rate)

		public static AxisInfo CreateDefault()
		{
			//Refernce : G7.5 HDC LD/UL UP HAND Z
			AxisInfo axisInfo;
			axisInfo.Type = AxisType.Normal;
			axisInfo.Acc = 300;
			axisInfo.Ratio = 10.0;
			axisInfo.Encoder = 8192;
			axisInfo.Vel = 0.0;
            axisInfo.Theta = 60.0; // 10.05.12 minhan
			axisInfo.DefaultVel = 1.0;
			axisInfo.Dec = 100;
			axisInfo.EStopRate = 10;
			return axisInfo;
		}
    }
    #endregion

    #region Structure fot Rbt Control
    [Editor(typeof(UIEditorServoSync), typeof(UITypeEditor))]
    public class ServoSyncInfo
    {
        public Boolean Sync;        // Is synchronized ?
        public ServoMotor Master;   // Master Axis
        public ServoMotor Slave;    // Slave Axis

        public ServoSyncInfo()
        { 
        
        }

        public void Clear()
        {
            Sync = false;
            Master = null;
            Slave = null;
        }
        public override string ToString()
        {
            if (!this.Sync)
            {
                return "False";
            }
            else
            { 
                string temp = string.Format("Sync control : M({0:d}) - S({1:d})", Master.AxisId, Slave.AxisId);
                return temp;
            }
        }
    }

    // ServoUnit Information
    //--------------------------------------------------------------------
    // Information of each Unit
    // robot 1 : x, y ( linked )
    // robot 2 : y, z
    // robot 3 : z
    // robot 4 : y, z
    // robot 5 : z
    //
    // 1. start_axis : Start exis number. ex) Transfer Unit : 0, 1 -> start_axis = 0
    // 2. max_axes : number of axes.  ex) transfer Unit : 0, 1 -> max_axes = 2;
    // 3. mask : masking for used axis
    //    0x03 => x,y; 0x04 => z;  0x06 => y,z; x, y, z => 0 1 1 1 => mask = 0x07
    // 4. max_pos_no : teaching point number
    // 5. move_type : robot moving type
    // 6. home_order :   x  y  z  o
    //                  {1, 1, 0, 2} : z-> x, y (simultaneous) -> 0
    //                  {0, 0, 0, 0} => simultaneous homing all axes, {0, 1, 2, 3} => x=>y=>z=>0
    // 7. xyz0_to_axis : index for used axes
    // 8. link :  master slave  is_linked ( is sync control ?)
    //           {   0 ,   1 ,    TRUE   }
    //    ex)  Transfer Unit : start_axis = 0 , master axis = start_axis + master(0) = 0
    //         slave axis = start_axis + slave(1) = 1
    // 9. *pos : Teaching Point from file  => Init NULL
    // 10. vel : velocity from Registry => Init { 0.0, 0.0, 0.0, 0.0}
    //--------------------------------------------------------------------
    public class ServoUnitInfo
    {
        //public short    nStartAxis;     // start axis 
        //public short    nMaxAxes;       // max_axes
        //public short    nMaxPositonNo;  // max. teaching point no
        //public short[]  nXyzoToAxis;    // xyz0_to_axis
        //public short Mask;              // mask (x, y axis)
        public MoveType MoveType;       // robot moving type
        public short[] HomeOrder;       // home order
        public string[] PointName;      // teaching pointName
        public RbtPos[] TeachPoint;     // position : NULL
        public ServoSyncInfo SyncInfo;  // Sync Control information (master : 0, slave : 1)
        public double VelRatio;         // velocity ratio
        //public RbtPos CurPos;
        //public AxisStatus[] CurStatus;

        public ServoUnitInfo()
        {
            SyncInfo = new ServoSyncInfo();
        }
    }
    #endregion
}

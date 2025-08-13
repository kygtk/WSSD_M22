using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{    
    public enum RbtAction
    {
        None,
        Estop,
        ServoOn,
        Home,
        Move,
        RMove,
		FindHome,
		// RbtAction define 할때 아래 100 ~ 200 사이에 정의 하지 말것
		MoveRepeat = 100,	
        EndOfActionCode = 200,
    };

    public enum AxisEvent
    {
        NoEvent = 0,
        StopEvent = 1,
        EStopEvent = 2,
        AbortEvent = 3
    }

    [Flags]
    public enum AxisSource
    {
        StNone = 0x0000,
        StHomeSwitch = 0x0001,
        StPosLimit = 0x0002,
        StNegLimit = 0x0004,
        StAmpFault = 0x0008,
        StALimit = 0x0010,
        StVLimit = 0x0020,
        StXNegLimit = 0x0040,
        StXPosLimit = 0x0080,
        StErrorLimit = 0x0100,
        StPcCommand = 0x0200,
        StOutofFrames = 0x0400,
        StAmpPowerOnOff = 0x0800,
        StAbsCommError = 0x1000,
        StInpositonStatus = 0x2000,
        StRunStopCommand = 0x4000,
        StCollisionState = 0x8000,
        StPaustateState = 0x10000,
    };

    [Serializable()]
    public struct RbtPos
    {
        public double[] Pos;
        public RbtPos(int size)
        {
            this.Pos = new double[size];
        }

        public RbtPos Clone()
        {
            int count = this.Pos.Length;
            RbtPos rbtPos = new RbtPos(count);
            for (int i = 0; i < count; i++)
            {
                rbtPos.Pos[i] = this.Pos[i];
            }

            return rbtPos;
        }
    }

    [Serializable()]
    public struct RbtVel
    {
        public double[] Vel;
        public RbtVel(int size)
        {
            this.Vel = new double[size];
        }

        public RbtVel Clone()
        {
            int count = this.Vel.Length;
            RbtVel rbtVel = new RbtVel(count);
            for (int i = 0; i < count; i++)
            {
                rbtVel.Vel[i] = this.Vel[i];
            }

            return rbtVel;
        }
    }

    [Serializable()]
    public struct LimitSensor
    {
        public Boolean Positive;
        public Boolean Home;
        public Boolean Negative;
    }

    [Serializable()]
    public struct AxisStatus
    {
        public int RbtId;
        public int AxisId;
        public AxisEvent State;
        public AxisSource Source;
        public LimitSensor Limit;
    }
}

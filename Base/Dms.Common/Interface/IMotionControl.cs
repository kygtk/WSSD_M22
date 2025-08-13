using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    public interface IMotionControl
    {
        #region Properties
        bool Initialized
        {
            get;
        } 
        #endregion

        #region Methods
        // return 0 : 정상
        short GetError();
        // return 0 : 정상
        short ServoOn(short ax, bool enable);
        // return 0 : Servo Off / else : ON
        short GetServoOnState(short ax, ref short enable);
        // return 0 : 정상
        short ServoEstop(short ax);
        bool IsCmdDone(short ax);
        bool SetSyncControl(short masterAx, short slaveAx, bool enable);
        bool ClearStatus(short ax);
        bool ClearFrames(short ax);
        AxisEvent GetAxisState(short ax);
        AxisSource GetAxisSource(short ax);
        short SetPosition(short ax, double position);
        short GetNegSwLimitAct(short ax, ref double limit, ref AxisEvent action);
        short SetNegSwLimitAct(short ax, double limit, AxisEvent action);
        short GetPosSwLimitAct(short ax, ref double limit, ref AxisEvent action);
        short SetPosSwLimitAct(short ax, double limit, AxisEvent action);
        short SetHomeAct(short ax, AxisEvent action);
        short SetNegLimitAct(short ax, AxisEvent action);
        short SetPosLimitAct(short ax, AxisEvent action);
        short SetStopRate(short ax, short acc);
        short SetIndexRequired(short ax, bool enable);
        short StartVmove(short ax, double vel, short acc);
        short StopVmove(short ax);
        short StartRmove(short ax, double distance, double vel, short acc);
        short StartSmove(short ax, double posPulse, double velPulse, short acc);
        short StartATmove(short ax, double posPulse, double velPulse, short acc, short dec);
        bool GetPosition(short ax, ref double rpos);
        bool GetHomeSwitch(short ax);
        bool GetInMotion(short ax);
        bool GetNegSwitch(short ax);
        bool GetPosSwitch(short ax);
        short GetEncoderDir(short ax, ref short dir);
        bool GetCommand(short ax, ref double rpos);
        short SetServoEstopRate(short ax, short rate);
        short GetActVelocity(short ax);
        #endregion
    }
}

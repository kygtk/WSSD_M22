//using System;

//namespace Dms.Common
//{
//    public interface IServoMotor
//    {
//        short AxisAcc { get; set; }
//        short AxisDec { get; set; }
//        //double AxisDefaultVel { get; set; }
//        //uint AxisEncoder { get; set; }
//        //short AxisEstopRate { get; set; }
//        short AxisId { get; set; }
//        double AxisRatio { get; set; }
//        //AxisType AxisType { get; set; }
//        double AxisVel { get; set; }
//        //bool ClearAxisErr();
//        //bool ClearFrames();
//        //bool ClearStatus();
//        //void CreateTag(Dms.Common.DeviceTags tagContainer);
//        //void EStop();
//        //Type FamilyType { get; }
//        //short GetEncoderDir();
//        //bool GetHomeSwitch();
//        //short GetControllerError();
//        //bool GetNegSwitch();
//        //short GetNegSwLimitAct(ref double limitPos, ref Dms.Common.AxisEvent action);
//        //bool GetPosition(ref double rpos);
//        //double GetPosition();
//        //bool GetPosSwitch();
//        //short GetPosSwLimitAct(ref double limitPos, ref Dms.Common.AxisEvent action);
//        bool GetServoOnState();
//        //short HomeAcc { get; set; }
//        //bool HomeComp { get; set; }
//        //double HomeDist { get; set; }
//        //HomeType HomeType { get; set; }
//        //double HomeVel { get; set; }
//        //int Homing();
//        //Dms.Common.DmsErrors Initialize();
//        //bool IsCmdDone();
//        double Len2Pulse(double len);
//        //double MaxJogVelocity { get; set; }
//        //double MinJogVelocity { get; set; }
//        //MotorType MotorType { get; set; }
//        double Pulse2Len(double pulse);
//        void ServoOn(bool on);
//        //short SetHomeAct(Dms.Common.AxisEvent action);
//        //short SetIndexRequired(bool enable);
//        //void SetMotionController(Dms.Common.IMotionControl mmc);
//        //short SetNegLimitAct(Dms.Common.AxisEvent action);
//        //short SetNegSwLimitAct(double limitPos, Dms.Common.AxisEvent action);
//        //short SetPosition(double pos);
//        //short SetPosLimitAct(Dms.Common.AxisEvent action);
//        //short SetPosSwLimitAct(double limitPos, Dms.Common.AxisEvent action);
//        //short SetStopRate(short acc);
//        //bool SetSyncControl(short slaveAxisId, bool enable);
//        //short StartATmove(double posPulse, double velPulse, short acc, short dec);
//        //short StartRmove(double distance, double vel, short acc);
//        //short StartSmove(double posPulse, double velPulse, short acc);
//        short StartVelMove(double velPulse);
//        //short StartVmove(double vel, short acc);
//        short StopVelMove();
//        //Dms.Common.DmsErrors Uninitialize();
//        //void UpdateTag();
//        //T Len2Pulse<T>(double len);
//    }
//}

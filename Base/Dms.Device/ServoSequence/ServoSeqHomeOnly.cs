using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqHomeOnly : XSeqFunction
    {
        public SeqHomeOnly()
        { 
        }
        public SeqHomeOnly(ServoMotor_Mc servoMotor)
        {
            Servo = servoMotor;
        }
        private ServoMotor_Mc Servo;

        private Double dLimitPosNeg;
        private AxisEvent nActionNeg;

        private Double dLimitPosPos;
        private AxisEvent nActionPos;

        public override int Do()
        {
            int result = -1;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:

                    Servo.HomeComp = false;

                    // set no event for software limit
                    Servo.GetNegSwLimitAct(ref dLimitPosNeg, ref nActionNeg);
                    Servo.SetNegSwLimitAct(dLimitPosNeg, AxisEvent.NoEvent);
                    Servo.GetPosSwLimitAct(ref dLimitPosPos, ref nActionPos);
                    Servo.SetPosSwLimitAct(dLimitPosPos, AxisEvent.NoEvent);

                    Servo.SetHomeAct(AxisEvent.NoEvent);
                    Servo.SetNegLimitAct(AxisEvent.NoEvent);
                    Servo.SetPosLimitAct(AxisEvent.NoEvent);

                    Servo.ClearStatus();

                    Servo.SetStopRate((short)(Servo.HomeAcc / 2));
                    Servo.SetIndexRequired(false);

                    Servo.SetHomeAct(AxisEvent.StopEvent);
                    Servo.SetNegLimitAct(AxisEvent.StopEvent);

                    Servo.ClearFrames();
                    Servo.StartVmove((double)(-Servo.HomeVel * 4.0), (short)(Servo.HomeAcc / 2));

                    nSeqNo = 10;
                    break;
                case 10:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetHomeAct(AxisEvent.NoEvent);
                        Servo.SetNegLimitAct(AxisEvent.NoEvent);
                        Servo.SetPosLimitAct(AxisEvent.EStopEvent);

                        Servo.ClearStatus();

                        double dDistance = Servo.Len2Pulse(Servo.HomeDist);

                        Servo.ClearFrames();

                        if (Servo.GetNegSwitch())
                        {
                            Servo.SetHomeAct(AxisEvent.StopEvent);
                            Servo.StartVmove(Servo.HomeVel * 4.0, (short)(Servo.HomeAcc / 2));
                            nSeqNo = 20;
                        }
                        else if (Servo.GetHomeSwitch())
                        {
                            Servo.StartRmove(dDistance, Servo.HomeVel * 2.0, (short)(Servo.HomeAcc / 2));
                            nSeqNo = 50;
                        }
                    }
                    break;
                case 20:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetHomeAct(AxisEvent.NoEvent);
                        Servo.ClearStatus();

                        double dDistance = Servo.Len2Pulse(Servo.HomeDist);
                        Servo.ClearFrames();

                        Servo.StartRmove(dDistance * 3.0, Servo.HomeVel * 2.0, (short)(Servo.HomeAcc / 2));
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetNegSwLimitAct(dLimitPosNeg, nActionNeg);
                        Servo.SetPosSwLimitAct(dLimitPosPos, nActionPos);
                        nSeqNo = 0;
                    }
                    break;
                case 50:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetStopRate((short)(Servo.HomeAcc / 2));
                        Servo.SetHomeAct(AxisEvent.StopEvent);
                        Servo.SetNegLimitAct(AxisEvent.EStopEvent);

                        if (Servo.GetHomeSwitch() == false || Servo.Simul.Motion)
                        {
                            Servo.ClearFrames();
                            Servo.StartVmove(-Servo.HomeVel / 3.0, (short)(Servo.HomeAcc / 3));
                            nSeqNo = 60;
                        }
                        else
                        {
                            Servo.SetNegSwLimitAct(dLimitPosNeg, nActionNeg);
                            Servo.SetPosSwLimitAct(dLimitPosPos, nActionPos);

                            result = 100;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 60:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetHomeAct(AxisEvent.NoEvent);
                        Servo.ClearStatus();
                        nSeqNo = 70;
                    }
                    break;
                case 70:
                    if (Servo.IsCmdDone())
                    {
                        Servo.SetNegSwLimitAct(dLimitPosNeg, nActionNeg);
                        Servo.SetPosSwLimitAct(dLimitPosPos, nActionPos);

                        if (Servo.GetAxisState() != AxisEvent.NoEvent)
                        {
                            result = 1;
                        }
                        else
                        {
                            Servo.SetPosition(0.0);

                            Servo.HomeComp = true;
                            result = 0;
                        }
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}

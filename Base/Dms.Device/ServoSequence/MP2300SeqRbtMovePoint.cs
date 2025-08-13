using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMovePointMP2300 : XSeqFunction
    {
        Simul m_simul = null;
        public SeqRbtMovePointMP2300()
        {
        }
        public SeqRbtMovePointMP2300(ServoUnitMp2300 unit)
        {
            Unit = unit;
            m_simul = unit.Simul;
            m_AxisCount = Unit.AxisCount;
        }
        private ServoUnitMp2300 Unit;
        private short RetryCnt = 0;
        private int m_AxisCount = 0;

        public int Do(short posId)
        {
            int nRv = -1;
            int result = -1;
            int nSeqNo = this.m_SeqNo;
            uint nTimeover = 10000;

            switch (nSeqNo)
            {
                case 0:
                    if (!Unit.IsServoOn()) return (int)MP2300Error.errServoOn;
                    else if (!Unit.HomeComp) return (int)MP2300Error.errHomeComp;
                    else if (posId < 0 || posId > (Unit.TeachPoints - 1)) return (int)MP2300Error.errMovePosNo;
                    else if ((nRv = Unit.GetRbtError()) > 0)
                    {//2009.08.31 command 날리기전에 최소한 알람은 봐야 되지 않나??
                        result = nRv;
                    }
                    else if (!Unit.IsTryLink || m_simul.Motion)
                    {
                        ServoMotorMp2300 motor;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];
                            int pos = motor.Len2Pulse<int>(Unit.GetTeachPointPos(posId).Pos[id]);
                            int speed = (int)(motor.Len2Pulse<int>(Unit.GetVel().Vel[id]) * Unit.VelRatio);
                            int acc = motor.AxisAcc;

                            motor.SetControlType(ControlType.CtrlPosition);
                            motor.SetRefPosition(pos);
                            motor.SetRefSpeed(speed);
                            motor.SetRefAcceleration(acc);
                            //motor.SetStartReference(true);
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 100)
                    {
                        ServoMotorMp2300 motor;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];
                            motor.SetActStartReference(true);
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        ServoMotorMp2300 motor;

                        bool IsBusy = true;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];
                            IsBusy &= (true == motor.GetActBusy());
                            IsBusy &= (!motor.IsCmdDone());
                            if (!IsBusy) motor.SetActStartReference(true);
                        }

                        if (true == IsBusy)
                        {
                            for (int i = 0; i < m_AxisCount; i++)
                            {
                                motor = Unit.Axis[i];
                                motor.SetActStartReference(false);
                            }
                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > nTimeover)
                        {
                            for (int i = 0; i < m_AxisCount; i++)
                            {
                                motor = Unit.Axis[i];
                                motor.SetActStartReference(false);
                            }
                            result = (int)MP2300Error.errTimeOver;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 30:
                    if (Unit.IsMoveOk())
                    {
                        ServoMotorMp2300 motor;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];
                            motor.SetActStartReference(false);
                        }
                        nSeqNo = 40;
                    }
                    else if ((nRv = Unit.GetRbtError()) > 0)
                    {
                        ServoMotorMp2300 motor;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];
                            motor.SetActStartReference(false);
                        }
                        RetryCnt = 0;
                        result = nRv;
                        nSeqNo = 0;
                    }
                    break;
                case 40:
                    if (Unit.IsRbtStatusOk())
                    {
                        nSeqNo = 50;
                    }
                    else
                    {
                        RetryCnt = 0;
                        result = (int)MP2300Error.errMoveRbtState;
                        nSeqNo = 0;
                    }
                    break;
                case 50:
                    {
                        bool inpos = true;
                        for (int i = 0; i < m_AxisCount; i++)
                        {
                            double teachPos = Unit.TeachPoint[posId].Pos[i];
                            //double curPos = Unit.CurPos.Pos[i];
                            //double curPos = Unit.CurPos.Pos[i];
                            double curPos = 0.0;
                            Unit.GetCurPosition((short)i, ref curPos);
                            inpos &= (Math.Abs(teachPos - curPos) < (double)InPosition.Margin);
                        }

                        if (true == inpos)
                        {
                            RetryCnt = 0;
                            result = 0;
                            nSeqNo = 0;
                        }
                        else
                        {
                            if (RetryCnt < (int)MaxRetry.Cnt)
                            {
                                RetryCnt++;
                                nSeqNo = 0;
                            }
                            else
                            {
                                RetryCnt = 0;
                                result = (int)MP2300Error.errMoveInPos;
                                nSeqNo = 0;
                            }
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}

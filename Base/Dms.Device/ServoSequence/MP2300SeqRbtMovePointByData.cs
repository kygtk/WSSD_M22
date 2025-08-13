///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.
// Author       : eun
// Description  : MP2300에만 있는 기능.
//                TeachingPoint의 Index를 넘겨주면 MP2300이 가지고 있는 pos, speed, acc정보를 가지고 움직인다.
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMovePointByDataMP2300 : XSeqFunction
    {
        private ServoUnitMp2300 Unit;
        private int m_AxisCount = 0;
        private short RetryCnt = 0;

        public SeqRbtMovePointByDataMP2300()
        {
        }

        public SeqRbtMovePointByDataMP2300(ServoUnitMp2300 unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
        }

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
                    else
                    {
                        ServoMotorMp2300 motor;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            motor = Unit.Axis[id];

                            motor.SetControlType(ControlType.CtrlPosition);
                            motor.SetPoint((int)posId + 1); //MP2300은 POSID가 1부터 시작한다.
                        }
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        ServoMotorMp2300 motor;
                        for (int i = 0; i < m_AxisCount; i++)
                        {
                            motor = Unit.Axis[i];
                            motor.SetActStartPoint(true);
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        ServoMotorMp2300 motor;

                        bool IsBusy = true;
                        for (int i = 0; i < m_AxisCount; i++)
                        {
                            motor = Unit.Axis[i];
                            IsBusy &= (true == motor.GetActBusy());
                            IsBusy &= (!motor.IsCmdDone());
                            if (!IsBusy) motor.SetActStartPoint(true);
                        }

                        if (true == IsBusy)
                        {
                            for (int i = 0; i < m_AxisCount; i++)
                            {
                                motor = Unit.Axis[i];
                                motor.SetActStartPoint(false);
                            }
                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > nTimeover)
                        {
                            for (int i = 0; i < m_AxisCount; i++)
                            {
                                motor = Unit.Axis[i];
                                motor.SetActStartPoint(false);
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
                        for (int i = 0; i < m_AxisCount; i++)
                        {
                            motor = Unit.Axis[i];
                            motor.SetActStartPoint(false);
                        }
                        nSeqNo = 40;
                    }
                    else if ((nRv = Unit.GetRbtError()) > 0)
                    {
                        ServoMotorMp2300 motor;
                        for (int i = 0; i < m_AxisCount; i++)
                        {
                            motor = Unit.Axis[i];
                            motor.SetActStartPoint(false);
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

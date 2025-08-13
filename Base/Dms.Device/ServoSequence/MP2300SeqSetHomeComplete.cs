using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqSetHomeCompleteMP2300 : XSeqFunction
    {
        private ServoUnitMp2300 m_Unit;
        private int m_AxisCount;
        private int m_Count = 0;

        public SeqSetHomeCompleteMP2300()
        {
        }

        public SeqSetHomeCompleteMP2300(ServoUnitMp2300 unit)
        {
            m_Unit = unit;
            m_AxisCount = m_Unit.AxisCount;
        }

        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            int result = -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_Unit.RbtReset())
                        {
                            nSeqNo = 20;
                        }
                        else if (GetElapsedTicks() > 10000)
                        {
                            result = (int)MP2300Error.errTimeOver;
                            nSeqNo = 0;
                            break;
                        }
                    }
                    break;
                case 20:
                    {
                        if (m_Count == m_AxisCount)
                        {
                            m_Unit.HomeComp = true;
                            m_Count = 0;
                            result = 0;
                            nSeqNo = 0;
                        }
                        else
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        ServoMotorMp2300 motor = m_Unit.Axis[m_Count];
                        int rv = motor.SeqSetHomeComp.Do();
                        if (rv == 0)
                        {
                            m_Count++;
                            nSeqNo = 20;
                        }
                        else if (0 < rv)
                        {
                            m_Count = 0;
                            nSeqNo = 0;
                            result = rv;
                            break;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}

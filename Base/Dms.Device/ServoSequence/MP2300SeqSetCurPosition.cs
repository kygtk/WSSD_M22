using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqSetCurPositionMP2300 : XSeqFunction
    {
        private ServoUnitMp2300 m_Unit;
        private int m_AxisCount;
        private int m_Count = 0;

        public SeqSetCurPositionMP2300()
        { 
        }

        public SeqSetCurPositionMP2300(ServoUnitMp2300 unit)
        {
            m_Unit = unit;
            m_AxisCount = m_Unit.AxisCount;
        }

        public int Do(short pointId)
        {
            int nSeqNo = this.m_SeqNo;
            int result = -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Count == m_AxisCount)
                        {
                            m_Unit.Ready = false;
                            m_Count = 0;
                            result = 0;
                        }
                        else
                        {
                            nSeqNo = 10;
                        }
                    }
                    break;
               case 10:
                    {
                        ServoMotorMp2300 motor = m_Unit.Axis[m_Count];
                        double pos = m_Unit.GetTeachPointPos(pointId, (short)m_Count);
                        int rv = motor.SeqSetCurPos.Do(pos);
                        if (rv == 0)
                        {
                            m_Count++;
                            nSeqNo = 0;
                        }
                        else if( 0 < rv)
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

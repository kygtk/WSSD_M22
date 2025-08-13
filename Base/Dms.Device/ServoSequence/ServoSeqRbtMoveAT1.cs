using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMoveAT1 : XSeqFunction
    {
        private int m_AxisCount;
        private Simul m_Simul;
        private ServoUnit m_Unit;

        public SeqRbtMoveAT1()
        {
        }

        public SeqRbtMoveAT1(ServoUnit unit)
        {
            m_Unit = unit;
            m_AxisCount = m_Unit.AxisCount;
            m_Simul = m_Unit.Simul;
        }

        public int Do(RbtPos rbtPos, RbtVel rbtVel)
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        ServoMotor axis;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            axis = m_Unit.Axis[id];

                            if (m_Simul.Motion != true)
                            {
                                if (m_Unit.Sync && (axis.AxisId == m_Unit.SyncInfo.Slave.AxisId)) continue;
                            }

                            double posPulse = axis.Len2Pulse(rbtPos.Pos[id]);
                            double velPulse = axis.Len2Pulse(rbtVel.Vel[id]) * m_Unit.VelRatio;
                            short acc = axis.AxisAcc;
                            short dec = axis.AxisDec;
                            m_Unit.Axis[id].StartATmove(posPulse, velPulse, acc, dec);
                        }

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (true == m_Unit.IsMoveOk())
                    {
                        result = m_Unit.Axis[0].GetControllerError();
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return result;
        }
    }
}

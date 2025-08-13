using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMoveS1 : XSeqFunction
    {
        private int m_AxisCount;
        private Simul m_Simul;
        private ServoUnit m_Unit;

        public SeqRbtMoveS1()
        {
        }
        public SeqRbtMoveS1(ServoUnit unit)
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
                            double acc = axis.Len2Pulse(axis.AxisAcc) * m_Unit.VelRatio;
                            axis.StartSmove(posPulse, velPulse, acc);
                        }

                        nSeqNo = 10;

                    }
                    break;
                case 10:
                    if (true == m_Unit.IsMoveOk())
                    {
                        //if (m_Simul.Motion == true)
                        //{
                        //    if (m_Unit.Sync)
                        //    {
                        //        ServoMotor slave = m_Unit.SyncInfo.Slave;
                        //        double pulse = slave.Len2Pulse(m_Unit.SyncInfo.Master.GetPosition());
                        //        slave.SetPosition(pulse);
                        //    }
                        //}

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

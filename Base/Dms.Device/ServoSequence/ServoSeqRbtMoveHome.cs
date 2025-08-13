using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMoveHome : XSeqFunction
    {
        private const int SEQ_AxisStatusCheck = 0;
        private const int SEQ_AxisHomeCheck = 10;
        private const int SEQ_AxisHoming = 20;

        private int m_AxisCount;
        public SeqRbtMoveHome()
        {
        }
        public SeqRbtMoveHome(ServoUnit unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
        }
        private ServoUnit Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case SEQ_AxisStatusCheck:
                    if (!Unit.IsRbtSensorOk())
                    {
                        return 1000;
                    }
                    else
                    {
                        Unit.HomeComp = false;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].HomeComp = false;
                        }
                        nSeqNo = SEQ_AxisHomeCheck;
                    }
                    break;
                case SEQ_AxisHomeCheck:
                    if (GetHomeAxes() > 0)
                    {
                        nSeqNo = SEQ_AxisHoming;
                    }
                    else
                    {
                        Unit.HomeComp = true;
                        result = 0;
                        nSeqNo = SEQ_AxisStatusCheck;
                    }
                    break;
                case SEQ_AxisHoming:
                    if (0 == GetSameTimeHomeAxes()) nSeqNo = SEQ_AxisHomeCheck;
                    else
                    {
                        for (int countId = 0; countId < GetSameTimeHomeAxes(); countId++)
                        {
                            int index = FindOrderedAxisIndex(countId);
                            if (index >= 0)
                            {
                                int rv = Unit.Axis[index].Homing();
                                if (0 == rv)
                                {
                                    if (Unit.Sync && (Unit.Axis[index].AxisId == Unit.SyncInfo.Master.AxisId))
                                    {
                                        Unit.SyncInfo.Slave.SetPosition(0.0);
                                    }

                                    nSeqNo = SEQ_AxisHomeCheck;
                                }
                                else if (0 < rv)
                                {
                                    result = rv;
                                    nSeqNo = SEQ_AxisStatusCheck;
                                    break;
                                }
                            }
                        }
                    }
                    break;

            }

            this.m_SeqNo = nSeqNo;

            return result;
        }

        public int GetHomeAxes()
        {
            int axes = m_AxisCount;
            for (int id = 0; id < m_AxisCount; id++)
            {
                if (Unit.Axis[id].HomeComp)
                {
                    axes--;
                }
            }

            if (true == Unit.Sync)
            {
                axes--;
            }

            return axes;
        }

        public int GetMinHomeOrder()
        {
            int minOrder = m_AxisCount;
            ServoMotor axis;
            for (int id = 0; id < m_AxisCount; id++)
            {
                axis = Unit.Axis[id];
                if (axis.HomeComp) continue;
                else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
                else if (minOrder > Unit.HomeOrder[id])
                {
                    minOrder = Unit.HomeOrder[id];
                }
            }

            if (minOrder == m_AxisCount) minOrder = -1;

            return minOrder;
        }

        public int GetSameTimeHomeAxes()
        {
            int axes = 0;
            int order = GetMinHomeOrder();
            ServoMotor axis;
            for (int id = 0; id < m_AxisCount; id++)
            {
                axis = Unit.Axis[id];
                if (axis.HomeComp) continue;
                else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
                else if (order == Unit.HomeOrder[id])
                {
                    axes++;
                }
            }
            return axes;
        }

        public int FindOrderedAxisIndex(int countId)
        {
            int axes = -1;
            int result = -1;
            int order = GetMinHomeOrder();
            ServoMotor axis;
            for (int index = 0; index < m_AxisCount; index++)
            {
                axis = Unit.Axis[index];
                if (axis.HomeComp) continue;
                else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
                else
                {
                    if (order == Unit.HomeOrder[index])
                    {
                        if (++axes == countId)
                        {
                            result = index;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}

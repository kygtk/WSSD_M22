using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Device
{
    public class SeqRbtMoveHomeMP2300 : XSeqFunction
    {
        private int m_AxisCount;
        public SeqRbtMoveHomeMP2300()
        {
        }
        public SeqRbtMoveHomeMP2300(ServoUnitMp2300 unit)
        {
            Unit = unit;
            m_AxisCount = Unit.AxisCount;
        }
        private ServoUnitMp2300 Unit;

        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (!Unit.IsServoOn()) return (int)MP2300Error.errServoOn;
                    else if (!Unit.IsRbtSensorOk()) return (int)MP2300Error.errMoveSensor;
                    else
                    {
                        Unit.HomeComp = false;
                        for (int id = 0; id < m_AxisCount; id++)
                        {
                            Unit.Axis[id].HomeComp = false;
                            Unit.Axis[id].InitSeq();
                        }
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetHomeAxes() > 0)
                    {
                        nSeqNo = 20;
                    }
                    else
                    {
                        Unit.HomeComp = true;
                        result = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    if (0 == GetSameTimeHomeAxes()) nSeqNo = 10;
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
                                    //if (Unit.Sync && (Unit.Axis[index].AxisId == Unit.SyncInfo.Master.AxisId))
                                    //{
                                    //    Unit.SyncInfo.Slave.SetPosition(0.0);
                                    //}

                                    nSeqNo = 10;
                                }
                                else if (0 < rv)
                                {
                                    result = rv;
                                    nSeqNo = 0;
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
            ServoMotorMp2300 axis;
            for (int id = 0; id < m_AxisCount; id++)
            {
                axis = Unit.Axis[id];
                if (axis.HomeComp) continue;
                //else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
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
            ServoMotorMp2300 axis;
            for (int id = 0; id < m_AxisCount; id++)
            {
                axis = Unit.Axis[id];
                if (axis.HomeComp) continue;
                //else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
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
            ServoMotorMp2300 axis;
            for (int index = 0; index < m_AxisCount; index++)
            {
                axis = Unit.Axis[index];
                if (axis.HomeComp) continue;
                //else if (Unit.Sync && (axis.AxisId == Unit.SyncInfo.Slave.AxisId)) continue;
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

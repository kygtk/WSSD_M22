using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    public class SeqUpdateStatus : XSeqFunction
    {
        //private int m_AxisCount;

        //public SeqUpdateStatus()
        //{ 
        //}
        //public SeqUpdateStatus(ServoUnit unit)
        //{
        //    Unit = unit;
        //    m_AxisCount = Unit.AxisCount;

        //    oldStatus = new AxisStatus[m_AxisCount];
        //    curStatus = new AxisStatus[m_AxisCount];

        //}
        //private ServoUnit Unit;

        //private AxisStatus[] oldStatus;
        //private AxisStatus[] curStatus;

        //public override int Do()
        //{
        //    int result = -1;

        //    bool[] changed = new bool[m_AxisCount];

        //    curStatus = Unit.GetRbtStatus();

        //    for (short id = 0; id < m_AxisCount; id++)
        //    {
        //        if (oldStatus[id].Limit.Negative != curStatus[id].Limit.Negative)
        //        {
        //            oldStatus[id].Limit.Negative = curStatus[id].Limit.Negative;
        //            changed[id] |= true;
        //        }
        //        if (oldStatus[id].Limit.Home != curStatus[id].Limit.Home)
        //        {
        //            oldStatus[id].Limit.Home = curStatus[id].Limit.Home;
        //            changed[id] |= true;
        //        }
        //        if (oldStatus[id].Limit.Positive != curStatus[id].Limit.Positive)
        //        {
        //            oldStatus[id].Limit.Positive = curStatus[id].Limit.Positive;
        //            changed[id] |= true;
        //        }
        //        if (oldStatus[id].Source != curStatus[id].Source)
        //        {
        //            oldStatus[id].Source = curStatus[id].Source;
        //            changed[id] |= true;
        //        }
        //        if (oldStatus[id].State != curStatus[id].State)
        //        {
        //            oldStatus[id].State = curStatus[id].State;
        //            changed[id] |= true;
        //        }
        //    }

        //    for (short id = 0; id < m_AxisCount; id++)
        //    {
        //        if (true == changed[id])
        //        {
        //            curStatus[id].RbtId = Unit.Id;
        //            curStatus[id].AxisId = id;
        //            Unit.FireEventUpdateStatus(curStatus[id]);
        //        }
        //    }

        //    result = 0;
        //    return result;
        //}
    }        
}

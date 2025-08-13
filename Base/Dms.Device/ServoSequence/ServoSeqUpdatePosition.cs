using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Device
{
    public class SeqUpdatePosition : XSeqFunction
    {
        //private int m_AxisCount;

        //public SeqUpdatePosition()
        //{ 
        //}
        //public SeqUpdatePosition(ServoUnit unit)
        //{
        //    Unit = unit;
        //    m_AxisCount = Unit.AxisCount;

        //    this.oldPos = new RbtPos(m_AxisCount);
        //    this.curPos = new RbtPos(m_AxisCount);

        //    Unit.CurPos = curPos;
        //}
        //private ServoUnit Unit;

        //private RbtPos oldPos;
        //private RbtPos curPos;

        //public override int Do()
        //{
        //    int result = -1;
        //    if (!Unit.GetCurPosition(ref curPos))
        //    {
        //        result = 1;
        //        return result;
        //    }
        //    else
        //    {
        //        Boolean changed = false;
        //        for (int id = 0; id < m_AxisCount; id++)
        //        {
        //            if (curPos.Pos[id] < 0 && curPos.Pos[id] > -0.01)
        //            {
        //                curPos.Pos[id] = 0.0;
        //            }

        //            if ( 0.005 < (Math.Abs(oldPos.Pos[id] - curPos.Pos[id])))
        //            {
        //                changed = true;
        //            }
        //        }

        //        if (true == changed)
        //        {
        //            oldPos = curPos.Clone();
        //            Unit.FireEventUpdatePosition(Unit.CurPos);
        //        }

        //        result = 0;
        //        return result;
        //    }
        //}
    }
}

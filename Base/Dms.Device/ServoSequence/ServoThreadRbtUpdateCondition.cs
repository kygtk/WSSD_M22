using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;

namespace Dms.Device
{
    public class ThreadRbtUpdateCondition : XSequence
    {
        //private IServerManager m_Server;
        //private ServoUnit m_Unit;

        //public ThreadRbtUpdateCondition(int scanTime, ServoUnit unit)
        //    : base(scanTime)
        //{
        //    m_Server = unit.ServerManager;
        //    m_Unit = unit;

        //    RegisterSequence(new SeqUpdatePosition(m_Unit));
        //    RegisterSequence(new SeqUpdateStatus(m_Unit));
        //}

        //public override void Sequence()
        //{
        //    try
        //    {
        //        Thread.Sleep(m_ScanTime);

        //        if (m_Server.State != ActiveState.Run) return;

        //        foreach (XSeqFunction seq in SeqFunctions)
        //        {
        //            seq.Do();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        XFunc.ExceptionHandler.Add(err);
        //    }
        //}
    } 
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;

namespace Dms.Device
{
    class ThreadShutterManualAct : XSequence
    {
        private ShutterUnit m_Unit;
        private IServerManager m_Server;
        private SeqShutterManualAct m_SeqShutterManualAct;

        public ThreadShutterManualAct(int scanTime, ShutterUnit unit)
            : base (scanTime)
        {
            m_Server = unit.ServerManager;
            m_Unit = unit;

            m_SeqShutterManualAct = new SeqShutterManualAct(m_Unit);
            
            RegisterSequence(m_SeqShutterManualAct);
        }

        public int ManualActionCmd
        {
            get { return m_SeqShutterManualAct.ManualCmd; }
            set { m_SeqShutterManualAct.ManualCmd = value; }
        }

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;

namespace Dms.Device
{
    public class ThreadRbtManualAct : XSequence
    {
        private ServoUnit m_Unit;
        private IServerManager m_Server;
        private SeqRbtManualAct m_SeqRbtManualAct;
        private SeqRbtEmo m_SeqRbtEmo;

        public ThreadRbtManualAct(int scanTime, ServoUnit unit)
            : base (scanTime)
        {
            m_Server = unit.ServerManager;
            m_Unit = unit;

            m_SeqRbtManualAct = new SeqRbtManualAct(m_Unit);
            m_SeqRbtEmo = new SeqRbtEmo(m_Unit);

            RegisterSequence(m_SeqRbtManualAct);
            RegisterSequence(m_SeqRbtEmo);
        }

        public int ManualActionCmd
        {
            get { return m_SeqRbtManualAct.ManualCmd; }
            set { m_SeqRbtManualAct.ManualCmd = value; }
        }

        public short SelectedPointId
        {
            get { return m_SeqRbtManualAct.SelectedPointId; }
            set { m_SeqRbtManualAct.SelectedPointId = value; }
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

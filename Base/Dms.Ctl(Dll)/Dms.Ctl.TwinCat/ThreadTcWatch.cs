using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.Ctl
{
    public class ThreadTcWatch : XSequence
    {
        private EtherCAT m_EtherCat;
        private SeqTcMonitorState m_SeqMonitorState;

        public ThreadTcWatch(EtherCAT etherCat)
        {
            m_EtherCat = etherCat;
            m_SeqMonitorState = new SeqTcMonitorState(m_EtherCat);
        }

        public override void Sequence()
        {
            try
            {
                if (!m_EtherCat.Initialized)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    m_SeqMonitorState.Do();
                    Thread.Sleep(2);

                    if (m_EtherCat.DeviceState == ActiveState.Run)
                    {
                        m_EtherCat.WriteDo();
                        m_EtherCat.WriteAo();
                        Thread.Sleep(2);

                        if (m_EtherCat.UpdateMode == IoUpdateMode.Polling)
                        {
                            m_EtherCat.ReadDi();
                            m_EtherCat.ReadDo();
                            m_EtherCat.ReadAi();
                            m_EtherCat.ReadAo();
                            Thread.Sleep(2);
                        }
                    }
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                //XFunc.ExceptionHandler.Add(err);
                m_EtherCat.WriteLog(err.ToString());
            }
        }
    }


    public class SeqTcMonitorState : XSeqFunction
    {
        private EtherCAT m_EtherCat;

        public SeqTcMonitorState(EtherCAT etherCat)
        {
            m_EtherCat = etherCat;
        }

        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {
                        m_EtherCat.MonitorState();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (this.GetElapsedTicks() > 2000)
                    {
                        m_EtherCat.MonitorState();
                        m_StartTicks = XFunc.GetTickCount();
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
    }
}

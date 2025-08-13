using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;

namespace Dms.Ctl
{
    public class ThreadTcPlcWatch : XSequence
    {
        private TwinCATPlc m_TwinCATPlc;
        private SeqTcPlcMonitorState m_SeqMonitorState;

        public ThreadTcPlcWatch(TwinCATPlc twinCATPlc)
        {
            m_TwinCATPlc = twinCATPlc;
            m_SeqMonitorState = new SeqTcPlcMonitorState(m_TwinCATPlc);
        }

        public override void Sequence()
        {
            try
            {
                if (!m_TwinCATPlc.Initialized)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    m_SeqMonitorState.Do();
                    Thread.Sleep(2);

                    if (m_TwinCATPlc.DeviceState == ActiveState.Run)
                    {
                        //m_TwinCATPlc.WriteOutput();

                        Thread.Sleep(2);

                        if (m_TwinCATPlc.UpdateMode == IoUpdateMode.Polling)
                        {
                            m_TwinCATPlc.ReadInput();
                            m_TwinCATPlc.ReadOutput();

                            m_TwinCATPlc.ReadDi();
                            m_TwinCATPlc.ReadDo();
                            m_TwinCATPlc.ReadAi();
                            m_TwinCATPlc.ReadAo();

                            Thread.Sleep(2);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                m_TwinCATPlc.WriteLog(err.ToString());
            }
        }
    }

    public class SeqTcPlcMonitorState : XSeqFunction
    {
        private TwinCATPlc m_TwincatPlc;

        public SeqTcPlcMonitorState(TwinCATPlc twincatPlc)
        {
            m_TwincatPlc = twincatPlc;
        }

        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {
                        m_TwincatPlc.MonitorState();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (this.GetElapsedTicks() > 2000)
                    {
                        m_TwincatPlc.MonitorState();
                        m_StartTicks = XFunc.GetTickCount();
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
    }
}

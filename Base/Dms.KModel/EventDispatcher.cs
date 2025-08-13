using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;

namespace Dms.KModel
{
    public class EventDispatcher
    {
        #region Fields
        private Thread m_Thread = null;
        private int m_Timespan = 10;
        private UInt32 m_PauseTime;
        private UInt32 m_ResumeTime;
        //private DateTime m_CurTime;
        private bool m_Stop = true;
        private EventQueue m_EventQueue = EventQueue.Instance;
        #endregion

        #region Constructor
        public EventDispatcher()
        {
            m_Thread = new Thread(new ThreadStart(this.Run));
            m_Thread.IsBackground = true;
            m_Thread.Start();
        }
        #endregion

        #region Methods
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(m_Timespan);
                if (m_Stop) continue;

                if (!m_Stop && !m_EventQueue.Empty())
                {
                    ModelEvent ev = new ModelEvent(null, "", null, 0, false);
                    ev = m_EventQueue.Peek();
                    if (ev.GetTime() > XFunc.GetTickCount())
                        break;

                    if (ev.To != null) m_EventQueue.Pop().To.ProcessEvent(ev);
                }
            }
        }

        public void SetStop(bool flag)
        {
            if (flag == true)
                m_PauseTime = XFunc.GetTickCount();
            else
            {
                m_ResumeTime = XFunc.GetTickCount();
            }
        }
        #endregion
    }
}

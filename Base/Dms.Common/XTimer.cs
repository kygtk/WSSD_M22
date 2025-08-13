using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices;

namespace Dms.Common
{
    public class XTimer
    {
        private struct TagTimer
        {
            public bool Over;
            public bool Start;
            public bool Pause;

            //public long TargetTicks;
            //public double ElapsedTicks;
            //public DateTime StartedTime;

            public uint StartedTickCounts;
            public uint ElapsedTickCounts;
            public uint TargetTickCounts;
        }

        private static object m_LockKey = new object();
        private const int _TimerResolution = 20;
        private int m_Id;
        private string m_Name;
        private TagTimer m_Flag;
        private static int m_Count;
        private static System.Timers.Timer m_SystemTimer = null;

        private static List<XTimer> m_List = new List<XTimer>();

        public bool Over
        {
            get
            {
                if (true != m_Flag.Over) return false;
                else
                {
                    m_Flag.Over = false;
                    return true;
                }
            }
        }

        public bool IsPaused
        {
            get { return m_Flag.Pause; }
        }

        public uint CurElapsedTickCounts
        {
            get { return m_Flag.ElapsedTickCounts; }
        }

        public XTimer(string name)
        {
            m_Name = name;
            m_Id = m_Count++;

            if (null == m_SystemTimer)
            {
                m_SystemTimer = new System.Timers.Timer();
                m_SystemTimer.Interval = _TimerResolution;
                m_SystemTimer.Elapsed += new ElapsedEventHandler(this.TimerTick);
            }

            m_List.Add(this);
        }

        public XTimer(string name, int milliseconds)
            : this(name)
        {
            m_Flag.TargetTickCounts = (uint)(milliseconds - (uint)(_TimerResolution * 0.5));
        }

        ~XTimer()
        {
            if (null != m_SystemTimer)
            {
                m_SystemTimer.Enabled = false;
            }
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            lock (m_LockKey)
            {
                //DateTime time = DateTime.Now;

                uint TickCount = XFunc.GetTickCount();

                foreach (XTimer timer in m_List)
                {
                    if (timer.m_Flag.Start)
                    {
                        if (!timer.m_Flag.Pause)
                        {
                            //TimeSpan diff = time - timer.m_Flag.StartedTime;
                            //timer.m_Flag.ElapsedTicks = diff.TotalMilliseconds;
                            timer.m_Flag.ElapsedTickCounts = TickCount - timer.m_Flag.StartedTickCounts;
                        }

                        //if (timer.m_Flag.ElapsedTicks >= timer.m_Flag.TargetTicks)
                        //{
                        //    timer.m_Flag.Start = false;
                        //    timer.m_Flag.Over = true;
                        //}

                        if (timer.m_Flag.ElapsedTickCounts >= timer.m_Flag.TargetTickCounts)
                        {
                            timer.m_Flag.Start = false;
                            timer.m_Flag.Over = true;
                        }
                    }
                }
            }
        }

        public void Start(int milliseconds)
        {
            lock (m_LockKey)
            {
                m_Flag.Over = false;

                m_Flag.ElapsedTickCounts = 0;
                int tagetTicks = (int)(milliseconds - (int)(_TimerResolution * 0.5));
                tagetTicks = tagetTicks < 0 ? 0 : tagetTicks;
                m_Flag.TargetTickCounts = (uint)tagetTicks;
                m_Flag.StartedTickCounts = XFunc.GetTickCount();

                m_Flag.Pause = false;
                m_Flag.Start = true;
            }

            if (!m_SystemTimer.Enabled)
            {
                m_SystemTimer.Start();
            }
        }

        public void Start()
        {
            lock (m_LockKey)
            {
                m_Flag.Over = false;
                m_Flag.ElapsedTickCounts = 0;
                m_Flag.StartedTickCounts = XFunc.GetTickCount();

                m_Flag.Pause = false;
                m_Flag.Start = true;
            }

            if (!m_SystemTimer.Enabled)
            {
                m_SystemTimer.Start();
            }
        }

        public void Pause()
        {
            m_Flag.Pause = true;
        }

        public void Resume()
        {
            //m_Flag.StartedTime = DateTime.Now.AddMilliseconds(-m_Flag.ElapsedTicks);
            m_Flag.StartedTickCounts = XFunc.GetTickCount() - m_Flag.ElapsedTickCounts;
            m_Flag.Pause = false;
        }
    }
}

using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Dms.Common
{
    public class XSequence //: Object
    {
        #region Feilds
        protected Thread m_SeqThread = null;
        protected List<XSeqFunction> m_SeqFunctions = new List<XSeqFunction>();
        protected int m_ScanTime = 300;
        #endregion

        #region Properties
        public int SeqFunctionCount { get { return m_SeqFunctions.Count; } }
        public int ScanTime { get { return m_ScanTime; } set { m_ScanTime = value; } }
        #endregion

        #region Constructor
        public XSequence()
        {
            m_SeqThread = new Thread(new ThreadStart(ThreadProc)) { IsBackground = true };
        }

        public XSequence(int ScanTime) : this()
        {
            m_ScanTime = ScanTime;
        }
        #endregion

        #region Destructor
        ~XSequence() { }
        #endregion

        public void RegisterSequence(XSeqFunction seq)
        {
            m_SeqFunctions.Add(seq);
        }

        protected virtual void RegisterSequences()
        {
            throw new NotImplementedException();
        }

        public void ThreadProc()
        {
            while (true) { Sequence(); }
        }

        public virtual void Sequence()
        {
            Thread.Sleep(m_ScanTime);
        }

        public bool IsAlive { get { return m_SeqThread.IsAlive; } }
        public bool IsStarted { get { return (m_SeqThread.ThreadState & ThreadState.Unstarted) == 0; } }
        public bool IsRunnig { get { return m_SeqThread.ThreadState == ThreadState.Running; } }
        public bool IsSuspended { get { return (m_SeqThread.ThreadState & ThreadState.Suspended) != 0; } }

        public void Start()
        {
            if (!IsStarted)
            {
                m_SeqThread.Start();
            }
            else
            {
                Resume();
            }
        }

        public void Abort()
        {
            Resume();

            if (IsAlive) m_SeqThread.Abort();
        }

        public void Pause()
        {
            if (IsStarted && !IsSuspended)
            {
                m_SeqThread.Suspend();
            }
        }

        public void Resume()
        {
            if (IsStarted && IsSuspended)
            {
                m_SeqThread.Resume();
            }
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}

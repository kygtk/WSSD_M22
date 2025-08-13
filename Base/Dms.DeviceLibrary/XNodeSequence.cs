///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 
// Author       : Koosangseo
// Description  : XNodeSequence Class
//-------------------------------------------------------------------------
// Revison History
// * 2009.10.22 : jemoon - 상호참조 문제 해결을 위해 위치 이동
//                Dms.Common -> Dms.DeviceLibrary
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace Dms.DeviceLibrary
{
    [Serializable]
    public class XNodeSequence : DmsNode
    {
        private Thread thread = null;
        private List<XNodeSeqFunction> m_SeqFunctions = new List<XNodeSeqFunction>();
        private int m_OrderFuncValue;
        protected int m_ScanTime = 300;

        [XmlIgnore]
        public List<XNodeSeqFunction> SeqFunctions
        {
            get { return m_SeqFunctions; }
        }

        [Browsable(false)]
        public int ScanTime
        {
            get { return m_ScanTime; }
            set { m_ScanTime = value; }
        }

        [Browsable(false), XmlIgnore]
        public Thread ThreadInfo
        {
            get { return thread; }
            set { thread = value; }
        }

        public XNodeSequence()
        {
            this.thread = new Thread(new ThreadStart(this.threadProc));
            this.thread.IsBackground = true;
        }

        public XNodeSequence(int scanTime)
            : this()
        {
            m_ScanTime = scanTime;
        }

        ~XNodeSequence()
        {
        }

        public void SetOrderValue(int order)
        {
            m_Order = order;
        }

        public void SetOrderFuncValue(int order)
        {
            m_OrderFuncValue = order;
        }

        public void RegisterSequence(XNodeSeqFunction seq)
        {
            SeqFunctions.Add(seq);
        }

        protected virtual void RegisterSequences()
        { 
        
        }

        public void RegisterSeqInitialize()
        {
            foreach (XNodeSeqFunction seq in m_SeqFunctions)
            {
                seq.AddNode(this, m_OrderFuncValue++, seq.SeqFunName).Initialize();
            }
        }

        public void threadProc()
        {
            while (true)
            {
                Thread.Sleep(10);

                if (m_IsConfigMode) return;

                Sequence();
            }
        }

        public virtual void Sequence()
        {
            Thread.Sleep(m_ScanTime);
        }

        public Boolean IsAlive()
        {
            return this.thread.IsAlive;
        }

        public Boolean IsStarted()
        {
            int i = (int)(this.thread.ThreadState & ThreadState.Unstarted);

            return (i == 0) ? true : false;
        }

        public Boolean IsRunnig()
        {
            return ( this.thread.ThreadState == ThreadState.Running );
        }

        public Boolean IsSuspended()
        {
            int i = (int)(this.thread.ThreadState & ThreadState.Suspended);

            return (i != 0) ? true : false;
        }
        
        public void Start()
        {
            if (false == IsStarted())
            {
                this.thread.Start();
            }
            else
            {
                Resume();
            }
        }

        public void Abort()
        {
            Resume();

            if (IsAlive()) this.thread.Abort();
        }

        public void Pause()
        {
            if (true == IsStarted() && 
                false == IsSuspended())
            {
                this.thread.Suspend();
            }
        }

        public void Resume()
        {
            if (true == IsStarted() &&
                true == IsSuspended())
            {
                this.thread.Resume();
            }
        }

        protected override void ReadConfiguration(DmsSerializingService dss, string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteConfiguration()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

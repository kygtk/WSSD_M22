using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.Sequence
{
    public class ThreadProcessTime : XSequence
    {
        #region Fields
        private static IServerManager m_Server;
        private static _GenericCollection<ProcessTime> m_Times;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (ProcessTime time in m_Times)
            {
                RegisterSequence(new SeqProcessTime(this, time));
            }
        } 
        #endregion

        #region Constructor
        public ThreadProcessTime(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Times = m_Server.ComponentContainer.GetCollection<ProcessTime>();
        } 
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (m_Server.IoController.DeviceState != ActiveState.Run) return;

                foreach (XSeqFunction seq in SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        } 
        #endregion

        #region Static Methods
        public static _GenericCollection<ProcessTime> Units
        {
            get
            {
                if (m_Times == null) m_Times = new _GenericCollection<ProcessTime>();
                return m_Times;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqProcessTime : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadProcessTime m_Control;
        protected ProcessTime m_Time;
        protected System.Threading.Timer timerPreProcessTime;
        protected System.Threading.Timer timerNextProcessTime;

        bool m_CheckPreProcessTime = false;
        bool m_CheckNextProcessTime = false;

        int m_PreProcessTime = 0;
        int m_NextProcessTime = 0; 
        #endregion

        #region Constructor
        public SeqProcessTime(ThreadProcessTime control, ProcessTime time)
        {
            m_Time = time;
            m_Server = m_Time.ServerManager;
            m_Control = control;

            SeqFunName = m_Time.Name;
        } 
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_Server.GenInfos.EqpInitComp) return -1;

            m_Time.Sequence = this;

            if (timerPreProcessTime == null)
            {
                timerPreProcessTime = new System.Threading.Timer(new TimerCallback(CheckPreProcessTime), null, 0, 1000);
            }
            if (timerNextProcessTime == null)
            {
                timerNextProcessTime = new System.Threading.Timer(new TimerCallback(CheckNextProcessTime), null, 0, 1000);
            }

            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Time.GlsInSensor.IsDetected())
                    {
                        m_CheckPreProcessTime = true;
                        m_PreProcessTime = 0;

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_Time.GlsInSensor.IsDetected())
                    {
                        nSeqNo = 20;
                    }
                    break;

                case 20:
                    if (m_Time.GlsOutSensor.IsDetected())
                    {
                        nSeqNo = 30;
                    }
                    else if (m_Time.GlsInSensor.IsDetected())
                    {
                        m_CheckNextProcessTime = true;
                        nSeqNo = 30;
                    }
                    break;

                case 30:
                    if (!m_Time.GlsOutSensor.IsDetected())
                    {
                        if (m_CheckNextProcessTime)
                        {
                            m_PreProcessTime = m_NextProcessTime;
                            m_NextProcessTime = 0;
                            m_CheckNextProcessTime = false;

                            nSeqNo = 20;
                        }
                        else
                        {
                            m_CheckPreProcessTime = false;
                            m_PreProcessTime = 0;

                            nSeqNo = 0;
                        }
                    }
                    else if (m_Time.GlsInSensor.IsDetected())
                    {
                        m_CheckNextProcessTime = true;
                        nSeqNo = 30;
                    }
                    break;
            }

            this.SeqNo = nSeqNo;
            return -1;
        } 
        #endregion

        #region General Methods
        public void CheckPreProcessTime(Object stateInfo)
        {
            if (m_Server.GenInfos.AutoMode &&
                !m_Server.GenInfos.Pause &&
                !m_Server.SeqFlag.LdTimeOut &&
                m_CheckPreProcessTime)
            {
                m_PreProcessTime++;
                m_Time.UpdateTag(m_PreProcessTime.ToString());
            }
        }

        public void CheckNextProcessTime(Object stateInfo)
        {
            if (m_Server.GenInfos.AutoMode &&
                !m_Server.GenInfos.Pause &&
                !m_Server.SeqFlag.LdTimeOut &&
                m_CheckNextProcessTime)
            {
                m_NextProcessTime++;
            }
        }

        public override void InitSeq()
        {
            SeqNo = 0;

            m_CheckPreProcessTime = false;
            m_CheckNextProcessTime = false;

            m_PreProcessTime = 0;
            m_NextProcessTime = 0;
        } 
        #endregion
    }     
}

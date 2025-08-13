using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public delegate bool GetInterlockCheckConditionDelegate(Gauge gauge);

    public class ThreadGauge : XSequence
    {
        #region Fields
        protected static IServerManager m_Server = null;
        protected static _GenericCollection<Gauge> m_Gauges;
        public GetInterlockCheckConditionDelegate GetInterlockCheckCondition;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Gauge device in m_Gauges)
            {
                RegisterSequence(new SeqGaugeInterlock(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadGauge(int scanTime, IServerManager server, GetInterlockCheckConditionDelegate checkCondition)
            : base(scanTime)
        {
            m_Server = server;
            m_Gauges = DmsComponents.Instance.ComponentContainer.GetCollection<Gauge>();
            GetInterlockCheckCondition = checkCondition;

            RegisterSequences();
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                // 값을 갱신하고
                foreach (Gauge device in m_Gauges)
                {
                    device.UpdateValue();
                }

                foreach (SeqGaugeInterlock seq in m_SeqFunctions)
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
        public static _GenericCollection<Gauge> Units
        {
            get
            {
                if (m_Gauges == null) m_Gauges = new _GenericCollection<Gauge>();
                return m_Gauges;
            }
        }
        #endregion

        #region Virtual Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqGaugeInterlock : XSeqFunction
    {
        #region Fields
        private enum IntrState
        {
            LowAlarm, UpAlarm, LowWarning, UpWarning, Noop
        }

        private Gauge m_Gauge;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadGauge m_Control;
        private double m_RecheckDelayTime = 1000;
        private double m_FirstDelayTime = 5000;
        private IntrState m_IntrState = IntrState.Noop;
        private bool[] m_IsAlarmSet;
        private string m_Msg = "";
        #endregion

        #region Contructor
        public SeqGaugeInterlock(ThreadGauge control, Gauge gauge)
        {
            m_Gauge = gauge;
            m_Server = m_Gauge.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", m_Gauge.Name);
            m_IsAlarmSet = new bool[(int)IntrState.Noop];
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;   //TODO:자신의 Init을 보도록 

            int nSeqNo = this.m_SeqNo;
            bool checkCond = true;
            checkCond = m_Control.GetInterlockCheckCondition(m_Gauge);

            switch (nSeqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        m_StartTicks = Common.XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (m_IsAlarmSet[(int)IntrState.LowAlarm])
                        {
                            m_IsAlarmSet[(int)IntrState.LowAlarm] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.UpAlarm])
                        {
                            m_IsAlarmSet[(int)IntrState.UpAlarm] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.LowWarning])
                        {
                            m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.UpWarning])
                        {
                            m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                            m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }

                        //if (m_IsAlarm)
                        //{
                        //    m_IsAlarm = false;
                        //}
                        if (m_Gauge.IsAlarm)
                        {
                            m_Gauge.IsAlarm = false;
                        }
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > m_FirstDelayTime)
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal = m_Gauge.CurValue;

                        double lowAlarm = m_Gauge.SetupInterlock.LowAlarm;
                        double upAlarm = m_Gauge.SetupInterlock.HighAlarm;
                        double lowWarning = m_Gauge.SetupInterlock.LowWarning;
                        double upWarning = m_Gauge.SetupInterlock.HighWarning;

                        if (curVal < lowAlarm)
                        {
                            m_IntrState = IntrState.LowAlarm;
                        }
                        else if (curVal > upAlarm)
                        {
                            m_IntrState = IntrState.UpAlarm;
                        }
                        else if (curVal < lowWarning)
                        {
                            m_IntrState = IntrState.LowWarning;
                        }
                        else if (curVal > upWarning)
                        {
                            m_IntrState = IntrState.UpWarning;
                        }

                        if (m_IntrState != IntrState.Noop)
                        {
                            m_StartTicks = Common.XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > m_RecheckDelayTime)
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal = m_Gauge.CurValue;

                        double lowAlarm = m_Gauge.SetupInterlock.LowAlarm;
                        double upAlarm = m_Gauge.SetupInterlock.HighAlarm;
                        double lowWarning = m_Gauge.SetupInterlock.LowWarning;
                        double upWarning = m_Gauge.SetupInterlock.HighWarning;

                        if (curVal < lowAlarm)
                        {
                            m_IntrState = IntrState.LowAlarm;
                        }
                        else if (curVal > upAlarm)
                        {
                            m_IntrState = IntrState.UpAlarm;
                        }
                        else if (curVal < lowWarning)
                        {
                            m_IntrState = IntrState.LowWarning;
                        }
                        else if (curVal > upWarning)
                        {
                            m_IntrState = IntrState.UpWarning;
                        }

                        if (m_IntrState == IntrState.LowAlarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.LowAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.LowAlarm] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                            }
                            if (m_IsAlarmSet[(int)IntrState.LowWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.LowAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.LowAlarm] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerAlarm.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Lower Alarm");
                            }
                        }

                        if (m_IntrState == IntrState.UpAlarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.UpAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.UpAlarm] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                            }
                            if (m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.UpAlarm])
                            {
                                m_IsAlarmSet[(int)IntrState.UpAlarm] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperAlarm.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Upper Alarm");
                            }
                        }

                        if (m_IntrState == IntrState.LowWarning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.LowWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_LowerWarning.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.LowWarning] && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_IsAlarmSet[(int)IntrState.LowWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_LowerWarning.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Lower Warning");
                            }
                        }

                        if (m_IntrState == IntrState.UpWarning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = true;
                                m_EqpManager.SetAlarm(m_Gauge.ALM_UpperWarning.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.UpWarning] && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_IsAlarmSet[(int)IntrState.UpWarning] = false;
                                m_EqpManager.ResetAlarm(m_Gauge.ALM_UpperWarning.Id);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Recovery : Upper Warning");
                            }
                        }

                        if (!m_IsAlarmSet[(int)IntrState.LowAlarm] && !m_IsAlarmSet[(int)IntrState.UpAlarm])
                        {
                            //if (m_IsAlarm)
                            if (m_Gauge.IsAlarm)
                            {
                                //m_IsAlarm = false;
                                m_Gauge.IsAlarm = false;
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "OK : Recovery");
                            }

                            if (!m_IsAlarmSet[(int)IntrState.LowWarning] && !m_IsAlarmSet[(int)IntrState.UpWarning])
                            {
                                nSeqNo = 20;
                            }
                        }
                        else
                        {
                            //if (!m_IsAlarm)
                            if (!m_Gauge.IsAlarm)
                            {
                                //m_IsAlarm = true;
                                m_Gauge.IsAlarm = true;
                            }
                            else
                            {
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "NG : Recovery");
                            }

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Check Recovery");
                        m_RecheckDelayTime = m_FirstDelayTime;
                        //StartTime = DateTime.Now;
                        m_StartTicks = Common.XFunc.GetTickCount();
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}
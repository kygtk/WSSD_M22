using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadFfu : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<FanFilter> m_Units;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (FanFilter device in m_Units)
            {
                RegisterSequence(new SeqFfu(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadFfu(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<FanFilter>();

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
        #endregion

        #region Static Methods
        public static _GenericCollection<FanFilter> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<FanFilter>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqFfu : XSeqFunction
    {
        #region Fields
        protected static ThreadFfu m_Control;
        protected static IEqpManager m_EqpManager;
        protected static _GenInfoHandler m_GenInfos;
        protected FanFilter m_Device;
        protected new int[] m_AlarmId = { 0, 0 };
        #endregion

        #region Constructor
        public SeqFfu(ThreadFfu control, FanFilter device)
        {
            m_Device = device;
            m_EqpManager = m_Device.ServerManager.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = string.Format("{0} INTR", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool checkCond = m_Device.SetupFfuInterlock.Use;

            if (m_Device.IsAlarmDetect() && checkCond && m_AlarmId[0] == 0)
            {
                m_AlarmId[0] = m_Device.ALM_FfuAlarm.Id;
                m_EqpManager.SetAlarm(m_AlarmId[0]);
                log = string.Format("Alarm Set : {0} Alarm Detect", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsAlarmDetect() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_EqpManager.ResetAlarm(m_AlarmId[0]);
                m_AlarmId[0] = 0;
                log = string.Format("Alarm Reset : {0} Alarm Detect", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            if (!m_Device.IsCpOn() && checkCond && m_AlarmId[1] == 0)
            {
                m_AlarmId[1] = m_Device.ALM_CpOff.Id;
                m_EqpManager.SetAlarm(m_AlarmId[1]);
                log = string.Format("Alarm Set : {0} CP Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((m_Device.IsCpOn() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_EqpManager.ResetAlarm(m_AlarmId[1]);
                m_AlarmId[1] = 0;
                log = string.Format("Alarm Reset : {0} CP Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            //Make Alarm Condition
            bool alarmCondition = false;
            for (int i = 0; i < m_AlarmId.Length; i++)
            {
                alarmCondition |= (m_AlarmId[i] != 0);
            }

            //Set CycleStop
            if (alarmCondition && m_Device.CycleStopInAlarmCondition && !m_Device.IsAlarm)
            {
                m_GenInfos.CycleStop = true;
            }

            m_Device.IsAlarm = alarmCondition;
            return -1;
        }
        #endregion
    }
}

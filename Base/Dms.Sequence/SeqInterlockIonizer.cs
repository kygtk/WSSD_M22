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
    public class ThreadIonizer : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<Ionizer> m_Units;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Ionizer device in m_Units)
            {
                RegisterSequence(new SeqIonizer(this, device));
            }
        }

        #endregion

        #region Constructor
        public ThreadIonizer(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<Ionizer>();

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
        public static _GenericCollection<Ionizer> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<Ionizer>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqIonizer : XSeqFunction
    {
        #region Fields
        protected Ionizer m_Device;
        protected static IEqpManager m_EqpManager;
        protected static ThreadIonizer m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected new int[] m_AlarmId = { 0, 0, 0, 0 };
        #endregion

        #region Constructor
        public SeqIonizer(ThreadIonizer control, Ionizer device)
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
            bool checkCond = true;
            checkCond &= m_Device.SetupIonizerInterlock.Use;

            //출력이 꺼져있다면 무조건 켠다
            if (!m_Device.IsRun())
            {
                m_Device.SetRun(true);
            }

            if (m_Device.IsLevelAlarm() && checkCond && m_AlarmId[0] == 0)
            {
                //m_Device.IsAlarm = true;
                //m_GenInfos.CycleStop = true;
                m_AlarmId[0] = m_Device.ALM_LevelAlarm.Id;
                m_EqpManager.SetAlarm(m_AlarmId[0]);
                log = string.Format("Alarm Set : {0} Level Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsLevelAlarm() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                //m_Device.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId[0]);
                m_AlarmId[0] = 0;
                log = string.Format("Alarm Reset : {0} Level Alarm Detect", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            if (m_Device.IsConditionAlarm() && checkCond && m_AlarmId[1] == 0)
            {
                //m_Device.IsAlarm = true;
                //m_GenInfos.CycleStop = true;
                m_AlarmId[1] = m_Device.ALM_ConditionAlarm.Id;
                m_EqpManager.SetAlarm(m_AlarmId[1]);
                log = string.Format("Alarm Set : {0} Condition Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsConditionAlarm() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                //m_Device.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId[1]);
                m_AlarmId[1] = 0;
                log = string.Format("Alarm Reset : {0} Condition Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            if (m_Device.IsControlAlarm() && checkCond && m_AlarmId[2] == 0)
            {
                //m_Device.IsAlarm = true;
                //m_GenInfos.CycleStop = true;
                m_AlarmId[2] = m_Device.ALM_ControlAlarm.Id;
                m_EqpManager.SetAlarm(m_AlarmId[2]);
                log = string.Format("Alarm Set : {0} Control Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsControlAlarm() || !checkCond) && (m_AlarmId[2] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                //m_Device.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId[2]);
                m_AlarmId[2] = 0;
                log = string.Format("Alarm Reset : {0} Control Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            if (m_Device.IsRunAlarm() && checkCond && m_AlarmId[3] == 0)
            {
                //m_Device.IsAlarm = true;
                //m_GenInfos.CycleStop = true;
                m_AlarmId[3] = m_Device.ALM_RunAlarm.Id;
                m_EqpManager.SetAlarm(m_AlarmId[3]);
                log = string.Format("Alarm Set : {0} Run Alarm", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsRunAlarm() || !checkCond) && (m_AlarmId[3] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                //m_Device.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId[3]);
                m_AlarmId[3] = 0;
                log = string.Format("Alarm Reset : {0} Run Alarm", m_Device.Name);
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

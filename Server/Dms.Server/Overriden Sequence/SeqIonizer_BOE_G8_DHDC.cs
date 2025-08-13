using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Server
{

    public class ThreadSeqIonizer_BOE_G8_DHDC : ThreadIonizer // 11.03.25 minhan
    {
        public ThreadSeqIonizer_BOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {

        }

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Ionizer device in m_Units)
            {
                RegisterSequence(new SeqControlIonizer(this, device));
            }
        }
        #endregion

        public class SeqControlIonizer : XSeqFunction
        {
            #region Fields
            protected Ionizer m_Device;
            protected static IEqpManager m_EqpManager;
            protected static ThreadIonizer m_Control;
            protected static _GenInfoHandler m_GenInfos;
            protected static ServerManager m_Server;
            protected new int[] m_AlarmId = { 0, 0, 0, 0 };
            protected Alarm ALM_LowIonAlarm = null;
            protected Alarm ALM_HighVoltAlarm = null;
            protected Alarm ALM_TipCleanAlarm = null;
            protected Alarm ALM_StopAlarm = null;
            private bool StopAlarmCheck = false;
            private double IonizerAlarmCheckTime;
            #endregion

            #region Constructor
            public SeqControlIonizer(ThreadIonizer control, Ionizer device)
            {
                m_Device = device;
                m_EqpManager = m_Device.ServerManager.EqpStateManager;
                m_Control = control;
                m_GenInfos = GenInfoHandler.Instance;
                m_Server = ServerManager.Instance;
                m_SeqFunName = string.Format("{0} INTR", m_Device.Name);

                ALM_LowIonAlarm = new Alarm(m_Device.Name + " Low Ion Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HighVoltAlarm = new Alarm(m_Device.Name + " High Voltage Abnormal Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_TipCleanAlarm = new Alarm(m_Device.Name + " Tip Cleaning Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StopAlarm = new Alarm(m_Device.Name + " Stop Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                IonizerAlarmCheckTime = 0;
            }
            #endregion

            #region Sequence
            public override int Do()
            {
                string log;
                bool checkCond = true;
                checkCond &= m_Device.SetupIonizerInterlock.Use;
                IonizerAlarmCheckTime = m_Server.IonizerStopAlarmCheckTime.GetValue<double>() * 1000;

                if (!m_Device.IsRun())
                {
                    m_Device.SetRun(true);
                }

                if (m_Device.IsLevelAlarm() && checkCond && m_AlarmId[0] == 0)
                {
                    m_Device.IsAlarm = true;
                    //m_GenInfos.CycleStop = true; // 11.03.25 minhan
                    m_AlarmId[0] = ALM_LowIonAlarm.Id;
                    m_EqpManager.SetAlarm(m_AlarmId[0]);
                    log = string.Format("Alarm Set : {0} Low Ion Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }
                else if ((!m_Device.IsLevelAlarm() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                {
                    m_Device.IsAlarm = false;
                    m_EqpManager.ResetAlarm(m_AlarmId[0]);
                    m_AlarmId[0] = 0;
                    log = string.Format("Alarm Reset : {0} Low Ion Alarm Detect", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }

                if (m_Device.IsConditionAlarm() && checkCond && m_AlarmId[1] == 0)
                {
                    m_Device.IsAlarm = true;
                    //m_GenInfos.CycleStop = true;
                    m_AlarmId[1] = ALM_HighVoltAlarm.Id;
                    m_EqpManager.SetAlarm(m_AlarmId[1]);
                    log = string.Format("Alarm Set : {0} High Voltage Abnormal Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }
                else if ((!m_Device.IsConditionAlarm() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                {
                    m_Device.IsAlarm = false;
                    m_EqpManager.ResetAlarm(m_AlarmId[1]);
                    m_AlarmId[1] = 0;
                    log = string.Format("Alarm Reset : {0} High Voltage Abnormal Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }

                if (m_Device.IsControlAlarm() && checkCond && m_AlarmId[2] == 0)
                {
                    m_Device.IsAlarm = true;
                    //m_GenInfos.CycleStop = true;
                    m_AlarmId[2] = ALM_TipCleanAlarm.Id;
                    m_EqpManager.SetAlarm(m_AlarmId[2]);
                    log = string.Format("Alarm Set : {0} Tip Cleaning Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }
                else if ((!m_Device.IsControlAlarm() || !checkCond) && (m_AlarmId[2] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                {
                    m_Device.IsAlarm = false;
                    m_EqpManager.ResetAlarm(m_AlarmId[2]);
                    m_AlarmId[2] = 0;
                    log = string.Format("Alarm Reset : {0} Tip Cleaning Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }

                if (m_Device.IsRunAlarm() && checkCond && (m_AlarmId[3] == 0) &&
                    m_Device.IsRun())
                {
                    if (!StopAlarmCheck)
                    {
                        StopAlarmCheck = true;
                        m_StartTicks = XFunc.GetTickCount();
                    }
                    else
                    {
                        if (GetElapsedTicks() > IonizerAlarmCheckTime)
                        {
                            m_Device.IsAlarm = true;
                            //m_GenInfos.CycleStop = true;
                            m_AlarmId[3] = ALM_StopAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[3]);
                            log = string.Format("Alarm Set : {0} Stop Alarm", m_Device.Name);
                            m_Device.SetLog(m_SeqFunName, 0, 0, log);
                        }
                    }
                }
                else if ((!m_Device.IsRunAlarm() || !checkCond) && (m_AlarmId[3] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                {
                    m_Device.IsAlarm = false;
                    m_EqpManager.ResetAlarm(m_AlarmId[3]);
                    m_AlarmId[3] = 0;
                    log = string.Format("Alarm Reset : {0} Stop Alarm", m_Device.Name);
                    m_Device.SetLog(m_SeqFunName, 0, 0, log);
                }

                if (!m_Device.IsRunAlarm() && StopAlarmCheck) StopAlarmCheck = false;
                return -1;
            }
            #endregion
        }


    }
}

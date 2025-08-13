using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using System.Threading;

namespace Dms.Server
{
    public class ThreadSeqIonizer_P8E : ThreadIonizer
    {
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
        public ThreadSeqIonizer_P8E(int scanTime, ServerManager server)
            : base(scanTime,server)
        {
            m_Server = server;
            m_Units = m_Server.ComponentContainer.GetCollection<Ionizer>();

            RegisterSequences();
        } 
        #endregion

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
                m_GenInfos = m_Device.ServerManager.GenInfos;// GenInfoHandler.Instance;

                SeqFunName = string.Format("{0} INTR", m_Device.Name);
            }
            #endregion

            #region Sequence
            public override int Do()
            {
                bool runCond = true;
                runCond &= m_Device.AlwaysOn;
                runCond &= m_GenInfos.AutoMode;
                //seq 처리 하자 (출력을 바로 주고 체크 해서.... 프로그램 시작시 Run Alarm 발생) taegoo
                int nSeqNo = this.SeqNo;

                string log;
                bool checkCond = true;
                checkCond &= m_Device.SetupIonizerInterlock.Use;

                switch(nSeqNo)
                {
                    case 0:
                        if (m_GenInfos.AutoMode && runCond && !m_Device.IsRun())
                        {
                            m_Device.SetRun(true);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else if (m_GenInfos.AutoMode && !runCond && m_Device.IsRun())
                        {
                            m_Device.SetRun(false);
                            nSeqNo = 0;
                        }
                        break;
                    case 10:
                        if(GetElapsedTicks()>3000)
                        {
                            if (m_Device.IsLevelAlarm() && checkCond && m_AlarmId[0] == 0)
                            {
                                //m_Device.IsAlarm = true;
                                //m_GenInfos.CycleStop = true;
                                m_AlarmId[0] = m_Device.ALM_LevelAlarm.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[0]);
                                log = string.Format("Alarm Set : {0} Level Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                            }
                            else if ((!m_Device.IsLevelAlarm() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                //m_Device.IsAlarm = false;
                                m_EqpManager.ResetAlarm(m_AlarmId[0]);
                                m_AlarmId[0] = 0;
                                log = string.Format("Alarm Reset : {0} Level Alarm Detect", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                                nSeqNo = 0;
                            }

                            if (m_Device.IsConditionAlarm() && checkCond && m_AlarmId[1] == 0)
                            {
                                //m_Device.IsAlarm = true;
                                //m_GenInfos.CycleStop = true;
                                m_AlarmId[1] = m_Device.ALM_ConditionAlarm.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[1]);
                                log = string.Format("Alarm Set : {0} Condition Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                            }
                            else if ((!m_Device.IsConditionAlarm() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                //m_Device.IsAlarm = false;
                                m_EqpManager.ResetAlarm(m_AlarmId[1]);
                                m_AlarmId[1] = 0;
                                log = string.Format("Alarm Reset : {0} Condition Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                                nSeqNo = 0;
                            }

                            if (m_Device.IsControlAlarm() && checkCond && m_AlarmId[2] == 0)
                            {
                                //m_Device.IsAlarm = true;
                                //m_GenInfos.CycleStop = true;
                                m_AlarmId[2] = m_Device.ALM_ControlAlarm.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[2]);
                                log = string.Format("Alarm Set : {0} Control Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                            }
                            else if ((!m_Device.IsControlAlarm() || !checkCond) && (m_AlarmId[2] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                //m_Device.IsAlarm = false;
                                m_EqpManager.ResetAlarm(m_AlarmId[2]);
                                m_AlarmId[2] = 0;
                                log = string.Format("Alarm Reset : {0} Control Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                                nSeqNo = 0;
                            }

                            if (m_Device.IsRunAlarm() && checkCond && m_AlarmId[3] == 0)
                            {
                                //m_Device.IsAlarm = true;
                                //m_GenInfos.CycleStop = true;
                                m_AlarmId[3] = m_Device.ALM_RunAlarm.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[3]);
                                log = string.Format("Alarm Set : {0} Run Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                            }
                            else if ((!m_Device.IsRunAlarm() || !checkCond) && (m_AlarmId[3] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                //m_Device.IsAlarm = false;
                                m_EqpManager.ResetAlarm(m_AlarmId[3]);
                                m_AlarmId[3] = 0;
                                log = string.Format("Alarm Reset : {0} Run Alarm", m_Device.Name);
                                m_Device.SetLog(SeqFunName, 0, 0, log);
                                nSeqNo = 0;
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
                        }
                        break;
                }
                this.SeqNo=nSeqNo;
                return -1;
            }
            #endregion
        }
    }
}

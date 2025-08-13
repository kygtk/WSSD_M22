using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Sequence;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadFfuControl : XSequence // 11.03.07 minhan
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<FanFilterControl> m_Units;
        private XLog FfuControlLog;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (FanFilterControl device in m_Units)
            {
                RegisterSequence(new SeqFfuControl(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadFfuControl(int scanTime, IServerManager server)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<FanFilterControl>();
            FfuControlLog = new XLog("FFU Control Log", XLog.LogStampType.UseStamp);
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

        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("Ffu Control \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            FfuControlLog.TextOut(log);
        }

        #endregion

        #region Static Methods
        public static _GenericCollection<FanFilterControl> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<FanFilterControl>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqFfuControl : XSeqFunction // 11.03.07 minhan
    {
        #region Fields
        private ThreadFfuControl m_Control;
        private ServerManager m_Server;
        private IEqpManager m_EqpManager;
        private _GenInfoHandler m_GenInfos;
        private Simul m_Simul;
        private FanFilterControl m_Device;
        private Alarm ALM_ComportErr;
        private int[] m_FfuConAlarmId;
        private bool m_checkCond;
        private int m_MaxNo;
        private bool m_CurAlarm;
        private bool m_MotorAlarm;
        private bool m_NoConnect;
        private bool m_bRecived;
        private string m_CurAlarmLog;
        private string m_MotorAlarmLog;
        private string m_NoConnectLog;
        private string log;
        private int m_Count;
        private int m_alarmCount; // 11.05.05 minhan
        private int m_SetMotorSpeed; // 11.05.06 minhan
        private bool m_MotorSpeedChange; // 11.05.06 minhan
        #endregion

        #region Constructor
        public SeqFfuControl(ThreadFfuControl control, FanFilterControl device)
        {
            m_Control = control;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_GenInfos = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_Device = device;
            ALM_ComportErr = new Alarm("FFU Control Comport Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_FfuConAlarmId = new int[4];
            m_checkCond = false;
            m_MaxNo = 0;
            m_CurAlarm = false;
            m_MotorAlarm = false;
            m_NoConnect = false;
            m_bRecived = false;
            m_CurAlarmLog = "";
            m_MotorAlarmLog = "";
            m_NoConnectLog = "";
            log = "";
            m_Count = 0;
            m_alarmCount = 0;
            m_SetMotorSpeed = 0; // 11.05.06 minhan
            m_MotorSpeedChange = false; // 11.05.06 minhan
            m_Device.FfuDataReceived += new FanFilterControl.GetFfuControlReceivedData(DataReceived);
            m_SeqFunName = string.Format("{0} INTR", m_Device.Name);
        }
        #endregion

        #region Methods
        private void DataReceived(object sender)
        {
            m_bRecived = true;
        }
        public void AlarmDected()
        {
            m_CurAlarm = false;
            m_MotorAlarm = false;
            m_NoConnect = false;
            m_CurAlarmLog = "";
            m_MotorAlarmLog = "";
            m_NoConnectLog = "";
            m_SetMotorSpeed = m_Server.FfuMotorSpeed.GetValue<int>(); // 11.05.06 minhan

            for (int i = 0; i < m_MaxNo; i++)
            {
                if (m_Device.IsCurrentAlarm(i))
                {
                    m_CurAlarmLog += "/" + i.ToString();
                    if (!m_CurAlarm) m_CurAlarm = true;
                }

                if (m_Device.IsMotorAlarm(i))
                {
                    m_MotorAlarmLog += "/" + i.ToString();
                    if (!m_MotorAlarm) m_MotorAlarm = true;
                }

                if (m_Device.IsNoConnect(i))
                {
                    m_NoConnectLog += "/" + i.ToString();
                    if (!m_NoConnect) m_NoConnect = true;
                }

                if (!m_MotorSpeedChange && (m_SetMotorSpeed != m_Device.IsSetMotorSpeed(i))) // 11.05.06 minhan
                {
                    m_MotorSpeedChange = true;
                }
            }
        }
        public void AlarmSet()
        {
            if (((m_FfuConAlarmId[0] > 0) ||
                 (m_FfuConAlarmId[1] > 0) ||
                 (m_FfuConAlarmId[2] > 0) ||
                 (m_FfuConAlarmId[3] > 0)) && !m_Device.IsAlarm)
            {
                m_Device.IsAlarm = true;
            }
            else if ((m_FfuConAlarmId[0] == 0) &&
                     (m_FfuConAlarmId[1] == 0) &&
                     (m_FfuConAlarmId[2] == 0) &&
                     (m_FfuConAlarmId[3] == 0)) // 11.04.11 minhan
            {
                if (m_Device.IsAlarm) m_Device.IsAlarm = false;
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            m_checkCond = m_Device.SetupFfuControlIntr.GetValue<bool>();
            m_MaxNo = m_Device.Max_FFU_No;

            AlarmSet();

            if (m_EqpManager.AlarmResetSwitchPushed) // 11.05.05 minhan
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m_FfuConAlarmId[i] > 0)
                    {
                        m_EqpManager.ResetAlarm(m_FfuConAlarmId[i]);
                        m_FfuConAlarmId[i] = 0;
                    }
                }
            }


            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_checkCond)
                        {
                            m_bRecived = false;
                            m_Count = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 5;
                        }
                        else
                        {
                            if (m_alarmCount > 0) m_alarmCount = 0; // 11.05.05 minhan
                        }
                    }
                    break;
                case 5:
                    {
                        if (GetElapsedTicks() > 2000) // 11.05.05 minhan
                        {
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        m_Count++;

                        if (!m_checkCond)
                        {
                            m_bRecived = false;
                            m_Count = 0;
                            nSeqNo = 0;
                        }
                        else if (m_Count > 10)
                        {
                            m_Count = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else if (m_bRecived || m_Simul.Comport)
                        {
                            m_bRecived = false;
                            m_Count = 0;
                            m_alarmCount = 0; // 11.05.05 minhan
                            if (GlobalVar.FfuControlComErr) GlobalVar.FfuControlComErr = false;

                            nSeqNo = 30;
                        }
                        else
                        {
                            m_Device.SendCommand();
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 5;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!m_checkCond)
                        {
                            m_bRecived = false;
                            m_Count = 0;
                            nSeqNo = 0;
                        }
                        else if (m_bRecived || m_Simul.Comport)
                        {
                            m_bRecived = false;
                            m_Count = 0;
                            m_alarmCount = 0; // 11.05.05 minhan
                            if (GlobalVar.FfuControlComErr) GlobalVar.FfuControlComErr = false; // 11.04.11 minhan

                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > 3000) // 11.04.11 minhan
                        {
                            //m_Control.SetLog(SeqFunName, 0, 0, "ComPort Error(case 20)"); // 11.05.14 minhan
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 30:
                    {
                        AlarmDected();

                        if (m_checkCond && m_CurAlarm && (m_FfuConAlarmId[1] == 0))
                        {
                            m_FfuConAlarmId[1] = m_Device.ALM_OverCurAlarm.Id;
                            m_EqpManager.SetAlarm(m_FfuConAlarmId[1]);
                            log = string.Format("Alarm Set : {0} Over Current", m_CurAlarmLog);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if ((!m_CurAlarm || !m_checkCond) && (m_FfuConAlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_FfuConAlarmId[1]);
                            m_FfuConAlarmId[1] = 0;
                            log = string.Format("Alarm Reset : {0} Over Current", m_Device.Name);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }

                        if (m_checkCond && m_MotorAlarm && (m_FfuConAlarmId[2] == 0))
                        {
                            m_FfuConAlarmId[2] = m_Device.ALM_MotorAlarm.Id;
                            m_EqpManager.SetAlarm(m_FfuConAlarmId[2]);
                            log = string.Format("Alarm Set : {0} Motor Alarm", m_MotorAlarmLog);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if ((!m_MotorAlarm || !m_checkCond) && (m_FfuConAlarmId[2] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_FfuConAlarmId[2]);
                            m_FfuConAlarmId[2] = 0;
                            log = string.Format("Alarm Reset : {0} Motor Alarm", m_Device.Name);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }

                        if (m_checkCond && m_NoConnect && !m_Simul.Comport && (m_FfuConAlarmId[3] == 0))
                        {
                            m_FfuConAlarmId[3] = m_Device.ALM_NoConnect.Id;
                            m_EqpManager.SetAlarm(m_FfuConAlarmId[3]);
                            log = string.Format("Alarm Set : {0} No Connect", m_NoConnectLog);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if ((!m_NoConnect || !m_checkCond) && (m_FfuConAlarmId[3] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_FfuConAlarmId[3]);
                            m_FfuConAlarmId[3] = 0;
                            log = string.Format("Alarm Reset : {0} No Connect", m_Device.Name);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                        }

                        if (!m_MotorSpeedChange || m_Simul.Comport) // 11.05.14 minhan
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            m_MotorSpeedChange = false;
                            m_SetMotorSpeed = m_Server.FfuMotorSpeed.GetValue<int>();
                            m_Device.SendMotorSpeed(m_SetMotorSpeed);
                            log = string.Format("Motor Speed Change : {0}", m_SetMotorSpeed);
                            m_Control.SetLog(m_SeqFunName, 0, 0, log);
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1000: // 11.05.05 minhan
                    {
                        m_alarmCount++;

                        if (m_alarmCount > 3) // 11.05.14 minhan
                        {
                            m_alarmCount = 0;
                            GlobalVar.FfuControlComErr = true;
                            m_FfuConAlarmId[0] = ALM_ComportErr.Id;
                            m_EqpManager.SetAlarm(m_FfuConAlarmId[0]);
                            nSeqNo = 1100;
                        }
                        else
                        {
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1100:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        GlobalVar.FfuControlComErr = false;
                        m_EqpManager.ResetAlarm(m_FfuConAlarmId[0]);
                        m_FfuConAlarmId[0] = 0;

                        m_Control.SetLog(m_SeqFunName, 0, 0, "Alarm Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }
}

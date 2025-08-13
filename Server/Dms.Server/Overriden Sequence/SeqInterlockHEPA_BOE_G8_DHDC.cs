using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Data;

namespace Dms.Server
{
    public class ThreadHepaInterlockBOE_G8_DHDC : ThreadHepa
    {
        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (HepaFilter device in m_Units)
            {
                RegisterSequence(new SeqHepa(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadHepaInterlockBOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {
        }
        #endregion
    }

    public class SeqHepa : XSeqFunction
    {
        #region Fields
        protected HepaFilter m_Device;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHepa m_Control;
        protected new int[] m_AlarmId = { 0, 0 };
        #endregion

        #region Constructor
        public SeqHepa(ThreadHepa control, HepaFilter device)
        {
            m_Device = device;
            m_EqpManager = m_Device.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool checkCond = m_Device.SetupHepaInterlock.Use;
            int nSeqNo = this.m_SeqNo;

            if (!m_Device.IsCpOn() && checkCond && m_AlarmId[1] == 0)
            {
                m_Device.IsAlarm = true;
                m_AlarmId[1] = m_Device.ALM_CpOff.Id;
                m_EqpManager.SetAlarm(m_AlarmId[1]);
                log = string.Format("Alarm Set : {0} CP Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((m_Device.IsCpOn() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_Device.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId[1]);
                m_AlarmId[1] = 0;
                log = string.Format("Alarm Reset : {0} CP Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            switch (nSeqNo)
            {
                case 0:
                    if (m_Device.IsAlarmDetect() && checkCond && m_AlarmId[0] == 0)
                    {
                        //m_Device.IsAlarm = true;
                        //m_AlarmId[0] = m_Device.ALM_HepaAlarm.Id;
                        //m_EqpManager.SetAlarm(m_AlarmId[0]);


                        log = string.Format("First Alarm Check {0} Alarm Detect", m_Device.Name);
                        m_Device.SetLog(m_SeqFunName, 0, 0, log);

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else if ((!m_Device.IsAlarmDetect() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Device.IsAlarm = false;
                        m_EqpManager.ResetAlarm(m_AlarmId[0]);
                        m_AlarmId[0] = 0;
                        log = string.Format("Alarm Reset : {0} Alarm Detect", m_Device.Name);
                        m_Device.SetLog(m_SeqFunName, 0, 0, log);
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 3000)
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (!checkCond)
                    {
                        log = string.Format("Setting Change : {0} Alarm Setting -> No Check", m_Device.Name);
                        m_Device.SetLog(m_SeqFunName, 0, 0, log);
                    }
                    if (m_Device.IsAlarmDetect() && checkCond && m_AlarmId[0] == 0)
                    {
                        m_Device.IsAlarm = true;
                        m_AlarmId[0] = m_Device.ALM_HepaAlarm.Id;
                        m_EqpManager.SetAlarm(m_AlarmId[0]);
                        log = string.Format("Recheck Alarm Set : {0} Alarm Detect", m_Device.Name);
                        m_Device.SetLog(m_SeqFunName, 0, 0, log);
                    }
                    else if (!m_Device.IsAlarmDetect())
                    {
                        log = string.Format("Alarm ReCheck : {0} No Alarm Condition", m_Device.Name);
                        m_Device.SetLog(m_SeqFunName, 0, 0, log);
                    }
                    nSeqNo = 0;
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}

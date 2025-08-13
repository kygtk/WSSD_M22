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
    public class ThreadForcedExhaust : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<ForcedExhaust> m_Units;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (ForcedExhaust device in m_Units)
            {
                RegisterSequence(new SeqForcedExhaust(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadForcedExhaust(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<ForcedExhaust>();

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
        public static _GenericCollection<ForcedExhaust> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<ForcedExhaust>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqForcedExhaust : XSeqFunction
    {
        #region Fields
        protected static ThreadForcedExhaust m_Control;
        protected static IEqpManager m_EqpManager;
        protected static _GenInfoHandler m_GenInfos;
        protected ForcedExhaust m_Device;
        protected new int[] m_AlarmId = { 0, 0 };
        #endregion

        #region Constructor
        public SeqForcedExhaust(ThreadForcedExhaust control, ForcedExhaust device)
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
            //출력이 꺼져있다면 무조건 켠다
            bool runCond = true;
            // runCond &= m_Device.AlwaysOn;
            runCond &= m_GenInfos.AutoMode;
            runCond &= m_Device.AlwaysOn || m_Device.ServerManager.JobCond.GetGlassCountInEqp() > 0;
            if (m_GenInfos.AutoMode && runCond && !m_Device.IsRun())
            {
                m_Device.SetRun(true);
            }
            else if (m_GenInfos.AutoMode && !runCond && m_Device.IsRun())
            {
                m_Device.SetRun(false);
            }

            string log;
            bool checkCond = m_Device.SetupInterlock.Use;

            if (m_Device.IsMcOff() && checkCond && m_AlarmId[0] == 0)
            {
                m_AlarmId[0] = m_Device.ALM_McOff.Id;
                m_EqpManager.SetAlarm(m_AlarmId[0]);
                log = string.Format("Alarm Set : {0} MC Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsMcOff() || !checkCond) && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_EqpManager.ResetAlarm(m_AlarmId[0]);
                m_AlarmId[0] = 0;
                log = string.Format("Alarm Reset : {0} MC Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }

            if (m_Device.IsElbOff() && checkCond && m_AlarmId[1] == 0)
            {
                m_AlarmId[1] = m_Device.ALM_ElbOff.Id;
                m_EqpManager.SetAlarm(m_AlarmId[1]);
                log = string.Format("Alarm Set : {0} ELB Off", m_Device.Name);
                m_Device.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if ((!m_Device.IsElbOff() || !checkCond) && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_EqpManager.ResetAlarm(m_AlarmId[1]);
                m_AlarmId[1] = 0;
                log = string.Format("Alarm Reset : {0} ELB Off", m_Device.Name);
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

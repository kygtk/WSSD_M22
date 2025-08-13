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
    public class ThreadLeakInterlock : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<LeakSensor> m_Units;
        #endregion

        #region Constructor
        public ThreadLeakInterlock(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<LeakSensor>();

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (LeakSensor device in m_Units)
            {
                RegisterSequence(new SeqLeakInterlock(this, device));
            }
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
        public static _GenericCollection<LeakSensor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<LeakSensor>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqLeakInterlock : XSeqFunction
    {
        #region Fields
        protected LeakSensor Leak;
        protected static IEqpManager m_EqpManager;
        protected static ThreadLeakInterlock m_Control;
        #endregion

        #region Constructor
        public SeqLeakInterlock(ThreadLeakInterlock control, LeakSensor leak)
        {
            Leak = leak;
            m_EqpManager = Leak.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", Leak.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool detected = true;
            //detected &= Leak.IsDetected();
            //detected &= Leak.SetupLeakInterlock.Use;
            detected &= Leak.IsDetectedInterlock();

            if (detected && !Leak.IsAlarm)
            {
                Leak.SetInterlockCondtion(true);
                Leak.IsAlarm = true;
                m_AlarmId = Leak.ALM_Interlock.Id;
                m_EqpManager.SetAlarm(m_AlarmId);
                log = string.Format("Alarm Set : {0} Detect", Leak.Name);
                Leak.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if (Leak.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
            {
                Leak.SetInterlockCondtion(false);
                Leak.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                log = string.Format("Alarm Reset : {0} Detect", Leak.Name);
                Leak.SetLog(m_SeqFunName, 0, 0, log);
            }

            return -1;
        }
        #endregion
    }
}

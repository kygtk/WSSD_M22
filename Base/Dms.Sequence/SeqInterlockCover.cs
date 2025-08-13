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
    public class ThreadCoverInterlock : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<CoverSensor> m_Units;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (CoverSensor device in m_Units)
            {
                RegisterSequence(new SeqCoverInterlock(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadCoverInterlock(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<CoverSensor>();

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
        public static _GenericCollection<CoverSensor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<CoverSensor>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqCoverInterlock : XSeqFunction
    {
        #region Fields
        private CoverSensor Cover;
        protected static IEqpManager m_EqpManager;
        protected static ThreadCoverInterlock m_Control;
        #endregion

        #region Constructor
        public SeqCoverInterlock(ThreadCoverInterlock control, CoverSensor cover)
        {
            Cover = cover;
            m_EqpManager = Cover.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", Cover.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool detected = true;
            //detected &= Cover.IsDetected();
            //detected &= Cover.SetupCoverInterlock.Use;
            detected &= Cover.IsDetectedInterlock();

            if (detected && !Cover.IsAlarm)
            {
                Cover.SetInterlockCondtion(true);
                Cover.IsAlarm = true;
                m_AlarmId = Cover.ALM_Interlock.Id;
                m_EqpManager.SetAlarm(m_AlarmId);
                log = string.Format("Alarm Set : {0} Open", Cover.Name);
                Cover.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if (Cover.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
            {
                Cover.SetInterlockCondtion(false);
                Cover.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                log = string.Format("Alarm Reset : {0} Open", Cover.Name);
                Cover.SetLog(m_SeqFunName, 0, 0, log);
            }

            return -1;
        }
        #endregion
    }
}

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
    public class ThreadAreaInterlock : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<AreaSensor> m_Units;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (AreaSensor device in m_Units)
            {
                RegisterSequence(new SeqAreaInterlock(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadAreaInterlock(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<AreaSensor>();

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
        public static _GenericCollection<AreaSensor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<AreaSensor>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqAreaInterlock : XSeqFunction
    {
        #region Fields
        private AreaSensor Area;
        protected static IEqpManager m_EqpManager;
        protected static ThreadAreaInterlock m_Control;
        #endregion

        #region Constructor
        public SeqAreaInterlock(ThreadAreaInterlock control, AreaSensor area)
        {
            Area = area;
            m_EqpManager = Area.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", Area.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool detected = true;
            //detected &= Cover.IsDetected();
            //detected &= Cover.SetupCoverInterlock.Use;
            detected &= Area.IsDetectedInterlock();

            if (detected && !Area.IsAlarm)
            {
                Area.SetInterlockCondtion(true);
                Area.IsAlarm = true;
                m_AlarmId = Area.ALM_Interlock.Id;
                m_EqpManager.SetAlarm(m_AlarmId);
                log = string.Format("Alarm Set : {0} Open", Area.Name);
                Area.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if (Area.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
            {
                Area.SetInterlockCondtion(false);
                Area.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                log = string.Format("Alarm Reset : {0} Open", Area.Name);
                Area.SetLog(m_SeqFunName, 0, 0, log);
            }

            return -1;
        }
        #endregion
    }
}

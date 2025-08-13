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
    public class ThreadDoorInterlock : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DoorSensor> m_Units;
        #endregion

        #region Properties
        public static _GenericCollection<DoorSensor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<DoorSensor>();
                return m_Units;
            }
        }
        #endregion

        #region Constructor
        public ThreadDoorInterlock(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<DoorSensor>();

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (DoorSensor device in m_Units)
            {
                RegisterSequence(new SeqDoorInterlock(this, device));
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
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqDoorInterlock : XSeqFunction
    {
        #region Fields
        private DoorSensor Door;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDoorInterlock m_Control;
        #endregion

        #region Constructor
        public SeqDoorInterlock(ThreadDoorInterlock control, DoorSensor door)
        {
            Door = door;
            m_EqpManager = Door.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", Door.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            string log;
            bool detected = true;
            //detected &= Door.IsDetected();
            //detected &= Door.SetupDoorInterlock.Use;
            detected &= Door.IsDetectedInterlock();

            if (detected && !Door.IsAlarm)
            {
                Door.SetInterlockCondtion(true);
                Door.IsAlarm = true;
                m_AlarmId = Door.ALM_Interlock.Id;
                m_EqpManager.SetAlarm(m_AlarmId);
                log = string.Format("Alarm Set : {0} Open", Door.Name);
                Door.SetLog(m_SeqFunName, 0, 0, log);
            }
            else if (Door.IsAlarm && !detected && m_EqpManager.AlarmResetSwitchPushed)
            {
                Door.SetInterlockCondtion(false);
                Door.IsAlarm = false;
                m_EqpManager.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                log = string.Format("Alarm Reset : {0} Open", Door.Name);
                Door.SetLog(m_SeqFunName, 0, 0, log);
            }

            return -1;
        }
        #endregion
    }
}

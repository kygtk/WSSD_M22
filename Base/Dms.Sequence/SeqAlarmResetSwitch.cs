///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.06
// Author       : jemoon
// Description  : ThreadAlarmResetSwitch Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadAlarmResetSwitch : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<AlarmResetSwitch> m_Units;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (AlarmResetSwitch device in m_Units)
            {
                RegisterSequence(new SeqAlarmResetLamp(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadAlarmResetSwitch(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<AlarmResetSwitch>();

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
        public static _GenericCollection<AlarmResetSwitch> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<AlarmResetSwitch>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods

        #endregion

        #region General Methods

        #endregion
    }

    public class SeqAlarmResetLamp : XSeqFunction
    {
        #region Fields
        private AlarmResetSwitch m_Device;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadAlarmResetSwitch m_Control;
        private bool m_OldState = false;
        #endregion

        #region Constructor
        public SeqAlarmResetLamp(ThreadAlarmResetSwitch control, AlarmResetSwitch device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} LAMP", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;

            bool bOn = m_EqpManager.IsAlarmState || m_EqpManager.IsWarningState;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (bOn)
                    {
                        m_Device.SetLampState(Lamp.On);
                        m_OldState = true;

                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!bOn)
                    {
                        m_Device.SetLampState(Lamp.Off);
                        m_OldState = false;
                        nSeqNo = 0;
                    }
                    else if (this.GetElapsedTicks() > 500)
                    {
                        m_OldState = !m_OldState;
                        m_Device.SetLampState(m_OldState ? Lamp.On : Lamp.Off);

                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}
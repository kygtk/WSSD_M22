///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.06
// Author       : jemoon
// Description  : ThreadBuzzerControl Class
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
    public class ThreadBuzzerControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<Buzzer> m_Units;
        #endregion

        #region Properties
        public static _GenericCollection<Buzzer> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<Buzzer>();
                return m_Units;
            }
        }
        #endregion

        #region Constructor
        public ThreadBuzzerControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<Buzzer>();

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Buzzer device in m_Units)
            {
                RegisterSequence(new SeqBuzzerMelodyControl(this, device));
                RegisterSequence(new SeqBuzzerStopControl(this, device));
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
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqBuzzerStopControl : XSeqFunction
    {
        #region Fields
        protected Buzzer m_Device;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadBuzzerControl m_Control;
        private bool m_OldState = false;
        #endregion

        #region Constructor
        public SeqBuzzerStopControl(ThreadBuzzerControl control, Buzzer device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} SWITCH", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Device.IsMelodyOn() && !m_Device.GetLampState())
                    {
                        m_Device.SetLampState(Lamp.On);
                        m_OldState = true;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Device.IsPushed() || !m_Device.IsMelodyOn())
                    {
                        m_EqpManager.BuzzerOffSwitchPushed = false;
                        m_Device.SetLampState(Lamp.Off);
                        m_Device.MelodyAllOff();
                        m_OldState = false;

                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 500)
                    {
                        m_OldState = !m_OldState;
                        m_Device.SetLampState(m_OldState ? Lamp.On : Lamp.Off);

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
            }

            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqBuzzerMelodyControl : XSeqFunction
    {
        #region Fields
        protected Buzzer m_Device;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadBuzzerControl m_Control;
        protected int m_OldAlarmCount = 0;
        protected int m_AlarmMelodyNumber = 1;
        #endregion

        #region Constructor
        public SeqBuzzerMelodyControl(ThreadBuzzerControl control, Buzzer device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_AlarmMelodyNumber = device.AlarmMelodyNumber;

            m_SeqFunName = string.Format("{0} MELODY", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int alarmCount = m_EqpManager.AlarmCount;

            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (alarmCount < m_OldAlarmCount)
                    {
                        m_OldAlarmCount = alarmCount;

                        if (alarmCount == 0)
                        {
                            m_Device.SetMelodyState(m_AlarmMelodyNumber, Melody.Off);
                        }
                    }
                    else if (alarmCount > m_OldAlarmCount)
                    {
                        m_OldAlarmCount = alarmCount;
                        m_Device.SetMelodyState(m_AlarmMelodyNumber, Melody.On);
                    }
                    break;
            }

            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}
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
    public class ThreadLampSwitchControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<LampSwitch> m_Units;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (LampSwitch device in m_Units)
            {
                RegisterSequence(new SeqSwitchLampToggle(device));
            }
        }
        #endregion

        #region Constructor
        public ThreadLampSwitchControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<LampSwitch>();

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
        public static _GenericCollection<LampSwitch> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<LampSwitch>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqSwitchLampToggle : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected LampSwitch m_Device;
        protected Lamp m_LampCommand = Lamp.Off;
        protected bool m_OldState;
        #endregion

        #region Constructor
        public SeqSwitchLampToggle(LampSwitch device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;

            m_SeqFunName = string.Format("{0} S/W LAMP", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        switch (m_Device.LampCommand)
                        {
                            case Lamp.Off:
                                m_Device.DoLamp.SetState(false);
                                m_OldState = false;
                                break;
                            case Lamp.On:
                                m_Device.DoLamp.SetState(true);
                                m_OldState = true;
                                break;
                            case Lamp.Toggle:
                                break;
                        }

                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (m_Device.LampCommand != Lamp.Toggle)
                    {
                        nSeqNo = 0;
                    }
                    else if (this.GetElapsedTicks() > 500)
                    {
                        m_OldState = !m_OldState;
                        m_Device.DoLamp.SetState(m_OldState);
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion

        #region Methods
        #endregion
    }
}

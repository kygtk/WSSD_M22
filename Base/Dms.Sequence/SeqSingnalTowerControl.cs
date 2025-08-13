///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.06
// Author       : jemoon
// Description  : ThreadSignalTowerControl Class
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
    public class ThreadSignalTowerControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<SignalTower> m_Units;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (SignalTower device in m_Units)
            {
                RegisterSequence(new SeqSignalLampToggle(this, device));
                RegisterSequence(new SeqSignalLampControl(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadSignalTowerControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<SignalTower>();

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
        public static _GenericCollection<SignalTower> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<SignalTower>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqSignalLampToggle : XSeqFunction
    {
        #region Fields
        protected SignalTower m_Device;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadSignalTowerControl m_Contorol;
        //protected List<IoDigitalOutput> m_Lamps = new List<IoDigitalOutput>();
        protected List<Lamp> m_LampCommands = new List<Lamp>();
        protected bool[] m_OldState;
        protected int m_LampCount;
        #endregion

        #region Constructor
        public SeqSignalLampToggle(ThreadSignalTowerControl control, SignalTower device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Contorol = control;

            //m_Lamps.Add(m_Device.DoSignalLampR);
            //m_Lamps.Add(m_Device.DoSignalLampY);
            //m_Lamps.Add(m_Device.DoSignalLampG);
            //m_Lamps.Add(m_Device.DoSignalLampB);
            m_LampCount = m_Device.LampCount;
            m_OldState = new bool[m_LampCount];

            m_SeqFunName = string.Format("{0} LAMP", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        UpdateCommand();
                        for (int i = 0; i < m_LampCount; i++)
                        {
                            switch (m_LampCommands[i])
                            {
                                case Lamp.Off:
                                    m_Device.SetState(i, false);
                                    //m_Lamps[i].SetState(false);
                                    m_OldState[i] = false;
                                    break;
                                case Lamp.On:
                                    m_Device.SetState(i, true);
                                    //m_Lamps[i].SetState(true);
                                    m_OldState[i] = true;
                                    break;
                                case Lamp.Toggle:
                                    break;
                            }
                        }

                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (this.GetElapsedTicks() > 500)
                    {
                        UpdateCommand();
                        for (int i = 0; i < m_LampCount; i++)
                        {
                            if (m_LampCommands[i] == Lamp.Toggle)
                            {
                                m_OldState[i] = !m_OldState[i];
                                m_Device.SetState(i, m_OldState[i]);
                                //m_Lamps[i].SetState(m_OldState[i]);
                            }
                        }

                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion

        #region Methods
        private void UpdateCommand()
        {
            m_LampCommands.Clear();
            m_LampCommands.Add(m_Device.LampCommandR);
            m_LampCommands.Add(m_Device.LampCommandY);
            m_LampCommands.Add(m_Device.LampCommandG);
            m_LampCommands.Add(m_Device.LampCommandB);
        }
        #endregion
    }

    public class SeqSignalLampControl : XSeqFunction
    {
        #region Fields
        protected SignalTower m_Device;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadSignalTowerControl m_Contorol;
        #endregion

        #region Constructor
        public SeqSignalLampControl(ThreadSignalTowerControl control, SignalTower device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Contorol = control;

            m_SeqFunName = string.Format("{0} LAMP", m_Device.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            LampConrolScenario();

            return -1;
        }
        #endregion

        #region Virtual
        protected virtual void LampConrolScenario()
        {
            if (m_EqpManager.IsAlarmState)
            {
                m_Device.LampCommandR = Lamp.Toggle;
                m_Device.LampCommandY = Lamp.Off;
                m_Device.LampCommandG = Lamp.Off;
                m_Device.LampCommandB = Lamp.Off;
            }
            else
            {   // 사양에 따라 짜야겠지
                if (m_Server.EqpStateManager.EqpUnit.EqpState == EqpState.Normal)
                {
                    m_Device.LampCommandR = Lamp.Off;
                    m_Device.LampCommandY = Lamp.Off;
                    m_Device.LampCommandG = Lamp.Toggle;
                    m_Device.LampCommandB = Lamp.Off;
                }
                else
                {
                    m_Device.LampCommandR = Lamp.Off;
                    m_Device.LampCommandY = Lamp.Off;
                    m_Device.LampCommandG = Lamp.Off;
                    m_Device.LampCommandB = Lamp.Off;
                }
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;


namespace Dms.Server
{
    public class ThreadSeqTowerLamp_BOE_G8_DHDC : ThreadSignalTowerControl
    {
        public ThreadSeqTowerLamp_BOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {

        }

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (SignalTower device in m_Units)
            {
                RegisterSequence(new SeqSignalLampToggle(this, device));
                RegisterSequence(new SeqSignalTowerLampControl(this, device));
            }
        }
        #endregion

    }

    public class SeqSignalTowerLampControl : XSeqFunction
    {
        #region Fields
        protected SignalTower m_Device;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadSignalTowerControl m_Contorol;
        //protected static Buzzer m_Buzzer;
        private GenInfoHandler m_GenInfo;
        private bool m_firstmatch; // 11.03.20 minhan
        private bool m_matchComp; // 11.03.20 minhan
        #endregion

        #region Constructor
        public SeqSignalTowerLampControl(ThreadSignalTowerControl control, SignalTower device)
        {
            m_Device = device;
            m_Server = m_Device.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Contorol = control;
            //m_Buzzer = eqpBuzzers._SW_Buzzer;

            m_SeqFunName = string.Format("{0} LAMP", m_Device.Name);
            m_GenInfo = GenInfoHandler.Instance;
            m_firstmatch = false;
            m_matchComp = false;
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
        protected virtual void LampConrolScenario() // 11.03.02 minhan
        {
            if (m_EqpManager.IsAlarmState)// || m_EqpManager.IsWarningState)    // dspcrassus - 111128
            {
                if (!m_firstmatch) // 11.03.20 minhan
                {
                    m_firstmatch = true;
                    m_matchComp = false;
                    m_Device.LampCommandR = Lamp.Off;
                    m_Device.LampCommandY = Lamp.Off;
                    m_Device.LampCommandG = Lamp.Off;
                    m_StartTicks = XFunc.GetTickCount();
                }
                else
                {
                    if (m_matchComp)
                    {
                        m_Device.LampCommandR = Lamp.Toggle;
                        m_Device.LampCommandY = Lamp.Toggle;
                        m_Device.LampCommandG = Lamp.Off;
                    }
                    else if (GetElapsedTicks() > 1000)
                    {
                        m_matchComp = true;
                    }
                }
            }
            else if (m_EqpManager.IsWarningState)
            {
                m_firstmatch = false;
                m_matchComp = false;
                m_Device.LampCommandR = Lamp.Off;
                m_Device.LampCommandY = Lamp.Toggle;
                m_Device.LampCommandG = Lamp.Off;
                // m_Device.LampCommandB = Lamp.Off;
            }
            else if (!m_GenInfo.AutoMode ||
                (m_GenInfo.AutoMode && !m_GenInfo.EqpInitComp))
            {
                m_firstmatch = false;
                m_matchComp = false;
                m_Device.LampCommandR = Lamp.On;
                m_Device.LampCommandY = Lamp.Off;
                m_Device.LampCommandG = Lamp.Off;
                // m_Device.LampCommandB = Lamp.Off;
            }
            else if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Idle)
            {
                m_firstmatch = false;
                m_matchComp = false;
                m_Device.LampCommandR = Lamp.Off;
                m_Device.LampCommandY = Lamp.Off;
                m_Device.LampCommandG = Lamp.Toggle;
                // m_Device.LampCommandB = Lamp.Off;
            }
            else if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Excute)
            {
                m_firstmatch = false;
                m_matchComp = false;
                m_Device.LampCommandR = Lamp.Off;
                m_Device.LampCommandY = Lamp.Off;
                m_Device.LampCommandG = Lamp.On;
                // m_Device.LampCommandB = Lamp.Off;
            }

            if (GlobalVar.LoaderReady) // 11.03.02 minhan
            {
                m_Device.LampCommandB = Lamp.On;
            }
            else
            {
                m_Device.LampCommandB = Lamp.Off;
            }

            //if (GlobalVar.BuzzerOff)
            //{
            //    m_Buzzer.MelodyAllOff();
            //    GlobalVar.BuzzerOff = false;
            //}
        }
        #endregion
    }
}
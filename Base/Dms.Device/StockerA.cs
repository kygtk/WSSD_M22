///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.11.12
// Author       : jemoon
// Description  : Interface io device for Chengdu Tianma G4.5 Stocker(Muratec)
//-------------------------------------------------------------------------
// Revison History
// * 
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class StockerA : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private bool m_IsCstDetected = false;
        [Browsable(false), XmlIgnore()]
        public bool IsCstDetected
        {
            get { return m_IsCstDetected; }
            set { m_IsCstDetected = value; }
        }

        //Stocker Interface PIO
        private IoDigitalInput m_DiValid = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiValid
        {
            get { return m_DiValid; }
            set { m_DiValid = value; }
        }

        private IoDigitalInput m_DiCs0 = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCs0
        {
            get { return m_DiCs0; }
            set { m_DiCs0 = value; }
        }

        private IoDigitalInput m_DiCs1 = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCs1
        {
            get { return m_DiCs1; }
            set { m_DiCs1 = value; }
        }

        private IoDigitalInput m_DiTrReq = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiTrReq
        {
            get { return m_DiTrReq; }
            set { m_DiTrReq = value; }
        }

        private IoDigitalInput m_DiBusy = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiBusy
        {
            get { return m_DiBusy; }
            set { m_DiBusy = value; }
        }

        private IoDigitalInput m_DiCompt = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCompt
        {
            get { return m_DiCompt; }
            set { m_DiCompt = value; }
        }

        private IoDigitalInput m_DiCont = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCont
        {
            get { return m_DiCont; }
            set { m_DiCont = value; }
        }

        private IoDigitalOutput m_DoLReq = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoLReq
        {
            get { return m_DoLReq; }
            set { m_DoLReq = value; }
        }

        private IoDigitalOutput m_DoUReq = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoUReq
        {
            get { return m_DoUReq; }
            set { m_DoUReq = value; }
        }

        private IoDigitalOutput m_DoReady = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoReady
        {
            get { return m_DoReady; }
            set { m_DoReady = value; }
        }

        private IoDigitalOutput m_DoHoAvbl = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoHoAvbl
        {
            get { return m_DoHoAvbl; }
            set { m_DoHoAvbl = value; }
        }

        private IoDigitalOutput m_DoEs = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoEs
        {
            get { return m_DoEs; }
            set { m_DoEs = value; }
        }

        //Stocker Interface PIO
        private IoDigitalOutput m_DoMioOnline = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioOnline
        {
            get { return m_DoMioOnline; }
            set { m_DoMioOnline = value; }
        }

        private IoDigitalOutput m_DoMioError = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioError
        {
            get { return m_DoMioError; }
            set { m_DoMioError = value; }
        }

        private IoDigitalOutput m_DoMioLoadPresence = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioLoadPresence
        {
            get { return m_DoMioLoadPresence; }
            set { m_DoMioLoadPresence = value; }
        }

        private IoDigitalOutput m_DoMioReadyforLoad = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioReadyforLoad
        {
            get { return m_DoMioReadyforLoad; }
            set { m_DoMioReadyforLoad = value; }
        }

        private IoDigitalOutput m_DoMioReadyforUnload = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioReadyforUnload
        {
            get { return m_DoMioReadyforUnload; }
            set { m_DoMioReadyforUnload = value; }
        }

        private IoDigitalOutput m_DoMioPowerOn = new IoDigitalOutput();
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMioPowerOn
        {
            get { return m_DoMioPowerOn; }
            set { m_DoMioPowerOn = value; }
        }
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public StockerA()
        {
            this.Name = "__ Stocker";
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }


        public override void UpdateTag()
        {
        }


        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion
            //ok &= (m_DiAlive != null);
            //ok &= (m_AiCurrentRecipeId != null);


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                ////////////////////////////////////////////////////////////////////////////////////////
                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }
        #endregion
    }


    public class SeqStokcerA_StateSinals : XSeqFunction
    {
        private IServerManager m_ServerManager;
        private _GenInfoHandler m_GenInfos;
        private StockerA m_Stocker;
        private XTimer m_TimerPowerOn;
        private const int m_TimePowerOn = 500; //500 msec

        public SeqStokcerA_StateSinals(StockerA stocker)
        {
            m_Stocker = stocker;
            m_ServerManager = m_Stocker.ServerManager;
            m_GenInfos = m_Stocker.GenInfos;
            m_TimerPowerOn = new XTimer(m_Stocker.Name, m_TimePowerOn);
        }

        public override int Do()
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //*Signal Name : Online 
            //               The Indexer PLC puts ON this signal when it can perform normal transfer in automatic mode.
            //               The Indexer PLC puts OFF this signal in any of the following conditions;
            //               - Impossible to transfer
            //               - Error
            //               - Switched to manual mode during automatic operation
            //*Signal Name : Error
            //               The Indexer PLC puts ON this signal when it has some trouble in the load/unload stage.
            //*Signal Name : Power ON
            //               The Indexer PLC puts ON and OFF this signal continuously (ON:0.5sec, OFF:0.5sec).
            //               The Gateway-PLC will watch this signal to know that the Equipment is power on or off.
            //////////////////////////////////////////////////////////////////////////////////////////////////////////// 


            if (m_GenInfos.EqpInitComp == false) return -1;

            // MIO
            {
                StockerOnlineSignal();                      // Online
                StockerErrorSignal();                       // Error
                StockerLoadPresenceSignal();                // Load Presence
                StockerReadyForLoadSignal();                // Ready For Load
                StockerReadyForUnloadSignal();              // Ready For Unload
                StockerPowerOnSignal();                     // PowerON
            }

            // PIO
            {
                StockerHoAvblSignal();                      // HoAvbl
                StockerEsSignal();                          // ES
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerOnlineSignal()
        {
            bool error = false;
            error |= m_ServerManager.EqpStateManager.IsAlarmState;
            error |= m_ServerManager.JobCond.Interlock.EmoCondition.IsAlarm;

            bool online = true;
            online &= m_GenInfos.AutoMode;
            online &= !error;

            m_Stocker.DoMioOnline.SetState(online);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerErrorSignal()
        {
            bool error = false;
            error |= m_ServerManager.EqpStateManager.IsAlarmState;
            error |= m_ServerManager.JobCond.Interlock.EmoCondition.IsAlarm;

            m_Stocker.DoMioError.SetState(error);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerLoadPresenceSignal()
        {
            if (m_Stocker.IsCstDetected && !m_Stocker.DoMioLoadPresence.GetState())
                m_Stocker.DoMioLoadPresence.SetState(true);
            else if (!m_Stocker.IsCstDetected && m_Stocker.DoMioLoadPresence.GetState())
                m_Stocker.DoMioLoadPresence.SetState(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerReadyForLoadSignal()
        {
            if (!m_Stocker.DoMioLoadPresence.GetState() && !m_Stocker.DoMioReadyforLoad.GetState())
                m_Stocker.DoMioReadyforLoad.SetState(true);
            else if (m_Stocker.DoMioLoadPresence.GetState() && m_Stocker.DoMioReadyforLoad.GetState())
                m_Stocker.DoMioReadyforLoad.SetState(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerReadyForUnloadSignal()
        {
            if (m_Stocker.DoMioLoadPresence.GetState() && !m_Stocker.DoMioReadyforUnload.GetState())
                m_Stocker.DoMioReadyforUnload.SetState(true);
            else if (!m_Stocker.DoMioLoadPresence.GetState() && m_Stocker.DoMioReadyforUnload.GetState())
                m_Stocker.DoMioReadyforUnload.SetState(false);
        }

        protected void StockerPowerOnSignal()
        {
            int nSeqNo = this.m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Stocker.DoMioPowerOn.SetState(true);
                        m_TimerPowerOn.Start();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_TimerPowerOn.Over)
                    {
                        bool curState = m_Stocker.DoMioPowerOn.GetState();
                        m_Stocker.DoMioPowerOn.SetState(!curState);
                        m_TimerPowerOn.Start();
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;
        }


        /// <summary>
        /// Stoker I/F에 사용되는 독립 sequence
        /// </summary>
        public virtual void StockerHoAvblSignal()
        {
            // * Off Condition : in any of the following conditions.
            // Cassette not placed correctly
            // Impossible to transfer
            // in Error
            // Switched to manual mode during automatic operation
            // when Es signal turn off

            bool offCondition = false;
            offCondition |= !m_GenInfos.AutoMode;
            offCondition |= !m_Stocker.DoEs.GetState();
            // 상기 상황이 추가적으로 고려 되어야 한다.

            bool stockHoAvState = m_Stocker.DoHoAvbl.GetState();
            if (!offCondition && stockHoAvState == false)
            {
                m_Stocker.DoHoAvbl.SetState(true);
            }
            else if (offCondition && stockHoAvState == true)
            {
                m_Stocker.DoHoAvbl.SetState(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StockerEsSignal()
        {
            // * Stocker I/F ES Signal control
            // for immediate stop

            bool emoState = m_ServerManager.JobCond.Interlock.EmoCondition.IsAlarm;
            bool stockEoState = m_Stocker.DoEs.GetState();

            if (emoState == true && stockEoState == true)
            {
                m_Stocker.DoEs.SetState(false);
            }
            else if (emoState == false && stockEoState == false)
            {
                m_Stocker.DoEs.SetState(true);
            }
        }
    }
}
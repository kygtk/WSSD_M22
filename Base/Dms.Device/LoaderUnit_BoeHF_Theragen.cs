using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LoaderUnit_BoeHF_Theragen : _DeviceAsm, ILoaderSlave
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private _GenericCollection<PortUnit_BoeHF_Theragen> m_Ports = new _GenericCollection<PortUnit_BoeHF_Theragen>();
        [Category("DMS : Relation")]
        public _GenericCollection<PortUnit_BoeHF_Theragen> Ports
        {
            get { return m_Ports; }
            set { m_Ports = value; }
        }
        private Yaskawa_BoeHF_Theragen m_Robot = new Yaskawa_BoeHF_Theragen();
        [Category("DMS : Relation")]
        public Yaskawa_BoeHF_Theragen Robot
        {
            get { return m_Robot; }
            set { m_Robot = value; }
        }

        private int m_InterfaceTimeout = 2000;
        [Category("DMS : General Setting"), Description("Melsec Interface Timeout Setting(msec)")]
        public int InterfaceTimeout
        {
            get { return m_InterfaceTimeout; }
            set { m_InterfaceTimeout = value; }
        }

        private bool m_LoaderViewManualEnable = false;
        [Category("DMS : General Setting"), Description("Loader View Manual Enable")]
        public bool LoaderViewManualEnable
        {
            get { return m_LoaderViewManualEnable; }
            set { m_LoaderViewManualEnable = value; }
        }

        private LoaderStatus m_Status;
        [Browsable(false), XmlIgnore()]
        public LoaderStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        private LoaderEqpState m_EqpState;
        [Browsable(false), XmlIgnore()]
        public LoaderEqpState EqpState
        {
            get { return m_EqpState; }
            set { m_EqpState = value; }
        }

        private Queue m_QueueLotStart = new Queue();
        [Browsable(false), XmlIgnore()]
        public Queue QueueLotStart
        {
            get { return m_QueueLotStart; }
            set { m_QueueLotStart = value; }
        }

        private LoaderMode m_Mode = LoaderMode.Auto;
        [Browsable(false), XmlIgnore()]
        public LoaderMode Mode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        private HostControlMode m_HostControlMode = HostControlMode.Offline;
        private LoaderControlMode m_LoaderControlMode = LoaderControlMode.Offline;
        private bool m_OpCallState = false;

        private bool m_TimeSyncRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool TimeSyncRequest
        {
            get { return m_TimeSyncRequest; }
            set { m_TimeSyncRequest = value; }
        }

        private bool m_InterlockCheckEnableRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool InterlockCheckEnableRequest
        {
            get { return m_InterlockCheckEnableRequest; }
            set { m_InterlockCheckEnableRequest = value; }
        }

        private bool m_InterlockCheckDisableRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool InterlockCheckDisableRequest
        {
            get { return m_InterlockCheckDisableRequest; }
            set { m_InterlockCheckDisableRequest = value; }
        }

        private bool m_InterlockEnable = true;
        [Browsable(false), XmlIgnore()]
        public bool InterlockEnable
        {
            get { return m_InterlockEnable; }
            set { m_InterlockEnable = value; }
        }

        private bool m_EqpRestart = false;
        [Browsable(false), XmlIgnore()]
        public bool EqpRestart
        {
            get { return m_EqpRestart; }
            set { m_EqpRestart = value; }
        }

        private short m_T3TimeoutId = 0;
        [Browsable(false), XmlIgnore()]
        public short T3TimeoutId
        {
            get { return m_T3TimeoutId; }
            set { m_T3TimeoutId = value; }
        }

        #region I/F Signals (Bit Input)
        private IoDigitalInput m_mibControlStateReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibControlStateReport
        {
            get { return m_mibControlStateReport; }
            set { m_mibControlStateReport = value; }
        }
        private IoDigitalInput m_mibLinkTestRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLinkTestRequest
        {
            get { return m_mibLinkTestRequest; }
            set { m_mibLinkTestRequest = value; }
        }
        private IoDigitalInput m_mibTimeDataRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibTimeDataRequest
        {
            get { return m_mibTimeDataRequest; }
            set { m_mibTimeDataRequest = value; }
        }
        private IoDigitalInput m_mibTimeDataReceive = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibTimeDataReceive
        {
            get { return m_mibTimeDataReceive; }
            set { m_mibTimeDataReceive = value; }
        }
        private IoDigitalInput m_mibGlassThickDataReceive = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibGlassThickDataReceive
        {
            get { return m_mibGlassThickDataReceive; }
            set { m_mibGlassThickDataReceive = value; }
        }
        private IoDigitalInput m_mibInterlockCheckEnableConfirm = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibInterlockCheckEnableConfirm
        {
            get { return m_mibInterlockCheckEnableConfirm; }
            set { m_mibInterlockCheckEnableConfirm = value; }
        }
        private IoDigitalInput m_mibInterlockCheckDisableConfirm = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibInterlockCheckDisableConfirm
        {
            get { return m_mibInterlockCheckDisableConfirm; }
            set { m_mibInterlockCheckDisableConfirm = value; }
        }
        private IoDigitalInput m_mibT3Timeout = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibT3Timeout
        {
            get { return m_mibT3Timeout; }
            set { m_mibT3Timeout = value; }
        }
        private IoDigitalInput m_mibAlarmOccurReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibAlarmOccurReport
        {
            get { return m_mibAlarmOccurReport; }
            set { m_mibAlarmOccurReport = value; }
        }
        private IoDigitalInput m_mibAlarmReleaseReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibAlarmReleaseReport
        {
            get { return m_mibAlarmReleaseReport; }
            set { m_mibAlarmReleaseReport = value; }
        }
        private IoDigitalInput m_mibControlStateOnline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibControlStateOnline
        {
            get { return m_mibControlStateOnline; }
            set { m_mibControlStateOnline = value; }
        }
        private IoDigitalInput m_mibControlStateOffline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibControlStateOffline
        {
            get { return m_mibControlStateOffline; }
            set { m_mibControlStateOffline = value; }
        }
        #endregion

        #region I/F Signals (Bit Output)
        private IoDigitalOutput m_mobControlStateConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobControlStateConfirm
        {
            get { return m_mobControlStateConfirm; }
            set { m_mobControlStateConfirm = value; }
        }
        private IoDigitalOutput m_mobLinkTestResponse = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobLinkTestResponse
        {
            get { return m_mobLinkTestResponse; }
            set { m_mobLinkTestResponse = value; }
        }
        private IoDigitalOutput m_mobTimeDataConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobTimeDataConfirm
        {
            get { return m_mobTimeDataConfirm; }
            set { m_mobTimeDataConfirm = value; }
        }
        private IoDigitalOutput m_mobTimeDataSend = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobTimeDataSend
        {
            get { return m_mobTimeDataSend; }
            set { m_mobTimeDataSend = value; }
        }
        private IoDigitalOutput m_mobGlassThickDataSend = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobGlassThickDataSend
        {
            get { return m_mobGlassThickDataSend; }
            set { m_mobGlassThickDataSend = value; }
        }
        private IoDigitalOutput m_mobVcrReadConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobVcrReadConfirm
        {
            get { return m_mobVcrReadConfirm; }
            set { m_mobVcrReadConfirm = value; }
        }
        private IoDigitalOutput m_mobInterlockCheckEnableRequest = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobInterlockCheckEnableRequest
        {
            get { return m_mobInterlockCheckEnableRequest; }
            set { m_mobInterlockCheckEnableRequest = value; }
        }
        private IoDigitalOutput m_mobInterlockCheckDisableRequest = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobInterlockCheckDisableRequest
        {
            get { return m_mobInterlockCheckDisableRequest; }
            set { m_mobInterlockCheckDisableRequest = value; }
        }
        private IoDigitalOutput m_mobAlarmOccurConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobAlarmOccurConfirm
        {
            get { return m_mobAlarmOccurConfirm; }
            set { m_mobAlarmOccurConfirm = value; }
        }
        private IoDigitalOutput m_mobAlarmReleaseConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobAlarmReleaseConfirm
        {
            get { return m_mobAlarmReleaseConfirm; }
            set { m_mobAlarmReleaseConfirm = value; }
        }
        private IoDigitalOutput m_mobEQPRestart = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobEQPRestart
        {
            get { return m_mobEQPRestart; }
            set { m_mobEQPRestart = value; }
        }
        #endregion

        #region I/F Signals (Word Input)
        private IoAnalogInput m_miwT3Timeout = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwT3Timeout
        {
            get { return m_miwT3Timeout; }
            set { m_miwT3Timeout = value; }
        }
        private IoAnalogInput m_miwOccurAlarmId = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwOccurAlarmId
        {
            get { return m_miwOccurAlarmId; }
            set { m_miwOccurAlarmId = value; }
        }
        private IoAnalogInput m_miwReleaseAlarmId = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwReleaseAlarmId
        {
            get { return m_miwReleaseAlarmId; }
            set { m_miwReleaseAlarmId = value; }
        }
        #endregion

        #region I/F Signals (Word Output)
        private IoAnalogOutput m_mowTimeDataYear = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataYear
        {
            get { return m_mowTimeDataYear; }
            set { m_mowTimeDataYear = value; }
        }
        private IoAnalogOutput m_mowTimeDataMonth = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataMonth
        {
            get { return m_mowTimeDataMonth; }
            set { m_mowTimeDataMonth = value; }
        }
        private IoAnalogOutput m_mowTimeDataDay = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataDay
        {
            get { return m_mowTimeDataDay; }
            set { m_mowTimeDataDay = value; }
        }
        private IoAnalogOutput m_mowTimeDataHour = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataHour
        {
            get { return m_mowTimeDataHour; }
            set { m_mowTimeDataHour = value; }
        }
        private IoAnalogOutput m_mowTimeDataMinute = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataMinute
        {
            get { return m_mowTimeDataMinute; }
            set { m_mowTimeDataMinute = value; }
        }
        private IoAnalogOutput m_mowTimeDataSecond = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTimeDataSecond
        {
            get { return m_mowTimeDataSecond; }
            set { m_mowTimeDataSecond = value; }
        }
        private IoAnalogOutput m_mowGlassThickPortNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowGlassThickPortNo
        {
            get { return m_mowGlassThickPortNo; }
            set { m_mowGlassThickPortNo = value; }
        }
        private IoAnalogOutput m_mowGlassThickValue = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowGlassThickValue
        {
            get { return m_mowGlassThickValue; }
            set { m_mowGlassThickValue = value; }
        }
        #endregion

        #region I/F Signals (Handshake Bit Input)
        private IoDigitalInput m_mibIFEqpNormalStatus = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpNormalStatus
        {
            get { return m_mibIFEqpNormalStatus; }
            set { m_mibIFEqpNormalStatus = value; }
        }
        private IoDigitalInput m_mibIFEqpGetEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpGetEnable
        {
            get { return m_mibIFEqpGetEnable; }
            set { m_mibIFEqpGetEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpPutEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpPutEnable
        {
            get { return m_mibIFEqpPutEnable; }
            set { m_mibIFEqpPutEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpExchangeEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpExchangeEnable
        {
            get { return m_mibIFEqpExchangeEnable; }
            set { m_mibIFEqpExchangeEnable = value; }
        }
        private IoDigitalInput m_mibIFEqpGlassExist = new IoDigitalInput();
        [Category("DMS : I/O Setting (EQP Handshake Bit Input)")]
        public IoDigitalInput mibIFEqpGlassExist
        {
            get { return m_mibIFEqpGlassExist; }
            set { m_mibIFEqpGlassExist = value; }
        }
        private IoDigitalInput m_mibIFNormalStatus = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFNormalStatus
        {
            get { return m_mibIFNormalStatus; }
            set { m_mibIFNormalStatus = value; }
        }
        private IoDigitalInput m_mibIFGetReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFGetReady
        {
            get { return m_mibIFGetReady; }
            set { m_mibIFGetReady = value; }
        }
        private IoDigitalInput m_mibIFPutReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFPutReady
        {
            get { return m_mibIFPutReady; }
            set { m_mibIFPutReady = value; }
        }
        private IoDigitalInput m_mibIFExchangeReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFExchangeReady
        {
            get { return m_mibIFExchangeReady; }
            set { m_mibIFExchangeReady = value; }
        }
        private IoDigitalInput m_mibIFRobotBusy = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFRobotBusy
        {
            get { return m_mibIFRobotBusy; }
            set { m_mibIFRobotBusy = value; }
        }
        private IoDigitalInput m_mibIFRobotComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Indexer Handshake Bit Input)")]
        public IoDigitalInput mibIFRobotComplete
        {
            get { return m_mibIFRobotComplete; }
            set { m_mibIFRobotComplete = value; }
        }
        #endregion

        #region I/F Signals (Handshake Bit Output)
        #endregion
        #endregion

        #region Constructor
        public LoaderUnit_BoeHF_Theragen()
        {
            this.Name = "__ Loader";
        }
        #endregion

        #region Methods
        public int GetFirstWaitingPort()
        {
            if (m_QueueLotStart.Count > 0)
                return (int)m_QueueLotStart.Peek();
            return -1;
        }
        #endregion

        #region Override
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
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
                    m_Robot.OnRobotTransferEvent += new RobotTransferEventHandler(OnRobotTransferEvent);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override Type FamilyType
        {
            get
            {
                return this.GetType();
            }
        }
        #endregion

        #region ILoaderSlave 멤버
        public string GetRecipeId(int portId, int slotId)
        {
            return m_Ports[portId].Cst.GetRecipeId(slotId);
        }

        public LoaderCommandReply SetGlassRecipeId(int portId, string[] recipe)
        {
            int slotCount = m_Ports[portId].Cst.SlotCount;
            if (portId >= m_Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            if (recipe.Length != slotCount)
            {
                MessageBox.Show("Bug!! slot no range over");
                return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < slotCount; i++)
            {
                m_Ports[portId].Cst.SetRecipeId(i, recipe[i]);
            }

            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetPortEnable(int portId, PortUsage use)
        {
            string text = "Bug Report";
            string caption = "Error";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Error;
            MessageBoxDefaultButton defaultButtons = MessageBoxDefaultButton.Button1;
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly;

            if (portId >= m_Ports.Count || portId < 0)
            {
                text = "BUG!! Port No. range over";
                MessageBox.Show(text, caption, buttons, icons, defaultButtons, options);
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].PortEnable = use;

            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply AbortConfirm(int portNo)
        {
            return LoaderCommandReply.Ack;
        }

        public int GetPortCount()
        {
            return m_Ports.Count;
        }

        public string GetCstId(int portId)
        {
            string cstid = "";

            if (m_Ports[portId].Cst.CstID == null)
            {
            }
            else
            {
                cstid = m_Ports[portId].Cst.CstID;
            }

            return cstid;
        }

        public GlassStatus GetGlassStatus(int portId, int slotId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return GlassStatus.Empty;
            }
            if (slotId >= Ports[portId].Cst.SlotCount || slotId < 0)
            {
                MessageBox.Show("Bug!! slot no range over");
                return GlassStatus.Empty;
            }
            return m_Ports[portId].Cst.GetGlassStatus(slotId);
        }

        public LoaderGlass GetLoaderGlass(int portId, int slotId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return null;
            }
            if (slotId >= Ports[portId].Cst.Glasses.Length || slotId < 0)
            {
                MessageBox.Show("Bug!! slot no range over");
                return null;
            }

            return Ports[portId].Cst.Glasses[slotId];
        }

        public LoaderStatus GetLoaderStatus()
        {
            return m_Status;
        }

        public LoaderEqpState GetLoaderEqpState()
        {
            if (m_Server.EqpStateManager.EqpUnit.EqpState == Common.EqpState.Fault)
            {
                if (m_EqpState != LoaderEqpState.Down)
                {
                    m_EqpState = LoaderEqpState.Down;
                }
            }
            else
            {
                if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Excute)
                {
                    if (m_EqpState != LoaderEqpState.Run)
                    {
                        m_EqpState = LoaderEqpState.Run;
                    }
                }
                else
                {
                    if (m_EqpState != LoaderEqpState.Idle)
                    {
                        m_EqpState = LoaderEqpState.Idle;
                    }
                }
            }

            return m_EqpState;
        }

        public MappingStatus[] GetMappingData(int portId)
        {
            MappingStatus[] status = new MappingStatus[m_Ports[portId].Cst.SlotCount];
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return status;
            }
            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                status[i] = m_Ports[portId].Cst.Glasses[i].GlassMappingStatus;
            }
            return status;
        }

        public LoaderGlass GetNextOutputGlass()
        {
            //if (m_QueueLotStart.Count > 0 && m_QueueLotStart.Peek() != null)
            if (GetFirstWaitingPort() >= 0)
            {
                int portId = (int)m_QueueLotStart.Peek();
                PortUnit_BoeHF_Theragen port = m_Ports[portId];
                int slotCnt = port.Cst.SlotCount;
                if (port.Cst.SlotOrder == CstSlotOrder.Increase)
                {
                    for (int i = 0; i < slotCnt; i++)
                    {
                        if (port.Cst.Glasses[i].Status == GlassStatus.Wait)
                        {
                            return port.Cst.Glasses[i];
                        }
                    }
                }
                else
                {
                    for (int i = slotCnt - 1; i >= 0; i--)
                    {
                        if (port.Cst.Glasses[i].Status == GlassStatus.Wait)
                        {
                            return port.Cst.Glasses[i];
                        }
                    }
                }

                //Wait glass가 더이상 없다면
                m_QueueLotStart.Dequeue();
                return null;
            }
            else return null;
        }

        public GlassStatus[] GetPortGlassStatus(int portId)
        {
            int slotCnt = m_Ports[portId].Cst.SlotCount;
            GlassStatus[] status = new GlassStatus[slotCnt];
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return status;
            }
            PortUnit_BoeHF_Theragen port = m_Ports[portId];
            for (int i = 0; i < slotCnt; i++)
            {
                status[i] = port.Cst.Glasses[i].Status;
            }
            return status;
        }

        public PortStatus GetPortStatus(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return PortStatus.None;
            }
            return m_Ports[portId].PortStatus;
        }

        public PortTransferMode GetTransferMode(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return PortTransferMode.None;
            }
            return m_Ports[portId].TransferMode;
        }

        public PortCommand GetPortCommand(int portId)
        {
            return m_Ports[portId].PortCommand;
        }

        public int CountOfProcessGlassStatus(int portId)
        {
            int count = 0;
            int slotCnt = m_Ports[portId].Cst.SlotCount;

            PortUnit_BoeHF_Theragen port = m_Ports[portId];
            for (int i = 0; i < slotCnt; i++)
            {
                GlassStatus status = port.Cst.Glasses[i].Status;

                if (status == GlassStatus.Selected ||
                    status == GlassStatus.Wait ||
                    status == GlassStatus.Started ||
                    status == GlassStatus.Proc ||
                    status == GlassStatus.Ok)
                {
                    count += 1;
                }

            }
            return count;
        }

        public bool IsPortCommand()
        {
            bool IsPortCommand = false;

            foreach (PortUnit_BoeHF_Theragen port in m_Ports.Items)
            {
                if (port.InProcess)
                {
                    IsPortCommand = true;
                    break;
                }
            }

            return IsPortCommand;
        }

        public bool IsAreaSensorDetected(int portId)
        {
            return false;
        }

        public bool IsPortPassable(int portId)
        {
            //            bool rv = Ports[portId].IsCstClampPos();
            //            rv &= Ports[portId].MappingUnit.IsMappingDriveUnitBw();
            //            rv |= !Ports[portId].IsCstExist();
            //            rv &= !Ports[portId].DiCstOppositeDetect.GetState();

            //            return rv;
            return false;
        }

        public bool IsCassetteExist(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return false;
            }
            return Ports[portId].IsCstExist();
        }

        public bool IsDoorOpen()
        {
            //return m_LoaderDoorSensor.IsDetected();
            return false;
        }

        public bool IsGlassOkInputToCst(int portId, int slotId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                //                MessageBox.Show("Bug!! port no range over");
                return false;
            }

            if (m_Ports[portId].Cst.Glasses[slotId].Status == GlassStatus.Proc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLoaderAutoMode()
        {
            if (m_Mode == LoaderMode.Auto) return true;
            else return false;
        }

        public bool IsPortEnable(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return false;
            }
            if (m_Ports[portId].PortEnable == PortUsage.Use) return true;
            else return false;
        }

        public bool IsPortProcess()
        {
            bool processing = false;

            foreach (PortUnit_BoeHF_Theragen port in m_Ports.Items)
            {
                if (port.PortStatus == PortStatus.Wait ||
                    port.PortStatus == PortStatus.InProcess)
                {
                    processing = true;
                    break;
                }
            }

            return processing;
        }

        public bool IsPortError(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return false;
            }
            if (m_Ports[portId].PortError) return true;
            else return false;
        }

        public LoaderCommandReply LotAbortRequest(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            m_Ports[portId].LotAbortSet();
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotCancelRequest(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            m_Ports[portId].LotCancelSet();
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotEndRequest(int portId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }

            if (m_Ports[portId].PortStatus == PortStatus.InProcess ||
                m_Ports[portId].PortStatus == PortStatus.Aborting) { }  // Abort일때는 Lot End 명령을 받을 수 있어야 함.
            else return LoaderCommandReply.Nak;

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Proc ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Wait ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Selected ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Started ||
                    m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Canceled)
                    return LoaderCommandReply.Nak;
            }

            if (m_Ports[portId].PortStatus == PortStatus.Aborting) m_Ports[portId].PortStatus = PortStatus.Abort;
            else m_Ports[portId].PortStatus = PortStatus.ProcessEnd;
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply LotStartRequest(int portId)
        {
            // 먼저 해당 Port에 cst가 있는지, 그 안의 glass들이 준비가 되어 있는지 Check한다.
            // 그 다음 Queue에 port order를 정하고 glass 상태들을 wait로 변경한다.

            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            PortUnit_BoeHF_Theragen port = Ports[portId];
            if (port.IsCstExist() == false ||
                port.Cst == null) return LoaderCommandReply.Nak;

            if (m_QueueLotStart.Contains(portId)) return LoaderCommandReply.Nak;
            for (int i = 0; i < port.Cst.SlotCount; i++)
            {
                if (port.Cst.Glasses[i].Status == GlassStatus.Aborted ||
                    port.Cst.Glasses[i].Status == GlassStatus.Canceled ||
                    port.Cst.Glasses[i].Status == GlassStatus.Proc ||
                    port.Cst.Glasses[i].Status == GlassStatus.Scraped)
                    return LoaderCommandReply.Nak;
            }

            port.LotStartSet();

            m_QueueLotStart.Enqueue(portId);

            return LoaderCommandReply.Ack;
        }

        #region Port Command
        public LoaderCommandReply CstIdReadRequest(int portNo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoaderCommandReply MappingRequest(int portId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoaderCommandReply ReChuckingRequest(int portId)
        {
            if (m_Ports[portId].PortStatus != PortStatus.UnloadRequest ||
                m_Ports[portId].TransferMode == PortTransferMode.AGV ||
                m_HostControlMode != HostControlMode.Offline)
            {
                m_Ports[portId].PortCommand = PortCommand.None;

                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].PortCommand = PortCommand.ReChuckingRequest;

            return LoaderCommandReply.Ack;
        }
        #endregion

        public void SetCstId(int portId, string cstId)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return;
            }
            m_Ports[portId].Cst.CstID = cstId;
        }

        public LoaderCommandReply SetGlassSelectData(int portId, SelectStatus[] status)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            if (status.Length != Ports[portId].Cst.Glasses.Length)
            {
                MessageBox.Show("Bug!! slot no range over");
                return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                if (m_Ports[portId].Cst.Glasses[i].GlassMappingStatus == MappingStatus.Off &&
                    status[i] == SelectStatus.Selected)
                    return LoaderCommandReply.Nak;
            }

            for (int i = 0; i < m_Ports[portId].Cst.SlotCount; i++)
            {
                m_Ports[portId].Cst.Glasses[i].GlassSelectStatus = status[i];

                if (status[i] == SelectStatus.Selected)
                {
                    m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Selected;
                    m_Ports[portId].Cst.Glasses[i].OrgPortNo = portId + 1;
                    m_Ports[portId].Cst.Glasses[i].OrgSlotNo = i + 1;
                    m_Ports[portId].Cst.Glasses[i].TargetPortNo = portId + 1;
                    m_Ports[portId].Cst.Glasses[i].TargetSlotNo = i + 1;
                }
                else
                {
                    if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Exist)
                    {
                        m_Ports[portId].Cst.Glasses[i].OrgPortNo = portId + 1;
                        m_Ports[portId].Cst.Glasses[i].OrgSlotNo = i + 1;
                        m_Ports[portId].Cst.Glasses[i].TargetPortNo = portId + 1;
                        m_Ports[portId].Cst.Glasses[i].TargetSlotNo = i + 1;
                    }
                    else if (m_Ports[portId].Cst.Glasses[i].Status == GlassStatus.Selected)
                    {
                        m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Exist;
                    }
                    else
                        m_Ports[portId].Cst.Glasses[i].Status = GlassStatus.Empty;
                }

            }

            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetGlassStatus(int portId, int slotId, GlassStatus status)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }
            if (slotId >= Ports[portId].Cst.SlotCount || slotId < 0)
            {
                MessageBox.Show("Bug!! slot no range over");
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].Cst.SetGlassStatus(slotId, status);
            return LoaderCommandReply.Ack;
        }

        public LoaderCommandReply SetLoaderReady()
        {
            throw new Exception("The method or operation is not implemented.");

            // Port와 Robot을 초기화 하여야 한다. 
            // 11.09 이함수는 사용안하기로 협의 홍/구
        }

        public LoaderCommandReply SetTransferMode(int portId, PortTransferMode mode)
        {
            if (portId >= Ports.Count || portId < 0)
            {
                MessageBox.Show("Bug!! port no range over");
                return LoaderCommandReply.Nak;
            }

            m_Ports[portId].TransferMode = mode; // /////////////////////////////////////////// < Need to modify
            return LoaderCommandReply.Nak;
        }

        public event RobotEventHandler OnRobotEvent;

        public void FireRobotEvent(int portId, int slotId, RobotActionType action)
        {
            if (OnRobotEvent != null)
            {
                OnRobotEvent(portId, slotId, action);
            }
        }

        public void OnRobotTransferEvent(RobotActionType act, int portId, int slotId)
        {
            switch (act)
            {
                case RobotActionType.Get:
                    {
                        bool fromCst = portId < m_Ports.Count;
                        if (fromCst)
                        {
                            if (m_Ports[portId].PortStatus != PortStatus.Aborting) m_Ports[portId].PortStatus = PortStatus.InProcess;

                            //Event Fire
                            FireRobotEvent(portId, slotId, act);
                        }
                    }
                    break;
                case RobotActionType.Put:
                    {
                        bool intoCst = portId < m_Ports.Count;
                        if (intoCst)
                        {
                            //Event Fire
                            FireRobotEvent(portId, slotId, act);
                        }
                    }
                    break;
            }
        }

        public void SetHostControlMode(HostControlMode mode)
        {
            m_HostControlMode = mode;
        }

        public HostControlMode GetHostControlMode()
        {
            return m_HostControlMode;
        }

        public void SetLoaderControlMode(LoaderControlMode mode)
        {
            m_LoaderControlMode = mode;
        }

        public LoaderControlMode GetLoaderControlMode()
        {
            return m_LoaderControlMode;
        }

        public void SetOpCallState(bool call)
        {
            m_OpCallState = call;
        }

        public bool GetOpCallState()
        {
            return m_OpCallState;
        }

        //        public void SetTimeSyncReqeust(bool sync)
        //        {
        //            m_TimeSyncRequest = sync;
        //        }

        //        public bool GetTimeSyncRequest()
        //        {
        //            return m_TimeSyncRequest;
        //        }

        //        public void SetInterlockChangeRequest(bool change)
        //        {
        //            m_InterlockCheckChangeRequest = change;
        //        }

        //        public bool GetInterlockChangeRequest()
        //        {
        //            return m_InterlockCheckChangeRequest;
        //        }

        //        public void SetInterlockEnable(bool enable)
        //        {
        //            m_InterlockEnable = enable;
        //        }

        //        public bool GetInterlockEnable()
        //        {
        //            return m_InterlockEnable;
        //        }

        //        public void SetEqpRestart(bool change)
        //        {
        //            m_EqpRestart = change;
        //        }

        //        public bool GetEqpRestart()
        //        {
        //            return m_EqpRestart;
        //        }
        #endregion
    }

    /*
        public class LoaderUnit_BoeHF_Theragen_LinkTestSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_LinkTestSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if (m_LoaderUnit.mibLinkTestRequest.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "LINKTEST", 0, 0, "mibLinkTestRequest ON");

                            m_LoaderUnit.mobLinkTestResponse.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "LINKTEST", 0, 0, "mobLinkTestResponse ON");

                            seqNo = 10;
                        }
                        break;
                    case 10:
                        if (m_LoaderUnit.mibLinkTestRequest.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mibLinkTestRequest OFF");

                            m_LoaderUnit.mobLinkTestResponse.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mobLinkTestResponse OFF");

                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }
    */
    /*
        public class LoaderUnit_BoeHF_Theragen_ControlStateChangeSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_ControlStateChangeSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if(m_LoaderUnit.mibControlStateReport.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mibControlStateReport ON");

                            if ((m_LoaderUnit.mibControlStateOffline.GetState() == true) &&
                               (m_LoaderUnit.mibControlStateOnline.GetState() == false))
                            {
                                m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mibControlStateOnline OFF / mibControlStateOffline ON");
                                m_LoaderUnit.SetLoaderControlMode(LoaderControlMode.Offline);
                            }
                            else if ((m_LoaderUnit.mibControlStateOffline.GetState() == false) &&
                                    (m_LoaderUnit.mibControlStateOnline.GetState() == true))
                            {
                                m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mibControlStateOnline ON / mibControlStateOffline OFF");
                                m_LoaderUnit.SetLoaderControlMode(LoaderControlMode.Online);
                            }
                            else
                            {
                                m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "Bit On/Off Duplication Fault.");
                                break;
                            }
                            m_LoaderUnit.mobControlStateConfirm.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mobControlStateConfirm ON");

                            seqNo = 10;
                        }
                        break;
                    case 10:
                        if (m_LoaderUnit.mibControlStateReport.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mibControlStateReport OFF");

                            m_LoaderUnit.mobControlStateConfirm.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "CTLSTATUS", 0, 0, "mobControlStateConfirm OFF");

                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }
    */
    /*
        public class LoaderUnit_BoeHF_Theragen_TimeSyncSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_TimeSyncSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;

            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if (m_LoaderUnit.GetTimeSyncRequest() == true)
                        {
                            m_LoaderUnit.SetTimeSyncReqeust(false);

                            DateTime time = SetTimeData();

                            string log = string.Format("Sync Time : {0}", time.ToString("yyyy-MM-dd HH:mm:ss"));
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, log);

                            m_LoaderUnit.mobTimeDataSend.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mobTimeDataSend ON");

                            this.m_StartTicks = XFunc.GetTickCount();

                            seqNo = 100;
                        }
                        else if (m_LoaderUnit.mibTimeDataRequest.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mibTimeDataRequest ON");

                            DateTime time = SetTimeData();

                            string log = string.Format("Sync Time : {0}", time.ToString("yyyy-MM-dd HH:mm:ss"));
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, log);

                            m_LoaderUnit.mobTimeDataConfirm.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mobTimeDataConfirm ON");

                            seqNo = 200;
                        }
                        break;
                    case 100:
                        if (m_LoaderUnit.mibTimeDataReceive.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mibTimeDataReceive ON");

                            m_LoaderUnit.mobTimeDataSend.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mobTimeDataSend OFF");

                            seqNo = 110;
                        }
                        else if (GetElapsedTicks() > m_LoaderUnit.InterfaceTimeout)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mibTimeDataReceive ON Timeout");

                            m_LoaderUnit.mobTimeDataSend.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mobTimeDataSend OFF");

                            returnValue = 1;
                            seqNo = 0;
                        }
                        break;
                    case 110:
                        if (m_LoaderUnit.mibTimeDataReceive.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mibTimeDataReceive ON");

                            seqNo = 0;
                        }
                        break;
                    case 200:
                        if (m_LoaderUnit.mibTimeDataRequest.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mibTimeDataRequest OFF");

                            m_LoaderUnit.mobTimeDataConfirm.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "TIMESYNC", 0, 0, "mobTimeDataConfirm OFF");

                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion

            #region Methods
            private DateTime SetTimeData()
            {
                DateTime time = DateTime.Now;

                m_LoaderUnit.mowTimeDataYear.SetState((ushort)time.Year);
                m_LoaderUnit.mowTimeDataMonth.SetState((ushort)time.Month);
                m_LoaderUnit.mowTimeDataDay.SetState((ushort)time.Day);
                m_LoaderUnit.mowTimeDataHour.SetState((ushort)time.Hour);
                m_LoaderUnit.mowTimeDataMinute.SetState((ushort)time.Minute);
                m_LoaderUnit.mowTimeDataSecond.SetState((ushort)time.Second);

                return time;
            }

            private void SetTimeData(DateTime time)
            {
                m_LoaderUnit.mowTimeDataYear.SetState((ushort)time.Year);
                m_LoaderUnit.mowTimeDataMonth.SetState((ushort)time.Month);
                m_LoaderUnit.mowTimeDataDay.SetState((ushort)time.Day);
                m_LoaderUnit.mowTimeDataHour.SetState((ushort)time.Hour);
                m_LoaderUnit.mowTimeDataMinute.SetState((ushort)time.Minute);
                m_LoaderUnit.mowTimeDataSecond.SetState((ushort)time.Second);
            }
            #endregion
        }
    */
    /*
        public class LoaderUnit_BoeHF_Theragen_InterlockEnalbeSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            private bool m_InterlockCheckEnable = false;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_InterlockEnalbeSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Methods
            public void SetInterlockCheck(bool enable)
            {
                m_InterlockCheckEnable = enable;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if (m_InterlockCheckEnable == true)
                        {
                            m_LoaderUnit.mobInterlockCheckEnableRequest.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckEnable ON");

                            this.m_StartTicks = XFunc.GetTickCount();
                            seqNo = 100;
                        }
                        else if (m_InterlockCheckEnable == false)
                        {
                            m_LoaderUnit.mobInterlockCheckDisableRequest.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckDisable ON");

                            this.m_StartTicks = XFunc.GetTickCount();

                            seqNo = 200;
                        }
                        break;
                    case 100:
                        if (m_LoaderUnit.mibInterlockCheckEnableConfirm.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mibInterlockCheckEnableConfirm ON");

                            m_LoaderUnit.mobInterlockCheckEnableRequest.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckEnableRequest OFF");

                            returnValue = 0;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > m_LoaderUnit.InterfaceTimeout)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mibInterlockCheckEnableConfirm ON Timeout");

                            m_LoaderUnit.mobInterlockCheckEnableRequest.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckEnableRequest OFF");

                            returnValue = 1;
                            seqNo = 0;
                        }
                        break;
                    case 200:
                        if (m_LoaderUnit.mibInterlockCheckDisableConfirm.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mibInterlockCheckDisableConfirm ON");

                            m_LoaderUnit.mobInterlockCheckDisableRequest.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckDisableRequest OFF");

                            returnValue = 0;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > m_LoaderUnit.InterfaceTimeout)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mibInterlockCheckDisableConfirm ON Timeout");

                            m_LoaderUnit.mobInterlockCheckDisableRequest.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobInterlockCheckDisableRequest OFF");

                            returnValue = 1;
                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }
    */
    /*
        public class LoaderUnit_BoeHF_Theragen_EqpRestartSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_EqpRestartSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        {
                            m_LoaderUnit.mobEQRestart.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobEQRestart ON");

                            this.m_StartTicks = XFunc.GetTickCount();
                            seqNo = 10;
                        }
                        break;
                    case 10:
                        if (GetElapsedTicks() > 2000)
                        {
                            m_LoaderUnit.mobEQRestart.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "INTERLOCK", 0, 0, "mobEQRestart OFF");

                            returnValue = 0;
                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }
    */
    /*
        public class LoaderUnit_BoeHF_Theragen_AlarmSetSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            private int m_AlarmId;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_AlarmSetSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if( m_LoaderUnit.mibAlarmOccurReport.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM SET", 0, 0, "mibAlarmOccurReport ON");

                            m_AlarmId = (int)m_LoaderUnit.miwOccurAlarmId.GetState();

                            m_LoaderUnit.mobAlarmOccurConfirm.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM SET", 0, 0, "mobAlarmOccurConfirm ON");

                            seqNo = 10;
                        }
                        break;
                    case 10:
                        if (m_LoaderUnit.mibAlarmOccurReport.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM SET", 0, 0, "mibAlarmOccurReport OFF");

                            m_LoaderUnit.mobAlarmOccurConfirm.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM SET", 0, 0, "mobAlarmOccurConfirm OFF");

                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }

        public class LoaderUnit_BoeHF_Theragen_AlarmResetSeq : XSeqFunction
        {
            #region Fields
            private LoaderUnit_BoeHF_Theragen m_LoaderUnit;
            private int m_AlarmId;
            #endregion

            #region Constructor
            public LoaderUnit_BoeHF_Theragen_AlarmResetSeq(LoaderUnit_BoeHF_Theragen loaderUnit)
            {
                m_LoaderUnit = loaderUnit;
            }
            #endregion

            #region Override
            public override int Do()
            {
                int returnValue = -1;
                int seqNo = this.SeqNo;

                switch (seqNo)
                {
                    case 0:
                        if (m_LoaderUnit.mibAlarmReleaseReport.GetState() == true)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM RESET", 0, 0, "mibAlarmReleaseReport ON");

                            m_AlarmId = (int)m_LoaderUnit.miwReleaseAlarmId.GetState();

                            m_LoaderUnit.mobAlarmReleaseConfirm.SetState(true);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM RESET", 0, 0, "mobAlarmReleaseConfirm ON");

                            seqNo = 10;
                        }
                        break;
                    case 10:
                        if (m_LoaderUnit.mibAlarmReleaseReport.GetState() == false)
                        {
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM RESET", 0, 0, "mibAlarmReleaseReport OFF");

                            m_LoaderUnit.mobAlarmReleaseConfirm.SetState(false);
                            m_LoaderUnit.SetLog(m_LoaderUnit.Name, "ALARM RESET", 0, 0, "mobAlarmReleaseConfirm OFF");

                            seqNo = 0;
                        }
                        break;
                }

                this.SeqNo = seqNo;
                return returnValue;
            }
            #endregion
        }
    */
}

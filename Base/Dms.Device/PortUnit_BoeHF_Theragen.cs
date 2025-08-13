///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.02.18
// Author       : Kim Youngsik
// Description  : PortUnit for BOE-HF G6 
///////////////////////////////////////////////////////////////////////////
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
using System.Threading;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PortUnit_BoeHF_Theragen : PortUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        protected static Mutex m_Mutex = new Mutex();
        private static bool m_InProcess = false;
        private static bool m_CstThickenssProcess = false;
        #endregion

        #region Fields
        #region General Fields
        private int m_PortNo = 1;
        [Category("DMS : General Setting")]
        public int PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        private int m_MaxSlotCount = 30;
        [Category("DMS : General Setting")]
        public int MaxSlotCount
        {
            get { return m_MaxSlotCount; }
            set { m_MaxSlotCount = value; }
        }
        private int m_PortCommandEndIFTimeout = 30000;
        [Category("DMS : General Setting"), Description("Port Command End Interface Timeout Setting(msec)")]
        public int PortCommandEndIFTimeout
        {
            get { return m_PortCommandEndIFTimeout; }
            set { m_PortCommandEndIFTimeout = value; }
        }

        private int m_InterfaceTimeout = 5000;
        [Category("DMS : General Setting"), Description("Melsec Interface Timeout Setting(msec)")]
        public int InterfaceTimeout
        {
            get { return m_InterfaceTimeout; }
            set { m_InterfaceTimeout = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool InProcess
        {
            get { return m_InProcess; }
            set
            {
                m_Mutex.WaitOne();
                m_InProcess = value;
                m_Mutex.ReleaseMutex();
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool CstThickenssProcess
        {
            get { return m_CstThickenssProcess; }
            set
            {
                m_CstThickenssProcess = value;
            }
        }

        private bool m_ManaualPortCommandReq;
        [Browsable(false), XmlIgnore()]
        public bool ManaualPortCommandReq
        {
            get { return m_ManaualPortCommandReq; }
            set { m_ManaualPortCommandReq = value; }
        }

        private PortCommandCode m_ManaualPortCommand;
        [Browsable(false), XmlIgnore()]
        public PortCommandCode ManaualPortCommand
        {
            get { return m_ManaualPortCommand; }
            set { m_ManaualPortCommand = value; }
        }

        private ThicknessKind m_ManaualThicknessValue;
        [Browsable(false), XmlIgnore()]
        public ThicknessKind ManaualThicknessValue
        {
            get { return m_ManaualThicknessValue; }
            set { m_ManaualThicknessValue = value; }
        }

        private ThicknessKind m_Thickness = ThicknessKind.T070;
        [Browsable(false), XmlIgnore()]
        public ThicknessKind Thickness
        {
            get { return m_Thickness; }
            set { m_Thickness = value; }
        }

        private PortStatus m_OldPortStatus;
        [Browsable(false), XmlIgnore()]
        public PortStatus OldPortStatus
        {
            get { return m_OldPortStatus; }
            set { m_OldPortStatus = value; }
        }

        private bool m_IsLoaderOnline = false;
        [Browsable(false), XmlIgnore()]
        public bool IsLoaderOnline
        {
            get { return m_IsLoaderOnline; }
            set { m_IsLoaderOnline = value; }
        }

        private bool m_InitSeqCstLoad = false;
        [Browsable(false), XmlIgnore()]
        public bool InitSeqCstLoad
        {
            get { return m_InitSeqCstLoad; }
            set { m_InitSeqCstLoad = value; }
        }

        private bool m_InitSeqCstUnload = false;
        [Browsable(false), XmlIgnore()]
        public bool InitSeqCstUnload
        {
            get { return m_InitSeqCstUnload; }
            set { m_InitSeqCstUnload = value; }
        }

        private bool m_InitSeqCstUnloadReturnValue = false;
        [Browsable(false), XmlIgnore()]
        public bool InitSeqCstUnloadReturnValue
        {
            get { return m_InitSeqCstUnloadReturnValue; }
            set { m_InitSeqCstUnloadReturnValue = value; }
        }

        private bool m_CstThicknessDataSendReq = false;
        [Browsable(false), XmlIgnore()]
        public bool CstThicknessDataSendReq
        {
            get { return m_CstThicknessDataSendReq; }
            set { m_CstThicknessDataSendReq = value; }
        }

        private bool m_ReClampingRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool ReClampingRequest
        {
            get { return m_ReClampingRequest; }
            set { m_ReClampingRequest = value; }
        }

        private bool m_MappingRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool MappingRequest
        {
            get { return m_MappingRequest; }
            set { m_MappingRequest = value; }
        }

        private PortCommandCode m_AutoPortCommand;
        [Browsable(false), XmlIgnore()]
        public PortCommandCode AutoPortCommand
        {
            get { return m_AutoPortCommand; }
            set { m_AutoPortCommand = value; }
        }

        private PortUnit_BoeHF_Theragen_SeqGlassThickSend m_ThickSeq;
        private PortUnit_BoeHF_Theragen_SeqPortCommand m_SeqPortCommand;
        #endregion 

        #region LampSwitches
        private LampSwitch m_LampInputOk = new LampSwitch();
        [Category("DMS : Lamp Setting")]
        public override LampSwitch LampInputOk
        {
            get { return m_LampInputOk; }
            set { m_LampInputOk = value; }
        }
        private LampSwitch m_LampInputEnd = new LampSwitch();
        [Category("DMS : Lamp Setting")]
        public override LampSwitch LampInputEnd
        {
            get { return m_LampInputEnd; }
            set { m_LampInputEnd = value; }
        }
        private LampSwitch m_LampOutputOk = new LampSwitch();
        [Category("DMS : Lamp Setting")]
        public override LampSwitch LampOutputOk
        {
            get { return m_LampOutputOk; }
            set { m_LampOutputOk = value; }
        }
        private LampSwitch m_LampOutputEnd = new LampSwitch();
        [Category("DMS : Lamp Setting")]
        public override LampSwitch LampOutputEnd
        {
            get { return m_LampOutputEnd; }
            set { m_LampOutputEnd = value; }
        }
        private LampSwitch m_LampForceEnd = new LampSwitch();
        [Category("DMS : Lamp Setting")]
        public override LampSwitch LampForceEnd
        {
            get { return m_LampForceEnd; }
            set { m_LampForceEnd = value; }
        }
        #endregion

        #region BitInput
        private IoDigitalInput m_mibLoaderOnline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOnline
        {
            get { return m_mibLoaderOnline; }
            set { m_mibLoaderOnline = value; }
        }

        private IoDigitalInput m_mibLoaderOffline = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibLoaderOffline
        {
            get { return m_mibLoaderOffline; }
            set { m_mibLoaderOffline = value; }
        }

        private IoDigitalInput m_mibPortComReadConfirm = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortComReadConfirm
        {
            get { return m_mibPortComReadConfirm; }
            set { m_mibPortComReadConfirm = value; }
        }
        private IoDigitalInput m_mibPortComEndReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortComEndReport
        {
            get { return m_mibPortComEndReport; }
            set { m_mibPortComEndReport = value; }
        }
        private IoDigitalInput m_mibPortStateReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortStateReport
        {
            get { return m_mibPortStateReport; }
            set { m_mibPortStateReport = value; }
        }
        private IoDigitalInput m_mibPortLoadRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortLoadRequest
        {
            get { return m_mibPortLoadRequest; }
            set { m_mibPortLoadRequest = value; }
        }
        private IoDigitalInput m_mibPortPreLoadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortPreLoadComplete
        {
            get { return m_mibPortPreLoadComplete; }
            set { m_mibPortPreLoadComplete = value; }
        }
        private IoDigitalInput m_mibPortCstReady = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCstReady
        {
            get { return m_mibPortCstReady; }
            set { m_mibPortCstReady = value; }
        }
        private IoDigitalInput m_mibPortUnloadReqeust = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortUnloadReqeust
        {
            get { return m_mibPortUnloadReqeust; }
            set { m_mibPortUnloadReqeust = value; }
        }
        private IoDigitalInput m_mibPortUnloadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortUnloadComplete
        {
            get { return m_mibPortUnloadComplete; }
            set { m_mibPortUnloadComplete = value; }
        }
        private IoDigitalInput m_mibPortDown = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortDown
        {
            get { return m_mibPortDown; }
            set { m_mibPortDown = value; }
        }
        private IoDigitalInput m_mibPortEnable = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortEnable
        {
            get { return m_mibPortEnable; }
            set { m_mibPortEnable = value; }
        }
        private IoDigitalInput m_mibPortDisable = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortDisable
        {
            get { return m_mibPortDisable; }
            set { m_mibPortDisable = value; }
        }
        private IoDigitalInput m_mibPortStkMode = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortStkMode
        {
            get { return m_mibPortStkMode; }
            set { m_mibPortStkMode = value; }
        }
        private IoDigitalInput m_mibPortMgvMode = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortMgvMode
        {
            get { return m_mibPortMgvMode; }
            set { m_mibPortMgvMode = value; }
        }
        private IoDigitalInput m_mibGlassThickDataReceive = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibGlassThickDataReceive
        {
            get { return m_mibGlassThickDataReceive; }
            set { m_mibGlassThickDataReceive = value; }
        }
        #endregion

        #region BitOutput
        private IoDigitalOutput m_mobPortComReadReqeust = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortComReadReqeust
        {
            get { return m_mobPortComReadReqeust; }
            set { m_mobPortComReadReqeust = value; }
        }
        private IoDigitalOutput m_mobPortComEndConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortComEndConfirm
        {
            get { return m_mobPortComEndConfirm; }
            set { m_mobPortComEndConfirm = value; }
        }
        private IoDigitalOutput m_mobPortStateConfirm = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortStateConfirm
        {
            get { return m_mobPortStateConfirm; }
            set { m_mobPortStateConfirm = value; }
        }
        private IoDigitalOutput m_mobGlassThickDataSend = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobGlassThickDataSend
        {
            get { return m_mobGlassThickDataSend; }
            set { m_mobGlassThickDataSend = value; }
        }
        #endregion

        #region WordInput
        private IoAnalogInput m_miwCommandPortNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandPortNo
        {
            get { return m_miwCommandPortNo; }
            set { m_miwCommandPortNo = value; }
        }
        private IoAnalogInput m_miwCommandCodeNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandCodeNo
        {
            get { return m_miwCommandCodeNo; }
            set { m_miwCommandCodeNo = value; }
        }
        private IoAnalogInput m_miwCommandResultNo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwCommandResultNo
        {
            get { return m_miwCommandResultNo; }
            set { m_miwCommandResultNo = value; }
        }
        private IoAnalogInput m_miwPortCstId = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwPortCstId
        {
            get { return m_miwPortCstId; }
            set { m_miwPortCstId = value; }
        }
        private IoAnalogInput m_miwPortMapInfo = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwPortMapInfo
        {
            get { return m_miwPortMapInfo; }
            set { m_miwPortMapInfo = value; }
        }
        #endregion

        #region WordOutput
        private IoAnalogOutput m_mowCommandPortNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandPortNo
        {
            get { return m_mowCommandPortNo; }
            set { m_mowCommandPortNo = value; }
        }
        private IoAnalogOutput m_mowCommandCodeNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowCommandCodeNo
        {
            get { return m_mowCommandCodeNo; }
            set { m_mowCommandCodeNo = value; }
        }
        private IoAnalogOutput m_mowGlassThickPortNo = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowGlassThickPortNo
        {
            get { return m_mowGlassThickPortNo; }
            set { m_mowGlassThickPortNo = value; }
        }
        private IoAnalogOutput m_mowTlassThickValue = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowTlassThickValue
        {
            get { return m_mowTlassThickValue; }
            set { m_mowTlassThickValue = value; }
        }
        #endregion

        //Coupled unit
        private _BCR m_BCR = null;
        [Category("Relation")]
        public override _BCR BCR
        {
            get { return m_BCR; }
            set { m_BCR = value; }
        }
        private _CstMappingUnit m_MappingUnit = null;
        [Category("Relation")]
        public override _CstMappingUnit MappingUnit
        {
            get { return m_MappingUnit; }
            set { m_MappingUnit = value; }
        }

        private PortUnitBoeHFInitSeq m_PortInitSeq = null;

        public Alarm ALM_PortIllegalCommandReceive = null;
        public Alarm ALM_PortAbnormalEnd = null;
        public Alarm ALM_LoaderOffline = null;
        public Alarm ALM_PortCommandEndTimeOutError = null;
        public Alarm ALM_GlassThicknessOffline = null;
        public Alarm ALM_GlassThicknessTimeOutError = null;
        #endregion

        #region Constructor
        public PortUnit_BoeHF_Theragen()
        {
            this.Name = "Port __";
        }
        #endregion

        #region Methods
        public void CreateCassette(int slotCount, string cstType)
        {
            m_Cst = new LoaderCst(slotCount);
            m_Cst.CstType = cstType;
            m_Cst.SlotOrder = CstSlotOrder.Decrease;
            m_Cst.Slot1Position = CstSlot1Position.Bottom;

            //jemoon : SlotOrder 지정방법이 필요함. 우선은 Default로 BtoT
            //sangseo :천마는 Topdown이므로 Default 변경
            //jemoon : SlotOrder, 1st glass posion을 외부에서 설정해 줄 수 있는 방법이 필요함
        }

        public void DestroyCassette()
        {
            if (m_Cst != null)
            {
                m_Cst.Dispose();
            }
        }

        public void SetGlassStatus(int slotId, GlassStatus status)
        {
            if (m_Cst.SetGlassStatus(slotId, status))
            {
                if (status == GlassStatus.Proc)
                {
                    //m_PortStatus = PortStatus.InProcess;
                }
            }
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
                ALM_PortIllegalCommandReceive = new Alarm(this.Name + " Job Illegal Receive Command Error", AlarmLevel.L, AlarmCode.ParameterControlWarning);
                ALM_PortAbnormalEnd = new Alarm(this.Name + " Job Abnormal End Error", AlarmLevel.L, AlarmCode.ParameterControlWarning);
                ALM_LoaderOffline = new Alarm(this.Name + " Command Error (Loader Offline Change)", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_PortCommandEndTimeOutError = new Alarm(this.Name + " Command End Report ON TimeOut Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_GlassThicknessOffline = new Alarm(this.Name + " Glass Thickness Error(Loader Offline Change)", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_GlassThicknessTimeOutError = new Alarm(this.Name + " Glass Thickness Receive ON Time Out Error", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_ThickSeq = new PortUnit_BoeHF_Theragen_SeqGlassThickSend(this);
                m_SeqPortCommand = new PortUnit_BoeHF_Theragen_SeqPortCommand(this);

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
                    CreateCassette(m_MaxSlotCount, "normal");
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

        public override bool IsCstInProcessing()
        {
            int count = m_Cst.SlotCount;
            for (int i = 0; i < count; i++)
            {
                if (m_Cst.Glasses[i].Status == GlassStatus.Proc)
                    return true;
            }
            return false;
        }

        public override void LotStartSet()
        {
            for (int i = 0; i < Cst.SlotCount; i++)
            {
                if (m_Cst.Glasses[i].Status == GlassStatus.Selected)
                    m_Cst.Glasses[i].Status = GlassStatus.Wait;
            }
            m_PortStatus = PortStatus.Wait;
        }

        public override void LotAbortSet()
        {
            for (int i = 0; i < Cst.SlotCount; i++)
            {
                if (m_Cst.Glasses[i].Status == GlassStatus.Wait)
                {
                    m_Cst.Glasses[i].Status = GlassStatus.Aborted;
                }
            }
            m_PortStatus = PortStatus.Aborting;
        }

        public override void LotCancelSet()
        {
            for (int i = 0; i < Cst.SlotCount; i++)
            {
                if (m_Cst.Glasses[i].Status == GlassStatus.Selected ||
                    m_Cst.Glasses[i].Status == GlassStatus.Wait)
                {
                    m_Cst.Glasses[i].Status = GlassStatus.Canceled;
                }
            }
            m_PortStatus = PortStatus.CancelEnable;
        }

        public override bool IsCstUnloadCondition()
        {
            bool lotStarted = false;
            lotStarted |= (m_PortStatus == PortStatus.Wait);
            lotStarted |= (m_PortStatus == PortStatus.CancelEnable);
            lotStarted |= (m_PortStatus == PortStatus.InProcess);
            lotStarted |= (m_PortStatus == PortStatus.ProcessEnd);
            lotStarted |= (m_PortStatus == PortStatus.Aborting);
            lotStarted |= (m_PortStatus == PortStatus.Abort);

            bool lotEnd = false;
            lotEnd |= (m_PortStatus == PortStatus.Abort);
            lotEnd |= (m_PortStatus == PortStatus.Aborting);
            lotEnd |= (m_PortStatus == PortStatus.CancelEnable);
            lotEnd |= (m_PortStatus == PortStatus.ProcessEnd);

            bool unloadCondition = true;
            unloadCondition &= lotStarted;
            unloadCondition &= lotEnd;

            if (unloadCondition)
            {
                int count = m_Cst.SlotCount;
                for (int i = 0; i < count; i++)
                {
                    if (m_Cst.Glasses[i].Status == GlassStatus.Proc ||
                        m_Cst.Glasses[i].Status == GlassStatus.Wait ||
                        m_Cst.Glasses[i].Status == GlassStatus.Selected ||
                        m_Cst.Glasses[i].Status == GlassStatus.Started)
                    {
                        unloadCondition = false;
                        break;
                    }
                }
            }

            return unloadCondition;
        }

        public override bool IsCstExist()
        {
            if (m_mibPortPreLoadComplete.GetState() ||
                m_mibPortCstReady.GetState() ||
                m_mibPortUnloadReqeust.GetState())
                return true;
            else return false;

        }

        public override bool IsSafeCstLoading()
        {
            bool safeCondition = true;
            //            safeCondition &= m_mibPortLoadRequest.GetState() == true;

            if (m_mibPortLoadRequest.GetState() ||
                 m_mibPortPreLoadComplete.GetState() ||
                 m_mibPortUnloadComplete.GetState() ||
                 m_mibPortCstReady.GetState())
            {
                if (m_mibPortCstReady.GetState())
                {
                    if (m_PortStatus == PortStatus.CstOn)
                    {
                        safeCondition = true;
                    }
                    else
                    {
                        safeCondition = false;
                    }
                }
                else
                {
                    safeCondition = true;
                }
            }
            else
            {
                safeCondition = false;
            }

            safeCondition &= m_mibPortEnable.GetState() == true;
            safeCondition &= m_mibPortDisable.GetState() == false;
            safeCondition &= m_PortEnable == PortUsage.Use;

            return safeCondition;
        }

        public override bool IsCstClampPos()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override bool IsCstClampNeg()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override bool IsCstFloatPos()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override bool IsCstFloatNeg()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsCstLoadCondition()
        {
            bool loadCondition = true;
            loadCondition &= IsSafeCstLoading();
            // 필요하면 조건 추가

            return loadCondition;
        }

        public override bool IsCstOpposite()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override CstSlotOrder GetSlotOrder()
        {
            return m_Cst.SlotOrder;
        }

        public override void SetIonizerRun(bool on)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SeqPortInit()
        {
            return m_PortInitSeq.Do();
        }

        public int SeqGlassThickData(ThicknessKind thick)
        {
            m_ThickSeq.SetThickData(thick);
            return m_ThickSeq.Do();
        }

        public int SeqPortCommand(PortCommand command)
        {
            m_PortCommand = command;

            PortCommandCode commadcode = PortCommandCode.None;

            if (command == PortCommand.LoadRequest) commadcode = PortCommandCode.LoadRequest;
            else if (command == PortCommand.UnloadRequest) commadcode = PortCommandCode.UnloadRequest;
            else if (command == PortCommand.Clamping) commadcode = PortCommandCode.Clamping;
            else if (command == PortCommand.CstIdReadRequest) commadcode = PortCommandCode.CSTIDRead;
            else if (command == PortCommand.MappingRequest) commadcode = PortCommandCode.Mapping;

            m_SeqPortCommand.PortCommand = commadcode;
            return m_SeqPortCommand.Do();
        }
        #endregion
    }

    public class PortUnitBoeHFInitSeq : XSeqFunction
    {
    }

    public class PortUnit_BoeHF_Theragen_SeqPortCommand : XSeqFunction
    {
        #region Fields
        private PortUnit_BoeHF_Theragen m_PortUnit;
        private PortCommandCode m_PortCommand;
        private short m_PortCommandResult = 0;
        #endregion

        #region #Property
        public PortCommandCode PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }
        #endregion

        #region Constructor
        public PortUnit_BoeHF_Theragen_SeqPortCommand(PortUnit_BoeHF_Theragen port)
        {
            m_PortUnit = port;
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int rv = -1;
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_PortUnit.InProcess == false &&
                        m_PortUnit.mibLoaderOnline.GetState() &&
                        (m_PortUnit.mibPortDown.GetState() == false))
                    {
                        m_PortUnit.InProcess = true;

                        if (m_PortCommand == PortCommandCode.LoadRequest ||
                            m_PortCommand == PortCommandCode.UnloadRequest ||
                            m_PortCommand == PortCommandCode.Clamping)
                        {
                            string log = "";
                            m_PortUnit.mowCommandPortNo.SetState((ushort)m_PortUnit.PortNo);
                            m_PortUnit.mowCommandCodeNo.SetState((ushort)m_PortCommand);
                            log = string.Format("{0} Command ", m_PortCommand.ToString());
                            m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, log);

                            m_StartTicks = XFunc.GetTickCount();

                            seqNo = 5;
                        }
                        else
                        {
                            if (m_PortCommand == PortCommandCode.CSTIDRead)
                            {
                                seqNo = 100;
                            }
                            else if (m_PortCommand == PortCommandCode.Mapping)
                            {
                                seqNo = 200;
                            }
                        }
                    }
                    break;

                case 5:
                    if (GetElapsedTicks() > 300)
                    {
                        m_PortUnit.mobPortComReadReqeust.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "mobPortCommandReadReq ON");

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (m_PortUnit.mibPortComReadConfirm.GetState() == true)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "mibPortCommandReqComp ON");

                        m_PortUnit.mobPortComReadReqeust.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "mobPortCommandReadReq OFF");
                        seqNo = 20;
                    }
                    else if (m_PortUnit.mibLoaderOffline.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "Loader Control Mode Offline Change");

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;

                        returnValue = 10;
                        seqNo = 0;
                    }
                    break;

                case 20:
                    if (m_PortUnit.mibPortComReadConfirm.GetState() == false)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "mibPortCommandReqComp OFF");

                        m_PortUnit.mowCommandPortNo.SetState(0);
                        m_PortUnit.mowCommandCodeNo.SetState(0);

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 30;
                    }
                    else if (m_PortUnit.mibLoaderOffline.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "Loader Control Mode Change Offline");

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;

                        returnValue = 10;
                        seqNo = 0;
                    }
                    break;

                case 30:
                    if (m_PortUnit.mibPortComEndReport.GetState())
                    {
                        if (m_PortUnit.miwCommandCodeNo.GetState() == (short)m_PortCommand &&
                            m_PortUnit.PortNo == (int)m_PortUnit.miwCommandPortNo.GetState())
                        {
                            m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "mobPortCommandEndReport ON");

                            string log = "";
                            log = string.Format("Port {0}, Command End ({1})", m_PortUnit.PortNo, m_PortCommand.ToString());
                            m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, log);

                            seqNo = 40;
                        }
                    }
                    else if (m_PortUnit.mibLoaderOffline.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", m_PortUnit.Id + 1, 0, "Loader Control Mode Change Offline");

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;

                        returnValue = 10;
                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > m_PortUnit.PortCommandEndIFTimeout)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", 0, 0, "mibPortCommandEndReport ON Timeout");

                        m_PortUnit.mobPortComReadReqeust.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command Read", 0, 0, "mobPortCommandReadReq OFF");

                        returnValue = 100;
                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    break;

                case 40:
                    {
                        m_PortCommandResult = m_PortUnit.miwCommandResultNo.GetState();

                        string log = "";
                        log = string.Format("Port {0}, Command Result : {1}", m_PortUnit.PortNo, m_PortCommandResult.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, log);

                        seqNo = 50;
                    }
                    break;

                case 50:
                    {
                        m_PortUnit.mobPortComEndConfirm.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "mobPortCommandEndConfirm ON");

                        seqNo = 60;
                    }
                    break;

                case 60:
                    if (m_PortUnit.mibPortComEndReport.GetState() == false)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "mobPortCommandEndReport OFF");

                        m_PortUnit.mobPortComEndConfirm.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "mobPortCommandEndConfirm OFF");

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 70;
                    }
                    break;

                case 70:
                    if (GetElapsedTicks() > 500)
                    {
                        int nCommandResult = (int)m_PortCommandResult;

                        if (nCommandResult == 1)
                        {
                            returnValue = 0;
                        }
                        else if (nCommandResult == 2)
                        {
                            returnValue = 2;
                        }
                        else if (nCommandResult == 3)
                        {
                            returnValue = 3;
                        }
                        else
                        {
                            returnValue = 4;
                        }

                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    break;


                case 100: // CST ID Reading
                    if ((rv = m_PortUnit.BCR.Reading(m_PortUnit.Id)) == 0)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "Port Command Complete");

                        returnValue = 0;
                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    else if (rv > 0)
                    {
                        returnValue = rv;
                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    break;

                case 200: // Mapping
                    if ((rv = m_PortUnit.MappingUnit.Mapping(m_PortUnit.Id)) == 0)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "Command End", m_PortUnit.Id + 1, 0, "Port Command Complete");

                        returnValue = 0;
                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    else if (rv > 0)
                    {
                        returnValue = rv;
                        seqNo = 0;

                        m_PortUnit.PortCommand = Dms.Common.PortCommand.None;
                        m_PortUnit.InProcess = false;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }


    public class PortUnit_BoeHF_Theragen_SeqPortCommandReadReq : XSeqFunction
    {
        #region Fields
        private PortUnit_BoeHF_Theragen m_PortUnit;
        private PortCommandCode m_PortCommand;
        #endregion

        #region #Property
        public PortCommandCode PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }
        #endregion

        #region Constructor
        public PortUnit_BoeHF_Theragen_SeqPortCommandReadReq(PortUnit_BoeHF_Theragen port)
        {
            m_PortUnit = port;
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_PortUnit.InProcess == false)
                    {
                        m_PortUnit.InProcess = true;

                        string log = "";
                        m_PortUnit.mowCommandPortNo.SetState((ushort)m_PortUnit.PortNo);
                        m_PortUnit.mowCommandCodeNo.SetState((ushort)m_PortCommand);
                        log = string.Format("Port {0}, Command {1}", m_PortUnit.PortNo, m_PortCommand.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, log);

                        m_PortUnit.mobPortComReadReqeust.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mobPortCommandReadReq ON");

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (m_PortUnit.mibPortComReadConfirm.GetState() == true)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mibPortCommandReqComp ON");

                        m_PortUnit.mobPortComReadReqeust.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mobPortCommandReadReq OFF");
                        seqNo = 20;
                    }
                    else if (GetElapsedTicks() > m_PortUnit.InterfaceTimeout)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mibPortCommandReqComp ON Timeout");

                        m_PortUnit.mobPortComReadReqeust.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mobPortCommandReadReq OFF");

                        returnValue = 1;
                        seqNo = 0;

                        m_PortUnit.InProcess = false;
                    }
                    break;

                case 20:
                    if (m_PortUnit.mibPortComReadConfirm.GetState() == false)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND READ", m_PortUnit.Id + 1, 0, "mibPortCommandReqComp OFF");

                        returnValue = 0;
                        seqNo = 0;

                        m_PortUnit.InProcess = false;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }

    public class PortUnit_BoeHF_Theragen_SeqPortCommandEndRpt : XSeqFunction
    {
        #region Fields
        private PortUnit_BoeHF_Theragen m_PortUnit;
        private PortCommandCode m_PortCommand;
        private short m_PortCommandResult = 0;
        private short[] m_CstId;
        private short[] m_Mapping;
        #endregion

        #region #Property
        public PortCommandCode PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }
        #endregion

        #region Constructor
        public PortUnit_BoeHF_Theragen_SeqPortCommandEndRpt(PortUnit_BoeHF_Theragen port)
        {
            m_PortUnit = port;
            m_CstId = new short[10];
            m_Mapping = new short[2];
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_PortUnit.mibPortComEndReport.GetState())
                    {
                        if (m_PortUnit.miwCommandCodeNo.GetState() == (short)m_PortCommand &&
                            m_PortUnit.PortNo == (int)m_PortUnit.miwCommandPortNo.GetState())
                        {
                            m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndReport ON");

                            string log = "";
                            log = string.Format("Port {0}, Command End{1}", m_PortUnit.PortNo, m_PortCommand.ToString());
                            m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, log);

                            seqNo = 10;
                        }
                    }
                    break;

                case 10:
                    //                    if (m_PortUnit.InProcess == false)
                    {
                        //                        m_PortUnit.InProcess = true;

                        m_PortCommandResult = m_PortUnit.miwCommandResultNo.GetState();

                        string log = "";
                        log = string.Format("Port {0}, Command Result : {1}", m_PortUnit.PortNo, m_PortCommandResult.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, log);

                        seqNo = 20;
                    }
                    break;

                case 20:
                    {
                        m_PortUnit.mobPortComEndConfirm.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndConfirm ON");

                        seqNo = 30;
                    }
                    break;

                case 30:
                    if (m_PortUnit.mibPortComEndReport.GetState() == false)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndReport OFF");

                        m_PortUnit.mobPortComEndConfirm.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndConfirm OFF");

                        returnValue = (int)m_PortCommandResult;
                        seqNo = 0;

                        //                        m_PortUnit.InProcess = false;
                    }
                    //                    else if (GetElapsedTicks() > m_PortUnit.InterfaceTimeout)
                    //                    {
                    //                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndReport OFF Timeout");

                    //                        m_PortUnit.mobPortComEndConfirm.SetState(false);
                    //                        m_PortUnit.SetLog(m_PortUnit.Name, "COMMAND END", m_PortUnit.Id + 1, 0, "mobPortCommandEndConfirm OFF");

                    //                        returnValue = 2;
                    //                        seqNo = 0;

                    //                        m_PortUnit.InProcess = false;
                    //                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }


    public class PortUnit_BoeHF_Theragen_SeqGlassThickSend : XSeqFunction
    {
        #region Fields
        private PortUnit_BoeHF_Theragen m_PortUnit;
        private ThicknessKind m_GlassThick = ThicknessKind.T070;
        #endregion

        #region Constructor
        public PortUnit_BoeHF_Theragen_SeqGlassThickSend(PortUnit_BoeHF_Theragen port)
        {
            m_PortUnit = port;
        }
        #endregion

        #region Methods
        public void SetThickData(ThicknessKind thick)
        {
            m_GlassThick = thick;
        }
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_PortUnit.CstThickenssProcess == false)
                    {
                        m_PortUnit.CstThickenssProcess = true;

                        string log = "";
                        m_PortUnit.mowGlassThickPortNo.SetState((ushort)m_PortUnit.PortNo);
                        m_PortUnit.mowTlassThickValue.SetState((ushort)m_GlassThick);
                        log = string.Format("Port {0}, Thick {1}", m_PortUnit.PortNo, m_GlassThick.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, log);

                        m_PortUnit.mobGlassThickDataSend.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mobGlassThickDataSend ON");

                        m_StartTicks = XFunc.GetTickCount();

                        if (m_PortUnit.Simul.Loader)
                        {
                            m_PortUnit.mibGlassThickDataReceive.SetState(true);
                        }

                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (m_PortUnit.mibGlassThickDataReceive.GetState() == true)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mibGlassThickDataReceive ON");

                        m_PortUnit.mobGlassThickDataSend.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mobGlassThickDataSend OFF");

                        if (m_PortUnit.Simul.Loader)
                        {
                            m_PortUnit.mibGlassThickDataReceive.SetState(false);
                        }

                        seqNo = 20;
                    }
                    else if (m_PortUnit.mibLoaderOffline.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "Loader Offline Change Alarm");

                        returnValue = 1;
                        seqNo = 0;

                        m_PortUnit.CstThickenssProcess = false;
                    }
                    else if (GetElapsedTicks() > m_PortUnit.InterfaceTimeout)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mibGlassThickDataReceive ON Timeout");

                        m_PortUnit.mobGlassThickDataSend.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mobGlassThickDataSend OFF");

                        returnValue = 2;
                        seqNo = 0;

                        m_PortUnit.CstThickenssProcess = false;
                    }
                    break;
                case 20:
                    if (m_PortUnit.mibGlassThickDataReceive.GetState() == false)
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "mibGlassThickDataReceive OFF");

                        returnValue = 0;
                        seqNo = 0;

                        m_PortUnit.CstThickenssProcess = false;
                    }
                    else if (m_PortUnit.mibLoaderOffline.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "THICK", m_PortUnit.Id + 1, 0, "Loader Offline Change Alarm");

                        seqNo = 0;
                        returnValue = 1;

                        m_PortUnit.CstThickenssProcess = false;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }
}

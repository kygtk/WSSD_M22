///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.10.28
// Author       : Koo Sangseo
// Description  : PortUnit for LGE Solar
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
    public enum LGESolarPortCommandCode
    {
        None = 0,
        PortStart = 1,
        PortReject = 2,
        MappingOn = 3,
        MappingOff = 4,
    }

    public enum LGERecipeRequestType
    {
        ContainerArrivedRecipe,
        CurRecipeDataReport,
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PortUnit_LGE_Solar : PortUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        protected static Mutex m_Mutex = new Mutex();
        private static bool m_InProcess = false;

        new protected SolarPortCommand m_PortCommand;
        [Browsable(false), XmlIgnore()]
        new public SolarPortCommand PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }

        private bool m_PortCommandReq;
        [Browsable(false), XmlIgnore()]
        public bool PortCommandReq
        {
            get { return m_PortCommandReq; }
            set { m_PortCommandReq = value; }
        }

        private bool m_PortRecipeReq;
        [Browsable(false), XmlIgnore()]
        public bool PortRecipeReq
        {
            get { return m_PortRecipeReq; }
            set { m_PortRecipeReq = value; }
        }

        private LGERecipeRequestType m_PortRecipeRequestType;
        [Browsable(false), XmlIgnore()]
        public LGERecipeRequestType PortRecipeRequestType
        {
            get { return m_PortRecipeRequestType; }
            set { m_PortRecipeRequestType = value; }
        }

        private int m_EqpRecipeId;
        [Browsable(false), XmlIgnore()]
        public int EqpRecipeId
        {
            get { return m_EqpRecipeId; }
            set { m_EqpRecipeId = value; }
        }

        private bool m_ContainerIdReq;
        [Browsable(false), XmlIgnore()]
        public bool ContainerIdReq
        {
            get { return m_ContainerIdReq; }
            set { m_ContainerIdReq = value; }
        }

        private string m_ReqeuestedContainerId;
        [Browsable(false), XmlIgnore()]
        public string ReqeuestedContainerId
        {
            get { return m_ReqeuestedContainerId; }
            set { m_ReqeuestedContainerId = value; }
        }
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
        private int m_MaxSlotCount = 100;
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

        private eqpSolarUnitType m_UnitType = eqpSolarUnitType.Loading_Unit;
        [Category("DMS : General Setting"), Description("Set Unit Type")]
        public eqpSolarUnitType UnitType
        {
            get { return m_UnitType; }
            set { m_UnitType = value; }
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

        private PortStatus m_OldPortStatus;
        [Browsable(false), XmlIgnore()]
        public PortStatus OldPortStatus
        {
            get { return m_OldPortStatus; }
            set { m_OldPortStatus = value; }
        }

        private bool m_InitSeqInit = false;
        [Browsable(false), XmlIgnore()]
        public bool InitSeqInit
        {
            get { return m_InitSeqInit; }
            set { m_InitSeqInit = value; }
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

        private bool m_MappingRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool MappingRequest
        {
            get { return m_MappingRequest; }
            set { m_MappingRequest = value; }
        }

        private PortUnit_LGE_Solar_SeqPortCommand m_SeqPortCommand;
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
        private IoDigitalInput m_mibPortLoadRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortLoadRequest
        {
            get { return m_mibPortLoadRequest; }
            set { m_mibPortLoadRequest = value; }
        }
        private IoDigitalInput m_mibPortLoadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortLoadComplete
        {
            get { return m_mibPortLoadComplete; }
            set { m_mibPortLoadComplete = value; }
        }
        private IoDigitalInput m_mibPortUnloadRequest = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortUnloadRequest
        {
            get { return m_mibPortUnloadRequest; }
            set { m_mibPortUnloadRequest = value; }
        }
        private IoDigitalInput m_mibPortUnloadComplete = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortUnloadComplete
        {
            get { return m_mibPortUnloadComplete; }
            set { m_mibPortUnloadComplete = value; }
        }
        private IoDigitalInput m_mibPortStatusChangeReport = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortStatusChangeReport
        {
            get { return m_mibPortStatusChangeReport; }
            set { m_mibPortStatusChangeReport = value; }
        }
        private IoDigitalInput m_mibPortCommandReqAck = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortCommandReqAck
        {
            get { return m_mibPortCommandReqAck; }
            set { m_mibPortCommandReqAck = value; }
        }
        private IoDigitalInput m_mibPortContainerIdReq = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibPortContainerIdReq
        {
            get { return m_mibPortContainerIdReq; }
            set { m_mibPortContainerIdReq = value; }
        }
        private IoDigitalInput m_mibMappingUse = new IoDigitalInput();
        [Category("DMS : I/O Setting (Bit Input)")]
        public IoDigitalInput mibMappingUse
        {
            get { return m_mibMappingUse; }
            set { m_mibMappingUse = value; }
        }
        #endregion

        #region BitOutput
        private IoDigitalOutput m_mobPortStatusChangeReportAck = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortStatusChangeReportAck
        {
            get { return m_mobPortStatusChangeReportAck; }
            set { m_mobPortStatusChangeReportAck = value; }
        }
        private IoDigitalOutput m_mobPortCommandReq = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortCommandReq
        {
            get { return m_mobPortCommandReq; }
            set { m_mobPortCommandReq = value; }
        }
        private IoDigitalOutput m_mobPortContainerIdReqAck = new IoDigitalOutput();
        [Category("DMS : I/O Setting (Bit Output)")]
        public IoDigitalOutput mobPortContainerIdReqAck
        {
            get { return m_mobPortContainerIdReqAck; }
            set { m_mobPortContainerIdReqAck = value; }
        }
        #endregion

        #region WordInput
        private IoAnalogInput m_miwPortStatus = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwPortStatus
        {
            get { return m_miwPortStatus; }
            set { m_miwPortStatus = value; }
        }
        private IoCollection<IoAnalogInput> m_miwPortContainerId = new IoCollection<IoAnalogInput>();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoCollection<IoAnalogInput> miwPortContainerId
        {
            get { return m_miwPortContainerId; }
            set { m_miwPortContainerId = value; }
        }
        private IoCollection<IoAnalogInput> m_miwPortMappingData = new IoCollection<IoAnalogInput>();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoCollection<IoAnalogInput> miwPortMappingData
        {
            get { return m_miwPortMappingData; }
            set { m_miwPortMappingData = value; }
        }
        private IoAnalogInput m_miwPortCommandResult = new IoAnalogInput();
        [Category("DMS : I/O Setting (Word Input)")]
        public IoAnalogInput miwPortCommandResult
        {
            get { return m_miwPortCommandResult; }
            set { m_miwPortCommandResult = value; }
        }
        #endregion

        #region WordOutput
        private IoAnalogOutput m_mowPortRequestedContainerId = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowPortRequestedContainerId
        {
            get { return m_mowPortRequestedContainerId; }
            set { m_mowPortRequestedContainerId = value; }
        }
        private IoAnalogOutput m_mowPortCommandData = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowPortCommandData
        {
            get { return m_mowPortCommandData; }
            set { m_mowPortCommandData = value; }
        }
        private IoAnalogOutput m_mowPortCommandEqpRecipeId = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowPortCommandEqpRecipeId
        {
            get { return m_mowPortCommandEqpRecipeId; }
            set { m_mowPortCommandEqpRecipeId = value; }
        }

        private IoAnalogOutput m_mowPortCommandContainerId1 = new IoAnalogOutput();
        [Category("DMS : I/O Setting (Word Output)")]
        public IoAnalogOutput mowPortCommandContainerId1
        {
            get { return m_mowPortCommandContainerId1; }
            set { m_mowPortCommandContainerId1 = value; }
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
        #endregion

        #region Constructor
        public PortUnit_LGE_Solar()
        {
            this.Name = "Port __";

            m_InitSeqInit = true;
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
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_SeqPortCommand = new PortUnit_LGE_Solar_SeqPortCommand(this);

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
            if (m_mibPortLoadComplete.GetState() ||
                m_mibPortUnloadRequest.GetState())
                return true;
            else return false;

        }

        public override bool IsSafeCstLoading()
        {
            bool safeCondition = true;
            safeCondition &= m_mibPortLoadRequest.GetState() == true;
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

        public MappingStatus[] GetMappingStatus()
        {
            MappingStatus[] status = new MappingStatus[Cst.SlotCount];

            for (int i = 0; i < Cst.SlotCount; i++)
            {
                status[i] = Cst.Glasses[i].GlassMappingStatus;
            }
            return status;
        }

        public int SeqPortCommand(SolarPortCommand command, short eqprecipeid, string containerid)
        {
            m_PortCommand = command;

            LGESolarPortCommandCode commadcode = LGESolarPortCommandCode.None;

            if (command == SolarPortCommand.PortStart) commadcode = LGESolarPortCommandCode.PortStart;
            else if (command == SolarPortCommand.PortReject) commadcode = LGESolarPortCommandCode.PortReject;
            else if (command == SolarPortCommand.MappingOn_Off)
            {
                if (mibMappingUse.GetState())
                {
                    commadcode = LGESolarPortCommandCode.MappingOff;
                }
                else
                {
                    commadcode = LGESolarPortCommandCode.MappingOn;
                }
            }

            m_SeqPortCommand.PortCommand = commadcode;
            m_SeqPortCommand.EqpRecipeId = eqprecipeid;
            m_SeqPortCommand.ContainerId = containerid;
            return m_SeqPortCommand.Do();
        }
        #endregion
    }

    public class PortUnitLGESolarInitSeq : XSeqFunction
    {
    }

    public class PortUnit_LGE_Solar_SeqPortCommand : XSeqFunction
    {
        #region Fields
        private PortUnit_LGE_Solar m_PortUnit;
        private LGESolarPortCommandCode m_PortCommand;
        private short m_PortCommandResult = 0;
        private short m_EqpRecipeId = 0;
        private string m_ContainerId = "";
        #endregion

        #region #Property
        public LGESolarPortCommandCode PortCommand
        {
            get { return m_PortCommand; }
            set { m_PortCommand = value; }
        }

        public short EqpRecipeId
        {
            get { return m_EqpRecipeId; }
            set { m_EqpRecipeId = value; }
        }

        public string ContainerId
        {
            get { return m_ContainerId; }
            set { m_ContainerId = value; }
        }
        #endregion

        #region Constructor
        public PortUnit_LGE_Solar_SeqPortCommand(PortUnit_LGE_Solar port)
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
                        m_PortUnit.mowPortCommandData.SetState((ushort)m_PortCommand);
                        log = string.Format("{0} Command ", m_PortCommand.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "Port Command", m_PortUnit.Id + 1, 0, log);

                        m_PortUnit.mowPortCommandEqpRecipeId.SetState((ushort)m_EqpRecipeId);
                        log = string.Format("Equipment Recipe ID =  {0}", m_EqpRecipeId);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Port Command", m_PortUnit.Id + 1, 0, log);

                        ushort[] data = new ushort[10];
                        XFunc.ConvertToWord(m_ContainerId, ref data, 0, 10, ByteOrder.LittleEndian);
                        m_PortUnit.mowPortCommandContainerId1.SetStates(data, 0, 10);
                        log = string.Format("Container ID =  {0}", m_ContainerId);
                        m_PortUnit.SetLog(m_PortUnit.Name, "Port Command", m_PortUnit.Id + 1, 0, log);

                        m_StartTicks = XFunc.GetTickCount();

                        seqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 250)
                    {
                        m_PortUnit.mobPortCommandReq.SetState(true);
                        m_PortUnit.SetLog(m_PortUnit.Name, "PortCommand", m_PortUnit.Id + 1, 0, "mobPortCommandRequest ON");

                        seqNo = 20;
                    }
                    break;

                case 20:
                    if (m_PortUnit.mibPortCommandReqAck.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "PortCommand", m_PortUnit.Id + 1, 0, "mibPortCommandReqAck ON");

                        m_PortCommandResult = m_PortUnit.miwPortCommandResult.GetState();

                        string log = "";
                        log = string.Format("Port {0}, Command Result : {1}", m_PortUnit.PortNo, m_PortCommandResult.ToString());
                        m_PortUnit.SetLog(m_PortUnit.Name, "PortCommand", m_PortUnit.Id + 1, 0, log);

                        m_PortUnit.mobPortCommandReq.SetState(false);
                        m_PortUnit.SetLog(m_PortUnit.Name, "PortCommand", m_PortUnit.Id + 1, 0, "mobPortCommandRequest OFF");

                        seqNo = 30;
                    }
                    break;


                case 30:
                    if (!m_PortUnit.mibPortCommandReqAck.GetState())
                    {
                        m_PortUnit.SetLog(m_PortUnit.Name, "PortCommand", m_PortUnit.Id + 1, 0, "mibPortCommandReqAck OFF");

                        seqNo = 40;
                    }
                    break;

                case 40:
                    {
                        int nCommandResult = (int)m_PortCommandResult;

                        if (nCommandResult == 1)
                        {
                            returnValue = 0;
                        }
                        else
                        {
                            returnValue = 2;
                        }

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
}

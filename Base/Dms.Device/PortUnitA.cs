///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.11.12
// Author       : jemoon
// Description  : PortUnit for Chengdu Tianma G4.5 
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

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PortUnitA : PortUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();
        #endregion

        #region Fields
        private bool m_CstDetectError = false;
        [Browsable(false), XmlIgnore()]
        public bool CstDetectError
        {
            get { return m_CstDetectError; }
            set { m_CstDetectError = value; }
        }

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

        //Cassette detect
        private IoDigitalInput m_DiCstDetectLeft = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCstDetectLeft
        {
            get { return m_DiCstDetectLeft; }
            set { m_DiCstDetectLeft = value; }
        }

        private IoDigitalInput m_DiCstDetectRight = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCstDetectRight
        {
            get { return m_DiCstDetectRight; }
            set { m_DiCstDetectRight = value; }
        }

        private IoDigitalInput m_DiCstOppositeDetect = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiCstOppositeDetect
        {
            get { return m_DiCstOppositeDetect; }
            set { m_DiCstOppositeDetect = value; }
        }

        private IoDigitalInput m_DiFloatConfim = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiFloatConfim
        {
            get { return m_DiFloatConfim; }
            set { m_DiFloatConfim = value; }
        }

        //Stocker
        private IoDigitalInput m_DiStockForkDetected = new IoDigitalInput();
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiStockForkDetected
        {
            get { return m_DiStockForkDetected; }
            set { m_DiStockForkDetected = value; }
        }

        private StockerA m_Stocker;
        [Category("DMS : Relation")]
        public StockerA Stocker
        {
            get { return m_Stocker; }
            set { m_Stocker = value; }
        }

        //Coupled unit
        private _BCR m_BCR = null;
        private _CstMappingUnit m_MappingUnit = null;
        private ActuatorUnit m_ClampUnit = null;
        private ActuatorUnit m_FloatUnit = null;
        private Ionizer m_Ionizer = null;
        private AreaSensor m_AreaInterlock = null;

        private PortUnitAInitSeq m_PortInitSeq = null;

        private string m_CassetteType;
        [Browsable(false), XmlIgnore()]
        public string CassetteType
        {
            get { return m_CassetteType; }
            set { m_CassetteType = value; }
        }
        private bool m_KeyInCstIdRequest = false;
        [Browsable(false), XmlIgnore()]
        public bool KeyInCstIdRequest
        {
            get { return m_KeyInCstIdRequest; }
            set { m_KeyInCstIdRequest = value; }
        }

        //private bool m_CstIdReadReq;
        //[Browsable(false), XmlIgnore()]
        //public bool CstIdReadReq
        //{
        //    get { return m_CstIdReadReq; }
        //    set { m_CstIdReadReq = value; }
        //}

        //private bool m_CstIdReadComp;
        //[Browsable(false), XmlIgnore()]
        //public bool CstIdReadComp
        //{
        //    get { return m_CstIdReadComp; }
        //    set { m_CstIdReadComp = value; }
        //}
        #endregion

        #region Alarm
        public Alarm ALM_PortClampCloseTimeout = null;
        public Alarm ALM_PortClampOpenTimeout = null;
        public Alarm ALM_PortCstExistError = null;
        public Alarm ALM_PortOppositeDetected = null;
        public Alarm ALM_PortFloatingError = null;

        //        public Alarm ALM_StockerInfcTimeoutTA1  = null; // Infc = Interface
        //        public Alarm ALM_StockerInfcTimeoutTA2  = null;
        //        public Alarm ALM_StockerInfcTimeoutTA3  = null;
        //        public Alarm ALM_StockerInfcTimeoutTA4  = null;
        public Alarm ALM_StockerInfcTimeoutTP1 = null;
        public Alarm ALM_StockerInfcTimeoutTP2 = null;
        public Alarm ALM_StockerInfcTimeoutTP3 = null;
        public Alarm ALM_StockerInfcTimeoutTP4 = null;
        public Alarm ALM_StockerInfcTimeoutTP5 = null;
        public Alarm ALM_StockerInfcTimeoutTP6 = null;
        public Alarm ALM_StockerInfcTimeoutTD1 = null;
        public Alarm ALM_StockerInfcValidOffError = null;


        public Alarm ALM_MappingError = null;
        public Alarm ALM_StockerForkDetected = null;
        #endregion

        #region Properties
        [Category("Relation")]
        public override _BCR BCR
        {
            get { return m_BCR; }
            set { m_BCR = value; }
        }
        [Category("Relation")]
        public override _CstMappingUnit MappingUnit
        {
            get { return m_MappingUnit; }
            set { m_MappingUnit = value; }
        }
        [Category("Relation")]
        public ActuatorUnit ClampUnit
        {
            get { return m_ClampUnit; }
            set { m_ClampUnit = value; }
        }
        [Category("Relation")]
        public ActuatorUnit FloatUnit
        {
            get { return m_FloatUnit; }
            set { m_FloatUnit = value; }
        }
        [Category("Relation")]
        public Ionizer Ionizer
        {
            get { return m_Ionizer; }
            set { m_Ionizer = value; }
        }
        [Category("Relation")]
        public AreaSensor AreaInterlock
        {
            get { return m_AreaInterlock; }
            set { m_AreaInterlock = value; }
        }
        #endregion

        #region Constructor
        public PortUnitA()
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
                    //                    m_PortStatus = PortStatus.InProcess;
                }
            }
        }

        //public void StockerLoadPresenceSignal()
        //{
        //    // This signal turns ON when detects a cassette on the stage.
        //    bool loadPresenceOn = true;
        //    loadPresenceOn &= this.DiCstDetectLeft.GetState();
        //    loadPresenceOn &= this.DiCstDetectRight.GetState();

        //    m_Stocker.DoMioLoadPresence.SetState(loadPresenceOn);
        //}

        //public void StockerReadyForLoadSignal()
        //{
        //    //This signal turns ON when the load/unload stage can load a cassette from the Crane
        //    //The signal turns OFF when the cassette is placed on the stage. 
        //    //It also turns OFF when an error occurs in the Equipment
        //    bool readyForLoadOff = false;
        //    readyForLoadOff |= m_Stocker.DoMioError.GetState();
        //    readyForLoadOff |= m_Stocker.DoMioLoadPresence.GetState();

        //    m_Stocker.DoMioReadyforLoad.SetState(!readyForLoadOff);
        //}

        //public void StockerReadyForUnloadSignal()
        //{
        //    //This signal turns ON when the load/unload stage can unload a cassette onto the Crane. 
        //    //The signal turns OFF when the cassette is removed from the stage. 
        //    //It also turns OFF when an error occurs in the Equipment.
        //    bool readyForUnloadOff = false;
        //    readyForUnloadOff |= m_Stocker.DoMioError.GetState();
        //    readyForUnloadOff |= !m_Stocker.DoMioLoadPresence.GetState();

        //    m_Stocker.DoMioReadyforUnload.SetState(!readyForUnloadOff);
        //}
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

                ALM_PortClampCloseTimeout = new Alarm(this.Name + " Clamp Close Timeout Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PortClampOpenTimeout = new Alarm(this.Name + " Clamp Open Timeout Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PortCstExistError = new Alarm(this.Name + " Cst Exist Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PortOppositeDetected = new Alarm(this.Name + " Cst Opposite detected Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PortFloatingError = new Alarm(this.Name + " Floating unit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                //                ALM_StockerInfcTimeoutTA1 = new Alarm(this.Name + " Stocker I/F Active EQP Timeout1 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // Infc = Interface
                //                ALM_StockerInfcTimeoutTA2 = new Alarm(this.Name + " Stocker I/F Active EQP Timeout2 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //                ALM_StockerInfcTimeoutTA3 = new Alarm(this.Name + " Stocker I/F Active EQP Timeout3 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //                ALM_StockerInfcTimeoutTA4 = new Alarm(this.Name + " Stocker I/F Active EQP Timeout4 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP1 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout1 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP2 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout2 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP3 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout3 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP4 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout4 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP5 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout5 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTP6 = new Alarm(this.Name + " Stocker I/F Passive EQP Timeout6 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcTimeoutTD1 = new Alarm(this.Name + " Stocker I/F TD1 Timeout Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerInfcValidOffError = new Alarm(this.Name + " Stock I/F Actvie VALID Signal OFF Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_MappingError = new Alarm(this.Name + " Mapping unit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_StockerForkDetected = new Alarm(this.Name + " Stocker Fork Detected Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                m_SetupLoaderInfoProvider = SetupLoaderInfoProvider.Instance;
                m_SetupPortEnable[this.Id] = new TagSetupInfo(this.Name + " Unit Enable / Disable Select", OptionType.None, OptionFormat.Enable, UnitType.None, "null");
                m_SetupLoaderInfoProvider.InitFromDB(m_SetupPortEnable[this.Id]);

                if (m_SetupPortEnable[this.Id].Val.ToUpper() == "DISABLE")
                {
                    this.PortEnable = PortUsage.NoUse;
                }
                else if (m_SetupPortEnable[this.Id].Val.ToUpper() == "ENABLE")
                {
                    this.PortEnable = PortUsage.Use;
                }


                //m_SetupPortEnable = new TagSetupInfo
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_PortInitSeq = new PortUnitAInitSeq(this);

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
                    CreateCassette(m_MappingUnit.MaxSlotCount, "normal");
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
            if (m_DiCstDetectLeft.GetState() || m_DiCstDetectRight.GetState()) return true;
            else return false;
        }

        public override bool IsSafeCstLoading()
        {
            bool safeCondition = true;
            safeCondition &= m_DiCstDetectLeft.GetState() == false;
            safeCondition &= m_DiCstDetectRight.GetState() == false;
            safeCondition &= m_DiCstOppositeDetect.GetState() == false;
            safeCondition &= m_PortEnable == PortUsage.Use;

            if (!m_Simul.Device)
            {
                safeCondition &= m_MappingUnit.IsMappingDriveUnitBw();
            }

            if (m_PortCommand != PortCommand.ReChuckingRequest) return safeCondition;
            else return true;

            //            return safeCondition;
        }

        public override bool IsCstClampPos()
        {
            bool clampPos = true;
            clampPos &= m_ClampUnit.GetCurAct() == ActuatorAct.Pos;
            clampPos &= m_ClampUnit.IsPositive();

            return clampPos;
        }
        public override bool IsCstClampNeg()
        {
            bool clampNeg = true;
            clampNeg &= m_ClampUnit.GetCurAct() == ActuatorAct.Neg;
            clampNeg &= m_ClampUnit.IsNegative();

            return clampNeg;
        }
        public override bool IsCstFloatPos()
        {
            bool floatPos = true;
            floatPos &= m_FloatUnit.GetCurAct() == ActuatorAct.Pos;
            floatPos &= m_FloatUnit.IsPositive();

            return floatPos;
        }
        public override bool IsCstFloatNeg()
        {
            bool floatNeg = true;
            floatNeg &= m_FloatUnit.GetCurAct() == ActuatorAct.Neg;
            floatNeg &= m_FloatUnit.IsNegative();

            return floatNeg;
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
            bool opposite = true;
            opposite &= DiCstOppositeDetect.GetState();

            return opposite;
        }

        public override CstSlotOrder GetSlotOrder()
        {
            return m_Cst.SlotOrder;
        }

        public override void SetIonizerRun(bool on)
        {
            bool ionStatus = m_Ionizer.IsRun();
            if (!ionStatus && on)
            {
                m_Ionizer.SetRun(true);
            }
            else if (ionStatus && !on)
            {
                m_Ionizer.SetRun(false);
            }
        }

        public override int SeqPortInit()
        {
            return m_PortInitSeq.Do();
        }
        #endregion
    }

    public class PortUnitAInitSeq : XSeqFunction
    {
        #region Fields
        private PortUnitA m_Port = null;
        #endregion

        #region Constructor
        public PortUnitAInitSeq(PortUnitA port)
        {
            m_Port = port;
        }
        #endregion

        #region Override
        public override int Do()
        {
            int returnValue = -1;
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    {
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "PORT Unit Initial Begin");
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Mapping Drive Homing Begin");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Port.MappingUnit.Homing(m_Port.Id) == 0)
                    {
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Mapping Drive Homing Complete");
                        m_Port.ClampUnit.SetNegativeAct();
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Port Clamp Open Begin");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    else if (XFunc.GetTickCount() - m_StartTicks > (uint)5000)
                    {
                        // MappingUnit Homing Timeout Alarm
                        returnValue = 1;
                    }
                    break;
                case 20:
                    if (m_Port.ClampUnit.IsNegative())
                    {
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Port Clamp Open Complete");
                        m_Port.FloatUnit.SetNegativeAct();
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Port Floating Unlock Begin");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 30;
                    }
                    else if (XFunc.GetTickCount() - m_StartTicks > (uint)5000)
                    {
                        // ClampUnit Set Negative Timeout Alarm
                        returnValue = 1;
                    }
                    break;
                case 30:
                    if (m_Port.FloatUnit.IsNegative())
                    {
                        m_Port.SetLog(m_Port.Name, "Port Init", 0, 0, "Port Floating Unlock Complete");
                        returnValue = 0;
                        seqNo = 0;
                    }
                    else if (XFunc.GetTickCount() - m_StartTicks > (uint)5000)
                    {
                        // Float Unit Set Negative Timeout Alarm
                        returnValue = 1;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return returnValue;
        }
        #endregion
    }
}

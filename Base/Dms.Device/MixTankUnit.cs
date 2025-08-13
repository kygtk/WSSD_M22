///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.19
// Author       : eun
// Description  : Mix Tank
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Data;
using Dms.Common;
using System.IO;

namespace Dms.Device
{
    public enum TankStatus
    {
        NOT_USE, NOT_READY, WASHING, HEATING, STANDBY, USING
    }

    public enum MixtureDensity
    {
        densityNONE, densityDRAIN, densityRECYCLE
    }

    #region TagIfFlag
    public class TagMixTankIfFlag
    {
        public bool TankReady;
        public bool TankLevelFault;
        public bool MixtureReq;
        public bool MixtureComp;
        public bool MixTankForceDrainReq;
        public bool MixTankWashingReq;
        public bool MixTankWashingComp;
        public bool MixTankDetReq;
        public bool MixTankDetComp;
        public bool InsufficientChemFlowrate;
        public TankStatus TankStatus;
        public bool MixTankHeaterReady;
        public MixtureDensity RequireDensity;

        public void Reset()
        {
            TankReady = false;
            TankLevelFault = false;
            MixtureReq = false;
            MixtureComp = false;
            MixTankForceDrainReq = false;
            MixTankWashingReq = false;
            MixTankWashingComp = false;
            MixTankDetReq = false;
            MixTankDetComp = false;
            InsufficientChemFlowrate = false;
            TankStatus = TankStatus.NOT_USE;
            MixTankHeaterReady = false;
            RequireDensity = MixtureDensity.densityNONE;
        }
    }
    #endregion

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class MixTankUnit : _DeviceAsm, ICtlTank
    {
        #region Tag Descriptor
        //protected TagDescriptorTankUnit tagDescriptor = new TagDescriptorTankUnit();
        protected TagDescriptorMixTank tagDescriptor = new TagDescriptorMixTank();
        #endregion

        #region Fields
        private TagMixTankIfFlag m_IfFlag = null;
        private HeaterUnit m_HeaterUnit = null;
        private AutoValve m_DetSupplyAutoValve = null;
        private AutoValve m_ReturnAutoValve = null;
        private AutoValve m_DrainAutoValve = null;
        private AutoValve m_DiSupplyAutoValve = null;
        private AutoValve m_DetOutAutoValve = null;
        private AutoValve m_LCSSSupplyValve = null;
        private TankLevel m_ILevel = null;
        private Pump m_Pump = null;
        private IoDigitalInput m_CcssStanby = null;
        private IoDigitalOutput m_CcssReq = null;
        public Alarm ALM_MixTankHighLevel = null;
        public Alarm ALM_MixTankLevelFault = null;
        public Alarm ALM_MixTankCcssNotEnable = null;

        private TagSetupInfo m_SetupTankUse = null;
        private static TagSetupInfo m_SetupMixTankMode = null;
        private static TagSetupInfo m_SetupDetSupplyMode = null;
        private static TagSetupInfo m_SetupTankUseLifeTime = null;
        private static TagSetupInfo m_SetupGlassLimitCount = null;
        private static TagSetupInfo m_SetupRecycleNo = null;
        private static TagSetupInfo m_SetupDrainNo = null;
        private static TagSetupInfo m_SetupWashingNo = null;
        private static TagSetupInfo m_SetupWashingTime = null;
        private static TagSetupInfo m_SetupDrainOverTime = null;
        private static TagSetupInfo m_SetupDISupplyOverTime = null;
        private static TagSetupInfo m_SetupDETSupplyOverTime = null;
        public Alarm ALM_MixTankDrainTimeout = null;
        public Alarm ALM_MixTankDiSupplyTimeout = null;
        private bool m_UseCCSS = false;
        private bool m_UseLCSS = false;

        private ChemicalInfoMix m_ChemicalVal;//2009.10.12 kimgun

        private MixTankItem m_Item;
        private bool m_UseDETnDISeperately = false;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public AutoValve DetSupplyAutoValve
        {
            get { return m_DetSupplyAutoValve; }
            set { m_DetSupplyAutoValve = value; }
        }
        [Category("DMS : Relation")]
        public AutoValve ReturnAutoValve
        {
            get { return m_ReturnAutoValve; }
            set { m_ReturnAutoValve = value; }
        }
        [Category("DMS : Relation")]
        public AutoValve DrainAutoValve
        {
            get { return m_DrainAutoValve; }
            set { m_DrainAutoValve = value; }
        }
        [Category("DMS : Relation")]
        public AutoValve DiSupplyAutoValve
        {
            get { return m_DiSupplyAutoValve; }
            set { m_DiSupplyAutoValve = value; }
        }
        [Category("DMS : Relation")]
        public AutoValve DetOutAutoValve
        {
            get { return m_DetOutAutoValve; }
            set { m_DetOutAutoValve = value; }
        }
        [Category("DMS : Relation")]
        public AutoValve LCSSSupplyValve
        {
            get { return m_LCSSSupplyValve; }
            set { m_LCSSSupplyValve = value; }
        }
        [Category("DMS : Relation")]
        public TankLevel TankLevel
        {
            get { return m_ILevel; }
            set { m_ILevel = value; }
        }
        [Category("DMS : Relation")]
        public HeaterUnit HeaterUnit
        {
            get { return m_HeaterUnit; }
            set { m_HeaterUnit = value; }
        }
        [Category("DMS : Relation")]
        public IoDigitalInput CcssStanby
        {
            get { return m_CcssStanby; }
            set { m_CcssStanby = value; }
        }
        [Category("DMS : Relation")]
        public IoDigitalOutput CcssReq
        {
            get { return m_CcssReq; }
            set { m_CcssReq = value; }
        }
        [Category("DMS : Setting"),
        Description("Set true if no use ")]
        public bool UseDETnDISeperately
        {
            get { return m_UseDETnDISeperately; }
            set { m_UseDETnDISeperately = value; }
        }
        [Category("DMS : Setting"),
        Description("Set true if no use ")]
        public bool UseCCSS
        {
            get { return m_UseCCSS; }
            set { m_UseCCSS = value; }
        }
        [Category("DMS : Setting"),
        Description("Set true if no use ")]
        public bool UseLCSS
        {
            get { return m_UseLCSS; }
            set { m_UseLCSS = value; }
        }
        [Category("DMS : Setting"),
        Description("Chemical Type ")]
        public ChemicalType ChemicalVal
        {
            get { return m_ChemicalVal.Type; }
            set { m_ChemicalVal.Type = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagMixTankIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool LevelFault
        {
            get { return IfFlag.TankLevelFault; }
        }
        [Browsable(false), XmlIgnore()]
        public bool TopLevel
        {
            get { return (m_ILevel.TopLevelConfirm == LevelConfirm.Confirm); }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyRequest
        {
            get { return (m_ILevel.SupplyRequestConfirm == LevelConfirm.Confirm); }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyStop
        {
            get { return (m_ILevel.SupplyStopConfirm == LevelConfirm.Confirm); }
        }
        [Browsable(false), XmlIgnore()]
        public bool BottomLevel
        {
            get { return (m_ILevel.BottomLevelConfirm == LevelConfirm.Confirm); }
        }
        [Browsable(false), XmlIgnore()]
        public bool RunEnable
        {
            get { return (m_ILevel.RunEnableConfirm == LevelConfirm.Confirm); }
        }
        [Browsable(false), XmlIgnore()]
        public bool TopLevelDetect
        {
            get { return (m_ILevel.TopLevelDetect == LevelDetect.Detect); }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyRequestDetect
        {
            get { return (m_ILevel.SupplyRequestDetect == LevelDetect.Detect); }
        }
        [Browsable(false), XmlIgnore()]
        public bool SupplyStopDetect
        {
            get { return (m_ILevel.SupplyStopDetect == LevelDetect.Detect); }
        }
        [Browsable(false), XmlIgnore()]
        public bool BottomLevelDetect
        {
            get { return (m_ILevel.BottomLevelDetect == LevelDetect.Detect); }
        }
        [Browsable(false), XmlIgnore()]
        public bool RunEnableDetect
        {
            get { return (m_ILevel.RunEnableDetect == LevelDetect.Detect); }
        }
        [Browsable(false), XmlIgnore()]
        public Pump OwnerPump
        {
            get { return m_Pump; }
            set { m_Pump = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool PumpUse
        {
            get
            {
                if (m_Pump == null)
                {
                    return false;
                }
                else
                {
                    return m_Pump.IsUse;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool PumpRun
        {
            get
            {
                if (m_Pump == null)
                {
                    return false;
                }
                else
                {
                    return m_Pump.IsRun();
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupTankUse
        {
            get { return m_SetupTankUse; }
            set { m_SetupTankUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupMixTankMode
        {
            get { return MixTankUnit.m_SetupMixTankMode; }
            set { MixTankUnit.m_SetupMixTankMode = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDetSupplyMode
        {
            get { return MixTankUnit.m_SetupDetSupplyMode; }
            set { MixTankUnit.m_SetupDetSupplyMode = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupTankUseLifeTime
        {
            get { return MixTankUnit.m_SetupTankUseLifeTime; }
            set { MixTankUnit.m_SetupTankUseLifeTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupGlassLimitCount
        {
            get { return MixTankUnit.m_SetupGlassLimitCount; }
            set { MixTankUnit.m_SetupGlassLimitCount = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupRecycleNo
        {
            get { return MixTankUnit.m_SetupRecycleNo; }
            set { MixTankUnit.m_SetupRecycleNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDrainNo
        {
            get { return MixTankUnit.m_SetupDrainNo; }
            set { MixTankUnit.m_SetupDrainNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupWashingNo
        {
            get { return MixTankUnit.m_SetupWashingNo; }
            set { MixTankUnit.m_SetupWashingNo = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupWashingTime
        {
            get { return MixTankUnit.m_SetupWashingTime; }
            set { MixTankUnit.m_SetupWashingTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDrainOverTime
        {
            get { return MixTankUnit.m_SetupDrainOverTime; }
            set { MixTankUnit.m_SetupDrainOverTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDISupplyOverTime
        {
            get { return MixTankUnit.m_SetupDISupplyOverTime; }
            set { MixTankUnit.m_SetupDISupplyOverTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDETSupplyOverTime
        {
            get { return MixTankUnit.m_SetupDETSupplyOverTime; }
            set { MixTankUnit.m_SetupDETSupplyOverTime = value; }
        }
        [Browsable(false)]
        public MixTankItem Items
        {
            get { return m_Item; }
            set { m_Item = value; }
        }
        #endregion

        #region Constructor
        public MixTankUnit()
        {
            this.Name = "__ Mix Tank";
        }
        #endregion

        #region Methods
        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("MixTank \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        //public void WriteItems()
        //{
        //    m_Item.WriteXml();
        //}

        //public void ReadItems()
        //{
        //    m_Item.ReadXml();
        //}
        #endregion

        #region Override
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
            ok &= (m_DetSupplyAutoValve != null);
            ok &= (m_ReturnAutoValve != null);
            ok &= (m_DrainAutoValve != null);
            ok &= (m_DiSupplyAutoValve != null);
            if (m_UseCCSS)
            {
                ok &= (m_CcssReq != null);
                ok &= (m_CcssStanby != null);
            }
            if (m_UseLCSS)
            {
                ok &= (m_LCSSSupplyValve != null);
            }

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
                if (TankLevel.TopLevelConfirm != LevelConfirm.NotUsed)
                {
                    ALM_MixTankHighLevel = new Alarm(this.Name + " High Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }
                ALM_MixTankLevelFault = new Alarm(this.Name + " Level Fault Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_MixTankDrainTimeout = new Alarm(this.Name + " Drain Timeout Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_MixTankDiSupplyTimeout = new Alarm(this.Name + " DI Supply Timeout Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_MixTankCcssNotEnable = new Alarm(this.Name + " Ccss Not Enable Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupTankUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                SetupGenInfoProvider.Instance.InitFromDB(m_SetupTankUse);
                if (null == m_SetupMixTankMode)
                {
                    if (ChemicalVal == ChemicalType.DET)
                        m_SetupMixTankMode = new TagSetupInfo("Mix Tank Mode", OptionType.Alternative, OptionFormat.MixTankMode, UnitType.None, MixTankMode.DET.ToString());
                    else
                        m_SetupMixTankMode = new TagSetupInfo("Mix Tank Mode", OptionType.Alternative, OptionFormat.MixTankModeHF, UnitType.None, MixTankModeHF.HF.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupMixTankMode);
                }
                if (null == m_SetupDetSupplyMode)
                {
                    m_SetupDetSupplyMode = new TagSetupInfo("Det Supply Mode", OptionType.Alternative, OptionFormat.DetSupplyMode, UnitType.None, DetSupplyMode.Ccss.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupDetSupplyMode);
                }
                if (null == m_SetupTankUseLifeTime)
                {
                    m_SetupTankUseLifeTime = new TagSetupInfo("Mix Tank Use Limit Time", OptionType.None, OptionFormat.Digit, UnitType.min, "60");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupTankUseLifeTime);
                }
                if (null == m_SetupGlassLimitCount)
                {
                    m_SetupGlassLimitCount = new TagSetupInfo("Mix Tank Glass Limit Time", OptionType.None, OptionFormat.Digit, UnitType.EA, "1000");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupGlassLimitCount);
                }
                if (null == m_SetupRecycleNo)
                {
                    m_SetupRecycleNo = new TagSetupInfo("Mix Tank Recycle No", OptionType.None, OptionFormat.Digit, UnitType.Times, "100");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupRecycleNo);
                }
                if (null == m_SetupDrainNo)
                {
                    m_SetupDrainNo = new TagSetupInfo("Mix Tank Drain No", OptionType.None, OptionFormat.Digit, UnitType.Times, "100");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupDrainNo);
                }
                if (null == m_SetupWashingNo)
                {
                    m_SetupWashingNo = new TagSetupInfo("Mix Tank Washing No", OptionType.None, OptionFormat.Digit, UnitType.Times, "100");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupWashingNo);
                }
                if (null == m_SetupWashingTime)
                {
                    m_SetupWashingTime = new TagSetupInfo("Mix Tank Washing Time", OptionType.None, OptionFormat.Digit, UnitType.min, "60");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupWashingTime);
                }
                if (null == m_SetupDrainOverTime)
                {
                    m_SetupDrainOverTime = new TagSetupInfo("Mix Tank Drain Over Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "60");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupDrainOverTime);
                }
                if (null == m_SetupDISupplyOverTime)
                {
                    m_SetupDISupplyOverTime = new TagSetupInfo("Mix Tank DI Supply Over Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "60");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupDISupplyOverTime);
                }
                if (null == m_SetupDETSupplyOverTime)
                {
                    if (ChemicalVal == ChemicalType.DET)
                        m_SetupDETSupplyOverTime = new TagSetupInfo("Mix Tank DET Supply Over Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "60");
                    else
                        m_SetupDETSupplyOverTime = new TagSetupInfo("Mix Tank HF Supply Over Time", OptionType.None, OptionFormat.Digit, UnitType.sec, "60");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupDETSupplyOverTime);
                }



                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagMixTankIfFlag();
                m_IfFlag.Reset();

                m_Item = new MixTankItem(this.Name + " Item");
                //m_Item.MixTankName = this.Name;
                ok &= m_Item.InitParameter();
                if (!ok)
                {
                    SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                    return DmsErrors.NotInitialized;
                }

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

        public override void HandleEvent(object sender, IoStateEventArgs e)
        {
            if (!this.Initialized) return;

            UpdateTag();

            //string msg = string.Format("{0} : Tank state change", this.Name);
            //m_Server.FireEvent(m_Tag, msg);
        }

        public override void SetSubscriber()
        {
            base.SetSubscriber();

            this.TankLevel.OnLevelSatateChanged += new IoStateChangeEventHandler(HandleEvent);
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
                int levelCount = (this.TankLevel == null) ? 0 : this.TankLevel.Count;
                m_Tag[tagDescriptor.LEVELS].Value = levelCount.ToString();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        private string m_Temp;
        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.TANKLEVELNAME, this.TankLevel.Name);
            m_Tag.SetValue(tagDescriptor.TOPLEVEL, this.TopLevel);
            m_Tag.SetValue(tagDescriptor.SUPPLYSTOP, this.SupplyStop);
            m_Tag.SetValue(tagDescriptor.SUPPLYREQUEST, this.SupplyRequest);
            m_Tag.SetValue(tagDescriptor.RUNENABLE, this.RunEnable);
            m_Tag.SetValue(tagDescriptor.BOTTOMLEVEL, this.BottomLevel);

            if (ChemicalVal == ChemicalType.DET)
                m_Tag.SetValue(tagDescriptor.SHWMODE, m_Item.CurShowerMode.ToString());
            else
            {//2009.10.12 kimgun 아..손 안 되고 코푸는건 진정 이짓거리밖에 없나??
                if (m_Item.CurShowerMode.ToString() == "DI")
                    m_Tag.SetValue(tagDescriptor.SHWMODE, m_Item.CurShowerMode.ToString());
                else
                    m_Tag.SetValue(tagDescriptor.SHWMODE, MixTankModeHF.HF.ToString());
            }
            m_Tag.SetValue(tagDescriptor.USEDTIME, m_Item.MixTankUsedTime);
            m_Tag.SetValue(tagDescriptor.GLSCOUNT, m_Item.MixTankGlassCount);
            m_Tag.SetValue(tagDescriptor.RECYCLECOUNT, m_Item.MixTankRecycleCount);
            m_Tag.SetValue(tagDescriptor.DRAINCOUNT, m_Item.MixTankDrainCount);
            m_Tag.SetValue(tagDescriptor.STANDBYTIME, m_Item.MixTankStandbyTime);
            m_Tag.SetValue(tagDescriptor.PREVUSEDTIME, m_Item.MixTankPrevUsedTime);
            m_Temp = m_IfFlag.TankStatus.ToString();
            m_Tag.SetValue(tagDescriptor.TANKSTATUS, m_Temp.Replace("_", " "));
            m_Tag.SetValue(tagDescriptor.SETUPDRAINCOUNT, m_SetupDrainNo.GetValue<int>().ToString());
            m_Tag.SetValue(tagDescriptor.SETUPRECYCLECOUNT, m_SetupRecycleNo.GetValue<int>().ToString());
        }
        #endregion

        #region ICtlTank Member
        public bool IsTankReady()
        {
            return (IfFlag.TankStatus == TankStatus.USING);// IfFlag.TankReady;
        }

        public void SetOwnerPump(PumpUnit unit)
        {
            m_Pump = unit.Pump;
        }

        public bool IsLevelFault()
        {
            return IfFlag.TankLevelFault;
        }

        public bool IsRunEnableLevel()
        {
            return (BottomLevel & RunEnable);
        }
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.21
// Author       : jemoon
// Description  : CvUnit class basic
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class TagCvIfFlag : TagTransferIfFlag
    {
        #region Fields
        private bool inSick;
        private bool outSick;
        private bool inLogicalCheckIgnore;
        private bool outLogicalCheckIgnore;
        private bool recoveryReq;
        private CvUnit m_Parent;
        #endregion

        #region Properties
        public bool InSick
        {
            get { return inSick; }
            set
            {
                inSick = value;
                if (m_Parent.GlsInSensor != null)
                {
                    m_Parent.GlsInSensor.Sick = value;
                }
            }
        }
        public bool OutSick
        {
            get { return outSick; }
            set
            {
                outSick = value;
                if (m_Parent.GlsOutSensor != null)
                {
                    m_Parent.GlsOutSensor.Sick = value;
                }
            }
        }
        public bool InLogicalCheckIgnore
        {
            get { return inLogicalCheckIgnore; }
            set { inLogicalCheckIgnore = value; }
        }
        public bool OutLogicalCheckIgnore
        {
            get { return outLogicalCheckIgnore; }
            set { outLogicalCheckIgnore = value; }
        }
        public bool RecoveryReq
        {
            get { return recoveryReq; }
            set { recoveryReq = value; }
        }
        #endregion

        #region Constructor
        public TagCvIfFlag()
        {
        }

        public TagCvIfFlag(CvUnit unit)
        {
            m_Parent = unit;
        }
        #endregion

        #region Methods
        public override void Reset()
        {
            base.Reset();

            InSick = false;
            OutSick = false;
            InLogicalCheckIgnore = true;
            OutLogicalCheckIgnore = true;
            RecoveryReq = false;
        }
        #endregion
    }

    public enum _GSS
    {
        ssIN, ssOUT
    }

    ///////////////////////////////////////////////////////////////////////////
    // Components
    // * InSensor : 1
    // * OutSenssor : 1
    // * CvMotors : n
    // * FwDecelSensor : 1
    // * BwDecelSensor : 1
    // * BrokenFix : 1
    // * BrokenScan : 1
    ///////////////////////////////////////////////////////////////////////////
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class CvUnit : TransferUnit
    {
        #region Fields
        private static int m_StartId;
        private _GenericCollection<_Motor> m_Motors = new _GenericCollection<_Motor>();
        private MotorControl m_MotorControl;
        private GlsSensor m_InSensor = null;
        private GlsSensor m_OutSensor = null;
        private Sensor m_FwDecelSensor = null;
        private Sensor m_BwDecelSensor = null;
        private BrokenDetectFixedType m_BrokenDetectFixedType = null;
        private BrokenDetectScanType m_BrokenDetectScanType = null;
        private TagCvIfFlag m_IfFlag = null;
        private CvUnit m_PrevCv = null;
        private CvUnit m_NextCv = null;
        private TagSetupInfo m_SetupDistance = null;
        private TagSetupInfo m_SetupDistanceNext = null;
        private TagSetupInfo m_SetupInOffTimeoutUse = null;
        private TagSetupInfo m_SetupOutOffTimeoutUse = null;
        private TagSetupSenSorInterlock m_SetupBrokenFixedTypeIntr = null;
        private static TagSetupInfo m_SetupCvTimeoutMargin = null;
        private SetupGenInfoProvider m_SetupGenInfoProvider = null;
        private SetupCvDistanceProvider m_SetupCvDistanceProvider = null;
        private SetupSensorTimeoutProvider m_SetupSensorTimeoutProvider = null;
        private SetupSensorInterlockProvider m_SetupSensorIntrProvider = null;
        private CvMotorAct m_AutoAct = CvMotorAct.Stop;
        private CvMotorAct[] m_ManualAct;
        private int m_AutoSpeed = 0;
        private int[] m_ManualSpeed;
        public Alarm ALM_GlsDataSensing = null;
        public Alarm ALM_GlsDataError = null;
        private bool m_CvMotorCond = true;
        private bool[] m_TimerPause;// = { false, false };
        //public XSeqFunction[] Sequence = null;
        private bool m_LinkToFirstUnit = false;
        #endregion

        #region Properties
        [Category("Setting : Base")]
        public _GenericCollection<_Motor> Motors
        {
            get { return m_Motors; }
            set { m_Motors = value; }
        }
        [Category("Setting : Base")]
        public GlsSensor GlsInSensor
        {
            get { return m_InSensor; }
            set { m_InSensor = value; }
        }
        [Category("Setting : Base")]
        public GlsSensor GlsOutSensor
        {
            get { return m_OutSensor; }
            set { m_OutSensor = value; }
        }
        [Category("Setting : Base")]
        public Sensor FwDecelSensor
        {
            get { return m_FwDecelSensor; }
            set { m_FwDecelSensor = value; }
        }
        [Category("Setting : Base")]
        public Sensor BwDecelSensor
        {
            get { return m_BwDecelSensor; }
            set { m_BwDecelSensor = value; }
        }
        [Category("Setting : Base")]
        public BrokenDetectFixedType BrokenDetectFixedType
        {
            get { return m_BrokenDetectFixedType; }
            set { m_BrokenDetectFixedType = value; }
        }
        [Category("Setting : Base")]
        public BrokenDetectScanType BrokenDetectScanType
        {
            get { return m_BrokenDetectScanType; }
            set { m_BrokenDetectScanType = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagCvIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Category("Relation"), Browsable(false), XmlIgnore()]
        public CvUnit PrevCv
        {
            get { return m_PrevCv; }
            set { m_PrevCv = value; }
        }
        [Category("Relation"), Browsable(false), XmlIgnore()]
        public CvUnit NextCv
        {
            get { return m_NextCv; }
            set { m_NextCv = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDistance
        {
            get { return m_SetupDistance; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupDistanceNext
        {
            get { return m_SetupDistanceNext; }
            set { m_SetupDistanceNext = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupInOffUse
        {
            get { return m_SetupInOffTimeoutUse; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupOutOffUse
        {
            get { return m_SetupOutOffTimeoutUse; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupTimeoutMargin
        {
            get { return m_SetupCvTimeoutMargin; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupSenSorInterlock SetupBrokenFixedTypeInterlock
        {
            get { return m_SetupBrokenFixedTypeIntr; }
        }
        [Browsable(false), XmlIgnore()]
        public CvMotorAct AutoAct
        {
            get { return m_AutoAct; }
            set { m_AutoAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int AutoSpeed
        {
            get { return m_AutoSpeed; }
            set { m_AutoSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public CvMotorAct[] ManualAct
        {
            get { return m_ManualAct; }
            set { m_ManualAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int[] ManualSpeed
        {
            get { return m_ManualSpeed; }
            set { m_ManualSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool CvMotorCond
        {
            get { return m_CvMotorCond; }
            set { m_CvMotorCond = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool[] TimerPause
        {
            get { return m_TimerPause; }
            set { m_TimerPause = value; }
        }
        [Browsable(false), XmlIgnore()]
        public MotorControl MotorControl
        {
            get { return m_MotorControl; }
        }
        [Browsable(false), XmlIgnore()]
        public static int StartId
        {
            get { return m_StartId; }
            set { m_StartId = value; }
        }
        [Category("Relation"),
        Description("Set true if this unit is the last unit but it link to the first unit. (for making cv distance setup parameter)")]
        public bool LinkToFirstUnit
        {
            get { return m_LinkToFirstUnit; }
            set { m_LinkToFirstUnit = value; }
        }
        #endregion

        #region Constructor
        public CvUnit()
        {
            m_GlassDataCount = 2;
            this.Name = "__ CvUnit";
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

            log = string.Format("CvUnit  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        //public bool TimerPause(_GSS type)
        //{
        //    return m_TimerPause[(int)type];
        //}
        public void SetReverseMotorAct(bool reverse)
        {
            int motorCount = m_Motors.Count;
            for (int i = 0; i < motorCount; i++)
            {
                m_Motors[i].SetReverseMotorAct(reverse);
            }
        }
        public void SetReverseMotorUI(bool reverse)
        {
            int motorCount = m_Motors.Count;
            for (int i = 0; i < motorCount; i++)
            {
                m_Motors[i].SetReverseMotorUI(reverse);
            }
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(TransferUnit); }
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
                if (GlsInSensor != null || GlsOutSensor != null)
                {
                    ALM_GlsDataSensing = new Alarm(this.Name + " Glass Data Sensing Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_GlsDataError = new Alarm(this.Name + " Glass Data Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                m_SetupCvDistanceProvider = SetupCvDistanceProvider.Instance;
                m_SetupDistance = new TagSetupInfo(this.Name + " In ~ Out", OptionType.None, OptionFormat.Digit, UnitType.mm, "1000", "10", "3000");
                m_SetupCvDistanceProvider.InitFromDB(m_SetupDistance);
                if ((null != m_NextCv) || m_LinkToFirstUnit)
                {
                    m_SetupDistanceNext = new TagSetupInfo(this.Name + " Out ~ Next In", OptionType.None, OptionFormat.Digit, UnitType.mm, "240", "10", "2000");
                    m_SetupCvDistanceProvider.InitFromDB(m_SetupDistanceNext);
                }
                if (m_SetupCvTimeoutMargin == null)
                {
                    m_SetupGenInfoProvider = SetupGenInfoProvider.Instance;
                    m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin(+/-)", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                    m_SetupGenInfoProvider.InitFromDB(m_SetupCvTimeoutMargin);
                }
                m_SetupSensorTimeoutProvider = SetupSensorTimeoutProvider.Instance;
                m_SetupInOffTimeoutUse = new TagSetupInfo(this.Name + " In Off TimeOut", OptionType.Alternative, OptionFormat.Check, UnitType.None, Check.NoCheck.ToString());
                m_SetupSensorTimeoutProvider.InitFromDB(m_SetupInOffTimeoutUse);
                m_SetupOutOffTimeoutUse = new TagSetupInfo(this.Name + " Out Off TimeOut", OptionType.Alternative, OptionFormat.Check, UnitType.None, Check.NoCheck.ToString());
                m_SetupSensorTimeoutProvider.InitFromDB(m_SetupOutOffTimeoutUse);
                if (m_BrokenDetectFixedType != null)
                {
                    m_SetupSensorIntrProvider = SetupSensorInterlockProvider.Instance;
                    m_SetupBrokenFixedTypeIntr = new TagSetupSenSorInterlock(this.Name + " Broken All", false, SensorInterlockType.Broken);
                    m_SetupSensorIntrProvider.InitFromDB(m_SetupBrokenFixedTypeIntr);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_MotorControl = new MotorControl(m_Motors);

                m_IfFlag = new TagCvIfFlag(this);
                m_IfFlag.Reset();

                Sequence = new XSeqFunction[2];
                m_TimerPause = new bool[2];

                int motors = m_Motors.Count;
                m_ManualAct = new CvMotorAct[motors];
                m_ManualSpeed = new int[motors];

                for (int i = 0; i < motors; i++)
                {
                    m_ManualAct[i] = CvMotorAct.Stop;
                    m_ManualSpeed[i] = 0;
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


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

        public override void CreateTag(DeviceTags tagContainer)
        {

        }

        public override void UpdateTag()
        {

        }
        public bool IsDecelCase()
        {
            bool decel = false;

            //Fw Case
            if (m_FwDecelSensor != null)
            {
                decel |= (m_FwDecelSensor.IsDetected() && !m_OutSensor.IsDetected());
            }
            //Bw Case
            else if (m_BwDecelSensor != null)
            {
                decel |= (m_BwDecelSensor.IsDetected() && !m_InSensor.IsDetected());
            }
            return decel;
        }
        #endregion
    }
}

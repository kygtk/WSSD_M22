using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using Dms.Ctl;

namespace Dms.Device
{
    public class TagEyeEuvIfFlag
    {
        #region Fields
        private bool m_bLampAlarm;
        private bool m_bLampPause;
        private bool m_bLampOnComp;
        private bool m_bLampOnFail;
        private bool m_bEuvResetReq;
        private bool m_bEuvResetComp;
        private bool m_bEuvUtilAlarm;
        private int m_nLampControl;
        private EyeEuvUnit m_Parent;
        #endregion

        #region Properties
        public bool bLampAlarm
        {
            get { return m_bLampAlarm; }
            set { m_bLampAlarm = value; }
        }
        public bool bLampPause
        {
            get { return m_bLampPause; }
            set { m_bLampPause = value; }
        }
        public bool bLampOnComp
        {
            get { return m_bLampOnComp; }
            set { m_bLampOnComp = value; }
        }
        public bool bLampOnFail
        {
            get { return m_bLampOnFail; }
            set { m_bLampOnFail = value; }
        }
        public bool bEuvUtilAlarm
        {
            get { return m_bEuvUtilAlarm; }
            set { m_bEuvUtilAlarm = value; }
        }
        public bool bEuvResetReq
        {
            get { return m_bEuvResetReq; }
            set { m_bEuvResetReq = value; }
        }
        public bool bEuvResetComp
        {
            get { return m_bEuvResetComp; }
            set { m_bEuvResetComp = value; }
        }
        public int nLampControl
        {
            get { return m_nLampControl; }
            set { m_nLampControl = value; }
        }
        #endregion

        #region Constructor
        public TagEyeEuvIfFlag()
        {
        }

        public TagEyeEuvIfFlag(EyeEuvUnit euv)
        {
            m_Parent = euv;
        }
        #endregion

        #region Methods
        public void ClearInfo()
        {
            m_bLampAlarm = false;
            m_bLampOnComp = false;
            m_bLampPause = false;
            m_bLampOnFail = false;
            m_bEuvUtilAlarm = false;
            m_nLampControl = -1;
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class EyeEuvUnit : _EuvUnit
    {
        #region enum
        public enum EuvAlarmCode
        {//2009.11.23 kimgun 참 불편하네..
            insideAlarm = 0x01,
            outsideAlarm = 0x02,
            LampTimeOverAlarm = 0x04,
        }
        #endregion
        #region Fields
        private TagEyeEuvIfFlag m_IfFlag = null;
        private CvUnit m_Cv = null;
        private bool m_IsAlarm;
        private _GenericCollection<EuvLamp> m_Lamps = new _GenericCollection<EuvLamp>();//2009.11.23 kimgun
        #endregion

        #region euv unit auto valve
        private AutoValve m_N2Valve = null;
        private AutoValve m_PCWInValve = null;
        private AutoValve m_PCWOutValve = null;
        private AutoValve m_CDAValve = null;
        #endregion

        #region euv unit House up/dn
        private IoDigitalInput m_DiHouseClose = new IoDigitalInput();
        private ActuatorUnit m_ActuatorUnit = null;
        #endregion

        #region euv unit Leak
        private IoDigitalInput m_DiEuvLeak = new IoDigitalInput();
        #endregion

        #region euv interface
        //Input
        private IoDigitalInput m_DiRemote = new IoDigitalInput();
        private IoDigitalInput m_DiReady = new IoDigitalInput();
        private IoDigitalInput m_DiLamp1Ok = new IoDigitalInput();
        private IoDigitalInput m_DiLamp2Ok = new IoDigitalInput();
        private IoDigitalInput m_DiLamp1TimeOver = new IoDigitalInput();
        private IoDigitalInput m_DiLamp2TimeOver = new IoDigitalInput();
        private IoDigitalInput m_DiTroubleInside = new IoDigitalInput();
        private IoDigitalInput m_DiTroubleOutside = new IoDigitalInput();

        //Output
        private IoDigitalOutput m_DoRemote = new IoDigitalOutput();
        private IoDigitalOutput m_DoStandby = new IoDigitalOutput();
        private IoDigitalOutput m_DoLamp1ON = new IoDigitalOutput();
        private IoDigitalOutput m_DoLamp2ON = new IoDigitalOutput();
        private IoDigitalOutput m_DoWaterFlowOK = new IoDigitalOutput();
        private IoDigitalOutput m_DoWaterLeakOK = new IoDigitalOutput();
        private IoDigitalOutput m_DoLampCoolCDAOK = new IoDigitalOutput();
        private IoDigitalOutput m_DoEmergencyStop = new IoDigitalOutput();
        private IoDigitalOutput m_DoReset = new IoDigitalOutput();

        //setup info
        public TagSetupInfo m_SetupEuvUse = null;

        //Alarm List
        public Alarm ALM_EUVNotRemoteMode = null;
        public Alarm ALM_EUVNotReady = null;
        public Alarm ALM_EUVLamp1TimeOver = null;
        public Alarm ALM_EUVLamp2TimeOver = null;
        public Alarm ALM_EUVInsideTrouble = null;
        public Alarm ALM_EUVOutsideTrouble = null;
        public Alarm ALM_EUVHouseNotClosed = null;//2009.11.23 kimgun
        #endregion        

        #region Properties
        [Category("DMS : Relation")]
        public CvUnit Cv
        {
            get { return m_Cv; }
            set { m_Cv = value; }
        }
        [Category("DMS : Relation")]
        public _GenericCollection<EuvLamp> Lamps
        {//2009.11.23 kimgun
            get { return m_Lamps; }
            set { m_Lamps = value; }
        }
        [Category("DMS : Relation")]
        public ActuatorUnit ActuatorUnit
        {//2009.11.23 kimgun
            get { return m_ActuatorUnit; }
            set { m_ActuatorUnit = value; }
        }
        #region Setting
        [Category("Setting")]
        public IoDigitalInput DiRemote
        {
            get { return m_DiRemote; }
            set { m_DiRemote = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiReady
        {
            get { return m_DiReady; }
            set { m_DiReady = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiLamp1Ok
        {
            get { return m_DiLamp1Ok; }
            set { m_DiLamp1Ok = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiLamp2Ok
        {
            get { return m_DiLamp2Ok; }
            set { m_DiLamp2Ok = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiLamp1TimeOver
        {
            get { return m_DiLamp1TimeOver; }
            set { m_DiLamp1TimeOver = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiLamp2TimeOver
        {
            get { return m_DiLamp2TimeOver; }
            set { m_DiLamp2TimeOver = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiTroubleInside
        {
            get { return m_DiTroubleInside; }
            set { m_DiTroubleInside = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiTroubleOutside
        {
            get { return m_DiTroubleOutside; }
            set { m_DiTroubleOutside = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoRemote
        {
            get { return m_DoRemote; }
            set { m_DoRemote = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoStandby
        {
            get { return m_DoStandby; }
            set { m_DoStandby = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoLamp1ON
        {
            get { return m_DoLamp1ON; }
            set { m_DoLamp1ON = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoLamp2ON
        {
            get { return m_DoLamp2ON; }
            set { m_DoLamp2ON = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoWaterFlowOK
        {
            get { return m_DoWaterFlowOK; }
            set { m_DoWaterFlowOK = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoWaterLeakOK
        {
            get { return m_DoWaterLeakOK; }
            set { m_DoWaterLeakOK = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoLampCoolCDAOK
        {
            get { return m_DoLampCoolCDAOK; }
            set { m_DoLampCoolCDAOK = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoEmergencyStop
        {
            get { return m_DoEmergencyStop; }
            set { m_DoEmergencyStop = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoReset
        {
            get { return m_DoReset; }
            set { m_DoReset = value; }
        }

        [Category("Setting")]
        public IoDigitalInput DiHouseClose
        {
            get { return m_DiHouseClose; }
            set { m_DiHouseClose = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiEuvLeak
        {//2009.11.23 kimgun
            get { return m_DiEuvLeak; }
            set { m_DiEuvLeak = value; }
        }
        [Category("Setting")]
        public AutoValve N2Valve
        {
            get { return m_N2Valve; }
            set { m_N2Valve = value; }
        }
        [Category("Setting")]
        public AutoValve PCWInValve
        {
            get { return m_PCWInValve; }
            set { m_PCWInValve = value; }
        }
        [Category("Setting")]
        public AutoValve PCWOutValve
        {
            get { return m_PCWOutValve; }
            set { m_PCWOutValve = value; }
        }
        [Category("Setting")]
        public AutoValve CDAValve
        {
            get { return m_CDAValve; }
            set { m_CDAValve = value; }
        }
        #endregion

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupEuvUse
        {
            get { return m_SetupEuvUse; }
            set { m_SetupEuvUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagEyeEuvIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get { return m_IsAlarm; }
            set { m_IsAlarm = value; }
        }
        #endregion

        #region Constructor
        public EyeEuvUnit()
        {
            this.Name = "Eye EUV Unit __";
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

            log = string.Format("EuvUnit  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public void Utcontrol(AutoValveAct act)
        {//2009.12.18 kimgun
            if (m_N2Valve != null)
                m_N2Valve.SetAutoValveAct(act);
            if (m_CDAValve != null)
                m_CDAValve.SetAutoValveAct(act);
            if (m_PCWInValve != null)
                m_PCWInValve.SetAutoValveAct(act);
            if (m_PCWOutValve != null)
                m_PCWOutValve.SetAutoValveAct(act);
        }

        public void LampControl(bool bOn)
        {//2009.11.23 kimgun
            foreach (EuvLamp lamp in m_Lamps)
            {
                lamp.LampOnSelect(bOn & lamp.IsUse());
                if (bOn & lamp.IsUse()) lamp.On(); //view용//2009.12.03 kimgun
                else lamp.Off();    //view용
            }
        }

        public void EuvEmergency(bool bOn)
        {
            m_DoEmergencyStop.SetState(bOn);
        }

        public bool EuvRemote(bool bOn)
        {
            if (bOn)
            {
                if (!m_DoRemote.GetState()) m_DoRemote.SetState(bOn);
                else return false;
            }
            else
            {
                m_DoRemote.SetState(bOn);
            }
            return true;
        }

        public bool IsRemoteMode()
        {
            bool bMode;
            bMode = (m_DiRemote.GetState());
            return bMode;
        }

        public bool EuvStandby(bool bOn)
        {
            if (bOn)
            {
                if (!m_DoStandby.GetState()) m_DoStandby.SetState(bOn);
                else return false;
            }
            else
            {
                m_DoStandby.SetState(bOn);
            }

            return true;
        }

        public bool IsEuvReady()
        {
            bool bMode;
            bMode = (m_DiReady.GetState());
            return bMode;
        }

        //public void LampControl(bool bOn)
        //{
        //    foreach (EuvLamp LampUnit in m_Lamps)
        //    {
        //        if (bOn) LampUnit.On(); //view용
        //        else LampUnit.Off();    //view용
        //    }
        //}

        public bool GetHouseCloseState()
        {
            bool close = true;
            close &= m_ActuatorUnit.IsNegative();
            close &= DiHouseClose.GetState();
            return close;
        }

        public bool GetUtState()
        {
            bool bOpen = true;
            bOpen &= m_N2Valve.IsOpen();
            bOpen &= m_CDAValve.IsOpen();
            bOpen &= m_PCWInValve.IsOpen();

            return bOpen;
        }

        public int GetEuvAlarm()
        {
            int nRv = 0;
            if (m_DiTroubleInside.GetState()) nRv |= (int)EuvAlarmCode.insideAlarm;
            if (m_DiTroubleOutside.GetState()) nRv |= (int)EuvAlarmCode.outsideAlarm;
            foreach (EuvLamp lamp in m_Lamps)
                if (lamp.GetUsedTimeOver()) nRv |= (int)EuvAlarmCode.LampTimeOverAlarm << lamp.Id;
            return nRv;
        }

        public bool GetLampOnState()
        {//2009.11.23 kimgun
            bool bOn = false;
            if (!m_Simul.Device)
            {
                bOn |= m_DiLamp1Ok.GetState();
                bOn |= m_DiLamp2Ok.GetState();
            }
            else
            {
                foreach (EuvLamp lamp in m_Lamps)
                {
                    bOn |= lamp.IsOn(); //view용
                }
            }
            return bOn;
        }
        // Move to ThreadUshioEuvControl
        //public virtual bool IsGlassExist()
        //{
        //    bool bExist = false;

        //    bExist |= m_Cv.GlsInSensor.IsDetected();
        //    bExist |= m_Cv.GlsOutSensor.IsDetected();

        //    return bExist;
        //}

        private bool IsEuvUse()
        {
            return m_Server.JobCond.EuvUse(this);
        }

        //public void LampOnSelect()
        //{
        //    bool bOn = IsEuvUse();
        //    int count = m_Lamps.Count;
        //    EuvLamp lamp;
        //    for (int i = 0; i < count; i++)
        //    {
        //        lamp = m_Lamps[i];
        //        lamp.LampOnSelect(bOn & lamp.IsUse());
        //    }
        //}
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(EyeEuvUnit); }
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

            ok &= (m_DiRemote != null);
            ok &= (m_DiReady != null);
            ok &= (m_DiLamp1Ok != null);
            ok &= (m_DiLamp2Ok != null);
            ok &= (m_DiLamp1TimeOver != null);
            ok &= (m_DiLamp2TimeOver != null);
            ok &= (m_DiTroubleInside != null);
            ok &= (m_DiTroubleOutside != null);

            ok &= (m_DoRemote != null);
            ok &= (m_DoStandby != null);
            ok &= (m_DoLamp1ON != null);
            ok &= (m_DoLamp2ON != null);
            ok &= (m_DoWaterFlowOK != null);
            ok &= (m_DoWaterLeakOK != null);
            ok &= (m_DoLampCoolCDAOK != null);
            ok &= (m_DoEmergencyStop != null);

            ok &= (m_DiHouseClose != null);
            ok &= (m_Cv != null);
            ok &= (m_ActuatorUnit != null);
            ok &= (m_N2Valve != null);
            ok &= (m_CDAValve != null);
            ok &= (m_PCWInValve != null);
            ok &= (m_DiEuvLeak != null);

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

                ALM_EUVInsideTrouble = new Alarm(this.Name + "Euv Inside Trouble", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //2009.11.23 kimgun  lamp가 overtime일때도 발생하는데 어쩌라고. 그래서 경알람 처리하자.
                ALM_EUVOutsideTrouble = new Alarm(this.Name + "Euv Outside Trouble", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVLamp1TimeOver = new Alarm(this.Name + "Euv Lamp1 TimeOver", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVLamp2TimeOver = new Alarm(this.Name + "Euv Lamp2 TimeOver", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                ALM_EUVNotRemoteMode = new Alarm(this.Name + "Euv Not Remote Mode", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVNotReady = new Alarm(this.Name + "Euv Not Ready", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EUVHouseNotClosed = new Alarm(this.Name + "Euv House Not Closed", AlarmLevel.S, AlarmCode.EquipmentSafety);//2009.11.23 kimgun

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion

                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupEuvUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupGenInfoProvider.InitFromDB(this.m_SetupEuvUse);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagEyeEuvIfFlag(this);
                m_IfFlag.ClearInfo();

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

                if (m_Simul.Device)
                {
                    m_ActuatorUnit.SetNegativeAct();
                    m_DiHouseClose.SetState(true);
                    m_DiRemote.SetState(true);
                }

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
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (m_Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }
        }

        public override void UpdateTag()
        {
            if (m_Tag != null)
            {
                m_Tag.SetValue(tagDescriptor.ALARM, m_IsAlarm);
                m_Tag.SetValue(tagDescriptor.USE, IsEuvUse());
            }
        }
        #endregion
    }
}

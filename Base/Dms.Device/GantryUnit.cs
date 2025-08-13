using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Xml.Serialization;
using Dms.Data;
using Dms.Common;
using System.Linq;

namespace Dms.Device
{
    public class TagGantryIfFlag : TagTransferIfFlag
    {
        #region Fields
        private bool waitPos;
        private GantryUnit m_Parent;
        #endregion

        #region Properties
        public bool WaitPos
        {
            get { return waitPos; }
            set
            {
                waitPos = value;
            }
        }
        #endregion

        #region Constructor
        public TagGantryIfFlag()
        {
        }

        public TagGantryIfFlag(GantryUnit unit)
        {
            m_Parent = unit;
        }
        #endregion

        #region Methods
        public override void Reset()
        {
            base.Reset();

            waitPos = false;
        }
        #endregion
    }
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class GantryUnit : TransferRobot
    {
        #region Fields
        private Cylinder m_AlignCylinder1 = null;
        private Cylinder m_AlignCylinder2 = null;
        private ActuatorUnit m_AlignUnit = null;	//Align Cylinder가 두개 이상인 경우
        private Cylinder m_TurnCylinder = null;
        private Sensor m_RobotHandInterlock = null;
        private Sensor m_AreaSensor = null;
        private Sensor m_PosWait = null;
        private Sensor m_PosSend = null;
        private Sensor m_PosRecv = null;
        private Sensor m_PosExch = null;
        private Sensor m_PosHome = null;
        private BrokenDetectFixedType m_BrokenSensor = null;
        private GlsSensor m_GlassExistSensor = null;
        //private GlsSensor m_GlassWaitCheck = null;
        //private GlsSensor m_GlassRecvCheck = null;
        public Alarm ALM_GlassExist = null;
        public Alarm ALM_AlignFw = null;
        public Alarm ALM_AlignBw = null;
        public Alarm ALM_TurnExchangePos = null;
        public Alarm ALM_TurnProcessPos = null;
        public Alarm ALM_TurnInvertedPos = null;
        public Alarm ALM_HomePosMove = null;
        public Alarm ALM_WaitPosMove = null;
        public Alarm ALM_SendPosMove = null;
        public Alarm ALM_RecvPosMove = null;
        public Alarm ALM_ExchPosMove = null;
        public Alarm ALM_HomePosSensor = null;
        public Alarm ALM_WaitPosSensor = null;
        public Alarm ALM_SendPosSensor = null;
        public Alarm ALM_RecvPosSensor = null;
        public Alarm ALM_ExchangePosSensor = null;
        public Alarm ALM_PosNotDetect = null;
        public Alarm ALM_RobotInterrupt = null;
        public Alarm ALM_DataSensorUnmatch = null;
        public Alarm ALM_ServoReset = null;
        public Alarm ALM_powerJointup = null;
        public Alarm ALM_powerJointdown = null;

        private TagSetupInfo m_SetupAlignFwTimeout = null;
        private TagSetupInfo m_SetupAlignBwTimeout = null;
        private TagSetupInfo m_SetupTurnMoveTimeout = null;

        //jemoon : 사이트 확인결과 구현된 내용없고 의미 파악 안됨
        //private TagSetupInfo m_SetupRunMode = null;

        private TagGantryIfFlag m_IfFlag = null;
        private bool m_IsTurnUse = false;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public Sensor AreaSensor
        {
            get { return m_AreaSensor; }
            set { m_AreaSensor = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diWait_Pos_Sensor
        {
            get { return m_PosWait; }
            set { m_PosWait = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diSend_Pos_Sensor
        {
            get { return m_PosSend; }
            set { m_PosSend = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diRecv_Pos_Sensor
        {
            get { return m_PosRecv; }
            set { m_PosRecv = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diHome_Pos_Sensor
        {
            get { return m_PosHome; }
            set { m_PosHome = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diExch_Pos_Sensor
        {//2009.11.25 kimgun
            get { return m_PosExch; }
            set { m_PosExch = value; }
        }
        [Category("DMS : Relation")]
        public BrokenDetectFixedType FixedBrokenSensor
        {
            get { return m_BrokenSensor; }
            set { m_BrokenSensor = value; }
        }
        [Category("DMS : Relation")]
        public Sensor RobotHandInterlock
        {
            get { return m_RobotHandInterlock; }
            set { m_RobotHandInterlock = value; }
        }
        [Category("DMS : Relation")]
        public GlsSensor GlassExistSensor
        {
            get { return m_GlassExistSensor; }
            set { m_GlassExistSensor = value; }
        }
        //[Category("DMS : Relation")]
        //public GlsSensor GlassWaitCheck
        //{
        //    get { return m_GlassWaitCheck; }
        //    set { m_GlassWaitCheck = value; }
        //}
        //[Category("DMS : Relation")]
        //public GlsSensor GlassRecvCheck
        //{
        //    get { return m_GlassRecvCheck; }
        //    set { m_GlassRecvCheck = value; }
        //}
        [Category("DMS : Relation")]
        public Cylinder Align1
        {
            get { return m_AlignCylinder1; }
            set { m_AlignCylinder1 = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder Align2
        {
            get { return m_AlignCylinder2; }
            set { m_AlignCylinder2 = value; }
        }
        [Category("DMS : Relation"), Description("Align Cylinder가 두개이상인 경우")]
        public ActuatorUnit AlignUnit
        {
            get { return m_AlignUnit; }
            set { m_AlignUnit = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder TurnCylinder
        {
            get { return m_TurnCylinder; }
            set { m_TurnCylinder = value; }
        }
        [Category("DMS : Relation")]
        public override _ServoUnit Servo
        {
            get { return m_Servo; }
            set
            {
                m_Servo = value;
                if (m_Servo == null) return;
                List<string> TeachPointName = m_Servo.TeachPointName.ToList();
                string[] EssentialPoints = { "Home", "Wait", "Recv", "Send" };

                for (int i = 0; i < EssentialPoints.Length; i++)
                {
                    if (TeachPointName.Count <= i) TeachPointName.Add(EssentialPoints[i]);
                    else if (TeachPointName[i] != EssentialPoints[i]) TeachPointName.Insert(i, EssentialPoints[i]);
                }

                m_Servo.TeachPointName = TeachPointName.ToArray();
            }
        }
        [Browsable(false), XmlIgnore()]
        public TagGantryIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Category("DMS : Relation")]
        public bool IsTurnUse
        {
            get { return m_IsTurnUse; }
            set { m_IsTurnUse = value; }
        }

        //jemoon : 사이트 확인결과 구현된 내용없고 의미 파악 안됨
        //[Browsable(false), XmlIgnore()]
        //public TagSetupInfo SetupRunMode
        //{
        //    get { return m_SetupRunMode; }
        //}
        #endregion

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupAlignFwTimeout
        {
            get { return m_SetupAlignFwTimeout; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupAlignBwTimeout
        {
            get { return m_SetupAlignBwTimeout; }
        }

        #region Constructor
        public GantryUnit()
        {
            this.Name = "Gantry__";
            //this.m_UnitType = GetType();
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

            log = string.Format("Gantry  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public void AlignSet(Dms.Common.ActuatorAct act)
        {
            if (m_AlignUnit != null)
            {
                m_AlignUnit.SetAct(act);
            }
            else
            {
                Align1.SetAct(act);
                Align2.SetAct(act);
            }
        }

        public Dms.Common.ActuatorAct GetAlignStatus()
        {
            if (m_AlignUnit != null)
            {
                return m_AlignUnit.GetCurAct();
            }
            else
            {
                Dms.Common.ActuatorAct act1 = Align1.GetCurAct();
                Dms.Common.ActuatorAct act2 = Align2.GetCurAct();
                if (act1 == act2) return act1;
                else return Dms.Common.ActuatorAct.Noop;
            }
        }

        public bool IsAlignFw()
        {
            if (m_AlignUnit != null)
            {
                return m_AlignUnit.IsPositive();
            }
            else
            {
                bool fw = true;
                fw &= Align1.IsFwSensingOnly();
                fw &= Align2.IsFwSensingOnly();
                //fw &= !Align1.IsBwSensing();
                //fw &= !Align2.IsBwSensing();
                return fw;
            }
        }

        public bool IsAlignBw()
        {
            if (m_AlignUnit != null)
            {
                return m_AlignUnit.IsNegative();
            }
            else
            {
                bool bw = true;
                bw &= Align1.IsBwSensingOnly();
                bw &= Align2.IsBwSensingOnly();
                //bw &= !Align1.DiFwSensor.GetState();
                //bw &= !Align2.DiFwSensor.GetState();
                return bw;
            }
        }

        public bool IsGlassExist()
        {
            if (GlassExistSensor == null) return false;
            return this.GlassExistSensor.IsDetected();
        }

        public bool IsGlassExist(Logic logic)
        {
            if (GlassExistSensor == null) return false;
            return this.GlassExistSensor.IsDetected(logic);
        }

        public void SetTurnProcessPos()
        {
        }

        public void SetTurnExchangePos()
        {
        }

        public void SetTurnInvertedPos()
        {
        }

        public bool IsRobotInterlock()
        {
            return m_RobotHandInterlock.IsDetected();/*DiSensor.GetState();*/
        }

        public virtual bool IsGlassBroken()
        {
            return false;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get
            {
                return typeof(TransferUnit);
            }
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
                ALM_GlassExist = new Alarm(this.Name + " Glass Exist Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_AlignFw = new Alarm(this.Name + " Align Forward Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_AlignBw = new Alarm(this.Name + " Align Backward Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_TurnExchangePos = new Alarm(this.Name + " Turn Exchange Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_TurnProcessPos = new Alarm(this.Name + " Turn Process Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_TurnInvertedPos = new Alarm(this.Name + " Turn Inverted Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_HomePosSensor = new Alarm(this.Name + " Home Position Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_WaitPosSensor = new Alarm(this.Name + " Wait Position Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_SendPosSensor = new Alarm(this.Name + " Send Position Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_RecvPosSensor = new Alarm(this.Name + " Receive Position Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                if (Servo.TeachPoints == 5)
                    ALM_ExchangePosSensor = new Alarm(this.Name + " Exchange Position Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HomePosMove = new Alarm(this.Name + " Home Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_WaitPosMove = new Alarm(this.Name + " Wait Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_SendPosMove = new Alarm(this.Name + " Send Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_RecvPosMove = new Alarm(this.Name + " Receive Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                if (Servo.TeachPoints == 5)//2009.11.25 kimgun
                    ALM_ExchPosMove = new Alarm(this.Name + " Exchange Position Move Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_PosNotDetect = new Alarm(this.Name + " Position Sensor Not Detect Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_RobotInterrupt = new Alarm(this.Name + " Robot Hand Interrupt Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_DataSensorUnmatch = new Alarm(this.Name + " Glass Data and Sensor Unmatch Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ServoReset = new Alarm(this.Name + " Servo Reset", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ALM_powerJointup = new Alarm(this.Name + " Joint Up Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_powerJointdown = new Alarm(this.Name + " Joint Down Sensor Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupAlignFwTimeout = new TagSetupInfo(this.Name + " Align Fw Timeout value", Dms.Common.OptionType.None, Dms.Common.OptionFormat.Digit, Dms.Common.UnitType.sec, "10");
                setupGenInfoProvider.InitFromDB(m_SetupAlignFwTimeout);
                m_SetupAlignBwTimeout = new TagSetupInfo(this.Name + " Align Bw Timeout value", Dms.Common.OptionType.None, Dms.Common.OptionFormat.Digit, Dms.Common.UnitType.sec, "10");
                setupGenInfoProvider.InitFromDB(m_SetupAlignBwTimeout);
                if (m_IsTurnUse && m_TurnCylinder != null)
                {
                    m_SetupTurnMoveTimeout = new TagSetupInfo(this.Name + " Turn Move Timeout value", Dms.Common.OptionType.None, Dms.Common.OptionFormat.Digit, Dms.Common.UnitType.sec, "10");
                    setupGenInfoProvider.InitFromDB(m_SetupTurnMoveTimeout);
                }
                //jemoon : 사이트 확인결과 구현된 내용없고 의미 파악 안됨
                //m_SetupRunMode = new TagSetupInfo(this.Name + " Run Mode", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                //setupGenInfoProvider.InitFromDB(m_SetupRunMode);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagGantryIfFlag(this);
                m_IfFlag.Reset();
                Sequence = new XSeqFunction[1]; // 09.05.30 minhan
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

        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
        {

        }

        public override void UpdateTag()
        {

        }
        #endregion
    }
}

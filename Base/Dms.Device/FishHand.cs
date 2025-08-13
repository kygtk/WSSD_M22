using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Common;
using Dms.Data;
using System.Linq;

namespace Dms.Device
{
    #region Tag
    public class TagLdFishIfFlag : TagTransferIfFlag
    {
        public bool LdFishAirInterlock;
        public bool Alarm;
        private FishHand m_Parent;

        #region Constructor
        public TagLdFishIfFlag()
        {
        }

        public TagLdFishIfFlag(FishHand unit)
        {
            m_Parent = unit;
        }
        #endregion

        public override void Reset()
        {
            base.Reset();

            LdFishAirInterlock = false;
            Alarm = false;
        }
    }
    #endregion

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class FishHand : TransferRobot
    {
        #region Enums
        public enum HandUseType
        {
            Load,
            Unload
        }
        #endregion

        #region Fields
        private Sensor m_CylinderLiftUpCDASensor = null;
        private Sensor m_CylinderLiftDownCDASensor = null;
        private Sensor m_CylinderLiftUpCDASensorMT = null;
        private Sensor m_CylinderLiftDownCDASensorMT = null;
        private Sensor m_PosRecv1 = null;
        private Sensor m_PosRecv2 = null;
        private Sensor m_PosRecv3 = null;
        private Sensor m_PosSend1 = null;
        private Sensor m_PosSend2 = null;
        private Sensor m_PosSend3 = null;
        private Sensor m_PosWait = null;
        private Sensor m_PosHome = null;
        private Cylinder m_FrontRib = null;
        private Cylinder m_RearRib = null;
        private GlsSensor m_GlassExistSensor = null;
        private ServoCheckPoint m_ServoPositionSensor = null;
        private TagLdFishIfFlag m_IfFlag = null;
        private HandUseType m_UseType = HandUseType.Unload;

        public Alarm ALM_FishLiftUp = null;
        public Alarm ALM_FishLiftDown = null;
        public Alarm ALM_FishLiftAbnormal = null;
        public Alarm ALM_FishDataAbnormal = null;
        public Alarm ALM_FishGlsNotSensing = null;
        public Alarm ALM_HomePosSensor = null;
        public Alarm ALM_Recv1PosSensor = null;
        public Alarm ALM_Recv2PosSensor = null;
        public Alarm ALM_Recv3PosSensor = null;
        public Alarm ALM_WaitPosSensor = null;
        public Alarm ALM_Send1PosSensor = null;
        public Alarm ALM_Send2PosSensor = null;
        public Alarm ALM_Send3PosSensor = null;
        public Alarm ALM_HomePosMove = null;
        public Alarm ALM_Recv1PosMove = null;
        public Alarm ALM_Recv2PosMove = null;
        public Alarm ALM_Recv3PosMove = null;
        public Alarm ALM_WatiPosMove = null;
        public Alarm ALM_Send1PosMove = null;
        public Alarm ALM_Send2PosMove = null;
        public Alarm ALM_Send3PosMove = null;
        public Alarm ALM_PosNotDetect = null;
        public Alarm ALM_ServoReset = null;
        public Alarm ALM_FishHandCdaLow = null;
        //public XSeqFunction[] Sequence = null; // 09.05.30 minhan 시퀀스 초기화를 위해 필요 추가 요망
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public Sensor CylinderLiftUpCDASensor
        {
            get { return m_CylinderLiftUpCDASensor; }
            set { m_CylinderLiftUpCDASensor = value; }
        }

        [Category("DMS : Relation")]
        public Sensor CylinderLiftDownCDASensor
        {
            get { return m_CylinderLiftDownCDASensor; }
            set { m_CylinderLiftDownCDASensor = value; }
        }

        [Category("DMS : Relation")]
        public Sensor CylinderLiftUpCDASensorMT
        {
            get { return m_CylinderLiftUpCDASensorMT; }
            set { m_CylinderLiftUpCDASensorMT = value; }
        }

        [Category("DMS : Relation")]
        public Sensor CylinderLiftDownCDASensorMT
        {
            get { return m_CylinderLiftDownCDASensorMT; }
            set { m_CylinderLiftDownCDASensorMT = value; }
        }

        [Category("DMS : Relation")]
        public Sensor diSend1_Pos_Sensor
        {
            get { return m_PosSend1; }
            set { m_PosSend1 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diSend2_Pos_Sensor
        {
            get { return m_PosSend2; }
            set { m_PosSend2 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diSend3_Pos_Sensor
        {
            get { return m_PosSend3; }
            set { m_PosSend3 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diRecv1_Pos_Sensor
        {
            get { return m_PosRecv1; }
            set { m_PosRecv1 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diRecv2_Pos_Sensor
        {
            get { return m_PosRecv2; }
            set { m_PosRecv2 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diRecv3_Pos_Sensor
        {
            get { return m_PosRecv3; }
            set { m_PosRecv3 = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diWait_Pos_Sensor
        {
            get { return m_PosWait; }
            set { m_PosWait = value; }
        }
        [Category("DMS : Relation")]
        public Sensor diHome_Pos_Sensor
        {
            get { return m_PosHome; }
            set { m_PosHome = value; }
        }
        [Category("DMS : Relation")]
        public Cylinder FrontRib
        {
            get { return m_FrontRib; }
            set { m_FrontRib = value; }
        }

        [Category("DMS : Relation")]
        public Cylinder RearRib
        {
            get { return m_RearRib; }
            set { m_RearRib = value; }
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
                string[] EssentialPoints = { "Home", "Recv1", "Recv2", "Recv3", "Wait", "Send1", "Send2", "Send3" };

                for (int i = 0; i < EssentialPoints.Length; i++)
                {
                    if (TeachPointName.Count <= i) TeachPointName.Add(EssentialPoints[i]);
                    else if (TeachPointName[i] != EssentialPoints[i]) TeachPointName.Insert(i, EssentialPoints[i]);
                }

                m_Servo.TeachPointName = TeachPointName.ToArray();
            }
        }

        [Category("DMS : Relation")]
        public GlsSensor GlassExistSensor
        {
            get { return m_GlassExistSensor; }
            set { m_GlassExistSensor = value; }
        }

        [Category("Dms : Relation")]
        public ServoCheckPoint ServoPositionSensor
        {
            get { return m_ServoPositionSensor; }
            set { m_ServoPositionSensor = value; }
        }

        //[Browsable(false), XmlIgnore()]
        //public TagCvIfFlag IfFlag
        //{
        //    get { return m_IfFlag; }
        //    set { m_IfFlag = value; }
        //}

        [Browsable(false), XmlIgnore()]
        public TagLdFishIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }

        [Category("Dms : Hand Use Type")]
        public HandUseType UseType { get { return m_UseType; } set { m_UseType = value; } }
        #endregion

        #region Constructor
        public FishHand()
        {
            this.Name = "__ Fish Hand";
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

            log = string.Format("FishHand  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }

        public bool IsHandUp()
        {
            bool up = true;
            up &= FrontRib.GetCurAct() == ActuatorAct.Pos;
            up &= RearRib.GetCurAct() == ActuatorAct.Pos;
            return up;
        }

        public bool IsHandDown()
        {
            bool down = true;
            down &= FrontRib.GetCurAct() == ActuatorAct.Neg;
            down &= RearRib.GetCurAct() == ActuatorAct.Neg;
            return down;
        }

        public bool IsGlassExist()
        {
            if (m_GlassExistSensor != null)
                return m_GlassExistSensor.IsDetected();
            else return false;
        }

        public bool IsGlassExist(Logic logic)
        {
            if (m_GlassExistSensor != null)
                return m_GlassExistSensor.IsDetected(logic);
            else return false;
        }

        public void SetHandUp()
        {
            if (FrontRib != null)
                FrontRib.SetAct(ActuatorAct.Pos);
            if (RearRib != null)
                RearRib.SetAct(ActuatorAct.Pos);
        }

        public void SetHandDown()
        {
            if (FrontRib != null)
                FrontRib.SetAct(ActuatorAct.Neg);
            if (RearRib != null)
                RearRib.SetAct(ActuatorAct.Neg);
        }

        public bool IsHandPressureOk()
        {
            bool ok = (m_CylinderLiftUpCDASensor == null && m_CylinderLiftUpCDASensorMT == null);
            ok |= (m_CylinderLiftUpCDASensor != null || m_CylinderLiftUpCDASensorMT != null);

            if (m_CylinderLiftUpCDASensor != null)
            {
                ok &= m_CylinderLiftUpCDASensor.IsDetected();
            }
            if (m_CylinderLiftUpCDASensorMT != null)
            {
                ok &= m_CylinderLiftUpCDASensorMT.IsDetected();
            }

            return ok;
        }
        #endregion

        #region _DeviceAsm
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }

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
                ALM_FishLiftUp = new Alarm(this.Name + " Lift Up", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_FishLiftDown = new Alarm(this.Name + " Lift Down", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_FishLiftAbnormal = new Alarm(this.Name + " Lift Abnormal Status", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_FishDataAbnormal = new Alarm(this.Name + " Data Abnormal Status", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_FishGlsNotSensing = new Alarm(this.Name + " Glass Not Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HomePosSensor = new Alarm(this.Name + " Home Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv1PosSensor = new Alarm(this.Name + " Recv1 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv2PosSensor = new Alarm(this.Name + " Recv2 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv3PosSensor = new Alarm(this.Name + " Recv3 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_WaitPosSensor = new Alarm(this.Name + " Wait Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send1PosSensor = new Alarm(this.Name + " Send1 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send2PosSensor = new Alarm(this.Name + " Send2 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send3PosSensor = new Alarm(this.Name + " Send3 Position Sensing", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HomePosMove = new Alarm(this.Name + " Home Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv1PosMove = new Alarm(this.Name + " Recv1 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv2PosMove = new Alarm(this.Name + " Recv2 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Recv3PosMove = new Alarm(this.Name + " Recv3 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_WatiPosMove = new Alarm(this.Name + " Wait Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send1PosMove = new Alarm(this.Name + " Send1 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send2PosMove = new Alarm(this.Name + " Send2 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Send3PosMove = new Alarm(this.Name + " Send3 Position Moving", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PosNotDetect = new Alarm(this.Name + " Position Sensor Not Detect", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ServoReset = new Alarm(this.Name + " Servo Reset", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_FishHandCdaLow = new Alarm(this.Name + " FishHand CDA Low", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagLdFishIfFlag(this);
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
        #endregion
    }
}

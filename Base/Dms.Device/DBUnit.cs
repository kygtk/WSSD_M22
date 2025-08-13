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
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DBUnit : _DeviceAsm
    {
        #region enum
        private enum DBPosId
        { 
            Wait = 0 ,
            Zero = 1
        }
        public enum DBUnitAct 
        { 
            Off, 
            Noop, 
            Wait, 
            Zero, 
            Process
        }
        #endregion

        #region Structures
        private struct TagMotorPara
        {
            public bool SetComp;
            public double ProcSpeed;
            public double RefSpeed;
            public double ManualSpeed;
            public double CurSpeed;
        }

        private struct TagGapPara
        {
            public bool SetComp;
            public DBUnitAct RefPosAct;
            public RbtPos RefPos;
            public RbtPos CurPos;
            public RbtPos ProcPos;
            public RbtPos WaitPos;
            public double ProcGap;
            public bool DBManualModeChange;
        }
        #endregion

        #region Fields
        private TagMotorPara m_MotorPara;
        private TagGapPara m_GapPara;
        private DBMotor m_Motor = null;
        private _ServoUnit m_ServoUnit = null;
        public Alarm ALM_ServoMoveFail = null;
        public Alarm ALM_ServoEstopFail = null;
        public Alarm ALM_ServoResetFail = null;
        public Alarm ALM_ServoHomeFail = null;
        public TagSetupInfo SetupServoUse = null;
        public TagSetupInfo SetupIdleDir;
        public TagSetupInfo SetupIdleSpeed;
        private bool m_DBMotorCond = true;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public DBMotor Motor
        {
            get { return m_Motor; }
            set { m_Motor = value; }
        }
        [Category("DMS : Relation")]
        public _ServoUnit ServoUnit
        {
            get { return m_ServoUnit; }
            set { m_ServoUnit = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double RefSpeed
        {
            get { return m_MotorPara.RefSpeed; }
            set { m_MotorPara.RefSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double ManualSpeed
        {
            get { return m_MotorPara.ManualSpeed; }
            set { m_MotorPara.ManualSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double CurSpeed
        {
            get { return m_MotorPara.CurSpeed; }
            set { m_MotorPara.CurSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double ProcSpeed
        {
            get { return m_MotorPara.ProcSpeed; }
            set { m_MotorPara.ProcSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool SpeedSetComp
        {
            get { return m_MotorPara.SetComp; }
            set { m_MotorPara.SetComp = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool GapSetComp
        {
            get { return m_GapPara.SetComp; }
            set { m_GapPara.SetComp = value; }
        }
        [Browsable(false), XmlIgnore()]
        public DBUnitAct RefPosAct
        {
            get { return m_GapPara.RefPosAct; }
            set { m_GapPara.RefPosAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos RefPos
        {
            get { return m_GapPara.RefPos; }
            set { m_GapPara.RefPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool DBManualModeChange
        {
            get { return m_GapPara.DBManualModeChange; }
            set { m_GapPara.DBManualModeChange = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos CurPos
        {
            get
            {
                m_ServoUnit.GetCurPosition(ref m_GapPara.CurPos);
                return m_GapPara.CurPos; 
            }
            set { m_GapPara.CurPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos ProcPos
        {
            get 
            {
                if (!this.Initialized) return new RbtPos();
                else
                {
                    double pos = ProcGap;
                    pos += m_ServoUnit.GetTeachPointPos((short)DBPosId.Zero).Pos[0];
                    m_GapPara.ProcPos.Pos[0] = pos;
                    return m_GapPara.ProcPos;
                }
            }
            set 
            {
                if (this.Initialized)
                {
                    m_GapPara.ProcPos = value;
                }
            }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos WaitPos
        {
            get { return m_GapPara.WaitPos; }
            set { m_GapPara.WaitPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double ProcGap
        {
            get { return m_GapPara.ProcGap; }
            set { m_GapPara.ProcGap = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool DBMotorCond
        {
            get { return m_DBMotorCond; }
            set { m_DBMotorCond = value; }
        }
        #endregion

        #region Constructor
        public DBUnit()
        {
            this.Name = "__ DBUnit";
        }
        #endregion

        #region Methods
        //private void UpdatePosition(object sender, PositionChangeEventArgs e)
        //{
        //    if (this.ServoUnit.Id == e.RbtId)
        //    {
        //        this.CurPos = e.CurPos;
        //    }
        //}

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

            log = string.Format("DBUnit  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(DBUnit); }
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
                
                if (null != this.ServoUnit)
                {
                    ALM_ServoMoveFail = new Alarm(this.Name + " Moving Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_ServoEstopFail = new Alarm(this.Name + " E-Stop Fail Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_ServoResetFail = new Alarm(this.Name + " Reset Fail Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                    ALM_ServoHomeFail = new Alarm(this.Name + " Home Fail Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                }
                
                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                
                SetupIdleDir = new TagSetupInfo(this.Name + " Dir", OptionType.Alternative, OptionFormat.Cw, UnitType.None, Cw.Cw.ToString());
                SetupIdleInfoProvider.Instance.InitFromDB(this.SetupIdleDir);
                
                SetupIdleSpeed = new TagSetupInfo(this.Name + " Speed", OptionType.None, OptionFormat.Digit, UnitType.rpm, "200");
                SetupIdleInfoProvider.Instance.InitFromDB(this.SetupIdleSpeed);
                
                if (null != this.ServoUnit)
                {
                    SetupServoUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    SetupGenInfoProvider.Instance.InitFromDB(this.SetupServoUse);
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_GapPara = new TagGapPara();
                m_MotorPara = new TagMotorPara();
                
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
                    if (null != this.ServoUnit)
                    {
                        this.RefPos = new RbtPos(this.ServoUnit.AxisCount);
                        this.CurPos = new RbtPos(this.ServoUnit.AxisCount);
                        this.ProcPos = new RbtPos(this.ServoUnit.AxisCount);
                        this.WaitPos = new RbtPos(this.ServoUnit.AxisCount);

                        //this.ServoUnit.OnPositionChange += new PositionChangeEventHandler(UpdatePosition);
                    }
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
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoTilt : _Actuator
    {
        #region enum
        private enum TiltPosId
        { 
            Home = 0,
            Down = 1,
            Up = 2
        }
        public enum TiltAct 
        { 
            Off, 
            Noop, 
            Up, 
            Down
        }
        #endregion

        #region Structures
        private struct TagPosPara
        {
            public bool SetComp;
            public TiltAct RefPosAct;
            public RbtPos RefPos;
            public RbtPos CurPos;
            public RbtPos UpPos;
            public RbtPos DownPos;
        }
        #endregion

        #region Fields
        private TagPosPara m_PosPara;
        private ServoUnit m_ServoUnit = null;
        private IoDigitalInput m_DiUpSensor = null;
        private IoDigitalInput m_DiDnSensor = null;
        private bool m_HomeSensorIsDown = false;
        public Alarm ALM_ServoMoveFail = null;
        #endregion

        #region Properties
        [Category("DMS : Relation")]
        public ServoUnit ServoUnit
        {
            get { return m_ServoUnit; }
            set { m_ServoUnit = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiUpSensor
        {
            get { return m_DiUpSensor; }
            set { m_DiUpSensor = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiDnSensor
        {
            get { return m_DiDnSensor; }
            set { m_DiDnSensor = value; }
        }
        [Category("DMS : Setting")]
        public bool HomeSensorIsDown
        {
            get { return m_HomeSensorIsDown; }
            set { m_HomeSensorIsDown = value; }
        }
        [Browsable(false), XmlIgnore()]
        public ActuatorAct RefAct
        {
            get { return m_RefAct; }
            set { m_RefAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public ActuatorAct CurAct
        {
            get { return m_CurAct; }
            set { m_CurAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TiltAct RefPosAct
        {
            get { return m_PosPara.RefPosAct; }
            set { m_PosPara.RefPosAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos RefPos
        {
            get { return m_PosPara.RefPos; }
            set { m_PosPara.RefPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos CurPos
        {
            get 
            {
                m_ServoUnit.GetCurPosition(ref m_PosPara.CurPos);
                return m_PosPara.CurPos; 
            }
            set { m_PosPara.CurPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos UpPos
        {
            get { return m_PosPara.UpPos; }
            set { m_PosPara.UpPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public RbtPos DownPos
        {
            get { return m_PosPara.DownPos; }
            set { m_PosPara.DownPos = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool PosSetComp
        {
            get { return m_PosPara.SetComp; }
            set { m_PosPara.SetComp = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool HomeComp
        {
            get { return m_ServoUnit.HomeComp; }
        }

        #endregion

        #region Constructor
        public ServoTilt()
        {
            this.Name = "__ Servo Tilt Unit";
        }
        #endregion

        #region Methods
        //private void UpdatePosition(object sender, PositionChangeEventArgs e)
        //{
        //    if (this.ServoUnit.Id == e.RbtId)
        //    {
        //        this.CurPos = e.CurPos;
        //        UpdateTag();
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

            log = string.Format("RbUnit  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ServoTilt); }
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
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_PosPara = new TagPosPara();
                if (null != this.ServoUnit)
                {
                    this.RefPos = new RbtPos(this.ServoUnit.AxisCount);
                    this.CurPos = new RbtPos(this.ServoUnit.AxisCount);
                    this.UpPos = new RbtPos(this.ServoUnit.AxisCount);
                    this.DownPos = new RbtPos(this.ServoUnit.AxisCount);

                    this.UpPos = m_ServoUnit.GetTeachPointPos((short)TiltPosId.Up);
                    this.DownPos = m_ServoUnit.GetTeachPointPos((short)TiltPosId.Down);

                    //this.ServoUnit.OnPositionChange += new PositionChangeEventHandler(UpdatePosition);
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

        public override int SetAct(ActuatorAct act)
        {
            int rv = -1;
            m_RefAct = act;
            
            //switch (act)
            //{
            //    case ActuatorAct.Pos:
            //        rv = SetFw();
            //        break;
            //    case ActuatorAct.Neg:
            //        rv = SetBw();
            //        break;
            //}

            return rv;
        }

        //protected int SetFw()
        //{
        //    return m_ServoUnit.RbtMovePos(UpPos);
        //}

        //protected int SetBw()
        //{
        //    return m_ServoUnit.RbtMovePos(DownPos);
        //}

        protected bool IsUpSensing()
        {
            bool confirm = false;

            confirm = m_DiUpSensor.GetState();

            return confirm;
        }

        protected bool IsDnSensing()
        {
            bool confirm = false;

            if (!m_Simul.Motion)
            {
                if (HomeSensorIsDown) confirm = m_ServoUnit.Axis[0].GetHomeSwitch();
                else confirm = DiDnSensor.GetState();
            }
            else
            {
                confirm = DiDnSensor.GetState();
            }

            return confirm;
        }

        protected bool IsUp()
        {
            bool up = IsUpSensing();
            bool dn = IsDnSensing();
            bool confirm = (up && !dn);

            return confirm;
        }

        protected bool IsDn()
        {
            bool up = IsUpSensing();
            bool dn = IsDnSensing();
            bool confirm = (!up && dn);

            return confirm;
        }
        
        public override ActuatorAct GetRefAct()
        {
            return m_RefAct;
        }

        public override ActuatorAct GetCurAct()
        {
            if (IsUp())
            {
                m_CurAct = ActuatorAct.Pos;
            }
            else if (IsDn())
            {
                m_CurAct = ActuatorAct.Neg;
            }
            else
            {
                m_CurAct = ActuatorAct.Noop;
            }

            return m_CurAct;
        }

        public override bool IsActStatus(ActuatorAct act)
        {
            GetCurAct();
            return (m_CurAct == act);
        }

        public override bool IsAlarm()
        {
            return false;
        }

        public override AlarmList GetAlarmList()
        {
            AlarmList alarms = new AlarmList();

            return alarms;
        }

        public override void CreateTag(DeviceTags tagContainer)
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
            m_Tag.SetValue(tagDescriptor.ACT_STATUS, GetCurAct());
            m_Tag.SetValue(tagDescriptor.ACT_COMMAND, GetRefAct());
            m_Tag.SetValue(tagDescriptor.POS_SENSOR, IsUpSensing());
            m_Tag.SetValue(tagDescriptor.NEG_SENSOR, IsDnSensing());
        }
        #endregion
    }
}

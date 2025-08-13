using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Drawing.Design;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoMotor : _ServoMotor
    {
        #region Fields
        protected short m_AxisId;

        protected AxisInfo m_AxisPara;
        protected HomeInfo m_HomeInfo;

        protected bool m_HomeComp;

        private double m_Radius = 5; // 10.03.28 minhan
        private double m_MaxJogVel = 100;
        private double m_MinJogVel = 1;
        #endregion

        #region Properties
        [Category("DMS : Basic Info")]
        public override short AxisId
        {
            get { return m_AxisId; }
            set { m_AxisId = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("kind of Axis : General, R/B Gap(Cam)")]
        public AxisType AxisType
        {
            get { return m_AxisPara.Type; }
            set { m_AxisPara.Type = value; }
        }
        [Browsable(false), XmlIgnore()]
        [Category("DMS : Servo Info"), ReadOnly(true)]
        [Description("Current velocity : mm/s")]
        public override double AxisVel
        {
            get { return m_AxisPara.Vel; }
            set { m_AxisPara.Vel = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Acceleration Speed : mm/s")]
        public override short AxisAcc
        {
            get { return m_AxisPara.Acc; }
            set { m_AxisPara.Acc = value; }
        }
        [Browsable(false), XmlIgnore()]
        public override short AxisDec
        {
            get { return m_AxisPara.Dec; }
            set { m_AxisPara.Dec = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("the distance of 1 rev. : mm")]
        public override double AxisRatio
        {
            get { return m_AxisPara.Ratio; }
            set { m_AxisPara.Ratio = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("the pulse count of 1 rev. : ea")]
        public uint AxisEncoder
        {
            get { return m_AxisPara.Encoder; }
            set { m_AxisPara.Encoder = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Angle of Home Sensor(RB Servo Use only)")]
        public double AxisHomeAngle
        {//2010.03.27 kimgun
            get { return m_AxisPara.Theta; }
            set { m_AxisPara.Theta = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Default velocity : mm/s")]
        public double AxisDefaultVel
        {
            get { return m_AxisPara.DefaultVel; }
            set { m_AxisPara.DefaultVel = value; }
        }

        [Category("DMS : Servo Info")]
        [Description("Radus of RB(RB Servo Use only)")]
        public double Radius
        {// 10.03.28 minhan
            get { return m_Radius; }
            set { m_Radius = value; }
        }
        [Category("DMS : Servo Jog Info")]
        [Description("Maximum Jog Velocity : mm/s. Normally, Hand unit is 100, RB unit is 1.")]
        public double MaxJogVelocity
        {
            get { return m_MaxJogVel; }
            set { m_MaxJogVel = value; }
        }
        [Category("DMS : Servo Jog Info")]
        [Description("Minimum Jog Velocity : mm/s. Normally, Hand unit is 1, RB unit is 0.1. This value must bigger than 0.")]
        public double MinJogVelocity
        {
            get { return m_MinJogVel; }
            set { m_MinJogVel = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool HomeComp
        {
            get { return m_HomeComp; }
            set { m_HomeComp = value; }
        }
        #endregion

        #region Constructor
        protected ServoMotor() { }
        #endregion

        #region Virtuals
        public override bool GetHomeSwitch()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetPosSwitch()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetNegSwitch()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetInMotion()
        {
            throw new NotImplementedException();
        }

        public virtual int Homing()
        {
            throw new NotImplementedException();
        }
        public virtual void EStop()
        {
            throw new NotImplementedException();
        }
        public virtual AxisEvent GetAxisState()
        {
            throw new NotImplementedException();
        }
        public virtual AxisSource GetAxisSource()
        {
            throw new NotImplementedException();
        }
        public virtual bool CheckAxisState()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsCmdDone()
        {
            throw new NotImplementedException();
        }
        public virtual bool ClearAxisErr()
        {
            throw new NotImplementedException();
        }

        public virtual double GetPosition()
        {
            throw new NotImplementedException();
        }
        public virtual bool GetPosition(ref double pos)
        {
            throw new NotImplementedException();
        }
        public virtual int SetPosition(double pos)
        {
            throw new NotImplementedException();
        }
        public virtual double GetActVelocity()
        {
            throw new NotImplementedException();
        }
        public virtual bool SetSyncControl(short slaveAxisId, Boolean enable)
        {
            throw new NotImplementedException();
        }
        public virtual bool ClearStatus()
        {
            throw new NotImplementedException();
        }
        public virtual bool ClearFrames()
        {
            throw new NotImplementedException();
        }
        public virtual short GetEncoderDir()
        {
            return 0;
        }
        public virtual int StartRmove(double distance, double vel, short acc)
        {
            throw new NotImplementedException();
        }
        public virtual int StartSmove(double pos, double vel, double acc)
        {
            throw new NotImplementedException();
        }
        public virtual int StartATmove(double pos, double vel, short acc, short dec)
        {
            throw new NotImplementedException();
        }
        public virtual short GetControllerError()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsCpOn()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsElbOn()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsBrakeOn()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _ServoMotor Overrides
        public override void ServoOn(bool on)
        {
            throw new NotImplementedException();
        }
        public override bool GetServoOnState()
        {
            throw new NotImplementedException();
        }
        public override double Len2Pulse(double len)
        {
            double pulse = 0.0;

            if (AxisType == AxisType.Normal)
            {
                pulse = len * AxisEncoder / AxisRatio;
            }
            else if (AxisType == AxisType.RbGap)
            {
                if (len > m_Radius * 2) len = m_Radius * 2;

                double theta = Math.PI * (m_AxisPara.Theta / 180.0);
                double radian = Math.Acos(Math.Cos(theta) - (len / m_Radius)) - theta;

                pulse = radian * AxisEncoder / AxisRatio;
            }

            return pulse;
        }
        public override double Pulse2Len(double pulse)
        {
            double len = 0.0;

            if (AxisType == AxisType.Normal)
            {
                len = pulse * AxisRatio / AxisEncoder;
            }
            else if (AxisType == AxisType.RbGap)
            {
                double theta = Math.PI * (m_AxisPara.Theta / 180.0);
                double radian = pulse * AxisRatio / AxisEncoder;

                len = m_Radius * (Math.Cos(theta) - Math.Cos(theta + radian));
            }

            return len;
        }
        public override int StartVelMove(double velPulse)
        {
            throw new NotImplementedException();
        }
        public override int StopVelMove()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _DeviceAsm Overrides
        public override Type FamilyType
        {
            get { return typeof(ServoMotor); }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
        }

        public override void UpdateTag()
        {
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoMotor_Mc : ServoMotor
    {
        #region Fields
        private IMotionControl m_Mmc;
        public SeqClearAxisErr SeqClearAxisErr;
        //public SeqHomeAndIndex SeqHomeAndIndex;
        public SeqHomeOnly SeqHomeOnly;
        private double m_MaxVel = 1; // 10.05.19 minhan
        private double m_MinVel = 0; // 10.05.19 minhan
        private MotorType m_MotorType = MotorType.Servo;

        private IoDigitalInput m_DiCpOn;
        private IoDigitalInput m_DiElbOn;
        private IoDigitalInput m_DiBrakeOn;
        #endregion

        #region Properties
        [Category("DMS : Servo Info")]
        [Description("Max velocity : mm/s")]
        public double AxisMaxVel // 10.05.19 minhan
        {
            get { return m_MaxVel; }
            set { m_MaxVel = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Min velocity : mm/s")]
        public double AxisMinVel // 10.05.19 minhan
        {
            get { return m_MinVel; }
            set { m_MinVel = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Type of motor and motor driver : Servo, Stepper")]
        public MotorType MotorType
        {
            get { return m_MotorType; }
            set { m_MotorType = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("E-Stop rate : (minimum 10) * 10 msec")]
        public short AxisEstopRate
        {
            get
            {
                if (m_AxisPara.EStopRate < 10)
                {
                    m_AxisPara.EStopRate = 10;
                }

                return m_AxisPara.EStopRate;
            }
            set
            {
                if (value < 10)
                {
                    m_AxisPara.EStopRate = 10;
                }
                else if (value > 300)
                {
                    m_AxisPara.EStopRate = 300;
                }
                else
                {
                    m_AxisPara.EStopRate = value;
                }
            }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Homing type")]
        public HomeType HomeType
        {
            get { return m_HomeInfo.Type; }
            set { m_HomeInfo.Type = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Homing Velocity : mm/s")]
        public double HomeVel
        {
            get { return m_HomeInfo.Vel; }
            set { m_HomeInfo.Vel = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Acceleration Speed : mm/s")]
        public short HomeAcc
        {
            get { return m_HomeInfo.Acc; }
            set { m_HomeInfo.Acc = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("the distance for 2nd checking home sensor")]
        public double HomeDist
        {
            get { return m_HomeInfo.Dist; }
            set { m_HomeInfo.Dist = value; }
        }

        [Category("DMS : Setting (Option)")]
        public IoDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        [Category("DMS : Setting (Option)")]
        public IoDigitalInput DiElbOn
        {
            get { return m_DiElbOn; }
            set { m_DiElbOn = value; }
        }
        [Category("DMS : Setting (Option)")]
        public IoDigitalInput DiBrakeOn
        {
            get { return m_DiBrakeOn; }
            set { m_DiBrakeOn = value; }
        }
        #endregion

        #region Constructor
        public ServoMotor_Mc()
        {
            this.Name = "__ Servo Motor";

            m_AxisPara = AxisInfo.CreateDefault();
            m_HomeInfo = HomeInfo.CreateDefault();
        }
        public ServoMotor_Mc(IMotionControl mmc)
        {
            m_Mmc = mmc;
        }
        #endregion

        #region Methods
        public override bool IsCpOn()
        {
            if (!m_Initialized) return false;

            if (m_DiCpOn == null) return true;
            else
                return m_DiCpOn.GetState();
        }
        public override bool IsElbOn()
        {
            if (!m_Initialized) return false;

            if (m_DiElbOn == null) return true;
            else
                return m_DiElbOn.GetState();
        }
        public override bool IsBrakeOn()
        {
            if (!m_Initialized) return false;

            if (m_DiBrakeOn == null) return true;
            else
                return m_DiBrakeOn.GetState();
        }

        public void SetMotionController(IMotionControl mmc)
        {
            m_Mmc = mmc;
        }

        public override void ServoOn(bool on)
        {
            m_Mmc.ServoOn((short)this.AxisId, on);
        }

        public override bool GetServoOnState()
        {
            short result = 0;
            m_Mmc.GetServoOnState((short)this.AxisId, ref result);

            return ((result == 0) ? false : true);
        }

        public override void EStop()
        {
            m_Mmc.SetServoEstopRate((short)this.AxisId, this.AxisEstopRate);
            m_Mmc.ServoEstop((short)this.AxisId);

            ClearFrames();

            SeqClearAxisErr.InitSeq();
            SeqHomeOnly.InitSeq();
            //SeqHomeAndIndex.InitSeq();
        }

        public override bool IsCmdDone()
        {
            return m_Mmc.IsCmdDone((short)this.AxisId);
        }

        public override bool ClearAxisErr()
        {
            return SeqClearAxisErr.Do() == 0;
        }

        public override AxisEvent GetAxisState()
        {
            return m_Mmc.GetAxisState((short)this.AxisId);
        }

        public override AxisSource GetAxisSource()
        {
            return m_Mmc.GetAxisSource((short)this.AxisId);
        }

        public override bool CheckAxisState()
        {
            bool bResult = true;

            AxisSource source = GetAxisSource();
            source &= (AxisSource)(0x1FFF);
            if ((source & AxisSource.StPosLimit) > 0) bResult = false;
            if ((source & AxisSource.StNegLimit) > 0) bResult = false;
            if ((source & AxisSource.StAmpFault) > 0) bResult = false;
            if ((source & AxisSource.StAmpPowerOnOff) > 0) bResult = false;
            if ((source & AxisSource.StOutofFrames) > 0) bResult = false;
            return bResult;
        }

        public override int SetPosition(double pos)
        {
            return m_Mmc.SetPosition((short)this.AxisId, pos);
        }

        public override int Homing()
        {
            int nRv = 0;

            switch (HomeType)
            {
                case HomeType.HomeSensorOnly:
                    nRv = SeqHomeOnly.Do();
                    break;
                    //case HomeType.HomeSensorAndIndex:
                    //    nRv = SeqHomeAndIndex.Do();
                    //    break;
            }

            return nRv;
        }

        //public override double Len2Pulse(double len)
        //{
        //    double dPulse = 0.0;

        //    if (AxisType == AxisType.Normal)
        //    {
        //        dPulse = (double)AxisEncoder * len / AxisRatio;
        //    }
        //    else if (AxisType == AxisType.RbGap)
        //    {//2010.03.27 kimgun
        //        //if (len >  8.0) len = 8.0;
        //        //else if (len < 0.0) len = 0.0;
        //        //double fRadian = Math.Acos(1 - len / 4.0);
        //        //dPulse = (double)AxisEncoder * fRadian / AxisRatio;

        //        if (len > Radius * 2) len = Radius * 2;
        //        //else if (len < 0.0) len = 0.0;

        //        double theta = Math.PI * (m_AxisPara.Theta / 180.0);
        //        double radian = Math.Acos(Math.Cos(theta) - (len / Radius)) - theta;
        //        dPulse = (int)(AxisEncoder * radian / AxisRatio);
        //    }
        //    return dPulse;
        //}

        //public override double Pulse2Len(double pulse)
        //{
        //    double len = 0.0;
        //    if (AxisType == AxisType.Normal)
        //    {
        //        len = AxisRatio * pulse / (double)AxisEncoder;
        //    }
        //    else if (AxisType == AxisType.RbGap)
        //    {//2010.03.27 kimgun
        //        //double radian = AxisRatio * pulse / (double)AxisEncoder;
        //        //len = 4.0 * (1.0 - Math.Cos(radian));
        //        double Theta = m_AxisPara.Theta;
        //        double theta = Math.PI * (Theta / 180.0);
        //        double radian = AxisRatio * pulse / (double)AxisEncoder;
        //        len = Radius * (Math.Cos(theta) - Math.Cos(theta + radian));
        //    }
        //    return len;
        //}

        public override bool ClearStatus()
        {
            return m_Mmc.ClearStatus((short)this.AxisId);
        }

        public override bool ClearFrames()
        {
            return m_Mmc.ClearFrames((short)this.AxisId);
        }

        public short GetNegSwLimitAct(ref double limitPos, ref AxisEvent action)
        {
            return m_Mmc.GetNegSwLimitAct((short)this.AxisId, ref limitPos, ref action);
        }

        public short SetNegSwLimitAct(double limitPos, AxisEvent action)
        {
            return m_Mmc.SetNegSwLimitAct((short)this.AxisId, limitPos, action);
        }

        public short GetPosSwLimitAct(ref double limitPos, ref AxisEvent action)
        {
            return m_Mmc.GetPosSwLimitAct((short)this.AxisId, ref limitPos, ref action);
        }

        public short SetPosSwLimitAct(double limitPos, AxisEvent action)
        {
            return m_Mmc.SetPosSwLimitAct((short)this.AxisId, limitPos, action);
        }

        public short SetHomeAct(AxisEvent action)
        {
            return m_Mmc.SetHomeAct((short)this.AxisId, action);
        }

        public short SetNegLimitAct(AxisEvent action)
        {
            return m_Mmc.SetNegLimitAct((short)this.AxisId, action);
        }

        public short SetPosLimitAct(AxisEvent action)
        {
            return m_Mmc.SetPosLimitAct((short)this.AxisId, action);
        }

        public short SetStopRate(short acc)
        {
            return m_Mmc.SetStopRate((short)this.AxisId, acc);
        }

        public short SetIndexRequired(bool enable)
        {
            return m_Mmc.SetIndexRequired((short)this.AxisId, enable);
        }

        public override bool GetHomeSwitch()
        {
            return m_Mmc.GetHomeSwitch((short)this.AxisId);
        }
        public override bool GetInMotion()
        {
            return m_Mmc.GetInMotion((short)this.AxisId);
        }
        public override bool GetNegSwitch()
        {
            return m_Mmc.GetNegSwitch((short)this.AxisId);
        }

        public override bool GetPosSwitch()
        {
            return m_Mmc.GetPosSwitch((short)this.AxisId);
        }

        public short StartVmove(double vel, short acc)
        {
            return m_Mmc.StartVmove((short)this.AxisId, vel, acc);
        }

        public override int StartRmove(double distance, double vel, short acc)
        {
            return m_Mmc.StartRmove((short)this.AxisId, distance, vel, acc);
        }

        public override int StartSmove(double posPulse, double velPulse, double acc)
        {
            return m_Mmc.StartSmove((short)this.AxisId, posPulse, velPulse, (short)acc);
        }

        public override int StartATmove(double posPulse, double velPulse, short acc, short dec)
        {
            return m_Mmc.StartATmove((short)this.AxisId, posPulse, velPulse, acc, dec);
        }

        public override bool GetPosition(ref double rpos)
        {
            double curPulse = 0.0;

            if (((m_MotorType == MotorType.Servo) && (true == m_Mmc.GetPosition((short)this.AxisId, ref curPulse))) ||
                ((m_MotorType == MotorType.Stepper) && (true == m_Mmc.GetCommand((short)this.AxisId, ref curPulse))))
            {
                return false;
            }
            else
            {
                rpos = Pulse2Len(curPulse);
            }

            return true;
        }

        public override double GetPosition()
        {
            double pos = 0.0;
            GetPosition(ref pos);
            return pos;
        }

        public override bool SetSyncControl(short slaveAxisId, Boolean enable)
        {
            return m_Mmc.SetSyncControl((short)this.AxisId, slaveAxisId, enable);
        }

        public override short GetControllerError()
        {
            return m_Mmc.GetError();
        }

        public override int StartVelMove(double velPulse)
        {
            ClearFrames();
            return StartVmove(velPulse, AxisAcc);
        }

        public override int StopVelMove()
        {
            return m_Mmc.StopVmove((short)this.AxisId);
        }

        public override short GetEncoderDir()
        {
            short dir = 0;
            m_Mmc.GetEncoderDir((short)this.AxisId, ref dir);
            return dir;
        }
        //2010.12.28 kang Currunt velocity check
        public override double GetActVelocity()
        {
            return m_Mmc.GetActVelocity((short)this.AxisId);
        }
        #endregion

        #region Override
        //public override Type FamilyType
        //{
        //    get { return typeof(ServoMotor); }
        //}

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


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                SetMotionController(m_Server.MotionController);
                m_HomeComp = false;
                SeqClearAxisErr = new SeqClearAxisErr(this);
                //SeqHomeAndIndex = new SeqHomeAndIndex(this);
                SeqHomeOnly = new SeqHomeOnly(this);


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
                    ServoOn(false);
                    _GenericCollection<ServoMotor_Mc> motors = m_Server.ComponentContainer.GetCollection<ServoMotor_Mc>();
                    foreach (ServoMotor_Mc motor in motors)
                    {
                        if (!this.Equals(motor))
                        {
                            if (m_AxisId == motor.AxisId)
                            {
                                m_Initialized = false;
                                System.Windows.Forms.MessageBox.Show(m_Name + "'s AxisId is duplicated!");
                                break;
                            }
                        }
                    }
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override DmsErrors Uninitialize()
        {
            if (m_Mmc != null) EStop();

            this.Initialized = false;

            return DmsErrors.Success;
        }
        #endregion
    }
}

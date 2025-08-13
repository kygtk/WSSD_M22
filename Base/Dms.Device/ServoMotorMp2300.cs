///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.
// Author       : eun
// Description  : Servo Motor for YASKAWA MP2300
//-------------------------------------------------------------------------
// Revison History
// * 2010.01.22 : jemoon - ServoParamete들을 현재 자주 쓰는 값으로 default 값 지정함
//                초기 setting을 쉽게 하자
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
//using System.Collections;
//using System.Threading;
using System.Drawing.Design;
using Dms.Ctl;
using Dms.Common;
using System.Xml.Serialization;


namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoMotorMp2300 : _ServoMotor
    {
        #region Tag Descriptor
        protected static TagDescriptorServoMp2300 tagDescriptor = new TagDescriptorServoMp2300();
        #endregion

        #region Fields
        private short m_AxisId;
        private MP2300Ctl m_Mp2300;
        private bool m_HomeComp;
        private AxisInfoMP2300 m_AxisPara;
        private HomeInfoMP2300 m_HomeInfo;
        public SeqClearAxisErrMP2300 SeqClearAxisErr;
        public SeqHomeMP2300 SeqHome;
        public SeqSetPosMP2300 SeqSetCurPos;
        public SeqSetHomeCompMP2300 SeqSetHomeComp;
        private double m_MaxJogVel = 100;
        private double m_MinJogVel = 1;
        private float m_Radius = 5;
        private MotorType m_MotorType = MotorType.Servo;
        private static ushort[] m_HomeInfoBuf;
        private ServoUnitMp2300 m_ServoUnit = new ServoUnitMp2300();
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
        [Category("DMS : Servo Info")]
        [Description("Acceleration Speed : mm/s")]
        public override short AxisAcc
        {
            get { return m_AxisPara.Acc; }
            set { m_AxisPara.Acc = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("the distance of 1 rev. : mm")]
        public override double AxisRatio
        {
            get { return m_AxisPara.Ratio; }
            set { m_AxisPara.Ratio = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Angle of Home Sensor(RB Servo Use only)")]
        public double AxisHomeAngle
        {
            get { return m_AxisPara.Theta; }
            set { m_AxisPara.Theta = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Radus of RB(RB Servo Use only)")]
        public float Radius
        {//2010.01.27 kimgun
            get { return m_Radius; }
            set { m_Radius = value; }
        }
        [Category("DMS : Servo Info (refer to MPE720)")]
        [Description("Command Unit : mm, pulse, degree")]
        public MP2300CmdUnit AxisUnit
        {
            get { return m_AxisPara.Unit; }
            set { m_AxisPara.Unit = value; }
        }
        [Category("DMS : Servo Info (refer to MPE720)")]
        [Description("Instruction minimal unit(mm, degree use only)")]
        public int AxisDecimal
        {
            get { return m_AxisPara.Decimal; }
            set { m_AxisPara.Decimal = value; }
        }
        [Category("DMS : Servo Info (refer to MPE720)")]
        [Description("the pulse count of 1 rev. : ea")]
        public uint AxisEncoder
        {
            get { return m_AxisPara.Encoder; }
            set { m_AxisPara.Encoder = value; }
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
        [Description("Default velocity : mm/s")]
        public double AxisDefaultVel
        {
            get { return m_AxisPara.DefaultVel; }
            set { m_AxisPara.DefaultVel = value; }
        }
        [Category("DMS : Servo Info")]
        [Description("Type of motor and motor driver : Servo, Stepper")]
        public MotorType MotorType
        {
            get { return m_MotorType; }
            set { m_MotorType = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Homing type")]
        public HomeTypeMP2300 HomeType
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
        [Description("Approach Velocity : mm/s")]
        public double ApproachVel
        {
            get { return m_HomeInfo.AppVel; }
            set { m_HomeInfo.AppVel = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Creep Velocity : mm/s")]
        public double CreepVel
        {
            get { return m_HomeInfo.CrpVel; }
            set { m_HomeInfo.CrpVel = value; }
        }
        [Category("DMS : Servo Home Info")]
        [Description("Acceleration Speed : mm/s")]
        public int HomeAcc
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
        [Category("DMS : Servo Info")]
        [Description("Maximum Jog Velocity : mm/s. Normally, Hand unit is 100, RB unit is 1.")]
        public double MaxJogVelocity
        {
            get { return m_MaxJogVel; }
            set { m_MaxJogVel = value; }
        }
        [Category("DMS : Servo Info")]
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
        [Browsable(false), XmlIgnore()]
        public override short AxisDec
        {
            get { return m_AxisPara.Dec; }
            set { m_AxisPara.Dec = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsCommConnected
        {
            get { return (/*m_Mp2300.IsConnected &&*/ m_Mp2300.IsLinked); }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsTryLink
        {
            get { return (m_Mp2300.IsTryLink); }
        }
        [Browsable(false), XmlIgnore()]
        public static ushort[] HomeInfoBuf
        {
            get { return m_HomeInfoBuf; }
            set { m_HomeInfoBuf = value; }
        }
        #endregion

        #region Constructor
        public ServoMotorMp2300()
        {
            this.Name = "__ Servo Motor";
            m_AxisPara = AxisInfoMP2300.CreateDefault();
            m_HomeInfo = HomeInfoMP2300.CreateDefault();
        }
        #endregion

        #region Methods
        public void EStop(bool state)
        {
            m_Mp2300.SetEstop((short)this.AxisId, state);
        }
        public void InitSeq()
        {
            SeqClearAxisErr.InitSeq();
            SeqHome.InitSeq();
            SeqSetCurPos.InitSeq();
            SeqSetHomeComp.InitSeq();
        }
        //public bool ClearFrames()
        //{
        //    return m_Mp2300.ClearFrames((short)this.AxisId);
        //}
        public bool IsCmdDone()
        {
            return m_Mp2300.IsCmdDone((short)this.AxisId);
        }
        public bool ClearStatus(bool state)
        {
            return m_Mp2300.ClearStatus((short)this.AxisId, state);
        }
        public int ClearAxisErr()
        {
            return SeqClearAxisErr.Do();
        }
        public int GetAxisErr()
        {
            int nErr = 0;
            if (!IsCommConnected)
            {
                nErr = (int)MP2300Error.errLink;
                return nErr;
            }
            if (!GetServoReady())
            {
                nErr = (int)MP2300Error.errServoReady;
                return nErr;
            }
            if ((nErr = m_Mp2300.GetError((short)this.AxisId)) > 0)
            {
                return nErr;
            }
            return nErr;
        }
        public AxisEventMP2300 GetAxisState()
        {
            return m_Mp2300.GetAxisState((short)this.AxisId);
        }
        public AxisSourceMP2300 GetAxisSource()
        {
            return m_Mp2300.GetAxisSource((short)this.AxisId);
        }
        public int Homing()
        {
            int nRv = -1;
            nRv = SeqHome.Do();
            return nRv;
        }
        public bool GetPosition(ref double rpos)
        {
            int pos = m_Mp2300.GetPosition((short)this.AxisId);
            rpos = Pulse2Len(pos);
            return true;
        }
        public int GetRawPosition() //MP2300의 mpIN_CUR_POSITION값을 바로 받아옴
        {
            return m_Mp2300.GetPosition((short)this.AxisId);
        }
        public double GetPosition()
        {
            double pos = 0.0;
            GetPosition(ref pos);
            return pos;
        }

        public double Pulse2Len(int pulse)
        {
            double len = 0.0;
            int nDeciaml = m_AxisPara.Decimal;
            MP2300CmdUnit unit = m_AxisPara.Unit;
            AxisType type = m_AxisPara.Type;

            switch (unit)
            {
                case MP2300CmdUnit.mm:
                case MP2300CmdUnit.degree:
                    {
                        len = (double)pulse / (double)nDeciaml;
                    }
                    break;
                case MP2300CmdUnit.pulse:
                    {
                        uint encoder = m_AxisPara.Encoder;
                        double ratio = m_AxisPara.Ratio;

                        if (type == AxisType.Normal)
                        {
                            len = (double)(pulse * ratio / (double)encoder);
                        }
                        else if (type == AxisType.RbGap)
                        {
                            double theta = m_AxisPara.Theta;
                            double radian = ratio * pulse / (double)encoder;
                            if (pulse == 0)
                            {
                                len = 0.0;
                            }
                            else
                            {
                                len = Radius * (Math.Cos(theta) - Math.Cos(theta + radian));
                            }
                        }
                    }
                    break;
            }
            return len;
        }
        public T Len2Pulse<T>(double len)
        {
            int pulse = 0;
            int nDecimal = m_AxisPara.Decimal;
            MP2300CmdUnit unit = m_AxisPara.Unit;
            AxisType type = m_AxisPara.Type;

            switch (unit)
            {
                case MP2300CmdUnit.mm:
                case MP2300CmdUnit.degree:
                    {
                        pulse = (int)(len * nDecimal);
                    }
                    break;
                case MP2300CmdUnit.pulse:
                    {
                        uint encoder = m_AxisPara.Encoder;
                        double ratio = m_AxisPara.Ratio;

                        if (type == AxisType.Normal)
                        {
                            pulse = (int)(encoder * len / ratio);
                        }
                        else if (type == AxisType.RbGap)
                        {
                            if (len > Radius * 2) len = Radius * 2;
                            else if (len < 0.0) len = 0.0;

                            double theta = m_AxisPara.Theta;
                            double radian = Math.Acos(Math.Cos(theta) - (len / Radius)) - theta;
                            pulse = (int)(encoder * radian / ratio);
                        }
                    }
                    break;
            }
            return (T)Convert.ChangeType(pulse, typeof(T));
        }
        public T Len2Pulse_Org<T>(double len)       //MP2300에 써주거나 읽어올때만 사용
        {
            int pulse = 0;
            int nDecimal = m_AxisPara.Decimal;
            pulse = (int)(len * nDecimal);
            return (T)Convert.ChangeType(pulse, typeof(T));
        }
        public double Pulse2Len_Org(int pulse)      //MP2300에 써주거나 읽어올때만 사용
        {
            double len = 0.0;
            int nDeciaml = m_AxisPara.Decimal;
            len = (double)pulse / (double)nDeciaml;
            return len;
        }
        public int GetControllerError()
        {
            return m_Mp2300.GetError((short)this.AxisId);
        }
        public override bool GetHomeSwitch()
        {
            return m_Mp2300.GetHomeSwitch((short)this.AxisId);
        }
        /// <summary>
        /// only for simulation
        /// </summary>
        /// <param name="state">only for simualtion</param>
        public void SetHomeSwitch(bool state)
        {
            m_Mp2300.SetHomeSwitch((short)this.AxisId, state);
        }
        public bool GetNegSwitch()
        {
            return m_Mp2300.GetNegSwitch((short)this.AxisId);
        }

        public bool GetPosSwitch()
        {
            return m_Mp2300.GetPosSwitch((short)this.AxisId);
        }
        public void SetHomeStart(bool state)
        {
            m_Mp2300.SetHomeStart((short)this.AxisId, state);
        }
        public bool GetHomeBusy()
        {
            return m_Mp2300.GetHomeBusy((short)this.AxisId);
        }
        public bool GetHomeComp()
        {
            return m_Mp2300.GetHomeComp((short)this.AxisId);
        }
        public void SetControlType(ControlType type)
        {
            m_Mp2300.SetControlType((short)this.AxisId, type);
        }
        public void SetPoint(int posNo)
        {
            m_Mp2300.SetPoint((short)this.AxisId, posNo);
        }
        public void SetActStartPoint(bool state)
        {
            m_Mp2300.SetActStartPoint((short)this.AxisId, state);
        }
        public bool GetActBusy()
        {
            return m_Mp2300.GetActBusy((short)this.AxisId);
        }
        public void SetActStartReference(bool state)
        {
            m_Mp2300.SetActStartReference((short)this.AxisId, state);
        }
        public void SetRefPosition(int pos)
        {
            m_Mp2300.SetRefPosition((short)this.AxisId, pos);
        }
        public void SetRefSpeed(int speed)
        {
            m_Mp2300.SetRefSpeed((short)this.AxisId, speed);
        }
        public void SetRefAcceleration(int acc)
        {
            m_Mp2300.SetRefAcceleration((short)this.AxisId, acc);
        }
        public void SetJogPlus(bool state)
        {
            m_Mp2300.SetJogPlus((short)this.AxisId, state);
        }
        public void SetJogMinus(bool state)
        {
            m_Mp2300.SetJogMinus((short)this.AxisId, state);
        }
        public void SetJogSpeed(int speed)
        {
            m_Mp2300.SetJogSpeed((short)this.AxisId, speed);
        }
        //private ushort[] HomeInfo2Buf()
        //{
        //    int dWord = 0;
        //    int index = 0;
        //    ushort[] buf = new ushort[20];

        //    dWord = (int)m_HomeInfo.Type;
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index+1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    index = 2;
        //    dWord = Len2Pulse<int>(m_HomeInfo.Vel);
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index+1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    index = 4;
        //    dWord = Len2Pulse<int>(m_HomeInfo.AppVel);
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    index = 6;
        //    dWord = Len2Pulse<int>(m_HomeInfo.CrpVel);
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    index = 8;
        //    dWord = m_HomeInfo.Acc;
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    index = 10;
        //    dWord = Len2Pulse<int>(m_HomeInfo.Dist);
        //    buf[index] = (ushort)(dWord & 0x0000FFFF);
        //    buf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

        //    return buf;
        //}
        private void HomeInfo2Buf()
        {
            int dWord = 0;
            int index = 0;

            index = (0 + this.AxisId * 20);
            dWord = (int)m_HomeInfo.Type;
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

            index = (2 + this.AxisId * 20);
            dWord = Len2Pulse_Org<int>(m_HomeInfo.Vel);
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

            index = (4 + this.AxisId * 20);
            dWord = Len2Pulse_Org<int>(m_HomeInfo.AppVel);
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

            index = (6 + this.AxisId * 20);
            dWord = Len2Pulse_Org<int>(m_HomeInfo.CrpVel);
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

            index = (8 + this.AxisId * 20);
            dWord = m_HomeInfo.Acc;
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);

            index = (10 + this.AxisId * 20);
            dWord = Len2Pulse_Org<int>(m_HomeInfo.Dist);
            m_HomeInfoBuf[index] = (ushort)(dWord & 0x0000FFFF);
            m_HomeInfoBuf[index + 1] = (ushort)((dWord >> 16) & 0x0000FFFF);
        }
        public void SetActStop(bool state)
        {
            m_Mp2300.SetActStop((short)this.AxisId, state);
        }
        public void SetAlarmReset(bool state)
        {
            m_Mp2300.SetAlarmReset((short)this.AxisId, state);
        }
        public void SetHomeStop(bool state)
        {
            m_Mp2300.SetHomeStop((short)this.AxisId, state);
        }
        public bool GetHomeStop()
        {
            return m_Mp2300.GetHomeStop((short)this.AxisId);
        }
        public bool ClearBit(bool servoOfF)
        {
            return m_Mp2300.ClearBit((short)this.AxisId, servoOfF);
        }
        public void SetPause(bool state)
        {
            m_Mp2300.SetPause((short)this.AxisId, state);
        }
        //public void SetPointInfo(ushort[][] buf)
        //{
        //    m_Mp2300.SetPointInfo(buf);
        //}
        public bool GetServoReady()
        {
            return m_Mp2300.GetServoReady((short)this.AxisId);
        }
        public void SetPosition(int data)
        {
            m_Mp2300.SetPosition((short)this.AxisId, data);
        }
        public void SetPositionRequest(bool state)
        {
            m_Mp2300.SetPositionRequest((short)this.AxisId, state);
        }
        public void SetHomeCompRequest(bool state)
        {
            m_Mp2300.SetHomeCompRequest((short)this.AxisId, state);
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(ServoMotorMp2300); }
        }
        public override void ServoOn(bool on)
        {
            m_Mp2300.ServoOn((short)this.AxisId, on);
        }
        public override bool GetServoOnState()
        {
            return m_Mp2300.GetServoOnState((short)this.AxisId);
        }

        public override double Len2Pulse(double len)
        {
            return Len2Pulse<double>(len);
        }

        public override double Pulse2Len(double pulse)
        {
            return Pulse2Len((int)pulse);
        }

        public override int StartVelMove(double velPulse)
        {
            SetControlType(ControlType.CtrlSpeed);
            //long vel = Len2Pulse<long>(this.AxisVel);
            int acc = AxisAcc;
            SetRefSpeed((int)velPulse);
            SetRefAcceleration(acc);
            SetActStartReference(true);
            return 0;
        }

        public override int StopVelMove()
        {
            SetControlType(ControlType.CtrlSpeed);
            SetActStartPoint(false);
            SetActStartReference(false);
            return 0;
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


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_Mp2300 = MP2300Ctl.Instance;
                this.HomeComp = false;
                SeqClearAxisErr = new SeqClearAxisErrMP2300(this);
                SeqHome = new SeqHomeMP2300(this);
                SeqSetCurPos = new SeqSetPosMP2300(this);
                SeqSetHomeComp = new SeqSetHomeCompMP2300(this);

                //1. 무조건 Designer에서 입력한 HomeInfo들로 MP2300에 써준다.
                //m_Mp2300.SetHomeInfo((short)this.AxisId, HomeInfo2Buf());

                //2. 나중에 다른 곳에서 HomeInfo를 비교하기 위해 HomeInfo를 버퍼에만 쓴다.
                //   다른 곳에서는 m_HomeInfoBuf를 가지고 비교하면 된다.
                if (m_HomeInfoBuf == null)
                {
                    m_HomeInfoBuf = new ushort[Dms.Ctl.MP2300.ORG_NUM];
                }

                HomeInfo2Buf();

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
                    _GenericCollection<ServoMotorMp2300> motors = m_Server.ComponentContainer.GetCollection<ServoMotorMp2300>();
                    foreach (ServoMotorMp2300 motor in motors)
                    {
                        if (!this.Equals(motor))
                        {
                            if (m_AxisId == motor.m_AxisId)
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
            if (m_Mp2300 != null) EStop(true);

            this.Initialized = false;

            return DmsErrors.Success;
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            //m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            //m_Tag.SetValue(tagDescriptor.VALUE, m_CurValue);
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

        private enum RbPosId
        {
            Wait = 0,
            Zero = 1
        }

        public override void UpdateTag()
        {
            string msg = string.Format("{0:f2}", GetPosition());
            m_Tag.SetValue(tagDescriptor.CURRENTPOS, msg);//khh090807
            m_Tag.SetValue(tagDescriptor.DETECT, GetHomeSwitch());
        }
        #endregion
    }
}

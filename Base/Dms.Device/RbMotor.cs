using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using Dms.Util.IODefine;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RbMotor : _Motor
    {
        #region Fields
        public Alarm ALM_DriverError = null;
        public Alarm ALM_CpOff = null;
        protected ushort m_CurSpeed = 0;
        #endregion

        #region Properties
        #endregion

        #region Cosntructor
        protected RbMotor() { }
        #endregion

        #region Virtuals
        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void TurnCw()
        {
            throw new NotImplementedException();
        }

        public override void TurnCcw()
        {
            throw new NotImplementedException();
        }

        public override bool IsStop()
        {
            throw new NotImplementedException();
        }

        public override bool IsTurnCw()
        {
            throw new NotImplementedException();
        }

        public override bool IsTurnCcw()
        {
            throw new NotImplementedException();
        }

        public override bool IsAlarm()
        {
            throw new NotImplementedException();
        }

        public override bool IsCpOn()
        {
            throw new NotImplementedException();
        }

        public override bool SetMotorAct(int act)
        {
            throw new NotImplementedException();
        }

        public override void SetSpeed(ushort speed)
        {
            throw new NotImplementedException();
        }

        public override void SetAcc(short acc)
        {
            throw new NotImplementedException();
        }

        public override int GetCurSpeed()
        {
            throw new NotImplementedException();
        }

        public override int GetMaxSpeed()
        {
            throw new NotImplementedException();
        }

        public override int GetMinSpeed()
        {
            throw new NotImplementedException();
        }

        public override void UpdateTag()
        {
            throw new NotImplementedException();
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Alarm GetDriverAlarm()
        {
            return ALM_DriverError;
        }

        public override Alarm GetCpAlarm()
        {
            return ALM_CpOff;
        }

        public override Type FamilyType
        {
            get { return typeof(RbMotor); }
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
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RbMotor_Io : RbMotor
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiCpOn = new IoDigitalInput();
        private IoDigitalOutput m_DoTurnCw = new IoDigitalOutput();
        private IoDigitalOutput m_DoTurnCcw = new IoDigitalOutput();
        private IoAnalogOutput m_AoSpeed = new IoAnalogOutput();

        protected ushort m_MaxSpeedAoValue = 0x4000;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTurnCw
        {
            get { return m_DoTurnCw; }
            set { m_DoTurnCw = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTurnCcw
        {
            get { return m_DoTurnCcw; }
            set { m_DoTurnCcw = value; }
        }
        [Category("DMS : Setting")]
        public IoAnalogOutput AoSpeed
        {
            get { return m_AoSpeed; }
            set { m_AoSpeed = value; }
        }

        [Category("DMS : Setting")]
        public ushort MaxSpeedAoValue
        {
            get { return m_MaxSpeedAoValue; }
            set { m_MaxSpeedAoValue = value; }
        }
        #endregion

        #region Constructor
        public RbMotor_Io()
        {
            this.Name = "__ Unit __ RbMotor";
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override bool IsTurnCw()
        {
            if (!this.Initialized) return false;

            return m_DoTurnCw.GetState();
        }

        public override bool IsTurnCcw()
        {
            if (!this.Initialized) return false;

            return m_DoTurnCcw.GetState();
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool brun = false;

            brun |= IsTurnCw();
            brun |= IsTurnCcw();

            return !brun;
        }

        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            return DiAlarm.GetState();
        }

        public override bool IsCpOn()
        {
            if (!this.Initialized) return false;

            return DiCpOn.GetState();
        }

        public override void Stop()
        {
            if (!this.Initialized) return;

            if (true == IsTurnCw())
            {
                DoTurnCw.SetState(false);
            }
            if (true == IsTurnCcw())
            {
                DoTurnCcw.SetState(false);
            }
        }

        public override void TurnCw()
        {
            if (!this.Initialized) return;

            if (true == IsTurnCcw())
            {
                DoTurnCcw.SetState(false);
            }
            if (!IsTurnCw())
            {
                DoTurnCw.SetState(true);
            }
        }

        public override void TurnCcw()
        {
            if (!this.Initialized) return;

            if (true == IsTurnCw())
            {
                DoTurnCw.SetState(false);
            }
            if (!IsTurnCcw())
            {
                DoTurnCcw.SetState(true);
            }
        }

        public override int GetCurSpeed()
        {
            return m_CurSpeed;
        }

        public override int GetMaxSpeed()
        {
            return 600;
        }

        public override int GetMinSpeed()
        {
            return 100;
        }

        public override void SetSpeed(ushort speed)
        {
            if (!this.Initialized) return;

            double gearRatio = 5.0;
            ushort maxSpeedAoValue = m_MaxSpeedAoValue;
            ushort adc = (ushort)((maxSpeedAoValue * speed) / (3000.0 / gearRatio));
            if (adc > maxSpeedAoValue) adc = maxSpeedAoValue;

            m_CurSpeed = speed;
            m_AoSpeed.SetState(adc);
        }

        public override void SetAcc(short acc)
        {

        }

        public override bool SetMotorAct(int act)
        {
            if (!this.Initialized) return false;

            RbMotorAct motorAct = (RbMotorAct)act;

            switch (motorAct)
            {
                case RbMotorAct.Noop:
                    break;
                case RbMotorAct.Stop:
                    Stop();
                    break;
                case RbMotorAct.Cw:
                    TurnCw();
                    break;
                case RbMotorAct.Ccw:
                    TurnCcw();
                    break;
            }

            return true;
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
            ok &= (DiAlarm != null);
            ok &= (DiCpOn != null);
            ok &= (DoTurnCw != null);
            ok &= (DoTurnCcw != null);


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
                ALM_DriverError = new Alarm(this.Name + " Driver Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


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
                    DiCpOn.SetState(true);
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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.CPON, DiCpOn.GetState());
            m_Tag.SetValue(tagDescriptor.CW, DoTurnCw.GetState());
            m_Tag.SetValue(tagDescriptor.CCW, DoTurnCcw.GetState());
            m_Tag.SetValue(tagDescriptor.STOP, IsStop());
            m_Tag.SetValue(tagDescriptor.SPEED, m_CurSpeed);
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RbMotor_Xqd : RbMotor
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiCpOn = new IoDigitalInput();
        private SlaveDigitalInput m_DiCpOn_Ec = new SlaveDigitalInput();
        private IoDigitalOutput m_DoTurnCw = new IoDigitalOutput();
        private IoDigitalOutput m_DoTurnCcw = new IoDigitalOutput();

        private IoAnalogInput m_AiCurrentRpm = new IoAnalogInput();
        private IoAnalogOutput m_AoTargetRpm = new IoAnalogOutput();
        private IoDigitalOutput m_DoRpmReq = new IoDigitalOutput();
        private IoDigitalInput m_DiRpmSet = new IoDigitalInput();

        private IoDigitalOutput m_DoSlowStop = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        [Category("DMS : Setting")]
        public SlaveDigitalInput DiCpOn_Ec
        {
            get { return m_DiCpOn_Ec; }
            set { m_DiCpOn_Ec = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTurnFw
        {
            get { return m_DoTurnCw; }
            set { m_DoTurnCw = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTurnBw
        {
            get { return m_DoTurnCcw; }
            set { m_DoTurnCcw = value; }
        }


        [Category("DMS : Setting")]
        public IoAnalogInput AiCurrentRpm
        {
            get { return m_AiCurrentRpm; }
            set { m_AiCurrentRpm = value; }
        }
        [Category("DMS : Setting")]
        public IoAnalogOutput AoTargetRpm
        {
            get { return m_AoTargetRpm; }
            set { m_AoTargetRpm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRpmReq
        {
            get { return m_DoRpmReq; }
            set { m_DoRpmReq = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiRpmSet
        {
            get { return m_DiRpmSet; }
            set { m_DiRpmSet = value; }
        }

        [Category("DMS : Setting (Optional)")]
        public IoDigitalOutput DoSlowStop
        {
            get { return m_DoSlowStop; }
            set { m_DoSlowStop = value; }
        }
        #endregion

        #region Constructor
        public RbMotor_Xqd()
        {
            this.Name = "__ Unit __ RbMotor __ Xqd";
        }
        #endregion

        #region Methods
        private short _GetCurrentRPM()
        {
            if (!m_Initialized) return 0;

            return m_AiCurrentRpm.GetState();
        }

        private bool _IsTurnCw()
        {
            if (!m_Initialized) return false;

            bool state;
            if (!m_MotorReverseAct)
                state = m_DoTurnCw.GetState();
            else
                state = m_DoTurnCcw.GetState();

            return state;
        }

        private bool _IsTurnCcw()
        {
            if (!m_Initialized) return false;

            bool state;
            if (!m_MotorReverseAct)
                state = m_DoTurnCcw.GetState();
            else
                state = m_DoTurnCw.GetState();

            return state;
        }

        private bool _IsStop()
        {
            if (!m_Initialized) return false;

            bool isRun = m_DoTurnCw.GetState() || m_DoTurnCcw.GetState();

            return !isRun;
        }

        private bool _IsAlarm()
        {
            if (!m_Initialized) return false;

            return m_DiAlarm.GetState();
        }

        private void _Stop()
        {
            if (!m_Initialized) return;

            m_DoTurnCw.SetState(false);
            m_DoTurnCcw.SetState(false);
        }

        private void QuickStop()
        {
            if (!m_Initialized) return;

            m_DoTurnCw.SetState(true);
            m_DoTurnCcw.SetState(true);
        }

        private void _TurnCw()
        {
            if (!m_Initialized) return;

            if (!m_MotorReverseAct)
            {
                m_DoTurnCw.SetState(true);
                m_DoTurnCcw.SetState(false);
            }
            else
            {
                m_DoTurnCw.SetState(false);
                m_DoTurnCcw.SetState(true);
            }
        }

        private void _TurnCcw()
        {
            if (!m_Initialized) return;

            if (!m_MotorReverseAct)
            {
                m_DoTurnCw.SetState(false);
                m_DoTurnCcw.SetState(true);
            }
            else
            {
                m_DoTurnCw.SetState(true);
                m_DoTurnCcw.SetState(false);
            }
        }

        private void _SetTargetRPM(ushort RPM)
        {
            if (!m_Initialized) return;

            m_AoTargetRpm.SetState(RPM);

            Thread t = new Thread(() => _ReqRpm());
            t.IsBackground = true;
            t.Name = m_Name + "Req RPM";
            t.Start();
        }

        private void _ReqRpm()
        {
            if (!m_Initialized) return;

            m_DoRpmReq.SetState(true);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 3000)
            {
                bool ok = m_DiRpmSet.GetState();

                if (ok) break;
            }
            sw.Stop();

            m_DoRpmReq.SetState(false);
        }
        #endregion

        #region Override
        public override bool IsTurnCw()
        {
            return _IsTurnCw();
        }
        public override bool IsTurnCcw()
        {
            return _IsTurnCcw();
        }

        public override bool IsStop()
        {
            return _IsStop();
        }

        public override bool IsAlarm()
        {
            return _IsAlarm();
        }

        public override bool IsCpOn()
        {
            if (!this.Initialized) return false;

            if (m_DiCpOn != null)
                return m_DiCpOn.GetState();
            else
                return m_DiCpOn_Ec.GetState();
        }

        public override void Stop()
        {
            _Stop();
        }

        public override void TurnCw()
        {
            _TurnCw();
        }

        public override void TurnCcw()
        {
            _TurnCcw();
        }

        public override int GetCurSpeed()
        {
            m_CurSpeed = (ushort)_GetCurrentRPM();
            return m_CurSpeed;
        }

        public override int GetMaxSpeed()
        {
            return 600;
        }

        public override int GetMinSpeed()
        {
            return 100;
        }

        public override void SetSpeed(ushort speed)
        {
            if (!this.Initialized) return;

            double gearRatio = 5.0;
            ushort rpm = (ushort)(speed * gearRatio);

            m_CurSpeed = speed;

            _SetTargetRPM(rpm);
        }

        public override void SetAcc(short acc)
        {
        }

        public override bool SetMotorAct(int act)
        {
            RbMotorAct motorAct = (RbMotorAct)act;

            switch (motorAct)
            {
                case RbMotorAct.Noop:
                    break;
                case RbMotorAct.Stop:
                    _Stop();
                    break;
                case RbMotorAct.Cw:
                    _TurnCw();
                    break;
                case RbMotorAct.Ccw:
                    _TurnCcw();
                    break;
            }

            return true;
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
            ok &= m_DiAlarm != null;
            ok &= (m_DiCpOn != null) || (m_DiCpOn_Ec != null);
            ok &= m_DoTurnCw != null;
            ok &= m_DoTurnCcw != null;

            ok &= m_AiCurrentRpm != null;
            ok &= m_AoTargetRpm != null;
            ok &= m_DoRpmReq != null;
            ok &= m_DiRpmSet != null;


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
                ALM_DriverError = new Alarm(this.Name + " Driver Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


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
                    if (m_DiCpOn != null)
                        m_DiCpOn.SetState(true);
                    if (m_DiCpOn_Ec != null)
                        m_DiCpOn_Ec.SetState(true);
                }
                if (m_DoSlowStop != null)
                {
                    m_DoSlowStop.SetState(true);
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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, _IsAlarm());
            m_Tag.SetValue(tagDescriptor.CPON, IsCpOn());

            m_Tag.SetValue(tagDescriptor.FW, _IsTurnCw());
            m_Tag.SetValue(tagDescriptor.BW, _IsTurnCcw());

            m_Tag.SetValue(tagDescriptor.STOP, _IsStop());
            m_Tag.SetValue(tagDescriptor.SPEED, GetCurSpeed());
            m_Tag.SetValue(tagDescriptor.MAXSPEED, GetMaxSpeed());
            m_Tag.SetValue(tagDescriptor.MINSPEED, GetMinSpeed());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RbMotor_Ec : RbMotor
    {
        #region Fields
        private SlaveBLDC m_SlaveBLDC = new SlaveBLDC();

        private bool m_FwIsCw = true;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveBLDC SlaveBLDC
        {
            get { return m_SlaveBLDC; }
            set { m_SlaveBLDC = value; }
        }
        #endregion

        #region Constructor
        public RbMotor_Ec()
        {
            this.Name = "__ Unit __ RbMotor";
        }
        #endregion

        #region Methods
        private double _GetLoadFactor()
        {
            if (!m_Initialized) return 0;

            return m_SlaveBLDC.GetLoadFactor();
        }

        private short _GetCurrentRPM()
        {
            if (!m_Initialized) return 0;

            return m_SlaveBLDC.GetCurrentRPM();
        }

        private bool _IsTurnFw()
        {
            if (!m_Initialized) return false;

            bool state;
            if (!m_MotorReverseAct)
            {
                state = m_SlaveBLDC.IsTurnFw();
            }
            else
            {
                state = m_SlaveBLDC.IsTurnBw();
            }

            return state;
        }

        private bool _IsTurnBw()
        {
            if (!m_Initialized) return false;

            bool state;
            if (!m_MotorReverseAct)
            {
                state = m_SlaveBLDC.IsTurnBw();
            }
            else
            {
                state = m_SlaveBLDC.IsTurnFw();
            }

            return state;
        }

        private bool _IsStop()
        {
            if (!m_Initialized) return false;

            bool isRun = m_SlaveBLDC.IsTurnFw() || m_SlaveBLDC.IsTurnBw();

            return !isRun;
        }

        private bool _IsAlarm()
        {
            if (!m_Initialized) return false;

            return m_SlaveBLDC.IsAlarm();
        }

        private void _Stop()
        {
            if (!m_Initialized) return;

            m_SlaveBLDC.SetRotateFw(false);
            m_SlaveBLDC.SetRotateBw(false);
        }

        private void QuickStop()
        {
            if (!m_Initialized) return;

            m_SlaveBLDC.SetRotateFw(true);
            m_SlaveBLDC.SetRotateBw(true);
        }

        private void _TurnFw()
        {
            if (!m_Initialized) return;

            if (!m_MotorReverseAct)
            {
                m_SlaveBLDC.SetRotateFw(true);
                m_SlaveBLDC.SetRotateBw(false);
            }
            else
            {
                m_SlaveBLDC.SetRotateFw(false);
                m_SlaveBLDC.SetRotateBw(true);
            }
        }

        private void _TurnBw()
        {
            if (!m_Initialized) return;

            if (!m_MotorReverseAct)
            {
                m_SlaveBLDC.SetRotateFw(false);
                m_SlaveBLDC.SetRotateBw(true);
            }
            else
            {
                m_SlaveBLDC.SetRotateFw(true);
                m_SlaveBLDC.SetRotateBw(false);
            }
        }

        private void _SetAccTime(double AccTime)
        {
            if (!m_Initialized) return;

            m_SlaveBLDC.SetAccTime((ushort)(AccTime * 10));
        }

        private void _SetDecTime(double DecTime)
        {
            if (!m_Initialized) return;

            m_SlaveBLDC.SetDecTime((ushort)(DecTime * 10));
        }

        private void _SetTargetRPM(ushort RPM)
        {
            if (!m_Initialized) return;

            m_SlaveBLDC.SetTargetRPM(RPM);
        }
        #endregion

        #region Override
        public override bool IsTurnCw()
        {
            if (m_FwIsCw)
                return _IsTurnFw();
            else
                return _IsTurnBw();
        }

        public override bool IsTurnCcw()
        {
            if (m_FwIsCw)
                return _IsTurnBw();
            else
                return _IsTurnFw();
        }

        public override bool IsStop()
        {
            return _IsStop();
        }

        public override bool IsAlarm()
        {
            return _IsAlarm();
        }

        public override bool IsCpOn()
        {
            return !_IsAlarm();
        }

        public override void Stop()
        {
            _Stop();
        }

        public override void TurnCw()
        {
            if (m_FwIsCw)
                _TurnFw();
            else
                _TurnBw();
        }

        public override void TurnCcw()
        {
            if (m_FwIsCw)
                _TurnBw();
            else
                _TurnFw();
        }

        public override int GetCurSpeed()
        {
            return m_CurSpeed;
        }

        public override int GetMaxSpeed()
        {
            return 600;
        }

        public override int GetMinSpeed()
        {
            return 100;
        }

        public override void SetSpeed(ushort speed)
        {
            double GearRatio = 50.0;
            ushort rpm = (ushort)(speed * GearRatio);
            _SetTargetRPM(rpm);
        }

        public override void SetAcc(short acc)
        {

        }

        public override bool SetMotorAct(int act)
        {
            if (!this.Initialized) return false;

            RbMotorAct motorAct = (RbMotorAct)act;

            switch (motorAct)
            {
                case RbMotorAct.Noop:
                    break;
                case RbMotorAct.Stop:
                    Stop();
                    break;
                case RbMotorAct.Cw:
                    TurnCw();
                    break;
                case RbMotorAct.Ccw:
                    TurnCcw();
                    break;
            }

            return true;
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
            ok &= m_SlaveBLDC != null;


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
                ALM_DriverError = new Alarm(this.Name + " Driver Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_CpOff = new Alarm(this.Name + " C/P Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, IsAlarm());
            m_Tag.SetValue(tagDescriptor.CPON, IsCpOn());
            m_Tag.SetValue(tagDescriptor.CW, IsTurnCw());
            m_Tag.SetValue(tagDescriptor.CCW, IsTurnCcw());
            m_Tag.SetValue(tagDescriptor.STOP, IsStop());
            m_Tag.SetValue(tagDescriptor.SPEED, GetCurSpeed());
        }
        #endregion
    }
}

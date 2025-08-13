using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Drawing.Design;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Ctl;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoMotor_Ec : ServoMotor
    {
        #region Fields
        private SlaveServo m_SlaveServo;
        private SlaveDigitalInput m_DiCpOn;
        private SlaveDigitalInput m_DiElbOn;
        private SlaveDigitalInput m_DiBrakeOn;

        private bool m_NowHoming = false;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public SlaveServo SlaveServo
        {
            get { return m_SlaveServo; }
            set { m_SlaveServo = value; }
        }
        [Category("DMS : Setting (Option)")]
        public SlaveDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        [Category("DMS : Setting (Option)")]
        public SlaveDigitalInput DiElbOn
        {
            get { return m_DiElbOn; }
            set { m_DiElbOn = value; }
        }
        [Category("DMS : Setting (Option)")]
        public SlaveDigitalInput DiBrakeOn
        {
            get { return m_DiBrakeOn; }
            set { m_DiBrakeOn = value; }
        }
        #endregion

        #region Constructor
        public ServoMotor_Ec()
        {
            this.Name = "__ Servo Motor";

            m_AxisPara = AxisInfo.CreateDefault();
            m_HomeInfo = HomeInfo.CreateDefault();
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

        public override bool GetHomeSwitch()
        {
            return m_SlaveServo.IsHomeSwitchDetected();
        }
        public override bool GetPosSwitch()
        {
            return m_SlaveServo.IsPositiveLimitSwitchDetected();
        }
        public override bool GetNegSwitch()
        {
            return m_SlaveServo.IsNegativeLimitSwitchDetected();
        }
        public override bool GetInMotion()
        {
            return !m_SlaveServo.IsDone();
        }
        public override bool IsCmdDone()
        {
            return m_SlaveServo.IsDone();
        }

        public override int StartRmove(double distance, double vel, short acc)
        {
            m_SlaveServo.SetAxisCommandMode(SlaveServo.AxisCommandMode.Position);
            return m_SlaveServo.StartMove_R(distance, vel, acc);
        }
        public override int StartSmove(double pos, double vel, double acc)
        {
            m_SlaveServo.SetAxisCommandMode(SlaveServo.AxisCommandMode.Position);
            return m_SlaveServo.StartMove_S(pos, vel, acc);
        }
        public override int StartATmove(double pos, double vel, short acc, short dec)
        {
            m_SlaveServo.SetAxisCommandMode(SlaveServo.AxisCommandMode.Position);
            return m_SlaveServo.StartMove_T(pos, vel, acc, dec);
        }

        public override int Homing()
        {
            //  Home 작업 실행 안했으면 작업 실행
            if (!m_NowHoming)
            {
                m_SlaveServo.SetAxisCommandMode(SlaveServo.AxisCommandMode.Position);

                //  BM Homing Parameter 넘겨야 함, 일단은 두고 Home Sensor, Limit Sensor 연결하면 작업 할 것.
                int m_Result = m_SlaveServo.StartHoming();

                if (m_Result == 0)          //  Homing 진입, 진행중으로 처리하여 -1 반환
                {
                    m_NowHoming = true;
                    return -1;
                }
                else                        //  Homing 진입 실패 시, 오류 코드 반환
                    return m_Result;
            }
            else
            {
                if (m_SlaveServo.IsDone())  //  Idle 상태 진입했으면, 0 반환
                {
                    m_HomeComp = true;
                    return 0;
                }
                else                        //  Homing중 일 경우, -1 반환
                    return -1;
            }
        }

        public void Stop()
        {
            m_SlaveServo.Stop();
        }

        public override void EStop()
        {
            //m_SlaveServo.EStop();
            m_SlaveServo.ServoOff();
        }

        public override AxisEvent GetAxisState()
        {
            return m_SlaveServo.AxisState();
        }

        public override AxisSource GetAxisSource()
        {
            AxisSource src = 0;

            if (GetHomeSwitch()) src |= AxisSource.StHomeSwitch;
            if (GetPosSwitch()) src |= AxisSource.StPosLimit;
            if (GetNegSwitch()) src |= AxisSource.StNegLimit;

            return src;
        }

        public override bool CheckAxisState()
        {
            bool err = false;
            err |= !GetServoOnState();
            err |= m_SlaveServo.IsAlarm();
            err |= GetNegSwitch();
            err |= GetPosSwitch();
            return !err;
        }

        public override bool ClearAxisErr()
        {
            return m_SlaveServo.AlarmReset() == 0;
        }

        public override double GetPosition()
        {
            return Pulse2Len(m_SlaveServo.CurrentPosition());
        }

        public override bool GetPosition(ref double pos)
        {
            double rpos = GetPosition();

            if (double.IsNaN(rpos))
                return false;

            pos = rpos;
            return true;
        }

        public override int SetPosition(double pos)
        {
            return m_SlaveServo.SetPosition(pos);
        }
        public override double GetActVelocity()
        {
            return m_SlaveServo.CurrentVelocity();
        }

        public override bool SetSyncControl(short slaveId, bool enable)
        {
            return m_SlaveServo.SetSync(slaveId, enable) == 0;
        }

        public override bool ClearStatus()
        {
            return true;
        }

        public override bool ClearFrames()
        {
            return true;
        }

        public override short GetControllerError()
        {
            return m_SlaveServo.ControllerError();
        }
        #endregion

        #region IServo Overrides
        public override void ServoOn(bool on)
        {
            if (on)
                m_SlaveServo.ServoOn();
            else
                m_SlaveServo.ServoOff();
        }

        public override bool GetServoOnState()
        {
            return m_SlaveServo.IsOn();
        }

        public override int StartVelMove(double velPulse)
        {
            m_SlaveServo.SetAxisCommandMode(SlaveServo.AxisCommandMode.Velocity);
            System.Threading.Thread.Sleep(10);  //  BM
            return m_SlaveServo.StartMove_V(velPulse, Len2Pulse(AxisAcc), Len2Pulse(AxisDec));
        }

        public override int StopVelMove()
        {
            return m_SlaveServo.Stop();
        }
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
            ok &= m_SlaveServo != null;

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
                m_SlaveServo.SetAdvController();
                m_HomeComp = false;

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
                    _GenericCollection<ServoMotor_Ec> motors = m_Server.ComponentContainer.GetCollection<ServoMotor_Ec>();
                    foreach (ServoMotor_Ec motor in motors)
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
            if (m_SlaveServo != null) EStop();

            m_Initialized = false;

            return DmsErrors.Success;
        }
        #endregion
    }
}

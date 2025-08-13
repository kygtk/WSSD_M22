using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using Dms.Util.IODefine;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ServoCvMotor : _Motor
    {
        #region Fields
        //private ServoMotor m_ServoMotor = null;
        private _ServoUnit m_ServoUnit = null;
        private _ServoMotor m_ServoMotor = null;

        public Alarm ALM_DriverError = null;
        public Alarm ALM_CpOff = null;
        private ushort m_CurSpeed = 0;
        private TagSetupCvInfo m_SetupCvInfo = null;
        private int m_MaxSpeed = 6000;
        private int m_MinSpeed = 600;

        private bool m_IsCw = false;
        private bool m_IsCcw = false;
        private bool m_IsOn = false;
        #endregion

        #region Properties
        //[Category("DMS : Setting")]
        //public ServoMotor ServoMotor
        //{
        //    get { return m_ServoMotor; }
        //    set { m_ServoMotor = value; }
        //}
        [Category("DMS : Setting")]
        public _ServoUnit ServoUnit
        {
            get { return m_ServoUnit; }
            set { m_ServoUnit = value; }
        }
        [Category("DMS : Setting")]
        public int MaxSpeed
        {
            get { return m_MaxSpeed; }
            set { m_MaxSpeed = value; }
        }
        [Category("DMS : Setting")]
        public int MinSpeed
        {
            get { return m_MinSpeed; }
            set { m_MinSpeed = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupCvInfo SetupCvInfo
        {
            get { return m_SetupCvInfo; }
            set { m_SetupCvInfo = value; }
        }
        #endregion

        #region Constructor
        public ServoCvMotor()
        {
            this.Name = "__ Unit ServoCvMotor";
        }
        #endregion

        #region Methods

        #endregion

        #region Override
        public override bool SetMotorAct(int act)
        {
            if (!this.Initialized) return false;

            CvMotorAct motorAct = (CvMotorAct)act;

            switch (motorAct)
            {
                case CvMotorAct.Noop:
                    break;
                case CvMotorAct.Stop:
                    if (this.IsTurnCw() || this.IsTurnCcw()) Stop();
                    break;
                case CvMotorAct.Fw:
                    if (!this.IsTurnCw()) TurnCw();
                    break;
                case CvMotorAct.Bw:
                    if (!this.IsTurnCcw()) TurnCcw();
                    break;
                case CvMotorAct.On:
                    if (!this.IsCpOn()) ServoCvOn();
                    break;
                case CvMotorAct.Off:
                    if (this.IsCpOn()) ServoCvOff();
                    break;
            }

            return true;
        }

        public override void SetSpeed(ushort speed)
        {
            if (!this.Initialized) return;

            if (m_CurSpeed != speed)
            {
                m_CurSpeed = speed;

                if (IsTurnCw()) TurnCw();
                else if (IsTurnCcw()) TurnCcw();
            }
        }

        public override void SetAcc(short acc)
        {
            if (!this.Initialized) return;

            if (m_ServoMotor.AxisAcc != acc)
            {
                m_ServoMotor.AxisAcc = acc;

                if (IsTurnCw()) TurnCw();
                else if (IsTurnCcw()) TurnCcw();
            }
        }

        public override bool IsTurnCw()
        {
            if (!this.Initialized) return false;

            return m_IsCw;
        }

        public override bool IsTurnCcw()
        {
            if (!this.Initialized) return false;

            return m_IsCcw;
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool brun = false;

            brun |= m_IsCw;
            brun |= m_IsCcw;

            return !brun;
        }

        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            //AxisSource source = m_ServoMotor.GetAxisSource();
            //AxisSourceStNone 
            //StHomeSwitch
            //StPosLimit 
            //StNegLimit 
            //StAmpFault 
            //StALimit 
            //StVLimit 
            //StXNegLimit 
            //StXPosLimit 
            //StErrorLimit
            //StPcCommand 
            //StOutofFrames  
            //StAmpPowerOnOff 
            //StAbsCommError 
            //StInpositonStatus
            //StRunStopCommand 
            //StCollisionState 
            //StPaustateState 
            return false;
        }

        public override bool IsCpOn()
        {
            if (!this.Initialized) return false;

            bool result = false;

            if (!m_Simul.Motion) result = m_ServoMotor.GetServoOnState();
            else result = m_IsOn;

            return result;
        }

        public override void Stop()
        {
            if (!this.Initialized) return;
            if (0 == m_ServoMotor.StopVelMove() && !m_Simul.Motion)
            {
                //m_ServoMotor.StopVelMove();
            }
            if (m_IsCw || m_IsCcw)
            {
                m_IsCw = false;
                m_IsCcw = false;
            }

            UpdateTag();
        }

        public void ServoCvOn()
        {
            if (!this.Initialized) return;

            //if (!m_Simul.Motion && !m_ServoMotor.GetServoOnState())
            //{
            //    m_ServoMotor.ServoOn(true);
            //}

            ServoUnit.ManualActionCmd = (int)RbtAction.ServoOn;

            if (m_Simul.Motion) m_IsOn = true;

            UpdateTag();
        }

        public void ServoCvOff()
        {
            if (!this.Initialized) return;

            //if (!m_Simul.Motion && m_ServoMotor.GetServoOnState())
            //{
            //    m_ServoMotor.EStop();
            //}

            ServoUnit.ManualActionCmd = (int)RbtAction.Estop;

            if (m_Simul.Motion) m_IsOn = false;

            UpdateTag();
        }

        public override void TurnCw()
        {
            if (!this.Initialized) return;

            double velRatio = m_SetupCvInfo.VelRatio;
            double gearRatio = m_SetupCvInfo.GearRatio;
            double diaMeter = m_SetupCvInfo.DiaMeter;

            m_ServoMotor.AxisRatio = diaMeter * Math.PI / gearRatio;

            double len = m_CurSpeed * velRatio / 60; // rpm/min -> rpm/sec

            double pulse = m_ServoMotor.Len2Pulse(len);

            if ((0 == m_ServoMotor.StartVelMove(pulse) && !m_Simul.Motion) || m_Simul.Motion)
            {
                m_IsCw = true;
                UpdateTag();
            }
        }

        public override void TurnCcw()
        {
            if (!this.Initialized) return;

            double velRatio = m_SetupCvInfo.VelRatio;
            double gearRatio = m_SetupCvInfo.GearRatio;
            double diaMeter = m_SetupCvInfo.DiaMeter;

            m_ServoMotor.AxisRatio = diaMeter * Math.PI / gearRatio;

            double len = m_CurSpeed * velRatio / 60; // rpm/min -> rpm/sec

            double pulse = m_ServoMotor.Len2Pulse(len);

            if ((0 == m_ServoMotor.StartVelMove(-pulse) && !m_Simul.Motion) || m_Simul.Motion)
            {
                m_IsCcw = true;
                UpdateTag();
            }
        }

        public override int GetCurSpeed()
        {
            return m_CurSpeed;
        }

        public override int GetMaxSpeed()
        {
            return m_MaxSpeed;
        }

        public override int GetMinSpeed()
        {
            return m_MinSpeed;
        }

        public override Alarm GetDriverAlarm()
        {
            return ALM_DriverError;
        }

        public override Alarm GetCpAlarm()
        {
            return ALM_CpOff;
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

            ok &= (m_ServoUnit != null);
            if (ok)
            {
                //jemoon : 무조건 0번째 축이어야 한다.
                m_ServoMotor = m_ServoUnit.GetServoMotor(0);
                ok &= (m_ServoMotor != null);
            }


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
                m_SetupCvInfo = new TagSetupCvInfo(this.Name, 1.0, 50.0, 60.0);
                SetupCvInfoProvider.Instance.InitFromDB(this.m_SetupCvInfo);

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
                    ServoCvOn();
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
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ALARM, IsAlarm());
            m_Tag.SetValue(tagDescriptor.CPON, IsCpOn());
            m_Tag.SetValue(tagDescriptor.FW, IsTurnCw());
            m_Tag.SetValue(tagDescriptor.BW, IsTurnCcw());
            m_Tag.SetValue(tagDescriptor.STOP, IsStop());
            m_Tag.SetValue(tagDescriptor.SPEED, m_CurSpeed);
            m_Tag.SetValue(tagDescriptor.MAXSPEED, m_MaxSpeed);
            m_Tag.SetValue(tagDescriptor.MINSPEED, m_MinSpeed);
        }
        #endregion
    }
}

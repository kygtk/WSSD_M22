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

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class DBMotor : _Motor
    {
        #region Fields
        private IoDigitalInput m_DiAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiCpOn = new IoDigitalInput();
        private IoDigitalOutput m_DoTurnCw = new IoDigitalOutput();
        private IoDigitalOutput m_DoTurnCcw = new IoDigitalOutput();
        private IoAnalogOutput m_AoSpeed = new IoAnalogOutput();

        public Alarm ALM_DriverError = null;
        public Alarm ALM_CpOff = null;
        private ushort m_CurSpeed = 0;
        #endregion

        #region Properties
        [Category("Setting")]
        public IoDigitalInput DiAlarm
        {
            get { return m_DiAlarm; }
            set { m_DiAlarm = value; }
        }
        [Category("Setting")]
        public IoDigitalInput DiCpOn
        {
            get { return m_DiCpOn; }
            set { m_DiCpOn = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoTurnCw
        {
            get { return m_DoTurnCw; }
            set { m_DoTurnCw = value; }
        }
        [Category("Setting")]
        public IoDigitalOutput DoTurnCcw
        {
            get { return m_DoTurnCcw; }
            set { m_DoTurnCcw = value; }
        }
        [Category("Setting")]
        public IoAnalogOutput AoSpeed
        {
            get { return m_AoSpeed; }
            set { m_AoSpeed = value; }
        }
        #endregion

        #region Constructor
        public DBMotor()
        {
            this.Name = "__ Unit __ DBMotor";
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
            ushort adc = (ushort)((0x4000 * speed) / (3000.0 / gearRatio));
            if (adc > 0x4000) adc = 0x4000;

            m_CurSpeed = speed;
            m_AoSpeed.SetState(adc);
        }

        public override void SetAcc(short acc)
        {

        }

        public override Alarm GetDriverAlarm()
        {
            return ALM_DriverError;
        }

        public override Alarm GetCpAlarm()
        {
            return ALM_CpOff;
        }

        public override bool SetMotorAct(int act)
        {
            if (!this.Initialized) return false;

            DBMotorAct motorAct = (DBMotorAct)act;

            switch (motorAct)
            {
                case DBMotorAct.Noop:
                    break;
                case DBMotorAct.Stop:
                    Stop();
                    break;
                case DBMotorAct.Cw:
                    TurnCw();
                    break;
                case DBMotorAct.Ccw:
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
            m_Tag.SetValue(tagDescriptor.ALARM, DiAlarm.GetState());
            m_Tag.SetValue(tagDescriptor.CPON, DiCpOn.GetState());
            m_Tag.SetValue(tagDescriptor.CW, DoTurnCw.GetState());
            m_Tag.SetValue(tagDescriptor.CCW, DoTurnCcw.GetState());
            m_Tag.SetValue(tagDescriptor.STOP, IsStop());
            m_Tag.SetValue(tagDescriptor.SPEED, m_CurSpeed);
        }
        #endregion
    }
}

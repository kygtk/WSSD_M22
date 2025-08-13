using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Cylinder : _Actuator
    {
        #region Fields
        protected bool m_SingleAct = false;           // 단동이냐 / 복동이냐
        protected bool m_VirtualFwConfirm = false;    // FW Confirm DI를 가상으로(Do State로) 할것인지?
        protected bool m_VirtualBwConfirm = false;    // BW Confirm DI를 가상으로(Do State로) 할것인지?
        protected bool FwActState = false;
        protected bool BwActState = false;
        protected float CylinderTime = 0.0f;
        protected string DisplayTime = "0";
        protected uint m_StartTicks = XFunc.GetTickCount();
        #endregion

        #region Properties
        [Category("DMS : Option"), Description("Single Acting Type or Double Acting Type")]
        public bool SingleAct
        {
            get { return m_SingleAct; }
            set { m_SingleAct = value; }
        }
        [Category("DMS : Option"), Description("No Use Fw confirm sensor")]
        public bool VirtualFwConfirm
        {
            get { return m_VirtualFwConfirm; }
            set { m_VirtualFwConfirm = value; }
        }
        [Category("DMS : Option"), Description("No Use Bw confirm sensor")]
        public bool VirtualBwConfirm
        {
            get { return m_VirtualBwConfirm; }
            set { m_VirtualBwConfirm = value; }
        }
        #endregion

        #region Constructor
        protected Cylinder() { }
        #endregion

        #region Methods
        public uint GetElapsedTicks()
        {
            return XFunc.GetTickCount() - m_StartTicks;
        }
        #endregion

        #region Virtuals
        protected virtual string GetTime()
        {
            throw new NotImplementedException();
        }
        public virtual void SetFw()
        {
            throw new NotImplementedException();
        }
        public virtual void SetBw()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsSetFw()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsSetBw()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsFwSensing()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsBwSensing()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsFwSensingOnly()
        {
            bool fw = IsFwSensing();
            bool bw = IsBwSensing();
            bool confirm = (fw && !bw);

            return confirm;
        }
        public virtual bool IsBwSensingOnly()
        {
            bool fw = IsFwSensing();
            bool bw = IsBwSensing();
            bool confirm = (!fw && bw);

            return confirm;
        }
        public virtual void SetStop()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Actuator Overrides
        public override int SetAct(ActuatorAct act)
        {
            if (IsInterlockCondition(act))
            {
                if (!m_GenInfos.AutoMode)
                {
                    DisplayNotice();
                }

                return 0;
            }

            m_RefAct = act;
            switch (act)
            {
                case ActuatorAct.Pos:
                    SetFw();
                    break;
                case ActuatorAct.Neg:
                    SetBw();
                    break;
                case ActuatorAct.Stop:
                    SetStop();
                    break;
            }

            return 0;
        }

        public override ActuatorAct GetCurAct()
        {
            if (IsFwSensingOnly())
            {
                m_CurAct = ActuatorAct.Pos;
            }
            else if (IsBwSensingOnly())
            {
                m_CurAct = ActuatorAct.Neg;
            }
            else
            {
                m_CurAct = ActuatorAct.Noop;
            }

            return m_CurAct;
        }

        public override ActuatorAct GetRefAct()
        {
            return m_RefAct;
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
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(Cylinder); }
        }
        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.ACT_STATUS, GetCurAct());
            m_Tag.SetValue(tagDescriptor.ACT_COMMAND, GetRefAct());
            m_Tag.SetValue(tagDescriptor.POS_SENSOR, IsFwSensing());
            m_Tag.SetValue(tagDescriptor.NEG_SENSOR, IsBwSensing());
            m_Tag.SetValue(tagDescriptor.CYLINDERTIME, GetTime());
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
    public class Cylinder_Io : Cylinder
    {
        #region Fields
        private IoDigitalInput m_DiFwSensor = new IoDigitalInput();
        private IoDigitalInput m_DiBwSensor = new IoDigitalInput();
        private IoDigitalOutput m_DoFwSolenoid = new IoDigitalOutput();
        private IoDigitalOutput m_DoBwSolenoid = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiFwSensor
        {
            get { return m_DiFwSensor; }
            set { m_DiFwSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiBwSensor
        {
            get { return m_DiBwSensor; }
            set { m_DiBwSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoFwSolenoid
        {
            get { return m_DoFwSolenoid; }
            set { m_DoFwSolenoid = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoBwSolenoid
        {
            get { return m_DoBwSolenoid; }
            set { m_DoBwSolenoid = value; }
        }
        #endregion

        #region Constructor
        public Cylinder_Io()
        {
            this.Name = "__ Cylinder";
        }
        #endregion

        #region Cylinder Methods

        protected override string GetTime()
        {
            if (m_DoFwSolenoid.GetState() && !m_DoBwSolenoid.GetState() && !m_DiFwSensor.GetState() && !FwActState)
            {
                FwActState = true;
                m_StartTicks = Common.XFunc.GetTickCount();

            }
            else if (!m_DoFwSolenoid.GetState() && m_DoBwSolenoid.GetState() && !m_DiBwSensor.GetState() && !BwActState)
            {
                BwActState = true;
                m_StartTicks = Common.XFunc.GetTickCount();

            }

            if (m_DoFwSolenoid.GetState() && !m_DoBwSolenoid.GetState() && m_DiFwSensor.GetState() && FwActState)
            {
                FwActState = false;
                CylinderTime = (float)GetElapsedTicks() / 1000;
                if (CylinderTime > 10000) CylinderTime = 10000;
                DisplayTime = string.Format("{0:f1}", CylinderTime);
            }
            else if (!m_DoFwSolenoid.GetState() && m_DoBwSolenoid.GetState() && m_DiBwSensor.GetState() && BwActState)
            {
                BwActState = false;
                CylinderTime = (float)GetElapsedTicks() / 1000;
                if (CylinderTime > 10000) CylinderTime = 10000;
                DisplayTime = string.Format("{0:f1}", CylinderTime);
            }
            else if (FwActState || BwActState)
            {
                return DisplayTime = "Move";
            }
            return DisplayTime + " sec";
        }

        public override void SetFw()
        {
            m_DoFwSolenoid.SetState(true);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(false);
            }

            if (AppConfig.Instance.Simul.Device)
            {
                System.Threading.Thread.Sleep(700);
                if (!m_VirtualFwConfirm)
                {
                    this.m_DiFwSensor.SetState(true);
                }

                if (!m_VirtualBwConfirm)
                {
                    this.m_DiBwSensor.SetState(false);
                }
            }
        }

        public override void SetBw()
        {
            m_DoFwSolenoid.SetState(false);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(true);
            }

            if (AppConfig.Instance.Simul.Device)
            {
                //  System.Threading.Thread.Sleep(1000);
                if (!m_VirtualFwConfirm)
                {
                    this.m_DiFwSensor.SetState(false);
                }

                if (!m_VirtualBwConfirm)
                {
                    this.m_DiBwSensor.SetState(true);
                }
            }
        }

        public override bool IsSetFw()
        {
            return m_DoFwSolenoid.GetState();
        }

        public override bool IsSetBw()
        {
            return m_DoBwSolenoid.GetState();
        }

        public override bool IsFwSensing()
        {
            bool confirm = false;

            if (!m_VirtualFwConfirm)
            {
                confirm = m_DiFwSensor.GetState();
            }
            else
            {
                confirm = m_DoFwSolenoid.GetState();
            }
            return confirm;
        }

        public override bool IsBwSensing()
        {
            bool confirm = false;
            if (m_DiBwSensor != null && !m_SingleAct)
            {
                if (!m_VirtualBwConfirm)
                {
                    confirm = m_DiBwSensor.GetState();
                }
                else
                {
                    confirm = m_DoBwSolenoid.GetState();
                }
            }
            else
            {//2009.12.02 kimgun
                if (!m_VirtualBwConfirm)
                {
                    confirm = !m_DiFwSensor.GetState();
                }
                else
                {
                    confirm = !m_DoFwSolenoid.GetState();
                }
            }
            return confirm;
        }

        public override void SetStop()
        {
            m_DoFwSolenoid.SetState(false);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(false);
            }
        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////

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
            ok &= (m_DoFwSolenoid != null);
            ok &= (m_DoBwSolenoid != null || m_SingleAct);
            ok &= (m_DiFwSensor != null || m_VirtualFwConfirm);
            ok &= (m_DiBwSensor != null || m_VirtualBwConfirm); //khh090814 m_VirtualBwConfirm ->Fw로 변경.


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

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Cylinder_Ec : Cylinder
    {
        #region Fields
        private SlaveDigitalInput m_DiFwSensor = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiBwSensor = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoFwSolenoid = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoBwSolenoid = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiFwSensor
        {
            get { return m_DiFwSensor; }
            set { m_DiFwSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiBwSensor
        {
            get { return m_DiBwSensor; }
            set { m_DiBwSensor = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoFwSolenoid
        {
            get { return m_DoFwSolenoid; }
            set { m_DoFwSolenoid = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoBwSolenoid
        {
            get { return m_DoBwSolenoid; }
            set { m_DoBwSolenoid = value; }
        }
        #endregion

        #region Constructor
        public Cylinder_Ec()
        {
            this.Name = "__ Cylinder";
        }
        #endregion

        #region Cylinder Methods

        protected override string GetTime()
        {
            if (m_DoFwSolenoid.GetState() && !m_DoBwSolenoid.GetState() && !m_DiFwSensor.GetState() && !FwActState)
            {
                FwActState = true;
                m_StartTicks = Common.XFunc.GetTickCount();

            }
            else if (!m_DoFwSolenoid.GetState() && m_DoBwSolenoid.GetState() && !m_DiBwSensor.GetState() && !BwActState)
            {
                BwActState = true;
                m_StartTicks = Common.XFunc.GetTickCount();

            }

            if (m_DoFwSolenoid.GetState() && !m_DoBwSolenoid.GetState() && m_DiFwSensor.GetState() && FwActState)
            {
                FwActState = false;
                CylinderTime = (float)GetElapsedTicks() / 1000;
                if (CylinderTime > 10000) CylinderTime = 10000;
                DisplayTime = string.Format("{0:f1}", CylinderTime);
            }
            else if (!m_DoFwSolenoid.GetState() && m_DoBwSolenoid.GetState() && m_DiBwSensor.GetState() && BwActState)
            {
                BwActState = false;
                CylinderTime = (float)GetElapsedTicks() / 1000;
                if (CylinderTime > 10000) CylinderTime = 10000;
                DisplayTime = string.Format("{0:f1}", CylinderTime);
            }
            else if (FwActState || BwActState)
            {
                return DisplayTime = "Move";
            }
            return DisplayTime + " sec";
        }

        public override void SetFw()
        {
            m_DoFwSolenoid.SetState(true);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(false);
            }

            if (AppConfig.Instance.Simul.Device)
            {
                System.Threading.Thread.Sleep(700);
                if (!m_VirtualFwConfirm)
                {
                    this.m_DiFwSensor.SetState(true);
                }

                if (!m_VirtualBwConfirm)
                {
                    this.m_DiBwSensor.SetState(false);
                }
            }
        }

        public override void SetBw()
        {
            m_DoFwSolenoid.SetState(false);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(true);
            }

            if (AppConfig.Instance.Simul.Device)
            {
                //  System.Threading.Thread.Sleep(1000);
                if (!m_VirtualFwConfirm)
                {
                    this.m_DiFwSensor.SetState(false);
                }

                if (!m_VirtualBwConfirm)
                {
                    this.m_DiBwSensor.SetState(true);
                }
            }
        }

        public override bool IsSetFw()
        {
            return m_DoFwSolenoid.GetState();
        }

        public override bool IsSetBw()
        {
            return m_DoBwSolenoid.GetState();
        }

        public override bool IsFwSensing()
        {
            bool confirm = false;

            if (!m_VirtualFwConfirm)
            {
                confirm = m_DiFwSensor.GetState();
            }
            else
            {
                confirm = m_DoFwSolenoid.GetState();
            }
            return confirm;
        }

        public override bool IsBwSensing()
        {
            bool confirm = false;
            if (m_DiBwSensor != null && !m_SingleAct)
            {
                if (!m_VirtualBwConfirm)
                {
                    confirm = m_DiBwSensor.GetState();
                }
                else
                {
                    confirm = m_DoBwSolenoid.GetState();
                }
            }
            else
            {//2009.12.02 kimgun
                if (!m_VirtualBwConfirm)
                {
                    confirm = !m_DiFwSensor.GetState();
                }
                else
                {
                    confirm = !m_DoFwSolenoid.GetState();
                }
            }
            return confirm;
        }

        public override void SetStop()
        {
            m_DoFwSolenoid.SetState(false);

            if (!m_SingleAct)
            {
                m_DoBwSolenoid.SetState(false);
            }
        }
        #endregion

        #region Override
        public override DmsErrors Initialize()
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.
            ////////////////////////////////////////////////////////////////////////////////////////

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
            ok &= (m_DoFwSolenoid != null);
            ok &= (m_DoBwSolenoid != null || m_SingleAct);
            ok &= (m_DiFwSensor != null || m_VirtualFwConfirm);
            ok &= (m_DiBwSensor != null || m_VirtualBwConfirm); //khh090814 m_VirtualBwConfirm ->Fw로 변경.


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

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : AlarmResetSwitch Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using System.Collections;

namespace Dms.Device
{
    public class AlarmResetSwitch : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorLampSwitch tagDescriptor = new TagDescriptorLampSwitch();
        #endregion

        #region Fields
        protected bool m_UseLogicalSwitch = false;
        protected bool m_LogicalSwitchPushed = false;
        protected bool m_LogicalLampState = false;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        public bool UseLogicalSwitch
        {
            get { return m_UseLogicalSwitch; }
            set { m_UseLogicalSwitch = value; }
        }
        #endregion

        #region Cosntructor
        protected AlarmResetSwitch() { }
        #endregion

        #region Methods
        public virtual bool IsPushed()
        {
            throw new NotImplementedException();
        }
        public virtual void SetSwitchPushed(bool on)
        {
            throw new NotImplementedException();
        }
        public virtual void SetLampState(Lamp lamp)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Overrides
        public override Type FamilyType
        {
            get { return typeof(AlarmResetSwitch); }
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
            throw new NotImplementedException();
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AlarmResetSwitch_Io : AlarmResetSwitch
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiAlarmResetSwitch = new IoDigitalInput();
        private IoDigitalOutput m_DoAlarmResetLamp = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiAlarmResetSwitch
        {
            get { return m_DiAlarmResetSwitch; }
            set { m_DiAlarmResetSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoAlarmResetLamp
        {
            get { return m_DoAlarmResetLamp; }
            set { m_DoAlarmResetLamp = value; }
        }
        #endregion

        #region Constructor
        public AlarmResetSwitch_Io()
        {
            this.Name = "__ Alarm Reset";
        }
        #endregion

        #region Methods

        public bool GetLampState()
        {
            if (!m_Initialized) return false;

            if (m_UseLogicalSwitch) return m_LogicalLampState;
            else
            {
                if (null == m_DoAlarmResetLamp) return false;
                if (!m_DoAlarmResetLamp.Initialized) return false;

                return m_DoAlarmResetLamp.GetState();
            }
        }
        #endregion

        #region Override
        public override bool IsPushed()
        {
            if (!this.Initialized) return false;

            bool pushed = false;

            if (m_UseLogicalSwitch)
            {
                pushed = m_LogicalSwitchPushed;
            }
            else
            {
                pushed = m_DiAlarmResetSwitch.GetState();
                if (pushed && m_Simul.Device)
                {
                    //m_DiAlarmResetSwitch.SetState(false);
                }
            }

            return pushed;
        }

        public override void SetSwitchPushed(bool on)
        {
            if (m_UseLogicalSwitch)
            {
                m_LogicalSwitchPushed = on;
            }
            else if (m_Simul.Device && on)
            {
                m_DiAlarmResetSwitch.SetPulse(on, 500);
            }
        }

        public override void SetLampState(Lamp lamp)
        {
            if (!m_UseLogicalSwitch)
            {
                if (!this.Initialized) return;
                if (null == m_DoAlarmResetLamp) return;
                if (!m_DoAlarmResetLamp.Initialized) return;
            }

            switch (lamp)
            {
                case Lamp.Off:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = false;
                        else m_DoAlarmResetLamp.SetState(false);
                    }
                    break;
                case Lamp.On:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = true;
                        else m_DoAlarmResetLamp.SetState(true);
                    }
                    break;
                case Lamp.Toggle:
                    break;
            }
        }

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
            ok &= (m_UseLogicalSwitch || (m_DiAlarmResetSwitch != null));


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.LAMP, GetLampState());
        }
        #endregion 
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class AlarmResetSwitch_Ec : AlarmResetSwitch
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiAlarmResetSwitch = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoAlarmResetLamp = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiAlarmResetSwitch
        {
            get { return m_DiAlarmResetSwitch; }
            set { m_DiAlarmResetSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoAlarmResetLamp
        {
            get { return m_DoAlarmResetLamp; }
            set { m_DoAlarmResetLamp = value; }
        }
        #endregion

        #region Constructor
        public AlarmResetSwitch_Ec()
        {
            this.Name = "__ Alarm Reset";
        }
        #endregion

        #region Methods
        public override bool IsPushed()
        {
            if (!this.Initialized) return false;

            bool pushed = false;

            if (m_UseLogicalSwitch)
            {
                pushed = m_LogicalSwitchPushed;
            }
            else
            {
                pushed = m_DiAlarmResetSwitch.GetState();
                if (pushed && m_Simul.Device)
                {
                    //m_DiAlarmResetSwitch.SetState(false);
                }
            }

            return pushed;
        }

        public override void SetSwitchPushed(bool on)
        {
            if (m_UseLogicalSwitch)
            {
                m_LogicalSwitchPushed = on;
            }
            else if (m_Simul.Device && on)
            {
                m_DiAlarmResetSwitch.SetPulse(on, 500);
            }
        }

        public override void SetLampState(Lamp lamp)
        {
            if (!m_UseLogicalSwitch)
            {
                if (!this.Initialized) return;
                if (null == m_DoAlarmResetLamp) return;
                if (!m_DoAlarmResetLamp.Initialized) return;
            }

            switch (lamp)
            {
                case Lamp.Off:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = false;
                        else m_DoAlarmResetLamp.SetState(false);
                    }
                    break;
                case Lamp.On:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = true;
                        else m_DoAlarmResetLamp.SetState(true);
                    }
                    break;
                case Lamp.Toggle:
                    break;
            }
        }

        public bool GetLampState()
        {
            if (!m_Initialized) return false;

            if (m_UseLogicalSwitch) return m_LogicalLampState;
            else
            {
                if (null == m_DoAlarmResetLamp) return false;
                if (!m_DoAlarmResetLamp.Initialized) return false;

                return m_DoAlarmResetLamp.GetState();
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
            ok &= (m_UseLogicalSwitch || (m_DiAlarmResetSwitch != null));


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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.LAMP, GetLampState());
        }
        #endregion 
    }
}

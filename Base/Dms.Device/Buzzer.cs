///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : Buzzer Class
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Buzzer : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorBuzzer tagDescriptor = new TagDescriptorBuzzer();
        #endregion

        #region Fields
        protected int m_AlarmMelodyNumber = 1;
        protected bool m_UseLogicalSwitch = false;
        protected bool m_LogicalSwitchPushed = false;
        protected bool m_LogicalLampState = false;
        #endregion

        #region Properties
        [Category("DMS : Option")]
        public int AlarmMelodyNumber
        {
            get { return m_AlarmMelodyNumber; }
            set
            {
                if (value < 1) value = 1;
                else if (value > 4) value = 4;

                m_AlarmMelodyNumber = value;
            }
        }
        [Category("DMS : Option")]
        public bool UseLogicalSwitch
        {
            get { return m_UseLogicalSwitch; }
            set { m_UseLogicalSwitch = value; }
        }
        #endregion

        #region Cosntructor
        protected Buzzer() { }
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
        public virtual bool IsMelodyOn()
        {
            throw new NotImplementedException();
        }
        public virtual void MelodyAllOff()
        {
            throw new NotImplementedException();
        }
        public virtual void SetLampState(Lamp lamp)
        {
            throw new NotImplementedException();
        }
        public virtual bool GetLampState()
        {
            throw new NotImplementedException();
        }
        public virtual void SetMelodyState(int melodyNumber, Melody state)
        {
            throw new NotImplementedException();
        }
        public virtual Melody GetMelodyState(int melodyNumber)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(Buzzer); }
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
    public class Buzzer_Io : Buzzer
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private IoDigitalInput m_DiBuzzerOffSwitch = new IoDigitalInput();
        private IoDigitalOutput m_DoBuzzerOffLamp = new IoDigitalOutput();
        private IoDigitalOutput m_DoMelody1 = new IoDigitalOutput();
        private IoDigitalOutput m_DoMelody2 = new IoDigitalOutput();
        private IoDigitalOutput m_DoMelody3 = new IoDigitalOutput();
        private IoDigitalOutput m_DoMelody4 = new IoDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiBuzzerOffSwitch
        {
            get { return m_DiBuzzerOffSwitch; }
            set { m_DiBuzzerOffSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoBuzzerOffLamp
        {
            get { return m_DoBuzzerOffLamp; }
            set { m_DoBuzzerOffLamp = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMelody1
        {
            get { return m_DoMelody1; }
            set { m_DoMelody1 = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMelody2
        {
            get { return m_DoMelody2; }
            set { m_DoMelody2 = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMelody3
        {
            get { return m_DoMelody3; }
            set { m_DoMelody3 = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoMelody4
        {
            get { return m_DoMelody4; }
            set { m_DoMelody4 = value; }
        }
        #endregion

        #region Constructor
        public Buzzer_Io()
        {
            this.Name = "__ Buzzer";
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
                pushed = m_DiBuzzerOffSwitch.GetState();
                if (pushed && m_Simul.Device)
                {
                    //m_DiBuzzerOffSwitch.SetState(false);
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
                m_DiBuzzerOffSwitch.SetPulse(on, 500);
            }
        }

        public override bool IsMelodyOn()
        {
            bool on = false;
            if (m_DoMelody1 != null) on |= m_DoMelody1.GetState();
            if (m_DoMelody2 != null) on |= m_DoMelody2.GetState();
            if (m_DoMelody3 != null) on |= m_DoMelody3.GetState();
            if (m_DoMelody4 != null) on |= m_DoMelody4.GetState();

            return on;
        }

        public override void MelodyAllOff()
        {
            if (m_DoMelody1 != null) m_DoMelody1.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody2.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody3.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody4.SetState(false);
        }

        public override void SetLampState(Lamp lamp)
        {
            if (!this.Initialized) return;

            if (!m_UseLogicalSwitch)
            {
                if (null == m_DoBuzzerOffLamp) return;
                if (!m_DoBuzzerOffLamp.Initialized) return;
            }

            switch (lamp)
            {
                case Lamp.Off:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = false;
                        else m_DoBuzzerOffLamp.SetState(false);
                    }
                    break;
                case Lamp.On:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = true;
                        else m_DoBuzzerOffLamp.SetState(true);
                    }
                    break;
                case Lamp.Toggle:
                    break;
            }
        }

        public override bool GetLampState()
        {
            if (!m_Initialized) return false;

            if (m_UseLogicalSwitch) return m_LogicalLampState;
            else
            {
                if (null == m_DoBuzzerOffLamp) return false;
                if (!m_DoBuzzerOffLamp.Initialized) return false;

                return m_DoBuzzerOffLamp.GetState();
            }
        }

        public override void SetMelodyState(int melodyNumber, Melody state)
        {
            bool on = state == Melody.On;
            switch (melodyNumber)
            {
                case 1:
                    if (m_DoMelody1 != null) m_DoMelody1.SetState(on);
                    break;
                case 2:
                    if (m_DoMelody2 != null) m_DoMelody2.SetState(on);
                    break;
                case 3:
                    if (m_DoMelody3 != null) m_DoMelody3.SetState(on);
                    break;
                case 4:
                    if (m_DoMelody4 != null) m_DoMelody4.SetState(on);
                    break;
            }
        }
        public override Melody GetMelodyState(int melodyNumber)
        {
            Melody state = Melody.Off;
            switch (melodyNumber)
            {
                case 1:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 2:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 3:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 4:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
            }

            return state;
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
            ok &= (m_UseLogicalSwitch || (m_DiBuzzerOffSwitch != null));


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

            if (m_DoMelody1 != null) m_Tag.SetValue(tagDescriptor.MELODY1, m_DoMelody1.GetState());
            if (m_DoMelody2 != null) m_Tag.SetValue(tagDescriptor.MELODY2, m_DoMelody2.GetState());
            if (m_DoMelody3 != null) m_Tag.SetValue(tagDescriptor.MELODY3, m_DoMelody3.GetState());
            if (m_DoMelody4 != null) m_Tag.SetValue(tagDescriptor.MELODY4, m_DoMelody4.GetState());
        }
        #endregion
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Buzzer_Ec : Buzzer
    {
        #region Tag Descriptor
        #endregion

        #region Fields
        private SlaveDigitalInput m_DiBuzzerOffSwitch = new SlaveDigitalInput();
        private SlaveDigitalOutput m_DoBuzzerOffLamp = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoMelody1 = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoMelody2 = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoMelody3 = new SlaveDigitalOutput();
        private SlaveDigitalOutput m_DoMelody4 = new SlaveDigitalOutput();
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public SlaveDigitalInput DiBuzzerOffSwitch
        {
            get { return m_DiBuzzerOffSwitch; }
            set { m_DiBuzzerOffSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoBuzzerOffLamp
        {
            get { return m_DoBuzzerOffLamp; }
            set { m_DoBuzzerOffLamp = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoMelody1
        {
            get { return m_DoMelody1; }
            set { m_DoMelody1 = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoMelody2
        {
            get { return m_DoMelody2; }
            set { m_DoMelody2 = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoMelody3
        {
            get { return m_DoMelody3; }
            set { m_DoMelody3 = value; }
        }
        [Category("DMS : I/O Setting")]
        public SlaveDigitalOutput DoMelody4
        {
            get { return m_DoMelody4; }
            set { m_DoMelody4 = value; }
        }
        #endregion

        #region Constructor
        public Buzzer_Ec()
        {
            this.Name = "__ Buzzer";
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
                pushed = m_DiBuzzerOffSwitch.GetState();
                if (pushed && m_Simul.Device)
                {
                    //m_DiBuzzerOffSwitch.SetState(false);
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
                m_DiBuzzerOffSwitch.SetPulse(on, 500);
            }
        }

        public override bool IsMelodyOn()
        {
            bool on = false;
            if (m_DoMelody1 != null) on |= m_DoMelody1.GetState();
            if (m_DoMelody2 != null) on |= m_DoMelody2.GetState();
            if (m_DoMelody3 != null) on |= m_DoMelody3.GetState();
            if (m_DoMelody4 != null) on |= m_DoMelody4.GetState();

            return on;
        }

        public override void MelodyAllOff()
        {
            if (m_DoMelody1 != null) m_DoMelody1.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody2.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody3.SetState(false);
            if (m_DoMelody1 != null) m_DoMelody4.SetState(false);
        }

        public override void SetLampState(Lamp lamp)
        {
            if (!this.Initialized) return;

            if (!m_UseLogicalSwitch)
            {
                if (null == m_DoBuzzerOffLamp) return;
                if (!m_DoBuzzerOffLamp.Initialized) return;
            }

            switch (lamp)
            {
                case Lamp.Off:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = false;
                        else m_DoBuzzerOffLamp.SetState(false);
                    }
                    break;
                case Lamp.On:
                    {
                        if (m_UseLogicalSwitch) m_LogicalLampState = true;
                        else m_DoBuzzerOffLamp.SetState(true);
                    }
                    break;
                case Lamp.Toggle:
                    break;
            }
        }

        public override bool GetLampState()
        {
            if (!m_Initialized) return false;

            if (m_UseLogicalSwitch) return m_LogicalLampState;
            else
            {
                if (null == m_DoBuzzerOffLamp) return false;
                if (!m_DoBuzzerOffLamp.Initialized) return false;

                return m_DoBuzzerOffLamp.GetState();
            }
        }

        public override void SetMelodyState(int melodyNumber, Melody state)
        {
            bool on = state == Melody.On;
            switch (melodyNumber)
            {
                case 1:
                    if (m_DoMelody1 != null) m_DoMelody1.SetState(on);
                    break;
                case 2:
                    if (m_DoMelody2 != null) m_DoMelody2.SetState(on);
                    break;
                case 3:
                    if (m_DoMelody3 != null) m_DoMelody3.SetState(on);
                    break;
                case 4:
                    if (m_DoMelody4 != null) m_DoMelody4.SetState(on);
                    break;
            }
        }
        public override Melody GetMelodyState(int melodyNumber)
        {
            Melody state = Melody.Off;
            switch (melodyNumber)
            {
                case 1:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 2:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 3:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
                case 4:
                    state = m_DoMelody1.GetState() ? Melody.On : Melody.Off;
                    break;
            }

            return state;
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
            ok &= (m_UseLogicalSwitch || (m_DiBuzzerOffSwitch != null));


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

            if (m_DoMelody1 != null) m_Tag.SetValue(tagDescriptor.MELODY1, m_DoMelody1.GetState());
            if (m_DoMelody2 != null) m_Tag.SetValue(tagDescriptor.MELODY2, m_DoMelody2.GetState());
            if (m_DoMelody3 != null) m_Tag.SetValue(tagDescriptor.MELODY3, m_DoMelody3.GetState());
            if (m_DoMelody4 != null) m_Tag.SetValue(tagDescriptor.MELODY4, m_DoMelody4.GetState());
        }
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.30
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
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LampSwitch : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorLampSwitch tagDescriptor = new TagDescriptorLampSwitch();
        #endregion

        #region Fields
        private IoDigitalInput m_DiSwitch = new IoDigitalInput();
        private IoDigitalOutput m_DoLamp = new IoDigitalOutput();
        private Lamp m_LampCommand = Lamp.Off;
        #endregion

        #region Properties
        [Category("DMS : I/O Setting")]
        public IoDigitalInput DiSwitch
        {
            get { return m_DiSwitch; }
            set { m_DiSwitch = value; }
        }
        [Category("DMS : I/O Setting")]
        public IoDigitalOutput DoLamp
        {
            get { return m_DoLamp; }
            set { m_DoLamp = value; }
        }
        [Browsable(false), XmlIgnore()]
        public Lamp LampCommand
        {
            get { return m_LampCommand; }
            set { m_LampCommand = value; }
        }
        #endregion

        #region Constructor
        public LampSwitch()
        {
            this.Name = "__ Switch";
        }
        #endregion

        #region Methods
        public bool IsPushed()
        {
            if (!this.Initialized) return false;

            bool pushed = m_DiSwitch.GetState();
            if (pushed && m_Simul.Device)
            {
                //m_DiAlarmResetSwitch.SetState(false);
            }

            return pushed;
        }

        public void SetLampState(Lamp lamp)
        {
            if (!this.Initialized) return;
            if (!m_DoLamp.Initialized) return;

            switch (lamp)
            {
                case Lamp.Off:
                    m_DoLamp.SetState(false);
                    break;
                case Lamp.On:
                    m_DoLamp.SetState(true);
                    break;
                case Lamp.Toggle:
                    break;
            }

            m_LampCommand = lamp;
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(LampSwitch); }
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
            ok &= (m_DiSwitch != null);


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
            if (m_DoLamp != null)
            {
                if (m_DoLamp.Initialized)
                {
                    m_Tag.SetValue(tagDescriptor.LAMP, m_DoLamp.GetState());
                }
            }
        }
        #endregion 
    }
}

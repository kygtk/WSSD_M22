using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using Dms.Ctl;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Heater : _Heater
    {
        #region Fields
        private IoDigitalInput m_DiTprAlarm = new IoDigitalInput();
        private IoDigitalInput m_DiTprMcOn = new IoDigitalInput();
        private IoDigitalInput m_DiSafetyRelayOn = new IoDigitalInput();

        private IoDigitalOutput m_DoTprOn = new IoDigitalOutput();
        private IoDigitalOutput m_DoTprMcOn = new IoDigitalOutput();
        private IoDigitalOutput m_DoTankSafetyRelayReset = new IoDigitalOutput();

        private TagSetupInfo m_SetupHeaterUse;
        private TagSetupInfo m_SetupHeaterTemp;
        private TagSetupInfo m_SetupHeaterOverTemp;
        private TagSetupInfo m_SetupHeaterMarginTemp;
        private TagSetupInfo m_SetupHeaterDelay;

        private Alarm ALM_TprError = null;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiTprAlarm
        {
            get { return m_DiTprAlarm; }
            set { m_DiTprAlarm = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiTprMcOn
        {
            get { return m_DiTprMcOn; }
            set { m_DiTprMcOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiSafetyRelayOn
        {
            get { return m_DiSafetyRelayOn; }
            set { m_DiSafetyRelayOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTprOn
        {
            get { return m_DoTprOn; }
            set { m_DoTprOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTprMcOn
        {
            get { return m_DoTprMcOn; }
            set { m_DoTprMcOn = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoTankSafetyRelayReset
        {
            get { return m_DoTankSafetyRelayReset; }
            set { m_DoTankSafetyRelayReset = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsUse
        {
            get
            {
                return m_SetupHeaterUse.GetValue<bool>();
            }
        }
        [Browsable(false), XmlIgnore()]
        public double SetupHeaterTemp
        {
            get { return m_SetupHeaterTemp.GetValue<double>(); }
        }
        [Browsable(false), XmlIgnore()]
        public double SetupHeaterOverTemp
        {
            get { return m_SetupHeaterOverTemp.GetValue<double>(); }
        }
        [Browsable(false), XmlIgnore()]
        public double SetupHeaterMarginTemp
        {
            get { return m_SetupHeaterMarginTemp.GetValue<double>(); }
        }
        [Browsable(false), XmlIgnore()]
        public int SetupHeaterDelay
        {
            get { return m_SetupHeaterDelay.GetValue<int>(); }
        }

        #endregion

        #region Constructor
        public Heater()
        {
            this.Name = "__ Heater";
        }
        #endregion

        #region Methods
        public bool SetHeaterAct(HeaterAct act)
        {
            if (!this.Initialized) return false;

            switch (act)
            {
                case HeaterAct.Noop:
                    break;
                case HeaterAct.Off:
                    Off();
                    break;
                case HeaterAct.On:
                    On();
                    break;

                case HeaterAct.Reset:
                    Reset();
                    break;
            }

            return true;
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
            ok &= (DiTprAlarm != null);
            ok &= (DoTprOn != null);
            ok &= (DoTprMcOn != null);


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
                ALM_TprError = new Alarm(this.Name + "TPR Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupGenInfoProvider setupGenInfoProvider = SetupGenInfoProvider.Instance;
                m_SetupHeaterUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupGenInfoProvider.InitFromDB(m_SetupHeaterUse);

                m_SetupHeaterTemp = new TagSetupInfo(this.Name + " Temp", OptionType.None, OptionFormat.Float, UnitType.Celsius, "40.0");
                setupGenInfoProvider.InitFromDB(m_SetupHeaterTemp);

                m_SetupHeaterOverTemp = new TagSetupInfo(this.Name + " Over Temp", OptionType.None, OptionFormat.Float, UnitType.Celsius, "45.0");
                setupGenInfoProvider.InitFromDB(m_SetupHeaterOverTemp);

                m_SetupHeaterMarginTemp = new TagSetupInfo(this.Name + " Margin Temp", OptionType.None, OptionFormat.Float, UnitType.Celsius, "3.0");
                setupGenInfoProvider.InitFromDB(m_SetupHeaterMarginTemp);

                m_SetupHeaterDelay = new TagSetupInfo(this.Name + " Delay", OptionType.None, OptionFormat.Digit, UnitType.msec, "1000");
                setupGenInfoProvider.InitFromDB(m_SetupHeaterDelay);
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
            m_Tag.SetValue(tagDescriptor.ALARM, IsAlarm());
            m_Tag.SetValue(tagDescriptor.ON, IsOn());
            m_Tag.SetValue(tagDescriptor.OFF, IsOff());
        }

        public override void On()
        {
            m_DoTprMcOn.SetState(true);
            m_DoTprOn.SetState(true);
        }

        public override void Off()
        {
            m_DoTprMcOn.SetState(false);
            m_DoTprOn.SetState(false);
        }

        public void Reset()
        {
            m_DoTankSafetyRelayReset.SetPulse(true, 1000);
        }

        public override bool IsAlarm()
        {
            return (m_DiTprAlarm.GetState());
        }

        public override bool IsOn()
        {
            return (m_DoTprOn.GetState() && m_DoTprMcOn.GetState());
        }

        public override bool IsOff()
        {
            return !m_DoTprOn.GetState() || !m_DoTprMcOn.GetState();
        }
        #endregion
    }
}

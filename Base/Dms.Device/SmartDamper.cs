using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Data;
using System.Xml.Serialization;
using Dms.Util.IODefine;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class SmartDamper : _DevicePeer
    {
        #region Tag Descriptor
        TagDescriptorSmartDamper tagDescriptor = new TagDescriptorSmartDamper();
        #endregion

        #region Fields
        private Gauge_Ap m_GaugePressure;
        private Gauge_Ap m_GaugeValveAngle;

        private SmartDamperMode m_SV_Mode = SmartDamperMode.Auto;
        private ushort m_SV_Pressure = 0;
        private ushort m_SV_PressureHys = 0;
        private ushort m_SV_ValveAngle = 0;
        #endregion

        #region Properties
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugePressure
        {
            get { return m_GaugePressure; }
            set
            {
                m_GaugePressure = value;
                if (m_GaugePressure != null)
                {
                    m_GaugePressure.Peer = this;
                }
            }
        }
        [Category("DMS : Gauge")]
        public Gauge_Ap GaugeValveAngle
        {
            get { return m_GaugeValveAngle; }
            set
            {
                m_GaugeValveAngle = value;
                if (m_GaugeValveAngle != null)
                {
                    m_GaugeValveAngle.Peer = this;
                }
            }
        }
        #endregion

        #region Constructor
        public SmartDamper()
        {
            m_Name = "__ SmartDamper";
            m_PeerType = PeerType.SmartDamper;
        }
        #endregion

        #region Methods
        public SmartDamperMode GetSmartDamperMode()
        {
            if (!m_Initialized) return SmartDamperMode.Error;

            return m_SlaveAP.SmartDamper_GetSmartDamperMode();
        }

        public ushort GetTargetPressure()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.SmartDamper_GetTargetPressure();
        }

        public ushort GetTargetPressureHysteresis()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.SmartDamper_GetTargetPressureHysteresis();
        }

        public ushort GetCurrentValveAngle()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.SmartDamper_GetCurrentValveAngle();
        }

        public ushort GetCurrentPressure()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.SmartDamper_GetCurrentPressure();
        }

        public ushort GetAlarmCode()
        {
            if (!m_Initialized) return 0;

            return m_SlaveAP.SmartDamper_GetAlarmCode();
        }

        public void SetSmartDamperMode(SmartDamperMode mode)
        {
            if (!m_Initialized) return;

            if (m_SlaveAP.SmartDamper_SetSmartDamperMode(mode))
                m_SV_Mode = mode;
        }

        public void SetTargetPressure(ushort value)
        {
            if (!m_Initialized) return;

            if (m_SlaveAP.SmartDamper_SetTargetPressure(value))
                m_SV_Pressure = value;
        }

        public void SetTargetPressureHysteresis(ushort value)
        {
            if (!m_Initialized) return;

            if (m_SlaveAP.SmartDamper_SetTargetPressureHysteresis(value))
                m_SV_PressureHys = value;
        }

        public void SetTargetValveAngle(ushort value)
        {
            if (!m_Initialized) return;

            if (m_SlaveAP.SmartDamper_SetTargetValveAngle(value))
                m_SV_ValveAngle = value;
        }
        #endregion

        #region Overrides
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
            m_Tag.SetValue(tagDescriptor.PAIRING, IsPaired());

            m_Tag.SetValue(tagDescriptor.PV_MODE, GetSmartDamperMode());
            m_Tag.SetValue(tagDescriptor.PV_TGT_PRESSURE, GetTargetPressure());
            m_Tag.SetValue(tagDescriptor.PV_TGT_HYSTERESIS, GetTargetPressureHysteresis());
            m_Tag.SetValue(tagDescriptor.PV_CUR_VALVEANGLE, GetCurrentValveAngle());
            m_Tag.SetValue(tagDescriptor.PV_CUR_PRESSURE, GetCurrentPressure());
            m_Tag.SetValue(tagDescriptor.PV_ALARMCODE, GetAlarmCode());

            m_Tag.SetValue(tagDescriptor.SV_MODE, m_SV_Mode);
            m_Tag.SetValue(tagDescriptor.SV_TGT_PRESSURE, m_SV_Pressure);
            m_Tag.SetValue(tagDescriptor.SV_TGT_HYSTERESIS, m_SV_PressureHys);
            m_Tag.SetValue(tagDescriptor.SV_TGT_VALVEANGLE, m_SV_ValveAngle);
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
            ok &= m_SlaveAP != null;


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
                m_SlaveAP.SetAdvController();
                m_SlaveAP.SetPeer(PeerType.SmartDamper);


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

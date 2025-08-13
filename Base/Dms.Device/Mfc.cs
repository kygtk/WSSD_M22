///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.03
// Author       : Y.S.Lee
// Description  : Mfc control for On/OFF function
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Mfc : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorMfc tagDescriptor = new TagDescriptorMfc();
        #endregion

        #region Fields
        private CalibrationType m_CalibrationType = CalibrationType.ByDesigner;
        private Gauge m_Gauge;
        private IoAnalogOutput m_AoGaugeFlowSet = new IoAnalogOutput();
        private IoDigitalOutput m_DoMfcOn = new IoDigitalOutput();

        private static CalibrationProvider m_CalibrationProvider = null;
        private TagCalibrationInfo m_Info = null;

        private static TagSetupInfo m_SetupMfcFlowInterlockRage = null;
        //private TagSetupInfo m_SetupMfcFullScale;
        private double m_SetValue;
        private bool m_IsAlarm;

        private double m_AdcMax = 0;
        private double m_AdcMin = 0;
        private double m_RealMax = 0;
        private double m_RealMin = 0;
        #endregion

        #region Properties
        [Category("DMS :Calibration Setting")]
        public CalibrationType CalibrationType
        {
            get { return m_CalibrationType; }
            set { m_CalibrationType = value; }
        }

        [Category("DMS :Setting")]
        public Gauge Gauge
        {
            get { return m_Gauge; }
            set { m_Gauge = value; }
        }
        [Category("DMS :Setting")]
        public IoAnalogOutput AoFlowSet
        {
            get { return m_AoGaugeFlowSet; }
            set { m_AoGaugeFlowSet = value; }
        }
        [Category("DMS :Setting")]
        public IoDigitalOutput DoMfcOn
        {
            get { return m_DoMfcOn; }
            set { m_DoMfcOn = value; }
        }
        [Category("DMS :Calibration Setting")]
        public double AoAdcMax
        {
            get { return m_AdcMax; }
            set { m_AdcMax = value; }
        }
        [Category("DMS :Calibration Setting")]
        public double AoAdcMin
        {
            get { return m_AdcMin; }
            set { m_AdcMin = value; }
        }
        [Category("DMS :Calibration Setting")]
        public double AoRealMax
        {
            get { return m_RealMax; }
            set { m_RealMax = value; }
        }
        [Category("DMS :Calibration Setting")]
        public double AoRealMin
        {
            get { return m_RealMin; }
            set { m_RealMin = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagCalibrationInfo Info
        {
            get { return m_Info; }
            set { m_Info = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double SettingValue
        {
            get { return m_SetValue; }
            set { m_SetValue = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsAlarm
        {
            get
            {
                m_IsAlarm = ((DiffValue() > 10));
                return m_IsAlarm;
            }
            //set { m_IsAlarm = value; }
        }

        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupMfcFlowInterlockRage
        {
            get { return m_SetupMfcFlowInterlockRage; }
            set { m_SetupMfcFlowInterlockRage = value; }
        }

        //[Browsable(false), XmlIgnore()]
        //public TagSetupInfo SetupMfcFullScale
        //{
        //    get { return m_SetupMfcFullScale; }
        //    set { m_SetupMfcFullScale = value; }
        //}      
        #endregion

        #region Constructor
        public Mfc()
        {
            this.Name = "__ Mfc Control";
        }
        #endregion

        #region Methods
        public double GetFlow()
        {
            return m_Gauge.CurValue;
        }

        public bool UpdateFromStorage()
        {
            if (this.Initialized == false || m_CalibrationType != CalibrationType.ByDataBase) return false;

            return m_CalibrationProvider.UpdateFromDB(m_Info.Name);
        }

        private bool WriteToStorage(TagCalibrationInfo info)
        {
            if (this.Initialized == false || m_CalibrationType != CalibrationType.ByDataBase) return false;

            return m_CalibrationProvider.UpdateToDB(info);
        }

        public void SetGaugeInfo(TagCalibrationInfo info)
        {
            if (this.Initialized == false || m_CalibrationType != CalibrationType.ByDataBase) return;

            WriteToStorage(info);
        }

        public void SetFlow(double flow)
        {
            short diffAdc = 0;
            short diffReal = 0;

            ushort value = 0;

            if (m_CalibrationType == CalibrationType.ByDataBase)
            {

                diffAdc = (short)(m_Info.AdcMax - m_Info.AdcMin);
                diffReal = (short)(m_Info.RealMax - m_Info.RealMin);

                value = (ushort)(((flow - m_Info.RealMin) * ((double)diffAdc / diffReal)) + (double)m_Info.AdcMin);
            }
            else
            {
                diffAdc = (short)(m_AdcMax - m_AdcMin);
                diffReal = (short)(m_RealMax - m_RealMin);

                value = (ushort)(((flow - m_RealMin) * ((double)diffAdc / diffReal)) + (double)m_AdcMin);
            }


            m_AoGaugeFlowSet.SetState(value);

            m_SetValue = flow;
        }

        public bool SetMfcOn(MFCAct act)
        {
            if (this.Initialized == false) return false;

            switch (act)
            {
                case MFCAct.OFF:
                    m_DoMfcOn.SetState(false);
                    break;
                case MFCAct.ON:
                    m_DoMfcOn.SetState(true);
                    break;
            }

            return true;
        }

        private bool IsMfcFlowRateInterlock()
        {
            bool result = false;

            double curValue = m_Gauge.CurValue;
            double setValue = m_SetValue;
            double interlockRange = Convert.ToDouble(m_SetupMfcFlowInterlockRage.Val);

            double min = m_SetValue - (m_SetValue * interlockRange / 100);
            double max = m_SetValue + (m_SetValue * interlockRange / 100);

            if (curValue < min || curValue > max) result = true;

            return result;
        }

        private double DiffValue()
        {
            return Math.Abs(m_SetValue - m_Gauge.CurValue);
        }
        #endregion

        #region Override
        public override void CreateTag(Dms.Common.DeviceTags tagContainer)
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
            m_Tag.SetValue(tagDescriptor.ON, m_DoMfcOn.GetState());
            m_Tag.SetValue(tagDescriptor.OFF, !m_DoMfcOn.GetState());
            m_Tag.SetValue(tagDescriptor.SETVAL, m_SetValue.ToString());
            m_Tag.SetValue(tagDescriptor.CURVAL, m_Gauge.CurValue);
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
            ok &= (m_AoGaugeFlowSet != null);
            ok &= (m_Gauge != null);
            //ok &= (m_DoMfcOn != null);


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
                //m_SetupMfcFullScale = new TagSetupInfo(this.Name + " Full Scale", OptionType.None, OptionFormat.Float, UnitType.SCCM, "10");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupMfcFullScale);
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);

                m_SetupMfcFlowInterlockRage = new TagSetupInfo(this.Name + "MFC Flowrate Interlock Range", OptionType.None, OptionFormat.Digit, UnitType.Percent, "10");
                #endregion

                if (m_CalibrationType == CalibrationType.ByDataBase)
                {
                    m_CalibrationProvider = CalibrationProvider.Instance;
                    m_Info = new TagCalibrationInfo(GaugeType.MfcFlow, this.Name);
                    m_CalibrationProvider.InitFromDB(m_Info);
                }
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
                    m_CalibrationProvider = CalibrationProvider.Instance;
                    m_Info = new TagCalibrationInfo(GaugeType.MfcFlow, this.Name);
                    m_CalibrationProvider.InitFromDB(m_Info);
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Util.IODefine;
using System.Collections;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class ShinkoDryCleaner : _DryCleaner
    {
        #region Tag Descriptor
        protected TagDescriptorShinkoDryCleaner tagDescriptor = new TagDescriptorShinkoDryCleaner();
        #endregion

        #region Fields
        private IoDigitalInput m_DiBlowerRun = new IoDigitalInput();
        private IoDigitalInput m_DiPreFilterError = new IoDigitalInput();
        private IoDigitalInput m_DiHepaFilterError = new IoDigitalInput();
        private IoDigitalInput m_DiInverterError = new IoDigitalInput();
        //private IoDigitalInput m_DiBlower1Error = new IoDigitalInput();   //090330_LSB_Spare로 바뀜
        //private IoDigitalInput m_DiBlower2Error = new IoDigitalInput();
        private IoDigitalInput m_DiWaterLeak = new IoDigitalInput();
        private IoDigitalInput m_DiTempError = new IoDigitalInput();
        private IoDigitalInput m_DiPresError = new IoDigitalInput();
        private IoDigitalInput m_DiEmo = new IoDigitalInput();

        private IoDigitalOutput m_DoEmo = new IoDigitalOutput();
        private IoDigitalOutput m_DoRun = new IoDigitalOutput();
        private IoDigitalOutput m_DoReady = new IoDigitalOutput();

        private Gauge m_GaugePressure1;
        private Gauge m_GaugePressure2;
        private Gauge m_GaugeVaccum1;
        private Gauge m_GaugeVaccum2;
        private Gauge m_GaugeTemperature;

        public Alarm ALM_PreFilterError = null;
        public Alarm ALM_HepaFilterError = null;
        public Alarm ALM_InverterError = null;
        //public Alarm ALM_Blower1Error = null;
        //public Alarm ALM_Blower2Error = null;
        public Alarm ALM_WaterLeak = null;
        public Alarm ALM_TempError = null;
        public Alarm ALM_PresError = null;
        public Alarm ALM_Emo = null;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public IoDigitalInput DiBlowerRun
        {
            get { return m_DiBlowerRun; }
            set { m_DiBlowerRun = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiPreFilterError
        {
            get { return m_DiPreFilterError; }
            set { m_DiPreFilterError = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiHepaFilterError
        {
            get { return m_DiHepaFilterError; }
            set { m_DiHepaFilterError = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiInverterError
        {
            get { return m_DiInverterError; }
            set { m_DiInverterError = value; }
        }
        //[Category("DMS : Setting")]
        //public IoDigitalInput DiBlower1Error
        //{
        //    get { return m_DiBlower1Error; }
        //    set { m_DiBlower1Error = value; }
        //}
        //[Category("DMS : Setting")]
        //public IoDigitalInput DiBlower2Error
        //{
        //    get { return m_DiBlower2Error; }
        //    set { m_DiBlower2Error = value; }
        //}
        [Category("DMS : Setting")]
        public IoDigitalInput DiWaterLeak
        {
            get { return m_DiWaterLeak; }
            set { m_DiWaterLeak = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiTempError
        {
            get { return m_DiTempError; }
            set { m_DiTempError = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiPresError
        {
            get { return m_DiPresError; }
            set { m_DiPresError = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalInput DiEmo
        {
            get { return m_DiEmo; }
            set { m_DiEmo = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoEmo
        {
            get { return m_DoEmo; }
            set { m_DoEmo = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoRun
        {
            get { return m_DoRun; }
            set { m_DoRun = value; }
        }
        [Category("DMS : Setting")]
        public IoDigitalOutput DoReady
        {
            get { return m_DoReady; }
            set { m_DoReady = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugePressure1
        {
            get { return m_GaugePressure1; }
            set { m_GaugePressure1 = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugePressure2
        {
            get { return m_GaugePressure2; }
            set { m_GaugePressure2 = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeVaccum1
        {
            get { return m_GaugeVaccum1; }
            set { m_GaugeVaccum1 = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeVaccum2
        {
            get { return m_GaugeVaccum2; }
            set { m_GaugeVaccum2 = value; }
        }
        [Category("DMS : Setting")]
        public Gauge GaugeTemperature
        {
            get { return m_GaugeTemperature; }
            set { m_GaugeTemperature = value; }
        }
        #endregion

        #region Constructor
        public ShinkoDryCleaner()
        {
            this.Name = "__ Dry Cleaner";
        }
        #endregion

        //#region Methods
        //public bool SetShinkoDryCleanerAct(ShinkoDryCleanerAct act)
        //{
        //    if (!this.Initialized) return false;

        //    switch (act)
        //    {
        //        case ShinkoDryCleanerAct.Noop:
        //            break;
        //        case ShinkoDryCleanerAct.Run:
        //            Run();
        //            break;
        //        case ShinkoDryCleanerAct.Stop:
        //            Stop();
        //            break;
        //    }

        //    return true;
        //} 
        //#endregion

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
            ok &= (DiBlowerRun != null);
            ok &= (DiPreFilterError != null);
            ok &= (DiHepaFilterError != null);
            ok &= (DiInverterError != null);
            //ok &= (DiBlower1Error != null);
            //ok &= (DiBlower2Error != null);
            ok &= (DiWaterLeak != null);
            ok &= (DiTempError != null);
            ok &= (DiPresError != null);
            ok &= (DiEmo != null);
            ok &= (DoEmo != null);
            ok &= (DoRun != null);
            ok &= (DoReady != null);
            //ok &= (AiPressure1 != null);
            //ok &= (AiPressure2 != null);
            //ok &= (AiVaccum1 != null);
            //ok &= (AiVaccum2 != null);
            //ok &= (AiTemperature != null);


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
                ALM_PreFilterError = new Alarm(this.Name + " Pre Filter Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_HepaFilterError = new Alarm(this.Name + " Hepa Filter Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_InverterError = new Alarm(this.Name + " Inverter Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_Blower1Error = new Alarm(this.Name + " Blower1 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_Blower2Error = new Alarm(this.Name + " Blower2 Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_WaterLeak = new Alarm(this.Name + " Water Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_TempError = new Alarm(this.Name + " Temperature Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PresError = new Alarm(this.Name + " Pressure Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_Emo = new Alarm(this.Name + " EMO Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                //m_SetupShinkoDryCleanerUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupShinkoDryCleanerUse);


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
                    DiInverterError.SetState(false);
                    //DiBlower1Error.SetState(false);
                    //DiBlower2Error.SetState(false);
                    DiEmo.SetState(false);
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
            m_Tag.SetValue(tagDescriptor.PREFILTER_ALARM, DiPreFilterError.GetState());
            m_Tag.SetValue(tagDescriptor.HEPAFILTER_ALARM, DiHepaFilterError.GetState());
            m_Tag.SetValue(tagDescriptor.INVERTER_ALARM, DiInverterError.GetState());
            //m_Tag.SetValue(tagDescriptor.BLOWER1_ALARM, DiBlower1Error.GetState());
            //m_Tag.SetValue(tagDescriptor.BLOWER2_ALARM, DiBlower2Error.GetState());
            m_Tag.SetValue(tagDescriptor.WATERLEAK_ALARM, DiWaterLeak.GetState());
            m_Tag.SetValue(tagDescriptor.TEMP_ALARM, DiTempError.GetState());
            m_Tag.SetValue(tagDescriptor.PRES_ALARM, DiPresError.GetState());
            m_Tag.SetValue(tagDescriptor.EMO_ALARM, DiEmo.GetState());
            m_Tag.SetValue(tagDescriptor.RUN, DoRun.GetState());
            m_Tag.SetValue(tagDescriptor.STOP, !DoRun.GetState());
            m_Tag.SetValue(tagDescriptor.POWERON, DoReady.GetState());
            m_Tag.SetValue(tagDescriptor.POWEROFF, !DoReady.GetState());
            m_Tag.SetValue(tagDescriptor.EMO_ALARM, DoEmo.GetState());
        }

        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool alarm = false;
            //alarm |= m_DiBlower1Error.GetState();
            //alarm |= m_DiBlower2Error.GetState();
            alarm |= m_DiEmo.GetState();
            alarm |= m_DiHepaFilterError.GetState();
            alarm |= m_DiInverterError.GetState();
            alarm |= m_DiPreFilterError.GetState();
            alarm |= m_DiTempError.GetState();
            alarm |= m_DiWaterLeak.GetState();

            return alarm;
        }

        public override bool IsRun()
        {
            if (!this.Initialized) return false;

            bool bRun = false;

            bRun = m_DiBlowerRun.GetState();

            return bRun;
            //return m_DiBlowerRun.GetState();
        }

        public override bool IsStop()
        {
            if (!this.Initialized) return false;

            bool bRun = false;

            bRun |= IsRun();

            return !bRun;
        }

        public override void Run()
        {
            if (!this.Initialized) return;

            //if (!IsRun())
            //{
            DoRun.SetState(true);
            //}
        }


        public override void Stop()
        {
            if (!this.Initialized) return;

            //if (true == IsRun())
            //{
            DoRun.SetState(false);
            //}
        }

        //public override Boolean IsProcess()
        //{
        //    return IsRun();
        //}
        #endregion

        public void EStop()
        {
            m_DoEmo.SetState(true);
        }

        public void EStopRe()
        {
            m_DoEmo.SetState(false);
        }

        public void PowerOn()
        {
            m_DoReady.SetState(true);
        }

        public void PowerOff()
        {
            m_DoReady.SetState(false);
        }
    }
}


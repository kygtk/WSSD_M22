///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.10.22
// Author       : eun
// Description  : Mfc for PSM's AP Plasma
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class RfUnit : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorRfUnit tagDescriptor = new TagDescriptorRfUnit();
        #endregion

        #region Fields
        private Seren_Rfg m_Rfg;
        private RfTuner m_RfTuner;
        #endregion

        #region Properties
        [Category("DMS :Di Setting")]
        public Seren_Rfg Rfg
        {
            get { return m_Rfg; }
            set { m_Rfg = value; }
        }
        [Category("DMS :Di Setting")]
        public RfTuner RfTuner
        {
            get { return m_RfTuner; }
            set { m_RfTuner = value; }
        }
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfSetPoint
        //{
        //    get { return m_DiRfSetPoint; }
        //    set { m_DiRfSetPoint = value; }
        //}
        //[Category("DMS :Do Setting")]
        //public IoDigitalOutput RfInterlockSet
        //{
        //    get { return m_DoRfInterlockSet; }
        //    set { m_DoRfInterlockSet = value; }
        //}
        //[Category("DMS :Do Setting")]
        //public IoDigitalOutput RfOn
        //{
        //    get { return m_DoRfOn; }
        //    set { m_DoRfOn = value; }
        //}
        //[Category("DMS :Ao Setting")]
        //public IoAnalogOutput AoRfPower
        //{
        //    get { return m_AoRfPower; }
        //    set { m_AoRfPower = value; }
        //}
        //[Category("DMS :PowerInput Setting")]
        //public Gauge ForwardRfPower
        //{
        //    get { return m_ForwardPower; }
        //    set { m_ForwardPower = value; }
        //}
        //[Category("DMS :PowerInput Setting")]
        //public Gauge ReflectedPower
        //{
        //    get { return m_ReflectedPower; }
        //    set { m_ReflectedPower = value; }
        //}
        //[Category("DMS :Forward AoInfo Setting")]
        //public double AoForwardAdcMax
        //{
        //    get { return m_ForwardAdcMax; }
        //    set { m_ForwardAdcMax = value; }
        //}
        //[Category("DMS :Forward AoInfo Setting")]
        //public double AoForwardAdcMin
        //{
        //    get { return m_ForwardAdcMin; }
        //    set { m_ForwardAdcMin = value; }
        //}
        //[Category("DMS :Forward AoInfo Setting")]
        //public double AoForwardRealMax
        //{
        //    get { return m_ForwardRealMax; }
        //    set { m_ForwardRealMax = value; }
        //}
        //[Category("DMS :Forward AoInfo Setting")]
        //public double AoForwardRealMin
        //{
        //    get { return m_ForwardRealMin; }
        //    set { m_ForwardRealMin = value; }
        //}
        //[Category("DMS :Reflected AoInfo Setting")]
        //public double AoReflectedAdcMax
        //{
        //    get { return m_ReflectedAdcMax; }
        //    set { m_ReflectedAdcMax = value; }
        //}
        //[Category("DMS :Reflected AoInfo Setting")]
        //public double AoReflectedAdcMin
        //{
        //    get { return m_ReflectedAdcMin; }
        //    set { m_ReflectedAdcMin = value; }
        //}
        //[Category("DMS :Reflected AoInfo Setting")]
        //public double AoReflectedRealMax
        //{
        //    get { return m_ReflectedRealMax; }
        //    set { m_ReflectedRealMax = value; }
        //}
        //[Category("DMS :Reflected AoInfo Setting")]
        //public double AoReflectedRealMin
        //{
        //    get { return m_ReflectedRealMin; }
        //    set { m_ReflectedRealMin = value; }
        //}
        //[Browsable(false), XmlIgnore()]
        //public double RfPower
        //{
        //    get { return m_SetRfPower; }
        //    set { m_SetRfPower = value; }
        //}
        //[Browsable(false), XmlIgnore()]
        //public TagSetupInfo SetupForwardInterlockRange
        //{
        //    get { return m_SetupForwardInterlockRange; }
        //    set { m_SetupForwardInterlockRange = value; }
        //}
        //[Browsable(false), XmlIgnore()]
        //public TagSetupInfo SetupReflectedInterlockRange
        //{
        //    get { return m_SetupReflectedInterlockRange; }
        //    set { m_SetupReflectedInterlockRange = value; }
        //}
        #endregion

        #region Constructor
        public RfUnit()
        {
            this.Name = "__ Rf Unit";
        }
        #endregion

        #region Methods
        public void SetRfPower(double power)
        {
            m_Rfg.SetRfPower(power);
        }

        public double GetForwardRfPower()
        {
            return m_Rfg.GetForwardRfPower();
        }

        public double GetReflectedRfPower()
        {
            return m_Rfg.GetReflectedRfPower();
        }

        public void RfPowerOn(bool state)
        {
            m_Rfg.RfPowerOn(state);           
        }

        public bool IsRfPowerOn()
        {
            return m_Rfg.IsRfPowerOn();
        }

        public void SetInterlock(bool state)
        {
            m_Rfg.SetInterlock(state);
        }

        public bool IsForwardInterlock(double range)
        {
            return m_Rfg.IsForwardInterlock(range);
        }

        public bool IsReflectedInterlock(double range)
        {
            return m_Rfg.IsReflectedInterlock(range);
        }

        public void SetPresetTunePosition(double position)
        {
            m_RfTuner.SetRfTunerTunePreset(position);
        }

        public void SetPresetLoadPosition(double position)
        {
            m_RfTuner.SetRfTunerLoadPreset(position);
        }

        public double GetPresetLoadPosition()
        {
            return m_RfTuner.GetRfTunerLoadPosition();
        }

        public double GetPresetTunePosition()
        {
            return m_RfTuner.GetRfTunerTunePosition();
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
            m_Tag.SetValue(tagDescriptor.ONSTATE, IsRfPowerOn());
            m_Tag.SetValue(tagDescriptor.SETRFPOWER, m_Rfg.RfPower);
            m_Tag.SetValue(tagDescriptor.FORWARDPOWER, GetForwardRfPower());
            m_Tag.SetValue(tagDescriptor.REFLECTEDPOWER, GetReflectedRfPower());
            m_Tag.SetValue(tagDescriptor.LOADPOS, GetPresetLoadPosition());
            m_Tag.SetValue(tagDescriptor.TUNEPOS, GetPresetTunePosition());
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
            //ok &= (m_AoGaugeFlowSet != null);
            //ok &= (m_Gauge != null);
            //ok &= (m_DiRfEnable != null);
            //ok &= (m_DiRfOverTemp != null);

            //ok &= (m_DoRfInterlockSet != null);
            //ok &= (m_DoRfOn != null);

            //ok &= (m_AoRfPower != null);

            //ok &= (m_ForwardPower != null);
            //ok &= (m_ReflectedPower != null);

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
                //ALM_ForwardInterlockAlarm = new Alarm(this.Name + "Forward Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_ReflectedInterlockAlarm = new Alarm(this.Name + "Reflected Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                //if (null == m_SetupForwardInterlockRange)
                //{
                //    m_SetupForwardInterlockRange = new TagSetupInfo("Rfg Forward Interlock Range", OptionType.None, OptionFormat.Digit, UnitType.Percent, "5");
                //    SetupGenInfoProvider.Instance.InitFromDB(m_SetupForwardInterlockRange);
                //}
                //if (null == m_SetupReflectedInterlockRange)
                //{
                //    m_SetupReflectedInterlockRange = new TagSetupInfo("Rfg Reflected Interlock Range", OptionType.None, OptionFormat.Digit, UnitType.Percent, "5");
                //    SetupGenInfoProvider.Instance.InitFromDB(m_SetupReflectedInterlockRange);
                //}
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

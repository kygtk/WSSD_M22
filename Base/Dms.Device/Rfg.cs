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
    public class Rfg : _Rfg
    {
        #region Tag Descriptor
        //protected static TagDescriptorRfg tagDescriptor = new TagDescriptorRfg();
        #endregion

        #region Fields
        //private IoDigitalInput m_DiRfEnable = new IoDigitalInput();
        //private IoDigitalInput m_DiRfSetPoint = new IoDigitalInput();
        //private IoDigitalInput m_DiRfOverTemp = new IoDigitalInput();

        private IoDigitalOutput m_DoRfInterlockSet = new IoDigitalOutput();
        private IoDigitalOutput m_DoRfOn = new IoDigitalOutput();

        private IoAnalogOutput m_AoRfPower = new IoAnalogOutput();

        private Gauge m_ForwardPower;
        private Gauge m_ReflectedPower;

        private double m_ForwardAdcMax = 0x7FF0;
        private double m_ForwardAdcMin = 0;
        private double m_ForwardRealMax = 10000;
        private double m_ForwardRealMin = 0;

        //private double m_ReflectedAdcMax = 0x7FF0;
        //private double m_ReflectedAdcMin = 0;
        //private double m_ReflectedRealMax = 10000;
        //private double m_ReflectedRealMin = 0;


        #endregion

        #region Properties
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfEnable
        //{
        //    get { return m_DiRfEnable; }
        //    set { m_DiRfEnable = value; }
        //}
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfgOverTemp
        //{
        //    get { return m_DiRfOverTemp; }
        //    set { m_DiRfOverTemp = value; }
        //}
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfSetPoint
        //{
        //    get { return m_DiRfSetPoint; }
        //    set { m_DiRfSetPoint = value; }
        //}
        [Category("DMS :Do Setting")]
        public IoDigitalOutput RfInterlockSet
        {
            get { return m_DoRfInterlockSet; }
            set { m_DoRfInterlockSet = value; }
        }
        [Category("DMS :Do Setting")]
        public IoDigitalOutput RfOn
        {
            get { return m_DoRfOn; }
            set { m_DoRfOn = value; }
        }
        [Category("DMS :Ao Setting")]
        public IoAnalogOutput AoRfPower
        {
            get { return m_AoRfPower; }
            set { m_AoRfPower = value; }
        }
        [Category("DMS :PowerInput Setting")]
        public Gauge ForwardRfPower
        {
            get { return m_ForwardPower; }
            set { m_ForwardPower = value; }
        }
        [Category("DMS :PowerInput Setting")]
        public Gauge ReflectedPower
        {
            get { return m_ReflectedPower; }
            set { m_ReflectedPower = value; }
        }
        [Category("DMS :Forward AoInfo Setting")]
        public double AoForwardAdcMax
        {
            get { return m_ForwardAdcMax; }
            set { m_ForwardAdcMax = value; }
        }
        [Category("DMS :Forward AoInfo Setting")]
        public double AoForwardAdcMin
        {
            get { return m_ForwardAdcMin; }
            set { m_ForwardAdcMin = value; }
        }
        [Category("DMS :Forward AoInfo Setting")]
        public double AoForwardRealMax
        {
            get { return m_ForwardRealMax; }
            set { m_ForwardRealMax = value; }
        }
        [Category("DMS :Forward AoInfo Setting")]
        public double AoForwardRealMin
        {
            get { return m_ForwardRealMin; }
            set { m_ForwardRealMin = value; }
        }
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
        public Rfg()
        {
            this.Name = "__ Rfg";
        }
        #endregion

        #region Methods
        public override void SetRfPower(double power)
        {
            short diffAdc = (short)(m_ForwardAdcMax - m_ForwardAdcMin);
            short diffReal = (short)(m_ForwardRealMax - m_ForwardRealMin);

            ushort value = (ushort)(((power - m_ForwardRealMin) * ((double)diffAdc / diffReal)) + (double)m_ForwardAdcMin);

            m_AoRfPower.SetState(value);

            m_SetRfPower = power;
        }

        public override double GetForwardRfPower()
        {
            return m_ForwardPower.CurValue;
        }

        public override double GetReflectedRfPower()
        {
            return m_ReflectedPower.CurValue;
        }

        public override void RfPowerOn(bool state)
        {
            //m_DoRfInterlockSet.SetState(state);
            m_DoRfOn.SetState(state);
        }

        public override bool IsRfPowerOn()
        {
            return m_DoRfOn.GetState();
            //return false;
        }

        public override void SetInterlock(bool state)
        {
            m_DoRfInterlockSet.SetState(state);
        }

        public override bool IsForwardInterlock(double range)
        {
            bool result = false;

            // SetupItem 추가 할것~
            double minValue = m_SetRfPower * 1000 - (m_SetRfPower * 1000 * SetupForwardInterlockRange.GetValue<int>() / 100.0);
            double maxValue = m_SetRfPower * 1000 + (m_SetRfPower * 1000 * SetupForwardInterlockRange.GetValue<int>() / 100.0);

            if (GetForwardRfPower() > minValue && GetForwardRfPower() < maxValue) result = false;
            else result = true;

            return result;
        }

        public override bool IsReflectedInterlock(double range)
        {
            bool result = false;

            // SetupItem 추가 할것~
            double minValue = 0;
            double maxValue = m_SetRfPower * 1000 * SetupReflectedInterlockRange.GetValue<int>() / 100.0;

            if (GetReflectedRfPower() >= minValue && GetReflectedRfPower() < maxValue) result = false;
            else result = true;

            return result;
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
            m_Tag.SetValue(tagDescriptor.ONSTATE, m_DoRfOn.GetState());
            m_Tag.SetValue(tagDescriptor.SETRFPOWER, m_SetRfPower);
            //m_Tag.SetValue(tagDescriptor.ONSTATE, RfEnable.GetState());
            //m_Tag.SetValue(tagDescriptor.OVERTEMP, RfgOverTemp .GetState());

            m_Tag.SetValue(tagDescriptor.FORWARDPOWER, m_ForwardPower.CurValue);
            m_Tag.SetValue(tagDescriptor.REFLECTEDPOWER, m_ReflectedPower.CurValue);
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

            ok &= (m_DoRfInterlockSet != null);
            ok &= (m_DoRfOn != null);

            ok &= (m_AoRfPower != null);

            ok &= (m_ForwardPower != null);
            ok &= (m_ReflectedPower != null);

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
                ALM_ForwardInterlockAlarm = new Alarm(this.Name + "Forward Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_ReflectedInterlockAlarm = new Alarm(this.Name + "Reflected Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                if (null == SetupForwardInterlockRange)
                {
                    SetupForwardInterlockRange = new TagSetupInfo("Rfg Forward Interlock Range", OptionType.None, OptionFormat.Digit, UnitType.Percent, "5");
                    SetupGenInfoProvider.Instance.InitFromDB(SetupForwardInterlockRange);
                }
                if (null == SetupReflectedInterlockRange)
                {
                    SetupReflectedInterlockRange = new TagSetupInfo("Rfg Reflected Interlock Range", OptionType.None, OptionFormat.Digit, UnitType.Percent, "5");
                    SetupGenInfoProvider.Instance.InitFromDB(SetupReflectedInterlockRange);
                }
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

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
using Dms.Ctl;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Seren_Rfg : _Rfg
    {
        #region Tag Descriptor

        #endregion

        #region Fields



        // For Analog Type
        private IoDigitalOutput m_DoRfInterlockSet = new IoDigitalOutput();
        private IoDigitalOutput m_DoRfOn = new IoDigitalOutput();

        private IoAnalogOutput m_AoRfPower = new IoAnalogOutput();

        private Gauge m_ForwardPower;
        private Gauge m_ReflectedPower;



        private double m_ForwardAdcMax = 0x7FF0;
        private double m_ForwardAdcMin = 0;
        private double m_ForwardRealMax = 10000;
        private double m_ForwardRealMin = 0;

        // For Serial Type



        //private static TagSetupInfo m_SetupForwardInterlockRange = null;
        //private static TagSetupInfo m_SetupReflectedInterlockRange = null;

        //public Alarm ALM_ForwardInterlockAlarm;
        //public Alarm ALM_ReflectedInterlockAlarm;
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
        //[Category("DMS :Comm Setting")]
        //public CommType CommType
        //{
        //    get { return m_CommType; }
        //    set { m_CommType = value; }
        //}
        //[Category("DMS :Comm Setting")]
        //public PortNo PortNo
        //{
        //    get { return m_PortNo; }
        //    set { m_PortNo = value; }
        //}
        //[Browsable(false), XmlIgnore()]
        //public XComm Comm
        //{
        //    get { return m_Comm; }
        //    set { m_Comm = value; }
        //}



        [Category("DMS :IO Setting")]
        public IoDigitalOutput RfInterlockSet
        {
            get { return m_DoRfInterlockSet; }
            set { m_DoRfInterlockSet = value; }
        }
        [Category("DMS :IO Setting")]
        public IoDigitalOutput RfOn
        {
            get { return m_DoRfOn; }
            set { m_DoRfOn = value; }
        }
        [Category("DMS :IO Setting")]
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
        public Seren_Rfg()
        {
            this.Name = "__ Seren Rfg";
        }
        #endregion

        #region Methods
        public override void SetRfPower(double power)
        {
            if (m_CommType == CommType.Analog)
            {

                short diffAdc = (short)(m_ForwardAdcMax - m_ForwardAdcMin);
                short diffReal = (short)(m_ForwardRealMax - m_ForwardRealMin);

                ushort value = (ushort)(((power - m_ForwardRealMin) * ((double)diffAdc / diffReal)) + (double)m_ForwardAdcMin);

                m_AoRfPower.SetState(value);
            }
            else if (m_CommType == CommType.RS232 || m_CommType == CommType.AnalogRS232)
            {
                string msg = (power * 1000).ToString() + " W" + Environment.NewLine;
                //msg = "COMMAND2" + Environment.NewLine;

                //Comm.Write(msg);

                //msg = "NOECHO" + Environment.NewLine;

                //Comm.Write(msg);

                //msg = "1234 W" + Environment.NewLine;

                Comm.Write(msg);
            }

            m_SetRfPower = power;
        }


        public void ReqRfStatus()
        {
            Comm.Write("Q" + Environment.NewLine);
        }

        public override double GetForwardRfPower()
        {
            if (m_CommType == CommType.Analog)
            {
                m_ForwardPowerValue = m_ForwardPower.CurValue;
            }

            return m_ForwardPowerValue;
        }

        public override double GetReflectedRfPower()
        {
            if (m_CommType == CommType.Analog)
            {
                m_ReflectedPowerValue = m_ReflectedPower.CurValue;
            }

            return m_ReflectedPowerValue;
        }

        public override void RfPowerOn(bool state)
        {
            if (m_CommType == CommType.Analog || m_CommType == CommType.AnalogRS232)
            {
                m_DoRfOn.SetState(state);

                m_RfgOnState = state;
            }
            else if (m_CommType == CommType.RS232)
            {
                string msg;

                if (state == true)
                {
                    msg = "G\r";
                }
                else
                {
                    msg = "S\r";
                }

                Comm.Write(msg);

                m_RfgOnState = state;
            }

        }

        public override bool IsRfPowerOn()
        {
            return m_RfgOnState;
        }

        public override void SetInterlock(bool state)
        {
            m_DoRfInterlockSet.SetState(state);
        }

        public override bool IsForwardInterlock(double range)
        {
            bool result = false;

            double minValue = m_SetRfPower * 1000 - (m_SetRfPower * 1000 * SetupForwardInterlockRange.GetValue<int>() / 100.0);
            double maxValue = m_SetRfPower * 1000 + (m_SetRfPower * 1000 * SetupForwardInterlockRange.GetValue<int>() / 100.0);

            if (GetForwardRfPower() > minValue && GetForwardRfPower() < maxValue) result = false;
            else result = true;

            return result;
        }

        public override bool IsReflectedInterlock(double range)
        {
            bool result = false;

            double minValue = 0;
            double maxValue = m_SetRfPower * 1000 * SetupReflectedInterlockRange.GetValue<int>() / 100.0;

            if (GetReflectedRfPower() >= minValue && GetReflectedRfPower() < maxValue) result = false;
            else result = true;

            return result;
        }

        public void ReceivedData(object sender)
        {
            // ReqRfStatus()에 대한 응답
            // XXXXXXX_aaaa_bbbbb_cccc_ddddd<cr>
            string recvData = Comm.ReadTo("\r");
            string[] data = recvData.Split(' ');

            if (data.Length == 5)
            {
                m_SetRfPower = Convert.ToDouble(data[1]) / 1000;
                m_ForwardPowerValue = Convert.ToDouble(data[2]);
                m_ReflectedPowerValue = Convert.ToDouble(data[3]);
                double maximumPower = Convert.ToDouble(data[4]);
            }
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
            m_Tag.SetValue(tagDescriptor.ONSTATE, m_RfgOnState);
            m_Tag.SetValue(tagDescriptor.SETRFPOWER, m_SetRfPower);

            m_Tag.SetValue(tagDescriptor.FORWARDPOWER, m_ForwardPowerValue);
            m_Tag.SetValue(tagDescriptor.REFLECTEDPOWER, m_ReflectedPowerValue);
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

            if (m_CommType == CommType.Analog)
            {
                ok &= (m_DoRfInterlockSet != null);
                ok &= (m_DoRfOn != null);

                ok &= (m_AoRfPower != null);

                ok &= (m_ForwardPower != null);
                ok &= (m_ReflectedPower != null);
            }

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
                Comm = new XComm();

                if (m_CommType == CommType.AnalogRS232 || m_CommType == CommType.RS232)
                {

                    Comm.Initialize();
                    Comm.Simulate = m_Simul.Comport;
                    Comm.Open(PortNo.ToString(), 19200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

                    Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
                }
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

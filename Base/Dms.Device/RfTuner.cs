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
    public class RfTuner : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorRfTuner tagDescriptor = new TagDescriptorRfTuner();
        #endregion

        #region Fields      
        private IoAnalogOutput m_AoRfTunePreset = new IoAnalogOutput();
        private IoAnalogOutput m_AoRfLoadPreset = new IoAnalogOutput();

        private static TagSetupInfo m_SetupTunePreset = null;
        private static TagSetupInfo m_SetupLoadPreset = null;

        Gauge m_RfTunePositionGauge;
        Gauge m_RfLoadPositionGauge;

        //private IoDigitalInput m_DiRfPowerSensed = new IoDigitalInput();
        //private IoDigitalInput m_DiRfPowerTuned = new IoDigitalInput();
        //private IoDigitalInput m_DiRfTunerFault = new IoDigitalInput();
        //private IoDigitalInput m_DiRfOnState = new IoDigitalInput();    

        //private IoDigitalOutput m_DoRfTunerInterlockSet = new IoDigitalOutput();
        //private IoDigitalOutput m_DoRfTunerOperationMode = new IoDigitalOutput();

        //private IoAnalogOutput m_AoRfTunPreset = new IoAnalogOutput();
        //private IoAnalogOutput m_AoRfLoadPreset = new IoAnalogOutput();

        //Gauge m_RfTunePosition = new Gauge();
        //Gauge m_RfLoadPosition = new Gauge();
        //Gauge m_RfDcBiasPosition = new Gauge();


        private double m_TuneAdcMax = 0x7FF0;
        private double m_TuneAdcMin = 0;
        private double m_TuneRealMax = 100;
        private double m_TuneRealMin = 0;

        private double m_LoadAdcMax = 0x7FF0;
        private double m_LoadAdcMin = 0;
        private double m_LoadRealMax = 100;
        private double m_LoadRealMin = 0;

        protected CommType m_CommType;
        protected PortNo m_PortNo = PortNo.COM2;
        protected BaudRate m_BaudRate = BaudRate.Low;
        protected XComm m_Comm;

        protected string m_Command = "NONE";

        protected double m_RfTunePosition = 0;
        protected double m_RfLoadPosition = 0;
        //private double m_DcBiasAdcMax = 0x7FF0;
        //private double m_DcBiasAdcMin = 0;
        //private double m_DcBiasRealMax = 2000;
        //private double m_DcBiasRealMin = 0;
        #endregion

        #region Properties
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfOnState
        //{
        //    get { return m_DiRfOnState; }
        //    set { m_DiRfOnState = value; }
        //}
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfPowerSensed
        //{
        //    get { return m_DiRfPowerSensed; }
        //    set { m_DiRfPowerSensed = value; }
        //}
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfPowerTuned
        //{
        //    get { return m_DiRfPowerTuned; }
        //    set { m_DiRfPowerTuned = value; }
        //}
        //[Category("DMS :Di Setting")]
        //public IoDigitalInput RfTunerFault
        //{
        //    get { return m_DiRfTunerFault; }
        //    set { m_DiRfTunerFault = value; }
        //}
        //[Category("DMS :Do Setting")]
        //public IoDigitalOutput RfTunerInterlockSet
        //{
        //    get { return m_DoRfTunerInterlockSet; }
        //    set { m_DoRfTunerInterlockSet = value; }
        //}
        //[Category("DMS :Do Setting")]
        //public IoDigitalOutput RfTunerOperationMode
        //{
        //    get { return m_DoRfTunerOperationMode; }
        //    set { m_DoRfTunerOperationMode = value; }
        //}
        [Category("DMS :Capacitor Postion Setting")]
        public Gauge RfTunePosition
        {
            get { return m_RfTunePositionGauge; }
            set { m_RfTunePositionGauge = value; }
        }
        [Category("DMS :Capacitor Postion Setting")]
        public Gauge RfLoadPosition
        {
            get { return m_RfLoadPositionGauge; }
            set { m_RfLoadPositionGauge = value; }
        }
        //[Category("DMS :Capacitor Postion Setting")]
        //public Gauge RfDcBiasPosition
        //{
        //    get { return m_RfDcBiasPosition; }
        //    set { m_RfDcBiasPosition = value; }
        //}
        [Category("DMS :Ao Setting")]
        public IoAnalogOutput AoRfTunePreset
        {
            get { return m_AoRfTunePreset; }
            set { m_AoRfTunePreset = value; }
        }
        [Category("DMS :Ao Setting")]
        public IoAnalogOutput AoRfLoadPreset
        {
            get { return m_AoRfLoadPreset; }
            set { m_AoRfLoadPreset = value; }
        }
        [Category("DMS :Tune AoInfo Setting")]
        public double AoTuneAdcMax
        {
            get { return m_TuneAdcMax; }
            set { m_TuneAdcMax = value; }
        }
        [Category("DMS :Tune AoInfo Setting")]
        public double AoTuneAdcMin
        {
            get { return m_TuneAdcMin; }
            set { m_TuneAdcMin = value; }
        }
        [Category("DMS :Tune AoInfo Setting")]
        public double AoTuneRealMax
        {
            get { return m_TuneRealMax; }
            set { m_TuneRealMax = value; }
        }
        [Category("DMS :Tune AoInfo Setting")]
        public double AoTuneRealMin
        {
            get { return m_TuneRealMin; }
            set { m_TuneRealMin = value; }
        }
        [Category("DMS :Load AoInfo Setting")]
        public double AoLoadAdcMax
        {
            get { return m_LoadAdcMax; }
            set { m_LoadAdcMax = value; }
        }
        [Category("DMS :Load AoInfo Setting")]
        public double AoLoadAdcMin
        {
            get { return m_LoadAdcMin; }
            set { m_LoadAdcMin = value; }
        }
        [Category("DMS :Load AoInfo Setting")]
        public double AoLoadRealMax
        {
            get { return m_LoadRealMax; }
            set { m_LoadRealMax = value; }
        }
        [Category("DMS :Load AoInfo Setting")]
        public double AoLoadRealMin
        {
            get { return m_LoadRealMin; }
            set { m_LoadRealMin = value; }
        }
        [Category("DMS :Comm Setting")]
        public CommType CommType
        {
            get { return m_CommType; }
            set { m_CommType = value; }
        }
        [Category("DMS :Comm Setting")]
        public PortNo PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }


        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupTunePreset
        {
            get { return m_SetupTunePreset; }
            set { m_SetupTunePreset = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupLoadPreset
        {
            get { return m_SetupLoadPreset; }
            set { m_SetupLoadPreset = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        //[Category("DMS :DcBias AoInfo Setting")]
        //public double AoDcBiasAdcMax
        //{
        //    get { return m_DcBiasAdcMax; }
        //    set { m_DcBiasAdcMax = value; }
        //}
        //[Category("DMS :DcBias AoInfo Setting")]
        //public double AoDcBiasAdcMin
        //{
        //    get { return m_DcBiasAdcMin; }
        //    set { m_DcBiasAdcMin = value; }
        //}
        //[Category("DMS :DcBias AoInfo Setting")]
        //public double AoDcBiasRealMax
        //{
        //    get { return m_DcBiasRealMax; }
        //    set { m_DcBiasRealMax = value; }
        //}
        //[Category("DMS :DcBias AoInfo Setting")]
        //public double AoDcBiasRealMin
        //{
        //    get { return m_DcBiasRealMin; }
        //    set { m_DcBiasRealMin = value; }
        //}       
        #endregion

        #region Constructor
        public RfTuner()
        {
            this.Name = "__ RfTuner";
        }
        #endregion

        #region Methods
        public double GetRfTunerTunePosition()
        {
            if (m_CommType == CommType.Analog) return m_RfTunePositionGauge.CurValue;
            else if (m_CommType == CommType.RS232) return m_RfTunePosition;
            else return 0;
        }

        public double GetRfTunerLoadPosition()
        {
            if (m_CommType == CommType.Analog) return m_RfLoadPositionGauge.CurValue;
            else if (m_CommType == CommType.RS232) return m_RfLoadPosition;
            else return 0;
        }

        public void ReqTunePosition()
        {
            if (m_CommType == CommType.RS232 && m_Comm != null && m_Comm.IsOpen() == true)
            {
                //m_Command = "@00";
                //m_Comm.Write(m_Command + Convert.ToChar(0x0D));


                m_Command = "TPS?\r";
                m_Comm.Write(m_Command);
            }
        }
        //public double GetRfTunerDcBias()
        //{
        //    return m_RfDcBiasPosition.CurValue;
        //}

        public void ReqLoadPosition()
        {
            if (m_CommType == CommType.RS232 && m_Comm != null && m_Comm.IsOpen() == true)
            {
                //m_Command = "@00";
                //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

                m_Command = "LPS?\r";
                m_Comm.Write(m_Command);
            }
        }

        public void SetRfTunerTunePreset(double position)
        {
            if (m_CommType == CommType.Analog)
            {
                short diffAdc = (short)(m_TuneAdcMax - m_TuneAdcMin);
                short diffReal = (short)(m_TuneRealMax - m_TuneRealMin);

                ushort value = (ushort)(((position - m_TuneRealMin) * ((double)diffAdc / diffReal)) + (double)m_TuneAdcMin);

                m_AoRfTunePreset.SetState(value);
            }
            else if (m_CommType == CommType.RS232)
            {
                //m_Command = "@00";
                //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

                int pos = Convert.ToInt32(position);

                m_Comm.Write(pos.ToString() + " MPT\r");
            }
        }

        public void SetRfTunerLoadPreset(double position)
        {
            if (m_CommType == CommType.Analog)
            {
                short diffAdc = (short)(m_LoadAdcMax - m_LoadAdcMin);
                short diffReal = (short)(m_LoadRealMax - m_LoadRealMin);

                ushort value = (ushort)(((position - m_LoadRealMin) * ((double)diffAdc / diffReal)) + (double)m_LoadAdcMin);

                m_AoRfLoadPreset.SetState(value);
            }
            else if (m_CommType == CommType.RS232)
            {
                //m_Command = "@00";
                //m_Comm.Write(m_Command + "\r");

                int pos = Convert.ToInt32(position);
                m_Comm.Write(pos.ToString() + " MPL\r");
            }
        }

        public void SetLoadPositionManual()
        {
            //m_Command = "@00";
            //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

            m_Comm.Write("MLD" + Convert.ToChar(0x0D));
        }

        public void SetLoadPositionAuto()
        {
            //m_Command = "@00";
            //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

            m_Comm.Write("ALD" + Convert.ToChar(0x0D));
        }

        public void SetTunePositionManual()
        {
            //m_Command = "@00";
            //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

            m_Comm.Write("MTN" + Convert.ToChar(0x0D));
        }

        public void SetTunePositionAuto()
        {
            //m_Command = "@00";
            //m_Comm.Write(m_Command + Convert.ToChar(0x0D));

            m_Comm.Write("ATN" + Convert.ToChar(0x0D));
        }

        public bool SetInterlock(bool state)
        {
            bool result = false;


            //if (m_DiRfOnState.GetState()) result = false;
            //else
            //{
            //    m_DoRfTunerInterlockSet.SetState(state);
            //    result = true;
            //}

            return result;
        }

        public bool SetManualMode()
        {
            bool result = false;


            //if (!m_DoRfTunerInterlockSet.GetState()) result = false;
            //else if (m_DiRfOnState.GetState()) result = false;
            //else
            //{
            //    m_DoRfTunerOperationMode.SetState(false);
            //    result = true;
            //}

            return result;
        }

        public bool SetAutoMode()
        {
            bool result = false;

            return result;
        }

        public void ReceivedData(object sender)
        {
            //// XXXXXXX_aaaa_bbbbb_cccc_ddddd<cr>
            string recvData = m_Comm.ReadExisting();
            //string[] data = recvData.Split(' ');

            //if (data.Length == 5)
            //{
            //    m_SetRfPower = Convert.ToDouble(data[1]) / 1000;
            //    m_ForwardPowerValue = Convert.ToDouble(data[2]);
            //    m_ReflectedPowerValue = Convert.ToDouble(data[3]);
            //    double maximumPower = Convert.ToDouble(data[4]);
            //}

            if (m_Command == "TPS?\r")
            {
                m_Command = "NONE";

                m_RfTunePosition = Convert.ToDouble(recvData);
            }

            if (m_Command == "LPS?\r")
            {
                m_Command = "NONE";

                m_RfLoadPosition = Convert.ToDouble(recvData);
            }
            //if (!m_DoRfTunerInterlockSet.GetState()) result = false;
            //else if (m_DiRfOnState.GetState()) result = false;
            //else
            //{
            //    m_DoRfTunerOperationMode.SetState(true);
            //    result = true;
            //}

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
            //m_Tag.SetValue(tagDescriptor.POWERSENSED, RfPowerSensed.GetState());
            //m_Tag.SetValue(tagDescriptor.POWERTUNED, RfPowerTuned.GetState());
            //m_Tag.SetValue(tagDescriptor.TUNERFAULT, RfTunerFault.GetState());
            //m_Tag.SetValue(tagDescriptor.DCBIAS, m_RfDcBiasPosition.CurValue);

            m_Tag.SetValue(tagDescriptor.TUNEPOS, m_RfTunePosition);
            m_Tag.SetValue(tagDescriptor.LOADPOS, m_RfLoadPosition);
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
                if (null == m_SetupTunePreset)
                {
                    m_SetupTunePreset = new TagSetupInfo("Rfg Tune Preset", OptionType.None, OptionFormat.Digit, UnitType.Percent, "50");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupTunePreset);
                }
                if (null == m_SetupLoadPreset)
                {
                    m_SetupLoadPreset = new TagSetupInfo("Rfg Load Preset", OptionType.None, OptionFormat.Digit, UnitType.Percent, "50");
                    SetupGenInfoProvider.Instance.InitFromDB(m_SetupLoadPreset);
                }
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                //TagCalibrationInfo LoadInfo = new TagCalibrationInfo(m_RfLoadPosition.GaugeType, m_RfLoadPosition.Name);

                //LoadInfo.AdcMax = 16383;
                //LoadInfo.AdcMin = 0;
                //LoadInfo.RealMax = 100;
                //LoadInfo.RealMin = 0;

                //m_RfLoadPosition.SetGaugeInfo(LoadInfo);

                //TagCalibrationInfo TuneInfo = new TagCalibrationInfo(m_RfTunePosition.GaugeType, m_RfTunePosition.Name);
                //TuneInfo.AdcMax = 16383;
                //TuneInfo.AdcMin = 0;
                //TuneInfo.RealMax = 100;
                //TuneInfo.RealMin = 0;

                //m_RfTunePosition.SetGaugeInfo(LoadInfo);


                //TagCalibrationInfo LoadInfo = new TagCalibrationInfo(m_RfLoadPosition.GaugeType, m_RfLoadPosition.Name);

                //LoadInfo.AdcMax = 16383;
                //LoadInfo.AdcMin = 0;
                //LoadInfo.RealMax = 100;
                //LoadInfo.RealMin = 0;

                //m_RfLoadPosition.SetGaugeInfo(LoadInfo);

                //TagCalibrationInfo TuneInfo = new TagCalibrationInfo(m_RfTunePosition.GaugeType, m_RfTunePosition.Name);
                //TuneInfo.AdcMax = 16383;
                //TuneInfo.AdcMin = 0;
                //TuneInfo.RealMax = 100;
                //TuneInfo.RealMin = 0;
                if (m_CommType == CommType.RS232)
                {
                    m_Comm = new XComm();
                    m_Comm.Initialize();
                    m_Comm.Simulate = m_Simul.Comport;
                    m_Comm.Open(m_PortNo.ToString(), 19200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                    //m_RfTunePosition.SetGaugeInfo(LoadInfo);

                    m_Comm.ReceivedData += new XComm.ReceivedDataEventHandler(ReceivedData);
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
                    SetRfTunerTunePreset(Convert.ToDouble(m_SetupTunePreset.Val));
                    SetRfTunerLoadPreset(Convert.ToDouble(m_SetupLoadPreset.Val));
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }
        #endregion
    }
}

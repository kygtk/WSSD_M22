using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using Dms.Util.IODefine;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class HeatExchanger : _HeatExchanger
    {
        #region Fields
        private IoCollection<IoDigitalInput> m_DiHeatExchangerTempBits = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalInput> m_DiHeatExchangerChReadys = new IoCollection<IoDigitalInput>();
        private IoCollection<IoDigitalOutput> m_DoHeatExchangerTempBits = new IoCollection<IoDigitalOutput>();

        private double[] m_GetTemperature = new double[3];
        private double[] m_SetTemperature = new double[3];

        // Di
        private IoDigitalInput m_DiHeatExchangerEmo = new IoDigitalInput();
        private IoDigitalInput m_DiHeatExchangerAlarm1 = new IoDigitalInput();
        private IoDigitalInput m_DiHeatExchangerAlarm2 = new IoDigitalInput();

        //private IoDigitalInput m_DiHeatExchangerTempBit0 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit1 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit2 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit3 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit4 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit5 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit6 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit7 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit8 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit9 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit10 = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerTempBit11 = new IoDigitalInput();

        //private IoDigitalInput m_DiHeatExchangerCh1Ready = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerCh2Ready = new IoDigitalInput();
        //private IoDigitalInput m_DiHeatExchangerCh3Ready = new IoDigitalInput();

        private IoDigitalInput m_DiHeatExchangerRunning = new IoDigitalInput();

        private IoDigitalInput m_DiHeatExchangerPowerOnLed = new IoDigitalInput();
        private IoDigitalInput m_DiHeatExchangerAlarmLed = new IoDigitalInput();
        private IoDigitalInput m_DiHeatExchangerReadyLed = new IoDigitalInput();

        // Do
        //private IoDigitalOutput m_DoHeatExchangerTempBit0 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit1 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit2 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit3 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit4 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit5 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit6 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit7 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit8 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit9 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit10 = new IoDigitalOutput();
        //private IoDigitalOutput m_DoHeatExchangerTempBit11 = new IoDigitalOutput();

        private IoDigitalOutput m_DoHeatExchangerPvChannelBit0 = new IoDigitalOutput();
        private IoDigitalOutput m_DoHeatExchangerPvChannelBit1 = new IoDigitalOutput();

        private IoDigitalOutput m_DoHeatExchangerSvChannelBit0 = new IoDigitalOutput();
        private IoDigitalOutput m_DoHeatExchangerSvChannelBit1 = new IoDigitalOutput();

        private IoDigitalOutput m_DoHeatExchangerOpMode = new IoDigitalOutput();

        private IoDigitalOutput m_DoHeatExchangerPowerOn = new IoDigitalOutput();
        private IoDigitalOutput m_DoHeatExchangerRunning = new IoDigitalOutput();

        private IoDigitalOutput m_DoHeatExchangerTempSensorUse = new IoDigitalOutput();

        //private List<IoDigitalOutput> m_DoHeatExchangerList = new List<IoDigitalOutput>();
        #endregion

        #region Properties
        [Category("DMS : Di Setting")]
        public IoCollection<IoDigitalInput> DiHeatExchangerTempBits
        {
            get { return m_DiHeatExchangerTempBits; }
            set { m_DiHeatExchangerTempBits = value; }
        }

        [Category("DMS : Di Setting")]
        public IoCollection<IoDigitalInput> DiHeatExchangerChReadys
        {
            get { return m_DiHeatExchangerChReadys; }
            set { m_DiHeatExchangerChReadys = value; }
        }

        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit0
        //{
        //    get { return m_DiHeatExchangerTempBit0; }
        //    set { m_DiHeatExchangerTempBit0 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit1
        //{
        //    get { return m_DiHeatExchangerTempBit1; }
        //    set { m_DiHeatExchangerTempBit1 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit2
        //{
        //    get { return m_DiHeatExchangerTempBit2; }
        //    set { m_DiHeatExchangerTempBit2 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit3
        //{
        //    get { return m_DiHeatExchangerTempBit3; }
        //    set { m_DiHeatExchangerTempBit3 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit4
        //{
        //    get { return m_DiHeatExchangerTempBit4; }
        //    set { m_DiHeatExchangerTempBit4 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit5
        //{
        //    get { return m_DiHeatExchangerTempBit5; }
        //    set { m_DiHeatExchangerTempBit5 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit6
        //{
        //    get { return m_DiHeatExchangerTempBit6; }
        //    set { m_DiHeatExchangerTempBit6 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit7
        //{
        //    get { return m_DiHeatExchangerTempBit7; }
        //    set { m_DiHeatExchangerTempBit7 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit8
        //{
        //    get { return m_DiHeatExchangerTempBit8; }
        //    set { m_DiHeatExchangerTempBit8 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit9
        //{
        //    get { return m_DiHeatExchangerTempBit9; }
        //    set { m_DiHeatExchangerTempBit9 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit10
        //{
        //    get { return m_DiHeatExchangerTempBit10; }
        //    set { m_DiHeatExchangerTempBit10 = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerTempBit11
        //{
        //    get { return m_DiHeatExchangerTempBit11; }
        //    set { m_DiHeatExchangerTempBit11 = value; }
        //}

        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerAlarm1
        {
            get { return m_DiHeatExchangerAlarm1; }
            set { m_DiHeatExchangerAlarm1 = value; }
        }
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerRunning
        {
            get { return m_DiHeatExchangerRunning; }
            set { m_DiHeatExchangerRunning = value; }
        }
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerCh1Ready
        //{
        //    get { return m_DiHeatExchangerCh1Ready; }
        //    set { m_DiHeatExchangerCh1Ready = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerCh2Ready
        //{
        //    get { return m_DiHeatExchangerCh2Ready; }
        //    set { m_DiHeatExchangerCh2Ready = value; }
        //}
        //[Category("DMS : Di Setting")]
        //public IoDigitalInput DiHeatExchangerCh3Ready
        //{
        //    get { return m_DiHeatExchangerCh3Ready; }
        //    set { m_DiHeatExchangerCh3Ready = value; }
        //}
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerAlarm2
        {
            get { return m_DiHeatExchangerAlarm2; }
            set { m_DiHeatExchangerAlarm2 = value; }
        }
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerPowerOnLed
        {
            get { return m_DiHeatExchangerPowerOnLed; }
            set { m_DiHeatExchangerPowerOnLed = value; }
        }
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerAlarmLed
        {
            get { return m_DiHeatExchangerAlarmLed; }
            set { m_DiHeatExchangerAlarmLed = value; }
        }
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerReadyLed
        {
            get { return m_DiHeatExchangerReadyLed; }
            set { m_DiHeatExchangerReadyLed = value; }
        }
        [Category("DMS : Di Setting")]
        public IoDigitalInput DiHeatExchangerEmo
        {
            get { return m_DiHeatExchangerEmo; }
            set { m_DiHeatExchangerEmo = value; }
        }

        [Category("DMS : Do Setting")]
        public IoCollection<IoDigitalOutput> DoHeatExchangerTempBits
        {
            get { return m_DoHeatExchangerTempBits; }
            set { m_DoHeatExchangerTempBits = value; }
        }

        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit0
        //{
        //    get { return m_DoHeatExchangerTempBit0; }
        //    set { m_DoHeatExchangerTempBit0 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit1
        //{
        //    get { return m_DoHeatExchangerTempBit1; }
        //    set { m_DoHeatExchangerTempBit1 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit2
        //{
        //    get { return m_DoHeatExchangerTempBit2; }
        //    set { m_DoHeatExchangerTempBit2 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit3
        //{
        //    get { return m_DoHeatExchangerTempBit3; }
        //    set { m_DoHeatExchangerTempBit3 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit4
        //{
        //    get { return m_DoHeatExchangerTempBit4; }
        //    set { m_DoHeatExchangerTempBit4 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit5
        //{
        //    get { return m_DoHeatExchangerTempBit5; }
        //    set { m_DoHeatExchangerTempBit5 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit6
        //{
        //    get { return m_DoHeatExchangerTempBit6; }
        //    set { m_DoHeatExchangerTempBit6 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit7
        //{
        //    get { return m_DoHeatExchangerTempBit7; }
        //    set { m_DoHeatExchangerTempBit7 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit8
        //{
        //    get { return m_DoHeatExchangerTempBit8; }
        //    set { m_DoHeatExchangerTempBit8 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit9
        //{
        //    get { return m_DoHeatExchangerTempBit9; }
        //    set { m_DoHeatExchangerTempBit9 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit10
        //{
        //    get { return m_DoHeatExchangerTempBit10; }
        //    set { m_DoHeatExchangerTempBit10 = value; }
        //}
        //[Category("DMS : Do Setting")]
        //public IoDigitalOutput DoHeatExchangerTempBit11
        //{
        //    get { return m_DoHeatExchangerTempBit11; }
        //    set { m_DoHeatExchangerTempBit11 = value; }
        //}

        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerSvChannelBit0
        {
            get { return m_DoHeatExchangerSvChannelBit0; }
            set { m_DoHeatExchangerSvChannelBit0 = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerSvChannelBit1
        {
            get { return m_DoHeatExchangerSvChannelBit1; }
            set { m_DoHeatExchangerSvChannelBit1 = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerPvChannelBit0
        {
            get { return m_DoHeatExchangerPvChannelBit0; }
            set { m_DoHeatExchangerPvChannelBit0 = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerPvChannelBit1
        {
            get { return m_DoHeatExchangerPvChannelBit1; }
            set { m_DoHeatExchangerPvChannelBit1 = value; }
        }

        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerOpMode
        {
            get { return m_DoHeatExchangerOpMode; }
            set { m_DoHeatExchangerOpMode = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerTempSensorUse
        {
            get { return m_DoHeatExchangerTempSensorUse; }
            set { m_DoHeatExchangerTempSensorUse = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerPowerOn
        {
            get { return m_DoHeatExchangerPowerOn; }
            set { m_DoHeatExchangerPowerOn = value; }
        }
        [Category("DMS : Do Setting")]
        public IoDigitalOutput DoHeatExchangerRunning
        {
            get { return m_DoHeatExchangerRunning; }
            set { m_DoHeatExchangerRunning = value; }
        }
        #endregion

        #region Constructor
        public HeatExchanger()
        {
            this.Name = "__ HeatExchanger";

            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit0); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit1);
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit2); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit3); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit4); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit5); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit6); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit7); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit8); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit9); 
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit10);
            //m_DiHeatExchangerList.Add(m_DiHeatExchangerTempBit11);

            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit0);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit1);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit2);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit3);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit4);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit5);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit6);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit7);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit8);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit9);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit10);
            //m_DoHeatExchangerList.Add(m_DoHeatExchangerTempBit11);
        }
        #endregion

        #region Method
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

        public override void On()
        {
            m_DoHeatExchangerPowerOn.SetState(true);
            m_DoHeatExchangerRunning.SetState(true);
        }

        public override void Off()
        {
            m_DoHeatExchangerPowerOn.SetState(false);
            m_DoHeatExchangerRunning.SetState(false);
        }

        public override bool IsOn()
        {
            bool rv = false;

            rv = DiHeatExchangerRunning.GetState();

            return rv;
        }

        public override bool IsOff()
        {
            bool rv = false;

            rv = !DiHeatExchangerRunning.GetState();

            return rv;
        }

        public override bool IsAlarm()
        {
            bool rv = false;

            rv = DiHeatExchangerAlarm1.GetState();

            return rv;
        }

        public bool IsWarning()
        {
            bool rv = false;

            rv = DiHeatExchangerAlarm2.GetState();

            return rv;
        }

        public bool IsChamber1Ready()
        {
            bool rv = false;

            rv = m_DiHeatExchangerChReadys[0].GetState();

            return rv;
        }

        public bool IsChamber2Ready()
        {
            bool rv = false;

            rv = m_DiHeatExchangerChReadys[1].GetState();

            return rv;
        }

        public bool IsChamber3Ready()
        {
            bool rv = false;

            rv = m_DiHeatExchangerChReadys[2].GetState();

            return rv;
        }

        public bool IsEmo()
        {
            bool rv = false;

            rv = DiHeatExchangerEmo.GetState();

            return rv;
        }

        public bool IsPowerOnLed()
        {
            bool rv = false;

            rv = DiHeatExchangerPowerOnLed.GetState();

            return rv;
        }

        public bool IsAlarmLed()
        {
            bool rv = false;

            rv = DiHeatExchangerAlarmLed.GetState();

            return rv;
        }

        public bool IsReadyLed()
        {
            bool rv = false;

            rv = DiHeatExchangerReadyLed.GetState();

            return rv;
        }

        public override double GetTemp(int channelNo)
        {
            bool bit0 = false, bit1 = false;
            bool[] getTempBit = new bool[12];
            int temp = 0;
            double temperature = 0.0;

            switch (channelNo)
            {
                case 0: bit1 = true; bit0 = true; break; /* Channel */
                case 1: bit1 = true; bit0 = false; break; /* Channel */
                case 2: bit1 = false; bit0 = true; break;  /* Channel */
            }

            m_DoHeatExchangerPvChannelBit0.SetState(bit0);
            m_DoHeatExchangerPvChannelBit1.SetState(bit1);

            for (int i = 0; i < 12; i++)
            {
                getTempBit[i] = !m_DiHeatExchangerTempBits[i].GetState();
            }

            for (int i = 0; i < 12; i++)
            {
                temp += Convert.ToInt32(getTempBit[i]) * Convert.ToInt32(Math.Pow(2, i));
            }

            m_GetTemperature[channelNo] = temperature = ((temp * 0.05) - 25.0);

            if (channelNo == 0) m_Tag.SetValue(tagDescriptor.CURTEMP1, m_GetTemperature[channelNo]);
            if (channelNo == 1) m_Tag.SetValue(tagDescriptor.CURTEMP2, m_GetTemperature[channelNo]);

            return temperature;
        }

        public override void SetTemp(int channelNo, int setTemp)
        {
            bool bit0 = false, bit1 = false;
            int setTemperature = 0;
            bool[] setTempBit = new bool[8];

            switch (channelNo)
            {
                case 0: bit1 = true; bit0 = true; break; /* Channel 1*/
                case 1: bit1 = true; bit0 = false; break; /* Channel 2*/
                case 2: bit1 = false; bit0 = true; break;  /* Channel 3*/
            }

            m_DoHeatExchangerSvChannelBit0.SetState(bit0);
            m_DoHeatExchangerSvChannelBit1.SetState(bit1);

            m_SetTemperature[channelNo] = setTemp;

            setTemperature = (setTemp + 25) * 2;

            for (int i = 0; i < 8; i++)
            {
                setTemp = setTemperature;
                setTempBit[i] = !Convert.ToBoolean(setTemp % 2);

                m_DoHeatExchangerTempBits[i].SetState(setTempBit[i]);

                setTemperature = setTemp / 2;
            }

            if (channelNo == 0) m_Tag.SetValue(tagDescriptor.SETTEMP1, setTemp);
            if (channelNo == 1) m_Tag.SetValue(tagDescriptor.SETTEMP2, setTemp);
        }

        public override void SetOperationMode(OpMode opMode)
        {
            bool mode = false;

            if (opMode == OpMode.Remote) mode = true;

            m_DoHeatExchangerOpMode.SetState(mode);
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
            m_Tag.SetValue(tagDescriptor.ALARMLED, IsAlarmLed());
            m_Tag.SetValue(tagDescriptor.CH1READY, IsChamber1Ready());
            m_Tag.SetValue(tagDescriptor.CH2READY, IsChamber2Ready());
            m_Tag.SetValue(tagDescriptor.CH3READY, IsChamber3Ready());
            m_Tag.SetValue(tagDescriptor.OFF, IsOff());
            m_Tag.SetValue(tagDescriptor.ON, IsOn());
            m_Tag.SetValue(tagDescriptor.POWERONLED, IsPowerOnLed());
            m_Tag.SetValue(tagDescriptor.WARNING, IsWarning());
            if (m_Simul.Device == false)
            {
                m_Tag.SetValue(tagDescriptor.CURTEMP1, GetTemp(0));
                m_Tag.SetValue(tagDescriptor.CURTEMP2, GetTemp(1));
            }
        }
        #endregion
    }
}

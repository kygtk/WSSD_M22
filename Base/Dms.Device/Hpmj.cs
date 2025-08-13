using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using Dms.Util.IODefine;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Dms.Device
{
    #region Tag
    public class TagHpmjIfFlag
    {
        #region Fields
        private bool bRemoteReq;
        private bool bLocalReq;
        private bool bPumpRun;
        private bool bReady;
        private bool bParaChangeReq;
        private bool bPPIDChangeReq;
        private bool bTimeSetReq;

        private bool bRemote;
        private bool bAlarm;
        private bool bWarning;
        private bool bPowerCut;
        private bool bDiLack;

        private bool bMoniterModeChange;
        private bool bGaugeInterlockAlarm;
        private bool bPidError;
        private int nRefPressure;
        private int nPressure;
        private int nCurAnalogOutput;
        private double dCurPressure;
        private double dErrPressure;
        private double dCurHertz;

        private Hpmj m_Parent;
        #endregion

        #region Properties
        public bool RemoteRequest
        {
            get { return bRemoteReq; }
            set { bRemoteReq = value; }
        }
        public bool LocalRequest
        {
            get { return bLocalReq; }
            set { bLocalReq = value; }
        }
        public bool PumpRun
        {
            get { return bPumpRun; }
            set { bPumpRun = value; }
        }
        public bool Ready
        {
            get { return bReady; }
            set { bReady = value; }
        }
        public bool ParameterChangeRequest
        {
            get { return bParaChangeReq; }
            set { bParaChangeReq = value; }
        }
        public bool PPIDChangeRequest
        {
            get { return bPPIDChangeReq; }
            set { bPPIDChangeReq = value; }
        }
        public bool TimeSetRequest
        {
            get { return bTimeSetReq; }
            set { bTimeSetReq = value; }
        }
        public bool Remote
        {
            get { return bRemote; }
            set { bRemote = value; }
        }
        public bool Alarm
        {
            get { return bAlarm; }
            set { bAlarm = value; }
        }
        public bool Warning
        {
            get { return bWarning; }
            set { bWarning = value; }
        }
        public bool GaugeInterlockAlarm
        {
            get { return bGaugeInterlockAlarm; }
            set { bGaugeInterlockAlarm = value; }
        }
        public bool PowerCut
        {
            get { return bPowerCut; }
            set { bPowerCut = value; }
        }
        public bool DiLack
        {
            get { return bDiLack; }
            set { bDiLack = value; }
        }
        public bool PIDError
        {
            get { return bPidError; }
            set { bPidError = value; }
        }
        public int ReferencePressure
        {
            get { return nRefPressure; }
            set { nRefPressure = value; }
        }
        public int PressureSet
        {
            get { return nPressure; }
            set { nPressure = value; }
        }
        public int CurrentAnalogOutput
        {
            get { return nCurAnalogOutput; }
            set { nCurAnalogOutput = value; }
        }
        public double CurrentPressure
        {
            get { return dCurPressure; }
            set { dCurPressure = value; }
        }
        public double ErrorPressure
        {
            get { return dErrPressure; }
            set { dErrPressure = value; }
        }
        public double CurrentHertz
        {
            get { return dCurHertz; }
            set { dCurHertz = value; }
        }
        public bool ModeChange
        {
            get { return bMoniterModeChange; }
            set { bMoniterModeChange = value; }
        }
        #endregion

        #region Constructor
        public TagHpmjIfFlag()
        {
        }

        public TagHpmjIfFlag(Hpmj unit)
        {
            m_Parent = unit;
        }
        #endregion

        #region Methods
        public void Reset()
        {
            bRemoteReq = false;
            bLocalReq = false;
            bPumpRun = false;
            bReady = false;
            bParaChangeReq = false;
            bPPIDChangeReq = false;
            bTimeSetReq = false;

            bRemote = false;
            bAlarm = false;
            bWarning = false;
            bPowerCut = false;
            bDiLack = false;

            bGaugeInterlockAlarm = false;
            bPidError = false;
            bMoniterModeChange = false;
            nRefPressure = 0;
            nPressure = 0;
            nCurAnalogOutput = 0;
            dCurPressure = 0.0;
            dErrPressure = 0.0;
            dCurHertz = 0.0;
        }
        #endregion
    }
    #endregion

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class Hpmj : _DeviceAsm
    {
        #region Tag Descriptor
        protected TagDescriptorHpmj tagDescriptor = new TagDescriptorHpmj();
        #endregion

        #region Fields
        protected TagHpmjIfFlag m_IfFlag = null;
        private ProcessControlBy m_ControlBy = ProcessControlBy.Setup;
        protected TagSetupInfo m_SetupHpmjUse;
        protected TagSetupInfo m_SetupHpmjMode;
        protected TagSetupInfo m_SetupHpmjPressure;
        protected TagSetupInfo m_SetupHpmjFrequency;
        private TagSetupInfo m_SetupHpmjFilterChangeTime;
        private TagSetupInfo m_SetupHpmjPackingChangeTime;
        protected TagSetupInfo m_SetupHpmjCO2InValveCloseDiLevel; // dspcrassus - HPMJ Unit
        protected TagSetupInfo m_SetupHpmjCO2VentControl;
        protected TagSetupInfo m_SetupHpmjCO2VentValveOpenTime;
        protected TagSetupInfo m_SetupHpmjCO2VentValveOpenPeriod;
        protected TagSetupInfo m_SetupHpmjStandardPressure;
        protected TagSetupInfo m_SetupHpmjStandardFlowrate;
        protected Pump m_Pump = null;
        //private PumpAct m_RefAct = PumpAct.Stop;
        //private PumpAct m_ManualAct;
        private Gauge m_Co2InPress = null;
        private Gauge m_Resistivity = null;
        protected Gauge m_FilterInPress = null;
        protected Gauge m_FilterOutPress = null;
        private Gauge m_ShowerFlow = null;
        private Gauge m_MainDiPress = null;
        private Gauge m_InverHertz = null;
        private Gauge m_InvertCurrent = null;
        private Gauge m_Co2InFlow = null; // 11.05.03 minhan
        private LeakSensor m_LeakSensor = null;
        private AutoValve m_Co2InValve = null;
        private AutoValve m_Co2VentValve = null;
        protected Inverter m_Inverter = null;
        //private IoDigitalInput m_DiPowerOn = new IoDigitalInput();
        //private IoDigitalInput m_DiInverterAlarm = new IoDigitalInput();
        //private IoDigitalInput m_DiPumpManualRun = new IoDigitalInput();
        //private IoDigitalInput m_DiPumpManualStop = new IoDigitalInput();
        //private IoDigitalOutput m_DoInverterRun = new IoDigitalOutput();
        //private IoDigitalOutput m_DoInverterReset = new IoDigitalOutput();
        //private IoAnalogOutput m_AoInverterFrequency = new IoAnalogOutput();
        static protected XLog m_HpmjLog;// = new XLog("HpmjLog", XLog.LogStampType.UseStamp);

        private CvUnit m_Owner = null;
        //private List<Alarm> m_DiAlarmList = null;

        public Alarm ALM_LocalAlarm = null;
        public Alarm ALM_RecipeAlarm = null;
        //public Alarm ALM_PumpRunAlarm = null;
        public Alarm ALM_NotReadyAlarm = null;
        public Alarm ALM_DisconnectAlarm = null;
        public Alarm ALM_EmoAlarm = null;
        //public Alarm ALM_PowerOnAlarm = null;
        public Alarm ALM_LeakAlarm = null;
        public Alarm ALM_InverterAlarm = null;
        public Alarm ALM_InverterElbTripAlarm = null;
        public Alarm ALM_InverterMcTripAlarm = null;
        //public Alarm ALM_Co2GenPowerAlarm = null;
        public Alarm ALM_InverterMcOnCheckAlarm = null;
        public Alarm ALM_PumpPackingTimeoverAlarm = null;
        public Alarm ALM_FilterChangeTimeoverAlarm = null;
        public Alarm ALM_MelsecCommAlarm = null;
        public Alarm ALM_PidControlAlarm = null;
        protected ushort m_MaxHertzAoValue = 0x7FFF;
        protected int m_PressureMinimumValue = 50; //11.09.07 sungyong
        protected int m_PressureMaximumValue = 150; //11.09.07 sungyong 
        protected int m_FrequencyMinimumValue = 10; //11.09.07 sungyong
        protected int m_FrequencyMaximumValue = 60; //11.09.07 sungyong
        #endregion

        #region Properties
        [Category("Setting : Device")]
        public Pump Pump
        {
            get { return m_Pump; }
            set { m_Pump = value; }
        }
        [Category("Setting : Device")]
        public Inverter Inverter
        {
            get { return m_Inverter; }
            set { m_Inverter = value; }
        }
        [Category("Setting : Device")]
        public Gauge CO2InPress
        {
            get { return m_Co2InPress; }
            set { m_Co2InPress = value; }
        }
        [Category("Setting : Device")]
        public Gauge MainDiPress
        {
            get { return m_MainDiPress; }
            set { m_MainDiPress = value; }
        }
        [Category("Setting : Device")]
        public Gauge InverHertz
        {
            get { return m_InverHertz; }
            set { m_InverHertz = value; }
        }
        [Category("Setting : Device")]
        public Gauge InvertCurrent
        {
            get { return m_InvertCurrent; }
            set { m_InvertCurrent = value; }
        }
        [Category("Setting : Device")]
        public Gauge Resistivity
        {
            get { return m_Resistivity; }
            set { m_Resistivity = value; }
        }
        [Category("Setting : Device")]
        public Gauge FilterInPress
        {
            get { return m_FilterInPress; }
            set { m_FilterInPress = value; }
        }
        [Category("Setting : Device")]
        public Gauge FilterOutPress
        {
            get { return m_FilterOutPress; }
            set { m_FilterOutPress = value; }
        }
        [Category("Setting : Device")]
        public Gauge HpmjFlow
        {
            get { return m_ShowerFlow; }
            set { m_ShowerFlow = value; }
        }
        [Category("Setting : Device")]
        public Gauge HpmjCo2Flow // 11.05.03 minhan
        {
            get { return m_Co2InFlow; }
            set { m_Co2InFlow = value; }
        }
        [Category("Setting : Device")]
        public LeakSensor LeakSensor
        {
            get { return m_LeakSensor; }
            set { m_LeakSensor = value; }
        }
        [Category("Setting : Device")]
        public AutoValve CO2InValve
        {
            get { return m_Co2InValve; }
            set { m_Co2InValve = value; }
        }
        [Category("Setting : Device")]
        public AutoValve CO2VentValve
        {
            get { return m_Co2VentValve; }
            set { m_Co2VentValve = value; }
        }
        //[Category("Setting : IO")]
        //public IoDigitalInput diPower_On
        //{
        //    get { return m_DiPowerOn; }
        //    set { m_DiPowerOn = value; }
        //}
        [Category("DMS : Setting")]
        public CvUnit OwnerUnit
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        //        [Category("Setting : IO")]
        //        public IoDigitalInput diCO2_Generator_Power
        //        {
        //            get { return m_DiCo2GenPower; }
        //            set { m_DiCo2GenPower = value; }
        //        }
        //[Category("Setting : IO")]
        //public IoDigitalInput diPump_Manual_Run
        //{
        //    get { return m_DiPumpManualRun; }
        //    set { m_DiPumpManualRun = value; }
        //}
        //[Category("Setting : IO")]
        //public IoDigitalInput diPump_Manual_Stop
        //{
        //    get { return m_DiPumpManualStop; }
        //    set { m_DiPumpManualStop = value; }
        //}
        //[Category("Setting : IO")]
        //public IoDigitalOutput doManual_On
        //{
        //    get { return m_DoManualOn; }
        //    set { m_DoManualOn = value; }
        //}
        //[Category("Setting : IO")]
        //public IoDigitalOutput doPump_Mc_On
        //{
        //    get { return m_DoPumpMcOn; }
        //    set { m_DoPumpMcOn = value; }
        //}
        [Browsable(false), XmlIgnore()]
        public TagHpmjIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjUse
        {
            get { return m_SetupHpmjUse; }
            set { m_SetupHpmjUse = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjMode
        {
            get { return m_SetupHpmjMode; }
            set { m_SetupHpmjMode = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjPressure
        {
            get { return m_SetupHpmjPressure; }
            set { m_SetupHpmjPressure = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjFrequency
        {
            get { return m_SetupHpmjFrequency; }
            set { m_SetupHpmjFrequency = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjFilterChangeTime
        {
            get { return m_SetupHpmjFilterChangeTime; }
            set { m_SetupHpmjFilterChangeTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjPackingChangeTime
        {
            get { return m_SetupHpmjPackingChangeTime; }
            set { m_SetupHpmjPackingChangeTime = value; }
        }
        [Browsable(false), XmlIgnore()]                     // dspcrassus - HPMJ Unit
        public TagSetupInfo SetupHpmjCO2InValveCloseDiLevel
        {
            get { return m_SetupHpmjCO2InValveCloseDiLevel; }
            set { m_SetupHpmjCO2InValveCloseDiLevel = value; }
        }
        [Browsable(false), XmlIgnore()]                     // dspcrassus - 090831 : CO2 Vent Valve 제어권 설정(H/W or S/W), 불필요시 삭제
        public TagSetupInfo SetupHpmjCO2VentControl
        {
            get { return m_SetupHpmjCO2VentControl; }
            set { m_SetupHpmjCO2VentControl = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjCO2VentValveOpenTime
        {
            get { return m_SetupHpmjCO2VentValveOpenTime; }
            set { m_SetupHpmjCO2VentValveOpenTime = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjCO2VentValveOpenPeriod
        {
            get { return m_SetupHpmjCO2VentValveOpenPeriod; }
            set { m_SetupHpmjCO2VentValveOpenPeriod = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjStandardPressure
        {
            get { return m_SetupHpmjStandardPressure; }
            set { m_SetupHpmjStandardPressure = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagSetupInfo SetupHpmjStandardFlowrate
        {
            get { return m_SetupHpmjStandardFlowrate; }
            set { m_SetupHpmjStandardFlowrate = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsUse
        {
            get
            {
                if (m_SetupHpmjUse == null) return false;
                return m_SetupHpmjUse.GetValue<bool>();
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool IsPressureMode
        {
            get
            {
                if (m_SetupHpmjMode == null) return false;
                return m_SetupHpmjMode.GetValue<bool>();
            }
        }
        [Category("DMS : Option"), Description("Control by Setup or Recipe")]
        public ProcessControlBy ControlBy
        {
            get { return m_ControlBy; }
            set { m_ControlBy = value; }
        }
        public XLog HpmjLog
        {
            get { return m_HpmjLog; }
        }
        [Category("DMS : Setting")]
        public ushort MaxHertzAoValue
        {
            get { return m_MaxHertzAoValue; }
            set { m_MaxHertzAoValue = value; }
        }
        [Category("DMS : Setting")]
        public int PressureMinimumValue//11.09.07 sungyong
        {
            get { return m_PressureMinimumValue; }
            set { m_PressureMinimumValue = value; }
        }
        [Category("DMS : Setting")]
        public int PressureMaximumValue//11.09.07 sungyong
        {
            get { return m_PressureMaximumValue; }
            set { m_PressureMaximumValue = value; }
        }
        [Category("DMS : Setting")]
        public int FrequencyMinimumValue//11.09.07 sungyong
        {
            get { return m_FrequencyMinimumValue; }
            set { m_FrequencyMinimumValue = value; }
        }
        [Category("DMS : Setting")]
        public int FrequencyMaximumValue//11.09.07 sungyong
        {
            get { return m_FrequencyMaximumValue; }
            set { m_FrequencyMaximumValue = value; }
        }
        #endregion

        #region Constructure
        protected Hpmj() { }
        #endregion

        #region Methods
        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("HpmjUnit\t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);
            HpmjLog.TextOut(log);
            //m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        public void SetHpmjLog(string message)
        {
            m_HpmjLog.TextOut(message);
        }

        public void PumpRun()
        {
            m_Pump.Run();//only for view!
            m_Inverter.Run(true);
        }

        public void PumpStop()
        {
            m_Pump.Stop();//only for view!
            m_Inverter.Run(false);
        }

        public void Remote()
        {
            //m_DoManualOn.SetState(false);
        }

        public void Local()
        {
            //m_DoManualOn.SetState(true);
        }

        //public void HpmjPumpMcOn()
        //{
        //    //m_DoPumpMcOn.SetState(true);
        //}

        //public void HpmjPumpMcOff()
        //{
        //    //m_DoPumpMcOn.SetState(false);
        //}

        public bool IsHpmjInverterAlarm()
        {
            return m_Inverter.IsAlarm();
        }

        public void HpmjInverterResetOn()
        {
            m_Inverter.Reset(true);
        }

        public void HpmjInverterResetOff()
        {
            m_Inverter.Reset(false);
        }

        public void HpmjInverterRun()
        {
            m_Pump.Run();
            m_Inverter.Run(true);
        }

        public void HpmjInverterStop()
        {
            m_Pump.Stop();//only for view!
            m_Inverter.Run(false);
        }

        public void SetPumpHertz(double hertz)
        {
            m_Inverter.SetCurrentFrequency(hertz);
        }

        public void SetPumpAdc(ushort adc)
        {
            m_Inverter.SetCurrentOutput(adc);
        }

        public double GetPumpHertz()
        {
            return m_Inverter.GetCurrentFrequency();
        }

        public ushort ConvertHertzToAdc(double hertz)
        {
            ushort adc;

            adc = (ushort)(hertz * m_MaxHertzAoValue / 60);

            return adc;
        }

        public double ConvertAdcToHertz(ushort adc)
        {
            double hertz;

            hertz = (double)adc * 60 / m_MaxHertzAoValue;

            return hertz;
        }
        #endregion

        #region Virtuals
        public virtual void SetEmo(bool bOn)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsAlarm()
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                       bool CheckAlarmINV, ref int AlarmId_INV,
                                       bool CheckAlarmEMO, ref int AlarmId_EMO,
                                       bool CheckAlarmPMC, ref int AlarmId_PMC)
        {
            throw new NotImplementedException();
        }
        public virtual bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                       bool CheckAlarmINV, bool CheckAlarmEMO, bool CheckAlarmPMC)
        {
            throw new NotImplementedException();
        }
        public virtual void ResetAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                       bool ResetAlarmINV, ref int AlarmId_INV,
                                       bool ResetAlarmEMO, ref int AlarmId_EMO,
                                       bool ResetAlarmPMC, ref int AlarmId_PMC)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override Type FamilyType
        {
            get { return typeof(Hpmj); }
        }

        public override DmsErrors Initialize()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        #endregion
    }

    public class Hpmj_Io : Hpmj
    {
        #region Fields
        private IoDigitalInput m_DiEmo = new IoDigitalInput();
        private IoDigitalInput m_DiPumpMcTrip = new IoDigitalInput();

        public List<IoDigitalInput> ForAlarmDiList = null;
        #endregion

        #region Properties
        [Category("Setting : IO")]
        public IoDigitalInput diEMO
        {
            get { return m_DiEmo; }
            set { m_DiEmo = value; }
        }
        [Category("Setting : IO")]
        public IoDigitalInput diPump_Mc_Trip
        {
            get { return m_DiPumpMcTrip; }
            set { m_DiPumpMcTrip = value; }
        }
        #endregion

        #region Constructure
        public Hpmj_Io()
        {
            this.Name = "__ HPMJ Unit";
        }
        #endregion

        #region Methods
        #endregion

        #region Hpmj Overrides
        public override void SetEmo(bool bOn)
        {
            m_DiEmo.SetState(bOn);
        }

        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool alarm = false;

            alarm |= m_Inverter.IsAlarm();
            alarm |= m_DiEmo.GetState();
            //alarm |= m_LeakSensor.IsDetected();
            //            alarm |= m_DiPumpElbTrip.GetState();
            alarm |= m_DiPumpMcTrip.GetState();
            //            alarm |= m_DiCo2GenPower.GetState();

            return alarm;
        }

        public override bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool CheckAlarmINV, ref int AlarmId_INV,
                                        bool CheckAlarmEMO, ref int AlarmId_EMO,
                                        bool CheckAlarmPMC, ref int AlarmId_PMC)
        {
            bool bAlarm = false;
            foreach (IoDigitalInput Di in ForAlarmDiList)
            {
                if (Di.GetState())
                {
                    if (CheckAlarmINV && Di.Id == m_Inverter.GetAlarmId())
                    {
                        EqpManager.SetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                        AlarmId_INV = ALM_InverterAlarm.Id;
                    }
                    if (CheckAlarmEMO && Di.Id == m_DiEmo.Id)
                    {
                        EqpManager.SetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                        AlarmId_EMO = ALM_EmoAlarm.Id;
                    }
                    if (CheckAlarmPMC && Di.Id == m_DiPumpMcTrip.Id)
                    {
                        EqpManager.SetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");
                        AlarmId_PMC = ALM_InverterMcTripAlarm.Id;
                    }
                }
            }
            return bAlarm;
        }

        public override bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool CheckAlarmINV, bool CheckAlarmEMO, bool CheckAlarmPMC)
        {
            bool bAlarm = false;
            foreach (IoDigitalInput Di in ForAlarmDiList)
            {
                if (Di.GetState())
                {
                    if (CheckAlarmINV && Di.Id == m_Inverter.GetAlarmId())
                    {
                        EqpManager.SetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                    }
                    if (CheckAlarmEMO && Di.Id == m_DiEmo.Id)
                    {
                        EqpManager.SetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                    }
                    if (CheckAlarmPMC && Di.Id == m_DiPumpMcTrip.Id)
                    {
                        EqpManager.SetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");
                    }
                    bAlarm = true;
                }
            }
            return bAlarm;
        }

        public override void ResetAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool ResetAlarmINV, ref int AlarmId_INV,
                                        bool ResetAlarmEMO, ref int AlarmId_EMO,
                                        bool ResetAlarmPMC, ref int AlarmId_PMC)
        {
            foreach (IoDigitalInput Di in ForAlarmDiList)
            {
                if (!Di.GetState())
                {
                    if (ResetAlarmINV && Di.Id == m_Inverter.GetAlarmId() && AlarmId_INV != 0)
                    {
                        EqpManager.ResetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm Reset");
                        AlarmId_INV = 0;
                    }
                    if (ResetAlarmEMO && Di.Id == m_DiEmo.Id && AlarmId_EMO != 0)
                    {
                        EqpManager.ResetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm Reset");
                        AlarmId_EMO = 0;
                    }
                    if (ResetAlarmPMC && Di.Id == m_DiPumpMcTrip.Id && AlarmId_PMC != 0)
                    {
                        EqpManager.ResetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm Reset");
                        AlarmId_PMC = 0;
                    }
                }
            }
        }
        #endregion

        #region Overrides
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
            ok &= (m_Pump != null);
            ok &= (m_FilterInPress != null);
            ok &= (m_FilterOutPress != null);
            ok &= (m_Inverter != null);
            ok &= (m_DiEmo != null);
            // ok &= (m_DiLeak != null);
            ok &= (m_DiPumpMcTrip != null);
            // ok &= (m_DiPowerOn != null);
            // ok &= (m_DiPumpMcTrip != null);
            // ok &= (m_DiPumpElbTrip != null);
            // ok &= (m_DiCo2GenPower != null);
            // ok &= (m_DiPumpManualRun != null);
            // ok &= (m_DiPumpManualStop != null);

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
                ALM_LocalAlarm = new Alarm(this.Name + " Mode is Local Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_RecipeAlarm = new Alarm(this.Name + " Recipe Setting Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_PumpRunAlarm = new Alarm(this.Name + " Pump Run Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_NotReadyAlarm = new Alarm(this.Name + " Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_DisconnectAlarm = new Alarm(this.Name + " Disconnected Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EmoAlarm = new Alarm(this.Name + " EMO Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_PowerOnAlarm = new Alarm(this.Name + " Power Off Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_LeakAlarm = new Alarm(this.Name + " Leakage Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_InverterAlarm = new Alarm(this.Name + " Inverter Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_InverterElbTripAlarm = new Alarm(this.Name + " Inverter ELB Trip Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_InverterMcTripAlarm = new Alarm(this.Name + " Inverter MC Trip Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_Co2GenPowerAlarm = new Alarm(this.Name + " CO2 Generator Power Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_InverterMcOnCheckAlarm = new Alarm(this.Name + " Inverter MC On Check Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PumpPackingTimeoverAlarm = new Alarm(this.Name + " Pump Packing Change Time-Over Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_FilterChangeTimeoverAlarm = new Alarm(this.Name + " Filter Change Time-Over Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_MelsecCommAlarm = new Alarm(this.Name + " Melset-Net Communication Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_PidControlAlarm = new Alarm(this.Name + " PID Control Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ForAlarmDiList.Add(m_DiEmo);
                //ForAlarmDiList.Add(m_DiPowerOn);
                //ForAlarmDiList.Add(m_DiPumpElbTrip);
                ForAlarmDiList.Add(m_DiPumpMcTrip);
                //ForAlarmDiList.Add(m_DiCo2GenPower);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupHpmjInfoProvider setupHpmjInfoProvider = SetupHpmjInfoProvider.Instance;
                //if (m_ControlBy == ProcessControlBy.Setup)
                {
                    if (m_PressureMinimumValue > m_PressureMaximumValue || m_PressureMinimumValue < 50) m_PressureMinimumValue = 50; //11.09.07 sungyong
                    if (m_PressureMinimumValue > m_PressureMaximumValue || m_PressureMaximumValue > 150) m_PressureMaximumValue = 150; //11.09.07 sungyong
                    string min = Convert.ToString(m_PressureMinimumValue);//11.09.07 sungyong
                    string max = Convert.ToString(m_PressureMaximumValue);//11.09.07 sungyong

                    m_SetupHpmjUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjUse);
                    m_SetupHpmjPressure = new TagSetupInfo(this.Name + " Pressure Set", OptionType.Alternative, OptionFormat.Digit, UnitType.Bar, min, min, max);//11.09.07 sungyong
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjPressure);
                }
                m_SetupHpmjMode = new TagSetupInfo(this.Name + " Running Mode", OptionType.Alternative, OptionFormat.HpmjMode, UnitType.None, HpmjMode.Pressure.ToString());
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjMode);
                {
                    if (m_FrequencyMinimumValue > m_FrequencyMaximumValue || m_FrequencyMinimumValue < 10) m_FrequencyMinimumValue = 10;//11.09.07 sungyong
                    if (m_FrequencyMinimumValue > m_FrequencyMaximumValue || m_FrequencyMaximumValue > 60) m_FrequencyMaximumValue = 60;//11.09.07 sungyong
                    string min = Convert.ToString(m_FrequencyMinimumValue);//11.09.07 sungyong
                    string max = Convert.ToString(m_FrequencyMaximumValue);//11.09.07 sungyong
                    m_SetupHpmjFrequency = new TagSetupInfo(this.Name + " Frequency Set", OptionType.Alternative, OptionFormat.Digit, UnitType.Hz, min, min, max);//11.09.07 sungyong
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjFrequency);
                }
                //m_SetupHpmjFilterChangeTime = new TagSetupInfo(this.Name + " Filter Change Cycle", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "1000"); // 09.12.20 minhan
                //setupHpmjInfoProvider.InitFromDB(m_SetupHpmjFilterChangeTime);
                //m_SetupHpmjPackingChangeTime = new TagSetupInfo(this.Name + " Pump Packing Change Cycle", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "1000");
                //setupHpmjInfoProvider.InitFromDB(m_SetupHpmjPackingChangeTime);
                m_SetupHpmjCO2InValveCloseDiLevel = new TagSetupInfo(this.Name + " CO2 IN Valve Close DI Level", OptionType.Alternative, OptionFormat.Float, UnitType.Bar, "0.5", "0.5", "5");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2InValveCloseDiLevel);
                m_SetupHpmjCO2VentControl = new TagSetupInfo(this.Name + " CO2 Vent Valve S/W Control Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentControl);
                m_SetupHpmjCO2VentValveOpenTime = new TagSetupInfo(this.Name + " CO2 Vent Valve Open Time", OptionType.Alternative, OptionFormat.Float, UnitType.sec, "1.0", "0.5", "3");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentValveOpenTime);
                m_SetupHpmjCO2VentValveOpenPeriod = new TagSetupInfo(this.Name + " CO2 Vent Valve Open Period", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "6", "1", "10");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentValveOpenPeriod);
                m_SetupHpmjStandardPressure = new TagSetupInfo(this.Name + " Standard Pressure for Interlock", OptionType.Alternative, OptionFormat.Digit, UnitType.Bar, "150");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjStandardPressure);
                m_SetupHpmjStandardFlowrate = new TagSetupInfo(this.Name + " Standard Flowrate for Interlock", OptionType.Alternative, OptionFormat.Float, UnitType.lpm, "10.0");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjStandardFlowrate);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagHpmjIfFlag(this);
                m_IfFlag.Reset();
                m_HpmjLog = new XLog("HpmjLog", XLog.LogStampType.UseStamp);

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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.EMO, diEMO.GetState());
            //m_Tag.SetValue(tagDescriptor.POWER, diPower_On.GetState());
            m_Tag.SetValue(tagDescriptor.MCTRIP, diPump_Mc_Trip.GetState());
            //m_Tag.SetValue(tagDescriptor.ELBTRIP, diPump_Elb_Trip.GetState());
            //            m_Tag.SetValue(tagDescriptor.CO2GENPOWER, diCO2_Generator_Power.GetState());
            m_Tag.SetValue(tagDescriptor.INVERTER_FRQ, Inverter.GetCurrentFrequency());
        }
        #endregion
    }

    public class Hpmj_Ec : Hpmj
    {
        #region Fields
        private SlaveDigitalInput m_DiEmo = new SlaveDigitalInput();
        private SlaveDigitalInput m_DiPumpMcTrip = new SlaveDigitalInput();

        public List<SlaveDigitalInput> ForAlarmDiList = null;
        #endregion

        #region Properties
        [Category("Setting : IO")]
        public SlaveDigitalInput diEMO
        {
            get { return m_DiEmo; }
            set { m_DiEmo = value; }
        }
        [Category("Setting : IO")]
        public SlaveDigitalInput diPump_Mc_Trip
        {
            get { return m_DiPumpMcTrip; }
            set { m_DiPumpMcTrip = value; }
        }
        #endregion

        #region Constructure
        public Hpmj_Ec()
        {
            this.Name = "__ HPMJ Unit";
        }
        #endregion

        #region Methods
        #endregion

        #region Hpmj Overrides
        public override void SetEmo(bool bOn)
        {
            m_DiEmo.SetState(bOn);
        }

        public override bool IsAlarm()
        {
            if (!this.Initialized) return false;

            bool alarm = false;

            alarm |= m_Inverter.IsAlarm();
            alarm |= m_DiEmo.GetState();
            //alarm |= m_LeakSensor.IsDetected();
            //            alarm |= m_DiPumpElbTrip.GetState();
            alarm |= m_DiPumpMcTrip.GetState();
            //            alarm |= m_DiCo2GenPower.GetState();

            return alarm;
        }

        public override bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool CheckAlarmINV, ref int AlarmId_INV,
                                        bool CheckAlarmEMO, ref int AlarmId_EMO,
                                        bool CheckAlarmPMC, ref int AlarmId_PMC)
        {
            bool bAlarm = false;
            foreach (SlaveDigitalInput Di in ForAlarmDiList)
            {
                if (Di.GetState())
                {
                    if (CheckAlarmINV && Di.Id == m_Inverter.GetAlarmId())
                    {
                        EqpManager.SetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                        AlarmId_INV = ALM_InverterAlarm.Id;
                    }
                    if (CheckAlarmEMO && Di.Id == m_DiEmo.Id)
                    {
                        EqpManager.SetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                        AlarmId_EMO = ALM_EmoAlarm.Id;
                    }
                    if (CheckAlarmPMC && Di.Id == m_DiPumpMcTrip.Id)
                    {
                        EqpManager.SetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");
                        AlarmId_PMC = ALM_InverterMcTripAlarm.Id;
                    }
                }
            }
            return bAlarm;
        }

        public override bool CheckAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool CheckAlarmINV, bool CheckAlarmEMO, bool CheckAlarmPMC)
        {
            bool bAlarm = false;
            foreach (SlaveDigitalInput Di in ForAlarmDiList)
            {
                if (Di.GetState())
                {
                    if (CheckAlarmINV && Di.Id == m_Inverter.GetAlarmId())
                    {
                        EqpManager.SetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                    }
                    if (CheckAlarmEMO && Di.Id == m_DiEmo.Id)
                    {
                        EqpManager.SetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                    }
                    if (CheckAlarmPMC && Di.Id == m_DiPumpMcTrip.Id)
                    {
                        EqpManager.SetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");
                    }
                    bAlarm = true;
                }
            }
            return bAlarm;
        }

        public override void ResetAlarm(string SeqFunName, ref IEqpManager EqpManager,
                                        bool ResetAlarmINV, ref int AlarmId_INV,
                                        bool ResetAlarmEMO, ref int AlarmId_EMO,
                                        bool ResetAlarmPMC, ref int AlarmId_PMC)
        {
            foreach (SlaveDigitalInput Di in ForAlarmDiList)
            {
                if (!Di.GetState())
                {
                    if (ResetAlarmINV && Di.Id == m_Inverter.GetAlarmId() && AlarmId_INV != 0)
                    {
                        EqpManager.ResetAlarm(ALM_InverterAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm Reset");
                        AlarmId_INV = 0;
                    }
                    if (ResetAlarmEMO && Di.Id == m_DiEmo.Id && AlarmId_EMO != 0)
                    {
                        EqpManager.ResetAlarm(ALM_EmoAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm Reset");
                        AlarmId_EMO = 0;
                    }
                    if (ResetAlarmPMC && Di.Id == m_DiPumpMcTrip.Id && AlarmId_PMC != 0)
                    {
                        EqpManager.ResetAlarm(ALM_InverterMcTripAlarm.Id);
                        SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm Reset");
                        AlarmId_PMC = 0;
                    }
                }
            }
        }
        #endregion

        #region Overrides
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
            ok &= (m_Pump != null);
            ok &= (m_FilterInPress != null);
            ok &= (m_FilterOutPress != null);
            ok &= (m_Inverter != null);
            ok &= (m_DiEmo != null);
            // ok &= (m_DiLeak != null);
            ok &= (m_DiPumpMcTrip != null);
            // ok &= (m_DiPowerOn != null);
            // ok &= (m_DiPumpMcTrip != null);
            // ok &= (m_DiPumpElbTrip != null);
            // ok &= (m_DiCo2GenPower != null);
            // ok &= (m_DiPumpManualRun != null);
            // ok &= (m_DiPumpManualStop != null);

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
                ALM_LocalAlarm = new Alarm(this.Name + " Mode is Local Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_RecipeAlarm = new Alarm(this.Name + " Recipe Setting Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_PumpRunAlarm = new Alarm(this.Name + " Pump Run Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_NotReadyAlarm = new Alarm(this.Name + " Not Ready Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_DisconnectAlarm = new Alarm(this.Name + " Disconnected Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_EmoAlarm = new Alarm(this.Name + " EMO Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_PowerOnAlarm = new Alarm(this.Name + " Power Off Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_LeakAlarm = new Alarm(this.Name + " Leakage Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_InverterAlarm = new Alarm(this.Name + " Inverter Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_InverterElbTripAlarm = new Alarm(this.Name + " Inverter ELB Trip Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_InverterMcTripAlarm = new Alarm(this.Name + " Inverter MC Trip Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_Co2GenPowerAlarm = new Alarm(this.Name + " CO2 Generator Power Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //ALM_InverterMcOnCheckAlarm = new Alarm(this.Name + " Inverter MC On Check Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                ALM_PumpPackingTimeoverAlarm = new Alarm(this.Name + " Pump Packing Change Time-Over Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_FilterChangeTimeoverAlarm = new Alarm(this.Name + " Filter Change Time-Over Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_MelsecCommAlarm = new Alarm(this.Name + " Melset-Net Communication Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
                ALM_PidControlAlarm = new Alarm(this.Name + " PID Control Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);


                ForAlarmDiList.Add(m_DiEmo);
                //ForAlarmDiList.Add(m_DiPowerOn);
                //ForAlarmDiList.Add(m_DiPumpElbTrip);
                ForAlarmDiList.Add(m_DiPumpMcTrip);
                //ForAlarmDiList.Add(m_DiCo2GenPower);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupHpmjInfoProvider setupHpmjInfoProvider = SetupHpmjInfoProvider.Instance;
                //if (m_ControlBy == ProcessControlBy.Setup)
                {
                    if (m_PressureMinimumValue > m_PressureMaximumValue || m_PressureMinimumValue < 50) m_PressureMinimumValue = 50; //11.09.07 sungyong
                    if (m_PressureMinimumValue > m_PressureMaximumValue || m_PressureMaximumValue > 150) m_PressureMaximumValue = 150; //11.09.07 sungyong
                    string min = Convert.ToString(m_PressureMinimumValue);//11.09.07 sungyong
                    string max = Convert.ToString(m_PressureMaximumValue);//11.09.07 sungyong

                    m_SetupHpmjUse = new TagSetupInfo(this.Name + " Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjUse);
                    m_SetupHpmjPressure = new TagSetupInfo(this.Name + " Pressure Set", OptionType.Alternative, OptionFormat.Digit, UnitType.Bar, min, min, max);//11.09.07 sungyong
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjPressure);
                }
                m_SetupHpmjMode = new TagSetupInfo(this.Name + " Running Mode", OptionType.Alternative, OptionFormat.HpmjMode, UnitType.None, HpmjMode.Pressure.ToString());
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjMode);
                {
                    if (m_FrequencyMinimumValue > m_FrequencyMaximumValue || m_FrequencyMinimumValue < 10) m_FrequencyMinimumValue = 10;//11.09.07 sungyong
                    if (m_FrequencyMinimumValue > m_FrequencyMaximumValue || m_FrequencyMaximumValue > 60) m_FrequencyMaximumValue = 60;//11.09.07 sungyong
                    string min = Convert.ToString(m_FrequencyMinimumValue);//11.09.07 sungyong
                    string max = Convert.ToString(m_FrequencyMaximumValue);//11.09.07 sungyong
                    m_SetupHpmjFrequency = new TagSetupInfo(this.Name + " Frequency Set", OptionType.Alternative, OptionFormat.Digit, UnitType.Hz, min, min, max);//11.09.07 sungyong
                    setupHpmjInfoProvider.InitFromDB(m_SetupHpmjFrequency);
                }
                //m_SetupHpmjFilterChangeTime = new TagSetupInfo(this.Name + " Filter Change Cycle", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "1000"); // 09.12.20 minhan
                //setupHpmjInfoProvider.InitFromDB(m_SetupHpmjFilterChangeTime);
                //m_SetupHpmjPackingChangeTime = new TagSetupInfo(this.Name + " Pump Packing Change Cycle", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "1000");
                //setupHpmjInfoProvider.InitFromDB(m_SetupHpmjPackingChangeTime);
                m_SetupHpmjCO2InValveCloseDiLevel = new TagSetupInfo(this.Name + " CO2 IN Valve Close DI Level", OptionType.Alternative, OptionFormat.Float, UnitType.Bar, "0.5", "0.5", "5");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2InValveCloseDiLevel);
                m_SetupHpmjCO2VentControl = new TagSetupInfo(this.Name + " CO2 Vent Valve S/W Control Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.Use.ToString());
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentControl);
                m_SetupHpmjCO2VentValveOpenTime = new TagSetupInfo(this.Name + " CO2 Vent Valve Open Time", OptionType.Alternative, OptionFormat.Float, UnitType.sec, "1.0", "0.5", "3");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentValveOpenTime);
                m_SetupHpmjCO2VentValveOpenPeriod = new TagSetupInfo(this.Name + " CO2 Vent Valve Open Period", OptionType.Alternative, OptionFormat.Digit, UnitType.hour, "6", "1", "10");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjCO2VentValveOpenPeriod);
                m_SetupHpmjStandardPressure = new TagSetupInfo(this.Name + " Standard Pressure for Interlock", OptionType.Alternative, OptionFormat.Digit, UnitType.Bar, "150");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjStandardPressure);
                m_SetupHpmjStandardFlowrate = new TagSetupInfo(this.Name + " Standard Flowrate for Interlock", OptionType.Alternative, OptionFormat.Float, UnitType.lpm, "10.0");
                setupHpmjInfoProvider.InitFromDB(m_SetupHpmjStandardFlowrate);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagHpmjIfFlag(this);
                m_IfFlag.Reset();
                m_HpmjLog = new XLog("HpmjLog", XLog.LogStampType.UseStamp);

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

        public override void UpdateTag()
        {
            m_Tag.SetValue(tagDescriptor.EMO, diEMO.GetState());
            //m_Tag.SetValue(tagDescriptor.POWER, diPower_On.GetState());
            m_Tag.SetValue(tagDescriptor.MCTRIP, diPump_Mc_Trip.GetState());
            //m_Tag.SetValue(tagDescriptor.ELBTRIP, diPump_Elb_Trip.GetState());
            //            m_Tag.SetValue(tagDescriptor.CO2GENPOWER, diCO2_Generator_Power.GetState());
            m_Tag.SetValue(tagDescriptor.INVERTER_FRQ, Inverter.GetCurrentFrequency());
        }
        #endregion
    }
}

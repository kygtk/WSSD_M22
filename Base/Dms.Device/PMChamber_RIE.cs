using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Dms.Data;
using Dms.Common;

namespace Dms.Device
{
    public class TagChamberIfFlag
    {
        #region
        private bool recvReady;
        private bool recvComp;
        private bool sendReady;
        private bool sendComp;
        #endregion

        #region Properties
        public bool RecvReady
        {
            get { return recvReady; }
            set { recvReady = value; }
        }
        public bool RecvComp
        {
            get { return recvComp; }
            set { recvComp = value; }
        }
        public bool SendReady
        {
            get { return sendReady; }
            set { sendReady = value; }
        }
        public bool SendComp
        {
            get { return sendComp; }
            set { sendComp = value; }
        }
        #endregion

        public void Reset()
        {
            recvReady = false;
            recvComp = false;
            sendReady = false;
            sendComp = false;
        }
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PMChamber_RIE : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorPmChamber tagDescriptor = new TagDescriptorPmChamber();
        #endregion

        #region Fields
        private Mfc m_MfcSF6 = new Mfc();
        private Mfc m_MfcCL2 = new Mfc();
        private Mfc m_MfcO2 = new Mfc();
        private Mfc m_MfcPN2 = new Mfc();

        private AutoValve m_GN2InValve;
        private AutoValve m_GN2SlowValve;
        private AutoValve m_GN2FastValve;

        private AutoValve m_PN2InValve;
        private AutoValve m_PN2OutValve;

        private AutoValve m_SF6InValve;
        private AutoValve m_SF6PurgeValve;
        private AutoValve m_SF6OutValve;

        private AutoValve m_CL2InValve;
        private AutoValve m_CL2PurgeValve;
        private AutoValve m_CL2OutValve;

        private AutoValve m_O2InValve;
        private AutoValve m_O2OutValve;

        private AutoValve m_MixGasInValve;
        private AutoValve m_GN2OutValve;

        private AutoValve m_ChamberBGValve;
        private AutoValve m_ChamberIGValve;

        private AutoValve m_ReliefValve;

        private AutoValve m_AngleValve1;
        private AutoValve m_AngleValve2;

        private AutoValve2 m_GateValve;

        private ServoUnit m_PinUpdownServoUnit;
        private BufferUnit m_BufferUnit;

        private Apc m_Apc;
        private ShutterUnit m_Shutter;
        private DryPumpUnit m_DryPumpUnit;
        private HeatExchanger m_HeatExchanger;

        private Seren_Rfg m_Rfg;
        private RfTuner m_RfTuner;

        private Sensor m_ChamberAtmSensor;
        private Sensor m_VentLineAtmSensor;

        private StepRunAct m_StepRunAct = StepRunAct.Noop;
        private int m_CyclePurgeCount = 0;
        private bool m_ManaulaInitComp = false;

        private Gauge m_ChamberCG;
        private Gauge m_ChamberIG;

        private Buzzer m_Buzzer;

        private string m_RunState = "Noop";
        private string m_Process = "0";
        private string m_ChamberMessage = "None";
        private bool m_TrayExist = false;

        private StepRunAct m_ProcessStep = StepRunAct.Noop;

        private TagTransferIfFlag m_IfFlag = null;
        private double m_ProcessPressure = 0.0;
        private double m_BasePreseure = 0.0;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public string RunState
        {
            get { return m_RunState; }
            set { m_RunState = value; }
        }
        [Browsable(false), XmlIgnore()]
        public StepRunAct ProcessStep
        {
            get { return m_ProcessStep; }
            set { m_ProcessStep = value; }
        }
        [Browsable(false), XmlIgnore()]
        public string Process
        {
            get { return m_Process; }
            set { m_Process = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool TrayExist
        {
            get { return m_TrayExist; }
            set { m_TrayExist = value; }
        }
        [Category("Sensor Setting")]
        public Sensor ChamberAtmSensor
        {
            get { return m_ChamberAtmSensor; }
            set { m_ChamberAtmSensor = value; }
        }
        [Category("Sensor Setting")]
        public Sensor VentLineAtmSensor
        {
            get { return m_VentLineAtmSensor; }
            set { m_VentLineAtmSensor = value; }
        }

        [Category("Valve Setting")]
        public AutoValve GN2InValve
        {
            get { return m_GN2InValve; }
            set { m_GN2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2SlowValve
        {
            get { return m_GN2SlowValve; }
            set { m_GN2SlowValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2FastValve
        {
            get { return m_GN2FastValve; }
            set { m_GN2FastValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve PN2InValve
        {
            get { return m_PN2InValve; }
            set { m_PN2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve PN2OutValve
        {
            get { return m_PN2OutValve; }
            set { m_PN2OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve SF6PurgeValve
        {
            get { return m_SF6PurgeValve; }
            set { m_SF6PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve SF6InValve
        {
            get { return m_SF6InValve; }
            set { m_SF6InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve SF6OutValve
        {
            get { return m_SF6OutValve; }
            set { m_SF6OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CL2InValve
        {
            get { return m_CL2InValve; }
            set { m_CL2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CL2PurgeValve
        {
            get { return m_CL2PurgeValve; }
            set { m_CL2PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CL2OutValve
        {
            get { return m_CL2OutValve; }
            set { m_CL2OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve O2InValve
        {
            get { return m_O2InValve; }
            set { m_O2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve O2OutValve
        {
            get { return m_O2OutValve; }
            set { m_O2OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve MixGasInValve
        {
            get { return m_MixGasInValve; }
            set { m_MixGasInValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2OutValve
        {
            get { return m_GN2OutValve; }
            set { m_GN2OutValve = value; }
        }

        [Category("Valve Setting")]
        public AutoValve ChamberBGValve
        {
            get { return m_ChamberBGValve; }
            set { m_ChamberBGValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve ChamberIGValve
        {
            get { return m_ChamberIGValve; }
            set { m_ChamberIGValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve ReliefValve
        {
            get { return m_ReliefValve; }
            set { m_ReliefValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve AngleValve1
        {
            get { return m_AngleValve1; }
            set { m_AngleValve1 = value; }
        }
        [Category("Valve Setting")]
        public AutoValve AngleValve2
        {
            get { return m_AngleValve2; }
            set { m_AngleValve2 = value; }
        }
        [Category("Valve Setting")]
        public AutoValve2 GateValve
        {
            get { return m_GateValve; }
            set { m_GateValve = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcPN2
        {
            get { return m_MfcPN2; }
            set { m_MfcPN2 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcSF6
        {
            get { return m_MfcSF6; }
            set { m_MfcSF6 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcCL2
        {
            get { return m_MfcCL2; }
            set { m_MfcCL2 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcO2
        {
            get { return m_MfcO2; }
            set { m_MfcO2 = value; }
        }
        [Category("Servo Unit Setting")]
        public ServoUnit PinUpdownServoUnit
        {
            get { return m_PinUpdownServoUnit; }
            set { m_PinUpdownServoUnit = value; }
        }
        [Category("Buffer Unit Setting")]
        public BufferUnit BufferUnit
        {
            get { return m_BufferUnit; }
            set { m_BufferUnit = value; }
        }
        [Category("Apc Setting")]
        public Apc Apc
        {
            get { return m_Apc; }
            set { m_Apc = value; }
        }
        [Category("Dry Pump Unit Setting")]
        public DryPumpUnit DryPumpUnit
        {
            get { return m_DryPumpUnit; }
            set { m_DryPumpUnit = value; }
        }
        [Category("Shutter Setting")]
        public ShutterUnit Shutter
        {
            get { return m_Shutter; }
            set { m_Shutter = value; }
        }
        [Category("RF Setting")]
        public Seren_Rfg Rfg
        {
            get { return m_Rfg; }
            set { m_Rfg = value; }
        }
        [Category("RF Setting")]
        public RfTuner RfTuner
        {
            get { return m_RfTuner; }
            set { m_RfTuner = value; }
        }
        [Category("HeatExchanger Setting")]
        public HeatExchanger HeatExchanger
        {
            get { return m_HeatExchanger; }
            set { m_HeatExchanger = value; }
        }
        [Category("Gauge Setting")]
        public Gauge ChamberCG
        {
            get { return m_ChamberCG; }
            set { m_ChamberCG = value; }
        }
        [Category("Gauge Setting")]
        public Gauge ChamberIG
        {
            get { return m_ChamberIG; }
            set { m_ChamberIG = value; }
        }
        [Category("Buzzer Control Setting")]
        public Buzzer Buzzer
        {
            get { return m_Buzzer; }
            set { m_Buzzer = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagTransferIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public double ProcessPressure
        {
            get { return m_ProcessPressure; }
            set { m_ProcessPressure = value; }
        }
        public double BasePressure
        {
            get { return m_BasePreseure; }
            set { m_BasePreseure = value; }
        }
        [Browsable(false), XmlIgnore()]
        public StepRunAct StepRunAct
        {
            get { return m_StepRunAct; }
            set { m_StepRunAct = value; }
        }
        [Browsable(false), XmlIgnore()]
        public int CyclePurgeCount
        {
            get { return m_CyclePurgeCount; }
            set { m_CyclePurgeCount = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool ManualInitComp
        {
            get { return m_ManaulaInitComp; }
            set { m_ManaulaInitComp = value; }
        }
        #endregion

        #region Constructor
        public PMChamber_RIE()
        {
            this.Name = "__ PmChamber";
        }
        #endregion

        #region Method
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

            log = string.Format("Chamber \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion

        #region Method

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
            //ok &= (m_Ai != null);


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
                //if (m_InterlockEnable)
                //{
                //    ALM_LowerAlarm = new Alarm(this.Name + " Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //    ALM_LowerWarning = new Alarm(this.Name + " Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                //    ALM_UpperWarning = new Alarm(this.Name + " Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
                //    ALM_UpperAlarm = new Alarm(this.Name + " Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
                //}

                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                //m_CalibrationProvider = CalibrationProvider.Instance;
                //m_Info = new TagCalibrationInfo(m_GaugeType, this.Name);
                //m_CalibrationProvider.InitFromDB(m_Info);

                //if (m_InterlockEnable)
                //{
                //    m_InterlockProvider = SetupGaugeInterlockProvider.Instance;
                //    m_SetupInterlock = new TagGaugeInterlock(this.Name, m_Info.Unit.Unit, 10.0, 20.0, 100.0, 150.0, false);
                //    m_InterlockProvider.InitFromDB(m_SetupInterlock);
                //}

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                //m_OldAdc = m_Ai.GetState();
                m_IfFlag = new TagTransferIfFlag();
                m_IfFlag.Reset();

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
                    if (Shutter.IsClose() == true) Shutter.Close();
                    else if (Shutter.IsOpen() == true) Shutter.Open();
                    else if (BufferUnit.HandServoUnit.IsDetectHomeSwitch(0) == true)
                    {
                        Shutter.Close();
                    }
                    else
                    {
                        Shutter.Open();
                    }

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
            string temp;

            m_Tag.SetValue(tagDescriptor.RUNSTATE, m_RunState);
            m_Tag.SetValue(tagDescriptor.PROCESS, m_Process);
            m_Tag.SetValue(tagDescriptor.TRAYEXIST, m_TrayExist);

            temp = string.Format("{0:f1}", m_Apc.GetPressure());
            m_Tag.SetValue(tagDescriptor.PRESSURE, temp);
            //m_Tag.SetValue(tagDescriptor.RFONSTATE, m_Rfg.IsRfPowerOn());
            m_Tag.SetValue(tagDescriptor.CHAMBERMESSAGE, m_ChamberMessage);
            m_Tag.SetValue(tagDescriptor.PROCESSPRESSURE, m_ProcessPressure);
            m_Tag.SetValue(tagDescriptor.ISBASEPRESSURE, m_Apc.IsBasePressureSensed(m_BasePreseure));
            m_Tag.SetValue(tagDescriptor.ISPROCESSPRESSURE, m_Apc.IsProcessPressureSensed());
            m_Tag.SetValue(tagDescriptor.PROCESSSTEP, Convert.ToInt32(m_ProcessStep));
        }
        #endregion
    }
}

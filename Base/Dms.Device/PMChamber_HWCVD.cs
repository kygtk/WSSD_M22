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
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PMChamber_HWCVD : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorPmChamber_HWCVD tagDescriptor = new TagDescriptorPmChamber_HWCVD();
        #endregion

        #region Fields

        private Mfc m_MfcPN2 = new Mfc();
        private Mfc m_MfcSiH4 = new Mfc();
        private Mfc m_MfcNF3 = new Mfc();
        private Mfc m_MfcCH4 = new Mfc();
        private Mfc m_MfcPH3 = new Mfc();
        private Mfc m_MfcB2H6 = new Mfc();
        private Mfc m_MfcNH3 = new Mfc();
        private Mfc m_MfcH2 = new Mfc();

        private AutoValve m_GN2InValve;
        private AutoValve m_GN2OutValve;

        private AutoValve m_PN2InValve;
        private AutoValve m_PN2OutValve;

        private AutoValve m_SiH4InValve;
        private AutoValve m_SiH4OutValve;
        private AutoValve m_SiH4PurgeValve;

        private AutoValve m_NF3InValve;
        private AutoValve m_NF3OutValve;
        private AutoValve m_NF3PurgeValve;

        private AutoValve m_CH4InValve;
        private AutoValve m_CH4OutValve;
        private AutoValve m_CH4PurgeValve;

        private AutoValve m_B2H6InValve;
        private AutoValve m_B2H6OutValve;
        private AutoValve m_B2H6PurgeValve;

        private AutoValve m_PH3InValve;
        private AutoValve m_PH3OutValve;
        private AutoValve m_PH3PurgeValve;

        private AutoValve m_NH3InValve;
        private AutoValve m_NH3OutValve;
        private AutoValve m_NH3PurgeValve;

        private AutoValve m_H2InValve;
        private AutoValve m_H2OutValve;

        private AutoValve m_MixGasInChamberValve;
        private AutoValve m_MixGasOutValve;
        private AutoValve m_GN2InChamberValve;

        private AutoValve m_ChamberBGValve;

        //private AutoValve m_Chamber2LockChamberValve;

        private AutoValve2 m_AngleValve;
        private AutoValve2 m_GateValve;

        private ServoUnit m_TrayTransferServoUnit;

        private DryPumpUnit m_PumpUnit;
        private Apc m_Apc;
        private ShutterUnit m_Shutter;
        private LockChamber m_LockChamber;

        private Sensor m_ChamberAtmSensor;
        private Sensor m_DoorClose;
        //private Sensor m_TrayTransferInPosSensor;
        //private Sensor m_TrayTransferOutPosSensor;

        private Gauge m_ChamberCG;
        private HotWireUnit m_HotWireUnit;

        private string m_RunState = "None";
        private string m_Process = "0";
        private string m_ChamberMessage = "None";

        private TagTrayTransferIfFlag m_IfFlag = new TagTrayTransferIfFlag();

        private double m_ProcessPressure = 0.0;
        private bool m_TrayExist = false;

        private StepRunAct m_StepRunAct = StepRunAct.Noop;
        private int m_CyclePurgeCount = 0;
        private bool m_ManaulaInitComp = false;


        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public string RunState
        {
            get { return m_RunState; }
            set { m_RunState = value; }
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
        public Sensor ChamberDoorClose
        {
            get { return m_DoorClose; }
            set { m_DoorClose = value; }
        }
        //[Category("Sensor Setting")]
        //public Sensor TrayTransferOutPosSensor
        //{
        //    get { return m_TrayTransferOutPosSensor; }
        //    set { m_TrayTransferOutPosSensor = value; }
        //}
        [Category("Valve Setting")]
        public AutoValve GN2InValve
        {
            get { return m_GN2InValve; }
            set { m_GN2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2OutValve
        {
            get { return m_GN2OutValve; }
            set { m_GN2OutValve = value; }
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
        public AutoValve SiH4InValve
        {
            get { return m_SiH4InValve; }
            set { m_SiH4InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve SiH4OutValve
        {
            get { return m_SiH4OutValve; }
            set { m_SiH4OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve SiH4PurgeValve
        {
            get { return m_SiH4PurgeValve; }
            set { m_SiH4PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NF3InValve
        {
            get { return m_NF3InValve; }
            set { m_NF3InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NF3OutValve
        {
            get { return m_NF3OutValve; }
            set { m_NF3OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NF3PurgeValve
        {
            get { return m_NF3PurgeValve; }
            set { m_NF3PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CH4InValve
        {
            get { return m_CH4InValve; }
            set { m_CH4InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CH4OutValve
        {
            get { return m_CH4OutValve; }
            set { m_CH4OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve CH4PurgeValve
        {
            get { return m_CH4PurgeValve; }
            set { m_CH4PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve B2H6InValve
        {
            get { return m_B2H6InValve; }
            set { m_B2H6InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve B2H6OutValve
        {
            get { return m_B2H6OutValve; }
            set { m_B2H6OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve B2H6PurgeValve
        {
            get { return m_B2H6PurgeValve; }
            set { m_B2H6PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve PH3InValve
        {
            get { return m_PH3InValve; }
            set { m_PH3InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve PH3OutValve
        {
            get { return m_PH3OutValve; }
            set { m_PH3OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve PH3PurgeValve
        {
            get { return m_PH3PurgeValve; }
            set { m_PH3PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NH3InValve
        {
            get { return m_NH3InValve; }
            set { m_NH3InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NH3OutValve
        {
            get { return m_NH3OutValve; }
            set { m_NH3OutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve NH3PurgeValve
        {
            get { return m_NH3PurgeValve; }
            set { m_NH3PurgeValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve H2InValve
        {
            get { return m_H2InValve; }
            set { m_H2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve H2OutValve
        {
            get { return m_H2OutValve; }
            set { m_H2OutValve = value; }
        }

        //[Category("Valve Setting")]
        //public AutoValve H2_N2InValve
        //{
        //    get { return m_H2_N2InValve; }
        //    set { m_H2_N2InValve = value; }
        //}
        [Category("Valve Setting")]
        public AutoValve MixGasOutValve
        {
            get { return m_MixGasOutValve; }
            set { m_MixGasOutValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve ChamberGasInValve
        {
            get { return m_MixGasInChamberValve; }
            set { m_MixGasInChamberValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2InChamberValve
        {
            get { return m_GN2InChamberValve; }
            set { m_GN2InChamberValve = value; }
        }

        [Category("Valve Setting")]
        public AutoValve PmChamberBGValve
        {
            get { return m_ChamberBGValve; }
            set { m_ChamberBGValve = value; }
        }
        //[Category("Valve Setting")]
        //public AutoValve Chamber2LockChamberValve
        //{
        //    get { return m_Chamber2LockChamberValve; }
        //    set { m_Chamber2LockChamberValve = value; }
        //}

        [Category("Valve Setting")]
        public AutoValve2 AngleValve
        {
            get { return m_AngleValve; }
            set { m_AngleValve = value; }
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
        public Mfc MfcSiH4
        {
            get { return m_MfcSiH4; }
            set { m_MfcSiH4 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcNF3
        {
            get { return m_MfcNF3; }
            set { m_MfcNF3 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcCH4
        {
            get { return m_MfcCH4; }
            set { m_MfcCH4 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcB2H6
        {
            get { return m_MfcB2H6; }
            set { m_MfcB2H6 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcPH3
        {
            get { return m_MfcPH3; }
            set { m_MfcPH3 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcNH3
        {
            get { return m_MfcNH3; }
            set { m_MfcNH3 = value; }
        }
        [Category("Mfc Setting")]
        public Mfc MfcH2
        {
            get { return m_MfcH2; }
            set { m_MfcH2 = value; }
        }

        [Category("ServoUnit Setting")]
        public ServoUnit TrayTransferServoUnit
        {
            get { return m_TrayTransferServoUnit; }
            set { m_TrayTransferServoUnit = value; }
        }
        [Category("Dry Pump Setting")]
        public DryPumpUnit PumpUnit
        {
            get { return m_PumpUnit; }
            set { m_PumpUnit = value; }
        }
        [Category("Apc Setting")]
        public Apc Apc
        {
            get { return m_Apc; }
            set { m_Apc = value; }
        }
        [Category("Shutter Setting")]
        public ShutterUnit Shutter
        {
            get { return m_Shutter; }
            set { m_Shutter = value; }
        }
        [Category("Gauge Setting")]
        public Gauge PmChamberCG
        {
            get { return m_ChamberCG; }
            set { m_ChamberCG = value; }
        }
        [Category("Servo Unit Setting")]
        public LockChamber LockChamber
        {
            get { return m_LockChamber; }
            set { m_LockChamber = value; }
        }
        [Category("HotWireUnit Setting")]
        public HotWireUnit HotWireUnit
        {
            get { return m_HotWireUnit; }
            set { m_HotWireUnit = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagTrayTransferIfFlag IfFlag
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
        [Browsable(false), XmlIgnore()]
        public StepRunAct ProcessStep
        {
            get { return m_StepRunAct; }
            set { m_StepRunAct = value; }
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
        public PMChamber_HWCVD()
        {
            this.Name = "__ HotWireCVD PmChamber";
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
        //public void ApplyStepRecipe(int stepNo)
        //{
        //    TagRecipe curRecipe = m_Server.JobCond.CurrentRecipe;

        //    switch (stepNo+1)
        //    {
        //        case 1:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step1.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step1.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step1.StepCL2FlowRate;
        //                m_StepRecipe.StepGasStableTime = curRecipe.Step1.StepGasStableTime;
        //                m_StepRecipe.StepO2FlowRate = curRecipe.Step1.StepO2FlowRate;
        //                m_StepRecipe.StepProcessPress = curRecipe.Step1.StepProcessPress;
        //                m_StepRecipe.StepRfPower = curRecipe.Step1.StepRfPower;
        //                m_StepRecipe.StepSF6FlowRate = curRecipe.Step1.StepSF6FlowRate;
        //            }
        //            break;

        //        case 2:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step2.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step2.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step2.StepCL2FlowRate;
        //                m_StepRecipe.StepGasStableTime = curRecipe.Step2.StepGasStableTime;
        //                m_StepRecipe.StepO2FlowRate = curRecipe.Step2.StepO2FlowRate;
        //                m_StepRecipe.StepProcessPress = curRecipe.Step2.StepProcessPress;
        //                m_StepRecipe.StepRfPower = curRecipe.Step2.StepRfPower;
        //                m_StepRecipe.StepSF6FlowRate = curRecipe.Step2.StepSF6FlowRate;
        //            }
        //            break;

        //        case 3:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step3.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step3.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step3.StepCL2FlowRate;
        //                m_StepRecipe.StepGasStableTime = curRecipe.Step3.StepGasStableTime;
        //                m_StepRecipe.StepO2FlowRate = curRecipe.Step3.StepO2FlowRate;
        //                m_StepRecipe.StepProcessPress = curRecipe.Step3.StepProcessPress;
        //                m_StepRecipe.StepRfPower = curRecipe.Step3.StepRfPower;
        //                m_StepRecipe.StepSF6FlowRate = curRecipe.Step3.StepSF6FlowRate;
        //            }
        //            break;

        //        case 4:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step4.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step4.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step4.StepCL2FlowRate;
        //                m_StepRecipe.StepGasStableTime = curRecipe.Step4.StepGasStableTime;
        //                m_StepRecipe.StepO2FlowRate = curRecipe.Step4.StepO2FlowRate;
        //                m_StepRecipe.StepProcessPress = curRecipe.Step4.StepProcessPress;
        //                m_StepRecipe.StepRfPower = curRecipe.Step4.StepRfPower;
        //                m_StepRecipe.StepSF6FlowRate = curRecipe.Step4.StepSF6FlowRate;
        //            }
        //            break;

        //        case 5:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step5.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step5.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step5.StepCL2FlowRate;
        //                m_StepRecipe.StepGasStableTime = curRecipe.Step5.StepGasStableTime;
        //                m_StepRecipe.StepO2FlowRate = curRecipe.Step5.StepO2FlowRate;
        //                m_StepRecipe.StepProcessPress = curRecipe.Step5.StepProcessPress;
        //                m_StepRecipe.StepRfPower = curRecipe.Step5.StepRfPower;
        //                m_StepRecipe.StepSF6FlowRate = curRecipe.Step5.StepSF6FlowRate;
        //            }
        //            break;
        //    }
        //}

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
                m_IfFlag = new TagTrayTransferIfFlag();
                m_IfFlag.Reset();

                if (LockChamber != null)
                {
                    m_LockChamber.SetPmChamver(this);
                }

                if (m_ChamberCG != null) m_ChamberCG.CurAdc = 750;
                if (m_Apc != null) m_Apc.Pressure = 750;


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
            m_Tag.SetValue(tagDescriptor.CHAMBERMESSAGE, m_ChamberMessage);
            //m_Tag.SetValue(tagDescriptor.PROCESSPRESSURE, m_StepRecipe.StepProcessPress);
        }
        #endregion
    }
}

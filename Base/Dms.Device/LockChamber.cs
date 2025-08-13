using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class LockChamber : _DeviceAsm
    {
        #region Tag Descriptor
        protected static TagDescriptorLockChamber tagDescriptor = new TagDescriptorLockChamber();
        #endregion

        #region Fields
        private Sensor m_LockChamberATMSensor;
        private Gauge m_LockChamberCG;

        //private Sensor m_ShutterOpenSensor;
        private Sensor m_ShutterCloseSensor;

        //private Sensor m_TrayTransferInPosSensor;
        //private Sensor m_TrayTransferOutPosSensor;

        private ServoUnit m_TrayTransferServoUnit;

        private AutoValve m_GN2InValve;
        private AutoValve2 m_AngleValve;
        //private RotaryPump m_RotaryPump;

        private PMChamber_HWCVD m_PmChamber;

        private bool m_TrayExist = false;

        private string m_Process = "0";
        private string m_RunState = "None";
        //private TagStepRecipe m_StepRecipe;
        private TagTrayTransferIfFlag m_IfFlag = new TagTrayTransferIfFlag();
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public PMChamber_HWCVD PMChamber
        {
            get { return m_PmChamber; }
            set { m_PmChamber = value; }
        }
        [Category("ServoUnit Setting")]
        public ServoUnit TrayTransferServoUnit
        {
            get { return m_TrayTransferServoUnit; }
            set { m_TrayTransferServoUnit = value; }
        }
        [Category("CG Setting")]
        public Gauge LockChamberCG
        {
            get { return m_LockChamberCG; }
            set { m_LockChamberCG = value; }
        }
        [Category("Setting")]
        public Sensor LockChamberATMSensor
        {
            get { return m_LockChamberATMSensor; }
            set { m_LockChamberATMSensor = value; }
        }
        //[Category("Setting")]
        //public Sensor ShutterOpenSensor
        //{
        //    get { return m_ShutterOpenSensor; }
        //    set { m_ShutterOpenSensor = value; }
        //}
        //[Category("Setting")]
        //public Sensor TrayTransferInPosSensor
        //{
        //    get { return m_TrayTransferInPosSensor; }
        //    set { m_TrayTransferInPosSensor = value; }
        //}
        //[Category("Setting")]
        //public Sensor TrayTransferOutPosSensor
        //{
        //    get { return m_TrayTransferOutPosSensor; }
        //    set { m_TrayTransferOutPosSensor = value; }
        //}
        [Category("Setting")]
        public Sensor ShutterCloseSensor
        {
            get { return m_ShutterCloseSensor; }
            set { m_ShutterCloseSensor = value; }
        }
        [Category("Valve Setting")]
        public AutoValve GN2InValve
        {
            get { return m_GN2InValve; }
            set { m_GN2InValve = value; }
        }
        [Category("Valve Setting")]
        public AutoValve2 AngleValve
        {
            get { return m_AngleValve; }
            set { m_AngleValve = value; }
        }
        [Browsable(false), XmlIgnore()]
        public bool TrayExist
        {
            get { return m_TrayExist; }
            set { m_TrayExist = value; }
        }

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
        //[Browsable(false), XmlIgnore()]
        //public TagStepRecipe StepRecipe
        //{
        //    get { return m_StepRecipe; }
        //    set { m_StepRecipe = value; }
        //}
        [Browsable(false), XmlIgnore()]
        public TagTrayTransferIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        #endregion

        #region Constructor
        public LockChamber()
        {

        }
        #endregion

        #region Method
        public void SetPmChamver(PMChamber_HWCVD pmChamber)
        {
            m_PmChamber = pmChamber;
        }

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

        //public void ApplyStepRecipe(int stepNo)
        //{
        //    TagRecipe curRecipe = m_Server.JobCond.CurrentRecipe;

        //    switch (stepNo + 1)
        //    {
        //        case 1:
        //            {
        //                m_StepRecipe.StepApply = curRecipe.Step1.StepApply;
        //                m_StepRecipe.StepAshingTime = curRecipe.Step1.StepAshingTime;
        //                m_StepRecipe.StepCL2FlowRate = curRecipe.Step1.StepCL2FlowRate;
        //                m_StepRecipe.StepGap = curRecipe.Step1.StepGap;
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
        //                m_StepRecipe.StepGap = curRecipe.Step2.StepGap;
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
        //                m_StepRecipe.StepGap = curRecipe.Step3.StepGap;
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
        //                m_StepRecipe.StepGap = curRecipe.Step4.StepGap;
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
        //                m_StepRecipe.StepGap = curRecipe.Step5.StepGap;
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

            bool shutterClose = m_ShutterCloseSensor.IsDetected();/*DiSensor.GetState();*/
            bool shutterOpen = !shutterClose;
            string shutterState;


            if (shutterOpen && !shutterClose) shutterState = "OPEN";
            else if (!shutterOpen && shutterClose) shutterState = "CLOSE";
            else shutterState = "UNKNOWN";

            //if (stopperFw && !stopperBw) stopperState = "STOPPER FW";
            //else if (!stopperFw && stopperBw) stopperState = "STOPPER BW";
            //else stopperState = "UNKNOWN";

            //if (trayTransferRecv && !trayTransferSend) trayTransferPos = "RECV POS";
            //else if (!trayTransferRecv && trayTransferSend) trayTransferPos = "SEND POS";
            //else trayTransferPos = "UNKNOWN";            

            m_Tag.SetValue(tagDescriptor.TRAYEXIST, m_TrayExist);

            if (m_Simul.Device) m_Tag.SetValue(tagDescriptor.PRESSURE, m_LockChamberCG.SimulCurValue);
            else m_Tag.SetValue(tagDescriptor.PRESSURE, m_LockChamberCG.CurValue);
            m_Tag.SetValue(tagDescriptor.RUNSTATE, m_RunState);
            m_Tag.SetValue(tagDescriptor.PROCESS, m_Process);
            m_Tag.SetValue(tagDescriptor.SHUTTERSTATE, shutterState);
            //m_Tag.SetValue(tagDescriptor.STOPPERSTATE, stopperState);
            //m_Tag.SetValue(tagDescriptor.TRAYTRANSFERPOS, trayTransferPos);
        }
        #endregion
    }
}

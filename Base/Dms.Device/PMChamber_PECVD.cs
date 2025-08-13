///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.07.21
// Author       : EUN
// Description  : PECVD Process Chamber(reference project-10EV01)
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class PMChamber_PECVD : TransferUnit
    {
        #region Tag Descriptor
        protected static TagDescriptorPmChamber tagDescriptor = new TagDescriptorPmChamber();
        #endregion

        #region Fields
        private Mfc m_MfcPN2;
        private Mfc m_MfcNH3;
        private Mfc m_MfcSIH4;
        private Mfc m_MfcH2;
        private Mfc m_MfcAr;
        private Mfc m_MfcNF3;

        private AutoValve m_PN2InValve;
        private AutoValve m_PN2OutValve;
        private AutoValve m_NH3InValve;
        private AutoValve m_NH3PurgeValve;
        private AutoValve m_NH3OutValve;
        private AutoValve m_SIH4InValve;
        private AutoValve m_SIH4PurgeValve;
        private AutoValve m_SIH4OutValve;
        private AutoValve m_H2InValve;
        private AutoValve m_H2PurgeValve;
        private AutoValve m_H2OutValve;
        private AutoValve m_ArInValve;
        private AutoValve m_ArOutValve;
        private AutoValve m_NF3InValve;
        private AutoValve m_NF3PurgeValve;
        private AutoValve m_NF3OutValve;
        private AutoValve m_ProcessGasInValve;
        private AutoValve m_CleaningGasInValve;
        private AutoValve m_BaratronValve;
        private AutoValve m_ReliefValve;
        private AutoValve m_AngleValve;
        private AutoValve m_GateValve;

        private _ServoUnit m_SusceptorServoUnit;
        private DryPumpUnit m_DryPumpUnit;
        private ShutterUnit m_ShutterUnit;
        private HotWireUnit m_HotWireUnit;
        private RfUnit m_RfUnit;

        private RPS m_Rps;
        private Apc m_Apc;
        private Gauge m_ChamberPiraniGauge;
        private Gauge m_DryPumpPiraniGauge;
        private Sensor m_ChamberATM;
        private Sensor m_DryPumpATM;

        private TagTrayTransferIfFlag m_IfFlag = new TagTrayTransferIfFlag();
        private StepRunAct m_ProcessStep = StepRunAct.Noop;
        private string m_RunState = "None";
        private string m_Process = "0";
        private string m_ChamberMessage = "";
        #endregion

        #region Properties
        [Category("MFC")]
        public Mfc MfcPN2
        {
            get { return m_MfcPN2; }
            set { m_MfcPN2 = value; }
        }
        [Category("MFC")]
        public Mfc MfcNH3
        {
            get { return m_MfcNH3; }
            set { m_MfcNH3 = value; }
        }
        [Category("MFC")]
        public Mfc MfcSIH4
        {
            get { return m_MfcSIH4; }
            set { m_MfcSIH4 = value; }
        }
        [Category("MFC")]
        public Mfc MfcH2
        {
            get { return m_MfcH2; }
            set { m_MfcH2 = value; }
        }
        [Category("MFC")]
        public Mfc MfcAr
        {
            get { return m_MfcAr; }
            set { m_MfcAr = value; }
        }
        [Category("MFC")]
        public Mfc MfcNF3
        {
            get { return m_MfcNF3; }
            set { m_MfcNF3 = value; }
        }
        [Category("Valve")]
        public AutoValve PN2InValve
        {
            get { return m_PN2InValve; }
            set { m_PN2InValve = value; }
        }
        [Category("Valve")]
        public AutoValve PN2OutValve
        {
            get { return m_PN2OutValve; }
            set { m_PN2OutValve = value; }
        }
        [Category("Valve")]
        public AutoValve NH3InValve
        {
            get { return m_NH3InValve; }
            set { m_NH3InValve = value; }
        }
        [Category("Valve")]
        public AutoValve NH3PurgeValve
        {
            get { return m_NH3PurgeValve; }
            set { m_NH3PurgeValve = value; }
        }
        [Category("Valve")]
        public AutoValve NH3OutValve
        {
            get { return m_NH3OutValve; }
            set { m_NH3OutValve = value; }
        }
        [Category("Valve")]
        public AutoValve SIH4InValve
        {
            get { return m_SIH4InValve; }
            set { m_SIH4InValve = value; }
        }
        [Category("Valve")]
        public AutoValve SIH4PurgeValve
        {
            get { return m_SIH4PurgeValve; }
            set { m_SIH4PurgeValve = value; }
        }
        [Category("Valve")]
        public AutoValve SIH4OutValve
        {
            get { return m_SIH4OutValve; }
            set { m_SIH4OutValve = value; }
        }
        [Category("Valve")]
        public AutoValve H2InValve
        {
            get { return m_H2InValve; }
            set { m_H2InValve = value; }
        }
        [Category("Valve")]
        public AutoValve H2PurgeValve
        {
            get { return m_H2PurgeValve; }
            set { m_H2PurgeValve = value; }
        }
        [Category("Valve")]
        public AutoValve H2OutValve
        {
            get { return m_H2OutValve; }
            set { m_H2OutValve = value; }
        }
        [Category("Valve")]
        public AutoValve ArInValve
        {
            get { return m_ArInValve; }
            set { m_ArInValve = value; }
        }
        [Category("Valve")]
        public AutoValve ArOutValve
        {
            get { return m_ArOutValve; }
            set { m_ArOutValve = value; }
        }
        [Category("Valve")]
        public AutoValve NF3InValve
        {
            get { return m_NF3InValve; }
            set { m_NF3InValve = value; }
        }
        [Category("Valve")]
        public AutoValve NF3PurgeValve
        {
            get { return m_NF3PurgeValve; }
            set { m_NF3PurgeValve = value; }
        }
        [Category("Valve")]
        public AutoValve NF3OutValve
        {
            get { return m_NF3OutValve; }
            set { m_NF3OutValve = value; }
        }
        [Category("Valve")]
        public AutoValve ProcessGasInValve
        {
            get { return m_ProcessGasInValve; }
            set { m_ProcessGasInValve = value; }
        }
        [Category("Valve")]
        public AutoValve CleaningGasInValve
        {
            get { return m_CleaningGasInValve; }
            set { m_CleaningGasInValve = value; }
        }
        [Category("Valve")]
        public AutoValve BaratronValve
        {
            get { return m_BaratronValve; }
            set { m_BaratronValve = value; }
        }
        [Category("Valve")]
        public AutoValve ReliefValve
        {
            get { return m_ReliefValve; }
            set { m_ReliefValve = value; }
        }
        [Category("Valve")]
        public AutoValve AngleValve
        {
            get { return m_AngleValve; }
            set { m_AngleValve = value; }
        }
        [Category("Valve")]
        public AutoValve GateValve
        {
            get { return m_GateValve; }
            set { m_GateValve = value; }
        }
        [Category("Units")]
        public _ServoUnit SusceptorServoUnit
        {
            get { return m_SusceptorServoUnit; }
            set { m_SusceptorServoUnit = value; }
        }
        [Category("Units")]
        public DryPumpUnit DryPumpUnit
        {
            get { return m_DryPumpUnit; }
            set { m_DryPumpUnit = value; }
        }
        [Category("Units")]
        public ShutterUnit ShutterUnit
        {
            get { return m_ShutterUnit; }
            set { m_ShutterUnit = value; }
        }
        [Category("Units")]
        public HotWireUnit HotWireUnit
        {
            get { return m_HotWireUnit; }
            set { m_HotWireUnit = value; }
        }
        [Category("Units")]
        public RfUnit RfUnit
        {
            get { return m_RfUnit; }
            set { m_RfUnit = value; }
        }
        [Category("Modules")]
        public RPS RPS
        {
            get { return m_Rps; }
            set { m_Rps = value; }
        }
        [Category("Modules")]
        public Apc Apc
        {
            get { return m_Apc; }
            set { m_Apc = value; }
        }
        [Category("Modules")]
        public Gauge ChamberPiraniGauge
        {
            get { return m_ChamberPiraniGauge; }
            set { m_ChamberPiraniGauge = value; }
        }
        [Category("Modules")]
        public Gauge DryPumpPiraniGauge
        {
            get { return m_DryPumpPiraniGauge; }
            set { m_DryPumpPiraniGauge = value; }
        }
        [Category("Modules")]
        public Sensor ChamberATMSensor
        {
            get { return m_ChamberATM; }
            set { m_ChamberATM = value; }
        }
        [Category("Modules")]
        public Sensor DryPumpATMSensor
        {
            get { return m_DryPumpATM; }
            set { m_DryPumpATM = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagTrayTransferIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        [Browsable(false), XmlIgnore()]
        public StepRunAct ProcessStep
        {
            get { return m_ProcessStep; }
            set { m_ProcessStep = value; }
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
        [Browsable(false), XmlIgnore()]
        public string ChamberMessage
        {
            get { return m_ChamberMessage; }
            set { m_ChamberMessage = value; }
        }
        #endregion

        #region Constructor
        public PMChamber_PECVD()
        {
            this.Name = "__ PECVD PM Chamber";
        }
        #endregion

        #region Override
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
        public override DmsErrors Initialize(IServerManager server, _GenInfoHandler geninfos)
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

            //for UpdateTag()
            ok &= (m_Apc != null);
            ok &= (m_RfUnit != null);


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
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagTrayTransferIfFlag(this);
                m_IfFlag.Reset();



                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion
                if (m_Simul.Device)
                {
                    if (m_ChamberPiraniGauge != null) m_ChamberPiraniGauge.CurAdc = 750;
                    if (m_Apc != null) m_Apc.Pressure = 750;
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
            string temp = "";
            temp = string.Format("{0:f1}", m_Apc.GetPressure());
            m_Tag.SetValue(tagDescriptor.PRESSURE, temp);
            m_Tag.SetValue(tagDescriptor.ISBASEPRESSURE, m_Apc.IsBasePressureSensed(m_Server.JobCond.BasePressure()));
            m_Tag.SetValue(tagDescriptor.ISPROCESSPRESSURE, m_Apc.IsProcessPressureSensed());
            m_Tag.SetValue(tagDescriptor.PROCESSPRESSURE, m_Server.JobCond.ProcessPressure());
            m_Tag.SetValue(tagDescriptor.PROCESS, m_Process);
            //m_Tag.SetValue(tagDescriptor.RUNSTATE, m_RunState); //Replaced by following
            m_Tag.SetValue(tagDescriptor.RUNSTATE, m_ProcessStep.ToString());
            m_Tag.SetValue(tagDescriptor.RFONSTATE, m_RfUnit.IsRfPowerOn());
            m_Tag.SetValue(tagDescriptor.PROCESSSTEP, Convert.ToInt32(m_ProcessStep));
            //m_Tag.SetValue(tagDescriptor.TRAYEXIST, m_Server.GlassData.IsExist(this.DataMatchingKey(0))); //No use
            //m_Tag.SetValue(tagDescriptor.CHAMBERMESSAGE, m_ChamberMessage);  //No use
        }
        #endregion
    }
}

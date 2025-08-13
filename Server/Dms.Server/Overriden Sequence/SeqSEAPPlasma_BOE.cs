using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Data;
using Dms.Device;
using Dms.Server; 
using Dms.Sequence; 

namespace Dms.Server
{
    public enum ApInterlockStatus 
    {
        ApPCWflowInterlock = 0x01,
        ApN2flowInterlock = 0x02,
    }
    //public enum boeSeApAlarmIndex // 10.12.21 minhan 셋업지에서 검토해야 합니다. 일단 삭제. 사양서의 on이라는 의미는 전장적인 내용이라서.
    //{
    //    plasmaAlmInterlockErr = 7,
    //    plasmaAlmInverterErr = 6,
    //    plasmaAlmLowVoltageErr = 5,
    //    plasmaAlmHighVoltageErr = 4,
    //    plasmaAlmArcFault = 3,
    //    plasmaAlmLocalModeRunErr = 2,
    //    plasmaAlmOnFaultErr = 1,
    //    plasmaNoAlm = 0,
    //}
    public class ThreadSeApBOE_G8_DHDC : ThreadSeApControl
    { 
        #region Fields
        public static ThreadSeApBOE_G8_DHDC Instance; 
        private XLog ApLog;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_Aps.Count == 0) return;

            m_Server.AddSeqInitFunction(new SeqInitSeAp(this, m_Server));

            foreach (SeAp device in m_Aps)
            {
                RegisterSequence(new SeqSeAp(this, device));
                RegisterSequence(new SeqUtControl(this, device));
                RegisterSequence(new SeqSeApAlarm(this, device));
                RegisterSequence(new SeqSeApMfcGaugeInterlock(this, device, device.MfcN2.Gauge));
                RegisterSequence(new SeqSeApMfcGaugeInterlock(this, device, device.MfcCDA.Gauge)); 
                RegisterSequence(new SeqSeApMfcGaugeInterlock(this, device, device.MfcVoltage.Gauge));
                RegisterSequence(new SeqSeApHouseSensor(this, device));
                RegisterSequence(new SeqSeApManualInterlock(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadSeApBOE_G8_DHDC(int scanTime, IServerManager server)
            : base(scanTime, server) 
        {
            Instance = this; 
            ApLog = new XLog("SE PlasmaLog", XLog.LogStampType.UseStamp); 
        }
        #endregion

        #region  Override Methods
        public override bool GetHouseCloseState(SeAp ap) 
        {
            bool close = true;

            if (m_Server.Simul.Device)
            {
                close = true;
                eqpSensors._EUV_Unit_House_Close_Sensor.DiSensor.SetState(true);
            }
            else
            {
                close &= ap.ActuatorUnit.IsNegative();
                close &= ap.DiHouseClose.GetState();
            }
            return close;
        }

        public override bool GetApCond(SeAp ap)
        {
            bool cond = true;
            if (ap.IsUse())
            {
                cond &= !ap.IsAlarm;
                cond &= GetHouseCloseState(ap);
                cond &= ap.PcwInValve.IsOpen();
                cond &= !GetGaugeNgCond(ap); // 11.04.07 minhan
                //cond &= !eqpGauges._AP_Up_Exhaust_Gauge.IsAlarm; // 일단 배기는 제외 하자..!!
                //cond &= !eqpGauges._AP_Dn_Exhaust_Gauge.IsAlarm; //
            }
            else
            {
                cond &= GetHouseCloseState(ap);
            }
            return cond;
        }

        public override bool IsGlassExist(SeAp ap) 
        {
            bool bExist = false;

            bExist |= (ap.OwnerUnit.PrevCv.GlsInSensor.IsDetected(Logic.OR)); 
            bExist |= ap.OwnerUnit.GlsInSensor.IsDetected(Logic.OR);
            bExist |= ap.OwnerUnit.GlsOutSensor.IsDetected(Logic.OR);
            bExist |= m_Server.GlassData.IsExist(ap.OwnerUnit.DataMatchingKey(0));
            bExist |= m_Server.GlassData.IsExist(ap.OwnerUnit.DataMatchingKey(1));
            bExist |= (m_Server.GlassData.IsExist(ap.OwnerUnit.PrevCv.DataMatchingKey(0)));
            return bExist;
        }

        public override bool SeApOffCondition(SeAp ap) 
        {
            bool apOff = false;
            CvUnit cv;
            //for (int i = 0; i < ap.OwnerUnit.Id; i++) 
            //{
            //    cv = m_CvUnits[i];
            //    apOff |= (cv.IfFlag.InError | cv.IfFlag.OutError);
            //}
            for (int i = eqpTransferUnits._EUV_CvUnit.Id ; i <= eqpTransferUnits._RB_CvUnit.Id; i++)
            {
                cv = m_CvUnits[i-2]; // 11.05.17 minhan
                //if (i == eqpTransferUnits._LD_CvUnit.Id) 
                //{
                //    if (!cv.IfFlag.RecoveryReq) apOff |= cv.IfFlag.OutError;
                //}
                //else
                //{
                    if (!cv.IfFlag.RecoveryReq) // 11.05.17 minhan
                    {
                        apOff |= (cv.IfFlag.InError | cv.IfFlag.OutError);
                    }
                //}
            }
            return apOff;
        }

        public override bool GetGaugeNgCond(SeAp ap) // 10.12.21 minhan
        {
            bool ng = false;
            int count = m_Gauges.Count;
            Gauge gauge;

            for (int i = 0; i < count; i++)
            {
                gauge = m_Gauges[i];
                if ((gauge.InterlockEnable) && (gauge.OwnerUnit != null) && (gauge.OwnerUnit.Name == ap.OwnerUnit.Name))
                {
                    if ((gauge.Id == eqpGauges._AP_Unit_CDA_Pressure_Gauge.Id) ||
                        (gauge.Id == eqpGauges._AP_Unit_N2_Pressure_Gauge.Id) ||
                        (gauge.Id == eqpGauges._AP_Unit_PCW_Pressure_Gauge.Id) ||
                        (gauge.Id == eqpGauges._AP_Unit_PCW_Out_Gauge.Id) ||
                        (gauge.Id == eqpGauges._AP_Unit_Power_Gauge.Id)) // 사양에서 이 알람을 사용하기 때문에 설정
                    {
                        ng |= gauge.IsAlarm;
                    }
                }
            }

            return ng;
        }

        public override bool GetRefOperation(SeAp ap)
        {
            bool run = IsRunCondition(ap);
            bool interlock = IsInterlockCondition(ap);

            return run && !interlock;
        }

        public override bool IsInterlockCondition(SeAp ap)
        {
            bool interlock = false;
            interlock |= !GetHouseCloseState(ap);
            interlock |= (ap.GetStatusAlarm() != (int)SeApAlarmIndex.plasmaNoAlm);
            interlock |= m_Server.GenInfos.Pause;
            interlock |= (m_Server.JobCond.HeavyInterlock > 0) ? true : false;
            interlock |= SeApOffCondition(ap);
            interlock |= ap.IsAlarm; 
            interlock |= !ap.PcwInValve.IsOpen();
            interlock |= GetGaugeNgCond(ap); // 10.12.21 minhan
            return interlock;
        }

        public override bool IsRunCondition(SeAp ap) 
        {
            bool run = true;
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_Server.JobCond.ProcessMode;
            run &= ap.IsUse();
            run &= ((ap.OwnerUnit.PrevCv.MotorControl.IsFw(Logic.OR)) | ap.OwnerUnit.MotorControl.IsFw(Logic.OR)) ? true : false;
            run &= IsGlassExist(ap);
            return run;
        }
        #endregion

        #region Static Methods

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

            log = string.Format("SE Plasma \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            ApLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        
        #endregion

    }

    public class SeqInitSeAp : XSeqInitFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private InitState m_InitState = InitState.Noop;
        private _GenericCollection<SeAp> m_Units;
        private new int[] m_AlarmId;
        private ThreadSeApBOE_G8_DHDC m_Control;

        protected GenericTag m_InitCheckApInterlock = new GenericTag("Ap Interlock Check", InitCheckState.NotReady);
        protected GenericTag m_InitCheckApStatusAlarm = new GenericTag("AP Status Alarm", InitCheckState.NotReady);
        protected GenericTag m_InitCheckApClose = new GenericTag("AP House Close", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitSeAp(ThreadSeApBOE_G8_DHDC control, IServerManager server)
        {
            m_Server = server;
            m_Eqp = m_Server.EqpStateManager;
            m_Units = m_Server.ComponentContainer.GetCollection<SeAp>();
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = m_Server.GenInfos;// GenInfoHandler.Instance;

            this.SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            int nSeqNo = this.SeqNo;
            int alarmCode = 0;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)//m_Server.GenInfos.EqpInitReq/m_GenInfos.EqpInitReq
                    {
                        m_InitState = InitState.Init;
                        m_Control.SetLog(SeqFunName, 0, 0, "SeqApInit : Start ");
                        nSeqNo = 10;

                        if (m_Server.Simul.Device)
                        {
                            foreach (SeAp ap in m_Units)
                            {
                                ap.ActuatorUnit.SetNegativeAct();
                                ap.DiHouseClose.SetState(true);
                                //ap.DiAlarmStatus0.SetState(false); // 10.12.21 minhan 
                                //ap.DiAlarmStatus1.SetState(false);
                                //ap.DiAlarmStatus2.SetState(false);
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        int count = m_Units.Count;
                        bool use = false;

                        for (int i = 0; i < count; i++)
                        {
                            if (m_Server.JobCond.ApUse(m_Units[i]))
                            {
                                use = true;
                            }
                        }
                        if (use == false)
                        {
                            m_InitCheckApInterlock.Value = InitCheckState.NoUse; 
                            m_InitCheckApStatusAlarm.Value = InitCheckState.NoUse;

                            //eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.Open(); 
                            //m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : PCW SOL VALVE OPEN"); 

                            nSeqNo = 40;
                        }
                        else
                        {
                            bool alarm = false;

                            foreach (SeAp ap in m_Units)
                            {
                                alarm |= ap.IsAlarm;
                            }

                            if (!alarm) // 10.12.21 minhan
                            {
                                double setN2Flow = m_Server.JobCond.ApN2Flow(eqpSeAps._SE_AP_Plasma_Unit);
                                double setCDAFlow = m_Server.JobCond.ApCDAFlow(eqpSeAps._SE_AP_Plasma_Unit);
                                double setVoltage = m_Server.JobCond.ApVoltage(eqpSeAps._SE_AP_Plasma_Unit);
                                string m_Msg = "";

                                //eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.Open();
                                //m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : PCW SOL VALVE OPEN");

                                eqpSeAps._SE_AP_Plasma_Unit.SetN2Flow(setN2Flow);
                                m_Msg = string.Format("N2 Flow set : {0}", setN2Flow.ToString());
                                m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                                eqpSeAps._SE_AP_Plasma_Unit.SetCDAFlow(setCDAFlow);
                                m_Msg = string.Format("CDA Flow set : {0}", setCDAFlow);
                                m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                                eqpSeAps._SE_AP_Plasma_Unit.SetVoltage(setVoltage);
                                m_Msg = string.Format("Plasma Power : {0}", setVoltage);
                                m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                                m_InitCheckApInterlock.Value = InitCheckState.Checking;
                                m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : Interlock Check");

                                StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                            }
                            else
                            {
                                m_InitState = InitState.Fail;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        bool alarm = false;

                        foreach (SeAp ap in m_Units) // 10.12.21 minhan
                        {
                            alarm |= ap.IsAlarm;
                        }
                        
                        if (GetElapsedTicks() > 5000)
                        {
                            bool ready = true;

                            if (!m_Server.Simul.Device)
                            {
                                foreach (SeAp ap in m_Units)
                                {
                                    if (ap.DiN2FlowLowLimitAlarm.GetState() || ap.DiPCWFlowLowLimitAlarm.GetState()) // 10.12.21 minhan
                                    {
                                        ready = false;
                                    }
                                }
                            }

                            if (ready)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : Check Ap Interlock Ok");
                                m_InitCheckApInterlock.Value = InitCheckState.OK;
                                m_InitCheckApStatusAlarm.Value = InitCheckState.Checking;
                                StartTicks = XFunc.GetTickCount();
                                nSeqNo = 30;
                            }
                            else
                            {
                                m_InitState = InitState.Fail;
                                m_InitCheckApInterlock.Value = InitCheckState.NG;
                                nSeqNo = 1000;
                            }
                        }
                        else if (alarm)
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckApInterlock.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 30:
                    {
                        bool alarm = false;

                        foreach (SeAp ap in m_Units) // 10.12.21 minhan
                        {
                            alarm |= ap.IsAlarm;
                        }

                        if (GetElapsedTicks() > 3000)
                        {
                            bool ready = true;
                            int count = m_Units.Count;

                            for (int i = 0; i < count; i++)
                            {
                                if (((alarmCode = m_Units[i].GetStatusAlarm()) != (int)SeApAlarmIndex.plasmaNoAlm) && !m_Server.Simul.Device)
                                {
                                    ready = false;

                                    m_AlarmId[i] = alarmCode + m_Units[i].ALM_InterlockErr.Id;
                                    m_Eqp.SetAlarm(m_AlarmId[i]);
                                    break;
                                }
                            }

                            if (ready)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : Check Ap Status Alarm OK");
                                m_InitCheckApStatusAlarm.Value = InitCheckState.OK;
                                m_InitCheckApClose.Value = InitCheckState.Checking;
                                nSeqNo = 40;
                            }
                            else
                            {
                                // m_Control.SetLog("SeqInitAp : Ap Status Alarm");
                                m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : Ap Status Alarm : " + alarmCode.ToString());
                                m_InitState = InitState.Fail;
                                m_InitCheckApStatusAlarm.Value = InitCheckState.NG;
                                nSeqNo = 1000;
                            }
                        }
                        else if (alarm)
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckApStatusAlarm.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 40:
                    {
                        bool ready = true;
                        int count = m_Units.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (m_Control.GetHouseCloseState(m_Units[i]) == false)
                            {
                                ready = false;
                                m_AlarmId[i] = m_Units[i].ALM_HouseOpen.Id;
                                m_Eqp.SetAlarm(m_AlarmId[i]);
                            }
                        }

                        if (ready)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : House Close Confirm");
                            m_InitCheckApClose.Value = InitCheckState.OK;
                            nSeqNo = 100;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckApClose.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 100:
                    {
                        //int count = m_Units.Count;
                        //for (int i = 0; i < count; i++) m_Units[i].IfFlag.Ready = true;   //No Use

                        m_Control.SetLog(SeqFunName, 0, 0, "SeqInitAp : Init Complete");
                        m_InitState = InitState.Comp;
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (m_AlarmId[i] > 0)
                            {
                                m_Eqp.ResetAlarm(m_AlarmId[i]);
                                m_AlarmId[i] = 0;
                            }
                        }
                        nSeqNo = 1010;
                    }
                    break;
                case 1010:
                    if (!m_GenInfos.EqpInitReq)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqSeAp : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private SeAp m_Ap;
        private string m_Msg;
        private ThreadSeApBOE_G8_DHDC m_Control;
        private Alarm ALM_PowerOffAlarm;
        private int onMargin; 
        protected SeqSeApProgress m_SeqProgress;
        private TagGlassData m_GlassData; 
        private int m_PortNo;
        private int m_SlotNo;
        #endregion

        #region Constructor
        public SeqSeAp(ThreadSeApBOE_G8_DHDC control, SeAp ap) 
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = m_Server.GenInfos;// GenInfoHandler.Instance;
            m_Msg = "";
            onMargin = 0;

            this.SeqFunName = m_Ap.Name + " SeqSeAp";
            ALM_PowerOffAlarm = new Alarm(m_Ap.Name + " Power Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_SeqProgress = new SeqSeApProgress(m_Control, m_Ap);
            m_GlassData = new TagGlassData(); 
            m_PortNo = 0;
            m_SlotNo = 0;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1; 

            int nSeqNo = this.SeqNo;

            bool apPowerOn = m_Control.GetRefOperation(m_Ap);
            onMargin = m_Ap.SetupPowerOnWaitTime.GetValue<int>() * 1000; 

            m_SeqProgress.Do(apPowerOn);

            if (m_Server.GlassData.GetData(m_Ap.OwnerUnit.DataMatchingKey(0), ref m_GlassData) ||
                m_Server.GlassData.GetData(m_Ap.OwnerUnit.DataMatchingKey(1), ref m_GlassData)) 
            {
                m_PortNo = (int)m_GlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_GlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            switch (nSeqNo)
            {
                case 0:
                    if (apPowerOn)
                    {
                       // if ((m_Ap.PcwInValve != null) && m_Ap.PcwInValve.IsClose()) m_Ap.PcwInValve.Open(); // 10.12.21 minhan pcw는 여기서 처리 할 필요가 없을 것 같다.

                        double setN2Flow = m_Server.JobCond.ApN2Flow(m_Ap);
                        double setCDAFlow = m_Server.JobCond.ApCDAFlow(m_Ap);
                        double setVoltage = m_Server.JobCond.ApVoltage(m_Ap);

                        m_Ap.SetN2Flow(setN2Flow);
                        m_Msg = string.Format("N2 Flow set : {0}", setN2Flow.ToString());
                        m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, m_Msg);

                        m_Ap.SetCDAFlow(setCDAFlow);
                        m_Msg = string.Format("CDA Flow set : {0}", setCDAFlow);
                        m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, m_Msg);

                        m_Ap.SetVoltage(setVoltage);
                        m_Msg = string.Format("Plasma Power : {0}", setVoltage);
                        m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, m_Msg);

                        //m_Ap.PowerOn();
                        //m_Control.SetLog(SeqFunName, 0, 0, "Plasma Power On");

                        StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10: 
                    {
                        if ((GetElapsedTicks() > onMargin) && apPowerOn) 
                        //if(GetElapsedTicks() > onMargin) 
                        {
                            m_Ap.PowerOn();
                            m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "Plasma Power On");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                            
                        }
                        else if (!apPowerOn) 
                        {
                            m_Ap.PowerOff();
                            m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "Ap Plasma Power Off Case 10");

                            nSeqNo = 0;
                        }
                    }
                    break;
                case 20: 
                    {
                        if (m_Ap.IsPowerOn())
                        {
                            m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "AP Plasma Power On State Confirm");
                            nSeqNo = 30;

                        }
                        else if (GetElapsedTicks() > 1000)
                        {
                            AlarmId = ALM_PowerOffAlarm.Id; 
                            m_Eqp.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "AP Plasma Power Off Alarm Set");
                            m_Ap.IsAlarm = true;
                            //ReturnSeqNo = nSeqNo; 
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 30:
                    {
                        if (!apPowerOn)
                        {
                            m_Ap.PowerOff();
                            m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "Ap Plasma Power Off");

                            //m_Ap.SetN2Flow(0.0);
                            //m_Control.SetLog(SeqFunName, 0, 0, "N2 Flow Reset");

                            //m_Ap.SetCDAFlow(0.0);
                            //m_Control.SetLog(SeqFunName, 0, 0, "CDA Flow Reset");

                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1000: 
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Ap.IsAlarm = false;
                        m_Control.SetLog(SeqFunName, m_PortNo, m_SlotNo, "AP Plasma Alarm Reset Case 0");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqUtControl : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private ThreadSeApBOE_G8_DHDC m_Control; // 11.03.20 minhan
        private SeAp m_Ap;
        //private bool AlarmSkip; // 10.12.21 minhan
        //private string m_Msg; 
        //private ThreadSeApBOE_G6_DHDC m_Control;
        #endregion


        #region Constructor
        public SeqUtControl(ThreadSeApBOE_G8_DHDC control, SeAp ap)
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager; // 10.12.21 minhan
            m_Control = control; // 11.03.20 minhan
            this.SeqFunName = m_Ap.Name + " SeqUtControl";
            //AlarmSkip = false; // 11.05.17 minhan
        }
        #endregion

        #region Sequence
        public override int Do() // 10.12.21 minhan 사양이 알람(4가지 조건) 발생시 리셋을 누르면 3초 동안 강제 on을 하고 다시 off..
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool UtRunCon = true;
            UtRunCon &= !(eqpLeakSensors._EUV_Unit_Leak_Sensor1.IsDetectedInterlock() ||
                          eqpLeakSensors._EUV_Unit_Leak_Sensor2.IsDetectedInterlock() ||
                          eqpLeakSensors._EUV_Unit_Leak_Sensor3.IsDetectedInterlock() ||
                          eqpLeakSensors._AK_Unit_Leak_Sensor1.IsAlarm ||
                          eqpLeakSensors._AK_Unit_Leak_Sensor2.IsAlarm ||
                          eqpLeakSensors._AK_Unit_Leak_Sensor3.IsAlarm) ? true : false; // 11.03.20 minhan

            //if (!m_Server.Simul.Device) // 11.03.24 minhan
            //{
            //    UtRunCon &= !m_Ap.DiPCWFlowLowLimitAlarm.GetState(); // 10.12.21 minhan 사양참조.
            //}

            UtRunCon &= !GlobalVar.PcwPressureUpAlarm; // 11.03.24 minhan
            UtRunCon &= !GlobalVar.PcwFlowUpAlarm; // 11.04.07 minhan se plasma 요청사항
            UtRunCon &= !((heavy & HeavyInterlock.Emo) > 0 ? true : false); // 11.03.17 minhan
            //UtRunCon &= !((heavy & ~HeavyInterlock.Emo) > 0 ? true : false);
            //UtRunCon &= !((heavy & ~HeavyInterlock.Leak) > 0 ? true : false);

            //if (m_Server.GenInfos.AutoMode)
            //{
            //    if (!UtRunCon && !AlarmSkip)
            //    {
            //        m_Ap.PowerOff();
            //        m_Ap.PcwInValve.Close();
            //        m_Ap.SetN2Flow(0);
            //        m_Ap.SetCDAFlow(0);
            //        m_Ap.SetVoltage(0);

            //        if (m_Eqp.AlarmResetSwitchPushed)
            //        {
            //            m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma Alarm Reset"); // 11.03.20 minhan
            //            StartTicks = XFunc.GetTickCount();
            //            AlarmSkip = true;
            //        }
            //        else
            //        {
            //            AlarmSkip = false;
            //            return -1;
            //        }
            //    }
            //}
            //else // 11.03.24 minhan
            //{
                if (!UtRunCon) // 11.03.24 minhan
                {
                    m_Ap.PowerOff();
                    m_Ap.PcwInValve.Close();
                    m_Ap.SetN2Flow(0);
                    m_Ap.SetCDAFlow(0);
                    m_Ap.SetVoltage(0);
                    return -1;
                }
            //}

            
            //if (!m_Server.GenInfos.EqpInitComp) return -1;
            if (!m_Server.GenInfos.AutoMode) return -1;

            if (!m_Ap.PcwInValve.IsOpen()) // 11.05.17 minhan
            {
                m_Ap.PcwInValve.Open();
            }

            if (!m_Ap.IsUse())
            {
                m_Ap.SetN2Flow(0);
                m_Ap.SetCDAFlow(0);
                m_Ap.SetVoltage(0);
            }

            //if (AlarmSkip && (GetElapsedTicks() > 3000)) AlarmSkip = false; // 11.03.24 minhan
            return -1;
        }
        #endregion
    }

    public class SeqSeApProgress : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected ThreadSeApBOE_G8_DHDC m_Control;
        protected int m_ProgressTime;
        protected int m_OldTime = 0;
        protected XTimer m_Timer;
        protected int m_Tm;
        private SeAp m_Ap;
        private bool m_NextGlass = false;
        private bool checkGlass = false; 
        #endregion

        public SeqSeApProgress(ThreadSeApBOE_G8_DHDC control, SeAp ap)
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = m_Server.GenInfos;// GenInfoHandler.Instance;

            this.SeqFunName = m_Ap.Name + " Progress";
            m_Timer = new XTimer(m_Ap.Name + " Timer : SeqApOnProgress");
        }

        public int Do(bool progress)
        {
            int ldOut2ApIn = m_Ap.OwnerUnit.PrevCv.SetupDistanceNext.GetValue<int>();
            int apIn2Out = m_Ap.OwnerUnit.SetupDistance.GetValue<int>();
            int plasmaOnTime = (ldOut2ApIn + apIn2Out + m_Server.SetupGlassSize.GetValue<int>()) / ((m_Server.JobCond.CvProcessSpeed(1) / 60));

            bool runProgress = true;
            runProgress = m_Server.GenInfos.AutoMode;

            int nSeqNo = this.SeqNo;
            int nTime = 0;
            bool nextGlassEnter = m_Ap.OwnerUnit.GlsInSensor.IsDetected(Logic.OR) && /*m_Ap.OwnerUnit.GlsOutSensor.IsDetected() &&*/
                                  m_Ap.OwnerUnit.PrevCv.MotorControl.IsFw(Logic.OR) && m_Server.GlassData.IsExist(m_Ap.OwnerUnit.DataMatchingKey(0));

            switch (nSeqNo)
            {
                case 0:
                    if (progress)
                    {
                        m_OldTime = 0;
                        m_Tm = plasmaOnTime * 1000;
                        m_Timer.Start(m_Tm);
                        //StartTicks = XFunc.GetTickCount();
                        SetSeApOnProgress(ProgressAct.PROGRESS_START, plasmaOnTime);
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (runProgress)
                    {
                        //if (m_Timer.Over)
                        //{
                        //    nSeqNo = 20;
                        //}
                        nTime = (int)m_Timer.CurElapsedTickCounts / 1000;// (int)GetElapsedTicks() / 1000;
                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            SetSeApOnProgress(ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!progress)
                        {
                            SetSeApOnProgress(ProgressAct.PROGRESS_END, 0);
                            m_OldTime = 0;
                            nSeqNo = 0;
                        }
                        if (!m_NextGlass && m_Ap.OwnerUnit.GlsOutSensor.IsDetected(Logic.OR) &&
                             m_Server.GlassData.IsExist(m_Ap.OwnerUnit.DataMatchingKey(1))) 
                        {
                            m_NextGlass = true;
                        }
                        if (m_NextGlass && nextGlassEnter)
                        {
                            m_NextGlass = false;
                            SetSeApOnProgress(ProgressAct.PROGRESS_START, plasmaOnTime);
                            m_Tm = plasmaOnTime * 1000;
                            m_Timer.Start(m_Tm);
                            m_OldTime = 0;
                            //StartTicks = XFunc.GetTickCount();
                        }
                    }
                    else
                    {
                        m_Timer.Pause();
                        nSeqNo = 30;
                    }
                    break;
                case 20:
                    if (!progress)
                    {
                        SetSeApOnProgress(ProgressAct.PROGRESS_END, 0);
                        m_OldTime = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 30: 
                    {
                        checkGlass = m_Control.IsGlassExist(m_Ap);
                        if ((progress || !checkGlass) && runProgress)
                        {
                            m_Timer.Resume();
                            nSeqNo = 10;
                        }
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }

        private void SetSeApOnProgress(ProgressAct act, int time)
        {
            switch (act)
            {
                case ProgressAct.PROGRESS_START:
                    {
                        m_ProgressTime = time;
                        m_GenInfos.ApOnProgress = m_ProgressTime.ToString();
                    }
                    break;
                case ProgressAct.PROGRESS_END:
                    {
                        m_GenInfos.ApOnProgress = "0";
                    }
                    break;
                case ProgressAct.PROGRESS_SET:
                    {
                        string val = string.Format("{0} / {1}", time, m_ProgressTime);
                        m_GenInfos.ApOnProgress = val;
                    }
                    break;
            }
        }
    }

    public class SeqSeApMfcGaugeInterlock : XSeqFunction
    {
        private enum IntrState
        {
            Alarm, Warning, Noop
        }

        #region Fields
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected static Simul m_Simul;
        private ThreadSeApBOE_G8_DHDC m_Control;
        private SeAp m_Ap;
        private Gauge m_Gauge;
        private float m_RecheckDelayTime; // 11.03.20 minhan
        //private double m_FirstDelayTime = 5000; 
        private IntrState m_IntrState = IntrState.Noop;
        private bool[] m_IsAlarmSet;
        private Alarm ALM_LimitAlarm;
        private Alarm ALM_LimitWarning;
        private Alarm ALM_SetValue; 
        private string m_Msg;
        private string m_AlarmData; 
        private string m_Date; 
        #endregion

        #region Constructor
        public SeqSeApMfcGaugeInterlock(ThreadSeApBOE_G8_DHDC control, SeAp ap, Gauge gauge)
        {
            m_Ap = ap;
            m_Gauge = gauge;
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = m_Server.GenInfos;// GenInfoHandler.Instance;
            m_Simul = m_Server.Simul;
            this.SeqFunName = m_Gauge.Name + " Gauge Interlock";
            m_IsAlarmSet = new bool[(int)IntrState.Noop];

            ALM_LimitAlarm = new Alarm(m_Gauge.Name + " Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_LimitWarning = new Alarm(m_Gauge.Name + " Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_SetValue = new Alarm(m_Gauge.Name + "SetValue Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_RecheckDelayTime = 0;
            m_Msg = "";
            m_AlarmData = "";
            m_Date = "";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1; 

            //if (m_Simul.Device) return -1;

            int nSeqNo = this.SeqNo;

            bool checkCond = true;
            //checkCond &= m_Server.GenInfos.AutoMode; 
            checkCond &= m_Ap.IsPowerOn();
            checkCond &= !m_Simul.Device;
            m_RecheckDelayTime = m_Server.ApAlarmcheckTime.GetValue<float>() * 1000; // 11.03.20 minhan

            //checkCond &= m_Ap.IsUse(); 
            //checkCond &= !m_Ap.IsAlarm; 

            double setValue = 0.0;
            if (m_Server.GenInfos.AutoMode) 
            {
                if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name) 
                {
                    setValue = m_Server.JobCond.ApN2Flow(m_Ap);

                    if (m_Server.JobCond.ApUse(m_Ap))
                    {
                        if ((setValue < 700 || setValue > 1500) && (AlarmId == 0)) // 10.12.21 minhan 사양 700
                        {
                            AlarmId = ALM_SetValue.Id;
                            m_Eqp.SetAlarm(AlarmId);
                            m_Msg = string.Format("N2 : {0}", setValue);
                            m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                            m_Ap.IsAlarm = true;
                            m_Ap.PowerOff();
                            m_Control.SetLog(SeqFunName, 0, 0, "N2 Set Value Error : Power Off");
                            nSeqNo = 1000;
                        }
                    }
                }
                else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name)
                {
                    setValue = m_Server.JobCond.ApCDAFlow(m_Ap);

                    if (m_Server.JobCond.ApUse(m_Ap))
                    {
                        if ((setValue < 0 || setValue > 10) && (AlarmId == 0)) // 11.03.24 minhan
                        {
                            AlarmId = ALM_SetValue.Id;
                            m_Eqp.SetAlarm(AlarmId);
                            m_Msg = string.Format("CDA : {0}", setValue);
                            m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                            m_Ap.IsAlarm = true;
                            m_Ap.PowerOff();
                            m_Control.SetLog(SeqFunName, 0, 0, "CDA Set Value Error : Power Off");
                            nSeqNo = 1000;
                        }
                    }
                }
                else if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name)
                {
                    setValue = m_Server.JobCond.ApVoltage(m_Ap);

                    if (m_Server.JobCond.ApUse(m_Ap))
                    {
                        if ((setValue < 7 || setValue > 13) && (AlarmId == 0)) // 11.04.20 minhan
                        {
                            AlarmId = ALM_SetValue.Id;
                            m_Eqp.SetAlarm(AlarmId);
                            m_Msg = string.Format("Voltage : {0}", setValue);
                            m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                            m_Ap.IsAlarm = true;
                            m_Ap.PowerOff();
                            m_Control.SetLog(SeqFunName, 0, 0, "Voltage Set Value Error : Power Off");
                            nSeqNo = 1000;
                        }
                    }
                }
            }
            else // 범위를 벗어난 n2,cda발생시 무조건 power off
            {
                
                if (((GlobalVar.ApManualN2Set < 700) || (GlobalVar.ApManualN2Set > 1500)) ||
                    ((GlobalVar.ApManualCDASet < 0) || (GlobalVar.ApManualCDASet > 10)) ||
                    ((GlobalVar.ApManualVolSet < 7) || (GlobalVar.ApManualVolSet > 13))) // 11.03.24 minhan
                {
                    m_Ap.PowerOff();
                }
                else if ((eqpGauges._AP_Unit_N2_Flow_Gauge.CurValue < 700) || 
                        (eqpGauges._AP_Unit_PCW_Out_Gauge.CurValue <= 3.5) ||
                        (eqpGauges._AP_Unit_CDA_Flow_Gauge.CurValue < 0))  // 11.03.26 minhan
                {
                    m_Ap.PowerOff();
                }

                if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name) setValue = GlobalVar.ApManualN2Set;
                else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name) setValue = GlobalVar.ApManualCDASet;
                else if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name) setValue = GlobalVar.ApManualVolSet;
            }
            switch (nSeqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        //StartTicks = XFunc.GetTickCount(); // 11.03.20 minhan
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (m_IsAlarmSet[(int)IntrState.Alarm])
                        {
                            m_IsAlarmSet[(int)IntrState.Alarm] = false;
                            m_Eqp.ResetAlarm(ALM_LimitAlarm.Id);
                            AlarmId = 0; 
                            m_Control.SetLog(SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.Warning])
                        {
                            m_IsAlarmSet[(int)IntrState.Warning] = false;
                            m_Eqp.ResetAlarm(ALM_LimitWarning.Id);
                            AlarmId = 0; 
                            m_Control.SetLog(SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }

                        if (m_Gauge.IsAlarm)
                        {
                            m_Gauge.IsAlarm = false;
                        }
                    }
                    break;
                case 10:
                    {
                        //if (GetElapsedTicks() > 1000) nSeqNo = 20; // 11.03.20 minhan
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (!checkCond) nSeqNo = 0;
                    else
                    {
                        double alarmLimit = 0; 
                        double warningLimit = 0;

                        if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name) // 11.02.01 minhan
                        {
                            alarmLimit = m_Ap.SetupApVolAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApVolWarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApN2AlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApN2WarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApCDAAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApCDAWarningIntrMargin.GetValue<double>();
                        }

                        double alarmLimitVal = (alarmLimit / 100.0) * setValue;
                        double warningLimitVal = (warningLimit / 100.0) * setValue;

                        if (Math.Abs(m_Gauge.CurValue - setValue) > alarmLimitVal)
                        {
                            m_IntrState = IntrState.Alarm;
                        }
                        else if (Math.Abs(m_Gauge.CurValue - setValue) > warningLimitVal)
                        {
                            m_IntrState = IntrState.Warning;
                        }
                        else
                        {
                            m_IntrState = IntrState.Noop; 
                        }

                        if (m_IntrState != IntrState.Noop)
                        {
                            //if (m_IntrState == IntrState.Alarm)
                            //{
                            //    m_RecheckDelayTime = 3000;
                            //}
                            //else m_RecheckDelayTime = 5000;
                            //m_RecheckDelayTime = 1500; // 11.03.20 minhan 
                            m_IntrState = IntrState.Noop; 
                            StartTicks = XFunc.GetTickCount(); 
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > m_RecheckDelayTime)
                    {
                        nSeqNo = 40;
                    }
                    else
                    {
                        double alarmLimit = 0; 
                        double warningLimit = 0;

                        if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name) // 11.02.01 minhan
                        {
                            alarmLimit = m_Ap.SetupApVolAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApVolWarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApN2AlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApN2WarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApCDAAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApCDAWarningIntrMargin.GetValue<double>();
                        }

                        double alarmLimitVal = (alarmLimit / 100.0) * setValue;
                        double warningLimitVal = (warningLimit / 100.0) * setValue;

                        if (Math.Abs(m_Gauge.CurValue - setValue) > alarmLimitVal)
                        {
                            m_IntrState = IntrState.Alarm;
                        }
                        else if (Math.Abs(m_Gauge.CurValue - setValue) > warningLimitVal)
                        {
                            m_IntrState = IntrState.Warning;
                        }
                        else
                        {
                            m_IntrState = IntrState.Noop; 
                        }

                        //if (m_IntrState != IntrState.Noop)
                        //{
                        //}
                        //else nSeqNo = 20;
                        if (m_IntrState == IntrState.Noop) nSeqNo = 20; 
                    }
                    break;
                case 40:
                    if (!checkCond)
                    {
                        nSeqNo = 0;
                    }
                    else 
                    {
                        double alarmLimit = 0; 
                        double warningLimit = 0;

                        if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name) // 11.02.01 minhan
                        {
                            alarmLimit = m_Ap.SetupApVolAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApVolWarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApN2AlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApN2WarningIntrMargin.GetValue<double>();
                        }
                        else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name)
                        {
                            alarmLimit = m_Ap.SetupApCDAAlarmIntrMargin.GetValue<double>();
                            warningLimit = m_Ap.SetupApCDAWarningIntrMargin.GetValue<double>();
                        }

                        double alarmLimitVal = (alarmLimit / 100.0) * setValue;
                        double warningLimitVal = (warningLimit / 100.0) * setValue;

                        double curVal = m_Gauge.CurValue;

                        if (Math.Abs(curVal - setValue) > alarmLimitVal)
                        {
                            m_IntrState = IntrState.Alarm;
                        }
                        else if (Math.Abs(curVal - setValue) > warningLimitVal)
                        {
                            m_IntrState = IntrState.Warning;
                        }
                        else 
                        {
                            m_IntrState = IntrState.Noop;
                        }

                        if (m_IntrState == IntrState.Alarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.Alarm])
                            {
                                m_IsAlarmSet[(int)IntrState.Alarm] = true;
                                AlarmId = ALM_LimitAlarm.Id; 
                                m_Eqp.SetAlarm(AlarmId); 
                                m_Msg = string.Format("{0:F2} ({1}, {2}%)", curVal, setValue, alarmLimit);
                                m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                                m_Ap.IsAlarm = true;
                                m_Ap.PowerOff();
                                m_Control.SetLog(SeqFunName, 0, 0, "Gauge Alarm : Power Off");

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss"); 
                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}%)", m_Date, m_Gauge.Name, curVal, setValue, alarmLimit); // 11.03.20 minhan

                                if (GlobalVar.ApCurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.ApCurGaugeAlarm.Clear();
                                    GlobalVar.ApCurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.ApCurGaugeAlarm.Add(m_AlarmData);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.Alarm])
                            {
                                m_IsAlarmSet[(int)IntrState.Alarm] = false;
                                m_Eqp.ResetAlarm(AlarmId); 
                                AlarmId = 0; 
                            }
                        }

                        if (m_IntrState == IntrState.Warning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.Warning])
                            {
                                m_IsAlarmSet[(int)IntrState.Warning] = true;
                                AlarmId = ALM_LimitWarning.Id; 
                                m_Eqp.SetAlarm(m_AlarmId); 
                                m_Msg = string.Format("{0:F2} ({1}, {2}%)", curVal, setValue, warningLimit);
                                m_Control.SetLog(SeqFunName, 0, 0, m_Msg);

                                m_Date = DateTime.Now.ToString("yyyyMMddHHmmss");
                                m_AlarmData = string.Format("{0}/{1}/{2:F2}/({3}, {4}%)", m_Date, m_Gauge.Name, curVal, setValue, alarmLimit); // 11.03.20 minhan

                                if (GlobalVar.ApCurGaugeAlarm.Count > 50)
                                {
                                    GlobalVar.ApCurGaugeAlarm.Clear();
                                    GlobalVar.ApCurGaugeAlarm.Add(m_AlarmData);
                                }
                                else GlobalVar.ApCurGaugeAlarm.Add(m_AlarmData);
                 
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.Warning])
                            {
                                m_IsAlarmSet[(int)IntrState.Warning] = false;
                                m_Eqp.ResetAlarm(AlarmId); 
                                AlarmId = 0;
                            }
                        }

                        if (!m_IsAlarmSet[(int)IntrState.Alarm])
                        {
                            if (m_Gauge.IsAlarm)
                            {
                                m_Gauge.IsAlarm = false;
                                m_Control.SetLog(SeqFunName, 0, 0, "OK : Recovery");
                            }

                            if (!m_IsAlarmSet[(int)IntrState.Warning])
                            {
                                nSeqNo = 20;
                            }
                        }
                        else
                        {
                            if (!m_Gauge.IsAlarm)
                            {
                                m_Gauge.IsAlarm = true;
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "NG");
                            }

                            //ReturnSeqNo = nSeqNo; 
                            m_Ap.IsAlarm = true;
                            nSeqNo = 1000;
                            break;
                        }
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "Check Recovery[AP FLOW]");
                        m_Eqp.ResetAlarm(AlarmId);
                        AlarmId = 0; 
                        m_Ap.IsAlarm = false;
  
                        nSeqNo = 0; 
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqSeApAlarm : XSeqFunction 
    {
        #region Fileds
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private SeAp m_Ap;
        private ThreadSeApBOE_G8_DHDC m_Control;
        private Alarm ALM_PcwInterlock;
        private Alarm ALM_N2Interlock;
        private new int[] m_AlarmId;
        //private uint Delaytime; 
        private float Delaytime; // 10.12.15 minhan
        #endregion

        #region Constructor
        public SeqSeApAlarm(ThreadSeApBOE_G8_DHDC control, SeAp ap)
        {
            m_Ap = ap;
            m_Server = ServerManager.Instance; // 10.12.21 minhan
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = m_Server.GenInfos;
            m_AlarmId = new int[3]; // 10.12.21 minhan
            this.SeqFunName = m_Ap.Name + " ALARM";
            Delaytime = 0;
            ALM_PcwInterlock = new Alarm(m_Ap.Name + "PCW Flow Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_N2Interlock = new Alarm(m_Ap.Name + "N2 Flow Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

        }
        #endregion

        #region Methode
        public int GetInterlockalarm()
        {
            SeAp Ap = eqpSeAps._SE_AP_Plasma_Unit;
            ApInterlockStatus nRv = 0;
             
            // 10.12.21 minhan Flow 인터락 알람이 Low 아닌가?
            //if (Ap.DiPCWFlowHighLimitAlarm.GetState()) nRv |= ApInterlockStatus.ApPCWflowInterlock;
            //if (Ap.DiN2PressHighLimitAlarm.GetState()) nRv |= ApInterlockStatus.ApN2flowInterlock;

            if (Ap.DiPCWFlowLowLimitAlarm.GetState() && Ap.PcwInValve.IsOpen()) nRv |= ApInterlockStatus.ApPCWflowInterlock;
            if (Ap.DiN2FlowLowLimitAlarm.GetState() && (Ap.MfcN2.AoFlowSet.GetState() >= 1911)) nRv |= ApInterlockStatus.ApN2flowInterlock; // 11.03.24 minhan
            // DiN2FlowLowLimitAlarm Base 에 추가. 7645는 n2 set 700을 표시한 값.

            return (int)nRv;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp || !m_GenInfos.AutoMode) return -1; 

            int nSeqNo = this.SeqNo;
            int alarmCode = 0;
            int checkInterlock = 0;
            bool simulation = m_Server.Simul.Device;
            //Delaytime = m_Server.ApAlarmcheckTime.GetValue<float>() * 1000; // 10.12.21 minhan
            Delaytime = m_Server.ApStatusCheckTime.GetValue<float>() * 1000; // 11.04.27 minhan

            switch (nSeqNo)
            {
                case 0:
                    {
                        checkInterlock = GetInterlockalarm();
                        alarmCode = m_Ap.GetStatusAlarm();

                        if (((alarmCode != (int)SeApAlarmIndex.plasmaNoAlm) || (checkInterlock != 0)) &&
                            m_Ap.IsUse() &&
                            !m_Server.Simul.Device)
                        {
                            if (alarmCode != (int)SeApAlarmIndex.plasmaNoAlm) 
                            {
                                //m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma Status Alarm");
                                //Delaytime = 0;

                                m_AlarmId[2] = alarmCode + m_Ap.ALM_InterlockErr.Id; // 10.12.21 minhan 바로 알람처리하는것이.
                                m_Eqp.SetAlarm(m_AlarmId[2]);
                                m_Ap.IsAlarm = true;
                                m_Ap.PowerOff();
                                m_Control.SetLog(SeqFunName, 0, 0, "Alarm Set : Power Off");
                                nSeqNo = 1000;
                            }
                            else 
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma Interlock Alarm");
                                //Delaytime = 2000; //일단 그냥 DelayTime 설정Setup에서 설정하도록 사이트마다 다르기 때문에 동일하게 하기가 어렵습니다. 10.12.21 minhan
                                StartTicks = XFunc.GetTickCount();
                                nSeqNo = 10;
                            }
                        }
                    }
                    break;
                case 10: // 10.12.21 minhan
                    {
                        checkInterlock = GetInterlockalarm();
                        alarmCode = m_Ap.GetStatusAlarm(); 

                        if (!m_Ap.IsUse() ||
                           ((alarmCode == (int)SeApAlarmIndex.plasmaNoAlm) &&
                           (checkInterlock == 0)))
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            if (alarmCode != (int)SeApAlarmIndex.plasmaNoAlm)
                            {
                                m_AlarmId[2] = alarmCode + m_Ap.ALM_InterlockErr.Id;
                                m_Eqp.SetAlarm(m_AlarmId[2]);
                                m_Ap.IsAlarm = true;
                                m_Ap.PowerOff();
                                m_Control.SetLog(SeqFunName, 0, 0, "Alarm Set : Power Off");
                                nSeqNo = 1000;
                            }
                            else if ((GetElapsedTicks() > Delaytime) && (checkInterlock != 0))
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if ((checkInterlock & (0x0001 << i)) != 0)
                                    {
                                        m_AlarmId[i] = ALM_PcwInterlock.Id + i;
                                        m_Eqp.SetAlarm(m_AlarmId[i]);
                                    }
                                }
                                m_Ap.IsAlarm = true;
                                m_Ap.PowerOff();
                                m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma Limit Alarm Set : Power Off");
                                nSeqNo = 1000;
                            }

                        }
                    }
                    break;
                case 1000: 
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (m_AlarmId[i] != 0)
                            {	// Reset Alarm
                                m_Eqp.ResetAlarm(m_AlarmId[i]);
                                m_AlarmId[i] = 0;
                            }
                        }
                        m_Control.SetLog(SeqFunName, 0, 0, "Alarm Recovery");
                        m_Ap.IsAlarm = false;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }


    public class SeqSeApHouseSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private ThreadSeApBOE_G8_DHDC m_Control;
        private SeAp m_Ap;
        #endregion

        #region Constructor
        public SeqSeApHouseSensor(ThreadSeApBOE_G8_DHDC control, SeAp ap)
        {
            m_Control = control;
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_GenInfos = m_Server.GenInfos;// GenInfoHandler.Instance;

            SeqFunName = m_Ap.Name + " HOUSE OPEN";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;

            // bool ok = false;

            bool close = true; 
            close = m_Control.GetHouseCloseState(m_Ap);

            if (!m_Server.GenInfos.AutoMode || !m_Ap.IsUse())
            {

                if ((m_AlarmId != 0) && m_Eqp.AlarmResetSwitchPushed && close)
                {
                    m_Eqp.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;
                    m_Ap.IsAlarm = false;
                    m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma House Open Alarm Reset");
                }

                return -1; 
            }

            if (!close && (m_AlarmId == 0))
            {
                m_AlarmId = m_Ap.ALM_HouseOpen.Id;
                m_Eqp.SetAlarm(m_AlarmId);
                m_Ap.IsAlarm = true;
                m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma House Open Alarm Set");
            }
            else if ((m_AlarmId != 0) && m_Eqp.AlarmResetSwitchPushed && close)
            {
                m_Eqp.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                m_Ap.IsAlarm = false;
                m_Control.SetLog(SeqFunName, 0, 0, "AP Plasma House Open Alarm Reset");
            }
            return -1;
        }
        #endregion

    }

    public class SeqSeApManualInterlock : XSeqFunction 
    {
        #region Fields
        protected static ServerManager m_Server;
        private ThreadSeApBOE_G8_DHDC m_Control;
        private SeAp m_Ap;
        #endregion

        #region Constructor
        public SeqSeApManualInterlock(ThreadSeApBOE_G8_DHDC control, SeAp ap)
        {
            m_Control = control;
            m_Ap = ap;
            m_Server = ServerManager.Instance;

            SeqFunName = m_Ap.Name + " Manual";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_Server.GenInfos.AutoMode/* || !m_Ap.IsUse()*/) return -1;

            bool ApPowerOn = m_Ap.IsPowerOn();
            bool HouseClose = m_Control.GetHouseCloseState(m_Ap);
            bool HeavyInterlock = (m_Server.JobCond.HeavyInterlock > 0) ? true : false;

            if (!HouseClose)
            {
                m_Ap.PowerOff();
            }
            else if (HeavyInterlock)
            {
                m_Ap.PowerOff();
            }
            else if (!m_Ap.PcwInValve.IsOpen())
            {
                m_Ap.PowerOff();
            }
            else if (!m_Ap.IsUse()) 
            {
                m_Ap.PowerOff();
            }
            else if (m_Ap.DiPCWFlowLowLimitAlarm.GetState() || m_Ap.DiN2FlowLowLimitAlarm.GetState()) // 11.03.26 minhan
            {
                m_Ap.PowerOff();
            }

            return -1;
        }
        #endregion

    }
}
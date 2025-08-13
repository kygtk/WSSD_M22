using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.Sequence;
using Dms.ServerCommon;


namespace Dms.Server
{
    public class ThreadHpmjIntefaceControl : ThreadHpmjControl
    {
        #region enum
        private enum HpmjGauge { GAUGE_CO2, GAUGE_DI, GAUGE_RESIST, GAUGE_PRESS_IN, GAUGE_PRESS_OUT, GAUGE_FLOW, GAUGE_MAX_NO, }
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ThreadHpmjIntefaceControl(int scanTime, IServerManager server)
            : base(scanTime, server)
        {

        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Hpmj device in m_HpmjUnits)
            {
                //RegisterSequence(new SeqHpmj(this, device));
                RegisterSequence(new SeqHpmjAlarm(this, device));
                RegisterSequence(new SeqHpmjModeChange(this, device));
                //RegisterSequence(new SeqHpmjPowerCut(this, device));
                RegisterSequence(new SeqCo2Valve(this, device));
                //RegisterSequence(new SeqHpmjManual(this, device));
                RegisterSequence(new SeqPumpRun(this, device));
                RegisterSequence(new SeqStateMonitor(this, device));
                RegisterSequence(new SeqHpmjAi(this, device));
                RegisterSequence(new SeqHpmjPumpInterlock(this, device));
                RegisterSequence(new SeqPowerCut(this, device));
                //RegisterSequence(new SeqHpmjIdleRunning(this, device)); //11.06.13 mang
                for (int ngauge = 0; ngauge < (int)HpmjGauge.GAUGE_MAX_NO; ngauge++)//2009.09.17 kimgun
                {
                    Gauge gauge = GetItem(device, ngauge);
                    if (gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id) == 0 ||
                        gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) == 0 ||
                        gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)//lkl 150929
                        RegisterSequence(new SeqHpmjGaugeInterlock(this, device, gauge));
                }
                RegisterSequence(new SeqHpmjThreadCheck(this));
                RegisterSequence(new SeqInitCo2VentValve(this, device));    // 20121019 ssm
            }
            m_Server.AddSeqInitFunction(new SeqInitHpmj(this, m_HpmjUnits));
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                CheckStopCountCond();

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Override Methods
        public override void CheckStopCountCond()
        {
        }

        public override bool IsAlarmCondition(Hpmj hpmj)
        {
            bool ng = false;

            return ng;
        }

        public override bool IsInterlock(Hpmj hpmj)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy & ~HeavyInterlock.Cover) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Emo) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Leak) > 0);

            return interlock;
        }

        public override bool IsRunEnable(Hpmj hpmj)
        {
            bool run = true;
            run &= m_Server.JobCond.ProcessMode;
            run &= !IsAlarmCondition(hpmj);

            return run;
        }

        public override bool IsAutoRunCondition(Hpmj hpmj)
        {
            bool run = true;

            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= !IsInterlock(hpmj);
            run &= IsRunEnable(hpmj);

            return run;
        }
        #endregion

        #region General Methods
        private Gauge GetItem(Hpmj device, int id)
        {
            Gauge gauge;
            switch (id)
            {
                case (int)HpmjGauge.GAUGE_CO2:
                    gauge = device.CO2InPress;
                    break;
                case (int)HpmjGauge.GAUGE_DI:
                    gauge = device.MainDiPress;
                    break;
                case (int)HpmjGauge.GAUGE_RESIST:
                    gauge = device.Resistivity;
                    break;
                case (int)HpmjGauge.GAUGE_PRESS_IN:
                    gauge = device.FilterInPress;
                    break;
                case (int)HpmjGauge.GAUGE_PRESS_OUT:
                    gauge = device.FilterOutPress;
                    break;
                case (int)HpmjGauge.GAUGE_FLOW:
                    gauge = device.HpmjFlow;
                    break;
                default:
                    gauge = null;
                    break;
            }
            return gauge;
        }
        #endregion
    }

    public class SeqHpmj : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadHpmjControl m_Control;
        private Hpmj HpmjUnit;
        #endregion

        #region Constructor
        public SeqHpmj(ThreadHpmjControl control, Hpmj hpmj)
        {
            HpmjUnit = hpmj;
            m_Server = HpmjUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            this.m_SeqFunName = HpmjUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            switch (seqNo)
            {
                case 0:
                    break;
            }

            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }

    public class SeqHpmjGaugeInterlock : XSeqFunction
    {
        #region Fields
        private enum IntrState { LowAlarm, UpAlarm, LowWarning, UpWarning, Noop, }

        protected static ServerManager m_Server; //090923:LeeChungWon
        protected static IEqpManager m_EqpManager;
        private GenInfoHandler m_GenInfo;//090923 LeeChungWon
        protected static ThreadHpmjControl m_Control;
        protected static Simul m_Simul;

        private Hpmj m_HpmjUnit;

        protected Gauge m_gauge;

        //private double m_DelayTime = 2000; // 09.12.01 minhan
        private IntrState m_IntrState = IntrState.Noop;
        private string m_Msg = "";
        private static double StandardFlowrate = 0; // 09.12.01 minhan
        private static double targetVal = 0; // 09.12.01 minhan  
        private double m_CheckDelayTime = 0; // 09.12.01 minhan
        #endregion

        #region Constructor
        public SeqHpmjGaugeInterlock(ThreadHpmjControl control, Hpmj hpmj, Gauge gauge)
        {
            m_HpmjUnit = hpmj;
            m_Server = ServerManager.Instance;//090923:LeeChungWon
            //m_Server = m_HpmjUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            //m_GenInfos = GenInfoHandler.Instance; //090923 LeeChungWon
            m_GenInfo = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;

            m_SeqFunName = string.Format("HPMJ {0}GAUGE", gauge.Name);
            gauge.ALM_LowerAlarm = new Alarm(gauge.Name + " : Lower Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            gauge.ALM_LowerWarning = new Alarm(gauge.Name + " : Lower Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            gauge.ALM_UpperWarning = new Alarm(gauge.Name + " : Upper Limit Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            gauge.ALM_UpperAlarm = new Alarm(gauge.Name + " : Upper Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_gauge = gauge;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            double PreOutDiff = 2;//090924 :LeeChungWon
            bool checkCond = true;
            checkCond &= GetCheckCond(m_gauge.Id);
            checkCond &= !simulation;
            //   checkCond &= m_gauge.SetupInterlock.Use;
            m_CheckDelayTime = m_Server.GaugeAlarmCheckTime.GetValue<double>() * 1000; // 09.12.01 minhan 

            m_GenInfo.HpmjTargetFlow = string.Format("{0:F2}", GetStandardFlowrate());

            switch (seqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        StandardFlowrate = GetStandardFlowrate(); // 09.12.01 minhan

                        if (GenInfoHandler.Instance.EQPGlassCount > 0) targetVal = m_Server.JobCond.HpmjPressure(m_HpmjUnit); // 09.12.01 minhan
                        else targetVal = m_Server.SetupIdleHpmjPress.GetValue<int>();

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    else
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        m_gauge.IsAlarm = false;//2009.09.17 kimgun
                        if (m_HpmjUnit.IfFlag.GaugeInterlockAlarm)
                        {
                            m_HpmjUnit.IfFlag.GaugeInterlockAlarm = false;
                        }
                    }
                    break;
                case 10:
                    if (!checkCond) // 09.12.01 minhan
                    {
                        m_SeqNo = 0;
                    }
                    // else if (GetElapsedTicks() > m_DelayTime)
                    else if (GetElapsedTicks() > m_CheckDelayTime) // 09.12.01 minhan
                    {
                        StandardFlowrate = GetStandardFlowrate(); // 10.01.13 minhan

                        if (GenInfoHandler.Instance.EQPGlassCount > 0) targetVal = m_Server.JobCond.HpmjPressure(m_HpmjUnit); // 10.01.13 minhan
                        else targetVal = m_Server.SetupIdleHpmjPress.GetValue<int>();

                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (!checkCond)
                    {
                        seqNo = 0;
                    }
                    else
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal;

                        double flowWarningMargin = 0;
                        double flowAlarmMargin = 0;
                        double pressWarningMargin = 0;
                        double pressAlarmMargin = 0;
                        double lowAlarm = 0;
                        double upAlarm = 0;
                        double lowWarning = 0;
                        double upWarning = 0;

                        if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id) == 0 ||
                            m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) == 0 ||
                            m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                        {
                            flowWarningMargin = m_Server.SetupFlowWarningMargin.GetValue<double>();
                            flowAlarmMargin = m_Server.SetupFlowAlarmMargin.GetValue<double>();
                            pressWarningMargin = m_Server.SetupPressWarningMargin.GetValue<double>();
                            pressAlarmMargin = m_Server.SetupPressAlarmMargin.GetValue<double>();
                        }

                        if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id) == 0)
                        {

                            curVal = m_gauge.CurValue;
                            lowAlarm = StandardFlowrate + (flowAlarmMargin * -1);
                            upAlarm = StandardFlowrate + (flowAlarmMargin * 1);
                            lowWarning = StandardFlowrate + (flowWarningMargin * -1);
                            upWarning = StandardFlowrate + (flowWarningMargin * 1);


                            if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                            else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                            else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                            else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }

                        }
                        else if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) == 0 ||
                        m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                        {

                            curVal = m_gauge.CurValue;// -targetVal;

                            if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                            {
                                pressAlarmMargin += PreOutDiff;
                                pressWarningMargin += PreOutDiff;
                            }
                            lowAlarm = targetVal + (pressAlarmMargin * -1);
                            upAlarm = targetVal + (pressAlarmMargin * 1);
                            lowWarning = targetVal + (pressWarningMargin * -1);
                            upWarning = targetVal + (pressWarningMargin * 1);
                            if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                            else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                            else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                            else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }
                        }

                        if (m_IntrState != IntrState.Noop)
                        {
                            m_Msg = string.Format("GAUGE:{0} is Interlock Condition", m_gauge.Id);//20091020 LeeChungWon
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 30;
                        }
                        else if (m_IntrState != IntrState.LowAlarm &&
                                 m_IntrState != IntrState.UpAlarm)
                            m_gauge.IsAlarm = false;
                    }
                    break;
                case 30:
                    if (!checkCond)
                    {
                        seqNo = 0;
                    }
                    else
                    {
                        StandardFlowrate = GetStandardFlowrate(); // 10.01.13 minhan

                        if (GenInfoHandler.Instance.EQPGlassCount > 0) targetVal = m_Server.JobCond.HpmjPressure(m_HpmjUnit); // 10.01.13 minhan
                        else targetVal = m_Server.SetupIdleHpmjPress.GetValue<int>();

                        m_IntrState = IntrState.Noop;

                        double curVal = 0;

                        double flowWarningMargin = 0;
                        double flowAlarmMargin = 0;
                        double pressWarningMargin = 0;
                        double pressAlarmMargin = 0;
                        double lowAlarm = 0;
                        double upAlarm = 0;
                        double lowWarning = 0;
                        double upWarning = 0;

                        //090923 : LeeChungWon
                        if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id) == 0 ||
                            m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) == 0 ||
                            m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                        {
                            flowWarningMargin = m_Server.SetupFlowWarningMargin.GetValue<double>();
                            flowAlarmMargin = m_Server.SetupFlowAlarmMargin.GetValue<double>();
                            pressWarningMargin = m_Server.SetupPressWarningMargin.GetValue<double>();
                            pressAlarmMargin = m_Server.SetupPressAlarmMargin.GetValue<double>();
                        }


                        if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge.Id) == 0)
                        {

                            curVal = m_gauge.CurValue;// -GetStandardFlowrate();
                            lowAlarm = StandardFlowrate + (flowAlarmMargin * -1); // 09.12.01 minhan
                            upAlarm = StandardFlowrate + (flowAlarmMargin * 1);
                            lowWarning = StandardFlowrate + (flowWarningMargin * -1);
                            upWarning = StandardFlowrate + (flowWarningMargin * 1);

                            if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                            else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                            else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                            else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }
                        }
                        else if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge.Id) == 0 ||
                        m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                        {

                            curVal = m_gauge.CurValue;// -targetVal;

                            if (m_gauge.Id.CompareTo(eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge.Id) == 0)
                            {
                                pressAlarmMargin += PreOutDiff;
                                pressWarningMargin += PreOutDiff;
                            }
                            lowAlarm = targetVal + (pressAlarmMargin * -1);
                            upAlarm = targetVal + (pressAlarmMargin * 1);
                            lowWarning = targetVal + (pressWarningMargin * -1);
                            upWarning = targetVal + (pressWarningMargin * 1);
                            if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                            else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                            else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                            else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }

                        }

                        if (GetElapsedTicks() > m_CheckDelayTime) // 09.12.01 minhan
                        {
                            if (m_IntrState == IntrState.LowAlarm)
                            {
                                {
                                    if (m_AlarmId > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId = m_gauge.ALM_LowerAlarm.Id;
                                    m_gauge.IsAlarm = true;//2009.09.17 kimgun
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                                    seqNo = 1000;
                                }
                            }
                            else if (m_IntrState == IntrState.UpAlarm)
                            {
                                {
                                    if (m_AlarmId > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }
                                    m_gauge.IsAlarm = true;//2009.09.17 kimgun
                                    m_AlarmId = m_gauge.ALM_UpperAlarm.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);


                                    seqNo = 1000;
                                }
                            }
                            else if (m_IntrState == IntrState.LowWarning)
                            {
                                if (m_AlarmId != m_gauge.ALM_LowerWarning.Id)
                                {
                                    if (m_AlarmId > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId = m_gauge.ALM_LowerWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                }
                            }
                            else if (m_IntrState == IntrState.UpWarning)
                            {
                                if (m_AlarmId != m_gauge.ALM_UpperWarning.Id)
                                {
                                    if (m_AlarmId > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId = m_gauge.ALM_UpperWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                }
                            }
                        }

                        if (m_IntrState == IntrState.Noop)
                        {
                            if (m_AlarmId > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId);
                                m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                                m_AlarmId = 0;
                            }

                            m_Msg = string.Format("GAUGE:{0} is not Interlock Condition", m_gauge.Id);
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            seqNo = 0;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion

        #region Methods

        private bool GetCheckCond(int id) // 09.12.01 minhan 않쓰는 게이지는 ProcessScenario.cs 에서 처리.
        {
            bool check = true;
            check &= m_GenInfo.AutoMode;
            check &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            check &= m_Server.JobCond.ProcessMode;
            check &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            check &= m_HpmjUnit.IfFlag.PumpRun;
            check &= m_HpmjUnit.IfFlag.Ready;


            return check;
        }

        private double GetStandardFlowrate()
        {
            int StandardPress = m_HpmjUnit.SetupHpmjStandardPressure.GetValue<int>();
            double StandardFlow = m_HpmjUnit.SetupHpmjStandardFlowrate.GetValue<double>();
            double StandardArea = StandardFlow / Math.Pow(StandardPress, 0.5);

            //090923 : LeeChungWon 
            if (!GenInfoHandler.Instance.AutoMode)
            {
                if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>()) return (StandardArea * Math.Pow(m_HpmjUnit.SetupHpmjPressure.GetValue<double>(), 0.5));
                else return 0.0;
            }
            else
            {
                if (GenInfoHandler.Instance.EQPGlassCount > 0) return (StandardArea * Math.Pow(m_Server.JobCond.HpmjPressure(m_HpmjUnit), 0.5));
                else if (m_Server.SetupHpmjIdleUse.GetValue<bool>()) return (StandardArea * Math.Pow(m_Server.SetupIdleHpmjPress.GetValue<int>(), 0.5));
                else return 0.0;
            }
        }
        #endregion
    }
    public class SeqHpmjModeChange : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected static double PrevPressure;
        #endregion

        #region Constructor
        public SeqHpmjModeChange(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = ServerManager.Instance;//2009.09.17 kimgun//m_HpmjUnit.ServerManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "HPMJ MODE";
            //PrevPressure = m_HpmjUnit.SetupHpmjPressure.GetValue<double>();
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            bool Auto = m_GenInfos.AutoMode;

            switch (seqNo)
            {
                case 0:
                    if (Auto)
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Auto Mode");

                        seqNo = 10;
                    }
                    else if (!Auto)
                    {
                        PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                        string log = string.Format(m_SeqFunName, 0, 0, "Save Previous HPMJ Pressure {0}", PrevPressure);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, log);
                        seqNo = 20;
                    }
                    break;
                case 10:
                    if (!Auto)
                    {

                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Manual Mode");
                        m_HpmjUnit.IfFlag.PumpRun = false;//110410 bkh
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (Auto)
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Auto Mode");

                        if (m_HpmjUnit.IfFlag.PumpRun)
                        {
                            m_HpmjUnit.IfFlag.ModeChange = true;
                            if (!m_GenInfos.EqpInitComp)
                            {
                                m_HpmjUnit.IfFlag.PumpRun = false;
                                m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "EQP was not initialized. Pump Stop.");
                            }
                        }
                        seqNo = 10;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCo2Valve : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        #endregion

        #region Constructor
        public SeqCo2Valve(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
            m_Control = control;
            if (AppConfig.Instance.Simul.Device) m_HpmjUnit.IfFlag.DiLack = true;
            else m_HpmjUnit.IfFlag.DiLack = false;
            m_SeqFunName = "HPMJ VALVE";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            double setupInOpenLevel = (double)m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<float>();
            double setupVentOpenTime = ((double)m_HpmjUnit.SetupHpmjCO2VentValveOpenTime.GetValue<float>() * 1000);
            double setupVentOpenPeriod = ((double)m_HpmjUnit.SetupHpmjCO2VentValveOpenPeriod.GetValue<float>() * 60 * 1000); // 3600 * 1000);

            bool setupVentControl = (bool)m_HpmjUnit.SetupHpmjCO2VentControl.GetValue<bool>();

            // dspcrassus - HPMJ Unit : CO2 In Valve Open/Close Interlock
            double dMainPress = m_HpmjUnit.MainDiPress.CurValue;
            if (!m_HpmjUnit.IfFlag.PumpRun && m_HpmjUnit.CO2InValve.IsOpen())//20130131 long The CO2 valve is closed for prevent excessive CO2, if the HPMJ stop.
            {
                m_HpmjUnit.CO2InValve.Close();
                m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, dMainPress.ToString());
                m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "CO2 Valve Close");
            }
            else if (m_HpmjUnit.IfFlag.PumpRun)
            {
                if ((!m_HpmjUnit.IfFlag.DiLack || m_HpmjUnit.CO2InValve.IsOpen()) && (dMainPress < setupInOpenLevel))
                {
                    m_HpmjUnit.IfFlag.DiLack = true;
                    m_HpmjUnit.CO2InValve.Close();
                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, dMainPress.ToString());
                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "CO2 Valve Close");
                }
                else if ((m_HpmjUnit.IfFlag.DiLack || m_HpmjUnit.CO2InValve.IsClose()) && (dMainPress >= setupInOpenLevel))
                {
                    m_HpmjUnit.IfFlag.DiLack = false;
                    m_HpmjUnit.CO2InValve.Open();
                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "CO2 Valve Open");
                }
            }

            // dspcrassus - HPMJ Unit : CO2 Vent Valve Open/Close (S/W Control)
            switch (seqNo)
            {
                case 0:
                    if (setupVentControl)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > setupVentOpenPeriod)
                    {
                        m_HpmjUnit.CO2VentValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    else if (!setupVentControl)
                    {
                        seqNo = 0;
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > setupVentOpenTime)
                    {
                        m_HpmjUnit.CO2VentValve.Close();
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }

    public class SeqHpmjAlarm : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        protected static _GenInfoHandler m_GenInfos;

        enum HPMJ_ALARM { INVERTER, LEAK, EMO, PUMP_MC, MAX_NO };
        private static bool InverterReset;
        private new int[] m_AlarmId;
        private Alarm m_AlarmRecipePara; // 09.12.08 minhan
        #endregion

        #region Contructor
        public SeqHpmjAlarm(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_SeqFunName = "HPMJ ALARM";

            InverterReset = false;
            m_AlarmRecipePara = new Alarm(m_HpmjUnit.Name + " : HPMJ Setup No-Use Status, Recipe Unmatch", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.12.08 minhan
            m_AlarmId = new int[(int)HPMJ_ALARM.MAX_NO];
            for (int i = 0; i < (int)HPMJ_ALARM.MAX_NO; i++)
                m_AlarmId[i] = 0;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            //if (!m_GenInfos.EqpInitComp) return -1;//Del by bkh //110126 초기화전에도 HPMJ는 Run할수 있는데 Inverter에 Alarm이 있어도 장비 Alarm 발생안하잖아!

            bool checkCond = true;
            checkCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            checkCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit) | !m_GenInfos.AutoMode;
            //checkCond &= !m_HpmjUnit.IfFlag.PowerCut;

            switch (seqNo)
            {
                case 0:
                    if (!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() &&
                        m_GenInfos.AutoMode && m_Server.JobCond.HpmjUse(m_HpmjUnit)) // setup 에는 no use 인데 래시피 상에서 use 일 경우 알람처리
                    {
                        m_HpmjUnit.IfFlag.Alarm = true;
                        m_EqpManager.SetAlarm(m_AlarmRecipePara.Id);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Recipe parameter Unmatch");
                        seqNo = 2000;
                    }
                    else
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (GetElapsedTicks() > 2500)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                        else if (!InverterReset)
                        {
                            InverterReset = true;
                            m_HpmjUnit.HpmjInverterResetOn();
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Reset On");
                        }
                        //else if(checkCond && !m_HpmjUnit.doPump_Cp_On.GetState())
                        //{
                        //    m_HpmjUnit.HpmjPumpMcOn();
                        //}
                    }
                    break;
                case 20:
                    if (checkCond && GetElapsedTicks() > 2000)
                    {
                        seqNo = 30;
                    }
                    else if (InverterReset)
                    {
                        InverterReset = false;
                        m_HpmjUnit.HpmjInverterResetOff();
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Reset Off");
                    }
                    else if (!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() &&
                        m_GenInfos.AutoMode && m_Server.JobCond.HpmjUse(m_HpmjUnit)) // setup 에는 no use 인데 래시피 상에서 use 일 경우 알람처리
                    {
                        m_HpmjUnit.IfFlag.Alarm = true;
                        m_EqpManager.SetAlarm(m_AlarmRecipePara.Id);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Recipe parameter Unmatch");
                        seqNo = 2000;
                    }
                    //else if (!checkCond)
                    //{
                    //    m_StartTicks = XFunc.GetTickCount();
                    //    seqNo = 10;
                    //}
                    break;
                case 30:
                    if (checkCond)
                    {
                        if (m_HpmjUnit.IsAlarm())
                        {
                            m_HpmjUnit.IfFlag.Alarm = true;
                            seqNo = 40;
                        }
                    }
                    else if (!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() &&
                              m_GenInfos.AutoMode && m_Server.JobCond.HpmjUse(m_HpmjUnit)) // setup 에는 no use 인데 래시피 상에서 use 일 경우 알람처리
                    {
                        m_HpmjUnit.IfFlag.Alarm = true;
                        m_EqpManager.SetAlarm(m_AlarmRecipePara.Id);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Recipe parameter Unmatch");
                        seqNo = 2000;
                    }
                    else if (m_HpmjUnit.IfFlag.Alarm)
                    {
                        m_HpmjUnit.IfFlag.Alarm = false;
                    }
                    break;
                case 40:
                    if (checkCond)
                    {
                        //bool check = false; // 09.12.28 minhan

                        bool check = m_HpmjUnit.CheckAlarm(m_SeqFunName, ref m_EqpManager,
                                                           false, ref m_AlarmId[(int)HPMJ_ALARM.INVERTER],
                                                           true, ref m_AlarmId[(int)HPMJ_ALARM.EMO],
                                                           true, ref m_AlarmId[(int)HPMJ_ALARM.PUMP_MC]);

                        //foreach (IoDigitalInput Di in m_HpmjUnit.ForAlarmDiList)
                        //{
                        //    if (Di.GetState())
                        //    {
                        //        if (Di.Id == m_HpmjUnit.diEMO.Id)
                        //        {
                        //            m_EqpManager.SetAlarm(m_HpmjUnit.ALM_EmoAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                        //            m_AlarmId[(int)HPMJ_ALARM.EMO] = m_HpmjUnit.ALM_EmoAlarm.Id;
                        //        }
                        //        if (Di.Id == m_HpmjUnit.diPump_Mc_Trip.Id)
                        //        {
                        //            m_EqpManager.SetAlarm(m_HpmjUnit.ALM_InverterMcTripAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");
                        //            m_AlarmId[(int)HPMJ_ALARM.PUMP_MC] = m_HpmjUnit.ALM_InverterMcTripAlarm.Id;
                        //        }
                        //        check = true;
                        //    }
                        //}

                        if (m_HpmjUnit.Inverter.IsAlarm()) // 09.12.28 minhan base 손대기 싫어서 여기로
                        {
                            m_EqpManager.SetAlarm(m_HpmjUnit.ALM_InverterAlarm.Id);
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                            m_AlarmId[(int)HPMJ_ALARM.INVERTER] = m_HpmjUnit.ALM_InverterAlarm.Id;
                            check = true;
                        }

                        if (check) // 09.12.28 minhan
                        {
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Alarm Check OK");
                            seqNo = 1000;
                        }
                        else
                        {
                            m_HpmjUnit.IfFlag.Alarm = false;
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Alarm Check NG");
                            seqNo = 30;
                        }

                        //m_StartTicks = XFunc.GetTickCount();
                        //seqNo = 1000; // 09.12.28 minhan
                    }
                    else if (m_HpmjUnit.IfFlag.Alarm)
                    {
                        m_HpmjUnit.IfFlag.Alarm = false;
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_HpmjUnit.HpmjInverterResetOn();
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Reset On");
                        InverterReset = true;

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 1010;
                    }
                    break;
                case 1010:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_HpmjUnit.HpmjInverterResetOff();
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Reset Off");
                        InverterReset = false;

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 1020;
                    }
                    break;
                case 1020:
                    if (GetElapsedTicks() > 500)
                    {
                        m_HpmjUnit.ResetAlarm(m_SeqFunName, ref m_EqpManager,
                                              false, ref m_AlarmId[(int)HPMJ_ALARM.INVERTER],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.EMO],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.PUMP_MC]);

                        //foreach (IoDigitalInput Di in m_HpmjUnit.ForAlarmDiList)
                        //{
                        //    if (!Di.GetState())
                        //    {

                        //        if (Di.Id == m_HpmjUnit.diEMO.Id && m_AlarmId[(int)HPMJ_ALARM.EMO] != 0)
                        //        {
                        //            m_EqpManager.ResetAlarm(m_HpmjUnit.ALM_EmoAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm Reset");
                        //            m_AlarmId[(int)HPMJ_ALARM.EMO] = 0;
                        //        }
                        //        if (Di.Id == m_HpmjUnit.diPump_Mc_Trip.Id && m_AlarmId[(int)HPMJ_ALARM.PUMP_MC] != 0)
                        //        {
                        //            m_EqpManager.ResetAlarm(m_HpmjUnit.ALM_InverterMcTripAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm Reset");
                        //            m_AlarmId[(int)HPMJ_ALARM.PUMP_MC] = 0;
                        //        }
                        //    }
                        //}

                        if (!m_HpmjUnit.Inverter.IsAlarm() && (m_AlarmId[(int)HPMJ_ALARM.INVERTER] != 0)) // 10.01.07 minhan
                        {
                            m_EqpManager.ResetAlarm(m_HpmjUnit.ALM_InverterAlarm.Id);
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm Reset");
                            m_AlarmId[(int)HPMJ_ALARM.INVERTER] = 0;
                        }

                        if (m_HpmjUnit.IsAlarm())
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else //if (!nowAlarmState)
                        {
                            m_HpmjUnit.IfFlag.Alarm = false;
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit All Digital Input Alarm Reset");

                            seqNo = 30;
                        }
                    }
                    break;
                case 2000: // 09.12.08 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed &&
                       ((!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() && m_Server.JobCond.HpmjUse(m_HpmjUnit)) == false))
                    {
                        m_HpmjUnit.IfFlag.Alarm = false;
                        m_EqpManager.ResetAlarm(m_AlarmRecipePara.Id);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Recipe parameter Unmatch");
                        seqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }

    public class SeqInitHpmj : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHpmjControl m_Control;
        protected static _GenericCollection<Hpmj> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        public Alarm ALM_InitFail = null;

        protected GenericTag m_InitCheckHpmjAlarm = new GenericTag("Hpmj Alarm", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitHpmj(ThreadHpmjControl control, _GenericCollection<Hpmj> units)
        {
            m_Units = units;
            m_Server = m_Units.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
            ALM_InitFail = new Alarm("HpmjUnit" + " : Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            if (m_Simul.Device) eqpHpmjs._HPMJ_Unit.SetEmo(false);
            //if (m_Simul.Device) eqpHpmjs._HPMJ_Unit.diEMO.SetState(false);
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        bool Use = false;
                        m_InitState = InitState.Init;
                        m_InitCheckHpmjAlarm.Value = InitCheckState.Checking;
                        m_Server.Log("SeqInitHpmj : Start");
                        foreach (Hpmj device in m_Units)
                        {
                            Use = m_Server.JobCond.HpmjUse(device);
                            //Use |= device.SetupHpmjUse.GetValue<bool>();
                        }

                        if (Use) seqNo = 10;
                        else seqNo = 30;
                    }
                    break;

                case 10:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    break;

                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        bool alarm = false;
                        foreach (Hpmj device in m_Units)
                        {
                            alarm |= device.CheckAlarm(m_SeqFunName, ref m_EqpManager, false, true, true);

                            //foreach (IoDigitalInput Di in device.ForAlarmDiList)
                            //{
                            //    if (Di.GetState())
                            //    {
                            //        //if (Di.Id == device.Inverter.DiAlarm.Id) // 09.12.28 minhan
                            //        //{
                            //        //    m_EqpManager.SetAlarm(device.ALM_InverterAlarm.Id);
                            //        //    device.SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                            //        //}
                            //        //else if(Di.Id == m_HpmjUnit.LeakSensor.DiSensor.Id)
                            //        //{
                            //        //    m_EqpManager.SetAlarm(m_HpmjUnit.ALM_LeakAlarm.Id);
                            //        //    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Leak Sensor Alarm");
                            //        //}
                            //        if (Di.Id == device.diEMO.Id)
                            //        {
                            //            m_EqpManager.SetAlarm(device.ALM_EmoAlarm.Id);
                            //            device.SetLog(SeqFunName, 0, 0, "HPMJ Unit EMO Alarm");
                            //        }
                            //        if (Di.Id == device.diPump_Mc_Trip.Id)
                            //        {
                            //            m_EqpManager.SetAlarm(device.ALM_InverterMcTripAlarm.Id);
                            //            device.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Mc Trip Alarm");

                            //        }
                            //        alarm = true; // 09.12.28 minhan
                            //    }
                            //}
                        }

                        if (eqpHpmjs._HPMJ_Unit.Inverter.IsAlarm()) // 09.12.28 minhan base 손대기 싫어서
                        {
                            m_EqpManager.SetAlarm(eqpHpmjs._HPMJ_Unit.ALM_InverterAlarm.Id);
                            eqpHpmjs._HPMJ_Unit.SetLog(m_SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                            alarm = true;
                        }

                        if (alarm)
                        {
                            m_Server.Log("HPMJ Unit Alarm");
                            m_InitState = InitState.Fail;
                            m_InitCheckHpmjAlarm.Value = InitCheckState.NG;
                            seqNo = 1000;
                        }
                        else seqNo = 30;

                    }
                    break;
                case 30:
                    {
                        m_Server.Log("HPMJ Unit Initialize OK");
                        m_InitState = InitState.Comp;
                        m_InitCheckHpmjAlarm.Value = InitCheckState.OK;

                        seqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        foreach (Hpmj device in m_Units)
                        {
                            device.HpmjInverterResetOn();
                            m_Server.Log("HPMJ Unit Reset On");
                        }
                        m_Server.Log("HPMJ Unit Alarm Recovery!");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 1010;
                    }
                    break;
                case 1010:
                    if (GetElapsedTicks() > 2000)
                    {
                        foreach (Hpmj device in m_Units)
                        {
                            device.HpmjInverterResetOff();
                            m_Server.Log("HPMJ Unit Reset Off");
                        }
                        //m_InitState = InitState.Noop;
                        seqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = seqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqPumpRun : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private GenInfoHandler m_GenInfo;//2009.09.21 kimgun
        private Simul m_Simul;

        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private double press = 0.0;
        private int PrevPressure = 0;//2009.09.17 kimgun
        #endregion

        #region Constructor
        public SeqPumpRun(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = ServerManager.Instance;//2009.09.17 kimgun //m_HpmjUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            m_Timer1 = new XTimer("PumpRun1");
            m_Timer2 = new XTimer("PumpRun2");

            m_SeqFunName = "HPMJ PUMP RUN";
            m_GenInfo = GenInfoHandler.Instance as GenInfoHandler;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            string log;
            bool simulation = m_Simul.Device;

            if (simulation == true)
            {
                if (m_HpmjUnit.IfFlag.PumpRun)
                {
                    press += 0.1;

                    if (press >= (m_HpmjUnit.IfFlag.ReferencePressure - 1.0) &&
                        press <= (m_HpmjUnit.IfFlag.ReferencePressure + 1.0))
                    {
                        press = m_HpmjUnit.IfFlag.ReferencePressure;
                    }

                    if (!m_HpmjUnit.IfFlag.Ready)
                        m_HpmjUnit.IfFlag.CurrentPressure = (int)press;
                }
                else
                {
                    m_HpmjUnit.IfFlag.CurrentPressure = 0;
                    press = 0.0;
                }
            }


            m_GenInfo.HpmjInHz = string.Format("{0:F2}", m_HpmjUnit.IfFlag.CurrentHertz);//2009.09.21 kimgun
            switch (seqNo)
            {
                case 0:
                    if (m_HpmjUnit.IfFlag.PumpRun)//&& RunCond)
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump Start.");

                        m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                        m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                        m_StartTicks = XFunc.GetTickCount();//2014.01.02 DuanXiaolong
                        seqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (GetElapsedTicks() < 3 * 1000)//2014.01.02 DuanXiaolong
                        {
                            break;
                        }
                        m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                        m_HpmjUnit.IfFlag.Ready = false;

                        if (GenInfoHandler.Instance.AutoMode)
                        {
                            if (GenInfoHandler.Instance.EQPGlassCount > 0)
                            {//2009.09.17 kimgun 
                                PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                            }
                            else
                            {
                                if (m_Server.SetupHpmjIdleUse.GetValue<bool>())//2009.09.17 kimgun user 협의 필요.일단 idle상태이고 idle 미사용이면 펌프 안 돌려.
                                    PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();
                                else PrevPressure = 0;
                            }
                            m_HpmjUnit.IfFlag.PressureSet = PrevPressure;// m_Server.JobCond.HpmjPressure(m_HpmjUnit);//2009.09.17 kimgun 왜 막았냐?

                            m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;

                            log = string.Format("Auto = Press Mode : Pressure = {0:0.00} ", m_HpmjUnit.IfFlag.ReferencePressure);
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                            seqNo = 20;
                        }
                        else //if(!GenInfoHandler.Instance.AutoMode)
                        {
                            if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                            {
                                m_HpmjUnit.IfFlag.PressureSet = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();
                                m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;

                                log = string.Format("Manual = Press Mode : Pressure = {0:0.00} ", m_HpmjUnit.IfFlag.ReferencePressure);
                                m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);

                                seqNo = 20;
                            }
                            else if (!m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                            {

                                m_HpmjUnit.IfFlag.CurrentHertz = m_HpmjUnit.SetupHpmjFrequency.GetValue<int>();

                                m_HpmjUnit.SetPumpHertz(m_HpmjUnit.IfFlag.CurrentHertz);
                                log = string.Format("Manual = Frequency Mode : Frequence = {0:0.00}", m_HpmjUnit.IfFlag.CurrentHertz);

                                m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);

                                //m_HpmjUnit.HpmjPumpMcOn();
                                m_HpmjUnit.HpmjInverterRun();
                                //m_HpmjUnit.CO2InValve.Open();

                                //m_HpmjUnit.Pump.Run();

                                m_Timer1.Start(10000);

                                seqNo = 100;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        m_HpmjUnit.IfFlag.CurrentHertz = 20;

                        log = string.Format("PreSetting Frequency Value : Frequency = {0:0.00}", m_HpmjUnit.IfFlag.CurrentHertz);
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);

                        m_HpmjUnit.IfFlag.CurrentAnalogOutput = m_HpmjUnit.ConvertHertzToAdc(m_HpmjUnit.IfFlag.CurrentHertz);
                        m_HpmjUnit.SetPumpAdc((ushort)m_HpmjUnit.IfFlag.CurrentAnalogOutput);

                        //m_HpmjUnit.HpmjPumpMcOn();
                        m_HpmjUnit.HpmjInverterRun();
                        //m_HpmjUnit.CO2InValve.Open();
                        //m_HpmjUnit.Pump.Run();

                        m_Timer1.Start(200);
                        m_Timer2.Start(45000);

                        seqNo = 50;
                    }
                    break;
                case 30:
                    {
                        m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                        m_HpmjUnit.IfFlag.Ready = false;

                        if (GenInfoHandler.Instance.AutoMode)
                        {
                            if (GenInfoHandler.Instance.EQPGlassCount > 0)
                            {//2009.09.17 kimgun 
                                PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                            }
                            else
                            {
                                if (m_Server.SetupHpmjIdleUse.GetValue<bool>())//2009.09.17 kimgun user 협의 필요.일단 idle상태이고 idle 미사용이면 펌프 안 돌려.
                                    PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();
                                else PrevPressure = 0;
                            }
                            m_HpmjUnit.IfFlag.PressureSet = PrevPressure;// m_Server.JobCond.HpmjPressure(m_HpmjUnit);//2009.09.17 kimgun 왜 막았냐?
                            m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;

                            log = string.Format("Auto Mode Recipe Change = Press Mode : Pressure = {0:0.00}", m_HpmjUnit.IfFlag.ReferencePressure);
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                        }
                        else if (!GenInfoHandler.Instance.AutoMode)
                        {
                            m_HpmjUnit.IfFlag.PressureSet = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();
                            m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;

                            log = string.Format("Manual Mode Recipe Change = Press Mode : Pressure = {0:0.00}", m_HpmjUnit.IfFlag.ReferencePressure);
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                        }

                        m_Timer1.Start(200);
                        m_Timer2.Start(45000);

                        seqNo = 50;
                    }
                    break;
                case 50:
                    if (m_HpmjUnit.IfFlag.PumpRun && m_Timer1.Over)
                    {
                        //2009.09.17 kimgun
                        if (GenInfoHandler.Instance.AutoMode)
                        {
                            if (GenInfoHandler.Instance.EQPGlassCount > 0)
                            {//2009.09.17 kimgun 
                                PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                            }
                            else
                            {
                                if (m_Server.SetupHpmjIdleUse.GetValue<bool>())//2009.09.17 kimgun user 협의 필요.일단 idle상태이고 idle 미사용이면 펌프 안 돌려.
                                    PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();
                                else PrevPressure = 0;
                            }
                            if (m_HpmjUnit.IfFlag.PressureSet != PrevPressure)
                                m_HpmjUnit.IfFlag.PPIDChangeRequest = true;
                        }
                        else
                        {
                            if (m_HpmjUnit.IfFlag.PressureSet != m_HpmjUnit.SetupHpmjPressure.GetValue<int>())
                                m_HpmjUnit.IfFlag.PPIDChangeRequest = true;
                            log = string.Format("Manual = Press Mode1 : Pressure = {0:0.00} // {0:0.00} ", m_HpmjUnit.IfFlag.PressureSet, m_HpmjUnit.SetupHpmjPressure.GetValue<int>());
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                        }
                        double err = SetPumpControl();

                        if (err >= 20.0)
                        {
                            m_Timer1.Start(200);
                        }
                        else
                        {
                            m_Timer1.Start(500);
                        }

                        if (simulation) // wzy test

                        {
                            m_HpmjUnit.IfFlag.Ready = true;
                        }

                        if (m_HpmjUnit.IfFlag.PPIDChangeRequest)
                        {
                            m_HpmjUnit.IfFlag.PPIDChangeRequest = false; // 09.12.20 minhan
                            m_HpmjUnit.IfFlag.Ready = false;
                            m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                            m_HpmjUnit.HpmjInverterStop();
                            log = string.Format("Auto Mode Press Val Change[200]");
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                            log = string.Format("Pump Stop");
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                            m_StartTicks = XFunc.GetTickCount();

                            if (simulation)
                            {
                                m_HpmjUnit.IfFlag.ReferencePressure = 0;
                                press = 0.0;

                                m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                                m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            }
                            seqNo = 210;
                        }
                    }
                    else if (!m_HpmjUnit.IfFlag.PumpRun)
                    {
                        seqNo = 200;
                    }
                    else if ((!m_HpmjUnit.IfFlag.Ready && m_Timer2.Over) || (m_HpmjUnit.IfFlag.CurrentAnalogOutput < 0))
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump PID Control Error.");

                        m_AlarmId = m_HpmjUnit.ALM_PidControlAlarm.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);

                        m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                        m_HpmjUnit.IfFlag.CurrentHertz = 0.0;

                        //m_HpmjUnit.Pump.Stop();
                        m_HpmjUnit.HpmjInverterStop();
                        //m_HpmjUnit.CO2InValve.Close();

                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Pump Stop.");

                        m_HpmjUnit.IfFlag.Ready = false;
                        m_HpmjUnit.IfFlag.PIDError = true;

                        if (simulation)
                        {
                            m_HpmjUnit.IfFlag.CurrentPressure = 0.0;
                            press = 0.0;
                        }
                        seqNo = 1000;
                    }
                    break;
                case 100:
                    if (m_HpmjUnit.IfFlag.PumpRun)
                    {
                        m_HpmjUnit.IfFlag.CurrentHertz = m_HpmjUnit.GetPumpHertz();

                        if (m_Timer1.Over)
                        {
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump is Ready.");
                            m_HpmjUnit.IfFlag.Ready = true;
                        }

                        if (m_HpmjUnit.IfFlag.PPIDChangeRequest)
                        {

                            m_HpmjUnit.IfFlag.PPIDChangeRequest = false; // 09.12.20 minhan
                            m_HpmjUnit.IfFlag.Ready = false;
                            m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                            m_HpmjUnit.HpmjInverterStop();
                            log = string.Format("Auto Mode Press Val Change[200]");
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                            log = string.Format("Pump Stop");
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
                            m_StartTicks = XFunc.GetTickCount();

                            if (simulation)
                            {
                                m_HpmjUnit.IfFlag.ReferencePressure = 0;
                                press = 0.0;

                                m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                                m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            }
                            seqNo = 210;
                        }
                    }
                    else if (!m_HpmjUnit.IfFlag.PumpRun)
                    {
                        seqNo = 200;
                    }
                    break;
                case 200:

                    m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                    //m_HpmjUnit.Pump.Stop();
                    m_HpmjUnit.HpmjInverterStop();
                    //m_HpmjUnit.CO2InValve.Close();

                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump Stop.");
                    m_HpmjUnit.IfFlag.Ready = false;

                    if (GenInfoHandler.Instance.AutoMode)
                    {
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "Auto Mode Pump Stop.");
                    }
                    else
                    {
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "Manual Mode Pump Stop.");
                    }

                    seqNo = 0;

                    break;
                case 210: // 09.12.20 minhan
                    if (GetElapsedTicks() > 7000)
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Auto Mode Press Val Change[210]");
                        seqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_HpmjUnit.IfFlag.PIDError = false;
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Error Recovery request");

                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion

        #region Methods
        private double SetPumpControl()
        {
            double Error = 0.0;
            int Property = 0;
            bool simulation = m_Simul.Device;

            if (simulation == false)
            {
                m_HpmjUnit.IfFlag.CurrentPressure = m_HpmjUnit.FilterOutPress.CurValue;
            }
            else
            {
                m_HpmjUnit.FilterOutPress.CurValue = m_HpmjUnit.IfFlag.CurrentPressure;
            }

            m_HpmjUnit.IfFlag.ErrorPressure = m_HpmjUnit.IfFlag.ReferencePressure - m_HpmjUnit.IfFlag.CurrentPressure;
            Error = Math.Abs(m_HpmjUnit.IfFlag.ErrorPressure);

            if ((Error <= 1.0 && !simulation) || (Error <= 5.0 && simulation))
            {
                m_HpmjUnit.IfFlag.ErrorPressure = 0.0;
                m_HpmjUnit.IfFlag.Ready = true;

                if (m_HpmjUnit.IfFlag.ReferencePressure != m_HpmjUnit.IfFlag.PressureSet)
                {
                    m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet;
                    string log = string.Format("Reference Pressure is changed to {0:0.00}", m_HpmjUnit.IfFlag.ReferencePressure);
                    m_HpmjUnit.HpmjLog.TextOut(log);
                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump is Ready.");
                }
            }

            if (!m_HpmjUnit.IfFlag.Ready)
            {
                if (m_HpmjUnit.IfFlag.CurrentPressure < 5) Property = 4;
                else if (Error > 100) Property = 80;
                else if (Error > 90) Property = 25;
                else if (Error > 50) Property = 70;
                else if (Error > 40) Property = 25;
                else if (Error > 30) Property = 70;
                else if (Error > 20) Property = 50;
                else if (Error > 10) Property = 35;
                else if (Error > 5) Property = 25;
                else if (Error > 3) Property = 14;
                else if (Error > 1) Property = 8;
                else Property = 8;
            }
            else
            {
                if (Error > 50) Property = 18;
                else if (Error > 30) Property = 10;
                else if (Error > 3) Property = 5;
                else if (Error > 1) Property = 2;
                else Property = 1;
            }

            if (m_HpmjUnit.IfFlag.ErrorPressure != 0.0)
            {
                string log = string.Format("Property : {0}\tCurrent Pressure : {1:0.00}", Property, m_HpmjUnit.IfFlag.CurrentPressure);
                m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);

                if (m_HpmjUnit.IfFlag.ErrorPressure > 0)
                {
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput += Property;
                }
                else
                {
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput -= Property;
                }

                if (m_HpmjUnit.IfFlag.CurrentAnalogOutput > 6000)
                {
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 6000;
                }
                else if (m_HpmjUnit.IfFlag.CurrentAnalogOutput < 0x0000)
                {
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0x0000;
                }

                m_HpmjUnit.SetPumpAdc((ushort)m_HpmjUnit.IfFlag.CurrentAnalogOutput);
                m_HpmjUnit.IfFlag.CurrentHertz = m_HpmjUnit.GetPumpHertz();

                log = string.Format("ADC : {0}\tHertz : {1:00.00}", m_HpmjUnit.IfFlag.CurrentAnalogOutput, m_HpmjUnit.IfFlag.CurrentHertz);
                m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);
            }

            return Error;
        }
        #endregion
    }

    public class SeqStateMonitor : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private Simul m_simul;//2009.09.14 kimgun
        private GenInfoHandler m_GenInfo;//090923 LeeChungWon
        #endregion

        #region Constructor
        public SeqStateMonitor(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = ServerManager.Instance;//m_HpmjUnit.ServerManager;
            m_simul = AppConfig.Instance.Simul;//2009.09.14 kimgun
            m_GenInfo = GenInfoHandler.Instance as GenInfoHandler; //090923 LeeChungWon
            m_Control = control;

            m_SeqFunName = "HPMJ STATE";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            bool GaugeAlarm = false;
            bool RunCond = true;

            GaugeAlarm |= m_HpmjUnit.CO2InPress.IsAlarm;
            GaugeAlarm |= m_HpmjUnit.MainDiPress.IsAlarm;
            GaugeAlarm |= m_HpmjUnit.FilterInPress.IsAlarm;
            GaugeAlarm |= m_HpmjUnit.FilterOutPress.IsAlarm;
            GaugeAlarm |= m_HpmjUnit.HpmjFlow.IsAlarm;

            if (GenInfoHandler.Instance.AutoMode)
            {
                if (GenInfoHandler.Instance.EQPGlassCount > 0)
                    m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", m_Server.JobCond.HpmjPressure(m_HpmjUnit));//090923 : LeeChungWon
                else
                {
                    if (m_Server.SetupHpmjIdleUse.GetValue<bool>())//2009.09.17 kimgun user 협의 필요.일단 idle상태이고 idle 미사용이면 펌프 안 돌려.
                        m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", m_Server.SetupIdleHpmjPress.GetValue<int>());
                    else m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
                }
            }
            else
            {
                if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                {
                    if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>()) m_GenInfo.HpmjTargetPress = string.Format("{0:F2}", m_HpmjUnit.SetupHpmjPressure.GetValue<int>());
                    else m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
                }
                else m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
            }

            if (GaugeAlarm)
                m_HpmjUnit.IfFlag.GaugeInterlockAlarm = true;
            else
                m_HpmjUnit.IfFlag.GaugeInterlockAlarm = false;

            if (!GenInfoHandler.Instance.AutoMode)
            {
                if (!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() ||
                    m_HpmjUnit.IfFlag.Alarm ||
                    m_HpmjUnit.IfFlag.GaugeInterlockAlarm ||
                    m_HpmjUnit.IfFlag.PIDError ||
                    GlobalVar.HpmjPumpInterlock ||
                    (m_HpmjUnit.MainDiPress.CurValue <= (double)m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<float>() && !m_simul.Device))//2009.09.21 kimgun
                {
                    m_HpmjUnit.IfFlag.PumpRun = false;
                    return -1; // 11.01.03 minhan 이럴 경우는 그냥 리턴해 버려야하는 것 아닐런지.
                }
            }

            //if (!GenInfoHandler.Instance.EqpInitComp) return -1;//Del by bkh //110126 초기화전에도 HPMJ는 Run할수 있는데 

            bool alarm2 = false;
            alarm2 |= m_HpmjUnit.OwnerUnit.IfFlag.InError;
            alarm2 |= m_HpmjUnit.OwnerUnit.IfFlag.OutError;
            alarm2 |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.InError;
            alarm2 |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.OutError;
            //alarm2 |= (m_GenInfo.AutoMode && GlobalVar.AkCvStop) ; // 11.01.03 minhan ak 구간 정체시 stop 단 auto모드에서만.
            //alarm2 |= (m_GenInfo.AutoMode && GlobalVar.Swr3CvStop) ; // 11.01.03 minhan swr3 구간 정체시 stop 2013.05.23 long

            if (GenInfoHandler.Instance.AutoMode)//110126 bkh
            {
                RunCond &= !IsInterlock(m_HpmjUnit);
                RunCond &= !m_HpmjUnit.IfFlag.Alarm;
                RunCond &= !m_HpmjUnit.IfFlag.GaugeInterlockAlarm;
                RunCond &= !m_HpmjUnit.IfFlag.PIDError;
                RunCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
                RunCond &= GenInfoHandler.Instance.EqpInitComp;
                RunCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit) | GenInfoHandler.Instance.EQPGlassCount == 0;//2009.09.17 kimgun;
                RunCond &= m_Server.JobCond.ProcessMode;
                RunCond &= (m_HpmjUnit.MainDiPress.CurValue > (double)m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<float>()) || m_simul.Device;
                RunCond &= !alarm2;//2009.09.15 kimgun
                RunCond &= (m_Server.SetupHpmjIdleUse.GetValue<bool>() && GenInfoHandler.Instance.IdleRunning && GenInfoHandler.Instance.DiStart) | GenInfoHandler.Instance.EQPGlassCount > 0;//2009.09.17 kimgun 2013.03.01 long
                RunCond &= (GenInfoHandler.Instance.EQPGlassCount > 0 || (m_GenInfo.DiStart && m_GenInfo.IdleRunning && GenInfoHandler.Instance.EQPGlassCount == 0));
                RunCond &= !GlobalVar.HpmjPumpInterlock;//2009.09.21 kimgun
                RunCond &= !GlobalVar.Powercut;//2009.09.21 kimgun
                RunCond &= !GlobalVar.EqpRunSwrDiStop;// 13.01.21 wzy
            }
            else//↓110126 bkh
            {
                RunCond &= !IsInterlock(m_HpmjUnit);
                RunCond &= !m_HpmjUnit.IfFlag.Alarm;
                RunCond &= !m_HpmjUnit.IfFlag.PIDError;
                RunCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
                //RunCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit);//2009.09.17 kimgun;
                RunCond &= m_Server.JobCond.ProcessMode;
                RunCond &= (m_HpmjUnit.MainDiPress.CurValue > (double)m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<float>()) || m_simul.Device;
                RunCond &= !GlobalVar.HpmjPumpInterlock;//2009.09.21 kimgun
                RunCond &= !GlobalVar.Powercut;//2009.09.21 kimgun
                RunCond &= m_HpmjUnit.IfFlag.PumpRun;
            }//↑110126 bkh

            switch (seqNo)
            {
                case 0:
                    if (RunCond /*&& !m_HpmjUnit.IfFlag.PumpRun*/) // 09.12.01 minhan
                    {
                        m_HpmjUnit.IfFlag.PumpRun = true;
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set True");
                        seqNo = 10;
                    }

                    break;
                case 10:
                    if (!RunCond)//2009.09.14 kimgun
                    {
                        if (m_HpmjUnit.IfFlag.PumpRun)
                        {
                            m_HpmjUnit.IfFlag.PumpRun = false;
                            m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set False");
                        }
                        seqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion

        #region Methods
        public bool IsInterlock(Hpmj hpmj)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy & ~HeavyInterlock.Door) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Emo) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Leak) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Cover) > 0);

            return interlock;
        }
        #endregion
    }

    public class SeqHpmjAi : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        #endregion

        #region Constructor
        public SeqHpmjAi(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
            m_Control = control;

            m_SeqFunName = "HPMJ AI";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if (m_HpmjUnit.IfFlag.Ready)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 20000)   // 2000 -> 20000, log 는 굳이 2초마다 안해도 log 파일 커짐
                    {
                        double Ai1, Ai2, Ai3, Ai4, Ai5, Ai6; // 09.12.28 minhan

                        Ai1 = m_HpmjUnit.HpmjFlow.CurValue;
                        Ai2 = m_HpmjUnit.FilterInPress.CurValue;
                        Ai3 = m_HpmjUnit.FilterOutPress.CurValue;
                        Ai4 = m_HpmjUnit.CO2InPress.CurValue;
                        Ai5 = m_HpmjUnit.MainDiPress.CurValue;
                        //Ai6 = m_HpmjUnit.InvertCurrent.CurValue; // 09.12.28 minhan
                        Ai6 = m_HpmjUnit.Resistivity.CurValue;// 2013.01.31 long

                        //string log = string.Format("Shower Flow : {0:0.00} \t Filter In Pressure : {1:0.00} \t Filter Out Pressure : {2:0.00} \t CO2 In Pressure : {3:0.00} \t Main Di In Pressure : {4:0.00}",
                        //                    Ai1, Ai2, Ai3, Ai4, Ai5);
                        string log = string.Format("Shower Flow : {0:0.00} \t Filter In Pressure : {1:0.00} \t Filter Out Pressure : {2:0.00} \t CO2 In Pressure : {3:0.00} \t Main Di In Pressure : {4:0.00} \t CO2 Resistivity : {5:0.00}",//2013.01.31
                                            Ai1, Ai2, Ai3, Ai4, Ai5, Ai6); // 09.12.28 minhan
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, log);

                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion
    }

    public class SeqHpmjPumpInterlock : XSeqFunction
    {//2009.09.21 kimgun
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private Alarm ALM_PumpInterlock;
        private string m_Msg;
        private Simul m_Simul;
        #endregion

        #region Constructor
        public SeqHpmjPumpInterlock(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = ServerManager.Instance;
            m_Control = control;
            m_Simul = AppConfig.Instance.Simul;
            ALM_PumpInterlock = new Alarm("HPMJ Unit Pump Interlock", AlarmLevel.S, AlarmCode.EquipmentSafety); // 10.01.13 minhan
            m_SeqFunName = "HPMJ Pump Interlock";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            int InterlockTime = m_Server.SetupPumpIntTime.GetValue<int>() * 1000;
            int PumpPrePressInterlock = m_Server.SetupPumpIntPrePress.GetValue<int>();
            int PumpPostPressInterlock = m_Server.SetupPumpIntPostPress.GetValue<int>();
            int PumpFlowInterlock = m_Server.SetupPumpIntFlow.GetValue<int>();
            switch (seqNo)
            {
                case 0:
                    if (m_HpmjUnit.IfFlag.PumpRun)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_HpmjUnit.IfFlag.PumpRun) seqNo = 0;
                    else if (m_HpmjUnit.HpmjFlow.CurValue > PumpFlowInterlock &&
                        m_HpmjUnit.FilterInPress.CurValue > PumpPrePressInterlock &&
                        m_HpmjUnit.FilterOutPress.CurValue > PumpPostPressInterlock)
                    {
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "Pump Interlock:OK");
                        seqNo = 20;
                    }
                    else if (GetElapsedTicks() > InterlockTime && !m_Simul.Device)
                    {
                        m_AlarmId = ALM_PumpInterlock.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        if (m_HpmjUnit.HpmjFlow.CurValue < PumpFlowInterlock)
                            m_Msg = string.Format("CurFlow:{0} < IntFlow:{1}", m_HpmjUnit.HpmjFlow.CurValue, PumpFlowInterlock);
                        if (m_HpmjUnit.FilterInPress.CurValue < PumpPrePressInterlock)
                            m_Msg += string.Format(" CurPrePress:{0} < IntPress:{1}", m_HpmjUnit.FilterInPress.CurValue, PumpPrePressInterlock);
                        if (m_HpmjUnit.FilterOutPress.CurValue < PumpPostPressInterlock)
                            m_Msg += string.Format(" CurPostPress:{0} < IntPress:{1}", m_HpmjUnit.FilterOutPress.CurValue, PumpPostPressInterlock);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        GlobalVar.HpmjPumpInterlock = true;
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "Pump Interlock:NG");
                        seqNo = 1000;
                    }
                    break;
                case 20:
                    if (!m_HpmjUnit.IfFlag.PumpRun) seqNo = 0;
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        GlobalVar.HpmjPumpInterlock = false;
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion
    }

    public class SeqPowerCut : XSeqFunction
    {//2009.09.21 kimgun
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private string m_Msg;
        #endregion

        #region Constructor
        public SeqPowerCut(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = ServerManager.Instance;
            m_Control = control;
            m_SeqFunName = "HPMJ PowerCut";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;
            int seqNo = this.m_SeqNo;

            bool bPowerCutCond = true;
            int nPercentage = 0;

            if (GenInfoHandler.Instance.EQPGlassCount > 0)
                nPercentage = (int)(m_Server.JobCond.HpmjPressure(m_HpmjUnit) * 0.7); //091104 LeeChungWon
            else if (GenInfoHandler.Instance.EQPGlassCount == 0)
                nPercentage = (int)(m_Server.SetupIdleHpmjPress.GetValue<int>() * 0.7); //091104 LeeChungWon

            double dCurPress = m_HpmjUnit.FilterOutPress.CurValue;//091104 LeeChungWon

            bPowerCutCond &= GenInfoHandler.Instance.AutoMode;
            bPowerCutCond &= !m_Control.IsInterlock(m_HpmjUnit);
            bPowerCutCond &= m_HpmjUnit.IfFlag.PumpRun;
            bPowerCutCond &= m_HpmjUnit.IfFlag.Ready; //091104 LeeChungWon
            bPowerCutCond &= !m_HpmjUnit.IsAlarm();
            bPowerCutCond &= !m_HpmjUnit.IfFlag.GaugeInterlockAlarm;
            switch (seqNo)
            {
                case 0:
                    //if (bPowerCutCond && (nPercentage > dCurPress))//m_HpmjUnit.Inverter.DiPowerCut.GetState())////여기에 I/O를 집어넣으면 된다..//091104 LeeChungWon
                    if (bPowerCutCond && (nPercentage > dCurPress) && !AppConfig.Instance.Simul.Device) // 09.11.17 minhan                    
                    {
                        GlobalVar.Powercut = true;
                        m_Msg = string.Format("순간 정전 발생.");
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 1000 && (dCurPress < 5)) //!m_HpmjUnit.Inverter.DiPowerCut.GetState()) //091104 LeeChungWon
                    {
                        GlobalVar.Powercut = false;
                        m_Msg = string.Format("Powercut False");
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion
    }

    class SeqHpmjThreadCheck : XSeqFunction
    {
        #region Fields
        private string m_Oldmsg = "";
        private int Threadcount = 0;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public SeqHpmjThreadCheck(ThreadHpmjControl control)
        {
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            try
            {
                switch (nSeqNo)
                {
                    case 0:
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        break;
                    case 10:
                        {
                            if (GetElapsedTicks() > 1000)
                            {
                                //GlobalVar.SeqHpmjCount = Threadcount;
                                Threadcount = 0;
                                nSeqNo = 0;
                            }
                            else
                            {
                                Threadcount += 1;
                            }
                        }
                        break;
                }
            }
            catch (Exception err)
            {
                string msg = err.ToString();

                if (m_Oldmsg != msg)
                {
                    m_Oldmsg = msg;
                    XFunc.ExceptionHandler.Add(err);
                }
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    class SeqHpmjIdleRunning : XSeqFunction  //11.06.13 mang
    {
        #region Fields

        protected static ServerManager m_Server;
        protected _GenInfoHandler m_GenInfos;
        protected int m_OldTime = 0;
        protected XTimer m_Timer;
        private Hpmj m_HpmjUnit;
        private int m_OldIdleRunTime = 0;
        private int m_OldIdleStopTime = 0;
        private bool m_OldHpmjStatus = false;
        private string m_Msg;
        protected static ThreadHpmjControl m_Control;

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public SeqHpmjIdleRunning(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_Control = control;
            m_Server = ServerManager.Instance;
            m_GenInfos = GenInfoHandler.Instance;
            m_Timer = new XTimer("Timer : SeqIdleRunning");
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            int RunTime = (int)m_Server.JobCond.SetupHpmjRunTime;
            int StopTime = (int)m_Server.JobCond.SetupHpmjStopTime;
            int nTime = 0;

            bool IdleCond = true;
            IdleCond &= m_GenInfos.EqpInitComp;
            IdleCond &= m_GenInfos.AutoMode;
            IdleCond &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            IdleCond &= (m_Server.EqpStateManager.EqpUnit.EqpState != EqpState.Fault);
            IdleCond &= m_Server.JobCond.ProcessMode;
            IdleCond &= m_Server.SetupHpmjIdleUse.GetValue<bool>();
            IdleCond &= (RunTime > 0);
            IdleCond &= !m_GenInfos.CycleStop;
            IdleCond &= m_GenInfos.IdleRunning;

            int nSeqNo = this.m_SeqNo;


            switch (nSeqNo)
            {
                case 0:
                    if (IdleCond)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = (StopTime == 0);
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!IdleCond)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = false;
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_OldTime = 0;
                        m_OldIdleRunTime = (int)m_Server.JobCond.SetupHpmjRunTime;
                        m_OldIdleStopTime = (int)m_Server.JobCond.SetupHpmjStopTime;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        nTime = ((int)GetElapsedTicks() / 60000) + m_OldTime;

                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            m_StartTicks = XFunc.GetTickCount();
                        }

                        if (StopTime != m_OldIdleStopTime)
                        {
                            m_OldIdleStopTime = StopTime;
                        }

                        if (!IdleCond)
                        {
                            if (!m_GenInfos.AutoMode)
                            {
                                m_OldHpmjStatus = m_HpmjUnit.IfFlag.PumpRun;
                                m_HpmjUnit.IfFlag.PumpRun = false;
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 0;
                                break;
                            }
                            else if (GenInfoHandler.Instance.EQPGlassCount == 0)
                            {
                                m_HpmjUnit.IfFlag.PumpRun = false;
                                nSeqNo = 0;
                            }
                            else
                            {
                                nSeqNo = 0;
                            }
                        }
                        else if (nTime > StopTime)
                        {
                            m_HpmjUnit.IfFlag.PumpRun = true;
                            m_Msg = string.Format("HPMJ Pump Idle Run");
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            m_StartTicks = XFunc.GetTickCount();
                            m_OldTime = 0;
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        nTime = ((int)GetElapsedTicks() / 60000) + m_OldTime;

                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            m_StartTicks = XFunc.GetTickCount();
                        }

                        if (RunTime != m_OldIdleRunTime)
                        {
                            m_OldIdleRunTime = RunTime;
                        }

                        if (!IdleCond)
                        {
                            if (!m_GenInfos.AutoMode)
                            {
                                m_OldHpmjStatus = m_HpmjUnit.IfFlag.PumpRun;
                                m_HpmjUnit.IfFlag.PumpRun = false;
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 0;
                                break;
                            }
                            else if (GenInfoHandler.Instance.EQPGlassCount == 0)
                            {
                                m_HpmjUnit.IfFlag.PumpRun = false;
                                nSeqNo = 0;
                            }
                            else
                            {
                                nSeqNo = 0;
                            }
                        }
                        else if (nTime > RunTime)
                        {
                            m_HpmjUnit.IfFlag.PumpRun = (StopTime == 0);
                            m_Msg = string.Format("HPMJ Pump Idle Stop.");
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            m_OldTime = 0;
                            nSeqNo = 10;
                        }
                    }
                    break;

            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }

        #endregion
    }

    public class SeqInitCo2VentValve : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private static bool InitVentValve;
        #endregion

        #region Constructor
        public SeqInitCo2VentValve(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
            m_Control = control;
            if (AppConfig.Instance.Simul.Device) m_HpmjUnit.IfFlag.DiLack = true;
            else m_HpmjUnit.IfFlag.DiLack = false;
            m_SeqFunName = "HPMJ VALVE2";
            InitVentValve = false;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            // 20121019 ssm HPMJ run before vent valve 10sec Open
            switch (seqNo)
            {
                case 0:
                    if (m_HpmjUnit.CO2VentValve.IsClose() && !InitVentValve)
                    {
                        InitVentValve = true;
                        m_HpmjUnit.CO2VentValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Co2 Vent valve Init Open");
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > (10 * 1000))
                    {
                        m_HpmjUnit.CO2VentValve.Close();
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "HPMJ Co2 Vent valve Init Close");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        seqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }
}

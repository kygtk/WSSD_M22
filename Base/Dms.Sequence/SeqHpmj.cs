using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadHpmjControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<Hpmj> m_HpmjUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties

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
                RegisterSequence(new SeqHpmjGaugeInterlock(this, device));
            }

            m_Server.AddSeqInitFunction(new SeqInitHpmj(this, m_HpmjUnits));
        }
        #endregion

        #region Constructor
        public ThreadHpmjControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_HpmjUnits = DmsComponents.Instance.ComponentContainer.GetCollection<Hpmj>();
            m_GenInfos = GenInfoHandler.Instance;

            RegisterSequences();
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

        #region Static Methods
        public static _GenericCollection<Hpmj> Units
        {
            get
            {
                if (m_HpmjUnits == null) m_HpmjUnits = new _GenericCollection<Hpmj>();
                return m_HpmjUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual void CheckStopCountCond()
        {
        }

        public virtual bool IsAlarmCondition(Hpmj hpmj)
        {
            bool ng = false;

            return ng;
        }

        public virtual bool IsInterlock(Hpmj hpmj)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy & ~HeavyInterlock.Door) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Emo) > 0);
            interlock |= ((heavy & ~HeavyInterlock.Leak) > 0);

            return interlock;
        }

        public virtual bool IsRunEnable(Hpmj hpmj)
        {
            bool run = true;
            run &= m_Server.JobCond.ProcessMode;
            run &= !IsAlarmCondition(hpmj);

            return run;
        }

        public virtual bool IsAutoRunCondition(Hpmj hpmj)
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

            HpmjUnit.IfFlag.Remote = true;

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
        private enum HpmjGauge { GAUGE_CO2, GAUGE_DI, GAUGE_RESIST, GAUGE_PRESS_IN, GAUGE_PRESS_OUT, GAUGE_FLOW, GAUGE_MAX_NO, }
        private enum IntrState { LowAlarm, UpAlarm, LowWarning, UpWarning, Noop, }

        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static _GenInfoHandler m_GenInfos;
        protected static ThreadHpmjControl m_Control;
        protected static Simul m_Simul;

        private Hpmj m_HpmjUnit;

        protected static _GenericCollection<Gauge> m_Gauges;
        static private new int[] m_AlarmId;
        private int[] m_SeqNos;
        private static int m_GaugeNo = 0;

        private double m_DelayTime = 2000;
        private IntrState m_IntrState = IntrState.Noop;
        private string m_Msg = "";
        #endregion

        #region Constructor
        public SeqHpmjGaugeInterlock(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_Server = m_HpmjUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_SeqFunName = "HPMJ GAUGE";

            m_Gauges = DmsComponents.Instance.ComponentContainer.GetCollection<Gauge>();
            m_AlarmId = new int[(int)HpmjGauge.GAUGE_MAX_NO];
            m_SeqNos = new int[(int)HpmjGauge.GAUGE_MAX_NO];
            for (int i = 0; i < (int)HpmjGauge.GAUGE_MAX_NO; i++)
            {
                m_AlarmId[i] = 0;
                m_SeqNos[i] = 0;
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //int seqNo = this.SeqNo;			
            int seqNo = m_SeqNos[m_GaugeNo];
            bool simulation = m_Simul.Device;
            Gauge gauge = GetItem(m_GaugeNo);

            bool checkCond = true;
            checkCond &= GetCheckCond(m_GaugeNo);
            checkCond &= gauge.SetupInterlock.Use;

            switch (seqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        m_SeqNo = 10;
                    }
                    else
                    {
                        if (m_AlarmId[m_GaugeNo] > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                            m_AlarmId[m_GaugeNo] = 0;
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_HpmjUnit.IfFlag.GaugeInterlockAlarm)
                        {
                            m_HpmjUnit.IfFlag.GaugeInterlockAlarm = false;
                        }
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > m_DelayTime)
                    {
                        m_SeqNo = 20;
                    }
                    break;
                case 20:
                    if (!checkCond)
                    {
                        m_SeqNo = 0;
                    }
                    else
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal;

                        double lowAlarm = gauge.SetupInterlock.LowAlarm;
                        double upAlarm = gauge.SetupInterlock.HighAlarm;
                        double lowWarning = gauge.SetupInterlock.LowWarning;
                        double upWarning = gauge.SetupInterlock.HighWarning;

                        switch (m_GaugeNo)
                        {
                            case (int)HpmjGauge.GAUGE_FLOW:
                                {
                                    lowAlarm = lowAlarm * -1;
                                    lowWarning = lowWarning * -1;
                                    curVal = gauge.CurValue - GetStandardFlowrate();
                                }
                                break;
                            case (int)HpmjGauge.GAUGE_PRESS_IN:
                            case (int)HpmjGauge.GAUGE_PRESS_OUT:
                                {
                                    lowAlarm = lowAlarm * -1;
                                    lowWarning = lowWarning * -1;
                                    curVal = gauge.CurValue - m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                                }
                                break;
                            default:
                                curVal = gauge.CurValue;
                                break;
                        }

                        if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                        else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                        else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                        else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }

                        if (m_IntrState != IntrState.Noop)
                        {
                            m_Msg = string.Format("GAUGE:{0} is Interlock Condition", m_GaugeNo);
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                            m_StartTicks = XFunc.GetTickCount();
                            m_SeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (!checkCond)
                    {
                        m_SeqNo = 0;
                    }
                    else
                    {
                        m_IntrState = IntrState.Noop;

                        double curVal;

                        double lowAlarm = gauge.SetupInterlock.LowAlarm;
                        double upAlarm = gauge.SetupInterlock.HighAlarm;
                        double lowWarning = gauge.SetupInterlock.LowWarning;
                        double upWarning = gauge.SetupInterlock.HighWarning;

                        switch (m_GaugeNo)
                        {
                            case (int)HpmjGauge.GAUGE_FLOW:
                                {
                                    lowAlarm = lowAlarm * -1;
                                    lowWarning = lowWarning * -1;
                                    curVal = gauge.CurValue - GetStandardFlowrate();
                                }
                                break;
                            case (int)HpmjGauge.GAUGE_PRESS_IN:
                            case (int)HpmjGauge.GAUGE_PRESS_OUT:
                                {
                                    lowAlarm = lowAlarm * -1;
                                    lowWarning = lowWarning * -1;
                                    curVal = gauge.CurValue - m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                                }
                                break;
                            default:
                                curVal = gauge.CurValue;
                                break;
                        }

                        if (curVal < lowAlarm) { m_IntrState = IntrState.LowAlarm; }
                        else if (curVal > upAlarm) { m_IntrState = IntrState.UpAlarm; }
                        else if (curVal < lowWarning) { m_IntrState = IntrState.LowWarning; }
                        else if (curVal > upWarning) { m_IntrState = IntrState.UpWarning; }

                        if (GetElapsedTicks() > m_DelayTime)
                        {
                            if (m_IntrState == IntrState.LowAlarm)
                            {
                                {
                                    if (m_AlarmId[m_GaugeNo] > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId[m_GaugeNo] = gauge.ALM_LowerAlarm.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[m_GaugeNo]);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                                    seqNo = 1000;
                                }
                            }
                            else if (m_IntrState == IntrState.UpAlarm)
                            {
                                {
                                    if (m_AlarmId[m_GaugeNo] > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId[m_GaugeNo] = gauge.ALM_UpperAlarm.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[m_GaugeNo]);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                                    seqNo = 1000;
                                }
                            }
                            else if (m_IntrState == IntrState.LowWarning)
                            {
                                if (m_AlarmId[m_GaugeNo] != gauge.ALM_LowerWarning.Id)
                                {
                                    if (m_AlarmId[m_GaugeNo] > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId[m_GaugeNo] = gauge.ALM_LowerWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[m_GaugeNo]);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                }
                            }
                            else if (m_IntrState == IntrState.UpWarning)
                            {
                                if (m_AlarmId[m_GaugeNo] != gauge.ALM_UpperWarning.Id)
                                {
                                    if (m_AlarmId[m_GaugeNo] > 0)
                                    {
                                        m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    }

                                    m_AlarmId[m_GaugeNo] = gauge.ALM_UpperWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[m_GaugeNo]);
                                    m_Msg = string.Format("{0:F2} ({1}, {2}, {3}, {4})", curVal, lowAlarm, lowWarning, upWarning, upAlarm);
                                    m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                }
                            }
                        }

                        if (m_IntrState == IntrState.Noop)
                        {
                            if (m_AlarmId[m_GaugeNo] > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId[m_GaugeNo]);
                                m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                                m_AlarmId[m_GaugeNo] = 0;
                            }

                            m_Msg = string.Format("GAUGE:{0} is not Interlock Condition", m_GaugeNo);
                            m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            seqNo = 0;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Msg = string.Format("Alarm Reset : Alarm ID[{0}]", m_AlarmId[m_GaugeNo]);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        m_StartTicks = XFunc.GetTickCount();
                        m_SeqNo = 0;
                    }
                    break;
            }

            //this.SeqNo = seqNo;
            m_SeqNos[m_GaugeNo] = seqNo;

            if (++m_GaugeNo >= (int)HpmjGauge.GAUGE_MAX_NO)
                m_GaugeNo = 0;

            return -1;
        }
        #endregion

        #region Methods
        private Gauge GetItem(int id)
        {
            Gauge gauge;
            switch (id)
            {
                case (int)HpmjGauge.GAUGE_CO2:
                    gauge = m_HpmjUnit.CO2InPress;
                    break;
                case (int)HpmjGauge.GAUGE_DI:
                    gauge = m_HpmjUnit.MainDiPress;
                    break;
                case (int)HpmjGauge.GAUGE_RESIST:
                    gauge = m_HpmjUnit.Resistivity;
                    break;
                case (int)HpmjGauge.GAUGE_PRESS_IN:
                    gauge = m_HpmjUnit.FilterInPress;
                    break;
                case (int)HpmjGauge.GAUGE_PRESS_OUT:
                    gauge = m_HpmjUnit.FilterOutPress;
                    break;
                case (int)HpmjGauge.GAUGE_FLOW:
                    gauge = m_HpmjUnit.HpmjFlow;
                    break;
                default:
                    gauge = null;
                    break;
            }
            return gauge;
        }

        private bool GetCheckCond(int id)
        {
            bool check = true;
            check &= m_GenInfos.AutoMode;
            check &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            check &= m_Server.JobCond.ProcessMode;
            check &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();

            if (id == (int)HpmjGauge.GAUGE_CO2 ||
                id == (int)HpmjGauge.GAUGE_DI ||
                id == (int)HpmjGauge.GAUGE_RESIST)
            {
                ;   // dspcrassus - HPMJ 사용조건에서는 무조건 Check
            }
            else
            {
                check &= m_HpmjUnit.IfFlag.PumpRun;
                check &= m_HpmjUnit.IfFlag.Ready;
            }

            return check;
        }

        private double GetStandardFlowrate()
        {
            int StandardPress = m_HpmjUnit.SetupHpmjStandardPressure.GetValue<int>();
            double StandardFlow = m_HpmjUnit.SetupHpmjStandardFlowrate.GetValue<double>();
            double StandardArea = StandardFlow / Math.Pow(StandardPress, 0.5);

            return (StandardArea * Math.Pow(m_Server.JobCond.HpmjPressure(m_HpmjUnit), 0.5));
        }
        #endregion
    }

    public class SeqHpmjModeChange : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
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
            m_Server = m_HpmjUnit.ServerManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "HPMJ MODE";

            PrevPressure = m_HpmjUnit.SetupHpmjPressure.GetValue<double>();
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
                        //m_HpmjUnit.doManual_On.SetState(false);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Auto Mode");
                        seqNo = 10;
                    }
                    else if (!Auto)
                    {
                        //m_HpmjUnit.ManualOn.SetState(true);
                        PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                        string log = string.Format(m_SeqFunName, 0, 0, "Save Previous HPMJ Pressure {0}", PrevPressure);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, log);
                        seqNo = 20;
                    }
                    break;
                case 10:
                    if (!Auto)
                    {
                        //m_HpmjUnit.ManualOn.SetState(true);
                        PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Manual Mode");
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (Auto)
                    {
                        //m_HpmjUnit.doManual_On.SetState(false);
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Auto Mode");
                        m_HpmjUnit.IfFlag.PressureSet = (int)PrevPressure;

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

    //public class SeqHpmjPowerCut : XSeqFunction
    //{
    //    #region Fields
    //    private Hpmj m_HpmjUnit;
    //    protected static IServerManager m_Server;
    //    protected static IEqpManager m_EqpManager;
    //    protected static ThreadHpmjControl m_Control;
    //    protected static GenInfoHandler m_GenInfos;
    //    #endregion

    //    #region Constructor
    //    public SeqHpmjPowerCut(ThreadHpmjControl control, Hpmj hpmj)
    //    {
    //        m_HpmjUnit = hpmj;
    //        m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
    //        m_Server = m_HpmjUnit.ServerManager;
    //        m_Control = control;
    //        m_GenInfos = GenInfoHandler.Instance;

    //        SeqFunName = "HPMJ POWER";
    //    }
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        int seqNo = this.SeqNo;
    //        HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;

    //        int percentage = (int)(m_HpmjUnit.SetupHpmjFrequency.GetValue<int>() * 0.7);
    //        double curPressure;

    //        if (AppConfig.Instance.Simul.Device)
    //        {
    //            curPressure = m_HpmjUnit.IfFlag.CurrentPressure;
    //        }
    //        else
    //        {
    //            curPressure = m_HpmjUnit.FilterOutPress.CurValue;
    //        }


    //        bool emo = ((heavy & ~HeavyInterlock.Emo) > 0 );
    //        bool initComp = GenInfoHandler.Instance.EqpInitComp;
    //        bool auto = GenInfoHandler.Instance.AutoMode;
    //        bool hpmjUse = m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
    //        bool pumpRun = m_HpmjUnit.IfFlag.PumpRun;
    //        bool ready = m_HpmjUnit.IfFlag.Ready;
    //        bool alarm = m_HpmjUnit.IfFlag.Alarm;
    //        bool gaugeInterlock = m_HpmjUnit.IfFlag.GaugeInterlockAlarm;


    //        switch (seqNo)
    //        {
    //            case 0:
    //                if (!emo && initComp && auto && hpmjUse && pumpRun && ready && !alarm && !gaugeInterlock
    //                    && (percentage > curPressure))
    //                {
    //                    m_HpmjUnit.IfFlag.PowerCut = true;
    //                    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "PowerCut True");
    //                    m_StartTicks = XFunc.GetTickCount();
    //                    seqNo = 10;
    //                }
    //                break;
    //            case 10:
    //                if (GetElapsedTicks() > 1000)
    //                {
    //                    m_HpmjUnit.IfFlag.PowerCut = false;
    //                    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "PowerCut False");
    //                    seqNo = 0;
    //                }
    //                break;
    //        }

    //        this.SeqNo = seqNo;
    //        return -1;
    //    }
    //    #endregion
    //}

    //public class SeqSample : XSeqFunction
    //{
    //    #region Fields
    //    private Hpmj m_HpmjUnit;
    //    protected static IServerManager m_Server;
    //    protected static IEqpManager m_EqpManager;
    //    protected static ThreadHpmjControl m_Control;
    //    #endregion

    //    #region Constructor
    //    public SeqSample(ThreadHpmjControl control, Hpmj hpmj)
    //    {
    //        m_HpmjUnit = hpmj;
    //        m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
    //        m_Server = m_HpmjUnit.ServerManager;
    //        m_Control = control;

    //        SeqFunName = "HPMJ SAMPLE";
    //    }
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        int seqNo = this.SeqNo;

    //        switch (seqNo)
    //        {
    //            case 0:
    //                break;
    //        }

    //        this.SeqNo = seqNo;
    //        return -1;
    //    }
    //    #endregion
    //}

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
            double setupVentOpenPeriod = ((double)m_HpmjUnit.SetupHpmjCO2VentValveOpenPeriod.GetValue<float>() * 3600 * 1000);

            bool setupVentControl = (bool)m_HpmjUnit.SetupHpmjCO2VentControl.GetValue<bool>();

            // dspcrassus - HPMJ Unit : CO2 In Valve Open/Close Interlock
            double dMainPress = m_HpmjUnit.MainDiPress.CurValue;
            if ((!m_HpmjUnit.IfFlag.DiLack || m_HpmjUnit.CO2InValve.IsOpen()) && (dMainPress < setupInOpenLevel))
            {
                m_HpmjUnit.IfFlag.DiLack = true;
                m_HpmjUnit.CO2InValve.Close();
                m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "CO2 Valve Close");
            }
            else if ((m_HpmjUnit.IfFlag.DiLack || m_HpmjUnit.CO2InValve.IsClose()) && (dMainPress >= setupInOpenLevel))
            {
                m_HpmjUnit.IfFlag.DiLack = false;
                m_HpmjUnit.CO2InValve.Open();
                m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "CO2 Valve Open");
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
            m_AlarmId = new int[(int)HPMJ_ALARM.MAX_NO];
            for (int i = 0; i < (int)HPMJ_ALARM.MAX_NO; i++)
                m_AlarmId[i] = 0;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;
            if (!m_GenInfos.EqpInitComp) return -1;

            bool checkCond = true;
            checkCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            checkCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit) || !m_GenInfos.AutoMode;
            //checkCond &= !m_HpmjUnit.IfFlag.PowerCut;

            switch (seqNo)
            {
                case 0:
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
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
                    else if (m_HpmjUnit.IfFlag.Alarm)
                    {
                        m_HpmjUnit.IfFlag.Alarm = false;
                    }
                    break;
                case 40:
                    if (checkCond)
                    {
                        m_HpmjUnit.CheckAlarm(m_SeqFunName, ref m_EqpManager,
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.INVERTER],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.EMO],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.PUMP_MC]);
                        //foreach (IoDigitalInput Di in m_HpmjUnit.ForAlarmDiList)
                        //{
                        //    if (Di.GetState())
                        //    {
                        //        if (Di.Id == m_HpmjUnit.Inverter.DiAlarm.Id)
                        //        {
                        //            m_EqpManager.SetAlarm(m_HpmjUnit.ALM_InverterAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                        //            m_AlarmId[(int)HPMJ_ALARM.INVERTER] = m_HpmjUnit.ALM_InverterAlarm.Id;
                        //        }
                        //        //else if(Di.Id == m_HpmjUnit.LeakSensor.DiSensor.Id)
                        //        //{
                        //        //    m_EqpManager.SetAlarm(m_HpmjUnit.ALM_LeakAlarm.Id);
                        //        //    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Leak Sensor Alarm");
                        //        //    m_AlarmId[(int)HPMJ_ALARM.LEAK] = m_HpmjUnit.ALM_LeakAlarm.Id;
                        //        //}
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
                        //    }
                        //}

                        //m_StartTicks = XFunc.GetTickCount();
                        seqNo = 1000;
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
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.INVERTER],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.EMO],
                                              true, ref m_AlarmId[(int)HPMJ_ALARM.PUMP_MC]);
                        //foreach (IoDigitalInput Di in m_HpmjUnit.ForAlarmDiList)
                        //{
                        //    if (!Di.GetState())
                        //    {
                        //        if (Di.Id == m_HpmjUnit.Inverter.DiAlarm.Id && m_AlarmId[(int)HPMJ_ALARM.INVERTER] != 0)
                        //        {
                        //            m_EqpManager.ResetAlarm(m_HpmjUnit.ALM_InverterAlarm.Id);
                        //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm Reset");
                        //            m_AlarmId[(int)HPMJ_ALARM.INVERTER] = 0;
                        //        }
                        //        //else if(Di.Id == m_HpmjUnit.LeakSensor.DiSensor.Id && m_AlarmId[(int)HPMJ_ALARM.LEAK] != 0)
                        //        //{
                        //        //    m_EqpManager.SetAlarm(m_HpmjUnit.ALM_LeakAlarm.Id);
                        //        //    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Leak Sensor Alarm");
                        //        //    m_AlarmId[(int)HPMJ_ALARM.LEAK] = 0;
                        //        //}
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
            ALM_InitFail = new Alarm("HpmjUnit" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

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
                            alarm |= device.CheckAlarm(m_SeqFunName, ref m_EqpManager,
                                                       true, true, true);

                            //foreach (IoDigitalInput Di in device.ForAlarmDiList)
                            //{
                            //    if (Di.GetState())
                            //    {
                            //        if (Di.Id == device.Inverter.DiAlarm.Id)
                            //        {
                            //            m_EqpManager.SetAlarm(device.ALM_InverterAlarm.Id);
                            //            device.SetLog(SeqFunName, 0, 0, "HPMJ Unit Inverter Alarm");
                            //        }
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
                            //    }
                            //}
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

    //public class SeqHpmjManual : XSeqFunction
    //{
    //    #region Fields
    //    private Hpmj m_HpmjUnit;
    //    protected static IServerManager m_Server;
    //    protected static IEqpManager m_EqpManager;
    //    protected static ThreadHpmjControl m_Control;
    //    protected static GenInfoHandler m_GenInfos;
    //    protected bool PumpRun = false;
    //    protected bool PumpStop = false;
    //    #endregion

    //    #region Constructor
    //    public SeqHpmjManual(ThreadHpmjControl control, Hpmj hpmj)
    //    {
    //        m_HpmjUnit = hpmj;
    //        m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
    //        m_Server = m_HpmjUnit.ServerManager;
    //        m_Control = control;
    //        m_GenInfos = GenInfoHandler.Instance;

    //        SeqFunName = "HPMJ MANUAL";
    //    }
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        bool Use = m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
    //        bool Auto = m_GenInfos.AutoMode;

    //        if (m_HpmjUnit.IfFlag.m.GetState() && !PumpRun && m_HpmjUnit.doManual_On.GetState() &&
    //            Use)
    //        {
    //            PumpRun = true;
    //            m_HpmjUnit.IfFlag.PumpRun = true;
    //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Manual Test Run.");
    //        }
    //        else if ((!m_HpmjUnit.diPump_Manual_Run.GetState() || Auto) && PumpRun)
    //        {
    //            PumpRun = false;
    //        }

    //        if (m_HpmjUnit.diPump_Manual_Stop.GetState() && !PumpStop && m_HpmjUnit.doManual_On.GetState())
    //        {
    //            PumpStop = true;
    //            m_HpmjUnit.IfFlag.PumpRun = false;
    //            m_HpmjUnit.SetLog(SeqFunName, 0, 0, "HPMJ Unit Manual Test Stop.");
    //        }
    //        else if ((!m_HpmjUnit.diPump_Manual_Stop.GetState() || Auto) && PumpStop)
    //        {
    //            PumpStop = false;
    //        }

    //        return -1;
    //    }
    //    #endregion
    //}

    public class SeqPumpRun : XSeqFunction
    {
        #region Fields
        private Hpmj m_HpmjUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        private Simul m_Simul;

        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private double press = 0.0;
        #endregion

        #region Constructor
        public SeqPumpRun(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            m_Timer1 = new XTimer("PumpRun1");
            m_Timer2 = new XTimer("PumpRun2");

            m_SeqFunName = "HPMJ PUMP RUN";
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

            //bool RunCond = true;
            //if (GenInfoHandler.Instance.AutoMode)
            //{
            //    RunCond &= GenInfoHandler.Instance.EqpInitComp;
            //    RunCond &= GenInfoHandler.Instance.DiStart;
            //    RunCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            //    RunCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit);
            //    RunCond &= !m_HpmjUnit.IfFlag.DiLack;
            //    RunCond &= !m_HpmjUnit.IfFlag.Alarm;
            //    //RunCond |= (m_HpmjUnit.SetupHpmjUse.GetValue<bool>() && GenInfoHandler.Instance.IdleRunning);

            //    if (!RunCond && m_HpmjUnit.IfFlag.PumpRun)
            //    {
            //        m_HpmjUnit.IfFlag.PumpRun = false;
            //    }
            //}
            //else if (!GenInfoHandler.Instance.AutoMode)
            //{
            //    //RunCond &= m_HpmjUnit.Pump.IsRun();
            //    RunCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            //    RunCond &= !m_HpmjUnit.IfFlag.DiLack;
            //    if (!RunCond && m_HpmjUnit.IfFlag.PumpRun) m_HpmjUnit.IfFlag.PumpRun = false;
            //}

            switch (seqNo)
            {
                case 0:
                    if (m_HpmjUnit.IfFlag.PumpRun)//&& RunCond)
                    {
                        m_HpmjUnit.SetLog(m_SeqFunName, 0, 0, "Pump Start.");

                        m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                        m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    {
                        m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                        m_HpmjUnit.IfFlag.Ready = false;

                        if (GenInfoHandler.Instance.AutoMode)
                        {
                            //m_HpmjUnit.IfFlag.PressureSet = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
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
                                if (!m_HpmjUnit.IfFlag.Remote)
                                {
                                    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 30;
                                }
                                else
                                {
                                    //m_HpmjUnit.IfFlag.CurrentAnalogOutput = m_HpmjUnit.SetupHpmjFrequency.GetValue<int>();
                                    m_HpmjUnit.IfFlag.CurrentHertz = m_HpmjUnit.SetupHpmjFrequency.GetValue<int>();
                                }

                                m_HpmjUnit.SetPumpHertz(m_HpmjUnit.IfFlag.CurrentHertz);
                                log = string.Format("Manual = Frequency Mode : Frequence = {0:0.00}", m_HpmjUnit.IfFlag.CurrentHertz);
                                //if(m_HpmjUnit.IfFlag.CurrentAnalogOutput > 0x7FFF)
                                //    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0x7FFF;
                                //else if(m_HpmjUnit.IfFlag.CurrentAnalogOutput < 0)
                                //    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0x0000;

                                //m_HpmjUnit.SetPumpHertz((double)m_HpmjUnit.IfFlag.CurrentAnalogOutput);

                                //log = string.Format("Manual = Frequency Mode : Frequency = {0:0.00}", m_HpmjUnit.IfFlag.CurrentAnalogOutput);
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
                        m_HpmjUnit.IfFlag.CurrentHertz = 10;
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
                            //m_HpmjUnit.IfFlag.PressureSet = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
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
                        double err = SetPumpControl();

                        if (err >= 20.0)
                        {
                            m_Timer1.Start(200);
                        }
                        else
                        {
                            m_Timer1.Start(500);
                        }

                        if (m_HpmjUnit.IfFlag.PPIDChangeRequest)
                        {
                            if (simulation)
                            {
                                m_HpmjUnit.IfFlag.ReferencePressure = 0;
                                press = 0.0;
                                m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                                m_HpmjUnit.IfFlag.CurrentHertz = 0;
                            }
                            seqNo = 30;

                            //m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                            //m_HpmjUnit.IfFlag.Ready = false;
                            //m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            //m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                            //m_HpmjUnit.HpmjInverterStop();
                            //log = string.Format("Auto Mode Press Val Change[200]");
                            //m_HpmjUnit.SetLog(this.SeqFunName, 0, 0, log);
                            //log = string.Format("Pump Stop");
                            //m_HpmjUnit.SetLog(this.SeqFunName, 0, 0, log);
                            //m_StartTicks = XFunc.GetTickCount();

                            //if(simulation)
                            //{
                            //    m_HpmjUnit.IfFlag.ReferencePressure = 0;
                            //    press = 0.0;

                            //    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                            //    m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            //}
                            //seqNo = 210;
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
                            if (simulation)
                            {
                                m_HpmjUnit.IfFlag.ReferencePressure = 0;
                                press = 0;

                                m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                                m_HpmjUnit.IfFlag.CurrentHertz = 0;

                                seqNo = 10;
                            }
                            //m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                            //m_HpmjUnit.IfFlag.Ready = false;
                            //m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            //m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                            //m_HpmjUnit.HpmjInverterStop();

                            ////m_HpmjUnit.HpmjInverterStop();
                            //log = string.Format("Auto Mode Press Val Change[200]");
                            //m_HpmjUnit.SetLog(this.SeqFunName, 0, 0, log);
                            //log = string.Format("Pump Stop");
                            //m_HpmjUnit.SetLog(this.SeqFunName, 0, 0, log);
                            //m_StartTicks = XFunc.GetTickCount();

                            //if(simulation)
                            //{
                            //    m_HpmjUnit.IfFlag.ReferencePressure = 0;
                            //    press = 0.0;

                            //    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                            //    m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                            //}
                            //seqNo = 210;
                        }
                    }
                    else if (!m_HpmjUnit.IfFlag.PumpRun)
                    {
                        seqNo = 200;
                    }
                    break;
                case 200:
                    if (!m_HpmjUnit.IfFlag.PumpRun)// || !RunCond)
                    {
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
                    }
                    //else if(m_HpmjUnit.IfFlag.PPIDChangeRequest)
                    //{
                    //    m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                    //    m_HpmjUnit.IfFlag.Ready = false;
                    //    m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                    //    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;

                    //    m_HpmjUnit.HpmjInverterStop();

                    //    m_HpmjUnit.SetLog(SeqFunName, 0, 0, "Pump Stop.");
                    //    if(simulation)
                    //    {
                    //        m_HpmjUnit.IfFlag.ReferencePressure = 0;
                    //        press = 0.0;

                    //        m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0;
                    //        m_HpmjUnit.IfFlag.CurrentHertz = 0.0;
                    //    }
                    //    m_StartTicks = XFunc.GetTickCount();
                    //    seqNo = 210;
                    //}
                    break;
                //case 210:
                //    if(GetElapsedTicks() > 7 * 1000)
                //    {
                //        m_HpmjUnit.SetLog(SeqFunName, 0, 0, "Auto Mode Press Val Change[210]");
                //        seqNo = 0;
                //    }
                //    break;
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
                if (m_HpmjUnit.IfFlag.CurrentPressure < 5) Property = 10;
                else if (Error > 100) Property = 350;
                else if (Error > 90) Property = 80;
                else if (Error > 50) Property = 300;
                else if (Error > 40) Property = 80;
                else if (Error > 30) Property = 300;
                else if (Error > 20) Property = 200;
                else if (Error > 10) Property = 150;
                else if (Error > 5) Property = 100;
                else if (Error > 3) Property = 80;
                else if (Error > 1) Property = 50;
                else Property = 20;
            }
            else
            {
                if (Error > 50) Property = 80;
                else if (Error > 30) Property = 50;
                else if (Error > 3) Property = 30;
                else if (Error > 1) Property = 10;
                else Property = 5;
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

                if (m_HpmjUnit.IfFlag.CurrentAnalogOutput > 0x7FFF)
                {
                    m_HpmjUnit.IfFlag.CurrentAnalogOutput = 0x7FFF;
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
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHpmjControl m_Control;
        #endregion

        #region Constructor
        public SeqStateMonitor(ThreadHpmjControl control, Hpmj hpmj)
        {
            m_HpmjUnit = hpmj;
            m_EqpManager = m_HpmjUnit.ServerManager.EqpStateManager;
            m_Server = m_HpmjUnit.ServerManager;
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

            if (GaugeAlarm)
                m_HpmjUnit.IfFlag.GaugeInterlockAlarm = true;
            else
                m_HpmjUnit.IfFlag.GaugeInterlockAlarm = false;

            if (!GenInfoHandler.Instance.AutoMode)
            {
                if (m_HpmjUnit.IfFlag.Alarm || m_HpmjUnit.IfFlag.GaugeInterlockAlarm ||
                    m_HpmjUnit.IfFlag.PIDError)
                {
                    m_HpmjUnit.IfFlag.PumpRun = false;
                }
            }

            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            bool alarm2 = false;
            alarm2 |= m_HpmjUnit.OwnerUnit.IfFlag.InError;
            alarm2 |= m_HpmjUnit.OwnerUnit.IfFlag.OutError;
            alarm2 |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.InError;
            alarm2 |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.OutError;

            RunCond &= GenInfoHandler.Instance.AutoMode;
            //RunCond &= GenInfoHandler.Instance.DiStart;
            RunCond &= GenInfoHandler.Instance.IdleRunning;
            RunCond &= !IsInterlock(m_HpmjUnit);
            RunCond &= !m_HpmjUnit.IfFlag.Alarm;
            RunCond &= !m_HpmjUnit.IfFlag.GaugeInterlockAlarm;
            RunCond &= !m_HpmjUnit.IfFlag.PIDError;
            RunCond &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            RunCond &= GenInfoHandler.Instance.EqpInitComp;
            RunCond &= m_Server.JobCond.HpmjUse(m_HpmjUnit);
            RunCond &= m_Server.JobCond.ProcessMode;
            RunCond &= (m_HpmjUnit.MainDiPress.CurValue > (double)m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<float>());

            switch (seqNo)
            {
                case 0:
                    if (RunCond && !m_HpmjUnit.IfFlag.PumpRun)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = true;
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set True");
                        seqNo = 10;
                    }
                    //else if(GenInfoHandler.Instance.AutoMode)
                    //{
                    //    if(!m_HpmjUnit.IfFlag.PumpRun)
                    //    {
                    //        m_HpmjUnit.IfFlag.PumpRun = false;
                    //    }
                    //}
                    //if (m_HpmjUnit.IfFlag.ModeChange)
                    //{
                    //    m_HpmjUnit.IfFlag.ModeChange = false;
                    //    seqNo = 10;
                    //}
                    break;
                case 10:
                    if (!RunCond && m_HpmjUnit.IfFlag.PumpRun && GenInfoHandler.Instance.AutoMode)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = false;
                        m_HpmjUnit.SetLog(this.m_SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set False");
                        seqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion

        #region Methods
        public virtual bool IsInterlock(Hpmj hpmj)
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
                    if (GetElapsedTicks() > 2000)
                    {
                        double Ai1, Ai2, Ai3, Ai4, Ai5;

                        Ai1 = m_HpmjUnit.HpmjFlow.CurValue;
                        Ai2 = m_HpmjUnit.FilterInPress.CurValue;
                        Ai3 = m_HpmjUnit.FilterOutPress.CurValue;
                        Ai4 = m_HpmjUnit.CO2InPress.CurValue;
                        Ai5 = m_HpmjUnit.MainDiPress.CurValue;

                        string log = string.Format("Shower Flow : {0:0.00} \t Filter In Pressure : {1:0.00} \t Filter Out Pressure : {2:0.00} \t CO2 In Pressure : {3:0.00} \t Main Di In Pressure : {4:0.00}",
                                            Ai1, Ai2, Ai3, Ai4, Ai5);
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
}

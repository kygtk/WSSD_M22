using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Data;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadPSMApControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<PSMAp> m_Aps;
        protected static _GenInfoHandler m_GenInfos;
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static _GenericCollection<Gauge> m_Gauges;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_Aps.Count == 0) return;

            m_Server.AddSeqInitFunction(new SeqInitAp(this, m_Server));

            foreach (PSMAp device in m_Aps)
            {
                RegisterSequence(new SeqPSMAp(this, device));
                RegisterSequence(new SeqPSMApAlarm(this, device));

                RegisterSequence(new SeqPSMApMfcGaugeInterlock(this, device, device.MfcN2.Gauge));
                RegisterSequence(new SeqPSMApMfcGaugeInterlock(this, device, device.MfcCDA.Gauge));
                RegisterSequence(new SeqPSMApMfcGaugeInterlock(this, device, device.MfcVoltage.Gauge));

                RegisterSequence(new SeqPSMApHouseSensor(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadPSMApControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Aps = DmsComponents.Instance.ComponentContainer.GetCollection<PSMAp>();
            m_GenInfos = GenInfoHandler.Instance;
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_Gauges = DmsComponents.Instance.ComponentContainer.GetCollection<Gauge>();

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
        public static _GenericCollection<PSMAp> Units
        {
            get
            {
                if (m_Aps == null) m_Aps = new _GenericCollection<PSMAp>();
                return m_Aps;
            }
        }
        #endregion   

        #region Virtual Methods
        public virtual bool GetHouseCloseState(PSMAp ap)
        {
            bool close = true;
            close &= ap.ActuatorUnit.IsNegative();
            close &= ap.DiHouseClose.GetState();
            close &= (ap.DiHouseClose2 != null) ? ap.DiHouseClose2.GetState() : true;
            return close;
        }

        //public virtual bool IsPCWValueError(PSMAp ap)
        //{
        //    double value = ap.GaugePCW.CurValue;
        //    bool rv = (value < ap.SetupPCWIntrFlow.GetValue<float>()) ;
        //    return rv;
        //}

        public virtual bool GetApCond(PSMAp ap)
        {
            bool cond = true;
            if (ap.IsUse())
            {
                cond &= !ap.IsAlarm;
                cond &= GetHouseCloseState(ap);
            }
            else
            {
                cond &= GetHouseCloseState(ap);
            }
            return cond;
        }

        public virtual bool IsGlassExist(PSMAp ap)
        {
            bool bExist = false;

            bExist |= ap.OwnerUnit.PrevCv.GlsOutSensor.IsDetected();
            bExist |= ap.OwnerUnit.GlsInSensor.IsDetected();
            bExist |= ap.OwnerUnit.GlsOutSensor.IsDetected();

            return bExist;
        }

        public virtual bool PSMApOffCondition(PSMAp ap)
        {
            bool apOff = false;
            CvUnit cv;
            for (int i = 0; i < ap.OwnerUnit.Id; i++)
            {
                cv = m_CvUnits[i];
                apOff |= (cv.IfFlag.InError || cv.IfFlag.OutError);
            }

            return apOff;
        }

        public virtual bool GetGaugeNgCond(PSMAp ap)
        {
            bool ng = false;
            int count = m_Gauges.Count;
            Gauge gauge;

            for (int i = 0; i < count; i++)
            {
                gauge = m_Gauges[i];
                if ((gauge.InterlockEnable) && (gauge.OwnerUnit != null) && (gauge.OwnerUnit.Name == ap.OwnerUnit.Name))
                {
                    ng |= gauge.IsAlarm;
                }
            }
            return ng;
        }

        public virtual bool GetRefOperation(PSMAp ap)
        {
            bool run = IsRunCondition(ap);
            bool interlock = IsInterlockCondition(ap);

            return run && !interlock;
        }

        public virtual bool IsInterlockCondition(PSMAp ap)
        {
            bool interlock = false;
            interlock |= !GetHouseCloseState(ap);
            interlock |= (ap.GetStatusAlarm() != (int)PSMApAlarmIndex.errNone);
            interlock |= m_GenInfos.Pause;
            interlock |= (m_Server.JobCond.HeavyInterlock > 0);
            interlock |= PSMApOffCondition(ap);
            interlock |= GetGaugeNgCond(ap);
            return interlock;
        }

        public virtual bool IsRunCondition(PSMAp ap)
        {
            bool run = true;
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_Server.JobCond.ProcessMode;
            run &= ap.IsUse();
            run &= (ap.OwnerUnit.PrevCv.MotorControl.IsFw(Logic.OR) || ap.OwnerUnit.MotorControl.IsFw(Logic.OR));
            run &= IsGlassExist(ap);
            return run;
        }
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqInitAp : XSeqInitFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected InitState m_InitState = InitState.Noop;
        protected _GenericCollection<PSMAp> m_Units;
        protected new int[] m_AlarmId;
        protected ThreadPSMApControl m_Control;

        protected GenericTag m_InitCheckApPowerReady = new GenericTag("AP Power Ready", InitCheckState.NotReady);
        protected GenericTag m_InitCheckApStatusAlarm = new GenericTag("AP Status Alarm", InitCheckState.NotReady);
        protected GenericTag m_InitCheckApClose = new GenericTag("AP House Close", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitAp(ThreadPSMApControl control, IServerManager server)
        {
            m_Server = server;
            m_Eqp = m_Server.EqpStateManager;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<PSMAp>();
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            int nSeqNo = this.m_SeqNo;
            int alarmCode = 0;
            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;
                        m_Server.Log("SeqApInit : Start ");
                        nSeqNo = 10;

                        if (AppConfig.Instance.Simul.Device)
                        {
                            foreach (PSMAp ap in m_Units)
                            {
                                ap.ActuatorUnit.SetNegativeAct();
                                ap.DiHouseClose.SetState(true);
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
                            //if (m_Server.JobCond.ApUse(m_Units[i]))
                            if (m_Units[i].IsUse())	//Use Option이 Setup이거나 Recipe일 수 있으므로 내부의 IsUse()를 사용한다.
                            {
                                use = true;
                            }
                        }
                        if (use == false)
                        {
                            m_InitCheckApPowerReady.Value = InitCheckState.NoUse;
                            m_InitCheckApStatusAlarm.Value = InitCheckState.NoUse;
                            nSeqNo = 40;
                        }
                        else
                        {
                            foreach (PSMAp ap in m_Units)
                            {
                                if (ap.IsUse())
                                {
                                    if (ap.ColorBoard != null)
                                    {
                                        ap.ColorBoard.On();
                                    }
                                    //ap.SetMfcFlow(ap.MfcN2.Name, m_Server.JobCond.SeApN2Flow(ap));
                                    //ap.SetMfcFlow(ap.MfcCDA.Name, m_Server.JobCond.SeApCDAFlow(ap));
                                    //ap.SetMfcFlowbySetupValue();
                                    //ap.SetVoltage();  //AP ON할때 
                                }
                            }
                            m_Server.Log("SeqInitAp : UT Turn ON");
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        m_InitCheckApPowerReady.Value = InitCheckState.Checking;

                        bool ready = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (!m_Units[i].IsPowerReady())
                            {
                                ready = false;
                                m_AlarmId[i] = m_Units[i].ALM_PowerNotReady.Id;
                                m_Eqp.SetAlarm(m_AlarmId[i]);
                                break;
                            }
                        }

                        if (ready)
                        {
                            m_Server.Log("SeqInitAp : Check Ap Power Ready Ok");
                            m_InitCheckApPowerReady.Value = InitCheckState.OK;
                            nSeqNo = 30;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckApPowerReady.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 30:
                    {
                        m_InitCheckApStatusAlarm.Value = InitCheckState.Checking;

                        bool ready = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {

                            if ((alarmCode = m_Units[i].GetStatusAlarm()) != (int)PSMApAlarmIndex.errNone)
                            {
                                ready = false;

                                m_AlarmId[i] = alarmCode + m_Units[i].ALM_InterlockOpen.Id;
                                m_Eqp.SetAlarm(m_AlarmId[i]);
                                break;
                            }
                        }

                        if (ready)
                        {
                            m_Server.Log("SeqInitAp : Check Ap Status Alarm OK");
                            m_InitCheckApStatusAlarm.Value = InitCheckState.OK;
                            nSeqNo = 40;
                        }
                        else
                        {
                            m_Server.Log("SeqInitAp : Ap Status Alarm");
                            m_InitState = InitState.Fail;
                            m_InitCheckApStatusAlarm.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 40:
                    {
                        m_InitCheckApClose.Value = InitCheckState.Checking;

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
                            m_Server.Log("SeqInitAp : House Close Confirm");
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
                        m_Server.Log("SeqInitAp : Init Complete");
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
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqPSMAp : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected PSMAp m_Ap;
        protected string m_Msg;
        protected ThreadPSMApControl m_Control;
        protected Alarm ALM_PowerOffAlarm;
        protected SeqPSMApProgress m_SeqProgress;
        #endregion

        #region Constructor
        public SeqPSMAp(ThreadPSMApControl control, PSMAp ap)
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_Msg = "";

            this.m_SeqFunName = m_Ap.Name + " SeqPSMAp";
            ALM_PowerOffAlarm = new Alarm(m_Ap.Name + " Power Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_SeqProgress = new SeqPSMApProgress(m_Control, m_Ap);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int nSeqNo = this.m_SeqNo;

            bool apPowerOn = m_Control.GetRefOperation(m_Ap);
            m_SeqProgress.Do(apPowerOn);
            int onMargin = m_Ap.SetupPowerOnWaitTime.GetValue<int>() * 1000;

            switch (nSeqNo)
            {
                case 0:
                    if (apPowerOn)
                    {
                        double setN2Flow = m_Server.JobCond.ApN2Flow(m_Ap);
                        double setCDAFlow = m_Server.JobCond.ApCDAFlow(m_Ap);
                        double setVoltage = m_Server.JobCond.ApVoltage(m_Ap);

                        m_Ap.SetN2Flow(setN2Flow);
                        m_Msg = string.Format("N2 Flow set : {0}", setN2Flow.ToString());
                        m_Ap.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        m_Ap.SetCDAFlow(setCDAFlow);
                        m_Msg = string.Format("CDA Flow set : {0}", setCDAFlow);
                        m_Ap.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        m_Ap.SetVoltage(setVoltage);
                        m_Msg = string.Format("Plasma Power : {0}", setVoltage);
                        m_Ap.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        //m_Ap.PowerOn();
                        //m_Ap.SetLog(SeqFunName, 0, 0, "Plasma Power On");

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > onMargin && apPowerOn)
                    {
                        m_Ap.PowerOn();
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "Plasma Power On");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (m_Ap.IsPowerOn())
                    {
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "AP Plasma Power On State Confirm");
                        nSeqNo = 30;

                    }
                    else if (GetElapsedTicks() > 1000)
                    {
                        m_AlarmId = ALM_PowerOffAlarm.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "AP Plasma Power Off Alarm Set");
                        m_Ap.IsAlarm = true;
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 1000;
                    }
                    break;
                case 30:
                    if (!apPowerOn)
                    {
                        m_Ap.PowerOff();
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "Ap Plasma Power Off");

                        //m_Ap.SetN2Flow(0.0);
                        //m_Ap.SetLog(SeqFunName, 0, 0, "N2 Flow Reset");

                        //m_Ap.SetCDAFlow(0.0);
                        //m_Ap.SetLog(SeqFunName, 0, 0, "CDA Flow Reset");

                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        if (m_Ap.IsPowerOn())
                        {
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_Ap.IsAlarm = false;
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqPSMApProgress : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected ThreadPSMApControl m_Control;
        protected int m_ProgressTime;
        protected int m_OldTime = 0;
        protected XTimer m_Timer;
        protected int m_Tm;
        private PSMAp m_Ap;
        private bool m_NextGlass = false;
        #endregion

        public SeqPSMApProgress(ThreadPSMApControl control, PSMAp ap)
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = m_Ap.Name + " Progress";
            m_Timer = new XTimer(m_Ap.Name + " Timer : SeqApOnProgress");
        }

        public int Do(bool progress)
        {
            int ldOut2ApIn = m_Ap.OwnerUnit.PrevCv.SetupDistanceNext.GetValue<int>();
            int apIn2Out = m_Ap.OwnerUnit.SetupDistance.GetValue<int>();
            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
            int plasmaOnTime = (ldOut2ApIn + apIn2Out + glassSize) / ((m_Server.JobCond.CvProcessSpeed(1) / 60));

            bool runProgress = true;
            runProgress = m_GenInfos.AutoMode;

            int nSeqNo = this.m_SeqNo;
            int nTime = 0;
            bool nextGlassEnter = m_Ap.OwnerUnit.GlsInSensor.IsDetected() && /*m_Ap.OwnerUnit.GlsOutSensor.IsDetected() &&*/
                                  m_Ap.OwnerUnit.PrevCv.MotorControl.IsFw(Logic.OR) && m_Server.GlassData.IsExist(m_Ap.OwnerUnit.Id * 2 + 0);

            switch (nSeqNo)
            {
                case 0:
                    if (progress)
                    {
                        m_OldTime = 0;
                        m_Tm = plasmaOnTime * 1000;
                        m_Timer.Start(m_Tm);
                        m_StartTicks = XFunc.GetTickCount();
                        SetPSMApOnProgress(ProgressAct.PROGRESS_START, plasmaOnTime);
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
                        nTime = (int)m_Timer.CurElapsedTickCounts / 1000;
                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            SetPSMApOnProgress(ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!progress)
                        {
                            SetPSMApOnProgress(ProgressAct.PROGRESS_END, 0);
                            m_OldTime = 0;
                            nSeqNo = 0;
                        }
                        if (!m_NextGlass && m_Ap.OwnerUnit.GlsOutSensor.IsDetected() && m_Server.GlassData.IsExist(m_Ap.OwnerUnit.Id * 2 + 1))
                        {
                            m_NextGlass = true;
                        }
                        if (m_NextGlass && nextGlassEnter)
                        {
                            m_NextGlass = false;
                            SetPSMApOnProgress(ProgressAct.PROGRESS_START, plasmaOnTime);
                            m_Tm = plasmaOnTime * 1000;
                            m_Timer.Start(m_Tm);
                            m_OldTime = 0;
                            //m_StartTicks = XFunc.GetTickCount();
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
                        SetPSMApOnProgress(ProgressAct.PROGRESS_END, 0);
                        m_OldTime = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 30:
                    if (progress && runProgress)
                    {
                        m_Timer.Resume();
                        nSeqNo = 10;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }

        private void SetPSMApOnProgress(ProgressAct act, int time)
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

    public class SeqPSMApMfcGaugeInterlock : XSeqFunction
    {
        private enum IntrState
        {
            Alarm, Warning, Noop
        }

        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        protected static Simul m_Simul;
        private ThreadPSMApControl m_Control;
        private PSMAp m_Ap;
        private Gauge m_Gauge;
        private double m_RecheckDelayTime = 1000;
        private double m_FirstDelayTime = 5000;
        private IntrState m_IntrState = IntrState.Noop;
        private bool[] m_IsAlarmSet;
        private Alarm ALM_LimitAlarm;
        private Alarm ALM_LimitWarning;
        private string m_Msg;
        #endregion

        #region Constructor
        public SeqPSMApMfcGaugeInterlock(ThreadPSMApControl control, PSMAp ap, Gauge gauge)
        {
            m_Ap = ap;
            m_Gauge = gauge;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;
            this.m_SeqFunName = m_Gauge.Name + " Gauge Interlock";
            m_IsAlarmSet = new bool[(int)IntrState.Noop];

            ALM_LimitAlarm = new Alarm(m_Gauge.Name + " Limit Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_LimitWarning = new Alarm(m_Gauge.Name + " Limit Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            //if (m_Simul.Device) return -1;

            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_GenInfos.AutoMode;
            checkCond &= m_Ap.IsPowerOn();
            checkCond &= m_Ap.IsUse();
            checkCond &= !m_Ap.IsAlarm;

            double setValue = 0.0;
            if (m_Gauge.Name == m_Ap.MfcN2.Gauge.Name) setValue = m_Server.JobCond.ApN2Flow(m_Ap);
            else if (m_Gauge.Name == m_Ap.MfcCDA.Gauge.Name) setValue = m_Server.JobCond.ApCDAFlow(m_Ap);
            else if (m_Gauge.Name == m_Ap.MfcVoltage.Gauge.Name) setValue = m_Server.JobCond.ApVoltage(m_Ap);

            switch (nSeqNo)
            {
                case 0:
                    if (checkCond)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (m_IsAlarmSet[(int)IntrState.Alarm])
                        {
                            m_IsAlarmSet[(int)IntrState.Alarm] = false;
                            m_Eqp.ResetAlarm(ALM_LimitAlarm.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }
                        if (m_IsAlarmSet[(int)IntrState.Warning])
                        {
                            m_IsAlarmSet[(int)IntrState.Warning] = false;
                            m_Eqp.ResetAlarm(ALM_LimitWarning.Id);
                            m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "Interlock Recovery(No Check)");
                        }

                        if (m_Gauge.IsAlarm)
                        {
                            m_Gauge.IsAlarm = false;
                        }
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 1000) nSeqNo = 20;
                    break;
                case 20:
                    if (!checkCond) nSeqNo = 0;
                    else
                    {
                        double alarmLimit = m_Ap.SetupAlarmIntrMargin.GetValue<double>();
                        double warningLimit = m_Ap.SetupWarningIntrMargin.GetValue<double>();

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

                        if (m_IntrState != IntrState.Noop)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            if (m_IntrState == IntrState.Alarm)
                            {
                                m_RecheckDelayTime = 3000;
                            }
                            else m_RecheckDelayTime = 5000;
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
                        double alarmLimit = m_Ap.SetupAlarmIntrMargin.GetValue<double>();
                        double warningLimit = m_Ap.SetupWarningIntrMargin.GetValue<double>();

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

                        if (m_IntrState != IntrState.Noop)
                        {
                        }
                        else nSeqNo = 20;
                    }
                    break;
                case 40:
                    if (!checkCond) nSeqNo = 0;
                    else
                    {
                        double alarmLimit = m_Ap.SetupAlarmIntrMargin.GetValue<double>();
                        double warningLimit = m_Ap.SetupWarningIntrMargin.GetValue<double>();

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

                        if (m_IntrState == IntrState.Alarm)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.Alarm])
                            {
                                m_IsAlarmSet[(int)IntrState.Alarm] = true;
                                m_Eqp.SetAlarm(ALM_LimitAlarm.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}%)", curVal, setValue, alarmLimit);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);

                                m_Ap.IsAlarm = true;
                                m_Ap.PowerOff();
                                m_Ap.SetLog(m_SeqFunName, 0, 0, "Gauge Alarm : Power Off");
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.Alarm])
                            {
                                m_IsAlarmSet[(int)IntrState.Alarm] = false;
                                m_Eqp.ResetAlarm(ALM_LimitAlarm.Id);
                            }
                        }

                        if (m_IntrState == IntrState.Warning)
                        {
                            if (!m_IsAlarmSet[(int)IntrState.Warning])
                            {
                                m_IsAlarmSet[(int)IntrState.Warning] = true;
                                m_Eqp.SetAlarm(ALM_LimitWarning.Id);
                                m_Msg = string.Format("{0:F2} ({1}, {2}%)", curVal, setValue, warningLimit);
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, m_Msg);
                            }
                        }
                        else
                        {
                            if (m_IsAlarmSet[(int)IntrState.Warning])
                            {
                                m_IsAlarmSet[(int)IntrState.Warning] = false;
                                m_Eqp.ResetAlarm(ALM_LimitWarning.Id);
                            }
                        }

                        if (!m_IsAlarmSet[(int)IntrState.Alarm])
                        {
                            if (m_Gauge.IsAlarm)
                            {
                                m_Gauge.IsAlarm = false;
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "OK : Recovery");
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
                                m_Gauge.SetLog(this.m_SeqFunName, 0, 0, "NG : Recovery");
                            }

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                            break;
                        }
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "Check Recovery[AP FLOW]");
                        m_RecheckDelayTime = m_FirstDelayTime;
                        m_StartTicks = XFunc.GetTickCount();
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_Ap.IsAlarm = false;
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqPSMApAlarm : XSeqFunction
    {
        #region Fileds
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private PSMAp m_Ap;
        private ThreadPSMApControl m_Control;
        #endregion

        #region Constructor
        public SeqPSMApAlarm(ThreadPSMApControl control, PSMAp ap)
        {
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = m_Ap.Name + " ALARM";

        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp || !m_GenInfos.AutoMode) return -1;

            int nSeqNo = this.m_SeqNo;
            int alarmCode = 0;
            bool simulation = AppConfig.Instance.Simul.Device;

            switch (nSeqNo)
            {
                case 0:
                    if ((alarmCode = m_Ap.GetStatusAlarm()) == (int)PSMApAlarmIndex.errNone) {; }
                    else if (m_Ap.IsUse())
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_Ap.IsUse() || ((alarmCode = m_Ap.GetStatusAlarm()) == (int)PSMApAlarmIndex.errNone))
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 1500)
                    {
                        m_AlarmId = alarmCode + m_Ap.ALM_InterlockOpen.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Ap.IsAlarm = true;
                        m_Ap.PowerOff();
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Power Off");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed || ((GetElapsedTicks() > 1000) && m_Ap.GetStatusAlarm() == (int)PSMApAlarmIndex.errNone))
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Ap.SetLog(m_SeqFunName, 0, 0, "Alarm Recovery");
                        m_Ap.IsAlarm = false;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }


    public class SeqPSMApHouseSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        private ThreadPSMApControl m_Control;
        private PSMAp m_Ap;
        #endregion

        #region Constructor
        public SeqPSMApHouseSensor(ThreadPSMApControl control, PSMAp ap)
        {
            m_Control = control;
            m_Ap = ap;
            m_Server = m_Ap.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = m_Ap.Name + " HOUSE OPEN";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            bool ok = false;
            ok = m_Control.GetHouseCloseState(m_Ap);

            if (!ok && (m_AlarmId == 0))
            {
                m_AlarmId = m_Ap.ALM_HouseOpen.Id;
                m_Eqp.SetAlarm(m_AlarmId);
                m_Ap.IsAlarm = true;
                m_Ap.SetLog(m_SeqFunName, 0, 0, "AP Plasma House Open Alarm Set");
            }
            else if ((m_AlarmId != 0) && m_Eqp.AlarmResetSwitchPushed && ok)
            {
                m_Eqp.ResetAlarm(m_AlarmId);
                m_AlarmId = 0;
                m_Ap.IsAlarm = false;
                m_Ap.SetLog(m_SeqFunName, 0, 0, "AP Plasma House Open Alarm Reset");
            }
            return -1;
        }
        #endregion

    }
}

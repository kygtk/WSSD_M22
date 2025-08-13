using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using System.Threading;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadMixSupplyPumpControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<MixSupplyPumpUnit> m_PumpUnits;
        protected static _GenericCollection<DetergentUnit> m_DetUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_PumpUnits.Count == 0) return;

            foreach (MixSupplyPumpUnit pump in m_PumpUnits)
            {
                RegisterSequence(new SeqMixSupplyPump(this, pump));
                RegisterSequence(new SeqMixPumpAlarm(this, pump));
                RegisterSequence(new SeqMixPumpInterlock(this, pump));
            }

            m_Server.AddSeqInitFunction(new SeqInitMixPump(this, m_PumpUnits));
        }
        #endregion

        #region Constructor
        public ThreadMixSupplyPumpControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_PumpUnits = DmsComponents.Instance.ComponentContainer.GetCollection<MixSupplyPumpUnit>();
            m_DetUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DetergentUnit>();
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
        public static _GenericCollection<MixSupplyPumpUnit> Units
        {
            get
            {
                if (m_PumpUnits == null) m_PumpUnits = new _GenericCollection<MixSupplyPumpUnit>();
                return m_PumpUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(MixSupplyPumpUnit pump)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= pump.IfFlag.InsufficientFlowrate;
            interlock |= (heavy > 0);
            return interlock;
        }

        public virtual bool IsRunEnable(MixSupplyPumpUnit pump)
        {
            bool run = true;
            run &= pump.Pump.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !pump.IfFlag.Alarm;
            run &= (pump.MixTank == null) ? false : !pump.MixTank.IsLevelFault();
            run &= (pump.MixTank == null) ? false : pump.MixTank.IsRunEnableLevel();
            run &= (pump.MixTank == null) ? false : pump.MixTank.DetOutAutoValve.IsOpen();

            return run;
        }

        public virtual bool IsAutoRunCondition(MixSupplyPumpUnit pump)
        {
            bool run = true;
            run &= !IsInterlock(pump);
            run &= IsRunEnable(pump);
            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= m_GenInfos.DiStart;
            run &= (pump.MixTank == null) ? false : pump.MixTank.IsTankReady();
            run &= (GetGlassCount() > 0) || ((GetGlassCount() == 0) && (m_GenInfos.IdleRunning));

            return run;
        }

        public virtual PumpAct GetRefPumpAct(MixSupplyPumpUnit pump)
        {
            bool interlock = IsInterlock(pump);
            bool runEnable = IsRunEnable(pump);
            bool autoRun = IsAutoRunCondition(pump);
            bool autoMode = m_GenInfos.AutoMode;

            if (interlock)
            {
                pump.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
            else if (autoRun)
            {
                pump.ManualAct = PumpAct.Stop;
                return PumpAct.Run;
            }
            else if (!autoMode && runEnable)
            {
                if (m_GenInfos.DiStart && ((pump.MixTank == null) ? false : pump.MixTank.IsTankReady())) return PumpAct.Run;
                else return pump.ManualAct;
            }
            else
            {
                pump.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
        }

        public virtual int GetGlassCount()
        {
            int glassNo = 0;

            glassNo = m_Server.GlassData.Count;

            return glassNo;
        }
        #endregion

        #region General Methods
        public bool IsAlarm()
        {
            bool alarm = false;
            foreach (MixSupplyPumpUnit device in m_PumpUnits)
            {
                alarm |= device.Pump.SetupPumpUse.GetValue<bool>() && device.Pump.IsAlarm();
                if (alarm) break;
            }

            return alarm;
        }
        #endregion
    }

    public class SeqMixSupplyPump : XSeqFunction
    {
        #region Fields
        protected static ThreadMixSupplyPumpControl m_Control;
        protected static MixSupplyPumpUnit m_PumpUnit;
        #endregion

        #region Constructor
        public SeqMixSupplyPump(ThreadMixSupplyPumpControl control, MixSupplyPumpUnit pump)
        {
            m_Control = control;
            m_PumpUnit = pump;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            m_PumpUnit.RefAct = m_Control.GetRefPumpAct(m_PumpUnit);
            m_PumpUnit.Pump.SetPumpAct(m_PumpUnit.RefAct);

            return -1;
        }
        #endregion
    }

    public class SeqMixPumpInterlock : XSeqFunction
    {
        #region Fields
        protected MixSupplyPumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadMixSupplyPumpControl m_Control;
        #endregion

        #region Contstructor
        public SeqMixPumpInterlock(ThreadMixSupplyPumpControl control, MixSupplyPumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = m_PumpUnit.Name + " INTR";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            bool checkCond = true;
            checkCond &= m_PumpUnit.Pump.IsRun();

            bool simulation = m_Simul.Device;

            double gaugeFlowRate = m_PumpUnit.Pump.GetInterlockGaugeValue();
            int setupTime = m_PumpUnit.SetupInfoPumpInterlockTime.GetValue<int>() * 1000;
            int setupFlowRate = m_PumpUnit.SetupInfoPumpInterlockFlowRate.GetValue<int>();

            //if (simulation) gaugeFlowRate = (float)setupFlowRate;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (checkCond)
                        {
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!checkCond ||
                            gaugeFlowRate >= setupFlowRate)
                        {
                            m_PumpUnit.IfFlag.InsufficientFlowrate = false;
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > setupTime)
                        {
                            m_PumpUnit.Pump.Stop();
                            m_PumpUnit.IfFlag.InsufficientFlowrate = true;
                            m_AlarmId = m_PumpUnit.ALM_PumpInterlockAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Insufficient Flow Rate: " + gaugeFlowRate.ToString());
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if (m_PumpUnit.Pump.IsRun()) m_PumpUnit.Pump.Stop();
                        if (m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Pump Insufficient Flow Rate");
                            m_PumpUnit.IfFlag.InsufficientFlowrate = false;
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqMixPumpAlarm : XSeqFunction
    {
        #region Fields
        protected MixSupplyPumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMixSupplyPumpControl m_Control;
        #endregion

        #region Contstructor
        public SeqMixPumpAlarm(ThreadMixSupplyPumpControl control, MixSupplyPumpUnit pump)
        {
            m_PumpUnit = pump;
            m_EqpManager = m_PumpUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = m_PumpUnit.Name + " ALARM";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_PumpUnit.Pump.IsUse;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_PumpUnit.Pump.IsAlarm() && checkCond)
                        {
                            m_AlarmId = m_PumpUnit.Pump.ALM_PumpAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_PumpUnit.IfFlag.Alarm = true;
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Alarm");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if ((!m_PumpUnit.Pump.IsAlarm() || !checkCond) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_PumpUnit.IfFlag.Alarm = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Pump Alarm");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqInitMixPump : XSeqInitFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadMixSupplyPumpControl m_Control;
        protected static _GenericCollection<MixSupplyPumpUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        protected InitState m_InitState = InitState.Noop;
        protected new int[] m_AlarmId;

        protected GenericTag m_InitCheckPumpAlarm = new GenericTag("Mix Supply Pump Alarm", InitCheckState.NotReady);
        #endregion

        #region Contructor
        public SeqInitMixPump(ThreadMixSupplyPumpControl control, _GenericCollection<MixSupplyPumpUnit> units)
        {
            m_Units = units;
            m_Server = m_Units.ServerManager;
            m_Eqp = m_Server.EqpStateManager;
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

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;
                        m_Server.Log("SeqMixPumpInit : Start ");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        m_InitCheckPumpAlarm.Value = InitCheckState.Checking;

                        bool bAlarm = false;

                        if (m_Server.JobCond.ProcessMode)
                        {
                            bAlarm = m_Control.IsAlarm();
                        }

                        if (!bAlarm)
                        {
                            m_InitCheckPumpAlarm.Value = InitCheckState.OK;
                            nSeqNo = 100;
                        }
                        else
                        {
                            int count = m_Units.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (m_Units[i].Pump.IsAlarm())
                                {
                                    //AlarmId = unit.Pump.ALM_PumpAlarm.Id;
                                    m_AlarmId[i] = m_Units[i].Pump.ALM_PumpAlarm.Id;
                                    m_Eqp.SetAlarm(m_AlarmId[i]);
                                }
                            }
                            m_InitState = InitState.Fail;
                            m_InitCheckPumpAlarm.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                            //break;
                        }
                    }
                    break;
                case 100:
                    {
                        m_InitState = InitState.Comp;
                        m_Server.Log("SeqMixPumpInit : Complete ");
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
}

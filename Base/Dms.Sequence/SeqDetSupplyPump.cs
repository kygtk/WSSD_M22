using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using System.Threading;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadDetSupplyPumpControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DetSupplyPumpUnit> m_DetPumpUnits;
        protected static _GenericCollection<DetergentUnit> m_DetUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_DetPumpUnits.Count == 0) return;

            foreach (DetSupplyPumpUnit pump in m_DetPumpUnits)
            {
                RegisterSequence(new SeqDetSupplyPump(this, pump));
                RegisterSequence(new SeqDetPumpAlarm(this, pump));
                //RegisterSequence(new SeqDetPumpInterlock(this, pump));
            }

            m_Server.AddSeqInitFunction(new SeqInitDetPump(this, m_DetPumpUnits));
        }
        #endregion

        #region Constructor
        public ThreadDetSupplyPumpControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_DetPumpUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DetSupplyPumpUnit>();
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
        public static _GenericCollection<DetSupplyPumpUnit> Units
        {
            get
            {
                if (m_DetPumpUnits == null) m_DetPumpUnits = new _GenericCollection<DetSupplyPumpUnit>();
                return m_DetPumpUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(DetSupplyPumpUnit pump)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            //interlock |= pump.IfFlag.InsufficientFlowrate;
            interlock |= (heavy > 0);
            return interlock;
        }

        public virtual bool IsRunEnable(DetSupplyPumpUnit pump)
        {
            bool run = true;
            run &= pump.DetPump.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !pump.IfFlag.Alarm;
            run &= (pump.DetTank == null) ? false : !pump.DetTank.IsLevelFault();
            run &= (pump.DetTank == null) ? false : pump.DetTank.IsRunEnableLevel();
            run &= (pump.DetTank == null) ? false : pump.DetTank.AutoValve.IsOpen();

            return run;
        }

        public virtual bool IsAutoRunCondition(DetSupplyPumpUnit pump)
        {
            bool run = true;
            run &= !IsInterlock(pump);
            run &= IsRunEnable(pump);
            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= m_GenInfos.DiStart;
            run &= (pump.DetTank == null) ? false : pump.DetTank.IsTankReady();
            //run &= (GetGlassCount() > 0) || ((GetGlassCount() == 0) && (m_GenInfos.IdleRunning));

            return run;
        }

        public virtual PumpAct GetRefPumpAct(DetSupplyPumpUnit pump)
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
                if (m_GenInfos.DiStart && ((pump.DetTank == null) ? false : pump.DetTank.IsTankReady())) return PumpAct.Run;
                else return pump.ManualAct;
            }
            else
            {
                pump.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
        }
        #endregion

        #region General Methods
        public bool IsAlarm()
        {
            bool alarm = false;
            foreach (DetSupplyPumpUnit device in m_DetPumpUnits)
            {
                alarm |= device.DetPump.SetupPumpUse.GetValue<bool>() && device.DetPump.IsAlarm();
                if (alarm) break;
            }

            return alarm;
        }
        #endregion
    }

    public class SeqDetSupplyPump : XSeqFunction
    {
        #region Fields
        protected static ThreadDetSupplyPumpControl m_Control;
        protected static DetSupplyPumpUnit m_DetPumpUnit;
        #endregion

        #region Constructor
        public SeqDetSupplyPump(ThreadDetSupplyPumpControl control, DetSupplyPumpUnit pump)
        {
            m_Control = control;
            m_DetPumpUnit = pump;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            m_DetPumpUnit.RefAct = m_Control.GetRefPumpAct(m_DetPumpUnit);
            m_DetPumpUnit.DetPump.SetPumpAct(m_DetPumpUnit.RefAct);

            return -1;
        }
        #endregion
    }

    /*
        public class SeqDetPumpInterlock : XSeqFunction
        {
            #region Fields
            protected DetSupplyPumpUnit m_DetPumpUnit;
            protected static IEqpManager m_EqpManager;
            protected static IServerManager m_Server;
            protected static Simul m_Simul;
            protected static ThreadDetSupplyPumpControl m_Control;
            #endregion

            #region Contstructor
            public SeqDetPumpInterlock(ThreadDetSupplyPumpControl control, DetSupplyPumpUnit pump)
            {
                m_DetPumpUnit = pump;
                m_Server = m_PumpUnit.ServerManager;
                m_Simul = AppConfig.Instance.Simul;
                m_EqpManager = m_Server.EqpStateManager;
                m_Control = control;

                SeqFunName = m_DetPumpUnit.Name +" INTR";
            }
            #endregion

            #region Sequence
            public override int Do()
            {
                int nSeqNo = this.SeqNo;
                bool checkCond = true;
                checkCond &= m_DetPumpUnit.Pump.IsRun();

                bool simulation = m_Simul.Device;

                //double gaugeFlowRate = m_PumpUnit.Pump.GetInterlockGaugeValue();
                //int setupTime = m_PumpUnit.SetupInfoPumpInterlockTime.GetValue<int>() * 1000;
                //int setupFlowRate = m_PumpUnit.SetupInfoPumpInterlockFlowRate.GetValue<int>();

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
                            if (!checkCond)
                            {
                                //m_PumpUnit.IfFlag.InsufficientFlowrate = false;
                                nSeqNo = 0;
                            }
                            else if (GetElapsedTicks() > setupTime)
                            {
                                m_PumpUnit.Pump.Stop();
                                m_PumpUnit.IfFlag.InsufficientFlowrate = true;
                                AlarmId = m_PumpUnit.ALM_PumpInterlockAlarm.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_PumpUnit.SetLog(SeqFunName, 0, 0, "Alarm Set : Pump Insufficient Flow Rate: " + gaugeFlowRate.ToString());
                                nSeqNo = 1000;
                            }
                        }
                        break;
                    case 1000:
                        {
                            if (m_PumpUnit.Pump.IsRun()) m_PumpUnit.Pump.Stop();
                            if (m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_EqpManager.ResetAlarm(AlarmId);
                                m_PumpUnit.SetLog(SeqFunName, 0, 0, "Alarm Reset : Pump Insufficient Flow Rate");
                                m_PumpUnit.IfFlag.InsufficientFlowrate = false;
                                nSeqNo = 0;
                            }
                        }
                        break;
                }
                this.SeqNo = nSeqNo;

                return -1;
            }
            #endregion
        }
        */

    public class SeqDetPumpAlarm : XSeqFunction
    {
        #region Fields
        protected DetSupplyPumpUnit m_DetPumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDetSupplyPumpControl m_Control;
        #endregion

        #region Contstructor
        public SeqDetPumpAlarm(ThreadDetSupplyPumpControl control, DetSupplyPumpUnit pump)
        {
            m_DetPumpUnit = pump;
            m_EqpManager = m_DetPumpUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = m_DetPumpUnit.Name + " ALARM";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_DetPumpUnit.DetPump.IsUse;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_DetPumpUnit.DetPump.IsAlarm() && checkCond)
                        {
                            m_AlarmId = m_DetPumpUnit.DetPump.ALM_PumpAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_DetPumpUnit.IfFlag.Alarm = true;
                            m_DetPumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Det Supply Pump Alarm");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if ((!m_DetPumpUnit.DetPump.IsAlarm() || !checkCond) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_DetPumpUnit.IfFlag.Alarm = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_DetPumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Det Supply Pump Alarm");
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

    public class SeqInitDetPump : XSeqInitFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadDetSupplyPumpControl m_Control;
        protected static _GenericCollection<DetSupplyPumpUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        protected InitState m_InitState = InitState.Noop;
        protected new int[] m_AlarmId;

        protected GenericTag m_InitCheckPumpAlarm = new GenericTag("Det Supply Pump Alarm", InitCheckState.NotReady);
        #endregion

        #region Contructor
        public SeqInitDetPump(ThreadDetSupplyPumpControl control, _GenericCollection<DetSupplyPumpUnit> units)
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
                        m_Server.Log("SeqDetPumpInit : Start ");
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
                                if (m_Units[i].DetPump.IsAlarm())
                                {
                                    //AlarmId = unit.Pump.ALM_PumpAlarm.Id;
                                    m_AlarmId[i] = m_Units[i].DetPump.ALM_PumpAlarm.Id;
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
                        m_Server.Log("SeqDetPumpInit : Complete ");
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

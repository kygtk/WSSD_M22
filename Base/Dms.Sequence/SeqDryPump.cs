using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadDryPumpControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DryPumpUnit> m_DryPumpUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_DryPumpUnits.Count == 0) return;

            foreach (DryPumpUnit device in m_DryPumpUnits)
            {
                RegisterSequence(new SeqDryPumpUnit(this, device));
                RegisterSequence(new SeqDryPumpAlarm(this, device));
            }

            m_Server.AddSeqInitFunction(new SeqInitDryPump(this, m_Server));
        }
        #endregion

        #region Contructor
        public ThreadDryPumpControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_DryPumpUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DryPumpUnit>();
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
        public static _GenericCollection<DryPumpUnit> Units
        {
            get
            {
                if (m_DryPumpUnits == null) m_DryPumpUnits = new _GenericCollection<DryPumpUnit>();
                return m_DryPumpUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(DryPumpUnit unit)
        {
            bool interlock = false;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            //interlock |= ((heavy & ~HeavyInterlock.Door) > 0 );

            //if (unit.TankLevel != null)
            //{
            //    interlock |= (unit.TankLevel.BottomLevelDetect != LevelDetect.Detect) ;
            //}

            return interlock;
        }

        public virtual bool IsRunEnable(DryPumpUnit unit)
        {
            bool run = true;
            //run &= unit.Pump.IsUse;
            //run &= m_Server.JobCond.ProcessMode;
            //run &= !unit.IfFlag.Alarm;
            //run &= IsRunEnableTankLevel(unit);

            return run;
        }

        public virtual bool IsAutoRunCondition()
        {
            bool run = true;
            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            //run &= m_GenInfos.DiStart;

            return run;
        }

        public virtual PumpAct GetRefPumpAct(DryPumpUnit unit)
        {
            bool interlock = IsInterlock(unit);
            bool runEnable = IsRunEnable(unit);
            bool autoRun = IsAutoRunCondition();
            bool autoMode = m_GenInfos.AutoMode;

            if (interlock)
            {
                unit.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
            else if (autoRun && runEnable)
            {
                unit.ManualAct = PumpAct.Stop;
                return PumpAct.Run;
            }
            else if (!autoMode)
            {
                return unit.ManualAct;
            }
            else
            {
                unit.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
        }
        #endregion

        #region General Methods
        public bool IsAlarm()
        {
            bool alarm = false;
            foreach (DryPumpUnit device in m_DryPumpUnits)
            {
                alarm |= device.DryPump.IsAlarm();
                if (alarm) break;
            }

            return alarm;
        }

        //public void SetRemoteMode()
        //{
        //foreach (DryPumpUnit device in m_DryPumpUnits)
        //{
        //    device.DryPump.DoRemoteMode.SetState(true);
        //}            
        //}


        #endregion
    }

    public class SeqInitDryPump : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static _GenericCollection<DryPumpUnit> m_Units;
        protected static ThreadDryPumpControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        #endregion

        #region Constructor
        public SeqInitDryPump(ThreadDryPumpControl control, IServerManager server)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<DryPumpUnit>();
            m_Eqp = m_Server.EqpStateManager;
            m_AlarmId = new int[m_Units.Count];
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            bool bAlarm = false;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        if (m_Server.JobCond.ProcessMode)
                        {
                            bAlarm = m_Control.IsAlarm();
                        }
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        if (!bAlarm)
                        {
                            //m_Control.SetRemoteMode();

                            nSeqNo = 100;
                        }
                        else
                        {
                            int count = m_Units.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (m_Units[i].DryPump.IsAlarm())
                                {
                                    m_AlarmId[i] = m_Units[i].DryPump.ALM_PumpAlarm.Id;
                                    m_Eqp.SetAlarm(m_AlarmId[i]);
                                }
                            }
                            m_InitState = InitState.Fail;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 100:
                    {
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
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqDryPumpUnit : XSeqFunction
    {
        #region Fields
        private DryPumpUnit m_Unit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDryPumpControl m_Control;
        #endregion

        #region Constructor
        public SeqDryPumpUnit(ThreadDryPumpControl control, DryPumpUnit pump)
        {
            m_Unit = pump;
            m_EqpManager = m_Unit.ServerManager.EqpStateManager;

            m_Control = control;
            m_SeqFunName = "DRY PUMP UNIT";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            m_Unit.RefAct = m_Control.GetRefPumpAct(m_Unit);
            m_Unit.DryPump.SetPumpAct(m_Unit.RefAct);

            return -1;
        }
        #endregion
    }

    public class SeqDryPumpAlarm : XSeqFunction
    {
        #region Fields
        private DryPumpUnit m_Unit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDryPumpControl m_Control;
        #endregion

        #region Constructor
        public SeqDryPumpAlarm(ThreadDryPumpControl control, DryPumpUnit pump)
        {
            m_Unit = pump;
            m_EqpManager = m_Unit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "PUMP ALARM";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_Unit.DryPump.IsUse;

            m_Unit.RefAct = m_Control.GetRefPumpAct(m_Unit);

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Unit.DryPump.IsAlarm() && checkCond)
                        {
                            m_AlarmId = m_Unit.DryPump.ALM_PumpAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Unit.IfFlag.Alarm = true;
                            m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Alarm");
                            nSeqNo = 1000;
                        }
                        else if (m_Unit.DryPump.IsWarning() && checkCond)
                        {
                            m_AlarmId = m_Unit.DryPump.ALM_PumpWarning.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Unit.IfFlag.Alarm = true;
                            m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Warning");
                            nSeqNo = 2000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if ((!m_Unit.DryPump.IsAlarm() || !checkCond) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_Unit.IfFlag.Alarm = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Pump Alarm");
                            nSeqNo = 0;
                        }
                    }
                    break;

                case 2000:
                    if ((!m_Unit.DryPump.IsWarning() || !checkCond) && m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_Unit.IfFlag.Alarm = false;
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Pump Alarm");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}


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
    public class ThreadTankControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<TankUnit> m_TankUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (TankUnit device in m_TankUnits)
            {
                RegisterSequence(new SeqTankUnit(this, device));
                RegisterSequence(new SeqTankLevel(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadTankControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_TankUnits = DmsComponents.Instance.ComponentContainer.GetCollection<TankUnit>();
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
        public static _GenericCollection<TankUnit> Units
        {
            get
            {
                if (m_TankUnits == null) m_TankUnits = new _GenericCollection<TankUnit>();
                return m_TankUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(TankUnit tank)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= (heavy > 0);
            interlock |= tank.LevelFault;
            return interlock;
        }

        public virtual bool IsSeqRunCondition(TankUnit tank)
        {
            bool stop = false;
            stop |= !IsRunCondition(tank);
            stop |= (m_GenInfos.AutoMode && !m_GenInfos.EqpInitComp);// TODO:Tank의 SeqInit을 보아야 함.
            stop |= (!m_GenInfos.AutoMode &&
                     !m_GenInfos.DiStart);
            return !stop;
        }

        public virtual bool IsRunCondition(TankUnit tank)
        {
            bool run = true;
            run &= !IsInterlock(tank);
            run &= m_Server.JobCond.ProcessMode;
            run &= tank.PumpUse;
            return run;
        }
        #endregion
    }

    public class SeqTankUnit : XSeqFunction
    {
        #region Fields
        protected TankUnit m_TankUnit;
        protected TagTankIfFlag m_IfFlag;
        protected static IServerManager m_Server;
        protected static ThreadTankControl m_Control;
        protected int m_SimulSensor = 0;
        #endregion

        #region Constructor
        public SeqTankUnit(ThreadTankControl control, TankUnit tank)
        {
            m_TankUnit = tank;
            m_IfFlag = m_TankUnit.IfFlag;
            m_Server = m_TankUnit.ServerManager;
            m_Control = control;

            m_SeqFunName = "TANK UNIT";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool runCond = true;
            runCond &= m_Control.IsRunCondition(m_TankUnit);

            if (!runCond || m_TankUnit.SupplyStop || m_TankUnit.TopLevel)
            {
                if (m_TankUnit.AutoValve.IsOpen())
                {
                    m_TankUnit.AutoValve.Close();
                    if (!runCond)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close : Run Condition(FALSE)");
                    if (m_TankUnit.SupplyStop)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close : H Level Exist");
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close");
                }
            }

            bool notReady = false;
            notReady |= !m_TankUnit.RunEnable;
            notReady |= !m_TankUnit.BottomLevel;
            notReady |= m_TankUnit.LevelFault;

            if (notReady && m_IfFlag.TankReady)
            {
                m_IfFlag.TankReady = false;
                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "TankReady : False");
            }

            //시퀀스가 안돌 조건이면 return
            if (!m_Control.IsSeqRunCondition(m_TankUnit)) return -1;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (runCond &&
                            (!m_TankUnit.TopLevel ||
                             !m_TankUnit.SupplyStop) &&
                            (!m_TankUnit.SupplyRequest ||
                             !m_TankUnit.RunEnable ||
                             !m_TankUnit.BottomLevel))
                        {
                            if (m_TankUnit.AutoValve.IsClose())
                            {
                                m_TankUnit.AutoValve.Open();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Open");

                                if (AppConfig.Instance.Simul.Device)
                                {
                                    //StartTime = DateTime.Now;
                                    m_StartTicks = XFunc.GetTickCount();

                                    m_SimulSensor = 0;
                                }
                            }

                        }
                        else if (!runCond ||
                                 m_TankUnit.TopLevel ||
                                 m_TankUnit.SupplyStop)
                        {
                            if (m_TankUnit.AutoValve.IsOpen())
                            {
                                m_TankUnit.AutoValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close");
                            }
                        }
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if ((!m_TankUnit.RunEnable ||
                             !m_TankUnit.BottomLevel) &&
                             m_IfFlag.TankReady)
                        {
                            m_IfFlag.TankReady = false;
                        }
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        if (AppConfig.Instance.Simul.Device)
                        {
                            if ((GetElapsedTicks() > 4000) && m_TankUnit.AutoValve.IsOpen() && !m_TankUnit.SupplyStop)
                            {
                                if (m_SimulSensor < m_TankUnit.TankLevel.Sensors.Count)
                                {
                                    //StartTime = DateTime.Now;
                                    m_StartTicks = XFunc.GetTickCount();

                                    m_TankUnit.TankLevel.Sensors[m_SimulSensor].SetState(true);

                                    m_SimulSensor++;
                                }
                            }
                        }
                        if (m_TankUnit.SupplyRequest &&
                            m_TankUnit.RunEnable &&
                            m_TankUnit.BottomLevel &&
                            !m_IfFlag.TankReady)
                        {
                            m_IfFlag.TankReady = true;
                        }
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqTankLevel : XSeqFunction
    {
        #region Fields
        protected TankUnit m_TankUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadTankControl m_Control;
        #endregion

        #region Constructor
        public SeqTankLevel(ThreadTankControl control, TankUnit tank)
        {
            m_TankUnit = tank;
            m_EqpManager = m_TankUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "TANK LEVEL";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_TankUnit.TopLevel &&
                           (!m_TankUnit.SupplyStop ||
                            !m_TankUnit.SupplyRequest ||
                            !m_TankUnit.RunEnable ||
                            !m_TankUnit.BottomLevel))
                        {
                            if (m_TankUnit.AutoValve.IsOpen())
                            {
                                m_TankUnit.AutoValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            }

                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Top Level : Fault");
                        }
                        else if (m_TankUnit.TopLevel)
                        {
                            if (m_TankUnit.AutoValve.IsOpen())
                            {
                                m_TankUnit.AutoValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            }

                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_AlarmId = m_TankUnit.ALM_TankHighLevel.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Tank Top Level Alarm");
                            nSeqNo = 2000;
                            break;
                        }
                        else if (m_TankUnit.RunEnable && !m_TankUnit.BottomLevel)
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank RunEnable Level : Fault");
                        }
                        else if (m_TankUnit.SupplyRequest &&
                                (!m_TankUnit.RunEnable ||
                                 !m_TankUnit.BottomLevel))
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank SupplyRequest Level : Fault");
                        }
                        else if (m_TankUnit.SupplyStop &&
                                (!m_TankUnit.SupplyRequest ||
                                 !m_TankUnit.RunEnable ||
                                 !m_TankUnit.BottomLevel))
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank SupplyStop Level : Fault");
                        }
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_TankUnit.IfFlag.TankLevelFault == true)
                        {
                            m_AlarmId = m_TankUnit.ALM_LevelFault.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Tank Level Fault Alarm");
                            nSeqNo = 1000;
                        }
                        else nSeqNo = 0;
                    }
                    break;
                case 1000:
                    {
                        if (m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_TankUnit.IfFlag.TankLevelFault = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Tank Level Fault Alarm");
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 2000:
                    {
                        if (!m_TankUnit.TopLevel && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_TankUnit.IfFlag.TankLevelFault = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Tank Top Level Alarm");
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
}

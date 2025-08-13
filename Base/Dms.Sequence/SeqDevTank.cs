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
    public class ThreadDevTankControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DevTankUnit> m_TankUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (DevTankUnit device in m_TankUnits)
            {
                RegisterSequence(new SeqDevTankUnit(this, device));
                RegisterSequence(new SeqDevTankLevel(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadDevTankControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_TankUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DevTankUnit>();
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
        public static _GenericCollection<DevTankUnit> Units
        {
            get
            {
                if (m_TankUnits == null) m_TankUnits = new _GenericCollection<DevTankUnit>();
                return m_TankUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(DevTankUnit devTank)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy &= ~HeavyInterlock.Door) > 0);
            interlock |= devTank.LevelFault;
            return interlock;
        }

        public virtual bool IsRunCondition(DevTankUnit devTank)
        {
            bool run = true;
            run &= !IsInterlock(devTank);
            run &= m_Server.JobCond.ProcessMode;
            run &= devTank.OwnerPump.IsUse;
            return run;
        }

        public virtual bool IsSeqRunCondition(DevTankUnit devTank)
        {
            bool stop = false;
            stop |= !IsRunCondition(devTank);
            stop |= (m_GenInfos.AutoMode && !m_GenInfos.EqpInitComp);// TODO:Tank의 SeqInit을 보아야 함.
            stop |= (!m_GenInfos.AutoMode &&
                     !m_GenInfos.DiStart &&
                     !m_GenInfos.EqpInitComp);

            return !stop;
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqDevTankUnit : XSeqFunction
    {
        #region Fields
        private DevTankUnit m_TankUnit;
        private TagTankIfFlag m_IfFlag;
        protected static IServerManager m_Server;
        protected static ThreadDevTankControl m_Control;
        private int m_SimulSensor = 0;
        //private bool m_SupplyValveOpen = false;
        #endregion

        #region Constructor
        public SeqDevTankUnit(ThreadDevTankControl control, DevTankUnit tank)
        {
            m_TankUnit = tank;
            m_Server = m_TankUnit.ServerManager;
            m_IfFlag = m_TankUnit.IfFlag;
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
                if (m_TankUnit.DiInValve.IsOpen())
                {
                    m_TankUnit.DiInValve.Close();
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
                            if (m_TankUnit.DiInValve.IsClose())
                            {

                                m_TankUnit.DiInValve.Open();

                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Open");

                                m_StartTicks = XFunc.GetTickCount();

                                if (AppConfig.Instance.Simul.Device)
                                {
                                    //StartTime = DateTime.Now;
                                    m_SimulSensor = 0;
                                }
                            }

                        }
                        else if (!runCond ||
                                 m_TankUnit.TopLevel ||
                                 m_TankUnit.SupplyStop)
                        {
                            if (m_TankUnit.DiInValve.IsOpen())
                            {
                                m_TankUnit.DiInValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close");
                            }
                        }

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if ((!m_TankUnit.RunEnable || !m_TankUnit.BottomLevel) &&
                             m_IfFlag.TankReady && !m_TankUnit.IsRunEnableTemprature())
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
                            if ((GetElapsedTicks() > 4000) && m_TankUnit.DiInValve.IsOpen() && !m_TankUnit.SupplyStop)
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
                            !m_IfFlag.TankReady &&
                            m_TankUnit.IsRunEnableTemprature())
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

    public class SeqDevTankLevel : XSeqFunction
    {
        #region Fields
        private DevTankUnit m_TankUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDevTankControl m_Control;
        #endregion

        #region Constructor
        public SeqDevTankLevel(ThreadDevTankControl control, DevTankUnit tank)
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
                            if (m_TankUnit.DiInValve.IsOpen())
                            {
                                m_TankUnit.DiInValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            }

                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Top Level : Fault");
                        }
                        else if (m_TankUnit.TopLevel)
                        {
                            if (m_TankUnit.DiInValve.IsOpen())
                            {
                                m_TankUnit.DiInValve.Close();
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

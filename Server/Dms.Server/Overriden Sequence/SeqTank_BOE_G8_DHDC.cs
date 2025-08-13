using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Data;
using Dms.Sequence;

namespace Dms.Server
{
    public class ThreadTankControl_BOE_G8_DHDC : ThreadTankControl
    {
        #region Fields
        private TankUnit m_Tank;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (TankUnit device in m_TankUnits)
            {
                RegisterSequence(new SeqTankUnit(this, device));
                RegisterSequence(new SeqTankLevel(this, device));
                m_Tank = device;
            }
        }
        #endregion

        #region Constructor
        public ThreadTankControl_BOE_G8_DHDC(int scanTime, IServerManager server)
            : base(scanTime, server)
        {
            //TankLevels = new bool[4];
            //m_Server = server;
            //m_TankUnits = m_Server.ComponentContainer.GetCollection<TankUnit>() ;
            //m_GenInfos = GenInfoHandler.Instance

            //RegisterSequences();
        }
        #endregion
        #region Properties
        public bool TopLevel
        {
            get { return m_Tank.SupplyStop; }
        }
        public bool SupplyStop
        {
            get { return m_Tank.SupplyRequest; }
        }
        public bool SupplyRequest
        {
            get { return m_Tank.RunEnable; }
        }
        public bool RunEnable
        {
            get { return m_Tank.BottomLevel; }
        }
        public bool BottomLevel
        {
            get { return m_Tank.BottomLevel; }
        }
        #endregion

        #region override Methods
        public override bool IsInterlock(TankUnit tank)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= (heavy > 0);
            interlock |= tank.LevelFault;
            return interlock;
        }

        public override bool IsSeqRunCondition(TankUnit tank)
        {
            bool stop = false;
            stop |= !IsRunCondition(tank);
            stop |= (m_GenInfos.AutoMode && !m_GenInfos.EqpInitComp);// TODO:Tank의 SeqInit을 보아야 함.
            stop |= (!m_GenInfos.AutoMode && !m_GenInfos.DiStart);
            return !stop;
        }

        public override bool IsRunCondition(TankUnit tank)
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
        protected static ThreadTankControl_BOE_G8_DHDC m_Control;
        protected int m_SimulSensor = 0;
        #endregion

        #region Constructor
        public SeqTankUnit(ThreadTankControl_BOE_G8_DHDC control, TankUnit tank)
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

            if (!runCond || m_Control.SupplyStop || m_Control.TopLevel)
            {
                if (m_TankUnit.AutoValve.IsOpen())
                {
                    m_TankUnit.AutoValve.Close();
                    if (!runCond)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close : Run Condition(FALSE)");
                    if (m_Control.SupplyStop)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close : H Level Exist");
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Valve Close");
                }
            }
            bool notReady = false;
            notReady |= !m_Control.RunEnable;
            notReady |= !m_Control.BottomLevel;
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
                            (!m_Control.TopLevel ||
                             !m_Control.SupplyStop) &&
                            (!m_Control.SupplyRequest ||
                             !m_Control.RunEnable ||
                             !m_Control.BottomLevel))
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
                                 m_Control.TopLevel ||
                                 m_Control.SupplyStop)
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
                        if ((!m_Control.RunEnable ||
                             !m_Control.BottomLevel) &&
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
                        if (m_Control.SupplyRequest &&
                            m_Control.RunEnable &&
                            m_Control.BottomLevel &&
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
        protected static ThreadTankControl_BOE_G8_DHDC m_Control;
        protected ServerManager m_Server;
        private Alarm ALM_TankHighLevel;
        #endregion

        #region Constructor
        public SeqTankLevel(ThreadTankControl_BOE_G8_DHDC control, TankUnit tank)
        {
            m_TankUnit = tank;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_TankUnit.ServerManager.EqpStateManager;
            m_Control = control;
            ALM_TankHighLevel = new Alarm(m_TankUnit.Name + " High Level Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_SeqFunName = "TANK LEVEL";
        }
        #endregion
        #region Methods
        //public bool SensorOffConfirm()

        #endregion

        #region 원본
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            int OffDelay = eqpLevelSensors._FR_Unit_Tank_H_Level.SetupLevelSensorConfirmTime.GetValue<int>() * 1000;
            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Control.TopLevel &&
                           (!m_Control.SupplyStop ||
                            !m_Control.SupplyRequest ||
                            !m_Control.RunEnable ||
                            !m_Control.BottomLevel))
                        {
                            if (m_TankUnit.AutoValve.IsOpen())
                            {
                                m_TankUnit.AutoValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            }
                            if (!m_Control.SupplyStop) { m_TankUnit.SetLog(m_SeqFunName, 0, 0, "The third Sensor NG"); } // 12.11.07 wang
                            if (!m_Control.SupplyRequest || !m_Control.RunEnable) { m_TankUnit.SetLog(m_SeqFunName, 0, 0, "The Second Sensor NG"); } // 12.11.07 wang
                            if (!m_Control.BottomLevel) { m_TankUnit.SetLog(m_SeqFunName, 0, 0, "The first Sensor NG"); } // 12.11.07 wang
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Top Level : Fault");
                            nSeqNo = 10;
                            break;
                        }
                        else if (m_Control.TopLevel)
                        {
                            if (m_TankUnit.AutoValve.IsOpen())
                            {
                                m_TankUnit.AutoValve.Close();
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            }

                            //m_TankUnit.IfFlag.TankLevelFault = true;
                            GlobalVar.HighLevelDetectInterlock = true;
                            m_AlarmId = ALM_TankHighLevel.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Tank Top Level Alarm");
                            nSeqNo = 2000;
                            break;
                        }
                        else if (m_Control.RunEnable && !m_Control.BottomLevel)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                        }
                        else if (m_Control.SupplyRequest &&
                           (!m_Control.RunEnable ||
                            !m_Control.BottomLevel))
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                        }
                        else if (m_Control.SupplyStop &&
                           (!m_Control.SupplyRequest ||
                            !m_Control.RunEnable ||
                            !m_Control.BottomLevel))
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                        }
                        //  nSeqNo = 10;
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
                case 100:
                    if (GetElapsedTicks() > OffDelay)
                    {//2010.09.10 kimgun off도 delay를 봐달라캐서...base 수정하면 한줄이면 되는데...
                        if (m_Control.RunEnable && !m_Control.BottomLevel)
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank RunEnable Level : Fault");
                            nSeqNo = 10;
                        }
                        else if (m_Control.SupplyRequest &&
                               (!m_Control.RunEnable ||
                                !m_Control.BottomLevel))
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank SupplyRequest Level : Fault");
                            nSeqNo = 10;
                        }
                        else if (m_Control.SupplyStop &&
                              (!m_Control.SupplyRequest ||
                               !m_Control.RunEnable ||
                               !m_Control.BottomLevel))
                        {
                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank SupplyStop Level : Fault");
                            nSeqNo = 10;
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
                        if (!m_Control.TopLevel && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            //m_TankUnit.IfFlag.TankLevelFault = false;
                            GlobalVar.HighLevelDetectInterlock = false;
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

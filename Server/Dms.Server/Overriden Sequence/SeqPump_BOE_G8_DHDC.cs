using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;
using Dms.Data; // 11.02.01 minhan
using Dms.Sequence;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadPumpControl_BOE_G8_DHDC : ThreadPumpControl
    {
        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_PumpUnits.Count == 0) return;

            foreach (PumpUnit device in m_PumpUnits)
            {
                RegisterSequence(new SeqPumpUnit(this, device));
                RegisterSequence(new SeqPumpAlarm(this, device));
                RegisterSequence(new SeqPumpInterlock(this, device));
            }
            m_Server.AddSeqInitFunction(new SeqInitPump(this, m_PumpUnits));
        }
        #endregion

        #region Contructor
        public ThreadPumpControl_BOE_G8_DHDC(int scanTime, IServerManager server)
            : base(scanTime, server)
        {
            m_Server = server;
            //m_PumpUnits = m_Server.ComponentContainer.GetCollection<PumpUnit>();
            //m_GenInfos = GenInfoHandler.Instance

            //  RegisterSequences();
        }
        #endregion

        #region Override Methods
        public override bool IsInterlock(PumpUnit pump)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            //int glassNo = 0;
            interlock |= pump.IfFlag.InsufficientFlowrate;
            //2010.06.29 kimgun leak는 세정구간에 glass가 있으면 일단 정상으로 취급한다.유저요청. // 11.01.27 minhan 없앤다 컨셉이 다르다.
            //for (int i = eqpTransferUnits._RB_CvUnit.DataMatchingKey(0); i <= eqpTransferUnits._UL_CvUnit.DataMatchingKey(0); i++) // minhan
            //{
            //    if (m_Server.GlassData.IsExist(i)) glassNo++;
            //}
            //if (glassNo == 0) 
            //{
            interlock |= (heavy > 0);
            interlock |= GlobalVar.HighLevelDetectInterlock;//2010.09.09 kimgun 세정구간의 glass는 일단 빼고 ld부만 출발 안 시킨다.

            if (m_Server.JobCond.CurrentRecipe.MjUse && (!m_GenInfos.AutoMode || !GlobalVar.UlTimeOut)) // 11.04.27 minhan
            {
                interlock |= !eqpAutoValves._RB_MJ_CDA_In_Valve.IsOpen(); // 11.03.26 minhan
                //interlock |= eqpGauges._RB_Unit_MJ_CDA_Pressure_Gauge.IsAlarm; // 11.03.25 minhan 현재 recipe 쪽은 협의를 해야한다.
                interlock |= GlobalVar.MjPressureLowAlarm; // 11.04.08 minhan
            }

            //}
            //else
            //    interlock |= ((heavy & ~HeavyInterlock.Leak) > 0) ;

            interlock |= pump.Tank.IfFlag.TankLevelFault;//2010.09.09 kimgun 무조건 정지
            // interlock |= (heavy > 0) ;
            return interlock;
        }
        public override bool IsRunEnable(PumpUnit pump)
        {
            bool run = true;
            run &= pump.Pump.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !pump.IfFlag.Alarm;
            run &= (pump.Tank == null) ? true : !pump.Tank.IsLevelFault();
            run &= (pump.Tank == null) ? true : pump.Tank.BottomLevel;//.IsRunEnableLevel();

            return run;
        }
        #endregion
    }

    public class SeqPumpUnit : XSeqFunction
    {
        #region Fields
        protected PumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadPumpControl m_Control;
        #endregion

        #region Contructor
        public SeqPumpUnit(ThreadPumpControl control, PumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "PUMP UNIT";
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

    public class SeqPumpInterlock : XSeqFunction
    {
        #region Fields
        protected PumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadPumpControl m_Control;
        #endregion

        #region Contstructor
        public SeqPumpInterlock(ThreadPumpControl control, PumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "PUMP INTR";
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

    public class SeqPumpAlarm : XSeqFunction // 11.02.01 minhan
    {
        #region Fields
        protected PumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadPumpControl m_Control;
        private bool checkCond;
        private bool checkRun;
        private bool checkMcSensor;
        private bool m_McOn;
        private Alarm m_AlarmPumpMc;
        #endregion

        #region Contstructor
        public SeqPumpAlarm(ThreadPumpControl control, PumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_EqpManager = m_PumpUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "PUMP ALARM";
            checkCond = true;
            checkRun = false;
            checkMcSensor = false;
            m_McOn = false;
            m_AlarmPumpMc = new Alarm(m_SeqFunName + " MC ON Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            checkCond = m_PumpUnit.Pump.IsUse;
            checkRun = m_PumpUnit.Pump.IsRun();
            checkMcSensor = eqpSensors._RB_Unit_PUMP_ON_Sensor.IsDetected();

            if (AppConfig.Instance.Simul.Device) eqpSensors._RB_Unit_PUMP_ON_Sensor.SetState(true);

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
                        else if (!checkRun)
                        {
                            m_McOn = false;
                        }
                        else if (checkRun && !m_McOn)
                        {
                            m_McOn = true;
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (checkRun && !checkMcSensor && (GetElapsedTicks() > 4000))
                        {
                            m_AlarmId = m_AlarmPumpMc.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_PumpUnit.IfFlag.Alarm = true;
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Mc Alarm");
                            nSeqNo = 2000;
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
                case 2000:
                    {
                        if ((checkMcSensor || !checkRun) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_PumpUnit.IfFlag.Alarm = false;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Pump Mc Alarm");
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


    public class SeqInitPump : XSeqInitFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadPumpControl m_Control;
        protected static _GenericCollection<PumpUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        protected InitState m_InitState = InitState.Noop;
        protected new int[] m_AlarmId;

        protected GenericTag m_InitCheckPumpAlarm = new GenericTag("Pump Alarm", InitCheckState.NotReady);
        #endregion

        #region Contructor
        public SeqInitPump(ThreadPumpControl control, _GenericCollection<PumpUnit> units)
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
                        m_Server.Log("SeqPumpInit : Start ");
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
                        m_Server.Log("SeqPumpInit : Complete ");
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

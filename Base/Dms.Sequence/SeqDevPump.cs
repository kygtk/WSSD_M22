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
    public class ThreadDevPumpControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DevPumpUnit> m_PumpUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_PumpUnits.Count == 0) return;

            foreach (DevPumpUnit device in m_PumpUnits)
            {
                RegisterSequence(new SeqDevPumpUnit(this, device));
                RegisterSequence(new SeqDevPumpAlarm(this, device));
                RegisterSequence(new SeqDevPumpInterlock(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadDevPumpControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_PumpUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DevPumpUnit>();
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
        public static _GenericCollection<DevPumpUnit> Units
        {
            get
            {
                if (m_PumpUnits == null) m_PumpUnits = new _GenericCollection<DevPumpUnit>();
                return m_PumpUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(DevPumpUnit devPump)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= devPump.IfFlag.InsufficientFlowrate;
            interlock |= ((heavy & ~HeavyInterlock.Door) > 0);
            return interlock;
        }

        public virtual bool IsRunEnable(DevPumpUnit devPump)
        {
            bool run = true;
            run &= devPump.Pump.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !devPump.IfFlag.Alarm;
            run &= (devPump.Tank == null) ? true : !devPump.Tank.IsLevelFault();
            run &= (devPump.Tank == null) ? true : devPump.Tank.IsRunEnableLevel();
            //Heater Use 
            // run &= (devPump.Tank.HeaterUnit == null) ? true : devPump.Tank.IsTankReady();

            return run;
        }

        public virtual bool IsAutoRunCondition(DevPumpUnit devPump)
        {
            bool run = true;
            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= (devPump.Tank == null) ? true : ThreadHeaterControl.IsRunTemperature(devPump.Tank.HeaterUnit);

            return run;
        }

        public virtual PumpAct GetRefPumpAct(DevPumpUnit devPump)
        {
            bool interlock = IsInterlock(devPump);
            bool runEnable = IsRunEnable(devPump);
            bool autoRun = IsAutoRunCondition(devPump);
            bool autoMode = m_GenInfos.AutoMode;

            if (interlock)
            {
                devPump.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
            else if (autoRun && runEnable)
            {
                devPump.ManualAct = PumpAct.Stop;
                return PumpAct.Run;
            }
            else if (!autoMode && runEnable)
            {
                //if (m_GenInfos.DiStart && ((Tank == null) ? true : Tank.IsTankReady())) return PumpAct.Run;
                //else return ManualAct;

                return devPump.ManualAct;
            }
            else
            {
                devPump.ManualAct = PumpAct.Stop;
                return PumpAct.Stop;
            }
        }

        public virtual int GetGlassCount()
        {
            int glassNo = 0;

            //for (int i = eqpCvUnits._DEV1_1_CvUnit.Id * 2 + 1; i < eqpCvUnits._SHW1_1_CvUnit.Id * 2; i++)
            //{
            //    if (m_Server.GlassData.IsExist(i)) glassNo++;
            //}

            return glassNo;
        }

        public virtual void ValveControl()
        {
            //eqpAutoValves._DEV_Bath_SHW_Valve.Open();

            //if (GetGlassCount() > 0)
            //{
            //    eqpAutoValves._DEV1_Nozzle1_Valve.Open();
            //    eqpAutoValves._DEV1_Nozzle2_Valve.Open();
            //    eqpAutoValves._DEV1_Bar_Valve.Open();
            //    eqpAutoValves._DEV4_SHW_Valve.Open();
            //}
            //else
            //{
            //    eqpAutoValves._DEV1_Nozzle1_Valve.Close();
            //    eqpAutoValves._DEV1_Nozzle2_Valve.Close();
            //    eqpAutoValves._DEV1_Bar_Valve.Close();
            //    eqpAutoValves._DEV4_SHW_Valve.Close();
            //}        
        }
        #endregion

        #region General Methods
        public bool IsAlarm()
        {
            bool alarm = false;
            foreach (DevPumpUnit device in m_PumpUnits)
            {
                alarm |= device.Pump.IsAlarm();
                if (alarm) break;
            }

            return alarm;
        }
        #endregion
    }

    public class SeqDevPumpUnit : XSeqFunction
    {
        #region Fields
        private DevPumpUnit m_PumpUnit;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDevPumpControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqDevPumpUnit(ThreadDevPumpControl control, DevPumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "PUMP UNIT";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            m_PumpUnit.RefAct = m_Control.GetRefPumpAct(m_PumpUnit);

            if (m_GenInfos.AutoMode)
            {
                m_Control.ValveControl();
            }

            m_PumpUnit.Pump.SetPumpAct(m_PumpUnit.RefAct);

            return -1;
        }
        #endregion
    }

    public class SeqDevPumpInterlock : XSeqFunction
    {
        #region Fields
        private DevPumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadDevPumpControl m_Control;
        #endregion

        #region Constructor
        public SeqDevPumpInterlock(ThreadDevPumpControl control, DevPumpUnit pump)
        {
            m_PumpUnit = pump;
            m_Server = m_PumpUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
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

            double gaugeFlowRate = m_PumpUnit.Pump.GetInterlockGaugeMaxValue();
            int setupTime = m_PumpUnit.SetupInfoPumpInterlockTime.GetValue<int>() * 1000;
            int setupFlowRate = m_PumpUnit.SetupInfoPumpInterlockFlowRate.GetValue<int>();

            //if (simulation) gaugeFlowRate = (float)setupFlowRate;
            //if (PumpUnit.Name == eqpDevPumpUnits._DEV_Pump_Unit_Name) gaugeFlowRate = (float)setupFlowRate;


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
                            m_PumpUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Pump Insufficient Flow Rate");
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

    public class SeqDevPumpAlarm : XSeqFunction
    {
        #region Fields
        private DevPumpUnit m_PumpUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDevPumpControl m_Control;
        #endregion

        #region Contructor
        public SeqDevPumpAlarm(ThreadDevPumpControl control, DevPumpUnit pump)
        {
            m_PumpUnit = pump;
            m_EqpManager = m_PumpUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "PUMP ALARM";
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

    public class SeqInitDevPump : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadDevPumpControl m_Control;
        protected static _GenericCollection<DevPumpUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;

        protected GenericTag m_InitCheckDevPumpAlarm = new GenericTag("DevPump Alarm", InitCheckState.NotReady);
        #endregion

        #region Contructor
        public SeqInitDevPump(ThreadDevPumpControl control, _GenericCollection<DevPumpUnit> units)
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

            bool bAlarm = false;
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;

                        m_InitCheckDevPumpAlarm.Value = InitCheckState.Checking;
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_Server.JobCond.ProcessMode)
                        {
                            if (m_Server.JobCond.ProcessMode)
                            {
                                bAlarm = m_Control.IsAlarm();
                            }
                        }
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        if (!bAlarm)
                        {
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
                            m_InitCheckDevPumpAlarm.Value = InitCheckState.NG;

                            nSeqNo = 1000;
                            //break;
                        }
                    }
                    break;
                case 100:
                    {
                        m_InitState = InitState.Comp;

                        m_InitCheckDevPumpAlarm.Value = InitCheckState.OK;
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
}

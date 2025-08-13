using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Dms.Device;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadHeatExchangerControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server = null;
        protected static _GenericCollection<HeatExchanger> m_HeatExchangers;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (HeatExchanger device in m_HeatExchangers)
            {
                m_Server.AddSeqInitFunction(new SeqInitHeatExchanger(this, m_HeatExchangers));

                RegisterSequence(new SeqHeatExchanger(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadHeatExchangerControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_HeatExchangers = DmsComponents.Instance.ComponentContainer.GetCollection<HeatExchanger>();
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
        public static _GenericCollection<HeatExchanger> Units
        {
            get
            {
                if (m_HeatExchangers == null) m_HeatExchangers = new _GenericCollection<HeatExchanger>();
                return m_HeatExchangers;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual void CheckStopCountCond()
        {
        }

        public virtual bool IsAlarmCondition(HeaterUnit heater)
        {
            bool ng = false;
            ng |= heater.Tic.IsAlarm();
            ng |= heater.Heater.IsAlarm();
            return ng;
        }

        //public virtual bool IsInterlock(HeaterUnit heater)
        //{
        //    HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
        //    bool interlock = false;
        //    interlock |= ((heavy & ~HeavyInterlock.Door) > 0 );
        //    interlock |= (heater.TankLevel.RunEnableConfirm == LevelConfirm.Confirm ? false : true);

        //    return interlock;
        //}

        //public virtual bool IsRunEnable(HeaterUnit heater)
        //{
        //    bool run = true;
        //    run &= heater.Heater.IsUse;
        //    run &= m_Server.JobCond.ProcessMode;
        //    run &= !IsAlarmCondition(heater);
        //    run &= (heater.TankLevel == null) ? true : IsRunEnableLevel(heater);

        //    return run;
        //}

        //public virtual bool IsAutoRunCondition(HeaterUnit heater)
        //{
        //    bool run = true;

        //    run &= m_GenInfos.AutoMode;
        //    run &= m_GenInfos.EqpInitComp;
        //    run &= !IsInterlock(heater);
        //    run &= IsRunEnable(heater);

        //    //run &= m_Server.GenInfo.Eqp.ReadyComp;
        //    //run &= (m_Server.GlassData.Count > 0 );
        //    //run &= m_GenInfos.DiStart;
        //    //run &= (Tank == null) ? true : Tank.IsTankReady();

        //    return run;
        //}

        //public virtual bool IsRunEnableLevel(HeaterUnit heater)
        //{
        //    if (heater.TankLevel.RunEnableConfirm == LevelConfirm.Confirm) return true;
        //    else return false;
        //}

        //public virtual HeaterStatus CheckTemp(HeaterUnit heater)
        //{
        //    double useTemp = 0;
        //    double overTemp = heater.Heater.SetupHeaterOverTemp;
        //    double marginTemp = heater.Heater.SetupHeaterMarginTemp;
        //    double curTemp = (heater.Tic.GetPvValue(0) + heater.Tic.GetPvValue(1)) / 2;

        //    if (m_GenInfos.AutoMode) useTemp = heater.Heater.SetupHeaterTemp;
        //    else useTemp = heater.ManualTemp;


        //    if (curTemp < useTemp - marginTemp) return HeaterStatus.Heating;
        //    if (curTemp >= useTemp - marginTemp && curTemp <= useTemp + marginTemp) return HeaterStatus.Ready;
        //    if (curTemp > useTemp + marginTemp) return HeaterStatus.Alarm;

        //    return HeaterStatus.NotReady;
        //}

        //public virtual HeaterAct GetRefHeaterAct(HeaterUnit heaterUnit)
        //{
        //    bool interlock = IsInterlock(heaterUnit);
        //    bool runEnable = IsRunEnable(heaterUnit);
        //    bool autoRun = IsAutoRunCondition(heaterUnit);
        //    bool autoMode = m_GenInfos.AutoMode;

        //    if (!heaterUnit.Heater.IsUse)
        //    {
        //        heaterUnit.HeaterStatus = HeaterStatus.NoUse;
        //        return HeaterAct.Off;
        //    }

        //    if (interlock)
        //    {
        //        heaterUnit.ManualAct = HeaterAct.Off;
        //        return HeaterAct.Off;
        //    }
        //    else if (autoRun)
        //    {
        //        if (CheckTemp(heaterUnit) == HeaterStatus.Heating)
        //        {
        //            heaterUnit.ManualAct = HeaterAct.Off;
        //            heaterUnit.HeaterStatus = HeaterStatus.Heating;
        //            return HeaterAct.On;
        //        }
        //        else if (CheckTemp(heaterUnit) == HeaterStatus.Ready)
        //        {
        //            heaterUnit.ManualAct = HeaterAct.Off;
        //            heaterUnit.HeaterStatus = HeaterStatus.Ready;
        //            return HeaterAct.On;
        //        }
        //        else if (CheckTemp(heaterUnit) == HeaterStatus.Alarm)
        //        {
        //            heaterUnit.ManualAct = HeaterAct.Off;
        //            heaterUnit.HeaterStatus = HeaterStatus.Alarm;
        //            return HeaterAct.Off;
        //        }
        //        else return HeaterAct.Noop;
        //    }
        //    else if (!autoRun)
        //    {
        //        if (heaterUnit.ManualAct == HeaterAct.Off)
        //        {
        //            heaterUnit.HeaterStatus = HeaterStatus.Off;
        //        }
        //        else if (CheckTemp(heaterUnit) == HeaterStatus.Heating)
        //        {
        //            heaterUnit.HeaterStatus = HeaterStatus.Heating;
        //        }
        //        else if (CheckTemp(heaterUnit) == HeaterStatus.Ready)
        //        {
        //            heaterUnit.HeaterStatus = HeaterStatus.Ready;
        //        }
        //        else if (CheckTemp(heaterUnit) == HeaterStatus.Alarm)
        //        {
        //            heaterUnit.HeaterStatus = HeaterStatus.Alarm;
        //        }
        //        return heaterUnit.ManualAct;
        //    }
        //    else
        //    {
        //        heaterUnit.ManualAct = HeaterAct.Off;
        //        return HeaterAct.Off;
        //    }
        //}

        #endregion
    }

    public class SeqHeatExchanger : XSeqFunction
    {
        #region Fields      
        private HeatExchanger m_HeatExchanger;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static ThreadHeatExchangerControl m_Control;
        #endregion

        #region Contructor
        public SeqHeatExchanger(ThreadHeatExchangerControl control, HeatExchanger heatExchanger)
        {
            m_HeatExchanger = heatExchanger;
            m_Server = m_HeatExchanger.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = string.Format("{0} INTR", m_HeatExchanger.Name);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;   //TODO:자신의 Init을 보도록 

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            //m_HeatExchanger.GetTemp(i);
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqInitHeatExchanger : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHeatExchangerControl m_Control;
        protected static _GenericCollection<HeatExchanger> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        public Alarm ALM_InitFail = null;

        protected GenericTag m_InitCheckHeatExchangerAlarm = new GenericTag("HeatExchanger Alarm", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitHeatExchanger(ThreadHeatExchangerControl control, _GenericCollection<HeatExchanger> units)
        {
            m_Units = units;
            m_Server = m_Units.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
            ALM_InitFail = new Alarm("HeatExchanger" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
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
                        m_InitCheckHeatExchangerAlarm.Value = InitCheckState.Checking;
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        bool alarm = true;
                        bool warning = true;

                        int count = m_Units.Count;

                        for (int i = 0; i < count; i++)
                        {
                            alarm &= m_Units[i].IsAlarm();
                            warning &= m_Units[i].IsWarning();
                        }

                        if (alarm)
                        {


                        }
                        else if (warning)
                        {


                        }

                        nSeqNo = 20;
                    }
                    break;

                case 20:
                    {
                        int count = m_Units.Count;

                        for (int i = 0; i < count; i++)
                        {
                            m_Units[i].SetOperationMode(OpMode.Remote);
                            m_Units[i].On();
                        }

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 30;
                    }
                    break;

                case 30:
                    {
                        bool running = true;
                        int count = m_Units.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (m_Simul.Device == false) running &= m_Units[i].DiHeatExchangerRunning.GetState();
                        }

                        if (running == true)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                        else if (GetElapsedTicks() > 30 * 1000)
                        {
                            // Set Alarm                            
                            //int count = m_Units.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (m_Units[i].Pump.IsAlarm())
                            //    {
                            //        //AlarmId = unit.Pump.ALM_PumpAlarm.Id;
                            //        m_AlarmId[i] = m_Units[i].Pump.ALM_PumpAlarm.Id;
                            //        m_Eqp.SetAlarm(m_AlarmId[i]);
                            //    }
                            //}
                            m_InitState = InitState.Fail;
                            m_InitCheckHeatExchangerAlarm.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;

                case 40:
                    {
                        bool ready = true;
                        int count = m_Units.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (m_Simul.Device == false) ready &= m_Units[i].IsChamber1Ready() &&
                                                                  m_Units[i].IsChamber2Ready() &&
                                                                  m_Units[i].IsChamber3Ready();
                        }

                        if (ready == true)
                        {
                            nSeqNo = 50;
                        }
                        else if (GetElapsedTicks() > 30 * 1000)
                        {
                            // Set Alarm
                            //int count = m_Units.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    if (m_Units[i].Pump.IsAlarm())
                            //    {
                            //        //AlarmId = unit.Pump.ALM_PumpAlarm.Id;
                            //        m_AlarmId[i] = m_Units[i].Pump.ALM_PumpAlarm.Id;
                            //        m_Eqp.SetAlarm(m_AlarmId[i]);
                            //    }
                            //}
                            m_InitState = InitState.Fail;
                            m_InitCheckHeatExchangerAlarm.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;

                case 50:
                    {
                        m_InitState = InitState.Comp;
                        m_InitCheckHeatExchangerAlarm.Value = InitCheckState.OK;

                        nSeqNo = 0;
                    }
                    break;


                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (m_AlarmId[i] > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId[i]);
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
                    //case 10:
                    //    {
                    //        int count = m_Units.Count;
                    //        for (int i = 0; i < count; i++)
                    //        {
                    //            m_Units[i].Heater.DoTankSafetyRelayReset.SetState(true);
                    //        }

                    //        m_StartTicks = XFunc.GetTickCount();

                    //        nSeqNo = 20;
                    //    }
                    //    break;

                    //case 20:
                    //    if (GetElapsedTicks() > 1000)
                    //    {
                    //        int count = m_Units.Count;
                    //        for (int i = 0; i < count; i++)
                    //        {
                    //            m_Units[i].Heater.DoTankSafetyRelayReset.SetState(false);
                    //        }

                    //        m_InitState = InitState.Comp;
                    //        m_InitCheckHeaterAlarm.Value = InitCheckState.OK;

                    //        nSeqNo = 0;
                    //    }
                    //    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }
}

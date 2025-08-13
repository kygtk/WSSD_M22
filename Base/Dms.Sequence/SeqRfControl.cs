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
    public class ThreadRfControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<Seren_Rfg> m_Rfgs;
        protected static _GenericCollection<RfTuner> m_RfTuners;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_Rfgs.Count == 0) return;

            foreach (Seren_Rfg device in m_Rfgs)
            {
                //RegisterSequence(new SeqRfInterlock(this, device));

                if (device.CommType == CommType.RS232 || device.CommType == CommType.AnalogRS232)
                {
                    RegisterSequence(new SeqRfStatusUpdate(this, device));
                }
            }

            foreach (RfTuner device in m_RfTuners)
            {
                if (device.CommType == CommType.RS232)
                {
                    RegisterSequence(new SeqRfTunerUpdate(this, device));
                }
            }

            //m_Server.AddSeqInitFunction(new SeqInitPump(this, m_PumpUnits));
        }
        #endregion

        #region Contructor
        public ThreadRfControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Rfgs = DmsComponents.Instance.ComponentContainer.GetCollection<Seren_Rfg>();
            m_RfTuners = DmsComponents.Instance.ComponentContainer.GetCollection<RfTuner>();
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
        //public static _GenericCollection<PumpUnit> Units
        //{
        //    get
        //    {
        //        if (m_PumpUnits == null) m_PumpUnits = new _GenericCollection<PumpUnit>();
        //        return m_PumpUnits;
        //    }
        //}
        #endregion

        #region Virtual Methods
        //public virtual bool IsInterlock(PumpUnit pump)
        //{
        //    HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
        //    bool interlock = false;
        //    interlock |= pump.IfFlag.InsufficientFlowrate;
        //    interlock |= (heavy > 0) ;
        //    return interlock;
        //}

        //public virtual bool IsRunEnable(PumpUnit pump)
        //{
        //    bool run = true;
        //    run &= pump.Pump.IsUse;
        //    run &= m_Server.JobCond.ProcessMode;
        //    run &= !pump.IfFlag.Alarm;
        //    run &= (pump.Tank == null) ? true : !pump.Tank.IsLevelFault();
        //    run &= (pump.Tank == null) ? true : pump.Tank.IsRunEnableLevel();

        //    return run;
        //}

        //public virtual bool IsAutoRunCondition(PumpUnit pump)
        //{
        //    bool run = true;
        //    run &= !IsInterlock(pump);
        //    run &= IsRunEnable(pump);
        //    run &= m_GenInfos.AutoMode;
        //    run &= m_GenInfos.EqpInitComp;
        //    run &= m_GenInfos.DiStart;
        //    run &= (pump.Tank == null) ? true : pump.Tank.IsTankReady();
        //    run &= (GetGlassCount() > 0) || ((GetGlassCount() == 0) && (m_GenInfos.IdleRunning));

        //    return run;
        //}

        //public virtual PumpAct GetRefPumpAct(PumpUnit pump)
        //{
        //    bool interlock = IsInterlock(pump);
        //    bool runEnable = IsRunEnable(pump);
        //    bool autoRun = IsAutoRunCondition(pump);
        //    bool autoMode = m_GenInfos.AutoMode;

        //    if (interlock)
        //    {
        //        pump.ManualAct = PumpAct.Stop;
        //        return PumpAct.Stop;
        //    }
        //    else if (autoRun)
        //    {
        //        pump.ManualAct = PumpAct.Stop;
        //        return PumpAct.Run;
        //    }
        //    else if (!autoMode && runEnable)
        //    {
        //        if (m_GenInfos.DiStart && ((pump.Tank == null) ? true : pump.Tank.IsTankReady())) return PumpAct.Run;
        //        else return pump.ManualAct;
        //    }
        //    else
        //    {
        //        pump.ManualAct = PumpAct.Stop;
        //        return PumpAct.Stop;
        //    }
        //}

        //public virtual int GetGlassCount()
        //{
        //    int glassNo = 0;

        //    glassNo = m_Server.GlassData.Count;

        //    return glassNo;
        //}
        #endregion

        #region General Methods
        //public bool IsAlarm()
        //{
        //    bool alarm = false;
        //    foreach (PumpUnit device in m_PumpUnits)
        //    {
        //        alarm |= device.Pump.SetupPumpUse.GetValue<bool>() && device.Pump.IsAlarm();
        //        if (alarm) break;
        //    }

        //    return alarm;
        //}
        #endregion
    }

    //public class SeqRfUnit : XSeqFunction
    //{
    //    #region Fields
    //    protected PumpUnit m_PumpUnit;
    //    protected static IEqpManager m_EqpManager;
    //    protected static IServerManager m_Server;
    //    protected static ThreadPumpControl m_Control; 
    //    #endregion

    //    #region Contructor
    //    public SeqRfUnit(ThreadRfControl control, Rfg pump)
    //    {
    //        m_PumpUnit = pump;
    //        m_Server = m_PumpUnit.ServerManager;
    //        m_EqpManager = m_Server.EqpStateManager;
    //        m_Control = control;

    //        SeqFunName = "PUMP UNIT";
    //    } 
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        m_PumpUnit.RefAct = m_Control.GetRefPumpAct(m_PumpUnit);
    //        m_PumpUnit.Pump.SetPumpAct(m_PumpUnit.RefAct);

    //        return -1;
    //    } 
    //    #endregion
    //}




    public class SeqRfInterlock : XSeqFunction
    {
        #region Fields
        protected Seren_Rfg m_Rfg;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadRfControl m_Control;
        #endregion

        #region Contstructor
        public SeqRfInterlock(ThreadRfControl control, Seren_Rfg rfg)
        {
            m_Rfg = rfg;
            m_Server = m_Rfg.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "RF INTR";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            bool checkCond = true;
            checkCond &= m_Rfg.IsRfPowerOn();

            bool simulation = m_Simul.Device;



            //if (simulation) gaugeFlowRate = (float)setupFlowRate;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (checkCond && m_Rfg.IsForwardInterlock(m_Rfg.RfPower * 1000) == true)
                        {
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 10;
                        }

                        else if (checkCond && m_Rfg.IsReflectedInterlock(m_Rfg.RfPower * 1000) == true)
                        {
                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 20;
                        }

                    }
                    break;
                case 10:
                    {
                        if (checkCond && m_Rfg.IsForwardInterlock(m_Rfg.RfPower * 1000) == true && GetElapsedTicks() > 10 * 1000)
                        {
                            m_AlarmId = m_Rfg.ALM_ForwardInterlockAlarm.Id;

                            m_EqpManager.SetAlarm(m_AlarmId);

                            nSeqNo = 1000;
                        }

                    }
                    break;

                case 20:
                    {
                        if (checkCond && m_Rfg.IsReflectedInterlock(m_Rfg.RfPower * 1000) == true && GetElapsedTicks() > 3000)
                        {
                            m_AlarmId = m_Rfg.ALM_ReflectedInterlockAlarm.Id;

                            m_EqpManager.SetAlarm(m_AlarmId);

                            nSeqNo = 2000;
                        }
                    }
                    break;

                case 1000:
                    {
                        if (m_Rfg.IsRfPowerOn()) m_Rfg.RfPowerOn(false);

                        if (m_EqpManager.AlarmResetSwitchPushed && m_Rfg.IsForwardInterlock(m_Rfg.RfPower) == false)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_Rfg.SetLog(m_Rfg.Name, m_SeqFunName, 0, 0, "Alarm Reset : RFG Forward Power Interlock");

                            nSeqNo = 0;
                        }
                    }
                    break;

                case 2000:
                    {
                        if (m_Rfg.IsRfPowerOn()) m_Rfg.RfPowerOn(false);

                        if (m_EqpManager.AlarmResetSwitchPushed && m_Rfg.IsReflectedInterlock(m_Rfg.RfPower) == false)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_Rfg.SetLog(m_Rfg.Name, m_SeqFunName, 0, 0, "Alarm Reset : RFG Reflected Power Interlock");

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

    //public class SeqPumpAlarm : XSeqFunction
    //{
    //    #region Fields
    //    protected PumpUnit m_PumpUnit;
    //    protected static IEqpManager m_EqpManager;
    //    protected static ThreadPumpControl m_Control; 
    //    #endregion

    //    #region Contstructor
    //    public SeqPumpAlarm(ThreadPumpControl control, PumpUnit pump)
    //    {
    //        m_PumpUnit = pump;
    //        m_EqpManager = m_PumpUnit.ServerManager.EqpStateManager;
    //        m_Control = control;

    //        SeqFunName = "PUMP ALARM";
    //    } 
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        int nSeqNo = this.SeqNo;

    //        bool checkCond = true;
    //        checkCond &= m_PumpUnit.Pump.IsUse;

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                {
    //                    if (m_PumpUnit.Pump.IsAlarm() && checkCond)
    //                    {
    //                        AlarmId = m_PumpUnit.Pump.ALM_PumpAlarm.Id;
    //                        m_EqpManager.SetAlarm(AlarmId);
    //                        m_PumpUnit.IfFlag.Alarm = true;
    //                        m_PumpUnit.SetLog(SeqFunName, 0, 0, "Alarm Set : Pump Alarm");
    //                        nSeqNo = 1000;
    //                    }
    //                }
    //                break;
    //            case 1000:
    //                {
    //                    if ((!m_PumpUnit.Pump.IsAlarm() || !checkCond) &&
    //                        m_EqpManager.AlarmResetSwitchPushed)
    //                    {
    //                        m_PumpUnit.IfFlag.Alarm = false;
    //                        m_EqpManager.ResetAlarm(AlarmId);
    //                        m_PumpUnit.SetLog(SeqFunName, 0, 0, "Alarm Reset : Pump Alarm");
    //                        nSeqNo = 0;
    //                    }
    //                }
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;

    //        return -1;
    //    } 
    //    #endregion
    //}

    //public class SeqInitRf : XSeqInitFunction
    //{
    //    #region Fields
    //    protected static IServerManager m_Server;
    //    protected static IEqpManager m_Eqp;
    //    protected static ThreadPumpControl m_Control;
    //    protected static _GenericCollection<PumpUnit> m_Units;
    //    protected static _GenInfoHandler m_GenInfos;
    //    protected InitState m_InitState = InitState.Noop;
    //    protected new int[] m_AlarmId;

    //    protected GenericTag m_InitCheckPumpAlarm = new GenericTag("Pump Alarm", InitCheckState.NotReady);
    //    #endregion

    //    #region Contructor
    //    public SeqInitPump(ThreadPumpControl control, _GenericCollection<PumpUnit> units)
    //    {
    //        m_Units = units;
    //        m_Server = m_Units.ServerManager;
    //        m_Eqp = m_Server.EqpStateManager;
    //        m_Control = control;
    //        m_AlarmId = new int[m_Units.Count];
    //        m_GenInfos = GenInfoHandler.Instance;

    //        this.SeqFunName = "INIT    ";
    //    } 
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        if (m_InitState == InitState.Comp) return (int)m_InitState;

    //        int nSeqNo = this.SeqNo;

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                if (m_GenInfos.EqpInitReq)
    //                {
    //                    m_InitState = InitState.Init;
    //                    m_Server.Log("SeqPumpInit : Start ");
    //                    nSeqNo = 10;
    //                }
    //                break;
    //            case 10:
    //                {
    //                    m_InitCheckPumpAlarm.Value = InitCheckState.Checking;

    //                    bool bAlarm = false;

    //                    if (m_Server.JobCond.ProcessMode)
    //                    {
    //                        bAlarm = m_Control.IsAlarm();
    //                    }

    //                    if (!bAlarm)
    //                    {
    //                        m_InitCheckPumpAlarm.Value = InitCheckState.OK;
    //                        nSeqNo = 100;
    //                    }
    //                    else
    //                    {
    //                        int count = m_Units.Count;
    //                        for (int i = 0; i < count; i++)
    //                        {
    //                            if (m_Units[i].Pump.IsAlarm())
    //                            {
    //                                //AlarmId = unit.Pump.ALM_PumpAlarm.Id;
    //                                m_AlarmId[i] = m_Units[i].Pump.ALM_PumpAlarm.Id;
    //                                m_Eqp.SetAlarm(m_AlarmId[i]);
    //                            }
    //                        }
    //                        m_InitState = InitState.Fail;
    //                        m_InitCheckPumpAlarm.Value = InitCheckState.NG;
    //                        nSeqNo = 1000;
    //                        //break;
    //                    }
    //                }
    //                break;
    //            case 100:
    //                {
    //                    m_InitState = InitState.Comp;
    //                    m_Server.Log("SeqPumpInit : Complete ");
    //                    nSeqNo = 0;
    //                }
    //                break;
    //            case 1000:
    //                if (m_Eqp.AlarmResetSwitchPushed)
    //                {
    //                    int count = m_Units.Count;
    //                    for (int i = 0; i < count; i++)
    //                    {
    //                        if (m_AlarmId[i] > 0)
    //                        {
    //                            m_Eqp.ResetAlarm(m_AlarmId[i]);
    //                            m_AlarmId[i] = 0;
    //                        }
    //                    }
    //                    nSeqNo = 1010;
    //                }
    //                break;
    //            case 1010:
    //                if (!m_GenInfos.EqpInitReq)
    //                {
    //                    nSeqNo = 0;
    //                }
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;

    //        return (int)m_InitState;
    //    } 
    //    #endregion
    //}

    public class SeqRfStatusUpdate : XSeqFunction
    {
        #region Fields
        protected Seren_Rfg m_Rfg;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadRfControl m_Control;
        #endregion

        #region Contstructor
        public SeqRfStatusUpdate(ThreadRfControl control, Seren_Rfg rfg)
        {
            m_Rfg = rfg;
            m_Server = m_Rfg.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "RF UPDATE";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Rfg != null)
                    {
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Rfg.ReqRfStatus();

                        //m_Rfg.ReceivedData("test");

                        nSeqNo = 0;
                    }
                    break;

            }

            this.m_SeqNo = nSeqNo;


            return -1;
        }
        #endregion
    }

    public class SeqRfTunerUpdate : XSeqFunction
    {
        #region Fields
        protected RfTuner m_RfTuner;
        protected static IEqpManager m_EqpManager;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadRfControl m_Control;
        #endregion

        #region Contstructor
        public SeqRfTunerUpdate(ThreadRfControl control, RfTuner rfTuner)
        {
            m_RfTuner = rfTuner;
            m_Server = m_RfTuner.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "RFTUNER UPDATE";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            switch (nSeqNo)
            {
                case 0:
                    if (m_RfTuner != null)
                    {
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (m_RfTuner.Command == "NONE")
                    {
                        m_RfTuner.ReqTunePosition();
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_RfTuner.Command = "NONE";
                        nSeqNo = 10;
                    }
                    break;

                case 20:
                    if (m_RfTuner.Command == "NONE")
                    {
                        m_RfTuner.ReqLoadPosition();
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_RfTuner.Command = "NONE";
                        nSeqNo = 20;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;


            return -1;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadHotWireControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<HotWireUnit> m_HotWireUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (HotWireUnit device in m_HotWireUnits)
            {
                RegisterSequence(new SeqHotWire(this, device));
                RegisterSequence(new SeqUpdateHotWireTemperature(this, device));
                //RegisterSequence(new SeqChiller(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadHotWireControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_HotWireUnits = DmsComponents.Instance.ComponentContainer.GetCollection<HotWireUnit>();
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
        public static _GenericCollection<HotWireUnit> Units
        {
            get
            {
                if (m_HotWireUnits == null) m_HotWireUnits = new _GenericCollection<HotWireUnit>();
                return m_HotWireUnits;
            }
        }
        public static bool IsRunTemperature(HotWireUnit hotWire)
        {
            return (hotWire.HeaterStatus == HeaterStatus.Ready);
        }
        #endregion

        #region Virtual Methods
        public virtual void CheckStopCountCond()
        {
        }

        public virtual bool IsAlarmCondition(HotWireUnit hotWire)
        {
            bool ng = false;
            ng |= hotWire.Tic.IsAlarm();
            ng |= hotWire.Heater.IsAlarm();
            return ng;
        }

        public virtual bool IsInterlock(HotWireUnit hotWire)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy & ~HeavyInterlock.Door) > 0);

            return interlock;
        }

        public virtual bool IsRunEnable(HotWireUnit hotWire)
        {
            bool run = true;
            run &= hotWire.Heater.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !IsAlarmCondition(hotWire);

            return run;
        }

        public virtual bool IsAutoRunCondition(HotWireUnit hotWire)
        {
            bool run = true;

            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= !IsInterlock(hotWire);
            run &= IsRunEnable(hotWire);

            //run &= m_Server.GenInfo.Eqp.ReadyComp;
            //run &= (m_Server.GlassData.Count > 0 );
            //run &= m_GenInfos.DiStart;
            //run &= (Tank == null) ? true : Tank.IsTankReady();

            return run;
        }

        public virtual HeaterStatus CheckTemp(HotWireUnit hotWire)
        {
            double useTemp = 0;
            double overTemp = hotWire.Heater.SetupHeaterOverTemp;
            double marginTemp = hotWire.Heater.SetupHeaterMarginTemp;
            double curTemp = hotWire.Tic.GetPvValue(hotWire.Id);

            if (m_GenInfos.AutoMode) useTemp = hotWire.Heater.SetupHeaterTemp;
            else useTemp = hotWire.ManualTemp;

            if (curTemp < useTemp - marginTemp) return HeaterStatus.Heating;
            if (curTemp >= useTemp - marginTemp && curTemp <= useTemp + marginTemp) return HeaterStatus.Ready;
            if (curTemp > useTemp + marginTemp) return HeaterStatus.Alarm;

            return HeaterStatus.NotReady;
        }

        public virtual HeaterAct GetRefHeaterAct(HotWireUnit hotWire)
        {
            bool interlock = IsInterlock(hotWire);
            bool runEnable = IsRunEnable(hotWire);
            bool autoRun = IsAutoRunCondition(hotWire);
            bool autoMode = m_GenInfos.AutoMode;

            if (!hotWire.Heater.IsUse)
            {
                hotWire.HeaterStatus = HeaterStatus.NoUse;
                return HeaterAct.Off;
            }

            if (interlock)
            {
                hotWire.ManualAct = HeaterAct.Off;
                return HeaterAct.Off;
            }
            else if (autoRun)
            {
                if (CheckTemp(hotWire) == HeaterStatus.Alarm)
                {
                    hotWire.ManualAct = HeaterAct.Off;
                    hotWire.HeaterStatus = HeaterStatus.Alarm;
                    return HeaterAct.Off;
                }

                if (hotWire.RefAct == HeaterAct.On)
                {
                    if (CheckTemp(hotWire) == HeaterStatus.Ready)
                    {
                        hotWire.HeaterStatus = HeaterStatus.Ready;
                    }
                    else
                    {
                        hotWire.HeaterStatus = HeaterStatus.Heating;
                    }

                    hotWire.ManualAct = HeaterAct.Off;

                    return HeaterAct.On;
                }
                else if (hotWire.RefAct == HeaterAct.Off)
                {
                    hotWire.ManualAct = HeaterAct.Off;
                    hotWire.HeaterStatus = HeaterStatus.Off;
                    return HeaterAct.Off;
                }
                else return HeaterAct.Noop;
            }
            else if (!autoRun)
            {
                if (hotWire.ManualAct == HeaterAct.Off)
                {
                    hotWire.HeaterStatus = HeaterStatus.Off;
                }
                else if (hotWire.ManualAct == HeaterAct.McOn)
                {

                }
                else if (hotWire.ManualAct == HeaterAct.McOff)
                {

                }
                else if (hotWire.ManualAct == HeaterAct.Reset)
                {


                }
                else if (hotWire.ManualAct == HeaterAct.Set)
                {

                }
                else if (hotWire.ManualAct == HeaterAct.On)
                {
                    if (CheckTemp(hotWire) == HeaterStatus.Heating)
                    {
                        hotWire.HeaterStatus = HeaterStatus.Heating;
                    }
                    else if (CheckTemp(hotWire) == HeaterStatus.Ready)
                    {
                        hotWire.HeaterStatus = HeaterStatus.Ready;
                    }
                    else if (CheckTemp(hotWire) == HeaterStatus.Alarm)
                    {
                        hotWire.HeaterStatus = HeaterStatus.Alarm;
                    }
                }
                return hotWire.ManualAct;
            }
            else
            {
                hotWire.ManualAct = HeaterAct.Off;
                return HeaterAct.Off;
            }
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqHotWire : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadHotWireControl m_Control;
        private HotWireUnit HotWireUnit;
        private HeaterStatus OldHeaterStatus = HeaterStatus.NotReady;
        #endregion

        #region Constructor
        public SeqHotWire(ThreadHotWireControl control, HotWireUnit hotWire)
        {
            HotWireUnit = hotWire;
            m_Server = HotWireUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            this.m_SeqFunName = HotWireUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            double useTemp;

            if (GenInfoHandler.Instance.AutoMode) useTemp = HotWireUnit.Heater.SetupHeaterTemp;
            else useTemp = HotWireUnit.ManualTemp;



            double overTemp = HotWireUnit.Heater.SetupHeaterOverTemp;
            double marginTemp = HotWireUnit.Heater.SetupHeaterMarginTemp;
            int delayTime = HotWireUnit.Heater.SetupHeaterDelay;

            int address = HotWireUnit.Heater.Id;

            double curTemp = HotWireUnit.Tic.GetPvValue(address);

            HeaterAct act = m_Control.GetRefHeaterAct(HotWireUnit);

            switch (nSeqNo)
            {
                case 0:
                    if (HotWireUnit.RefAct != act)
                    {
                        HotWireUnit.RefAct = act;

                        if (act == HeaterAct.On) nSeqNo = 100;
                        else if (act == HeaterAct.Off) nSeqNo = 200;
                        else HotWireUnit.Heater.SetHeaterAct(HotWireUnit.RefAct);
                    }
                    break;

                case 100:
                    {
                        HotWireUnit.Tic.SetOutputHigh(address + 1, HotWireUnit.OutputRatio * 10);
                        HotWireUnit.Heater.SetHeaterAct(HeaterAct.On);

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 110;
                    }
                    break;

                case 110:
                    if (GetElapsedTicks() > delayTime && (curTemp > useTemp - marginTemp && curTemp < useTemp + marginTemp))
                    {
                        nSeqNo = 120;
                    }
                    else if (act == HeaterAct.Off)
                    {
                        nSeqNo = 200;
                    }
                    else if (GetElapsedTicks() > delayTime)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        HotWireUnit.OutputRatio++;
                        HotWireUnit.Tic.SetOutputHigh(address + 1, HotWireUnit.OutputRatio * 10);
                    }

                    break;

                case 120:
                    if (act == HeaterAct.Off)
                    {
                        nSeqNo = 200;
                    }
                    break;

                case 200:
                    if (HotWireUnit.OutputRatio <= 0)
                    {
                        HotWireUnit.OutputRatio = 0;
                        HotWireUnit.Heater.SetHeaterAct(HeaterAct.Off);

                        nSeqNo = 0;
                    }
                    else
                    {
                        HotWireUnit.OutputRatio--;
                        HotWireUnit.Tic.SetOutputHigh(address + 1, HotWireUnit.OutputRatio * 10);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 210;
                    }
                    break;

                case 210:
                    if (act == HeaterAct.On)
                    {
                        nSeqNo = 100;
                    }
                    if (GetElapsedTicks() > delayTime)
                    {
                        HotWireUnit.OutputRatio--;
                        HotWireUnit.Tic.SetOutputHigh(address + 1, HotWireUnit.OutputRatio * 10);
                        nSeqNo = 200;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            //if (HotWireUnit.RefAct != act)
            //{
            //    HotWireUnit.RefAct = act;
            //    HotWireUnit.Heater.SetHeaterAct(HotWireUnit.RefAct);
            //}

            if (OldHeaterStatus != HotWireUnit.HeaterStatus)
            {
                OldHeaterStatus = HotWireUnit.HeaterStatus;

                HotWireUnit.UpdateTag();
            }

            return -1;
        }
        #endregion
    }

    public class SeqHotWireTempSetting : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadHotWireControl m_Control;
        private HotWireUnit HotWireUnit;
        //private HeaterStatus OldHeaterStatus = HeaterStatus.NotReady;
        #endregion

        #region Constructor
        public SeqHotWireTempSetting(ThreadHotWireControl control, HotWireUnit hotWire)
        {
            HotWireUnit = hotWire;
            m_Server = HotWireUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            this.m_SeqFunName = HotWireUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            double useTemp = HotWireUnit.Heater.SetupHeaterTemp;
            double overTemp = HotWireUnit.Heater.SetupHeaterOverTemp;
            double marginTemp = HotWireUnit.Heater.SetupHeaterMarginTemp;

            double curTemp = HotWireUnit.Tic.GetPvValue(HotWireUnit.Id);

            HeaterAct act = m_Control.GetRefHeaterAct(HotWireUnit);

            return -1;
        }
        #endregion
    }

    public class SeqHotWireAlarm : XSeqFunction
    {
        #region Fields
        private HotWireUnit m_HotWireUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHotWireControl m_Control;
        #endregion

        #region Contructor
        public SeqHotWireAlarm(ThreadHotWireControl control, HotWireUnit hotWire)
        {
            m_HotWireUnit = hotWire;
            m_EqpManager = m_HotWireUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "HotWire ALARM";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_HotWireUnit.Heater.IsUse;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_HotWireUnit.Tic.IsAlarm() && checkCond)
                        {
                            m_AlarmId = m_HotWireUnit.Tic.ALM_TicAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);

                            m_HotWireUnit.HeaterStatus = HeaterStatus.Alarm;
                            m_HotWireUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Tic Alarm");

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    {
                        if ((!m_HotWireUnit.Tic.IsAlarm() || !checkCond) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_HotWireUnit.HeaterStatus = HeaterStatus.NotReady;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_HotWireUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Tic Alarm");
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

    public class SeqInitHotWire : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHotWireControl m_Control;
        protected static _GenericCollection<HotWireUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        public Alarm ALM_InitFail = null;

        protected GenericTag m_InitCheckHotWireAlarm = new GenericTag("HotWire Alarm", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitHotWire(ThreadHotWireControl control, _GenericCollection<HotWireUnit> units)
        {
            m_Units = units;
            m_Server = m_Units.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
            ALM_InitFail = new Alarm("HotWireUnit" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
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
                        m_InitCheckHotWireAlarm.Value = InitCheckState.Checking;
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    {
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            m_Units[i].Heater.DoTankSafetyRelayReset.SetState(true);
                        }

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    break;

                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            m_Units[i].Heater.DoTankSafetyRelayReset.SetState(false);
                        }

                        m_InitState = InitState.Comp;
                        m_InitCheckHotWireAlarm.Value = InitCheckState.OK;

                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqUpdateHotWireTemperature : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHotWireControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private HotWireUnit m_HotWireUnit;
        //private int m_HotWireCount = 0;
        public Alarm ALM_InitFail = null;
        #endregion

        #region Contructor
        public SeqUpdateHotWireTemperature(ThreadHotWireControl control, HotWireUnit hotWire)
        {
            m_HotWireUnit = hotWire;
            m_Server = m_HotWireUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_HotWireUnit.ManualTemp = Convert.ToInt32(m_HotWireUnit.Heater.SetupHeaterTemp);
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "Update  ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int monitorNo = m_HotWireUnit.Tic.MonitorNo;

            int useTemp = 0;
            double marginTemp = m_HotWireUnit.Heater.SetupHeaterMarginTemp;

            if (m_GenInfos.AutoMode) useTemp = Convert.ToInt32(m_HotWireUnit.Heater.SetupHeaterTemp);
            else useTemp = m_HotWireUnit.ManualTemp;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_HotWireUnit.Tic.RequsetCurTemperature(m_HotWireUnit.Id + 1);
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 500)
                    {
                        #region simulation
                        if (m_Simul.Device)
                        {
                            m_HotWireUnit.Tic.CurTemperature[m_HotWireUnit.Id] = m_HotWireUnit.OutputRatio * 30;
                            m_HotWireUnit.UpdateTag();

                        }
                        //if (m_Simul.Device && m_HotWireUnit.RefAct == HeaterAct.On)
                        //{
                        //    for (int i = 0; i < monitorNo; i++)
                        //    {
                        //        if (m_HotWireUnit.Tic.CurTemperature[i] < useTemp)
                        //        {
                        //            m_HotWireUnit.Tic.CurTemperature[i] += 5.0;

                        //            m_HotWireUnit.UpdateTag();
                        //        }
                        //        else if (m_HotWireUnit.Tic.CurTemperature[i] > useTemp)
                        //        {
                        //            m_HotWireUnit.Tic.CurTemperature[i] -= 5.0;

                        //            m_HotWireUnit.UpdateTag();
                        //        }
                        //    }
                        //}
                        //else if (m_Simul.Device && (m_HotWireUnit.RefAct == HeaterAct.Off))
                        //{
                        //    for (int i = 0; i < monitorNo; i++)
                        //    {
                        //        if (m_HotWireUnit.Tic.CurTemperature[i] > 0)
                        //        {
                        //            m_HotWireUnit.Tic.CurTemperature[i] -= 5.0;

                        //            m_HotWireUnit.UpdateTag();
                        //        }
                        //    }
                        //}
                        #endregion

                        m_HotWireUnit.Tic.SetSvValue(m_HotWireUnit.Id + 1, useTemp);

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    break;

                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_HotWireUnit.UpdateTag();
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    //public class SeqChiller : XSeqFunction
    //{
    //    #region Fields
    //    protected static IServerManager m_Server;
    //    protected static Simul m_Simul;
    //    protected static ThreadHeaterControl m_Control;
    //    protected static _GenInfoHandler m_GenInfos;
    //    private HeaterUnit m_HeaterUnit;
    //    #endregion

    //    #region Contructor
    //    public SeqChiller(ThreadHeaterControl control, HeaterUnit heater)
    //    {
    //        m_HeaterUnit = heater;
    //        m_Server = m_HeaterUnit.ServerManager;
    //        m_Simul = AppConfig.Instance.Simul;
    //        m_Control = control;
    //        m_GenInfos = GenInfoHandler.Instance;

    //        this.SeqFunName = m_HeaterUnit.Name;
    //    } 
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        double useTemp = 0;
    //        if (m_GenInfos.AutoMode) useTemp = Convert.ToInt32(m_HeaterUnit.Heater.SetupHeaterTemp);
    //        else useTemp = m_HeaterUnit.ManualTemp;

    //        double overTemp = m_HeaterUnit.Heater.SetupHeaterOverTemp;
    //        double marginTemp = m_HeaterUnit.Heater.SetupHeaterMarginTemp;

    //        double curTemp = (m_HeaterUnit.Tic.GetPvValue(0) + m_HeaterUnit.Tic.GetPvValue(1)) / 2;

    //        int nSeqNo = this.SeqNo;

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                if (curTemp > useTemp && m_HeaterUnit.ChillerValve.IsClose())
    //                {
    //                    m_HeaterUnit.ChillerValve.Open();
    //                    nSeqNo = 10;
    //                }
    //                break;

    //            case 10:
    //                if (curTemp <= useTemp && m_HeaterUnit.ChillerValve.IsOpen())
    //                {
    //                    m_HeaterUnit.ChillerValve.Close();
    //                    nSeqNo = 10;
    //                }
    //                else if (curTemp > useTemp) m_HeaterUnit.ChillerValve.Open();
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;

    //        return -1;
    //    } 
    //    #endregion
    //}
}

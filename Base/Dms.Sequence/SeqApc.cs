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
    public class ThreadApcControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<Apc> m_ApcUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (Apc device in m_ApcUnits)
            {
                RegisterSequence(new SeqUpdateBaratronGauge(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadApcControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_ApcUnits = DmsComponents.Instance.ComponentContainer.GetCollection<Apc>();
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

                //CheckStopCountCond();

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
        public static _GenericCollection<Apc> Units
        {
            get
            {
                if (m_ApcUnits == null) m_ApcUnits = new _GenericCollection<Apc>();
                return m_ApcUnits;
            }
        }
        //public static bool IsRunTemperature(Apc apc)
        //{
        //    return (heater.HeaterStatus == HeaterStatus.Ready) ;
        //}
        #endregion

        #region Virtual Methods
        //public virtual void CheckStopCountCond()
        //{
        //}

        //public virtual bool IsAlarmCondition(HeaterUnit heater)
        //{
        //    bool ng = false;
        //    ng |= heater.Tic.IsAlarm();
        //    ng |= heater.Heater.IsAlarm();
        //    return ng;
        //}

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

        #region General Methods

        #endregion
    }

    public class SeqUpdateBaratronGauge : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadApcControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private Apc m_ApcUnit;
        public Alarm ALM_InitFail = null;
        #endregion

        #region Contructor
        public SeqUpdateBaratronGauge(ThreadApcControl control, Apc apc)
        {
            m_ApcUnit = apc;
            m_Server = apc.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "Update  ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_ApcUnit != null)
                    {
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_ApcUnit.ReqCurPressure();
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

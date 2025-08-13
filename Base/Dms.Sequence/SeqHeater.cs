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
    public class ThreadHeaterControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<HeaterUnit> m_HeaterUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (HeaterUnit device in m_HeaterUnits)
            {
                RegisterSequence(new SeqHeater(this, device));
                RegisterSequence(new SeqUpdateTemperature(this, device));
                RegisterSequence(new SeqChiller(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadHeaterControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_HeaterUnits = DmsComponents.Instance.ComponentContainer.GetCollection<HeaterUnit>();
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
        public static _GenericCollection<HeaterUnit> Units
        {
            get
            {
                if (m_HeaterUnits == null) m_HeaterUnits = new _GenericCollection<HeaterUnit>();
                return m_HeaterUnits;
            }
        }
        public static bool IsRunTemperature(HeaterUnit heater)
        {
            return (heater.HeaterStatus == HeaterStatus.Ready);
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

        public virtual bool IsInterlock(HeaterUnit heater)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy & ~HeavyInterlock.Door) > 0);
            interlock |= (heater.TankLevel.RunEnableConfirm == LevelConfirm.Confirm ? false : true);

            return interlock;
        }

        public virtual bool IsRunEnable(HeaterUnit heater)
        {
            bool run = true;
            run &= heater.Heater.IsUse;
            run &= m_Server.JobCond.ProcessMode;
            run &= !IsAlarmCondition(heater);
            run &= (heater.TankLevel == null) ? true : IsRunEnableLevel(heater);

            return run;
        }

        public virtual bool IsAutoRunCondition(HeaterUnit heater)
        {
            bool run = true;

            run &= m_GenInfos.AutoMode;
            run &= m_GenInfos.EqpInitComp;
            run &= !IsInterlock(heater);
            run &= IsRunEnable(heater);

            //run &= m_Server.GenInfo.Eqp.ReadyComp;
            //run &= (m_Server.GlassData.Count > 0 );
            //run &= m_GenInfos.DiStart;
            //run &= (Tank == null) ? true : Tank.IsTankReady();

            return run;
        }

        public virtual bool IsRunEnableLevel(HeaterUnit heater)
        {
            if (heater.TankLevel.RunEnableConfirm == LevelConfirm.Confirm) return true;
            else return false;
        }

        public virtual HeaterStatus CheckTemp(HeaterUnit heater)
        {
            double useTemp = 0;
            double overTemp = heater.Heater.SetupHeaterOverTemp;
            double marginTemp = heater.Heater.SetupHeaterMarginTemp;
            double curTemp = (heater.Tic.GetPvValue(0) + heater.Tic.GetPvValue(1)) / 2;

            if (m_GenInfos.AutoMode) useTemp = heater.Heater.SetupHeaterTemp;
            else useTemp = heater.ManualTemp;


            if (curTemp < useTemp - marginTemp) return HeaterStatus.Heating;
            if (curTemp >= useTemp - marginTemp && curTemp <= useTemp + marginTemp) return HeaterStatus.Ready;
            if (curTemp > useTemp + marginTemp) return HeaterStatus.Alarm;

            return HeaterStatus.NotReady;
        }

        public virtual HeaterAct GetRefHeaterAct(HeaterUnit heaterUnit)
        {
            bool interlock = IsInterlock(heaterUnit);
            bool runEnable = IsRunEnable(heaterUnit);
            bool autoRun = IsAutoRunCondition(heaterUnit);
            bool autoMode = m_GenInfos.AutoMode;

            if (!heaterUnit.Heater.IsUse)
            {
                heaterUnit.HeaterStatus = HeaterStatus.NoUse;
                return HeaterAct.Off;
            }

            if (interlock)
            {
                heaterUnit.ManualAct = HeaterAct.Off;
                return HeaterAct.Off;
            }
            else if (autoRun)
            {
                if (CheckTemp(heaterUnit) == HeaterStatus.Heating)
                {
                    heaterUnit.ManualAct = HeaterAct.Off;
                    heaterUnit.HeaterStatus = HeaterStatus.Heating;
                    return HeaterAct.On;
                }
                else if (CheckTemp(heaterUnit) == HeaterStatus.Ready)
                {
                    heaterUnit.ManualAct = HeaterAct.Off;
                    heaterUnit.HeaterStatus = HeaterStatus.Ready;
                    return HeaterAct.On;
                }
                else if (CheckTemp(heaterUnit) == HeaterStatus.Alarm)
                {
                    heaterUnit.ManualAct = HeaterAct.Off;
                    heaterUnit.HeaterStatus = HeaterStatus.Alarm;
                    return HeaterAct.Off;
                }
                else return HeaterAct.Noop;
            }
            else if (!autoRun)
            {
                if (heaterUnit.ManualAct == HeaterAct.Off)
                {
                    heaterUnit.HeaterStatus = HeaterStatus.Off;
                }
                else if (CheckTemp(heaterUnit) == HeaterStatus.Heating)
                {
                    heaterUnit.HeaterStatus = HeaterStatus.Heating;
                }
                else if (CheckTemp(heaterUnit) == HeaterStatus.Ready)
                {
                    heaterUnit.HeaterStatus = HeaterStatus.Ready;
                }
                else if (CheckTemp(heaterUnit) == HeaterStatus.Alarm)
                {
                    heaterUnit.HeaterStatus = HeaterStatus.Alarm;
                }
                return heaterUnit.ManualAct;
            }
            else
            {
                heaterUnit.ManualAct = HeaterAct.Off;
                return HeaterAct.Off;
            }
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqHeater : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadHeaterControl m_Control;
        private HeaterUnit HeaterUnit;
        private HeaterStatus OldHeaterStatus = HeaterStatus.NotReady;
        #endregion

        #region Constructor
        public SeqHeater(ThreadHeaterControl control, HeaterUnit heater)
        {
            HeaterUnit = heater;
            m_Server = HeaterUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            this.m_SeqFunName = HeaterUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            double useTemp = HeaterUnit.Heater.SetupHeaterTemp;
            double overTemp = HeaterUnit.Heater.SetupHeaterOverTemp;
            double marginTemp = HeaterUnit.Heater.SetupHeaterMarginTemp;

            double curTemp = (HeaterUnit.Tic.GetPvValue(0) + HeaterUnit.Tic.GetPvValue(1)) / 2;

            HeaterAct act = m_Control.GetRefHeaterAct(HeaterUnit);

            if (HeaterUnit.RefAct != act)
            {
                HeaterUnit.RefAct = act;
                HeaterUnit.Heater.SetHeaterAct(HeaterUnit.RefAct);
            }

            if (OldHeaterStatus != HeaterUnit.HeaterStatus)
            {
                OldHeaterStatus = HeaterUnit.HeaterStatus;

                HeaterUnit.UpdateTag();
            }

            return -1;
        }
        #endregion
    }

    public class SeqHeaterAlarm : XSeqFunction
    {
        #region Fields
        private HeaterUnit m_HeaterUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadHeaterControl m_Control;
        #endregion

        #region Contructor
        public SeqHeaterAlarm(ThreadHeaterControl control, HeaterUnit heater)
        {
            m_HeaterUnit = heater;
            m_EqpManager = m_HeaterUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = "HEATER ALARM";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool checkCond = true;
            checkCond &= m_HeaterUnit.Heater.IsUse;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_HeaterUnit.Tic.IsAlarm() && checkCond)
                        {
                            m_AlarmId = m_HeaterUnit.Tic.ALM_TicAlarm.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);

                            m_HeaterUnit.HeaterStatus = HeaterStatus.Alarm;
                            m_HeaterUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : Tic Alarm");

                            nSeqNo = 1000;
                        }


                    }
                    break;
                case 1000:
                    {
                        if ((!m_HeaterUnit.Tic.IsAlarm() || !checkCond) &&
                            m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_HeaterUnit.HeaterStatus = HeaterStatus.NotReady;
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_HeaterUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : Tic Alarm");
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


    public class SeqInitHeater : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHeaterControl m_Control;
        protected static _GenericCollection<HeaterUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        public Alarm ALM_InitFail = null;

        protected GenericTag m_InitCheckHeaterAlarm = new GenericTag("Heater Alarm", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitHeater(ThreadHeaterControl control, _GenericCollection<HeaterUnit> units)
        {
            m_Units = units;
            m_Server = m_Units.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
            ALM_InitFail = new Alarm("HeaterUnit" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
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
                        m_InitCheckHeaterAlarm.Value = InitCheckState.Checking;
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
                        m_InitCheckHeaterAlarm.Value = InitCheckState.OK;

                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqUpdateTemperature : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadHeaterControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private HeaterUnit m_HeaterUnit;
        private int m_HeaterCount = 0;
        public Alarm ALM_InitFail = null;
        #endregion

        #region Contructor
        public SeqUpdateTemperature(ThreadHeaterControl control, HeaterUnit heater)
        {
            m_HeaterUnit = heater;
            m_Server = m_HeaterUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_HeaterUnit.ManualTemp = Convert.ToInt32(m_HeaterUnit.Heater.SetupHeaterTemp);
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "Update  ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int monitorNo = m_HeaterUnit.Tic.MonitorNo;

            int useTemp = 0;
            double marginTemp = m_HeaterUnit.Heater.SetupHeaterMarginTemp;


            if (m_GenInfos.AutoMode) useTemp = Convert.ToInt32(m_HeaterUnit.Heater.SetupHeaterTemp);
            else useTemp = m_HeaterUnit.ManualTemp;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_HeaterUnit.Tic.RequsetCurTemperature(m_HeaterCount + 1);
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (GetElapsedTicks() > 1000)
                    {

                        #region simulation
                        if (m_Simul.Device && (m_HeaterUnit.RefAct == HeaterAct.On || m_HeaterUnit.HeaterStatus == HeaterStatus.Alarm))
                        {
                            for (int i = 0; i < monitorNo; i++)
                            {
                                if (m_HeaterUnit.Tic.CurTemperature[i] < useTemp)
                                {
                                    m_HeaterUnit.Tic.CurTemperature[i] += 1.0;

                                    m_HeaterUnit.UpdateTag();
                                }
                                else if (m_HeaterUnit.Tic.CurTemperature[i] > useTemp)
                                {
                                    m_HeaterUnit.Tic.CurTemperature[i] -= 1.0;

                                    m_HeaterUnit.UpdateTag();
                                }
                            }
                        }
                        #endregion



                        if (m_HeaterCount == 0)
                        {
                            m_HeaterUnit.Tic.SetSvValue(m_HeaterCount + 1, useTemp * 10);
                        }
                        else
                        {
                            m_HeaterUnit.Tic.SetSvValue(m_HeaterCount + 1, useTemp * 10 + (int)(marginTemp * 10));
                        }

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    break;

                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_HeaterCount++;
                        if (m_HeaterCount > monitorNo - 1) m_HeaterCount = 0;

                        if (m_HeaterCount == 0)
                        {
                            m_HeaterUnit.Tic.SetAlarmTemperature(m_HeaterCount + 1, useTemp * 10 + (int)(marginTemp * 10));
                        }

                        m_HeaterUnit.UpdateTag();
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 30;
                    }
                    break;

                case 30:
                    if (GetElapsedTicks() > 1000)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqChiller : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadHeaterControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private HeaterUnit m_HeaterUnit;
        #endregion

        #region Contructor
        public SeqChiller(ThreadHeaterControl control, HeaterUnit heater)
        {
            m_HeaterUnit = heater;
            m_Server = m_HeaterUnit.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = m_HeaterUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            double useTemp = 0;
            if (m_GenInfos.AutoMode) useTemp = Convert.ToInt32(m_HeaterUnit.Heater.SetupHeaterTemp);
            else useTemp = m_HeaterUnit.ManualTemp;

            double overTemp = m_HeaterUnit.Heater.SetupHeaterOverTemp;
            double marginTemp = m_HeaterUnit.Heater.SetupHeaterMarginTemp;

            double curTemp = (m_HeaterUnit.Tic.GetPvValue(0) + m_HeaterUnit.Tic.GetPvValue(1)) / 2;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (curTemp > useTemp && m_HeaterUnit.ChillerValve.IsClose())
                    {
                        m_HeaterUnit.ChillerValve.Open();
                        nSeqNo = 10;
                    }
                    break;

                case 10:
                    if (curTemp <= useTemp && m_HeaterUnit.ChillerValve.IsOpen())
                    {
                        m_HeaterUnit.ChillerValve.Close();
                        nSeqNo = 10;
                    }
                    else if (curTemp > useTemp) m_HeaterUnit.ChillerValve.Open();
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
}

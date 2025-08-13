///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05.25
// Author       : eun
// Description  : SeqMixTank - Control mixtank (Control Drain, DiSupply AutoValve)
//                SeqMeasureChemical - Supply detergent and DI to mix tank and mixture it. And check its density
//                SeqMixSupplyPump - Control mixsupplypump
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadMixTankControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<MixTankUnit> m_TankUnits;
        protected static _GenericCollection<DetergentUnit> m_DetUnits;
        protected static _GenericCollection<MeasureTankUnit> m_MesTankUnit;
        protected static _GenericCollection<DetTankUnit> m_DetTankUnit;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_DetUnits.Count != 0)
            {
                foreach (DetergentUnit detUnit in m_DetUnits)
                {
                    int tankCount = detUnit.MixTanks.Count;

                    for (int i = 0; i < tankCount; i++)
                    {
                        MixTankUnit tank = detUnit.MixTanks[i];
                        RegisterSequence(new SeqMixTank(this, detUnit, tank));
                        RegisterSequence(new SeqMeasureChemical(this, detUnit, tank));
                        RegisterSequence(new SeqMixTankWashing(this, detUnit, tank));
                    }

                    RegisterSequence(new SeqMixtureControl(this, detUnit));
                    RegisterSequence(new SeqChemicalIdleRunning(this, detUnit));
                }

                RegisterSequence(new SeqGlassExistforDetergentUnit(this, m_Server));
                RegisterSequence(new SeqModeChangeforDetrgentUnit(this, m_Server));
            }

            if (m_TankUnits.Count == 0) return;

            foreach (MixTankUnit tankUnit in m_TankUnits)
            {
                RegisterSequence(new SeqMixTankLevelSensor(this, tankUnit));

                if (AppConfig.Instance.Simul.Device)
                {
                    RegisterSequence(new SeqSimulationMixTankLevelSensor(this, tankUnit));
                }
            }
        }
        #endregion

        #region Constructor
        public ThreadMixTankControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_TankUnits = DmsComponents.Instance.ComponentContainer.GetCollection<MixTankUnit>();
            m_DetUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DetergentUnit>();
            m_DetTankUnit = DmsComponents.Instance.ComponentContainer.GetCollection<DetTankUnit>();
            m_MesTankUnit = DmsComponents.Instance.ComponentContainer.GetCollection<MeasureTankUnit>();
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
        public static _GenericCollection<MixTankUnit> MixTankUnits
        {
            get
            {
                if (m_TankUnits == null) m_TankUnits = new _GenericCollection<MixTankUnit>();
                return m_TankUnits;
            }
        }
        public static _GenericCollection<DetergentUnit> DetergentUnits
        {
            get
            {
                if (m_DetUnits == null) m_DetUnits = new _GenericCollection<DetergentUnit>();
                return m_DetUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsInterlock(MixTankUnit tank)
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= (heavy > 0);
            interlock |= tank.LevelFault;
            return interlock;
        }

        public virtual bool IsSeqRunCondition(MixTankUnit tank)
        {
            bool stop = false;
            stop |= !IsRunCondition(tank);
            stop |= (m_GenInfos.AutoMode && !m_GenInfos.EqpInitComp);// TODO:Tank의 SeqInit을 보아야 함.
            stop |= (!m_GenInfos.AutoMode &&
                     !m_GenInfos.DiStart);
            return !stop;
        }

        public virtual bool IsRunCondition(MixTankUnit tank)
        {
            bool run = true;
            run &= !IsInterlock(tank);
            run &= m_Server.JobCond.ProcessMode;
            run &= tank.PumpUse;
            return run;
        }

        public virtual void InitParameter(MixTankUnit tank)
        { }
        #endregion
    }
    public class SeqMixtureControl : XSeqFunction
    {
        #region Fields
        protected static DetergentUnit m_DetUnit;
        protected static MixTankUnit m_TankUnit;
        protected static ThreadMixTankControl m_Control;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfo;
        private bool firstRun = true;
        private string m_Msg;
        private int m_TankNo = 0;
        private TagMixTankIfFlag m_IfFlag;
        #endregion

        #region Constructor
        public SeqMixtureControl(ThreadMixTankControl control, DetergentUnit detUnit)
        {
            m_DetUnit = detUnit;
            m_Control = control;
            m_Server = m_DetUnit.ServerManager;
            m_GenInfo = GenInfoHandler.Instance;

            //SeqFunName = m_DetUnit.Name + " CONTROL";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            if (m_DetUnit.MixTanks.Count == 0) return -1;

            m_TankUnit = m_DetUnit.MixTanks[m_TankNo];
            m_IfFlag = m_TankUnit.IfFlag;
            m_SeqFunName = m_TankUnit.Name + " Control";

            bool runCond = true;
            runCond &= m_GenInfo.EqpInitComp;
            runCond &= m_GenInfo.AutoMode || m_IfFlag.MixtureReq;
            runCond &= m_Server.JobCond.HeavyInterlock <= 0;
            runCond &= m_Server.JobCond.ProcessMode;

            if (!runCond) return -1;

            MixTankMode showerMode = (m_TankUnit.SetupMixTankMode.GetValue<bool>()) ? MixTankMode.DET : MixTankMode.DI;
            bool tankUse = m_TankUnit.SetupTankUse.GetValue<bool>();

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (!firstRun && (showerMode == MixTankMode.DET) &&
                            ((int)m_IfFlag.TankStatus >= (int)TankStatus.STANDBY))
                        {
                            m_DetUnit.Items.ReplenishCount += 1;
                            m_DetUnit.UpdateData();
                            m_Msg = string.Format("Replenish Count : {0}", m_DetUnit.Items.ReplenishCount.ToString());
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        }
                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    {
                        if (tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                            m_IfFlag.TankStatus == TankStatus.USING)
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Cur Tank Status : Using");
                            nSeqNo = 110;
                        }
                        else if (tankUse && (m_IfFlag.TankStatus == TankStatus.STANDBY))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Status : Stanby");
                            nSeqNo = 200;
                        }
                        else if (firstRun && tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id))
                        {
                            if ((showerMode == MixTankMode.DI) || (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Cur Shower Mode : DI");
                                nSeqNo = 300;
                            }
                            else nSeqNo = 400;
                        }
                        //else if (tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) && (m_IfFlag.TankStatus == TankStatus.tankSTANDBY))
                        //{
                        //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Cur Tank Status : Standby");
                        //    nSeqNo = 200;
                        //}
                        //else if (tankUse && (m_DetUnit.Items.CurMixTankId != m_TankUnit.Id) && (m_IfFlag.TankStatus == TankStatus.tankSTANDBY))
                        //{
                        //    if (m_DetUnit.CurTankUnit.IfFlag.TankStatus != TankStatus.tankUSING)
                        //    {
                        //        m_TankUnit.SetLog(SeqFunName, 0, 0, "Tank Status : Standby");
                        //        nSeqNo = 200;
                        //    }
                        //}
                        else
                        {
                            m_TankNo += 1;
                            if (m_TankNo >= m_DetUnit.MixTanks.Count) m_TankNo = 0;

                            nSeqNo = 0;
                        }
                    }
                    break;
                case 110:
                    if (m_DetUnit.ChemicalStart && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                        (m_IfFlag.TankStatus == TankStatus.USING))
                    {
                        if (!m_TankUnit.DetOutAutoValve.IsOpen())
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Chemical Out Valve Open");
                            m_TankUnit.DetOutAutoValve.Open();
                        }

                        if (!m_TankUnit.ReturnAutoValve.IsOpen() && (showerMode == MixTankMode.DET))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Return Valve Open");
                            m_TankUnit.ReturnAutoValve.Open();
                        }
                        else if (!m_TankUnit.ReturnAutoValve.IsOpen() &&
                                (showerMode == MixTankMode.DI) &&
                                (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Chemical Unit Return Valve Open");
                            m_TankUnit.ReturnAutoValve.Open();
                        }
                        //else
                        //{
                        //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Out Valve, Return Valve Close");
                        //    m_TankUnit.DetOutAutoValve.Close();
                        //    m_TankUnit.ReturnAutoValve.Close();
                        //}
                    }
                    else if (!m_DetUnit.ChemicalStart && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                            (m_IfFlag.TankStatus == TankStatus.USING))
                    {
                        if (m_TankUnit.DetOutAutoValve.IsOpen())
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Chemical Out Valve Close");
                            m_TankUnit.DetOutAutoValve.Close();
                        }

                        if (m_TankUnit.ReturnAutoValve.IsOpen() && (showerMode == MixTankMode.DET))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Return Valve Close");
                            m_TankUnit.ReturnAutoValve.Close();
                        }
                        else if (m_TankUnit.ReturnAutoValve.IsOpen() &&
                                (showerMode == MixTankMode.DI) &&
                                (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Chemical Unit Return Valve Close");
                            m_TankUnit.ReturnAutoValve.Close();
                        }
                    }
                    else if ((m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                             ((int)m_IfFlag.TankStatus <= (int)TankStatus.STANDBY))
                    {
                        m_Msg = string.Format("Current Shower Mode : {0}", showerMode.ToString());
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                        if (showerMode == MixTankMode.DI && (int)m_IfFlag.TankStatus < (int)TankStatus.STANDBY)
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "USING->NOT READY : Chemical Out Valve Close");
                            m_TankUnit.DetOutAutoValve.Close();
                            nSeqNo = 300;
                        }
                        else
                        {
                            if ((int)m_IfFlag.TankStatus < (int)TankStatus.STANDBY)
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "USING->NOT USE : Chemical Out Valve Close");
                            }
                            else
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "USING->NOT READY : Chemical Out Valve Close");
                            }
                            m_TankUnit.DetOutAutoValve.Close();

                            m_TankNo += 1;
                            if (m_TankNo >= m_DetUnit.MixTanks.Count) m_TankNo = 0;

                            nSeqNo = 0;
                        }
                    }
                    break;
                case 200:
                    if ((int)m_IfFlag.TankStatus >= (int)TankStatus.STANDBY &&
                        (m_IfFlag.MixtureComp))
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "STANDBY->USING");
                        m_DetUnit.Items.CurMixTankId = m_TankUnit.Id;
                        m_DetUnit.UpdateData();
                        m_IfFlag.TankStatus = TankStatus.USING;
                        if (firstRun) firstRun = false;
                        nSeqNo = 110;
                    }
                    else if (firstRun)
                    {
                        nSeqNo = 400;
                    }
                    break;
                case 300:
                    if ((int)m_IfFlag.TankStatus >= (int)TankStatus.STANDBY)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "NOT READY->USING");
                        m_DetUnit.Items.CurMixTankId = m_TankUnit.Id;
                        m_DetUnit.UpdateData();
                        m_IfFlag.TankStatus = TankStatus.USING;
                        if (firstRun) firstRun = false;
                        nSeqNo = 110;
                    }
                    break;
                case 400:
                    if (firstRun)
                    {
                        if ((showerMode == MixTankMode.DET) &&
                            (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                            (m_TankUnit.Items.MixTankUsedTime > 0 || m_TankUnit.Items.MixTankGlassCount > 0))
                        {
                        }
                        else
                        {
                            m_TankNo += 1;
                            if (m_TankNo >= m_DetUnit.MixTanks.Count) m_TankNo = 0;
                        }

                        nSeqNo = 200;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    //public class SeqMixtureControl : XSeqFunction
    //{
    //    #region Fields
    //    protected static DetergentUnit m_DetUnit;
    //    protected static MixTankUnit m_TankUnit;
    //    protected static ThreadMixTankControl m_Control;
    //    protected static IServerManager m_Server;
    //    protected static TagMixTankIfFlag m_IfFlag;
    //    private bool firstRun = true;
    //    private string m_Msg;
    //    #endregion

    //    #region Constructor
    //    public SeqMixtureControl(ThreadMixTankControl control, DetergentUnit detUnit, MixTankUnit tank)
    //    {
    //        m_DetUnit = detUnit;
    //        m_TankUnit = tank;
    //        m_Control = control;
    //        m_Server = m_TankUnit.ServerManager;
    //        m_IfFlag = m_TankUnit.IfFlag;

    //        SeqFunName = m_TankUnit.Name + " MIX CONTROL";
    //    }
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        int nSeqNo = this.SeqNo;

    //        bool runCond = true;
    //        runCond &= m_Server.GenInfos.EqpInitComp;
    //        runCond &= (m_Server.GenInfos.AutoMode || m_IfFlag.MixtureReq);
    //        runCond &= (m_Server.JobCond.HeavyInterlock <= 0);
    //        runCond &= m_Server.JobCond.ProcessMode;

    //        if (!runCond) return -1;

    //        MixTankMode showerMode = (m_TankUnit.SetupMixTankMode.GetValue<bool>()) ? MixTankMode.DET : MixTankMode.DI;
    //        bool tankUse = m_TankUnit.SetupTankUse.GetValue<bool>();

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                {
    //                    if (!firstRun && (showerMode == MixTankMode.DET) &&
    //                        ((int)m_IfFlag.TankStatus >= (int)TankStatus.tankSTANDBY))
    //                    {
    //                        m_DetUnit.Items.ReplenishCount += 1;
    //                        m_DetUnit.UpdateData();
    //                        m_Msg = string.Format("Replenish Count : {0}", m_DetUnit.Items.ReplenishCount.ToString());
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, m_Msg);
    //                    }
    //                    nSeqNo = 100;
    //                }
    //                break;
    //            case 100:
    //                {
    //                    if (tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
    //                        m_IfFlag.TankStatus == TankStatus.tankUSING)
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Cur Tank Status : Using");
    //                        nSeqNo = 110;
    //                    }
    //                    //else if (tankUse && (m_IfFlag.TankStatus == TankStatus.tankSTANDBY))
    //                    //{
    //                    //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Tank Status : Stanby");
    //                    //    nSeqNo = 200;
    //                    //}
    //                    else if (firstRun && tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id))
    //                    {
    //                        if ((showerMode == MixTankMode.DI) || (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
    //                        {
    //                            m_TankUnit.SetLog(SeqFunName, 0, 0, "Cur Shower Mode : DI");
    //                            nSeqNo = 300;
    //                        }
    //                        else nSeqNo = 400;
    //                    }
    //                    else if (tankUse && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) && (m_IfFlag.TankStatus == TankStatus.tankSTANDBY))
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Cur Tank Status : Standby");
    //                        nSeqNo = 200;
    //                    }
    //                    else if (tankUse && (m_DetUnit.Items.CurMixTankId != m_TankUnit.Id) && (m_IfFlag.TankStatus == TankStatus.tankSTANDBY))
    //                    {
    //                        if (m_DetUnit.CurTankUnit.IfFlag.TankStatus != TankStatus.tankUSING)
    //                        {
    //                            m_TankUnit.SetLog(SeqFunName, 0, 0, "Tank Status : Standby");
    //                            nSeqNo = 200;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        nSeqNo = 0;
    //                    }
    //                }
    //                break;
    //            case 110:
    //                if (m_Server.GenInfos.ChemicalStart && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
    //                    (m_IfFlag.TankStatus == TankStatus.tankUSING))
    //                {
    //                    if (m_TankUnit.DetOutAutoValve.IsClose())
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Out Valve Open");
    //                        m_TankUnit.DetOutAutoValve.Open();
    //                    }

    //                    if (m_TankUnit.ReturnAutoValve.IsClose() && (showerMode == MixTankMode.DET))
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Return Valve Open");
    //                        m_TankUnit.ReturnAutoValve.Open();
    //                    }
    //                    else if (m_TankUnit.ReturnAutoValve.IsClose() &&
    //                            (showerMode == MixTankMode.DI) &&
    //                            (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Unit Return Valve Open");
    //                        m_TankUnit.ReturnAutoValve.Open();
    //                    }
    //                    //else
    //                    //{
    //                    //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Out Valve, Return Valve Close");
    //                    //    m_TankUnit.DetOutAutoValve.Close();
    //                    //    m_TankUnit.ReturnAutoValve.Close();
    //                    //}
    //                }
    //                else if (!m_Server.GenInfos.ChemicalStart && (m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
    //                        (m_IfFlag.TankStatus == TankStatus.tankUSING))
    //                {
    //                    if (m_TankUnit.DetOutAutoValve.IsOpen())
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Out Valve Close");
    //                        m_TankUnit.DetOutAutoValve.Close();
    //                    }

    //                    if (m_TankUnit.ReturnAutoValve.IsOpen() && (showerMode == MixTankMode.DET))
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Return Valve Close");
    //                        m_TankUnit.ReturnAutoValve.Close();
    //                    }
    //                    else if (m_TankUnit.ReturnAutoValve.IsOpen() &&
    //                            (showerMode == MixTankMode.DI) &&
    //                            (m_TankUnit.Items.CurShowerMode == MixTankMode.DI))
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "Chemical Unit Return Valve Close");
    //                        m_TankUnit.ReturnAutoValve.Close();
    //                    }
    //                }
    //                else if ((m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
    //                         ((int)m_IfFlag.TankStatus <= (int)TankStatus.tankSTANDBY))
    //                {
    //                    m_Msg = string.Format("Current Shower Mode : {0}", showerMode.ToString());
    //                    m_TankUnit.SetLog(SeqFunName, 0, 0, m_Msg);

    //                    if (showerMode == MixTankMode.DI && (int)m_IfFlag.TankStatus < (int)TankStatus.tankSTANDBY)
    //                    {
    //                        m_TankUnit.SetLog(SeqFunName, 0, 0, "USING->NOT READY : Chemical Out Valve Close");
    //                        m_TankUnit.DetOutAutoValve.Close();
    //                        nSeqNo = 300;
    //                    }
    //                    else
    //                    {
    //                        if ((int)m_IfFlag.TankStatus < (int)TankStatus.tankSTANDBY)
    //                        {
    //                            m_TankUnit.SetLog(SeqFunName, 0, 0, "USING->NOT USE : Chemical Out Valve Close");
    //                        }
    //                        else
    //                        {
    //                            m_TankUnit.SetLog(SeqFunName, 0, 0, "USING->NOT READY : Chemical Out Valve Close");
    //                        }
    //                        m_TankUnit.DetOutAutoValve.Close();
    //                        nSeqNo = 0;
    //                    }
    //                }
    //                break;
    //            case 200:
    //                if ((int)m_IfFlag.TankStatus >= (int)TankStatus.tankSTANDBY &&
    //                    (m_IfFlag.MixtureComp))
    //                {
    //                    m_TankUnit.SetLog(SeqFunName, 0, 0, "STANDBY->USING");
    //                    m_DetUnit.Items.CurMixTankId = m_TankUnit.Id;
    //                    m_DetUnit.UpdateData();
    //                    m_IfFlag.TankStatus = TankStatus.tankUSING;
    //                    if (firstRun) firstRun = false;
    //                    nSeqNo = 110;
    //                }
    //                else if (firstRun)
    //                {
    //                    nSeqNo = 400;
    //                }
    //                break;
    //            case 300:
    //                if ((int)m_IfFlag.TankStatus >= (int)TankStatus.tankSTANDBY)
    //                {
    //                    m_TankUnit.SetLog(SeqFunName, 0, 0, "NOT READY->USING");
    //                    m_DetUnit.Items.CurMixTankId = m_TankUnit.Id;
    //                    m_DetUnit.UpdateData();
    //                    m_IfFlag.TankStatus = TankStatus.tankUSING;
    //                    if (firstRun) firstRun = false;
    //                    nSeqNo = 110;
    //                }
    //                break;
    //            case 400:
    //                if (firstRun)
    //                {
    //                    nSeqNo = 200;
    //                }
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;

    //        return -1;
    //    }
    //    #endregion
    //}

    public class SeqMixTank : XSeqFunction
    {
        #region Fields
        protected MixTankUnit m_TankUnit;
        protected DetergentUnit m_DetUnit;
        protected TagMixTankIfFlag m_IfFlag;
        protected MixTankItem m_TankPara;
        protected DetergentUnitItem m_DetPara;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMixTankControl m_Control;
        protected int m_SimulSensor = 0;
        protected _GenInfoHandler m_GenInfo;
        private string m_Msg;

        private MixTankMode m_ShowerModeReq;
        private TankStatus m_SaveTankStatus = TankStatus.NOT_USE;
        private bool m_FirstMixture = true;
        #endregion

        #region Constructor
        public SeqMixTank(ThreadMixTankControl control, DetergentUnit detUnit, MixTankUnit tank)
        {
            m_TankUnit = tank;
            m_DetUnit = detUnit;
            m_TankPara = m_TankUnit.Items;
            m_DetPara = m_DetUnit.Items;
            m_IfFlag = m_TankUnit.IfFlag;
            m_Server = m_TankUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfo = GenInfoHandler.Instance;

            m_SeqFunName = m_TankUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool runCond = true;
            runCond &= !m_Control.IsInterlock(m_TankUnit);// m_Control.IsRunCondition(m_TankUnit);

            if (!runCond || m_TankUnit.SupplyStop || m_TankUnit.TopLevel)
            {
                if (m_TankUnit.DiSupplyAutoValve.IsOpen())
                {
                    m_TankUnit.DiSupplyAutoValve.Close();
                    if (!runCond)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DI Valve Close : Run Condition(FALSE)");
                    if (m_TankUnit.SupplyStop)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DI Valve Close : H Level Exist");
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DI Valve Close");
                }
                if (m_TankUnit.DetSupplyAutoValve.IsOpen())
                {
                    m_TankUnit.DetSupplyAutoValve.Close();
                    if (!runCond)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET Valve Close : Run Condition(FALSE))");
                    if (m_TankUnit.SupplyStop)
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET Valve Close : H Level Exist");
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET Valve Close");
                }
            }

            bool notReady = false;
            notReady |= !m_TankUnit.RunEnable;
            notReady |= !m_TankUnit.BottomLevel;
            notReady |= m_TankUnit.LevelFault;

            if (notReady)
            {
                m_IfFlag.TankReady = false;

                if ((int)m_IfFlag.TankStatus > (int)TankStatus.WASHING)
                {
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Mix Tank Status : NOT READY");
                    m_IfFlag.TankStatus = TankStatus.NOT_READY;
                }
                if (m_IfFlag.MixtureComp)
                {
                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Mixture Completed : FALSE");
                    m_IfFlag.MixtureComp = false;
                }
            }

            //시퀀스가 안돌 조건이면 return
            //if (!m_Control.IsSeqRunCondition(m_TankUnit)) return -1;
            if (!m_GenInfo.EqpInitComp || !runCond) return -1;

            MixTankMode showerMode = (m_TankUnit.SetupMixTankMode.GetValue<bool>()) ? MixTankMode.DET : MixTankMode.DI;
            bool tankUse = m_TankUnit.SetupTankUse.GetValue<bool>();
            int useTime = m_TankUnit.SetupTankUseLifeTime.GetValue<int>();
            int glassCount = m_TankUnit.SetupGlassLimitCount.GetValue<int>();
            int recycleNo = m_TankUnit.SetupRecycleNo.GetValue<int>();
            int drainNo = m_TankUnit.SetupDrainNo.GetValue<int>();
            int diSupplyTimeout = m_TankUnit.SetupDISupplyOverTime.GetValue<int>() * 1000;
            int drainTimeout = m_TankUnit.SetupDrainOverTime.GetValue<int>() * 1000;

            switch (nSeqNo)
            {
                case 0:
                    if (m_IfFlag.MixTankForceDrainReq)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Force Drain : Start");
                        m_IfFlag.TankStatus = TankStatus.NOT_READY;
                        m_IfFlag.MixtureComp = false;
                        //m_IfFlag.TankReady = false;
                        m_IfFlag.RequireDensity = MixtureDensity.densityDRAIN;
                        nSeqNo = 250;
                    }
                    else if ((tankUse == false) ||
                             (!m_GenInfo.AutoMode && !m_IfFlag.MixtureReq))
                    {
                        if ((tankUse == false) && ((int)m_IfFlag.TankStatus > (int)TankStatus.NOT_USE))
                        {
                            m_SaveTankStatus = m_IfFlag.TankStatus;
                            m_Msg = string.Format("Tank not use : Save Status : {0}", m_SaveTankStatus.ToString());
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            m_IfFlag.TankStatus = TankStatus.NOT_USE;
                        }
                        nSeqNo = 0;
                    }
                    else
                    {
                        if ((m_DetPara.CurMixTankId == m_TankUnit.Id) && (showerMode == MixTankMode.DI))
                        {
                            nSeqNo = 50;
                        }
                        else
                        {
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 50:
                    {
                        if (m_DetPara.CurMixTankId != m_TankUnit.Id)
                        {
                            nSeqNo = 0;
                        }
                        else
                        {
                            if ((m_TankPara.CurShowerMode != showerMode) && (showerMode == MixTankMode.DI))
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : Change Shower Mode(DET->DIW)");
                                m_TankPara.MixTankUsedTime = 0;
                                m_TankPara.MixTankGlassCount = 0;
                                m_TankPara.MixTankRecycleCount = 0;
                                m_TankPara.UpdateData();

                                m_IfFlag.TankStatus = TankStatus.NOT_READY;
                                m_IfFlag.MixtureComp = false;
                                m_IfFlag.RequireDensity = MixtureDensity.densityNONE;
                                m_ShowerModeReq = MixTankMode.DI;

                                nSeqNo = 250;
                            }
                            else if ((tankUse == true) && !m_TankUnit.RunEnable || !m_TankUnit.BottomLevel)
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : Tank Level Sensor L is OFF");
                                nSeqNo = 500;
                            }
                            else if (m_FirstMixture && (m_TankPara.CurShowerMode == MixTankMode.DI))
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : Mix Tank SEQ Start(Check Status)");
                                m_ShowerModeReq = MixTankMode.DI;
                                nSeqNo = 500;
                            }
                            else if ((tankUse == true) && (m_IfFlag.TankStatus == TankStatus.NOT_USE))
                            {
                                if ((int)m_SaveTankStatus <= (int)TankStatus.NOT_USE)
                                {
                                    m_SaveTankStatus = TankStatus.NOT_READY;
                                }

                                m_Msg = string.Format("Mix Tank Status(DIW) : NOT USE->{0}", m_SaveTankStatus.ToString());
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                m_IfFlag.TankStatus = m_SaveTankStatus;
                            }
                            else
                            {
                                nSeqNo = 0;
                            }
                        }
                    }
                    break;
                case 100:
                    {
                        if (!m_TankUnit.RunEnable || (m_IfFlag.TankStatus == TankStatus.NOT_READY && !m_FirstMixture))
                        {
                            if (m_TankPara.MixTankDrainCount > drainNo ||
                                m_TankPara.MixTankUsedTime > useTime ||
                                m_TankPara.MixTankGlassCount > glassCount ||
                                m_TankPara.MixTankRecycleCount > recycleNo)//||
                                                                           //(!m_TankUnit.RunEnable && !m_TankUnit.BottomLevel))
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : Tank Drain Sequence(level)");
                                m_IfFlag.TankStatus = TankStatus.NOT_READY;
                                m_IfFlag.MixtureComp = false;
                                m_IfFlag.RequireDensity = MixtureDensity.densityDRAIN;
                                //m_ShowerModeReq = MixTankMode.DET;
                                m_ShowerModeReq = showerMode;

                                nSeqNo = 110;
                            }
                            //else if (m_IfFlag.MixtureComp == false)
                            else
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : Tank Recycle Sequence");
                                m_IfFlag.TankStatus = TankStatus.NOT_READY;
                                m_IfFlag.MixtureComp = false;
                                m_IfFlag.RequireDensity = MixtureDensity.densityRECYCLE;
                                //m_ShowerModeReq = MixTankMode.DET;
                                m_ShowerModeReq = showerMode;

                                nSeqNo = 150;
                            }
                        }
                        else if ((m_TankPara.CurShowerMode != showerMode) && (showerMode == MixTankMode.DET))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : Tank Drain Sequence(mode)");
                            m_IfFlag.TankStatus = TankStatus.NOT_READY;
                            m_IfFlag.MixtureComp = false;
                            m_IfFlag.RequireDensity = MixtureDensity.densityDRAIN;
                            //m_ShowerModeReq = MixTankMode.DET;
                            m_ShowerModeReq = showerMode;

                            nSeqNo = 110;
                        }
                        else if (m_FirstMixture && /*m_TankUnit.RunEnable &&*/ (m_TankPara.CurShowerMode == MixTankMode.DET))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : Mix Tank SEQ Start(Check Status)");
                            //m_ShowerModeReq = MixTankMode.DET;
                            m_ShowerModeReq = showerMode;

                            nSeqNo = 520;
                        }
                        else if ((tankUse == true) && (m_IfFlag.TankStatus == TankStatus.NOT_USE))
                        {
                            if ((int)m_SaveTankStatus <= (int)TankStatus.NOT_USE)
                            {
                                //m_SaveTankStatus = TankStatus.tankNOT_USE;
                                m_SaveTankStatus = TankStatus.NOT_READY;
                            }

                            m_Msg = string.Format("Mix Tank Status(DET) : NOT USE->{0}", m_SaveTankStatus.ToString());
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            m_IfFlag.TankStatus = m_SaveTankStatus;
                            //nSeqNo = 600;
                        }
                        else
                        {
                            if (m_TankUnit.RunEnable && m_TankUnit.BottomLevel && ((m_TankUnit.HeaterUnit == null) || (m_TankUnit.HeaterUnit != null && !m_IfFlag.MixTankHeaterReady)))
                            {
                                if ((m_TankUnit.HeaterUnit != null) && (int)m_IfFlag.TankStatus < (int)TankStatus.HEATING)
                                {
                                    m_Msg = string.Format("Mix Tank Status(DET) : {0}->HEATING", m_IfFlag.TankStatus.ToString());
                                    m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                    m_IfFlag.TankStatus = TankStatus.HEATING;
                                }
                            }

                            nSeqNo = 600;
                        }
                    }
                    break;
                case 110:
                    {
                        if ((m_TankPara.CurShowerMode != showerMode) && (m_ShowerModeReq == MixTankMode.DET))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Change Shower Mode(DIW->DET)");
                            m_TankPara.CurShowerMode = showerMode;
                            m_TankPara.UpdateData();
                            nSeqNo = 200;
                        }
                        else if (m_TankPara.MixTankDrainCount >= drainNo)
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Renewal of Tank with Drain-Washing");
                            nSeqNo = 250;
                        }
                        else
                        {
                            m_TankPara.MixTankDrainCount += 1;
                            m_TankPara.UpdateData();

                            if (m_TankPara.MixTankDrainCount >= drainNo) nSeqNo = 110;
                            else
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Renewal of Tank with Drain");
                                nSeqNo = 250;
                            }
                        }

                        m_TankPara.MixTankPrevUsedTime = m_TankPara.MixTankUsedTime;
                        m_TankPara.MixTankUsedTime = 0;
                        m_TankPara.MixTankGlassCount = 0;
                        m_TankPara.MixTankRecycleCount = 0;
                        m_TankPara.UpdateData();
                    }
                    break;
                case 150:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Recycle of Tank");

                        m_TankPara.MixTankRecycleCount += 1;
                        m_TankPara.MixTankPrevUsedTime = m_TankPara.MixTankUsedTime;
                        m_TankPara.UpdateData();

                        nSeqNo = 400;
                    }
                    break;
                case 200:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Start(DIW->DET)");
                        m_TankUnit.DrainAutoValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 210;
                    }
                    break;
                case 210:
                    if (!m_TankUnit.BottomLevel && (GetElapsedTicks() > drainTimeout))
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain End(DIW->DET)");
                        m_TankUnit.DrainAutoValve.Close();
                        nSeqNo = 320;
                    }
                    else if (GetElapsedTicks() > drainTimeout)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Timeout Error(DIW->DET)");
                        m_TankUnit.DrainAutoValve.Close();
                        m_AlarmId = m_TankUnit.ALM_MixTankDrainTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 200;
                        nSeqNo = 1000;
                    }
                    break;
                case 250:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Start");
                        m_TankUnit.DrainAutoValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 260;
                    }
                    break;
                case 260:
                    if (!m_TankUnit.BottomLevel && (GetElapsedTicks() > drainTimeout))
                    {
                        m_TankUnit.DrainAutoValve.Close();

                        if (m_IfFlag.MixTankForceDrainReq)
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Forced Drain End");
                            m_IfFlag.MixTankForceDrainReq = false;
                            nSeqNo = 0;
                        }
                        else
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain End");
                            nSeqNo = 300;
                        }
                    }
                    else if (GetElapsedTicks() > drainTimeout)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Timeout Error");
                        m_TankUnit.DrainAutoValve.Close();
                        m_AlarmId = m_TankUnit.ALM_MixTankDrainTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 250;
                        nSeqNo = 1000;
                    }
                    break;
                case 300:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Washing Request");
                        m_IfFlag.MixTankWashingReq = true;
                        nSeqNo = 310;
                    }
                    break;
                case 310:
                    if (m_IfFlag.MixTankWashingComp)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Washing Completed");
                        m_IfFlag.MixTankWashingComp = false;

                        if ((m_TankPara.CurShowerMode != m_ShowerModeReq) && (m_ShowerModeReq == MixTankMode.DET)) nSeqNo = 320;
                        else nSeqNo = 350;
                    }
                    break;
                case 320:
                    if (!m_TankUnit.SupplyStop)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Confirm : Tank Level is H OFF");
                        if (m_TankUnit.DrainAutoValve.IsOpen())
                        {
                            m_TankUnit.DrainAutoValve.Close();
                        }
                        nSeqNo = 400;
                    }
                    else if (m_TankUnit.DrainAutoValve.IsClose())
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Drain Valve Open");
                        m_TankUnit.DrainAutoValve.Open();
                    }
                    break;
                case 350:
                    if (!m_TankUnit.BottomLevel)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Confirm : Tank Level is LL");
                        if (m_TankUnit.DrainAutoValve.IsOpen())
                        {
                            m_TankUnit.DrainAutoValve.Close();
                        }
                        nSeqNo = 400;
                    }
                    else if (m_TankUnit.DrainAutoValve.IsClose())
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Drain Valve Open");
                        m_TankUnit.DrainAutoValve.Open();
                    }
                    break;
                case 400:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Detergent Supply Request");
                        m_IfFlag.MixTankDetReq = true;
                        nSeqNo = 410;
                    }
                    break;
                case 410:
                    if (m_IfFlag.MixTankDetComp)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Detergent Supply Complete");
                        m_IfFlag.MixTankDetComp = false;
                        if (m_TankUnit.UseDETnDISeperately && m_ShowerModeReq == MixTankMode.DET)
                        {
                            nSeqNo = 550;
                        }
                        else nSeqNo = 500;
                    }
                    break;
                case 500:
                    {
                        if (!m_TankUnit.SupplyStop)
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Tank DIW Supply");

                            m_TankUnit.DiSupplyAutoValve.Open();
                        }

                        if ((m_ShowerModeReq == MixTankMode.DI) &&
                            (m_IfFlag.TankStatus == TankStatus.USING || m_DetPara.CurMixTankId == m_TankUnit.Id))
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : Tank Replenishment");
                            nSeqNo = 510;
                        }
                        else
                        {
                            nSeqNo = 550;
                        }

                        m_StartTicks = XFunc.GetTickCount();
                    }
                    break;
                case 510:
                    if (m_TankUnit.SupplyStop && m_TankUnit.BottomLevel)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : Tank Level H Sensor ON");
                        m_TankUnit.DiSupplyAutoValve.Close();
                        nSeqNo = 520;
                    }
                    else if (GetElapsedTicks() > diSupplyTimeout)
                    {
                        m_TankUnit.DiSupplyAutoValve.Close();
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank DIW Supply Timeout Error");
                        m_AlarmId = m_TankUnit.ALM_MixTankDiSupplyTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 500;
                        nSeqNo = 1000;
                    }
                    break;
                case 520:
                    {
                        if (m_FirstMixture &&
                            (m_ShowerModeReq == MixTankMode.DET) &&
                            (m_TankPara.CurShowerMode == MixTankMode.DET))
                        {
                            m_FirstMixture = false;
                            if (m_TankUnit.RunEnable)
                            {
                                m_IfFlag.MixtureComp = true;
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : Mixture Set Completed");
                            }

                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DET SHW : First Mixture Set Completed");
                            nSeqNo = 600;
                        }
                        else
                        {
                            if (m_FirstMixture && (m_TankPara.CurShowerMode == MixTankMode.DI))
                            {
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "DIW SHW : First Mixture Set Completed");
                            }
                            //else
                            //{
                            //    m_TankUnit.SetLog(SeqFunName, 0, 0, "DIW SHW : Tank Replenishment Completed");
                            //}

                            if (m_TankPara.CurShowerMode != m_ShowerModeReq)
                            {
                                m_Msg = string.Format("Save Shower Mode : {0}->{1}", m_TankPara.CurShowerMode.ToString(), m_ShowerModeReq.ToString());
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                                m_TankPara.CurShowerMode = m_ShowerModeReq;
                                m_TankPara.UpdateData();
                            }

                            m_IfFlag.TankStatus = TankStatus.USING;
                            m_IfFlag.MixtureComp = true;
                            m_FirstMixture = false;

                            nSeqNo = 0;
                        }
                    }
                    break;
                case 550:
                    if (m_TankUnit.SupplyStop && m_TankUnit.BottomLevel)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "END : Tank DIW Supply");

                        m_TankUnit.DiSupplyAutoValve.Close();
                        m_IfFlag.MixtureComp = true;

                        if (!m_FirstMixture &&
                            (m_ShowerModeReq == MixTankMode.DI) &&
                            (m_TankPara.CurShowerMode == MixTankMode.DET) &&
                            ((int)m_IfFlag.TankStatus < (int)TankStatus.STANDBY) &&
                            (m_DetPara.CurMixTankId != m_TankUnit.Id))
                        {
                            m_Msg = string.Format("This Mix Tank is {0} mode set", m_TankPara.CurShowerMode.ToString());
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        }
                        else if (m_TankPara.CurShowerMode != m_ShowerModeReq)
                        {
                            m_Msg = string.Format("Save Shower Mode : {0}->{1}", m_TankPara.CurShowerMode.ToString(), m_ShowerModeReq.ToString());
                            m_TankPara.CurShowerMode = m_ShowerModeReq;
                            m_TankPara.UpdateData();
                        }

                        nSeqNo = 600;
                    }
                    else if (GetElapsedTicks() > diSupplyTimeout)
                    {
                        m_TankUnit.DiSupplyAutoValve.Close();

                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank DIW Supply Timeout Error");
                        m_AlarmId = m_TankUnit.ALM_MixTankDiSupplyTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 500;
                        nSeqNo = 1000;
                    }
                    break;
                case 600:
                    {
                        if ((m_TankUnit.HeaterUnit == null) || (m_TankUnit.HeaterUnit != null && m_IfFlag.MixTankHeaterReady))
                        {
                            if (m_IfFlag.MixtureComp && ((int)m_IfFlag.TankStatus < (int)TankStatus.STANDBY))
                            {
                                m_IfFlag.TankStatus = TankStatus.STANDBY;
                                m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Heater Ready : Renewal of Mix Tank Completed");
                            }
                        }

                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset");
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        nSeqNo = m_ReturnSeqNo;
                        m_Msg = string.Format("Return SeqNo = {0}", nSeqNo.ToString());
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqMixTankWashing : XSeqFunction
    {
        #region Fields
        protected MixTankUnit m_TankUnit;
        protected DetergentUnit m_DetUnit;
        protected TagMixTankIfFlag m_IfFlag;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMixTankControl m_Control;
        protected _GenInfoHandler m_GenInfo;
        private string m_Msg;
        private int m_WashingCount = 0;
        #endregion

        #region Constructor
        public SeqMixTankWashing(ThreadMixTankControl control, DetergentUnit detUnit, MixTankUnit tank)
        {
            m_TankUnit = tank;
            m_DetUnit = detUnit;
            m_IfFlag = m_TankUnit.IfFlag;
            m_Server = m_TankUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfo = GenInfoHandler.Instance;

            m_SeqFunName = m_TankUnit.Name + " Washing";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            bool runCond = true;
            runCond &= m_Server.JobCond.HeavyInterlock <= 0;

            if (!m_GenInfo.EqpInitComp || !runCond) return -1;

            int nSeqNo = this.m_SeqNo;

            MixTankMode showerMode = (m_TankUnit.SetupMixTankMode.GetValue<bool>()) ? MixTankMode.DET : MixTankMode.DI;
            bool tankUse = m_TankUnit.SetupTankUse.GetValue<bool>();
            int useTime = m_TankUnit.SetupTankUseLifeTime.GetValue<int>();
            int glassCount = m_TankUnit.SetupGlassLimitCount.GetValue<int>();
            int recycleNo = m_TankUnit.SetupRecycleNo.GetValue<int>();
            int drainNo = m_TankUnit.SetupDrainNo.GetValue<int>();
            int diSupplyTimeout = m_TankUnit.SetupDISupplyOverTime.GetValue<int>() * 1000;
            int drainTimeout = m_TankUnit.SetupDrainOverTime.GetValue<int>() * 1000;
            int washingNo = m_TankUnit.SetupWashingNo.GetValue<int>();
            int washingTime = m_TankUnit.SetupWashingTime.GetValue<int>() * 1000 * 60;
            int drainWaitTime = 60 * 1000;

            switch (nSeqNo)
            {
                case 0:
                    if (m_IfFlag.MixTankWashingReq)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Mix Tank Washing Request : true");
                        m_IfFlag.MixTankWashingReq = false;
                        m_WashingCount = 0;
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (m_TankUnit.Items.MixTankDrainCount >= drainNo ||
                            ((m_DetUnit.Items.CurMixTankId == m_TankUnit.Id) &&
                            (m_TankUnit.Items.CurShowerMode == MixTankMode.DET) &&
                            (showerMode == MixTankMode.DI)))
                        {
                            if (m_TankUnit.Items.MixTankDrainCount >= drainNo)
                            {
                                m_Msg = string.Format("STATUS : Drain Count({0}/{1})", m_TankUnit.Items.MixTankDrainCount.ToString(), drainNo.ToString());
                            }
                            else
                            {
                                m_Msg = string.Format("STATUS : Change Mode(DET->DIW)");
                            }
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);

                            // Mix Tank Washing for Drain Count Reset
                            m_TankUnit.Items.MixTankDrainCount = 0;
                            m_TankUnit.Items.UpdateData();

                            if ((int)m_IfFlag.TankStatus < (int)TankStatus.WASHING)
                            {
                                m_IfFlag.TankStatus = TankStatus.WASHING;
                            }
                            nSeqNo = 200;
                        }
                        else
                        {
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "STATUS : Mix Tank Washing Skip");
                            m_WashingCount = washingNo + 1;
                            m_IfFlag.MixTankWashingReq = false;
                            if ((int)m_IfFlag.TankStatus < (int)TankStatus.WASHING)
                            {
                                m_IfFlag.TankStatus = TankStatus.WASHING;
                            }
                            nSeqNo = 340;
                        }
                    }
                    break;
                case 100:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Start");
                        m_TankUnit.DrainAutoValve.Open();
                        if ((int)m_IfFlag.TankStatus < (int)TankStatus.WASHING)
                        {
                            m_IfFlag.TankStatus = TankStatus.WASHING;
                        }
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    if (!m_TankUnit.BottomLevel)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Level LL Sensor OFF(1st)");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 120;
                    }
                    else if (m_TankUnit.BottomLevel && (GetElapsedTicks() > drainTimeout))
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Timeout Error");
                        m_TankUnit.DrainAutoValve.Close();
                        m_AlarmId = m_TankUnit.ALM_MixTankDrainTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 100;
                        nSeqNo = 1000;
                    }
                    else if (!m_TankUnit.DrainAutoValve.IsOpen())
                    {
                        //Mix Tank Drain 중에 Auto->Manual->Auto 전환 했을 경우 Valve 재설정
                        m_TankUnit.DrainAutoValve.Open();
                    }
                    break;
                case 120:
                    if (GetElapsedTicks() > drainWaitTime)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain End");
                        m_TankUnit.DrainAutoValve.Close();
                        nSeqNo = 200;
                    }
                    break;
                case 200:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank DIW Supply Start");
                        m_TankUnit.DiSupplyAutoValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 210;
                    }
                    break;
                case 210:
                    if (m_TankUnit.SupplyStop)
                    {
                        m_Msg = string.Format("Tank DI Supply End(Washing Time : {0}sec)", washingTime.ToString());
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        m_TankUnit.DiSupplyAutoValve.Close();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 220;
                    }
                    else if (GetElapsedTicks() > diSupplyTimeout)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank DIW Supply Timeout Error");
                        m_TankUnit.DiSupplyAutoValve.Close();
                        m_AlarmId = m_TankUnit.ALM_MixTankDiSupplyTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 200;
                        nSeqNo = 1000;
                    }
                    else if (!m_TankUnit.DiSupplyAutoValve.IsOpen())
                    {
                        //Mix Tank DI Supply 중에 Auto->Manual->Auto 전환 했을 경우 Valve 재설정
                        m_TankUnit.DiSupplyAutoValve.Open();
                    }
                    break;
                case 220:
                    if (GetElapsedTicks() > washingTime)
                    {
                        m_Msg = string.Format("Washing Time is over(Washing Time : {0}sec)", washingTime.ToString());
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        m_WashingCount += 1;
                        nSeqNo = 300;
                    }
                    break;
                case 300:
                    {
                        if (m_WashingCount >= washingNo)
                            nSeqNo = 310;
                        else
                        {
                            m_Msg = string.Format("STATUS : Washing Count({0}/{1})", m_WashingCount.ToString(), washingNo.ToString());
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 310:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "END : Tank Full Drain Start");
                        m_TankUnit.DrainAutoValve.Open();
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 320;
                    }
                    break;
                case 320:
                    if (!m_TankUnit.BottomLevel)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Level LL Sensor OFF(1st)");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 330;
                    }
                    else if (m_TankUnit.BottomLevel && (GetElapsedTicks() > drainTimeout))
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Full Drain Timeout Error");
                        m_TankUnit.DrainAutoValve.Close();
                        m_AlarmId = m_TankUnit.ALM_MixTankDrainTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = 310;
                        nSeqNo = 1000;
                    }
                    else if (!m_TankUnit.DrainAutoValve.IsOpen())
                    {
                        //Mix Tank Drain 중에 Auto->Manual->Auto 전환 했을 경우 Valve 재설정
                        m_TankUnit.DrainAutoValve.Open();
                    }
                    break;
                case 330:
                    if (GetElapsedTicks() > drainWaitTime)
                    {
                        if (m_WashingCount > 1000)
                        {
                            m_Msg = "END : Tank Washing Skip";
                        }
                        else
                        {
                            m_Msg = "END : Tank Washing Completed";
                        }

                        m_TankUnit.DrainAutoValve.Close();
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        m_IfFlag.MixTankWashingReq = false;
                        m_IfFlag.MixTankWashingComp = true;
                        m_WashingCount = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 340:
                    {
                        m_IfFlag.MixTankWashingReq = false;
                        m_IfFlag.MixTankWashingComp = true;
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "END : Tank Washing Completed");
                        m_WashingCount = 0;
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Alarm reset");
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqMeasureChemical : XSeqFunction
    {
        #region Fields
        protected MixTankUnit m_TankUnit;
        protected DetergentUnit m_DetUnit;
        protected TagMixTankIfFlag m_IfFlag;
        protected MixTankItem m_TankPara;
        protected DetergentUnitItem m_DetPara;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMixTankControl m_Control;
        protected int m_SimulSensor = 0;
        protected static _GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqMeasureChemical(ThreadMixTankControl control, DetergentUnit detUnit, MixTankUnit tank)
        {
            m_TankUnit = tank;
            m_DetUnit = detUnit;
            m_TankPara = m_TankUnit.Items;
            m_DetPara = m_DetUnit.Items;
            m_IfFlag = m_TankUnit.IfFlag;
            m_Server = m_TankUnit.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfo = GenInfoHandler.Instance;

            m_SeqFunName = m_TankUnit.Name + " Chemical";
        }
        #endregion

        #region Sequencer
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            bool runCond = true;
            runCond &= !m_Control.IsInterlock(m_TankUnit);
            runCond &= m_Server.JobCond.ProcessMode;
            runCond &= m_GenInfo.EqpInitComp &&
                       m_GenInfo.AutoMode;

            if (!runCond)
            {
                if (m_TankUnit.DetSupplyAutoValve.IsOpen())
                {
                    m_TankUnit.DetSupplyAutoValve.Close();
                }
                return -1;
            }

            MixTankMode showerMode = (m_TankUnit.SetupMixTankMode.GetValue<bool>()) ? MixTankMode.DET : MixTankMode.DI;
            bool tankUse = m_TankUnit.SetupTankUse.GetValue<bool>();

            switch (nSeqNo)
            {
                case 0:
                    if (showerMode == MixTankMode.DET && m_IfFlag.MixTankDetReq)
                    {
                        m_IfFlag.MixTankDetReq = false;
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Chemical Request");
                        nSeqNo = 100;
                    }
                    else if (m_IfFlag.MixTankDetReq)
                    {
                        m_IfFlag.MixTankDetReq = false;
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "START : Chemical Not Use Standby Skip");
                        nSeqNo = 200;
                    }
                    break;
                case 100:
                    {
                        //density setting
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "MEASURE : Detergent Supply Valve Open");
                        m_TankUnit.DetSupplyAutoValve.Open();
                        nSeqNo = 120;
                    }
                    break;
                case 120:
                    if (m_TankUnit.SupplyStop)
                    {
                        m_TankUnit.DetSupplyAutoValve.Close();
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "MEASURE : Detergent Level Confirm");
                        nSeqNo = 130;
                    }
                    else if (!m_TankUnit.DetSupplyAutoValve.IsOpen())
                    {
                        //Mix Tank Detergent Supply 중에 Auto->Manual->Auto 전환 했을 경우 Valve 재설정
                        m_TankUnit.DetSupplyAutoValve.Open();
                    }
                    break;
                case 130:
                    {
                        //density check
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "MEASURE : Detergent Density Confirm");
                        m_IfFlag.MixTankDetComp = true;
                        nSeqNo = 0;
                    }
                    break;
                case 200:
                    {
                        m_TankUnit.SetLog(m_SeqFunName, 0, 0, "MEASURE : Detergent Mixing Completed");
                        m_IfFlag.MixTankDetComp = true;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqMixTankLevelSensor : XSeqFunction
    {
        #region Fields
        protected MixTankUnit m_TankUnit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMixTankControl m_Control;
        #endregion

        #region Constructor
        public SeqMixTankLevelSensor(ThreadMixTankControl control, MixTankUnit tank)
        {
            m_TankUnit = tank;
            m_EqpManager = m_TankUnit.ServerManager.EqpStateManager;
            m_Control = control;

            m_SeqFunName = m_TankUnit.Name + " LEVEL";
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
                            //if (m_TankUnit.AutoValve.IsOpen())
                            //{
                            //    m_TankUnit.AutoValve.Close();
                            //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            //}

                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_TankUnit.SetLog(m_SeqFunName, 0, 0, "Tank Top Level : Fault");
                        }
                        else if (m_TankUnit.TopLevel)
                        {
                            //if (m_TankUnit.AutoValve.IsOpen())
                            //{
                            //    m_TankUnit.AutoValve.Close();
                            //    m_TankUnit.SetLog(SeqFunName, 0, 0, "Auto Valve : Close(Top Level)");
                            //}

                            m_TankUnit.IfFlag.TankLevelFault = true;
                            m_AlarmId = m_TankUnit.ALM_MixTankHighLevel.Id;
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
                            m_AlarmId = m_TankUnit.ALM_MixTankLevelFault.Id;
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

    public class SeqSimulationMixTankLevelSensor : XSeqFunction
    {
        #region Enum
        private enum SimulType
        {
            Only1Tank,
            ExchangeTank
        }
        #endregion

        #region Fields
        protected static ThreadMixTankControl m_Control;
        protected MixTankUnit m_TankUnit;
        protected IServerManager m_Server;
        //private double m_SimulTankLevel;
        private int m_Count;
        private int m_Level = 0;
        private SimulType m_Type;
        #endregion

        public SeqSimulationMixTankLevelSensor(ThreadMixTankControl control, MixTankUnit tank)
        {
            m_Control = control;
            m_TankUnit = tank;
            m_Server = m_TankUnit.ServerManager;
            m_Count = m_TankUnit.TankLevel.Count;
        }

        public override int Do()
        {
            if (AppConfig.Instance.Simul.Device == false) return -1;

            /*if (m_TankUnit.DetSupplyAutoValve.IsOpen() || m_TankUnit.DiSupplyAutoValve.IsOpen())
            {
                m_SimulTankLevel += 1.0;
            }
            if (m_TankUnit.DrainAutoValve.IsOpen())
            {
                m_SimulTankLevel -= 2.0;
            }
            if (m_TankUnit.DetOutAutoValve.IsOpen() && m_TankUnit.PumpRun)
            {
                m_SimulTankLevel -= 0.5;
            }
            if (m_TankUnit.ReturnAutoValve.IsOpen() && m_TankUnit.PumpRun)
            {
                m_SimulTankLevel += 0.5;
            }

            int setupMaxLevel = m_TankUnit.TankLevel.SetupTankLevel.GetValue<int>();
            if (m_SimulTankLevel >= setupMaxLevel)
            {
                m_SimulTankLevel = setupMaxLevel;
            }

            int curLevel;

            if (m_SimulTankLevel >= setupMaxLevel) curLevel = m_Count;
            else if (m_SimulTankLevel < setupMaxLevel && m_SimulTankLevel >= 60) curLevel = m_Count - 1;
            else if (m_SimulTankLevel < 60 && m_SimulTankLevel >= 40) curLevel = m_Count - 2;
            else if (m_SimulTankLevel < 40 && m_SimulTankLevel >= 10) curLevel = m_Count - 3;                                                                                 
            else curLevel = -1;

            for (int i = 0; i < m_Count; i++)
            {
                if (i <= curLevel) m_TankUnit.TankLevel.Sensors[i].DiSensor.SetState(true);
                else m_TankUnit.TankLevel.Sensors[i].DiSensor.SetState(false);
            }*/

            int nSeqNo = this.m_SeqNo;

            m_Type = SimulType.Only1Tank;

            int drainTime = 0;
            if (m_Type == SimulType.ExchangeTank)
            {
                drainTime = 6000;
            }
            else
            {
                drainTime = (m_TankUnit.SetupTankUseLifeTime.GetValue<int>() / (m_Count - 2)) * 60 * 1000;
            }

            switch (nSeqNo)
            {
                case 0:
                    if (m_TankUnit.DetSupplyAutoValve.IsOpen() || m_TankUnit.DiSupplyAutoValve.IsOpen())
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else if (m_TankUnit.DetOutAutoValve.IsOpen() || m_TankUnit.DrainAutoValve.IsOpen())
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 10:
                    if (m_TankUnit.DetSupplyAutoValve.IsOpen() || m_TankUnit.DiSupplyAutoValve.IsOpen())
                    {
                        if (GetElapsedTicks() > 2000)
                        {
                            if (m_Level < 0)
                            {
                                m_Level = 0;
                                nSeqNo = 0;
                            }
                            else if (m_Level < m_Count)
                            {
                                m_TankUnit.TankLevel.Sensors[m_Level].SetState(true);
                                m_Level += 1;
                                m_StartTicks = XFunc.GetTickCount();
                            }
                            else
                            {
                                m_Level -= 1;
                                nSeqNo = 0;
                            }
                        }
                    }
                    else
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    if (m_TankUnit.DetOutAutoValve.IsOpen())
                    {
                        if (GetElapsedTicks() > drainTime)
                        {
                            if (m_Level >= m_Count)
                            {
                                m_Level = m_Count - 1;
                            }
                            else if (m_Level < m_Count && m_Level >= 0)
                            {
                                m_TankUnit.TankLevel.Sensors[m_Level].SetState(false);
                                m_Level -= 1;
                                m_StartTicks = XFunc.GetTickCount();
                            }
                            else
                            {
                                m_Level += 1;
                                nSeqNo = 0;
                            }
                        }
                    }
                    else if (m_TankUnit.DrainAutoValve.IsOpen())
                    {
                        if (GetElapsedTicks() > 6000)
                        {
                            if (m_Level >= m_Count)
                            {
                                m_Level = m_Count - 1;
                            }
                            else if (m_Level < m_Count && m_Level >= 0)
                            {
                                m_TankUnit.TankLevel.Sensors[m_Level].SetState(false);
                                m_Level -= 1;
                                m_StartTicks = XFunc.GetTickCount();
                            }
                            else
                            {
                                m_Level += 1;
                                nSeqNo = 0;
                            }
                        }
                    }
                    else
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
    }

    public class SeqGlassExistforDetergentUnit : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DetergentUnit> m_DetUnits;
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static ThreadMixTankControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private int m_DetCount = 0;
        #endregion

        #region Constructor
        public SeqGlassExistforDetergentUnit(ThreadMixTankControl control, IServerManager server)
        {
            m_Server = server;
            m_DetUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DetergentUnit>();
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "MIXGLSEXIST";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1; -> processunit의 init이 끝났을때
            if (!m_GenInfos.AutoMode) return -1;
            if (m_DetUnits.Count == 0) return -1;

            int idleWaitTime = 60 * 1000 * m_Server.SetupIdleWaitTime.GetValue<int>();

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Server.GlassData.Count == 0)
                        {
                            nSeqNo = 20;
                        }
                        else nSeqNo = 20;
                    }
                    break;
                case 10:
                    {
                        if (m_Server.GlassData.Count == 0)
                        {
                            for (int i = 0; i < m_DetCount; i++)
                            {
                                m_DetUnits[i].ChemicalStart = false;
                            }
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else if ((m_Server.GlassData.Count > 0) && m_Server.JobCond.ProcessMode)
                        {
                            bool processRequired = false;
                            foreach (CvUnit cv in m_CvUnits)
                            {
                                processRequired |= m_Server.GlassData.IsExist(cv.Id * 2 + 0);
                                processRequired |= m_Server.GlassData.IsExist(cv.Id * 2 + 1);
                            }

                            for (int i = 0; i < m_DetUnits.Count; i++)
                            {
                                DetergentUnit detUnit = m_DetUnits[i];
                                if (!processRequired && detUnit.ChemicalStart)
                                {
                                    detUnit.ChemicalStart = false;
                                }
                                else if (processRequired && !detUnit.ChemicalStart)
                                {
                                    detUnit.ChemicalStart = true;
                                }
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (m_Server.GlassData.Count > 0)
                        {
                            m_GenInfos.ChemicalIdleRunning = false;
                            nSeqNo = 10;
                        }
                        else if (!m_GenInfos.EqpInitComp)
                        {
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (!m_GenInfos.ChemicalIdleRunning && (GetElapsedTicks() > idleWaitTime))
                        {
                            m_GenInfos.ChemicalIdleRunning = true;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqModeChangeforDetrgentUnit : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadMixTankControl m_Control;
        protected static _GenInfoHandler m_GenInfo;
        protected static _GenericCollection<MixTankUnit> m_MixTanks;
        protected static _GenericCollection<DetergentUnit> m_DetUnits;
        protected bool[] m_ChemicalStart;
        private int m_DetUnitCounts = 0;
        #endregion

        #region Constructor
        public SeqModeChangeforDetrgentUnit(ThreadMixTankControl control, IServerManager server)
        {
            m_Control = control;
            m_Server = server;
            m_GenInfo = GenInfoHandler.Instance;
            m_MixTanks = ThreadMixTankControl.MixTankUnits;
            m_DetUnits = ThreadMixTankControl.DetergentUnits;
            m_DetUnitCounts = m_DetUnits.Count;
            m_ChemicalStart = new bool[m_DetUnitCounts];

            m_SeqFunName = "MIXMODE ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfo.AutoMode)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_GenInfo.AutoMode)
                    {
                        for (int i = 0; i < m_DetUnitCounts; i++)
                        {
                            m_ChemicalStart[i] = m_DetUnits[i].ChemicalStart;
                            m_DetUnits[i].ChemicalStart = false;
                        }
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (m_GenInfo.AutoMode)
                    {
                        int count = m_MixTanks.Count;
                        for (int i = 0; i < count; i++)
                        {
                            MixTankUnit tank = m_MixTanks[i];
                            tank.DetOutAutoValve.Close();
                            tank.ReturnAutoValve.Close();
                            tank.DetSupplyAutoValve.Close();
                            tank.DiSupplyAutoValve.Close();
                            tank.DrainAutoValve.Close();
                        }
                        for (int i = 0; i < m_DetUnitCounts; i++)
                        {
                            m_ChemicalStart[i] = (m_Server.GlassData.Count > 0);
                            m_DetUnits[i].ChemicalStart = m_ChemicalStart[i];
                        }
                        nSeqNo = 10;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqChemicalIdleRunning : XSeqFunction
    {
        #region Fields
        protected static ThreadMixTankControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected static IServerManager m_Server;
        protected DetergentUnit m_DetUnit;
        protected int m_OldTime = 0;
        protected XTimer m_Timer;
        protected int m_Tm;
        protected int m_ProgressTime;
        #endregion

        #region Constructor
        public SeqChemicalIdleRunning(ThreadMixTankControl control, DetergentUnit detUnit)
        {
            m_Server = m_DetUnit.ServerManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_DetUnit = detUnit;

            m_Timer = new XTimer("Timer : SeqChemicalIdleRunning");
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int RunTime = m_DetUnit.SetupDetIdleRunTime.GetValue<int>() * (int)DetergentUnit.IdleRunTimeUnit;
            int StopTime = m_DetUnit.SetupDetIdleStopTime.GetValue<int>() * (int)DetergentUnit.IdleRunTimeUnit;
            int nTime = 0;

            bool runCond = true;
            runCond &= m_GenInfos.EqpInitComp;
            runCond &= m_GenInfos.AutoMode;
            runCond &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            runCond &= (m_Server.EqpStateManager.EqpUnit.EqpState != EqpState.Fault);

            bool idleCond = true;
            idleCond &= runCond;
            idleCond &= m_Server.JobCond.ProcessMode;
            idleCond &= m_DetUnit.SetupDetIdleUse.GetValue<bool>();
            idleCond &= m_GenInfos.ChemicalIdleRunning;
            idleCond &= (RunTime > 0);
            idleCond &= !m_GenInfos.CycleStop;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (idleCond)
                    {
                        m_DetUnit.ChemicalStart = (StopTime == 0);
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!idleCond)
                    {
                        m_DetUnit.ChemicalStart = false;
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_OldTime = 0;
                        m_Tm = StopTime * 10;
                        m_Timer.Start(m_Tm);
                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();
                        SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_START, StopTime);
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        nTime = (int)GetElapsedTicks() / 1000;
                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!idleCond)
                        {
                            m_DetUnit.ChemicalStart = false;
                            SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > StopTime * 1000)
                        {
                            m_DetUnit.ChemicalStart = true;
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();
                            SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_START, RunTime);
                            m_OldTime = 0;
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        nTime = (int)GetElapsedTicks() / 1000;
                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!idleCond)
                        {
                            m_DetUnit.ChemicalStart = false;
                            SetIdleRunningProgress(m_DetUnit.Id, ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > RunTime * 1000)
                        {
                            m_DetUnit.ChemicalStart = (StopTime == 0);
                            nSeqNo = 10;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion

        #region General Methods
        protected virtual void SetIdleRunningProgress(int detUnitId, ProgressAct act, int time)
        {
            switch (act)
            {
                case ProgressAct.PROGRESS_START:
                    {
                        m_ProgressTime = time;
                        m_GenInfos.ChemicalIdleRunningProgress = m_ProgressTime.ToString();
                    }
                    break;
                case ProgressAct.PROGRESS_END:
                    {
                        m_GenInfos.ChemicalIdleRunningProgress = "0";
                    }
                    break;
                case ProgressAct.PROGRESS_SET:
                    {
                        string val = string.Format("{0} / {1}", time, m_ProgressTime);
                        m_GenInfos.ChemicalIdleRunningProgress = val;
                    }
                    break;
            }
        }
        #endregion
    }
}

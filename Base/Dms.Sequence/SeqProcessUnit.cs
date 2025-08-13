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
    public delegate ProcessCondition GetProcessRunConditionDelegate(ProcessUnit unit, bool previouslyAutoMode, bool previouslySemiAutoMode);

    public class ThreadProcessUnit : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<ProcessUnit> m_Units;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqGlassExist(this, m_Server));
            RegisterSequence(new SeqModeChange(this, m_Server));
            RegisterSequence(new SeqDiwIdleRunning(this, m_Server));
        }
        #endregion 

        #region Contstructor
        public ThreadProcessUnit(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<ProcessUnit>();

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
        public static _GenericCollection<ProcessUnit> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<ProcessUnit>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqProcessUnit : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadProcessUnit m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected ProcessUnit m_Unit;
        protected GetProcessRunConditionDelegate GetProcessRunCondition;
        protected bool m_PreviousAutoMode = true; // jemoon : auto에서 manual전환시 stop해줘야하므로
        protected bool m_PreviousSemiAutoMode = true; // jemoon : DiStart에서 DiStop전환시 stop해줘야하므로
        #endregion

        #region Constructor
        public SeqProcessUnit(ThreadProcessUnit control, ProcessUnit unit, GetProcessRunConditionDelegate getRunCondition)
        {
            m_Unit = unit;
            m_Server = m_Unit.ServerManager;
            m_Control = control;
            GetProcessRunCondition = getRunCondition;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = m_Unit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {   // jemoon : 처음들어왔을때 안정화 시간을 주고
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 1000)
                    {
                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    {   // Process control
                        ProcessCondition refProcess = GetProcessRunCondition(m_Unit, m_PreviousAutoMode, m_PreviousSemiAutoMode);
                        m_Unit.RefProcessCondition = refProcess;

                        switch (refProcess)
                        {
                            case ProcessCondition.Stop:
                                {
                                    m_Unit.Stop();
                                }
                                break;
                            case ProcessCondition.Run:
                                {
                                    if (!m_Unit.IsRun()) m_Unit.Run();
                                }
                                break;
                            case ProcessCondition.DontCare:
                                // Don't care, accept any manual operation
                                break;
                        }

                        // AutoMode에서 ManualMode로 바뀌는 시점관리를 위해서
                        m_PreviousAutoMode = m_GenInfos.AutoMode;
                        m_PreviousSemiAutoMode = m_GenInfos.DiStart;

                        nSeqNo = 100;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }

    public class SeqGlassExist : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static ThreadProcessUnit m_Control;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqGlassExist(ThreadProcessUnit control, IServerManager server)
        {
            m_Server = server;
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "GLSEXIST";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1; -> processunit의 init이 끝났을때
            if (!m_GenInfos.AutoMode) return -1;

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
                            m_GenInfos.DiStart = false;
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

                            if (!processRequired && m_GenInfos.DiStart)
                            {
                                m_GenInfos.DiStart = false;
                            }
                            else if (processRequired && !m_GenInfos.DiStart)
                            {
                                m_GenInfos.DiStart = true;
                            }
                        }
                    }
                    break;
                case 20:
                    {
                        if (m_Server.GlassData.Count > 0)
                        {
                            m_GenInfos.IdleRunning = false;
                            nSeqNo = 10;
                        }
                        else if (!m_GenInfos.EqpInitComp)
                        {
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (!m_GenInfos.IdleRunning && (GetElapsedTicks() > idleWaitTime))
                        {
                            m_GenInfos.IdleRunning = true;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqModeChange : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadProcessUnit m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected bool diStart;
        #endregion

        #region Constructor
        public SeqModeChange(ThreadProcessUnit control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "MODE    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.AutoMode)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_GenInfos.AutoMode)
                    {
                        diStart = m_GenInfos.DiStart;
                        m_GenInfos.DiStart = false;

                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (m_GenInfos.AutoMode)
                    {
                        diStart &= (m_Server.GlassData.Count > 0);
                        m_GenInfos.DiStart = diStart;
                        nSeqNo = 10;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqDiwIdleRunning : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadProcessUnit m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected int m_OldTime = 0;
        protected XTimer m_Timer;
        protected int m_Tm;
        protected int m_ProgressTime;
        #endregion

        #region Constructor
        public SeqDiwIdleRunning(ThreadProcessUnit control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_Timer = new XTimer("Timer : SeqIdleRunning");
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int RunTime = (int)m_Server.JobCond.SetupIdleRunTime;
            int StopTime = (int)m_Server.JobCond.SetupIdleStopTime * 60;
            int nTime = 0;

            bool idleCond = true;
            idleCond &= m_GenInfos.EqpInitComp;   //->pumpseq의 initcomp확인
            idleCond &= m_GenInfos.AutoMode;
            idleCond &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            idleCond &= (m_Server.EqpStateManager.EqpUnit.EqpState != EqpState.Fault);
            idleCond &= m_Server.JobCond.ProcessMode;
            idleCond &= m_Server.JobCond.SetupIdleUse;
            idleCond &= m_GenInfos.IdleRunning;
            idleCond &= (RunTime > 0);
            idleCond &= !m_GenInfos.CycleStop;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (idleCond)
                    {
                        m_GenInfos.DiStart = (StopTime == 0);
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!idleCond)
                    {
                        m_GenInfos.DiStart = false;
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_OldTime = 0;
                        m_Tm = StopTime * 10;
                        m_Timer.Start(m_Tm);
                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();
                        SetIdleRunningProgress(ProgressAct.PROGRESS_START, StopTime);
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        nTime = (int)GetElapsedTicks() / 1000;
                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!idleCond)
                        {
                            m_GenInfos.DiStart = false;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > StopTime * 1000)
                        {
                            m_GenInfos.DiStart = true;
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();
                            SetIdleRunningProgress(ProgressAct.PROGRESS_START, RunTime);
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
                            SetIdleRunningProgress(ProgressAct.PROGRESS_SET, nTime);
                        }
                        if (!idleCond)
                        {
                            m_GenInfos.DiStart = false;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > RunTime * 1000)
                        {
                            m_GenInfos.DiStart = (StopTime == 0);
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
        private void SetIdleRunningProgress(ProgressAct act, int time)
        {
            switch (act)
            {
                case ProgressAct.PROGRESS_START:
                    {
                        m_ProgressTime = time;
                        m_GenInfos.IdleRunningProgress = m_ProgressTime.ToString();
                    }
                    break;
                case ProgressAct.PROGRESS_END:
                    {
                        m_GenInfos.IdleRunningProgress = "0";
                    }
                    break;
                case ProgressAct.PROGRESS_SET:
                    {
                        string val = string.Format("{0} / {1}", time, m_ProgressTime);
                        m_GenInfos.IdleRunningProgress = val;
                    }
                    break;
            }
        }
        #endregion
    }
}

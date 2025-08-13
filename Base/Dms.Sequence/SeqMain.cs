using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.Sequence;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadMain : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static List<XSeqInitFunction> m_SeqInitFunctions = new List<XSeqInitFunction>();
        protected static _GenInfoHandler m_GenInfos;
        protected System.Threading.Timer timerTactTime;
        #endregion

        #region Properties
        public List<XSeqInitFunction> SeqInitFunctions
        {
            get { return m_SeqInitFunctions; }
        }
        #endregion

        #region Constructor
        public ThreadMain(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_GenInfos = GenInfoHandler.Instance;

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqInit(this, m_Server));
            RegisterSequence(new SeqEqpProcessState(this, m_Server));
            RegisterSequence(new SeqEqpState(this, m_Server));
            RegisterSequence(new SeqCheckGlassExist(this, m_Server));
            RegisterSequence(new SeqCleanOut(this, m_Server));
            RegisterSequence(new SeqTodayGlassCntClear(this, m_Server));//2010.06.29 kimgun
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                ExceptionHandle();

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                if (timerTactTime == null)
                {
                    timerTactTime = new System.Threading.Timer(new TimerCallback(CheckTactTime), null, 0, 1000);
                }

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
        #endregion

        #region Virtual Methods
        public virtual void CheckTactTime(object stateInfo)
        {
            bool heavyInterlock = m_Server.JobCond.HeavyInterlock > 0;

            if (BaseGlobalVar.StartCount &&// m_Server.SeqFlag.StartCount &&
                m_GenInfos.AutoMode &&
                !m_GenInfos.Pause &&
                !BaseGlobalVar.StopCount1 &&// !m_Server.SeqFlag.StopCount1 &&
                !heavyInterlock)
            {
                m_GenInfos.TactTime++;
                //if (m_GenInfos.TactTime >= m_Server.JobCond.DelayTactTime)
                if (m_GenInfos.TactTime >= m_Server.JobCond.TactTime)
                {
                    BaseGlobalVar.TimeOver = true;// m_Server.SeqFlag.TimeOver = true;
                }
            }

            //m_Server.CheckSystemStatus();
            //m_Server.CheckDeviceControllerState();
        }
        #endregion

        #region General Methods
        public void AddSeqInitFunction(XSeqInitFunction func)
        {
            m_SeqInitFunctions.Add(func);
        }

        public void ExceptionHandle()
        {
            //if (XFunc.ExceptionHandler.Count > 0)
            //{
            //    m_Server.UninitializeByException();
            //
            //    foreach (Exception err in XFunc.ExceptionHandler)
            //    {
            //        string msg = err.ToString();
            //        m_Server.WriteExceptionLog(msg);
            //        MessageBox.Show(msg);
            //    }
            //    Application.Exit();
            //}		

            if (XExceptionHandler.ShutDownCondition)
            {
                m_Server.UninitializeByException();
                Application.Exit();
            }
        }
        #endregion
    }

    public class SeqInit : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadMain m_Control;
        protected static GenInfoHandler m_GenInfos;
        protected List<XSeqInitFunction> m_InitFuncs;
        #endregion

        #region Properies
        public List<XSeqInitFunction> InitFuncs
        {
            get { return m_InitFuncs; }
        }
        #endregion

        #region Constructor
        public SeqInit(ThreadMain control, IServerManager sever)
        {
            m_Control = control;
            m_Server = sever;
            m_Eqp = m_Server.EqpStateManager;
            m_InitFuncs = m_Control.SeqInitFunctions;
            m_GenInfos = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_GenInfos.EqpInitComp)
            {
                return -1;
            }

            int count = m_InitFuncs.Count;
            InitState[] initState = new InitState[count];
            for (int i = 0; i < count; i++)
            {
                initState[i] = (InitState)m_InitFuncs[i].Do();
            }

            int nSeqNo = m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    if (GetElapsedTicks() > 500)
                    {
                        m_GenInfos.EqpInitComp = false;

                        int compCount = 0;
                        bool initFail = false;
                        foreach (InitState state in initState)
                        {
                            if (state == InitState.Fail)
                            {
                                initFail = true;
                            }
                            else if (state == InitState.Comp)
                            {
                                compCount++;
                            }
                        }

                        if (initFail)
                        {
                            nSeqNo = 1000;
                        }
                        else if (compCount == m_InitFuncs.Count)
                        {
                            if (m_GenInfos.CleanOut)
                            {
                                MessageBox.Show("Convert to Online Mode and Process Recovery Mode ", "WSSD",
                                MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); //09.12.28 minhan
                            }
                            m_GenInfos.EqpInitReq = false;
                            m_GenInfos.EqpInitComp = true;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_GenInfos.EqpInitReq = false;
                        nSeqNo = 0;
                    }
                    break;
            }
            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEqpProcessState : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadMain m_Control;
        protected static GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqEqpProcessState(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfo = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;

            int nSeqNo = m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfo.EQPGlassCount > 0)
                    {
                        m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Pause;
                        m_GenInfo.Pause = true;

                        nSeqNo = 20;
                    }
                    else
                    {
                        m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Init;
                        m_GenInfo.CycleStop = true;

                        nSeqNo = 20;
                    }
                    break;
                case 10:
                    if (m_GenInfo.EQPGlassCount == 0)
                    {
                        if (m_GenInfo.Pause) m_GenInfo.Pause = false;

                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 20;
                    }
                    else if (m_Server.EqpStateManager.EqpUnit.ProcessState != ProcessState.Excute)
                    {//2009.08.19 kimgun 혹시 run상황인데 idle이라고 우기는 경우가 있어서리..
                        if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Idle)
                        {
                            BaseGlobalVar.EqpState = 'R';
                            BaseGlobalVar.EqpStatusChangeReq = true;
                            m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Excute;
                        }
                    }
                    break;
                case 20:
                    if (m_GenInfo.EQPGlassCount > 0)
                    {
                        if (BaseGlobalVar.EqpState != 'D')
                        {
                            BaseGlobalVar.EqpState = 'R';
                            BaseGlobalVar.EqpStatusChangeReq = true;
                            m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Excute;
                        }
                        nSeqNo = 10;
                    }
                    else if (m_Server.EqpStateManager.EqpUnit.ProcessState != ProcessState.Idle)
                    {
                        if (BaseGlobalVar.EqpState != 'D')
                        {
                            BaseGlobalVar.EqpState = 'I';
                            m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Idle;
                            BaseGlobalVar.EqpStatusChangeReq = true;
                        }
                    }
                    else if (m_GenInfo.EqpInitComp)
                    {
                        if (m_Server.EqpStateManager.EqpUnit.ProcessState != ProcessState.Idle &&
                            m_Server.EqpStateManager.EqpUnit.ProcessState != ProcessState.Excute &&
                            (m_GenInfo.EQPGlassCount > 0))
                        {
                            m_Server.EqpStateManager.EqpUnit.ProcessState = ProcessState.Excute;
                            BaseGlobalVar.EqpState = 'R';
                            BaseGlobalVar.EqpStatusChangeReq = true;
                        }
                    }

                    break;
            }
            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEqpState : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMain m_Control;
        protected static GenInfoHandler m_GenInfos;
        protected EqpState m_OldState = EqpState.UnKnown;
        #endregion

        #region Constructor
        public SeqEqpState(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_EqpManager.IsAlarmState && (m_OldState != EqpState.Fault))
                    {
                        m_OldState = EqpState.Fault;
                        m_Server.EqpStateManager.EqpUnit.EqpState = EqpState.Fault;
                        nSeqNo = 10;
                    }
                    else if (!m_GenInfos.EqpInitComp && (m_OldState != EqpState.UnKnown))
                    {
                        m_OldState = EqpState.UnKnown;
                        m_Server.EqpStateManager.EqpUnit.EqpState = EqpState.UnKnown;
                    }
                    else if (m_GenInfos.EqpInitComp && m_OldState != EqpState.Normal)
                    {
                        m_OldState = EqpState.Normal;
                        m_Server.EqpStateManager.EqpUnit.EqpState = EqpState.Normal;
                    }
                    break;
                case 10:
                    if (!m_EqpManager.IsAlarmState)//&& /*(m_GenInfos.EqpState == EqpState.Fault))*/(m_Server.Eqp.EqpState == EqpState.Fault))
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCheckGlassExist : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadMain m_Control;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqCheckGlassExist(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int nSeqNo = this.m_SeqNo;
            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Server.GlassData.Count == 0)
                        {
                            m_Server.Log("EQPGlassCount is 0");
                            nSeqNo = 20;
                        }
                        else nSeqNo = 20;
                    }
                    break;
                case 10:
                    if (m_Server.GlassData.Count == 0)
                    {
                        m_Server.Log("EQPGlassCount is 0 : TactTimeOver flag true");
                        BaseGlobalVar.TimeOver = true;// m_Server.SeqFlag.TimeOver = true;
                        m_Server.JobCond.SetTactTime(TactTimeAct.ttCOUNT_END);
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (m_Server.GlassData.Count > 0)
                    {
                        m_Server.Log(string.Format("EQPGlassCount is not {0}", m_Server.GlassData.Count));
                        nSeqNo = 10;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }

    public class SeqCleanOut : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server = null;
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static ThreadMain m_Control;
        protected static GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqCleanOut(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "CLEANOUT";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.CleanOut)
                    {
                        m_Server.Log("CleanOut : Set");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Server.GlassData.Count == 0)
                    {
                        bool glassExist = false;
                        foreach (CvUnit unit in m_CvUnits)
                        {
                            glassExist |= unit.GlsInSensor.IsDetected();
                            glassExist |= unit.GlsOutSensor.IsDetected();
                        }

                        if (!glassExist && BaseGlobalVar.GlassOutComp1)// m_Server.SeqFlag.GlassOutComp1)
                        {
                            m_GenInfos.EqpInitComp = false;
                            m_Server.Log("CleanOut : Confirm No Glass");

                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 500)
                    {
                        m_Server.Log("CleanOut : Finish");
                        m_GenInfos.CleanOut = false;
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqTodayGlassCntClear : XSeqFunction
    {
        #region Fields
        private IServerManager m_Server;
        private GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqTodayGlassCntClear(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_GenInfo = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    if (!HoldTimeCheck())
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    else
                    {
                        m_GenInfo.TodayGlassCount = 0;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 1000)
                    {
                        if (HoldTimeCheck())
                            m_GenInfo.TodayGlassCount = 0;

                        seqNo = 0;
                    }
                    break;
            }
            m_SeqNo = seqNo;
            return -1;
        }
        #endregion

        #region Methods
        private bool HoldTimeCheck()
        {
            bool bRv = false;
            DateTime curtime = DateTime.Now;
            string sCurTime = curtime.ToString("MMdd");
            if (sCurTime.CompareTo(BaseGlobalVar.SavedTime) != 0)
            {
                BaseGlobalVar.SavedTime = sCurTime;
                BaseGlobalVar.DailySavedTimeSet();
                bRv = true;
            }
            return bRv;
        }
        #endregion
    }

    #region Obsolete
    [Obsolete]
    public class SeqInitEqp : XSeqInitFunction
    {
        #region Fields
        protected InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadMain m_Control;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqInitEqp(ThreadMain control, IServerManager server)
        {
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            if (m_GenInfos.EqpInitReq)
            {
                m_InitState = InitState.Comp;
            }
            return (int)m_InitState;
        }
        #endregion
    }
    #endregion
}

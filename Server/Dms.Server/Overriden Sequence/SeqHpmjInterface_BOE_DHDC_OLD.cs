using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Sequence;
using Dms.Device;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Windows.Forms;

namespace Dms.Server
{
    public class ThreadHpmjIntefaceControl : XSequence
    {
        #region Fields
        protected static ServerManager m_Server;
        public static ThreadHpmjIntefaceControl Instance;

        private XLog HPMJLog;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            m_Server.AddSeqInitFunction(new SeqInithpmj(this, m_Server));

            RegisterSequence(new SeqPing(this));
            RegisterSequence(new SeqCheckSignal(this)); // 11.01.31 minhan
            RegisterSequence(new SeqCheckPumpRun(this));
            RegisterSequence(new SeqRunTimeCountReset(this));
            RegisterSequence(new SeqPumpChangeCountReset(this));
            RegisterSequence(new SeqFilterChangeCountReset(this));
            RegisterSequence(new SeqMode(this));
            //RegisterSequence(new SeqAlarmReset(this)); // 11.01.31 minhan
            RegisterSequence(new SeqBuzzerOff(this));
            RegisterSequence(new SeqhpmjStateMonitor(this)); // 11.02.01 minhan
            RegisterSequence(new SeqPumpRun(this));
            RegisterSequence(new SeqInterlockParaSet(this));
            RegisterSequence(new SeqhpmjAlarmCode(this));
            RegisterSequence(new SeqHpmjGaugeLogdata(this)); //단순히 hpmj gauge Log 남긴다.

        }
        #endregion

        #region Constructor
        public ThreadHpmjIntefaceControl(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            HPMJLog = new XLog("HPMJ Log", XLog.LogStampType.UseStamp); 
            RegisterSequences();
        }
        #endregion

        #region
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (m_Server.IoController.DeviceState != ActiveState.Run) return;

                foreach (XSeqFunction seq in SeqFunctions)
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

        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("HPMJ Log \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            HPMJLog.TextOut(log);

        }

        #endregion
    }

    //아래부터 Seq 만들면 된당.
    public class SeqInithpmj : XSeqInitFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private InitState m_InitState = InitState.Noop;

        protected static HpmjInterface m_Hpmj;
        private ThreadHpmjIntefaceControl m_Control;
        //초기화 항목
        protected GenericTag m_HpmjReadyStatus = new GenericTag("HPMJ Ready Status", InitCheckState.NotReady);
        protected GenericTag m_HpmjRemoteStatus = new GenericTag("HPMJ Remote Status", InitCheckState.NotReady);
        
        private GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqInithpmj(ThreadHpmjIntefaceControl control, ServerManager server)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
 //           m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ Init";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
        }
        #endregion
        public void InitParameter()
        {

        }
        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;
            int nSeqNo = this.SeqNo;
            int nRv = -1;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfo.EqpInitReq)
                    {
                        bool bUse = false;

                        m_InitState = InitState.Init;
                        m_HpmjReadyStatus.Value = InitCheckState.Checking;
                        if (m_Simul.Device) m_Hpmj.diReady.SetState(true);

                        bUse = m_Server.JobCond.HpmjUse(eqpHpmjs._HPMJ_Unit);

                        if (bUse) // 11.01.27 minhan
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, " Seq HPMJ Init : Use");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else
                        {
                            m_HpmjReadyStatus.Value = InitCheckState.OK;
                            m_HpmjRemoteStatus.Value = InitCheckState.OK;
                            //GlobalVar.HpmjPPIDReq = false; // 11.01.31 minhan
                            m_Hpmj.doRemote_Request.SetState(false);
                            m_Hpmj.doLocal_Request.SetState(false);
                            m_InitState = InitState.Comp;
                            m_Control.SetLog(SeqFunName, 0, 0, " Seq HPMJ Init : No Use");
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 10: // 11.01.27 minhan
                    {
                        bool bReady = !m_Hpmj.diAlarmHeavy.GetState();
                        bReady &= GlobalVar.HpmjReady;
                        bReady &= GlobalVar.HpmjOnline;

                        if (bReady)
                        {
                            m_HpmjReadyStatus.Value = InitCheckState.OK;
                            m_HpmjRemoteStatus.Value = InitCheckState.Checking;
                            //if (m_Simul.Device) m_Hpmj.diRemote.SetState(true);
                            m_Hpmj.doRemote_Request.SetState(true); // 11.10.27 minhan
                            m_Hpmj.doLocal_Request.SetState(false); // 11.01.27 minhan

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diRemote.SetState(true);
                                m_Hpmj.diLocal.SetState(false);
                            }

                            StartTicks = XFunc.GetTickCount(); // 11.01.27 minhan
                            m_Control.SetLog(SeqFunName, 0, 0, " HPMJ Unit Ready OK");
                            nSeqNo = 20;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            AlarmId = m_Hpmj.ALM_ReadyFail.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_HpmjReadyStatus.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_Control.SetLog(SeqFunName, 0, 0, " HPMJ Unit Not Ready Alarm");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20: // 11.01.27 minhan
                    {
                        if (GetElapsedTicks() > 3000)
                        {
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        if (m_Hpmj.diRemote.GetState())
                        {
                            m_HpmjRemoteStatus.Value = InitCheckState.OK;
                            //GlobalVar.HpmjPPIDReq = true; // 11.01.31 minhan
                            m_InitState = InitState.Comp;
                            m_Hpmj.doRemote_Request.SetState(false); // 11.10.27 minhan
                            m_Hpmj.doLocal_Request.SetState(false); // 11.01.27 minhan
                            m_Control.SetLog(SeqFunName, 0, 0, " HPMJ Unit Remote OK");
                            nSeqNo = 0;
                        }
                        else
                        {
                            AlarmId = m_Hpmj.ALM_RemoteFail.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_HpmjRemoteStatus.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_Hpmj.doRemote_Request.SetState(false); // 11.10.27 minhan
                            m_Hpmj.doLocal_Request.SetState(false); // 11.01.27 minhan
                            m_Control.SetLog(SeqFunName, 0, 0, " HPMJ Unit Not Remote Alarm");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if(m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqPing : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;

        private Alarm ALM_HPMJINERFACE_ONLINE;
        private ThreadHpmjIntefaceControl m_Control;
        private GenInfoHandler m_GenInfo;
        private bool nUse;
        #endregion

        #region Constructor
        public SeqPing(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            ALM_HPMJINERFACE_ONLINE = new Alarm("HPMJ INTERFACE" + "ON Line Status Error ", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.01.31 minhan
            this.SeqFunName = "HPMJ Ping";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            nUse = false;
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp) return -1;
            
            nUse = eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>();

            int nSeqNo = this.SeqNo;
            int nRv = -1;

            switch (nSeqNo)
            {
                case 0:
                    if (nUse)
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, " HPMJ Unit Use");
                        nSeqNo = 5;
                    }
                    else // 11.01.27 minhan
                    {
                        GlobalVar.HpmjOnline = false;
                        m_Hpmj.doMelsecNet_OnLine.SetState(false);
                    }
                    break;
                case 5:
                    if(m_Hpmj.diMelsecNet_OnLine.GetState())
                    {
                        m_Hpmj.doMelsecNet_OnLine.SetState(true);
                        if (m_Simul.Device) m_Hpmj.diMelsecNet_OnLine.SetState(false);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        m_Hpmj.doMelsecNet_OnLine.SetState(false);
                        if (m_Simul.Device) m_Hpmj.diMelsecNet_OnLine.SetState(true);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 10:
                    {
                        if (!nUse)
                        {
                            GlobalVar.HpmjOnline = false;
                            m_Hpmj.doMelsecNet_OnLine.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Ping status Cancel(No Use)");
                            nSeqNo = 0;
                            break;
                        }

                        if (!m_Hpmj.diMelsecNet_OnLine.GetState())
                        {
                            m_Hpmj.doMelsecNet_OnLine.SetState(false);
                            GlobalVar.HpmjOnline = true;
                            if (m_Simul.Device) m_Hpmj.diMelsecNet_OnLine.SetState(true);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else if (GetElapsedTicks() > 3000) // 11.01.27 minhan
                        {
                            GlobalVar.HpmjOnline = false;
                            AlarmId = ALM_HPMJINERFACE_ONLINE.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Hpmj.doMelsecNet_OnLine.SetState(false); // 11.01.27 minhan
                            m_Control.SetLog(SeqFunName, 0, 0, "Ping status Alarm(case 10)");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doMelsecNet_OnLine.SetState(true);
                        }
                    }
                    break;
                case 20:
                    {
                        if (!nUse)
                        {
                            GlobalVar.HpmjOnline = false;
                            m_Hpmj.doMelsecNet_OnLine.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Ping status Cancel(No Use)");
                            nSeqNo = 0;
                            break;
                        }

                        if (m_Hpmj.diMelsecNet_OnLine.GetState())
                        {
                            m_Hpmj.doMelsecNet_OnLine.SetState(true);
                            GlobalVar.HpmjOnline = true;
                            if (m_Simul.Device) m_Hpmj.diMelsecNet_OnLine.SetState(false);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            GlobalVar.HpmjOnline = false;
                            AlarmId = ALM_HPMJINERFACE_ONLINE.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Hpmj.doMelsecNet_OnLine.SetState(false); // 11.01.27 minhan
                            m_Control.SetLog(SeqFunName, 0, 0, "Ping status Alarm(case 20)");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doMelsecNet_OnLine.SetState(false);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqCheckSignal : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static HpmjInterface m_Hpmj;
        protected static IEqpManager m_EqpManager; // 11.03.02 minhan
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private Hpmj m_hpmjUnit; // 11.03.02 minhan
        private bool m_Use; // 11.03.02 minhan
        #endregion

        #region Constructor
        public SeqCheckSignal(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            m_hpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.03.02 minhan
            m_Control = control;
            m_Use = false; // 11.03.02 minhan
            this.SeqFunName = "HPMJ Check Signal";
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp) return -1; // 11.01.27 minhan

            m_Use = m_hpmjUnit.SetupHpmjUse.GetValue<bool>();

            int nSeqNo = this.SeqNo;
            int nRv = -1;

            if (m_EqpManager.AlarmResetSwitchPushed) // 11.03.02 minhan
            {
                if (AlarmId > 0)
                {
                    m_EqpManager.ResetAlarm(AlarmId);
                    AlarmId = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                }

            }

            switch (nSeqNo) // 그냥 체크하고 다른 시퀀스에 상황을 보고 알람처리하자.
            {
                case 0:
                    {
                        if (m_Hpmj.diReady.GetState())
                        {
                            if (!GlobalVar.HpmjReady) GlobalVar.HpmjReady = true;

                            if (AlarmId > 0) // 11.03.02 minhan
                            {
                                m_EqpManager.ResetAlarm(AlarmId);
                                AlarmId = 0;
                                m_Control.SetLog(SeqFunName, 0, 0, "Recovery(hpmj Ready On)");
                            }
                        }
                        else
                        {
                            if (GlobalVar.HpmjReady) GlobalVar.HpmjReady = false;

                            if ((AlarmId == 0) && m_Server.GenInfos.EqpInitComp && m_Use) // 11.03.02 minhan
                            {
                                AlarmId = m_hpmjUnit.ALM_NotReadyAlarm.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Not Ready");
                            }
                        }

                        if (m_Hpmj.diRemote.GetState())
                        {
                            if (!GlobalVar.HpmjRemote) GlobalVar.HpmjRemote = true;
                        }
                        else
                        {
                            if (GlobalVar.HpmjRemote) GlobalVar.HpmjRemote = false;
                        }
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqCheckPumpRun : XSeqFunction // 11.01.31 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static HpmjInterface m_Hpmj;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private Alarm ALM_HPMJ_PUMP_RUN_ERROR; // 11.01.27 minhan
        private bool m_CheckCond; 
        #endregion

        #region Constructor
        public SeqCheckPumpRun(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = m_Server.EqpStateManager;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            m_Control = control;
            ALM_HPMJ_PUMP_RUN_ERROR = new Alarm("HPMJ INTERFACE Pump Run Error", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.01.27 minhan
            this.SeqFunName = "HPMJ Check Pump Run";
            m_CheckCond = false;
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp /*|| !GlobalVar.HpmjOnline*/) return -1; // 11.01.27 minhan 구지 이니셜을 볼 필요가 있을까.
            int nSeqNo = this.SeqNo;
            int nRv = -1;

            m_CheckCond = GlobalVar.HpmjOnline;
            m_CheckCond &= GlobalVar.HpmjReady;
            m_CheckCond &= !m_Hpmj.diAlarmHeavy.GetState();
            m_CheckCond &= m_Hpmj.doStart_Pump.GetState();

            switch (nSeqNo)
            {
                case 0: // 11.01.27 minhan 일단 체크만
                    if (m_CheckCond)
                    {
                        GlobalVar.HpmjPumpRunErr = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Run Check Start");
                        if (m_Simul.Device) // 11.02.01 minhan
                        {
                            m_Hpmj.diRun_Pump.SetState(true);
                        }
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        if (GlobalVar.HpmjPumpRunErr) GlobalVar.HpmjPumpRunErr = false;

                        if (m_Simul.Device) // 11.02.01 minhan
                        {
                            m_Hpmj.diRun_Pump.SetState(false);
                        }
                    }
                    break;
                case 10:
                    {
                        if (!m_CheckCond)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Run Check Stop"); 
                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diRun_Pump.GetState())
                        {
                            StartTicks = XFunc.GetTickCount();
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            GlobalVar.HpmjPumpRunErr = true;
                            AlarmId = ALM_HPMJ_PUMP_RUN_ERROR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Run Time Over");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        GlobalVar.HpmjPumpRunErr = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqRunTimeCountReset : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        //private GenInfoHandler m_GenInfo;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private Alarm ALM_HPMJ_RUN_TIME_RESET; // 11.01.27 minhan
        #endregion

        #region Constructor
        public SeqRunTimeCountReset(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control; // 11.01.27 minhan
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Runing Time Reset";

            //m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            ALM_HPMJ_RUN_TIME_RESET = new Alarm("HPMJ INTERFACE Running Time Reset Time Over", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.01.27 minhan
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline ) return -1; // 11.01.27 minhan
            int nSeqNo = this.SeqNo;
            int nRv = -1;
            switch (nSeqNo)
            {
                case 0:
                    {
                        if (GlobalVar.HpmjRunningCountReset)
                        {
                            m_Hpmj.doTotal_Running_Time_Reset.SetState(true);
                            m_Control.SetLog(SeqFunName, 0, 0, "Run Time Count Reset Requset");
                            if (m_Simul.Device) m_Hpmj.diTotal_Running_Time_Reset_Ack.SetState(true);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!GlobalVar.HpmjOnline)
                        {
                            GlobalVar.HpmjRunningCountReset = false;
                            m_Hpmj.doTotal_Running_Time_Reset.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Run Time Count Reset Cancel(Offline)");
                            nSeqNo = 0;
                            break;
                        }

                        if (m_Hpmj.diTotal_Running_Time_Reset_Ack.GetState())
                        {
                            GlobalVar.HpmjRunningCountReset = false;
                            m_Hpmj.doTotal_Running_Time_Reset.SetState(false);
                            if (m_Simul.Device) m_Hpmj.diTotal_Running_Time_Reset_Ack.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Run Time Count Reset OK");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doTotal_Running_Time_Reset.SetState(false);
                            AlarmId = ALM_HPMJ_RUN_TIME_RESET.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "Run Time Count Reset Time Over");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doTotal_Running_Time_Reset.SetState(true);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        GlobalVar.HpmjRunningCountReset = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqPumpChangeCountReset : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        //private GenInfoHandler m_GenInfo;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private Alarm ALM_HPMJ_PUMP_PACKING_RESET; // 11.01.27 minhan
        #endregion

        #region Constructor
        public SeqPumpChangeCountReset(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control; // 11.01.27 minhan
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Packing Reset";

            //m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            ALM_HPMJ_PUMP_PACKING_RESET = new Alarm("HPMJ INTERFACE Pump Packing Reset Time Over", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.01.27 minhan
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline) return -1; // 11.01.31 minhan
            int nSeqNo = this.SeqNo;
            int nRv = -1;
            switch (nSeqNo) // 11.01.31 minhan
            {
                case 0:
                    {
                        if (GlobalVar.HpmjPackingCountReset)
                        {
                            m_Hpmj.doPump_Packing_Change_Count_Reset.SetState(true);
                            m_Control.SetLog(SeqFunName, 0, 0, "Pump Packing Count Reset Requset");
                            if (m_Simul.Device) m_Hpmj.diPump_Packing_Change_Count_Reset_Ack.SetState(true);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!GlobalVar.HpmjOnline)
                        {
                            GlobalVar.HpmjPackingCountReset = false;
                            m_Hpmj.doPump_Packing_Change_Count_Reset.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Pump Packing Count Reset Cancel(Offline)");
                            nSeqNo = 0;
                            break;
                        }

                        if (m_Hpmj.diPump_Packing_Change_Count_Reset_Ack.GetState())
                        {
                            GlobalVar.HpmjPackingCountReset = false;
                            m_Hpmj.doPump_Packing_Change_Count_Reset.SetState(false);
                            if (m_Simul.Device) m_Hpmj.diPump_Packing_Change_Count_Reset_Ack.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Pump Packing Count Reset OK");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doPump_Packing_Change_Count_Reset.SetState(false);
                            AlarmId = ALM_HPMJ_PUMP_PACKING_RESET.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "Pump Packing Count Reset Time Over");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doPump_Packing_Change_Count_Reset.SetState(true);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        GlobalVar.HpmjPackingCountReset = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqFilterChangeCountReset : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;

        protected static HpmjInterface m_Hpmj;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        //private GenInfoHandler m_GenInfo;
        private Alarm ALM_HPMJ_FILTER_RESET; // 11.01.27 minhan
        #endregion

        #region Constructor
        public SeqFilterChangeCountReset(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control; // 11.01.27 minhan
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Filter Reset";

            //m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            ALM_HPMJ_FILTER_RESET = new Alarm("HPMJ INTERFACE Filter Change Count Reset Time Over", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline) return -1;
            int nSeqNo = this.SeqNo;
            int nRv = -1;
            switch (nSeqNo) // 11.01.31 minhan
            {
                case 0:
                    {
                        if (GlobalVar.HpmjFilterCountReset)
                        {
                            m_Hpmj.doFilter_Change_Count_Reset.SetState(true);
                            m_Control.SetLog(SeqFunName, 0, 0, "Filter Count Reset Requset");
                            if (m_Simul.Device) m_Hpmj.diFliter_Change_Count_Reset_Ack.SetState(true);
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!GlobalVar.HpmjOnline)
                        {
                            GlobalVar.HpmjFilterCountReset = false;
                            m_Hpmj.doFilter_Change_Count_Reset.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Filter Count Reset Cancel(Offline)");
                            nSeqNo = 0;
                            break;
                        }

                        if (m_Hpmj.diFliter_Change_Count_Reset_Ack.GetState())
                        {
                            GlobalVar.HpmjFilterCountReset = false;
                            m_Hpmj.doFilter_Change_Count_Reset.SetState(false);
                            if (m_Simul.Device) m_Hpmj.diFliter_Change_Count_Reset_Ack.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "Filter Count Reset OK");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doFilter_Change_Count_Reset.SetState(false);
                            AlarmId = ALM_HPMJ_FILTER_RESET.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "Filter Count Reset Time Over");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doFilter_Change_Count_Reset.SetState(true);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        GlobalVar.HpmjFilterCountReset = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqMode : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;

        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private GenInfoHandler m_GenInfo;
        private bool m_Check; // 11.01.27 minhan
        private Alarm ALM_HPMJINERFACE_REMOTE; // 11.01.27 minhan
        #endregion

        #region Constructor
        public SeqMode(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control; // 11.01.27 minhan
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Mode";

            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_Check = false;
            ALM_HPMJINERFACE_REMOTE = new Alarm("HPMJ INTERFACE" + "Remote Mode Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.01.27 minhan
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp || !m_GenInfo.AutoMode) // 11.01.27 minhan
            {
                if (m_EqpManager.AlarmResetSwitchPushed)
                {
                    if (AlarmId > 0)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Hpmj.doRemote_Request.SetState(false);
                        m_Hpmj.doLocal_Request.SetState(false);
                        this.SeqNo = 0;
                    }
                }
                return -1; // 11.01.27 minhan
            }

            int nSeqNo = this.SeqNo;
            int nRv = -1;

            m_Check = eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>();
            m_Check &= GlobalVar.HpmjOnline;
            m_Check &= m_GenInfo.AutoMode;
            // 두가지 상황에서 리모트
            switch (nSeqNo) 
            {
                case 0:
                    {
                        if (AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(AlarmId);
                            AlarmId = 0;
                        }

                        if (m_Check && !m_Hpmj.diRemote.GetState()) // 11.01.31 minhan
                        {
                            m_Hpmj.doRemote_Request.SetState(true);
                            m_Hpmj.doLocal_Request.SetState(false);
                            if (m_Simul.Device)
                            {
                                m_Hpmj.diRemote.SetState(true);
                                m_Hpmj.diLocal.SetState(false);
                            }
                            m_Control.SetLog(SeqFunName, 0, 0, "Remote Requset On");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else
                        {
                            m_Hpmj.doRemote_Request.SetState(false);
                            m_Hpmj.doLocal_Request.SetState(false);
                        }
                    }
                    break;
                case 10:
                    {
                        if (!m_Check)
                        {
                            m_Hpmj.doRemote_Request.SetState(false);
                            m_Hpmj.doLocal_Request.SetState(false);
                            if (GlobalVar.HpmjOnline)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "Remote Requset Cancel");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "Remote Requset Cancel(Offline)");
                            }
                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diRemote.GetState())
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Remote On");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doRemote_Request.SetState(false);
                            m_Hpmj.doLocal_Request.SetState(false);
                            AlarmId = ALM_HPMJINERFACE_REMOTE.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "Remote Requset Alarm");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doRemote_Request.SetState(true);
                            m_Hpmj.doLocal_Request.SetState(false);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;

            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
 
    public class SeqBuzzerOff : XSeqFunction // 11.01.31 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        private GenInfoHandler m_GenInfo;
        private ThreadHpmjIntefaceControl m_Control;
        private Alarm ALM_HPNJ_BUZZER_ERR; // 11.01.31 minhan
        private Buzzer m_Buzzer; // 11.01.31 minhan
        private bool m_checkCond; // 11.01.31 minhan
        #endregion

        #region Constructor
        public SeqBuzzerOff(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Buzzer";
            ALM_HPNJ_BUZZER_ERR = new Alarm("HPMJ INTERFACE Buzzer Off Alarm ", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.01.31 minhan
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_Buzzer = eqpBuzzers._SW_Buzzer;
            m_checkCond = false;
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline) return -1; // 11.01.31 minhan
            int nSeqNo = this.SeqNo;
            int nRv = -1;

            m_checkCond = m_Buzzer.IsPushed();
            m_checkCond |= GlobalVar.BuzzerOff;
            m_checkCond &= (m_Hpmj.diAlarmHeavy.GetState() || m_Hpmj.diAlarmLight.GetState()) ? true : false; // 이거는 아니라면 제거
            m_checkCond &= GlobalVar.HpmjOnline;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_checkCond || GlobalVar.HpmjBuzzerOff) // 11.01.31 minhan
                        {
                            m_Hpmj.doBuzzer_Stop_Request.SetState(true);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Buzzer Off Request");

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diBuzzer_Stop_Request_Ack.SetState(true);
                            }
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else
                        {
                            if (m_Hpmj.doBuzzer_Stop_Request.GetState()) m_Hpmj.doBuzzer_Stop_Request.SetState(false);

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diBuzzer_Stop_Request_Ack.SetState(false);
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        if (!GlobalVar.HpmjOnline)
                        {
                            if(GlobalVar.HpmjBuzzerOff) GlobalVar.HpmjBuzzerOff = false;
                            m_Hpmj.doBuzzer_Stop_Request.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Buzzer Off Cancel(offline)");
                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diBuzzer_Stop_Request_Ack.GetState())
                        {
                            if (GlobalVar.HpmjBuzzerOff) GlobalVar.HpmjBuzzerOff = false;
                            m_Hpmj.doBuzzer_Stop_Request.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Buzzer Off OK");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doBuzzer_Stop_Request.SetState(false);
                            AlarmId = ALM_HPNJ_BUZZER_ERR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doBuzzer_Stop_Request.SetState(true);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        if (GlobalVar.HpmjBuzzerOff) GlobalVar.HpmjBuzzerOff = false;

                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqPumpRun : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private GenInfoHandler m_GenInfo;
        private Hpmj m_HpmjUnit; // 11.01.31 minhan
        private XTimer m_Timer1; // 11.01.31 minhan
        private XTimer m_Timer2; // 11.01.31 minhan
        private Gauge m_DifferPre; // 11.01.31 minhan
        private int PrevPressure; // 11.01.31 minhan
        private Alarm ALM_PARAMETER_ERR; // 11.01.31 minhan
        private Alarm ALM_CHANGE_ERR; // 11.01.31 minhan
        private Alarm ALM_PPID_ERR; // 11.01.31 minhan
        private Alarm ALM_PARAMETER_COMPARE; // 11.02.07 minhan
        private Alarm ALM_PPID_COMPARE; // 11.02.07 minhan
        #endregion

        #region Constructor
        public SeqPumpRun(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
 //           m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            m_Control = control;
            this.SeqFunName = "HPMJ INTERFACE Pump Run";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_HpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.01.31 minhan
            m_Timer1 = new XTimer("PumpRun1"); // 11.01.31 minhan
            m_Timer2 = new XTimer("PumpRun2"); // 11.01.31 minhan
            ALM_PARAMETER_ERR = new Alarm("HPMJ INTERFACE Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_CHANGE_ERR = new Alarm("HPMJ INTERFACE Parameter Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_PPID_ERR = new Alarm("HPMJ INTERFACE PPID Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_PARAMETER_COMPARE = new Alarm("HPMJ INTERFACE Parameter Compare Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_PPID_COMPARE = new Alarm("HPMJ INTERFACE PPID Compare Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_DifferPre = eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge;
            PrevPressure = 0;
        }
        #endregion
        #region Methode
        public bool SetPara() // 11.01.31 minhan
        {
            try
            {
                int StandardPress = m_HpmjUnit.SetupHpmjStandardPressure.GetValue<int>();
                double StandardFlow = m_HpmjUnit.SetupHpmjStandardFlowrate.GetValue<double>();
                double StandardArea = StandardFlow / Math.Pow(StandardPress, 0.5);
                double FlowbyPress = 0;
                ushort fSetValue = 0;
                ushort AlarmMargin = 0;
                ushort WarningMargin = 0;
                ushort fLowLimit = 0;
                ushort fLowWarning = 0;
                ushort fUpWarning = 0;
                ushort fUpAlarm = 0;

                if (m_GenInfo.AutoMode)
                {
                    if (m_Server.GenInfos.EQPGlassCount > 0)
                    {
                        FlowbyPress = (StandardArea * Math.Pow((double)m_Server.JobCond.HpmjPressure(m_HpmjUnit), 0.5));

                        if (FlowbyPress <= 0)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Flow Set value Error");
                            return false;
                        }
                    }
                    else if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                    {
                        FlowbyPress = (StandardArea * Math.Pow(m_Server.SetupIdleHpmjPress.GetValue<double>(), 0.5));

                        if (FlowbyPress <= 0)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Flow Set value Error");
                            return false;
                        }
                    }
                    else FlowbyPress = 0.0;
                }
                else
                {
                    if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                    {
                        FlowbyPress = (StandardArea * Math.Pow(m_HpmjUnit.SetupHpmjPressure.GetValue<double>(), 0.5));

                        if (FlowbyPress <= 0)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Flow Set value Error");
                            return false;
                        }
                    }
                    else
                    {
                        FlowbyPress = 0.0;
                    }
                }

                if (FlowbyPress > 0)
                {
                    if (((FlowbyPress - m_Server.SetupFlowWarningMargin.GetValue<double>()) <= 0) ||
                       ((FlowbyPress - m_Server.SetupFlowAlarmMargin.GetValue<double>()) <= 0))
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "Flow Set value Error");
                        return false;
                    }

                    m_GenInfo.HpmjTargetFlow = string.Format("{0}", FlowbyPress); // 11.02.01 minhan
                    fSetValue = Convert.ToUInt16(FlowbyPress * 100);
                    AlarmMargin = Convert.ToUInt16(m_Server.SetupFlowAlarmMargin.GetValue<double>() * 100);
                    WarningMargin = Convert.ToUInt16(m_Server.SetupFlowWarningMargin.GetValue<double>() * 100);
                }
                else
                {
                    m_GenInfo.HpmjTargetFlow = "0"; // 11.02.01 minhan
                    fSetValue = 0;
                    AlarmMargin = 0;
                    WarningMargin = 0;
                }

                // flow Set
                m_Hpmj.mowShower_Flow_Set.SetState(fSetValue);
                m_Hpmj.mowShower_Flow_Lower_Error.SetState(AlarmMargin);
                m_Hpmj.mowShower_Flow_Lower_Warning.SetState(WarningMargin);
                m_Hpmj.mowShower_Flow_Upper_Warning.SetState(WarningMargin);
                m_Hpmj.mowShower_Flow_Upper_Error.SetState(AlarmMargin);

                fSetValue = 0;
                AlarmMargin = 0;
                WarningMargin = 0;

                int m_Buf = 0;

                if (m_GenInfo.AutoMode)
                {
                    if (m_Server.GenInfos.EQPGlassCount > 0)
                    {
                        //PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                        m_Buf = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                        if ((m_Buf < 80) || (m_Buf > 130)) // 11.04.20 minhan
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Pressure Set value Error");
                            return false;
                        }

                        fSetValue = Convert.ToUInt16(m_Server.JobCond.HpmjPressure(m_HpmjUnit) * 10);
                    }
                    else if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                    {
                        //PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();
                        m_Buf = m_Server.SetupIdleHpmjPress.GetValue<int>();
                        if ((m_Buf < 80) || (m_Buf > 130)) // 11.04.20 minhan
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Pressure Set value Error");
                            return false;
                        }

                        fSetValue = Convert.ToUInt16(m_Server.SetupIdleHpmjPress.GetValue<int>() * 10);
                    }
                    else
                    {
                        //PrevPressure = 0;
                        fSetValue = 0;
                    }

                }
                else
                {
                    if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                    {
                        //PrevPressure = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();
                        m_Buf = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();
                        if ((m_Buf < 80) || (m_Buf > 130)) // 11.04.20 minhan
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Pressure Set value Error");
                            return false;
                        }

                        fSetValue = Convert.ToUInt16(m_HpmjUnit.SetupHpmjPressure.GetValue<int>() * 10);
                    }
                    else
                    {
                        //PrevPressure = 0;
                        fSetValue = 0;
                    }


                }

                //m_HpmjUnit.IfFlag.ReferencePressure = PrevPressure;

                if (fSetValue > 0)
                {
                    AlarmMargin = Convert.ToUInt16(m_Server.SetupPressAlarmMargin.GetValue<double>() * 10);
                    WarningMargin = Convert.ToUInt16(m_Server.SetupPressWarningMargin.GetValue<double>() * 10);
                }
                else
                {
                    fSetValue = 0;
                    AlarmMargin = 0;
                    WarningMargin = 0;
                }

                // Filter In
                m_Hpmj.mowFilter_In_Press_Set.SetState(fSetValue);
                m_Hpmj.mowFilter_In_Press_Lower_Error.SetState(AlarmMargin);
                m_Hpmj.mowFilter_In_Press_Lower_Warning.SetState(WarningMargin);
                m_Hpmj.mowFilter_In_Press_Upper_Warning.SetState(WarningMargin);
                m_Hpmj.mowFilter_In_Press_Upper_Error.SetState(AlarmMargin);

                // Filter Out
                m_Hpmj.mowFilter_Out_Press_Set.SetState(fSetValue);
                m_Hpmj.mowFilter_Out_Press_Lower_Error.SetState(AlarmMargin);
                m_Hpmj.mowFilter_Out_Press_Lower_Warning.SetState(WarningMargin);
                m_Hpmj.mowFilter_Out_Press_Upper_Warning.SetState(WarningMargin);
                m_Hpmj.mowFilter_Out_Press_Upper_Error.SetState(AlarmMargin);

                fSetValue = 0;
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                // 11.02.01 minhan 일단 사용하든 하지않든 무조건 날리고, 사용해제 부분은 협의가 필요하다.
                if ((m_HpmjUnit.MainDiPress.SetupInterlock != null) &&
                    //m_HpmjUnit.MainDiPress.SetupInterlock.Use && 
                   (m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue > 0))
                {
                    if (//(m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue < 0.5) ||
                       ((m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue - m_HpmjUnit.MainDiPress.SetupInterlock.LowWarning) < 0) ||
                       ((m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue - m_HpmjUnit.MainDiPress.SetupInterlock.LowAlarm) < 0)) // 11.02.09 minhan
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "Main DI Pressure Set value Error");
                        return false;
                    }

                    fSetValue = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue * 1000);
                    fLowLimit = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.LowAlarm * 1000);
                    fLowWarning = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.LowWarning * 1000);
                    fUpWarning = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.HighWarning * 1000);
                    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.HighAlarm * 1000);
                }
                else
                {
                    //if ((m_HpmjUnit.MainDiPress.SetupInterlock == null) ||
                    //     m_HpmjUnit.MainDiPress.SetupInterlock.Use ||
                    //   (m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                    //{
                    fSetValue = 0;
                    fLowLimit = 0;
                    fLowWarning = 0;
                    fUpWarning = 0;
                    fUpAlarm = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "Main DI Pressure Set value Error");
                    return false;
                    //}
                    //else
                    //{
                    //    fSetValue = 0;
                    //    fLowLimit = 0;
                    //    fLowWarning = 0;
                    //    fUpWarning = 0;
                    //    fUpAlarm = 0;
                    //    m_Control.SetLog(SeqFunName, 0, 0, "Main DI Pressure Set No use");
                    //}
                }

                // Main DI
                m_Hpmj.mowDI_Press_Set.SetState(fSetValue);
                m_Hpmj.mowDI_Press_Lower_Error.SetState(fLowLimit);
                m_Hpmj.mowDI_Press_Lower_Warning.SetState(fLowWarning);
                m_Hpmj.mowDI_Press_Upper_Warning.SetState(fUpWarning);
                m_Hpmj.mowDI_Press_Upper_Error.SetState(fUpAlarm);

                fSetValue = 0;
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                if ((m_HpmjUnit.Resistivity.SetupInterlock != null) &&
                    //m_HpmjUnit.Resistivity.SetupInterlock.Use && 
                    (m_HpmjUnit.Resistivity.SetupInterlock.SettingValue > 0))
                {
                    if (((m_HpmjUnit.Resistivity.SetupInterlock.SettingValue - m_HpmjUnit.Resistivity.SetupInterlock.LowWarning) < 0) ||
                       ((m_HpmjUnit.Resistivity.SetupInterlock.SettingValue - m_HpmjUnit.Resistivity.SetupInterlock.LowAlarm) < 0))
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set value Error");
                        return false;
                    }

                    fSetValue = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.SettingValue * 100);
                    fLowLimit = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.LowAlarm * 100);
                    fLowWarning = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.LowWarning * 100);
                    fUpWarning = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.HighWarning * 100);
                    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.HighAlarm * 100);
                }
                else
                {
                    //if ((m_HpmjUnit.Resistivity.SetupInterlock == null) ||
                    //     m_HpmjUnit.Resistivity.SetupInterlock.Use ||
                    //   (m_HpmjUnit.Resistivity.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                    //{
                    fSetValue = 0;
                    fLowLimit = 0;
                    fLowWarning = 0;
                    fUpWarning = 0;
                    fUpAlarm = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set value Error");
                    return false;
                    //}
                    //else
                    //{
                    //    fSetValue = 0;
                    //    fLowLimit = 0;
                    //    fLowWarning = 0;
                    //    fUpWarning = 0;
                    //    fUpAlarm = 0;
                    //    m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set No use");
                    //}
                }

                // DI Resistance
                m_Hpmj.mowResistivity_Set.SetState(fSetValue);
                m_Hpmj.mowResistivity_Lower_Error.SetState(fLowLimit);
                m_Hpmj.mowResistivity_Lower_Warning.SetState(fLowWarning);
                m_Hpmj.mowResistivity_Upper_Warning.SetState(fUpWarning);
                m_Hpmj.mowResistivity_Upper_Error.SetState(fUpAlarm);

                fSetValue = 0;
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                if ((m_DifferPre.SetupInterlock != null) &&
                    //m_DifferPre.SetupInterlock.Use && 
                    (m_DifferPre.SetupInterlock.SettingValue >= 0))
                {
                    fSetValue = Convert.ToUInt16(m_DifferPre.SetupInterlock.SettingValue * 10);
                    fUpWarning = Convert.ToUInt16(m_DifferPre.SetupInterlock.HighWarning * 10);
                    fUpAlarm = Convert.ToUInt16(m_DifferPre.SetupInterlock.HighAlarm * 10);
                }
                else
                {
                    //if ((m_DifferPre.SetupInterlock == null) ||
                    //     m_DifferPre.SetupInterlock.Use ||
                    //   (m_DifferPre.SetupInterlock.SettingValue < 0)) // 11.02.07 minhan
                    //{
                    fSetValue = 0;
                    fUpAlarm = 0;
                    fUpWarning = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "Differ Pressure Set value Error");
                    return false;
                    //}
                    //else
                    //{

                    //    fSetValue = 0;
                    //    fUpAlarm = 0;
                    //    fUpWarning = 0;
                    //    m_Control.SetLog(SeqFunName, 0, 0, "Differ Pressure Set No use");
                    //}
                }

                // Difference Press
                m_Hpmj.mowFilter_Difference_Press_Set.SetState(fSetValue);
                m_Hpmj.mowFilter_Difference_Press_Upper_Error.SetState(fUpAlarm);
                m_Hpmj.mowFilter_Difference_Press_Upper_Warning.SetState(fUpWarning);

                fSetValue = 0;
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                if ((m_HpmjUnit.InvertCurrent.SetupInterlock != null) &&
                    //m_HpmjUnit.InvertCurrent.SetupInterlock.Use && 
                    (m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue > 0))
                {
                    fSetValue = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue * 10);
                    fUpWarning = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.HighWarning * 10);
                    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.HighAlarm * 10);
                }
                else
                {
                    //if ((m_HpmjUnit.InvertCurrent.SetupInterlock == null) ||
                    //     m_HpmjUnit.InvertCurrent.SetupInterlock.Use ||
                    //   (m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                    //{
                    fSetValue = 0;
                    fUpWarning = 0;
                    fUpAlarm = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "InvertCurrent Set value Error");
                    return false;
                    //}
                    //else
                    //{
                    //    fSetValue = 0;
                    //    fUpWarning = 0;
                    //    fUpAlarm = 0;
                    //    m_Control.SetLog(SeqFunName, 0, 0, "InvertCurrent Set No use");
                    //}
                }

                // Inverter Current
                m_Hpmj.mowInverter_Load_Current_Set.SetState(fSetValue);
                m_Hpmj.mowInveter_Load_Current_Upper_Error.SetState(fUpAlarm);
                m_Hpmj.mowInveter_Load_Current_Upper_Warning.SetState(fUpWarning);

                fSetValue = 0;
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                if ((m_HpmjUnit.CO2InPress.SetupInterlock != null) &&
                    //m_HpmjUnit.CO2InPress.SetupInterlock.Use && 
                    (m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue > 0))
                {
                    if (//(m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue < 0.5) ||
                      ((m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue - m_HpmjUnit.CO2InPress.SetupInterlock.LowWarning) < 0) ||
                      ((m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue - m_HpmjUnit.CO2InPress.SetupInterlock.LowAlarm) < 0)) // 11.02.09 minhan
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set value Error");
                        return false;
                    }

                    fSetValue = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue * 1000);
                    fLowWarning = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.LowWarning * 1000);
                    fLowLimit = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.LowAlarm * 1000);
                    fUpWarning = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.HighWarning * 1000); // 11.04.20 minhan
                    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.HighAlarm * 1000);
                }
                else
                {
                    //if ((m_HpmjUnit.CO2InPress.SetupInterlock == null) ||
                    //     m_HpmjUnit.CO2InPress.SetupInterlock.Use ||
                    //   (m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                    //{
                    fSetValue = 0;
                    fLowWarning = 0;
                    fLowLimit = 0;
                    fUpWarning = 0; // 11.04.20 minhan
                    fUpAlarm = 0;
                    m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set value Error");
                    return false;
                    //}
                    //else
                    //{
                    //    fSetValue = 0;
                    //    fLowWarning = 0;
                    //    fLowLimit = 0;
                    //    m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set No use");
                    //}
                }

                // Co2 Main
                m_Hpmj.mowMain_CO2_Press_Set.SetState(fSetValue);
                m_Hpmj.mowMain_CO2_Press_Lower_Error.SetState(fLowLimit);
                m_Hpmj.mowMain_CO2_Press_Lower_Warning.SetState(fLowWarning);
                m_Hpmj.mowMain_CO2_Press_Upper_Warning.SetState(fUpWarning); // 11.04.20 minhan
                m_Hpmj.mowMain_CO2_Press_Upper_Error.SetState(fUpAlarm);

                fSetValue = 0; // 11.05.03 minhan
                fLowLimit = 0;
                fLowWarning = 0;
                fUpWarning = 0;
                fUpAlarm = 0;

                //if ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock != null) &&
                //    //m_HpmjUnit.CO2Flow.SetupInterlock.Use && 
                //   (m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue > 0)) // 11.05.03 minhan
                //{
                //    if (//(m_HpmjUnit.CO2Flow.SetupInterlock.SettingValue < 0.5) ||
                //      ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue - m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowWarning) < 0) ||
                //      ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue - m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowAlarm) < 0))
                //    {
                //        m_Control.SetLog(SeqFunName, 0, 0, "CO2Flow Set value Error");
                //        return false;
                //    }

                //    fSetValue = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue * 10); // 11.05.26 minhan
                //    fLowWarning = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowWarning * 10);
                //    fLowLimit = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowAlarm * 10);
                //    fUpWarning = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.HighWarning * 10);
                //    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.HighAlarm * 10);
                //}
                //else
                //{
                //    fSetValue = 0;
                //    fLowWarning = 0;
                //    fLowLimit = 0;
                //    fUpWarning = 0;
                //    fUpAlarm = 0;
                //    m_Control.SetLog(SeqFunName, 0, 0, "CO2Flow Set value Error");
                //    return false;
                //}

                //// Co2 Flow Main
                ////m_Hpmj.mowMain_CO2_Flow_Set.SetState(fSetValue);
                ////m_Hpmj.mowMain_CO2_Flow_Lower_Error.SetState(fLowLimit);
                ////m_Hpmj.mowMain_CO2_Flow_Lower_Warning.SetState(fLowWarning);
                ////m_Hpmj.mowMain_CO2_Flow_Upper_Warning.SetState(fUpWarning);
                ////m_Hpmj.mowMain_CO2_Flow_Upper_Error.SetState(fUpAlarm);
                return true;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                return false;
            }
        }

        private double HpmjControl() // 11.01.31 minhan
        {
            double Error = 0.0;
           
            if (!m_Simul.Device)
            {
                m_HpmjUnit.IfFlag.CurrentPressure = (double)m_HpmjUnit.FilterOutPress.CurAdc / 10;
            }
            else
            {
                m_HpmjUnit.IfFlag.CurrentPressure = (double)m_HpmjUnit.IfFlag.ReferencePressure;
            }

            m_HpmjUnit.IfFlag.ErrorPressure = (double)m_HpmjUnit.IfFlag.ReferencePressure - m_HpmjUnit.IfFlag.CurrentPressure;
            Error = Math.Abs(m_HpmjUnit.IfFlag.ErrorPressure);

            if (Error <= 1.5) // 11.05.05 minhan
            {
                m_HpmjUnit.IfFlag.ErrorPressure = 0.0;
                m_HpmjUnit.IfFlag.Ready = true;

                if (m_HpmjUnit.IfFlag.ReferencePressure != PrevPressure)
                {
                    m_HpmjUnit.IfFlag.ReferencePressure = PrevPressure;
                    string msg = string.Format("Reference Pressure is changed to {0:0.00}", m_HpmjUnit.IfFlag.ReferencePressure);
                    m_Control.SetLog(SeqFunName, 0, 0, msg);
                }
            }

            return Error;
        }
        private bool ComparePara() // 11.02.07 minhan
        {
            try
            {
                if (m_Simul.Device)
                {
                    short value = 0;
                    value = (short)m_Hpmj.mowShower_Flow_Set.GetState();
                    m_Hpmj.miwShower_Flow_Set.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Lower_Error.GetState();
                    m_Hpmj.miwShower_Flow_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Lower_Warning.GetState();
                    m_Hpmj.miwShower_Flow_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Upper_Error.GetState();
                    m_Hpmj.miwShower_Flow_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Upper_Warning.GetState();
                    m_Hpmj.miwShower_Flow_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Set.GetState();
                    m_Hpmj.miwFilter_In_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Lower_Error.GetState();
                    m_Hpmj.miwFilter_In_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Lower_Warning.GetState();
                    m_Hpmj.miwFilter_In_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_In_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_In_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Set.GetState();
                    m_Hpmj.miwFilter_Out_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Lower_Error.GetState();
                    m_Hpmj.miwFilter_Out_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Lower_Warning.GetState();
                    m_Hpmj.miwFilter_Out_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_Out_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_Out_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Set.GetState();
                    m_Hpmj.miwDI_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Lower_Error.GetState();
                    m_Hpmj.miwDI_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Lower_Warning.GetState();
                    m_Hpmj.miwDI_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Upper_Warning.GetState();
                    m_Hpmj.miwDI_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Upper_Error.GetState();
                    m_Hpmj.miwDI_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Set.GetState();
                    m_Hpmj.miwResistivity_Set.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Lower_Error.GetState();
                    m_Hpmj.miwResistivity_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Lower_Warning.GetState();
                    m_Hpmj.miwResistivity_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Upper_Warning.GetState();
                    m_Hpmj.miwResistivity_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Upper_Error.GetState();
                    m_Hpmj.miwResistivity_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Set.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowInverter_Load_Current_Set.GetState();
                    m_Hpmj.miwInverter_Load_Current_Set.SetState(value);

                    value = (short)m_Hpmj.mowInveter_Load_Current_Upper_Error.GetState();
                    m_Hpmj.miwInveter_Load_Current_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowInveter_Load_Current_Upper_Warning.GetState();
                    m_Hpmj.miwInveter_Load_Current_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Set.GetState();
                    m_Hpmj.miwMain_CO2_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Lower_Error.GetState();
                    m_Hpmj.miwMain_CO2_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Lower_Warning.GetState();
                    m_Hpmj.miwMain_CO2_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Upper_Warning.GetState(); // 11.04.20 minhan
                    m_Hpmj.miwMain_CO2_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Upper_Error.GetState();
                    m_Hpmj.miwMain_CO2_Press_Upper_Error.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Set.GetState(); // 11.05.03 minhan
                    //m_Hpmj.miwMain_CO2_Flow_Set.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Lower_Error.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Lower_Error.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Lower_Warning.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Lower_Warning.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Upper_Warning.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Upper_Warning.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Upper_Error.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Upper_Error.SetState(value);
                }
        
                if (m_Hpmj.mowShower_Flow_Set.GetState() != (ushort)m_Hpmj.miwShower_Flow_Set.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Lower_Error.GetState() != (ushort)m_Hpmj.miwShower_Flow_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Lower_Warning.GetState() != (ushort)m_Hpmj.miwShower_Flow_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Upper_Error.GetState() != (ushort)m_Hpmj.miwShower_Flow_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Upper_Warning.GetState() != (ushort)m_Hpmj.miwShower_Flow_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowFilter_In_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowFilter_Out_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowDI_Press_Set.GetState() != (ushort)m_Hpmj.miwDI_Press_Set.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwDI_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwDI_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwDI_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwDI_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowResistivity_Set.GetState() != (ushort)m_Hpmj.miwResistivity_Set.GetState()) return false;
                if (m_Hpmj.mowResistivity_Lower_Error.GetState() != (ushort)m_Hpmj.miwResistivity_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowResistivity_Lower_Warning.GetState() != (ushort)m_Hpmj.miwResistivity_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowResistivity_Upper_Warning.GetState() != (ushort)m_Hpmj.miwResistivity_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowResistivity_Upper_Error.GetState() != (ushort)m_Hpmj.miwResistivity_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowFilter_Difference_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_Difference_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_Difference_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowInverter_Load_Current_Set.GetState() != (ushort)m_Hpmj.miwInverter_Load_Current_Set.GetState()) return false;
                if (m_Hpmj.mowInveter_Load_Current_Upper_Error.GetState() != (ushort)m_Hpmj.miwInveter_Load_Current_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowInveter_Load_Current_Upper_Warning.GetState() != (ushort)m_Hpmj.miwInveter_Load_Current_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowMain_CO2_Press_Set.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Set.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Upper_Error.GetState()) return false; // 11.04.20 minhan
                if (m_Hpmj.mowMain_CO2_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Upper_Warning.GetState()) return false;

                //if (m_Hpmj.mowMain_CO2_Flow_Set.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Set.GetState()) return false; // 11.05.03 minhan
                //if (m_Hpmj.mowMain_CO2_Flow_Lower_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Lower_Error.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Lower_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Lower_Warning.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Upper_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Upper_Error.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Upper_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Upper_Warning.GetState()) return false;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                return false;
            }

            return true;
        }
        private bool PPIDCompare() // 11.02.07 minhan
        {
            try
            {
                if (m_Simul.Device)
                {
                    short value = 0;
                    value = (short)m_Hpmj.mowRunning_Mode.GetState();
                    m_Hpmj.miwRunning_Mode.SetState(value);

                    value = (short)m_Hpmj.mowPressure_Set.GetState();
                    m_Hpmj.miwPressure_Set.SetState(value);

                    value = (short)m_Hpmj.mowFrequency_Set.GetState();
                    m_Hpmj.miwFrequency_Set.SetState(value);
                }

                if (m_Hpmj.mowRunning_Mode.GetState() != (ushort)m_Hpmj.miwRunning_Mode.GetState()) return false;
                if (m_Hpmj.mowPressure_Set.GetState() != (ushort)m_Hpmj.miwPressure_Set.GetState()) return false;
                if (m_Hpmj.mowFrequency_Set.GetState() != (ushort)m_Hpmj.miwFrequency_Set.GetState()) return false;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                return false;
            }

            return true;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline) return -1;
            int nSeqNo = this.SeqNo;
            int nRv = -1;
            switch (nSeqNo)
            {
                case 0:
                    if (m_HpmjUnit.IfFlag.PumpRun)
                    {
                        PrevPressure = 0;
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Start.");
                        nSeqNo = 5;
                    }
                    else
                    {
                        if (m_Hpmj.doParameter_Change_Request.GetState()) m_Hpmj.doParameter_Change_Request.SetState(false);
                        if (m_Hpmj.doStart_Pump.GetState()) m_Hpmj.doStart_Pump.SetState(false);
                        if (m_Hpmj.doPPID_Change_Request.GetState()) m_Hpmj.doPPID_Change_Request.SetState(false);
                        if (m_HpmjUnit.IfFlag.Ready) m_HpmjUnit.IfFlag.Ready = false;
                        if (m_HpmjUnit.IfFlag.PPIDChangeRequest) m_HpmjUnit.IfFlag.PPIDChangeRequest = false;

                        if (m_Simul.Device) // 11.02.01 minhan
                        {
                            m_Hpmj.diPPID_Change_Request_Ack.SetState(false);
                            m_HpmjUnit.HpmjInverterStop();
                        }
                    }
                    break;
                case 5: // 래시피 변경이 생길 경우 여기저기서 살려줘서 잘못하면 공정 불량이 생기므로 펌프를 재 가동 할 경우 다시 설정한다.
                    {
                        ushort nVal = 0;

                        if (m_GenInfo.AutoMode)
                        {

                            if (m_Server.GenInfos.EQPGlassCount > 0)
                            {
                                PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);

                                if((PrevPressure < 80) || (PrevPressure > 130)) // 11.04.20 minhan
                                {
                                    AlarmId = ALM_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pressure Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                nVal = Convert.ToUInt16(m_Server.JobCond.HpmjPressure(m_HpmjUnit) * 10);
                            }
                            else if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                            {
                                PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();

                                if ((PrevPressure < 80) || (PrevPressure > 130)) // 11.04.20 minhan
                                {
                                    AlarmId = ALM_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pressure Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                nVal = Convert.ToUInt16(m_Server.SetupIdleHpmjPress.GetValue<int>() * 10);
                            }
                            else
                            {
                                PrevPressure = 0;
                                nVal = 0;
                            }

                            m_HpmjUnit.IfFlag.PressureSet = PrevPressure; // 11.02.01 minhan
                            m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;
                            m_Hpmj.mowRunning_Mode.SetState(1);
                            m_Hpmj.mowPressure_Set.SetState(nVal);
                            m_Hpmj.mowFrequency_Set.SetState(0); // 11.02.07 minhan
                            m_Hpmj.doPPID_Change_Request.SetState(true);

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diPPID_Change_Request_Ack.SetState(true);
                            }

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Request(Auto)");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 7;
                        }
                        else
                        {
                            if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                            {
                                m_HpmjUnit.IfFlag.PressureSet = m_HpmjUnit.SetupHpmjPressure.GetValue<int>(); // 11.02.01 minhan

                                if ((m_HpmjUnit.IfFlag.PressureSet < 80) || (m_HpmjUnit.IfFlag.PressureSet > 130)) // 11.04.20 minhan
                                {
                                    AlarmId = ALM_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pressure Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                m_HpmjUnit.IfFlag.ReferencePressure = m_HpmjUnit.IfFlag.PressureSet + 1;
                                m_Hpmj.mowRunning_Mode.SetState(1);
                                nVal = Convert.ToUInt16(m_HpmjUnit.SetupHpmjPressure.GetValue<int>() * 10);
                                m_Hpmj.mowPressure_Set.SetState(nVal);
                                m_Hpmj.mowFrequency_Set.SetState(0); // 11.02.07 minhan
                                m_Hpmj.doPPID_Change_Request.SetState(true);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Request(Pressure Manual)");
                            }
                            else
                            {
                                if (m_HpmjUnit.SetupHpmjFrequency.GetValue<int>() > 60) // 11.02.01 minhan
                                {
                                    AlarmId = ALM_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Frequency Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }
                               
                                m_Hpmj.mowRunning_Mode.SetState(2);
                                nVal = Convert.ToUInt16(m_HpmjUnit.SetupHpmjFrequency.GetValue<int>() * 10); // 11.02.09 minhan
                                m_Hpmj.mowFrequency_Set.SetState(nVal);
                                m_Hpmj.mowPressure_Set.SetState(0); // 11.02.07 minhan
                                m_Hpmj.doPPID_Change_Request.SetState(true);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Request(Frequency Manual)");
                            }

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diPPID_Change_Request_Ack.SetState(true);
                            }

                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 7;
                        }
                    }
                    break;
                case 7:
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            m_Hpmj.doPPID_Change_Request.SetState(false);

                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Cancel(PumpRun False)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Cancel(HPMJ Offline)");
                            }
                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diPPID_Change_Request_Ack.GetState())
                        {
                            m_Hpmj.doPPID_Change_Request.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change OK");
                            StartTicks = XFunc.GetTickCount(); // 11.02.07 minhan
                            nSeqNo = 9; // 11.02.07 minhan
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doPPID_Change_Request.SetState(false);
                            AlarmId = ALM_PPID_ERR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Request Error");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doPPID_Change_Request.SetState(true);
                        }
                    }
                    break;
                case 9: // 11.02.07 minhan
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            m_Hpmj.doPPID_Change_Request.SetState(false);
                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Cancel(PumpRun False case 9)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change Cancel(HPMJ Offline case 9)");
                            }
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 300)
                        {
                            bool checkPPIDCompare = PPIDCompare();
                            string m_Msg = "";

                            m_Msg = string.Format(" : {0}", m_Hpmj.mowRunning_Mode.GetState());
                            m_Control.SetLog("mowRunning_Mode", 0, 0, m_Msg);
                            m_Msg = string.Format(" : {0}", m_Hpmj.mowPressure_Set.GetState());
                            m_Control.SetLog("mowPressure_Set", 0, 0, m_Msg);
                            m_Msg = string.Format(" : {0}", m_Hpmj.mowFrequency_Set.GetState());
                            m_Control.SetLog("mowFrequency_Set", 0, 0, m_Msg);
                            m_Msg = string.Format(" : {0}", m_Hpmj.miwRunning_Mode.GetState());
                            m_Control.SetLog("miwRunning_Mode", 0, 0, m_Msg);
                            m_Msg = string.Format(" : {0}", m_Hpmj.miwPressure_Set.GetState());
                            m_Control.SetLog("miwPressure_Set", 0, 0, m_Msg);
                            m_Msg = string.Format(" : {0}", m_Hpmj.miwFrequency_Set.GetState());
                            m_Control.SetLog("miwFrequency_Set", 0, 0, m_Msg);

                            if (checkPPIDCompare)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Compare OK");
                                nSeqNo = 10;
                            }
                            else
                            {
                                AlarmId = ALM_PPID_COMPARE.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Compare Error");
                                nSeqNo = 1000;
                                break;
                            }
                        }
                        else
                        {
                            if (m_Hpmj.doPPID_Change_Request.GetState()) m_Hpmj.doPPID_Change_Request.SetState(false);
                        }
                    }
                    break;
                case 10:
                    {
                        bool checkPara = SetPara();
                        if (checkPara)
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(true);

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diParameter_Change_Request_Ack.SetState(true);
                                m_HpmjUnit.HpmjInverterRun();
                            }

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Request");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            AlarmId = ALM_PARAMETER_ERR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Set Error");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20: // 11.01.31 minhan 현재 파라미터를 비교를 하는 부분이 없다 베이스쪽 인터페이스에 자체가 없다.
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(PumpRun False)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(HPMJ Offline)");
                            }
                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diParameter_Change_Request_Ack.GetState())
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change OK");
                            StartTicks = XFunc.GetTickCount();
                            //nSeqNo = 25;
                            nSeqNo = 23; // 11.02.07 minhan
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            AlarmId = ALM_CHANGE_ERR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Request Error");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(true);
                        }
                    }
                    break;
                case 23: // 11.02.07 minhan
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(PumpRun False case 23)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(HPMJ Offline case 23)");
                            }
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 300)
                        {
                            bool checkparaCompare = ComparePara();

                            if (checkparaCompare)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Compare OK");
                                StartTicks = XFunc.GetTickCount();
                                nSeqNo = 25;
                            }
                            else
                            {
                                AlarmId = ALM_PARAMETER_COMPARE.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Compare Error");
                                nSeqNo = 1000;
                                break;
                            }
                        }
                        else
                        {
                            if(m_Hpmj.doParameter_Change_Request.GetState()) m_Hpmj.doParameter_Change_Request.SetState(false);
                        }
                    }
                    break;
                case 25:
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            m_Hpmj.doStart_Pump.SetState(false);
                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(PumpRun False)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(HPMJ Offline)");
                            }

                            nSeqNo = 0;
                        }
                        else if(GetElapsedTicks() > 300) // 11.02.01 minhan
                        {
                            if (m_GenInfo.AutoMode)
                            {
                                if ((m_Server.GenInfos.EQPGlassCount > 0) || m_Server.SetupHpmjIdleUse.GetValue<bool>())
                                {
                                    m_Hpmj.doStart_Pump.SetState(true);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run(Auto Mode)");
                                }
                                else // 11.02.01 minhan 이런상황리라면 0으로 떨어지는 것을 보는게 좋겠지.  
                                {
                                    m_Hpmj.doStart_Pump.SetState(false);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit doStart_Pump Off");
                                }
                                
                                m_Timer1.Start(200);
                                m_Timer2.Start(45000);
                                nSeqNo = 30;
                            }
                            else 
                            {
                                if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                                {
                                    m_Hpmj.doStart_Pump.SetState(true);
                                    m_Timer1.Start(200);
                                    m_Timer2.Start(45000);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run(Pressure)");
                                    nSeqNo = 30;
                                }
                                else // Hz Control
                                {
                                    m_Hpmj.doStart_Pump.SetState(true);
                                    m_Timer1.Start(10000); // 11.02.01 minhan 일단 10초정도
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run(Frequency)");
                                    nSeqNo = 35;
                                }
                            }
                        }
                    }
                    break;
                case 30:
                    if (m_HpmjUnit.IfFlag.PumpRun && m_Timer1.Over)
                    {
                        if (m_Server.GenInfos.AutoMode)
                        {
                            if (m_Server.GenInfos.EQPGlassCount > 0)
                            {
                                PrevPressure = m_Server.JobCond.HpmjPressure(m_HpmjUnit);
                            }
                            else
                            {
                                if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                                {
                                    PrevPressure = m_Server.SetupIdleHpmjPress.GetValue<int>();
                                }
                                else
                                {
                                    PrevPressure = 0;
                                }
                            }

                            if (m_HpmjUnit.IfFlag.PressureSet != PrevPressure)
                            {
                                m_HpmjUnit.IfFlag.PPIDChangeRequest = true;
                            }
                        }
                        else
                        {
                            PrevPressure = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();

                            if (m_HpmjUnit.IfFlag.PressureSet != PrevPressure)
                            {
                                m_HpmjUnit.IfFlag.PPIDChangeRequest = true;
                            }
                        }

                        double err = HpmjControl();

                        if (err >= 20.0)
                        {
                            m_Timer1.Start(200);
                        }
                        else
                        {
                            m_Timer1.Start(500);
                        }

                        if (m_HpmjUnit.IfFlag.PPIDChangeRequest)
                        {
                            m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                            m_HpmjUnit.IfFlag.Ready = false;
                            PrevPressure = 0;
                           
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change");
                            StartTicks = XFunc.GetTickCount();

                            nSeqNo = 50;
                        }
                    }
                    else if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(PumpRun False case 30)");
                        }
                        else
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(HPMJ Offline case 30)");
                        }

                        nSeqNo = 40;
                    }
                    else if (!m_HpmjUnit.IfFlag.Ready && m_Timer2.Over)
                    {
                        m_Hpmj.doStart_Pump.SetState(false);
                        m_HpmjUnit.IfFlag.Ready = false;
                        m_HpmjUnit.IfFlag.PIDError = true;
                        AlarmId = m_HpmjUnit.ALM_PidControlAlarm.Id;
                        m_EqpManager.SetAlarm(AlarmId);

                        m_Control.SetLog(SeqFunName, 0, 0, "Pump PID Control Error.");

                        nSeqNo = 2000;
                    }
                    break;
                case 35:
                    {
                        if (!m_HpmjUnit.IfFlag.PumpRun || !GlobalVar.HpmjOnline)
                        {
                            if (!m_HpmjUnit.IfFlag.PumpRun)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(PumpRun False case 35)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Cancel(HPMJ Offline case 35)");
                            }

                            nSeqNo = 40;
                        }
                        else if (m_Timer1.Over) // 11.02.01 minhan
                        {
                            m_HpmjUnit.IfFlag.Ready = true;
                            m_Control.SetLog(SeqFunName, 0, 0, "Pump is Ready.");
                        }
                        else if (m_HpmjUnit.IfFlag.PPIDChangeRequest)
                        {
                            m_HpmjUnit.IfFlag.PPIDChangeRequest = false;
                            m_HpmjUnit.IfFlag.Ready = false;
                            PrevPressure = 0;

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change(case 35)");
                            StartTicks = XFunc.GetTickCount();

                            nSeqNo = 50;
                        }
                    }
                    break;
                case 40:
                    {
                        m_Hpmj.doStart_Pump.SetState(false);
                        m_HpmjUnit.IfFlag.Ready = false;
                        m_HpmjUnit.IfFlag.PPIDChangeRequest = false;

                        if (m_Server.GenInfos.AutoMode)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Auto Mode Pump Stop.");
                        }
                        else
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "Manual Mode Pump Stop.");
                        }

                        nSeqNo = 0;
                    }
                    break;
                case 50:
                    if (GetElapsedTicks() > 7000)
                    {
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit PPID Change(case 50)");
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;

                        m_Control.SetLog(SeqFunName, 0, 0, "Error Recovery");

                        nSeqNo = 0;
                    }
                    break;
                case 2000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;

                        m_HpmjUnit.IfFlag.PIDError = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "Error Recovery");

                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqInterlockParaSet : XSeqFunction // 11.01.31 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        private ThreadHpmjIntefaceControl m_Control;
        private Hpmj m_HpmjUnit; // 11.01.31 minhan
        private GenInfoHandler m_GenInfo;
        private Gauge m_DifferPre; // 11.01.31 minhan
        private Alarm ALM_MANUAL_PARAMETER_ERR;
        private Alarm ALM_MANUAL_CHANGE_ERR;
        private Alarm ALM_MANUAL_PARAMETER_COMPARE; // 11.02.07 minhan
        #endregion

        #region Constructor
        public SeqInterlockParaSet(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control; // 11.01.31 minhan
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            m_HpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.01.31 minhan
            ALM_MANUAL_PARAMETER_ERR = new Alarm("HPMJ INTERFACE Manual Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_MANUAL_CHANGE_ERR = new Alarm("HPMJ INTERFACE Manual Parameter Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            ALM_MANUAL_PARAMETER_COMPARE = new Alarm("HPMJ INTERFACE Manual Parameter Compare Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
            this.SeqFunName = "HPMJ INTERFACE Para Set";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_DifferPre = eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge;
        }
        #endregion
        #region Methode
        private bool ComparePara() // 11.02.07 minhan
        {
            try
            {
                if (m_Simul.Device)
                {
                    short value = 0;
                    value = (short)m_Hpmj.mowShower_Flow_Set.GetState();
                    m_Hpmj.miwShower_Flow_Set.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Lower_Error.GetState();
                    m_Hpmj.miwShower_Flow_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Lower_Warning.GetState();
                    m_Hpmj.miwShower_Flow_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Upper_Error.GetState();
                    m_Hpmj.miwShower_Flow_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowShower_Flow_Upper_Warning.GetState();
                    m_Hpmj.miwShower_Flow_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Set.GetState();
                    m_Hpmj.miwFilter_In_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Lower_Error.GetState();
                    m_Hpmj.miwFilter_In_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Lower_Warning.GetState();
                    m_Hpmj.miwFilter_In_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_In_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_In_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_In_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Set.GetState();
                    m_Hpmj.miwFilter_Out_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Lower_Error.GetState();
                    m_Hpmj.miwFilter_Out_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Lower_Warning.GetState();
                    m_Hpmj.miwFilter_Out_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_Out_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Out_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_Out_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Set.GetState();
                    m_Hpmj.miwDI_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Lower_Error.GetState();
                    m_Hpmj.miwDI_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Lower_Warning.GetState();
                    m_Hpmj.miwDI_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Upper_Warning.GetState();
                    m_Hpmj.miwDI_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowDI_Press_Upper_Error.GetState();
                    m_Hpmj.miwDI_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Set.GetState();
                    m_Hpmj.miwResistivity_Set.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Lower_Error.GetState();
                    m_Hpmj.miwResistivity_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Lower_Warning.GetState();
                    m_Hpmj.miwResistivity_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Upper_Warning.GetState();
                    m_Hpmj.miwResistivity_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowResistivity_Upper_Error.GetState();
                    m_Hpmj.miwResistivity_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Set.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Upper_Error.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowFilter_Difference_Press_Upper_Warning.GetState();
                    m_Hpmj.miwFilter_Difference_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowInverter_Load_Current_Set.GetState();
                    m_Hpmj.miwInverter_Load_Current_Set.SetState(value);

                    value = (short)m_Hpmj.mowInveter_Load_Current_Upper_Error.GetState();
                    m_Hpmj.miwInveter_Load_Current_Upper_Error.SetState(value);

                    value = (short)m_Hpmj.mowInveter_Load_Current_Upper_Warning.GetState();
                    m_Hpmj.miwInveter_Load_Current_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Set.GetState();
                    m_Hpmj.miwMain_CO2_Press_Set.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Lower_Error.GetState();
                    m_Hpmj.miwMain_CO2_Press_Lower_Error.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Lower_Warning.GetState();
                    m_Hpmj.miwMain_CO2_Press_Lower_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Upper_Warning.GetState(); // 11.04.20 minhan
                    m_Hpmj.miwMain_CO2_Press_Upper_Warning.SetState(value);

                    value = (short)m_Hpmj.mowMain_CO2_Press_Upper_Error.GetState();
                    m_Hpmj.miwMain_CO2_Press_Upper_Error.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Set.GetState(); // 11.05.03 minhan
                    //m_Hpmj.miwMain_CO2_Flow_Set.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Lower_Error.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Lower_Error.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Lower_Warning.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Lower_Warning.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Upper_Warning.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Upper_Warning.SetState(value);

                    //value = (short)m_Hpmj.mowMain_CO2_Flow_Upper_Error.GetState();
                    //m_Hpmj.miwMain_CO2_Flow_Upper_Error.SetState(value);
                }

                if (m_Hpmj.mowShower_Flow_Set.GetState() != (ushort)m_Hpmj.miwShower_Flow_Set.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Lower_Error.GetState() != (ushort)m_Hpmj.miwShower_Flow_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Lower_Warning.GetState() != (ushort)m_Hpmj.miwShower_Flow_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Upper_Error.GetState() != (ushort)m_Hpmj.miwShower_Flow_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowShower_Flow_Upper_Warning.GetState() != (ushort)m_Hpmj.miwShower_Flow_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowFilter_In_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_In_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_In_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowFilter_Out_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowFilter_Out_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_Out_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowDI_Press_Set.GetState() != (ushort)m_Hpmj.miwDI_Press_Set.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwDI_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwDI_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwDI_Press_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowDI_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwDI_Press_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowResistivity_Set.GetState() != (ushort)m_Hpmj.miwResistivity_Set.GetState()) return false;
                if (m_Hpmj.mowResistivity_Lower_Error.GetState() != (ushort)m_Hpmj.miwResistivity_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowResistivity_Lower_Warning.GetState() != (ushort)m_Hpmj.miwResistivity_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowResistivity_Upper_Warning.GetState() != (ushort)m_Hpmj.miwResistivity_Upper_Warning.GetState()) return false;
                if (m_Hpmj.mowResistivity_Upper_Error.GetState() != (ushort)m_Hpmj.miwResistivity_Upper_Error.GetState()) return false;

                if (m_Hpmj.mowFilter_Difference_Press_Set.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Set.GetState()) return false;
                if (m_Hpmj.mowFilter_Difference_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowFilter_Difference_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwFilter_Difference_Press_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowInverter_Load_Current_Set.GetState() != (ushort)m_Hpmj.miwInverter_Load_Current_Set.GetState()) return false;
                if (m_Hpmj.mowInveter_Load_Current_Upper_Error.GetState() != (ushort)m_Hpmj.miwInveter_Load_Current_Upper_Error.GetState()) return false;
                if (m_Hpmj.mowInveter_Load_Current_Upper_Warning.GetState() != (ushort)m_Hpmj.miwInveter_Load_Current_Upper_Warning.GetState()) return false;

                if (m_Hpmj.mowMain_CO2_Press_Set.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Set.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Lower_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Lower_Error.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Lower_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Lower_Warning.GetState()) return false;
                if (m_Hpmj.mowMain_CO2_Press_Upper_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Upper_Error.GetState()) return false; // 11.04.20 minhan
                if (m_Hpmj.mowMain_CO2_Press_Upper_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Press_Upper_Warning.GetState()) return false;

                //if (m_Hpmj.mowMain_CO2_Flow_Set.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Set.GetState()) return false; // 11.05.03 minhan
                //if (m_Hpmj.mowMain_CO2_Flow_Lower_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Lower_Error.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Lower_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Lower_Warning.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Upper_Error.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Upper_Error.GetState()) return false;
                //if (m_Hpmj.mowMain_CO2_Flow_Upper_Warning.GetState() != (ushort)m_Hpmj.miwMain_CO2_Flow_Upper_Warning.GetState()) return false;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                return false;
            }

            return true;
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!GlobalVar.HpmjOnline) return -1; // 11.01.31 minhan
            int nSeqNo = this.SeqNo;
            int nRv = -1;
            switch (nSeqNo)
            {
                case 0:
                    if(GlobalVar.HpmjInterlockParaSet && !m_GenInfo.AutoMode)
                    {
                        //GlobalVar.HpmjInterlockParaSet = false;

                        try
                        {
                            int StandardPress = m_HpmjUnit.SetupHpmjStandardPressure.GetValue<int>();
                            double StandardFlow = m_HpmjUnit.SetupHpmjStandardFlowrate.GetValue<double>();
                            double StandardArea = StandardFlow / Math.Pow(StandardPress, 0.5);
                            double FlowbyPress = 0;
                            ushort fSetValue = 0;
                            ushort AlarmMargin = 0;
                            ushort WarningMargin = 0;
                            ushort fLowLimit = 0;
                            ushort fLowWarning = 0;
                            ushort fUpWarning = 0;
                            ushort fUpAlarm = 0;

                            if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                            {
                                FlowbyPress = (StandardArea * Math.Pow(m_HpmjUnit.SetupHpmjPressure.GetValue<double>(), 0.5));
                                
                                if (FlowbyPress <= 0)
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Flow Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }
                            }
                            else
                            {
                                FlowbyPress = 0.0;
                            }
                       
                            if (FlowbyPress > 0)
                            {
                                if (((FlowbyPress - m_Server.SetupFlowWarningMargin.GetValue<double>()) <= 0) ||
                                   ((FlowbyPress - m_Server.SetupFlowAlarmMargin.GetValue<double>()) <= 0))
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Flow Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                m_GenInfo.HpmjTargetFlow = string.Format("{0}", FlowbyPress); // 11.02.01 minhan
                                fSetValue = Convert.ToUInt16(FlowbyPress * 100);
                                AlarmMargin = Convert.ToUInt16(m_Server.SetupFlowAlarmMargin.GetValue<double>() * 100);
                                WarningMargin = Convert.ToUInt16(m_Server.SetupFlowWarningMargin.GetValue<double>() * 100);
                            }
                            else
                            {
                                m_GenInfo.HpmjTargetFlow = "0"; // 11.02.01 minhan
                                fSetValue = 0;
                                AlarmMargin = 0;
                                WarningMargin = 0;
                            }

                            // flow Set
                            m_Hpmj.mowShower_Flow_Set.SetState(fSetValue);
                            m_Hpmj.mowShower_Flow_Lower_Error.SetState(AlarmMargin);
                            m_Hpmj.mowShower_Flow_Lower_Warning.SetState(WarningMargin);
                            m_Hpmj.mowShower_Flow_Upper_Warning.SetState(WarningMargin);
                            m_Hpmj.mowShower_Flow_Upper_Error.SetState(AlarmMargin);

                            fSetValue = 0;
                            AlarmMargin = 0;
                            WarningMargin = 0;

                            int m_Buf = 0;
                            if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                            {
                                m_Buf = m_HpmjUnit.SetupHpmjPressure.GetValue<int>();
                                if ((m_Buf < 80) || (m_Buf > 130)) // 11.04.20 minhan
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pressure Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                fSetValue = Convert.ToUInt16(m_HpmjUnit.SetupHpmjPressure.GetValue<int>() * 10);
                            }
                            else
                            {
                                fSetValue = 0;
                            }

                            if (fSetValue > 0)
                            {
                                AlarmMargin = Convert.ToUInt16(m_Server.SetupPressAlarmMargin.GetValue<double>() * 10);
                                WarningMargin = Convert.ToUInt16(m_Server.SetupPressWarningMargin.GetValue<double>() * 10);
                            }
                            else
                            {
                                fSetValue = 0;
                                AlarmMargin = 0;
                                WarningMargin = 0;
                            }

                            // Filter In
                            m_Hpmj.mowFilter_In_Press_Set.SetState(fSetValue);
                            m_Hpmj.mowFilter_In_Press_Lower_Error.SetState(AlarmMargin);
                            m_Hpmj.mowFilter_In_Press_Lower_Warning.SetState(WarningMargin);
                            m_Hpmj.mowFilter_In_Press_Upper_Warning.SetState(WarningMargin);
                            m_Hpmj.mowFilter_In_Press_Upper_Error.SetState(AlarmMargin);

                            // Filter Out
                            m_Hpmj.mowFilter_Out_Press_Set.SetState(fSetValue);
                            m_Hpmj.mowFilter_Out_Press_Lower_Error.SetState(AlarmMargin);
                            m_Hpmj.mowFilter_Out_Press_Lower_Warning.SetState(WarningMargin);
                            m_Hpmj.mowFilter_Out_Press_Upper_Warning.SetState(WarningMargin);
                            m_Hpmj.mowFilter_Out_Press_Upper_Error.SetState(AlarmMargin);

                            fSetValue = 0;
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            // 11.02.01 minhan 일단 사용하든 하지않든 무조건 날리고, 사용해제 부분은 협의가 필요하다.
                            if ((m_HpmjUnit.MainDiPress.SetupInterlock != null) &&
                                //m_HpmjUnit.MainDiPress.SetupInterlock.Use && 
                               (m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue > 0))
                            {
                                if (//(m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue < 0.5) ||
                                   ((m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue - m_HpmjUnit.MainDiPress.SetupInterlock.LowWarning) < 0) ||
                                   ((m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue - m_HpmjUnit.MainDiPress.SetupInterlock.LowAlarm) < 0)) // 11.02.09 minhan
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "MainDiPress Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                fSetValue = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue * 1000);
                                fLowLimit = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.LowAlarm * 1000);
                                fLowWarning = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.LowWarning * 1000);
                                fUpWarning = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.HighWarning * 1000);
                                fUpAlarm = Convert.ToUInt16(m_HpmjUnit.MainDiPress.SetupInterlock.HighAlarm * 1000);
                            }
                            else
                            {
                                //if ((m_HpmjUnit.MainDiPress.SetupInterlock == null) ||
                                //     m_HpmjUnit.MainDiPress.SetupInterlock.Use ||
                                //   (m_HpmjUnit.MainDiPress.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                                //{
                                    fSetValue = 0;
                                    fLowLimit = 0;
                                    fLowWarning = 0;
                                    fUpWarning = 0;
                                    fUpAlarm = 0;
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "MainDiPress Set Error");
                                    nSeqNo = 1000;
                                    break;
                                //}
                                //else
                                //{
                                //    fSetValue = 0;
                                //    fLowLimit = 0;
                                //    fLowWarning = 0;
                                //    fUpWarning = 0;
                                //    fUpAlarm = 0;
                                //    m_Control.SetLog(SeqFunName, 0, 0, "MainDiPress Set No use");
                                //}
                            }
                            
                            // Main DI
                            m_Hpmj.mowDI_Press_Set.SetState(fSetValue);
                            m_Hpmj.mowDI_Press_Lower_Error.SetState(fLowLimit);
                            m_Hpmj.mowDI_Press_Lower_Warning.SetState(fLowWarning);
                            m_Hpmj.mowDI_Press_Upper_Warning.SetState(fUpWarning);
                            m_Hpmj.mowDI_Press_Upper_Error.SetState(fUpAlarm);

                            fSetValue = 0;
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            if ((m_HpmjUnit.Resistivity.SetupInterlock != null) &&
                                //m_HpmjUnit.Resistivity.SetupInterlock.Use && 
                               (m_HpmjUnit.Resistivity.SetupInterlock.SettingValue > 0))
                            {
                                if (((m_HpmjUnit.Resistivity.SetupInterlock.SettingValue - m_HpmjUnit.Resistivity.SetupInterlock.LowWarning) < 0) ||
                                   ((m_HpmjUnit.Resistivity.SetupInterlock.SettingValue - m_HpmjUnit.Resistivity.SetupInterlock.LowAlarm) < 0))
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                fSetValue = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.SettingValue * 100);
                                fLowLimit = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.LowAlarm * 100);
                                fLowWarning = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.LowWarning * 100);
                                fUpWarning = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.HighWarning * 100);
                                fUpAlarm = Convert.ToUInt16(m_HpmjUnit.Resistivity.SetupInterlock.HighAlarm * 100);
                            }
                            else
                            {
                                //if ((m_HpmjUnit.Resistivity.SetupInterlock == null) ||
                                //     m_HpmjUnit.Resistivity.SetupInterlock.Use ||
                                //   (m_HpmjUnit.Resistivity.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                                //{
                                    fSetValue = 0;
                                    fLowLimit = 0;
                                    fLowWarning = 0;
                                    fUpWarning = 0;
                                    fUpAlarm = 0;
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set Error");
                                    nSeqNo = 1000;
                                    break;
                                //}
                                //else
                                //{
                                //    fSetValue = 0;
                                //    fLowLimit = 0;
                                //    fLowWarning = 0;
                                //    fUpWarning = 0;
                                //    fUpAlarm = 0;
                                //    m_Control.SetLog(SeqFunName, 0, 0, "Resistivity Set No use");
                                //}
                            }


                            // DI Resistance
                            m_Hpmj.mowResistivity_Set.SetState(fSetValue);
                            m_Hpmj.mowResistivity_Lower_Error.SetState(fLowLimit);
                            m_Hpmj.mowResistivity_Lower_Warning.SetState(fLowWarning);
                            m_Hpmj.mowResistivity_Upper_Warning.SetState(fUpWarning);
                            m_Hpmj.mowResistivity_Upper_Error.SetState(fUpAlarm);

                            fSetValue = 0;
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            if ((m_DifferPre.SetupInterlock != null) &&
                                //m_DifferPre.SetupInterlock.Use && 
                               (m_DifferPre.SetupInterlock.SettingValue >= 0))
                            {
                                fSetValue = Convert.ToUInt16(m_DifferPre.SetupInterlock.SettingValue * 10);
                                fUpWarning = Convert.ToUInt16(m_DifferPre.SetupInterlock.HighWarning * 10);
                                fUpAlarm = Convert.ToUInt16(m_DifferPre.SetupInterlock.HighAlarm * 10);
                            }
                            else
                            {
                                //if ((m_DifferPre.SetupInterlock == null) ||
                                //     m_DifferPre.SetupInterlock.Use ||
                                //    (m_DifferPre.SetupInterlock.SettingValue < 0)) // 11.02.07 minhan
                                //{
                                    fSetValue = 0;
                                    fUpAlarm = 0;
                                    fUpWarning = 0;
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "DifferPre Set Error");
                                    nSeqNo = 1000;
                                    break;
                                //}
                                //else
                                //{
                                //    fSetValue = 0;
                                //    fUpAlarm = 0;
                                //    fUpWarning = 0;
                                //    m_Control.SetLog(SeqFunName, 0, 0, "DifferPre Set No use");
                                //}
                            }

                            // Difference Press
                            m_Hpmj.mowFilter_Difference_Press_Set.SetState(fSetValue);
                            m_Hpmj.mowFilter_Difference_Press_Upper_Error.SetState(fUpAlarm);
                            m_Hpmj.mowFilter_Difference_Press_Upper_Warning.SetState(fUpWarning);

                            fSetValue = 0;
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            if ((m_HpmjUnit.InvertCurrent.SetupInterlock != null) &&
                                 //m_HpmjUnit.InvertCurrent.SetupInterlock.Use && 
                               (m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue > 0))
                            {
                                fSetValue = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue * 10);
                                fUpWarning = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.HighWarning * 10);
                                fUpAlarm = Convert.ToUInt16(m_HpmjUnit.InvertCurrent.SetupInterlock.HighAlarm * 10);
                            }
                            else
                            {
                                //if ((m_HpmjUnit.InvertCurrent.SetupInterlock == null) ||
                                //     m_HpmjUnit.InvertCurrent.SetupInterlock.Use ||
                                //    (m_HpmjUnit.InvertCurrent.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                                //{
                                    fSetValue = 0;
                                    fUpWarning = 0;
                                    fUpAlarm = 0;
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "InvertCurrent Set Error");
                                    nSeqNo = 1000;
                                    break;
                                //}
                                //else
                                //{
                                //    fSetValue = 0;
                                //    fUpWarning = 0;
                                //    fUpAlarm = 0;
                                //    m_Control.SetLog(SeqFunName, 0, 0, "InvertCurrent Set No use");
                                //}
                            }

                            // Inverter Current
                            m_Hpmj.mowInverter_Load_Current_Set.SetState(fSetValue);
                            m_Hpmj.mowInveter_Load_Current_Upper_Error.SetState(fUpAlarm);
                            m_Hpmj.mowInveter_Load_Current_Upper_Warning.SetState(fUpWarning);

                            fSetValue = 0;
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            if ((m_HpmjUnit.CO2InPress.SetupInterlock != null) &&
                                 //m_HpmjUnit.CO2InPress.SetupInterlock.Use && 
                               (m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue > 0))
                            {
                                if (//(m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue < 0.5) ||
                                  ((m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue - m_HpmjUnit.CO2InPress.SetupInterlock.LowWarning) < 0) ||
                                  ((m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue - m_HpmjUnit.CO2InPress.SetupInterlock.LowAlarm) < 0)) // 11.02.09 minhan
                                {
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set Error");
                                    nSeqNo = 1000;
                                    break;
                                }

                                fSetValue = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue * 1000);
                                fLowWarning = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.LowWarning * 1000);
                                fLowLimit = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.LowAlarm * 1000);
                                fUpWarning = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.HighWarning * 1000); // 11.04.20 minhan
                                fUpAlarm = Convert.ToUInt16(m_HpmjUnit.CO2InPress.SetupInterlock.HighAlarm * 1000);
                            }
                            else
                            {
                                //if ((m_HpmjUnit.CO2InPress.SetupInterlock == null) ||
                                //     m_HpmjUnit.CO2InPress.SetupInterlock.Use ||
                                //   (m_HpmjUnit.CO2InPress.SetupInterlock.SettingValue <= 0)) // 11.02.07 minhan
                                //{
                                    fSetValue = 0;
                                    fLowWarning = 0;
                                    fLowLimit = 0;
                                    fUpWarning = 0; // 11.04.20 minhan
                                    fUpAlarm = 0;
                                    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set Error");
                                    nSeqNo = 1000;
                                    break;
                                //}
                                //else
                                //{
                                //    fSetValue = 0;
                                //    fLowWarning = 0;
                                //    fLowLimit = 0;
                                //    m_Control.SetLog(SeqFunName, 0, 0, "CO2InPress Set No use");
                                //}
                            }

                            // Co2 Main
                            m_Hpmj.mowMain_CO2_Press_Set.SetState(fSetValue);
                            m_Hpmj.mowMain_CO2_Press_Lower_Error.SetState(fLowLimit);
                            m_Hpmj.mowMain_CO2_Press_Lower_Warning.SetState(fLowWarning);
                            m_Hpmj.mowMain_CO2_Press_Upper_Warning.SetState(fUpWarning); // 11.04.20 minhan
                            m_Hpmj.mowMain_CO2_Press_Upper_Error.SetState(fUpAlarm);

                            fSetValue = 0; // 11.05.03 minhan
                            fLowLimit = 0;
                            fLowWarning = 0;
                            fUpWarning = 0;
                            fUpAlarm = 0;

                            //if ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock != null) &&
                            //    m_HpmjUnit.CO2Flow.SetupInterlock.Use && 
                            //   (m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue > 0)) // 11.05.03 minhan
                            //{
                            //    if (//(m_HpmjUnit.CO2Flow.SetupInterlock.SettingValue < 0.5) ||
                            //      ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue - m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowWarning) < 0) ||
                            //      ((m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue - m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowAlarm) < 0))
                            //    {
                            //        AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                            //        m_EqpManager.SetAlarm(AlarmId);
                            //        m_Control.SetLog(SeqFunName, 0, 0, "CO2Flow Set Error");
                            //        nSeqNo = 1000;
                            //        break;
                            //    }

                            //    fSetValue = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.SettingValue * 10); // 11.05.26 minhan
                            //    fLowWarning = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowWarning * 10);
                            //    fLowLimit = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.LowAlarm * 10);
                            //    fUpWarning = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.HighWarning * 10);
                            //    fUpAlarm = Convert.ToUInt16(m_HpmjUnit.HpmjCo2Flow.SetupInterlock.HighAlarm * 10);
                            //}
                            //else
                            //{
                            //    fSetValue = 0;
                            //    fLowWarning = 0;
                            //    fLowLimit = 0;
                            //    fUpWarning = 0;
                            //    fUpAlarm = 0;
                            //    AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                            //    m_EqpManager.SetAlarm(AlarmId);
                            //    m_Control.SetLog(SeqFunName, 0, 0, "CO2Flow Set Error");
                            //    nSeqNo = 1000;
                            //    break;
                            //}

                            // Co2 Flow Main
                            //m_Hpmj.mowMain_CO2_Flow_Set.SetState(fSetValue);
                            //m_Hpmj.mowMain_CO2_Flow_Lower_Error.SetState(fLowLimit);
                            //m_Hpmj.mowMain_CO2_Flow_Lower_Warning.SetState(fLowWarning);
                            //m_Hpmj.mowMain_CO2_Flow_Upper_Warning.SetState(fUpWarning);
                            //m_Hpmj.mowMain_CO2_Flow_Upper_Error.SetState(fUpAlarm);

                            m_Hpmj.doParameter_Change_Request.SetState(true);

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diParameter_Change_Request_Ack.SetState(true);
                            }
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        catch (Exception err)
                        {
                            string msg = err.ToString();
                            m_Server.WriteExceptionLog(msg);
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            AlarmId = ALM_MANUAL_PARAMETER_ERR.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Error");
                            nSeqNo = 1000;
                        }
                    }
                    else if (m_GenInfo.AutoMode)
                    {
                        if (GlobalVar.HpmjInterlockParaSet) GlobalVar.HpmjInterlockParaSet = false;
                    }
                    break;
                case 10:
                    if (!GlobalVar.HpmjOnline || m_GenInfo.AutoMode)
                    {
                        GlobalVar.HpmjInterlockParaSet = false; // 11.01.31 minhan
                        m_Hpmj.doParameter_Change_Request.SetState(false);
                        if (!GlobalVar.HpmjOnline)
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(HPMJ Offline)");
                        }
                        else
                        {
                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(Auto Mode)");
                        }
                        nSeqNo = 0;
                    }
                    else if (m_Hpmj.diParameter_Change_Request_Ack.GetState())
                    {
                        //GlobalVar.HpmjInterlockParaSet = false; // 11.01.31 minhan
                        m_Hpmj.doParameter_Change_Request.SetState(false);
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change OK");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20; // 11.02.07 minhan
                    }
                    else if (GetElapsedTicks() > 3000)
                    {
                        m_Hpmj.doParameter_Change_Request.SetState(false);
                        AlarmId = ALM_MANUAL_CHANGE_ERR.Id;
                        m_EqpManager.SetAlarm(AlarmId);
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Request Error");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        m_Hpmj.doParameter_Change_Request.SetState(true);
                    }
                    break;
                case 20: // 11.02.07 minhan
                    {
                        if (!GlobalVar.HpmjOnline || m_GenInfo.AutoMode)
                        {
                            GlobalVar.HpmjInterlockParaSet = false; // 11.01.31 minhan
                            m_Hpmj.doParameter_Change_Request.SetState(false);
                            if (!GlobalVar.HpmjOnline)
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(HPMJ Offline case 20)");
                            }
                            else
                            {
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Change Cancel(Auto Mode case 20)");
                            }
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            bool checkparaCompare = ComparePara();

                            if (checkparaCompare)
                            {
                                GlobalVar.HpmjInterlockParaSet = false; // 11.01.31 minhan
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Compare OK");
                                nSeqNo = 0;
                            }
                            else
                            {
                                AlarmId = ALM_MANUAL_PARAMETER_COMPARE.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Parameter Compare Error");
                                nSeqNo = 1000;
                                break;
                            }
                        }
                        else
                        {
                            if (m_Hpmj.doParameter_Change_Request.GetState()) m_Hpmj.doParameter_Change_Request.SetState(false);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        GlobalVar.HpmjInterlockParaSet = false; // 11.01.31 minhan
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;

                        m_Control.SetLog(SeqFunName, 0, 0, "Error Recovery");

                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqhpmjStateMonitor : XSeqFunction // 11.01.31 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.27 minhan
        private GenInfoHandler m_GenInfo;
        private Hpmj m_HpmjUnit; // 11.01.31 minhan
        private Alarm m_AlarmMainDI; // 11.02.09 minhan
        #endregion

        #region Constructor
        public SeqhpmjStateMonitor(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            m_Control = control;
            this.SeqFunName = "HPMJ INTERFACE Monitor";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            m_HpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.01.31 minhan
            m_AlarmMainDI = new Alarm("HPMJ Main DI Limit Alarm ", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.09 minhan
        }
        #endregion

        #region Methods
        public bool IsInterlock()
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool interlock = false;
            interlock |= ((heavy > 0) ? true : false); // 나중에 제외할 알람들이 생기면 제외하삼.
            
            return interlock;
        }

        public void FlagSet()
        {
            if (m_Server.GenInfos.AutoMode)
            {
                if (m_Server.GenInfos.EQPGlassCount > 0)
                {
                    m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", m_Server.JobCond.HpmjPressure(m_HpmjUnit));
                }
                else
                {
                    if (m_Server.SetupHpmjIdleUse.GetValue<bool>())
                    {
                        m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", m_Server.SetupIdleHpmjPress.GetValue<int>());
                    }
                    else
                    {
                        m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
                    }
                }
            }
            else
            {
                //if (m_Server.SetupHpmjIdleUse.GetValue<bool>()) // 확인해야한다.
                //{
                    if (m_HpmjUnit.SetupHpmjMode.GetValue<bool>())
                    {
                        m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", m_HpmjUnit.SetupHpmjPressure.GetValue<int>());
                    }
                    else
                    {
                        m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
                    }
                //}
                //else m_GenInfo.HpmjTargetPress = string.Format("{0:F1}", 0);
            }

        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.SeqNo;
            int nRv = -1;

            bool bRun = true;
            bool Interlock = IsInterlock();

            FlagSet(); // 11.01.31 minhan

            if (m_EqpManager.AlarmResetSwitchPushed) // 11.02.09 minhan
            {
                if (AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm (AlarmId);
                }
            }
            
            if (m_HpmjUnit.SetupHpmjUse.GetValue<bool>()) // 11.02.09 minhan
            {
                if (((double)(m_HpmjUnit.MainDiPress.CurAdc / 1000) <= m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<double>()) && (AlarmId == 0))
                {
                    AlarmId = m_AlarmMainDI.Id;
                    m_EqpManager.SetAlarm(AlarmId);
                }
            }

            if (!m_Server.GenInfos.AutoMode) // 메뉴얼 모드에서 강제로 off되는 상황들만.
            {
                if (!m_HpmjUnit.SetupHpmjUse.GetValue<bool>() ||
                    Interlock || 
                    !GlobalVar.HpmjOnline || GlobalVar.HpmjPumpRunErr || 
                    !GlobalVar.HpmjRemote || !GlobalVar.HpmjReady ||
                    m_HpmjUnit.IfFlag.PIDError ||
                    m_Hpmj.diAlarmHeavy.GetState() ||
                   ((double)(m_HpmjUnit.MainDiPress.CurAdc / 1000) <= m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<double>() && !m_Simul.Device))
                {
                    m_HpmjUnit.IfFlag.PumpRun = false;
                    //return -1;
                }
            }
                        
            if (!m_GenInfo.EqpInitComp) return -1;

            bool Alarm = false;
            Alarm |= m_HpmjUnit.OwnerUnit.IfFlag.InError;
            Alarm |= m_HpmjUnit.OwnerUnit.IfFlag.OutError;
            Alarm |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.InError;
            Alarm |= m_HpmjUnit.OwnerUnit.NextCv.IfFlag.OutError;
            Alarm |= (m_GenInfo.AutoMode && GlobalVar.UlTimeOut) ? true : false; // 11.02.01 minhan 일단 정체 발생시 hpmj stop

            bRun &= m_GenInfo.AutoMode;
            bRun &= m_GenInfo.DiStart; // 11.02.01 minhan 이거는 협의 좀 해야 겠네.
            bRun &= m_Server.JobCond.ProcessMode;
            bRun &= !Interlock; // 11.01.31 minhan
            //bRun &= (m_Server.JobCond.HeavyInterlock > 0 ? false : true);
            bRun &= !m_HpmjUnit.IfFlag.PIDError;
            bRun &= GlobalVar.HpmjRemote;
            bRun &= GlobalVar.HpmjReady;
            bRun &= m_HpmjUnit.SetupHpmjUse.GetValue<bool>();
            bRun &= GlobalVar.HpmjOnline;
            bRun &= !GlobalVar.HpmjPumpRunErr; // 11.01.31 minhan
            bRun &= !m_Hpmj.diAlarmHeavy.GetState();
            //bRun &= (((double)(m_HpmjUnit.MainDiPress.CurAdc / 1000) > m_HpmjUnit.SetupHpmjCO2InValveCloseDiLevel.GetValue<double>()) || m_Simul.Device) ? true : false; //11.11.04 test
            bRun &= !Alarm;
            bRun &= (m_Server.JobCond.HpmjUse(m_HpmjUnit) || m_Server.GenInfos.EQPGlassCount == 0) ? true : false;
            bRun &= ((m_GenInfo.IdleRunning && m_Server.SetupHpmjIdleUse.GetValue<bool>()) || m_Server.GenInfos.EQPGlassCount > 0) ? true : false; // 확인해야함.

            switch (nSeqNo)
            {
                case 0:
                    if (bRun)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = true;
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set True");
                        nSeqNo = 10;
                    }
                    else if (m_GenInfo.AutoMode) // 11.05.14 minhan
                    {
                        if (m_HpmjUnit.IfFlag.PumpRun) m_HpmjUnit.IfFlag.PumpRun = false;
                    }
                    break;
                case 10:
                    if (!bRun)
                    {
                        m_HpmjUnit.IfFlag.PumpRun = false;
                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Pump Run Flag Set False");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqhpmjAlarmCode : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        protected static HpmjInterface m_Hpmj;
        private GenInfoHandler m_GenInfo;
        private ThreadHpmjIntefaceControl m_Control; // 11.01.31 minhan
        private Alarm ALM_HPMJ_ALARM_RESET;
        private short[] m_AlarmCode;
        private bool[] m_AlarmFlag;
        private int[] m_AlarmIds;
        private int m_AlarmCount;
        private bool m_FirstRun; // 11.04.27 minhan
        #endregion

        #region Constructor
        public SeqhpmjAlarmCode(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
//            m_Hpmj = eqpHpmjInterfaces._HpmjInterface;
            this.SeqFunName = "HPMJ INTERFACE Alarm Code";
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            ALM_HPMJ_ALARM_RESET = new Alarm("HPMJ INTERFACE Alarm Reset Error ", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmCode = new short[3];
            m_AlarmFlag = new bool[48]; 
            m_AlarmIds = new int[46]; // 11.05.03 minhan
            m_AlarmCount = 0;
            m_FirstRun = true; // 11.04.27 minhan
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp || !GlobalVar.HpmjOnline) return -1; // 11.01.31 minhan
            bool nUse = eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>();
            int nSeqNo = this.SeqNo;
            
            int nRv = -1;
            m_AlarmCode[0] = m_Hpmj.miwAlarm_Code1.GetState();
            m_AlarmCode[1] = m_Hpmj.miwAlarm_Code2.GetState();
            m_AlarmCode[2] = m_Hpmj.miwAlarm_Code3.GetState();

            bool AlarmConfirm = m_Hpmj.diAlarmHeavy.GetState(); // 11.01.31 minhan
                 AlarmConfirm |= m_Hpmj.diAlarmLight.GetState(); 
            // 알람코드와 알람종류가 들어 왔을 경우를 같이 보는 것으로 하자. 일단.
            switch (nSeqNo)
            {
                case 0:
                    if (nUse && AlarmConfirm && GlobalVar.HpmjOnline) // 11.01.31 minhan
                    {
                        if (m_AlarmCode[0] > 0 || m_AlarmCode[1] > 0 || m_AlarmCode[2] > 0)
                        {
                            if (m_FirstRun) // 11.04.27 minhan
                            {
                                m_FirstRun = false;
                                m_Hpmj.doAlarm_Reset_Request.SetState(true);

                                if (m_Simul.Device) // 11.02.01 minhan
                                {
                                    m_Hpmj.diAlarm_Reset_Request_Ack.SetState(true);
                                }

                                m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset Request");
                                StartTicks = XFunc.GetTickCount();
                                nSeqNo = 1100;
                                break;
                            }

                            for (int i = 0; i < 3; i++)
                            {
                                byte[] bit = BitConverter.GetBytes(m_AlarmCode[i]);
                                BitArray nBitarr = new BitArray(bit);
                                for (int j = 0; j < 16; j++)
                                {
                                    if (nBitarr.Get(j)) m_AlarmFlag[16 * i + j] = true;
                                    else m_AlarmFlag[16 * i + j] = false;
                                }

                            }

                            for (int i = 0; i < 46; i++)
                            {
                                if (m_AlarmFlag[i] == true)
                                {
                                    int alarmnum;
                                    if (i > 9 && i < 22) alarmnum = i - 1;
                                    else if (i > 22) alarmnum = i - 2;
                                    //else if (i > 43) alarmnum = i - 3; // 11.05.03 minhan
                                    else alarmnum = i;

                                    AlarmId = m_Hpmj.ALM_HPMJ_UNIT_ESTOP.Id + alarmnum;
                                    m_AlarmCount++;
                                    m_AlarmIds[m_AlarmCount] = AlarmId;
                                    m_EqpManager.SetAlarm(AlarmId);
                                    string msg = string.Format("HPMJ Unit Alarm ID : {0}", AlarmId);
                                    m_Control.SetLog(SeqFunName, 0, 0, msg);
                                    nSeqNo = 1000;
                                }
                            }
                        }
                    }
                    else if (GlobalVar.HpmjAlarmReset) // 11.01.31 minhan
                    {
                        m_Hpmj.doAlarm_Reset_Request.SetState(true);

                        if (m_Simul.Device) // 11.02.01 minhan
                        {
                            m_Hpmj.diAlarm_Reset_Request_Ack.SetState(false);
                        }

                        m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset Request(GlobalVar.HpmjAlarmReset True)");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1100;
                    }
                    else if(m_GenInfo.AutoMode)
                    {
                        if (m_Hpmj.doAlarm_Reset_Request.GetState()) m_Hpmj.doAlarm_Reset_Request.SetState(false);
                        if (GlobalVar.HpmjAlarmReset) GlobalVar.HpmjAlarmReset = false;

                        if (m_Simul.Device) // 11.02.01 minhan
                        {
                            m_Hpmj.diAlarm_Reset_Request_Ack.SetState(false);
                        }

                    }
                    break;
                case 1000:
                    if ((m_EqpManager.AlarmResetSwitchPushed) || GlobalVar.HpmjAlarmReset)
                    {
                        if (!GlobalVar.HpmjOnline) // offlline 상태라면 그냥 삭제
                        {
                            if (GlobalVar.HpmjAlarmReset) GlobalVar.HpmjAlarmReset = false;

                            for (int i = 0; i < m_AlarmCount + 1; i++)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmIds[i]);
                            }
                            for (int i = 0; i < 46; i++) // 11.05.03 minhan
                            {
                                m_AlarmIds[i] = 0;
                            }
                            m_AlarmCount = 0;

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset(offline case 1000)"); 

                            nSeqNo = 0;
                        }
                        else // 11.01.31 minhan
                        {
                            m_Hpmj.doAlarm_Reset_Request.SetState(true);

                            if (m_Simul.Device) // 11.02.01 minhan
                            {
                                m_Hpmj.diAlarm_Reset_Request_Ack.SetState(true);
                            }

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset Request"); 
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1100;
                        }
                    }
                    break;
                case 1100:
                    {
                        if (!GlobalVar.HpmjOnline) // offlline 상태라면 그냥 삭제
                        {
                            if (GlobalVar.HpmjAlarmReset) GlobalVar.HpmjAlarmReset = false;
                            
                            m_Hpmj.doAlarm_Reset_Request.SetState(false);

                            for (int i = 0; i < m_AlarmCount + 1; i++)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmIds[i]);
                            }
                            for (int i = 0; i < 46; i++) // 11.05.03 minhan
                            {
                                m_AlarmIds[i] = 0;
                            }
                            m_AlarmCount = 0;

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset(offline case 1100)");

                            nSeqNo = 0;
                        }
                        else if (m_Hpmj.diAlarm_Reset_Request_Ack.GetState())
                        {
                            if (GlobalVar.HpmjAlarmReset) GlobalVar.HpmjAlarmReset = false;
                            
                            m_Hpmj.doAlarm_Reset_Request.SetState(false);

                            for (int i = 0; i < m_AlarmCount + 1; i++)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmIds[i]);
                            }
                            for (int i = 0; i < 46; i++) // 11.05.03 minhan
                            {
                                m_AlarmIds[i] = 0;
                            }
                            m_AlarmCount = 0;

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset OK");

                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_Hpmj.doAlarm_Reset_Request.SetState(false);

                            for (int i = 0; i < m_AlarmCount + 1; i++) // 일단 알람은 삭제하고..
                            {
                                m_EqpManager.ResetAlarm(m_AlarmIds[i]);
                            }
                            for (int i = 0; i < 46; i++) // 11.05.03 minhan
                            {
                                m_AlarmIds[i] = 0;
                            }
                            m_AlarmCount = 0;

                            AlarmId = ALM_HPMJ_ALARM_RESET.Id;
                            m_EqpManager.SetAlarm(AlarmId);

                            m_Control.SetLog(SeqFunName, 0, 0, "HPMJ Unit Alarm Reset Time Over");
                            nSeqNo = 1200;
                        }
                        else
                        {
                            m_Hpmj.doAlarm_Reset_Request.SetState(true);
                        }
                    }
                    break;
                case 1200:
                    {
                        if (m_EqpManager.AlarmResetSwitchPushed)
                        {
                            if (GlobalVar.HpmjAlarmReset) GlobalVar.HpmjAlarmReset = false;
                            m_EqpManager.ResetAlarm(AlarmId);
                            AlarmId = 0;
                            m_Control.SetLog(SeqFunName, 0, 0, "Recovery");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }
    public class SeqHpmjGaugeLogdata : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        //private ThreadHpmjIntefaceControl m_Control;
        //protected static _GenericCollection<Gauge> m_Gauges ; //hpmj Gauge Log 남길려고....11.01.31 minhan
        private XLog HPMJGaugeLog;
        private Hpmj m_HpmjUnit; // 11.01.31 minhan
        private Alarm ALM_HPMJ_LOG; // 11.01.31 minhan
        private string hpmjGuagelog;
        #endregion

        #region Constructor
        public SeqHpmjGaugeLogdata(ThreadHpmjIntefaceControl control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            //m_Control = control;
            HPMJGaugeLog = HPMJGaugeLog = new XLog("HPMJ Gauge Log", XLog.LogStampType.UseStamp); // 11.01.31 minhan 
            ALM_HPMJ_LOG = new Alarm("HPMJ Gauge Log Write Alarm ", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.01.31 minhan
            m_HpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.01.31 minhan
            //m_Gauges = new _GenericCollection<Gauge>();
            //m_Gauges = ThreadGauge.Units;
            this.SeqFunName = "HPMJ Guage Log";
            hpmjGuagelog = ""; // 11.01.31 minhan
        }
        #endregion

        #region methode
        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
            string log;

            if (portNo <= 0) portName = "";
            else
            {
                portName = portNo.ToString();
            }

            if (slotNo <= 0) slotName = "";
            else
            {
                slotName = slotNo.ToString();
            }

            log = string.Format("HPMJ Log \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            HPMJGaugeLog.TextOut(log);

        }
        #endregion
        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {

                case 0:
                    if (m_HpmjUnit.IfFlag.PumpRun)
                    {
                        //Log 남기면 된당.
                        hpmjGuagelog = "";
                        SetLog(SeqFunName, 0, 0, "hpmj pump run");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_HpmjUnit.IfFlag.PumpRun)
                    {
                        hpmjGuagelog = "";
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    else
                    {
                        SetLog(SeqFunName, 0, 0, "hpmj pump Stop");
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    if (m_HpmjUnit.IfFlag.PumpRun && GetElapsedTicks() > 20000) // 11.01.31 minhan 3초라.. 너무 그런거 아닌가. 하드용량이 많은 것도 아닌데.
                    {
                        try
                        {
                            //Log 남기장

                            //foreach (Gauge device in m_Gauges)
                            //{
                            //    //무식한 방법 kang
                            //    if (device.Name == eqpGauges._FR_Unit_HPMJ_CO2_Pressure_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_DI_Resistance_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Filter_In_Pressure_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Filter_Out_Pressure_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Inverter_Current_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Main_DI_Pressure_Gauge_Name ||
                            //        device.Name == eqpGauges._FR_Unit_HPMJ_Shower_Flowrate_Gauge_Name)
                            //    {
                            //        hpmjGuagelog += device.Name +" : "+ Convert.ToString(device.CurValue) + "\t";
                            //    }
                            //}
                            // 11.01.31 minhan 참 소수점 때문에 이런 짓을 하다니 맘이 아프다.
                            hpmjGuagelog = "";
                            hpmjGuagelog += string.Format("Co2 : {0}", (double)m_HpmjUnit.CO2InPress.CurAdc / 1000);
                            hpmjGuagelog += string.Format(" Res : {0}", (double)m_HpmjUnit.Resistivity.CurAdc / 100);
                            hpmjGuagelog += string.Format(" Dif : {0}", (double)eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge.CurAdc / 10);
                            hpmjGuagelog += string.Format(" Current : {0}", (double)m_HpmjUnit.InvertCurrent.CurAdc / 10);
                            hpmjGuagelog += string.Format(" MainDI : {0}", (double)m_HpmjUnit.MainDiPress.CurAdc / 1000);
                            hpmjGuagelog += string.Format(" FilterIn : {0}", (double)m_HpmjUnit.FilterInPress.CurAdc / 10);
                            hpmjGuagelog += string.Format(" FilterOut : {0}", (double)m_HpmjUnit.FilterOutPress.CurAdc / 10);
                            hpmjGuagelog += string.Format(" Flow : {0}", (double)m_HpmjUnit.HpmjFlow.CurAdc / 100);

                            SetLog(SeqFunName, 0, 0, hpmjGuagelog);
                            hpmjGuagelog = "";
                            nSeqNo = 10;
                        }
                        catch (Exception err)
                        {
                            hpmjGuagelog = "";
                            string msg = err.ToString();
                            m_Server.WriteExceptionLog(msg);
                            AlarmId = ALM_HPMJ_LOG.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            SetLog(SeqFunName, 0, 0, "hpmj Log Write Error");
                            nSeqNo = 1000;
                        }
                    }
                    else if (!m_HpmjUnit.IfFlag.PumpRun)
                    {
                        hpmjGuagelog = "";
                        SetLog(SeqFunName, 0, 0, "hpmj pump Stop");
                        nSeqNo = 0;
                    }
                    break;
                case 1000: // 11.01.31 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;

                        SetLog(SeqFunName, 0, 0, "Error Recovery");

                        nSeqNo = 0;
                    }
                    break;
            }

            this.SeqNo = nSeqNo;
            return -1;
        }
        #endregion
    }
}

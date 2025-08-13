using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Device;
using Dms.Common; // 09.05.30 minhan
using Dms.Data; // 10.12.25 minhan
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadProcessUnitLTPS_G8_DHDC : ThreadProcessUnit
    {
        public ThreadProcessUnitLTPS_G8_DHDC(int scanTime, IServerManager server)
            : base(scanTime, server)
        {
        }

        protected override void RegisterSequences()
        {
            //공통 시나리오 등록
            RegisterSequence(new SeqGlassExist(this, m_Server));
            RegisterSequence(new SeqModeChange(this, m_Server));
            RegisterSequence(new SeqDiwIdleRunning(this, m_Server));

            //각 ProcessUnit의 Scenario 등록
            RegisterSequence(new SeqProcessUnit(this, eqpProcessUnits._RB_MJ_CDA, ProcessScenario.RbMJProcess));
            RegisterSequence(new SeqProcessUnit(this, eqpProcessUnits._FR_Shower, ProcessScenario.FrShowerProcess));
            RegisterSequence(new SeqProcessUnit(this, eqpProcessUnits._AK_Up_CDA, ProcessScenario.AirKnifeProcess));
            RegisterSequence(new SeqProcessUnit(this, eqpProcessUnits._AK_Lo_CDA, ProcessScenario.AirKnifeProcess));

            //추가
            RegisterSequence(new SeqServoStatus(m_Server)); // 10.12.25 minhan

        }
    }

    public class SeqGlassExist : XSeqFunction // 09.05.30 minhan
    {
        #region Fields
        //protected static IServerManager m_Server;
        protected static ServerManager m_Server; // 09.10.30 minhan
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static ThreadProcessUnit m_Control;
        protected static GenInfoHandler m_GenInfos;
        #endregion

        #region Constructor
        public SeqGlassExist(ThreadProcessUnit control, IServerManager server)
        {
            //m_Server = server;
            m_Server = ServerManager.Instance; // 09.10.30 minhan
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

            if (m_Server.SetupSingleMode.GetValue<bool>() == true)
            {
                if (!m_GenInfos.SingleRunMode) m_GenInfos.SingleRunMode = true; // 09.10.30 minhan
            }
            else m_GenInfos.SingleRunMode = false;

            //if (false) // 10.12.25 minhan
            //{
            //    m_GenInfos.BypassCheck = "ON";
            //}
            //else m_GenInfos.BypassCheck = "OFF";

            //int idleWaitTime = 60 * 1000 * (int)m_Server.JobCond.SetupIdleStopTime;

            int idleWaitTime = 60 * 1000 * m_Server.SetupIdleWaitTime.GetValue<int>(); // 11.04.20 minhan

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Server.GlassData.Count == 0)
                        {
                            nSeqNo = 20;
                        }
                        else
                        {
                            nSeqNo = 20;
                        }
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
                            //foreach (CvUnit cv in m_CvUnits) // 11.04.20 minhan
                            //{
                            //    if (cv.Id == eqpTransferUnits._UL_CvUnit.Id) // 09.11.12 minhan
                            //    {
                            //        processRequired |= m_Server.GlassData.IsExist(cv.DataMatchingKey(0)); // 09.05.30 minhan
                            //    }
                            //    else
                            //    {
                            //        processRequired |= m_Server.GlassData.IsExist(cv.DataMatchingKey(0)); // 09.05.30 minhan
                            //        processRequired |= m_Server.GlassData.IsExist(cv.DataMatchingKey(1));
                            //    }
                            //}

                            for (int i = eqpTransferUnits._LD_CvUnit.DataMatchingKey(0); i <= eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0); i++) // 11.04.22 minhan
                            {
                                if (m_Server.GlassData.IsExist(i))
                                {
                                    if (!processRequired)
                                    {
                                        processRequired = true;
                                    }
                                }
                            }

                            if (!processRequired && m_GenInfos.DiStart)
                            {
                                m_GenInfos.DiStart = false;
                            }
                            else if (processRequired && !m_GenInfos.DiStart && m_GenInfos.EqpInitComp)
                            {   //jemoon : 100614 : 초기화 되지 않았으면 distart하면 안되지
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
                        if (!m_GenInfos.IdleRunning) m_GenInfos.DiStart = false; // 11.02.01 minhan 검토해보자.

                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (m_GenInfos.AutoMode)
                    {
                        if (!m_GenInfos.IdleRunning) diStart &= (m_Server.GlassData.Count > 0); // 11.02.01 minhan
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

    public class SeqDiwIdleRunning : XSeqFunction // 09.08.18 minhan
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static ThreadProcessUnit m_Control;
        protected static _GenInfoHandler m_GenInfos;
        protected int m_OldTime = 0;
        //protected XTimer m_Timer; 
        //protected int m_Tm;
        protected int m_ProgressTime;
        private int m_OldIdleRunTime = 0; // 09.08.18 minhan
        private int m_OldIdleStopTime = 0;
        private bool m_OldDiStatus = false; // 09.08.18 minhan
        #endregion

        #region Constructor
        public SeqDiwIdleRunning(ThreadProcessUnit control, IServerManager server)
        {
            m_Server = server;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            //m_Timer = new XTimer("Timer : SeqIdleRunning");
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int RunTime = (int)m_Server.JobCond.SetupIdleRunTime; // 11.03.22 minhan
            int StopTime = (int)m_Server.JobCond.SetupIdleStopTime; // 09.11.12 minhan
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
                        //m_Tm = StopTime * 10;
                        //m_Timer.Start(m_Tm);
                        //StartTime = DateTime.Now;
                        m_OldIdleRunTime = (int)m_Server.JobCond.SetupIdleRunTime; // 09.11.12 minhan
                        m_OldIdleStopTime = (int)m_Server.JobCond.SetupIdleStopTime; // 09.11.12 minhan
                        m_StartTicks = XFunc.GetTickCount();
                        SetIdleRunningProgress(ProgressAct.PROGRESS_START, StopTime);
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        nTime = (((int)GetElapsedTicks() / 1000) / 60) + m_OldTime; // 11.03.24 minhan

                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            m_StartTicks = XFunc.GetTickCount(); // 09.08.18 minhan
                            SetIdleRunningProgress(ProgressAct.PROGRESS_SET, nTime);
                        }

                        if (StopTime != m_OldIdleStopTime) // 09.08.18 minhan
                        {
                            m_OldIdleStopTime = StopTime;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_START, StopTime);
                        }

                        if (!idleCond)
                        {
                            if (!m_GenInfos.AutoMode) // 09.08.18 minhan
                            {
                                m_OldDiStatus = m_GenInfos.DiStart; // 09.08.18 minhan
                                m_GenInfos.DiStart = false;
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 40;
                                break;
                            }
                            m_GenInfos.DiStart = false;

                            SetIdleRunningProgress(ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (nTime > StopTime) // 09.08.18 minhan
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
                        nTime = (((int)GetElapsedTicks() / 1000) / 60) + m_OldTime; // 11.03.24 minhan

                        if (nTime - m_OldTime >= 1)
                        {
                            m_OldTime = nTime;
                            m_StartTicks = XFunc.GetTickCount(); // 09.08.18 minhan
                            SetIdleRunningProgress(ProgressAct.PROGRESS_SET, nTime);
                        }

                        if (RunTime != m_OldIdleRunTime) // 09.08.18 minhan
                        {
                            m_OldIdleRunTime = RunTime;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_START, RunTime);
                        }

                        if (!idleCond)
                        {
                            if (!m_GenInfos.AutoMode) // 09.08.18 minhan
                            {
                                m_OldDiStatus = m_GenInfos.DiStart; // 09.08.18 minhan
                                m_GenInfos.DiStart = false;
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 40;
                                break;
                            }
                            m_GenInfos.DiStart = false;
                            SetIdleRunningProgress(ProgressAct.PROGRESS_END, 0);
                            nSeqNo = 0;
                        }
                        else if (nTime > RunTime) // 09.08.18 minhan
                        {
                            m_GenInfos.DiStart = (StopTime == 0);
                            m_OldTime = 0; // 09.08.18 minhan
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 40: // 09.08.18 minhan
                    {
                        if (m_GenInfos.AutoMode)
                        {
                            m_GenInfos.DiStart = m_OldDiStatus; // 09.08.18 minhan
                            nSeqNo = m_ReturnSeqNo;
                            m_StartTicks = XFunc.GetTickCount();
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

    public class SeqServoStatus : XSeqFunction // 10.12.25 minhan
    //  서보 정상 상태를 알기 위해 적용함. 시퀀스에서 알람은 발생하지만, 기본적인 이유는 표시하는 것이 좋지 않을까.
    {
        #region Fields
        private GenInfoHandler m_GenInfo;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        private ServoMotor m_TrServo;
        //private ServoMotor m_LdServo;
        private ServoMotor m_UlServo;
        private ServoMotor m_Rb1UpServo;
        private ServoMotor m_Rb1LoServo;
        private ServoMotor m_Rb2UpServo;
        private ServoMotor m_Rb2LoServo;
        private Sensor m_TrServoCp;
        //private Sensor m_LdServoCp;
        private Sensor m_UlServoCp;
        private Sensor m_Rb1UpServoCp;
        private Sensor m_Rb1LoServoCp;
        private Sensor m_Rb2UpServoCp;
        private Sensor m_Rb2LoServoCp;
        //Cp
        private Alarm AlarmTrServoCpDn = new Alarm("TR Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        //private Alarm AlarmLdServoCpDn = new Alarm("LD Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmUlServoCpDn = new Alarm("UL Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        //MMC
        private Alarm AlarmTrServoMmcErr = new Alarm("TR Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        // private Alarm AlarmLdServoMmcErr = new Alarm("LD Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmUlServoMmcErr = new Alarm("UL Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        //Amp
        private Alarm AlarmTrServoAmpErr = new Alarm("TR Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        //private Alarm AlarmLdServoAmpErr = new Alarm("LD Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmUlServoAmpErr = new Alarm("UL Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);

        private Alarm AlarmRb1UpServoCpDn = new Alarm("RB1 Up Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb1LoServoCpDn = new Alarm("RB1 Lo Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2UpServoCpDn = new Alarm("RB2 Up Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2LoServoCpDn = new Alarm("RB2 Lo Servo Unit CP Down Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);

        private Alarm AlarmRb1UpServoMmcErr = new Alarm("RB1 Up Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb1LoServoMmcErr = new Alarm("RB1 Lo Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2UpServoMmcErr = new Alarm("RB2 Up Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2LoServoMmcErr = new Alarm("RB2 Lo Servo Unit MMC Error", AlarmLevel.S, AlarmCode.EquipmentSafety);

        private Alarm AlarmRb1UpServoAmpErr = new Alarm("RB1 Up Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb1LoServoAmpErr = new Alarm("RB1 Lo Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2UpServoAmpErr = new Alarm("RB2 Up Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2LoServoAmpErr = new Alarm("RB2 Lo Servo Unit AMP Error", AlarmLevel.S, AlarmCode.EquipmentSafety);

        private Alarm AlarmRb1UpParaUnmatch = new Alarm("RB1 Up Setup Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb1LoParaUnmatch = new Alarm("RB1 Lo Setup Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2UpParaUnmatch = new Alarm("RB2 Up Setup Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        private Alarm AlarmRb2LoParaUnmatch = new Alarm("RB2 Lo Setup Parameter Error", AlarmLevel.S, AlarmCode.EquipmentSafety);

        private new int[] m_AlarmId;
        private bool Rb1UpUse;
        private bool Rb1LoUse;
        private bool Rb2UpUse;
        private bool Rb2LoUse;
        private bool EqpLType;
        #endregion

        #region Constructor
        public SeqServoStatus(IServerManager server)
        {
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_GenInfo = GenInfoHandler.Instance;
            //Servo
            m_TrServo = eqpServoMotors._TR_Master_Servo_Motor;
            //m_LdServo = eqpServoMotors._LD_Hand_Servo_Motor;
            m_UlServo = eqpServoMotors._UL_Hand_Servo_Motor;
            m_Rb1UpServo = eqpServoMotors._RB_Up_Servo_Motor1;
            m_Rb1LoServo = eqpServoMotors._RB_Lo_Servo_Motor1;
            m_Rb2UpServo = eqpServoMotors._RB_Up_Servo_Motor2;
            m_Rb2LoServo = eqpServoMotors._RB_Lo_Servo_Motor2;
            //CP 
            m_TrServoCp = eqpSensors._TR_Unit_Servo_CP_Sensor;
            //m_LdServoCp = eqpSensors._LD_Hand_Unit_Servo_CP_Sensor;
            m_UlServoCp = eqpSensors._UL_Hand_Unit_Servo_CP_Sensor;
            m_Rb1UpServoCp = eqpSensors._RB1_UP_Unit_Servo_CP_Sensor; ;
            m_Rb1LoServoCp = eqpSensors._RB1_LO_Unit_Servo_CP_Sensor;
            m_Rb2UpServoCp = eqpSensors._RB2_UP_Unit_Servo_CP_Sensor;
            m_Rb2LoServoCp = eqpSensors._RB2_LO_Unit_Servo_CP_Sensor;
            m_AlarmId = new int[7];
            m_SeqFunName = "ServoStatus ";
            m_Simul = AppConfig.Instance.Simul;
        }
        #endregion

        #region Methods
        private void RbUseCheck()
        {
            if (m_Server.JobCond.RbUse(eqpRbUnits._RB_Unit_Up1)) Rb1UpUse = true;
            else Rb1UpUse = false;

            if (m_Server.JobCond.RbUse(eqpRbUnits._RB_Unit_Lo1)) Rb1LoUse = true;
            else Rb1LoUse = false;

            if (m_Server.JobCond.RbUse(eqpRbUnits._RB_Unit_Up2)) Rb2UpUse = true;
            else Rb2UpUse = false;

            if (m_Server.JobCond.RbUse(eqpRbUnits._RB_Unit_Lo2)) Rb2LoUse = true;
            else Rb2LoUse = false;
        }
        #endregion
        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            if (m_Simul.Device)
            {
                m_TrServoCp.SetState(true);
                //m_LdServoCp.DiSensor.SetState(true);
                m_UlServoCp.SetState(true);
                m_Rb1UpServoCp.SetState(true);
                m_Rb1LoServoCp.SetState(true);
                m_Rb2UpServoCp.SetState(true);
                m_Rb2LoServoCp.SetState(true);
            }

            EqpLType = m_Simul.LType;

            RbUseCheck();

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (m_AlarmId[i] > 0)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId[i]);
                        m_AlarmId[i] = 0;
                    }
                }

                if (GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = false;

                GlobalVar.Rb1UpServoErr = false;
                GlobalVar.Rb1LoServoErr = false;
                GlobalVar.Rb2UpServoErr = false;
                GlobalVar.Rb2LoServoErr = false;
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        // TR
                        if (!m_TrServoCp.IsDetected() && (m_AlarmId[0] == 0))
                        {
                            m_GenInfo.ServoStatusTR = "CP Down";
                            m_AlarmId[0] = AlarmTrServoCpDn.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[0]);
                            m_Server.Log(m_SeqFunName + " : TR Servo Cp Down");
                        }
                        else if ((m_TrServo.GetControllerError() != 0) && (m_AlarmId[0] == 0))
                        {
                            m_GenInfo.ServoStatusTR = "Error";
                            m_AlarmId[0] = AlarmTrServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[0]);
                            m_Server.Log(m_SeqFunName + " : TR Servo MMC Error");
                        }
                        else if (((m_TrServo.GetAxisSource() & AxisSource.StAmpFault) > 0) && (m_AlarmId[0] == 0))
                        {
                            m_GenInfo.ServoStatusTR = "AmpFault";
                            m_AlarmId[0] = AlarmTrServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[0]);
                            m_Server.Log(m_SeqFunName + " : TR Servo Amp Error");
                        }
                        else if (m_AlarmId[0] == 0) m_GenInfo.ServoStatusTR = "Nomal";

                        // LD
                        //if (!m_LdServoCp.IsDetected() && (m_AlarmId[1] == 0))
                        //{
                        //    m_GenInfo.ServoStatusLD = "CP Down";
                        //    m_AlarmId[1] = AlarmLdServoCpDn.Id;
                        //    m_EqpManager.SetAlarm(m_AlarmId[1]);
                        //    m_Server.Log(SeqFunName + " : LD Servo Cp Down");
                        //}
                        //else if ((m_LdServo.GetControllerError() != 0) && (m_AlarmId[1] == 0))
                        //{
                        //    m_GenInfo.ServoStatusLD = "Error";
                        //    m_AlarmId[1] = AlarmLdServoMmcErr.Id;
                        //    m_EqpManager.SetAlarm(m_AlarmId[1]);
                        //    m_Server.Log(SeqFunName + " : LD Servo MMC Error");
                        //}
                        //else if (((m_LdServo.GetAxisSource() & AxisSource.StAmpFault) > 0) && (m_AlarmId[1] == 0))
                        //{
                        //    m_GenInfo.ServoStatusLD = "AmpFault";
                        //    m_AlarmId[1] = AlarmLdServoAmpErr.Id;
                        //    m_EqpManager.SetAlarm(m_AlarmId[1]);
                        //    m_Server.Log(SeqFunName + " : LD Servo Amp Error");
                        //}
                        //else if (m_AlarmId[1] == 0) m_GenInfo.ServoStatusLD = "Nomal";

                        // UL
                        if (!m_UlServoCp.IsDetected() && (m_AlarmId[2] == 0))
                        {
                            m_GenInfo.ServoStatusUL = "CP Down";
                            m_AlarmId[2] = AlarmUlServoCpDn.Id; // 11.02.09 minhan
                            m_EqpManager.SetAlarm(m_AlarmId[2]);
                            m_Server.Log(m_SeqFunName + " : UL Servo Cp Down");
                        }
                        else if ((m_UlServo.GetControllerError() != 0) && (m_AlarmId[2] == 0))
                        {
                            m_GenInfo.ServoStatusUL = "Error";
                            m_AlarmId[2] = AlarmUlServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[2]);
                            m_Server.Log(m_SeqFunName + " : UL Servo MMC Error");
                        }
                        else if (((m_UlServo.GetAxisSource() & AxisSource.StAmpFault) > 0) && (m_AlarmId[2] == 0))
                        {
                            m_GenInfo.ServoStatusUL = "AmpFault";
                            m_AlarmId[2] = AlarmUlServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[2]);
                            m_Server.Log(m_SeqFunName + " : UL Servo Amp Error");
                        }
                        else if (m_AlarmId[2] == 0) m_GenInfo.ServoStatusUL = "Nomal";

                        // RB1 UP
                        if (!m_Rb1UpServoCp.IsDetected() &&
                            (m_AlarmId[3] == 0) &&
                            eqpRbUnits._RB_Unit_Up1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "CP Down1";
                            m_AlarmId[3] = AlarmRb1UpServoCpDn.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[3]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Up Servo Cp Down");
                        }
                        else if (!eqpRbUnits._RB_Unit_Up1.SetupServoUse.GetValue<bool>() &&
                                (m_AlarmId[3] == 0) && Rb1UpUse)
                        {
                            if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                            m_GenInfo.ServoStatusRB = "Par_Err1";
                            m_AlarmId[3] = AlarmRb1UpParaUnmatch.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[3]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Up Servo Para Error");
                        }
                        else if ((m_Rb1UpServo.GetControllerError() != 0) &&
                                (m_AlarmId[3] == 0) &&
                                 eqpRbUnits._RB_Unit_Up1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "Error";
                            m_AlarmId[3] = AlarmRb1UpServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[3]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Up Servo MMC Error");
                        }
                        else if (((m_Rb1UpServo.GetAxisSource() & AxisSource.StAmpFault) > 0) &&
                                (m_AlarmId[3] == 0) &&
                                 eqpRbUnits._RB_Unit_Up1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "AmpFault";
                            m_AlarmId[3] = AlarmRb1UpServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[3]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Up Servo Amp Error");
                        }
                        else if ((m_AlarmId[3] == 0) && Rb1UpUse)
                        {
                            if (EqpLType)
                            {
                                if (m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Up1))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err1";
                                    m_AlarmId[3] = AlarmRb1UpParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[3]);
                                    m_Server.Log(m_SeqFunName + " : Rb1 Up Servo Dir Error");
                                }
                            }
                            else
                            {
                                if (!m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Up1))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err1";
                                    m_AlarmId[3] = AlarmRb1UpParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[3]);
                                    m_Server.Log(m_SeqFunName + " : Rb1 Up Servo Dir Error");
                                }
                            }
                        }

                        // RB1 LO
                        if (!m_Rb1LoServoCp.IsDetected() &&
                            (m_AlarmId[4] == 0) &&
                            eqpRbUnits._RB_Unit_Lo1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "CP Down2";
                            m_AlarmId[4] = AlarmRb1LoServoCpDn.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[4]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo Cp Down");
                        }
                        else if (!eqpRbUnits._RB_Unit_Lo1.SetupServoUse.GetValue<bool>() &&
                                (m_AlarmId[4] == 0) && Rb1LoUse)
                        {
                            if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                            m_GenInfo.ServoStatusRB = "Par_Err2";
                            m_AlarmId[4] = AlarmRb1LoParaUnmatch.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[4]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo Para Error");
                        }
                        else if ((m_Rb1LoServo.GetControllerError() != 0) &&
                                (m_AlarmId[4] == 0) &&
                                 eqpRbUnits._RB_Unit_Lo1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "Error";
                            m_AlarmId[4] = AlarmRb1LoServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[4]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo MMC Error");
                        }
                        else if (((m_Rb1LoServo.GetAxisSource() & AxisSource.StAmpFault) > 0) &&
                                (m_AlarmId[4] == 0) &&
                                 eqpRbUnits._RB_Unit_Lo1.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb1LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb1LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "AmpFault";
                            m_AlarmId[4] = AlarmRb1LoServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[4]);
                            m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo Amp Error");
                        }
                        else if ((m_AlarmId[4] == 0) && Rb1LoUse)
                        {
                            if (EqpLType)
                            {
                                if (!m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Lo1))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err2";
                                    m_AlarmId[4] = AlarmRb1LoParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[4]);
                                    m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo Dir Error");
                                }
                            }
                            else
                            {
                                if (m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Lo1))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err2";
                                    m_AlarmId[4] = AlarmRb1LoParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[4]);
                                    m_Server.Log(m_SeqFunName + " : Rb1 Lo Servo Dir Error");
                                }
                            }
                        }

                        // RB2 UP
                        if (!m_Rb2UpServoCp.IsDetected() &&
                            (m_AlarmId[5] == 0) &&
                            eqpRbUnits._RB_Unit_Up2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "CP Down3";
                            m_AlarmId[5] = AlarmRb2UpServoCpDn.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[5]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Up Servo Cp Down");
                        }
                        else if (!eqpRbUnits._RB_Unit_Up2.SetupServoUse.GetValue<bool>() &&
                                (m_AlarmId[5] == 0) && Rb2UpUse)
                        {
                            if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                            m_GenInfo.ServoStatusRB = "Par_Err3";
                            m_AlarmId[5] = AlarmRb2UpParaUnmatch.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[5]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Up Servo Para Error");
                        }
                        else if ((m_Rb2UpServo.GetControllerError() != 0) &&
                                (m_AlarmId[5] == 0) &&
                                 eqpRbUnits._RB_Unit_Up2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "Error";
                            m_AlarmId[5] = AlarmRb2UpServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[5]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Up Servo MMC Error");
                        }
                        else if (((m_Rb2UpServo.GetAxisSource() & AxisSource.StAmpFault) > 0) &&
                                (m_AlarmId[5] == 0) &&
                                 eqpRbUnits._RB_Unit_Up2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2UpUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2UpServoErr = true;
                            m_GenInfo.ServoStatusRB = "AmpFault";
                            m_AlarmId[5] = AlarmRb2UpServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[5]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Up Servo Amp Error");
                        }
                        else if ((m_AlarmId[5] == 0) && Rb2UpUse)
                        {
                            if (EqpLType)
                            {
                                if (!m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Up2))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err3";
                                    m_AlarmId[5] = AlarmRb2UpParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[5]);
                                    m_Server.Log(m_SeqFunName + " : Rb2 Up Servo Dir Error");
                                }
                            }
                            else
                            {
                                if (m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Up2))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err3";
                                    m_AlarmId[5] = AlarmRb2UpParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[5]);
                                    m_Server.Log(m_SeqFunName + " : Rb2 Up Servo Dir Error");
                                }
                            }
                        }

                        // RB2 LO
                        if (!m_Rb2LoServoCp.IsDetected() &&
                            (m_AlarmId[6] == 0) &&
                            eqpRbUnits._RB_Unit_Lo2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "CP Down4";
                            m_AlarmId[6] = AlarmRb2LoServoCpDn.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[6]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo Cp Down");
                        }
                        else if (!eqpRbUnits._RB_Unit_Lo2.SetupServoUse.GetValue<bool>() &&
                                (m_AlarmId[6] == 0) && Rb2LoUse)
                        {
                            if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                            m_GenInfo.ServoStatusRB = "Par_Err4";
                            m_AlarmId[6] = AlarmRb2LoParaUnmatch.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[6]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo Para Error");
                        }
                        else if ((m_Rb2LoServo.GetControllerError() != 0) &&
                                (m_AlarmId[6] == 0) &&
                                 eqpRbUnits._RB_Unit_Lo2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "Error";
                            m_AlarmId[6] = AlarmRb2LoServoMmcErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[6]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo MMC Error");
                        }
                        else if (((m_Rb2LoServo.GetAxisSource() & AxisSource.StAmpFault) > 0) &&
                                (m_AlarmId[6] == 0) &&
                                 eqpRbUnits._RB_Unit_Lo2.SetupServoUse.GetValue<bool>())
                        {
                            if (Rb2LoUse) GlobalVar.RbServoParaErr = true;
                            GlobalVar.Rb2LoServoErr = true;
                            m_GenInfo.ServoStatusRB = "AmpFault";
                            m_AlarmId[6] = AlarmRb2LoServoAmpErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmId[6]);
                            m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo Amp Error");
                        }
                        else if ((m_AlarmId[6] == 0) && Rb2LoUse)
                        {
                            if (EqpLType)
                            {
                                if (m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Lo2))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err4";
                                    m_AlarmId[6] = AlarmRb2LoParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[6]);
                                    m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo Dir Error");
                                }
                            }
                            else
                            {
                                if (!m_Server.JobCond.RbDirection(eqpRbUnits._RB_Unit_Lo2))
                                {
                                    if (!GlobalVar.RbServoParaErr) GlobalVar.RbServoParaErr = true;
                                    m_GenInfo.ServoStatusRB = "Dir_Err4";
                                    m_AlarmId[6] = AlarmRb2LoParaUnmatch.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[6]);
                                    m_Server.Log(m_SeqFunName + " : Rb2 Lo Servo Dir Error");
                                }
                            }
                        }

                        if ((m_AlarmId[3] == 0) &&
                           (m_AlarmId[4] == 0) &&
                           (m_AlarmId[5] == 0) &&
                           (m_AlarmId[6] == 0))
                        {
                            m_GenInfo.ServoStatusRB = "Nomal";
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

}

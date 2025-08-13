using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using Dms.Data;
using System.Threading;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadUshioEuvControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<UshioEuvUnit> m_Units;
        protected static _GenericCollection<EuvLamp> m_Lamps;
        protected static int m_UnitCount;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_Units.Count == 0) return;

            foreach (UshioEuvUnit device in m_Units)
            {
                //RegisterSequence(new SeqInitEuv(this, device, m_Server));
                RegisterSequence(new SeqEuvReset(this, device));
                RegisterSequence(new SeqEuvAlarm(this, device));
                RegisterSequence(new SeqEuv(this, device));
                RegisterSequence(new SeqUtControl(this, device));
                RegisterSequence(new SeqEuvProgress(this, device));
                RegisterSequence(new SeqCommEuv(this, device));

                m_Server.AddSeqInitFunction(new SeqInitEuv(this, device, m_Server));

                if (m_Lamps.Count == 0) return;

                foreach (EuvLamp lamp in m_Lamps)
                {
                    RegisterSequence(new SeqTimeOver(this, lamp));
                }
            }
        }
        #endregion

        #region Constructor
        public ThreadUshioEuvControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<UshioEuvUnit>();
            m_Lamps = DmsComponents.Instance.ComponentContainer.GetCollection<EuvLamp>();
            m_UnitCount = m_Units.Count;
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
        public static _GenericCollection<UshioEuvUnit> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<UshioEuvUnit>();
                return m_Units;
            }
        }
        public static _GenericCollection<EuvLamp> Lamps
        {
            get
            {
                if (m_Lamps == null) m_Lamps = new _GenericCollection<EuvLamp>();
                return m_Lamps;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual void CheckStopCountCond()
        {
        }
        public virtual bool IsEuvUse(UshioEuvUnit euv)
        {
            bool bUse = true;

            bUse &= m_Server.JobCond.ProcessMode;
            bUse &= m_Server.JobCond.EuvUse(euv);

            bool lampSelecte = false;
            for (int i = 0; i < euv.Lamps.Count; i++)
            {
                lampSelecte |= m_Server.JobCond.EuvLampUse(euv.Lamps[i]);
            }

            bUse &= lampSelecte;

            return bUse;
        }
        public virtual bool GetEuvCond(UshioEuvUnit unit)
        {
            bool ok = false;
            return ok;
        }
        public virtual bool IsGlassExist(UshioEuvUnit unit)
        {
            bool bExist = false;

            bExist |= unit.Cv.GlsInSensor.IsDetected();
            bExist |= unit.Cv.GlsOutSensor.IsDetected();

            return bExist;
        }
        public virtual bool GetEuvUtilAlarm(UshioEuvUnit unit)
        {
            return unit.IsLevelAlarmExist();

            //bool bErr = false;
            //bErr |= unit.DiN2LevelHH.GetState();
            //bErr |= unit.DiN2LevelLL.GetState();
            //bErr |= unit.DiPCWLevelHH.GetState();
            //bErr |= unit.DiPCWLevelLL.GetState();
            //bErr |= unit.DiCDALevelHH.GetState();
            //bErr |= unit.DiCDALevelLL.GetState();
            //return bErr;
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqInitEuv : XSeqInitFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadUshioEuvControl m_Control;
        protected UshioEuvUnit m_Euv;
        protected static _GenInfoHandler m_GenInfos;
        protected InitState m_InitState = InitState.Noop;

        protected GenericTag m_InitCheckEuvReady = new GenericTag("EUV Ready", InitCheckState.NotReady);
        protected GenericTag m_InitCheckEuvHumReady = new GenericTag("EUV Hum Ready", InitCheckState.NotReady);
        protected GenericTag m_InitCheckEuvRemote = new GenericTag("EUV Remote", InitCheckState.NotReady);
        protected GenericTag m_InitCheckEuvClose = new GenericTag("EUV House Close", InitCheckState.NotReady);

        protected static Simul m_Simul;
        private bool m_bEuvOk = false;
        private bool m_bInitialCountStart = false;
        #endregion

        #region Constructor
        public SeqInitEuv(ThreadUshioEuvControl control, UshioEuvUnit Euv, IServerManager server)
        {
            m_Euv = Euv;
            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_Simul = AppConfig.Instance.Simul;

            this.m_SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;
            //if (!m_Control.IsEuvUse(m_Euv)) return (int)InitState.Comp;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;

                        nSeqNo = 10;

                        // Move to UshioEuvUnit.Initialize()
                        //if (m_Simul.Device)
                        //{
                        //    m_Euv.Cylinder.SetAct(ActuatorAct.Neg);
                        //    m_Euv.DiHouseClose.SetState(true);
                        //    m_Euv.DiUNRE.SetState(true);
                        //    m_Euv.DiURDY.SetState(true);
                        //}
                    }
                    break;

                case 10:
                    {
                        if (m_Control.IsEuvUse(m_Euv))
                        {
                            m_Euv.Utcontrol(AutoValveAct.Open);
                            m_Server.Log(m_Euv.Name + " : Ut Turn On");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Server.Log(m_Euv.Name + " : No Use");
                            m_InitCheckEuvReady.Value = InitCheckState.NoUse;
                            m_InitCheckEuvHumReady.Value = InitCheckState.NoUse;
                            m_InitCheckEuvRemote.Value = InitCheckState.NoUse;
                            m_InitCheckEuvClose.Value = InitCheckState.Checking;
                            nSeqNo = 70;
                        }
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Euv.LampEmergency(true);
                        m_Server.Log(m_Euv.Name + " : UEMG ON");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (GetElapsedTicks() > 3000)
                    {
                        m_Server.Log(m_Euv.Name + " : Reset Request : m_bEuvResetReq <T>");
                        m_Euv.IfFlag.bEuvResetReq = true;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 40;

                        m_InitCheckEuvReady.Value = InitCheckState.Checking;
                        if (m_Euv.HasHumidifier)
                        {
                            m_InitCheckEuvHumReady.Value = InitCheckState.Checking;
                        }
                        else
                        {
                            m_InitCheckEuvHumReady.Value = InitCheckState.NoUse;
                        }
                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 3000)
                    {
                        if (m_Euv.GetEuvAlarm() == 0)
                        {
                            if (m_Euv.GetHumAlarm() == 0)
                            {
                                m_Server.Log(m_Euv.Name + " : No Alarm");
                                m_InitCheckEuvReady.Value = InitCheckState.OK;
                                m_InitCheckEuvHumReady.Value = InitCheckState.OK;
                                m_InitCheckEuvRemote.Value = InitCheckState.Checking;
                                nSeqNo = 50;
                            }
                            else
                            {
                                m_AlarmId = m_Euv.ALM_HUMIDIFIERnotready.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Server.Log(m_Euv.Name + " : Hum error");
                                m_InitState = InitState.Fail;
                                m_InitCheckEuvHumReady.Value = InitCheckState.NG;
                                nSeqNo = 1000;
                            }
                        }
                        else
                        {
                            m_AlarmId = m_Euv.ALM_EUVnotready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.Log(m_Euv.Name + " : Euv error");
                            m_InitState = InitState.Fail;
                            m_InitCheckEuvReady.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 50:
                    if (m_Euv.IsRemoteMode())
                    {
                        m_Server.Log(m_Euv.Name + " : Confirm Remote Mode : OK");
                        m_Euv.LampLocalLock(true);
                        m_InitCheckEuvRemote.Value = InitCheckState.OK;

                        nSeqNo = 60;
                    }
                    else
                    {
                        m_AlarmId = m_Euv.ALM_EUVmanual.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Server.Log(m_Euv.Name + " : Local mode");
                        m_InitState = InitState.Fail;
                        m_InitCheckEuvRemote.Value = InitCheckState.NG;
                        nSeqNo = 1000;
                    }

                    break;
                case 60:
                    if (m_Euv.IsHumReady() && m_Euv.IsEuvReady() && !m_Euv.GetInitialCount())
                    {
                        m_Server.Log(m_Euv.Name + " : Confirm EUV and Hum Ready : OK");
                        nSeqNo = 70;
                        m_InitCheckEuvClose.Value = InitCheckState.Checking;
                    }
                    else if (!m_bEuvOk && m_Euv.GetInitialCount())
                    {
                        m_bEuvOk = true;
                        string sMsg = "Do you want to keep going EUV House N2 Purge?\n It task around 15minutes.";
                        if (MessageBox.Show(sMsg, "WSSD", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            m_Euv.SetInitialCount(true);
                            m_Server.Log(m_Euv.Name + " : Initial Counter ON");
                            m_bInitialCountStart = true;
                            m_StartTicks = XFunc.GetTickCount();
                        }
                    }
                    if (m_bInitialCountStart)
                    {
                        if (GetElapsedTicks() > 1000)
                        {
                            m_bInitialCountStart = false;
                            m_Euv.SetInitialCount(false);
                            m_Server.Log(m_Euv.Name + " : Initial Counter OFF");
                        }
                    }
                    break;
                case 70:
                    if (m_Euv.GetHouseCloseState())
                    {
                        m_Server.Log(m_Euv.Name + " : House Close Confirm");
                        m_InitCheckEuvClose.Value = InitCheckState.OK;
                        nSeqNo = 100;
                    }
                    else
                    {
                        m_AlarmId = m_Euv.ALM_EUVhouseclose.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Server.Log(m_Euv.Name + " : House not Close");
                        m_InitState = InitState.Fail;
                        m_InitCheckEuvClose.Value = InitCheckState.NG;
                        nSeqNo = 1000;
                    }
                    break;
                case 100:
                    {
                        m_Server.Log(m_Euv.Name + " : Init comp");
                        m_bEuvOk = false;
                        m_bInitialCountStart = false;
                        m_InitState = InitState.Comp;
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        nSeqNo = 1010;
                    }
                    break;
                case 1010:
                    if (!m_GenInfos.EqpInitReq)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqEuv : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadUshioEuvControl m_Control;
        private UshioEuvUnit m_Euv;
        private bool bInit = false;
        private int nOldControl;
        private bool bManualModeChangeComp;
        #endregion

        #region Constructor
        public SeqEuv(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            bInit = false;
            this.m_SeqFunName = m_Euv.Name;
            m_StartTicks = XFunc.GetTickCount();
            nOldControl = (int)euvLAMP_CONTROL.euvLAMP_OFF;
            bManualModeChangeComp = false;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!bInit)
            {
                if (GetElapsedTicks() > 2000) bInit = true;
                if (!bInit) return -1;
            }

            m_Euv.IfFlag.nLampControl = SetLampControl();

            switch (m_Euv.IfFlag.nLampControl)
            {
                case (int)euvLAMP_CONTROL.euvLAMP_MANUAL:
                    break;
                case (int)euvLAMP_CONTROL.euvLAMP_PAUSE:
                case (int)euvLAMP_CONTROL.euvLAMP_OFF:
                    if (m_Euv.GetLampOnState() == true)
                    {
                        m_Euv.LampControl(false);
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "Lamp OFF");
                    }
                    break;
                case (int)euvLAMP_CONTROL.euvLAMP_ON:
                case (int)euvLAMP_CONTROL.euvLAMP_ON_KEEP:
                    if (m_Euv.GetLampOnState() == false)
                    {
                        m_Euv.LampOnSelect();
                        m_Euv.LampControl(true);
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "Lamp ON");
                    }
                    break;
            }

            return -1;
        }
        #endregion

        #region Method
        public int SetLampControl()	//For SeqEuv()
        {
            int nControl = (int)euvLAMP_CONTROL.euvLAMP_OFF;
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool bRunCond = true;
            bRunCond &= !((heavy & ~HeavyInterlock.Emo) > 0);
            bRunCond &= !((heavy & ~HeavyInterlock.Leak) > 0);
            bRunCond &= !((heavy & ~HeavyInterlock.Cover) > 0);

            bool bEuvUse = m_Control.IsEuvUse(m_Euv);
            bool bEuvAlarm = m_Euv.GetEuvAlarm() > 0;
            // bEuvAlarm = m_pDicsData->m_IfEuv.bEuvUtilAlarm;//나중에 필요하면 추가하던지. kimgun
            bool bGlassExist = m_Control.IsGlassExist(m_Euv);
            bool bCvRun = (m_Euv.Cv.AutoAct == CvMotorAct.Fw);//->m_CvMotor.GetMotorFwState(IF_UNIT_EUV);

            bool bHumReady = m_Euv.IsHumReady();
            bool bEuvReady = m_Euv.IsEuvReady();

            bool bEuvRun = true;
            bEuvRun &= bRunCond;
            bEuvRun &= bEuvUse;
            bEuvRun &= !bEuvAlarm;
            bEuvRun &= bGlassExist;
            bEuvRun &= bCvRun;
            //	bEuvRun &= bHumReady;no use
            bEuvRun &= bEuvReady;

            if (GenInfoHandler.Instance.AutoMode)
            {
                bManualModeChangeComp = false;

                if (bEuvRun)
                {
                    if (nOldControl == (int)euvLAMP_CONTROL.euvLAMP_OFF)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_ON;
                    }
                    else if (nOldControl >= (int)euvLAMP_CONTROL.euvLAMP_PAUSE)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_ON_KEEP;
                    }
                }
                else
                {
                    if (!bGlassExist)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_OFF;
                    }
                    else if (nOldControl >= (int)euvLAMP_CONTROL.euvLAMP_PAUSE)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_PAUSE;
                    }
                }
            }
            else
            {
                if (!bRunCond || !bManualModeChangeComp)
                {
                    bManualModeChangeComp = true;

                    if (!bGlassExist)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_OFF;
                    }
                    else if (nOldControl >= (int)euvLAMP_CONTROL.euvLAMP_PAUSE)
                    {
                        nControl = (int)euvLAMP_CONTROL.euvLAMP_PAUSE;
                    }
                }
                else
                {
                    nControl = (int)euvLAMP_CONTROL.euvLAMP_MANUAL;
                }
            }

            if (nControl != (int)euvLAMP_CONTROL.euvLAMP_MANUAL) nOldControl = nControl;

            return nControl;
        }
        #endregion
    }

    public class SeqEuvReset : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadUshioEuvControl m_Control;
        private UshioEuvUnit m_Euv;
        public static int m_nCount;
        #endregion

        #region Constructor
        public SeqEuvReset(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            this.m_SeqFunName = "EuvReset    "; ;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Euv.GetUtState() && m_Euv.IfFlag.bEuvResetReq)
                    {
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "UERT <ON>");

                        m_Euv.EmoReset(true);

                        m_Euv.IfFlag.bEuvResetComp = false;
                        m_Euv.IfFlag.bEuvResetReq = false;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 500)
                    {
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "HUMIDIFIER_UHER <ON>");

                        m_Euv.HumidifierReset(true);

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 500)
                    {
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "UERT <OFF>, HUMIDIFIER_UHER <OFF>");

                        m_Euv.EmoReset(false);
                        m_Euv.HumidifierReset(false);
                        m_Euv.IfFlag.bEuvResetComp = true;
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEuvAlarm : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadUshioEuvControl m_Control;
        private UshioEuvUnit m_Euv;
        private new int[] m_AlarmId;
        public int nAlarmCode = 0;
        public static int m_nMaxalarmNo;
        #endregion

        #region Constructor
        public SeqEuvAlarm(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_nMaxalarmNo = Enum.GetNames(typeof(euvalarmIndex)).Length;
            m_AlarmId = new int[m_nMaxalarmNo + 1];
            this.m_SeqFunName = m_Euv.Name + " Alarm";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if ((nAlarmCode = m_Euv.GetEuvAlarm()) == 0 && m_Euv.GetHouseCloseState())
                    {
                        // No Operation
                    }
                    else if (m_Control.IsEuvUse(m_Euv))
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_Control.IsEuvUse(m_Euv) ||
                      ((nAlarmCode = m_Euv.GetEuvAlarm()) == 0 && m_Euv.GetHouseCloseState()))
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 1500)
                    {
                        m_Euv.LampEmergency(false);

                        // Parsing EUV Alarm Code
                        for (int i = 0; i < m_nMaxalarmNo; i++)
                        {
                            if ((nAlarmCode & (0x0001 << i)) == 1)
                            {
                                m_AlarmId[i] = m_Euv.ALM_EUVlamptimeover.Id + i;
                                m_EqpManager.SetAlarm(m_AlarmId[i]);

                            }
                        }
                        if (!GenInfoHandler.Instance.AutoMode && (nAlarmCode == 0)) nSeqNo = 0;
                        else
                        {
                            if (!m_Euv.GetHouseCloseState())
                            {
                                m_AlarmId[m_nMaxalarmNo] = m_Euv.ALM_EUVhouseclose.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[m_nMaxalarmNo]);
                                m_Euv.SetLog("ALARM   ", 0, 0, "EUV Not Close Alarm");
                            }
                            // Log
                            m_Euv.SetLog(m_SeqFunName, 0, 0, "Alarm Set");

                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed &&
                        (GetElapsedTicks() > 1000))
                    {
                        m_Euv.LampEmergency(true);
                        m_Euv.SetLog(m_SeqFunName, 0, 0, "EUV Alarm : LampEmergency( ON )");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1010;
                    }
                    else if ((m_Euv.GetEuvAlarm() == 0) && (m_Euv.GetHumAlarm() == 0)) nSeqNo = 1030;
                    break;

                case 1010:
                    if ((m_Euv.GetHumAlarm() == 0) && m_Euv.IsEuvEMO() &&
                         (GetElapsedTicks() > 1000))
                    {
                        m_Euv.IfFlag.bEuvResetReq = true;
                        m_Euv.SetLog("ALARM   ", 0, 0, "EUV Alarm : m_bEuvResetReq <T>");

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1020;
                    }
                    break;

                case 1020:
                    if (m_Euv.IfFlag.bEuvResetComp && (GetElapsedTicks() > 3000))
                    {
                        if ((m_Euv.GetEuvAlarm() > 0) && m_Control.IsEuvUse(m_Euv))
                        {
                            m_Euv.LampEmergency(false);
                            m_Euv.SetLog("ALARM   ", 0, 0, "EUV Alarm : remain");

                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1000;
                        }
                        else nSeqNo = 1030;
                    }
                    break;

                case 1030:
                    if (m_Euv.GetEuvAlarm() == 0)
                    {
                        for (int i = 0; i < m_nMaxalarmNo; i++)
                        {
                            if (m_AlarmId[i] != 0)
                            {	// Reset Alarm
                                m_EqpManager.ResetAlarm(m_AlarmId[i]);
                                m_AlarmId[i] = 0;
                            }
                        }
                        // Log
                        m_Euv.SetLog("ALARM   ", 0, 0, "EUV Alarm Recovery");
                        nSeqNo = 0;
                    }
                    else
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1000;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqUtControl : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static ThreadUshioEuvControl m_Control;
        private UshioEuvUnit m_Euv;
        private bool m_bStMaint = false;
        private bool m_bStAlarm = false;
        #endregion

        #region Constructor
        public SeqUtControl(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            this.m_SeqFunName = m_Euv + " UtControl";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;
            if (!GenInfoHandler.Instance.AutoMode) return -1;

            if (m_Control.IsEuvUse(m_Euv))
            {
                if ((m_Euv.GetEuvAlarm() == 0) &&
                    !m_Euv.GetUtState() &&
                     !m_Euv.IsN2LevelHHSensed())                //!m_Euv.DiN2LevelHH.GetState())
                {
                    m_Euv.Utcontrol(AutoValveAct.Open);
                    m_Euv.LampEmergency(true);
                    // limjy_071114 : Add Euv Reset
                    m_Euv.IfFlag.bEuvResetReq = true;
                }
                else if (m_bStMaint && m_bStAlarm &&
                 !m_Euv.GetUtState() &&
                 !m_Euv.IsN2LevelHHSensed())                //!m_Euv.DiN2LevelHH.GetState())
                {
                    m_Euv.Utcontrol(AutoValveAct.Open);
                }
                if (m_bStAlarm && m_Euv.GetUtState() && (m_Euv.GetEuvAlarm() == 0)) m_bStAlarm = false;
                if (m_bStMaint) m_bStMaint = false;

            }
            // Auto -> Maint mode 전환할 때 UtControl( OFF ) 한다.
            else
            {
                if (!m_bStMaint && m_Euv.GetUtState())
                {
                    m_Euv.Utcontrol(AutoValveAct.Close);
                }

                if (!m_bStMaint || (m_bStAlarm && !m_Euv.GetUtState()))
                {
                    if (!GenInfoHandler.Instance.AutoMode) m_bStMaint = true;
                }
            }

            // Alarm이 최초 발생 했을때만 UtControl( OFF ) 한다.
            if (!m_bStAlarm && (m_Euv.GetEuvAlarm() > 0))
            {
                if (!m_bStAlarm && m_Euv.GetUtState())
                {
                    m_bStAlarm = true;
                    m_Euv.Utcontrol(AutoValveAct.Close);
                }
            }

            return -1;
        }
        #endregion
    }

    public class SeqTimeOver : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static ThreadUshioEuvControl m_Control;
        private EuvLamp m_Lamp;
        #endregion

        #region Constructor
        public SeqTimeOver(ThreadUshioEuvControl control, EuvLamp lamp)
        {
            m_Lamp = lamp;
            m_Server = m_Lamp.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Control = control;
            this.m_SeqFunName = m_Lamp.Name + " TimeOver";
        }
        #endregion 

        #region Sequence
        public override int Do()
        {
            int nSeqNo = this.m_SeqNo;
            int timeOver = m_Lamp.SetupUsedTime.GetValue<int>();

            switch (nSeqNo)
            {
                case 0:
                    if (m_Lamp.GetLampUsedTime() > timeOver)
                    {
                        m_AlarmId = m_Lamp.ALM_UsedTimeOver.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Lamp.SetLog(m_SeqFunName, 0, 0, "Time Over Alarm : Set");
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        if (m_Lamp.GetLampUsedTime() <= timeOver)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_Lamp.SetLog(m_SeqFunName, 0, 0, "Time Over Alarm : Reset");
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

    public class SeqEuvProgress : XSeqFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        private UshioEuvUnit m_Euv;
        private XTimer m_EuvPogressTimer;
        protected static ThreadUshioEuvControl m_Control;
        static euvLAMP_CONTROL nOldControl;
        static uint nOldTime = 0;
        protected uint m_ProgressTime;
        private bool m_NextGlass = false;
        #endregion

        #region Constructor
        public SeqEuvProgress(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            this.m_SeqFunName = "EuvProgress    ";
            m_EuvPogressTimer = new XTimer("Timer " + m_SeqFunName);

            // ALM_InitFail = new Alarm("EuvUnit" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;
            uint nTime;
            int distance = m_Euv.Cv.SetupDistance.GetValue<int>();
            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
            uint nEuvOnTime = (uint)((distance + glassSize)
                                / (m_Euv.Cv.AutoSpeed / 60) * 1000);

            euvLAMP_CONTROL nControl = GetLampControl();
            bool nextGlassEnter = m_Euv.Cv.GlsInSensor.IsDetected() &&
                                  m_Euv.Cv.PrevCv.MotorControl.IsFw(Logic.OR) &&
                                  m_Server.GlassData.IsExist(m_Euv.Cv.Id * 2 + 0);

            int nSeqNo = this.m_SeqNo;
            switch (nControl)
            {
                case euvLAMP_CONTROL.euvLAMP_ON:
                    if (nOldControl != euvLAMP_CONTROL.euvLAMP_ON)
                    {
                        // Progress starts
                        SetEuvRunProgress(ProgressAct.PROGRESS_START, nEuvOnTime / 1000);
                        m_EuvPogressTimer.Start((int)nEuvOnTime);


                        nOldTime = 0;
                    }
                    break;
                case euvLAMP_CONTROL.euvLAMP_OFF:
                    if (nOldControl != euvLAMP_CONTROL.euvLAMP_OFF)
                    {	// Progress OFF                        
                        SetEuvRunProgress(ProgressAct.PROGRESS_END, nEuvOnTime / 1000);
                        m_NextGlass = false;
                        nOldTime = 0;
                    }
                    break;
                case euvLAMP_CONTROL.euvLAMP_ON_KEEP:
                    if (m_EuvPogressTimer.Over)
                    {	// Progress ++->
                        nOldTime = 0;
                    }
                    else if (m_EuvPogressTimer.IsPaused)
                    {
                        m_EuvPogressTimer.Resume();
                    }

                    nTime = m_EuvPogressTimer.CurElapsedTickCounts / 1000;
                    if (nTime - nOldTime >= 1)
                    {
                        nOldTime = nTime;
                        SetEuvRunProgress(ProgressAct.PROGRESS_SET, nTime);
                    }
                    //When glass enters before progress end..
                    if (!m_NextGlass && m_Euv.Cv.GlsOutSensor.IsDetected() && m_Server.GlassData.IsExist(m_Euv.Cv.Id * 2 + 1))
                    {
                        m_NextGlass = true;
                    }
                    if (m_NextGlass && nextGlassEnter)
                    {
                        m_NextGlass = false;
                        SetEuvRunProgress(ProgressAct.PROGRESS_START, nEuvOnTime / 1000);
                        m_EuvPogressTimer.Start((int)nEuvOnTime);

                        nOldTime = 0;
                    }
                    break;
                case euvLAMP_CONTROL.euvLAMP_MANUAL:
                case euvLAMP_CONTROL.euvLAMP_PAUSE:
                    if (!m_EuvPogressTimer.IsPaused)
                    {
                        m_EuvPogressTimer.Pause();
                    }
                    break;
            }

            if (nControl != euvLAMP_CONTROL.euvLAMP_MANUAL) nOldControl = nControl;


            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
        #region Methods
        public euvLAMP_CONTROL GetLampControl()
        {
            return (euvLAMP_CONTROL)m_Euv.IfFlag.nLampControl;
        }
        public void SetEuvRunProgress(ProgressAct act, uint time)
        {
            switch (act)
            {
                case ProgressAct.PROGRESS_START:
                    {
                        m_ProgressTime = time;
                        GenInfoHandler.Instance.EuvLampOnProgress = m_ProgressTime.ToString();
                    }
                    break;
                case ProgressAct.PROGRESS_END:
                    {
                        GenInfoHandler.Instance.EuvLampOnProgress = "0";
                    }
                    break;
                case ProgressAct.PROGRESS_SET:
                    {
                        string val = string.Format("{0} / {1}", time, m_ProgressTime);
                        GenInfoHandler.Instance.EuvLampOnProgress = val;
                    }
                    break;
            }
        }
        #endregion
    }

    public class SeqCommEuv : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        private UshioEuvUnit m_Euv;
        //private EuvUnitComm m_EuvComm;
        private XTimer m_Timer;
        protected static string sCommand = "";
        protected static int nLampNo = 0;
        protected static int nCount = 0;
        protected static int nDelay = 0;
        protected static int nReturnSeqNo;
        protected static int nLampCount = 0;
        protected static bool bDatarecieved = false;
        enum ComJurdge { _INTENSITY = 100, _LIFE_TIME = 200 };
        protected static ThreadUshioEuvControl m_Control;

        #endregion

        #region Constructor
        public SeqCommEuv(ThreadUshioEuvControl control, UshioEuvUnit euv)
        {
            m_Euv = euv;
            m_Server = m_Euv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            nLampCount = m_Euv.Lamps.Count;
            this.m_SeqFunName = "EuvComm    ";
            m_Timer = new XTimer("Timer " + m_SeqFunName);
            m_Euv.Comm.DataReceived += new EuvUnitComm.GetEuvReceivedData(DataReceived);
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;
            if (!m_Control.IsEuvUse(m_Euv)) return -1;
            if (m_Simul.Device) return -1;
            int nSeqNo = this.m_SeqNo;
            int nBCC = 0;
            string sRecvData = "";
            switch (nSeqNo)
            {
                case 0:
                    {
                        bool bCheckIntensity = false;
                        bCheckIntensity |= m_Euv.GetLampOnState();
                        //bCheckIntensity |= m_Euv.DoULON.GetState();
                        bCheckIntensity |= m_Control.IsGlassExist(m_Euv);
                        bCheckIntensity |= (m_Euv.Cv.PrevCv.AutoAct == CvMotorAct.Fw);

                        if (bCheckIntensity ||
                            (nDelay > 0))
                        {
                            if (bCheckIntensity) nDelay = 10;
                            else nDelay--;

                            sCommand = string.Format("LA0D");
                        }
                        else
                        {
                            nDelay = 0;
                            if (nLampNo >= nLampCount) nLampNo = 1;
                            else nLampNo++;

                            string sData = "";
                            sData = string.Format("RT%02d0001", nLampNo);
                            nBCC = GetBCC(sData);

                            sCommand = string.Format("RT%02d0001%02X", nLampNo, nBCC);
                        }
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_Euv.Comm.SendCommand(sCommand))
                    {
                        m_StartTicks = XFunc.GetTickCount();

                        if (sCommand == "LA0D") nSeqNo = (int)ComJurdge._INTENSITY;
                        else nSeqNo = (int)ComJurdge._LIFE_TIME;
                    }
                    break;

                case (int)ComJurdge._INTENSITY:
                    if (bDatarecieved)
                    {
                        sRecvData = m_Euv.Comm.CurData;
                        bDatarecieved = false;
                        if (sRecvData.Length == 32)
                        {
                            if (sRecvData.Substring(1, 2) == "LA")
                            {
                                if (sRecvData.Substring(3, 2) == "00")
                                {
                                    for (int i = 0; i < nLampCount; i++)
                                    {
                                        // if( i == 0)
                                        m_Euv.Lamps[i].LampIntensity = double.Parse(sRecvData.Substring(5 + (i * 3), 3));
                                        // m_Euv.Lamp1.nLampIntensity = double.Parse(sRecvData.Substring(5 + (i * 3), 3));
                                        //  else
                                        // m_Euv.Lamp2.nLampIntensity = double.Parse(sRecvData.Substring(5 + (i * 3), 3));
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < nLampCount; i++)
                                    {
                                        m_Euv.Lamps[i].LampIntensity = 0;
                                        //                                         if (i == 0)
                                        //                                             m_Euv.Lamp1.nLampIntensity = 0;
                                        //                                         else m_Euv.Lamp2.nLampIntensity = 0;
                                    }
                                }
                            }
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 300;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        nReturnSeqNo = 0;
                        nSeqNo = 1000;
                    }
                    break;

                case (int)ComJurdge._LIFE_TIME:
                    if (bDatarecieved)
                    {
                        sRecvData = m_Euv.Comm.CurData;
                        bDatarecieved = false;
                        if (sRecvData.Length == 12)
                        {
                            if (sRecvData.Substring(1, 2) == "RT")
                            {
                                int nIndex = nLampNo - 1;

                                if (sRecvData.Substring(3, 2) == "00")
                                {
                                    m_Euv.Lamps[nIndex].LampOnTime = uint.Parse(sRecvData.Substring(5, 4));
                                    //                                     if (nIndex == 0)
                                    //                                         m_Euv.Lamp1.nLampOnTime = uint.Parse(sRecvData.Substring(5, 4));
                                    //                                     else
                                    //                                         m_Euv.Lamp2.nLampOnTime = uint.Parse(sRecvData.Substring(5, 4));
                                }
                                else
                                {
                                    m_Euv.Lamps[nIndex].LampOnTime = 0;
                                    //                                     if (nIndex == 0)
                                    //                                         m_Euv.Lamp1.nLampOnTime = 0;
                                    //                                     else
                                    //                                         m_Euv.Lamp2.nLampOnTime = 0;
                                }
                            }
                        }

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 300;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        nReturnSeqNo = 0;
                        nSeqNo = 1000;
                    }
                    break;

                case 300:
                    if (GetElapsedTicks() > 1000)
                    {
                        nSeqNo = 0;
                    }
                    break;

                case 1000:
                    {
                        nCount++;
                        if (nCount >= 4)
                        {
                            nCount = 0;
                            //SET ALARM
                            m_AlarmId = m_Euv.ALM_EUVcommopenerror.Id;

                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1010;
                        }
                        else
                        {
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 1010:
                    if (m_EqpManager.AlarmResetSwitchPushed &&
                    (GetElapsedTicks() > 1000))
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        nSeqNo = nReturnSeqNo;
                    }
                    break;

            }
            this.m_SeqNo = nSeqNo;
            return -1;
        }
        #endregion

        #region Method
        public int GetBCC(string sData)
        {
            int nBCC = 0;
            int nVal = 0;
            int nSize = sData.Length;

            for (int i = 0; i < nSize; i++)
            {
                //  string sBuf = sData.Substring(i, 1);
                nVal = Convert.ToChar(sData.Substring(i, 1));
                if (i == 0)
                {
                    nBCC = nVal;
                }
                else
                {
                    nBCC ^= nVal;
                }
            }

            return nBCC;
        }
        private void DataReceived(object sender)
        {
            bDatarecieved = true;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadCvControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<CvUnit> m_CvUnits;
        protected static _GenericCollection<ActuatorUnit> m_ActuatorUnits;
        protected static int m_UnitCount;
        protected static GenInfoHandler m_GenInfos;

        public Alarm AlarmTrRobotInterlock = new Alarm("Tr Unit Robot Interlock Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);   //  bm
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ThreadCvControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>(Compatibility.Compatible);
            m_ActuatorUnits = DmsComponents.Instance.ComponentContainer.GetCollection<ActuatorUnit>();
            m_UnitCount = m_CvUnits.Count;
            m_GenInfos = GenInfoHandler.Instance;

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_CvUnits.Count == 0) return;

            foreach (CvUnit device in m_CvUnits)
            {
                RegisterSequence(new SeqCvMotor(this, device));
                RegisterSequence(new SeqCvMotorCondition(this, device));
                RegisterSequence(new SeqCvMotorSpeedControl(this, device));
            }

            m_Server.AddSeqInitFunction(new SeqInitCv(this, m_Server));
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
        public static _GenericCollection<CvUnit> Units
        {
            get
            {
                if (m_CvUnits == null) m_CvUnits = new _GenericCollection<CvUnit>();
                return m_CvUnits;
            }
        }
        #endregion   

        #region Virtual Methods
        public virtual void CheckStopCountCond()
        {
            bool alarm = false;
            foreach (CvUnit unit in m_CvUnits)
            {
                alarm |= unit.IfFlag.InError;
                alarm |= unit.IfFlag.OutError;
            }
            alarm |= BaseGlobalVar.UlTimeOut;// m_Server.SeqFlag.UlTimeOut;

            if (alarm) BaseGlobalVar.StopCount1 = true;// m_Server.SeqFlag.StopCount1 = true;
            else BaseGlobalVar.StopCount1 = false;// m_Server.SeqFlag.StopCount1 = false;
        }

        public virtual bool IsRunCondition(CvUnit cv)
        {
            bool run = false;

            run |= cv.IfFlag.InReady;
            run |= cv.IfFlag.OutReady;
            run |= !cv.IfFlag.OutComp;

            if (cv.PrevCv == null)
            {
                //run |= GlsInSensor.IsDetected();
            }
            else
            {
                run |= cv.PrevCv.IfFlag.OutReady;
                run |= !cv.PrevCv.IfFlag.OutComp;
                run |= m_Server.GlassData.IsExist(cv.Id * 2 + 0);
            }

            if (cv.NextCv != null)
            {
                run |= !cv.NextCv.IfFlag.InComp;
            }
            else
            {
                if (cv.IfFlag.OutReady && !cv.IfFlag.OutComp) run = false;
            }

            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.AutoMode;

            return run;
        }

        public virtual bool IsAlarmCondition(CvUnit cv)
        {
            bool ng = false;
            ng |= CvStopCondition(cv);
            return ng;
        }

        public virtual bool IsRecoveryReq(CvUnit cv)
        {
            return CvRecoveryRequest(cv);
        }

        public virtual bool IsInterlock(CvUnit cv)
        {
            bool interlock = false;
            interlock |= (m_Server.JobCond.HeavyInterlock > 0);
            return interlock;
        }

        public virtual bool IsSeqRunCondition(CvUnit cv)
        {
            bool run = true;
            run &= !IsInterlock(cv);
            run &= !CvStopCondition(cv.NextCv);
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.AutoMode;
            return run;
        }

        public virtual CvMotorAct GetRefMotorAct(CvUnit cv, int id)
        {
            bool interlock = IsInterlock(cv);
            bool run = IsRunCondition(cv);
            bool alarm = IsAlarmCondition(cv);
            bool recoveryReq = IsRecoveryReq(cv);
            bool autoMode = m_GenInfos.AutoMode;

            if (interlock)
            {
                cv.AutoAct = CvMotorAct.Stop;
                cv.ManualAct[id] = CvMotorAct.Stop;

                return CvMotorAct.Stop;
            }
            else if (run && (recoveryReq || !alarm))
            {
                cv.AutoAct = CvMotorAct.Fw;
                cv.ManualAct[id] = CvMotorAct.Stop;

                return cv.AutoAct;
            }
            else if (!autoMode)
            {
                cv.AutoAct = CvMotorAct.Stop;

                return cv.ManualAct[id];
            }
            else
            {
                cv.ManualAct[id] = CvMotorAct.Stop;
                cv.AutoAct = CvMotorAct.Stop;

                return CvMotorAct.Stop;
            }
        }

        public virtual int GetRefMotorSpeed(CvUnit cv, int id)
        {
            if (m_GenInfos.AutoMode)
            {
                cv.ManualSpeed[id] = 0;

                // TODO : How to set accelation speed or decelation speed
                return m_Server.JobCond.CvProcessSpeed(cv.Id);
            }
            else
            {
                return cv.ManualSpeed[id];
            }
        }

        public virtual void GetTimeOutParameter(CvUnit cv, int distance, int glassSize, ref int tm1, ref int tm2, ref int tm3)
        {
            int cvLength = distance;
            double cvSpeed = (double)(m_Server.JobCond.CvProcessSpeed(cv.Id)) / 60.0; // mm/min -> mm/sec
            int marginOnLength = cv.SetupTimeoutMargin.GetValue<int>();
            int marginOffLength = cv.SetupTimeoutMargin.GetValue<int>();
            double marginOnTime = (double)marginOnLength / cvSpeed;
            double marginOffTime = (double)marginOffLength / cvSpeed;

            if (!AppConfig.Instance.Simul.Device)
            {
                tm1 = (int)(((cvLength / cvSpeed) + marginOnTime) * 1000);
                tm2 = (int)(((glassSize / cvSpeed) + marginOffTime) * 1000);
                tm3 = (int)(((cvLength / cvSpeed) - marginOnTime) * 1000);
            }
            else
            {
                tm1 = (int)(((cvLength / cvSpeed)) * 1000);
                tm2 = (int)(((glassSize / cvSpeed)) * 1000);
                tm3 = (int)Math.Abs(((cvLength / cvSpeed) - marginOnTime) * 1000);
            }
        }

        public virtual void GetTimeOutParameter(CvUnit cv, int distance, int glassSize, int speed, ref int tm1, ref int tm2, ref int tm3)
        {
            int cvLength = distance;
            double cvSpeed = (double)speed / 60.0; // mm/min -> mm/sec
            int marginOnLength = cv.SetupTimeoutMargin.GetValue<int>();
            int marginOffLength = cv.SetupTimeoutMargin.GetValue<int>();
            double marginOnTime = (double)marginOnLength / cvSpeed;
            double marginOffTime = (double)marginOffLength / cvSpeed;

            if (!AppConfig.Instance.Simul.Device)
            {
                tm1 = (int)(((cvLength / cvSpeed) + marginOnTime) * 1000);
                tm2 = (int)(((glassSize / cvSpeed) + marginOffTime) * 1000);
                tm3 = (int)(((cvLength / cvSpeed) - marginOnTime) * 1000);
                tm1 = tm1 < 0 ? 0 : tm1;
                tm2 = tm2 < 0 ? 0 : tm2;
                tm3 = tm3 < 0 ? 0 : tm3;
            }
            else
            {
                tm1 = (int)(((cvLength / cvSpeed)) * 1000);
                tm2 = (int)(((glassSize / cvSpeed)) * 1000);
                tm3 = (int)(((cvLength / cvSpeed) - marginOnTime) * 1000);
                tm1 = tm1 < 0 ? 0 : tm1;
                tm2 = tm2 < 0 ? 0 : tm2;
                tm3 = tm3 < 0 ? 0 : tm3;
            }

            //string msg = string.Format("{0} : TM1 = {1}, TM2 = {2}, TM3 = {3}", cv.Name, tm1, tm2, tm3);
            //cv.SetLog("TIMERSET", 0, 0, msg);
        }
        public virtual bool GetCvCond(CvUnit cv)
        {
            return cv.CvMotorCond;
        }

        public virtual bool CheckUnitCondition(CvUnit cv)
        {
            bool ok = true;
            ok &= GetCvCond(cv);

            if (cv.NextCv == null)
            {
                ok &= true;
            }

            return ok;
        }

        public virtual void TimerControl(CvUnit cv, _GSS type, params XTimer[] timers)
        {
            if ((cv.AutoAct == CvMotorAct.Fw) && cv.TimerPause[(int)type])
            {
                foreach (XTimer timer in timers)
                {
                    if (timer != null)
                        timer.Resume();
                }
                cv.TimerPause[(int)type] = false;
            }
            else if ((cv.AutoAct == CvMotorAct.Stop) && !cv.TimerPause[(int)type])
            {
                foreach (XTimer timer in timers)
                {
                    if (timer != null)
                        timer.Pause();
                }
                cv.TimerPause[(int)type] = true;
            }
        }

        //public virtual bool CvStopCondition(int id)
        //{
        //    bool cvStop = false;
        //    int start = id - CvUnit.StartId;
        //    for (int i = start; i < m_UnitCount; i++)
        //    {
        //        TagCvIfFlag flag = m_CvUnits[i].IfFlag;
        //        cvStop |= flag.InError;
        //        cvStop |= flag.OutError;

        //        if (cvStop) break;
        //    }

        //    return cvStop;
        //}

        public virtual bool CvStopCondition(CvUnit cv)
        {
            bool cvStop = false;
            CvUnit temp = cv;

            while (temp != null)
            {
                TagCvIfFlag flag = temp.IfFlag;
                cvStop |= flag.InError;
                cvStop |= flag.OutError;

                if (cvStop) break;

                temp = temp.NextCv;
            }

            return cvStop;
        }

        //public virtual bool CvRecoveryRequest(int id)
        //{
        //    bool req = false;
        //    int start = id - CvUnit.StartId;
        //    for (int i = start; i <= start + 1; i++)
        //    {
        //        if (i >= m_CvUnits.Count) continue;

        //        req |= m_CvUnits[i].IfFlag.RecoveryReq;

        //        if (req) break;
        //    }

        //    return req;
        //}

        public virtual bool CvRecoveryRequest(CvUnit cv)
        {
            bool req = false;
            CvUnit temp = cv;
            for (int i = 0; i < 2; i++)
            {
                if (temp == null) break;
                req |= temp.IfFlag.RecoveryReq;

                if (req) break;

                temp = temp.NextCv;
            }

            return req;
        }

        public virtual void InitParameter()
        {
            foreach (CvUnit unit in m_CvUnits)
            {
                if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
                if (unit.Sequence[1] != null) unit.Sequence[1].InitSeq();
                unit.IfFlag.Reset();
                unit.AutoAct = CvMotorAct.Stop;

                int motorCount = unit.Motors.Count;
                for (int i = 0; i < motorCount; i++)
                {
                    unit.ManualAct[i] = CvMotorAct.Stop;
                    unit.ManualSpeed[i] = 0;
                }
            }

            BaseGlobalVar.GlassOutComp1 = true;// m_Server.SeqFlag.GlassOutComp1 = true;
            BaseGlobalVar.UlTimeOut = false;// m_Server.SeqFlag.UlTimeOut = false;
            BaseGlobalVar.LdTimeOut = false;// m_Server.SeqFlag.LdTimeOut = false;
        }
        #endregion

        #region General Methods
        public void SetLog(string seqName, int seqNo, int portNo, int slotNo, string message) // 11.02.01 minhan
        {
            string portName;
            string slotName;
            string seqNumber;
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

            try
            {
                seqNumber = "Case " + seqNo.ToString();
            }
            catch
            {
                seqNumber = "";
            }

            log = string.Format("CvUnit  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfos.EqpLog = log;
        }
        #endregion
    }

    public class SeqInitCv : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static GenInfoHandler m_GenInfos;
        protected static _GenericCollection<CvUnit> m_Units;
        private static _GenericCollection<GantryUnit> m_GantryUnits;
        private static _GenericCollection<ActuatorUnit> m_ActuatorUnits;
        protected static ThreadCvControl m_Control;
        private new int[] m_AlarmId;
        public Alarm ALM_InitFail = null;


        protected GenericTag m_InitCheckGlass = new GenericTag("Glasses in Conveyor", InitCheckState.NotReady);
        protected GenericTag m_InitCheckMotorCP = new GenericTag("CV Motor C/P", InitCheckState.NotReady);
        protected GenericTag m_InitCheckMotorAlarm = new GenericTag("CV Motor Alarm", InitCheckState.NotReady);
        protected GenericTag m_InitCheckTrRobot = new GenericTag("Tr Robot Status", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitCv(ThreadCvControl control, IServerManager server)
        {
            m_SeqFunName = "INIT    ";
            ALM_InitFail = new Alarm("CvUnit" + " Initialize Failed", AlarmLevel.S, AlarmCode.EquipmentSafety);

            m_Server = server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_GenInfos = GenInfoHandler.Instance;

            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_GantryUnits = DmsComponents.Instance.ComponentContainer.GetCollection<GantryUnit>();
            m_ActuatorUnits = DmsComponents.Instance.ComponentContainer.GetCollection<ActuatorUnit>();

            m_Control = control;

            m_AlarmId = new int[m_Units.Count];
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
                        m_Server.Log("SeqCvInit : Start ");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        m_InitCheckGlass.Value = InitCheckState.Checking;
                        //setrecipe2jobcond
                        bool bOk = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if ((m_Server.GlassData.Count == 0) &&
                               (m_Units[i].GlsInSensor.IsDetected(Logic.OR) || m_Units[i].GlsOutSensor.IsDetected(Logic.OR)))
                            {
                                bOk = false;
                                m_AlarmId[i] = m_Units[i].ALM_GlsDataSensing.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[i]);
                            }
                        }

                        if (bOk)
                        {
                            m_Server.Log(m_SeqFunName + ": Check Glass Count OK");
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckGlass.Value = InitCheckState.NG;
                            m_Server.Log(m_SeqFunName + ": Check Glass Count NG");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20:
                    {
                        bool bOk = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if ((m_Server.GlassData.IsExist(m_Units[i].GlsInSensor.Id) && !m_Units[i].GlsInSensor.IsDetected()) ||
                                (m_Server.GlassData.IsExist(m_Units[i].GlsOutSensor.Id) && !m_Units[i].GlsOutSensor.IsDetected()))
                            {
                                if (m_Simul.Device)
                                {
                                    if (m_Server.GlassData.IsExist(m_Units[i].GlsInSensor.Id))
                                        m_Units[i].GlsInSensor.SetState(true, Logic.AND);
                                    if (m_Server.GlassData.IsExist(m_Units[i].GlsOutSensor.Id))
                                    {
                                        if (m_Units[i].FwDecelSensor != null) m_Units[i].FwDecelSensor.SetState(true);
                                        m_Units[i].GlsOutSensor.SetState(true, Logic.AND);
                                    }
                                    bOk = true;
                                }
                                else if (DialogResult.No == MessageBox.Show("Data and Glass Unmatch! Do you want Pass?", "WSSD",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)) // 11.02.01 minhan
                                {
                                    bOk = false;
                                    m_AlarmId[i] = m_Units[i].ALM_GlsDataError.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[i]);
                                }
                            }
                        }

                        if (bOk)
                        {
                            m_Server.Log(m_SeqFunName + ": Check Glass Sensor and Data OK");
                            nSeqNo = 25;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckGlass.Value = InitCheckState.NG;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 25:
                    {
                        bool bOk = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (m_Units[i].FwDecelSensor == null) continue;

                            if (m_Units[i].FwDecelSensor.IsDetected())
                            {
                                if (!m_Server.GlassData.IsExist(m_Units[i].DataMatchingKey(0)) &&
                                    !m_Server.GlassData.IsExist(m_Units[i].DataMatchingKey(1)))
                                {
                                    bOk = false;
                                    m_AlarmId[i] = m_Units[i].ALM_GlsDataSensing.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[i]);
                                }
                            }
                        }

                        if (bOk)
                        {
                            m_InitCheckGlass.Value = InitCheckState.OK;
                            m_InitCheckMotorCP.Value = InitCheckState.Checking;
                            m_Server.Log(m_SeqFunName + ": Check DEC Sensor and Data OK");
                            nSeqNo = 30;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckGlass.Value = InitCheckState.NG;
                            m_Server.Log(m_SeqFunName + ": Check DEC Sensor and Data NG");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 30:
                    {
                        bool bOk = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            foreach (_Motor motor in m_Units[i].Motors)
                            {
                                if (!motor.IsCpOn())
                                {
                                    bOk = false;
                                    m_AlarmId[i] = motor.GetCpAlarm().Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[i]);
                                    break;
                                }
                            }
                        }

                        if (bOk)
                        {
                            m_InitCheckMotorCP.Value = InitCheckState.OK;
                            m_InitCheckMotorAlarm.Value = InitCheckState.Checking;
                            m_Server.Log(m_SeqFunName + ": Check CV Motor CP OK");
                            nSeqNo = 40;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckMotorCP.Value = InitCheckState.NG;
                            m_Server.Log(m_SeqFunName + ": Check CV Motor CP NG");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 40:
                    {
                        bool bOk = true;
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            foreach (_Motor motor in m_Units[i].Motors)
                            {
                                if (motor.IsAlarm())
                                {
                                    bOk = false;
                                    m_AlarmId[i] = motor.GetDriverAlarm().Id;
                                    m_EqpManager.SetAlarm(m_AlarmId[0]);
                                    break;
                                }
                            }
                        }

                        if (bOk)
                        {
                            m_InitCheckMotorAlarm.Value = InitCheckState.OK;
                            m_InitCheckTrRobot.Value = InitCheckState.Checking;
                            m_Server.Log(m_SeqFunName + ": Check CV Motor Driver OK");
                            nSeqNo = 45;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckMotorAlarm.Value = InitCheckState.NG;
                            m_Server.Log(m_SeqFunName + ": Check CV Motor Driver NG");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 45:
                    {
                        bool bOk = true;
                        foreach (GantryUnit gantry in m_GantryUnits)
                        {
                            //  Interlock 미적용 시 Skip
                            if (gantry.RobotHandInterlock == null) continue;

                            if (gantry.RobotHandInterlock.IsDetected())
                            {
                                m_AlarmId[0] = m_Control.AlarmTrRobotInterlock.Id;
                                m_EqpManager.SetAlarm(m_AlarmId[0]);
                                bOk = false;
                                break;
                            }
                        }

                        if (bOk)
                        {
                            m_InitCheckTrRobot.Value = InitCheckState.OK;
                            m_Server.Log(m_SeqFunName + " : Check Tr Robot Status OK");
                            nSeqNo = 50;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            m_InitCheckTrRobot.Value = InitCheckState.NG;
                            m_Server.Log(m_SeqFunName + " : Check Robot Status NG");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 50:
                    {
                        foreach (CvUnit unit in m_Units)
                        {
                            if (unit.GlsInSensor.IsDetected(Logic.OR) || unit.GlsOutSensor.IsDetected(Logic.OR) ||
                                m_Server.GlassData.IsExist(unit.DataMatchingKey(0)) || m_Server.GlassData.IsExist(unit.DataMatchingKey(1)))
                            {
                                m_GenInfos.CleanOut = true;
                                m_GenInfos.CycleStop = true;

                                BaseGlobalVar.TimeOver = false;// m_Server.SeqFlag.TimeOver = false;
                                //m_Server.JobCond.SetTactTime(true);
                            }
                        }

                        if (m_Simul.Device && (m_GenInfos.EQPGlassCount > 0)) //simulation일때는 sequnit이 돌아야 센서셋팅을 한다.
                        {
                            m_GenInfos.CleanOut = true;
                            m_GenInfos.CycleStop = true;

                            BaseGlobalVar.TimeOver = false;// m_Server.SeqFlag.TimeOver = false;
                            //m_Server.JobCond.SetTactTime(true);
                        }

                        nSeqNo = 100;
                    }
                    break;
                case 100:
                    {
                        if (m_GenInfos.CleanOut)
                        {
                            m_GenInfos.Pause = true;
                            m_Server.Log("SeqCvInit : CleanOut Mode Start");
                        }
                        else
                        {
                            foreach (ActuatorUnit actuator in m_ActuatorUnits)
                            {
                                switch (actuator.InitAct)
                                {
                                    case ActuatorInitAct.Positive:
                                        actuator.SetPositiveAct();
                                        break;
                                    case ActuatorInitAct.Negative:
                                        actuator.SetNegativeAct();
                                        break;
                                }
                            }

                            m_Server.Log("SeqCvInit : Complete ");
                        }
                        m_InitState = InitState.Comp;
                        //InitParameter
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (m_AlarmId[i] > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId[i]);
                                m_AlarmId[i] = 0;
                            }
                        }
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

    public class SeqInSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManger;
        protected static IServerManager m_Server;
        protected static ThreadCvControl m_Control;
        protected static Simul m_Simul;
        protected static _GenInfoHandler m_GenInfos;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        #endregion

        #region Constructor
        public SeqInSensor(ThreadCvControl control, CvUnit cv)
        {
            Cv = cv;
            m_Server = Cv.ServerManager;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = Cv.GlsInSensor.Name;

            m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Time warning");

            m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Methods
        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.InError)
            {
                Cv.IfFlag.InError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }

        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[0] == null) Cv.Sequence[0] = this;

            m_Control.TimerControl(Cv, _GSS.ssIN, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManger.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.Id * 2 + 0);
                        if (!glassDataExist)
                        {
                            if (!Cv.GlsInSensor.IsDetected())
                            {
                                nSeqNo = 10;
                            }
                            else
                            {
                                Cv.IfFlag.InComp = false;
                                nSeqNo = 500;
                            }
                        }
                        else
                        {
                            Cv.IfFlag.InComp = false;
                            if (simulation)
                            {
                                Cv.GlsInSensor.SetState(true);
                            }

                            if (Cv.PrevCv != null)
                            {
                                Cv.PrevCv.IfFlag.OutReady = true;
                                if (simulation)
                                {
                                    Cv.PrevCv.GlsOutSensor.SetState(true);
                                    Cv.PrevCv.Sequence[1].InitSeq();
                                }
                            }
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        // set flag and sensor condtion
                        bool prevOutReady;
                        bool prevOutError;
                        bool glassExist;

                        if (Cv.PrevCv == null)
                        {
                            prevOutReady = true;
                            prevOutError = false;
                        }
                        else
                        {
                            prevOutReady = Cv.PrevCv.IfFlag.OutReady;
                            prevOutError = Cv.PrevCv.IfFlag.OutError;
                        }

                        glassExist = Cv.GlsInSensor.IsDetected();

                        // check flag and sensor condtion
                        if (glassExist && prevOutReady && !prevOutError)
                        {
                            // Glass Data Move
                            if (Cv.PrevCv == null)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }
                            else
                            {
                                Cv.PrevCv.IfFlag.OutReady = false;

                                m_Server.GlassData.Move(Cv.Id * 2 - 1, Cv.Id * 2);
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }

                            // TODO : Glass animation control

                            // Timer Set
                            int distance = Cv.SetupDistance.GetValue<int>();
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                            m_Timer1.Start(m_Tm1);
                            m_Timer2.Start(m_Tm2);
                            m_Timer3.Start(m_Tm3);

                            m_Tm3Warning = false;
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        // Check the out complete
                        if (Cv.IfFlag.OutComp)
                        {
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete OK");

                            nSeqNo = 30;
                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.InLogicalCheckIgnore = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete NG");

                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            glassDetected = Cv.GlsOutSensor.IsDetected();
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.InLogicalCheckIgnore))
                        {
                            if (simulation)
                            {
                                Cv.GlsOutSensor.SetState(true);
                            }

                            // Flag set / reset
                            Cv.IfFlag.OutComp = false;
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.InLogicalCheckIgnore = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        else if (glassDetected && !m_Tm3Warning)
                        {
                            Cv.IfFlag.OutComp = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 40;
                        }
                        else if (m_Timer1.Over)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManger.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else if (!m_Control.CheckUnitCondition(Cv))
                        {
                            Cv.IfFlag.InError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 500)
                    {
                        if (Cv.GlsOutSensor.IsDetected() && !m_Tm3Warning)
                        {
                            m_Tm3Warning = true;
                            Cv.IfFlag.OutSick = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else
                        {
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    else if (!Cv.GlsOutSensor.IsDetected())
                    {
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 100:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            glassDetected = Cv.GlsInSensor.IsDetected();
                        }
                        else
                        {
                            glassDetected = !(m_Timer2.Over || !Cv.GlsInSensor.IsDetected());
                        }

                        if (!glassDetected)
                        {
                            if (simulation)
                            {
                                Cv.GlsInSensor.SetState(false);
                            }

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 110;
                        }
                        else if (m_Timer2.Over)
                        {
                            bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.InError = true;
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManger.SetAlarm(m_AlarmId);

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 110:
                    if (GetElapsedTicks() > 1000)
                    {
                        bool glassDetected = false;
                        glassDetected = Cv.GlsInSensor.IsDetected();

                        bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();

                        if (glassDetected && timeoutUse)
                        {
                            nSeqNo = 100;
                        }
                        else if (!glassDetected || !timeoutUse)
                        {
                            if (!glassDetected)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                            }
                            else
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                            }

                            if ((Cv.BrokenDetectScanType == null) ||
                                (!Cv.BrokenDetectScanType.SetupBrokenInterlock.Use) ||
                                (!Cv.BrokenDetectScanType.IsError()))
                            {
                                // Flag set
                                Cv.IfFlag.InComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "In Sensor : Finish");

                                nSeqNo = 10;
                            }
                            else
                            {
                                nSeqNo = 120;

                            }
                        }
                    }
                    break;
                case 120:
                    if (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use && Cv.BrokenDetectScanType.IsError())
                    {
                        Cv.IfFlag.InError = true;
                        m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                        m_ReturnSeqNo = 110;
                        nSeqNo = 4000;
                    }
                    else
                    {
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : No Alarm");
                        nSeqNo = 110;
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistance.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        if (simulation)
                        {
                            m_Tm2 = (int)(m_Tm2 / 3);
                        }

                        m_Timer2.Start(m_Tm2);

                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "In Sensor Detect : Timer2 Start(case500)");

                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv))
                    {
                        Cv.IfFlag.InError = false;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 4000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        if (DialogResult.Yes == MessageBox.Show("Glass Broken Scan Alarm. Do you really want to reset", "Broken Scan Alarm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            Cv.BrokenDetectScanType.Reset(false);
                            nSeqNo = 4010;
                        }
                    }
                    break;
                case 4010:
                    if (!Cv.BrokenDetectScanType.IsError())
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset Alarm");
                        if (Cv.IfFlag.InError)
                        {
                            Cv.IfFlag.InError = false;
                        }
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqOutSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManger;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadCvControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        #endregion

        #region Constructor
        public SeqOutSensor(ThreadCvControl control, CvUnit cv)
        {
            Cv = cv;
            m_Server = Cv.ServerManager;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = Cv.GlsOutSensor.Name;

            if (Cv.NextCv == null)
            {
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            }
            else
            {
                m_Timer1 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Timeout");
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_Timer3 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Time warning");
                m_AlarmOnTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            }
        }
        #endregion

        #region Methods
        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.OutError)
            {
                Cv.IfFlag.OutError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }

        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[1] == null) Cv.Sequence[1] = this;

            m_Control.TimerControl(Cv, _GSS.ssOUT, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;


            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManger.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.Id * 2 + 1);
                        if (!glassDataExist)
                        {
                            if (!Cv.GlsOutSensor.IsDetected())
                            {
                                nSeqNo = 10;
                            }
                            else
                            {
                                Cv.IfFlag.OutComp = false;
                                nSeqNo = 500;
                            }
                        }
                        else
                        {
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.OutComp = false;

                            if (simulation)
                            {
                                if (Cv.PrevCv != null)
                                {
                                    Cv.GlsInSensor.SetState(true);
                                }
                                Cv.GlsOutSensor.SetState(true);

                                Cv.Sequence[0].InitSeq();
                            }

                            nSeqNo = 10;
                        }

                    }
                    break;
                case 10:
                    {
                        // set flag and sensor condtion
                        bool curInReady = Cv.IfFlag.InReady;
                        bool curInError = Cv.IfFlag.InError;
                        bool glassExist = Cv.GlsOutSensor.IsDetected();

                        // check flag and sensor condtion
                        if (glassExist && curInReady && !curInError)
                        {
                            // Set Flags
                            Cv.IfFlag.InReady = false;

                            // Glass Data Move
                            m_Server.GlassData.Move(Cv.Id * 2 + 0, Cv.Id * 2 + 1);
                            Cv.SetLog(m_SeqFunName, 0, 0, "Cur Sensor is ON");
                            ApdItemsHandler.Instance.SetData(Cv.Id * 2 + 1, Cv.Name);

                            // Timer Set
                            int distance = 0;
                            if (Cv.NextCv != null)
                            {
                                distance = Cv.SetupDistanceNext.GetValue<int>();
                            }
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);

                            if (Cv.NextCv == null)
                            {
                                m_Timer2.Start(m_Tm2);
                            }
                            else
                            {
                                m_Timer1.Start(m_Tm1);
                                m_Timer2.Start(m_Tm2);
                                m_Timer3.Start(m_Tm3);
                            }

                            m_Tm3Warning = false;
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!m_Control.CheckUnitCondition(Cv) || !m_Control.CheckUnitCondition(Cv.NextCv))
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }
                        if (Cv.NextCv == null)
                        {
                            Cv.SetLog(m_SeqFunName, 0, 0, "This is last Out Sensor");

                            nSeqNo = 100;
                        }
                        else if (Cv.NextCv.IfFlag.InComp)
                        {
                            Cv.SetLog(m_SeqFunName, 0, 0, "Next Complete OK");

                            nSeqNo = 30;
                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.OutLogicalCheckIgnore = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete NG");

                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            glassDetected = Cv.NextCv.GlsInSensor.IsDetected();
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (!m_Control.CheckUnitCondition(Cv) || !m_Control.CheckUnitCondition(Cv.NextCv))
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.OutLogicalCheckIgnore))
                        {
                            if (simulation)
                            {
                                Cv.NextCv.GlsInSensor.SetState(true);
                            }

                            // Flag set / reset
                            Cv.NextCv.IfFlag.InComp = false;
                            Cv.IfFlag.OutReady = true;
                            Cv.IfFlag.OutLogicalCheckIgnore = false;

                            Cv.SetLog(m_SeqFunName, 0, 0, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        else if (glassDetected && !m_Tm3Warning)
                        {
                            Cv.NextCv.IfFlag.InComp = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                            m_StartTicks = XFunc.GetTickCount();

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 40;
                        }
                        else if (m_Timer1.Over)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManger.SetAlarm(m_AlarmId);
                            Cv.IfFlag.OutError = true;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }

                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 500)
                    {
                        if (Cv.NextCv.GlsInSensor.IsDetected() && !m_Tm3Warning)
                        {
                            m_Tm3Warning = true;
                            Cv.NextCv.IfFlag.InSick = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else
                        {
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    else if (!Cv.NextCv.GlsInSensor.IsDetected())
                    {
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 100:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            glassDetected = Cv.GlsOutSensor.IsDetected();
                        }
                        else
                        {
                            glassDetected = !m_Timer2.Over;
                        }

                        if (!glassDetected)
                        {
                            if (simulation)
                            {
                                Cv.GlsOutSensor.SetState(false);
                            }
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 110;
                        }
                        else if (m_Timer2.Over)
                        {
                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.OutError = true;
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManger.SetAlarm(m_AlarmId);

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 110:
                    {
                        if (GetElapsedTicks() > 1000)
                        {
                            bool glassDetected = false;
                            glassDetected = Cv.GlsOutSensor.IsDetected();

                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (glassDetected && timeoutUse)
                            {
                                nSeqNo = 100;
                            }
                            else if (!glassDetected || !timeoutUse)
                            {

                                if (!glassDetected)
                                {
                                    Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                                }
                                else
                                {
                                    Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                                }

                                // Flag set
                                Cv.IfFlag.OutComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                if (Cv.NextCv == null)
                                {
                                    m_Server.GlassData.Delete(Cv.Id * 2 + 1);
                                }

                                nSeqNo = 10;
                            }
                        }
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistanceNext.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        if (simulation)
                        {
                            m_Tm2 = (int)(m_Tm2 / 3);
                        }

                        m_Timer2.Start(m_Tm2);

                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv) && m_Control.CheckUnitCondition(Cv.NextCv))
                    {
                        Cv.IfFlag.OutError = false;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqDualInSensor : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected static Simul m_Simul;
        protected static GenInfoHandler m_GenInfos;
        protected static ThreadCvControl m_Control;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private Alarm m_OutOPWarning;
        private Alarm m_OutOOPWarning;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        private DualGlsSensor m_DualOutSensor;
        private bool m_IsSimulation;
        #endregion

        #region Constructor
        public SeqDualInSensor(ThreadCvControl control, CvUnit cv)
        {
            Cv = cv;
            m_Server = Cv.ServerManager;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_DualOutSensor = (Cv.GlsOutSensor as DualGlsSensor);

            m_SeqFunName = Cv.GlsInSensor.Name;

            m_Timer1 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Timeout");
            m_Timer2 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Off Timeout");
            m_Timer3 = new XTimer("Timer " + Cv.GlsInSensor.Name + " : Next On Time warning");

            m_AlarmOnTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_AlarmOffTimeOut = new Alarm(Cv.GlsInSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_OutOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
            m_OutOOPWarning = new Alarm(Cv.GlsOutSensor.Name + " : OOP Out Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Methods
        private void FlagSet()
        {
            m_IsSimulation = m_Simul.Device;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if ((m_AlarmId == m_OutOOPWarning.Id) || (m_AlarmId == m_OutOPWarning.Id))
                    m_EqpManager.ResetAlarm(m_AlarmId);
            }
        }

        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.InError)
            {
                Cv.IfFlag.InError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[0] == null) Cv.Sequence[0] = this;

            m_Control.TimerControl(Cv, _GSS.ssIN, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = m_SeqNo;

            FlagSet();

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.DataMatchingKey(0));
                        bool glassDataExistPrev = m_Server.GlassData.IsExist(Cv.PrevCv.DataMatchingKey(1));//2009.07.14 kimgun

                        if (!glassDataExist)
                        {
                            //if (!Cv.GlsInSensor.IsDetected())
                            if (!Cv.GlsInSensor.IsDetected(Logic.OR))
                            {
                                nSeqNo = 10;
                            }
                            else
                            {
                                Cv.IfFlag.InComp = false;
                                nSeqNo = 500;
                            }
                        }
                        else
                        {
                            Cv.IfFlag.InComp = false;
                            if (m_IsSimulation)
                            {
                                //Cv.GlsInSensor.DiSensor.SetState(true);
                                Cv.GlsInSensor.SetState(true, Logic.AND);
                            }

                            if (Cv.PrevCv != null)
                            {
                                Cv.PrevCv.IfFlag.OutReady = true;
                                if (m_IsSimulation)
                                {
                                    //Cv.PrevCv.GlsOutSensor.DiSensor.SetState(true);
                                    Cv.PrevCv.GlsOutSensor.SetState(true, Logic.AND);
                                    Cv.PrevCv.Sequence[1].InitSeq();
                                }
                            }
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        // set flag and sensor condtion
                        bool prevOutReady;
                        bool prevOutError;
                        bool glassExist;

                        if (Cv.PrevCv == null)
                        {
                            prevOutReady = true;
                            prevOutError = false;
                        }
                        else
                        {
                            prevOutReady = Cv.PrevCv.IfFlag.OutReady;
                            prevOutError = Cv.PrevCv.IfFlag.OutError;
                        }

                        //glassExist = Cv.GlsInSensor.IsDetected();
                        glassExist = Cv.GlsInSensor.IsDetected(Logic.OR);

                        // check flag and sensor condtion
                        if (glassExist && prevOutReady && !prevOutError)
                        {
                            // Glass Data Move
                            if (Cv.PrevCv == null)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }
                            else
                            {
                                Cv.PrevCv.IfFlag.OutReady = false;

                                m_Server.GlassData.Move(Cv.Id * 2 - 1, Cv.Id * 2);
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is ON");
                            }

                            // TODO : Glass animation control

                            // Timer Set
                            int distance = Cv.SetupDistance.GetValue<int>();
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                            m_Timer1.Start(m_Tm1);
                            m_Timer2.Start(m_Tm2);
                            m_Timer3.Start(m_Tm3);

                            m_Tm3Warning = false;
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        // Check the out complete
                        if (Cv.IfFlag.OutComp)
                        {
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete OK");
                            m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 25;

                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.InLogicalCheckIgnore = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete NG");
                            m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 25;
                        }
                    }
                    break;
                case 25:
                    {
                        if (!Cv.GlsOutSensor.IsDetected(Logic.OR))
                        {
                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > 500 && Cv.GlsOutSensor.IsDetected(Logic.OR))
                        {
                            if (m_DualOutSensor != null)
                            {
                                if (Cv.GlsOutSensor.IsDetected() && m_DualOutSensor.UseOp && m_DualOutSensor.UseOop)
                                {
                                    m_DualOutSensor.UseOop = false;
                                    m_AlarmId = m_OutOOPWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }
                                else if (m_DualOutSensor.UseOp && m_DualOutSensor.UseOop)
                                {
                                    m_DualOutSensor.UseOp = false;
                                    m_AlarmId = m_OutOPWarning.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }
                            }
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        bool glassDetected = false;
                        if (!m_IsSimulation)
                        {
                            //glassDetected = Cv.GlsOutSensor.IsDetected();
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.OR);
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.InLogicalCheckIgnore))
                        {
                            if (m_IsSimulation)
                            {
                                //Cv.GlsOutSensor.DiSensor.SetState(true);
                                Cv.GlsOutSensor.SetState(true, Logic.AND);
                            }

                            // Flag set / reset
                            Cv.IfFlag.OutComp = false;
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.InLogicalCheckIgnore = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        else if (glassDetected && !m_Tm3Warning)
                        {
                            Cv.IfFlag.OutComp = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                            //StartTime = DateTime.Now;
                            m_StartTicks = XFunc.GetTickCount();

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 40;
                        }
                        else if (m_Timer1.Over)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            Cv.IfFlag.InError = true;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else if (!m_Control.CheckUnitCondition(Cv))
                        {
                            Cv.IfFlag.InError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                        }
                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 500)
                    {
                        //if (Cv.GlsOutSensor.IsDetected() && !m_Tm3Warning)
                        if (Cv.GlsOutSensor.IsDetected(Logic.OR) && !m_Tm3Warning)
                        {
                            m_Tm3Warning = true;
                            Cv.IfFlag.OutSick = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else
                        {
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    //else if (!Cv.GlsOutSensor.IsDetected())
                    else if (!Cv.GlsOutSensor.IsDetected(Logic.AND))
                    {
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 100:
                    {
                        bool glassDetected = false;
                        if (!m_IsSimulation)
                        {
                            //glassDetected = Cv.GlsInSensor.IsDetected();
                            glassDetected = Cv.GlsInSensor.IsDetected(Logic.AND);
                        }
                        else
                        {
                            //glassDetected = !(m_Timer2.Over || !Cv.GlsInSensor.IsDetected());
                            glassDetected = !(m_Timer2.Over || !Cv.GlsInSensor.IsDetected(Logic.AND));
                        }

                        if (!glassDetected)
                        {
                            if (m_IsSimulation)
                            {
                                //Cv.GlsInSensor.DiSensor.SetState(false);
                                Cv.GlsInSensor.SetState(false, Logic.AND);
                            }

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 110;
                        }
                        else if (m_Timer2.Over)
                        {
                            bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.InError = true;
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 110:
                    if (GetElapsedTicks() > 1000)
                    {
                        bool glassDetected = false;
                        //glassDetected = Cv.GlsInSensor.IsDetected();
                        glassDetected = Cv.GlsInSensor.IsDetected(Logic.AND);

                        bool timeoutUse = Cv.SetupInOffUse.GetValue<bool>();

                        if (glassDetected && timeoutUse)
                        {
                            nSeqNo = 100;
                        }
                        else if (!glassDetected || !timeoutUse)
                        {
                            if (!glassDetected)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                            }
                            else
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                            }

                            if ((Cv.BrokenDetectScanType == null) ||
                                (!Cv.BrokenDetectScanType.SetupBrokenInterlock.Use) ||
                                (!Cv.BrokenDetectScanType.IsError()))
                            {
                                // Flag set
                                Cv.IfFlag.InComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "In Sensor : Finish");

                                nSeqNo = 10;
                            }
                            else
                            {
                                nSeqNo = 120;

                            }
                        }
                    }
                    break;
                case 120:
                    if (Cv.BrokenDetectScanType.SetupBrokenInterlock.Use && Cv.BrokenDetectScanType.IsError())
                    {
                        Cv.IfFlag.InError = true;
                        m_AlarmId = Cv.BrokenDetectScanType.ALM_BrokenDetect.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : Set Alarm");
                        m_ReturnSeqNo = 110;
                        nSeqNo = 4000;
                    }
                    else
                    {
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : No Alarm");
                        nSeqNo = 110;
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistance.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        if (m_IsSimulation)
                        {
                            m_Tm2 = (int)(m_Tm2 / 3);
                        }

                        m_Timer2.Start(m_Tm2);

                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "In Sensor Detect : Timer2 Start(case500)");

                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv))
                    {
                        Cv.IfFlag.InError = false;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 4000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        if (DialogResult.Yes == MessageBox.Show("Glass Broken Scan Alarm. Do you really want to reset", "Broken Scan Alarm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            Cv.BrokenDetectScanType.Reset(false);
                            nSeqNo = 4010;
                        }
                    }
                    break;
                case 4010:
                    if (!Cv.BrokenDetectScanType.IsError())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Glass Broken Scan : Reset Alarm");
                        if (Cv.IfFlag.InError)
                        {
                            Cv.IfFlag.InError = false;
                        }
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqDualOutSensor : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManger;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static ThreadCvControl m_Control;
        protected static _GenInfoHandler m_GenInfos;
        private CvUnit Cv;
        private XTimer m_Timer1;
        private XTimer m_Timer2;
        private XTimer m_Timer3;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmOffTimeOut;
        private Alarm m_NextInOPWarning;
        private Alarm m_NextInOOPWarning;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int m_Tm1;
        private int m_Tm2;
        private int m_Tm3;
        private bool m_Tm3Warning;
        private DualGlsSensor m_DualNextInSensor;
        #endregion

        #region Constructor
        public SeqDualOutSensor(ThreadCvControl control, CvUnit cv)
        {
            Cv = cv;
            m_Server = Cv.ServerManager;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;
            m_DualNextInSensor = (Cv.NextCv.GlsInSensor as DualGlsSensor);

            m_SeqFunName = Cv.GlsOutSensor.Name;

            if (Cv.NextCv == null)
            {
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
            }
            else
            {
                m_Timer1 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Timeout");
                m_Timer2 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Off Timeout");
                m_Timer3 = new XTimer("Timer " + Cv.GlsOutSensor.Name + " : Next On Time warning");
                m_AlarmOnTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Next On Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
                m_AlarmOffTimeOut = new Alarm(Cv.GlsOutSensor.Name + " : Off Timeout", AlarmLevel.S, AlarmCode.EquipmentSafety);
                m_NextInOPWarning = new Alarm(Cv.NextCv.GlsInSensor.Name + " : OP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
                m_NextInOOPWarning = new Alarm(Cv.NextCv.GlsInSensor.Name + " : OOP In Sensor Warning", AlarmLevel.L, AlarmCode.EquipmentSafety);
            }
        }
        #endregion

        #region Methods
        private void ClearErrorConditionFlags()
        {
            if (Cv.IfFlag.OutError)
            {
                Cv.IfFlag.OutError = false;
                Cv.IfFlag.RecoveryReq = false;
            }
        }

        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;
            if (Cv.Sequence[1] == null) Cv.Sequence[1] = this;

            m_Control.TimerControl(Cv, _GSS.ssOUT, m_Timer1, m_Timer2, m_Timer3);
            if (!m_Control.IsSeqRunCondition(Cv)) return -1;

            int nSeqNo = this.m_SeqNo;
            bool simulation = m_Simul.Device;

            if ((m_AlarmId == m_NextInOPWarning.Id) && m_EqpManger.AlarmResetSwitchPushed) m_EqpManger.ResetAlarm(m_AlarmId);
            if ((m_AlarmId == m_NextInOOPWarning.Id) && m_EqpManger.AlarmResetSwitchPushed) m_EqpManger.ResetAlarm(m_AlarmId);

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManger.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        bool glassDataExist = m_Server.GlassData.IsExist(Cv.Id * 2 + 1);
                        if (!glassDataExist)
                        {
                            //if (!Cv.GlsOutSensor.IsDetected())
                            if (!Cv.GlsOutSensor.IsDetected(Logic.OR))
                            {
                                nSeqNo = 10;
                            }
                            else
                            {
                                Cv.IfFlag.OutComp = false;
                                nSeqNo = 500;
                            }
                        }
                        else
                        {
                            Cv.IfFlag.InReady = true;
                            Cv.IfFlag.OutComp = false;

                            if (simulation)
                            {
                                if (Cv.PrevCv != null)
                                {
                                    //Cv.GlsInSensor.DiSensor.SetState(true);
                                    Cv.GlsInSensor.SetState(true, Logic.AND);
                                }
                                //Cv.GlsOutSensor.DiSensor.SetState(true);
                                Cv.GlsOutSensor.SetState(true, Logic.AND);

                                Cv.Sequence[0].InitSeq();
                            }

                            nSeqNo = 10;
                        }

                    }
                    break;
                case 10:
                    {
                        // set flag and sensor condtion
                        bool curInReady = Cv.IfFlag.InReady;
                        bool curInError = Cv.IfFlag.InError;
                        //bool glassExist = Cv.GlsOutSensor.IsDetected();
                        bool glassExist = Cv.GlsOutSensor.IsDetected(Logic.OR);

                        // check flag and sensor condtion
                        if (glassExist && curInReady && !curInError)
                        {
                            // Set Flags
                            Cv.IfFlag.InReady = false;

                            // Glass Data Move
                            m_Server.GlassData.Move(Cv.Id * 2 + 0, Cv.Id * 2 + 1);
                            Cv.SetLog(m_SeqFunName, 0, 0, "Cur Sensor is ON");
                            ApdItemsHandler.Instance.SetData(Cv.Id * 2 + 1, Cv.Name);

                            // Timer Set
                            int distance = 0;
                            if (Cv.NextCv != null)
                            {
                                distance = Cv.SetupDistanceNext.GetValue<int>();
                            }
                            int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                            m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);

                            if (Cv.NextCv == null)
                            {
                                m_Timer2.Start(m_Tm2);
                            }
                            else
                            {
                                m_Timer1.Start(m_Tm1);
                                m_Timer2.Start(m_Tm2);
                                m_Timer3.Start(m_Tm3);
                            }

                            m_Tm3Warning = false;
                            nSeqNo = 20;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!m_Control.CheckUnitCondition(Cv) || !m_Control.CheckUnitCondition(Cv.NextCv))
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }
                        if (Cv.NextCv == null)
                        {
                            Cv.SetLog(m_SeqFunName, 0, 0, "This is last Out Sensor");

                            nSeqNo = 100;
                        }
                        else if (Cv.NextCv.IfFlag.InComp)
                        {
                            Cv.SetLog(m_SeqFunName, 0, 0, "Next Complete OK");
                            m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 25;
                        }
                        else if (m_Timer3.Over)
                        {
                            Cv.IfFlag.OutLogicalCheckIgnore = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Complete NG");
                            m_StartTicks = XFunc.GetTickCount();//Dual Sensor Check
                            nSeqNo = 25;
                        }
                    }
                    break;
                case 25:
                    {
                        if (!Cv.NextCv.GlsInSensor.IsDetected(Logic.OR))
                        {
                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > 500 && Cv.NextCv.GlsInSensor.IsDetected(Logic.OR))
                        {
                            if (m_DualNextInSensor != null)
                            {
                                if (Cv.NextCv.GlsInSensor.IsDetected() && m_DualNextInSensor.UseOp && m_DualNextInSensor.UseOop)
                                {
                                    m_DualNextInSensor.UseOop = false;
                                    m_AlarmId = m_NextInOOPWarning.Id;
                                    m_EqpManger.SetAlarm(m_AlarmId);
                                }
                                else if (m_DualNextInSensor.UseOp && m_DualNextInSensor.UseOop)
                                {
                                    m_DualNextInSensor.UseOp = false;
                                    m_AlarmId = m_NextInOPWarning.Id;
                                    m_EqpManger.SetAlarm(m_AlarmId);
                                }
                            }
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            //glassDetected = Cv.NextCv.GlsInSensor.IsDetected();
                            glassDetected = Cv.NextCv.GlsInSensor.IsDetected(Logic.OR);
                        }
                        else
                        {
                            glassDetected = m_Timer1.Over;
                        }

                        if (!m_Control.CheckUnitCondition(Cv) || !m_Control.CheckUnitCondition(Cv.NextCv))
                        {
                            Cv.IfFlag.OutError = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : NG");
                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 3000;
                            break;
                        }

                        if (glassDetected && (m_Timer3.Over || Cv.IfFlag.OutLogicalCheckIgnore))
                        {
                            if (simulation)
                            {
                                //Cv.NextCv.GlsInSensor.DiSensor.SetState(true);
                                Cv.NextCv.GlsInSensor.SetState(true, Logic.AND);
                            }

                            // Flag set / reset
                            Cv.NextCv.IfFlag.InComp = false;
                            Cv.IfFlag.OutReady = true;
                            Cv.IfFlag.OutLogicalCheckIgnore = false;

                            Cv.SetLog(m_SeqFunName, 0, 0, "Next Sensor is ON");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            nSeqNo = 100;
                        }
                        else if (glassDetected && !m_Tm3Warning)
                        {
                            Cv.NextCv.IfFlag.InComp = false;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(1st)");
                            m_StartTicks = XFunc.GetTickCount();

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 40;
                        }
                        else if (m_Timer1.Over)
                        {
                            m_AlarmId = m_AlarmOnTimeOut.Id;
                            m_EqpManger.SetAlarm(m_AlarmId);
                            Cv.IfFlag.OutError = true;

                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor ON Timeout Error");

                            m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }

                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 500)
                    {
                        //if (Cv.NextCv.GlsInSensor.IsDetected() && !m_Tm3Warning)
                        if (Cv.NextCv.GlsInSensor.IsDetected(Logic.OR) && !m_Tm3Warning)
                        {
                            m_Tm3Warning = true;
                            Cv.NextCv.IfFlag.InSick = true;
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Next Sensor is NG(2nd)");
                            nSeqNo = m_ReturnSeqNo;
                        }
                        else
                        {
                            nSeqNo = m_ReturnSeqNo;
                        }
                    }
                    //else if (!Cv.NextCv.GlsInSensor.IsDetected())
                    else if (!Cv.NextCv.GlsInSensor.IsDetected(Logic.OR))
                    {
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 100:
                    {
                        bool glassDetected = false;
                        if (!simulation)
                        {
                            //glassDetected = Cv.GlsOutSensor.IsDetected();
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.AND);
                        }
                        else
                        {
                            glassDetected = !m_Timer2.Over;
                        }

                        if (!glassDetected)
                        {
                            if (simulation)
                            {
                                //Cv.GlsOutSensor.DiSensor.SetState(false);
                                Cv.GlsOutSensor.SetState(false, Logic.AND);
                            }
                            Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(1st)");

                            // Clear error condition
                            ClearErrorConditionFlags();

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 110;
                        }
                        else if (m_Timer2.Over)
                        {
                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (timeoutUse == false)
                            {
                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Skip");
                                m_StartTicks = XFunc.GetTickCount();

                                nSeqNo = 110;
                            }
                            else
                            {
                                Cv.IfFlag.OutError = true;
                                m_AlarmId = m_AlarmOffTimeOut.Id;
                                m_EqpManger.SetAlarm(m_AlarmId);

                                Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error : Set");

                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 110:
                    {
                        if (GetElapsedTicks() > 1000)
                        {
                            bool glassDetected = false;
                            //glassDetected = Cv.GlsOutSensor.IsDetected();
                            glassDetected = Cv.GlsOutSensor.IsDetected(Logic.AND);

                            bool timeoutUse = Cv.SetupOutOffUse.GetValue<bool>();
                            if (glassDetected && timeoutUse)
                            {
                                nSeqNo = 100;
                            }
                            else if (!glassDetected || !timeoutUse)
                            {

                                if (!glassDetected)
                                {
                                    Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor is OFF(2nd)");
                                }
                                else
                                {
                                    Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Cur Sensor OFF Timeout Error(2nd) : Skip");
                                }

                                // Flag set
                                Cv.IfFlag.OutComp = true;

                                // Clear error condition
                                ClearErrorConditionFlags();

                                if (Cv.NextCv == null)
                                {
                                    m_Server.GlassData.Delete(Cv.Id * 2 + 1);
                                }

                                nSeqNo = 10;
                            }
                        }
                    }
                    break;
                case 500:
                    {
                        int distance = Cv.SetupDistanceNext.GetValue<int>();
                        int glassSize = m_Server.SetupGlassSize.GetValue<int>();
                        m_Control.GetTimeOutParameter(Cv, distance, glassSize, Cv.AutoSpeed, ref m_Tm1, ref m_Tm2, ref m_Tm3);
                        if (simulation)
                        {
                            m_Tm2 = (int)(m_Tm2 / 3);
                        }

                        m_Timer2.Start(m_Tm2);

                        nSeqNo = 100;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Timer1.Start(m_Tm1);
                        m_Timer2.Start(m_Tm2);

                        Cv.IfFlag.RecoveryReq = true;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Error Recovery request");

                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 3000:
                    if (m_Control.CheckUnitCondition(Cv) && m_Control.CheckUnitCondition(Cv.NextCv))
                    {
                        Cv.IfFlag.OutError = false;
                        Cv.SetLog(m_SeqFunName, m_PortNo, m_SlotNo, "Check Unit Condition : OK(2nd)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCvMotor : XSeqFunction
    {
        #region Fields
        private CvUnit Cv;
        protected static ThreadCvControl m_Control;
        protected int m_MotorCount;
        private CvMotorAct[] oldAct;
        private string msg;
        #endregion

        #region Constructor
        public SeqCvMotor(ThreadCvControl control, CvUnit cv)
        {
            Cv = cv;
            m_MotorCount = Cv.Motors.Count;
            m_Control = control;
            oldAct = new CvMotorAct[m_MotorCount];
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            for (int i = 0; i < m_MotorCount; i++)
            {
                CvMotorAct act = m_Control.GetRefMotorAct(Cv, i);
                Cv.Motors[i].SetMotorAct((int)act);

                if (act != oldAct[i])
                {
                    msg = string.Format("{0} : {1} -> {2}", Cv.Name, oldAct[i].ToString(), act.ToString());
                    Cv.SetLog("MOTORACT", 0, 0, msg);
                    oldAct[i] = act;
                }
            }

            return -1;
        }
        #endregion
    }

    public class SeqCvMotorCondition : XSeqFunction
    {
        #region Fields
        private CvUnit m_Device;
        protected static IEqpManager m_EqpManager;
        private new int[] m_AlarmId;
        private string log;
        #endregion

        #region Constructor
        public SeqCvMotorCondition(ThreadCvControl control, CvUnit device)
        {
            m_Device = device;
            m_EqpManager = m_Device.ServerManager.EqpStateManager;

            m_SeqFunName = device.Name;

            m_AlarmId = new int[m_Device.Motors.Count * 2];
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
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 5000)
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        bool interlock = false;
                        int count = m_Device.Motors.Count;
                        for (int i = 0; i < count; i++)
                        {
                            _Motor motor = m_Device.Motors[i];
                            if (motor.IsAlarm() && (m_AlarmId[i * 2 + 0] == 0))
                            {
                                m_AlarmId[i * 2 + 0] = motor.GetDriverAlarm().Id;
                                m_EqpManager.SetAlarm(m_AlarmId[i * 2 + 0]);
                                log = string.Format("Alarm Set : {0} Driver Alarm", motor.Name);
                                m_Device.SetLog(m_SeqFunName, 0, 0, log);
                            }
                            else if (!motor.IsAlarm() && (m_AlarmId[i * 2 + 0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId[i * 2 + 0]);
                                m_AlarmId[i * 2 + 0] = 0;
                                log = string.Format("Alarm Reset : {0} Driver Alarm Detect", motor.Name);
                                m_Device.SetLog(m_SeqFunName, 0, 0, log);
                            }
                            if (!motor.IsCpOn() && (m_AlarmId[i * 2 + 1] == 0))
                            {
                                m_AlarmId[i * 2 + 1] = motor.GetCpAlarm().Id;
                                m_EqpManager.SetAlarm(m_AlarmId[i * 2 + 1]);
                                log = string.Format("Alarm Set : {0} CP Off Alarm", motor.Name);
                                m_Device.SetLog(m_SeqFunName, 0, 0, log);
                            }
                            else if (motor.IsCpOn() && (m_AlarmId[i * 2 + 1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId[i * 2 + 1]);
                                m_AlarmId[i * 2 + 1] = 0;
                                log = string.Format("Alarm Reset : {0} CP Off Alarm Detect", motor.Name);
                                m_Device.SetLog(m_SeqFunName, 0, 0, log);
                            }

                            interlock |= (m_AlarmId[i * 2 + 0] != 0);
                            interlock |= (m_AlarmId[i * 2 + 1] != 0);
                        }

                        if (interlock && m_Device.CvMotorCond)
                        {
                            m_Device.CvMotorCond = false;
                        }
                        else if (!interlock && !m_Device.CvMotorCond)
                        {
                            m_Device.CvMotorCond = true;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCvMotorSpeedControl : XSeqFunction
    {
        #region Fields
        private CvUnit m_Unit;

        protected static IServerManager m_Server = null;
        protected static GenInfoHandler m_GenInfos;
        protected static ThreadCvControl m_Control;

        protected int m_MotorCount;
        private int m_CurSpeedAuto = 0;
        #endregion

        #region Constructor
        public SeqCvMotorSpeedControl(ThreadCvControl control, CvUnit unit)
        {
            m_Unit = unit;
            m_SeqFunName = "SeqMotorSpeedControl";

            m_Server = m_Unit.ServerManager;
            m_GenInfos = GenInfoHandler.Instance;
            m_Control = control;

            m_MotorCount = m_Unit.Motors.Count;
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
                        if (m_GenInfos.AutoMode)
                        {
                            m_Unit.AutoSpeed = m_Server.JobCond.CvProcessSpeed(m_Unit.Id);
                            m_CurSpeedAuto = m_Unit.AutoSpeed;

                            for (int i = 0; i < m_MotorCount; i++)
                            {
                                m_Unit.Motors[i].SetSpeed((ushort)m_Unit.AutoSpeed);
                            }

                            m_GenInfos.ULCvSpeed = m_Unit.AutoSpeed.ToString();
                            m_GenInfos.LDCvSpeed = m_Unit.AutoSpeed.ToString();
                            m_GenInfos.SwrCvSpeed = m_Unit.AutoSpeed.ToString();
                        }
                        else
                        {
                            //case:ManualSpeed 5900 -> AutoSpeed 18000 -> ManualSpeed 5900
                            for (int i = 0; i < m_MotorCount; i++)
                            {
                                m_Unit.Motors[i].SetSpeed((ushort)m_Unit.ManualSpeed[i]);
                            }

                            m_GenInfos.LDCvSpeed = "0";
                            m_GenInfos.SwrCvSpeed = "0";
                            m_GenInfos.ULCvSpeed = "0";

                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (m_GenInfos.AutoMode)
                        {
                            m_CurSpeedAuto = 0;
                            nSeqNo = 0;
                        }
                        else
                        {
                            if ((m_Unit.FwDecelSensor != null) && m_Unit.FwDecelSensor.IsDetected())
                            {
                                for (int i = 0; i < m_MotorCount; i++)
                                {
                                    m_Unit.Motors[i].SetSpeed((ushort)m_Server.GetSetupCvStopSpeed);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < m_MotorCount; i++)
                                {
                                    m_Unit.Motors[i].SetSpeed((ushort)m_Unit.ManualSpeed[i]);
                                }
                            }
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

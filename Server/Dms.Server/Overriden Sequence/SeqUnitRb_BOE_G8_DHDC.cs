using System;
using System.Collections.Generic;
using System.Text;
using Dms.Device;
using Dms.Sequence;
using Dms.Common; // 10.02.01 minhan
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadRollBrush_BOE_G8_DHDC : ThreadRbControl
    {
        public static ThreadRollBrush_BOE_G8_DHDC Instance;//2009.08.25 kimgun
        private _GenericCollection<CvUnit> m_CvUnits;
        private static ServerManager m_Servermer; // 10.07.25 minhan
        //IServerManager m_Server;
        public ThreadRollBrush_BOE_G8_DHDC(int scanTime, IServerManager server)
            : base(scanTime, server)
        {
            m_Servermer = ServerManager.Instance; // 10.11.08 minhan
            m_CvUnits = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            Instance = this;//2009.08.25 kimgun
        }

        #region RegisterSequences
        protected override void RegisterSequences() // 10.02.01 minhan
        {
            if (m_RbUnits.Count == 0) return;

            foreach (RbUnit device in m_RbUnits)
            {
                RegisterSequence(new SeqRbMotor(this, device));
                RegisterSequence(new SeqRbMotorCondition(this, device));

                if (device.ServoUnit != null)
                {
                    RegisterSequence(new SeqRbGap(this, device));
                }
            }

            m_Server.AddSeqInitFunction(new SeqInitRb(this, m_Server));
        }
        #endregion

        public override bool IsAlarmCondition(RbUnit unit) // 09.11.09 minhan
        {
            bool alarm = false;
            foreach (RbUnit device in m_RbUnits)
            {
                if ((m_Server.JobCond.RbUse(device) == true) ||
                    device.Motor.IsTurnCw() ||
                    device.Motor.IsTurnCcw()) // 10.10.15 minhan
                {
                    alarm |= device.Motor.IsAlarm();
                    alarm |= !device.Motor.IsCpOn();
                    alarm |= !device.RbMotorCond; // 10.03.10 minhan

                    if (device.Id == eqpRbUnits._RB_Unit_Up1.Id) // 10.10.15 minhan
                    {
                        alarm |= GlobalVar.Rb1UpServoMoveErr;
                    }
                    else if (device.Id == eqpRbUnits._RB_Unit_Lo1.Id)
                    {
                        alarm |= GlobalVar.Rb1LoServoMoveErr;
                    }
                    else if (device.Id == eqpRbUnits._RB_Unit_Up2.Id)
                    {
                        alarm |= GlobalVar.Rb2UpServoMoveErr;
                    }
                    else if (device.Id == eqpRbUnits._RB_Unit_Lo2.Id)
                    {
                        alarm |= GlobalVar.Rb2UpServoMoveErr;
                    }
                }
            }

            alarm |= GlobalVar.RbServoParaErr; // 10.05.19 minhan

            //alarm |= unit.Motor.DiAlarm.GetState();
            //alarm |= !unit.Motor.DiCpOn.GetState();

            return alarm;
        }

        public override int GetGlassCount()
        {
            int glassNo = 0;

            for (int i = eqpTransferUnits._LD_CvUnit.DataMatchingKey(0); i <= eqpTransferUnits._UL_CvUnit.DataMatchingKey(0); i++) // 11.04.22 minhan
            {
                if (m_Server.GlassData.IsExist(i)) glassNo++;
            }

            return glassNo;
        }

        public override bool GetRbCond(RbUnit unit)
        {
            bool cond = true;

            if (!IsUse(unit))
            {
                cond &= (unit.RefSpeed == 0.0 || Math.Abs(unit.RefSpeed) == 100.0);
                cond &= (Math.Abs(unit.RefSpeed - unit.CurSpeed) < 50);
            }
            else if (m_Server.JobCond.ProcessMode)//2009.08.26 kimgun
            {
                cond &= unit.RbMotorCond;
                //cond &= (unit.RefSpeed != 0.0 || unit.ProcSpeed != 0.0);
                cond &= (unit.RefSpeed != 0.0 || m_Server.JobCond.RbProcessSpeed(unit) == 0.0);
                cond &= (Math.Abs(unit.RefSpeed - unit.CurSpeed) < 50);
            }

            if (unit.ServoUnit != null)
            {
                cond &= unit.GapSetComp;
            }

            return cond;
            //return base.GetRbCond(unit);
        }

        public override bool IsInterlockCondition(RbUnit unit) // 10.02.01 minhan
        {
            bool interlock = false;
            interlock |= base.IsInterlockCondition(unit);
            if (m_GenInfos.AutoMode) // // 10.10.15 minhan
            {
                interlock |= eqpTankUnits._FR_Unit_Tank.IfFlag.TankLevelFault;//2010.09.09 kimgun 무조건 정지

                for (int i = eqpTransferUnits._RB_CvUnit.Id; i <= eqpTransferUnits._UL_CvUnit.Id; i++) // 11.01.27 minhan
                {
                    CvUnit cvUnit = m_CvUnits[i - 1];
                    interlock |= !cvUnit.CvMotorCond;
                }
            }

            if (eqpPumps._RB_Shower_Pump.IsUse &&
                m_Server.JobCond.ProcessMode &&
                m_GenInfos.AutoMode) // 11.01.27 minhan
            {
                interlock |= (!eqpPumps._RB_Shower_Pump.IsRun());
            }

            interlock |= (!m_Server.JobCond.ProcessMode && m_GenInfos.AutoMode); // 10.02.01 minhan
            return interlock;
        }

        public override bool IsNoUseRunCondition(RbUnit unit) // 10.06.03 minhan
        {
            bool run = true;
            run &= !IsUse(unit); // // 10.10.15 minhan
            run &= unit.SetupServoUse.GetValue<bool>(); // 10.10.15 minhan
            run &= m_Servermer.RbIdleUse.GetValue<bool>(); // 10.11.08 minhan
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= m_Server.JobCond.ProcessMode;
            //run &= (GetGlassCount() > 0);
            return run;
        }

        public override bool IsRunCondition(RbUnit unit)
        {
            // TODO : write specific code
            bool run = true;
            run &= IsUse(unit);
            run &= unit.SetupServoUse.GetValue<bool>(); // 10.05.12 minhan 
            run &= (GetGlassCount() > 0 || (m_GenInfos.IdleRunning && (GetGlassCount() == 0))); // 10.07.13 minhan
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= m_GenInfos.EqpInitComp;
            run &= !GlobalVar.UlTimeOut; // 11.05.02 minhan

            return run;
        }

        public override double GetRefSpeed(RbUnit unit)
        {
            bool run = IsRunCondition(unit);
            bool alarm = IsAlarmCondition(unit);
            bool interlock = IsInterlockCondition(unit);
            bool autoMode = m_GenInfos.AutoMode;
            bool noUseRun = IsNoUseRunCondition(unit);

            if (interlock)
            {
                unit.ManualSpeed = 0.0;
                return 0.0;
            }
            else if (run && !alarm)
            {
                // TODO : make specific condition
                if (m_GenInfos.IdleRunning)
                {
                    unit.ManualSpeed = 0.0;
                    unit.ProcSpeed = unit.SetupIdleSpeed.GetValue<int>();
                    unit.ProcSpeed = unit.ProcSpeed * (unit.SetupIdleDir.GetValue<bool>() ? 1 : -1);
                }
                else
                {
                    unit.ManualSpeed = 0.0;
                    unit.ProcSpeed = m_Server.JobCond.RbProcessSpeed(unit);
                    unit.ProcSpeed = unit.ProcSpeed * (m_Server.JobCond.RbDirection(unit) ? 1 : -1);
                }

                return unit.ProcSpeed;
            }
            else if (!autoMode)
            {
                return unit.ManualSpeed;
            }
            else if (noUseRun) // // 10.10.15 minhan
            {
                if (unit.GapSetComp)
                {
                    unit.ManualSpeed = 0.0;
                    unit.ProcSpeed = 100;
                    unit.ProcSpeed = unit.ProcSpeed * (m_Server.JobCond.RbDirection(unit) ? 1 : -1);
                    return unit.ProcSpeed;
                }
                else
                {
                    unit.ManualSpeed = 0.0;
                    return 0.0;
                }
            }
            else
            {
                unit.ManualSpeed = 0.0; // 10.10.13 minhan
                return 0.0;
            }
        }

        public override Dms.Device.RbUnit.RbUnitAct GetRefPosAct(RbUnit unit)
        {
            if (unit.ServoUnit != null)
            {
                //bool RbNoop = true;
                //RbNoop &= IsNoUseRunCondition(unit);

                bool RbRunCond = true;
                RbRunCond &= IsRunCondition(unit);
                RbRunCond &= !IsInterlockCondition(unit);
                RbRunCond &= !IsAlarmCondition(unit);

                bool RbWaitCond = false;
                RbWaitCond |= !IsRunCondition(unit);
                RbWaitCond |= IsInterlockCondition(unit);
                RbWaitCond |= IsAlarmCondition(unit);

                //bool RbZeroCond = false; // 10.06.03 minhan

                if (!m_GenInfos.AutoMode)
                {
                    if (!unit.RbManualModeChange) // 10.06.03 minhan
                    {
                        unit.RbManualModeChange = true;

                        if (unit.SetupServoUse.GetValue<bool>()) // // 10.10.15 minhan
                        {
                            return RbUnit.RbUnitAct.Wait;
                        }
                        else
                        {
                            return RbUnit.RbUnitAct.Noop;
                        }
                    }
                    else
                    {
                        return RbUnit.RbUnitAct.Zero;
                    }
                }
                else
                {
                    unit.RbManualModeChange = false;

                    if (!unit.SetupServoUse.GetValue<bool>()) // 10.10.15 minhan
                    {
                        return RbUnit.RbUnitAct.Noop;
                    }
                    if (RbRunCond)
                    {
                        if ((m_GenInfos.IdleRunning && (GetGlassCount() == 0))) // 10.07.13 minhan
                        {
                            return RbUnit.RbUnitAct.Wait;
                        }
                        else
                        {
                            return RbUnit.RbUnitAct.Process;
                        }
                    }
                    else if (RbWaitCond)
                    {
                        return RbUnit.RbUnitAct.Wait;
                    }
                    //else if (RbZeroCond) // 10.06.03 minhan
                    //{
                    //    return RbUnit.RbUnitAct.Zero;
                    //}
                    //else if (RbNoop)
                    //{
                    //    return RbUnit.RbUnitAct.Noop;
                    //}
                    else // 10.06.17 minhan
                    {
                        return RbUnit.RbUnitAct.Off;
                    }
                }
            }
            else
            {
                return RbUnit.RbUnitAct.Noop;
            }
        }
    }

    public class SeqRbGap : XSeqFunction // 10.02.01 minhan
    {
        #region Fields
        protected const double m_Margin = 0.02; // 10.06.17 minhan
        protected static IEqpManager m_EqpManger;
        protected static ThreadRbControl m_Control;
        protected RbUnit m_Unit;
        protected static GenInfoHandler m_GenInfos; // 10.02.01 minhan
        protected static ServerManager m_Server; // 10.02.01 minhan
        private string m_Msg; // 10.02.01 minhan
        private string m_status; // 10.02.01 minhan
        #endregion

        #region Constructor
        public SeqRbGap(ThreadRbControl control, RbUnit rb)
        {
            m_Unit = rb;
            m_EqpManger = m_Unit.ServerManager.EqpStateManager;
            m_Control = control;
            m_Server = ServerManager.Instance; // 10.02.01 minhan ;
            m_GenInfos = GenInfoHandler.Instance; // 10.02.01 minhan
            m_Unit.RbManualModeChange = false;

            m_SeqFunName = "RB GAP";
            m_Msg = ""; // 10.02.01 minhan
            m_status = ""; // 10.02.01 minhan
        }
        #endregion

        #region Methods
        public void GapStatus(RbUnit unit) // 10.02.01 minhan
        {
            if (unit.ServoUnit != null)
            {
                if (unit.GapSetComp)
                {
                    if (unit.RefPos.Pos[0] == m_Unit.ProcPos.Pos[0]) m_status = "Zero";
                    else if (unit.RefPos.Pos[0] == m_Unit.WaitPos.Pos[0]) m_status = "Wait";
                    else m_status = "Noop";
                }
                else
                {
                    m_status = "Noop";
                }

                if (unit.Id == eqpRbUnits._RB_Unit_Up1.Id)
                {
                    m_GenInfos.Rb1UpGap = unit.ProcGap.ToString();
                    m_GenInfos.Rb1Position = m_status;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo1.Id)
                {
                    m_GenInfos.Rb1LoGap = unit.ProcGap.ToString();
                    m_GenInfos.Rb2Position = m_status;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Up2.Id)
                {
                    m_GenInfos.Rb2UpGap = unit.ProcGap.ToString();
                    m_GenInfos.Rb3Position = m_status;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo2.Id)
                {
                    m_GenInfos.Rb2LoGap = unit.ProcGap.ToString();
                    m_GenInfos.Rb4Position = m_status;
                }
            }
        }

        public bool ServoStatus(RbUnit unit) // 10.10.15 minhan
        {
            bool checkStatus = true;

            if ((unit.ServoUnit != null) && (unit.SetupServoUse.GetValue<bool>() == true))
            {
                if (unit.Id == eqpRbUnits._RB_Unit_Up1.Id)
                {
                    checkStatus &= !GlobalVar.Rb1UpServoErr;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo1.Id)
                {
                    checkStatus &= !GlobalVar.Rb1LoServoErr;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Up2.Id)
                {
                    checkStatus &= !GlobalVar.Rb2UpServoErr;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo2.Id)
                {
                    checkStatus &= !GlobalVar.Rb2LoServoErr;
                }
            }
            return checkStatus;
        }

        public void ServoErrorSet(RbUnit unit) // 10.10.15 minhan
        {
            if ((unit.ServoUnit != null) && (unit.SetupServoUse.GetValue<bool>() == true))
            {
                if (unit.Id == eqpRbUnits._RB_Unit_Up1.Id)
                {
                    GlobalVar.Rb1UpServoMoveErr = true;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo1.Id)
                {
                    GlobalVar.Rb1LoServoMoveErr = true;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Up2.Id)
                {
                    GlobalVar.Rb2UpServoMoveErr = true;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo2.Id)
                {
                    GlobalVar.Rb2LoServoMoveErr = true;
                }
            }
        }

        public void ServoErrorReset(RbUnit unit) // 10.10.15 minhan
        {
            if ((unit.ServoUnit != null)/* && (unit.SetupServoUse.GetValue<bool>() == true)*/)
            {
                if (unit.Id == eqpRbUnits._RB_Unit_Up1.Id)
                {
                    GlobalVar.Rb1UpServoMoveErr = false;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo1.Id)
                {
                    GlobalVar.Rb1LoServoMoveErr = false;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Up2.Id)
                {
                    GlobalVar.Rb2UpServoMoveErr = false;
                }
                else if (unit.Id == eqpRbUnits._RB_Unit_Lo2.Id)
                {
                    GlobalVar.Rb2LoServoMoveErr = false;
                }
            }
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!GenInfoHandler.Instance.EqpInitComp) return -1;//2010.01.29 kimgun
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            GapStatus(m_Unit); // 10.02.01 minhan

            if (ServoStatus(m_Unit) == false) // 10.10.15 minhan
            {
                //if (m_AlarmId != 0)
                //{
                //    m_EqpManger.ResetAlarm(m_AlarmId);
                //    m_AlarmId = 0;
                //}
                //this.SeqNo = 0;

                m_Unit.GapSetComp = false;

                return -1;
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Unit.RefPosAct = m_Control.GetRefPosAct(m_Unit);

                        switch (m_Unit.RefPosAct)
                        {
                            case RbUnit.RbUnitAct.Noop:
                                {
                                    //if ((m_Unit.ServoUnit != null) &&
                                    //   (m_Unit.ServoUnit.IsDetectHomeSwitch(0) == false)) // 10.11.08 minhan
                                    //{
                                    //    m_Unit.GapSetComp = false;
                                    //    m_AlarmId = m_Unit.ALM_ServoHomeFail.Id;
                                    //    m_EqpManger.SetAlarm(m_AlarmId);
                                    //    m_Msg = string.Format("{0} Homing Error", m_Unit.Name);
                                    //    m_Unit.SetLog(SeqFunName, 0, 0, m_Msg);
                                    //    ServoErrorSet(m_Unit); // 10.10.15 minhan
                                    //    nSeqNo = 1000;
                                    //}
                                    //else
                                    //{
                                    if (m_Unit.ServoUnit != null) m_Unit.RefPos = m_Unit.WaitPos; // 10.10.15 minhan
                                    m_Unit.GapSetComp = true;
                                    //}
                                }
                                break;
                            case RbUnit.RbUnitAct.Off:
                                {
                                    if (!m_Unit.ServoUnit.Ready)
                                    {
                                        m_Unit.GapSetComp = true;
                                    }
                                    else
                                    {
                                        // false 처리 하고 -> E-Stop
                                        m_Unit.GapSetComp = false;
                                        m_StartTicks = XFunc.GetTickCount(); // 10.02.01 minhan
                                        nSeqNo = 500;
                                    }
                                }
                                break;
                            case RbUnit.RbUnitAct.Wait:
                                {
                                    m_Unit.WaitPos.Pos[0] = m_Unit.ServoUnit.GetTeachPointPos(0).Pos[0]; // 10.11.08 minhan
                                    m_Unit.RefPos = m_Unit.WaitPos;
                                    nSeqNo = 10;
                                }
                                break;
                            case RbUnit.RbUnitAct.Process:
                                {
                                    m_Unit.ProcGap = m_Unit.ServerManager.JobCond.RbGap(m_Unit);
                                    m_Unit.RefPos = m_Unit.ProcPos;
                                    nSeqNo = 10;
                                }
                                break;
                            case RbUnit.RbUnitAct.Zero:
                                {
                                    if (!m_Unit.GapSetComp) nSeqNo = 10;
                                }
                                break;
                        }
                    }
                    break;
                case 10:
                    {
                        if (Math.Abs(m_Unit.CurPos.Pos[0] - m_Unit.RefPos.Pos[0]) < m_Margin)
                        {   // Set complete
                            if (true == m_Unit.ServoUnit.Ready)
                            {
                                m_Unit.GapSetComp = true;
                                nSeqNo = 0;
                            }
                            else
                            {   // servo off condition : Reset Servo
                                m_Unit.GapSetComp = false;
                                m_StartTicks = XFunc.GetTickCount(); // 10.02.01 minhan
                                nSeqNo = 600;
                            }
                        }
                        else
                        {   // change position
                            //if (!m_Unit.GapSetComp)
                            //{   // prev. progress is not complete : Stop -> Reset Servo
                            //    nSeqNo = 500;
                            //}
                            //else
                            {   // prev. progress is complete
                                m_Unit.GapSetComp = false;
                                nSeqNo = 100;
                            }
                        }
                    }
                    break;

                case 100:
                    if (!m_Unit.ServoUnit.Ready) // 과연 서보 on 안되었는데 그냥 서보 on만 해도 될까?.
                    {   // servo off condition : Reset Servo
                        m_StartTicks = XFunc.GetTickCount(); // 10.02.01 minhan
                        nSeqNo = 600;
                    }
                    else if (!m_Unit.ServoUnit.HomeComp)
                    {
                        m_AlarmId = m_Unit.ALM_ServoHomeFail.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Homing Error", m_Unit.Name);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        ServoErrorSet(m_Unit); // 10.10.15 minhan
                        nSeqNo = 1000;
                    }
                    else
                    {
                        m_Msg = string.Format("{0} Moving Start : {1}", m_Unit.Name, m_Unit.RefPos.Pos[0]);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        nSeqNo = 110;
                    }
                    break;
                case 110:
                    if (0 == (result = m_Unit.ServoUnit.RbtMovePos(m_Unit.RefPos)))
                    {
                        m_Unit.GapSetComp = true;
                        nSeqNo = 0;
                    }
                    else if (0 < result)
                    {
                        m_AlarmId = m_Unit.ALM_ServoMoveFail.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Moving Error: Alarm Code:{1}", m_Unit.Name, result);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        ServoErrorSet(m_Unit); // 10.10.15 minhan
                        nSeqNo = 1000;
                    }
                    break;
                case 500:
                    if (true == m_Unit.ServoUnit.RbtEStop())
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 4000) // 10.05.12 minhan
                    {
                        m_AlarmId = m_Unit.ALM_ServoEstopFail.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} EStop Error", m_Unit.Name);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        ServoErrorSet(m_Unit); // 10.10.15 minhan
                        nSeqNo = 1000;
                    }
                    break;
                case 600: // 10.02.01 minhan
                    if (true == m_Unit.ServoUnit.RbtReset()) // 10.05.12 minhan
                    {
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 4000)
                    {
                        m_AlarmId = m_Unit.ALM_ServoResetFail.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        m_Msg = string.Format("{0} Reset Error", m_Unit.Name);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, m_Msg);
                        ServoErrorSet(m_Unit); // 10.10.15 minhan
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset");

                        ServoErrorReset(m_Unit); // 10.10.15 minhan
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1010;
                    }
                    break;
                case 1010:
                    if (GetElapsedTicks() > 2000)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
        #endregion
    }

    public class SeqInitRb : XSeqInitFunction
    {
        #region Fields
        protected InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static _GenericCollection<RbUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private ThreadRbControl m_Control;
        private int m_ServoNo;
        private List<int> m_ServoIndex = new List<int>();
        private bool[] m_ServoUse;
        private string m_Msg;
        private int m_Rv = -1;
        protected GenericTag m_InitRbServo = null;
        //protected GenericTag m_InitRb1UpOrigine = new GenericTag("RB1 Upper Origin Position", InitCheckState.NotReady); // 10.05.12 minhan
        //protected GenericTag m_InitRb1LoOrigine = new GenericTag("RB1 Lower Origin Position", InitCheckState.NotReady);
        //protected GenericTag m_InitRb2UpOrigine = new GenericTag("RB2 Upper Origin Position", InitCheckState.NotReady);
        //protected GenericTag m_InitRb2LoOrigine = new GenericTag("RB2 Lower Origin Position", InitCheckState.NotReady);
        #endregion

        #region Constructor
        public SeqInitRb(ThreadRbControl control, IServerManager server)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<RbUnit>();
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            //RB Servo를 하나라도 쓰는 경우에만 Init Dialog에 등록
            int count = m_Units.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_Units[i].ServoUnit != null)
                {
                    m_InitRbServo = new GenericTag("RollBrush Servo", InitCheckState.NotReady);
                    m_InitCheckItems.Add(m_InitRbServo);
                    break;
                }
            }

            //jemoon : 100127 오류가 있어 생성자로 옴겨옴
            for (int i = 0; i < count; i++)
            {
                if (m_Units[i].ServoUnit != null)
                {
                    m_ServoIndex.Add(i);
                }
            }

            //jemoon : 100127 오류가 있어 생성자로 옴겨옴
            m_ServoUse = new bool[m_ServoIndex.Count];

            this.m_SeqFunName = "INIT    ";
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
                        m_Server.Log("Start");

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        // Servo init
                        m_ServoNo = 0;

                        //int count = m_Units.Count;
                        //for (int i = 0; i < count; i++) // 10.05.12 minhan
                        //{
                        //    if (m_Units[i].ServoUnit != null)
                        //    {
                        //        m_ServoIndex.Add(i);
                        //    }
                        //}

                        //if (m_ServoIndex.Count != 0)
                        //{
                        //    m_Server.Log("Rb Servo E-Stop Request");
                        //    m_InitRb1UpOrigine.Value = InitCheckState.Checking; // 10.02.01 minhan
                        //    m_InitRb1LoOrigine.Value = InitCheckState.Checking;
                        //    m_InitRb2UpOrigine.Value = InitCheckState.Checking;
                        //    m_InitRb2LoOrigine.Value = InitCheckState.Checking;
                        //    m_StartTicks = XFunc.GetTickCount();
                        //    nSeqNo = 20;
                        //}
                        //else
                        //{
                        //    m_Server.Log("No Rb Servo Unit");
                        //    m_StartTicks = XFunc.GetTickCount();
                        //    nSeqNo = 60;
                        //}

                        if (m_ServoIndex.Count != 0)
                        {
                            m_InitRbServo.Value = InitCheckState.ServoEStop;
                            m_Server.Log("Rb Servo E-Stop Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Server.Log("No Rb Servo Unit");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 60;
                        }

                    }
                    break;
                case 20:
                    if (true == m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtEStop())
                    {
                        m_Msg = string.Format("Rb ServoUnit{0}: E-Stop Complete", m_ServoNo);
                        m_Server.Log(m_Msg);

                        m_ServoNo++;

                        if (m_ServoNo < m_ServoIndex.Count)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (m_ServoNo >= m_ServoIndex.Count)
                        {
                            m_ServoNo = 0;
                            //m_ServoUse = new bool[m_ServoIndex.Count]; // 10.05.12 minhan

                            int count = m_ServoIndex.Count;
                            for (int i = 0; i < count; i++)
                            {
                                m_ServoUse[i] = m_Units[m_ServoIndex[i]].SetupServoUse.GetValue<bool>();
                                //m_ServoUse[i] &= m_Server.JobCond.ProcessMode; // 10.06.03 minhan
                            }

                            m_InitRbServo.Value = InitCheckState.ServoReset;

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 30;
                        }
                    }
                    else if (GetElapsedTicks() > 4000) // 10.02.01 minhan
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG;
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoEstopFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("E-Stop Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 30:
                    if (!m_ServoUse[m_ServoNo] || m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtReset()) // 10.06.03 minhan
                    {
                        if (m_ServoUse[m_ServoNo]) m_Msg = string.Format("Rb Servo{0} Reset Complete", m_ServoNo);
                        else m_Msg = string.Format("Rb Servo{0} Reset Skip", m_ServoNo);
                        m_Server.Log(m_Msg);

                        m_ServoNo++;

                        if (m_ServoNo < m_ServoIndex.Count)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (m_ServoNo >= m_ServoIndex.Count)
                        {
                            m_ServoNo = 0;
                            m_InitRbServo.Value = InitCheckState.ServoHoming;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                    }
                    else if (GetElapsedTicks() > 4000) // 10.02.01 minhan
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG;
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoResetFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("Reset Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 40:
                    if (GetElapsedTicks() > 500)
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 50;
                    }
                    break;
                case 50:
                    if (!m_ServoUse[m_ServoNo] || (m_Rv = m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtMoveHome()) == 0)
                    {
                        if (m_ServoUse[m_ServoNo]) m_Msg = string.Format("Rb Servo{0} Homing Complete", m_ServoNo);
                        else m_Msg = string.Format("Rb Servo{0} Homing Skip", m_ServoNo);

                        m_Server.Log(m_Msg);

                        m_ServoNo++;

                        if (m_ServoNo < m_ServoIndex.Count)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (m_ServoNo >= m_ServoIndex.Count) // 10.07.27 minhan
                        {
                            m_ServoNo = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 60;
                        }
                    }
                    else if (m_Rv > 0 && (GetElapsedTicks() > 15000)) // 10.05.12 minhan
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG;
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoHomeFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("Homing Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 60:
                    if (GetElapsedTicks() > 1000)
                    {
                        if (m_ServoIndex.Count != 0)
                        {
                            m_InitRbServo.Value = InitCheckState.OK;
                        }
                        else
                        {
                            m_InitRbServo.Value = InitCheckState.NoUse;
                        }
                        m_Server.Log("Init Complete");
                        m_InitState = InitState.Comp;
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.Log("Alarm Reset");
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
}

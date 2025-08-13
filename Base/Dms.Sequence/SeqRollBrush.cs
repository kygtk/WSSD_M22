using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Device;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadRbControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<RbUnit> m_RbUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
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

        #region Constructor
        public ThreadRbControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_RbUnits = DmsComponents.Instance.ComponentContainer.GetCollection<RbUnit>();
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
        public static _GenericCollection<RbUnit> Units
        {
            get
            {
                if (m_RbUnits == null) m_RbUnits = new _GenericCollection<RbUnit>();
                return m_RbUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsAlarmCondition(RbUnit unit)
        {
            bool alarm = false;

            alarm |= unit.Motor.IsAlarm();
            alarm |= !unit.Motor.IsCpOn();

            return alarm;
        }

        //EMO, Cover, Leak 등 Heavy Interlock이 있는가?
        public virtual bool IsInterlockCondition(RbUnit unit)
        {
            bool interlock = false;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            //interlock |= ((heavy & ~HeavyInterlock.Door) > 0 );
            interlock |= (heavy > 0);

            return interlock;
        }

        //Cv가 Stop되었는가? 
        //필히 override 되어야 한다.
        public virtual bool IsCvStopCondition(RbUnit unit)
        {
            bool cvStop = false;

            return cvStop;
        }

        //RB가 Auto Run 해야 하는 상황인가?
        public virtual bool IsRunCondition(RbUnit unit)
        {
            // TODO : write specific code
            bool run = true;
            run &= IsUse(unit);
            run &= m_Server.JobCond.ProcessMode;
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= (GetGlassCount() > 0 || (m_GenInfos.IdleRunning && GetGlassCount() == 0));
            run &= !IsCvStopCondition(unit);

            return run;
        }

        public virtual bool IsUse(RbUnit unit)
        {
            return m_Server.JobCond.RbUse(unit);
        }

        public virtual bool IsNoUseRunCondition(RbUnit unit)
        {
            bool run = true;
            run &= !IsUse(unit);
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= m_Server.JobCond.ProcessMode;
            run &= (GetGlassCount() > 0);
            return run;
        }

        public virtual double GetRefSpeed(RbUnit unit)
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
            //else if (noUseRun)
            //{
            //    unit.ManualSpeed = 0.0;
            //    unit.ProcSpeed = 100;
            //    unit.ProcSpeed = unit.ProcSpeed * (m_Server.JobCond.RbDirection(unit) ? 1 : -1);
            //    return unit.ProcSpeed;
            //}
            else
            {
                return 0.0;
            }
        }

        public virtual Dms.Device.RbUnit.RbUnitAct GetRefPosAct(RbUnit unit)
        {
            if (unit.ServoUnit == null)
            {
                return RbUnit.RbUnitAct.Noop;
            }
            else
            {
                bool rbNoop = true;
                rbNoop &= IsNoUseRunCondition(unit);

                bool rbRunCond = true;
                rbRunCond &= IsRunCondition(unit);
                rbRunCond &= !IsInterlockCondition(unit);
                rbRunCond &= !IsAlarmCondition(unit);

                bool rbWaitCond = true;
                rbWaitCond |= !IsRunCondition(unit);
                rbWaitCond |= IsInterlockCondition(unit);
                rbWaitCond |= IsAlarmCondition(unit);

                bool rbZeroCond = false;

                RbUnit.RbUnitAct act = RbUnit.RbUnitAct.Noop;

                if (rbRunCond)
                {
                    act = RbUnit.RbUnitAct.Process;
                }
                else if (rbWaitCond)
                {
                    act = RbUnit.RbUnitAct.Wait;
                }
                else if (rbZeroCond)
                {
                    act = RbUnit.RbUnitAct.Zero;
                }
                else if (rbNoop)
                {
                    act = RbUnit.RbUnitAct.Noop;
                }
                else
                {
                    act = RbUnit.RbUnitAct.Off;
                }

                return act;
            }
        }

        public virtual bool GetRbCond(RbUnit unit)
        {
            bool cond = true;

            if (!IsRunCondition(unit))
            {
                cond &= (unit.RefSpeed == 0.0 || Math.Abs(unit.RefSpeed) == 100.0);
                cond &= (Math.Abs(unit.RefSpeed - unit.CurSpeed) < 50);
            }
            else
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
        }

        public virtual int GetGlassCount()
        {
            int glassCount = 0;

            glassCount = m_Server.JobCond.GetGlassCountInProcess();

            return glassCount;
        }
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqRbGap : XSeqFunction
    {
        #region Fields
        protected const double m_Margin = 0.05;
        protected static IEqpManager m_EqpManger;
        protected static ThreadRbControl m_Control;
        protected RbUnit m_Unit;
        protected static _GenInfoHandler m_GenInfos;
        protected static IServerManager m_Server;
        #endregion

        #region Constructor
        public SeqRbGap(ThreadRbControl control, RbUnit rb)
        {
            m_Unit = rb;
            m_EqpManger = m_Unit.ServerManager.EqpStateManager;
            m_Control = control;
            m_Server = m_Unit.ServerManager;
            m_GenInfos = GenInfoHandler.Instance;
            m_Unit.RbManualModeChange = false;

            m_SeqFunName = "RB GAP";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfos.EqpInitComp) return -1;

            int result = -1;
            int nSeqNo = this.m_SeqNo;


            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Unit.RefPosAct = m_Control.GetRefPosAct(m_Unit);

                        switch (m_Unit.RefPosAct)
                        {
                            case RbUnit.RbUnitAct.Noop:
                                {
                                    m_Unit.GapSetComp = true;
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
                                        nSeqNo = 500;
                                    }
                                }
                                break;
                            case RbUnit.RbUnitAct.Wait:
                                {
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
                    if (!m_Unit.ServoUnit.Ready)
                    {   // servo off condition : Reset Servo
                        nSeqNo = 600;
                    }
                    else if (!m_Unit.ServoUnit.HomeComp)
                    {
                        m_EqpManger.SetAlarm(m_Unit.ALM_ServoMoveFail.Id);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : RB Servo moving failed");
                        nSeqNo = 1000;
                    }
                    else if (0 == (result = m_Unit.ServoUnit.RbtMovePos(m_Unit.RefPos)))
                    {
                        m_Unit.GapSetComp = true;
                        nSeqNo = 0;
                    }
                    else if (0 < result)
                    {
                        m_EqpManger.SetAlarm(m_Unit.ALM_ServoMoveFail.Id);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : RB Servo moving failed");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 500:
                    if (true == m_Unit.ServoUnit.RbtEStop())
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 600:
                    if (true == m_Unit.ServoUnit.RbtReset())
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : RB Servo moving failed");
                        m_EqpManger.ResetAlarm(m_Unit.ALM_ServoMoveFail.Id);

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

    public class SeqRbMotor : XSeqFunction
    {
        #region Fields
        protected static _GenInfoHandler m_GenInfos;
        protected const double m_Gain = 0.2;
        protected const double m_Limit = 1.0;
        protected RbUnit m_Unit;
        protected static IServerManager m_Server;
        protected static ThreadRbControl m_Control;
        protected double m_Increase;
        protected double m_ErrSpeed;
        #endregion

        #region Properties
        private double Increase
        {
            get { return m_Increase; }
            set
            {
                if (Math.Abs(value) <= m_Limit)
                {
                    m_Increase = value;
                }
                else
                {
                    if (value < 0)
                    {
                        m_Increase = -1 * m_Limit;
                    }
                    else
                    {
                        m_Increase = m_Limit;
                    }
                }
            }
        }
        #endregion

        #region Contructor
        public SeqRbMotor(ThreadRbControl control, RbUnit rb)
        {
            m_Unit = rb;
            m_Server = m_Unit.ServerManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

            m_SeqFunName = "RB MOTOR";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            //if (!m_GenInfos.EqpInitComp) return -1;

            m_Unit.RefSpeed = m_Control.GetRefSpeed(m_Unit);

            m_ErrSpeed = m_Unit.RefSpeed - m_Unit.CurSpeed;

            if (Math.Abs(m_ErrSpeed) < 1.0 && Math.Abs(m_Unit.CurSpeed) < 1.0)
            {
                m_ErrSpeed = 0.0;
                m_Unit.CurSpeed = 0.0;
            }

            if (m_ErrSpeed == 0.0 && m_Unit.RefSpeed == 0.0)
            {
                m_Unit.Motor.Stop();
            }
            else if (m_ErrSpeed != 0.0)
            {
                Increase = m_Gain * m_ErrSpeed;
                m_Unit.CurSpeed += Increase;

                m_Unit.Motor.SetSpeed((ushort)Math.Abs(m_Unit.CurSpeed));

                if (m_Unit.CurSpeed > 0)
                {
                    m_Unit.Motor.TurnCw();
                }
                else
                {
                    m_Unit.Motor.TurnCcw();
                }
            }

            return -1;
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
                        if (m_ServoIndex.Count != 0)
                        {
                            m_InitRbServo.Value = InitCheckState.ServoEStop.ToString();
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
                            int count = m_ServoIndex.Count;
                            for (int i = 0; i < count; i++)
                            {
                                m_ServoUse[i] = m_Units[m_ServoIndex[i]].SetupServoUse.GetValue<bool>();
                                m_ServoUse[i] &= m_Server.JobCond.ProcessMode;
                            }

                            m_InitRbServo.Value = InitCheckState.ServoReset.ToString();

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 30;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG.ToString();
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoEstopFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("E-Stop Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 30:
                    if (!m_ServoUse[m_ServoNo] || m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtReset())
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
                            m_InitRbServo.Value = InitCheckState.ServoHoming.ToString();
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG.ToString();
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
                        else if (m_SeqNo >= m_ServoIndex.Count)
                        {
                            m_ServoNo = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 60;
                        }
                    }
                    else if (m_Rv > 0 && (GetElapsedTicks() > 10000))
                    {
                        m_InitState = InitState.Fail;
                        m_InitRbServo.Value = InitCheckState.NG.ToString();
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoHomeFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("Homing Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 60:
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Server.Log("Init Complete");
                        m_InitState = InitState.Comp;
                        m_InitRbServo.Value = InitCheckState.OK.ToString();
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

    public class SeqRbMotorCondition : XSeqFunction
    {
        #region Fields
        protected RbUnit m_Device;
        protected static IEqpManager m_EqpManager;
        protected static ThreadRbControl m_Control;
        protected new int[] m_AlarmId;
        protected string log;
        #endregion

        #region Constructor
        public SeqRbMotorCondition(ThreadRbControl conrol, RbUnit device)
        {
            m_Device = device;
            m_EqpManager = m_Device.ServerManager.EqpStateManager;
            m_Control = conrol;

            m_SeqFunName = device.Name;

            m_AlarmId = new int[2];
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

                        RbMotor motor = m_Device.Motor;
                        if (motor.IsAlarm() && (m_AlarmId[0] == 0))
                        {
                            m_AlarmId[0] = motor.GetDriverAlarm().Id;
                            m_EqpManager.SetAlarm(m_AlarmId[0]);
                            log = string.Format("Alarm Set : {0} Driver Alarm", motor.Name);
                            m_Device.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if (!motor.IsAlarm() && (m_AlarmId[0] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId[0]);
                            m_AlarmId[0] = 0;
                            log = string.Format("Alarm Reset : {0} Driver Alarm Detect", motor.Name);
                            m_Device.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        if (!motor.IsCpOn() && (m_AlarmId[1] == 0))
                        {
                            m_AlarmId[1] = motor.GetCpAlarm().Id;
                            m_EqpManager.SetAlarm(m_AlarmId[1]);
                            log = string.Format("Alarm Set : {0} CP Off Alarm", motor.Name);
                            m_Device.SetLog(m_SeqFunName, 0, 0, log);
                        }
                        else if (motor.IsCpOn() && (m_AlarmId[1] != 0) && m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId[1]);
                            m_AlarmId[1] = 0;
                            log = string.Format("Alarm Reset : {0} CP Off Alarm Detect", motor.Name);
                            m_Device.SetLog(m_SeqFunName, 0, 0, log);
                        }

                        interlock |= (m_AlarmId[0] != 0);
                        interlock |= (m_AlarmId[1] != 0);

                        if (interlock && m_Device.RbMotorCond)
                        {
                            m_Device.RbMotorCond = false;
                        }
                        else if (!interlock && !m_Device.RbMotorCond)
                        {
                            m_Device.RbMotorCond = true;
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

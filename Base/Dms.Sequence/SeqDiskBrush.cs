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
    public class ThreadDBControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<DBUnit> m_DBUnits;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            if (m_DBUnits.Count == 0) return;

            foreach (DBUnit device in m_DBUnits)
            {
                RegisterSequence(new SeqDBMotor(this, device));
                RegisterSequence(new SeqDBMotorCondition(this, device));
                if (device.ServoUnit != null)
                {
                    RegisterSequence(new SeqDBGap(this, device));
                }
            }

            m_Server.AddSeqInitFunction(new SeqInitDB(this, m_Server));
        }
        #endregion

        #region Constructor
        public ThreadDBControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_DBUnits = DmsComponents.Instance.ComponentContainer.GetCollection<DBUnit>();
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
        public static _GenericCollection<DBUnit> Units
        {
            get
            {
                if (m_DBUnits == null) m_DBUnits = new _GenericCollection<DBUnit>();
                return m_DBUnits;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual bool IsAlarmCondition(DBUnit unit)
        {
            bool alarm = false;
            alarm |= unit.Motor.DiAlarm.GetState();
            alarm |= !unit.Motor.DiCpOn.GetState();

            return alarm;
        }

        public virtual bool IsInterlockCondition(DBUnit unit)
        {
            bool interlock = false;

            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            //interlock |= ((heavy & ~HeavyInterlock.Door) > 0 );
            interlock |= (heavy > 0);

            return interlock;
        }

        public virtual bool IsRunCondition(DBUnit unit)
        {
            // TODO : write specific code
            bool run = true;
            run &= IsUse(unit);
            run &= m_Server.JobCond.ProcessMode;
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= (GetGlassCount() > 0 || (m_GenInfos.IdleRunning && GetGlassCount() == 0));

            return run;
        }

        public virtual bool IsUse(DBUnit unit)
        {
            return m_Server.JobCond.DBUse(unit);
        }

        public virtual bool IsNoUseRunCondition(DBUnit unit)
        {
            bool run = true;
            run &= !IsUse(unit);
            run &= m_GenInfos.AutoMode;
            run &= !m_GenInfos.Pause;
            run &= m_GenInfos.DiStart;
            run &= (GetGlassCount() > 0);
            return run;
        }

        public virtual double GetRefSpeed(DBUnit unit)
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
                    unit.ProcSpeed = m_Server.JobCond.DBProcessSpeed(unit);
                    unit.ProcSpeed = unit.ProcSpeed * (m_Server.JobCond.DBDirection(unit) ? 1 : -1);
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
            //    unit.ProcSpeed = unit.ProcSpeed * (m_Server.JobCond.DBDirection(unit) ? 1 : -1);
            //    return unit.ProcSpeed;
            //}
            else
            {
                return 0.0;
            }
        }

        public virtual Dms.Device.DBUnit.DBUnitAct GetRefPosAct(DBUnit unit)
        {
            return DBUnit.DBUnitAct.Noop;
        }

        public virtual bool GetDBCond(DBUnit unit)
        {
            bool cond = true;

            if (!IsUse(unit))
            {
                cond &= (unit.RefSpeed == 0.0 || Math.Abs(unit.RefSpeed) == 100.0);
                cond &= (Math.Abs(unit.RefSpeed - unit.CurSpeed) < 50);
            }
            else
            {
                cond &= unit.DBMotorCond;
                //cond &= (unit.RefSpeed != 0.0 || unit.ProcSpeed != 0.0);
                cond &= (unit.RefSpeed != 0.0 || m_Server.JobCond.DBProcessSpeed(unit) == 0.0);
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
            int glassNo = 0;

            //for (int i = eqpCvUnits._AP_CvUnit.Id * 2; i < eqpCvUnits._UL1_CvUnit.Id * 2; i++)
            //{
            //    if (m_Server.GlassData.IsExist(i)) glassNo++;
            //}

            return glassNo;
        }
        #endregion

        #region General Methods
        #endregion
    }

    public class SeqDBGap : XSeqFunction
    {
        #region Fields
        protected const double m_Margin = 0.05;
        protected static IEqpManager m_EqpManger;
        protected static ThreadDBControl m_Control;
        protected DBUnit m_Unit;
        protected static _GenInfoHandler m_GenInfos;
        protected static IServerManager m_Server;
        #endregion

        #region Constructor
        public SeqDBGap(ThreadDBControl control, DBUnit db)
        {
            m_Unit = db;
            m_EqpManger = m_Unit.ServerManager.EqpStateManager;
            m_Control = control;
            m_Server = m_Unit.ServerManager;
            m_GenInfos = GenInfoHandler.Instance;
            m_Unit.DBManualModeChange = false;
            m_SeqFunName = "DB GAP";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;


            switch (nSeqNo)
            {
                case 0:
                    {
                        m_Unit.RefPosAct = m_Control.GetRefPosAct(m_Unit);

                        switch (m_Unit.RefPosAct)
                        {
                            case DBUnit.DBUnitAct.Noop:
                                {
                                    m_Unit.GapSetComp = true;
                                }
                                break;
                            case DBUnit.DBUnitAct.Off:
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
                            case DBUnit.DBUnitAct.Wait:
                                {
                                    m_Unit.RefPos = m_Unit.WaitPos;
                                    nSeqNo = 10;
                                }
                                break;
                            case DBUnit.DBUnitAct.Process:
                                {
                                    m_Unit.ProcGap = m_Unit.ServerManager.JobCond.DBGap(m_Unit);
                                    m_Unit.RefPos = m_Unit.ProcPos;
                                    nSeqNo = 10;
                                }
                                break;
                            case DBUnit.DBUnitAct.Zero:
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
                    {
                        nSeqNo = 600;
                    }
                    else if (!m_Unit.ServoUnit.HomeComp)
                    {
                        m_EqpManger.SetAlarm(m_Unit.ALM_ServoMoveFail.Id);
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Set : DB Servo moving failed");
                        nSeqNo = 1000;
                    }
                    else if (0 == (result = m_Unit.ServoUnit.RbtMovePos(m_Unit.RefPos)))
                    {
                        m_Unit.GapSetComp = true;
                        nSeqNo = 0;
                    }
                    else if (0 < result)
                    {

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
                        m_Unit.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : DB Servo moving failed");
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

    public class SeqDBMotor : XSeqFunction
    {
        #region Fields
        protected const double m_Gain = 0.2;
        protected const double m_Limit = 1.0;
        protected DBUnit m_Unit;
        protected static IServerManager m_Server;
        protected static ThreadDBControl m_Control;
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
        public SeqDBMotor(ThreadDBControl control, DBUnit db)
        {
            m_Unit = db;
            m_Server = m_Unit.ServerManager;
            m_Control = control;

            m_SeqFunName = "DB MOTOR";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
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

    public class SeqInitDB : XSeqInitFunction
    {
        #region Fields
        protected InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static _GenericCollection<DBUnit> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private ThreadDBControl m_Control;
        private int m_ServoNo;
        private List<int> m_ServoIndex = new List<int>();
        private bool[] m_ServoUse;
        private string m_Msg;
        private int m_Rv = -1;
        #endregion

        #region Constructor
        public SeqInitDB(ThreadDBControl control, IServerManager server)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<DBUnit>();
            m_Eqp = m_Server.EqpStateManager;
            m_Control = control;
            m_GenInfos = GenInfoHandler.Instance;

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

                        int count = m_Units.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (m_Units[i].ServoUnit != null)
                            {
                                m_ServoIndex.Add(i);
                            }
                        }

                        if (m_ServoIndex.Count != 0)
                        {
                            m_Server.Log("DB Servo E-Stop Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_Server.Log("No DB Servo Unit");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 60;
                        }
                    }
                    break;
                case 20:
                    if (true == m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtEStop())
                    {
                        m_Msg = string.Format("DB ServoUnit{0}: E-Stop Complete", m_ServoNo);
                        m_Server.Log(m_Msg);

                        m_ServoNo++;

                        if (m_ServoNo < m_ServoIndex.Count)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (m_ServoNo >= m_ServoIndex.Count)
                        {
                            m_ServoNo = 0;
                            m_ServoUse = new bool[m_ServoIndex.Count];

                            int count = m_ServoIndex.Count;
                            for (int i = 0; i < count; i++)
                            {
                                m_ServoUse[i] = m_Units[m_ServoIndex[i]].SetupServoUse.GetValue<bool>();
                                m_ServoUse[i] &= m_Server.JobCond.ProcessMode;
                            }

                            m_StartTicks = XFunc.GetTickCount();

                            nSeqNo = 30;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitState = InitState.Fail;
                        m_AlarmId = m_Units[m_ServoIndex[m_ServoNo]].ALM_ServoEstopFail.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Server.Log("E-Stop Fail Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 30:
                    if (!m_ServoUse[m_ServoNo] || m_Units[m_ServoIndex[m_ServoNo]].ServoUnit.RbtReset())
                    {
                        if (m_ServoUse[m_ServoNo]) m_Msg = string.Format("DB Servo{0} Reset Complete", m_ServoNo);
                        else m_Msg = string.Format("DB Servo{0} Reset Skip", m_ServoNo);
                        m_Server.Log(m_Msg);

                        m_ServoNo++;

                        if (m_ServoNo < m_ServoIndex.Count)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else if (m_ServoNo >= m_ServoIndex.Count)
                        {
                            m_ServoNo = 0;
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitState = InitState.Fail;
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
                        if (m_ServoUse[m_ServoNo]) m_Msg = string.Format("DB Servo{0} Homing Complete", m_ServoNo);
                        else m_Msg = string.Format("DB Servo{0} Homing Skip", m_ServoNo);
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

    public class SeqDBMotorCondition : XSeqFunction
    {
        #region Fields
        protected DBUnit m_Device;
        protected static IEqpManager m_EqpManager;
        protected static ThreadDBControl m_Control;
        protected new int[] m_AlarmId;
        protected string log;
        #endregion

        #region Constructor
        public SeqDBMotorCondition(ThreadDBControl conrol, DBUnit device)
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

                        DBMotor motor = m_Device.Motor;
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

                        if (interlock && m_Device.DBMotorCond)
                        {
                            m_Device.DBMotorCond = false;
                        }
                        else if (!interlock && !m_Device.DBMotorCond)
                        {
                            m_Device.DBMotorCond = true;
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

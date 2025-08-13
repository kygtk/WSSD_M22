using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadAbsodexMotor : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<AbsodexMotor> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        #endregion

        #region Properties

        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (AbsodexMotor device in m_Units)
            {
                RegisterSequence(new SeqAbsodexMotor(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadAbsodexMotor(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<AbsodexMotor>();
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
        public static _GenericCollection<AbsodexMotor> Units
        {
            get
            {
                if (m_Units == null) m_Units = new _GenericCollection<AbsodexMotor>();
                return m_Units;
            }
        }
        #endregion

        #region Virtual Methods
        public virtual ActuatorAct GetRefMotorAct(AbsodexMotor motor)
        {
            ActuatorAct act = ActuatorAct.Noop;

            // 인터락 조건들을 봐야 하겠지


            if (!m_GenInfos.AutoMode)
            {
                act = motor.GetRefAct();
            }
            else // Auto Mode 일때는 조건을 봐야 한다.
            {
                act = motor.GetRefAct();
                if (motor.IsInterlockCondition(act))
                {
                    act = ActuatorAct.EStop;
                }
            }

            return act;
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqAbsodexMotor : XSeqFunction
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManger;
        protected static ThreadAbsodexMotor m_Control;
        protected static AlarmListProvider m_AlarmList;
        private AbsodexMotor m_Unit;
        private ActuatorAct m_RefAct;
        private TagAlarm m_Alarm = new TagAlarm();
        #endregion

        #region Constructor
        public SeqAbsodexMotor(ThreadAbsodexMotor control, AbsodexMotor unit)
        {
            m_Unit = unit;
            m_Server = m_Unit.ServerManager;
            m_EqpManger = m_Server.EqpStateManager;
            m_Control = control;
            m_AlarmList = AlarmListProvider.Instance;

            m_SeqFunName = "Absodex";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            ActuatorAct refAct = m_Control.GetRefMotorAct(m_Unit);

            switch (nSeqNo)
            {
                case 0:
                    {
                        switch (refAct)
                        {
                            case ActuatorAct.Pos:
                                if (m_Unit.GetCurAct() == refAct)
                                {
                                    nSeqNo = 0;
                                }
                                else
                                {
                                    m_RefAct = refAct;
                                    m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Req : " + m_RefAct.ToString());
                                    nSeqNo = 100;
                                }
                                break;
                            case ActuatorAct.Stop:
                                if (m_Unit.GetCurAct() == refAct)
                                {
                                    nSeqNo = 0;
                                }
                                else
                                {
                                    m_RefAct = refAct;
                                    m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Req : " + m_RefAct.ToString());
                                    nSeqNo = 400;
                                }
                                break;
                            case ActuatorAct.EStop:
                                if (m_Unit.GetCurAct() == refAct)
                                {
                                    nSeqNo = 0;
                                }
                                else
                                {
                                    m_RefAct = refAct;
                                    m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Req : " + m_RefAct.ToString());
                                    nSeqNo = 500;
                                }
                                break;
                            case ActuatorAct.AlarmReset:
                                if (m_Unit.GetCurAct() == refAct)
                                {
                                    nSeqNo = 0;
                                }
                                else
                                {
                                    m_RefAct = refAct;
                                    m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Req : " + m_RefAct.ToString());
                                    nSeqNo = 600;
                                }
                                break;

                        }
                    }
                    break;
                case 100: //Move Command
                    {
                        if (refAct == ActuatorAct.EStop)
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Cancel : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 == (result = m_Unit.MotorAct()))
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Complete : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 < result)
                        {
                            m_AlarmId = result;
                            m_EqpManger.SetAlarm(m_AlarmId);

                            m_AlarmList.GetAlarm(m_AlarmId, m_Alarm);
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAlarm : " + m_Alarm.Name);

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 400: //Stop Command
                    {
                        if (refAct == ActuatorAct.EStop)
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Cancel : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 == (result = m_Unit.MotorAct()))
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Complete : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 < result)
                        {
                            m_AlarmId = result;
                            m_EqpManger.SetAlarm(m_AlarmId);

                            m_AlarmList.GetAlarm(m_AlarmId, m_Alarm);
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAlarm : " + m_Alarm.Name);

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 500: //EStop Command
                    {
                        if (0 == (result = m_Unit.MotorAct()))
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Complete : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 < result)
                        {
                            m_AlarmId = result;
                            m_EqpManger.SetAlarm(m_AlarmId);

                            m_AlarmList.GetAlarm(m_AlarmId, m_Alarm);
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAlarm : " + m_Alarm.Name);

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 600: //Alarm Reset Command
                    {
                        if (refAct == ActuatorAct.EStop)
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Cancel : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 == (result = m_Unit.MotorAct()))
                        {
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAct Complete : " + m_RefAct.ToString());
                            nSeqNo = 0;
                        }
                        else if (0 < result)
                        {
                            m_AlarmId = result;
                            m_EqpManger.SetAlarm(m_AlarmId);

                            m_AlarmList.GetAlarm(m_AlarmId, m_Alarm);
                            m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAlarm : " + m_Alarm.Name);

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.SetAlarm(m_AlarmId);

                        m_AlarmList.GetAlarm(m_AlarmId, m_Alarm);
                        m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "SetAlarm : " + m_Alarm.Name);
                        nSeqNo = 1100;
                    }
                    break;
                case 1100:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_EqpManger.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Unit.SetLog(m_Unit.Name, this.m_SeqFunName, 0, 0, "ResetAlarm : " + m_Alarm.Name);

                        nSeqNo = 0;
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return result;
        }
        #endregion
    }
}

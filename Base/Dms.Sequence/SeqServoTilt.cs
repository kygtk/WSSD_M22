using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadServoTiltControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<ServoTilt> m_ServoTilts;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (ServoTilt device in m_ServoTilts)
            {
                //RegisterSequence(new SeqRbMotor(this, device));
                if (device.ServoUnit != null) RegisterSequence(new SeqServoTiltPos(this, device));
                //RegisterSequence(new SeqRbMotorCondition(this, device));
            }
        }
        #endregion

        #region Constructor
        public ThreadServoTiltControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_ServoTilts = DmsComponents.Instance.ComponentContainer.GetCollection<ServoTilt>();

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
        public static _GenericCollection<ServoTilt> Units
        {
            get
            {
                if (m_ServoTilts == null) m_ServoTilts = new _GenericCollection<ServoTilt>();
                return m_ServoTilts;
            }
        }
        #endregion        

        #region Virtual Methods
        public virtual bool CheckInterlock()
        {
            bool rv = false;

            return rv;
        }

        public virtual int GetCurPos(int rbtId)
        {
            int rv = -1;

            switch (rbtId)
            {
                case 0:
                    {
                        //if (eqpSensor_Ios._DEV4_1_Unit_Tilt_Down_Sensor.IsDetected()&&
                        //    !eqpSensor_Ios._DEV4_1_Unit_Tilt_Up_Sensor.IsDetected()) rv = 1;
                        //if (!eqpSensor_Ios._DEV4_1_Unit_Tilt_Down_Sensor.IsDetected() &&
                        //    eqpSensor_Ios._DEV4_1_Unit_Tilt_Up_Sensor.IsDetected()) rv = 2;
                    }
                    break;

                case 1:
                    {
                        //if (eqpSensor_Ios._DEV4_2_Unit_Tilt_Down_Sensor.IsDetected() &&
                        //    !eqpSensor_Ios._DEV4_2_Unit_Tilt_Up_Sensor.IsDetected()) rv = 1;
                        //if (!eqpSensor_Ios._DEV4_2_Unit_Tilt_Down_Sensor.IsDetected() &&
                        //    eqpSensor_Ios._DEV4_2_Unit_Tilt_Up_Sensor.IsDetected()) rv = 2;
                    }
                    break;
            }

            return rv;
        }
        #endregion

        #region Gerneral Methods
        #endregion
    }

    public class SeqServoTiltPos : XSeqFunction
    {
        #region Fields
        protected const double m_Margin = 0.05;
        protected ServoTilt m_ServoTilt;
        protected static IEqpManager m_EqpManger;
        protected static IServerManager m_Server;
        protected static ThreadServoTiltControl m_Control;
        protected bool simulation;
        #endregion

        #region Constructor
        public SeqServoTiltPos(ThreadServoTiltControl control, ServoTilt tilt)
        {
            m_ServoTilt = tilt;
            m_Server = m_ServoTilt.ServerManager;
            m_EqpManger = m_Server.EqpStateManager;
            m_Control = control;
            simulation = AppConfig.Instance.Simul.Motion;

            m_SeqFunName = "Tilt Pos";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int result = -1;
            int nSeqNo = this.m_SeqNo;

            if (!AppConfig.Instance.Simul.Motion) m_ServoTilt.CurPos.Pos[0] = m_ServoTilt.ServoUnit.Axis[0].GetPosition();

            //m_ServoTilt.CurPos.Pos[0] = m_ServoTilt.ServoUnit.Axis[0].GetPosition();

            switch (nSeqNo)
            {
                case 0:
                    if (m_ServoTilt.RefAct != m_ServoTilt.CurAct)
                    {
                        switch (m_ServoTilt.RefAct)
                        {
                            case ActuatorAct.Noop:
                                {

                                }
                                break;

                            case ActuatorAct.Initialize:
                                {


                                }
                                break;

                            case ActuatorAct.EStop:
                                {
                                    nSeqNo = 500;
                                }
                                break;

                            case ActuatorAct.Stop:
                                {
                                    nSeqNo = 500;
                                }
                                break;

                            case ActuatorAct.Pos:
                                {
                                    m_ServoTilt.RefPos = m_ServoTilt.UpPos;

                                    nSeqNo = 10;
                                }
                                break;

                            case ActuatorAct.Neg:
                                {
                                    m_ServoTilt.RefPos = m_ServoTilt.DownPos;
                                    nSeqNo = 10;
                                }
                                break;
                        }
                    }
                    break;

                case 10:
                    {
                        if (simulation)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 50;
                        }
                        else if (Math.Abs(m_ServoTilt.CurPos.Pos[0] - m_ServoTilt.RefPos.Pos[0]) < m_Margin)
                        {   // Set complete
                            if (true == m_ServoTilt.ServoUnit.Ready)
                            {
                                m_ServoTilt.CurAct = m_ServoTilt.RefAct;

                                m_ServoTilt.RefAct = ActuatorAct.Noop;
                                m_ServoTilt.PosSetComp = true;

                                nSeqNo = 0;
                            }
                            else
                            {   // servo off condition : Reset Servo
                                nSeqNo = 600;
                            }
                        }
                        else
                        {   // change position
                            if (!m_ServoTilt.HomeComp)
                            {   // prev. progress is not complete : Stop -> Reset Servo
                                nSeqNo = 500;
                            }
                            else
                            {   // prev. progress is complete
                                m_ServoTilt.PosSetComp = false;
                                nSeqNo = 100;
                            }
                        }
                    }
                    break;

                case 50:
                    {
                        if (GetElapsedTicks() > m_ServoTilt.MoveTimeout * 500)
                        {
                            //if (m_ServoTilt.RefAct == ActuatorAct.Pos)
                            //m_ServoTilt.CurAct = m_ServoTilt.RefAct;
                            //m_ServoTilt.RefAct = ActuatorAct.Noop;

                            //m_ServoTilt.PosSetComp = true;

                            if (m_ServoTilt.RefAct == ActuatorAct.Pos)
                            {
                                m_ServoTilt.DiUpSensor.SetState(true);
                                m_ServoTilt.DiDnSensor.SetState(false);

                                m_ServoTilt.CurAct = ActuatorAct.Pos;
                            }

                            else if (m_ServoTilt.RefAct == ActuatorAct.Neg)
                            {
                                m_ServoTilt.DiUpSensor.SetState(false);
                                m_ServoTilt.DiDnSensor.SetState(true);

                                m_ServoTilt.CurAct = ActuatorAct.Neg;
                            }

                            m_ServoTilt.RefAct = ActuatorAct.Noop;
                            m_ServoTilt.PosSetComp = true;

                            nSeqNo = 0;
                        }
                    }
                    break;


                case 100:
                    if (!m_ServoTilt.ServoUnit.Ready)
                    {
                        nSeqNo = 600;
                    }
                    else if (!m_ServoTilt.ServoUnit.HomeComp)
                    {
                        m_EqpManger.SetAlarm(m_ServoTilt.ALM_ServoMoveFail.Id);
                        m_ServoTilt.SetLog(m_SeqFunName, 0, 0, "Alarm Set : RB Servo moving failed");
                        nSeqNo = 1000;
                    }
                    else if (0 == (result = m_ServoTilt.ServoUnit.RbtMovePos(m_ServoTilt.RefPos)))
                    {
                        m_ServoTilt.CurAct = m_ServoTilt.RefAct;

                        m_ServoTilt.RefAct = ActuatorAct.Noop;
                        m_ServoTilt.PosSetComp = true;

                        nSeqNo = 0;
                    }
                    else if (0 < result)
                    {

                    }
                    break;

                case 500:
                    if (true == m_ServoTilt.ServoUnit.RbtEStop())
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 600:
                    if (true == m_ServoTilt.ServoUnit.RbtReset())
                    {
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManger.AlarmResetSwitchPushed)
                    {
                        m_ServoTilt.SetLog(m_SeqFunName, 0, 0, "Alarm Reset : RB Servo moving failed");
                        m_EqpManger.ResetAlarm(m_ServoTilt.ALM_ServoMoveFail.Id);

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

            #region OLD
            //switch (nSeqNo)
            //{
            //    case 0:
            //        {                        
            //            {
            //                switch (servoTilt.RefPosAct)
            //                {
            //                    case ServoTilt.TiltAct.Noop:
            //                        {
            //                            servoTilt.PosSetComp = true;
            //                        }
            //                        break;
            //                    case ServoTilt.TiltAct.Off:
            //                        {
            //                            if (!servoTilt.ServoUnit.Ready)
            //                            {
            //                                servoTilt.PosSetComp = true;
            //                            }
            //                            else
            //                            {
            //                                servoTilt.PosSetComp = false;
            //                                nSeqNo = 500;
            //                            }
            //                        }
            //                        break;
            //                    case ServoTilt.TiltAct.Up:
            //                        {
            //                            servoTilt.RefPos = servoTilt.UpPos;
            //                            nSeqNo = 10;
            //                        }
            //                        break;
            //                    case ServoTilt.TiltAct.Down:
            //                        {
            //                            servoTilt.RefPos = servoTilt.DownPos;
            //                            nSeqNo = 10;
            //                        }
            //                        break;
            //                }
            //            }
            //        }
            //        break;
            //    case 10:
            //        {
            //            if (Math.Abs(servoTilt.CurPos.Pos[0] - servoTilt.RefPos.Pos[0]) < m_Margin)
            //            {   // Set complete
            //                if (true == servoTilt.ServoUnit.Ready)
            //                {
            //                    servoTilt.PosSetComp = true;
            //                    nSeqNo = 0;
            //                }
            //                else
            //                {   // servo off condition : Reset Servo
            //                    servoTilt.PosSetComp = false;
            //                    nSeqNo = 600;
            //                }
            //            }
            //            else
            //            {   // change position
            //                if (!servoTilt.PosSetComp)
            //                {   // prev. progress is not complete : Stop -> Reset Servo
            //                    nSeqNo = 500;
            //                }
            //                else
            //                {   // prev. progress is complete
            //                    servoTilt.PosSetComp = false;
            //                    nSeqNo = 100;
            //                }
            //            }
            //        }
            //        break;

            //    case 100:
            //        if (!servoTilt.ServoUnit.Ready)
            //        {
            //            nSeqNo = 600;
            //        }
            //        else if (!servoTilt.ServoUnit.HomeComp)
            //        {
            //            m_EqpManger.SetAlarm(servoTilt.ALM_ServoMoveFail.Id);
            //            servoTilt.SetLog(SeqFunName, 0, 0, "Alarm Set : RB Servo moving failed");
            //            nSeqNo = 1000;
            //        }
            //        else if (0 == (result = servoTilt.ServoUnit.RbtMovePos(servoTilt.RefPos)))
            //        {
            //            servoTilt.PosSetComp = true;
            //            nSeqNo = 0;
            //        }
            //        else if (0 < result)
            //        {

            //        }
            //        else
            //        {
            //            nSeqNo = 0;
            //        }
            //        break;
            //    case 500:
            //        if (true == servoTilt.ServoUnit.RbtEStop())
            //        {
            //            nSeqNo = 0;
            //        }
            //        break;
            //    case 600:
            //        if (true == servoTilt.ServoUnit.RbtReset())
            //        {
            //            nSeqNo = 0;
            //        }
            //        break;
            //    case 1000:
            //        if (m_EqpManger.AlarmResetSwitchPushed)
            //        {
            //            servoTilt.SetLog(SeqFunName, 0, 0, "Alarm Reset : RB Servo moving failed");
            //            m_EqpManger.ResetAlarm(servoTilt.ALM_ServoMoveFail.Id);

            //            m_StartTicks = XFunc.GetTickCount();
            //            nSeqNo = 1010;
            //        }
            //        break;
            //    case 1010:
            //        if (GetElapsedTicks() > 2000)
            //        {
            //            nSeqNo = 0;
            //        }
            //        break;

            //}
            #endregion

            this.m_SeqNo = nSeqNo;

            return result;
        }
        #endregion
    }

    public class SeqInitServoTilt : XSeqInitFunction
    {
        #region Fields
        protected InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static IEqpManager m_Eqp;
        protected static ThreadServoTiltControl m_Control;
        protected static _GenericCollection<ServoTilt> m_Units;
        protected static _GenInfoHandler m_GenInfos;
        private new int[] m_AlarmId;
        #endregion

        #region Constructor
        public SeqInitServoTilt(ThreadServoTiltControl control, IServerManager server)
        {
            m_Server = server;
            m_Eqp = m_Server.EqpStateManager;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<ServoTilt>();
            m_Control = control;
            m_AlarmId = new int[m_Units.Count];
            m_GenInfos = GenInfoHandler.Instance;

            this.m_SeqFunName = "INIT    ";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_InitState == InitState.Comp) return (int)m_InitState;

            ServoTilt Dev4_1_ServoTilt = new ServoTilt();// eqpServoTilts._DEV4_1_Servo_Tilt_Unit;
            ServoTilt Dev4_2_ServoTilt = new ServoTilt();// eqpServoTilts._DEV4_2_Servo_Tilt_Unit;

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfos.EqpInitReq)
                    {
                        m_InitState = InitState.Init;
                        //StartTime = DateTime.Now;
                        m_StartTicks = XFunc.GetTickCount();
                        //m_ServoNo = 0;
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    // Glass Exist?
                    // 100 : Homming
                    // 200 : Set Position
                    break;


                case 100:
                    if (Dev4_1_ServoTilt.ServoUnit.RbtReset())
                    {
                        nSeqNo = 110;
                    }
                    break;

                case 110:
                    if (0 == Dev4_1_ServoTilt.ServoUnit.RbtMoveHome())
                    {


                        nSeqNo = 120;
                    }
                    break;

                case 120:
                    if (Dev4_2_ServoTilt.ServoUnit.RbtReset())
                    {

                        nSeqNo = 130;
                    }
                    break;

                case 130:
                    if (0 == Dev4_2_ServoTilt.ServoUnit.RbtMoveHome())
                    {
                        m_InitState = InitState.Comp;

                        nSeqNo = 0;
                    }
                    break;

                case 200:
                    {
                        int pos = -1;
                        pos = m_Control.GetCurPos(Dev4_1_ServoTilt.Id);

                        if (pos > -1)
                        {
                            Dev4_1_ServoTilt.ServoUnit.SetCurPosition((short)pos);

                            nSeqNo = 210;
                        }
                        else
                        {
                            // Set Alarm

                        }
                    }
                    break;

                case 210:
                    {
                        int pos = -1;
                        pos = m_Control.GetCurPos(Dev4_2_ServoTilt.Id);

                        if (pos > -1)
                        {
                            Dev4_2_ServoTilt.ServoUnit.SetCurPosition((short)pos);

                            m_InitState = InitState.Comp;

                            nSeqNo = 0;
                        }
                        else
                        {
                            // Set Alarm

                        }
                    }
                    break;

                case 1000:
                    {

                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }
}

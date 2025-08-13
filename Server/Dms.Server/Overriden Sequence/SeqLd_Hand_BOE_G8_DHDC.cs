using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using System.Threading;

namespace Dms.Server
{
    public class ThreadLd_Hand_BOE_G8_DHDC : ThreadHandControl
    {
        private GenInfoHandler m_GenInfo;
        private XLog LdHandLog; // 11.02.25 minhan

        public ThreadLd_Hand_BOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {
            m_GenInfo = GenInfoHandler.Instance;
            LdHandLog = new XLog("LD Hand Log", XLog.LogStampType.UseStamp); // 11.02.25 minhan
        }

        #region override
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqUnitLdFish(this, eqpTransferUnits._LD_Fish_Hand));
            RegisterSequence(new SeqUnitLdFishAirInterlock(this, eqpTransferUnits._LD_Fish_Hand));
        }
        public override void InitParameter() // 09.05.30 minhan override 해서 사용
        {
            FishHand unit = eqpTransferUnits._LD_Fish_Hand;
            if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
            unit.IfFlag.Reset();
			unit.IfFlag.InComp = false;
            unit.IfFlag.OutComp = false; // 09.05.30 minhan ldcv 와 연동으로 인하여 초기에 false.검토중
        }
        #endregion

        #region Methods
        public void SetLog(string seqName, int seqNo, int portNo, int slotNo, string message) // 11.02.25 minhan
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

            log = string.Format("LdHand  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            LdHandLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }

        public bool IsSeqRunCondition(FishHand fs) // minhan
        {
            bool run = true;
            run &= !IsInterlock();
            // run &= !CvStopCondition(cv.NextCv);
            run &= !m_GenInfo.Pause;
            run &= m_GenInfo.AutoMode;
            run &= !GlobalVar.LdHandAirInterlock;
            return run;
        }
        public bool IsInterlock()
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool Interlock = false;
            //Interlock |= ((heavy & ~HeavyInterlock.Emo) > 0 ? true : false);
            //Interlock |= ((heavy & ~HeavyInterlock.Leak) > 0 ? true : false);//2010.06.29 kimgun
            Interlock |= ((heavy & HeavyInterlock.Door) > 0 ? true : false);//2010.06.29 kimgun
            Interlock |= ((heavy & HeavyInterlock.Area) > 0 ? true : false);//2010.06.29 kimgun
            return Interlock;
        }

		// LD Hand Position Sensors
		// After Recv(RECV3) + MT
		// Before Recv(RECV2)
		// TR Access(RECV1) + MT
		// Wait(WAIT)
		// Before Send(SEND1)
		// After Send(SEND2) == Home
		public bool IsPositionConfirmed(short posId)
		{
            if (posId == eqpPos_LD_Hand_Servo_Unit.Home_Position) // 11.01.27 minhan 제발 동일하게 하삼..
            {
                //return eqpServoMotorMp2300s._LD_Hand_Servo_Motor.GetHomeSwitch();
                //return eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.IsDetected();
                if (!m_Server.Simul.Device)
                {
                    return eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
                }
                else
                {
                    return eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.IsDetected();
                }
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Send2_Position)
            {
                //return eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.IsDetected();
                //return eqpServoMotorMp2300s._LD_Hand_Servo_Motor.GetHomeSwitch();
                if (!m_Server.Simul.Device)
                {
                    return eqpServoMotors._LD_Hand_Servo_Motor.GetHomeSwitch();
                }
                else
                {
                    return eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.IsDetected();
                }
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Send1_Position)
            {
                return eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected();
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Wait_Position)
            {
                return eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected();
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv1_Position)
            {
                return eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected();
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv2_Position)
            {
                return eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected();
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv3_Position)
            {
                return eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected();
            }
			return false;
		}

		/// <summary>
		/// only for simulation
		/// </summary>
		/// <param name="posId"></param>
		public void SetPositionConfirmed(short posId) // 11.02.25 minhan 소스에 deabak 이라고 표기하는 것은 하지 마시기바랍니다.사고칠라면 하던가.
		{
            if (posId == eqpPos_LD_Hand_Servo_Unit.Home_Position)//deabak2
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Send2_Position)
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Send1_Position)
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Wait_Position)
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv1_Position)
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv2_Position)
            {
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(true);
            }
            else if (posId == eqpPos_LD_Hand_Servo_Unit.Recv3_Position)
            {
                eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.DiSensor.SetState(false);
                eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.DiSensor.SetState(true);
            }
		}
         #endregion
    }

    public class SeqUnitLdFish : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        private GenInfoHandler m_GenInfo; // 11.02.25 minhan
        private Simul m_Simul;
        private FishHand m_LdHandUnit;
        #endregion

        #region Unit
        private TagGlassData m_RecvGlassData = new TagGlassData();
        private GantryUnit m_GantryUnit = null;
        private ThreadLd_Hand_BOE_G8_DHDC m_Control;
        private CvUnit m_LdCvUnit = null;
        private _ServoUnit m_LdServoUnit;
        private ActuatorUnit m_Aligner = null;
        private double servoCurPos = 0.0;
        private short m_GantryHomePosition;
        private short m_GantrySendPosition;
        private short m_GantryWaitPosition;
        private short m_GantryRecvPosition;
        private short m_LdHandHomePosition;
        private short m_LdHandRecv1Position;
        private short m_LdHandRecv2Position;
        private short m_LdHandRecv3Position;
        private short m_LdHandWaitPosition;
        private short m_LdHandSend1Position;
        private short m_LdHandSend2Position;
        private double CHANGE_VEL = 85.0; // 11.04.22 minhan
        private double OriginalVel = 0;
        private int VelocityChangeCount = 0; // 09.12.08 minhan
        private Alarm m_AlarmVelocityChange; // 09.12.08 minhan
        private Alarm m_ManualChangeErrLD; // 11.02.25 minhan
        private Alarm m_ServoNotReady; // 11.02.25 minhan
        private int m_AlarmIdServo; // 11.02.25 minhan
        //private float GetLoadRatio; // 11.02.25 minhan
        private ThreadTr_BOE_G8_DHDC m_TrControl;
		private ThreadUl_Hand_BOE_G8_DHDC m_UlHandControl;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private bool chkLdCondition; // 11.02.25 minhan 
        #endregion

        #region Constructor
        public SeqUnitLdFish(ThreadLd_Hand_BOE_G8_DHDC control, FishHand LdHand)
        {
            m_LdHandUnit = LdHand;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
            m_GenInfo = m_Server.GenInfos as GenInfoHandler;
            SeqFunName = m_LdHandUnit.Name;
            m_LdCvUnit = eqpTransferUnits._LD_CvUnit;
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_LdServoUnit = eqpServoUnits._LD_Hand_Servo_Unit;
            m_Aligner = eqpActuatorUnits._LD_Align;

            m_GantryHomePosition = eqpPos_TR_Servo_Unit.Home_Position;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send_Position;
            m_GantryWaitPosition = eqpPos_TR_Servo_Unit.Wait_Position;
            m_GantryRecvPosition = eqpPos_TR_Servo_Unit.Recv_Position;

            m_LdHandHomePosition = eqpPos_LD_Hand_Servo_Unit.Home_Position;
            m_LdHandRecv1Position = eqpPos_LD_Hand_Servo_Unit.Recv1_Position;
            m_LdHandRecv2Position = eqpPos_LD_Hand_Servo_Unit.Recv2_Position;
            m_LdHandRecv3Position = eqpPos_LD_Hand_Servo_Unit.Recv3_Position;
            m_LdHandWaitPosition = eqpPos_LD_Hand_Servo_Unit.Wait_Position;
            m_LdHandSend1Position = eqpPos_LD_Hand_Servo_Unit.Send1_Position;
            m_LdHandSend2Position = eqpPos_LD_Hand_Servo_Unit.Send2_Position;

            chkLdCondition = false; // 11.02.25 minhan
            //GetLoadRatio = 0; // 11.02.25 minhan
            OriginalVel = m_LdHandUnit.Servo.GetVel(0);//2009.08.17 kimgun
            m_AlarmVelocityChange = new Alarm(m_LdHandUnit.Name + " : Velocity Change Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.12.08 minhan
            m_ManualChangeErrLD = new Alarm(m_LdHandUnit.Name + " : Manual Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_ServoNotReady = new Alarm(m_LdHandUnit.Name + " : Servo Not Ready", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_AlarmIdServo = 0; // 11.02.25 minhan
		}
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_Server.GenInfos.EqpInitComp) return -1;
            if (!m_Control.IsSeqRunCondition(m_LdHandUnit)) return -1;
            if (m_LdHandUnit.Sequence[0] == null) m_LdHandUnit.Sequence[0] = this; // 09.05.30 minhan

			if (m_TrControl == null) m_TrControl = m_Server.ThreadHandler.TrControl;
            if (m_UlHandControl == null) m_UlHandControl = m_Server.ThreadHandler.UlHandControl;
            //2010.08.30 kimgun
            if (m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0)) &&
               (m_PortNo == 0) && (m_SlotNo == 0))
            {
                m_Server.GlassData.GetData(m_LdHandUnit.DataMatchingKey(0), ref m_RecvGlassData);
                m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            int seqNo = this.SeqNo;
            int Rv = -1;

            // 참 힘드네 Move 중에 인터락은 없고, 수정을 좀 많이 했습니다.
            switch (seqNo)
            {
                case 0:
                    {
                        if (AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(AlarmId);
                            AlarmId = 0;
                        }

                        if (m_AlarmIdServo > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmIdServo);
                            m_AlarmIdServo = 0;
                        }

                        if (OriginalVel != 0)//2009.08.17 kimgun 일단 속도 원복하자.
                            m_LdHandUnit.Servo.SetVel(0, OriginalVel);

                        if (VelocityChangeCount != 0) VelocityChangeCount = 0; // 09.12.08 minhan

                        int curTrPosition = m_GantryUnit.Servo.GetCurPointId();
                        int curLdUpPosition = m_LdHandUnit.Servo.GetCurPointId();

                        bool ldHandRibUp = m_LdHandUnit.IsHandUp();
                        bool ldHandRibDn = m_LdHandUnit.IsHandDown();

                        // tr Information
                        bool gtGlassExist = true;
                        gtGlassExist &= m_GantryUnit.IsGlassExist(Logic.AND);
                        gtGlassExist &= m_TrControl.IsPositionConfirmed(m_GantryWaitPosition);

                        // And you should check position of transfer unit whether it is at the wait position or not.
                        bool gtGlassDataExist = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        bool gtWaitPosSensor = m_TrControl.IsPositionConfirmed(m_GantryWaitPosition);
                        bool gtSendPosSensor = m_TrControl.IsPositionConfirmed(m_GantrySendPosition);
                        bool gtRecvPosSensor = m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);


                        if (m_Simul.Device && m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0))) m_LdHandUnit.GlassExistSensor.SetState(true, Logic.AND);

                        // LD buffer information
                        bool ldGlassDataExist = false;
                        ldGlassDataExist |= m_Server.GlassData.IsExist(m_LdCvUnit.DataMatchingKey(0));
                        ldGlassDataExist |= m_Server.GlassData.IsExist(m_LdCvUnit.DataMatchingKey(1));

                        bool ldGlassExist = false;
                        ldGlassExist |= m_LdCvUnit.GlsInSensor.IsDetected(Logic.OR);
                        ldGlassExist |= m_LdCvUnit.GlsOutSensor.IsDetected(Logic.OR);

                        // LD Hand Information
                        bool ldHandGlassExist = m_LdHandUnit.IsGlassExist(Logic.OR); // 11.02.25 minhan
                        bool ldHandDataExist = m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0));

                        // Condition
                        bool gtSendCondition = ((curTrPosition == m_GantryWaitPosition) & gtWaitPosSensor) |
                                               ((curTrPosition == m_GantryRecvPosition) & gtRecvPosSensor);

                        bool ldRecvCondition = (!ldGlassDataExist &
                                               !ldGlassExist &
                                               m_LdCvUnit.IfFlag.OutComp &
                                               (m_LdCvUnit.AutoAct == CvMotorAct.Stop) &
                                               (m_Aligner.IsNegative()));
                        // Error Case
                        if ((ldHandGlassExist || ldHandDataExist) && ldHandRibDn)
                        {
                            AlarmId = m_LdHandUnit.ALM_FishLiftAbnormal.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib abnormal Alarm(Rib Down)");
                            ReturnSeqNo = 0;
                            seqNo = 1000;
                        }
                        else if ((!ldHandGlassExist && ldHandDataExist) &&
                                ((curLdUpPosition != m_LdHandHomePosition) &&
                                (curLdUpPosition != m_LdHandRecv1Position) &&
                                (curLdUpPosition != m_LdHandSend2Position)) && !m_Simul.Motion)
                        {
                            AlarmId = m_LdHandUnit.ALM_FishDataAbnormal.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Data Alarm");
                            ReturnSeqNo = 0;
                            seqNo = 1000;
                        }
                        else if ((ldHandGlassExist || ldHandDataExist) & !ldHandRibUp)
                        {
                            AlarmId = m_LdHandUnit.ALM_FishLiftAbnormal.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib abnormal Alarm(Rib No Up)");
                            ReturnSeqNo = 0;
                            seqNo = 1000;
                        }
                        //2009.08.17 kimgun rib가 불안정하면 알람 발생.
                        else if (((ldHandRibDn && ldHandRibUp) ||
                                (!ldHandRibDn && !ldHandRibUp)))
                        {
                            AlarmId = m_LdHandUnit.ALM_FishLiftAbnormal.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib abnormal Alarm");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if ((!m_Control.IsPositionConfirmed(m_LdHandRecv1Position) &&
                                !m_Control.IsPositionConfirmed(m_LdHandRecv2Position) &&
                                !m_Control.IsPositionConfirmed(m_LdHandRecv3Position) &&
                                !m_Control.IsPositionConfirmed(m_LdHandWaitPosition) &&
                                !m_Control.IsPositionConfirmed(m_LdHandSend1Position) &&
                                !m_Control.IsPositionConfirmed(m_LdHandSend2Position) &&
                                !m_Control.IsPositionConfirmed(m_LdHandHomePosition)) ||
                                (curLdUpPosition == -1))
                        {//2009.09.14 kimgun case 0에서 position senosr가 하나도 감지 안 되면 NG
                            AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Position Not Detect Alarm");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if (!m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            AlarmId = m_ServoNotReady.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Servo Ready Alarm");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        #region case 1 : LD Hand Rib Down
                        else if (//!ldHandGlassExist && //LeeChungWon : Home에서 Gls 감지됨
                                !ldHandDataExist &&
                                gtSendCondition &&
                                ldHandRibUp &&
                                !ldHandRibDn &&
                                (((curLdUpPosition == m_LdHandSend2Position) && m_Control.IsPositionConfirmed(m_LdHandSend2Position)) || 
                                ((curLdUpPosition == m_LdHandHomePosition) && m_Control.IsPositionConfirmed(m_LdHandHomePosition)))) // 11.02.25 minhan
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Home or Send2 Position Start");
                            seqNo = 50;
                        }
                        #endregion

                        #region case 2 : LD Hand Recv1 Pos move
                        else if ((!ldHandGlassExist &&
                                 !ldHandDataExist &&
                                (ldHandRibUp && !ldHandRibDn) &&
                                gtGlassDataExist &&
                                ((curTrPosition == m_GantrySendPosition) && gtSendPosSensor) &&
                                ((curLdUpPosition == m_LdHandRecv1Position) && m_Control.IsPositionConfirmed(m_LdHandRecv1Position))) ||
                                (!ldHandGlassExist &&
                                !ldHandDataExist &&
                                gtSendCondition &&
                                (((curLdUpPosition == m_LdHandHomePosition) && m_Control.IsPositionConfirmed(m_LdHandHomePosition)) ||
                                ((curLdUpPosition == m_LdHandRecv1Position) && m_Control.IsPositionConfirmed(m_LdHandRecv1Position)) ||
                                ((curLdUpPosition == m_LdHandRecv3Position) && m_Control.IsPositionConfirmed(m_LdHandRecv3Position)) ||
                                ((curLdUpPosition == m_LdHandWaitPosition) && m_Control.IsPositionConfirmed(m_LdHandWaitPosition)) ||
                                ((curLdUpPosition == m_LdHandSend1Position) && m_Control.IsPositionConfirmed(m_LdHandSend1Position)) ||
                                ((curLdUpPosition == m_LdHandSend2Position) && m_Control.IsPositionConfirmed(m_LdHandSend2Position)))))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Home or Recv1 Position Start");
                            seqNo = 100;
                        }
                        else if ((ldHandGlassExist/* || m_Simul.Device*/) && //2009.08.26 kimgun 이자리는 무조건 glass가 감지되어야 한다.
                                !ldHandDataExist &&
                                (curLdUpPosition == m_LdHandRecv2Position) &&
                                m_Control.IsPositionConfirmed(m_LdHandRecv2Position) &&
                                gtGlassDataExist &&
                                (curTrPosition == m_GantrySendPosition) &&
                                gtSendPosSensor) // 11.02.25 minhan
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 150 Move");
                            seqNo = 150;
                        }
                        #endregion

                        #region 2009.08.15 kimgun tr에게 glass 인계 data가 없는경우
                        else if (ldHandGlassExist &&
                                !ldHandDataExist &&
                                (curLdUpPosition == m_LdHandRecv3Position) &&
                                m_Control.IsPositionConfirmed(m_LdHandRecv3Position) &&
                                (curTrPosition == m_GantrySendPosition))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 180 Move");
                            seqNo = 180;
                        }
                        //2009.08.15 kimgun tr에게 glass 인계 완료 후
                        else if (ldHandGlassExist &&
                                ldHandDataExist &&
                                (curLdUpPosition == m_LdHandRecv3Position) &&
                                m_Control.IsPositionConfirmed(m_LdHandRecv3Position) &&
                                (curTrPosition == m_GantrySendPosition))
                        {
                            //검토후 살리자.
                            m_LdHandUnit.IfFlag.InComp = true;
                            m_LdHandUnit.IfFlag.InReady = false;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 200 Move");
                            seqNo = 200;
                        }
                        #endregion

                        #region case 3: LD Hand has Glass..LD Buf not Ready..so Move Wait Position
                        else if (ldHandGlassExist &&
                                ldHandDataExist &&
                                gtSendCondition &&
                                !ldRecvCondition &&
                                (curLdUpPosition != m_LdHandWaitPosition))
                        {
                            // Velocity Change
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 400 Move");
                            seqNo = 400;
                        }
                        #endregion

                        #region case 4: LD Hand Send1 Pos move
                        else if (ldHandGlassExist &&
                                 ldHandDataExist &&
                                 (gtSendCondition || ((curLdUpPosition == m_LdHandWaitPosition) && m_Control.IsPositionConfirmed(m_LdHandWaitPosition))) &&
                                 ldRecvCondition &&
                                 ldHandRibUp &&
                                 !(m_Control.IsPositionConfirmed(m_LdHandHomePosition) || m_Control.IsPositionConfirmed(m_LdHandSend2Position))) // 11.04.14 minhan
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 600 Move");
                            seqNo = 600;
                        }
                        #endregion

                        #region case 5: Ld Hand send1 pos data not move
                        else if (!ldHandGlassExist &&
                                 ldHandDataExist &&
                                 gtSendCondition &&
                                 ldGlassExist &&
                                 //ldRecvCondition &&
                                 //ldHandRibUp &&
                                 ((curLdUpPosition == m_LdHandSend2Position) ||
                                 (curLdUpPosition == m_LdHandHomePosition)) &&
                                 (m_Control.IsPositionConfirmed(m_LdHandHomePosition) || m_Control.IsPositionConfirmed(m_LdHandSend2Position)))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "case 635 Move");
                            seqNo = 635;
                        }
                        else if (ldHandGlassExist && ldHandDataExist && ldHandRibUp &&
                                (m_Control.IsPositionConfirmed(m_LdHandHomePosition) || m_Control.IsPositionConfirmed(m_LdHandSend2Position)))
                        {
                            AlarmId = m_LdHandUnit.ALM_FishDataAbnormal.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Data Alarm(Home Position)");
                            ReturnSeqNo = 0;
                            seqNo = 1000;
                        }
                        #endregion

                    }
                    break;
                case 50:
                    {
                        m_LdHandUnit.SetHandDown();
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Down Start");
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 60;
                    }
                    break;
                case 60: // 11.02.25 minhan
                    {
                        if (GetElapsedTicks() > 4000) // 12.12.20 wamg
                        {
                            if (m_LdHandUnit.IsHandDown() && !m_LdHandUnit.IsHandUp())
                            {
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Down Confirm");
                                seqNo = 0;
                            }
                            else
                            {
                                AlarmId = m_LdHandUnit.ALM_FishLiftDown.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Down Alarm");
                                ReturnSeqNo = seqNo;
                                seqNo = 1500;
                            }
                        }
                    }
                    break;
                case 100:
                    {
                        //if (m_Simul.Motion) XFunc.GetTickCount(); // 11.02.25 minhan
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position Move Start");

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        seqNo = 110;
                    }
                    break;
                case 110: // 11.02.25 minhan 여기는 답없네..앞 조건을 만족한 상태로 와야지.
                    if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandRecv1Position))))
                    {
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position Move : OK");
                       
                        if (m_Simul.Motion)
                        {
                             m_Control.SetPositionConfirmed(m_LdHandRecv1Position);
                        }

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 120;
                    }
                    else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                    {
                        AlarmId = m_LdHandUnit.ALM_Recv1PosMove.Id;
                        m_EqpManager.SetAlarm(AlarmId);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position Move : Error");

                        string sRv;
                        sRv = string.Format("Rv = {0}", Rv);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 120:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandRecv1Position))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position Sensor : OK");
                            if ((m_GantryUnit.Servo.GetCurPointId() != m_GantrySendPosition) && !m_TrControl.IsPositionConfirmed(m_GantrySendPosition))
                            { // 11.02.25 minhan send 위치가 아니라면.
                                m_LdHandUnit.SetHandUp();//2010.06.28 KIMGUN 여기서 HAND RIB는 움직일 수 없다.. 간섭이 생긴다.
                                StartTicks = XFunc.GetTickCount();
                            }
                            seqNo = 130;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_Recv1PosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 130:
                    {
                        if (GetElapsedTicks() > 4000) // 11.02.25 minhan // 12.12.20 wang
                        {
                            if (m_LdHandUnit.IsHandUp() && !m_LdHandUnit.IsHandDown())
                            {
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv Ready Confirm");
                                m_LdHandUnit.IfFlag.InReady = true;
                                seqNo = 140;
                            }
                            else
                            {
                                AlarmId = m_LdHandUnit.ALM_FishLiftUp.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Up Alarm");
                                ReturnSeqNo = 50;
                                seqNo = 1000;
                            }
                        }
                    }
                    break;
                case 140: // 11.02.25 minhan
                    {
                        chkLdCondition = true;

                        chkLdCondition &= m_Control.IsPositionConfirmed(m_LdHandRecv1Position);
                        chkLdCondition &= m_LdHandUnit.IsHandUp();
                        chkLdCondition &= ((short)m_LdHandUnit.Servo.GetCurPointId() == m_LdHandRecv1Position) ? true : false;

                        if (!chkLdCondition) // 11.02.25 minhan
                        {
                            if (!m_LdHandUnit.IsHandUp())
                            {
                                AlarmId = m_LdHandUnit.ALM_FishLiftUp.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Up Alarm");
                                ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                            else
                            {
                                AlarmId = m_LdHandUnit.ALM_Recv1PosSensor.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv1 Position : Error");
                                ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                        }
                        else if (m_GantryUnit.IfFlag.OutReady &&
                                ((short)m_GantryUnit.Servo.GetCurPointId() == m_GantrySendPosition) &&
                                m_TrControl.IsPositionConfirmed(m_GantrySendPosition)) // 11.02.25 minhan
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Out Ready Confirm");

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            seqNo = 150;
                        }
                    }
                    break;
                case 150:
                    {
                        bool chkTrPosition = true;
                             chkTrPosition &= ((short)m_GantryUnit.Servo.GetCurPointId() == m_GantrySendPosition) ? true : false;
                             chkTrPosition &= m_TrControl.IsPositionConfirmed(m_GantrySendPosition);

                             if (!chkTrPosition)
                             {
                                 m_LdHandUnit.Servo.RbtEStop();
                                 m_GantryUnit.Servo.RbtEStop();
                                 AlarmId = m_LdHandUnit.ALM_Recv2PosMove.Id;
                                 m_EqpManager.SetAlarm(AlarmId);
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                                 StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                                 seqNo = 2000;
                             }
                             else if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandRecv2Position))))
                             {
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv2 Position Move : OK");

                                 if (m_Simul.Motion)
                                 {
                                     m_Control.SetPositionConfirmed(m_LdHandRecv2Position);
                                 }

                                 //***************** Position Log **************************
                                 RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                                 m_LdServoUnit.GetCurPosition(ref curPos);
                                 servoCurPos = curPos.Pos[0];

                                 string sCurPos;
                                 sCurPos = string.Format("End Position = {0}", servoCurPos);
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                                 //**********************************************************

                                 StartTicks = XFunc.GetTickCount();
                                 seqNo = 160;
                             }
                             else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                             {
                                 AlarmId = m_LdHandUnit.ALM_Recv2PosMove.Id;
                                 m_EqpManager.SetAlarm(AlarmId);
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv2 Position Move : Error");

                                 string sRv;
                                 sRv = string.Format("Rv = {0}", Rv);
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                                 //***************** Position Log **************************
                                 RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                                 m_LdServoUnit.GetCurPosition(ref curPos);
                                 servoCurPos = curPos.Pos[0];

                                 string sCurPos;
                                 sCurPos = string.Format("End Position = {0}", servoCurPos);
                                 m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                                 //**********************************************************

                                 ReturnSeqNo = seqNo;
                                 seqNo = 1000;
                             }
                    }
                    break;
                case 160:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandRecv2Position))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv2 Position Sensor : OK");

                            // Velocity Change
                            OriginalVel = m_LdHandUnit.Servo.GetVel(0);
                            m_LdHandUnit.Servo.SetVel(0, CHANGE_VEL);
                            //if (m_Simul.Motion) XFunc.GetTickCount(); // 09.12.08 minhan

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            //seqNo = 170;
                            seqNo = 165; // 09.12.08 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_Recv2PosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv2 Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 165: // 09.12.08 minhan
                    {
                        double CheckVel = m_LdHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == CHANGE_VEL)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Velocity Change OK");
                            seqNo = 170;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            AlarmId = m_AlarmVelocityChange.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv2 Velocity Change : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_LdHandUnit.Servo.SetVel(0, CHANGE_VEL);
                        }
                    }
                    break;
                case 170: // 11.02.25 minhan 
                    {
                        bool chkTrPosition = true;
                        chkTrPosition &= ((short)m_GantryUnit.Servo.GetCurPointId() == m_GantrySendPosition) ? true : false;
                        chkTrPosition &= m_TrControl.IsPositionConfirmed(m_GantrySendPosition);

                        if (!chkTrPosition)
                        {
                            m_LdHandUnit.Servo.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            AlarmId = m_LdHandUnit.ALM_Recv3PosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 2000;
                        }                       
                        else if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandRecv3Position))))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv3 Position Move : OK");
                            m_LdHandUnit.Servo.SetVel(0, OriginalVel);
                            
                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_LdHandRecv3Position);
                            }

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 180;
                        }
                        else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            AlarmId = m_LdHandUnit.ALM_Recv3PosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv3 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 180:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandRecv3Position))
                        {
                            if (m_Simul.Motion)
                            {
                                m_LdHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                                m_GantryUnit.GlassExistSensor.SetState(false, Logic.AND);
                            }
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv3 Position Sensor : OK");
                            //seqNo = 190; // 09.12.08 minhan
                            seqNo = 185; // 09.12.08 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_Recv3PosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv3 Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 185: // 09.12.08 minhan
                    {
                        double CheckVel = m_LdHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == OriginalVel)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Velocity Change OK");
                            StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 190;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            AlarmId = m_AlarmVelocityChange.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Recv3 Velocity Change : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_LdHandUnit.Servo.SetVel(0, OriginalVel);
                        }
                    }
                    break;
                case 190: // 11.02.25 minhan
                    {
                        if (m_LdHandUnit.IsGlassExist(Logic.AND) && m_LdHandUnit.IsHandUp()) // 11.02.25 minhan
                        {
                            m_Server.GlassData.Move(m_GantryUnit.DataMatchingKey(0), m_LdHandUnit.DataMatchingKey(0));
                            m_LdHandUnit.IfFlag.InComp = true;
                            m_LdHandUnit.IfFlag.InReady = false;

                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Glass Detected Confirm");

                            seqNo = 200;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            if (m_LdHandUnit.IsHandUp())
                            {
                                AlarmId = m_LdHandUnit.ALM_FishGlsNotSensing.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Glass Exist Sensor Error");
                                ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                            else
                            {
                                AlarmId = m_LdHandUnit.ALM_FishLiftUp.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Up Alarm");
                                ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                        }
                    }
                    break;
                case 200:
                    if (m_GantryUnit.IfFlag.OutComp)
                    {
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr Unit Glass Out Complete");
                        m_LdHandUnit.IfFlag.InComp = false;
                        m_GantryUnit.IfFlag.OutComp = false;//2009.09.02 kimgun
                        //2010.08.30 kimgun
                        TagGlassData RecvGlassData = new TagGlassData();
                        m_Server.GlassData.GetData(m_LdHandUnit.DataMatchingKey(0), ref RecvGlassData);
                        m_PortNo = (int)RecvGlassData.Item.GlassNumberCode.LotNo;
                        m_SlotNo = (int)RecvGlassData.Item.GlassNumberCode.SlotNo;
                        //if (!GlobalVar.AllGlsPosDataReq) // 11.02.25 minhan
                        //{
                        //    GlobalVar.AllGlsPosDataReq = true;
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Position Data Report");
                        //}
                        seqNo = 0;
                    }
                    break;
                case 400: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;      
                             chkTrpos &= (m_GantryUnit.Servo.GetCurPointId() == m_GantrySendPosition) ? false : true;
                             chkTrpos &= !m_TrControl.IsPositionConfirmed(m_GantrySendPosition);

                        if (!chkTrpos)
                        {
                            m_LdHandUnit.Servo.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            AlarmId = m_LdHandUnit.ALM_WatiPosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            StartTicks = XFunc.GetTickCount();
                            seqNo = 2000;
                        }
                        else if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandWaitPosition))))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Wait Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_LdHandWaitPosition);
                            }

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 410;
                        }
                        else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            AlarmId = m_LdHandUnit.ALM_WatiPosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Wait Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 410:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandWaitPosition))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Wait Position Sensor : OK");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_WaitPosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Wait Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 600:
                    {
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Position Move Start");

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        //StartTicks = XFunc.GetTickCount();//2010.07.07 kimgun 11.02.25 minhan
                        seqNo = 610;
                    }
                    break;
                case 610: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;
                        chkTrpos &= (m_GantryUnit.Servo.GetCurPointId() == m_GantrySendPosition) ? false : true;
                        chkTrpos &= !m_TrControl.IsPositionConfirmed(m_GantrySendPosition);

                        if (!chkTrpos)
                        {
                            m_LdHandUnit.Servo.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            AlarmId = m_LdHandUnit.ALM_Send1PosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            StartTicks = XFunc.GetTickCount();
                            seqNo = 2000;
                        }                 
                        else if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandSend1Position))))
                        {
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Position Move : OK");
                            //GetLoadRatio = 0; // 11.02.25 minhan

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_LdHandSend1Position);
                            }

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 620;
                        }
                        else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            AlarmId = m_LdHandUnit.ALM_Send1PosMove.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                            m_LdServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 620:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandSend1Position))
                        {
                            if (m_Simul.Motion)
                            {
                                //GlobalVar.Recv1Complete = true; // 11.02.25 minhan
                                m_LdCvUnit.GlsInSensor.SetState(true, Logic.AND);
                                m_LdHandUnit.GlassExistSensor.SetState(false, Logic.AND);
                            }
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Position Sensor : OK");
                            seqNo = 630;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_Send1PosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 630:
                    if (!m_Server.GlassData.IsExist(m_LdCvUnit.DataMatchingKey(0)) &&
                        !m_Server.GlassData.IsExist(m_LdCvUnit.DataMatchingKey(1)) &&
                        //!m_LdCvUnit.GlsInSensor.IsDetected() &&
                        !m_LdCvUnit.GlsOutSensor.IsDetected() &&
                        (CvMotorAct.Stop == m_LdCvUnit.AutoAct) &&
                        m_Aligner.IsNegative()) // 11.02.25 minhan
                    {
                        //if (m_Simul.Motion) XFunc.GetTickCount(); // 11.02.25 minhan
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Unit Recv Condition");
                        OriginalVel = m_LdHandUnit.Servo.GetVel(0);
                        m_LdHandUnit.Servo.SetVel(0, CHANGE_VEL);

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        //seqNo = 640; // 09.12.08 minhan
                        seqNo = 635; // 09.12.08 minhan
                    }
                    break;
                case 635: // 09.12.08 minhan
                    {
                        double CheckVel = m_LdHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == CHANGE_VEL)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Velocity Change OK");
                            seqNo = 640;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            AlarmId = m_AlarmVelocityChange.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send1 Velocity Change : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_LdHandUnit.Servo.SetVel(0, CHANGE_VEL);
                        }
                    }
                    break;
                case 640:
                    if ((0 == (Rv = m_LdHandUnit.Servo.RbtMovePos(m_LdHandSend2Position))))
                    {
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send2 Position Move : OK");
                        m_LdHandUnit.Servo.SetVel(0, OriginalVel);
                        //m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        if (m_Simul.Motion)
                        {
                           m_Control.SetPositionConfirmed(m_LdHandSend2Position);
                        }

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 650;
                    }
                    else if (Rv > 0 || !m_LdHandUnit.Servo.Ready || !m_LdHandUnit.Servo.HomeComp) // 11.03.25 minhan
                    {
                        AlarmId = m_LdHandUnit.ALM_Send2PosMove.Id;
                        m_EqpManager.SetAlarm(AlarmId);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send2 Position Move : Error");

                        string sRv;
                        sRv = string.Format("Rv = {0}", Rv);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                        m_LdServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 650:
                    {
                        if (m_Control.IsPositionConfirmed(m_LdHandSend2Position))
                        {
                            m_LdHandUnit.IfFlag.OutComp = true;
                            m_LdHandUnit.IfFlag.InComp = false;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send2 Position Sensor : OK");
                            //seqNo = 660; // 09.12.08  minhan
                            seqNo = 655; // 09.12.08 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            AlarmId = m_LdHandUnit.ALM_Send2PosSensor.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send12 Position Sensor : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 655: // 09.12.08 minhan
                    {
                        double CheckVel = m_LdHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == OriginalVel)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Velocity Change OK");
                            seqNo = 660;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            AlarmId = m_AlarmVelocityChange.Id;
                            m_EqpManager.SetAlarm(AlarmId);
                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit Send2 Velocity Change : Error");
                            ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_LdHandUnit.Servo.SetVel(0, OriginalVel);
                        }
                    }
                    break;
                case 660:
                    if (m_LdCvUnit.IfFlag.InComp)
                    {
                        m_LdCvUnit.IfFlag.InComp = false;
                        m_LdHandUnit.SetHandDown();
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Unit Glass In Complete");
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 670;
                    }
                    break;
                case 670:
                    {
                        if (GetElapsedTicks() > 4000) // 11.02.25 minhan // 12.12.20 wang
                        {
                            if (m_LdHandUnit.IsHandDown())
                            {
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Down Confirm");
                                seqNo = 0;
                            }
                            else
                            {
                                AlarmId = m_LdHandUnit.ALM_FishLiftDown.Id;
                                m_EqpManager.SetAlarm(AlarmId);
                                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Rib Down Alarm");
                                ReturnSeqNo = 50;
                                seqNo = 1500;
                            }
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;

                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1000)");

                        seqNo = ReturnSeqNo;
                    }
                    break;
                case 1500:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0;
                        StartTicks = XFunc.GetTickCount();
                        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1500)");

                        seqNo = ReturnSeqNo;
                    }
                    break;
                case 2000: // 11.02.25 minhan Tr,LD hand Estop
                    {
                        bool EstopCheckTR = true;
                        bool EstopCheckLD = true;

                        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();
                        EstopCheckLD = m_LdHandUnit.Servo.RbtEStop();

                        if (EstopCheckTR && EstopCheckLD)
                        {

                            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr and LD hand Estop OK"); 
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmIdServo = m_ManualChangeErrLD.Id;
                            m_EqpManager.SetAlarm(m_AlarmIdServo);
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;

                        }
                    }
                    break;
            }
            this.SeqNo = seqNo;

            return -1;
        }
        #endregion
    }

    public class SeqUnitLdFishAirInterlock : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        private Simul m_Simul;
        private FishHand m_LdHandUnit;
        #endregion

        #region Unit
        private TagGlassData m_RecvGlassData = new TagGlassData();
        private ThreadLd_Hand_BOE_G8_DHDC m_Control;
        #endregion

        #region Constructor
        public SeqUnitLdFishAirInterlock(ThreadLd_Hand_BOE_G8_DHDC control, FishHand LdHand)
        {
            m_LdHandUnit = LdHand;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_Control = control;
            SeqFunName = m_LdHandUnit.Name;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.SeqNo;
         
            switch (seqNo)
            {
                case 0:
                    if ((m_LdHandUnit.FrontRib.DoFwSolenoid.GetState() || m_LdHandUnit.FrontRib.DiFwSensor.GetState()) &&
                        !m_LdHandUnit.FrontRib.DoBwSolenoid.GetState() && !m_Simul.Motion) // 10.12.21 minhan나중에 이거 고쳐야 한다.
                    {
                        StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 5000) // 12.12.20 wamg
                    {
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (((m_LdHandUnit.FrontRib.DoFwSolenoid.GetState() &&
                        !m_LdHandUnit.FrontRib.DoBwSolenoid.GetState())&&
                        (m_LdHandUnit.FrontRib.DiFwSensor.GetState() &&
                        !m_LdHandUnit.FrontRib.DiBwSensor.GetState()))&&
                        !m_LdHandUnit.IsHandPressureOk())
                    {
                        m_Control.SetLog(SeqFunName, seqNo, 0, 0, "LD Fish Hand AirInterlock Detected!");
                        GlobalVar.LdHandAirInterlock = true;

                        AlarmId = m_LdHandUnit.ALM_FishHandCdaLow.Id;
                        m_EqpManager.SetAlarm(AlarmId);
                        seqNo = 1000;
                    }
                    else
                    {
                        seqNo = 0;
                    }
                    break;
                case 1000:
                    if ((m_EqpManager.AlarmResetSwitchPushed &&
                        m_LdHandUnit.FrontRib.DoFwSolenoid.GetState() &&
                        !m_LdHandUnit.FrontRib.DoBwSolenoid.GetState() &&
                        m_LdHandUnit.IsHandPressureOk()) ||
                        (m_EqpManager.AlarmResetSwitchPushed &&
                        !m_LdHandUnit.FrontRib.DoFwSolenoid.GetState() &&
                        m_LdHandUnit.FrontRib.DoBwSolenoid.GetState() &&
                        !m_LdHandUnit.IsHandPressureOk())) // 09.09.28 minhan
                    {

                        GlobalVar.LdHandAirInterlock = false;
                        
                        m_EqpManager.ResetAlarm(AlarmId);
                        AlarmId = 0; 
                        
                        m_Control.SetLog(SeqFunName, seqNo, 0, 0, "LD Fish Hand AirInterlock Recovery!");
                        seqNo = 0;
                    }
                    break;
            }
            this.SeqNo = seqNo;

            return -1;
        }
        #endregion
    }
}
 
  
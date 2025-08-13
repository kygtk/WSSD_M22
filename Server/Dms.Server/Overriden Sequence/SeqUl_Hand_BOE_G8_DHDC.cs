using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using Dms.Ctl;
using System.Windows.Forms;
using System.Threading;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadUl_Hand_BOE_G8_DHDC : ThreadHandControl
    {
        private static FishHand m_FishHand;
        private static GenInfoHandler m_GenInfo;
        private XLog UlHandLog; // 11.02.25 minhan

        public ThreadUl_Hand_BOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {
            m_FishHand = eqpTransferUnits._UL_Fish_Hand;
            m_GenInfo = GenInfoHandler.Instance;
            UlHandLog = new XLog("UL Hand Log", XLog.LogStampType.UseStamp); // 11.02.25 minhan
        }

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (FishHand device in m_FishHands)
            {
                device.IfFlag.Reset();

                if (device.Name == eqpTransferUnits._UL_Fish_Hand_Name)
                {
                    RegisterSequence(new SeqUnitUlFish(this, device));
                    RegisterSequence(new SeqUnitUlFishAirInterlock(this, device));
                }
            }
        }
        #endregion

        #region Override Methods
        public override void InitParameter() // 09.05.30 minhan override 해서 사용
        {
            foreach (FishHand unit in m_FishHands)
            {
                if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
                unit.IfFlag.Reset();
                unit.IfFlag.InComp = false;
                unit.IfFlag.OutComp = false;
            }
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

            log = string.Format("UlHand  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            UlHandLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }

        public bool IsSeqRunCondition(FishHand fs)
        {
            bool run = true;
            run &= !IsInterlock();
            // run &= !CvStopCondition(cv.NextCv);
            run &= !m_GenInfo.Pause;
            run &= m_GenInfo.AutoMode;
            run &= !GlobalVar.UlHandAirInterlock;
            return run;
        }

        public bool IsInterlock()
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool Interlock = false;
            //Interlock |= ((heavy & ~HeavyInterlock.Emo) > 0 );
            //Interlock |= ((heavy & ~HeavyInterlock.Leak) > 0 );//2010.06.29 kimgun
            Interlock |= ((heavy & HeavyInterlock.Door) > 0);//2010.06.29 kimgun
            Interlock |= ((heavy & HeavyInterlock.Area) > 0);//2010.06.29 kimgun
            return Interlock;
        }

        // UL Hand Position Sensors
        // Before Send(SEND3) + MT
        // After Send(SEND2)
        // TR Access(SEND1) + MT
        // Wait(WAIT)
        // After Recv (RECV2)
        // Before Recv(RECV1) ==  Home
        public bool IsPositionConfirmed(short posId) // 11.01.27 minhan
        {
            if (posId == eqpPos_UL_Hand_Servo_Unit.Home)
            {
                if (!AppConfig.Instance.Simul.Device)
                {
                    return m_FishHand.Servo.GetServoMotor(0).GetHomeSwitch();
                }
                else
                {
                    return m_FishHand.diRecv1_Pos_Sensor.IsDetected();
                }
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Recv1)
            {
                if (!AppConfig.Instance.Simul.Device)
                {
                    return m_FishHand.Servo.GetServoMotor(0).GetHomeSwitch();
                }
                else
                {
                    return m_FishHand.diRecv1_Pos_Sensor.IsDetected();
                }
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Recv2)
            {
                return m_FishHand.diRecv2_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Wait)
            {
                return m_FishHand.diWait_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send1)
            {
                return m_FishHand.diSend1_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send2)
            {
                return m_FishHand.diSend2_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send3)
            {
                return m_FishHand.diSend3_Pos_Sensor.IsDetected();
            }
            return false;
        }

        /// <summary>
        /// only for simulation
        /// </summary>
        /// <param name="posId"></param>
        public void SetPositionConfirmed(short posId)
        {
            if (posId == eqpPos_UL_Hand_Servo_Unit.Home)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(true);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Recv1)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(true);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Recv2)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(false);
                m_FishHand.diRecv2_Pos_Sensor.SetState(true);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Wait)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(false);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(true);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send1)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(false);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(true);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send2)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(false);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(true);
                m_FishHand.diSend3_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_UL_Hand_Servo_Unit.Send3)
            {
                m_FishHand.diRecv1_Pos_Sensor.SetState(false);
                m_FishHand.diRecv2_Pos_Sensor.SetState(false);
                m_FishHand.diWait_Pos_Sensor.SetState(false);
                m_FishHand.diSend1_Pos_Sensor.SetState(false);
                m_FishHand.diSend2_Pos_Sensor.SetState(false);
                m_FishHand.diSend3_Pos_Sensor.SetState(true);
            }
        }
        #endregion 
    }

    public class SeqUnitUlFish : XSeqFunction
    {
        #region Fields
        private GantryUnit m_GantryUnit = null;
        private FishHand m_UlHandUnit;

        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        private GenInfoHandler m_GenInfo;

        private ThreadUl_Hand_BOE_G8_DHDC m_Control;

        private Simul m_Simul;

        private TagGlassData m_RecvGlassData = new TagGlassData();
        private CvUnit m_UlCvUnit;
        private _ServoUnit m_UlServoUnit;
        private double servoCurPos = 0.0;

        private short m_GantryHomePosition;
        private short m_GantrySendPosition;
        private short m_GantryWaitPosition;
        private short m_GantryRecvPosition;

        private short m_UlHandHomePosition;
        private short m_UlHandRecv1Position;
        private short m_UlHandRecv2Position;
        private short m_UlHandWaitPosition;
        private short m_UlHandSend1Position;
        private short m_UlHandSend2Position;
        private short m_UlHandSend3Position;

        private double CHANGE_VEL = 85.0; // 11.04.22 minhan
        private double OriginalVel = 0;
        private Alarm m_AlarmFishMovingInterlock;
        private Alarm m_ManualChangeErrUL; // 11.02.25 minhan
        private int VelocityChangeCount = 0; // 09.12.08 minhan
        private Alarm m_AlarmVelocityChange; // 09.12.08 minhan
        private Alarm m_ServoNotReady; // 11.02.25 minhan

        private ThreadTr_BOE_G8_DHDC m_TrControl;
        //private ThreadLd_Hand_BOE_G8_DHDC m_LdHandControl;
        //private float GetLoadRatio; // 11.02.25 minhan
        private int m_PortNo = 0;//2010.08.30 kimgun
        private int m_SlotNo = 0;//2010.08.30 kimgun
        private bool chkhand; // 11.02.25 minhan
        private int m_AlarmIdServo; // 11.02.25 minhan
        #endregion

        #region Constructor
        public SeqUnitUlFish(ThreadUl_Hand_BOE_G8_DHDC control, FishHand UlHand)
        {
            m_UlHandUnit = UlHand;
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_UlServoUnit = m_UlHandUnit.Servo;
            m_SeqFunName = m_UlHandUnit.Name;
            m_GenInfo = GenInfoHandler.Instance;

            m_UlCvUnit = eqpTransferUnits._UL_CvUnit;
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;

            m_AlarmFishMovingInterlock = new Alarm(m_UlHandUnit.Name + " : Moving Interfere", AlarmLevel.S, AlarmCode.EquipmentSafety);//2009.08.25 kimgun
            m_AlarmVelocityChange = new Alarm(m_UlHandUnit.Name + " : Velocity Change Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 09.12.08 minhan
            m_ManualChangeErrUL = new Alarm(m_UlHandUnit.Name + " : Manual Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_ServoNotReady = new Alarm(m_UlHandUnit.Name + " : Servo Not Ready", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan

            m_GantryHomePosition = eqpPos_TR_Servo_Unit.Home;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send;
            m_GantryWaitPosition = eqpPos_TR_Servo_Unit.Wait;
            m_GantryRecvPosition = eqpPos_TR_Servo_Unit.Recv;

            m_UlHandHomePosition = eqpPos_UL_Hand_Servo_Unit.Home;
            m_UlHandRecv1Position = eqpPos_UL_Hand_Servo_Unit.Recv1;
            m_UlHandRecv2Position = eqpPos_UL_Hand_Servo_Unit.Recv2;
            m_UlHandWaitPosition = eqpPos_UL_Hand_Servo_Unit.Wait;
            m_UlHandSend1Position = eqpPos_UL_Hand_Servo_Unit.Send1;
            m_UlHandSend2Position = eqpPos_UL_Hand_Servo_Unit.Send2;
            m_UlHandSend3Position = eqpPos_UL_Hand_Servo_Unit.Send3;

            //GetLoadRatio = 0;
            m_AlarmIdServo = 0;
            chkhand = false; // 11.02.25 minhan
            OriginalVel = m_UlServoUnit.GetVel(0);//2009.08.17 kimgun 09.12.08 minhan 오류수정
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;
            if (m_UlHandUnit.Sequence[0] == null) m_UlHandUnit.Sequence[0] = this; // 09.05.30 minhan
            if (!m_Control.IsSeqRunCondition(m_UlHandUnit)) return -1;

            if (m_TrControl == null) m_TrControl = m_Server.ThreadHandler.TrControl;
            //if (m_LdHandControl == null) m_LdHandControl = m_Server.ThreadHandler.LdHandControl;

            //2010.08.30 kimgun
            if (m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0)) &&
                (m_PortNo == 0) && (m_SlotNo == 0))
            {
                m_Server.GlassData.GetData(m_UlHandUnit.DataMatchingKey(0), ref m_RecvGlassData);
                m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            int seqNo = this.m_SeqNo;
            int Rv = -1;
            switch (seqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            //alarm flag reset
                            GlobalVar.UlHnadAlarm = false;//2009.08.25 kimgun
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        if (m_AlarmIdServo > 0) // 11.02.25 minhan
                        {
                            m_Eqp.ResetAlarm(m_AlarmIdServo);
                            m_AlarmIdServo = 0;
                        }

                        if (OriginalVel != 0)//2009.08.17 kimgun 일단 속도 원복하자.
                            m_UlHandUnit.Servo.SetVel(0, OriginalVel);

                        if (VelocityChangeCount != 0) VelocityChangeCount = 0; // 09.12.08 minhan

                        short curGtPosition = (short)m_GantryUnit.Servo.GetCurPointId();
                        short curUlPosition = (short)m_UlHandUnit.Servo.GetCurPointId();

                        bool ulHandRibUp = m_UlHandUnit.IsHandUp();
                        bool ulHandRibDn = m_UlHandUnit.IsHandDown();

                        bool gtWaitPosSensor = m_TrControl.IsPositionConfirmed(m_GantryWaitPosition);
                        bool gtSendPosSensor = m_TrControl.IsPositionConfirmed(m_GantrySendPosition);
                        bool gtRecvPosSensor = m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);

                        bool gtGlassExist = m_GantryUnit.IsGlassExist(Logic.OR) &
                                            gtWaitPosSensor;

                        bool gtGlassDataExist = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        // And you should check position of transfer unit whether it is at the wait position or not.

                        if (m_Simul.Device && m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0))) m_UlHandUnit.GlassExistSensor.SetState(true, Logic.AND);

                        bool ulHandGlassExist = m_UlHandUnit.IsGlassExist(Logic.OR);
                        bool ulHandDataExist = m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0));

                        // bool ulHandSendCondition = false;
                        bool ulUnitGlassExist = m_UlCvUnit.GlsOutSensor.IsDetected();
                        bool ulUnitInGlassExist = m_UlCvUnit.GlsInSensor.IsDetected();//2009.08.25 kimgun
                        bool ulUnitDataExist = m_Server.GlassData.IsExist(m_UlCvUnit.DataMatchingKey(1));

                        bool ulUnitSendCondition = ulUnitGlassExist && ulUnitDataExist && m_UlCvUnit.IfFlag.OutReady;


                        int glassCount = m_Server.GlassData.Count;
                        bool isRobotInterlock = m_GantryUnit.IsRobotInterlock();

                        bool gtUnitCondition = ((curGtPosition == m_GantrySendPosition) && m_TrControl.IsPositionConfirmed(m_GantrySendPosition)) ||
                                               ((curGtPosition == m_GantryWaitPosition) && m_TrControl.IsPositionConfirmed(m_GantryWaitPosition)) ||
                                               ((curGtPosition == m_GantryHomePosition) && m_TrControl.IsPositionConfirmed(m_GantryHomePosition)); // 11.02.25 minhan

                        bool gtUnitRecvCondition = !m_GantryUnit.IsGlassExist(Logic.OR) &&
                                                   !gtGlassDataExist &&
                                                   (curGtPosition == m_GantryRecvPosition) &&
                                                   m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);
                        // Error Case
                        if ((ulHandGlassExist || ulHandDataExist) && ulHandRibDn)
                        {
                            //alarm flag set
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftAbnormal.Id;

                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib abnormal Alarm");
                            m_ReturnSeqNo = 0;
                            seqNo = 1000;
                        }
                        else if (((ulHandRibDn && ulHandRibUp) ||
                                (!ulHandRibDn && !ulHandRibUp)))
                        {
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftAbnormal.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib abnormal Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if ((!m_Control.IsPositionConfirmed(m_UlHandSend1Position) &&
                                !m_Control.IsPositionConfirmed(m_UlHandSend2Position) &&
                                !m_Control.IsPositionConfirmed(m_UlHandSend3Position) &&
                                !m_Control.IsPositionConfirmed(m_UlHandWaitPosition) &&
                                !m_Control.IsPositionConfirmed(m_UlHandRecv2Position) &&
                                !m_Control.IsPositionConfirmed(m_UlHandRecv1Position) &&
                                !m_Control.IsPositionConfirmed(m_UlHandHomePosition)) ||
                                (curUlPosition == -1))
                        {//2009.09.14 kimgun case 0에서 position senosr가 하나도 감지 안 되면 NG
                            m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Position Not Detect Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if (!m_UlHandUnit.Servo.Ready || !m_UlHandUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_ServoNotReady.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Servo Ready Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        //kimx
                        //else if (((!ulHandGlassExist && ulHandDataExist) || (ulHandGlassExist && !ulHandDataExist)) &&
                        //    ((curUlPosition != m_UlHandHomePosition) &&
                        //     (curUlPosition != m_UlHandRecv1Position) &&
                        //     (curUlPosition != m_UlHandSend1Position)) && !m_Simul.Motion )
                        //{
                        //    //alarm flag set
                        //    GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                        //    AlarmId = m_UlHandUnit.ALM_FishDataAbnormal.Id;
                        //    m_Eqp.SetAlarm(AlarmId);
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Data Alarm");
                        //    ReturnSeqNo = 0;
                        //    seqNo = 1000;
                        //}
                        else if (((curUlPosition == m_UlHandHomePosition) && m_Control.IsPositionConfirmed(m_UlHandHomePosition)) ||
                                 ((curUlPosition == m_UlHandRecv1Position) && m_Control.IsPositionConfirmed(m_UlHandRecv1Position))) // 11.02.25 minhan
                        {
                            if ((!ulHandGlassExist && !ulHandDataExist) ||
                               (ulHandGlassExist && !ulHandDataExist && ulUnitSendCondition))//2009.08.25 kimgun
                            {
                                if (!ulUnitInGlassExist)
                                {
                                    m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Home or Recv1 Pos, No Glass, No Data");
                                    seqNo = 100;
                                }
                                else if (ulUnitInGlassExist && ulHandGlassExist) // 11.02.25 minhan !ulUnitInGlassExist이게 아니겠지...
                                {//2009.08.25 kimgun
                                    GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                                    m_AlarmId = m_AlarmFishMovingInterlock.Id;
                                    m_Eqp.SetAlarm(m_AlarmId);
                                    m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand moving interfere");
                                    m_ReturnSeqNo = 0;
                                    seqNo = 2000;
                                }
                            }
                            else if (ulHandGlassExist && ulHandDataExist)
                            {
                                GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                                m_AlarmId = m_UlHandUnit.ALM_FishDataAbnormal.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Data Alarm");
                                m_ReturnSeqNo = 0;
                                seqNo = 1000;
                            }
                        }
                        else if ((curUlPosition == m_UlHandRecv2Position) && m_Control.IsPositionConfirmed(m_UlHandRecv2Position)) // 11.02.25 minhan
                        {
                            if (ulHandGlassExist && ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Recv2 Pos, Case 400");
                                seqNo = 400;
                            }
                            else if (ulHandGlassExist && ulHandDataExist && !gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Recv2 Pos, Case 300");
                                seqNo = 300;
                            }
                            else if ((!ulHandGlassExist && !ulHandDataExist && (ulUnitGlassExist || ulUnitInGlassExist)) && gtUnitCondition) // 11.02.25 minhan
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Recv2 Pos, Case 350");
                                seqNo = 350;
                            }
                            else if (!ulHandGlassExist && !ulHandDataExist && !ulUnitGlassExist && !ulUnitInGlassExist)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Recv2 Pos, Case 100");
                                seqNo = 120;
                            }
                            else if (ulHandGlassExist && !ulHandDataExist && ulUnitDataExist && !ulUnitGlassExist)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Recv2 Pos, Case 150");
                                seqNo = 150;
                            }
                        }
                        else if ((curUlPosition == m_UlHandWaitPosition) && m_Control.IsPositionConfirmed(m_UlHandWaitPosition)) // 11.02.25 minhan
                        {
                            if (ulHandGlassExist && ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Wait Pos, Case 400");
                                seqNo = 400;
                            }
                            else if (!ulHandGlassExist && !ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Wait Pos, Case 350");
                                seqNo = 350;
                            }
                        }
                        else if ((curUlPosition == m_UlHandSend3Position) && m_Control.IsPositionConfirmed(m_UlHandSend3Position)) // 11.02.25 minhan
                        {
                            if (ulHandGlassExist && ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send3 Pos, Case 400");
                                m_UlHandUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                                seqNo = 400;
                            }
                            else if (ulHandGlassExist && ulHandDataExist &&
                                    (curGtPosition == m_GantryRecvPosition) &&
                                    m_TrControl.IsPositionConfirmed(m_GantryRecvPosition)) // 11.02.25 minhan
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send3 Pos, Case 300");
                                m_UlHandUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                                seqNo = 430;
                            }
                            else if (!ulHandGlassExist && !ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send3 Pos, Case 350");
                                seqNo = 350;
                            }
                        }
                        else if ((curUlPosition == m_UlHandSend2Position) && m_Control.IsPositionConfirmed(m_UlHandSend2Position)) // 11.02.25 minhan
                        {
                            if (ulHandGlassExist && ulHandDataExist && !m_GantryUnit.IsGlassExist(Logic.OR) &&
                               (curGtPosition == m_GantryRecvPosition) &&
                               !m_TrControl.IsPositionConfirmed(m_GantryRecvPosition)) // 11.02.25 minhan
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send2 Pos, Case 440");
                                seqNo = 440;
                            }
                            else if (!ulHandGlassExist && !ulUnitDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send2 Pos, Case 350");
                                seqNo = 350;
                            }
                        }
                        else if ((curUlPosition == m_UlHandSend1Position) && m_Control.IsPositionConfirmed(m_UlHandSend1Position)) // 11.02.25 minhan 
                        {
                            if (ulHandGlassExist && ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send1 Pos, Case 400");
                                seqNo = 400;
                            }
                            //else if (ulHandGlassExist && ulHandDataExist &&
                            //         (curGtPosition == m_GantryRecvPosition) &&
                            //         m_TrControl.IsPositionConfirmed(m_GantryRecvPosition)) // 11.02.25 minhan 이거는 있을 수가 없는 케이스이고 400으로 가도 안된다.
                            //{
                            //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send1 Pos, Case 400");
                            //    seqNo = 400;
                            //}
                            else if (!ulHandGlassExist && !ulHandDataExist && gtUnitCondition)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send1 Pos, Case 100");
                                seqNo = 100;
                            }
                        }
                    }
                    break;
                case 100:
                    {
                        m_UlHandUnit.SetHandDown();
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Down Start");
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 110;
                    }
                    break;
                case 110:
                    {
                        if (GetElapsedTicks() > 4000) // 11.02.25 minhan // 13.6.6 wzy
                        {
                            if (!m_UlHandUnit.IsHandUp() && m_UlHandUnit.IsHandDown()
                                /*&& eqpSensors._UL_Fish_Unit_Hand_ROT_Down_Sensor.IsDetected() 
                                && eqpSensors._UL_Fish_Unit_Hand_ROT_Down_Sensor_MT.IsDetected()*/)//LKL 151110
                            {
                                //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Down Confirm");
                                //***************** Position Log **************************
                                RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                                m_UlServoUnit.GetCurPosition(ref curPos);
                                servoCurPos = curPos.Pos[0];

                                string sCurPos;
                                sCurPos = string.Format("Start Position = {0}", servoCurPos);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                                //**********************************************************
                                seqNo = 120;
                            }
                            else
                            {
                                GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                                m_AlarmId = m_UlHandUnit.ALM_FishLiftDown.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Down Alarm");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1500;
                            }
                        }
                    }
                    break;
                case 120:
                    if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandRecv1Position))))
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv1 Position Move : OK");

                        if (m_Simul.Motion)
                        {
                            m_Control.SetPositionConfirmed(m_UlHandRecv1Position);
                        }
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 130;
                    }
                    else if (Rv > 0 || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                    {
                        GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                        m_AlarmId = m_UlHandUnit.ALM_Recv1PosMove.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv1 Position Move : Error");

                        string sRv;
                        sRv = string.Format("Rv = {0}", Rv);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 130:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandRecv1Position))
                        {
                            m_UlHandUnit.SetHandUp();
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv1 Position Sensor : OK");
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Start");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 140;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Recv1PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv1 Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 140:
                    if (GetElapsedTicks() > 5000) // //zhangliang 13.06.08
                    {
                        if (m_UlHandUnit.IsHandUp() && !m_UlHandUnit.IsHandDown()
                            /*&&eqpSensors._UL_Fish_Unit_Hand_ROT_Up_Sensor.IsDetected()
                            &&eqpSensors._UL_Fish_Unit_Hand_ROT_Up_Sensor_MT.IsDetected()*/)//LKL 151110
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Confirm");

                            seqNo = 150;
                        }
                        else
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 150: // 11.02.25 minhan
                    {
                        chkhand = m_UlHandUnit.IsHandUp();
                        bool ulUnitGlassExist = m_UlCvUnit.GlsOutSensor.IsDetected();
                        bool ulUnitInGlassExist = m_UlCvUnit.GlsInSensor.IsDetected();//2009.08.25 kimgun
                        bool ulUnitDataExist = m_Server.GlassData.IsExist(m_UlCvUnit.DataMatchingKey(1));

                        bool ulUnitSendCondition = ulUnitGlassExist && ulUnitDataExist && m_UlCvUnit.IfFlag.OutReady;
                        if (!chkhand)
                        {
                            GlobalVar.UlHnadAlarm = true;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if (ulUnitSendCondition)
                        {
                            //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan

                            OriginalVel = m_UlServoUnit.GetVel(0);
                            m_UlServoUnit.SetVel(0, CHANGE_VEL);

                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Unit Out Ready Confirm");
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            //seqNo = 160; // 09.12.08 minhan
                            seqNo = 155; // 09.12.08 minhan
                        }
                    }
                    break;
                case 155: // 09.12.08 minhan
                    {
                        double CheckVel = m_UlHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == CHANGE_VEL)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Velocity Change OK");
                            seqNo = 160;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            m_AlarmId = m_AlarmVelocityChange.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv1 Velocity Change : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_UlHandUnit.Servo.SetVel(0, CHANGE_VEL);
                        }
                    }
                    break;
                case 160:
                    if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandRecv2Position))))
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv2 Position Move : OK");
                        m_UlServoUnit.SetVel(0, OriginalVel);


                        if (m_Simul.Motion)
                        {
                            m_Control.SetPositionConfirmed(m_UlHandRecv2Position);
                        }
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************

                        m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 170;
                    }
                    else if (Rv > 0 || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                    {
                        GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                        m_AlarmId = m_UlHandUnit.ALM_Recv2PosMove.Id;
                        m_Eqp.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv2 Position Move : Error");

                        string sRv;
                        sRv = string.Format("Rv = {0}", Rv);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("End Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 170:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandRecv2Position))
                        {
                            if (m_Simul.Motion)
                            {
                                m_UlCvUnit.GlsOutSensor.SetState(false, Logic.AND);
                                m_UlHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                            }
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv2 Position Sensor : OK");
                            //m_StartTicks = XFunc.GetTickCount(); // 09.12.08 minhan
                            //seqNo = 175; // 09.12.08 minhan
                            seqNo = 172; // 09.12.08 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Recv2PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv2 Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 172: // 09.12.08 minhan
                    {
                        double CheckVel = m_UlHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == OriginalVel)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Velocity Change OK");
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 175;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            m_AlarmId = m_AlarmVelocityChange.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Recv2 Velocity Change : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_UlHandUnit.Servo.SetVel(0, OriginalVel);
                        }
                    }
                    break;
                case 175: // 11.02.25 minhan
                    {
                        chkhand = m_UlHandUnit.IsHandUp();

                        if (m_UlHandUnit.IsGlassExist(Logic.AND) && chkhand) // 11.02.25 minhan
                        {
                            //m_Server.GlassData.Move(m_UlCvUnit.DataMatchingKey(1), m_UlHandUnit.DataMatchingKey(0));
                            m_Server.GlassData.Move(m_UlHandUnit.PrevUnit.DataMatchingKey(1), m_UlHandUnit.DataMatchingKey(0));
                            //2010.08.30 kimgun
                            TagGlassData RecvGlassData = new TagGlassData();
                            m_Server.GlassData.GetData(m_UlHandUnit.DataMatchingKey(0), ref RecvGlassData);
                            m_PortNo = (int)RecvGlassData.Item.GlassNumberCode.LotNo;
                            m_SlotNo = (int)RecvGlassData.Item.GlassNumberCode.SlotNo;
                            m_UlHandUnit.IfFlag.InComp = true;
                            m_UlHandUnit.IfFlag.InReady = false;

                            //if (!GlobalVar.AllGlsPosDataIng) // 11.02.25 minhan
                            //{
                            //    GlobalVar.AllGlsPosDataReq = true;
                            //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Position Data Report");
                            //}
                            seqNo = 180;
                        }
                        else if (GetElapsedTicks() > 5000)//zhangliang 13.06.08
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun

                            if (chkhand)
                            {
                                m_AlarmId = m_UlHandUnit.ALM_FishGlsNotSensing.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Glass Exist Sensor Error");
                            }
                            else
                            {
                                m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                                m_Eqp.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            }
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 180:
                    if (!m_UlCvUnit.IfFlag.OutReady)
                    {
                        m_UlHandUnit.IfFlag.InComp = false;
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand InComp False"); // 11.02.25 minhan
                        seqNo = 0;
                    }
                    break;
                case 300: // 11.02.25 minhan
                    {
                        chkhand = m_UlHandUnit.IsHandUp();

                        if (!chkhand)
                        {
                            GlobalVar.UlHnadAlarm = true;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            seqNo = 310;
                        }
                    }
                    break;
                case 310:
                    {
                        if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandWaitPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Wait Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_UlHandWaitPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 320;
                        }
                        else if (Rv > 0 || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_WatiPosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Wait Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 320:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandWaitPosition))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Wait Position Sensor : OK");
                            seqNo = 330; // 11.02.25 minhan
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_WaitPosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Wait Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 330: // 11.02.25 minhan
                    {
                        if (m_UlHandUnit.IsHandUp() && !m_UlHandUnit.IsHandDown())
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Confirm");
                            seqNo = 0;
                        }
                        else
                        {
                            GlobalVar.UlHnadAlarm = true;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 350:
                    {
                        //m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        seqNo = 360;
                    }
                    break;
                case 360: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;
                        chkTrpos &= m_GantryUnit.Servo.GetCurPointId() != m_GantryRecvPosition;
                        chkTrpos &= !m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);

                        if (!chkTrpos)
                        {
                            m_UlServoUnit.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 3000;
                        }
                        else if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandSend1Position))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_UlHandSend1Position);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 370;
                        }
                        else if (Rv > 0 || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 370:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandSend1Position))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Sensor : OK");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 400: // 11.02.25 minhan
                    {
                        chkhand = m_UlHandUnit.IsHandUp();

                        if (!chkhand)
                        {
                            GlobalVar.UlHnadAlarm = true;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            seqNo = 410;
                        }
                    }
                    break;
                case 410: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;
                        chkTrpos &= m_GantryUnit.Servo.GetCurPointId() != m_GantryRecvPosition;
                        chkTrpos &= !m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);

                        if (!chkTrpos)
                        {
                            m_UlServoUnit.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_UlHandUnit.ALM_Send3PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 3000;
                        }
                        else if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandSend3Position))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send3 Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_UlHandSend3Position);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 420;
                        }
                        else if (Rv > 0 || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send3PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send3 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 420:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandSend3Position))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send3 Position Sensor : OK");
                            m_UlHandUnit.IfFlag.OutReady = true;
                            seqNo = 430;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send3PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send3 Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 430:
                    {
                        bool checkTrGlass = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)); // 11.02.07 minhan
                        checkTrGlass |= m_GantryUnit.GlassExistSensor.IsDetected();

                        chkhand = m_UlHandUnit.IsHandUp(); // 11.02.25 minhan

                        if (!chkhand)
                        {
                            GlobalVar.UlHnadAlarm = true;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Rib Up Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if (m_GantryUnit.IfFlag.InReady && !checkTrGlass &&
                                ((short)m_GantryUnit.Servo.GetCurPointId() == m_GantryRecvPosition) &&
                                m_TrControl.IsPositionConfirmed(m_GantryRecvPosition))
                        {
                            int posId = m_UlHandUnit.DataMatchingKey(0);
                            TagGlassData data = new TagGlassData();
                            m_Server.GlassData.GetData(posId, ref data);

                            data.Processed = true;

                            m_Server.GlassData.Update(posId, data);

                            //if (m_Server.GlassData.GetData(m_UlHandUnit.DataMatchingKey(0), ref data) == true) 10.12.21 minhan
                            //{
                            //    //m_EcsInfo.UnloadGlassInfo.Clone(data);//2009.09.18 kimgun
                            //}

                            //if (!m_Server.SetupSingleMode.GetValue<bool>()) // 11.02.25 minhan
                            //{
                            //    if (!GlobalVar.UnloadDataSet)
                            //    {
                            //        GlobalVar.UnloadDataSet = true;
                            //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Normal Unload Data Set : OK");
                            //    }
                            //}

                            OriginalVel = m_UlServoUnit.GetVel(0);
                            m_UlServoUnit.SetVel(0, CHANGE_VEL);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv Position : Confirm");
                            seqNo = 440;
                        }
                    }
                    break;
                case 440:
                    {
                        //if (m_Simul.Motion) XFunc.GetTickCount(); // 09.12.08 minhan
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        //seqNo = 445; // 09.12.08 minhan
                        seqNo = 442; // 09.12.08 minhan
                    }
                    break;
                case 442: // 09.12.08 minhan
                    {
                        double CheckVel = m_UlHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == CHANGE_VEL)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Velocity Change OK");
                            seqNo = 445;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            m_AlarmId = m_AlarmVelocityChange.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send3 Velocity Change : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_UlHandUnit.Servo.SetVel(0, CHANGE_VEL);
                        }
                    }
                    break;
                case 445: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;
                        chkTrpos &= (m_GantryUnit.Servo.GetCurPointId() == m_GantryRecvPosition);
                        chkTrpos &= m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);

                        if (!chkTrpos)
                        {
                            m_UlServoUnit.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_UlHandUnit.ALM_Send2PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 3000;
                        }
                        else if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandSend2Position))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send2 Position Move : OK");

                            m_UlServoUnit.SetVel(0, OriginalVel);
                            //GlobalVar.UwGlsReq = true; // 11.02.09 minhan

                            if (!m_Server.SetupSingleMode.GetValue<bool>()) // 11.06.10 minhan
                            {
                                GlobalVar.UlIFStatus = "UW";
                            }

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_UlHandSend2Position);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 450;
                        }
                        else if ((Rv > 0) || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send2PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send2 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 450:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandSend2Position))
                        {
                            //if (m_Simul.Motion) XFunc.GetTickCount(); // 11.02.25 minhan
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send2 Position Sensor : OK");
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            //seqNo = 460; // 09.12.08 minhan
                            seqNo = 455;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send2PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send2 Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 455: // 09.12.08 minhan
                    {
                        double CheckVel = m_UlHandUnit.Servo.GetVel(0);
                        VelocityChangeCount++;
                        if (CheckVel == OriginalVel)
                        {
                            VelocityChangeCount = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Velocity Change OK");
                            seqNo = 460;
                        }
                        else if (VelocityChangeCount > 10)
                        {
                            VelocityChangeCount = 0;
                            m_AlarmId = m_AlarmVelocityChange.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send2 Velocity Change : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            m_UlHandUnit.Servo.SetVel(0, OriginalVel);
                        }
                    }
                    break;
                case 460: // 11.02.25 minhan
                    {
                        bool chkTrpos = true;
                        chkTrpos &= (m_GantryUnit.Servo.GetCurPointId() == m_GantryRecvPosition);
                        chkTrpos &= m_TrControl.IsPositionConfirmed(m_GantryRecvPosition);

                        if (!chkTrpos)
                        {
                            m_UlServoUnit.RbtEStop();
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Position : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 3000;
                        }
                        else if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandSend1Position))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_UlHandSend1Position);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************

                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 470;
                        }
                        else if ((Rv > 0) || !m_UlServoUnit.Ready || !m_UlServoUnit.HomeComp) // 11.03.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosMove.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 470:
                    {
                        if (m_Control.IsPositionConfirmed(m_UlHandSend1Position))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1 Position Sensor : OK");
                            // Flag Set
                            m_UlHandUnit.IfFlag.OutReady = false;
                            m_UlHandUnit.IfFlag.OutComp = true;
                            seqNo = 480;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            GlobalVar.UlHnadAlarm = true;//2009.08.25 kimgun
                            m_AlarmId = m_UlHandUnit.ALM_Send1PosSensor.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Send1Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 480:
                    if (m_GantryUnit.IfFlag.InComp &&
                       (m_GantryUnit.Servo.GetCurPointId() == m_GantryWaitPosition)) // 11.02.25 minhan
                    {
                        m_UlHandUnit.IfFlag.OutComp = false;
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit In Complete Confirm");
                        m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 490; // 11.02.25 minhan
                    }
                    break;
                case 490: // 11.02.25 minhan
                    {
                        if (!m_UlHandUnit.IsGlassExist(Logic.AND))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass Sensor Check OK");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftAbnormal.Id;
                            m_Eqp.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Glass Exist Sensor Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {

                        GlobalVar.UlHnadAlarm = false;//2009.08.25 kimgun
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1000)");

                        seqNo = m_ReturnSeqNo;

                    }
                    break;
                case 1500:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {
                        GlobalVar.UlHnadAlarm = false;//2009.08.25 kimgun
                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1500)");

                        seqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000:
                    if (m_Eqp.AlarmResetSwitchPushed)
                    {//2009.08.25 kimgun
                        if (DialogResult.Yes == MessageBox.Show("Ul Fish Hand Unit Up Moving Interlock. Check Please(UL IN Sensor)", "WSSD",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)) // 11.02.25 minhan
                        {
                            GlobalVar.UlHnadAlarm = false;//2009.08.25 kimgun
                            m_Eqp.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(2000)");
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 3000: // 11.02.25 minhan Tr,UL hand Estop
                    {
                        bool EstopCheckTR = true;
                        bool EstopCheckUL = true;

                        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();
                        EstopCheckUL = m_UlServoUnit.RbtEStop();

                        if (EstopCheckTR && EstopCheckUL)
                        {

                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr and UL hand Estop OK");
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmIdServo = m_ManualChangeErrUL.Id;
                            m_Eqp.SetAlarm(m_AlarmIdServo);
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;

                        }
                    }
                    break;
            }
            this.m_SeqNo = seqNo;
            return -1;
        }
        #endregion
    }

    public class SeqUnitUlFishAirInterlock : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_Eqp;
        protected static ServerManager m_Server;
        private Simul m_Simul;
        private FishHand m_UlHandUnit;
        private GenInfoHandler m_GenInfo;
        #endregion

        #region Unit
        private TagGlassData m_RecvGlassData = new TagGlassData();
        private ThreadUl_Hand_BOE_G8_DHDC m_Control;
        #endregion

        #region Constructor
        public SeqUnitUlFishAirInterlock(ThreadUl_Hand_BOE_G8_DHDC control, FishHand UlHand)
        {
            m_UlHandUnit = UlHand;
            m_Server = ServerManager.Instance;
            m_Eqp = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_SeqFunName = m_UlHandUnit.Name;
            m_GenInfo = GenInfoHandler.Instance;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            int seqNo = this.m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    if ((m_UlHandUnit.FrontRib.IsSetFw() || m_UlHandUnit.FrontRib.IsFwSensing()) &&
                       !m_UlHandUnit.FrontRib.IsSetBw() && !m_Simul.Motion) // 11.02.09 minhan
                    {
                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > 5000)
                    {
                        seqNo = 20;
                    }
                    break;
                case 20:
                    if (m_UlHandUnit.FrontRib.IsSetFw() && !m_UlHandUnit.FrontRib.IsSetBw() &&
                        m_UlHandUnit.FrontRib.IsFwSensingOnly() &&
                        !m_UlHandUnit.IsHandPressureOk())
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, 0, 0, "UL Fish Hand AirInterlock Detected!");
                        GlobalVar.UlHandAirInterlock = true;

                        m_AlarmId = m_UlHandUnit.ALM_FishHandCdaLow.Id;
                        m_Eqp.SetAlarm(m_AlarmId);

                        seqNo = 1000;
                    }
                    else
                    {
                        seqNo = 0;
                    }
                    break;
                case 1000:
                    if ((m_Eqp.AlarmResetSwitchPushed &&
                        m_UlHandUnit.FrontRib.IsSetFw() &&
                        !m_UlHandUnit.FrontRib.IsSetBw() &&
                        m_UlHandUnit.IsHandPressureOk()) ||
                        (m_Eqp.AlarmResetSwitchPushed &&
                        !m_UlHandUnit.FrontRib.IsSetFw() &&
                        m_UlHandUnit.FrontRib.IsSetBw() &&
                        !m_UlHandUnit.IsHandPressureOk())) // 09.09.28 minhan
                    {

                        GlobalVar.UlHandAirInterlock = false;

                        m_Eqp.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Control.SetLog(m_SeqFunName, seqNo, 0, 0, "UL Fish Hand AirInterlock Recovery!");
                        seqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }


}

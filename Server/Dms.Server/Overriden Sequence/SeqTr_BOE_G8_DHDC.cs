using System;
using System.Collections.Generic;
using System.Text;
using Dms.Sequence;
using Dms.Common;
using Dms.Data;
using Dms.Device;
using System.Windows.Forms;
using System.Threading;
using Dms.ServerCommon;

namespace Dms.Server
{
    #region Enum
    public class AlignCommand
    {
        public enum TrAlign
        {
            _alignON = 0,
            _alignOFF
        }
    }
    #endregion

    public class ThreadTr_BOE_G8_DHDC : ThreadGantryControl
    {
        #region Fields
        private static GantryUnit m_GantryUnit;
        private static GenInfoHandler m_GenInfo; // 11.02.01 minhan
        private XLog TRLog; // 11.02.25 minhan
        #endregion

        #region Constructor
        public ThreadTr_BOE_G8_DHDC(int scanTime, ServerManager server)
            : base(scanTime, server)
        {
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_GenInfo = GenInfoHandler.Instance; // 11.02.01 minhan
            TRLog = new XLog("TR Log", XLog.LogStampType.UseStamp);
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (GantryUnit device in m_GantryUnits)
            {
                device.IfFlag.Reset();

                if (device.Name == eqpTransferUnits._TR_Gantry_Unit_Name)
                {
                    RegisterSequence(new SeqUnitTr(this, device));
                    RegisterSequence(new SeqUnitTrInterlock(this, device));
                }
            }

            m_Server.AddSeqInitFunction(new SeqInitTr(this, m_Server));
        }
        #endregion

        #region Override Methods
        public override void InitParameter() // 09.05.30 minhan override 해서 사용
        {
            foreach (GantryUnit unit in m_GantryUnits)
            {
                if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
                unit.IfFlag.Reset();
                //jemoon : 아래 flag는 cvseq의 같은 flag와 의미가 다름 : false로 초기화 되어야함
                unit.IfFlag.InComp = false;
                unit.IfFlag.OutComp = false;
            }
        }
        #endregion

        #region Methods
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

            log = string.Format("TrUnit  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            TRLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }

        public bool IsSeqRunCondition(GantryUnit tr) // 09.05.30 minhan
        {
            bool run = true;
            run &= !IsInterlock();
            // run &= !CvStopCondition(cv.NextCv);
            run &= !m_GenInfo.Pause;
            run &= m_GenInfo.AutoMode;
            return run;
        }

        public bool IsTrRobotInterrupt()
        {
            if (AppConfig.Instance.Simul.Motion) return false;

            bool Intr = false;
            Intr |= m_GantryUnit.IsRobotInterlock();
            return Intr;
        }

        public bool IsInterlock()
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool Interlock = false;
            //Interlock |= ((heavy & ~HeavyInterlock.Emo) > 0 );
            //Interlock |= ((heavy & ~HeavyInterlock.Leak) > 0 );//2010.06.29 kimgun
            Interlock |= (heavy & HeavyInterlock.Door) > 0;//2010.06.29 kimgun
            Interlock |= (heavy & HeavyInterlock.Area) > 0;//2010.06.29 kimgun
            return Interlock;
        }

        public bool IsPositionConfirmed(short posId)
        {
            if (posId == eqpPos_TR_Servo_Unit.Home)
            {
                return m_GantryUnit.Servo.GetServoMotor(0).GetHomeSwitch();
            }
            else if (posId == eqpPos_TR_Servo_Unit.Wait)
            {
                return m_GantryUnit.diWait_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_TR_Servo_Unit.Recv)
            {
                return m_GantryUnit.diRecv_Pos_Sensor.IsDetected();
            }
            else if (posId == eqpPos_TR_Servo_Unit.Send)
            {
                return m_GantryUnit.diSend_Pos_Sensor.IsDetected();
            }
            return false;
        }

        /// <summary>
        /// only for simulation
        /// </summary>
        /// <param name="posId"></param>
        public void SetPositionConfirmed(short posId)
        {
            if (posId == eqpPos_TR_Servo_Unit.Home)
            {
                m_GantryUnit.diSend_Pos_Sensor.SetState(false);
                m_GantryUnit.diWait_Pos_Sensor.SetState(false);
                m_GantryUnit.diRecv_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_TR_Servo_Unit.Recv)
            {
                m_GantryUnit.diSend_Pos_Sensor.SetState(false);
                m_GantryUnit.diWait_Pos_Sensor.SetState(false);
                m_GantryUnit.diRecv_Pos_Sensor.SetState(true);
            }
            else if (posId == eqpPos_TR_Servo_Unit.Send)
            {
                m_GantryUnit.diSend_Pos_Sensor.SetState(true);
                m_GantryUnit.diWait_Pos_Sensor.SetState(false);
                m_GantryUnit.diRecv_Pos_Sensor.SetState(false);
            }
            else if (posId == eqpPos_TR_Servo_Unit.Wait)
            {
                m_GantryUnit.diSend_Pos_Sensor.SetState(false);
                m_GantryUnit.diWait_Pos_Sensor.SetState(true);
                m_GantryUnit.diRecv_Pos_Sensor.SetState(false);
            }
        }
        #endregion
    }

    public class SeqInitTr : XSeqInitFunction
    {
        #region Fields
        private InitState m_InitState = InitState.Noop;
        protected static IServerManager m_Server;
        protected static Simul m_Simul;
        protected static TransferUnit m_Unit;
        protected static IEqpManager m_EqpManager;
        protected static ThreadTr_BOE_G8_DHDC m_Control;
        protected static _GenericCollection<CvUnit> m_Units;

        public Alarm ALM_InitFail = null;

        private GantryUnit m_GantryUnit = null;
        //private FishHand m_LdHandUnit = null;
        private FishHand m_UlHandUnit = null;
        //private _ServoUnit m_LdServoUnit = null;
        private _ServoUnit m_UlServoUnit = null;
        protected GenericTag m_InitGantryMove = new GenericTag("Gantry Unit Move Wait Position", InitCheckState.NotReady);
        protected GenericTag m_InitRobotInterrupt = new GenericTag("Robot Hand Interrupt", InitCheckState.NotReady);
        // protected GenericTag m_InitCheckLdFish = new GenericTag("LD Fish Hand Position", InitCheckState.NotReady);
        // protected GenericTag m_InitCheckLdFishRip = new GenericTag("LD Fish Hand Rip", InitCheckState.NotReady);
        protected GenericTag m_InitCheckUlFish = new GenericTag("UL Fish Hand Position", InitCheckState.NotReady);
        protected GenericTag m_InitCheckUlFishRip = new GenericTag("UL Fish Hand Rip", InitCheckState.NotReady);
        protected GenericTag m_InitGnatryAlignBw = new GenericTag("Gantry Unit Align BW", InitCheckState.NotReady);
        protected GenericTag m_InitSetCleanOut = new GenericTag("Clean-Out Set", InitCheckState.NotReady);

        private short m_GantryHomePosition;
        private short m_GantrySendPosition;
        private short m_GantryWaitPosition;
        private short m_GantryRecvPosition;

        //private short m_LdHandHomePosition;
        //private short m_LdHandRecv1Position;
        //private short m_LdHandRecv2Position;
        //private short m_LdHandRecv3Position;
        //private short m_LdHandWaitPosition;
        //private short m_LdHandSend1Position;
        //private short m_LdHandSend2Position;

        private short m_UlHandHomePosition;
        private short m_UlHandRecv1Position;
        private short m_UlHandRecv2Position;
        private short m_UlHandWaitPosition;
        private short m_UlHandSend1Position;
        private short m_UlHandSend2Position;
        private short m_UlHandSend3Position;

        private GenInfoHandler m_GenInfo;

        //private ThreadLd_Hand_BOE_G8_DHDC m_LdHandControl;
        private ThreadUl_Hand_BOE_G8_DHDC m_UlHandControl;
        private bool InitialRestart = false;//2010.06.29 kimgun
        #endregion

        #region Constructor
        public SeqInitTr(ThreadTr_BOE_G8_DHDC control, IServerManager server)
        {
            this.m_SeqFunName = "INIT    ";
            m_Server = server;
            m_Control = control;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Units = DmsComponents.Instance.ComponentContainer.GetCollection<CvUnit>();
            m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            //m_LdHandUnit = eqpTransferUnits._LD_Fish_Hand;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;
            //m_LdServoUnit = eqpServoUnits._LD_Hand_Servo_Unit;
            m_UlServoUnit = eqpServoUnits._UL_Hand_Servo_Unit;

            m_GantryHomePosition = eqpPos_TR_Servo_Unit.Home;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send;
            m_GantryWaitPosition = eqpPos_TR_Servo_Unit.Wait;
            m_GantryRecvPosition = eqpPos_TR_Servo_Unit.Recv;

            //m_LdHandHomePosition = eqpPos_LD_Hand_Servo_Unit.Home_Position;
            //m_LdHandRecv1Position = eqpPos_LD_Hand_Servo_Unit.Recv1_Position;
            //m_LdHandRecv2Position = eqpPos_LD_Hand_Servo_Unit.Recv2_Position;
            //m_LdHandRecv3Position = eqpPos_LD_Hand_Servo_Unit.Recv3_Position;
            //m_LdHandWaitPosition = eqpPos_LD_Hand_Servo_Unit.Wait_Position;
            //m_LdHandSend1Position = eqpPos_LD_Hand_Servo_Unit.Send1_Position;
            //m_LdHandSend2Position = eqpPos_LD_Hand_Servo_Unit.Send2_Position;

            m_UlHandHomePosition = eqpPos_UL_Hand_Servo_Unit.Home;
            m_UlHandRecv1Position = eqpPos_UL_Hand_Servo_Unit.Recv1;
            m_UlHandRecv2Position = eqpPos_UL_Hand_Servo_Unit.Recv2;
            m_UlHandWaitPosition = eqpPos_UL_Hand_Servo_Unit.Wait;
            m_UlHandSend1Position = eqpPos_UL_Hand_Servo_Unit.Send1;
            m_UlHandSend2Position = eqpPos_UL_Hand_Servo_Unit.Send2;
            m_UlHandSend3Position = eqpPos_UL_Hand_Servo_Unit.Send3;

            m_GenInfo = GenInfoHandler.Instance;

            //m_LdHandControl = ServerManager.Instance.ThreadHandler.LdHandControl; // 11.02.25 minhan
            //m_UlHandControl = ServerManager.Instance.ThreadHandler.UlHandControl;
        }
        #endregion

        #region Methods
        private int GetRbtPos(_ServoUnit unit)
        {
            int nPos = -1;
            if (unit.Name == eqpServoUnits._TR_Servo_Unit_Name)
            {
                if (m_GantryUnit.Servo.GetServoMotor(0).GetHomeSwitch()) nPos = eqpPos_TR_Servo_Unit.Home;
                else if (m_GantryUnit.diWait_Pos_Sensor.IsDetected()) nPos = eqpPos_TR_Servo_Unit.Wait;
                else if (m_GantryUnit.diSend_Pos_Sensor.IsDetected()) nPos = eqpPos_TR_Servo_Unit.Send;
                else if (m_GantryUnit.diRecv_Pos_Sensor.IsDetected()) nPos = eqpPos_TR_Servo_Unit.Recv;
            }
            //else if (unit.Name == eqpServoUnits._LD_Hand_Servo_Unit_Name)
            //{
            //    if (eqpSensors._LD_Hand_Unit_Wait_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Wait_Position;
            //    else if (eqpSensors._LD_Hand_Unit_Send1_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Send1_Position;
            //    else if (eqpSensors._LD_Hand_Unit_Send2_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Send2_Position;
            //    else if (eqpSensors._LD_Hand_Unit_Recv1_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Recv1_Position;
            //    else if (eqpSensors._LD_Hand_Unit_Recv2_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Recv2_Position;
            //    else if (eqpSensors._LD_Hand_Unit_Recv3_Position_Sensor.IsDetected()) nPos = eqpPos_LD_Hand_Servo_Unit.Recv3_Position;
            //}
            else if (unit.Name == eqpServoUnits._UL_Hand_Servo_Unit_Name)
            {
                if (m_UlHandUnit.diWait_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Wait;
                else if (m_UlHandUnit.diRecv1_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Recv1;
                else if (m_UlHandUnit.diRecv2_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Recv2;
                else if (m_UlHandUnit.diSend1_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Send1;
                else if (m_UlHandUnit.diSend2_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Send2;
                else if (m_UlHandUnit.diSend3_Pos_Sensor.IsDetected()) nPos = eqpPos_UL_Hand_Servo_Unit.Send3;
            }

            return nPos;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_GenInfo.CleanOut && !InitialRestart)
            {
                InitialRestart = true;
            }
            if (m_InitState == InitState.Comp)
            {
                if (!m_GenInfo.CleanOut && InitialRestart)//2010.06.29 kimgun cleanout 완료되면 초기화를 새로 하기 위해.
                {
                    m_InitState = InitState.Noop;
                }
                return (int)m_InitState;
            }

            int nSeqNo = this.m_SeqNo;
            int Rv = -1;
            double servoCurPos = 0.0;

            //if (m_LdHandControl == null) m_LdHandControl = ServerManager.Instance.ThreadHandler.LdHandControl; // 11.02.25 minhan
            if (m_UlHandControl == null) m_UlHandControl = ServerManager.Instance.ThreadHandler.UlHandControl;

            switch (nSeqNo)
            {
                case 0:
                    if (m_GenInfo.EqpInitReq)
                    {
                        m_InitState = InitState.Init;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Seq Gantry Init : Start");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        bool bOk = true;

                        if ((m_Server.GlassData.Count == 0) && (m_GantryUnit.IsGlassExist(Logic.OR)))
                        {
                            bOk = false;
                            m_AlarmId = m_GantryUnit.ALM_GlassExist.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                        }
                        //else if ((m_Server.GlassData.Count == 0) && (m_LdHandUnit.IsGlassExist(Logic.OR)))
                        //{
                        //    bOk = false;
                        //    AlarmId = m_LdHandUnit.ALM_FishGlsNotSensing.Id;
                        //    m_EqpManager.SetAlarm(AlarmId);
                        //}
                        else if ((m_Server.GlassData.Count == 0) && (m_UlHandUnit.IsGlassExist(Logic.OR)))
                        {
                            bOk = false;
                            m_AlarmId = m_UlHandUnit.ALM_FishGlsNotSensing.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                        }

                        if (bOk)
                        {
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20: // 11.02.25 minhan
                    {
                        bool bOk = true;

                        //if ((m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0))) && (!m_LdHandUnit.IsGlassExist(Logic.OR))) // 듀얼
                        //{
                        //    if (m_Simul.Motion)
                        //    {
                        //        m_LdHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                        //        m_LdHandUnit.SetHandUp();
                        //        bOk = true;
                        //    }
                        //    else
                        //    {
                        //        bOk = false;
                        //        AlarmId = m_LdHandUnit.ALM_FishGlsNotSensing.Id;
                        //        m_EqpManager.SetAlarm(AlarmId);
                        //    }
                        //}
                        if ((m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0))) && (!m_UlHandUnit.IsGlassExist(Logic.OR))) // 듀얼
                        {
                            if (m_Simul.Motion)
                            {
                                m_UlHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                                m_UlHandUnit.SetHandUp();
                                bOk = true;
                            }
                            else
                            {
                                bOk = false;
                                m_AlarmId = m_UlHandUnit.ALM_FishGlsNotSensing.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                            }
                        }

                        if (bOk)
                        {
                            nSeqNo = 90;
                        }
                        else
                        {
                            m_InitState = InitState.Fail;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 90:
                    {
                        if (GetElapsedTicks() > 500)
                        {
                            nSeqNo = 100;
                        }
                    }
                    break;
                case 100:
                    {
                        if (m_Simul.Motion)
                        {
                            m_UlHandUnit.diRecv1_Pos_Sensor.SetState(true);
                        }
                        nSeqNo = 200;
                        //        }

                        //        bool ldfishRibUp = m_LdHandUnit.IsHandUp();
                        //        bool ldfishRibDn = m_LdHandUnit.IsHandDown();

                        //        short curLdUpPosition = (short)GetRbtPos(m_LdHandUnit.Servo); //LeeChungWon : 090901
                        //        m_InitCheckLdFishRip.Value = InitCheckState.Checking;
                        //        m_InitCheckLdFish.Value = InitCheckState.Checking;

                        //        if ((ldfishRibUp && ldfishRibDn) || (!ldfishRibUp && !ldfishRibDn))
                        //        {
                        //            if (m_Simul.Motion)
                        //            {
                        //                m_LdHandUnit.SetHandDown();
                        //            }
                        //            else
                        //            {
                        //                AlarmId = m_LdHandUnit.ALM_FishLiftAbnormal.Id;
                        //                m_EqpManager.SetAlarm(AlarmId);
                        //                m_InitCheckLdFishRip.Value = InitCheckState.NG; // 10.12.25 minhan
                        //                m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Rib Abnormal State : Error");
                        //                m_InitState = InitState.Fail;
                        //                nSeqNo = 1000;
                        //                break;
                        //            }
                        //        }

                        //        if (curLdUpPosition < 0)
                        //        {
                        //            if (m_Simul.Motion)
                        //            {
                        //                m_LdServoUnit.SetCurPosition(m_LdHandHomePosition);
                        //            }
                        //            else
                        //            {
                        //                AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                        //                m_EqpManager.SetAlarm(AlarmId);
                        //                m_InitCheckLdFish.Value = InitCheckState.NG;
                        //                m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Position Sensor Detected : Error");
                        //                m_InitState = InitState.Fail;
                        //                nSeqNo = 1000;
                        //                break;
                        //            }
                        //        }
                        //        else if (curLdUpPosition == m_LdHandRecv1Position ||
                        //                 curLdUpPosition == m_LdHandRecv2Position ||
                        //                 curLdUpPosition == m_LdHandRecv3Position)
                        //        {
                        //            int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 10.12.25 minhan Tr이 어느 포지션이 든 있다면...
                        //            if (curTrPosition > -1)
                        //            {
                        //                if (!ldfishRibUp)
                        //                {
                        //                    m_LdHandUnit.SetHandUp();
                        //                    m_StartTicks = XFunc.GetTickCount();
                        //                }
                        //                nSeqNo = 110;
                        //            }
                        //            else
                        //            {
                        //                m_InitCheckLdFish.Value = InitCheckState.NG;
                        //                m_InitState = InitState.Fail;
                        //                AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                        //                m_EqpManager.SetAlarm(AlarmId);
                        //                m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "TR Position Sensor : Error");
                        //                nSeqNo = 1000;
                        //                break;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit and TR Unit No Interfere");
                        //            nSeqNo = 190;
                        //        }
                    }
                    break;
                //case 110: // 11.02.25 minhan
                //    if (m_LdHandUnit.IsHandUp() && !m_LdHandUnit.IsHandDown())
                //    {
                //        int curLdUpPosition = GetRbtPos(m_LdHandUnit.Servo); //LeeChungWon : 090901

                //        if (curLdUpPosition == m_LdHandRecv2Position)
                //        {

                //            int rv = m_LdHandUnit.Servo.SetCurPosition((short)curLdUpPosition);

                //            if ((rv == 0) || m_Simul.Motion)
                //            {
                //                m_StartTicks = XFunc.GetTickCount();
                //                nSeqNo = 120;
                //            }
                //            else if (0 < rv)
                //            {
                //                AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Set Position : Error - " + rv.ToString());
                //                m_InitCheckLdFish.Value = InitCheckState.NG;  // 10.12.25 minhan
                //                m_InitState = InitState.Fail;
                //                nSeqNo = 1000;
                //                break;
                //            }
                //        }
                //        else if (curLdUpPosition < 0) // 11.02.25 minhan 이렇게 되야 되는거 아닌가?
                //        {
                //            AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Position : Error");
                //            m_InitCheckLdFish.Value = InitCheckState.NG;  // 10.12.25 minhan
                //            m_InitState = InitState.Fail;
                //            nSeqNo = 1000;
                //            break;
                //        }
                //        else
                //        {
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit and TR Unit No Interfere(case 110)");
                //            nSeqNo = 190;
                //        }
                //    }
                //    else if (GetElapsedTicks() > 5000)
                //    {
                //        m_InitCheckLdFishRip.Value = InitCheckState.NG;
                //        m_InitState = InitState.Fail; // 10.12.25 minhan
                //        AlarmId = m_LdHandUnit.ALM_FishLiftUp.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Rib Up : Error");
                //        nSeqNo = 1500;
                //    }
                //    break;
                //case 120:
                //    if (m_LdHandUnit.Servo.RbtReset())
                //    {
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Servo Reset");

                //        //***************** Position Log **************************
                //        RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                //        m_LdServoUnit.GetCurPosition(ref curPos);
                //        servoCurPos = curPos.Pos[0];

                //        string sCurPos;
                //        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, sCurPos);
                //        //**********************************************************

                //        nSeqNo = 130;
                //    }
                //    else if (GetElapsedTicks() > 5000)
                //    {
                //        m_InitCheckLdFish.Value = InitCheckState.NG;
                //        m_InitState = InitState.Fail; // 10.12.25 minhan
                //        AlarmId = m_LdHandUnit.ALM_ServoReset.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Servo Reset : Error");
                //        nSeqNo = 1500;
                //    }
                //    break;
                //case 130: // 11.02.25 minhan
                //    {
                //        bool checkhand = m_LdHandUnit.IsHandUp();
                //             checkhand &= !m_LdHandUnit.IsHandDown();

                //        int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 11.02.25 minhan


                //        if (curTrPosition < 0) // 이동중에 tr 포지션을 모른다 무조건 stop
                //        {
                //            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                //            m_LdHandUnit.Servo.RbtEStop();
                //            m_UlHandUnit.Servo.RbtEStop();
                //            m_InitCheckLdFish.Value = InitCheckState.NG;
                //            m_InitState = InitState.Fail;
                //            AlarmId = m_LdHandUnit.ALM_Recv1PosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "Tr Position : Error");
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //            nSeqNo = 2000;
                //        }
                //        else if (!checkhand)
                //        {
                //            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                //            m_LdHandUnit.Servo.RbtEStop();
                //            m_UlHandUnit.Servo.RbtEStop();
                //            m_InitCheckLdFish.Value = InitCheckState.NG;
                //            m_InitState = InitState.Fail;
                //            AlarmId = m_LdHandUnit.ALM_Recv1PosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "Hand Status : Error");
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //            nSeqNo = 2000;
                //        }
                //        else if ((0 == (Rv = m_LdServoUnit.RbtMovePos(m_LdHandRecv1Position))))
                //        {
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit Recv1 Position Move : OK");

                //            if (m_Simul.Motion) m_LdHandControl.SetPositionConfirmed(m_LdHandRecv1Position);

                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                //            m_LdServoUnit.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, sCurPos);
                //            //**********************************************************

                //            nSeqNo = 140;
                //        }
                //        else if (Rv > 0 || !m_LdServoUnit.Ready) // 11.02.25 minhan
                //        {
                //            m_InitCheckLdFish.Value = InitCheckState.NG;
                //            m_InitState = InitState.Fail; // 10.12.25 minhan
                //            AlarmId = m_LdHandUnit.ALM_Recv1PosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit Recv1 Position Move : Error");

                //            string sRv;
                //            sRv = string.Format("Rv = {0}", Rv);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, sRv);

                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_LdServoUnit.AxisCount);
                //            m_LdServoUnit.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, sCurPos);
                //            //**********************************************************

                //            nSeqNo = 1000;
                //        }
                //    }
                //    break;
                //case 140:
                //    if (m_LdHandControl.IsPositionConfirmed(m_LdHandRecv1Position))
                //    {
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit Recv1 Position Sensor : OK");
                //        nSeqNo = 190;
                //    }
                //    else if (GetElapsedTicks() > 2000)
                //    {
                //        m_InitCheckLdFish.Value = InitCheckState.NG;
                //        m_InitState = InitState.Fail; // 10.12.25 minhan
                //        AlarmId = m_LdHandUnit.ALM_Recv1PosSensor.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Unit Recv1 Position Sensor : Error");
                //        nSeqNo = 1500;
                //    }
                //    break;
                //case 190:
                //    {
                //        if (m_Simul.Device)
                //        {
                //            m_StartTicks = XFunc.GetTickCount();
                //            //m_UlHandUnit.SetHandUp(); // 10.12.25 minhan
                //        }
                //        nSeqNo = 195;
                //    }
                //    break;
                //case 195:
                //    {
                //        if (m_Simul.Device) // 10.12.25 minhan
                //        {
                //            if (GetElapsedTicks() > 500)
                //            {
                //                nSeqNo = 200;
                //            }
                //        }
                //        else nSeqNo = 200;
                //    }
                //    break;
                case 200:
                    {
                        short curUlUpPosition = (short)GetRbtPos(m_UlHandUnit.Servo); //LeeChungWon : 090901
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                        //m_InitCheckLdFish.Value = InitCheckState.OK;
                        // m_InitCheckLdFishRip.Value = InitCheckState.OK;

                        m_InitCheckUlFish.Value = InitCheckState.Checking;
                        m_InitCheckUlFishRip.Value = InitCheckState.Checking;

                        if ((ulFishRibUp && ulFishRibDn) || (!ulFishRibUp && !ulFishRibDn))
                        {
                            if (m_Simul.Motion)
                            {
                                m_UlHandUnit.SetHandDown();
                            }
                            else
                            {
                                m_AlarmId = m_UlHandUnit.ALM_FishLiftAbnormal.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_InitCheckUlFishRip.Value = InitCheckState.NG;
                                m_InitState = InitState.Fail; // 10.12.25 minhan
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Unit Rib State : Error");
                                nSeqNo = 1500;
                                break; // 10.12.25 minhan
                            }
                        }

                        if (curUlUpPosition < 0) // 10.12.25 minhan
                        {
                            if (m_Simul.Motion)
                            {
                                m_UlServoUnit.SetCurPosition(m_UlHandHomePosition);
                            }
                            else
                            {
                                m_InitCheckUlFish.Value = InitCheckState.NG;
                                m_InitState = InitState.Fail; // 10.12.25 minhan
                                m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Unit Position Sensor : Error");
                                nSeqNo = 1000;
                            }
                        }
                        else if (curUlUpPosition > m_UlHandSend1Position)
                        {
                            int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 10.12.25 minhan Tr이 어느 포지션이 든 있다면...

                            if (curTrPosition > -1)
                            {
                                if (!ulFishRibUp)
                                {
                                    m_UlHandUnit.SetHandUp();
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Unit Rib Up Start");
                                }
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 210;
                            }
                            else
                            {
                                m_InitCheckUlFish.Value = InitCheckState.NG;
                                m_InitState = InitState.Fail;
                                m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Position Sensor  : Error");
                                nSeqNo = 1000;
                            }
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR & UL Hand Unit No Interfere");
                            nSeqNo = 290;
                        }
                    }
                    break;
                case 210:
                    {
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();
                        //m_InitRobotInterrupt.Value = InitCheckState.Checking; // 10.12.25 minhan

                        if (ulFishRibUp && !ulFishRibDn)
                        {
                            int curUlUpPosition = GetRbtPos(m_UlHandUnit.Servo); //LeeChungWon : 090901
                            if (curUlUpPosition == m_UlHandSend2Position)
                            {
                                int rv = m_UlHandUnit.Servo.SetCurPosition((short)curUlUpPosition);

                                if (rv == 0 || m_Simul.Motion)
                                {
                                    m_StartTicks = XFunc.GetTickCount();
                                    nSeqNo = 220;
                                }
                                else if (0 < rv)
                                {
                                    m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_InitCheckUlFish.Value = InitCheckState.NG;  // 10.12.25 minhan
                                    m_InitState = InitState.Fail;
                                    m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Ul Hand Set Position : Error - " + rv.ToString());
                                    nSeqNo = 1000;
                                    break;
                                }
                            }
                            else if (curUlUpPosition < 0) // 11.02.25 minhan
                            {
                                m_InitCheckUlFish.Value = InitCheckState.NG;
                                m_InitState = InitState.Fail; // 10.12.25 minhan
                                m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Ul Hand Position : Error");
                                nSeqNo = 1000;
                                break;
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR & UL Hand Unit No Interfere");
                                nSeqNo = 290;
                            }
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            m_InitCheckUlFishRip.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail; // 10.12.25 minhan
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftUp.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Rib Up : Error");
                            nSeqNo = 1500;
                        }

                    }
                    break;
                case 220:
                    if (m_UlHandUnit.Servo.RbtReset())
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Servo Reset");

                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                        m_UlServoUnit.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                        //**********************************************************

                        nSeqNo = 230;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_InitCheckUlFish.Value = InitCheckState.NG; // 10.12.25 minhan
                        m_InitState = InitState.Fail;
                        m_AlarmId = m_UlHandUnit.ALM_ServoReset.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Servo Reset : Error");
                        nSeqNo = 1500;
                    }
                    break;
                case 230: // 11.02.25 minhan
                    {
                        bool checkhand = m_UlHandUnit.IsHandUp();
                        checkhand &= !m_UlHandUnit.IsHandDown();

                        int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 11.02.25 minhan

                        if (curTrPosition < 0) // 이동중에 tr 포지션을 모른다 무조건 stop
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            //m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitCheckUlFish.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_Send3PosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Tr Position : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if (!checkhand)
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            //m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitCheckUlFish.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_Send3PosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Hand Status : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if ((0 == (Rv = m_UlServoUnit.RbtMovePos(m_UlHandSend3Position))))
                        {

                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Unit Send3 Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_UlHandControl.SetPositionConfirmed(m_UlHandSend3Position);
                            }

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 10.12.25 minhan
                            nSeqNo = 240;
                        }
                        else if (Rv > 0 || !m_UlServoUnit.Ready) // 11.02.25 minhan
                        {
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_UlServoUnit.AxisCount);
                            m_UlServoUnit.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                            //**********************************************************

                            m_AlarmId = m_UlHandUnit.ALM_Send3PosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Unit Send3 Position Mov : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sRv);
                            m_InitCheckUlFish.Value = InitCheckState.NG; // 10.12.25 minhan
                            m_InitState = InitState.Fail;
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 240:
                    if (m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position))
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Unit Send3 Position Sensor : OK");
                        nSeqNo = 290;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitCheckUlFish.Value = InitCheckState.NG; // 10.12.25 minhan
                        m_InitState = InitState.Fail;
                        m_AlarmId = m_UlHandUnit.ALM_Send3PosSensor.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Unit Send3 Position Sensor : Error");
                        nSeqNo = 1500;
                    }
                    break;
                case 290:
                    {
                        m_InitCheckUlFish.Value = InitCheckState.OK;
                        m_InitCheckUlFishRip.Value = InitCheckState.OK;
                        m_InitRobotInterrupt.Value = InitCheckState.Checking; // 10.12.25 minhan
                        nSeqNo = 300;
                    }
                    break;
                case 300:
                    if (m_Control.IsTrRobotInterrupt())
                    {
                        m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_InitRobotInterrupt.Value = InitCheckState.NG;
                        m_InitState = InitState.Fail; // 10.12.25 minhan
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Robot Hand Interrupt : Error");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        m_InitRobotInterrupt.Value = InitCheckState.OK;
                        m_InitGnatryAlignBw.Value = InitCheckState.Checking;
                        m_GantryUnit.AlignSet(ActuatorAct.Neg);
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 310;
                    }
                    break;
                case 310:
                    if (m_GantryUnit.IsAlignBw())
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Align BW : OK");
                        m_InitGnatryAlignBw.Value = InitCheckState.OK;
                        m_InitGantryMove.Value = InitCheckState.ServoReset;
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 320;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Align BW : Error");
                        m_InitGnatryAlignBw.Value = InitCheckState.NG;
                        m_InitState = InitState.Fail; // 10.12.25 minhan
                        nSeqNo = 1500;
                    }
                    break;
                case 320:
                    if (m_GantryUnit.Servo.RbtReset())
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Servo Reset : OK");
                        m_InitGantryMove.Value = InitCheckState.ServoHoming;
                        nSeqNo = 330;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_InitGantryMove.Value = InitCheckState.NG;  // 10.12.25 minhan
                        m_InitState = InitState.Fail;
                        m_AlarmId = m_GantryUnit.ALM_ServoReset.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Servo Reset : Error");
                        nSeqNo = 1500;
                    }
                    break;
                case 330: // hand의 위치도 보면 좋은데...앞에서 Hand 처리하는 부분을 인정하고 들어 왔으니.
                    {
                        //short curLdUpPosition = (short)GetRbtPos(m_LdHandUnit.Servo);
                        short curUlUpPosition = (short)GetRbtPos(m_UlHandUnit.Servo); // 11.02.25 minhan
                        //bool ldfishRibUp = m_LdHandUnit.IsHandUp();
                        //bool ldfishRibDn = m_LdHandUnit.IsHandDown();
                        bool ulfishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulfishRibDn = m_UlHandUnit.IsHandDown();

                        if (m_Control.IsTrRobotInterrupt())
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;  // 10.12.25 minhan
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Robot Hand Interrupt : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if (!m_GantryUnit.IsAlignBw()) // 10.12.25 minhan
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Align BW : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        //else if (!ldfishRibUp && !ldfishRibDn)
                        //{
                        //    m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                        //    m_LdHandUnit.Servo.RbtEStop();
                        //    m_UlHandUnit.Servo.RbtEStop();
                        //    m_InitGantryMove.Value = InitCheckState.NG;
                        //    m_InitState = InitState.Fail;
                        //    AlarmId = m_LdHandUnit.ALM_FishLiftAbnormal.Id;
                        //    m_EqpManager.SetAlarm(AlarmId);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Fishbone Cylinder Error");
                        //    m_StartTicks = XFunc.GetTickCount();
                        //    nSeqNo = 2000;
                        //}
                        else if (!ulfishRibUp && !ulfishRibDn)
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            //m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_FishLiftAbnormal.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Fishbone Cylinder Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        //else if ((curLdUpPosition < 0) ||
                        //        (curLdUpPosition == m_LdHandRecv2Position) ||
                        //        m_LdHandControl.IsPositionConfirmed(m_LdHandRecv2Position)) // 11.02.25 minhan LD HAND의 위치를 모르거나 recv2 위치에 있다면.
                        //{
                        //    m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                        //    m_LdHandUnit.Servo.RbtEStop();
                        //    m_UlHandUnit.Servo.RbtEStop();
                        //    m_InitGantryMove.Value = InitCheckState.NG;
                        //    m_InitState = InitState.Fail;
                        //    AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                        //    m_EqpManager.SetAlarm(AlarmId);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Position : Error");
                        //    m_StartTicks = XFunc.GetTickCount();
                        //    nSeqNo = 2000;
                        //}
                        else if ((curUlUpPosition < 0) ||
                                (curUlUpPosition == m_UlHandSend2Position) ||
                                m_UlHandControl.IsPositionConfirmed(m_UlHandSend2Position)) // 11.02.25 minhan UL HAND의 위치를 모르거나 send2 위치에 있다면.
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            //m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Position : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        //else if (((curLdUpPosition == m_LdHandRecv1Position) ||
                        //          //(curLdUpPosition == m_LdHandRecv2Position) ||
                        //          (curLdUpPosition == m_LdHandRecv3Position)) &&
                        //          !ldfishRibUp)
                        //{
                        //    m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                        //    m_LdHandUnit.Servo.RbtEStop();
                        //    m_UlHandUnit.Servo.RbtEStop();
                        //    m_InitGantryMove.Value = InitCheckState.NG;
                        //    m_InitState = InitState.Fail;
                        //    AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                        //    m_EqpManager.SetAlarm(AlarmId);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand : Error");
                        //    m_StartTicks = XFunc.GetTickCount();
                        //    nSeqNo = 2000;
                        //}
                        else if (((curUlUpPosition == m_UlHandSend1Position) ||
                                //(curUlUpPosition == m_UlHandSend2Position) ||
                                (curUlUpPosition == m_UlHandSend3Position)) &&
                                !ulfishRibUp)
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if ((0 == (Rv = m_GantryUnit.Servo.RbtMoveHome())))
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Home Position Move : OK");

                            if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                            {
                                //***************** Position Log **************************
                                RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                                m_GantryUnit.Servo.GetCurPosition(ref curPos);
                                servoCurPos = curPos.Pos[0];

                                string sCurPos;
                                sCurPos = string.Format("Start Position = {0}", servoCurPos);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                                //**********************************************************
                                nSeqNo = 340;
                            }
                            else
                            {
                                m_InitGantryMove.Value = InitCheckState.NG;  // 10.12.25 minhan
                                m_InitState = InitState.Fail;
                                m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Align Bw : Error");
                                nSeqNo = 1000;
                            }

                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready) // 11.02.25 minhan
                        {
                            m_InitGantryMove.Value = InitCheckState.NG;  // 10.12.25 minhan
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_HomePosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Home Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sRv);

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 340:
                    {
                        if (m_Control.IsTrRobotInterrupt())
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Robot Hand Interrupt : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if (!m_GantryUnit.IsAlignBw()) // 10.12.25 minhan Home 에 와서 Wait  이동이니 이것만 봐도 될 것 같네.
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            //m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitGantryMove.Value = InitCheckState.NG;
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Align BW : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryWaitPosition))))// && !m_Simul.Motion) // 10.12.25 minhan 왜 이건 if
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Wait Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantryWaitPosition);
                            }

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 10.12.25 minhan
                            nSeqNo = 350;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready) // 11.02.25 minhan
                        {
                            m_InitGantryMove.Value = InitCheckState.NG; // 10.12.25 minhan
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Gantry Unit Wait Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sCurPos);
                            //**********************************************************

                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 350:
                    if (m_Control.IsPositionConfirmed(m_GantryWaitPosition))
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Unit Wait Position Sensor : OK");
                        m_InitGantryMove.Value = InitCheckState.OK;
                        nSeqNo = 390;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitGantryMove.Value = InitCheckState.NG; // 10.12.25 minhan
                        m_InitState = InitState.Fail;
                        m_AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Unit Wait Position Sensor : Error");
                        nSeqNo = 1500;
                    }
                    break;
                case 390:
                    {
                        nSeqNo = 500;
                    }
                    break;
                case 500:
                    {
                        bool GlassExist = false;
                        bool GlassValid = false;

                        foreach (CvUnit unit in m_Units)
                        {
                            if (unit.GlsInSensor.IsDetected(Logic.OR) || unit.GlsOutSensor.IsDetected(Logic.OR))
                            {
                                if (unit.GlsInSensor.IsDetected(Logic.OR))
                                {
                                    GlassExist = true;
                                }
                                else if (unit.GlsOutSensor.IsDetected(Logic.OR)) // 11.02.25 minhan 이건 솔직히 왜 이런 컨셉인지 모르겠다.
                                {
                                    GlassValid = true;
                                }

                            }
                        }

                        GlassValid |= m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        //GlassValid |= m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0));
                        GlassValid |= m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0));

                        GlassExist |= m_GantryUnit.IsGlassExist(Logic.OR);
                        //GlassExist |= m_LdHandUnit.IsGlassExist(Logic.OR);
                        GlassExist |= m_UlHandUnit.IsGlassExist(Logic.OR);

                        if (m_Simul.Motion)
                        {
                            if (m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)))
                            {
                                m_GantryUnit.GlassExistSensor.SetState(true, Logic.AND);
                            }
                            else
                            {
                                m_GantryUnit.GlassExistSensor.SetState(false, Logic.AND);
                            }

                            //if (m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0)))
                            //{
                            //    m_LdHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                            //}
                            //else
                            //{
                            //    m_LdHandUnit.GlassExistSensor.SetState(false, Logic.AND);
                            //}

                            if (m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0)))
                            {
                                m_UlHandUnit.GlassExistSensor.SetState(true, Logic.AND);
                            }
                            else
                            {
                                m_UlHandUnit.GlassExistSensor.SetState(false, Logic.AND);
                            }

                        }
                        if (GlassExist || GlassValid)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Clean-Out Mode : Check");
                            nSeqNo = 510;
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Clean-Out Mode : No Use");
                            nSeqNo = 530;
                        }
                    }
                    break;
                case 510:
                    {
                        bool IsTrValid = false;
                        bool TrGlsExist = false;
                        IsTrValid = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        TrGlsExist = m_GantryUnit.IsGlassExist(Logic.OR);

                        if (((IsTrValid && !TrGlsExist) || (!IsTrValid && TrGlsExist)) && !m_Simul.Motion)
                        {
                            m_InitState = InitState.Fail; // 10.12.25 minhan
                            m_AlarmId = m_GantryUnit.ALM_DataSensorUnmatch.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Unit Data Sensing Error");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            //int curLdUpPosition = GetRbtPos(m_LdHandUnit.Servo);
                            //if (curLdUpPosition > -1 || m_Simul.Motion)
                            //{
                            //    int rv = m_LdHandUnit.Servo.SetCurPosition((short)curLdUpPosition);
                            //    if (rv == 0 || m_Simul.Motion)
                            //    {
                            nSeqNo = 511;
                            //}
                            //else if (0 < rv)
                            //{
                            //    m_InitState = InitState.Fail; // 10.12.25 minhan
                            //    AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                            //    m_EqpManager.SetAlarm(AlarmId);
                            //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Set Position : Error - " + rv.ToString());
                            //    nSeqNo = 1000;
                            //    break;
                            //}
                            //}
                            //else
                            //{
                            //    m_InitState = InitState.Fail; // 10.12.25 minhan
                            //    AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                            //    m_EqpManager.SetAlarm(AlarmId);
                            //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Position : Error");
                            //    nSeqNo = 1000;
                            //break;
                            //}
                        }
                    }
                    break;
                case 511:
                    {
                        int curUlUpPosition = GetRbtPos(m_UlHandUnit.Servo);
                        if (curUlUpPosition > -1 || m_Simul.Motion)
                        {
                            int rv = m_UlHandUnit.Servo.SetCurPosition((short)curUlUpPosition);
                            if (rv == 0 || m_Simul.Motion)
                            {
                                nSeqNo = 512;
                            }
                            else if (0 < rv)
                            {
                                m_InitState = InitState.Fail; // 10.12.25 minhan
                                m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Set Position : Error - " + rv.ToString());
                                nSeqNo = 1000;
                                break;
                            }
                        }
                        else
                        {
                            m_InitState = InitState.Fail; // 10.12.25 minhan
                            m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Position : Error");
                            nSeqNo = 1000;
                            break;
                        }
                    }
                    break;
                case 512:
                    {
                        //20090628 eun SetHomeComp()로 대체
                        //m_LdHandUnit.Servo.HomeComp = true;
                        //m_UlHandUnit.Servo.HomeComp = true;
                        //int rv = m_LdHandUnit.Servo.SetHomeComp();
                        //if (rv == 0 || m_Simul.Motion)
                        //{
                        nSeqNo = 513;
                        //}
                        //else if (0 < rv)
                        //{
                        //    m_InitState = InitState.Fail; // 10.12.25 minhan
                        //    AlarmId = m_LdHandUnit.ALM_PosNotDetect.Id;
                        //    m_EqpManager.SetAlarm(AlarmId);
                        //    m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Set Home Comp : Error - " + rv.ToString());
                        //    nSeqNo = 1000;
                        //    break;
                        //}
                    }
                    break;
                case 513:
                    {
                        int rv = m_UlHandUnit.Servo.SetHomeComp();
                        if (rv == 0 || m_Simul.Motion)
                        {
                            nSeqNo = 515;
                        }
                        else if (0 < rv)
                        {
                            m_InitState = InitState.Fail; // 10.12.25 minhan
                            m_AlarmId = m_UlHandUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Set Home Comp : Error - " + rv.ToString());
                            nSeqNo = 1000;
                            break;
                        }
                    }
                    break;
                case 515:
                    {
                        m_GenInfo.CleanOut = true;
                        m_GenInfo.CycleStop = true;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Clean-Out Mdoe Set : OK");

                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 530;
                    }
                    break;
                //case 520:
                //    if (m_LdHandUnit.Servo.RbtReset())
                //    {
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Servo Reset : OK");
                //        m_StartTicks = XFunc.GetTickCount();
                //        nSeqNo = 530;
                //    }
                //    else if (GetElapsedTicks() > 2000)
                //    {
                //        m_InitState = InitState.Fail; // 10.12.25 minhan
                //        AlarmId = m_LdHandUnit.ALM_ServoReset.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Servo Reset : Error"); // minhan
                //        nSeqNo = 1000;
                //    }
                //    break;
                case 530:
                    if (m_UlHandUnit.Servo.RbtReset())
                    {
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Servo Reset : OK");
                        nSeqNo = 550;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_InitState = InitState.Fail; // 10.12.25 minhan
                        m_AlarmId = m_UlHandUnit.ALM_ServoReset.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Servo Reset : Error");
                        nSeqNo = 1000;
                    }
                    break;
                //case 540: // 11.02.25 minhan
                //    {
                //        bool checkhand = m_LdHandUnit.IsHandDown();

                //        bool checkCvGls = eqpTransferUnits._LD_CvUnit.GlsInSensor.IsDetected(Logic.OR);
                //        checkCvGls |= eqpTransferUnits._LD_CvUnit.GlsOutSensor.IsDetected(Logic.OR);
                //        checkCvGls |= m_Server.GlassData.IsExist(eqpTransferUnits._LD_CvUnit.DataMatchingKey(0));
                //        checkCvGls |= m_Server.GlassData.IsExist(eqpTransferUnits._LD_CvUnit.DataMatchingKey(1));

                //        int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 11.02.25 minhan

                //        if (curTrPosition < 0) // 이동중에 tr 포지션을 모른다 무조건 stop
                //        {
                //            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                //            m_LdHandUnit.Servo.RbtEStop();
                //            m_UlHandUnit.Servo.RbtEStop();
                //            m_InitState = InitState.Fail;
                //            AlarmId = m_LdHandUnit.ALM_HomePosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "Tr Position : Error");
                //            m_StartTicks = XFunc.GetTickCount();
                //            nSeqNo = 2000;
                //        }
                //        else if (!m_LdHandUnit.Servo.HomeComp && !checkhand && checkCvGls) // Clean Out이 아닌데, home 잡으려고 하는데, hand도 다운이 아니고.글래스가 ld에 있으면.
                //        {
                //            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                //            m_LdHandUnit.Servo.RbtEStop();
                //            m_UlHandUnit.Servo.RbtEStop();
                //            m_InitState = InitState.Fail;
                //            AlarmId = m_LdHandUnit.ALM_HomePosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD CV Gls Check : Error");
                //            m_StartTicks = XFunc.GetTickCount();
                //            nSeqNo = 2000;
                //        }
                //        else if (!m_LdHandUnit.Servo.HomeComp && (0 == (Rv = m_LdHandUnit.Servo.RbtMoveHome())))
                //        {
                //            m_LdHandUnit.Servo.HomeComp = true;
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Home Pos Move : OK");
                //        }
                //        else if (Rv > 0)
                //        {
                //            m_InitState = InitState.Fail; // 10.12.25 minhan
                //            AlarmId = m_LdHandUnit.ALM_HomePosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Hand Home Pos Move : Error");

                //            string sRv;
                //            sRv = string.Format("Rv = {0}", Rv);
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, sRv);

                //            nSeqNo = 1000;
                //            break;
                //        }

                //        if (m_LdHandUnit.Servo.HomeComp)
                //        {
                //            m_Control.SetLog(SeqFunName, nSeqNo, 0, 0, "LD Home Complete");
                //            nSeqNo = 550;
                //        }

                //    }
                //    break;
                case 550: // 11.02.25 minhan
                    {
                        bool checkhand = m_UlHandUnit.IsHandDown();

                        bool checkCvGls = eqpTransferUnits._UL_CvUnit.GlsInSensor.IsDetected(Logic.OR);
                        checkCvGls |= eqpTransferUnits._UL_CvUnit.GlsOutSensor.IsDetected(Logic.OR);
                        checkCvGls |= m_Server.GlassData.IsExist(eqpTransferUnits._UL_CvUnit.DataMatchingKey(0));
                        checkCvGls |= m_Server.GlassData.IsExist(eqpTransferUnits._UL_CvUnit.DataMatchingKey(1));

                        int curTrPosition = GetRbtPos(m_GantryUnit.Servo); // 11.02.25 minhan

                        if (curTrPosition < 0) // 이동중에 tr 포지션을 모른다 무조건 stop
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_HomePosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Tr Position : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if (!m_UlHandUnit.Servo.HomeComp && !checkhand && checkCvGls) // Clean Out이 아닌데, home 잡으려고 하는데, hand도 다운이 아니고.글래스가 ul에 있으면.
                        {
                            m_GantryUnit.Servo.RbtEStop(); // 11.02.25 minhan
                                                           // m_LdHandUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_InitState = InitState.Fail;
                            m_AlarmId = m_UlHandUnit.ALM_HomePosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL CV Gls Check : Error");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 2000;
                        }
                        else if (!m_UlHandUnit.Servo.HomeComp && (0 == (Rv = m_UlHandUnit.Servo.RbtMoveHome())))
                        {
                            m_UlHandUnit.Servo.HomeComp = true;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Home Pos Move : OK");
                        }
                        else if (Rv > 0)
                        {
                            m_InitState = InitState.Fail; // 10.12.25 minhan
                            m_AlarmId = m_UlHandUnit.ALM_HomePosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Hand Home Pos Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, sRv);

                            nSeqNo = 1000;
                            break;
                        }

                        if (m_UlHandUnit.Servo.HomeComp)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "UL Home Complete");
                            nSeqNo = 600;
                        }

                    }
                    break;
                case 600:
                    {
                        //InitParameter();이거 있어야하는데 kimgun
                        //m_Server.GenInfos.EqpInitReq = false;
                        //m_Server.GenInfos.EqpInitComp = true;
                        m_Control.InitParameter(); // 09.05.30 minhan
                        m_InitSetCleanOut.Value = InitCheckState.Checking;
                        // (m_Server as ServerManager).ThreadHandler.LdHandControl.InitParameter(); // 09.05.30 minhan
                        (m_Server as ServerManager).ThreadHandler.UlHandControl.InitParameter(); // 09.05.30 minhan
                        if (m_GenInfo.CleanOut)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Init Complete : Clean-Out Set");
                            m_InitSetCleanOut.Value = InitCheckState.OK;
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Init Complete : OK");
                            m_InitSetCleanOut.Value = InitCheckState.NoUse;
                        }
                        m_InitState = InitState.Comp;

                        if (m_Simul.Motion)
                        {
                            m_GenInfo.Pause = true;
                        }
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_GenInfo.EqpInitReq = false;
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Error Recovery(1000)");

                        nSeqNo = 0;
                    }
                    break;
                case 1500:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_GenInfo.EqpInitReq = false;
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        //m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Error Recovery(1500)");

                        nSeqNo = 0;
                    }
                    break;
                case 2000: // 11.02.25 minhan
                    {
                        bool EstopCheckTR = true;
                        //bool EstopCheckLD = true;
                        bool EstopCheckUL = true;

                        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();
                        // EstopCheckLD = m_LdHandUnit.Servo.RbtEStop();
                        EstopCheckUL = m_UlHandUnit.Servo.RbtEStop();

                        if (EstopCheckTR && EstopCheckUL)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Estop OK");
                            //m_EqpManager.ResetAlarm(AlarmId);
                            //AlarmId = 0;
                            //m_GenInfo.AutoMode = false;
                            nSeqNo = 2100;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_GenInfo.EqpInitReq = false;
                            m_GenInfo.AutoMode = false;
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 2100: // 11.02.25 minhan
                    {
                        if (m_EqpManager.AlarmResetSwitchPushed)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_GenInfo.EqpInitReq = false;
                            m_GenInfo.AutoMode = false;
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Error Recovery(2100)");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }

            this.m_SeqNo = nSeqNo;

            return (int)m_InitState;
        }
        #endregion
    }

    public class SeqUnitTr : XSeqFunction
    {
        #region Fields
        private GantryUnit m_GantryUnit = null;
        ///private FishHand m_LdHandUnit = null;
        private FishHand m_UlHandUnit = null;

        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        private GenInfoHandler m_GenInfo;

        private ThreadTr_BOE_G8_DHDC m_Control;

        private Simul m_Simul;

        private EGiSInterface m_EgisInterface; // 11.03.02 minhan
        private int m_AlignSeqNo = 0;
        private double servoCurPos = 0.0;

        private short m_GantryHomePosition;
        private short m_GantrySendPosition;
        private short m_GantryWaitPosition;
        private short m_GantryRecvPosition;

        //private short m_LdHandHomePosition;
        //private short m_LdHandRecv1Position;
        //private short m_LdHandRecv2Position;
        //private short m_LdHandRecv3Position;
        //private short m_LdHandWaitPosition;
        //private short m_LdHandSend1Position;
        //private short m_LdHandSend2Position;

        private short m_UlHandHomePosition;
        private short m_UlHandRecv1Position;
        private short m_UlHandRecv2Position;
        private short m_UlHandWaitPosition;
        private short m_UlHandSend1Position;
        private short m_UlHandSend2Position;
        private short m_UlHandSend3Position;

        private int gtFwTimeoutAlarm = 0; // 09.09.30 minhan
        private int gtBwTimeoutAlarm = 0; // 09.09.30 minhan
        private Alarm m_WarningAlignFw;//2010.09.09 kimgun
        private Alarm m_WarningAlignBw;//2010.09.09 kimgun
        private Alarm m_RecvDataError; // 10.12.25 minhan
        private Alarm m_ManualChangeErr; // 11.02.25 minhan
        private Alarm m_ServoNotReady; // 11.02.25 minhan
        private Alarm m_EgisErr; // 11.03.02 minhan
                                 // private ThreadLd_Hand_BOE_G8_DHDC m_LdHandControl;
        private ThreadUl_Hand_BOE_G8_DHDC m_UlHandControl;
        private TagGlassData m_RecvGlassData = new TagGlassData();//2010.08.30 kimgun
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int AlignRetryCnt = 0;//2010.09.09 kimgun
        private ApdItems items; //10.12.29 minhan
        private ApdItems itemLotapd;
        private int m_AlignAlarmIdfw; // 11.02.01 minhan
        private int m_AlignAlarmIdbw; // 11.02.01 minhan
        private int m_AlarmIdServo; // 11.02.25 minhan
        private int m_EgisTimeout; // 11.03.02 minhan
        #endregion

        #region Constructor
        public SeqUnitTr(ThreadTr_BOE_G8_DHDC control, GantryUnit tr)
        {
            m_GantryUnit = tr;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface; // 11.03.02 minhan
            m_SeqFunName = m_GantryUnit.Name;
            m_GenInfo = GenInfoHandler.Instance;

            //m_LdHandUnit = eqpTransferUnits._LD_Fish_Hand;
            m_UlHandUnit = eqpTransferUnits._UL_Fish_Hand;

            m_WarningAlignFw = new Alarm(m_GantryUnit.Name + " : Align FW Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun
            m_WarningAlignBw = new Alarm(m_GantryUnit.Name + " : Align BW Warning", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);//2010.09.09 kimgun
            m_RecvDataError = new Alarm(m_GantryUnit.Name + " : Recv Data Error", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_ManualChangeErr = new Alarm(m_GantryUnit.Name + " : Manual Change Error", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_ServoNotReady = new Alarm(m_GantryUnit.Name + " : Servo Not Ready", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
            m_EgisErr = new Alarm(m_GantryUnit.Name + " : Crack Unit Error", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan


            m_GantryHomePosition = eqpPos_TR_Servo_Unit.Home;
            m_GantrySendPosition = eqpPos_TR_Servo_Unit.Send;
            m_GantryWaitPosition = eqpPos_TR_Servo_Unit.Wait;
            m_GantryRecvPosition = eqpPos_TR_Servo_Unit.Recv;

            //m_LdHandHomePosition = eqpPos_LD_Hand_Servo_Unit.Home_Position;
            //m_LdHandRecv1Position = eqpPos_LD_Hand_Servo_Unit.Recv1_Position;
            //m_LdHandRecv2Position = eqpPos_LD_Hand_Servo_Unit.Recv2_Position;
            //m_LdHandRecv3Position = eqpPos_LD_Hand_Servo_Unit.Recv3_Position;
            //m_LdHandWaitPosition = eqpPos_LD_Hand_Servo_Unit.Wait_Position;
            //m_LdHandSend1Position = eqpPos_LD_Hand_Servo_Unit.Send1_Position;
            //m_LdHandSend2Position = eqpPos_LD_Hand_Servo_Unit.Send2_Position;

            m_UlHandHomePosition = eqpPos_UL_Hand_Servo_Unit.Home;
            m_UlHandRecv1Position = eqpPos_UL_Hand_Servo_Unit.Home;
            m_UlHandRecv2Position = eqpPos_UL_Hand_Servo_Unit.Recv2;
            m_UlHandWaitPosition = eqpPos_UL_Hand_Servo_Unit.Wait;
            m_UlHandSend1Position = eqpPos_UL_Hand_Servo_Unit.Send1;
            m_UlHandSend2Position = eqpPos_UL_Hand_Servo_Unit.Send2;
            m_UlHandSend3Position = eqpPos_UL_Hand_Servo_Unit.Send3;

            m_AlignAlarmIdfw = 0; // 11.02.01 minhan
            m_AlignAlarmIdbw = 0; // 11.02.01 minhan
            m_AlarmIdServo = 0; // 11.02.25 minhan
            m_EgisTimeout = 0; // 11.03.02 minhan
        }
        #endregion

        #region Method
        public int SeqTrAlign(int type)
        {

            int alignSeqNo = this.m_AlignSeqNo;
            int ReturnVal = -1;

            switch (alignSeqNo)
            {
                case 0:
                    if (type == (int)AlignCommand.TrAlign._alignON)
                    {
                        m_GantryUnit.AlignSet(ActuatorAct.Pos);
                        m_Control.SetLog(m_SeqFunName, alignSeqNo, 0, 0, "Align FW Start");

                        gtFwTimeoutAlarm = m_GantryUnit.SetupAlignFwTimeout.GetValue<int>();
                        m_StartTicks = XFunc.GetTickCount();
                        AlignRetryCnt++;//2010.09.09 kimgun
                        alignSeqNo = 10;
                    }
                    else
                    {
                        m_GantryUnit.AlignSet(ActuatorAct.Neg);
                        m_Control.SetLog(m_SeqFunName, alignSeqNo, 0, 0, "Align BW Start");
                        gtBwTimeoutAlarm = m_GantryUnit.SetupAlignBwTimeout.GetValue<int>();
                        m_StartTicks = XFunc.GetTickCount();
                        AlignRetryCnt++;//2010.09.09 kimgun
                        alignSeqNo = 100;
                    }
                    break;
                case 10:
                    if (m_GantryUnit.IsAlignFw())
                    {
                        if (GetElapsedTicks() > (gtFwTimeoutAlarm * 0.9 * 1000))
                        {//2010.09.09 kimgun
                            m_Control.SetLog(m_SeqFunName, alignSeqNo, 0, 0, "Align FW OK (Warning Case)");
                            //m_AlignAlarmIdfw = m_WarningAlignFw.Id; // zhangliang
                            //m_EqpManager.SetAlarm(m_AlignAlarmIdfw);
                        }
                        //else
                        //{
                        //    m_EqpManager.ResetAlarm(m_WarningAlignFw.Id);
                        //    m_AlignAlarmIdfw = 0;
                        //}
                        AlignRetryCnt = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        alignSeqNo = 200;
                    }
                    else if (GetElapsedTicks() > gtFwTimeoutAlarm * 1000)
                    {
                        if (AlignRetryCnt > 1)//2010.09.09 kimgun
                        {
                            AlignRetryCnt = 0;
                            ReturnVal = 1;
                            alignSeqNo = 0;
                        }
                        else alignSeqNo = 0;
                    }
                    break;
                case 100:
                    if (m_GantryUnit.IsAlignBw())
                    {
                        if (GetElapsedTicks() > (gtBwTimeoutAlarm * 0.9 * 1000))
                        {//2010.09.09 kimgun
                            m_Control.SetLog(m_SeqFunName, alignSeqNo, 0, 0, "Align BW OK (Warning Case)");
                            //m_AlignAlarmIdbw = m_WarningAlignBw.Id; // 11.02.01 minhan
                            //m_EqpManager.SetAlarm(m_AlignAlarmIdbw);
                        }
                        //else
                        //{
                        //    m_EqpManager.ResetAlarm(m_WarningAlignBw.Id);
                        //    m_AlignAlarmIdbw = 0;
                        //}
                        AlignRetryCnt = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        alignSeqNo = 200;
                    }
                    else if (GetElapsedTicks() > gtBwTimeoutAlarm * 1000)
                    {
                        if (AlignRetryCnt > 1)//2010.09.09 kimgun
                        {
                            AlignRetryCnt = 0;
                            ReturnVal = 1;
                            alignSeqNo = 0;
                        }
                        else alignSeqNo = 0; //zhangliang : Add
                    }
                    break;
                case 200:
                    if (GetElapsedTicks() > 500) // 11.04.22 minhan
                    {
                        ReturnVal = 0;
                        alignSeqNo = 0;
                    }
                    break;
            }
            this.m_AlignSeqNo = alignSeqNo;
            return ReturnVal;
        }
        #endregion

        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;
            if (m_GantryUnit.Sequence[0] == null) m_GantryUnit.Sequence[0] = this; // 09.05.30 minhan
            if (!m_Control.IsSeqRunCondition(m_GantryUnit)) return -1;

            //if (m_LdHandControl == null) m_LdHandControl = m_Server.ThreadHandler.LdHandControl;
            if (m_UlHandControl == null) m_UlHandControl = m_Server.ThreadHandler.UlHandControl;

            //2010.08.30 kimgun
            if (m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)) &&
                (m_PortNo == 0) && (m_SlotNo == 0))
            {
                m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref m_RecvGlassData);
                m_PortNo = (int)m_RecvGlassData.Item.GlassNumberCode.LotNo;
                m_SlotNo = (int)m_RecvGlassData.Item.GlassNumberCode.SlotNo;
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            if ((m_AlignAlarmIdfw != 0) && m_EqpManager.AlarmResetSwitchPushed) // 11.02.01 minhan 
            {
                m_EqpManager.ResetAlarm(m_AlignAlarmIdfw);
                m_AlignAlarmIdfw = 0;
            }
            if ((m_AlignAlarmIdbw != 0) && m_EqpManager.AlarmResetSwitchPushed)
            {
                m_EqpManager.ResetAlarm(m_AlignAlarmIdbw);
                m_AlignAlarmIdbw = 0;
            }

            int seqNo = this.m_SeqNo;
            int Rv = -1;
            switch (seqNo)
            {
                case 0:
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }

                        if (m_AlarmIdServo > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmIdServo);
                            m_AlarmIdServo = 0;
                        }

                        short curGtPosition = (short)m_GantryUnit.Servo.GetCurPointId();
                        //short curLdUpPosition = (short)m_LdHandUnit.Servo.GetCurPointId();
                        short curUlUpPosition = (short)m_UlHandUnit.Servo.GetCurPointId();

                        bool m_gtHomePositionSensor = eqpServoMotors._TR_Master_Servo_Motor.GetHomeSwitch();
                        int glassCount = m_Server.GlassData.Count;
                        bool isRobotInterlock = m_GantryUnit.IsRobotInterlock();
                        bool gtAlignFw = m_GantryUnit.IsAlignFw();
                        bool gtAlignBw = m_GantryUnit.IsAlignBw();
                        bool gtGlassExist = true;
                        gtGlassExist &= m_GantryUnit.IsGlassExist(Logic.OR);
                        gtGlassExist &= m_Control.IsPositionConfirmed(m_GantrySendPosition);

                        if (m_Simul.Device)
                        {
                            gtGlassExist = m_Server.GlassData.IsExist(m_GantryUnit.Id * 2 - 1);//zhangliang 130521
                        }

                        bool gtGlassDataExist = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        bool gtPosSensor = m_Control.IsPositionConfirmed(m_GantrySendPosition);

                        int posId = m_GantryUnit.DataMatchingKey(0);
                        TagGlassData data = new TagGlassData();
                        m_Server.GlassData.GetData(posId, ref data);
                        bool Processed = data.Processed;
                        //bool ldFishGlassExist = m_LdHandUnit.IsGlassExist(Logic.AND);
                        //bool ldFishDataExist = m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0));
                        bool ulFishGlassExist = m_UlHandUnit.IsGlassExist(Logic.AND);
                        bool ulFishDataExist = m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0));

                        bool glassBroken = m_GantryUnit.IsGlassBroken();

                        //bool ldfishRibUp = m_LdHandUnit.IsHandUp();
                        // bool ldfishRibDn = m_LdHandUnit.IsHandDown();
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                        /*-------------------------------------------------------------*/
                        //bool ldUpHandRecvCondition = false;
                        //ldUpHandRecvCondition = !ldFishGlassExist &
                        //                        !ldFishDataExist &
                        //                        (ldfishRibUp && !ldfishRibDn) &
                        //                        (curLdUpPosition == m_LdHandRecv1Position) &
                        //                        (m_LdHandControl.IsPositionConfirmed(m_LdHandRecv1Position)) &
                        //                        m_LdHandUnit.IfFlag.InReady; // 11.02.25 minhan
                        /*-------------------------------------------------------------*/
                        bool ulUpHandSendCondition = false;
                        ulUpHandSendCondition = ulFishGlassExist &
                                                ulFishDataExist &
                                                (ulFishRibUp & !ulFishRibDn) &
                                                (curUlUpPosition == m_UlHandSend3Position) &
                                                (m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position)) &
                                                m_UlHandUnit.IfFlag.OutReady; // 11.02.25 minhan

                        //bool ldInterferePos = false;
                        //ldInterferePos |= curLdUpPosition == m_LdHandRecv2Position;
                        //ldInterferePos |= m_LdHandControl.IsPositionConfirmed(m_LdHandRecv2Position);
                        //ldInterferePos |= curLdUpPosition == -1;//2009.08.15 kimgun

                        bool ulInterferePos = false;
                        ulInterferePos |= curUlUpPosition == m_UlHandSend2Position;
                        ulInterferePos |= m_UlHandControl.IsPositionConfirmed(m_UlHandSend2Position);
                        ulInterferePos |= curUlUpPosition == -1;//2009.08.15 kimgun

                        bool singleMode = m_Server.SetupSingleMode.GetValue<bool>();
                        if (singleMode)
                        {
                            m_GenInfo.CleanOut = false;
                            Processed = false;
                        }

                        if ((!m_Control.IsPositionConfirmed(m_GantryRecvPosition) &&
                            !m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                            !m_Control.IsPositionConfirmed(m_GantryWaitPosition) &&
                            !m_Control.IsPositionConfirmed(m_GantryHomePosition)) ||
                            (curGtPosition == -1))
                        {//2009.09.14 kimgun
                            m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }

                        if (!m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_ServoNotReady.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Servo Not Ready Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }

                        //if (GlobalVar.rcvON_IF2) // 11.02.25 minhan  zhangliang
                        //{
                        //    if ((curGtPosition == m_GantrySendPosition) &&
                        //        m_Control.IsPositionConfirmed(m_GantrySendPosition)) // wait가 아닌데 이 플래그가 살았다고 한다면.알람처리
                        //    {
                        //        if (!GlobalVar.ExchnageReq)
                        //        {
                        //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Receive)");
                        //            seqNo = 100;
                        //            break;
                        //        }
                        //        else // 11.06.10 minhan
                        //        {
                        //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Receive) and Exchange Request ON");
                        //            seqNo = 120;
                        //            break;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                        //        m_EqpManager.SetAlarm(AlarmId);
                        //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm(rcvON_IF2 True)");
                        //        ReturnSeqNo = seqNo;
                        //        seqNo = 1000;
                        //        break;
                        //    }
                        //}
                        //else if (GlobalVar.rcvCOMP)//2010.06.18 kimx 검토 필요
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface comp(Receive)");
                        //    seqNo = 120;
                        //    break;
                        //}
                        else if (GlobalVar.sndON_IF2) // 11.02.25 minhan
                        {
                            if ((curGtPosition == m_GantrySendPosition) &&
                                m_Control.IsPositionConfirmed(m_GantrySendPosition)) // wait가 아닌데 이 플래그가 살았다고 한다면.알람처리
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Send)");
                                seqNo = 240;
                                break;
                            }
                            else
                            {
                                m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm(sndON_IF2 True)");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                                break;
                            }
                        }

                        //#region Recv. From Loader case  zhangliang
                        //if (!m_GenInfo.CleanOut &&
                        //    !m_GenInfo.CycleStop &&
                        //    !m_GantryUnit.IsGlassExist(Logic.OR) &&
                        //    !gtGlassDataExist &&
                        //    (curGtPosition == m_GantryWaitPosition) &&
                        //    m_Control.IsPositionConfirmed(m_GantryWaitPosition) &&
                        //    !isRobotInterlock &&
                        //    (m_GenInfo.EQPGlassCount < m_Server.SetupMaxGlassNo.GetValue<int>()) &&
                        //    GlobalVar.LoaderReady &&
                        //    !singleMode &&
                        //    !GlobalVar.NoSubstrate) // 11.02.08 minhan 로더가 interface 가능 상태이고,싱글모드가 아니라면 
                        //{
                        //    if (GlobalVar.rcvCANCEL)
                        //    {
                        //        GlobalVar.rcvCANCEL = false;
                        //    }

                        //    GlobalVar.TrMoveLoadDir = false;//2009.08.25 kimgun
                        //    GlobalVar.TrMoveUnloadDir = false;//2009.08.25 kimgun
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from Loader case"); // 10.12.25 minhan
                        //    seqNo = 100;
                        //}
                        //#endregion

                        #region Send To Loader case
                        else if (gtGlassExist &&
                                 gtGlassDataExist &&
                                 Processed &&
                                 (curGtPosition == m_GantrySendPosition) &&
                                 m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                                 !isRobotInterlock)
                        {
                            GlobalVar.TrMoveLoadDir = false;//2009.08.25 kimgun
                            GlobalVar.TrMoveUnloadDir = false;//2009.08.25 kimgun
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Send to Loader case");
                            seqNo = 200;
                        }
                        #endregion

                        #region Go to Wait Pos Before Recv from UL Fish Hand case //zhangliang 130517
                        else if (!m_GantryUnit.IsGlassExist(Logic.OR) &&
                                 !gtGlassDataExist &&
                                 !isRobotInterlock &&
                                 !ulUpHandSendCondition && (curGtPosition == m_GantrySendPosition) &&
                                 m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                                 ((m_GenInfo.EQPGlassCount <= m_Server.SetupMaxGlassNo.GetValue<int>()) ||
                                     m_GenInfo.CleanOut || m_GenInfo.CycleStop || singleMode
                                  || GlobalVar.NoSubstrate || !GlobalVar.LoaderReady)) // 11.04.18 minhan
                        {
                            GlobalVar.TrMoveLoadDir = false;
                            GlobalVar.TrMoveUnloadDir = true;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Before TR Recv from UL Hand case(306)");
                            seqNo = 306;
                        }
                        #endregion

                        #region Recv from UL Fish Hand case
                        else if (!m_GantryUnit.IsGlassExist(Logic.OR) &&
                                !gtGlassDataExist &&
                                !isRobotInterlock &&
                                ulUpHandSendCondition &&
                                ((m_GenInfo.EQPGlassCount <= m_Server.SetupMaxGlassNo.GetValue<int>()) ||
                                m_GenInfo.CleanOut ||
                                m_GenInfo.CycleStop || singleMode || GlobalVar.NoSubstrate || !GlobalVar.LoaderReady)) // 11.04.18 minhan
                        {
                            GlobalVar.TrMoveLoadDir = false;
                            GlobalVar.TrMoveUnloadDir = true;
                            //GlobalVar.UlHandGlsSendCondition = true; // 10.12.25 minhan

                            if ((curGtPosition == m_GantryRecvPosition) &&
                                m_Control.IsPositionConfirmed(m_GantryRecvPosition))
                            {//startprocess==>recv pos move
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from UL Hand case(320)");
                                seqNo = 320;
                            }
                            else
                            {//startprocess==>Align Backword action
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from UL Hand case(300)");
                                seqNo = 300;
                            }
                        }
                        #endregion

                        //#region Send to LD Hand Case
                        //else if (gtGlassExist &&
                        //        gtGlassDataExist &&
                        //        !isRobotInterlock &&
                        //        !Processed &&
                        //        ldUpHandRecvCondition &&
                        //        (curGtPosition == m_GantryWaitPosition) &&
                        //        m_Control.IsPositionConfirmed(m_GantryWaitPosition) &&
                        //        !GlobalVar.LdHandAirInterlock)
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Send to LD Hand case");
                        //    GlobalVar.TrMoveLoadDir = true;
                        //    GlobalVar.TrMoveUnloadDir = false;
                        //    seqNo = 400;
                        //}
                        //#endregion

                        #region TR Recv Pos and Ul Hand SEND2 Pos case
                        else if ((curGtPosition == m_GantryRecvPosition) &&
                                 (m_Control.IsPositionConfirmed(m_GantryRecvPosition)/* || m_Simul.Device*/) &&
                                 !gtAlignFw && gtAlignBw &&
                                 !gtGlassDataExist &&
                                 ulInterferePos &&
                                 ulFishDataExist &&
                                 !GlobalVar.UlHandAirInterlock)
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send2 Pos case");
                            m_GantryUnit.IfFlag.InReady = true;//2009.08.26 kimgun
                            seqNo = 330;
                        }
                        #endregion

                        #region TR Recv Pos and Ul Hand SEND1 Pos case
                        else if ((curGtPosition == m_GantryRecvPosition) &&
                                (m_Control.IsPositionConfirmed(m_GantryRecvPosition)/* || m_Simul.Device*/) &&
                                !gtAlignFw && gtAlignBw &&
                                //gtGlassExist && // 10.12.25 minhan 이건 아닌데.
                                gtGlassDataExist &&
                                (curUlUpPosition == m_UlHandSend1Position) &&
                                (m_UlHandControl.IsPositionConfirmed(m_UlHandSend1Position)/* || m_Simul.Device*/) &&
                                //ulInterferePos &&
                                //ulFishDataExist &&
                                !GlobalVar.UlHandAirInterlock)
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send1 Pos case");
                            m_GantryUnit.IfFlag.InReady = true;//2009.08.26 kimgun
                            seqNo = 335; // 11.02.25 minhan
                        }
                        #endregion

                        //#region TR Send Pos and Ld Hand Recv1 Pos case
                        //else if ((curGtPosition == m_GantrySendPosition) &&
                        //        m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                        //        !gtAlignFw && gtAlignBw &&
                        //        gtGlassDataExist &&
                        //        //  ldInterferePos &&
                        //        !ldFishDataExist &&
                        //        !GlobalVar.LdHandAirInterlock &&
                        //        ldUpHandRecvCondition)
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV1 Pos case");
                        //    m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                        //    seqNo = 450;
                        //}
                        //#endregion

                        //#region TR Send Pos and Ld Hand Recv2 Pos case
                        //else if ((curGtPosition == m_GantrySendPosition) &&
                        //        m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                        //        !gtAlignFw && gtAlignBw &&
                        //        gtGlassDataExist &&
                        //        //ldInterferePos &&
                        //        !ldFishDataExist &&
                        //        !GlobalVar.LdHandAirInterlock &&
                        //        (curLdUpPosition == m_LdHandRecv2Position) &&
                        //        (m_LdHandControl.IsPositionConfirmed(m_LdHandRecv2Position)/* || m_Simul.Device*/))
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV2 Pos case");
                        //    m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                        //    seqNo = 450;
                        //}
                        //#endregion
                        // TR Send Pos and Ld Hand Recv3 Pos case 
                        //else if ((curGtPosition == m_GantrySendPosition) &&
                        //        m_Control.IsPositionConfirmed(m_GantrySendPosition) &&
                        //        !gtAlignFw && gtAlignBw &&
                        //        // !gtGlassDataExist &&
                        //        //  ldInterferePos &&
                        //        //ldFishDataExist && data가 인계전일 수도 있고 인계 후 일수도 있응께.
                        //        !GlobalVar.LdHandAirInterlock &&
                        //        (curLdUpPosition == m_LdHandRecv3Position) &&
                        //        (m_LdHandControl.IsPositionConfirmed(m_LdHandRecv3Position)/* || m_Simul.Device*/)) // 10.12.29 minhan
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV3 Pos case");
                        //    //m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                        //    seqNo = 450;
                        //}
                        //#region description TR wait로 귀환
                        /*2009.08.15 kimgun
                                            tr이 wait로 이동해야 할 조건.
                                            1.tr에 glass data가 없어야 한다.
                                            2.ld,ul hand는 간섭 위치거나 이동중이 아니어야 한다.
                                            3.tr이 send pos일 경우 ld hand는 recv condition이 아니어야 한다.
                                            4.tr이 recv pos일 경우 ul hand는 send condition이 아니어야 한다.
                                            5.tr은 wait pos가 아니어야 한다.
                                             */
                        //#endregion

                        //#region tr이 send position에 있고 ld hand가 Recv1,2,3 pos가 아니어야 하며 send1,2,home,wait중 한 곳에는 위치해야 한다.
                        //else if ((curLdUpPosition != m_LdHandRecv1Position) &&
                        //         (curLdUpPosition != m_LdHandRecv2Position) &&
                        //         (curLdUpPosition != m_LdHandRecv3Position) &&
                        //         (((curLdUpPosition == m_LdHandHomePosition) && (m_LdHandControl.IsPositionConfirmed(m_LdHandHomePosition))) ||
                        //         ((curLdUpPosition == m_LdHandSend1Position) && (m_LdHandControl.IsPositionConfirmed(m_LdHandSend1Position))) ||
                        //         ((curLdUpPosition == m_LdHandSend2Position) && (m_LdHandControl.IsPositionConfirmed(m_LdHandSend2Position))) ||
                        //         ((curLdUpPosition == m_LdHandWaitPosition) && (m_LdHandControl.IsPositionConfirmed(m_LdHandWaitPosition)))) &&
                        //         ((curGtPosition == m_GantrySendPosition) && (m_Control.IsPositionConfirmed(m_GantrySendPosition)))) // 11.02.25 minhan
                        //{
                        //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "From Send ==> TR to Wait case(500)");
                        //    seqNo = 500;
                        //}
                        //#endregion

                        #region tr이 recv position에 있고 ul hand가 send1,2,3위치가 아니어야 하며 recv1,2,home,wait중 한 곳에는 위치해야 한다.
                        else if ((curUlUpPosition != m_UlHandSend1Position) &&
                                (curUlUpPosition != m_UlHandSend2Position) &&
                                (curUlUpPosition != m_UlHandSend3Position) &&
                                (((curUlUpPosition == m_UlHandHomePosition) && (m_UlHandControl.IsPositionConfirmed(m_UlHandHomePosition))) ||
                                ((curUlUpPosition == m_UlHandRecv1Position) && (m_UlHandControl.IsPositionConfirmed(m_UlHandRecv1Position))) ||
                                ((curUlUpPosition == m_UlHandRecv2Position) && (m_UlHandControl.IsPositionConfirmed(m_UlHandRecv2Position))) ||
                                ((curUlUpPosition == m_UlHandWaitPosition) && (m_UlHandControl.IsPositionConfirmed(m_UlHandWaitPosition)))) &&
                                ((curGtPosition == m_GantryRecvPosition) && (m_Control.IsPositionConfirmed(m_GantryRecvPosition)))) // 11.02.25 minhan
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "From Recv ==> TR to Send case(500)");
                            seqNo = 500;
                        }
                        #endregion

                        #region tr이 Home에 가 있으면, Wait로 이동 글래스 유무에 상관없이 이동한다.
                        else if ((curGtPosition == m_GantryHomePosition) &&
                                m_Control.IsPositionConfirmed(m_GantryHomePosition) && !isRobotInterlock) // 11.02.25 minhan
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "From Home ==> TR to Wait case(500)");
                            seqNo = 500;
                        }
                        #endregion

                        #region 2009.09.11 kimgun 혹시나 glass data 삭제또는 이동에 의해서 flag가 죽지 않았을 경우를 대비해서
                        else if ((curGtPosition == m_GantryWaitPosition) &&
                                (!m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)) || Processed) &&
                                (!GlobalVar.TrMoveLoadDir ||
                                 !GlobalVar.TrMoveUnloadDir))
                        {
                            GlobalVar.TrMoveLoadDir = false;//2009.08.25 kimgun
                            GlobalVar.TrMoveUnloadDir = false;//2009.08.25 kimgun
                            //m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Move Dir Reset)"); // 10.12.29 minhan
                        }
                        #endregion
                    }
                    break;

                //#region TR Glass Recv from Loader
                //case 100:
                //    if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                //    {
                //        //***************** Position Log **************************
                //        RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //        m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //        servoCurPos = curPos.Pos[0];

                //        string sCurPos;
                //        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //        //**********************************************************
                //        seqNo = 102;
                //    }
                //    else
                //    {
                //        AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1000;
                //    }
                //    break;
                //case 102: // 11.02.25 minhan
                //    {
                //        bool checkAlign = m_GantryUnit.IsAlignBw();
                //             checkAlign &= !m_GantryUnit.IsAlignFw();

                //        if (!checkAlign) // 11.02.5 minhan 메뉴얼 전환하여 처리하도록 함. 강제로 멈춘다.
                //        {
                //            m_GantryUnit.Servo.RbtEStop();
                //            AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //            seqNo = 2000;
                //        }
                //        else if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryWaitPosition))))
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : OK");

                //            if (m_Simul.Motion)
                //            {
                //                m_Control.SetPositionConfirmed(m_GantryWaitPosition);
                //            }

                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************

                //            m_StartTicks = XFunc.GetTickCount(); // 10.12.29 minhan
                //            seqNo = 105;
                //        }
                //        else if ((Rv > 0) || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                //        {
                //            AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Error");

                //            string sRv;
                //            sRv = string.Format("Rv = {0}", Rv);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************

                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //    }
                //    break;
                //case 105:
                //    if (m_Control.IsPositionConfirmed(m_GantryWaitPosition))
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : OK");
                //        seqNo = 110;
                //    }
                //    else if (GetElapsedTicks() > 2000)
                //    {
                //        AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1500;
                //    }
                //    break;
                //case 110:
                //    {
                //        if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Glass Recv Ready");
                //            //GlobalVar.TrGlsRecvReady = true; // 10.12.25 minhan 사용하지 않음.
                //            GlobalVar.rcvREQ = true;
                //            seqNo = 120;
                //        }
                //        else if (Rv > 0)
                //        {
                //            AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //    }
                //    break;
                //case 120:
                //    {
                //        short curGtPosition = (short)m_GantryUnit.Servo.GetCurPointId();

                //        if (GlobalVar.rcvCOMP)
                //        {
                //            GlobalVar.rcvCOMP = false;
                //            //GlobalVar.rcvON_IF2 = false; // 10.12.25 minhan 여기서 하지 않아도 된다.
                //            //GlobalVar.IsRecvGlassData = false; // 10.12.25 minhan 사용하지 않음.
                //            m_GantryUnit.IfFlag.InReady = false;
                //            // dspcrassus - 111223 : Glass Recv Complete 일 뿐, 실제 LD Hand로 이동한 것은 아님...
                //            //GlobalVar.TrMoveLoadDir = true;//2010.09.07 kimgun 출발 조건이 glass 받으면 바로..
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Glass In Complete");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 125;
                //        }
                //        else if (GlobalVar.rcvCANCEL)
                //        {
                //            GlobalVar.rcvCANCEL = false;
                //            GlobalVar.rcvREQ = false; // 10.12.25 minhan
                //            //  GlobalVar.rcvON_IF2 = false;//2009.09.09 kimgun
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass Load Cancel");
                //            seqNo = 0;
                //        }
                //        else if (!m_GantryUnit.IsAlignBw()) // 10.12.25 Align Bw 상태가 아니라면 알람처리.
                //        {
                //            AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //        else if ((curGtPosition != m_GantryWaitPosition) ||
                //                 !m_Control.IsPositionConfirmed(m_GantryWaitPosition)) // 10.12.25 minhan wait가 아니라면.
                //        {
                //            AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //    }
                //    break;
                //case 125: // 10.12.25 minhan 일단 생성하고 여기서 래시피 정도만 보고 오류난게 있으면 알람처리 해 버리는게 좋지 않을까.
                //    if (GetElapsedTicks() > 100) // 11.04.22 minhan
                //    {
                //        TagGlassData data = new TagGlassData();

                //        if (!m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data) ||
                //            !m_Server.DataProvider.RecipeProvider.Adapter.IsExist(data.RecipeID))
                //        {
                //            AlarmId = m_RecvDataError.Id;
                //            m_EqpManager.SetAlarm(AlarmId);

                //            if (!m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data))
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Recv Data : Error");
                //            }
                //            else
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Recv Data : Error(Recipe ID)");
                //            }

                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //        else
                //        {
                //            string date = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.02.09 minhan
                //            m_Server.ApdItemsHandler.SetData(m_GantryUnit.DataMatchingKey(0), eqpApdItems._GLS_START.Id, date);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass Start Time : " + date); // 10.12.29 minhan

                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 127;
                //        }
                //    }
                //    break;
                //case 127: // 10.12.25 minhan
                //    {
                //        if (!m_GantryUnit.IsRobotInterlock())
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check OK");
                //            seqNo = 130;
                //        }
                //        else if (GetElapsedTicks() > 3000)
                //        {
                //            AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1500;
                //        }
                //    }
                //    break;
                //case 130:
                //    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignON)) == 0)
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit align FW");
                //        //m_StartTicks = XFunc.GetTickCount(); // 10.12.25 minhan
                //        seqNo = 140;
                //    }
                //    else if (Rv > 0)
                //    {
                //        AlarmId = m_GantryUnit.ALM_AlignFw.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Fw : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1000;
                //    }
                //    break;
                //case 140:
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Broken Sensor Check Skip");
                //        seqNo = 150;
                //    }
                //    break;
                //case 150:
                //    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Align BW OK");
                //        //GlobalVar.TrGlsRecvReady = true;2009.08.25 kimgun
                //        seqNo = 160;
                //    }
                //    else if (Rv > 0)
                //    {
                //        AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1000;
                //    }
                //    break;
                //case 160:
                //    {
                //        seqNo = 0;
                //    }
                //    break;
                //#endregion

                #region TR Glass Send to Loader
                case 200:
                    if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                    {
                        //***************** Position Log **************************
                        RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                        m_GantryUnit.Servo.GetCurPosition(ref curPos);
                        servoCurPos = curPos.Pos[0];

                        string sCurPos;
                        sCurPos = string.Format("Start Position = {0}", servoCurPos);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                        //**********************************************************
                        seqNo = 205;
                    }
                    else
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 205: // 11.02.25 minhan
                    {
                        bool checkAlign = m_GantryUnit.IsAlignBw();
                        checkAlign &= !m_GantryUnit.IsAlignFw();

                        if (!checkAlign) // 11.02.25 minhan
                        {
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 2000;
                        }
                        else if (0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantrySendPosition)))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantrySendPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 210;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 210:
                    if (m_Control.IsPositionConfirmed(m_GantrySendPosition))
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : OK");

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 215;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        m_AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1500;
                    }
                    break;
                case 215: // 10.12.29 minhan
                    {
                        if (!m_GantryUnit.IsRobotInterlock())
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check OK");

                            if (GlobalVar.TrGlsSendReady)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Ready : OK");
                                seqNo = 245;// 11.01.27 minhan
                            }
                            else
                            {
                                //m_StartTicks = XFunc.GetTickCount(); // 11.06.01 minhan
                                seqNo = 217; // 11.05.30 minhan
                            }
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 217: // 11.06.01 minhan
                    {
                        if (m_EgisInterface.SetupCrackTRUse.GetValue<bool>())
                        {
                            TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                            m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                            if (!data.TRCrackAlarm)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : OK");
                                seqNo = 220;
                            }
                            else
                            {
                                m_AlarmId = m_EgisErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : NG");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1200; // 11.10.24 sungyong
                            }
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Unit No Use");
                            seqNo = 220;
                        }
                    }
                    break;
                case 220:
                    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignON)) == 0)
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit align FW");
                        //m_StartTicks = XFunc.GetTickCount(); // 11.05.30 minhan
                        seqNo = 230;
                    }
                    else if (Rv > 0)
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignFw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Fw : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 230: // 11.05.30 minhan
                    {
                        //if (GetElapsedTicks() > 1000)
                        //{
                        seqNo = 240;
                        //}
                    }
                    break;
                case 240:
                    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Align BW OK");
                        seqNo = 245; // 10.12.29 minhan
                    }
                    else if (Rv > 0)
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 245: // 10.12.29 minhan Cim 사양으로 봐서 여기서 하는게 맞는 것 같네. 
                    {
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        m_Server.ApdItemsHandler.SetData(m_GantryUnit.DataMatchingKey(0), eqpApdItems._GLS_END.Id, date);

                        items = m_Server.ApdItemsHandler.GetItems(-1);
                        itemLotapd = m_Server.ApdItemsHandler.GetItems(-3);

                        if (items == null) m_Server.ApdItemsHandler.AddNewItem(-1); // apd 파일없을 경우 -1은 생성을 하지만 -3 lot은 생성하지 않는다.
                        if (itemLotapd == null) m_Server.ApdItemsHandler.AddNewItem(-3);

                        m_Server.ApdItemsHandler.SetLastData(m_GantryUnit.DataMatchingKey(0));
                        m_Server.ApdItemsHandler.SetLotData();

                        TagGlassData sendData = new TagGlassData(); // 11.06.07 minhan
                        m_Server.GlassData.GetData(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0), ref sendData);
                        m_Server.SendData.UpdateData(sendData);

                        GlobalVar.TrGlsSendReady = true;
                        //GlobalVar.UlGlsReq = true; // 10.12.29 minhan 제거
                        GlobalVar.sndREQ = true;

                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass End Time : " + date);
                        seqNo = 250;
                    }
                    break;
                case 250: // 10.12.29 minhan
                    {
                        short curGtPosition = (short)m_GantryUnit.Servo.GetCurPointId();

                        if (GlobalVar.sndCOMP && (!m_GantryUnit.IsGlassExist(Logic.OR)/* || m_Simul.Motion*/)) // 10.12.29 minhan
                        {
                            //GlobalVar.UlGlsComplete = true; // 10.12.29 minhan 제거
                            GlobalVar.sndCOMP = false;
                            GlobalVar.sndREQ = false; // 11.02.25 minhan
                            GlobalVar.sndON_IF2 = false;
                            GlobalVar.TrGlsSendReady = false;

                            //if (GlobalVar.ExchnageReq) // 11.06.10 minhan
                            //{
                            //    m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, " Exchange Glass Out complete");
                            //    seqNo = 120;
                            //}
                            //else
                            //{
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass Out complete");
                            m_StartTicks = XFunc.GetTickCount(); // 10.12.29 minhan
                            seqNo = 260; // 11.06.01 minhan 
                            //}
                        }
                        else if (GlobalVar.sndCANCEL)
                        {
                            GlobalVar.sndCANCEL = false;//2009.07.23 kimgun 이거 왜 false 안 시켜주지??
                            GlobalVar.sndREQ = false; // 10.12.29 minhan
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Glass Out Cancel");
                            seqNo = 0;
                        }
                        else if (!m_GantryUnit.IsAlignBw()) // 10.12.25 Align Bw 상태가 아니라면 알람처리.
                        {
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else if ((curGtPosition != m_GantrySendPosition) ||
                                !m_Control.IsPositionConfirmed(m_GantrySendPosition)) // 10.12.25 minhan wait가 아니라면.
                        {
                            m_AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                //case 255: // 11.06.01 minhan
                //    {
                //        //m_EgisTimeout = m_Server.EgisInterfaceTiemout.GetValue<int>() * 1000;
                //        m_EgisTimeout = m_Server.EgisLoaderInterfaceTiemout.GetValue<int>() * 1000; // 11.04.27 minhan

                //        if (m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>())
                //        {
                //            if (GlobalVar.CrackOutCheckOK)
                //            {
                //                GlobalVar.CrackOutCheckOK = false;
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Send Crack Check : OK");
                //                m_StartTicks = XFunc.GetTickCount();
                //                seqNo = 260;
                //            }
                //            else if (GetElapsedTicks() > m_EgisTimeout)
                //            {
                //                AlarmId = m_EgisErr.Id;
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Send Crack Check : NG");
                //                ReturnSeqNo = seqNo;
                //                seqNo = 1000;
                //            }
                //        }
                //        else
                //        {
                //            //GlobalVar.CrackOutCheckOK = false; // 11.03.09 minhan
                //            //GlobalVar.CrackOutCheckReq = false;
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Unit No Use");
                //            m_StartTicks = XFunc.GetTickCount();
                //            seqNo = 260;
                //        }
                //    }
                //    break;
                case 260: // 10.12.29 minhan
                    {
                        if (!m_GantryUnit.IsRobotInterlock())
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check OK");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                #endregion

                #region Before TR Recv Glass from UL Hand//zhangliang

                case 306: // 11.01.24 minhan 다른 조건들은 제외하지만 핸드 구간으로 이동중에는 조건을 계속 모니터링 할 필요는 있다. 
                    {

                        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryWaitPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantryWaitPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 309;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 309:
                    {
                        if (m_Control.IsPositionConfirmed(m_GantryWaitPosition))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : OK");
                            seqNo = 300;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                #endregion

                #region TR Recv Glass from UL Hand
                case 300:
                    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                    {
                        //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Align BW OK");
                        //GlobalVar.TrGlsRecvReady = true;2009.08.25 kimgun kimx
                        seqNo = 310;
                    }
                    else if (Rv > 0)
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 310:
                    {
                        int curUlUpPosition = m_UlHandUnit.Servo.GetCurPointId();
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                        if ((curUlUpPosition == m_UlHandSend3Position) &&
                             m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position) &&
                             ulFishRibUp &&
                             !ulFishRibDn)
                        {
                            //if (m_Simul.Motion) m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send Condition Confirm");

                            if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                            {
                                //***************** Position Log **************************
                                RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                                m_GantryUnit.Servo.GetCurPosition(ref curPos);
                                servoCurPos = curPos.Pos[0];

                                string sCurPos;
                                sCurPos = string.Format("Start Position = {0}", servoCurPos);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                                //**********************************************************
                                seqNo = 320;
                            }
                            else
                            {
                                m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                        }
                    }
                    break;
                case 320: // 11.01.24 minhan 다른 조건들은 제외하지만 핸드 구간으로 이동중에는 조건을 계속 모니터링 할 필요는 있다. 
                    {
                        int curUlUpPosition = m_UlHandUnit.Servo.GetCurPointId();
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                        bool checkUlhand = ((short)curUlUpPosition == m_UlHandSend3Position);
                        checkUlhand &= m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position);
                        checkUlhand &= ulFishRibUp;
                        checkUlhand &= !ulFishRibDn;

                        bool checkglass = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));
                        checkglass |= m_GantryUnit.GlassExistSensor.IsDetected();

                        bool checkAlign = m_GantryUnit.IsAlignBw();
                        checkAlign &= !m_GantryUnit.IsAlignFw();

                        if (!checkUlhand || checkglass || !checkAlign) // 11.02.25 minhan 무식하다. 그러나 인터락은 좀 그래야 한다고 본다.
                        {
                            m_GantryUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop(); // 11.02.25 minhan
                            m_AlarmId = m_GantryUnit.ALM_RecvPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);

                            if (!checkUlhand)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Move : UL Hand condition Error");
                            }
                            else if (checkglass)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Move : Tr Glass Exist");
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Align Error");
                            }
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 2100;
                            break;
                        }

                        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryRecvPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantryRecvPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 325;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_RecvPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);

                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 325:
                    {
                        if (m_Control.IsPositionConfirmed(m_GantryRecvPosition))
                        {
                            m_GantryUnit.IfFlag.InReady = true;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Sensor : OK");
                            seqNo = 330;
                        }

                        //            bool ldFishRibUp = m_LdHandUnit.IsHandUp();
                        //            bool curldposition = m_LdHandControl.IsPositionConfirmed(m_LdHandRecv3Position);
                        //            bool ldFishGlassExist = m_LdHandUnit.IsGlassExist(Logic.AND);
                        //            bool ldFishDataExist = m_Server.GlassData.IsExist(m_LdHandUnit.DataMatchingKey(0));

                        //            if (m_LdHandUnit.IfFlag.InComp &&
                        //                ldFishRibUp &&
                        //                curldposition &&
                        //                ldFishGlassExist &&
                        //                ldFishDataExist)
                        //            {
                        //                if (m_LdHandUnit.IfFlag.InComp)//2009.09.02 kimgun InComp가 살아 있으면 Outcomp를 살린다.왜냐면 outcomp의 용도는 Ld Hand를 자유롭게 해주기위함이니까
                        //                {
                        //                    m_GantryUnit.IfFlag.OutComp = true;
                        //                }
                        //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Direct Receive Case");
                        //                seqNo = 327;
                        //            }
                        //            else
                        //            {
                        //                seqNo = 330;
                        //            }
                        //        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_RecvPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Recv Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                //case 327:
                //    if (!m_LdHandUnit.IfFlag.InComp ||
                //        !m_LdHandUnit.IsGlassExist(Logic.AND))//2009.08.26 kimgun ld hand에 glass가 없다면 ld hand는 seq상 tr에 자유로운 상태라 판단. 
                //    {
                //        m_GantryUnit.IfFlag.OutReady = false;
                //        m_GantryUnit.IfFlag.OutComp = false;
                //        seqNo = 330;
                //    }
                //    break;
                case 330:
                    if (m_UlHandUnit.IfFlag.OutComp &&
                       (m_UlHandControl.IsPositionConfirmed(m_UlHandSend1Position)) &&
                       ((short)m_UlHandUnit.Servo.GetCurPointId() == m_UlHandSend1Position))
                    {
                        //if (m_Simul.Motion) XFunc.GetTickCount(); // 11.01.24 minhan

                        m_Server.GlassData.Move(m_UlHandUnit.DataMatchingKey(0), m_GantryUnit.DataMatchingKey(0));
                        //2010.08.30 kimgun
                        TagGlassData RecvGlassData = new TagGlassData();
                        m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref RecvGlassData);
                        m_PortNo = (int)RecvGlassData.Item.GlassNumberCode.LotNo;
                        m_SlotNo = (int)RecvGlassData.Item.GlassNumberCode.SlotNo;
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Unit Out Complete");

                        if (m_Simul.Motion)
                        {
                            m_GantryUnit.GlassExistSensor.SetState(true, Logic.AND);
                            m_UlHandUnit.GlassExistSensor.SetState(false, Logic.AND);
                        }
                        seqNo = 335; // 11.02.25 minhan
                    }
                    break;
                case 335: // 11.02.25 minhan 여기로 이동
                    {
                        if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                        {
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("Start Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            //Kang Crack 검사기 사용 유무 IN                            
                            seqNo = 340;
                        }
                        else
                        {
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                    }
                    break;
                case 340: // 11.02.25 minhan
                    {
                        int curUlUpPosition = m_UlHandUnit.Servo.GetCurPointId();
                        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                        bool checkUlhand = true;
                        checkUlhand &= (short)curUlUpPosition != m_UlHandSend2Position;
                        checkUlhand &= curUlUpPosition != -1;
                        checkUlhand &= !m_UlHandControl.IsPositionConfirmed(m_UlHandSend2Position);

                        if (m_UlHandControl.IsPositionConfirmed(m_UlHandSend1Position) ||
                            m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position))
                        {
                            checkUlhand &= ulFishRibUp;
                            checkUlhand &= !ulFishRibDn;
                        }

                        bool checkAlign = m_GantryUnit.IsAlignBw();
                        checkAlign &= !m_GantryUnit.IsAlignFw();

                        if (!checkUlhand || !checkAlign)
                        {
                            m_GantryUnit.Servo.RbtEStop();
                            m_UlHandUnit.Servo.RbtEStop();
                            m_AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);

                            if (!checkUlhand)
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : UL Hand condition Error");
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Align Error");
                            }
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 2100;
                            break;
                        }

                        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryWaitPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantryWaitPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 350;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 350:
                    {
                        if (m_Control.IsPositionConfirmed(m_GantryWaitPosition))
                        {
                            m_GantryUnit.IfFlag.InComp = true;
                            GlobalVar.TrMoveUnloadDir = false;//2009.08.26 kimgun wait이동이 완료되었다는 확신만 있다면 I/F는 해야 한다.
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : OK");

                            if (m_EgisInterface.SetupCrackTRUse.GetValue<bool>()) // 11.02.25 minhan
                            {
                                GlobalVar.CrackInCheckReq = true;
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack unit use");
                            }
                            else
                            {
                                //GlobalVar.CrackInCheckReq = false; // 11.03.09 minhan
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack unit no use");
                            }
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 355;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 355:
                    if (!m_UlHandUnit.IfFlag.OutComp)
                    {
                        m_GantryUnit.IfFlag.InReady = false;
                        m_GantryUnit.IfFlag.InComp = false;
                        //   GlobalVar.TrMoveUnloadDir = false;2009.08.26 kimgun 350에서 처리
                        seqNo = 360;
                    }
                    break;
                case 360:
                    if (GetElapsedTicks() > 500)
                    {
                        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantrySendPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : OK");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantrySendPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 365;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_SendPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 365:
                    {
                        if (m_Control.IsPositionConfirmed(m_GantrySendPosition))
                        {
                            // m_GantryUnit.IfFlag.InComp = true;
                            //GlobalVar.TrMoveUnloadDir = false;//2009.08.26 kimgun wait이동이 완료되었다는 확신만 있다면 I/F는 해야 한다.
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : OK");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 370;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;

                case 370: // 11.02.25 minhan 아님 어떻게 해야하나. 알람
                    {
                        if (m_GantryUnit.IsGlassExist(Logic.OR) && m_Control.IsPositionConfirmed(m_GantrySendPosition))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Glass Exist Confirm");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 375; // 11.06.01 minhan
                        }
                        else if (GetElapsedTicks() > 2000) // 11.02.25 minhan
                        {
                            if (m_GantryUnit.IsGlassExist(Logic.OR))
                            {
                                m_AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : Error");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                            else
                            {
                                m_AlarmId = m_GantryUnit.ALM_GlassExist.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Glass Exist Sensor : Error");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                            }
                        }
                    }
                    break;
                case 375: // 11.06.01 minhan
                    {
                        m_EgisTimeout = m_Server.EgisInterfaceTiemout.GetValue<int>() * 1000; // 11.02.25 minhan

                        if (m_EgisInterface.SetupCrackTRUse.GetValue<bool>()) // 11.03.02 minhan
                        {
                            if (GlobalVar.CrackInCheckOK)
                            {
                                TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                                m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                                data.TRCrackAlarm = false;
                                data.Clone(data);
                                m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);

                                GlobalVar.CrackInCheckOK = false;
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : OK"); // 11.01.24 minhan
                                m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                                seqNo = 380;
                            }
                            else if (GlobalVar.CrackInCheckNG)
                            {
                                TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                                m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                                data.TRCrackAlarm = true;
                                data.Clone(data);
                                m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);

                                GlobalVar.CrackInCheckNG = false;
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : NG"); // 11.01.24 minhan
                                m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                                seqNo = 380;
                            }
                            if (GlobalVar.CrackInCheckWarning) // 2012.06.13 wang
                            {
                                TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                                m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                                data.TRCrackAlarm = false;
                                data.Clone(data);
                                m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);

                                GlobalVar.CrackInCheckWarning = false;
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : Warning");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = 380;
                            }
                            else if (GetElapsedTicks() > m_EgisTimeout) // 11.02.25 minhan
                            {
                                //AlarmId = m_EgisErr.Id; // 11.03.02 minhan
                                //m_EqpManager.SetAlarm(AlarmId);
                                //m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : Time out");
                                //ReturnSeqNo = seqNo;
                                //seqNo = 1000;

                                TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                                m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                                data.TRCrackAlarm = true;
                                data.Clone(data);
                                m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);

                                //GlobalVar.CrackInCheckNG = false; // 11.06.01 minhan
                                m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : Time out");
                                m_StartTicks = XFunc.GetTickCount();
                                seqNo = 380;
                            }
                        }
                        else
                        {
                            TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                            m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                            data.TRCrackAlarm = false;
                            data.Clone(data);
                            m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);

                            //GlobalVar.CrackInCheckOK = false; // 11.03.09 minhan
                            //GlobalVar.CrackInCheckReq = false; // 11.03.02 minhan
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Unit No Use");
                            m_StartTicks = XFunc.GetTickCount(); // 11.01.24 minhan
                            seqNo = 380;
                        }
                    }
                    break;
                case 380: // 11.02.25 minhan
                    {
                        if (m_Server.SetupSingleMode.GetValue<bool>() && (GetElapsedTicks() > 1000))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "SingleMode Use");
                            seqNo = 0;
                        }
                        else if (!m_Server.SetupSingleMode.GetValue<bool>())
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "SingleMode No Use");
                            seqNo = 0;
                        }
                    }
                    break;
                #endregion

                //#region TR Send Glass to LD Hand
                //case 400:
                //    if (m_LdHandUnit.IfFlag.InReady)
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand InReady Confirm");
                //        if (m_Server.SetupSingleMode.GetValue<bool>())
                //        {
                //            //m_Server.GenInfos.CurGlassCount++;
                //            m_GenInfo.CurGlassCount++;//2010.06.29 kimgun
                //            m_GenInfo.TodayGlassCount++;//2010.06.29 kimgun
                //            //m_StartTicks = XFunc.GetTickCount(); // 11.06.01 minhan
                //            seqNo = 405;
                //        }
                //        else
                //        {
                //            seqNo = 430;
                //        }
                //    }
                //    break;
                //case 405: // 11.06.01 minhan
                //    {
                //        if (m_EgisInterface.SetupCrackTRUse.GetValue<bool>())
                //        {
                //            TagGlassData data = new TagGlassData(); // 11.06.01 minhan
                //            m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);

                //            if (!data.TRCrackAlarm)
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : OK");
                //                seqNo = 410;
                //            }
                //            else
                //            {
                //                AlarmId = m_EgisErr.Id;
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Check : NG");
                //                ReturnSeqNo = seqNo;
                //                seqNo = 1000;
                //            }
                //        }
                //        else
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Unit No Use");
                //            seqNo = 410;
                //        }
                //    }
                //    break;
                //case 410:
                //    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignON)) == 0)
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit align FW");
                //        m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                //        seqNo = 420;
                //    }
                //    else if (Rv > 0)
                //    {
                //        AlarmId = m_GantryUnit.ALM_AlignFw.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Fw : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1000;
                //    }
                //    break;
                //case 420:
                //    //if (GetElapsedTicks() > 500) // 11.04.22 minhan
                //    //{
                //        seqNo = 430;
                //    //}
                //    break;
                //case 430:
                //    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                //    {
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Align BW OK");
                //        // GlobalVar.TrGlsRecvReady = true;2009.10.16 kimgun
                //        m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //        seqNo = 432;
                //    }
                //    else if (Rv > 0)
                //    {
                //        AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1000;
                //    }
                //    break;
                //case 432: // 11.02.25 minhan
                //    {
                //        if (!m_GantryUnit.IsRobotInterlock())
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check OK");
                //            seqNo = 435;
                //        }
                //        else if (GetElapsedTicks() > 3000)
                //        {
                //            AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1500;
                //        }
                //    }
                //    break;
                //case 435:
                //    {
                //        int curLdUpPosition = m_LdHandUnit.Servo.GetCurPointId();

                //        bool ldFishRibUp = m_LdHandUnit.IsHandUp();
                //        bool ldFishRibDn = m_LdHandUnit.IsHandDown();

                //        if ((curLdUpPosition == m_LdHandRecv1Position) &&
                //            m_LdHandControl.IsPositionConfirmed(m_LdHandRecv1Position) &&
                //            ldFishRibUp &&
                //            !ldFishRibDn)
                //        {
                //            //m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Recv Condition Confirm");

                //            if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                //            {
                //                //***************** Position Log **************************
                //                RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //                m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //                servoCurPos = curPos.Pos[0];

                //                string sCurPos;
                //                sCurPos = string.Format("Start Position = {0}", servoCurPos);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //                //**********************************************************
                //                seqNo = 440;
                //            }
                //            else
                //            {
                //                AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //                ReturnSeqNo = seqNo;
                //                seqNo = 1000;
                //            }
                //        }
                //    }
                //    break;
                //case 440: // 11.02.01 minhan
                //    {
                //        int curLdUpPosition = m_LdHandUnit.Servo.GetCurPointId();
                //        bool ldFishRibUp = m_LdHandUnit.IsHandUp();
                //        bool ldFishRibDn = m_LdHandUnit.IsHandDown();

                //        bool checkLdhand = ((short)curLdUpPosition == m_LdHandRecv1Position) ;
                //             checkLdhand &= m_LdHandControl.IsPositionConfirmed(m_LdHandRecv1Position);
                //             checkLdhand &= ldFishRibUp;
                //             checkLdhand &= !ldFishRibDn;

                //        bool checkAlign = m_GantryUnit.IsAlignBw(); // 11.02.25 minhan
                //             checkAlign &= !m_GantryUnit.IsAlignFw();

                //        if (!checkLdhand || !checkAlign)
                //        {
                //            m_GantryUnit.Servo.RbtEStop();
                //            m_LdHandUnit.Servo.RbtEStop();
                //            AlarmId = m_GantryUnit.ALM_SendPosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);

                //            if (!checkLdhand)
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : LD Hand condition Error");
                //            }
                //            else
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : Align Error");
                //            }
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //            seqNo = 2200;
                //            break;
                //        }

                //        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantrySendPosition))))
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : OK");

                //            if (m_Simul.Motion)
                //            {
                //                m_Control.SetPositionConfirmed(m_GantrySendPosition);
                //            }
                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                //            seqNo = 445;
                //        }
                //        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                //        {
                //            AlarmId = m_GantryUnit.ALM_SendPosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : Error");

                //            string sRv;
                //            sRv = string.Format("Rv = {0}", Rv);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //    }
                //    break;
                //case 445:
                //    if (m_Control.IsPositionConfirmed(m_GantrySendPosition))
                //    {
                //        m_GantryUnit.IfFlag.OutReady = true;
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : OK");
                //        seqNo = 450;
                //    }
                //    else if (GetElapsedTicks() > 2000)
                //    {
                //        AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : Error");
                //        ReturnSeqNo = seqNo;
                //        seqNo = 1500;
                //    }
                //    break;
                //case 450:
                //    if (m_LdHandUnit.IfFlag.InComp &&
                //       (m_LdHandUnit.Servo.GetCurPointId() == m_LdHandRecv3Position) &&
                //        m_LdHandControl.IsPositionConfirmed(m_LdHandRecv3Position))
                //    {
                //        bool trGlassExist = m_GantryUnit.IsGlassExist(Logic.AND);

                //        bool trGlassDataExist = m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0));

                //        int MaxGlsNo = m_Server.SetupMaxGlassNo.GetValue<int>();

                //        bool ulFishGlassExist = m_UlHandUnit.IsGlassExist(Logic.AND);
                //        bool ulFishDataExist = m_Server.GlassData.IsExist(m_UlHandUnit.DataMatchingKey(0));
                //        bool ulFishRibUp = m_UlHandUnit.IsHandUp();
                //        bool ulFishRibDn = m_UlHandUnit.IsHandDown();

                //        int curUlUpPosition = m_UlHandUnit.Servo.GetCurPointId();

                //        bool ulHandSendCondition = !m_GantryUnit.IsGlassExist(Logic.OR) && !trGlassDataExist &&
                //                                   ulFishGlassExist && ulFishDataExist &&
                //                                   ulFishRibUp && !ulFishRibDn &&
                //                                   (curUlUpPosition == m_UlHandSend3Position) &&
                //                                   m_UlHandControl.IsPositionConfirmed(m_UlHandSend3Position) &&
                //                                   m_UlHandUnit.IfFlag.OutReady;

                //        if (ulHandSendCondition &&
                //           ((m_GenInfo.EQPGlassCount >= m_Server.SetupMaxGlassNo.GetValue<int>()) ||
                //             m_GenInfo.CleanOut ||
                //             m_GenInfo.CycleStop ||
                //             GlobalVar.NoSubstrate)) // 11.02.25 minhan
                //        {
                //            seqNo = 0;
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Direct move Recv Position");
                //        }
                //        else
                //        {
                //            if (m_GantryUnit.IsAlignBw() && !m_GantryUnit.IsAlignFw())
                //            {
                //                //***************** Position Log **************************
                //                RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //                m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //                servoCurPos = curPos.Pos[0];

                //                string sCurPos;
                //                sCurPos = string.Format("Start Position = {0}", servoCurPos);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //                //**********************************************************
                //                seqNo = 460;
                //            }
                //            else
                //            {
                //                AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                //                ReturnSeqNo = seqNo;
                //                seqNo = 1000;
                //            }
                //        }
                //    }
                //    break;
                //case 460: // 11.02.25 minhan
                //    {
                //        int curLdUpPosition = m_LdHandUnit.Servo.GetCurPointId();
                //        bool ldFishRibUp = m_LdHandUnit.IsHandUp();
                //        bool ldFishRibDn = m_LdHandUnit.IsHandDown();

                //        bool checkLdhand = true;
                //        checkLdhand &= ((short)curLdUpPosition != m_LdHandRecv2Position);
                //        checkLdhand &= (curLdUpPosition != -1);
                //        checkLdhand &= !m_LdHandControl.IsPositionConfirmed(m_LdHandRecv2Position);

                //        if (m_LdHandControl.IsPositionConfirmed(m_LdHandRecv1Position) ||
                //            m_LdHandControl.IsPositionConfirmed(m_LdHandRecv3Position))
                //        {
                //            checkLdhand &= ldFishRibUp;
                //            checkLdhand &= !ldFishRibDn;
                //        }

                //        bool checkAlign = m_GantryUnit.IsAlignBw();
                //             checkAlign &= !m_GantryUnit.IsAlignFw();

                //        if (!checkLdhand || !checkAlign)
                //        {
                //            m_GantryUnit.Servo.RbtEStop();
                //            m_LdHandUnit.Servo.RbtEStop();
                //            AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);

                //            if (!checkLdhand)
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : LD Hand condition Error");
                //            }
                //            else
                //            {
                //                m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Align Error");
                //            }

                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                //            seqNo = 2200;
                //            break;
                //        }

                //        if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantryWaitPosition))))
                //        {
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : OK");

                //            if (m_Simul.Motion)
                //            {
                //                m_Control.SetPositionConfirmed(m_GantryWaitPosition);
                //            }
                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************
                //            m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                //            seqNo = 470;
                //        }
                //        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                //        {
                //            AlarmId = m_GantryUnit.ALM_WaitPosMove.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Move : Error");

                //            string sRv;
                //            sRv = string.Format("Rv = {0}", Rv);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                //            //***************** Position Log **************************
                //            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                //            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                //            servoCurPos = curPos.Pos[0];

                //            string sCurPos;
                //            sCurPos = string.Format("End Position = {0}", servoCurPos);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                //            //**********************************************************
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1000;
                //        }
                //    }
                //    break;
                //case 470:
                //    {
                //        if (m_Control.IsPositionConfirmed(m_GantryWaitPosition))
                //        {
                //            //m_GantryUnit.IfFlag.InComp = true;
                //            if (m_LdHandUnit.IfFlag.InComp)//2009.09.02 kimgun InComp가 살아 있으면 Outcomp를 살린다.왜냐면 outcomp의 용도는 Ld Hand를 자유롭게 해주기위함이니까
                //            {
                //                m_GantryUnit.IfFlag.OutComp = true;
                //            }
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : OK");
                //            seqNo = 480;
                //        }
                //        else if (GetElapsedTicks() > 2000)
                //        {
                //            AlarmId = m_GantryUnit.ALM_WaitPosSensor.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Wait Position Sensor : Error");
                //            ReturnSeqNo = seqNo;
                //            seqNo = 1500;
                //        }
                //    }
                //    break;
                //case 480:
                //    if (!m_LdHandUnit.IfFlag.InComp)
                //    {
                //        m_GantryUnit.IfFlag.OutComp = false;
                //        m_GantryUnit.IfFlag.OutReady = false;
                //        GlobalVar.TrMoveLoadDir = false;

                //        m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand Unit InComp false");
                //        seqNo = 0;
                //    }
                //    break;
                //#endregion

                #region TR Send Move
                //2009.08.15 kimgun
                //case 0에서 wait로 이동할 경우.
                case 500:
                    if ((Rv = SeqTrAlign((int)AlignCommand.TrAlign._alignOFF)) == 0)
                    {
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Align BW OK_case 500");
                        //GlobalVar.TrGlsRecvReady = true;2009.08.25 kimgun
                        m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                        seqNo = 505; // 11.02.25 minhan
                    }
                    else if (Rv > 0)
                    {
                        m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 505: // 11.02.25 minhan
                    {
                        if (!m_GantryUnit.IsRobotInterlock())
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check OK_case 505");
                            seqNo = 510;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Robot hand Check : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                case 510:
                    {
                        bool checkAlign = m_GantryUnit.IsAlignBw();
                        checkAlign &= !m_GantryUnit.IsAlignFw();

                        if (!checkAlign) // 11.02.25 minhan
                        {
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_GantryUnit.ALM_AlignBw.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Align Bw : Error");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            seqNo = 2000;
                        }
                        else if ((0 == (Rv = m_GantryUnit.Servo.RbtMovePos(m_GantrySendPosition))))
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : OK_case 510");

                            if (m_Simul.Motion)
                            {
                                m_Control.SetPositionConfirmed(m_GantrySendPosition);
                            }
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
                            servoCurPos = curPos.Pos[0];

                            string sCurPos;
                            sCurPos = string.Format("End Position = {0}", servoCurPos);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sCurPos);
                            //**********************************************************
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.01 minhan
                            seqNo = 520;
                        }
                        else if (Rv > 0 || !m_GantryUnit.Servo.Ready || !m_GantryUnit.Servo.HomeComp) // 11.03.25 minhan
                        {
                            m_AlarmId = m_GantryUnit.ALM_SendPosMove.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Move : Error");

                            string sRv;
                            sRv = string.Format("Rv = {0}", Rv);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, sRv);
                            //***************** Position Log **************************
                            RbtPos curPos = new RbtPos(m_GantryUnit.Servo.AxisCount);
                            m_GantryUnit.Servo.GetCurPosition(ref curPos);
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
                case 520:
                    {
                        if (m_Control.IsPositionConfirmed(m_GantrySendPosition))
                        {
                            //m_GantryUnit.IfFlag.InComp = true;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : OK");
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 2000)
                        {
                            m_AlarmId = m_GantryUnit.ALM_SendPosSensor.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Send Position Sensor : Error");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1500;
                        }
                    }
                    break;
                #endregion

                #region Alarm Reset
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;

                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1000)");

                        seqNo = m_ReturnSeqNo;
                    }
                    break;
                case 1200:// 11.10.24 sungyong Crack 알람 리셋시 알람해제(유저요청)
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        TagGlassData data = new TagGlassData();
                        m_Server.GlassData.GetData(m_GantryUnit.DataMatchingKey(0), ref data);
                        if (data.TRCrackAlarm)
                        {
                            data.TRCrackAlarm = false;
                            m_Server.GlassData.Update(m_GantryUnit.DataMatchingKey(0), data);
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Crack Error Recovery(1200)");
                            seqNo = m_ReturnSeqNo;
                        }
                    }
                    break;
                case 1500:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Error Recovery(1500)");

                        seqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000: // 11.02.25 minhan Tr Estop
                    {
                        bool EstopCheckTR = true;

                        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();

                        if (EstopCheckTR)
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr Estop OK");
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmIdServo = m_ManualChangeErr.Id; // 다시 auto 전환시 이니셜하니깐 자동 해지된다.
                            m_EqpManager.SetAlarm(m_AlarmIdServo);
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;

                        }
                    }
                    break;
                case 2100: // 11.02.25 minhan Tr,UL hand Estop
                    {
                        bool EstopCheckTR = true;
                        bool EstopCheckUL = true;

                        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();
                        EstopCheckUL = m_UlHandUnit.Servo.RbtEStop();

                        if (EstopCheckTR && EstopCheckUL)
                        {
                            m_Control.SetLog(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr and UL hand Estop OK");
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;
                        }
                        else if (GetElapsedTicks() > 3000)
                        {
                            m_AlarmIdServo = m_ManualChangeErr.Id;
                            m_EqpManager.SetAlarm(m_AlarmIdServo);
                            m_GenInfo.AutoMode = false;
                            seqNo = 0;

                        }
                    }
                    break;
                    //case 2200: // 11.02.25 minhan Tr,LD hand Estop
                    //    {
                    //        bool EstopCheckTR = true;
                    //        bool EstopCheckLD = true;

                    //        EstopCheckTR = m_GantryUnit.Servo.RbtEStop();
                    //        EstopCheckLD = m_LdHandUnit.Servo.RbtEStop();

                    //        if (EstopCheckTR && EstopCheckLD)
                    //        {
                    //            m_Control.SetLog(SeqFunName, seqNo, m_PortNo, m_SlotNo, "Tr and LD hand Estop OK");
                    //            m_GenInfo.AutoMode = false;
                    //            seqNo = 0;
                    //        }
                    //        else if (GetElapsedTicks() > 3000)
                    //        {
                    //            m_AlarmIdServo = m_ManualChangeErr.Id;
                    //            m_EqpManager.SetAlarm(m_AlarmIdServo);
                    //            m_GenInfo.AutoMode = false;
                    //            seqNo = 0;

                    //        }
                    //    }
                    //    break;
                    #endregion

            }
            this.m_SeqNo = seqNo;
            return -1;
        }
    }

    public class SeqUnitTrInterlock : XSeqFunction
    {
        #region Fields
        private GantryUnit m_GantryUnit = null;
        protected static IEqpManager m_EqpManager;
        protected static ServerManager m_Server;
        private ThreadTr_BOE_G8_DHDC m_Control;
        private Simul m_Simul;
        private GenInfoHandler m_GenInfo;
        #endregion

        #region Constructor
        public SeqUnitTrInterlock(ThreadTr_BOE_G8_DHDC control, GantryUnit tr)
        {
            m_GantryUnit = tr;
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            m_SeqFunName = m_GantryUnit.Name;
            m_GenInfo = GenInfoHandler.Instance;
        }
        #endregion

        public override int Do()
        {
            //if (!m_GenInfo.EqpInitComp) return -1; // 11.02.25 minhan

            int nSeqNo = this.m_SeqNo;
            int Rv = -1;

            bool Interlock = true;
            Interlock &= m_GantryUnit.RobotHandInterlock.IsDetected();

            switch (nSeqNo)
            {
                case 0:
                    if (/*m_Server.GenInfos.AutoMode && */eqpServoMotors._TR_Master_Servo_Motor.GetInMotion()) // 11.02.25 minhan
                    {
                        if (Interlock)
                        {
                            m_GantryUnit.Servo.RbtEStop();
                            m_AlarmId = m_GantryUnit.ALM_RobotInterrupt.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Moving Interlock Status");
                            m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                            nSeqNo = 500; // 11.02.25 minhan
                        }
                    }
                    break;
                case 500: // 11.02.25 minhan
                    {
                        bool TrEstopOk = m_GantryUnit.Servo.RbtEStop();

                        if (TrEstopOk)
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Unit Estop OK");
                            nSeqNo = 1000;
                        }
                        else if (GetElapsedTicks() > 5000) // 이런 경우는 없어야 하겠지만서도.
                        {
                            m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "TR Unit Estop fail");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed && !Interlock)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, nSeqNo, 0, 0, "Interlock Status Error Recovery(1000)");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return Rv;
        }
    }

}

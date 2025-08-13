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
    //  BM 작업 중, 아직 미완성
    public class ThreadTransferRobotControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static GenInfoHandler m_GenInfo;

        protected static _GenericCollection<TransferRobot> m_TransferRobots;
        protected static int m_UnitCount;

        internal static GantryUnit m_GantryUnit;
        internal static FishHand m_LoadHand;
        internal static FishHand m_UnloadHand;

        protected XLog m_GantryLog;
        protected XLog m_LoadLog;
        protected XLog m_UnloadLog;
        #endregion

        #region Flags
        internal static bool Flag_UnloadHandAirInterlock = false;
        #endregion

        #region Constructor
        public ThreadTransferRobotControl(int ScanTime, IServerManager Server)
        : base(ScanTime)
        {
            m_Server = Server;
            m_GenInfo = GenInfoHandler.Instance;

            m_TransferRobots = DmsComponents.Instance.ComponentContainer.GetCollection<TransferRobot>();
            m_UnitCount = m_TransferRobots.Count;

            foreach (TransferRobot unit in m_TransferRobots)
            {
                //  BM : Unit의 Name이 아닌 다른 방식으로 구분 할 수 있어야 함
                if (m_GantryUnit == null && unit.DeviceType == typeof(GantryUnit))
                    m_GantryUnit = unit as GantryUnit;

                if (m_LoadHand == null && unit.DeviceType == typeof(FishHand))
                    if ((unit as FishHand).UseType == FishHand.HandUseType.Load)
                        m_LoadHand = unit as FishHand;

                if (m_UnloadHand == null && unit.DeviceType == typeof(FishHand))
                    if ((unit as FishHand).UseType == FishHand.HandUseType.Unload)
                        m_UnloadHand = unit as FishHand;
            }

            m_GantryLog = new XLog("Gantry Log", XLog.LogStampType.UseStamp);
            m_LoadLog = new XLog("LD Hand Log", XLog.LogStampType.UseStamp);
            m_UnloadLog = new XLog("UL Hand Log", XLog.LogStampType.UseStamp);
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            foreach (TransferRobot unit in m_TransferRobots)
            {
                if (unit.DeviceType == typeof(GantryUnit))
                    (unit as GantryUnit).IfFlag.Reset();
                else if (unit.DeviceType == typeof(FishHand))
                    (unit as FishHand).IfFlag.Reset();
            }

            if (m_GantryUnit != null)
            {
                RegisterSequence(new SeqUnitGantry(this, m_Server, m_GantryUnit, m_LoadHand, m_UnloadHand));
                //RegisterSequence(new SeqUnitGantryInterlock(this, m_GantryUnit));

                //m_Server.AddSeqInitFunction(new SeqInitGantry(this, m_Server));
            }

            if (m_LoadHand != null)
            {
                //RegisterSequence(new SeqUnitLoadHand(this, m_LoadHand));
                //RegisterSequence(new SeqUnitLoadHandInterlock(this, m_LoadHand));
            }

            if (m_UnloadHand != null)
            {
                //RegisterSequence(new SeqUnitUnloadHand(this, m_UnloadHand));
                //RegisterSequence(new SeqUnitUnloadHandInterlock(this, m_UnloadHand));
            }
        }
        #endregion

        #region Override Methods
        public virtual void InitParameter()
        {
            foreach (TransferRobot unit in m_TransferRobots)
            {
                if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();

                TagTransferIfFlag Flag = null;
                if (unit.DeviceType == typeof(GantryUnit))
                    Flag = (unit as GantryUnit).IfFlag;
                else if (unit.DeviceType == typeof(FishHand))
                    Flag = (unit as FishHand).IfFlag;

                if (Flag != null)
                {
                    Flag.Reset();
                    Flag.InComp = false;
                    Flag.OutComp = false;
                }
            }
        }
        #endregion

        #region Methods
        public void SetLog_Gantry(string seqName, int seqNo, int portNo, int slotNo, string message)
        {
            string portName = (portNo <= 0) ? "" : portNo.ToString();
            string slotName = (slotNo <= 0) ? "" : slotNo.ToString();
            string seqNumber;

            try
            {
                seqNumber = "Case " + seqNo.ToString();
            }
            catch
            {
                seqNumber = "";
            }

            string log = string.Format("TrUnit  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            m_GantryLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }
        public void SetLog_LDHand(string seqName, int seqNo, int portNo, int slotNo, string message) // 11.02.25 minhan
        {
            string portName = (portNo <= 0) ? "" : portNo.ToString();
            string slotName = (slotNo <= 0) ? "" : slotNo.ToString();
            string seqNumber;

            try
            {
                seqNumber = "Case " + seqNo.ToString();
            }
            catch
            {
                seqNumber = "";
            }

            string log = string.Format("TrUnit  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            m_LoadLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }
        public void SetLog_ULHand(string seqName, int seqNo, int portNo, int slotNo, string message) // 11.02.25 minhan
        {
            string portName = (portNo <= 0) ? "" : portNo.ToString();
            string slotName = (slotNo <= 0) ? "" : slotNo.ToString();
            string seqNumber;

            try
            {
                seqNumber = "Case " + seqNo.ToString();
            }
            catch
            {
                seqNumber = "";
            }

            string log = string.Format("TrUnit  \t{0}\t{1}\t{2}\t{3}\t{4}", seqName, seqNumber, portName, slotName, message);

            m_UnloadLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_GenInfo.EqpLog = log;
        }

        public bool IsSeqRunCondition(TransferRobot unit)
        {
            bool run = true;
            run &= !IsInterlock();
            run &= !m_GenInfo.Pause;
            run &= m_GenInfo.AutoMode;

            if (unit == m_UnloadHand)
                run &= !Flag_UnloadHandAirInterlock;

            return run;
        }

        private bool IsInterlock()
        {
            HeavyInterlock heavy = m_Server.JobCond.HeavyInterlock;
            bool Interlock = false;
            Interlock |= ((heavy & HeavyInterlock.Door) > 0);
            Interlock |= ((heavy & HeavyInterlock.Area) > 0);
            return Interlock;
        }

        public bool IsPositionConfirmed_Gantry(short PosId)
        {
            switch (PosId)
            {
                case 0: //  Home
                    return m_GantryUnit.Servo.GetServoMotor(0).GetHomeSwitch();
                case 1: //  Wait
                    return m_GantryUnit.diWait_Pos_Sensor.IsDetected();
                case 2: //  Recv
                    return m_GantryUnit.diRecv_Pos_Sensor.IsDetected();
                case 3: //  Send
                    return m_GantryUnit.diSend_Pos_Sensor.IsDetected();
                default:
                    return false;
            }
        }
        public bool IsPositionConfirmed_FishHand(short PosId, FishHand.HandUseType UseType)
        {
            FishHand unit = UseType == FishHand.HandUseType.Load ? m_LoadHand : m_UnloadHand;

            switch (PosId)
            {
                case 0: //  Home
                case 1: //  Recv1
                    if (!AppConfig.Instance.Simul.Device)
                        return m_UnloadHand.Servo.GetServoMotor(0).GetHomeSwitch();
                    else
                        return unit.diRecv1_Pos_Sensor.IsDetected();
                case 2: //  Recv2
                    return unit.diRecv2_Pos_Sensor.IsDetected();
                case 3: //  Recv3
                    return unit.diRecv3_Pos_Sensor.IsDetected();
                case 4: //  Wait
                    return unit.diWait_Pos_Sensor.IsDetected();
                case 5: //  Send1
                    return unit.diSend1_Pos_Sensor.IsDetected();
                case 6: //  Send2
                    return unit.diSend2_Pos_Sensor.IsDetected();
                case 7: //  Send3
                    return unit.diSend3_Pos_Sensor.IsDetected();
                default:
                    return false;
            }
        }

        public void SetPositionConfirmed_Gantry(short PosId)
        {
            if (PosId < 0 || PosId > 3) return;

            m_GantryUnit.diWait_Pos_Sensor.SetState(PosId == 1);
            m_GantryUnit.diRecv_Pos_Sensor.SetState(PosId == 2);
            m_GantryUnit.diSend_Pos_Sensor.SetState(PosId == 3);
        }
        public void SetPositionConfirmed_FishHand(short PosId, FishHand.HandUseType UseType)
        {
            if (PosId < 0 || PosId > 7) return;

            FishHand unit = UseType == FishHand.HandUseType.Load ? m_LoadHand : m_UnloadHand;

            unit.diRecv1_Pos_Sensor.SetState(PosId == 0 || PosId == 1);
            unit.diRecv2_Pos_Sensor.SetState(PosId == 2);
            unit.diRecv3_Pos_Sensor.SetState(PosId == 3);
            unit.diWait_Pos_Sensor.SetState(PosId == 4);
            unit.diSend1_Pos_Sensor.SetState(PosId == 5);
            unit.diSend2_Pos_Sensor.SetState(PosId == 6);
            unit.diSend3_Pos_Sensor.SetState(PosId == 7);
        }
        #endregion
    }

    public abstract class SeqUnitTransferRobot : XSeqFunction
    {
        #region Constansts
        protected const short m_GantryHomePosition = 0;
        protected const short m_GantryWaitPosition = 1;
        protected const short m_GantryRecvPosition = 2;
        protected const short m_GantrySendPosition = 3;

        protected const short m_LdHandHomePosition = 0;
        protected const short m_LdHandRecv1Position = 1;
        protected const short m_LdHandRecv2Position = 2;
        protected const short m_LdHandRecv3Position = 3;
        protected const short m_LdHandWaitPosition = 4;
        protected const short m_LdHandSend1Position = 5;
        protected const short m_LdHandSend2Position = 6;
        protected const short m_LdHandSend3Position = 7;

        protected const short m_UlHandHomePosition = 0;
        protected const short m_UlHandRecv1Position = 1;
        protected const short m_UlHandRecv2Position = 2;
        protected const short m_UlHandRecv3Position = 3;
        protected const short m_UlHandWaitPosition = 4;
        protected const short m_UlHandSend1Position = 5;
        protected const short m_UlHandSend2Position = 6;
        protected const short m_UlHandSend3Position = 7;
        #endregion

        protected GantryUnit m_GantryUnit;
        protected FishHand m_LoadHand;
        protected FishHand m_UnloadHand;

        protected static IServerManager m_Server;
        protected static IEqpManager m_EqpManager;
        protected Simul m_Simul;
        protected GenInfoHandler m_GenInfo;

        protected ThreadTransferRobotControl m_Control;

        protected double m_ServoCurPos = 0.0;

        protected int m_PortNo = 0;
        protected int m_SlotNo = 0;

        protected int m_AlarmIdServo; // 11.02.25 minhan

        protected SeqUnitTransferRobot(ThreadTransferRobotControl Control, IServerManager Server, GantryUnit GantryUnit, FishHand LoadHand, FishHand UnloadHand)
        {
            m_GantryUnit = GantryUnit;
            m_LoadHand = LoadHand;
            m_UnloadHand = UnloadHand;

            m_Server = Server;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_GenInfo = GenInfoHandler.Instance;

            m_Control = Control;
        }
    }

    public class SeqUnitGantry : SeqUnitTransferRobot
    {
        #region Fields
        private int m_AlignSeqNo = 0;

        protected Alarm m_ServoNotReady;
        #endregion

        #region Constructor
        public SeqUnitGantry(ThreadTransferRobotControl Control, IServerManager Server, GantryUnit GantryUnit, FishHand LoadHand, FishHand UnloadHand)
        : base(Control, Server, GantryUnit, LoadHand, UnloadHand)
        {
            m_ServoNotReady = new Alarm(m_GantryUnit.Name + " : Servo Not Ready", AlarmLevel.S, AlarmCode.EquipmentSafety);// 10.12.25 minhan
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (!m_GenInfo.EqpInitComp) return -1;
            if (m_GantryUnit.Sequence[0] == null) m_GantryUnit.Sequence[0] = this;
            if (!m_Control.IsSeqRunCondition(m_GantryUnit)) return -1;

            //2010.08.30 kimgun
            if (m_Server.GlassData.IsExist(m_GantryUnit.DataMatchingKey(0)) &&
                (m_PortNo == 0) && (m_SlotNo == 0))
            {
                m_Server.GlassData.GetPortNo(m_GantryUnit.DataMatchingKey(0), ref m_PortNo, ref m_SlotNo);
            }
            else
            {
                m_PortNo = 0;
                m_SlotNo = 0;
            }

            int seqNo = m_SeqNo;
            switch (seqNo)
            {
                case 0:
                    {
                        #region Alarm Check
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
                        #endregion

                        short CurGtPosition = (short)m_GantryUnit.Servo.GetCurPointId();
                        short CurLdUpPosition = (short)m_LoadHand.Servo.GetCurPointId();
                        short CurUlUpPosition = (short)m_UnloadHand.Servo.GetCurPointId();

                        bool IsRobotInterlock = m_GantryUnit.IsRobotInterlock();
                        bool IsAlignFw = m_GantryUnit.IsAlignFw();
                        bool IsAlignBw = m_GantryUnit.IsAlignBw();

                        bool IsGlassExist = true;
                        if (m_Simul.Device)
                            IsGlassExist = m_Server.GlassData.IsExist(m_GantryUnit.Id * 2 - 1);//zhangliang 130521
                        else
                        {
                            IsGlassExist &= m_GantryUnit.IsGlassExist(Logic.OR);
                            IsGlassExist &= m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition);
                        }

                        int PosId = m_GantryUnit.DataMatchingKey(0);
                        bool IsGlassDataExist = m_Server.GlassData.IsExist(PosId);
                        bool IsProcessed = m_Server.GlassData.IsProcessed(PosId);

                        bool LoadHandSendCondition = !m_LoadHand.IsGlassExist(Logic.AND)
                                                  && !m_Server.GlassData.IsExist(m_LoadHand.DataMatchingKey(0))
                                                  && (m_LoadHand.IsHandUp() && !m_LoadHand.IsHandDown())
                                                  && (CurLdUpPosition == m_LdHandRecv1Position)
                                                  && (m_Control.IsPositionConfirmed_FishHand(m_LdHandRecv1Position, FishHand.HandUseType.Load))
                                                  && m_LoadHand.IfFlag.InReady;

                        bool UnloadHandSendCondition = m_UnloadHand.IsGlassExist(Logic.AND)
                                                    && m_Server.GlassData.IsExist(m_UnloadHand.DataMatchingKey(0))
                                                    && (m_UnloadHand.IsHandUp() && !m_UnloadHand.IsHandDown())
                                                    && (CurUlUpPosition == m_UlHandSend3Position)
                                                    && (m_Control.IsPositionConfirmed_FishHand(m_UlHandSend3Position, FishHand.HandUseType.Unload))
                                                    && m_UnloadHand.IfFlag.OutReady;

                        bool LoadInterferePos = CurLdUpPosition == m_LdHandRecv2Position
                                             || m_Control.IsPositionConfirmed_FishHand(m_LdHandRecv2Position, FishHand.HandUseType.Load)
                                             || CurLdUpPosition == -1;

                        bool UnloadInterferePos = CurUlUpPosition == m_UlHandSend2Position
                                               || m_Control.IsPositionConfirmed_FishHand(m_UlHandSend2Position, FishHand.HandUseType.Unload)
                                               || CurUlUpPosition == -1;

                        bool IsSingleMode = m_Server.IsSingleMode;
                        if (IsSingleMode)
                        {
                            m_GenInfo.CleanOut = false;
                            IsProcessed = false;
                        }

                        #region Position Not Detect Alarm
                        if ((CurGtPosition == -1) ||
                            (!m_Control.IsPositionConfirmed_Gantry(m_GantryRecvPosition) &&
                             !m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                             !m_Control.IsPositionConfirmed_Gantry(m_GantryWaitPosition) &&
                             !m_Control.IsPositionConfirmed_Gantry(m_GantryHomePosition)))
                        {
                            m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        #endregion

                        #region Servo Not Ready Alarm
                        if (!m_GantryUnit.Servo.Ready || m_GantryUnit.Servo.HomeComp)
                        {
                            m_AlarmId = m_ServoNotReady.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Servo Not Ready Alarm");
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                            break;
                        }
                        #endregion

                        #region Receive On Detected
                        if (BaseGlobalVar.rcvON_IF2)
                        {
                            if ((CurGtPosition == m_GantrySendPosition) &&
                                m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition))
                            {
                                if (!BaseGlobalVar.ExchangeReq)
                                {
                                    m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Receive)");
                                    seqNo = 100;
                                    break;
                                }
                                else // 11.06.10 minhan
                                {
                                    m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Receive) and Exchange Request ON");
                                    seqNo = 120;
                                    break;
                                }
                            }
                            else
                            {
                                m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm(rcvON_IF2 True)");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                                break;
                            }
                        }
                        #endregion

                        #region Send On Detected
                        if (BaseGlobalVar.sndON_IF2) // 11.02.25 minhan
                        {
                            if ((CurGtPosition == m_GantrySendPosition) &&
                                m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition)) // wait가 아닌데 이 플래그가 살았다고 한다면.알람처리
                            {
                                m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Interface Start(Send)");
                                seqNo = 240;
                                break;
                            }
                            else
                            {
                                m_AlarmId = m_GantryUnit.ALM_PosNotDetect.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Unit Position Not Detect Alarm(sndON_IF2 True)");
                                m_ReturnSeqNo = seqNo;
                                seqNo = 1000;
                                break;
                            }
                        }
                        #endregion

                        #region Receive From Loader case
                        if (!m_GenInfo.CleanOut &&
                            !m_GenInfo.CycleStop &&
                            !m_GantryUnit.IsGlassExist(Logic.OR) &&
                            !IsGlassDataExist &&
                            (CurGtPosition == m_GantryWaitPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantryWaitPosition) &&
                            !IsRobotInterlock &&
                            (m_GenInfo.EQPGlassCount < m_Server.GetSetupMaxGlassNo) &&
                            BaseGlobalVar.LoaderReady &&
                            !IsSingleMode &&
                            !BaseGlobalVar.NoSubstrate) // 11.02.08 minhan 로더가 interface 가능 상태이고,싱글모드가 아니라면 
                        {
                            if (BaseGlobalVar.rcvCANCEL)
                            {
                                BaseGlobalVar.rcvCANCEL = false;
                            }

                            BaseGlobalVar.TrMoveLoadDir = false;//2009.08.25 kimgun
                            BaseGlobalVar.TrMoveUnloadDir = false;//2009.08.25 kimgun
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from Loader case"); // 10.12.25 minhan
                            seqNo = 100;
                            break;
                        }
                        #endregion

                        #region Send To Loader case
                        if (IsGlassExist &&
                            IsGlassDataExist &&
                            IsProcessed &&
                            (CurGtPosition == m_GantrySendPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                            !IsRobotInterlock)
                        {
                            BaseGlobalVar.TrMoveLoadDir = false;//2009.08.25 kimgun
                            BaseGlobalVar.TrMoveUnloadDir = false;//2009.08.25 kimgun
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Send to Loader case");
                            seqNo = 200;
                            break;
                        }
                        #endregion

                        #region Go to Wait Pos Before Recv from UL Fish Hand case //zhangliang 130517
                        if (!m_GantryUnit.IsGlassExist(Logic.OR) &&
                            !IsGlassDataExist &&
                            !IsRobotInterlock &&
                            !UnloadHandSendCondition && (CurGtPosition == m_GantrySendPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                            ((m_GenInfo.EQPGlassCount <= m_Server.GetSetupMaxGlassNo) ||
                            m_GenInfo.CleanOut || m_GenInfo.CycleStop || IsSingleMode
                            || BaseGlobalVar.NoSubstrate || !BaseGlobalVar.LoaderReady)) // 11.04.18 minhan
                        {
                            BaseGlobalVar.TrMoveLoadDir = false;
                            BaseGlobalVar.TrMoveUnloadDir = true;
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "Before TR Recv from UL Hand case(306)");
                            seqNo = 306;
                            break;
                        }
                        #endregion

                        #region Recv from UL Fish Hand case
                        if (!m_GantryUnit.IsGlassExist(Logic.OR) &&
                            !IsGlassDataExist &&
                            !IsRobotInterlock &&
                            UnloadHandSendCondition &&
                            ((m_GenInfo.EQPGlassCount <= m_Server.GetSetupMaxGlassNo) ||
                            m_GenInfo.CleanOut || m_GenInfo.CycleStop || IsSingleMode
                            || BaseGlobalVar.NoSubstrate || !BaseGlobalVar.LoaderReady)) // 11.04.18 minhan
                        {
                            BaseGlobalVar.TrMoveLoadDir = false;
                            BaseGlobalVar.TrMoveUnloadDir = true;

                            if ((CurGtPosition == m_GantryRecvPosition) &&
                                m_Control.IsPositionConfirmed_Gantry(m_GantryRecvPosition))
                            {//startprocess==>recv pos move
                                m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from UL Hand case(320)");
                                seqNo = 320;
                                break;
                            }
                            else
                            {//startprocess==>Align Backword action
                                m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Recv from UL Hand case(300)");
                                seqNo = 300;
                                break;
                            }
                        }
                        #endregion

                        #region Send to LD Hand Case
                        if (IsGlassExist &&
                            IsGlassDataExist &&
                            !IsRobotInterlock &&
                            !IsProcessed &&
                            LoadHandSendCondition &&
                            (CurGtPosition == m_GantryWaitPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantryWaitPosition) &&
                            !BaseGlobalVar.LdHandAirInterlock)
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "TR Send to LD Hand case");
                            BaseGlobalVar.TrMoveLoadDir = true;
                            BaseGlobalVar.TrMoveUnloadDir = false;
                            seqNo = 400;
                            break;
                        }
                        #endregion

                        #region TR Recv Pos and Ul Hand SEND2 Pos case
                        if ((CurGtPosition == m_GantryRecvPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantryRecvPosition)/* || m_Simul.Device*/ &&
                            !IsAlignFw && IsAlignBw &&
                            !IsGlassDataExist &&
                            UnloadInterferePos &&
                            m_Server.GlassData.IsExist(m_UnloadHand.DataMatchingKey(0)) &&
                            !BaseGlobalVar.UlHandAirInterlock)
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send2 Pos case");
                            m_GantryUnit.IfFlag.InReady = true;//2009.08.26 kimgun
                            seqNo = 330;
                            break;
                        }
                        #endregion

                        #region TR Recv Pos and Ul Hand SEND1 Pos case
                        if ((CurGtPosition == m_GantryRecvPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantryRecvPosition)/* || m_Simul.Device*/ &&
                            !IsAlignFw && IsAlignBw &&
                            IsGlassDataExist &&
                            (CurUlUpPosition == m_UlHandSend1Position) &&
                            m_Control.IsPositionConfirmed_FishHand(m_UlHandSend1Position, FishHand.HandUseType.Unload)/* || m_Simul.Device*/ &&
                            !BaseGlobalVar.UlHandAirInterlock)
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "UL Hand Send1 Pos case");
                            m_GantryUnit.IfFlag.InReady = true;//2009.08.26 kimgun
                            seqNo = 335; // 11.02.25 minhan
                            break;
                        }
                        #endregion

                        #region TR Send Pos and Ld Hand Recv1 Pos case
                        if ((CurGtPosition == m_GantrySendPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                            !IsAlignFw && IsAlignBw &&
                            IsGlassDataExist &&
                            !m_Server.GlassData.IsExist(m_LoadHand.DataMatchingKey(0)) &&
                            !BaseGlobalVar.LdHandAirInterlock &&
                            LoadHandSendCondition)
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV1 Pos case");
                            m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                            seqNo = 450;
                            break;
                        }
                        #endregion

                        #region TR Send Pos and Ld Hand Recv2 Pos case
                        if ((CurGtPosition == m_GantrySendPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                            !IsAlignFw && IsAlignBw &&
                            IsGlassDataExist &&
                            !m_Server.GlassData.IsExist(m_LoadHand.DataMatchingKey(0)) &&
                            !BaseGlobalVar.LdHandAirInterlock &&
                            (CurLdUpPosition == m_LdHandRecv2Position) &&
                            m_Control.IsPositionConfirmed_FishHand(m_LdHandRecv2Position, FishHand.HandUseType.Load)/* || m_Simul.Device*/)
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV2 Pos case");
                            m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                            seqNo = 450;
                            break;
                        }
                        #endregion

                        #region TR Send Pos and Ld Hand Recv3 Pos case
                        if ((CurGtPosition == m_GantrySendPosition) &&
                            m_Control.IsPositionConfirmed_Gantry(m_GantrySendPosition) &&
                            !IsAlignFw && IsAlignBw &&
                            !BaseGlobalVar.LdHandAirInterlock &&
                            (CurLdUpPosition == m_LdHandRecv3Position) &&
                            m_Control.IsPositionConfirmed_FishHand(m_LdHandRecv3Position, FishHand.HandUseType.Load)/* || m_Simul.Device*/) // 10.12.29 minhan
                        {
                            m_Control.SetLog_Gantry(m_SeqFunName, seqNo, m_PortNo, m_SlotNo, "LD Hand RECV3 Pos case");
                            m_GantryUnit.IfFlag.OutReady = true;//2009.08.17 kimgun
                            seqNo = 450;
                            break;
                        }
                        #endregion
                    }
                    break;
            }

            m_SeqNo = seqNo;

            return -1;
        }
        #endregion
    }
}

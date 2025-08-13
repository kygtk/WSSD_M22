using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Sequence;
using Dms.Device;
using Dms.Common;
using Dms.Data;
using Dms.Server;
using System.Windows.Forms;

namespace Dms.Server
{
    public class SeqCrackEqpInterface_BOE_DHDC : XSequence
    {
        #region Fields
        protected static ServerManager m_Server;
        // protected static GenInfoHandler m_GenInfos;
        protected static _GenInfoHandler m_GenInfos;
        private XLog CrackLog;
        public static SeqCrackEqpInterface_BOE_DHDC Instance;
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqEgisAliveStatus(this));
            RegisterSequence(new SeqBeforeGlassSend(this));
            RegisterSequence(new SeqTransferVelocity(this));
            RegisterSequence(new SeqAfterGlassSend(this)); // 11.03.02 minhan
                                                           //RegisterSequence(new SeqLoaderVelocity(this)); // 11.06.01 minhan

        }
        #endregion

        #region Constructor
        public SeqCrackEqpInterface_BOE_DHDC(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            Instance = this;
            CrackLog = new XLog("EGiS CrackLog", XLog.LogStampType.UseStamp);
            RegisterSequences();
        }
        #endregion

        #region
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

        public void SetLog(string seqName, int portNo, int slotNo, string message)
        {
            string portName;
            string slotName;
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

            log = string.Format("EGiS Crack \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            CrackLog.TextOut(log);
        }

        #endregion
    }

    public class SeqBeforeGlassSend : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private EGiSInterface m_EgisInterface;
        private TagGlassData m_GlassData;
        private SeqCrackEqpInterface_BOE_DHDC m_Control;
        private GantryUnit m_TransferUnit;
        private ServoMotor m_TrServo; // 11.02.25 minhan
        private ushort[] m_GlsID;
        private ushort[] m_Velocity; // 11.02.25 minhan
        private string m_Buf; // 11.02.25 minhan
        private Alarm m_ALM_Response_Off;
        private Alarm m_ALM_Crack_NG;
        private Alarm m_ALM_Crack_Warning; // 12.06.13 wang
        private Alarm m_ALM_Crack_Cancel; // 11.02.25 minhan
        private int m_portID;
        private int m_slotID;
        private int m_Timeout; // 11.02.25 minhan
        private bool m_Use; // 11.02.25 minhan
        private int WarningId; // 12.07.11 wang
        #endregion

        #region Constructor
        public SeqBeforeGlassSend(SeqCrackEqpInterface_BOE_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
            m_GlassData = new TagGlassData();
            m_TransferUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_TrServo = eqpServoMotors._TR_Master_Servo_Motor; // 11.02.25 minhan
            m_GlsID = new ushort[10];
            m_Velocity = new ushort[2]; // 11.02.25 minhan
            m_Buf = "";
            m_portID = 0; // 11.02.25 minhan
            m_slotID = 0;
            WarningId = 0; // 12.07.11 wang
            m_Timeout = 0;
            m_Use = false;

            m_ALM_Response_Off = new Alarm("EGiS before Response Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_ALM_Crack_NG = new Alarm("EGiS before Crack NG Alarm", AlarmLevel.L, AlarmCode.EquipmentSafety);
            m_ALM_Crack_Warning = new Alarm("EGiS before Crack Warning", AlarmLevel.L, AlarmCode.EquipmentSafety); // 12.06.13 wang
            m_ALM_Crack_Cancel = new Alarm("EGIS Crack Check Cancel Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.06.01 minhan
            m_Control = control;
            this.m_SeqFunName = "EGiS Crack before ";
        }
        #endregion

        #region Methods
        public bool GetActVelocity() // 11.02.25 minhan
        {
            try
            {
                m_Buf = string.Format("{0:d4}", Math.Abs(m_TrServo.GetActVelocity())); // 11.02.25 minhan
                XFunc.ConvertToWord(m_Buf, ref m_Velocity, 0, 2, ByteOrder.BigEndian);
                m_EgisInterface.mowVelocity_IN.SetStates(m_Velocity, 0, 2);
                //m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf); // 11.05.30 minhan
                return true;
            }
            catch (Exception err)
            {
                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, m_Buf);
                m_Server.WriteExceptionLog(err.ToString());
                return false;
            }
        }


        #endregion

        #region Sequence
        public override int Do()
        {
            if (GlobalVar.TrCrackResetReq) // 11.06.01 minhan
            {
                GlobalVar.TrCrackResetReq = false;
                GlobalVar.CrackInCheckReq = false;
                GlobalVar.CrackInCheckOK = false;
                GlobalVar.CrackInCheckNG = false;
                GlobalVar.CrackInCheckWarning = false; // 12.06.13 wang

                this.m_SeqNo = 0;
                return -1;
            }

            // ±âº»Àº ¾Ë¶÷ ¹ß»ý½Ã ½Ã³ª¸®¿À´Â ¿ÀÁ÷ SetUp¿¡¼­ No useÇÏ¿© ÇØÃ¼ÇÏµµ·Ï ÇÑ´Ù.
            m_Use = m_EgisInterface.SetupCrackTRUse.GetValue<bool>(); // 11.03.02 minhan
            m_Timeout = m_Server.EgisInterfaceTiemout.GetValue<int>() * 1000; // 11.02.25 minhan

            int nSeqNo = this.m_SeqNo;
            int nRv = -1;
            if (m_EqpManager.AlarmResetSwitchPushed || !m_Use) // 12.07.11 wang
            {
                if (WarningId > 0)
                {
                    m_EqpManager.ResetAlarm(WarningId);
                    WarningId = 0;
                    m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Warning recovery");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    if (m_Use)
                    {
                        if (m_AlarmId > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                        }

                        if (m_Simul.Device) m_EgisInterface.mibReady.SetState(true);
                        m_Control.SetLog(m_SeqFunName, 0, 0, "Crack Unit Use");
                        nSeqNo = 10;
                    }
                    else // 11.02.25 minhan
                    {
                        if (m_AlarmId > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                        }

                        if (GlobalVar.EgisVelScan) GlobalVar.EgisVelScan = false;
                        if (GlobalVar.CrackInCheckReq) GlobalVar.CrackInCheckReq = false;
                        if (GlobalVar.CrackInCheckOK) GlobalVar.CrackInCheckOK = false;
                        if (GlobalVar.CrackInCheckNG) GlobalVar.CrackInCheckNG = false; // 11.06.01 minhan
                        if (GlobalVar.CrackInCheckWarning) GlobalVar.CrackInCheckWarning = false; // 12.06.13 wang
                        //if (GlobalVar.CrackINPassReq) GlobalVar.CrackINPassReq = false; // 11.03.02 minhan

                        if (m_EgisInterface.mobStart_IN.GetState()) m_EgisInterface.mobStart_IN.SetState(false);
                        if (m_EgisInterface.mobOK_Accept_IN.GetState()) m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        if (m_EgisInterface.mobNG_Accept_IN.GetState()) m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        if (m_EgisInterface.mobWarning_Accept_IN.GetState()) m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang

                        nSeqNo = 0;
                    }
                    break;
                case 10:
                    {
                        if (!m_Use)
                        {
                            nSeqNo = 0;
                        }
                        else if (GlobalVar.CrackInCheckReq)
                        {
                            //GlobalVar.CrackInCheckReq = false;
                            // ÇÁ·ÎÆÛÆ¼¸é ÇÁ·ÎÆÛÆ¼¸¦ º¸°í ÅëÀÏÇÏ°Ô Â¥µµ·Ï ºÎÅ¹ÇÏ»ï.

                            if (GlobalVar.EgisVelScan) GlobalVar.EgisVelScan = false;
                            if (GlobalVar.CrackInCheckOK) GlobalVar.CrackInCheckOK = false;
                            if (GlobalVar.CrackInCheckNG) GlobalVar.CrackInCheckNG = false; // 11.06.01 minhan
                            if (GlobalVar.CrackInCheckWarning) GlobalVar.CrackInCheckWarning = false; // 12.06.13 wang
                            //if (GlobalVar.CrackINPassReq) GlobalVar.CrackINPassReq = false; // 11.03.02 minhan

                            if (m_EgisInterface.mobStart_IN.GetState()) m_EgisInterface.mobStart_IN.SetState(false);
                            if (m_EgisInterface.mobOK_Accept_IN.GetState()) m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            if (m_EgisInterface.mobNG_Accept_IN.GetState()) m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            if (m_EgisInterface.mobWarning_Accept_IN.GetState()) m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang

                            for (int i = 0; i < 10; i++) // 11.02.25 minhan
                            {
                                m_GlsID[i] = 0;
                            }

                            for (int j = 0; j < 2; j++) // 11.02.25 minhan
                            {
                                m_Velocity[j] = 0;
                            }

                            m_Buf = "";

                            if (!GlobalVar.EgisAlive)
                            {
                                GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Alive off 10)");
                                nSeqNo = 1000;
                            }
                            else if (!m_EgisInterface.mibReady.GetState())
                            {
                                GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, 0, 0, "Crack Check fail(Ready off 10)");
                                nSeqNo = 1000;
                            }
                            else
                            {
                                if (m_Server.GlassData.GetData(m_TransferUnit.DataMatchingKey(0), ref m_GlassData))
                                {
                                    XFunc.ConvertToWord(m_GlassData.GlassID, ref m_GlsID, 0, 10, ByteOrder.BigEndian);

                                    m_EgisInterface.mowGlass_ID_IN.SetStates(m_GlsID, 0, 10);

                                    if (int.TryParse(m_GlassData.PortID, out m_portID) && int.TryParse(m_GlassData.SlotID, out m_slotID))
                                    {
                                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Request");
                                    }
                                    else
                                    {
                                        m_portID = 0;
                                        m_slotID = 0;
                                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Request(port, slot Number Error)");
                                    }

                                    //m_StartTicks = XFunc.GetTickCount(); // 11.02.25 minhan
                                    nSeqNo = 20;
                                }
                                else
                                {
                                    GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                    m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Data Error 10");
                                    nSeqNo = 1000;
                                }
                            }
                        }
                        else // 11.02.25 minhan
                        {
                            if (m_AlarmId > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId);
                            }

                            if (GlobalVar.EgisVelScan) GlobalVar.EgisVelScan = false;
                            //if (GlobalVar.CrackINPassReq) GlobalVar.CrackINPassReq = false; // 11.03.02 minhan

                            if (m_EgisInterface.mobStart_IN.GetState()) m_EgisInterface.mobStart_IN.SetState(false);
                            if (m_EgisInterface.mobOK_Accept_IN.GetState()) m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            if (m_EgisInterface.mobNG_Accept_IN.GetState()) m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            if (m_EgisInterface.mobWarning_Accept_IN.GetState()) m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang

                        }
                    }
                    break;
                case 20: // 11.02.25 minhan À½....¼Óµµ°ªµµ ¾²Áöµµ ¾Ê¾Ò´Âµ¥...start¶ó..
                    {
                        bool Trmotion = m_TrServo.GetInMotion();

                        if (!GlobalVar.EgisAlive || !m_Use) // ÀÌ°Ô Á×°Å³ª.
                        {
                            if (!m_Use)
                            {
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(No Use 20)");
                                nSeqNo = 0;
                            }
                            else
                            {
                                GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Alive off 20)");
                                nSeqNo = 1000;
                            }
                        }
                        else if (!m_EgisInterface.mibReady.GetState()) // ready°¡ Á×°Å³ª...
                        {
                            GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                            m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Ready off 20)");
                            nSeqNo = 1000;
                        }
                        else if (Trmotion || m_Simul.Device) // 11.02.25 minhan
                        {
                            bool checkVel = GetActVelocity(); // ¸ÕÀú ¾²°í..

                            if (checkVel)
                            {
                                GlobalVar.EgisVelScan = true; // µ¹¾Æ¶ó
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Velocity OK 20");
                                nSeqNo = 25;
                            }
                            else
                            {
                                GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Velocity Error 20");
                                nSeqNo = 1000;
                            }
                        }
                    }
                    break;
                case 25: // ÇöÀç ½ºÄË Å¸ÀÓÀÌ 10ms°Åµç...ÀÌ Á¤µµ¸é µÇÁö ¾Ê³ª.¾Æ´Ô µô·¹ÀÌ Ãß°¡ÇÏ»ï.
                    {
                        if (!GlobalVar.EgisAlive || !m_Use)
                        {
                            if (!m_Use)
                            {
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(No Use 25)");
                                nSeqNo = 0;
                            }
                            else
                            {
                                GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                                GlobalVar.EgisVelScan = false;
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Alive off 25)");
                                nSeqNo = 1000;
                            }
                        }
                        else if (!m_EgisInterface.mibReady.GetState())
                        {
                            GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                            GlobalVar.EgisVelScan = false;
                            m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Ready off 25)");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang
                            m_EgisInterface.mobStart_IN.SetState(true);
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_OK_IN.SetState(true);
                            }

                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Start");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30: // 11.02.25 minhan ¿©±â´Â »ç¿ëÀ¯¹«¸¦ º¼ ¼ö°¡ ¾ø´Âµ¥...ÀÎÅÍÆäÀÌ½º°¡ ½ÃÀÛÇÑ »óÅÂ¶ó ¹«Á¶°Ç ¿Ï·á´Â ÇØ¾ßµÇÁö ¾Ê³ª...Èì..
                    {
                        if (!GlobalVar.EgisAlive)
                        {
                            GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan

                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang
                            m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Alive off 30)");
                            nSeqNo = 1000;
                        }
                        else if (!m_EgisInterface.mibReady.GetState())
                        {
                            GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); //12.06.13 wang
                            m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check fail(Ready off 30)");
                            nSeqNo = 1000;
                        }
                        else if (m_EgisInterface.mibResult_OK_IN.GetState())
                        {
                            //OK µÈ »óÈ² Return À» ³¯¸®ÀÚ...... TR Glass Send ÇÏ±âÀü.....
                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobOK_Accept_IN.SetState(true);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); //12.06.13 wang
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_OK_IN.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Result OK");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                        else if (m_EgisInterface.mibResult_NG_IN.GetState())
                        {
                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(true);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); //12.06.13 wang
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_NG_IN.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Result NG");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                        }
                        else if (m_EgisInterface.mibResult_Warning_IN.GetState()) // 12.06.13 wang
                        {
                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(true); // 12.06.13 wang

                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_Warning_IN.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Result Waring");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 110;
                        }
                        else if (GetElapsedTicks() > m_Timeout)
                        {
                            // ÀÀ´ä ¾È¿À´Â°ÍÀ¸·Î ¾Ë¶÷ ¹ß»ý½Ã°í.. TR Glass Send ¸øÇÏ°Ô ÇÏÀÚ.....
                            GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                            GlobalVar.EgisVelScan = false;
                            m_EgisInterface.mobStart_IN.SetState(false);
                            m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            m_EgisInterface.mobWarning_Accept_IN.SetState(false); //12.06.13 wang
                            m_AlarmId = m_ALM_Response_Off.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Result Time Out 30");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            if (!m_EgisInterface.mobStart_IN.GetState())
                            {
                                m_EgisInterface.mobStart_IN.SetState(true);
                            }

                            if (m_EgisInterface.mobOK_Accept_IN.GetState())
                            {
                                m_EgisInterface.mobOK_Accept_IN.SetState(false);
                            }

                            if (m_EgisInterface.mobNG_Accept_IN.GetState())
                            {
                                m_EgisInterface.mobNG_Accept_IN.SetState(false);
                            }

                            if (m_EgisInterface.mobWarning_Accept_IN.GetState()) // 12.06.13 wang
                            {
                                m_EgisInterface.mobWarning_Accept_IN.SetState(false);
                            }
                        }
                    }
                    break;
                case 40: // OK
                    if (!m_EgisInterface.mibResult_OK_IN.GetState())
                    {
                        GlobalVar.CrackInCheckReq = false;
                        GlobalVar.CrackInCheckOK = true;
                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check OK");
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        //ÀÀ´ä ¾È¿Â´Ù... alarm ¹ß»ý
                        GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check mibResult_OK_IN off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobOK_Accept_IN.GetState()) m_EgisInterface.mobOK_Accept_IN.SetState(true); // 11.03.02 minhan
                    }
                    break;
                case 100:
                    if (!m_EgisInterface.mibResult_NG_IN.GetState())
                    {
                        GlobalVar.CrackInCheckReq = false; // 11.06.01 minhan
                        GlobalVar.CrackInCheckOK = false; // 11.07.28 sungyong
                        //GlobalVar.CrackInCheckNG = true;

                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang

                        if (m_EgisInterface.SetupCrackInDetect.Use) // 11.07.28 sungyong
                        {
                            GlobalVar.CrackInCheckNG = true;
                            m_AlarmId = m_ALM_Crack_NG.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check NG");
                            nSeqNo = 1000; // 11.06.01 minhan
                        }
                        else // 11.07.28 sungyong
                        {
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack NG Check Interlock Skip");
                            GlobalVar.CrackInCheckNG = false;
                            nSeqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        //ÀÀ´ä ¾È¿Â´Ù... alarm ¹ß»ý
                        GlobalVar.CrackInCheckNG = true; // 11.06.01 minhan
                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false); // 12.06.13 wang
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check mibResult_NG_IN off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobNG_Accept_IN.GetState()) m_EgisInterface.mobNG_Accept_IN.SetState(true); // 11.03.02 minhan
                    }
                    break;
                case 110:
                    if (!m_EgisInterface.mibResult_Warning_IN.GetState()) // 12.06.13 wang
                    {
                        GlobalVar.CrackInCheckReq = false;
                        GlobalVar.CrackInCheckOK = false;

                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false);

                        if (m_EgisInterface.SetupCrackInDetect.Use)
                        {
                            GlobalVar.CrackInCheckWarning = true;
                            WarningId = m_ALM_Crack_Warning.Id; // 12.07.11 wang
                            m_EqpManager.SetAlarm(WarningId); // 12.07.11 wang
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check Warning");
                            nSeqNo = 0; //12.07.11 wang
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Warning Check Interlock Skip");
                            GlobalVar.CrackInCheckWarning = false;
                            nSeqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        //·¢Éú´ËAlarm£¬ÐèÒª½«Éè±¸Down»ú£¬ËùÒÔÊ¹CrackInCheckNGÖÃÎªtrue
                        GlobalVar.CrackInCheckNG = true;
                        m_EgisInterface.mobStart_IN.SetState(false);
                        m_EgisInterface.mobOK_Accept_IN.SetState(false);
                        m_EgisInterface.mobNG_Accept_IN.SetState(false);
                        m_EgisInterface.mobWarning_Accept_IN.SetState(false);
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Check mibResult_Waring_IN off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobWarning_Accept_IN.GetState()) m_EgisInterface.mobWarning_Accept_IN.SetState(true);
                    }
                    break;
                case 1000: // 11.02.25 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed || !m_Use) // 11.06.01 minhan
                    {
                        GlobalVar.CrackInCheckReq = false;
                        GlobalVar.CrackInCheckOK = false; // 11.02.25 minhan
                        GlobalVar.CrackInCheckNG = false; // 11.06.01 minhan
                        GlobalVar.CrackInCheckWarning = false; // 12.06.13 wang

                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Alarm recovery");
                        nSeqNo = 0;
                    }
                    break;
                    //case 2000: // 11.06.01 minhan
                    //    {
                    //        if (m_EqpManager.AlarmResetSwitchPushed && (GlobalVar.CrackINPassReq || !m_Use))
                    //        {
                    //            if (GlobalVar.CrackINPassReq)
                    //            {
                    //                GlobalVar.CrackInCheckReq = false;
                    //                GlobalVar.CrackInCheckOK = true;
                    //                GlobalVar.CrackINPassReq = false;
                    //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Alarm Pass Mode recovery");
                    //            }
                    //            else
                    //            {
                    //                GlobalVar.CrackInCheckReq = false;
                    //                GlobalVar.CrackInCheckOK = false;
                    //                GlobalVar.CrackINPassReq = false;
                    //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Alarm Pass Mode recovery(No Use)");
                    //            }

                    //            m_EqpManager.ResetAlarm(AlarmId);
                    //            AlarmId = 0; // 11.02.25 minhan
                    //            nSeqNo = 0;
                    //        }
                    //        else if (m_EgisInterface.mibPass_IN.GetState() && !GlobalVar.CrackINPassReq)
                    //        {
                    //            GlobalVar.CrackINPassReq = true;
                    //        }
                    //    }
                    //    break;
            }
            this.m_SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }

    public class SeqAfterGlassSend : XSeqFunction // 11.03.02 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private EGiSInterface m_EgisInterface;
        //private TagGlassData m_GlassData; // 11.06.11 minhan
        private SeqCrackEqpInterface_BOE_DHDC m_Control;
        private BOELoaderInterface m_BoeInterface; // 11.03.02 minhan
        private GantryUnit m_TransferUnit;
        private ushort[] m_GlsID;
        //private ushort[] m_Velocity; // 11.06.01 minhan
        //private short[] m_Rbtvel; // 11.06.01 minhan
        //private string m_Buf; // 11.06.01 minhan
        private Alarm m_ALM_Response_Off;
        private Alarm m_ALM_Crack_NG;
        private Alarm m_ALM_Crack_Warning; // 12.06.13 wang
        private Alarm m_ALM_Crack_Cancel; // 11.02.25 minhan
        private int m_portID;
        private int m_slotID;
        private int WarningId; // 12.07.11 wang
        private int m_Timeout; // 11.02.25 minhan
        private bool m_Use; // 11.02.25 minhan
        #endregion

        #region Constructor
        public SeqAfterGlassSend(SeqCrackEqpInterface_BOE_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface; // 11.03.02 minhan
            //m_GlassData = new TagGlassData(); // 11.06.11 minhan
            m_TransferUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_GlsID = new ushort[10];
            //m_Velocity = new ushort[2]; // 11.02.25 minhan
            //m_Rbtvel = new short[2]; // 11.03.02 minhan
            //m_Buf = "";
            m_portID = 0; // 11.02.25 minhan
            m_slotID = 0;
            m_Timeout = 0;
            WarningId = 0; // 12.07.11 wang
            m_Use = false;

            m_ALM_Response_Off = new Alarm("EGiS After Response Off Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.06.01 minhan
            m_ALM_Crack_NG = new Alarm("EGiS After Crack NG Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_ALM_Crack_Warning = new Alarm("EGiS After Crack Warning", AlarmLevel.L, AlarmCode.EquipmentSafety); // 12.06.13 wang
            m_ALM_Crack_Cancel = new Alarm("EGIS After Crack Check Cancel Alarm", AlarmLevel.S, AlarmCode.EquipmentSafety);
            m_Control = control;
            this.m_SeqFunName = "EGiS Crack After ";
        }
        #endregion

        #region Methods

        #region Ã»ÓÃµÄ¶«Î÷
        //public bool GetActVelocity() // 11.06.01 minhan
        //{
        //    try
        //    {
        //        for (int k = 0; k < 2; k++) // 11.03.02 minhan
        //        {
        //            m_Rbtvel[k] = 0;
        //        }

        //        if (!m_Simul.Device)
        //        {
        //            m_Rbtvel = m_BoeInterface.miwRobot_Velocity.GetStates(2);
        //            m_Buf = XFunc.ConvertToString(m_Rbtvel, 0, 2, ByteOrder.BigEndian);
        //        }
        //        else
        //        {
        //            m_Buf = "0987";
        //        }

        //        int velocity = 0; // 11.03.02 minhan

        //        if (int.TryParse(m_Buf, out velocity))
        //        {
        //            m_Buf = string.Format("{0:d4}", velocity);
        //        }
        //        else
        //        {
        //            m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);
        //            m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Robot velocity Format Error");
        //            return false;
        //        }


        //        XFunc.ConvertToWord(m_Buf, ref m_Velocity, 0, 2, ByteOrder.BigEndian);
        //        m_EgisInterface.mowVelocity_OUT.SetStates(m_Velocity, 0, 2);
        //        m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);
        //        return true;
        //    }
        //    catch (Exception err)
        //    {
        //        m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);
        //        m_Server.WriteExceptionLog(err.ToString());
        //        return false;
        //    }
        //}
        #endregion
        #endregion

        #region Sequence
        public override int Do()
        {
            if (GlobalVar.LoaderCrackResetReq) // 11.06.01 minhan
            {
                GlobalVar.LoaderCrackResetReq = false;
                GlobalVar.CrackOutCheckReq = false;
                GlobalVar.CrackOutCheckOK = false;
                GlobalVar.CrackOutCheckNG = false;
                GlobalVar.CrackOutCheckWarning = false; // 12.06.13 wang

                this.m_SeqNo = 0;
                return -1;
            }

            // ±âº»Àº ¾Ë¶÷ ¹ß»ý½Ã ½Ã³ª¸®¿À´Â ¿ÀÁ÷ SetUp¿¡¼­ No useÇÏ¿© ÇØÃ¼ÇÏµµ·Ï ÇÑ´Ù.
            m_Use = m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>(); // 11.03.02 minhan
            //m_Timeout = m_Server.EgisInterfaceTiemout.GetValue<int>() * 1000; // 11.02.25 minhan
            m_Timeout = m_Server.EgisLoaderInterfaceTiemout.GetValue<int>() * 1000; // 11.04.27 minhan

            int nSeqNo = this.m_SeqNo;
            int nRv = -1;
            if (m_EqpManager.AlarmResetSwitchPushed || !m_Use) // 12.07.11 wang
            {
                if (WarningId > 0)
                {
                    m_EqpManager.ResetAlarm(WarningId);
                    WarningId = 0;
                    m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Warning recovery");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    if (m_Use)
                    {
                        if (m_AlarmId > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                        }

                        m_Control.SetLog(m_SeqFunName, 0, 0, "Send Crack Unit Use");
                        nSeqNo = 10;
                    }
                    else // 11.02.25 minhan
                    {
                        if (m_AlarmId > 0) // 11.02.25 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                        }

                        //if (GlobalVar.EgisVelScanOut) GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                        if (GlobalVar.CrackOutCheckReq) GlobalVar.CrackOutCheckReq = false;
                        if (GlobalVar.CrackOutCheckOK) GlobalVar.CrackOutCheckOK = false;
                        if (GlobalVar.CrackOutCheckNG) GlobalVar.CrackOutCheckNG = false; // 11.06.01 minhan
                        if (GlobalVar.CrackOutCheckWarning) GlobalVar.CrackOutCheckWarning = false; // 12.06.13 wang

                        if (m_EgisInterface.mobStart_OUT.GetState()) m_EgisInterface.mobStart_OUT.SetState(false);
                        if (m_EgisInterface.mobOK_Accept_OUT.GetState()) m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        if (m_EgisInterface.mobNG_Accept_OUT.GetState()) m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        if (m_EgisInterface.mobWarning_Accept_OUT.GetState()) m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang

                        nSeqNo = 0;
                    }
                    break;
                case 10:
                    {
                        if (!m_Use)
                        {
                            nSeqNo = 0;
                        }
                        else if (GlobalVar.CrackOutCheckReq)
                        {
                            //if (GlobalVar.EgisVelScanOut) GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            if (GlobalVar.CrackOutCheckOK) GlobalVar.CrackOutCheckOK = false;
                            if (GlobalVar.CrackOutCheckNG) GlobalVar.CrackOutCheckNG = false; // 11.06.01 minhan
                            if (GlobalVar.CrackOutCheckWarning) GlobalVar.CrackOutCheckWarning = false; // 12.06.13 wang

                            if (m_EgisInterface.mobStart_OUT.GetState()) m_EgisInterface.mobStart_OUT.SetState(false);
                            if (m_EgisInterface.mobOK_Accept_OUT.GetState()) m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            if (m_EgisInterface.mobNG_Accept_OUT.GetState()) m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            if (m_EgisInterface.mobWarning_Accept_OUT.GetState()) m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang

                            for (int i = 0; i < 10; i++) // 11.02.25 minhan
                            {
                                m_GlsID[i] = 0;
                            }

                            //for (int j = 0; j < 2; j++) // 11.02.25 minhan
                            //{
                            //    m_Velocity[j] = 0;
                            //}

                            //m_Buf = "";

                            if (!GlobalVar.EgisAlive)
                            {
                                //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                                GlobalVar.CrackOutCheckNG = true;
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Alive off 10)");
                                nSeqNo = 1000;
                            }
                            else if (!m_EgisInterface.mibReady.GetState())
                            {
                                //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                                GlobalVar.CrackOutCheckNG = true;
                                m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, 0, 0, "Send Crack Check fail(Ready off 10)");
                                nSeqNo = 1000;
                            }
                            else
                            {
                                if (m_Server.SendData != null) // 11.06.11 minhan
                                {
                                    XFunc.ConvertToWord(m_Server.SendData.GlassID, ref m_GlsID, 0, 10, ByteOrder.BigEndian);

                                    m_EgisInterface.mowGlass_ID_OUT.SetStates(m_GlsID, 0, 10);

                                    if (int.TryParse(m_Server.SendData.PortID, out m_portID) && int.TryParse(m_Server.SendData.SlotID, out m_slotID))
                                    {
                                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Request");
                                    }
                                    else
                                    {
                                        m_portID = 0;
                                        m_slotID = 0;
                                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Request(port, slot Number Error)");
                                    }

                                    nSeqNo = 20; // 11.06.01 minhan
                                }
                                else
                                {
                                    //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                                    GlobalVar.CrackOutCheckNG = true;
                                    m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                    m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Data Error 10");
                                    nSeqNo = 1000;
                                    //break; // 11.06.01 minhan
                                }

                                //bool checkVel = GetActVelocity(); // 11.06.01 minhan

                                //if (checkVel)
                                //{
                                //    m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check Velocity OK 10");
                                //    nSeqNo = 20;
                                //}
                                //else
                                //{
                                //    GlobalVar.CrackOutCheckNG = true; // 11.06.01 minhan
                                //    AlarmId = m_ALM_Crack_Cancel.Id;
                                //    m_EqpManager.SetAlarm(AlarmId);
                                //    m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check Velocity Error 10");
                                //    nSeqNo = 1000;
                                //    break;
                                //}
                            }
                        }
                        else // 11.02.25 minhan
                        {
                            if (m_AlarmId > 0)
                            {
                                m_EqpManager.ResetAlarm(m_AlarmId);
                            }

                            //if (GlobalVar.EgisVelScanOut) GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan

                            if (m_EgisInterface.mobStart_OUT.GetState()) m_EgisInterface.mobStart_OUT.SetState(false);
                            if (m_EgisInterface.mobOK_Accept_OUT.GetState()) m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            if (m_EgisInterface.mobNG_Accept_OUT.GetState()) m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            if (m_EgisInterface.mobWarning_Accept_OUT.GetState()) m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang

                        }
                    }
                    break;
                //case 20: // 11.06.01 minhan
                //    {
                //        if (!GlobalVar.EgisAlive || !m_Use) // ÀÌ°Ô Á×°Å³ª.
                //        {
                //            if (!m_Use)
                //            {
                //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check fail(No Use 20)");
                //                nSeqNo = 0;
                //            }
                //            else
                //            {
                //                //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                //                AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Alive off 20)");
                //                nSeqNo = 1000;
                //            }
                //        }
                //        else if (!m_EgisInterface.mibReady.GetState()) // ready°¡ Á×°Å³ª...
                //        {
                //            //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                //            AlarmId = m_EgisInterface.ALM_Ready.Id;
                //            m_EqpManager.SetAlarm(AlarmId);
                //            m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Ready off 20)");
                //            nSeqNo = 1000;
                //        }
                //        else
                //        {
                //            bool checkVel = GetActVelocity(); // ¸ÕÀú ¾²°í..

                //            if (checkVel)
                //            {
                //                //GlobalVar.EgisVelScanOut = true; // 11.06.01 minhan
                //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check Velocity OK 20");
                //                nSeqNo = 25;
                //            }
                //            else
                //            {
                //                //GlobalVar.CrackReportReq = true; // // 11.06.01 minhan
                //                AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                //                m_EqpManager.SetAlarm(AlarmId);
                //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Check Velocity Error 20");
                //                nSeqNo = 1000;
                //            }
                //        }
                //    }
                //    break;
                case 20: // ÇöÀç ½ºÄË Å¸ÀÓÀÌ 10ms°Åµç...ÀÌ Á¤µµ¸é µÇÁö ¾Ê³ª.¾Æ´Ô µô·¹ÀÌ Ãß°¡ÇÏ»ï.
                    {
                        if (!GlobalVar.EgisAlive || !m_Use)
                        {
                            if (!m_Use)
                            {
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(No Use 25)");
                                nSeqNo = 0;
                            }
                            else
                            {
                                //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                                m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                                m_EqpManager.SetAlarm(m_AlarmId);
                                m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Alive off 25)");
                                nSeqNo = 1000;
                            }
                        }
                        else if (!m_EgisInterface.mibReady.GetState())
                        {
                            //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Ready off 25)");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                            m_EgisInterface.mobStart_OUT.SetState(true);
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_OK_OUT.SetState(true);
                            }

                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Start");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30: // 11.02.25 minhan ¿©±â´Â »ç¿ëÀ¯¹«¸¦ º¼ ¼ö°¡ ¾ø´Âµ¥...ÀÎÅÍÆäÀÌ½º°¡ ½ÃÀÛÇÑ »óÅÂ¶ó ¹«Á¶°Ç ¿Ï·á´Â ÇØ¾ßµÇÁö ¾Ê³ª...Èì..
                    {
                        if (!GlobalVar.EgisAlive)
                        {
                            //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                            //GlobalVar.EgisVelScanOut = false;
                            GlobalVar.CrackOutCheckNG = true;
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang

                            m_AlarmId = m_ALM_Crack_Cancel.Id; // 11.02.25 minhan
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Alive off 30)");
                            nSeqNo = 1000;
                        }
                        else if (!m_EgisInterface.mibReady.GetState())
                        {
                            //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                            //GlobalVar.EgisVelScanOut = false;
                            GlobalVar.CrackOutCheckNG = true;
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                            m_AlarmId = m_EgisInterface.ALM_Ready.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check fail(Ready off 30)");
                            nSeqNo = 1000;
                        }
                        else if (m_EgisInterface.mibResult_OK_OUT.GetState())
                        {
                            //OK µÈ »óÈ² Return À» ³¯¸®ÀÚ...... TR Glass Send ÇÏ±âÀü.....
                            //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobOK_Accept_OUT.SetState(true);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_OK_OUT.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Result OK");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 40;
                        }
                        else if (m_EgisInterface.mibResult_NG_OUT.GetState())
                        {
                            //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                            //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(true);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_NG_OUT.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Result NG");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                        }
                        else if (m_EgisInterface.mibResult_Warning_OUT.GetState()) // 12.06.13 wang
                        {
                            //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                            //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(true);
                            if (m_Simul.Device)
                            {
                                m_EgisInterface.mibResult_Warning_OUT.SetState(false);
                            }
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Result Warning");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 120;
                        }
                        else if (GetElapsedTicks() > m_Timeout)
                        {
                            // ÀÀ´ä ¾È¿À´Â°ÍÀ¸·Î ¾Ë¶÷ ¹ß»ý½Ã°í.. TR Glass Send ¸øÇÏ°Ô ÇÏÀÚ.....
                            //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                            //GlobalVar.EgisVelScanOut = false; // 11.06.01 minhan
                            GlobalVar.CrackOutCheckNG = true;
                            m_EgisInterface.mobStart_OUT.SetState(false);
                            m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                            m_AlarmId = m_ALM_Response_Off.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Result Time Out 30");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            if (!m_EgisInterface.mobStart_OUT.GetState())
                            {
                                m_EgisInterface.mobStart_OUT.SetState(true);
                            }

                            if (m_EgisInterface.mobOK_Accept_OUT.GetState())
                            {
                                m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                            }

                            if (m_EgisInterface.mobNG_Accept_OUT.GetState())
                            {
                                m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                            }

                            if (m_EgisInterface.mobWarning_Accept_OUT.GetState()) // 12.06.13 wang
                            {
                                m_EgisInterface.mobWarning_Accept_OUT.SetState(false);
                            }
                        }
                    }
                    break;
                case 40: // OK
                    if (!m_EgisInterface.mibResult_OK_OUT.GetState())
                    {
                        GlobalVar.CrackOutCheckReq = false;
                        GlobalVar.CrackOutCheckOK = true;
                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check OK");
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        //ÀÀ´ä ¾È¿Â´Ù... alarm ¹ß»ý
                        //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                        GlobalVar.CrackOutCheckNG = true;
                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check mibResult_OK_OUT off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobOK_Accept_OUT.GetState()) m_EgisInterface.mobOK_Accept_OUT.SetState(true); // 11.03.02 minhan
                    }
                    break;
                case 100:
                    if (!m_EgisInterface.mibResult_NG_OUT.GetState()) // 11.06.01 minhan boe ¿äÃ»¿¡ ÀÇÇÑ
                    {
                        GlobalVar.CrackOutCheckReq = false;
                        GlobalVar.CrackOutCheckOK = false; // 11.07.28 sungyong
                        //GlobalVar.CrackOutCheckNG = true;

                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang

                        if (m_EgisInterface.SetupCrackOutDetect.Use) // 11.07.28 sungyong
                        {
                            GlobalVar.CrackOutCheckNG = true;
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check NG - Flag Set");
                            nSeqNo = 110;
                            //AlarmId = m_ALM_Crack_NG.Id;
                            //m_EqpManager.SetAlarm(AlarmId);
                            //nSeqNo = 1000;
                        }
                        else // 11.07.28 sungyong
                        {
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack NG Check Interlock Skip");
                            GlobalVar.CrackOutCheckNG = false;
                            nSeqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        //ÀÀ´ä ¾È¿Â´Ù... alarm ¹ß»ý
                        //GlobalVar.CrackReportReq = true; // 11.06.01 minhan
                        GlobalVar.CrackOutCheckNG = true;
                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check mibResult_NG_OUT off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobNG_Accept_OUT.GetState()) m_EgisInterface.mobNG_Accept_OUT.SetState(true); // 11.03.02 minhan
                    }
                    break;

                case 110:
                    if (!GlobalVar.ExchangeReq && !GlobalVar.sndON_IF2 && !GlobalVar.rcvON_IF2)
                    {
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check NG Alarm Set");
                        m_AlarmId = m_ALM_Crack_NG.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        nSeqNo = 1000;
                    }
                    break;
                case 120:
                    if (!m_EgisInterface.mibResult_Warning_OUT.GetState()) // 12.06.13 wang
                    {
                        GlobalVar.CrackOutCheckReq = false;
                        GlobalVar.CrackOutCheckOK = false;

                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false);


                        if (m_EgisInterface.SetupCrackOutDetect.Use)
                        {
                            GlobalVar.CrackOutCheckWarning = true;
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check Warning - Flag Set");
                            WarningId = m_ALM_Crack_Warning.Id; // 12.07.11 wang
                            m_EqpManager.SetAlarm(WarningId); // 12.07.11 wang
                            nSeqNo = 0; // 12.07.11 wang   
                        }
                        else
                        {
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack NG Check Interlock Skip");
                            GlobalVar.CrackOutCheckWarning = false;
                            nSeqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > 2000)
                    {
                        // ·¢Éú´ËAlarm ÐèÒªDown»ú¡£ËùÒÔ£¬½«CrackOutCheckNGÖÃÎªtrue£»
                        GlobalVar.CrackOutCheckNG = true;
                        m_EgisInterface.mobStart_OUT.SetState(false);
                        m_EgisInterface.mobOK_Accept_OUT.SetState(false);
                        m_EgisInterface.mobNG_Accept_OUT.SetState(false);
                        m_EgisInterface.mobWarning_Accept_OUT.SetState(false); // 12.06.13 wang
                        m_AlarmId = m_ALM_Response_Off.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Crack Check mibResult_Warning_OUT off Time out");
                        nSeqNo = 1000;
                    }
                    else
                    {
                        if (!m_EgisInterface.mobWarning_Accept_OUT.GetState()) m_EgisInterface.mobWarning_Accept_OUT.SetState(true);
                    }
                    break;
                case 1000: // 11.02.25 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed || !m_Use) // 11.06.01 minhan
                    {
                        GlobalVar.CrackOutCheckReq = false;
                        GlobalVar.CrackOutCheckOK = false; // 11.02.25 minhan
                        GlobalVar.CrackOutCheckNG = false; // 11.06.01 minhan
                        GlobalVar.CrackOutCheckWarning = false; // 12.06.13 wang
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Send Alarm recovery");
                        nSeqNo = 0;
                    }
                    break;
                    //case 2000: // 11.06.01 minhan
                    //    {
                    //        if (m_EqpManager.AlarmResetSwitchPushed && (GlobalVar.CrackOUTPassReq || !m_Use))
                    //        {
                    //            if (GlobalVar.CrackOUTPassReq)
                    //            {
                    //                GlobalVar.CrackOutCheckReq = false;
                    //                GlobalVar.CrackOutCheckOK = true;
                    //                GlobalVar.CrackOUTPassReq = false;
                    //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Alarm Pass Mode recovery");
                    //            }
                    //            else
                    //            {
                    //                GlobalVar.CrackOutCheckReq = false;
                    //                GlobalVar.CrackOutCheckOK = false;
                    //                GlobalVar.CrackOUTPassReq = false;
                    //                m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Alarm Pass Mode recovery(No Use)");
                    //            }

                    //            m_EqpManager.ResetAlarm(AlarmId);
                    //            AlarmId = 0; // 11.02.25 minhan
                    //            nSeqNo = 0;
                    //        }
                    //        else if (m_EgisInterface.mibPass_OUT.GetState() && !GlobalVar.CrackOUTPassReq)
                    //        {
                    //            GlobalVar.CrackOUTPassReq = true;
                    //        }
                    //    }
                    //    break;
            }
            this.m_SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }

    public class SeqEgisAliveStatus : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private SeqCrackEqpInterface_BOE_DHDC m_Control; // 11.02.25 minhan
        private Alarm ALM_ALIVE_SIGNAL;
        private EGiSInterface m_EgisInterface;
        private bool nCrackUse; // 11.02.25 minhan
        #endregion

        #region Constructor
        public SeqEgisAliveStatus(SeqCrackEqpInterface_BOE_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_EqpManager = m_Server.EqpStateManager;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;
            ALM_ALIVE_SIGNAL = new Alarm("Crack Alive Signal Alarm", AlarmLevel.S, AlarmCode.EquipmentStatusWarning); // 11.02.25 minhan
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
            this.m_SeqFunName = "Crack Machine";
            nCrackUse = false;
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            nCrackUse = false; // 11.03.02 minhan
            nCrackUse |= m_EgisInterface.SetupCrackTRUse.GetValue<bool>();
            nCrackUse |= m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>();

            int nSeqNo = this.m_SeqNo;
            int nRv = -1;

            switch (nSeqNo)
            {
                case 0:
                    if (nCrackUse)
                    {
                        //GlobalVar.EgisAlive = true;
                        nSeqNo = 5;
                    }
                    else
                    {
                        if (GlobalVar.EgisAlive) GlobalVar.EgisAlive = false;
                    }
                    break;
                case 5:
                    if (m_EgisInterface.mibHeartBeat.GetState())
                    {
                        GlobalVar.EgisAlive = true;
                        if (m_Simul.Device) m_EgisInterface.mibHeartBeat.SetState(false); // 11.02.25 minhan
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    else
                    {
                        GlobalVar.EgisAlive = true;
                        if (m_Simul.Device) m_EgisInterface.mibHeartBeat.SetState(true); // 11.02.25 minhan
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 10:
                    {
                        if (!nCrackUse)
                        {
                            nSeqNo = 0;
                        }
                        else if (!m_EgisInterface.mibHeartBeat.GetState())
                        {
                            if (m_Simul.Device) m_EgisInterface.mibHeartBeat.SetState(true); // 11.02.25 minhan
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            GlobalVar.EgisAlive = false;
                            m_AlarmId = ALM_ALIVE_SIGNAL.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, 0, 0, "Crack HeartBeat Error 10");
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!nCrackUse) // 11.02.25 minhan
                        {
                            nSeqNo = 0;
                        }
                        else if (m_EgisInterface.mibHeartBeat.GetState())
                        {
                            if (m_Simul.Device) m_EgisInterface.mibHeartBeat.SetState(false); // 11.02.25 minhan
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            GlobalVar.EgisAlive = false;
                            m_AlarmId = ALM_ALIVE_SIGNAL.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, 0, 0, "Crack HeartBeat Error 20");
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed || !nCrackUse) // 11.06.01 minhan
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0; // 11.02.25 minhan Á» ½á¶ó...
                        m_Control.SetLog(m_SeqFunName, 0, 0, "Alarm Recovery");
                        nSeqNo = 0;
                    }
                    break;

            }
            this.m_SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }

    public class SeqTransferVelocity : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private ushort[] m_Velocity;
        private EGiSInterface m_EgisInterface;
        private GantryUnit m_TransferUnit;
        private ServoMotor m_TrServo; // 11.02.25 minhan
        private SeqCrackEqpInterface_BOE_DHDC m_Control;
        private TagGlassData m_GlassData;
        private Alarm m_ALM_Velocity_Report; // 11.02.25 minhan
        private int m_portID;
        private int m_slotID;
        private bool SendVel; // 11.02.25 minhan
        private string m_Buf; // 11.02.25 minhan
        #endregion

        #region Constructor
        public SeqTransferVelocity(SeqCrackEqpInterface_BOE_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = m_Server.EqpStateManager; // 11.02.25 minhans
            m_Velocity = new ushort[2];
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
            m_GlassData = new TagGlassData();
            m_TransferUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_TrServo = eqpServoMotors._TR_Master_Servo_Motor; // 11.02.25 minhan
            m_ALM_Velocity_Report = new Alarm("EGiS Velocity Report Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.02.25 minhan
            m_Control = control;
            SendVel = false;
            m_Buf = "";
            this.m_SeqFunName = "Transfer Act Velocity";
        }
        #endregion

        #region Sequence
        public override int Do()
        {
            if (m_Server.GlassData.GetData(m_TransferUnit.DataMatchingKey(0), ref m_GlassData)) // 11.02.25 minhan
            {
                if ((int.TryParse(m_GlassData.PortID, out m_portID) &&
                     int.TryParse(m_GlassData.SlotID, out m_slotID)) == false)
                {
                    m_portID = 0;
                    m_slotID = 0;
                }
            }

            SendVel = m_EgisInterface.SetupCrackTRUse.GetValue<bool>(); // 11.03.02 minhan
            //SendVel &= GlobalVar.CrackInCheckReq;
            //SendVel &= (m_TrServo.GetInMotion() || m_Simul.Device) ; // kang ¿òÁ÷ÀÏ¶§¸¸ º¸°í ÇÏ´Â°Ô ³´°ÚÁö¹¹~~ 11.02.25 minhan ±¸·¡? 11.05.24 minhan
            SendVel &= GlobalVar.EgisVelScan; // 11.02.25 minhan

            int nSeqNo = this.m_SeqNo;
            int nRv = -1;

            switch (nSeqNo)
            {
                case 0:
                    if (SendVel)
                    {
                        try
                        {
                            m_Buf = string.Format("{0:d4}", Math.Abs(m_TrServo.GetActVelocity())); // 11.02.25 minhan
                            XFunc.ConvertToWord(m_Buf, ref m_Velocity, 0, 2, ByteOrder.BigEndian);
                            m_EgisInterface.mowVelocity_IN.SetStates(m_Velocity, 0, 2);
                            //m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);

                            //m_StartTicks = XFunc.GetTickCount(); // 11.05.24 minhan
                            //nSeqNo = 10;
                            nSeqNo = 0;
                        }
                        catch (Exception err) // 11.02.25 minhan
                        {
                            m_AlarmId = m_ALM_Velocity_Report.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Crack Report Error");
                            m_Server.WriteExceptionLog(err.ToString());
                            nSeqNo = 1000;
                        }
                    }
                    else
                    {
                        nSeqNo = 0;
                    }
                    break;
                //case 10:
                //    {
                //        //if (SendVel && GetElapsedTicks() > 100) // 11.02.25 minhan ÀÌ°Ô ÀÇ¹Ì¾ø´Â °Í °°À¸»ï.
                //        //{
                //        //    nSeqNo = 0;
                //        //}
                //        //else if (!SendVel)
                //        //{
                //        //    nSeqNo = 0;
                //        //}

                //        if (GetElapsedTicks() > 100)
                //        {
                //            nSeqNo = 0;
                //        }
                //    }
                //    break;
                case 1000: // 11.02.25 minhan
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        if (m_AlarmId > 0)
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                        }
                        m_Control.SetLog(m_SeqFunName, m_portID, m_slotID, "Alarm recovery");
                        nSeqNo = 0;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;
            return nRv;
        }
        #endregion
    }

    //public class SeqLoaderVelocity : XSeqFunction // 11.03.02 minhan
    //{
    //    #region Fields
    //    protected static ServerManager m_Server;
    //    protected static Simul m_Simul;
    //    protected static IEqpManager m_EqpManager;
    //    private ushort[] m_Velocity;
    //    private EGiSInterface m_EgisInterface;
    //    private GantryUnit m_TransferUnit;
    //    private SeqCrackEqpInterface_BOE_DHDC m_Control;
    //    private BOELoaderInterface m_BoeInterface; // 11.03.02 minhan
    //    private TagGlassData m_GlassData;
    //    private Alarm m_ALM_Velocity_Report;
    //    private int m_portID;
    //    private int m_slotID;
    //    private bool SendVel;
    //    private string m_Buf;
    //    private short[] m_Rbtvel; // 11.03.02 minhan
    //    private int m_RbtVelocity; // 11.03.02 minhan
    //    #endregion

    //    #region Constructor
    //    public SeqLoaderVelocity(SeqCrackEqpInterface_BOE_DHDC control)
    //    {
    //        m_Server = ServerManager.Instance;
    //        m_Simul = AppConfig.Instance.Simul;
    //        m_EqpManager = m_Server.EqpStateManager;
    //        m_Velocity = new ushort[2];
    //        m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
    //        m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface; // 11.03.02 minhan
    //        m_GlassData = new TagGlassData();
    //        m_TransferUnit = eqpTransferUnits._TR_Gantry_Unit;
    //        m_ALM_Velocity_Report = new Alarm("EGiS After Velocity Report Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
    //        m_Control = control;
    //        SendVel = false;
    //        m_Buf = "";
    //        m_Rbtvel = new short[2]; // 11.03.02 minhan
    //        m_RbtVelocity = 0; // 11.03.02 minhan
    //        this.SeqFunName = "Loader Act Velocity";
    //    }
    //    #endregion

    //    #region Sequence
    //    public override int Do()
    //    {
    //        if (m_Server.GlassData.GetData(m_TransferUnit.DataMatchingKey(0), ref m_GlassData))
    //        {
    //            if ((int.TryParse(m_GlassData.PortID, out m_portID) &&
    //                 int.TryParse(m_GlassData.SlotID, out m_slotID)) == false)
    //            {
    //                m_portID = 0;
    //                m_slotID = 0;
    //            }
    //        }

    //        SendVel = m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>(); // 11.03.02 minhan
    //        SendVel &= GlobalVar.EgisVelScanOut; // 11.03.02 minhan

    //        int nSeqNo = this.SeqNo;
    //        int nRv = -1;

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                if (SendVel) // 11.03.02 minhan
    //                {
    //                    try
    //                    {
    //                        if (!m_Simul.Device)
    //                        {
    //                            m_Rbtvel = m_BoeInterface.miwRobot_Velocity.GetStates(2);
    //                            m_Buf = XFunc.ConvertToString(m_Rbtvel, 0, 2, ByteOrder.BigEndian);
    //                        }
    //                        else
    //                        {
    //                            m_Buf = "0987";
    //                        }

    //                        if (int.TryParse(m_Buf, out m_RbtVelocity))
    //                        {
    //                            m_Buf = string.Format("{0:d4}", m_RbtVelocity);
    //                        }
    //                        else // 11.03.02 minhan
    //                        {
    //                            m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);
    //                            m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Robot velocity Format Error");
    //                            nSeqNo = 1000;
    //                            break;
    //                        }

    //                        XFunc.ConvertToWord(m_Buf, ref m_Velocity, 0, 2, ByteOrder.BigEndian);
    //                        m_EgisInterface.mowVelocity_OUT.SetStates(m_Velocity, 0, 2);
    //                        m_Control.SetLog(SeqFunName, m_portID, m_slotID, m_Buf);

    //                        m_StartTicks = XFunc.GetTickCount();
    //                        nSeqNo = 10;
    //                    }
    //                    catch (Exception err)
    //                    {
    //                        AlarmId = m_ALM_Velocity_Report.Id;
    //                        m_EqpManager.SetAlarm(AlarmId);
    //                        m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Crack Report Error");
    //                        m_Server.WriteExceptionLog(err.ToString());
    //                        nSeqNo = 1000;
    //                    }
    //                }
    //                else
    //                {
    //                    nSeqNo = 0;
    //                }
    //                break;
    //            case 10:
    //                {
    //                    if (GetElapsedTicks() > 100)
    //                    {
    //                        nSeqNo = 0;
    //                    }
    //                }
    //                break;
    //            case 1000: // 11.02.25 minhan
    //                if (m_EqpManager.AlarmResetSwitchPushed)
    //                {
    //                    if (AlarmId > 0)
    //                    {
    //                        m_EqpManager.ResetAlarm(AlarmId);
    //                        AlarmId = 0;
    //                    }
    //                    m_Control.SetLog(SeqFunName, m_portID, m_slotID, "Send Alarm recovery");
    //                    nSeqNo = 0;
    //                }
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;
    //        return nRv;
    //    }
    //    #endregion
    //}
}
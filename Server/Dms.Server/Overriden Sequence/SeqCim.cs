using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Dms.Data;
using System.Globalization;
using Dms.Device;
using Dms.Sequence;

namespace Dms.Server
{
    public class ThreadCim : XSequence
    {
        protected static ServerManager m_Server;

        public ThreadCim(int scanTime, ServerManager server)
            :base(scanTime)
        {
            m_Server = server;

            RegisterSequences();
        }

        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqUpStreamStatus("UPSTATUS", m_Server.RootNode.IfFromUp1));
            RegisterSequence(new SeqDnStreamStatus("DNSTATUS", m_Server.RootNode.IfFromDn1));

            RegisterSequence(new SeqCPCAliveMonitor("CPCMONITOR", m_Server.RootNode.IfSigCimStatus));
            RegisterSequence(new SeqEqpAlive("EQPALIVE", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqDateTimeAdjust("DATETIME", m_Server.RootNode.IfSigCimStatus, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqCpcMessage("CPCMESSAGE", m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqReportEqpStatus("EQPSTATUS", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqCurrentData("CURDATA ", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqAlarmReport("ALARMREP", m_Server.RootNode.IfSigHandshakeToCim, m_Server.RootNode.IfSigEqpStatusToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqRecipeAvailableCheck("RCPCHECK", m_Server.RootNode.IfSigHandshakeToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqRequestRecipeBody("REQRECIPE", m_Server.RootNode.IfSigHandshakeToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqApdReport("APDREPORT", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqGlassDataTransfer("TRANSFERDATA", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqGlassCount("GLSCOUNT", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqSupplyEnable("SUPPLYENABLE", m_Server.RootNode.IfSigEqpStatusToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqGlassDataLostReq("LOSTGLASS", m_Server.RootNode.IfSigHandshakeToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqGlassDataRecoveryReq("RECOVERYGLASS", m_Server.RootNode.IfSigHandshakeToCim, m_Server.RootNode.IfSigHandshakeFromCim));
            RegisterSequence(new SeqSendPause("SENDPAUSE", m_Server.RootNode.IfSigEqpStatusToCim));
            RegisterSequence(new SeqProcessPause("PROCESSPAUSE", m_Server.RootNode.IfSigEqpStatusToCim));
        }

        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (m_Server.IoController.DeviceState != ActiveState.Run) return;

                foreach (XSeqFunction seq in SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (m_Server.AppConfig.Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }
        }
    }

    public class SeqUpStreamStatus : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private int m_UpStatus = -1;
        private IfSigFromUp m_IfSigFromUp;
        #endregion

        #region Constructor
        public SeqUpStreamStatus(string seqName, IfSigFromUp sigFromUp)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;

            m_IfSigFromUp = sigFromUp;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if (m_IfSigFromUp.MibUpstreamReady.GetStatus() && (m_UpStatus != 1))
            {
                m_UpStatus = 1;
                m_Server.GenInfos.UpStreamState = "Ready";
            }
            else if (!m_IfSigFromUp.MibUpstreamReady.GetStatus() && (m_UpStatus != 0))
            {
                m_UpStatus = 0;
                m_Server.GenInfos.UpStreamState = "Not Ready";
            }

            return -1;
        }
        #endregion
    }

    public class SeqDnStreamStatus : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private int m_DnStatus = -1;
        private IfSigFromDn m_IfSigFromDn;
        #endregion

        #region Constructor
        public SeqDnStreamStatus(string seqName, IfSigFromDn sigFromDn)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;

            m_IfSigFromDn = sigFromDn;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if (m_IfSigFromDn.MibDownstreamReady.GetStatus() && (m_DnStatus != 1))
            {
                m_DnStatus = 1;
                m_Server.GenInfos.DnStreamState = "Ready";
            }
            else if (!m_IfSigFromDn.MibDownstreamReady.GetStatus() && (m_DnStatus != 0))
            {
                m_DnStatus = 0;
                m_Server.GenInfos.DnStreamState = "Not Ready";
            }

            return -1;
        }
        #endregion
    }

    public class SeqCPCAliveMonitor : XSeqFunction      //Common Func 7.1 C/PC Alive
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private IfSigCimStatus m_IfCimStatus;
        private int oldCpcAlive = -1;
        private uint timeOut = 3000; // 3sec
        private Alarm ALM_CpcAlive;
        #endregion

        #region Constructor
        public SeqCPCAliveMonitor(string seqName, IfSigCimStatus sigCimStatus)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfCimStatus = sigCimStatus;

            ALM_CpcAlive = new Alarm("C/PC Alive Monitor Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            StartTicks = XFunc.GetTickCount();
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if (!m_Simul.Melsec)
            {
                if (m_IfCimStatus.MibCimAlive.GetStatus() && (oldCpcAlive != 1))
                {
                    oldCpcAlive = 1;
                    StartTicks = XFunc.GetTickCount();
                }
                else if (!m_IfCimStatus.MibCimAlive.GetStatus() && (oldCpcAlive != 0))
                {
                    oldCpcAlive = 0;
                    StartTicks = XFunc.GetTickCount();
                }
            }
            else
            {
                if (GetElapsedTicks() > 2000)
                {
                    oldCpcAlive = 1;
                    StartTicks = XFunc.GetTickCount();
                }
                else if (GetElapsedTicks() > 2000)
                {
                    oldCpcAlive = 0;
                    StartTicks = XFunc.GetTickCount();
                }
            }

            if (GetElapsedTicks() > timeOut)
            {
                if (m_Server.GenInfos.HostReady)
                {
                    m_AlarmId = ALM_CpcAlive.Id;
                    m_EqpManager.SetAlarm(m_AlarmId);
                    m_Server.GenInfos.HostReady = false;
                    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "CPC Alive : Set Alarm");
                }
            }
            else
            {
                if (!m_Server.GenInfos.HostReady)
                {
                    if (m_AlarmId != 0)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "CPC Alive :Reset Alarm");
                    }
                    m_Server.GenInfos.HostReady = true;
                }
            }

            return -1;
        }
        #endregion
    }

    public class SeqEqpAlive : XSeqFunction             //Common Func 7.2 Equipment Alive
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatus;
        private uint timeOut = 1000; // 1sec
        #endregion

        #region Constructor
        public SeqEqpAlive(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatus = sigEqpStatusToCim;

            StartTicks = XFunc.GetTickCount();
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch(nSeqNo)
            {
                case 0:
                    if (GetElapsedTicks() > timeOut)
                    {
                        m_IfEqpStatus.MobEqpAlive.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > timeOut)
                    {
                        m_IfEqpStatus.MobEqpAlive.SetStatus(false);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqDateTimeAdjust : XSeqFunction       //Common Func 7.3 Date and Time Adjust
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigCimStatus m_IfCimStatus;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private uint timeOut = 4000; // 4sec
        #endregion

        #region Constructor
        public SeqDateTimeAdjust(string seqName, IfSigCimStatus sigCimStatus, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfCimStatus = sigCimStatus;
            m_IfHandshakeFromCim = sigHandshakeFromCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            { 
                case 0:
                    if (m_IfHandshakeFromCim.MibDateTimeReq.GetStatus())
                    {
                        string year = Convert.ToString(XFunc.ConvertBcdToInt(m_IfCimStatus.MiwYYMM.GetValue()));
                        string day = Convert.ToString(XFunc.ConvertBcdToInt(m_IfCimStatus.MiwDDHH.GetValue()));
                        string min = Convert.ToString(XFunc.ConvertBcdToInt(m_IfCimStatus.MiwMMSS.GetValue()));

                        string message = year + day + min;
                        DateAndTime.Today = DateTime.ParseExact(message.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);
                        DateAndTime.TimeOfDay = DateTime.ParseExact(message.Substring(6, 6), "HHmmss", CultureInfo.InvariantCulture);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, message);

                        StartTicks = XFunc.GetTickCount();

                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if(GetElapsedTicks() > timeOut)
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqCpcMessage : XSeqFunction           //Common Func 7.4 C/PC Message
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private uint timeOut = 4000; // 4sec
        #endregion

        #region Constructor
        public SeqCpcMessage(string seqName, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfHandshakeFromCim = sigHandshakeFromCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if(m_IfHandshakeFromCim.MibMessageReq.GetStatus())
                    {
                        short[] values = m_IfHandshakeFromCim.MiwCimMessage.GetValues();
                        string message = XFunc.ConvertToString(values, 0, values.Length, ByteOrder.BigEndian);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, message);
                        message = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss  ") + message;
                        m_Server.GenInfos.CimMessage = message;
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GetElapsedTicks() > timeOut)
                    {
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    if(!m_IfHandshakeFromCim.MibMessageReq.GetStatus())
                    {
                        nSeqNo = 0;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqReportEqpStatus : XSeqFunction      //Common Func 5.1 Equipment status
    {
        #region Enum
        private enum State : short
        {
            UnKnown, Run, Idle, Down
        }
        #endregion

        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private State m_OldState = State.UnKnown;
        private State m_CurState = State.UnKnown;
        #endregion

        #region Constructor
        public SeqReportEqpStatus(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if (m_Server.EqpStateManager.EqpUnit.EqpState == EqpState.Fault)
            {
                m_CurState = State.Down;
            }
            else if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Excute)
            {
                m_CurState = State.Run;
            }
            else if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Idle)
            {
                m_CurState = State.Idle;
            }

            if (m_OldState != m_CurState)
            {
                m_IfEqpStatusToCim.MowEqpStatus.SetValue((short)m_CurState);
                m_OldState = m_CurState;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_CurState.ToString());
            }
               
            //    if (m_OldEqpState == EqpState.Fault)
            return -1;
        }
        #endregion
    }

    public class SeqCurrentData : XSeqFunction          //Common Func 6.5 Current Data
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private uint timeOut = 1000;
        private short[] m_Data;
        #endregion

        #region Constructor
        public SeqCurrentData(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
            StartTicks = XFunc.GetTickCount();
            if(m_Simul.Melsec)
            {
                m_Data = new short[32];
            }
            else
            {
                m_Data = new short[m_IfEqpStatusToCim.MowCurrentData.Size];
            }
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if(GetElapsedTicks() > timeOut)
            {
                int i = 0;
                m_Data[i++] = (m_Server.JobCond.DryTransMode ? (short)1 : (short)0);
                m_Data[i++] = Convert.ToInt16(m_Server.JobCond.CurrentRecipe.Id);
                m_Data[i++] = (short)(eqpGauges._AP_Exhaust_HOT_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AP_Exhaust_ACID_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._RB_AC_Press_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Up_Exhaust_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Lo_Exhaust_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Up_Flow_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Lo_Flow_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Up_Press_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._AK_Lo_Press_Gauge.CurValue * 10);
                m_Data[i++] = (short)(eqpGauges._Driving_Air_Press_Gauge.CurValue * 10);

                m_IfEqpStatusToCim.MowCurrentData.SetValues(m_Data);

                StartTicks = XFunc.GetTickCount();
            }

            return -1;
        }
        #endregion
    }

    public class SeqAlarmReport : XSeqFunction          //Common Spec 6.6 Alarm/Warning Report
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private IfSigHandshakeToCim m_IfHandshakeToCim;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private uint timeOut = 4000;
        private Alarm ALM_ReplyTimeout;
        private int m_CurSetAlarmId = 0;
        private int m_CurResetAlarmId = 0;
        #endregion

        #region Constructor
        public SeqAlarmReport(string seqName, IfSigHandshakeToCim sigHandshakeToCim, IfSigEqpStatusToCim sigEqpStatusToCim, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = EqpManager.Instance;

            m_IfHandshakeToCim = sigHandshakeToCim;
            m_IfEqpStatusToCim = sigEqpStatusToCim;
            m_IfHandshakeFromCim = sigHandshakeFromCim;

            ALM_ReplyTimeout = new Alarm("Alarm Report Reply Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Server.SeqFlag.AlarmSetReq)
                    {
                        m_Server.SeqFlag.AlarmSetReq = false;
                        m_CurSetAlarmId = m_Server.SeqFlag.SetAlarmId;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Set Request");
                        nSeqNo = 10;
                    }
                    else if (m_Server.SeqFlag.AlarmResetReq)
                    {
                        m_Server.SeqFlag.AlarmResetReq = false;
                        m_CurResetAlarmId = m_Server.SeqFlag.ResetAlarmId;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Reset Request");
                        nSeqNo = 100;
                    }
                    break;
                case 10:
                    {
                        int value = 0;
                        value = ((1 & 0x0001) << 15) + (m_CurSetAlarmId & 0x7FFF);
                        m_IfEqpStatusToCim.MowAlarmWarningData.SetValue((short)value);
                        m_IfHandshakeToCim.MobAlarmReport.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        string msg = string.Format("Alarm Set : {0}", m_CurSetAlarmId);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, msg);
                        nSeqNo = 20;

                        if (m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibAlarmReply.SetStatus(true);
                        }

                        //GlassStatusCode turn off if process finish abnormally.
                        TagGlassData glassData = new TagGlassData();
                        int[] ids;
                        m_Server.GlassData.GetAllPositionId(out ids);
                        foreach (int id in ids)
                        {
                            m_Server.DataProvider.GlassDataProvider.GetData(id, ref glassData);
                            if (glassData.Item.GlassStatusCode != StatusCodeFlag.NONE)
                            {
                                glassData.Item.GlassStatusCode = StatusCodeFlag.NONE;
                            }
                        }
                    }
                    break;
                case 20:
                    if (m_IfHandshakeFromCim.MibAlarmReply.GetStatus())
                    {
                        m_IfHandshakeToCim.MobAlarmReport.SetStatus(false);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Set Finish");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_ReplyTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Set Reply Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 30:
                    //if (!m_IfHandshakeFromCim.MibAlarmReply.GetStatus())
                    if(GetElapsedTicks() > 1000)
                    {
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Reply Off");
                        nSeqNo = 0;
                    }
                    break;
                case 100:
                    {
                        int value = 0;
                        value = ((0 & 0x0001) << 15) + (m_CurResetAlarmId & 0x7FFF);
                        m_IfEqpStatusToCim.MowAlarmWarningData.SetValue((short)value);
                        m_IfHandshakeToCim.MobAlarmReport.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        string msg = string.Format("Alarm Reset : {0}", m_CurResetAlarmId);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, msg);
                        nSeqNo = 110;

                        if (m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibAlarmReply.SetStatus(true);
                        }
                    }
                    break;
                case 110:
                    if(m_IfHandshakeFromCim.MibAlarmReply.GetStatus())
                    {
                        m_IfHandshakeToCim.MobAlarmReport.SetStatus(false);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Reset Finish");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 120;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_ReplyTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Reset Reply Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 120:
                    //if (!m_IfHandshakeFromCim.MibAlarmReply.GetStatus())
                    if (GetElapsedTicks() > 1000)
                    {
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Reply Off");
                        nSeqNo = 0;
                    }
                    break;
                case 1000:
                    //if(m_EqpManager.AlarmResetSwitchPushed)
                    if(m_IfHandshakeFromCim.MibAlarmReply.GetStatus())  //reset조건은?
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Release");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqRecipeAvailableCheck : XSeqFunction //Common Spec 6.3.1
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private IfSigHandshakeToCim m_IfHandshakeToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private uint timeOut = 4000;
        private Alarm ALM_RequestOffTimeout;
        #endregion

        #region Constructor
        public SeqRecipeAvailableCheck(string seqName, IfSigHandshakeToCim sigHandshakeToCim, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = EqpManager.Instance;

            m_IfHandshakeFromCim = sigHandshakeFromCim;
            m_IfHandshakeToCim = sigHandshakeToCim;

            ALM_RequestOffTimeout = new Alarm("Recipe Available Check Request Off Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion

        #region Methods
        private short IsAvailable(short[] data)
        {
            int count = m_Server.DataProvider.RecipeProvider.GetRecipeItemCount();
            if(count > data.Length) return (short)0;

            if (TagRecipe.IsAvailable(data)) return (short)1;
            else return (short)0;
        }

        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if(m_IfHandshakeFromCim.MibRecipeAvailableChkReq.GetStatus())
                    {
                        short[] data = m_IfHandshakeFromCim.MiwRecipeForAvailableCheck.GetValues();

                        m_IfHandshakeToCim.MowRecipeAvailableChkResult.SetValue(IsAvailable(data));
                        m_IfHandshakeToCim.MobRecipeAvailableChkReply.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (!m_IfHandshakeFromCim.MibRecipeAvailableChkReq.GetStatus())
                    {
                        m_IfHandshakeToCim.MobRecipeAvailableChkReply.SetStatus(false);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Available Check : Finish");
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_RequestOffTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Available Check : Set Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (!m_IfHandshakeFromCim.MibRecipeAvailableChkReq.GetStatus())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        nSeqNo = m_ReturnSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Available Check : Reset Alarm");
                    }
                    break;
            }
            this.SeqNo = nSeqNo;
            
            return -1;
        }
        #endregion
    }

    public class SeqRequestRecipeBody : XSeqFunction    //Common Spec 6.3.2
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private IfSigHandshakeToCim m_IfHandshakeToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private uint timeOut = 4000;
        private Alarm ALM_ReplyOnTimeout;
        private Alarm ALM_ReplyOffTimeout;
        private short m_CheckResult = -1;
        #endregion

        #region Constructor
        public SeqRequestRecipeBody(string seqName, IfSigHandshakeToCim sigHandshakeToCim, IfSigHandshakeFromCim sigHandShakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = EqpManager.Instance;

            m_IfHandshakeFromCim = sigHandShakeFromCim;
            m_IfHandshakeToCim = sigHandshakeToCim;

            ALM_ReplyOnTimeout = new Alarm("Request Recipe Body Reply On Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_ReplyOffTimeout = new Alarm("Request Recipe Body Reply Off Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion

        #region Methods
        private bool IsAvailable(short[] data)
        {
            int count = m_Server.DataProvider.RecipeProvider.GetRecipeItemCount();
            if (count > data.Length) return false;

            if (m_Simul.Melsec)
            {
                return true;
            }
            else
            {
                return TagRecipe.IsAvailable(data);
            }
        }

        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if(m_Server.SeqFlag.RecipeBodyReq)
                    {
                        m_Server.SeqFlag.RecipeBodyReq = false;
                        m_Server.SeqFlag.RecipeCheckResult = RecipeCheck.None;
                        m_IfHandshakeToCim.MowReqHostRecipeId.SetString(m_Server.SeqFlag.RecipeBodyReqId);
                        m_IfHandshakeToCim.MobRecipeBodyReq.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Start");
                        nSeqNo = 10;

                        if (m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibRecipeBodyReply.SetStatus(true);
                        }
                    }
                    break;
                case 10:
                    if(m_IfHandshakeFromCim.MibRecipeBodyReply.GetStatus())
                    {
                        short[] data = m_IfHandshakeFromCim.MiwRecipeForProcessing.GetValues();

                        if (m_Simul.Melsec)
                        {
                            //Simulation상에서 Recipe변경사항 적용 검토를 위한 코드
                            //data = new short[] { 5, 1, 30, 60, 220, 5900, 1, 1 };
                            data = new short[m_IfHandshakeFromCim.MiwRecipeForProcessing.Size];
                        }

                        m_Server.SeqFlag.RecipeCheckResult = IsAvailable(data) ? RecipeCheck.Ok : RecipeCheck.Ng;
                        m_IfHandshakeToCim.MowRecipeBodyChkResult.SetValue((short)m_CheckResult);
                        m_IfHandshakeToCim.MobRecipeBodyChkReply.SetStatus(true);
                        StartTicks = XFunc.GetTickCount();
                        //m_Server.SetInterfaceLog(SeqFunName, 0, 0, data.ToString());
                        nSeqNo = 20;

                        if(m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibRecipeBodyReply.SetStatus(false);
                        }

                        if (m_Server.SeqFlag.RecipeCheckResult == RecipeCheck.Ok)
                        {
                            if (!m_Simul.Melsec)
                            {
                                TagRecipe recipe = new TagRecipe(data);
                                m_Server.RecvRecipeBody = recipe;
                            }
                            else
                            {
                                m_Server.RecvRecipeBody = m_Server.JobCond.CurrentRecipe;
                            }
                        }
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_ReplyOnTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Set Alarm(case 10)");
                        nSeqNo = 1000;
                    }
                    break;
                case 20:
                    if (!m_IfHandshakeFromCim.MibRecipeBodyReply.GetStatus())
                    {
                        m_IfHandshakeToCim.MobRecipeBodyReq.SetStatus(false);
                        m_IfHandshakeToCim.MobRecipeBodyChkReply.SetStatus(false);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Finish");
                        m_Server.SeqFlag.RecipeBodyReqComp = true;
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_ReplyOffTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Set Alarm(case 20)");
                        nSeqNo = 2000;
                    }
                    break;
                case 1000:
                    if (m_IfHandshakeFromCim.MibRecipeBodyReply.GetStatus())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Reset Alarm(case 1000)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000:
                    if (!m_IfHandshakeFromCim.MibRecipeBodyReply.GetStatus())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recipe Body Request : Reset Alarm(case 2000)");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqApdReport : XSeqFunction            //Common Spec 6.4
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private short[] m_Data;
        private uint timeout = 4000;    //4sec
        //private CvUnit m_ReportCvUnit = eqpCvUnits._OUT_BUF2_CvUnit;
        private int m_Id;
        //private short m_Index = 0;
        #endregion

        #region Constructor
        public SeqApdReport(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatusToCim = sigEqpStatusToCim;

            if (m_Simul.Melsec)
            {
                m_Data = new short[64];
            }
            else
            {
                m_Data = new short[m_IfEqpStatusToCim.MowApdReportArea.Size];
            }
            StartTicks = XFunc.GetTickCount();
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Server.SeqFlag.ApdReportReq && (GetElapsedTicks() > timeout))
                    {
                        m_Server.SeqFlag.ApdReportReq = false;
                        m_Id = eqpCvUnits._OUT_BUF2_CvUnit.Id * 2 + 1;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Apd Report : Start");
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        //int i = 0;
                        //TagGlassData glassData = new TagGlassData();
                        //m_Server.DataProvider.GlassDataProvider.GetData(m_Id, ref glassData);
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Glass_Number_Code.Id, glassData.Item.GlassNumberCode.Code.ToString());
                        //glassData.Item.GlassStatusCode = StatusCodeFlag.PPCLN;
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Glass_Status_Code.Id, ((short)glassData.Item.GlassStatusCode).ToString());
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Recipe_ID.Id, "001");
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Process_Time.Id, "60");

                        //for CIM Test
                        //string date = DateTime.Now.ToString("yyMMddHHmmss");
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Glass_Loading_Time.Id, date);
                        //m_Server.ApdItemsHandler.SetData(m_Id, eqpApdItems._Glass_Unloading_Time.Id, date);

                        //for CIM Test
                        //foreach(CvUnit unit in CvControl.CvUnits)
                        //{
                        //    m_Server.ApdItemsHandler.SetData(m_Id, unit.Name);
                        //}

                        //int[] ldTime = new int[3];
                        //int[] ulTime = new int[3];
                        //string time = m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Glass_Loading_Time.Id);
                        //if (time.Length >= 12)
                        //{
                        //    ldTime[0] = Convert.ToInt16(time.Substring(0, 4));
                        //    ldTime[1] = Convert.ToInt16(time.Substring(4, 4));
                        //    ldTime[2] = Convert.ToInt16(time.Substring(8, 4));
                        //}
                        //time = m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Glass_Unloading_Time.Id);
                        //if (time.Length >= 12)
                        //{
                        //    ulTime[0] = Convert.ToInt16(time.Substring(0, 4));
                        //    ulTime[1] = Convert.ToInt16(time.Substring(4, 4));
                        //    ulTime[2] = Convert.ToInt16(time.Substring(8, 4));
                        //}

                        //m_Data[i++] = Convert.ToInt16(m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Glass_Number_Code.Id));
                        //m_Data[i++] = Convert.ToInt16(m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Glass_Status_Code.Id));
                        //m_Data[i++] = XFunc.ConvertToBcd(ldTime[0]);
                        //m_Data[i++] = XFunc.ConvertToBcd(ldTime[1]);
                        //m_Data[i++] = XFunc.ConvertToBcd(ldTime[2]);
                        //m_Data[i++] = XFunc.ConvertToBcd(ulTime[0]);
                        //m_Data[i++] = XFunc.ConvertToBcd(ulTime[1]);
                        //m_Data[i++] = XFunc.ConvertToBcd(ulTime[2]);
                        //m_Data[i++] = XFunc.ConvertToBcd(Convert.ToInt32(m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Recipe_ID.Id)));
                        //m_Data[i++] = Convert.ToInt16(m_Server.ApdItemsHandler.GetData(m_Id, eqpApdItems._Process_Time.Id));

                        //for (int j = eqpApdItems._AP_Unit_PCW_IN_Flow.Id; j < eqpApdItems._AK_Unit_Low_CDA_Filter_Used_Time.Id; j++)
                        //{
                        //    m_Data[i++] = (short)(Convert.ToDouble(m_Server.ApdItemsHandler.GetData(m_Id, j)) * 10);
                        //}

                        m_Server.ApdItemsHandler.SetLastData(m_Id);
                        m_Server.ApdItemsHandler.GetReportData(m_Id, ref m_Data);
                        m_IfEqpStatusToCim.MowApdReportArea.SetValues(m_Data);

                        m_Server.GenInfos.ApdReportIndex++;

                        if (m_Server.GenInfos.ApdReportIndex >= short.MaxValue)
                        {
                            m_Server.GenInfos.ApdReportIndex = 1;
                        }

                        m_IfEqpStatusToCim.MowApdReportIndex.SetValue(m_Server.GenInfos.ApdReportIndex);

                        //m_Index++;

                        //if (m_Index >= short.MaxValue)
                        //{
                        //    m_Index = 1;
                        //}

                        //m_IfEqpStatusToCim.MowApdReportIndex.SetValue(m_Index);

                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 0;

                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Server.GenInfos.ApdReportIndex.ToString());
                        //m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Index.ToString());
                        //m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Data.ToString());
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqGlassDataTransfer : XSeqFunction    //Common Spec 6.2
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private short[] m_data;
        private short[] m_OldData;
        private TagGlassData m_GlassData = new TagGlassData();
        private int m_Pos = 0;
        private int m_DataCount;
        #endregion

        #region Constructor
        public SeqGlassDataTransfer(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatusToCim = sigEqpStatusToCim;

            if (m_Simul.Melsec)
            {
                m_data = new short[32];
                m_OldData = new short[32];
            }
            else
            {
                m_data = new short[m_IfEqpStatusToCim.MowGlassDataTransfer.Size];
                m_OldData = new short[m_IfEqpStatusToCim.MowGlassDataTransfer.Size];
            }

            m_DataCount = m_data.Length;
        }
        #endregion

        #region Methods
        private bool IsContained(int key, int[] container)
        {
            foreach (int i in container)
            {
                if (key == i) return true;
            }

            return false;
        }
        private ulong oldPositionFlag = 0;
        public override int Do()
        {
            ulong positionFlag = m_Server.GlassData.GetPositionFlag();
            bool changed = (oldPositionFlag ^ positionFlag) > 0 ? true : false;
            if (changed)
            {
                oldPositionFlag = positionFlag;

                //먼저 clear하고
                //m_data.Initialize();
                for (int i = 0; i < m_DataCount; i++)
                {
                    m_data[i] = 0;
                }
                m_Pos = 0;

                //data읽어와서
                int[] ids;
                m_Server.GlassData.GetAllPositionId(out ids);
                
                foreach (int id in ids)
                {
                    if (id % 2 > 0)
                    {
                        if (IsContained(id/2, ids)) continue;
                    }

                    m_Server.DataProvider.GlassDataProvider.GetData(id, ref m_GlassData);

                    m_Pos = id / 2;
                    if (m_Pos >= eqpCvUnits._LD_CV2_2_CvUnit.Id) m_Pos--;   //CV2_1 and CV2_2 are one unit. 
                    m_data[m_Pos] = (short)m_GlassData.Item.GlassNumberCode.Code;
                }
                m_IfEqpStatusToCim.MowGlassDataTransfer.SetValues(m_data);
            }
            return -1;

            //int i = 0;
            //foreach (CvUnit unit in CvControl.CvUnits)
            //{
            //    if (m_Server.DataProvider.GlassDataProvider.GetData(unit.Id * 2, ref m_GlassData) ||
            //        m_Server.DataProvider.GlassDataProvider.GetData(unit.Id * 2 + 1, ref m_GlassData))
            //    {
            //        m_data[i] = (short)m_GlassData.Item.GlassNumberCode.Code;
            //    }
            //    else
            //    {
            //        m_data[i] = 0;
            //    }

            //    if (unit.Name != eqpCvUnits._LD_CV2_1_CvUnit_Name) i++;  //CV2_1 and CV2_2 are one unit

            //}

            //m_IfEqpStatusToCim.MowGlassDataTransfer.SetValues(m_data);
            //return -1;
        }
        #endregion
    }

    public class SeqGlassCount : XSeqFunction           //Common Spec 5.2.3 & 6.1
    {
        #region Fields
        private ServerManager m_Server;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private short m_OldMax = -1;
        private short m_OldCur = -1;
        private short m_PosCount = -1;
        private bool m_IsChange = false;
        //private string m_Msg;
        #endregion

        #region Constructor
        public SeqGlassCount(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int maxCount = m_Server.SetupMaxGlassNo.GetValue<int>() + 2;
            int curCount = m_Server.GenInfos.EQPGlassCount;

            if (m_OldMax != maxCount)
            {
                m_OldMax = (short)maxCount;
                m_IfEqpStatusToCim.MowMaxGlassCount.SetValue(m_OldMax);
                //m_Msg = string.Format("Max Glass Count : {0}", m_OldMax);
                //m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                m_IsChange = true;
            }

            if (m_OldCur != curCount)
            {
                m_OldCur = (short)curCount;
                m_IfEqpStatusToCim.MowCurGlassCount.SetValue(m_OldCur);
                //m_Msg = string.Format("Current Glass Count : {0}", m_OldCur);
                //m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                m_IsChange = true;
            }

            if (m_IsChange)
            {
                m_IsChange = false;
                m_PosCount = (short)(m_OldMax - m_OldCur);
                m_IfEqpStatusToCim.MowPossibleGlassCount.SetValue(m_PosCount);
                //m_Msg = string.Format("Possible Glass Count : {0}", m_PosCount);
                //m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
            }

            return -1;
        }
        #endregion
    }

    public class SeqSupplyEnable : XSeqFunction         //Common Spec 5.2.1 & 5.2.2
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private int m_State = -1;
        #endregion

        #region Constructor
        public SeqSupplyEnable(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
            m_IfHandshakeFromCim = sigHandshakeFromCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if ((m_State != 0) && (m_Server.GenInfos.CycleStop || !m_IfHandshakeFromCim.MibSupplyEnableReq.GetStatus()))
            {
                m_IfEqpStatusToCim.MobSupplyEnable.SetStatus(false);
                m_State = 0;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Supply Enable : False");
            }
            else if ((m_State != 1) && (!m_Server.GenInfos.CycleStop && m_IfHandshakeFromCim.MibSupplyEnableReq.GetStatus()))
            {
                m_IfEqpStatusToCim.MobSupplyEnable.SetStatus(true);
                m_State = 1;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Supply Enable : True");
            }

            return -1;
        }
        #endregion
    }

    public class SeqGlassDataLostReq : XSeqFunction     //Common Spec 5.2.6
    {
        #region Fields
        protected static ServerManager m_Server;
        private Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private IfSigHandshakeToCim m_IfHandshakeToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private TagGlassData m_GlassData = new TagGlassData();
        private uint timeOut = 4000;
        private Alarm ALM_LostGlassReplyOnTimeout;
        private string m_Msg;
        private ThreadCvControl m_CvControl;
        #endregion

        #region Constructor
        public SeqGlassDataLostReq(string seqName, IfSigHandshakeToCim sigHandshakeToCim, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName; 

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_CvControl = m_Server.ThreadHandler.CvControl;

            m_IfHandshakeToCim = sigHandshakeToCim;
            m_IfHandshakeFromCim = sigHandshakeFromCim;

            ALM_LostGlassReplyOnTimeout = new Alarm("Lost Glass Reply On Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            { 
                case 0:
                    if (m_Server.SeqFlag.GlassDataLostReq)
                    {
                        m_Server.SeqFlag.GlassDataLostReq = false;
                        m_Server.DataProvider.GlassDataProvider.GetData(m_Server.SeqFlag.LostGlassId, ref m_GlassData);
                        m_IfHandshakeToCim.MowLostGlassInfo.SetValue((short)m_GlassData.Item.GlassNumberCode.Code);
                        m_IfHandshakeToCim.MobLostGlassReq.SetStatus(true);
                        m_Msg = string.Format("Lost Glass Req : {0}", m_GlassData.Item.GlassNumberCode.Code);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;

                        if (m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibLostGlassReply.SetStatus(true);
                        }
                    }
                    break;
                case 10:
                    if (m_IfHandshakeFromCim.MibLostGlassReply.GetStatus() || m_Simul.Cim)
                    {
                        short reply = m_IfHandshakeFromCim.MiwLostGlassCommand.GetValue();

                        if (m_Simul.Melsec || m_Simul.Cim)
                        {
                            reply = (short)1;
                        }

                        if (reply == 1)
                        {
                            m_Server.GlassData.Delete(m_Server.SeqFlag.LostGlassId);
                            m_Server.DataProvider.LostGlassDataProvider.Create(m_GlassData);

                            m_CvControl.InitParameter();

                            if (m_Simul.Device)
                            {
                                foreach (CvUnit unit in ThreadCvControl.Units)
                                {
                                    unit.GlsInSensor.DiSensor.SetState(false);
                                    unit.GlsOutSensor.DiSensor.SetState(false);
                                }

                                eqpSensors._LD_CV2_1_Unit_Dec_Sensor.DiSensor.SetState(false);
                                eqpSensors._LD_CV2_1_Unit_Intr_Sensor.DiSensor.SetState(false);
                                eqpSensors._LD_CV7_Unit_Dec_Sensor.DiSensor.SetState(false);
                                eqpSensors._OUT_BUF2_Unit_Intr_Sensor.DiSensor.SetState(false);
                            }

                            _GenericCollection<GlsSensor> sensors = m_Server.ComponentContainer.GetCollection<GlsSensor>();
                            m_Msg = string.Format("Glass data delete  : {0}", sensors[m_Server.SeqFlag.LostGlassId].Name);
                            m_Server.Log(m_Msg);
                        }
                        m_IfHandshakeToCim.MobLostGlassReq.SetStatus(false);
                        m_Msg = string.Format("Lost Glass Reply : {0}", reply == 1 ? "OK" : "NG");
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_LostGlassReplyOnTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Lost Glass Req : Set Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (m_IfHandshakeFromCim.MibLostGlassReply.GetStatus())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Lost Glass Req : Reset Alarm");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqGlassDataRecoveryReq : XSeqFunction //Common Spec 5.2.7
    {
        #region Fields
        protected static ServerManager m_Server;
        private Simul m_Simul;
        protected static IEqpManager m_EqpManager;
        private IfSigHandshakeToCim m_IfHandshakeToCim;
        private IfSigHandshakeFromCim m_IfHandshakeFromCim;
        private TagGlassData m_GlassData = new TagGlassData();
        private uint timeOut = 4000;
        private Alarm ALM_RecoveryGlassReplyOnTimeout;
        private string m_Msg;
        private ThreadCvControl m_CvControl;
        #endregion

        #region Constructor
        public SeqGlassDataRecoveryReq(string seqName, IfSigHandshakeToCim sigHandshakeToCim, IfSigHandshakeFromCim sigHandshakeFromCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = m_Server.EqpStateManager;
            m_CvControl = m_Server.ThreadHandler.CvControl;

            m_IfHandshakeToCim = sigHandshakeToCim;
            m_IfHandshakeFromCim = sigHandshakeFromCim;

            ALM_RecoveryGlassReplyOnTimeout = new Alarm("Recovery Glass Reply On Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_Server.SeqFlag.GlassDataRecoveryReq)
                    {
                        m_Server.SeqFlag.GlassDataRecoveryReq = false;
                        m_Server.DataProvider.LostGlassDataProvider.GetData(m_Server.SeqFlag.RecoveryGlassNo, ref m_GlassData);
                        m_IfHandshakeToCim.MowRecoverGlassInfo.SetValue((short)m_GlassData.Item.GlassNumberCode.Code);
                        m_IfHandshakeToCim.MobRecoverGlassReq.SetStatus(true);
                        m_Msg = string.Format("Recovery Glass Req : {0}", m_GlassData.Item.GlassNumberCode.Code);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 10;

                        if (m_Simul.Melsec)
                        {
                            m_IfHandshakeFromCim.MibRecoverGlassReply.SetStatus(true);
                        }
                    }
                    break;
                case 10:
                    if (m_IfHandshakeFromCim.MibRecoverGlassReply.GetStatus())
                    {
                        short reply = m_IfHandshakeFromCim.MiwRecoverGlassCommand.GetValue();

                        if (m_Simul.Melsec)
                        {
                            reply = (short)1;
                        }

                        if (reply == 1)
                        {
                            m_GlassData.PositionId = m_Server.SeqFlag.RecoveryGlassPos;
                            m_Server.GlassData.Create(m_GlassData);
                            m_Server.DataProvider.LostGlassDataProvider.Delete(m_Server.SeqFlag.RecoveryGlassNo);

                            m_CvControl.InitParameter();

                            if (m_Simul.Device)
                            {
                                //foreach (CvUnit unit in CvControl.CvUnits)
                                //{
                                //    if (unit.Id == (m_Server.SeqFlag.RecoveryGlassId / 2))
                                //    {
                                //        unit.GlsInSensor.DiSensor.SetState(true);
                                //        unit.GlsOutSensor.DiSensor.SetState(true);
                                //        break;
                                //    }
                                //}
                                ThreadCvControl.Units[m_Server.SeqFlag.RecoveryGlassPos / 2].GlsInSensor.DiSensor.SetState(true);
                                ThreadCvControl.Units[m_Server.SeqFlag.RecoveryGlassPos / 2].GlsOutSensor.DiSensor.SetState(true);
                            }
                            m_Msg = string.Format("Glass data Recovery  : {0}", ThreadCvControl.Units[m_Server.SeqFlag.RecoveryGlassPos / 2].Name);
                            m_Server.Log(m_Msg);
                        }
                        m_IfHandshakeToCim.MobRecoverGlassReq.SetStatus(false);
                        m_Msg = string.Format("Recovery Glass Reply : {0}", reply == 1 ? "OK" : "NG");
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_Msg);
                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > timeOut)
                    {
                        m_AlarmId = ALM_RecoveryGlassReplyOnTimeout.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_ReturnSeqNo = nSeqNo;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recovery Glass Req : Set Alarm");
                        nSeqNo = 1000;
                    }
                    break;
                case 1000:
                    if (m_IfHandshakeFromCim.MibRecoverGlassReply.GetStatus())
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recovery Glass Req : Reset Alarm");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqSendPause : XSeqFunction            //Common Spec 5.2.9
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private EqpManager m_EqpManager;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private int m_State = -1;
        #endregion

        #region Constructor
        public SeqSendPause(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = EqpManager.Instance;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if ((m_State != 0) && (m_Server.GenInfos.SendPause == false))
            {
                m_IfEqpStatusToCim.MobSendPause.SetStatus(false);
                m_State = 0;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Send Pause : False");
            }
            else if ((m_State != 1) && (m_Server.GenInfos.SendPause == true))
            {
                m_IfEqpStatusToCim.MobSendPause.SetStatus(true);
                m_State = 1;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Send Pause : True");
            }

            return -1;
        }
        #endregion
    }

    public class SeqProcessPause : XSeqFunction         //Common Spec 5.2.10
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private EqpManager m_EqpManager;
        private IfSigEqpStatusToCim m_IfEqpStatusToCim;
        private int m_State = -1;
        #endregion

        #region Constructor
        public SeqProcessPause(string seqName, IfSigEqpStatusToCim sigEqpStatusToCim)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = m_Server.Simul;
            m_EqpManager = EqpManager.Instance;

            m_IfEqpStatusToCim = sigEqpStatusToCim;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            if ((m_State != 0) && (m_Server.GenInfos.ProcessPause == false))
            {
                m_IfEqpStatusToCim.MobProcessPause.SetStatus(false);
                m_State = 0;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Process Pause : False");
            }
            else if ((m_State != 1) && (m_Server.GenInfos.ProcessPause == true))
            {
                m_IfEqpStatusToCim.MobProcessPause.SetStatus(true);
                m_State = 1;
                m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Process Pause : True");
            }

            return -1;
        }
        #endregion
    }
}

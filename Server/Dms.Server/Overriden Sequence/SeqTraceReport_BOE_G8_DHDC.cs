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
using System.Net;
using System.Net.Sockets;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class ThreadTraceReport_BOE_G8_DHDC : XSequence
    {
        #region Fields
        protected static ServerManager m_Server;
        private XApdLog m_TraceDataLog;
        private XLog m_TcpLog = new XLog("TcpLog", XLog.LogStampType.UseStamp);
        private ApdItemsHandler m_ApdItemsHandler;
        private TcpStreamQueue m_TraceQueue = new TcpStreamQueue();

        private bool m_TraceReportMelsec = false;
        private bool m_TraceReportTcpIp = false;
        #endregion

        #region Properties
        public XApdLog TraceDataLog
        {
            get { return m_TraceDataLog; }
            set { m_TraceDataLog = value; }
        }
        public ApdItemsHandler ApdItemsHandler
        {
            get { return m_ApdItemsHandler; }
            set { m_ApdItemsHandler = value; }
        }
        public TcpStreamQueue TraceQueue
        {
            get { return m_TraceQueue; }
            set { m_TraceQueue = value; }
        }

        public bool TraceReportMelsec
        {
            get { return m_TraceReportMelsec; }
            set { m_TraceReportMelsec = value; }
        }
        public bool TraceReportTcpIp
        {
            get { return m_TraceReportTcpIp; }
            set { m_TraceReportTcpIp = value; }
        }
        #endregion

        #region Constructor
        public ThreadTraceReport_BOE_G8_DHDC(int scanTime, ServerManager server) : base(scanTime)
        {
            m_Server = server;
            m_TraceDataLog = new XApdLog(XApdLog.ApdType.TPD);
            m_ApdItemsHandler = m_Server.ApdItemsHandler;

            m_TraceReportTcpIp = eqpEquipmentTypes._Equipment_Type.TraceTransferMethod == EquipmentType.TraceTransferType.TCPIP;
            m_TraceReportMelsec = eqpEquipmentTypes._Equipment_Type.TraceTransferMethod == EquipmentType.TraceTransferType.MELSEC;

            RegisterSequences();
        }
        #endregion

        #region RegisterSequence
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqTraceDataMonitor(this));

            if (m_TraceReportTcpIp)
            {
                RegisterSequence(new SeqTraceClient(this));
            }
            if (m_TraceReportMelsec)
            {
                RegisterSequence(new SeqTraceReport(this));
            }
        }
        #endregion

        #region Override
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
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);

                if (AppConfig.Instance.Simul.Device)
                {
                    MessageBox.Show(msg);
                }
            }
        }
        #endregion

        #region Methods
        public void SetTraceLog(string log)
        {
            m_TraceDataLog.TextOut(log);
        }

        public void SetLog(string seqName, string message)
        {
            string log = string.Format("{0}\t{1}", seqName, message);
            m_TcpLog.TextOut(log);
        }
        #endregion
    }

    public class SeqTraceDataMonitor : XSeqFunction
    {
        #region Enum
        private enum IntervalTicks { Sampling, FileSave };
        #endregion

        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;

        private ThreadTraceReport_BOE_G8_DHDC m_Control;
        private ApdItems m_ApdItems;
        private ApdItemsHandler m_ApdItemsHandler;
        private int m_SamplingInterval;
        private int m_FileSaveInterval;

        private int m_Count;
        private int m_TraceWordCount = 0;
        private ushort[] m_TraceDatas;

        // dspcrassus - 120214 : Trace APD Item Value Converting Exception
        private int m_TraceApdItemsIndex = 0;
        private int m_TraceApdItemsType = 0;
        private string m_TraceApdItemsName = "";
        private string m_TraceApdItemsValueOrg = "";
        private string m_TraceApdItemsValueCvt = "";

        #endregion

        #region Constructor
        public SeqTraceDataMonitor(ThreadTraceReport_BOE_G8_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            m_ApdItemsHandler = m_Control.ApdItemsHandler;
            m_ApdItemsHandler.AddNewItem(-2);
            m_ApdItems = m_ApdItemsHandler.GetItems(-2);

            for (int i = 0; i < m_ApdItems.Count; i++)
            {
                if (m_ApdItems.Items[i].TpdReportEnable &&
                   (m_TraceWordCount < m_ApdItems.Items[i].StartTpdAddress + m_ApdItems.Items[i].WordCount))
                    m_TraceWordCount = m_ApdItems.Items[i].StartTpdAddress + m_ApdItems.Items[i].WordCount;
            }
            m_TraceDatas = new ushort[m_TraceWordCount];

            SetExtraStartTicks(2);

            m_SeqFunName = "TraceMonitor";
        }
        #endregion

        #region Override
        public override int Do()
        {
            int rv = -1;
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    {
                        try
                        {
                            m_Control.TraceDataLog.NewFile(m_Server.SetupTpdEqpName.Val);
                            m_Control.SetLog(m_SeqFunName, "Trace Data Log File Create");

                            m_SamplingInterval = m_Server.SetupTpdSamplingTime.GetValue<int>() * 1000;
                            m_FileSaveInterval = m_Server.SetupTpdFileSaveTime.GetValue<int>() * 60 * 1000;

                            string data = "";
                            m_Count = 0;
                            // dspcrassus - Write TPD Title
                            GetTraceData(m_Count++);

                            // dspcrassus - Make Data of TPD
                            m_ApdItemsHandler.SetTpdData();
                            DateTime datetime = DateTime.Now;
                            string time = string.Format("{0:D04}{1:D02}{2:D02}{3:D02}{4:D02}{5:D02}",
                                datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
                            data = time;

                            data += GetTraceData(m_Count++);
                            m_Control.TraceQueue.AddTrace(data);
                            seqNo = 10;
                        }
                        catch (Exception err)
                        {
                            // dspcrassus - 120214 : Exception Trace Log
                            string msg = string.Format("INDEX: {0}, NAME: {1}, TYPE: {2}, VALUE(ORG): {3}, VALUE(CVT): {4}",
                                                  m_TraceApdItemsIndex, m_TraceApdItemsName, m_TraceApdItemsType, m_TraceApdItemsValueOrg, m_TraceApdItemsValueCvt);
                            m_Server.WriteExceptionLog(msg);
                            ////////////////////////////////////////////

                            XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);
                            m_StartTicks = XFunc.GetTickCount();
                            m_ReturnSeqNo = seqNo;
                            seqNo = 100;
                        }
                    }
                    break;
                case 10:
                    {
                        try
                        {
                            if (GetElapsedTicks((int)IntervalTicks.Sampling) > m_SamplingInterval)
                            {
                                m_ApdItemsHandler.SetTpdData();
                                DateTime datetime = DateTime.Now;
                                string time = string.Format("{0:D04}{1:D02}{2:D02}{3:D02}{4:D02}{5:D02}",
                                    datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
                                string data = time;

                                data += GetTraceData(m_Count++);
                                m_Control.TraceQueue.AddTrace(data);

                                m_ExtraStartTicks[(int)IntervalTicks.Sampling] = XFunc.GetTickCount();
                                seqNo = 10;
                            }
                            else if (GetElapsedTicks((int)IntervalTicks.FileSave) > m_FileSaveInterval)
                            {
                                m_Control.TraceDataLog.CloseStream();
                                m_Control.SetLog(m_SeqFunName, "Trace Data Log File Closed");

                                m_ExtraStartTicks[(int)IntervalTicks.FileSave] = XFunc.GetTickCount();
                                seqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            // dspcrassus - 120214 : Exception Trace Log
                            string msg = string.Format("INDEX: {0}, NAME: {1}, TYPE: {2}, VALUE(ORG): {3}, VALUE(CVT): {4}",
                                                  m_TraceApdItemsIndex, m_TraceApdItemsName, m_TraceApdItemsType, m_TraceApdItemsValueOrg, m_TraceApdItemsValueCvt);
                            m_Server.WriteExceptionLog(msg);
                            ////////////////////////////////////////////

                            XFunc.ExceptionHandler.Add(err, ExceptionLevel.Log);
                            m_StartTicks = XFunc.GetTickCount();
                            m_ReturnSeqNo = seqNo;
                            seqNo = 100;
                        }
                    }
                    break;
                case 100:
                    if (GetElapsedTicks() > 1000)
                    {
                        seqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return rv;
        }
        #endregion

        #region Method
        public string GetTraceData(int nCount)
        {
            string log = "";
            string logTemp = "";
            string data = "";
            string unitName = ""; // 09.12.01 minhan
                                  //            int indexCount = 0; // 09.12.01 minhan

            //2010.04.08 Youngsik...
            //GetItems가 참조 타입에서 Clone 값을 Return하기 때문에 매번 GetItems를 호출해야 한다.
            m_ApdItems = m_ApdItemsHandler.GetItems(-2);

            if (nCount == 0)
            {
                if (m_ApdItems != null)
                {
                    log = "[TIME]";
                    foreach (ApdItem item in m_ApdItems.Items)
                    {
                        if (item.TpdReportEnable == true)
                        {
                            unitName = item.ItemUnit.ToString();
                            logTemp = string.Format("\t[{0}({1})]", item.Name, unitName); // 9.12.01 minhan
                            log += logTemp;
                        }
                    }

                    // dspcrassus - Trace Log를 이곳에서 작성
                    m_Control.SetTraceLog(log);
                }
            }
            else
            {
                if (m_ApdItems != null) // 9.12.01 minhan '을 `로 다 수정
                {
                    DateTime datetime = DateTime.Now;
                    string time = string.Format("{0:D04}{1:D02}{2:D02}{3:D02}{4:D02}{5:D02}",
                        datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
                    log = time;

                    // dspcrassus - 120214 : Trace APD Item Value Converting Exception
                    m_TraceApdItemsIndex = 0;

                    foreach (ApdItem item in m_ApdItems.Items)
                    {
                        if (item.TpdReportEnable == true)
                        {
                            if (item.ReferenceTagDescriptor != null)
                            {
                                logTemp = string.Format("{0:F01}", item.Value);
                                data += " " + logTemp;
                                log += "\t" + logTemp;

                                if (m_Control.TraceReportMelsec)
                                {
                                    string traceBuffer = "";
                                    int traceValue = 0;
                                    int count = logTemp.IndexOf('.');

                                    if (count >= 0)
                                    {
                                        traceBuffer = logTemp.Substring(0, count);
                                        // dspcrassus - 120214 : Tracing
                                        m_TraceApdItemsName = item.Name;
                                        m_TraceApdItemsType = 1;
                                        m_TraceApdItemsValueOrg = item.Value;
                                        m_TraceApdItemsValueCvt = traceBuffer;
                                        /////////////////////////////////
                                        traceValue = Convert.ToInt32(traceBuffer);
                                        if (traceValue >= 0)
                                        {
                                            traceBuffer = string.Format("{0:d4}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = logTemp.Substring(count + 1, 1);//120215
                                            // dspcrassus - 120214 : Tracing
                                            m_TraceApdItemsType = 1;
                                            m_TraceApdItemsValueCvt = traceBuffer;
                                            /////////////////////////////////
                                            traceValue = Convert.ToInt32(traceBuffer);
                                            traceBuffer = string.Format("{0:d2}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                        else
                                        {
                                            traceBuffer = "0000";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                    }
                                    else
                                    {
                                        // dspcrassus - 120214 : Tracing
                                        m_TraceApdItemsName = item.Name;
                                        m_TraceApdItemsType = 1;
                                        m_TraceApdItemsValueOrg = item.Value;
                                        m_TraceApdItemsValueCvt = item.Value;
                                        /////////////////////////////////
                                        traceValue = Convert.ToInt32(item.Value);
                                        if (traceValue >= 0) // 11.04.16 minhan
                                        {
                                            traceBuffer = string.Format("{0:d4}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceValue = 0;
                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                        else
                                        {
                                            traceBuffer = "0000";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                    }
                                }
                            }
                            else    // dspcrassus - No Referencial Items
                            {
                                #region No Referencial Items
                                // dspcrassus - 111223 : 이건 뭐... 애초에 HPMJ 관련 U/T를 Gauge 것으로 사용했으면 이런 개노가다 필요없잖오 ㅡㅡa
                                double value = 0.0;
                                if (item.Name == eqpApdItems._HJ_CO2_PRS.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.CO2InPress.CurAdc / 1000;
                                }
                                else if (item.Name == eqpApdItems._HJ_DI_PRS.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.MainDiPress.CurAdc / 1000;
                                }
                                else if (item.Name == eqpApdItems._HJ_DI_FLW.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.HpmjFlow.CurAdc / 100;
                                }
                                else if (item.Name == eqpApdItems._HJ_RES.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.Resistivity.CurAdc / 100;
                                }
                                else if (item.Name == eqpApdItems._HJ_IN_PRS.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.FilterInPress.CurAdc / 10;
                                }
                                else if (item.Name == eqpApdItems._HJ_OUT_PRS.Name)
                                {
                                    value = (double)eqpHpmjs._HPMJ_Unit.FilterOutPress.CurAdc / 10;
                                }
                                //else if(item.Name == eqpApdItems._HJ_PRS_DEV.Name)
                                //{
                                //    value = (double)eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge.CurAdc / 10;
                                //}
                                else if (item.Name == eqpApdItems._RB_RB1_SPD.Name)
                                {
                                    value = eqpRbMotors._RB_Unit_Up_RbMotor1.GetCurSpeed();
                                }
                                else if (item.Name == eqpApdItems._RB_RB2_SPD.Name)
                                {
                                    value = eqpRbMotors._RB_Unit_Lo_RbMotor1.GetCurSpeed();
                                }
                                else if (item.Name == eqpApdItems._RB_RB3_SPD.Name)
                                {
                                    value = eqpRbMotors._RB_Unit_Up_RbMotor2.GetCurSpeed();
                                }
                                else if (item.Name == eqpApdItems._RB_RB4_SPD.Name)
                                {
                                    value = eqpRbMotors._RB_Unit_Lo_RbMotor2.GetCurSpeed();
                                }

                                logTemp = string.Format("{0:F01}", value);
                                data += " " + logTemp;
                                log += "\t" + logTemp;

                                if (m_Control.TraceReportMelsec)
                                {
                                    string traceBuffer = "";
                                    int traceValue = 0;
                                    int count = logTemp.IndexOf('.');

                                    if (count >= 0)
                                    {
                                        traceBuffer = logTemp.Substring(0, count);
                                        // dspcrassus - 120214 : Tracing
                                        m_TraceApdItemsName = item.Name;
                                        m_TraceApdItemsType = 1;
                                        m_TraceApdItemsValueOrg = logTemp;
                                        m_TraceApdItemsValueCvt = traceBuffer;
                                        /////////////////////////////////
                                        traceValue = Convert.ToInt32(traceBuffer);
                                        if (traceValue >= 0)
                                        {
                                            traceBuffer = string.Format("{0:d4}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = logTemp.Substring(count + 1, 1);//120215
                                            // dspcrassus - 120214 : Tracing
                                            m_TraceApdItemsType = 1;
                                            m_TraceApdItemsValueCvt = traceBuffer;
                                            /////////////////////////////////
                                            traceValue = Convert.ToInt32(traceBuffer);
                                            traceBuffer = string.Format("{0:d2}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                        else
                                        {
                                            traceBuffer = "0000";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartTpdAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                    }
                                    else
                                    {
                                        // dspcrassus - 120214 : Tracing
                                        m_TraceApdItemsName = item.Name;
                                        m_TraceApdItemsType = 1;
                                        m_TraceApdItemsValueOrg = item.Value;
                                        m_TraceApdItemsValueCvt = item.Value;
                                        /////////////////////////////////
                                        traceValue = Convert.ToInt32(item.Value);
                                        if (traceValue >= 0) // 11.04.16 minhan
                                        {
                                            traceBuffer = string.Format("{0:d4}", traceValue);
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceValue = 0;
                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                        else
                                        {
                                            traceBuffer = "0000";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            traceBuffer = "00";
                                            XFunc.ConvertToWord(traceBuffer, ref m_TraceDatas, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        m_TraceApdItemsIndex++;
                    }

                    // dspcrassus - Trace Log를 이곳에서 작성
                    m_Control.SetTraceLog(log);

                    if (m_Control.TraceReportMelsec)
                    {
                        eqpBOELoaderInterfaces._LoaderInterface.mowTraceDataForCleaner.SetStates(m_TraceDatas, 0, m_TraceWordCount);
                    }
                }
            }

            return data;
        }
        #endregion
    }

    public class SeqTraceReport : XSeqFunction
    {
        #region Fields
        protected static ThreadTraceReport_BOE_G8_DHDC m_Control;
        protected static ServerManager m_Server;
        protected static Simul m_Simul;

        private IoDigitalInput m_TraceReadAck = null;
        private IoDigitalOutput m_TraceReadRequest = null;

        private Alarm ALM_TraceReadTimeout = null;
        #endregion

        #region Constructor
        public SeqTraceReport(ThreadTraceReport_BOE_G8_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            m_TraceReadAck = eqpBOELoaderInterfaces._LoaderInterface.mibTraceDataReadAck;
            m_TraceReadRequest = eqpBOELoaderInterfaces._LoaderInterface.mobTraceDataReadRequest;

            ALM_TraceReadTimeout = new Alarm("Trace Data Read-Ack Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_SeqFunName = "TraceRpt";
        }
        #endregion

        #region Override
        public override int Do()
        {
            int rv = -1;
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    {
                        // dspcrassus - SPT측과 사양협의가 된 뒤에 아래코드에서 선택...
                        // 1. Request와 ACK를 주고 받는 경우 - 아래 주석부분 해제
                        // seqNo = 10;


                        // 2. Request만으로 On/Off 하는 경우 - 아래 주석부분 해제
                        // seqNo = 100;

                        // 3. SPT측에서 별도의 I/F를 필요로 하지 않을 경우 - 그냥 냅두면 됨
                    }
                    break;
                case 10:
                    {
                        string trace = m_Control.TraceQueue.GetTrace();
                        if (trace.Length > 0)
                        {
                            m_TraceReadRequest.SetState(true);
                            m_Control.SetLog(m_SeqFunName, "Trace Read Request On");

                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                    }
                    break;
                case 20:
                    if (m_TraceReadAck.GetState())
                    {
                        m_Control.SetLog(m_SeqFunName, "Trace Read Ack On");

                        m_TraceReadRequest.SetState(false);
                        m_Control.SetLog(m_SeqFunName, "Trace Read Request Off");

                        m_StartTicks = XFunc.GetTickCount();
                        seqNo = 30;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_AlarmId = ALM_TraceReadTimeout.Id;
                        m_Server.EqpStateManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, "Trace Data Read Timeout Set");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 1000;
                    }
                    break;
                case 30:
                    if (!m_TraceReadAck.GetState())
                    {
                        m_Control.SetLog(m_SeqFunName, "Trace Read Ack Off");
                        seqNo = 0;
                    }
                    else if (GetElapsedTicks() > 5000)
                    {
                        m_AlarmId = ALM_TraceReadTimeout.Id;
                        m_Server.EqpStateManager.SetAlarm(m_AlarmId);
                        m_Control.SetLog(m_SeqFunName, "Trace Data Read Timeout Set");
                        m_ReturnSeqNo = seqNo;
                        seqNo = 2000;
                    }
                    break;
                case 100:
                    {
                        string trace = m_Control.TraceQueue.GetTrace();
                        if (trace.Length > 0)
                        {
                            bool toggle = m_TraceReadRequest.GetState();
                            m_TraceReadRequest.SetState(!toggle);
                            m_Control.SetLog(m_SeqFunName, string.Format("Trace Read Request {0}", toggle.ToString()));
                        }
                    }
                    break;
                case 1000:
                    if (m_Server.EqpStateManager.AlarmResetSwitchPushed || m_TraceReadAck.GetState())
                    {
                        m_Server.EqpStateManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, "Trace Data Read Timeout Reset");

                        if (m_TraceReadAck.GetState())
                            seqNo = m_ReturnSeqNo;
                        else
                            seqNo = 0;
                    }
                    break;
                case 2000:
                    if (m_Server.EqpStateManager.AlarmResetSwitchPushed || !m_TraceReadAck.GetState())
                    {
                        m_Server.EqpStateManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Control.SetLog(m_SeqFunName, "Trace Data Read Timeout Reset");

                        if (!m_TraceReadAck.GetState())
                            seqNo = m_ReturnSeqNo;
                        else
                            seqNo = 0;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return rv;
        }
        #endregion

        #region Method
        #endregion
    }

    public class SeqTraceClient : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static Simul m_Simul;

        private ThreadTraceReport_BOE_G8_DHDC m_Control;

        private static object m_LockKey = new object();
        private TcpClient m_TcpClient = new TcpClient();
        private NetworkStream m_TcpStream = null;
        private IPEndPoint m_TcpEndPoint;
        private int m_ServerPort;

        private bool m_IsConnectedSuccessful = false;
        private Exception m_SocketException;
        private ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        private bool m_StreamWriteWait = false;
        private bool m_LinkTestOk = false;
        #endregion

        #region Properties
        public int ServerPort
        {
            get { return m_ServerPort; }
            set { m_ServerPort = value; }
        }
        #endregion

        #region Constructor
        public SeqTraceClient(ThreadTraceReport_BOE_G8_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_Control = control;

            IPAddress address = IPAddress.Parse("127.0.0.1");
            m_ServerPort = 7000;
            m_TcpEndPoint = new IPEndPoint(address, m_ServerPort);

            m_SeqFunName = "TraceClient";
        }
        #endregion

        #region Override
        public override int Do()
        {
            int rv = -1;
            int seqNo = m_SeqNo;

            switch (seqNo)
            {
                case 0:
                    {
                        IPAddress address;
                        if (IPAddress.TryParse(m_Server.SetupTraceServerIp.Val, out address))
                        {
                            // TCP/IP Connection Try
                            m_TcpEndPoint.Address = address;
                            m_TcpEndPoint.Port = m_Server.SetupTraceServerPort.GetValue<int>();
                            m_Control.SetLog(m_SeqFunName, string.Format("Connection Server Setting : IP[{0}] PORT[{1}]", m_TcpEndPoint.Address.ToString(), m_TcpEndPoint.Port));

                            m_Control.SetLog(m_SeqFunName, "Trace Client is trying to connect to Server");
                            m_TcpClient = Connect(m_TcpEndPoint, 1000);
                            if (m_TcpClient.Client != null && m_TcpClient.Connected)
                            {
                                m_Control.SetLog(m_SeqFunName, "Trace Client is connected to Server");
                            }
                            else
                            {
                                m_Control.SetLog(m_SeqFunName, "Trace Client is not connected");
                            }

                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 10;
                        }
                    }
                    break;
                case 10:
                    if (m_TcpClient.Client != null && m_TcpClient.Connected)
                    {
                        m_TcpStream = m_TcpClient.GetStream();
                        m_TcpStream.ReadTimeout = 10;
                        m_TcpStream.WriteTimeout = 10;
                        m_Control.SetLog(m_SeqFunName, "Network Stream is created");

                        m_StreamWriteWait = false;
                        m_LinkTestOk = false;
                        if (WriteStream("[LINK_TEST]"))
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                        else
                        {
                            // dspcrassus - Stream Write 실패 시, 연결이 끊어진 것으로 간주한다.
                            CloseConnect();
                            seqNo = 0;
                        }
                    }
                    else if (GetElapsedTicks() > 10000)
                    {
                        // TCP/IP Connection Retry
                        bool configChanged = false;
                        configChanged |= m_Server.SetupTraceServerIp.Val != m_TcpEndPoint.Address.ToString();
                        configChanged |= (m_Server.SetupTraceServerPort.GetValue<int>() != m_TcpEndPoint.Port);

                        if (!configChanged)
                        {
                            m_TcpClient = Connect(m_TcpEndPoint, 1000);
                            if (m_TcpClient.Client != null && m_TcpClient.Connected)
                            {
                                m_Control.SetLog(m_SeqFunName, "Trace Client is connected to Server");
                            }

                            m_StartTicks = XFunc.GetTickCount();
                        }
                        else
                        {
                            seqNo = 0;
                        }
                    }
                    break;
                case 20:
                    {
                        string message = ReadStream();
                        if (message.Contains("[LINK_ACK]"))
                        {
                            m_Control.SetLog(m_SeqFunName, "RECV : [LINK_ACK]");
                            m_LinkTestOk = true;
                        }

                        if (m_StreamWriteWait && m_LinkTestOk)
                        {
                            // dspcrassus - 첫 연결 시에는 양측 모두 Link Test를 하므로, Server의 Link Test까지 확인한다.
                            m_StartTicks = XFunc.GetTickCount();
                            m_ReturnSeqNo = 100;
                            seqNo = 1000;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            // dspcrassus - 5초 이내에 Server, Client 양측 Link Test가 완료되지 않았다면, 문제가 있는것으로 판단하자.
                            CloseConnect();
                            seqNo = 0;
                        }
                    }
                    break;
                case 100:
                    {
                        // dspcrassus - SV_DATA를 쓰기 전에, Server에서 LinkTest를 보냈는지 먼저 확인한다.
                        ReadStream();
                        if (m_StreamWriteWait)
                        {
                            m_StartTicks = XFunc.GetTickCount();
                            m_ReturnSeqNo = seqNo;
                            seqNo = 1000;
                        }
                        else
                        {
                            string trace = m_Control.TraceQueue.GetTrace();
                            if (trace.Length > 0)
                            {
                                if (WriteStream(string.Format("[SV_DATA " + trace + "]")))
                                {
                                    m_StartTicks = XFunc.GetTickCount();
                                    seqNo = 110;
                                }
                                else
                                {
                                    // dspcrassus - Stream Write Fail
                                    CloseConnect();
                                    seqNo = 0;
                                }
                            }
                        }
                    }
                    break;
                case 110:
                    if (GetElapsedTicks() < 5000)
                    {
                        string message = ReadStream();
                        if (message.Contains("[SV_ACK]"))
                        {
                            m_Control.SetLog(m_SeqFunName, "RECV : [SV_ACK]");
                            seqNo = 100;
                        }
                    }
                    else
                    {
                        // dspcrassus - SV Data Read ACK Timeout : Client->Server 의 'Link Test'만 재시도 한다.
                        m_LinkTestOk = false;
                        if (WriteStream("[LINK_TEST]"))
                        {
                            m_StreamWriteWait = true;   // dspcrassus - Link 재연결 시, Server에서도 Link Test를 한다면, 이 부분은 빼야한다.
                            m_StartTicks = XFunc.GetTickCount();
                            seqNo = 20;
                        }
                        else
                        {
                            CloseConnect();
                            seqNo = 0;
                        }
                    }
                    break;
                case 1000:
                    if (GetElapsedTicks() > 100)
                    {
                        // dspcrassus - Server가 Link Test에 대한 Ack를 Confirm할 때까지 대기한다.
                        // 이놈이 졸라 바보라, Stream에 LinkAck와 다른 Data가 있으면, 구분을 못해... 문디들...
                        m_StreamWriteWait = false;
                        m_Control.SetLog(m_SeqFunName, "Trace Client wait server read [LINK_ACK]");

                        seqNo = m_ReturnSeqNo;
                    }
                    break;
            }

            m_SeqNo = seqNo;
            return rv;
        }
        #endregion

        #region Method
        private TcpClient Connect(IPEndPoint remoteEndPoint, int timeoutMSec)
        {
            TimeoutObject.Reset();
            m_SocketException = null;

            TcpClient tcpClient = new TcpClient();

            IPAddress ipCheck;
            if (!IPAddress.TryParse(remoteEndPoint.Address.ToString(), out ipCheck))
                return tcpClient;

            IAsyncResult asyncResult = tcpClient.BeginConnect(remoteEndPoint.Address, remoteEndPoint.Port, new AsyncCallback(ConnectAsyncCallBack), null);

            if (asyncResult.AsyncWaitHandle.WaitOne(timeoutMSec, false))
            {
                try
                {
                    tcpClient.EndConnect(asyncResult);
                    return tcpClient;
                }
                catch
                {
                    tcpClient.Close();
                    return tcpClient;
                }
            }
            else
            {
                tcpClient.Close();
                return tcpClient;
            }
        }

        private void ConnectAsyncCallBack(IAsyncResult asyncResult)
        {
            try
            {
                m_IsConnectedSuccessful = false;
                TcpClient tcpClient = asyncResult.AsyncState as TcpClient;

                if (tcpClient.Client != null)
                {
                    tcpClient.EndConnect(asyncResult);
                    m_IsConnectedSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                m_IsConnectedSuccessful = false;
                m_SocketException = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }

        private string ReadStream()
        {
            string stream = "";

            try
            {
                Byte[] data = new byte[256];
                m_TcpStream.Read(data, 0, data.Length);

                stream = System.Text.Encoding.ASCII.GetString(data).ToUpper();

                // dspcrassus - LinkTest 수신 시, 바로 Ack를 보내도록 한다.
                if (stream.Contains("[LINK_TEST]"))
                {
                    m_Control.SetLog(m_SeqFunName, "RECV : [LINK_TEST]");

                    if (WriteStream("[LINK_ACK]"))
                        m_StreamWriteWait = true;   // dspcrassus - Server에서 Ack를 온전히 읽어갈 때까지, 다음 Stream을 쓰는데 대기시간을 준다.
                }
            }
            catch
            {
                // dspcrassus - Stream Read 중 Exception은 무시 : Sequence 내에서 Timeout으로 대체
            }

            return stream;
        }

        private bool WriteStream(string message)
        {
            bool rv = false;
            string header = "";
            if (message.Contains("[SV_DATA]"))
                header = "[SV_DATA]";
            else
                header = message;

            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                m_TcpStream.Write(data, 0, data.Length);
                m_Control.SetLog(m_SeqFunName, string.Format("SEND : {0}", header));
                rv = true;
            }
            catch
            {
                m_Control.SetLog(m_SeqFunName, string.Format("SEND Fail : {0}", header));
                rv = false;
            }

            return rv;
        }

        private void CloseConnect()
        {
            m_TcpStream.Close();
            m_Control.SetLog(m_SeqFunName, "Network Stream is Closed");
            m_TcpClient.Close();
            m_Control.SetLog(m_SeqFunName, "Trace Client is Closed");
        }
        #endregion
    }

    public class TcpStreamQueue
    {
        #region Fields
        private static object m_LockKey = new object();
        private List<string> m_StreamList = new List<string>();
        #endregion

        #region Constructor
        public TcpStreamQueue()
        {
        }
        #endregion

        #region Method
        public void AddTrace(string stream)
        {
            lock (m_LockKey)
            {
                while (true)
                {
                    if (m_StreamList.Count >= 1)
                    {
                        m_StreamList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                m_StreamList.Add(stream);
            }
        }

        public void ClearTrace()
        {
            lock (m_LockKey)
            {
                while (m_StreamList.Count > 0)
                {
                    m_StreamList.RemoveAt(0);
                }
            }
        }

        public string GetTrace()
        {
            lock (m_LockKey)
            {
                string rv = "";
                if (m_StreamList.Count > 0)
                {
                    rv = m_StreamList[0];
                    m_StreamList.RemoveAt(0);
                }

                return rv;
            }
        }
        #endregion
    }
}
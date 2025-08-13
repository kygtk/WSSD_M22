using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Dms.Data;
using Dms.Device;
using Dms.Sequence;
using Dms.Common;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class SeqCimInterface_BOE_DOHC : XSequence
    {
        protected static ServerManager m_Server;
        private XLog ApddataLog; //2010.12.28  kang 보고되는 APD Data Log 남기고 History 에서 사용

        public SeqCimInterface_BOE_DOHC(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            ApddataLog = new XLog("GlassApdLog", XLog.LogStampType.UseStamp);
            RegisterSequences();
        }

        protected override void RegisterSequences()
        {

            RegisterSequence(new SeqEqpRunStatus("EQRUNSTUS"));

            //RegisterSequence(new SeqRecipeConfirm("EQRCPCONFIRM"));
            RegisterSequence(new SeqGlassLoadingReport("GLSLDREPORT"));
            RegisterSequence(new SeqGlassUnLoadingReport(this, "GLSULREPORT"));
            RegisterSequence(new SeqSubstrateIDDataInsideCln("SUBSTRATEIDDATA"));
            RegisterSequence(new SeqEqpRecipeParaChange("EQPRCPCHANGE")); // 10.12.25 minhan
            RegisterSequence(new SeqEqpRecipeParaAdd("EQPRCPADD")); // 10.12.25 minhan
            RegisterSequence(new SeqEqpRecipeParaDelete("EQPRCPDELETE")); // 10.12.25 minhan
            RegisterSequence(new SeqEqpRecipeParaCopy("EQPRCPCOPY")); // 10.12.25 minhan
            RegisterSequence(new SeqHostRecipeBodyReport("HOSTRCPREPORT")); // 10.12.25 minhan
            RegisterSequence(new SeqAlarmReport("ALARMREP"));
            //RegisterSequence(new SeqCrackReport("CRACKREPORT")); // 11.06.01 minhan
            // RegisterSequence(new SeqAlarmReset("ALARMRESET"));
        }

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
        public void SetLog(string GlassID, string RecipeID, string message)
        {

            string log;
            log = string.Format(",{0},{1},{2}", GlassID, RecipeID, message);
            ApddataLog.TextOut(log);

        }
    }

    // Transer Seq 3.1 A Change in a cleaner Status
    public class SeqEqpRunStatus : XSeqFunction
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
        private State m_OldState = State.UnKnown;
        private State m_CurState = State.UnKnown;
        private EqpManager m_EqpManager;
        //private int EqpRunState = -1;
        private GenInfoHandler m_GenInfo;
        private BOELoaderInterface m_BoeInterface;
        private bool m_Mode; // 10.12.21 minhan
        #endregion

        #region Constructor
        public SeqEqpRunStatus(string seqName)
        {
            m_SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = EqpManager.Instance;
            m_GenInfo = GenInfoHandler.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            m_Mode = false;
            m_Mode = m_Server.SetupSingleMode.GetValue<bool>(); // 11.02.08 minhan
            m_Mode |= !m_GenInfo.AutoMode;
            m_Mode |= !m_GenInfo.EqpInitComp;

            if (!m_BoeInterface.mobCleaner_is_Available.GetState()) m_BoeInterface.mobCleaner_is_Available.SetState(true); // 11.02.08 minhan

            //if ((m_Server.EqpStateManager.EqpUnit.EqpState == EqpState.Fault) ||
            //    m_Mode) // 메뉴얼 전환이면 알람 발생하는 부분과 싱글모드 일 경우는.
            //if (m_Server.EqpStateManager.IsAlarmState || m_Server.EqpStateManager.IsWarningState ||
            //   m_Mode) // 11.06.15 minhan
            if (m_Server.EqpStateManager.IsAlarmState || m_Mode)//131126.fan //zhangliang 
                                                                // m_Server.EqpStateManager.IsWarningState ||  // dspcrassus - 111128
            {
                //EqpRunState = 0;
                m_BoeInterface.mobTROUBLE.SetState(true);
                m_BoeInterface.mobRUN.SetState(false);
                m_BoeInterface.mobIDLE.SetState(false);

                m_CurState = State.Down;
            }
            else if ((m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Excute) && m_GenInfo.EqpInitComp)
            {
                //EqpRunState = 1;
                m_BoeInterface.mobTROUBLE.SetState(false);
                m_BoeInterface.mobRUN.SetState(true);
                m_BoeInterface.mobIDLE.SetState(false);

                m_CurState = State.Run;
            }
            else if (m_Server.EqpStateManager.EqpUnit.ProcessState == ProcessState.Idle)
            {
                //EqpRunState = -1;
                m_BoeInterface.mobTROUBLE.SetState(false);
                m_BoeInterface.mobRUN.SetState(false);
                m_BoeInterface.mobIDLE.SetState(true);

                m_CurState = State.Idle;
            }

            if (m_OldState != m_CurState)
            {
                m_OldState = m_CurState;
                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, m_CurState.ToString());
            }

            return -1;
        }
        #endregion
    }

    #region 제거
    /*public class SeqRecipeConfirm : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private Simul m_Simul;
        private MelsecBitOutRcpItem m_MelsecBitOutRcpItem;
        private RecipeProvider m_Provider;
        private short[] RcpId;
        #endregion

        #region Constructor
        public SeqRecipeConfirm(string seqName)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;

            m_MelsecBitOutRcpItem = sigMelsecBitOutRcpItem;
        }
        #endregion

        #region Methods
        private short IsAvailable(short[] data)
        {
            int count = m_Server.DataProvider.RecipeProvider.GetRecipeItemCount();
            if (count > data.Length) return (short)0;

            if (TagRecipe.IsAvailable(data)) return (short)1;
            else return (short)0;
        }

        public override int Do()
        {
            RcpId = new short[99];

            for (int i = 0; i < 99; i++)
            {

                TagRecipe item = new TagRecipe(Convert.ToString(i + 1));

                bool exist = m_Server.DataProvider.RecipeProvider.GetRecipe(Convert.ToString(i + 1), ref item);

                RcpId[i] = Convert.ToInt16(exist);
                //                m_MelsecBitOutRcpItem.MobEqpRecipeUseNo.SetStatus(RcpId[i]);

               // m_MelsecBitOutRcpItem.MelsecNet.SendBit(0, 255, (short)(m_MelsecBitOutRcpItem.MobEqpRecipeUseNo.GetStartAddress() + i), RcpId[i], false);
                m_MelsecBitOutRcpItem.MelsecNet.SendBit(0, 255, (short)(m_MelsecBitOutRcpItem.MobEqpRecipeUseNo.GetStartAddress() + i), RcpId[i]);
            }


            return -1;
        }
        #endregion
    }
    */
    #endregion

    public class SeqGlassLoadingReport : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_DataFormatErr; // 10.12.21 minhan
        //private TagGlassData RecvData = new TagGlassData();
        private BOELoaderInterface m_BoeInterface;
        //private short[] SimulData;
        //private short[] SimulrecipeId;
        private short m_simulCount = 0;
        private short[] m_Bufdata; // 11.02.08 minhan
        private short[] m_Bufdata10; // 11.02.09 minhan
        private string m_LogReport; // 11.02.09 minhan
        private XLog GlassDataLog; // 11.02.09 minhan;
        #endregion

        #region Constructor
        public SeqGlassLoadingReport(string seqName)
        {
            m_SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;

            m_AlarmOnTimeOut = new Alarm(m_SeqFunName + " : Loading Report Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_DataFormatErr = new Alarm(m_SeqFunName + " : Recv Data Format Error", AlarmLevel.S, AlarmCode.EquipmentSafety); // 10.12.21 minhan
            GlassDataLog = new XLog("Glass Data Log", XLog.LogStampType.UseStamp); // 11.021.09 minhan 
            m_Bufdata = new short[2];
            m_Bufdata10 = new short[10];
            m_LogReport = "";

            //if (m_Simul.Melsec)
            //{
            //    SimulData = new short[2];
            //    SimulrecipeId = new short[2];
            //    SimulData[0] = 0x0001;  // simul port num
            //    SimulData[1] = 0x0001;  // simul slot num
            //    SimulrecipeId[0] = 0;
            //    SimulrecipeId[1] = 1;
            //}
        }
        #endregion

        #region Methods
        public void SetLog(string seqName, int portNo, int slotNo, string message) // 11.02.09 minhan
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

            log = string.Format("Glass Data Log \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            GlassDataLog.TextOut(log);
        }

        public override int Do()
        {
            if (GlobalVar.rcvReportCancel) // 11.02.09 minhan
            {
                GlobalVar.rcvReportCancel = false;

                if (m_AlarmId > 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;
                }

                m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(false);

                this.m_SeqNo = 0;
                return -1;
            }

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    //if (!m_Simul.Melsec) 
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (GlobalVar.rcvReport && m_BoeInterface.mibCleaner_Recived_Data_Read_Request.GetState())
                    {
                        GlobalVar.rcvReport = false;
                        //m_Server.GlassData.GetData(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0), ref RecvData);
                        if (m_Simul.Melsec)
                        {
                            TagGlassData RecvData = new TagGlassData();

                            string m_Buf;
                            short[] m_SendData = new short[2];
                            short[] m_SendData10 = new short[10];

                            if (m_simulCount < 20)
                                m_simulCount++;
                            else
                                m_simulCount = 1;

                            m_LogReport = "";

                            // 시뮬은 간단히 하자 시뮬시는 같은 영역을 쓰기 때문에 이렇게 하지않으면 앞의 내용이 지워져 버린다.
                            // 다 BigEndiand으로 보내는 가보네... 잘못 되었으면 수정하삼.

                            m_LogReport = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.03.20 minhan

                            m_Buf = "01";
                            m_SendData[0] = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                            m_BoeInterface.miwPortID.SetState(m_SendData[0]);
                            m_SendData[0] = m_BoeInterface.miwPortID.GetState();
                            RecvData.PortID = XFunc.ConvertToString(m_SendData, 0, 1, ByteOrder.BigEndian);
                            m_LogReport += "/" + RecvData.PortID; // 11.03.20 minhan

                            string m_slotNo = string.Format("{0:d2}", m_simulCount); // 11.02.09 minhan
                            m_SendData[0] = XFunc.ConvertToWord(m_slotNo, ByteOrder.BigEndian);
                            m_BoeInterface.miwSlotID.SetState(m_SendData[0]);
                            m_SendData[0] = m_BoeInterface.miwSlotID.GetState();
                            RecvData.SlotID = XFunc.ConvertToString(m_SendData, 0, 1, ByteOrder.BigEndian);
                            m_LogReport += "/" + RecvData.SlotID;

                            m_Buf = "0001";
                            XFunc.ConvertToWord(m_Buf, ref m_SendData, 0, 2, ByteOrder.BigEndian);
                            m_BoeInterface.miw_Recipe_No.SetStates(m_SendData, 0, 2);
                            m_SendData = m_BoeInterface.miw_Recipe_No.GetStates(2);
                            RecvData.RecipeID = XFunc.ConvertToString(m_SendData, 0, 2, ByteOrder.BigEndian);
                            m_LogReport += "/" + RecvData.RecipeID;

                            m_Buf = "LtABCDEFGHIJKLMNOPQ0";
                            XFunc.ConvertToWord(m_Buf, ref m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_BoeInterface.miw_Lot_ID.SetStates(m_SendData10, 0, 10);
                            m_SendData10 = m_BoeInterface.miw_Lot_ID.GetStates(10);
                            RecvData.LotID = XFunc.ConvertToString(m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_LogReport += "/" + RecvData.LotID;

                            m_Buf = "CtABCDEFGHIJKLMNOPQ0";
                            XFunc.ConvertToWord(m_Buf, ref m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_BoeInterface.miw_Cst_ID.SetStates(m_SendData10, 0, 10);
                            m_SendData10 = m_BoeInterface.miw_Cst_ID.GetStates(10);
                            RecvData.CstID = XFunc.ConvertToString(m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_LogReport += "/" + RecvData.CstID;

                            m_Buf = "GsABCDEFGHIJKLMNOPQ0";
                            XFunc.ConvertToWord(m_Buf, ref m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_BoeInterface.miwGlsID.SetStates(m_SendData10, 0, 10);
                            m_SendData10 = m_BoeInterface.miwGlsID.GetStates(10);
                            RecvData.GlassID = XFunc.ConvertToString(m_SendData10, 0, 10, ByteOrder.BigEndian);
                            m_LogReport += "/ " + RecvData.GlassID;

                            RecvData.PositionId = eqpTransferUnits._LD_CvUnit.DataMatchingKey(0);
                            RecvData.Clone(RecvData);
                            m_Server.GlassData.Create(RecvData);

                            //m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(true);

                            if (GlobalVar.CurGlassData.Count > 50) // 11.02.09 minhan
                            {
                                GlobalVar.CurGlassData.Clear();
                                GlobalVar.CurGlassData.Add(m_LogReport);
                            }
                            else GlobalVar.CurGlassData.Add(m_LogReport);

                            SetLog(m_SeqFunName, 0, 0, m_LogReport);
                            m_LogReport = "";
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Recieve Data Read Request ON");

                            //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Cleaner Received Data Read Completed ON");
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                            break;
                        }
                        else
                        {
                            // 래시피 상에서 잘못 된 내용은 ld 부로 내려가서 처리하면 되지만... 만약 에러가 나는 처리부분은 있어야 한다.
                            try
                            {
                                TagGlassData RecvData = new TagGlassData();

                                for (int i = 0; i < 2; i++)
                                {
                                    m_Bufdata[i] = 0;
                                }

                                for (int i = 0; i < 10; i++)
                                {
                                    m_Bufdata10[i] = 0;
                                }

                                m_LogReport = "";

                                m_LogReport = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.03.20 minhan

                                m_Bufdata[0] = m_BoeInterface.miwPortID.GetState();
                                RecvData.PortID = XFunc.ConvertToString(m_Bufdata[0], ByteOrder.BigEndian);
                                m_LogReport += "/" + RecvData.PortID; // 11.03.20 minhan

                                m_Bufdata[0] = m_BoeInterface.miwSlotID.GetState();
                                RecvData.SlotID = XFunc.ConvertToString(m_Bufdata[0], ByteOrder.BigEndian);
                                m_LogReport += "/" + RecvData.SlotID;

                                m_Bufdata = m_BoeInterface.miw_Recipe_No.GetStates(2);
                                RecvData.RecipeID = XFunc.ConvertToString(m_Bufdata, 0, 2, ByteOrder.BigEndian);

                                int recipeid = 0; // 11.02.17 minhan
                                if (int.TryParse(RecvData.RecipeID, out recipeid))
                                {
                                    RecvData.RecipeID = string.Format("{0:d4}", recipeid);
                                }

                                m_LogReport += "/" + RecvData.RecipeID;

                                m_Bufdata10 = m_BoeInterface.miw_Lot_ID.GetStates(10);
                                RecvData.LotID = XFunc.ConvertToString(m_Bufdata10, 0, 10, ByteOrder.BigEndian);
                                m_LogReport += "/" + RecvData.LotID;

                                m_Bufdata10 = m_BoeInterface.miw_Cst_ID.GetStates(10);
                                RecvData.CstID = XFunc.ConvertToString(m_Bufdata10, 0, 10, ByteOrder.BigEndian);
                                m_LogReport += "/" + RecvData.CstID;

                                m_Bufdata10 = m_BoeInterface.miwGlsID.GetStates(10);
                                RecvData.GlassID = XFunc.ConvertToString(m_Bufdata10, 0, 10, ByteOrder.BigEndian);
                                m_LogReport += "/ " + RecvData.GlassID;

                                //m_Server.RecvData.UpdateData(RecvData); // 이걸 사용하지 않는다면..

                                RecvData.PositionId = eqpTransferUnits._LD_CvUnit.DataMatchingKey(0);
                                RecvData.Clone(RecvData);
                                m_Server.GlassData.Create(RecvData);

                                //m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(true);

                                if (GlobalVar.CurGlassData.Count > 50) // 11.02.09 minhan
                                {
                                    GlobalVar.CurGlassData.Clear();
                                    GlobalVar.CurGlassData.Add(m_LogReport);
                                }
                                else GlobalVar.CurGlassData.Add(m_LogReport);

                                SetLog(m_SeqFunName, 0, 0, m_LogReport);
                                m_LogReport = "";

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Recieve Data Read Request ON");
                                //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Cleaner Received Data Read Completed ON");
                                //m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                                break;
                            }
                            catch (Exception err) // 에러 발생시 리턴해 버리자. 알람이 필요하다면 생성해라.
                            {
                                GlobalVar.rcvReportComp = true;
                                m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(true);

                                m_AlarmId = m_DataFormatErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                SetLog(m_SeqFunName, 0, 0, m_LogReport);
                                m_LogReport = "";
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Glass Data Error");
                                m_Server.WriteExceptionLog(err.ToString());
                                m_ReturnSeqNo = nSeqNo;
                                nSeqNo = 1000;
                                break;
                            }
                        }
                    }
                    else if (m_Simul.Melsec)
                    {
                        if (GlobalVar.rcvReport) // 인터페이스에서 이 플래그를 true 한다면..
                        {
                            m_BoeInterface.mibCleaner_Recived_Data_Read_Request.SetState(true);
                        }
                    }

                    if (!m_BoeInterface.mibCleaner_Recived_Data_Read_Request.GetState() &&
                         m_BoeInterface.mobCleaner_Received_Data_Read_Completed.GetState()) // req 가 죽었는데 comp가 살아 있다면..
                    {
                        m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(false);
                    }
                    if (GlobalVar.rcvReport && GlobalVar.rcvReportComp) GlobalVar.rcvReportComp = false; // 이런 경우라라면 off 
                    break;
                case 20: // 11.02.08 minhan
                    {
                        m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(true);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Received Data Read Completed ON");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30: // 여기서는 신호가 off 되는 걸 보니 무한 대기는 없을 것 같네.
                    if (!m_BoeInterface.mibCleaner_Recived_Data_Read_Request.GetState())
                    {
                        m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(false);
                        GlobalVar.rcvReportComp = true;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Recieve Data Read Request OFF");
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Received Data Read Completed OFF");
                        nSeqNo = 0;
                        break;
                    }
                    else if (GetElapsedTicks() > 4000)
                    {
                        m_AlarmId = m_AlarmOnTimeOut.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loading Report T1 Timeout.");
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 2000;
                        break;
                    }
                    else if (m_Simul.Melsec && GetElapsedTicks() > 1000)
                    {
                        m_BoeInterface.mibCleaner_Recived_Data_Read_Request.SetState(false);
                    }

                    if (!m_BoeInterface.mobCleaner_Received_Data_Read_Completed.GetState())
                    {
                        m_BoeInterface.mobCleaner_Received_Data_Read_Completed.SetState(true);
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Data Error Alarm Reset.");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
                case 2000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loading Report T1 Timeout Alarm Reset.");
                        nSeqNo = m_ReturnSeqNo;
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqGlassUnLoadingReport : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private TagGlassData m_SendData; // 11.02.09 minhan
        private ApdItems m_ApdItems; // 11.02.09 minhan
        //private int m_PortNo = 0;
        //private int m_SlotNo = 0;
        private string m_Buf; // 11.02.09 minhan      
        private string m_ApdBuf; // 11.02.09 minhan
        private string m_Msg; // 11.02.09 minhan
        private bool m_CheckData; // 11.02.09 minhan
        private short m_PortNo;
        private short m_SlotNo;
        private short m_GlsCrack; // 11.06.01 minhan
        private ushort[] m_RecipeID;
        private ushort[] m_LotID;
        private ushort[] m_CstID;
        private ushort[] m_GlsID;
        private ushort[] m_ApdData;
        private double m_DoubleValue; // 11.02.09 minhan
        private int m_IntValue; // 11.02.09 minhan
        private string m_AsmApddata;// 11.02.09 minhan
        private string m_GlassDataBuf; // 11.02.18 minhan
        private Alarm m_AlarmOnTimeOut;
        private Alarm m_AlarmUnloadingReport; // 11.02.09 minhan
        private BOELoaderInterface m_BoeInterface;
        private EGiSInterface m_EgisInterface; // 11.06.10 minhan
        private SeqCimInterface_BOE_DOHC m_Control;
        private int nCnt; // 11.02.09 minhan
        private int m_TimeOut; // 11.06.10 minhan
        private int m_ApdWordCount = 0;

        // dspcrassus - 120214 : Trace APD Item Value Converting Exception
        private int m_TraceApdItemsIndex = 0;
        private int m_TraceApdItemsType = 0;
        private string m_TraceApdItemsName = "";
        private string m_TraceApdItemsValueOrg = "";
        private string m_TraceApdItemsValueCvt = "";

        #endregion

        #region Constructor
        public SeqGlassUnLoadingReport(SeqCimInterface_BOE_DOHC control, string seqName)
        {
            m_SeqFunName = seqName;
            m_Control = control;
            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_SendData = new TagGlassData();
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface; // 11.06.10 minhan
            m_Buf = "";
            m_ApdBuf = "";
            m_Msg = "";
            m_CheckData = false;
            m_PortNo = 0;
            m_SlotNo = 0;
            m_GlsCrack = 0; // 11.06.01 minhan
            m_RecipeID = new ushort[2];
            m_LotID = new ushort[10];
            m_CstID = new ushort[10];
            m_GlsID = new ushort[10];
            //m_ApdData = new ushort[137];
            m_DoubleValue = 0;
            m_IntValue = 0;
            nCnt = 0;
            m_TimeOut = 0; // 11.06.10 minhan
            m_AsmApddata = "";
            m_GlassDataBuf = "";

            m_ApdItems = m_Server.ApdItemsHandler.GetItems(-1);
            int count = m_ApdItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_ApdWordCount < m_ApdItems.Items[i].StartAddress + m_ApdItems.Items[i].WordCount)
                    m_ApdWordCount = m_ApdItems.Items[i].StartAddress + m_ApdItems.Items[i].WordCount;
            }
            m_ApdData = new ushort[m_ApdWordCount];

            m_AlarmOnTimeOut = new Alarm(m_SeqFunName + " : Unloading Report Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_AlarmUnloadingReport = new Alarm(m_SeqFunName + " : Unloading Report Error", AlarmLevel.S, AlarmCode.EquipmentSafety);
        }
        #endregion

        #region Methods
        private bool WriteGlassData()
        {
            try
            {
                m_Buf = "";
                m_PortNo = 0;
                m_SlotNo = 0;

                for (int i = 0; i < 2; i++)
                {
                    m_RecipeID[i] = 0;
                }

                for (int j = 0; j < 10; j++)
                {
                    m_LotID[j] = 0;
                }

                for (int k = 0; k < 10; k++)
                {
                    m_CstID[k] = 0;
                }

                for (int m = 0; m < 10; m++)
                {
                    m_GlsID[m] = 0;
                }

                m_GlassDataBuf = "";

                m_GlsCrack = 0; // 11.06.07 minhan

                //if (m_Server.GlassData.GetData(eqpTransferUnits._TR_Gantry_Unit.DataMatchingKey(0), ref m_SendData)) // 11.06.07 minhan
                //{
                //    int PortId = Convert.ToInt32(m_SendData.PortID); // 11.02.18 minhan
                //    int slotId = Convert.ToInt32(m_SendData.SlotID);
                //    m_GlsCrack = 0; // 11.06.01 minhan

                //    m_Buf = string.Format("{0:d2}", PortId);
                //    m_PortNo = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                //    m_BoeInterface.mowPortID.SetState((ushort)m_PortNo);
                //    m_GlassDataBuf = "PortID :" + m_Buf; // 11.02.18 minhan

                //    m_Buf = "";
                //    m_Buf = string.Format("{0:d2}", slotId);
                //    m_SlotNo = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                //    m_BoeInterface.mowSlotID.SetState((ushort)m_SlotNo);
                //    m_GlassDataBuf += "/SlotID :" + m_Buf;

                //    m_Buf = "";
                //    m_Buf = m_SendData.RecipeID;
                //    XFunc.ConvertToWord(m_Buf, ref m_RecipeID, 0, 2, ByteOrder.BigEndian);
                //    m_BoeInterface.mowRecipeNo.SetStates(m_RecipeID, 0, 2);
                //    m_GlassDataBuf += "/RecipeID :" + m_Buf;

                //    XFunc.ConvertToWord(m_SendData.LotID, ref m_LotID, 0, 10, ByteOrder.BigEndian);
                //    m_BoeInterface.mowLotID.SetStates(m_LotID, 0, 10);
                //    m_GlassDataBuf += "/LotID :" + m_SendData.LotID;


                //    XFunc.ConvertToWord(m_SendData.CstID, ref m_CstID, 0, 10, ByteOrder.BigEndian);
                //    m_BoeInterface.mowCstID.SetStates(m_CstID, 0, 10);
                //    m_GlassDataBuf += "/CstID :" + m_SendData.CstID;

                //    XFunc.ConvertToWord(m_SendData.GlassID, ref m_GlsID, 0, 10, ByteOrder.BigEndian);
                //    m_BoeInterface.mowGlassID.SetStates(m_GlsID, 0, 10);
                //    m_GlassDataBuf += "/GlassID :" + m_SendData.GlassID;

                //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_GlassDataBuf); // 11.02.18 minhan
                //    return true;
                //}
                //else // 아니라면... 모색해 봐야지...
                //{
                //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, m_GlassDataBuf);
                //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Unloading Report Error(Noting Glass data)");
                //    return false;
                //}

                if (m_Server.SendData != null) // 11.06.07 minhan
                {
                    int PortId = Convert.ToInt32(m_Server.SendData.PortID); // 11.02.18 minhan
                    int slotId = Convert.ToInt32(m_Server.SendData.SlotID);
                    m_GlsCrack = 0; // 11.06.01 minhan

                    m_Buf = string.Format("{0:d2}", PortId);
                    m_PortNo = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                    m_BoeInterface.mowPortID.SetState((ushort)m_PortNo);
                    m_GlassDataBuf = "PortID :" + m_Buf; // 11.02.18 minhan

                    m_Buf = "";
                    m_Buf = string.Format("{0:d2}", slotId);
                    m_SlotNo = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                    m_BoeInterface.mowSlotID.SetState((ushort)m_SlotNo);
                    m_GlassDataBuf += "/SlotID :" + m_Buf;

                    m_Buf = "";
                    m_Buf = string.Format("{0:d4}", m_Server.SendData.RecipeID);
                    XFunc.ConvertToWord(m_Buf, ref m_RecipeID, 0, 2, ByteOrder.BigEndian);
                    m_BoeInterface.mowRecipeNo.SetStates(m_RecipeID, 0, 2);
                    m_GlassDataBuf += "/RecipeID :" + m_Buf;

                    XFunc.ConvertToWord(m_Server.SendData.LotID, ref m_LotID, 0, 10, ByteOrder.BigEndian);
                    m_BoeInterface.mowLotID.SetStates(m_LotID, 0, 10);
                    m_GlassDataBuf += "/LotID :" + m_Server.SendData.LotID;


                    XFunc.ConvertToWord(m_Server.SendData.CstID, ref m_CstID, 0, 10, ByteOrder.BigEndian);
                    m_BoeInterface.mowCstID.SetStates(m_CstID, 0, 10);
                    m_GlassDataBuf += "/CstID :" + m_Server.SendData.CstID;

                    XFunc.ConvertToWord(m_Server.SendData.GlassID, ref m_GlsID, 0, 10, ByteOrder.BigEndian);
                    m_BoeInterface.mowGlassID.SetStates(m_GlsID, 0, 10);
                    m_GlassDataBuf += "/GlassID :" + m_Server.SendData.GlassID;

                    if (!m_Server.SendData.LoaderCrackAlarm) // 11.06.07 minhan
                    {
                        m_Buf = "01";
                        m_GlsCrack = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                        m_BoeInterface.mowGls_Crack.SetState((ushort)m_GlsCrack); // gls
                        //m_BoeInterface.mowApd_Crack.SetState((ushort)m_GlsCrack); // apd
                        m_GlassDataBuf += "/Crack :" + m_Buf;
                    }
                    else
                    {
                        m_Buf = "02";
                        m_GlsCrack = XFunc.ConvertToWord(m_Buf, ByteOrder.BigEndian);
                        m_BoeInterface.mowGls_Crack.SetState((ushort)m_GlsCrack); // gls
                        //m_BoeInterface.mowApd_Crack.SetState((ushort)m_GlsCrack); // apd
                        m_GlassDataBuf += "/Crack :" + m_Buf;
                    }

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, m_GlassDataBuf); // 11.02.18 minhan

                    m_GlassDataBuf = "";

                    m_GlassDataBuf = DateTime.Now.ToString("yyyyMMddHHmmss");
                    m_GlassDataBuf += "/" + m_Server.SendData.PortID; // 11.06.07 minhan
                    m_GlassDataBuf += "/" + m_Server.SendData.SlotID;
                    m_GlassDataBuf += "/" + m_Server.SendData.RecipeID;
                    m_GlassDataBuf += "/" + m_Server.SendData.LotID;
                    m_GlassDataBuf += "/" + m_Server.SendData.CstID;
                    m_GlassDataBuf += "/" + m_Server.SendData.GlassID;
                    m_GlassDataBuf += "/" + m_Server.SendData.TRCrackAlarm.ToString();
                    m_GlassDataBuf += "/" + m_Server.SendData.LoaderCrackAlarm.ToString();

                    if (GlobalVar.CurSendGlassData.Count > 50)
                    {
                        GlobalVar.CurSendGlassData.Clear();
                        GlobalVar.CurSendGlassData.Add(m_GlassDataBuf);
                    }
                    else GlobalVar.CurSendGlassData.Add(m_GlassDataBuf);

                    return true;
                }
                else
                {
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, m_GlassDataBuf);
                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Unloading Report Error(Noting Glass data)");
                    return false;
                }
            }
            catch (Exception err)
            {
                m_Msg = err.ToString();
                m_Server.WriteExceptionLog(m_Msg);
                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, m_GlassDataBuf);
                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Unloading Report Error(Exception)");
                return false;
            }

        }

        public override int Do()
        {
            if (GlobalVar.sndReportCancel) // 11.02.09 minhan
            {
                GlobalVar.sndReportCancel = false;

                if (m_AlarmId > 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;
                }

                m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(false);

                this.m_SeqNo = 0;
                return -1;
            }

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    //if (!m_Simul.Melsec)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    {
                        if (GlobalVar.sndReport && !m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_U.GetState()) // 11.06.10 minhan
                        {
                            GlobalVar.sndReport = false;

                            //if (GlobalVar.ExchnageReq) // 11.06.17 minhan
                            //{
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Glass Crack Check : Start");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 12;
                            break;
                            //}
                            //else
                            //{
                            //    m_Server.SendData.LoaderCrackAlarm = false;
                            //    m_Server.SendData.Clone();

                            //    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Check : Skip(Unload Mode)");
                            //    nSeqNo = 13;
                            //    break;
                            //}
                        }
                        else if (m_Simul.Melsec)
                        {
                            m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_U.SetState(false);
                        }

                        if (m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.GetState()) // 살아 있다면..
                        {
                            m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(false);
                        }

                        if (GlobalVar.sndReport && GlobalVar.sndReportComp) GlobalVar.sndReportComp = false; // 11.04.16 minhan
                    }
                    break;
                case 12: // 11.06.10 minhan
                    {
                        m_TimeOut = m_Server.EgisLoaderInterfaceTiemout.GetValue<int>() * 1000;

                        if (m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>())
                        {
                            if (GlobalVar.CrackOutCheckOK)
                            {
                                m_Server.SendData.LoaderCrackAlarm = false;
                                m_Server.SendData.Clone();
                                GlobalVar.CrackOutCheckOK = false;

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Crack Check OK");
                                nSeqNo = 13;
                            }
                            else if (GlobalVar.CrackOutCheckNG)
                            {
                                m_Server.SendData.LoaderCrackAlarm = true;
                                m_Server.SendData.Clone();

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Crack Check NG");
                                nSeqNo = 13;
                            }
                            else if (GlobalVar.CrackOutCheckWarning) // 12.06.13 wang
                            {
                                m_Server.SendData.LoaderCrackAlarm = false;
                                m_Server.SendData.Clone();
                                GlobalVar.CrackOutCheckWarning = false;

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Crack Check Warning");
                                nSeqNo = 13;
                            }
                            else if (GetElapsedTicks() > m_TimeOut)
                            {
                                m_Server.SendData.LoaderCrackAlarm = true;
                                m_Server.SendData.Clone();

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Crack Check : Time out");
                                nSeqNo = 13;
                            }
                        }
                        else
                        {
                            m_Server.SendData.LoaderCrackAlarm = false;
                            m_Server.SendData.Clone();

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Crack Check : No use");
                            nSeqNo = 13;
                        }
                    }
                    break;
                case 13: // 11.06.10 minhan
                    {
                        try
                        {
                            m_CheckData = WriteGlassData();

                            if (m_CheckData)
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Write Glass Data : OK");
                                nSeqNo = 15;
                            }
                            else
                            {
                                m_AlarmId = m_AlarmUnloadingReport.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                GlobalVar.sndReportComp = true;
                                nSeqNo = 1000;
                            }
                        }
                        catch (Exception err) // 에러 발생시 리턴해 버리자. 알람이 필요하다면 생성해라. 나중에 처리 방법 모색해야함.
                        {
                            m_AlarmId = m_AlarmUnloadingReport.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            GlobalVar.sndReportComp = true;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recv Glass Data Error");
                            m_Server.WriteExceptionLog(err.ToString());
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 15: // 11.02.09 minhan
                    {
                        // dspcrassus - 120214 : Trace APD Item Value Converting Exception
                        m_TraceApdItemsIndex = 0;

                        try
                        {
                            m_ApdItems = m_Server.ApdItemsHandler.GetItems(-1);

                            if (m_ApdItems != null)
                            {
                                //for (int i = 0; i < 137; i++)
                                for (int i = 0; i < m_ApdWordCount; i++)
                                {
                                    m_ApdData[i] = 0;
                                }

                                nCnt = 0;
                                m_ApdBuf = "";
                                m_DoubleValue = 0;
                                m_IntValue = 0;
                                m_AsmApddata = "";

                                foreach (ApdItem item in m_ApdItems.Items)
                                {
                                    if (item.Id != m_ApdItems.Count - 1)
                                    {
                                        m_AsmApddata += item.Value.ToString() + ",";
                                    }
                                    else
                                    {
                                        m_AsmApddata += item.Value.ToString();
                                    }

                                    if ((item.Id == eqpApdItems._GLS_START.Id) ||
                                        (item.Id == eqpApdItems._GLS_END.Id))
                                    {
                                        if (item.Value != "")
                                        {
                                            XFunc.ConvertToWord(item.Value, ref m_ApdData, item.StartAddress, item.WordCount, ByteOrder.BigEndian);
                                        }
                                        else
                                        {
                                            m_ApdBuf = "00000000000000";
                                            XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount, ByteOrder.BigEndian);
                                        }
                                    }
                                    //else if (item.Id == eqpApdItems._AP_VOLTAGE.Id)
                                    //{
                                    //    if (item.Value != "")
                                    //    {
                                    //        // dspcrassus - 120214 : Tracing
                                    //        m_TraceApdItemsName = item.Name;
                                    //        m_TraceApdItemsType = 2;
                                    //        m_TraceApdItemsValueOrg = item.Value;
                                    //        m_TraceApdItemsValueCvt = item.Value;
                                    //        /////////////////////////////////
                                    //        m_DoubleValue = Convert.ToDouble(item.Value);

                                    //        if (m_DoubleValue >= 0) // 11.04.16 minhan
                                    //        {
                                    //            m_ApdBuf = string.Format("{0:d2}", (int)m_DoubleValue);
                                    //            XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount, ByteOrder.BigEndian);
                                    //        }
                                    //        else
                                    //        {
                                    //            m_ApdBuf = "00";
                                    //            XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount, ByteOrder.BigEndian);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        m_ApdBuf = "00";
                                    //        XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount, ByteOrder.BigEndian);
                                    //    }
                                    //}
                                    else
                                    {

                                        if (item.Value != "")
                                        {
                                            nCnt = item.Value.IndexOf('.');

                                            if (nCnt != -1)
                                            {
                                                m_ApdBuf = item.Value.Substring(0, nCnt);
                                                // dspcrassus - 120214 : Tracing
                                                m_TraceApdItemsName = item.Name;
                                                m_TraceApdItemsType = 1;
                                                m_TraceApdItemsValueOrg = item.Value;
                                                m_TraceApdItemsValueCvt = m_ApdBuf;
                                                /////////////////////////////////
                                                m_IntValue = Convert.ToInt32(m_ApdBuf);

                                                if (m_IntValue >= 0) // 11.04.16 minhan
                                                {
                                                    m_ApdBuf = "";
                                                    m_ApdBuf = string.Format("{0:d4}", m_IntValue);
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                                    m_IntValue = 0;
                                                    m_ApdBuf = "";

                                                    m_ApdBuf = item.Value.Substring(nCnt + 1, 1);//120215
                                                    // dspcrassus - 120214 : Tracing
                                                    m_TraceApdItemsType = 1;
                                                    m_TraceApdItemsValueCvt = m_ApdBuf;
                                                    /////////////////////////////////
                                                    m_IntValue = Convert.ToInt32(m_ApdBuf);
                                                    m_ApdBuf = "";
                                                    m_ApdBuf = "." + m_IntValue/*string.Format("{0:d2}", m_IntValue)*/;// wzy 151124
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                                }
                                                else
                                                {
                                                    m_ApdBuf = "0000";
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                                    m_ApdBuf = ".0";
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                                }
                                            }
                                            else
                                            {
                                                // dspcrassus - 120214 : Tracing
                                                m_TraceApdItemsName = item.Name;
                                                m_TraceApdItemsType = 1;
                                                m_TraceApdItemsValueOrg = item.Value;
                                                m_TraceApdItemsValueCvt = m_ApdBuf;
                                                /////////////////////////////////
                                                m_IntValue = Convert.ToInt32(item.Value);

                                                if (m_IntValue >= 0) // 11.04.16 minhan
                                                {
                                                    m_ApdBuf = string.Format("{0:d4}", m_IntValue);
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                                    m_IntValue = 0;
                                                    m_ApdBuf = ".0";
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                                }
                                                else
                                                {
                                                    m_ApdBuf = "0000";
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                                    m_ApdBuf = ".0";
                                                    XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            m_ApdBuf = "0000";
                                            XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress, item.WordCount - 1, ByteOrder.BigEndian);

                                            //m_IntValue = 0;
                                            m_ApdBuf = "00";
                                            XFunc.ConvertToWord(m_ApdBuf, ref m_ApdData, item.StartAddress + 2, 1, ByteOrder.BigEndian);
                                        }

                                        m_TraceApdItemsIndex++;
                                    }
                                }
                                //m_Control.SetLog(m_SendData.GlassID, m_SendData.RecipeID, m_AsmApddata); // 11.02.09 minhan 11.06.07 minhan

                                m_Control.SetLog(m_Server.SendData.GlassID, m_Server.SendData.RecipeID, m_AsmApddata); // 11.06.07 minhan

                                //m_BoeInterface.mowProcess_Data_from_Cleaner.SetStates(m_ApdData, 0, 137);
                                m_BoeInterface.mowProcess_Data_from_Cleaner.SetStates(m_ApdData, 0, m_ApdWordCount);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "APD Data Write OK");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;

                            }
                            else
                            {
                                m_AlarmId = m_AlarmUnloadingReport.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                GlobalVar.sndReportComp = true;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "APD Data Write Error(APD DATA Null)");
                                nSeqNo = 1000;
                            }
                        }
                        catch (Exception err)
                        {
                            m_AlarmId = m_AlarmUnloadingReport.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            GlobalVar.sndReportComp = true;
                            m_Msg = err.ToString();
                            m_Server.WriteExceptionLog(m_Msg);

                            // dspcrassus - 120214 : Exception Trace Log
                            m_Msg = string.Format("INDEX: {0}, NAME: {1}, TYPE: {2}, VALUE(ORG): {3}, VALUE(CVT): {4}",
                                                  m_TraceApdItemsIndex, m_TraceApdItemsName, m_TraceApdItemsType, m_TraceApdItemsValueOrg, m_TraceApdItemsValueCvt);
                            m_Server.WriteExceptionLog(m_Msg);
                            ////////////////////////////////////////////

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "APD Data Write Error(APD DATA Exception)");
                            nSeqNo = 1000;
                        }
                    }
                    break;
                case 20:
                    if (GetElapsedTicks() > 100)
                    {
                        m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(true); // 10.12.21 minhan
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Reported Data Read Request");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                    }
                    break;
                case 30: // 만일 여기서 처리가 안되는 루트가 나온다면 생각해 봐야 한다.
                    if (!GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(false);
                        GlobalVar.sndReportComp = true;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Data Report Skip( case 30 Loader Ready Off)");
                        nSeqNo = 0;
                    }
                    else if (m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_U.GetState()) //kangtaegoo test 091201
                    {
                        m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(false);
                        GlobalVar.sndReportComp = true; // 10.12.21 minhan
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Reported Data Read Completed");
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Cleaner Reported Data Read Request:OFF");
                        //*****************************************************************************************
                        //APD Data 초기화
                        //*****************************************************************************************

                        nSeqNo = 0; // 10.12.21 minhan
                    }
                    else if (GetElapsedTicks() > 4000)
                    {
                        m_AlarmId = m_AlarmOnTimeOut.Id;
                        m_EqpManager.SetAlarm(m_AlarmId);
                        m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(false); // 11.02.17 minhan
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Unloading Report T1 Timeout.");
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 2000;
                        break; // 11.02.17 minhan
                    }
                    else if (m_Simul.Melsec && GetElapsedTicks() > 1000)
                    {
                        m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_U.SetState(true);
                    }

                    if (!m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.GetState())
                    {
                        m_BoeInterface.mobCleaner_Reported_Data_Read_Request_U.SetState(true);
                    }
                    break;
                //case 40: // 이걸 보는게 좋지 않는 것 같다 차라리 보려면 0에서..
                //    if (!m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_L.GetState())
                //    {
                //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Cleaner Reported Data Read Completed: OFF");
                //        nSeqNo = 0;
                //    }
                //    else if(m_Simul.Melsec)
                //    {
                //        m_BoeInterface.mibCleaner_Reported_Data_Read_Completed_U.SetState(false);
                //    }
                //    else if (GetElapsedTicks() > 4000)
                //    {
                //        AlarmId = m_AlarmOnTimeOut.Id;
                //        m_EqpManager.SetAlarm(AlarmId);
                //        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Unloading Report T1 Timeout.");
                //        nSeqNo = 1000;
                //    }
                //    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Reset.");
                        nSeqNo = 0;
                    }
                    break;
                case 2000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_StartTicks = XFunc.GetTickCount();
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Unloading Report T1 Timeout Alarm Reset.");
                        GlobalVar.sndReportComp = true; // 11.02.17 minhan
                        nSeqNo = 0;
                    }
                    break;

            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqAlarmReport : XSeqFunction // 11.02.08 minhan
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private AlarmReportQueue m_AlarmReportQueue;
        private TagAlarmReport m_AlarmReport;

        //private uint timeOut = 4000;
        private Alarm ALM_ReplyTimeout;
        private Alarm ALM_AlarmReportError; // 11.02.08 minhan
        private string m_CurSetAlarmId; // 11.02.08 minhan
        private string m_CurSetAlarmCode; // 11.02.08 minhan
        private string m_CurSetAlarmLevel; // 11.02.08 minhan
        private string m_CurSetAlarmText; // 11.02.08 minhan
        //private int m_CurResetAlarmId = 0;
        private BOELoaderInterface m_BoeInterface;
        private ushort[] m_CurAlarmBufId; // 11.02.08 minhan
        private ushort m_CurAlarmBufCode; // 11.02.08 minhan
        private ushort m_CurAlarmBufLevel; // 11.02.08 minhan
        private ushort[] m_CurAlarmText;
        private string m_msgReport; // 11.02.08 minhan
        #endregion

        #region Constructor
        public SeqAlarmReport(string seqName)
        {
            m_SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = EqpManager.Instance;
            m_AlarmReportQueue = AlarmReportQueue.Instance;

            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            ALM_ReplyTimeout = new Alarm("Alarm Report Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            ALM_AlarmReportError = new Alarm("Interface Alarm Report Error", AlarmLevel.S, AlarmCode.EquipmentSafety); // 11.02.08 minhan
            m_CurSetAlarmId = ""; // 11.02.08 minhan
            m_CurSetAlarmCode = "";
            m_CurSetAlarmLevel = "";
            m_CurSetAlarmText = "";
            m_CurAlarmBufId = new ushort[2];
            m_CurAlarmBufCode = 0;
            m_CurAlarmBufLevel = 0;
            m_CurAlarmText = new ushort[40];
            m_msgReport = "";
        }
        #endregion

        #region Methods
        public override int Do()
        {
            //if (m_Server.State != ActiveState.Run) return -1; // 11.02.08 minhan

            int nSeqNo = m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (m_AlarmReportQueue.Count > 0)
                    {
                        m_AlarmReport = m_AlarmReportQueue.Dequeue();

                        try
                        {
                            //if (GlobalVar.LoaderReady && (m_AlarmReport.ReportType == AlarmReportType.Set)) // 11.04.14 minhan
                            // dspcrassus - 111128 : EQP Warning 시 Warning에 대한 CIM Report 안함
                            if (GlobalVar.LoaderReady &&
                               (m_AlarmReport.ReportType == AlarmReportType.Set && m_AlarmReport.AlarmInfo.Level == AlarmLevel.S))
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    m_CurAlarmBufId[i] = 0;
                                }
                                m_CurAlarmBufCode = 0;
                                m_CurAlarmBufLevel = 0;

                                for (int i = 0; i < 40; i++)
                                {
                                    m_CurAlarmText[i] = 0;
                                }

                                m_CurSetAlarmId = "";
                                m_CurSetAlarmCode = "";
                                m_CurSetAlarmLevel = "";
                                m_CurSetAlarmText = "";

                                m_msgReport = "";

                                m_CurSetAlarmId = string.Format("{0:d4}", m_AlarmReport.AlarmInfo.Id);
                                m_CurSetAlarmCode = string.Format("{0:d2}", (ushort)m_AlarmReport.AlarmInfo.Code);
                                m_CurSetAlarmLevel = string.Format("{0:d2}", (ushort)m_AlarmReport.AlarmInfo.Level);
                                m_CurSetAlarmText = m_AlarmReport.AlarmInfo.Name;

                                int k = 0;
                                k = m_CurSetAlarmText.IndexOf(' ');

                                while (k != -1) // 11.02.17 minhan 공백처리
                                {
                                    m_CurSetAlarmText = m_CurSetAlarmText.Remove(k, 1);
                                    k = m_CurSetAlarmText.IndexOf(' ');
                                }

                                // ID
                                XFunc.ConvertToWord(m_CurSetAlarmId, ref m_CurAlarmBufId, 0, 2, ByteOrder.BigEndian);
                                m_BoeInterface.mowAlarmID.SetStates(m_CurAlarmBufId, 0, 2);

                                // Code
                                m_CurAlarmBufCode = (ushort)XFunc.ConvertToWord(m_CurSetAlarmCode, ByteOrder.BigEndian);
                                m_BoeInterface.mowAlarmCode.SetState(m_CurAlarmBufCode);

                                // Level
                                m_CurAlarmBufLevel = (ushort)XFunc.ConvertToWord(m_CurSetAlarmLevel, ByteOrder.BigEndian);
                                m_BoeInterface.mowAlarmLevel.SetState(m_CurAlarmBufLevel);

                                //Text
                                XFunc.ConvertToWord(m_CurSetAlarmText, ref m_CurAlarmText, 0, 40, ByteOrder.BigEndian);
                                m_BoeInterface.mowAlarmText.SetStates(m_CurAlarmText, 0, 40);

                                m_msgReport = string.Format("ID : {0}, CODE : {1}, LEVEL : {2}, NAME : {3}", m_CurSetAlarmId, m_CurSetAlarmCode, m_CurSetAlarmLevel, m_CurSetAlarmText);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, m_msgReport);

                                nSeqNo = 10;
                            }
                            else
                            {
                                if (m_BoeInterface.mobSerious_Alarm.GetState()) m_BoeInterface.mobSerious_Alarm.SetState(false);
                                if (m_BoeInterface.mobLight_Alarm.GetState()) m_BoeInterface.mobLight_Alarm.SetState(false);
                            }
                        }
                        catch (Exception err)
                        {
                            m_BoeInterface.mobSerious_Alarm.SetState(false);
                            m_BoeInterface.mobLight_Alarm.SetState(false);

                            m_AlarmId = ALM_AlarmReportError.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Report Data Error");
                            m_Server.WriteExceptionLog(err.ToString());
                            nSeqNo = 1000;
                            break;
                        }
                    }
                    else
                    {
                        if (m_BoeInterface.mobSerious_Alarm.GetState()) m_BoeInterface.mobSerious_Alarm.SetState(false);
                        if (m_BoeInterface.mobLight_Alarm.GetState()) m_BoeInterface.mobLight_Alarm.SetState(false);
                    }
                    break;
                case 10:
                    {
                        if (GlobalVar.LoaderReady)
                        {
                            if (m_AlarmReport.AlarmInfo.Level == AlarmLevel.S)
                            {
                                m_BoeInterface.mobSerious_Alarm.SetState(true);
                                m_BoeInterface.mobLight_Alarm.SetState(false);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Serious Alarm Report");
                            }
                            else
                            {
                                m_BoeInterface.mobLight_Alarm.SetState(true);
                                m_BoeInterface.mobSerious_Alarm.SetState(false);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Light Alarm Report");
                            }

                            if (m_Simul.Melsec) m_BoeInterface.mibCleaner_Alarm_Data_Read_Completed.SetState(true);
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 20;
                        }
                        else
                        {
                            m_BoeInterface.mobSerious_Alarm.SetState(false);
                            m_BoeInterface.mobLight_Alarm.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Report Skip(case 10 Loader Ready Off)");
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!GlobalVar.LoaderReady)
                        {
                            m_BoeInterface.mobSerious_Alarm.SetState(false);
                            m_BoeInterface.mobLight_Alarm.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Report Skip( case 20 Loader Ready Off)");
                            nSeqNo = 0;
                        }
                        else if (m_BoeInterface.mibCleaner_Alarm_Data_Read_Completed.GetState())
                        {
                            m_BoeInterface.mobLight_Alarm.SetState(false);
                            m_BoeInterface.mobSerious_Alarm.SetState(false);
                            if (m_Simul.Melsec) m_BoeInterface.mibCleaner_Alarm_Data_Read_Completed.SetState(false);

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Data Read Complete ON");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                        else if (GetElapsedTicks() > 4000)
                        {
                            m_AlarmId = ALM_ReplyTimeout.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Set Time Out(case 20)");
                            //m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else
                        {
                            if (m_AlarmReport.AlarmInfo.Level == AlarmLevel.S)
                            {
                                if (!m_BoeInterface.mobSerious_Alarm.GetState()) m_BoeInterface.mobSerious_Alarm.SetState(true);
                                if (m_BoeInterface.mobLight_Alarm.GetState()) m_BoeInterface.mobLight_Alarm.SetState(false);
                            }
                            else
                            {
                                if (m_BoeInterface.mobSerious_Alarm.GetState()) m_BoeInterface.mobSerious_Alarm.SetState(false);
                                if (!m_BoeInterface.mobLight_Alarm.GetState()) m_BoeInterface.mobLight_Alarm.SetState(true);
                            }
                        }
                    }
                    break;
                case 30:
                    {
                        if (!GlobalVar.LoaderReady)
                        {
                            m_BoeInterface.mobSerious_Alarm.SetState(false);
                            m_BoeInterface.mobLight_Alarm.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Report Skip( case 30 Loader Ready Off)");
                            nSeqNo = 0;
                        }
                        else if (!m_BoeInterface.mibCleaner_Alarm_Data_Read_Completed.GetState())
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Data Read Complete OFF");
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 4000)
                        {
                            m_AlarmId = ALM_ReplyTimeout.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Set Time Out(case 30)");
                            //m_ReturnSeqNo = nSeqNo;
                            nSeqNo = 1000;
                        }
                        else
                        {
                            if (m_BoeInterface.mobSerious_Alarm.GetState()) m_BoeInterface.mobSerious_Alarm.SetState(false);
                            if (m_BoeInterface.mobLight_Alarm.GetState()) m_BoeInterface.mobLight_Alarm.SetState(false);
                        }
                    }
                    break;
                case 1000:
                    if (m_EqpManager.AlarmResetSwitchPushed)
                    {
                        m_BoeInterface.mobSerious_Alarm.SetState(false); // 11.02.08 minhan
                        m_BoeInterface.mobLight_Alarm.SetState(false);
                        m_EqpManager.ResetAlarm(m_AlarmId);
                        m_AlarmId = 0;
                        m_AlarmReportQueue.Clear(); // 11.02.17 minhan
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Release");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 1100;
                    }
                    break;
                case 1100:
                    {
                        if (GetElapsedTicks() > 1000)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Release(return 0)");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
    #region 제거
    /*
    public class SeqAlarmReset : XSeqFunction
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager;
        private Simul m_Simul;
        private MelsecBitInCIM m_MelsecBitInCIM;
        private MelsecBitOutEQ m_MelsecBitOutEQ;
        private uint delaytime = 200;
        #endregion

        #region Constructor
        public SeqAlarmReset(string seqName, MelsecBitInCIM sigMelsecBitInCIM, MelsecBitOutEQ sigMelsecBitOutEQ)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_EqpManager = EqpManager.Instance;

            m_MelsecBitInCIM = sigMelsecBitInCIM;
            m_MelsecBitOutEQ = sigMelsecBitOutEQ;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    if (!m_Simul.Melsec)
                    {
                        nSeqNo = 10;
                    }
                    break;
                case 10:
                    if (m_MelsecBitInCIM.MibCimAlarmResetReq.GetStatus())
                    {
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Cim Alarm Reset Request : ON");
                        m_StartTicks = XFunc.GetTickCount();
                        
                        //**********************************************************************
                        // Alarm Clear Sequence
                        if (m_Simul.Melsec) m_EqpManager.AlarmResetSwitchPushed = true;
                        else
                        {
                            eqpAlarmResetSwitchs._EQP_Alarm_Reset.SetSwitchPushed(true);
                            eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetState(true);
                           // eqpAlarmResetSwitchs._EQP_Alarm_Reset.DoAlarmResetLamp.SetState(false);
                            //eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetState(true);//.DiAlarmResetSwitch.SetState(true);
                            //eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetState(true);
                            //eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetState(true);
                        }
                        nSeqNo = 15;
                    }

                    //else if (m_Simul.Melsec)
                    //{
                    //    m_MelsecBitInCIM.MibCimAlarmResetReq.SetStatus(true);
                    //}
                    break;
                case 15:
                    if (!m_EqpManager.AlarmResetSwitchPushed)
                    {
                        eqpAlarmResetSwitchs._EQP_Alarm_Reset.SetSwitchPushed(true);
                        eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetState(true);
                    }
                    else nSeqNo = 20;
                    break;
                case 20:
                    if (GetElapsedTicks() >delaytime)
                    {
                        m_MelsecBitOutEQ.MobEqpAlarmResetReqAck.SetStatus(true);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Eqp Alarm Reset Request ACK : ON");
                        nSeqNo = 30;
                    }
                    break;
                case 30:
                    if (!m_MelsecBitInCIM.MibCimAlarmResetReq.GetStatus())
                    {
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Cim Alarm Reset Request : OFF");

                        m_MelsecBitOutEQ.MobEqpAlarmResetReqAck.SetStatus(false);
                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Eqp Alarm Reset Request ACK : OFF");
                        //**********************************************************************
                        //Alarm Clear Sequence
                        if (m_Simul.Melsec) m_EqpManager.AlarmResetSwitchPushed = false;
                        else
                        {
                            // eqpAlarmResetSwitchs._EQP_Alarm_Reset.DiAlarmResetSwitch.SetPulse(true, 500);
                            eqpAlarmResetSwitchs._EQP_Alarm_Reset.SetSwitchPushed(false);
                        }
                        nSeqNo = 0;
                        //**********************************************************************
                    }
                    //else if (m_Simul.Melsec)
                    //{
                    //    m_MelsecBitInCIM.MibCimAlarmResetReq.SetStatus(false);
                    //}
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
    */
    #endregion
    //3.5 Substrate ID Data Inside Cleaner
    public class SeqSubstrateIDDataInsideCln : XSeqFunction // 11.02.09 minhan
    {
        #region Fields
        private ServerManager m_Server;
        private EqpManager m_EqpManager; // 11.02.09 minhan
        private Simul m_Simul;
        private ushort[] m_SubstateData;
        private TagGlassData m_GlassData; // 11.02.08 minhan
        //private int m_Pos = 0; // 11.02.08 minhan
        //private int m_DataCount;
        //private GantryUnit m_GantryUnit;
        private int idsarraycount; // 11.02.08 minhan
        private string m_Buf;
        private BOELoaderInterface m_BoeInterface;
        private Alarm ALM_SubIDReportTimeout;
        #endregion

        #region Constructor
        public SeqSubstrateIDDataInsideCln(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_Simul = AppConfig.Instance.Simul;
            m_GlassData = new TagGlassData();
            //m_GantryUnit = eqpTransferUnits._TR_Gantry_Unit;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;

            m_SubstateData = new ushort[2];
            ALM_SubIDReportTimeout = new Alarm("Substrate ID Report Timeout", AlarmLevel.L, AlarmCode.EquipmentStatusWarning); // 11.02.08 minhan
            idsarraycount = 0;
            m_Buf = "";

            //m_DataCount = m_SubstateData.Length;
        }
        #endregion

        #region Methods
        //private bool IsContained(int key, int[] container) // 11.02.08 minhan
        //{
        //    foreach (int i in container)
        //    {
        //        if (key == i) return true;
        //    }

        //    return false;
        //}
        public override int Do()
        {

            int nSeqNo = this.m_SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if ((m_BoeInterface.mibSubstrate_ID_Data_Inside_Cleaner_Set_Request.GetState()) && GlobalVar.LoaderReady) // 11.04.17 minhan
                        {
                            for (int i = 0; i < 2; i++) // 초기값 이니셜
                            {
                                m_SubstateData[i] = 0;
                            }

                            idsarraycount = 0;
                            m_Buf = "";

                            int[] ids;
                            m_Server.GlassData.GetAllPositionId(out ids);

                            int Lotid = 0; // 11.02.17 minhan
                            int Slotid = 0;

                            m_Buf = "0000"; // 11.04.18 minhan

                            XFunc.ConvertToWord(m_Buf, ref m_SubstateData, 0, 2, ByteOrder.BigEndian);

                            m_BoeInterface.mowUnload_No1_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                            m_BoeInterface.mowUnload_No2_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                            m_BoeInterface.mowUnload_No3_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                            m_BoeInterface.mowUnload_No4_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);


                            foreach (int id in ids)
                            {
                                //if (id % 2 > 0)
                                //{
                                //    if (IsContained(id / 2, ids)) continue;
                                //}

                                for (int i = 0; i < 2; i++)
                                {
                                    m_SubstateData[i] = 0;
                                }

                                idsarraycount++;

                                if (m_Server.DataProvider.GlassDataProvider.GetData(id, ref m_GlassData)) // 11.02.08 minhan
                                {
                                    if ((int.TryParse(m_GlassData.PortID, out Lotid)) && (int.TryParse(m_GlassData.SlotID, out Slotid))) // 11.02.17 minhan
                                    {
                                        m_Buf = string.Format("{0:d2}{1:d2}", Lotid, Slotid);
                                    }
                                    else
                                    {
                                        m_Buf = "0000";
                                    }
                                    XFunc.ConvertToWord(m_Buf, ref m_SubstateData, 0, 2, ByteOrder.BigEndian);

                                    if (idsarraycount == 1) m_BoeInterface.mowUnload_No1_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 2) m_BoeInterface.mowUnload_No2_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 3) m_BoeInterface.mowUnload_No3_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 4) m_BoeInterface.mowUnload_No4_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                }
                                else // 아니라면 0
                                {
                                    m_Buf = "0000"; // 11.04.18 minhan

                                    XFunc.ConvertToWord(m_Buf, ref m_SubstateData, 0, 2, ByteOrder.BigEndian);

                                    if (idsarraycount == 1) m_BoeInterface.mowUnload_No1_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 2) m_BoeInterface.mowUnload_No2_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 3) m_BoeInterface.mowUnload_No3_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                    else if (idsarraycount == 4) m_BoeInterface.mowUnload_No4_SubstrateID_Data.SetStates(m_SubstateData, 0, 2);
                                }
                            }

                            //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Substrate ID Data Inside cleaner Req"); // 11.02.18 minhan
                            //m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                        else
                        {
                            if (m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.GetState())
                            {
                                m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(false);
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(true);
                        if (m_Simul.Melsec) m_BoeInterface.mibSubstrate_ID_Data_Inside_Cleaner_Set_Request.SetState(false); // 11.02.17 minhan
                        //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Substrate ID Data Inside cleaner complete"); // 11.02.18 minhan
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 20;
                    }
                    break;
                case 20:
                    {
                        if (!GlobalVar.LoaderReady)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                m_SubstateData[i] = 0;
                            }

                            idsarraycount = 0;
                            m_Buf = "";

                            m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(false);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Substrate ID Data Report Skip(case 20 Loader Ready Off)");
                            nSeqNo = 0;
                        }
                        else if (!m_BoeInterface.mibSubstrate_ID_Data_Inside_Cleaner_Set_Request.GetState())
                        {
                            //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Substrate ID Data Inside cleaner Req: OFF"); // 11.02.18 minhan
                            //m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Substrate ID Data Inside cleaner Complete: OFF");
                            m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(false);
                            nSeqNo = 0;
                        }
                        else if (GetElapsedTicks() > 5000)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                m_SubstateData[i] = 0;
                            }

                            idsarraycount = 0;
                            m_Buf = "";
                            m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(false);

                            m_AlarmId = ALM_SubIDReportTimeout.Id;
                            m_EqpManager.SetAlarm(m_AlarmId);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Substrate ID Data Report Skip(case 20 Loader Ready Off)");
                            nSeqNo = 1000;
                        }
                        else
                        {
                            m_BoeInterface.mobSubstrate_ID_Data_Inside_Cleaner_Set_Completed.SetState(true); // 11.02.17 minhan
                        }
                    }
                    break;
                case 1000: // 11.02.09 minhan
                    {
                        if (m_EqpManager.AlarmResetSwitchPushed) // 11.02.17 minhan
                        {
                            m_EqpManager.ResetAlarm(m_AlarmId);
                            m_AlarmId = 0;
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Alarm Release");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }
    //3.6 Recipe Parameter Change
    public class SeqEqpRecipeParaChange : XSeqFunction // 10.12.25 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static EqpManager m_EqpManager;
        private BOELoaderInterface m_BoeInterface;
        private RecipeProvider m_Provider;
        private Alarm m_ALMChangeReportErr;
        private ushort[] m_Sendata;
        private ushort[] m_RecipeDateTime; // 11.04.18 minhan
        private short m_EventCode;
        private string m_Command;
        private string Msg;
        #endregion

        #region Constructor
        public SeqEqpRecipeParaChange(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Provider = RecipeProvider.Instance;
            m_ALMChangeReportErr = new Alarm("Recipe Change Report ERROR", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_Sendata = new ushort[31];
            m_RecipeDateTime = new ushort[7]; // 11.04.18 minhan
            m_EventCode = 0;
            m_Command = "";
            Msg = "";
        }
        #endregion


        #region Sequence

        public override int Do()
        {

            int nSeqNo = m_SeqNo;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if (m_AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery request");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (GlobalVar.RecipeIdChangeReport &&
                           !GlobalVar.SndRecipeReq)
                        {
                            GlobalVar.RecipeIdChangeReport = false;

                            if (GlobalVar.LoaderReady)
                            {
                                GlobalVar.SndRecipeReq = true;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready On");
                                nSeqNo = 10;
                            }
                            else
                            {
                                m_Server.CommandProc(Command.RecipeSave); // 이런 경우는 저장하자.
                                m_Provider.ChangedId.Clear();
                                GlobalVar.SndRecipeReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                            }
                        }
                    }
                    break;
                case 10:
                    if (m_Provider.ChangedId.Count > 0)
                    {
                        try
                        {
                            TagRecipe m_TagRecipe = new TagRecipe();

                            GlobalVar.ChangedRecipeId = m_Provider.ChangedId[0];
                            m_Provider.ChangedId.RemoveAt(0); ;

                            m_Provider.GetRecipe(GlobalVar.ChangedRecipeId, ref m_TagRecipe);

                            for (int i = 0; i < 31; i++)
                            {
                                m_Sendata[i] = 0;
                            }

                            for (int k = 0; k < 7; k++) // 11.04.18 minhan
                            {
                                m_RecipeDateTime[k] = 0;
                            }

                            m_Command = "";
                            m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                            m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                            m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                            int RecipeID = 0;

                            if (int.TryParse(m_TagRecipe.Id, out RecipeID)) // 11.02.09 minhan
                            {
                                m_Command = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.04.18 minhan
                                XFunc.ConvertToWord(m_Command, ref m_RecipeDateTime, 0, 7, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_TagRecipe.TactTime);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 0, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.CvSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 2, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP1_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 4, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP2_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP3_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApVoltage);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.ApN2Flow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 2, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApCDAFlow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 8, 1, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", RecipeID);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 7, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.MjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 9, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.HpmjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 10, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.HpmjPressure);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 11, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 13, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 14, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 15, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 16, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 17, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 18, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 19, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 20, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 21, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 23, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 25, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 27, 2, ByteOrder.BigEndian);

                                m_Command = "03";
                                m_EventCode = XFunc.ConvertToWord(m_Command, ByteOrder.BigEndian);
                                m_BoeInterface.mow_Recipe_Event.SetState((ushort)m_EventCode); // Event Code

                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // Set

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int m = 0; m < 7; m++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[m] = 0;
                                }

                                m_Command = "";

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Change Report Write");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                            }
                            else
                            {

                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                Msg = string.Format("Recipe ID Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);

                                if (m_AlarmId == 0)
                                {
                                    m_AlarmId = m_ALMChangeReportErr.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }

                                m_Provider.ViewerHold(false);
                                m_Provider.ChangedId.Clear();
                                GlobalVar.SndRecipeReq = false;
                                nSeqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                m_Provider.RejectChange(GlobalVar.ChangedRecipeId); // 11.02.17 minahn

                                m_AlarmId = m_ALMChangeReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Msg = string.Format("Recipe Parameter Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }

                            m_Provider.ViewerHold(false);
                            m_Provider.ChangedId.Clear();
                            GlobalVar.SndRecipeReq = false;
                            nSeqNo = 0;
                        }
                    }
                    else
                    {
                        m_Provider.ChangedId.Clear();
                        m_Server.CommandProc(Command.RecipeSave);
                        GlobalVar.SndRecipeReq = false;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Change ID Nothing");
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Change Report Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Change Report Comp On");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 40;
                    }
                    else if (GetElapsedTicks() > 5000 || !GlobalVar.LoaderReady) // 일단 10초 두는데.
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        GlobalVar.SndRecipeReq = false;
                        m_Provider.ViewerHold(false); // 이거 좋구나.
                        m_Provider.RejectChange(GlobalVar.ChangedRecipeId);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Changed Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        Msg = string.Format("{0},CHANGE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (!m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(true);
                    }
                    break;
                case 40:
                    if (!m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_Server.CommandProc(Command.RecipeSave);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},CHANGE,SUCCESS,EQP", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Change Complete");

                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 3000 || !GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        GlobalVar.SndRecipeReq = false;
                        m_Provider.ViewerHold(false); // 이거 좋구나.
                        m_Provider.RejectChange(GlobalVar.ChangedRecipeId);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Changed Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        Msg = string.Format("{0},CHANGE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(false);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEqpRecipeParaAdd : XSeqFunction // 10.12.25 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static EqpManager m_EqpManager;
        private BOELoaderInterface m_BoeInterface;
        private RecipeProvider m_Provider;
        private Alarm m_ALMAddReportErr;
        private ushort[] m_Sendata;
        private ushort[] m_RecipeDateTime; // 11.04.18 minhan
        private short m_EventCode;
        private string m_Command;
        private string Msg;
        #endregion

        #region Constructor
        public SeqEqpRecipeParaAdd(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Provider = RecipeProvider.Instance;
            m_ALMAddReportErr = new Alarm("Recipe Add Report ERROR", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_Sendata = new ushort[31];
            m_RecipeDateTime = new ushort[7]; // 11.04.18 minhan
            m_EventCode = 0;
            m_Command = "";
            Msg = "";
        }
        #endregion


        #region Sequence

        public override int Do()
        {

            int nSeqNo = m_SeqNo;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if (m_AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery request");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (GlobalVar.RecipeAddReport &&
                           !GlobalVar.SndRecipeReq)
                        {
                            GlobalVar.SndRecipeReq = true;
                            GlobalVar.RecipeAddReport = false;

                            if (GlobalVar.LoaderReady)
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready On");
                                nSeqNo = 10;
                            }
                            else
                            {
                                if (m_Provider.ChangedId.Count > 0)
                                {
                                    int nCnt = m_Provider.ChangedId.Count;
                                    m_Provider.ChangedId.RemoveAt(nCnt - 1);

                                    m_Server.CommandProc(Command.RecipeAdd, GlobalVar.ChangeRecipe);

                                    if (m_Provider.ChangedId.Count > 0)
                                    {
                                        GlobalVar.RecipeIdChangeReport = true;
                                    }
                                    else
                                    {
                                        m_Provider.ChangedId.Clear();
                                    }
                                }
                                else
                                {
                                    m_Provider.ViewerHold(false);
                                    m_Provider.ChangedId.Clear();
                                }

                                GlobalVar.SndRecipeReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                            }
                        }
                    }
                    break;
                case 10:
                    if (m_Provider.ChangedId.Count > 0)
                    {
                        try
                        {
                            TagRecipe m_TagRecipe = GlobalVar.ChangeRecipe;

                            int nCnt = m_Provider.ChangedId.Count;
                            GlobalVar.ChangedRecipeId = m_Provider.ChangedId[nCnt - 1];
                            m_Provider.ChangedId.RemoveAt(nCnt - 1); ;

                            for (int i = 0; i < 31; i++)
                            {
                                m_Sendata[i] = 0;
                            }

                            for (int k = 0; k < 7; k++) // 11.04.18 minhan
                            {
                                m_RecipeDateTime[k] = 0;
                            }

                            m_Command = "";
                            m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                            m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                            m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                            // 나중에 순서를 적용해서 정리하려면 recipe 프로젝트 고쳐야 함 진짜 내가 해 놓고도 이해하세요.
                            // 소수점 자리는 워드 영역을 보니 자리수가 모자란다. 협의가 필요하지 않을까.
                            int RecipeID = 0;

                            if (int.TryParse(m_TagRecipe.Id, out RecipeID)) // 11.02.09 minhan
                            {
                                m_Command = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.04.18 minhan
                                XFunc.ConvertToWord(m_Command, ref m_RecipeDateTime, 0, 7, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_TagRecipe.TactTime);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 0, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.CvSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 2, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP1_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 4, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP2_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP3_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 1, ByteOrder.BigEndian);
                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApVoltage);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.ApN2Flow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 2, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApCDAFlow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 8, 1, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", RecipeID);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 7, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.MjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 9, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.HpmjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 10, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.HpmjPressure);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 11, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 13, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 14, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 15, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 16, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 17, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 18, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 19, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 20, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 21, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 23, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 25, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 27, 2, ByteOrder.BigEndian);



                                m_Command = "02";
                                m_EventCode = XFunc.ConvertToWord(m_Command, ByteOrder.BigEndian);
                                m_BoeInterface.mow_Recipe_Event.SetState((ushort)m_EventCode); // Event Code

                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // Set

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int m = 0; m < 7; m++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[m] = 0;
                                }

                                m_Command = "";

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add Report Write");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                            }
                            else
                            {

                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                Msg = string.Format("Recipe ID Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);

                                if (m_AlarmId == 0)
                                {
                                    m_AlarmId = m_ALMAddReportErr.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }

                                m_Provider.ViewerHold(false);
                                m_Provider.ChangedId.Clear();
                                GlobalVar.SndRecipeReq = false;
                                nSeqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_Server.CommandProc(Command.RecipeAddFail, GlobalVar.ChangedRecipeId); // 11.02.17 minhan

                                m_AlarmId = m_ALMAddReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Msg = string.Format("Recipe Parameter Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }

                            m_Provider.ViewerHold(false);
                            m_Provider.ChangedId.Clear();
                            GlobalVar.SndRecipeReq = false;
                            nSeqNo = 0;
                        }
                    }
                    else
                    {
                        m_Provider.ViewerHold(false);
                        m_Provider.ChangedId.Clear();
                        GlobalVar.SndRecipeReq = false;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add ID Nothing");
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add Report Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add Report Comp On");
                        m_StartTicks = XFunc.GetTickCount();
                        nSeqNo = 40;
                    }
                    else if (GetElapsedTicks() > 5000 || !GlobalVar.LoaderReady) // 일단 10초 두는데.
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeAddFail, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},ADD,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (!m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(true);
                    }
                    break;
                case 40:
                    if (!m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_Server.CommandProc(Command.RecipeAdd, GlobalVar.ChangeRecipe);


                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},ADD,SUCCESS,EQP", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Add Complete");

                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 3000)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Add Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeAddFail, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},ADD,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(false);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEqpRecipeParaDelete : XSeqFunction // 10.12.25 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static EqpManager m_EqpManager;
        private BOELoaderInterface m_BoeInterface;
        private RecipeProvider m_Provider;
        private Alarm m_ALMDeleteReportErr;
        private ushort[] m_Sendata;
        private ushort[] m_RecipeDateTime; // 11.04.18 minhan
        private short m_EventCode;
        private string m_Command;
        private string Msg;
        #endregion

        #region Constructor
        public SeqEqpRecipeParaDelete(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Provider = RecipeProvider.Instance;
            m_ALMDeleteReportErr = new Alarm("Recipe Delete Report ERROR", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_Sendata = new ushort[31];
            m_RecipeDateTime = new ushort[7]; // 11.04.18 minhan
            m_EventCode = 0;
            m_Command = "";
            Msg = "";
        }
        #endregion


        #region Sequence

        public override int Do()
        {

            int nSeqNo = m_SeqNo;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if (m_AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery request");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (GlobalVar.RecipeDeleteReport &&
                            !GlobalVar.SndRecipeReq)
                        {
                            GlobalVar.SndRecipeReq = true;
                            GlobalVar.RecipeDeleteReport = false;

                            if (GlobalVar.LoaderReady)
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready On");
                                nSeqNo = 10;
                            }
                            else
                            {
                                if (m_Provider.ChangedId.Count > 0)
                                {
                                    int nCnt = m_Provider.ChangedId.Count;

                                    m_Server.CommandProc(Command.RecipeRemove, m_Provider.ChangedId[nCnt - 1]);
                                    m_Provider.ChangedId.RemoveAt(nCnt - 1);

                                    if (m_Provider.ChangedId.Count > 0)
                                    {
                                        GlobalVar.RecipeIdChangeReport = true;
                                    }
                                    else
                                    {
                                        m_Provider.ChangedId.Clear();
                                    }
                                }
                                else
                                {
                                    m_Provider.ViewerHold(false);

                                    m_Provider.ChangedId.Clear();
                                }

                                GlobalVar.SndRecipeReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                            }
                        }
                    }
                    break;
                case 10:
                    if (m_Provider.ChangedId.Count > 0)
                    {
                        try
                        {
                            TagRecipe m_TagRecipe = new TagRecipe();

                            int nCnt = m_Provider.ChangedId.Count;
                            GlobalVar.ChangedRecipeId = m_Provider.ChangedId[nCnt - 1];
                            m_Provider.ChangedId.RemoveAt(nCnt - 1); ;

                            m_Server.DataProvider.RecipeProvider.GetRecipe(GlobalVar.ChangedRecipeId, ref m_TagRecipe);

                            for (int i = 0; i < 31; i++)
                            {
                                m_Sendata[i] = 0;
                            }

                            for (int k = 0; k < 7; k++) // 11.04.18 minhan
                            {
                                m_RecipeDateTime[k] = 0;
                            }

                            m_Command = "";
                            m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                            m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                            m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                            int RecipeID = 0;

                            if (int.TryParse(m_TagRecipe.Id, out RecipeID))
                            {
                                m_Command = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.04.18 minhan
                                XFunc.ConvertToWord(m_Command, ref m_RecipeDateTime, 0, 7, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_TagRecipe.TactTime);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 0, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.CvSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 2, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP1_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 4, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP2_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP3_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApVoltage);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.ApN2Flow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 2, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApCDAFlow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 8, 1, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", RecipeID);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 7, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.MjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 9, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.HpmjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 10, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.HpmjPressure);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 11, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 13, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 14, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 15, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 16, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 17, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 18, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 19, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 20, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 21, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 23, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 25, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 27, 2, ByteOrder.BigEndian);


                                m_Command = "04";
                                m_EventCode = XFunc.ConvertToWord(m_Command, ByteOrder.BigEndian);
                                m_BoeInterface.mow_Recipe_Event.SetState((ushort)m_EventCode); // Event Code

                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // Set

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int m = 0; m < 7; m++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[m] = 0;
                                }

                                m_Command = "";

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete Report Write");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                            }
                            else
                            {

                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                Msg = string.Format("Recipe ID Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);

                                if (m_AlarmId == 0)
                                {
                                    m_AlarmId = m_ALMDeleteReportErr.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }

                                m_Provider.ViewerHold(false);
                                m_Provider.ChangedId.Clear();
                                GlobalVar.SndRecipeReq = false;
                                nSeqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                m_Server.CommandProc(Command.RecipeRemoveFail, GlobalVar.ChangedRecipeId); // 11.02.17 minhan

                                m_AlarmId = m_ALMDeleteReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Msg = string.Format("Recipe Parameter Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }

                            m_Provider.ViewerHold(false);
                            m_Provider.ChangedId.Clear();
                            GlobalVar.SndRecipeReq = false;
                            nSeqNo = 0;
                        }
                    }
                    else
                    {
                        m_Provider.ViewerHold(false);
                        m_Provider.ChangedId.Clear();
                        GlobalVar.SndRecipeReq = false;
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete ID Nothing");
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete Report Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete Report Comp On");
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 40;
                    }
                    else if (GetElapsedTicks() > 5000 || !GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeRemoveFail, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},DELETE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (!m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(true);
                    }
                    break;
                case 40:
                    if (!m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_Server.CommandProc(Command.RecipeRemove, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},DELETE,SUCCESS,EQP", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Delete Complete");

                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 3000 || !GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Delete Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeRemoveFail, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},DELETE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(false);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqEqpRecipeParaCopy : XSeqFunction // 10.12.25 minhan
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static EqpManager m_EqpManager;
        private BOELoaderInterface m_BoeInterface;
        private RecipeProvider m_Provider;
        private Alarm m_ALMCopyReportErr;
        private ushort[] m_Sendata;
        private ushort[] m_RecipeDateTime; // 11.04.18 minhan
        private short m_EventCode;
        private string m_Command;
        private string Msg;
        #endregion

        #region Constructor
        public SeqEqpRecipeParaCopy(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Provider = RecipeProvider.Instance;
            m_ALMCopyReportErr = new Alarm("Recipe Copy Report ERROR", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            m_Sendata = new ushort[31];
            m_RecipeDateTime = new ushort[7]; // 11.04.18 minhan
            m_EventCode = 0;
            m_Command = "";
            Msg = "";
        }
        #endregion


        #region Sequence

        public override int Do()
        {

            int nSeqNo = m_SeqNo;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if (m_AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery request");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (GlobalVar.RecipeCopyReport &&
                           !GlobalVar.SndRecipeReq)
                        {
                            GlobalVar.SndRecipeReq = true;
                            GlobalVar.RecipeCopyReport = false;

                            if (GlobalVar.LoaderReady)
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready On");
                                nSeqNo = 10;
                            }
                            else
                            {
                                if (m_Provider.ChangedId.Count > 0)
                                {
                                    int nCnt = m_Provider.ChangedId.Count;

                                    m_Provider.ChangedId.RemoveAt(nCnt - 1);
                                    m_Server.CommandProc(Command.RecipeCopy, GlobalVar.SourceRecipeId, GlobalVar.ChangedRecipeId);

                                    if (m_Provider.ChangedId.Count > 0)
                                    {
                                        GlobalVar.RecipeIdChangeReport = true;
                                    }
                                    else
                                    {
                                        m_Provider.ChangedId.Clear();
                                    }
                                }
                                else
                                {
                                    m_Provider.ViewerHold(false);

                                    m_Provider.ChangedId.Clear();
                                }

                                GlobalVar.SndRecipeReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                            }
                        }
                    }
                    break;
                case 10:
                    if (m_Provider.ChangedId.Count > 0)
                    {
                        try
                        {
                            TagRecipe m_TagRecipe = new TagRecipe();

                            int nCnt = m_Provider.ChangedId.Count;
                            GlobalVar.ChangedRecipeId = m_Provider.ChangedId[nCnt - 1];
                            m_Provider.ChangedId.RemoveAt(nCnt - 1); ;

                            //m_Server.DataProvider.RecipeProvider.GetRecipe(GlobalVar.ChangedRecipeId, ref m_TagRecipe);

                            for (int i = 0; i < 31; i++)
                            {
                                m_Sendata[i] = 0;
                            }

                            for (int k = 0; k < 7; k++) // 11.04.18 minhan
                            {
                                m_RecipeDateTime[k] = 0;
                            }

                            m_Command = "";
                            m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                            m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                            m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                            int RecipeID = 0;

                            if (int.TryParse(GlobalVar.ChangedRecipeId, out RecipeID)) // 11.02.17 minhan
                            {
                                m_Server.DataProvider.RecipeProvider.GetRecipe(GlobalVar.SourceRecipeId, ref m_TagRecipe);

                                m_Command = DateTime.Now.ToString("yyyyMMddHHmmss"); // 11.04.18 minhan
                                XFunc.ConvertToWord(m_Command, ref m_RecipeDateTime, 0, 7, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_TagRecipe.TactTime);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 0, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.CvSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 2, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP1_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 4, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP2_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP3_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 1, ByteOrder.BigEndian);
                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApVoltage);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.ApN2Flow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 2, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApCDAFlow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 8, 1, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", RecipeID);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 7, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.MjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 9, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.HpmjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 10, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.HpmjPressure);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 11, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 13, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 14, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 15, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 16, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 17, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 18, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 19, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 20, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 21, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 23, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 25, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 27, 2, ByteOrder.BigEndian);

                                m_Command = "03"; // Copy 도 03으로 보내삼.
                                m_EventCode = XFunc.ConvertToWord(m_Command, ByteOrder.BigEndian);
                                m_BoeInterface.mow_Recipe_Event.SetState((ushort)m_EventCode); // Event Code

                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // Set

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int m = 0; m < 7; m++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[m] = 0;
                                }

                                m_Command = "";

                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy Report Write");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 20;
                            }
                            else
                            {
                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                Msg = string.Format("Recipe ID Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);

                                if (m_AlarmId == 0)
                                {
                                    m_AlarmId = m_ALMCopyReportErr.Id;
                                    m_EqpManager.SetAlarm(m_AlarmId);
                                }

                                m_Provider.ViewerHold(false);
                                m_Provider.ChangedId.Clear();
                                GlobalVar.SndRecipeReq = false;
                                nSeqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                for (int i = 0; i < 31; i++)
                                {
                                    m_Sendata[i] = 0;
                                }

                                for (int j = 0; j < 7; j++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[j] = 0;
                                }

                                m_Command = "";
                                m_BoeInterface.mow_Recipe_Event.SetState(0); // clear
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                m_Server.CommandProc(Command.RecipeCopyFail, GlobalVar.SourceRecipeId, GlobalVar.ChangedRecipeId); // 11.02.17 minhan

                                m_AlarmId = m_ALMCopyReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Msg = string.Format("Recipe Parameter Error ID : {0}", GlobalVar.ChangedRecipeId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }

                            m_Provider.ViewerHold(false);
                            m_Provider.ChangedId.Clear();
                            GlobalVar.SndRecipeReq = false;
                            nSeqNo = 0;
                        }
                    }
                    else
                    {
                        m_Provider.ViewerHold(false);
                        GlobalVar.SndRecipeReq = false;
                        m_Provider.ChangedId.Clear();
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy ID Nothing");
                        nSeqNo = 0;
                    }
                    break;
                case 20:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy Report Request");
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    if (m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy Report Comp On");
                        m_StartTicks = XFunc.GetTickCount();

                        nSeqNo = 40;
                    }
                    else if (GetElapsedTicks() > 5000 || !GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeCopyFail, GlobalVar.SourceRecipeId, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},CHANGE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (!m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(true);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(true);
                    }
                    break;
                case 40:
                    if (!m_BoeInterface.mibRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_Server.CommandProc(Command.RecipeCopy, GlobalVar.SourceRecipeId, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},CHANGE,SUCCESS,EQP", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);
                        m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Copy Complete");

                        nSeqNo = 0;
                    }
                    else if (GetElapsedTicks() > 10000 || !GlobalVar.LoaderReady)
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);

                        if (GlobalVar.LoaderReady)
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Copy Timeover");
                        }
                        else
                        {
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "LoaderReady Off");
                        }

                        m_Server.CommandProc(Command.RecipeCopyFail, GlobalVar.SourceRecipeId, GlobalVar.ChangedRecipeId);

                        if (m_Provider.ChangedId.Count > 0)
                        {
                            GlobalVar.RecipeIdChangeReport = true;
                        }

                        GlobalVar.SndRecipeReq = false;
                        Msg = string.Format("{0},CHANGE,FAIL,EQP,1", GlobalVar.ChangedRecipeId);
                        m_Provider.RecipeDialogInfo.Add(Msg);

                        nSeqNo = 0;

                    }
                    else if (m_BoeInterface.mobRecipe_Parameter_Change_Req.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Req.SetState(false);
                    }
                    else if ((GetElapsedTicks() > 500) && AppConfig.Instance.Simul.Melsec)
                    {
                        m_BoeInterface.mibRecipe_Parameter_Change_Complete.SetState(false);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqHostRecipeBodyReport : XSeqFunction
    // 10.12.25 minhan 이거 사양이 Loader는 Change를 사용하지 않는 것으로 되어 있으니 그냥 Report 형식 아닌 가벼. 사양 협의 필요.
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static EqpManager m_EqpManager;
        private BOELoaderInterface m_BoeInterface;
        private RecipeProvider m_Provider;
        private Alarm m_ALMBodyReportErr;
        //private short[] m_RecvID; // 11.02.18 minhan
        private ushort[] m_Sendata;
        private ushort[] m_RecipeDateTime; // 11.04.18 minhan
        private short m_EventCode;
        private string m_Command;
        private string Msg;
        private bool m_chkvalue;
        #endregion

        #region Constructor
        public SeqHostRecipeBodyReport(string seqName)
        {
            m_SeqFunName = seqName;
            m_Server = ServerManager.Instance;
            m_EqpManager = EqpManager.Instance;
            m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
            m_Provider = RecipeProvider.Instance;
            m_ALMBodyReportErr = new Alarm("Recipe Body Report ERROR", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
            //m_RecvID = new short[2];
            m_Sendata = new ushort[31];
            m_RecipeDateTime = new ushort[7]; // 11.04.18 minhan
            m_EventCode = 0;
            m_Command = "";
            Msg = "";
        }
        #endregion


        #region Sequence

        public override int Do()
        {

            int nSeqNo = m_SeqNo;

            if (m_EqpManager.AlarmResetSwitchPushed)
            {
                if (m_AlarmId != 0)
                {
                    m_EqpManager.ResetAlarm(m_AlarmId);
                    m_AlarmId = 0;

                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Error Recovery request");
                }
            }

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_BoeInterface.mibRecipe_Parameter_Change_Req.GetState() && !GlobalVar.SndRecipeReq)
                        // 사양을 보면 우리가 보고 하는 것도 있던데 RecipeBodyreport 나중에 사용하면, 협의 필요.
                        {
                            GlobalVar.SndRecipeReq = true;

                            if (GlobalVar.LoaderReady)
                            {
                                m_EventCode = 0;
                                m_Command = "";
                                m_chkvalue = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready On");
                                nSeqNo = 10;
                            }
                            else
                            {
                                GlobalVar.SndRecipeReq = false;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                            }
                        }
                        else if (m_BoeInterface.mobRecipe_Parameter_Change_Complete.GetState())
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Complete.SetState(false);
                        }
                    }
                    break;
                case 10:
                    {
                        try
                        {
                            m_EventCode = m_BoeInterface.miw_Recipe_Event.GetState();
                            m_Command = XFunc.ConvertToString(m_EventCode, ByteOrder.BigEndian);

                            if (m_Command == "01") // code
                            {
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Body Request Confirm");
                                nSeqNo = 20;
                            }
                            else if (!m_BoeInterface.mibRecipe_Parameter_Change_Req.GetState() || !GlobalVar.LoaderReady) // return
                            {
                                m_EventCode = 0;
                                m_Command = "";
                                GlobalVar.SndRecipeReq = false;

                                if (!GlobalVar.LoaderReady)
                                {
                                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Loader Ready Off");
                                }
                                else
                                {
                                    m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Body Request Off");
                                }
                                nSeqNo = 0;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                //for (int i = 0; i < 2; i++)
                                //{
                                //    m_RecvID[i] = 0;
                                //}

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int k = 0; k < 7; k++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[k] = 0;
                                }

                                m_Command = "";
                                m_chkvalue = false;
                                m_EventCode = 0;
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length);
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                m_AlarmId = m_ALMBodyReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);

                                Msg = "Recipe Parameter Format Error(case 10)";
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }

                            GlobalVar.SndRecipeReq = false;
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 20:
                    {
                        try
                        {
                            TagRecipe m_TagRecipe = new TagRecipe();

                            //for (int i = 0; i < 2; i++)
                            //{
                            //    m_RecvID[i] = 0;
                            //}

                            for (int j = 0; j < 31; j++)
                            {
                                m_Sendata[j] = 0;
                            }

                            for (int k = 0; k < 7; k++) // 11.04.18 minhan
                            {
                                m_RecipeDateTime[k] = 0;
                            }

                            m_Command = "";

                            //m_RecvID = m_BoeInterface.miwRecipe_Para_Form_MainEQ.GetStates(2); // 11.02.18 minhan
                            //GlobalVar.RecipeBodyId = XFunc.ConvertToString(m_RecvID, 0, 2, ByteOrder.BigEndian);

                            GlobalVar.RecipeBodyId = GenInfoHandler.Instance.CurRecipeId; // 11.02.18 minhan
                            int m_RecipeId = 0;

                            //if ((m_Provider.Adapter.IsExist(GlobalVar.RecipeBodyId)) && int.TryParse(GlobalVar.RecipeBodyId, out m_RecipeId)) // 있다면...
                            if ((m_Provider.GetRecipe(GlobalVar.RecipeBodyId, ref m_TagRecipe)) && int.TryParse(GlobalVar.RecipeBodyId, out m_RecipeId)) // 11.02.18 minhan
                            {
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan

                                // 나중에 순서를 적용해서 정리하려면 recipe 프로젝트 고쳐야 함 진짜 내가 해 놓고도 이해하세요.

                                m_Command = m_TagRecipe.ChangedTime.ToString("yyyyMMddHHmmss"); // 11.04.18 minhan
                                XFunc.ConvertToWord(m_Command, ref m_RecipeDateTime, 0, 7, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_TagRecipe.TactTime);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 0, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.CvSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 2, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP1_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 4, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP2_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.EUVLAMP3_USE) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApVoltage);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 5, 1, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.ApN2Flow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 6, 2, ByteOrder.BigEndian);

                                //m_Command = "";
                                //m_Command = string.Format("{0:d2}", (uint)m_TagRecipe.ApCDAFlow);
                                //XFunc.ConvertToWord(m_Command, ref m_Sendata, 8, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.MjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 9, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.HpmjUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 10, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.HpmjPressure);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 11, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 13, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 14, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 15, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoUse) m_Command = "01";
                                else m_Command = "00";
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 16, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 17, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB1LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 18, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2UpDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 19, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                if (m_TagRecipe.RB2LoDir) m_Command = "00"; // cw
                                else m_Command = "01"; //ccw
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 20, 1, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 21, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB1LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 23, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2UpSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 25, 2, ByteOrder.BigEndian);

                                m_Command = "";
                                m_Command = string.Format("{0:d4}", (uint)m_TagRecipe.RB2LoSpeed);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 27, 2, ByteOrder.BigEndian);

                                m_Command = string.Format("{0:d4}", m_RecipeId);
                                XFunc.ConvertToWord(m_Command, ref m_Sendata, 29, 2, ByteOrder.BigEndian);

                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // Set

                                //for (int i = 0; i < 2; i++)
                                //{
                                //    m_RecvID[i] = 0;
                                //}

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int m = 0; m < 7; m++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[m] = 0;
                                }

                                m_Command = "";
                                m_chkvalue = true;
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Body Report Write");
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 30;
                            }
                            else // 없다면. 클리어하고 가자.
                            {
                                //for (int i = 0; i < 2; i++)
                                //{
                                //    m_RecvID[i] = 0;
                                //}

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int k = 0; k < 7; k++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[k] = 0;
                                }

                                m_Command = "";
                                m_chkvalue = false;
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear

                                //if (AlarmId == 0) // 11.02.17 minhan
                                //{
                                //    AlarmId = m_ALMBodyReportErr.Id;
                                //    m_EqpManager.SetAlarm(AlarmId);
                                //}
                                Msg = string.Format("Recipe Body Report Write(Noting ID) : {0}", GlobalVar.RecipeBodyId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_StartTicks = XFunc.GetTickCount();
                                nSeqNo = 30;
                            }
                        }
                        catch (Exception err)
                        {
                            if (m_AlarmId == 0)
                            {
                                //for (int i = 0; i < 2; i++)
                                //{
                                //    m_RecvID[i] = 0;
                                //}

                                for (int j = 0; j < 31; j++)
                                {
                                    m_Sendata[j] = 0;
                                }

                                for (int k = 0; k < 7; k++) // 11.04.18 minhan
                                {
                                    m_RecipeDateTime[k] = 0;
                                }

                                m_Command = "";
                                m_chkvalue = false;
                                m_BoeInterface.mowRecipe_DateTime.SetStates(m_RecipeDateTime, 0, m_RecipeDateTime.Length); // 11.04.18 minhan
                                m_BoeInterface.mowRecipe_parameter.SetStates(m_Sendata, 0, m_Sendata.Length); // clear

                                m_AlarmId = m_ALMBodyReportErr.Id;
                                m_EqpManager.SetAlarm(m_AlarmId);
                                Msg = string.Format("Recipe Parameter Error ID : {0}", GlobalVar.RecipeBodyId);
                                m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, Msg);
                                m_Server.WriteExceptionLog(err.ToString());
                            }
                            m_StartTicks = XFunc.GetTickCount();
                            nSeqNo = 30;
                        }
                    }
                    break;
                case 30:
                    {
                        if (GetElapsedTicks() > 100)
                        {
                            m_BoeInterface.mobRecipe_Parameter_Change_Complete.SetState(true);
                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Body Report Request");
                            nSeqNo = 40;
                        }
                    }
                    break;
                case 40: // 여기는 off 되는 걸 보니 정지하는 현상은 거의 없겠지.
                    if (!m_BoeInterface.mibRecipe_Parameter_Change_Req.GetState()) // 11.02.17 minhan
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Complete.SetState(false);

                        if (m_chkvalue)
                        {
                            Msg = string.Format("{0},CHANGE,SUCCESS,HOST", GlobalVar.RecipeBodyId);
                            m_Provider.RecipeDialogInfo.Add(Msg);

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Change Complete");
                        }
                        else
                        {
                            Msg = string.Format("{0},CHANGE,FAIL,HOST,1", GlobalVar.RecipeBodyId);
                            m_Provider.RecipeDialogInfo.Add(Msg);

                            m_Server.SetInterfaceLog(m_SeqFunName, 0, 0, "Recipe Parameter Change NG");
                        }

                        Msg = "";
                        m_chkvalue = false;
                        GlobalVar.SndRecipeReq = false;
                        nSeqNo = 0;
                    }
                    else if (!m_BoeInterface.mobRecipe_Parameter_Change_Complete.GetState())
                    {
                        m_BoeInterface.mobRecipe_Parameter_Change_Complete.SetState(true);
                    }
                    break;
            }
            this.m_SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    //public class SeqCrackReport : XSeqFunction // 11.03.02 minhan
    //{
    //    #region Fields
    //    private ServerManager m_Server;
    //    private EqpManager m_EqpManager;
    //    private Simul m_Simul;
    //    private BOELoaderInterface m_BoeInterface;
    //    private EGiSInterface m_EgisInterface;
    //    private Alarm ALM_CRACKReportError;
    //    private bool m_Use;
    //    #endregion

    //    #region Constructor
    //    public SeqCrackReport(string seqName)
    //    {
    //        SeqFunName = seqName;

    //        m_Server = ServerManager.Instance;
    //        m_Simul = AppConfig.Instance.Simul;
    //        m_EqpManager = EqpManager.Instance;
    //        m_BoeInterface = eqpBOELoaderInterfaces._LoaderInterface;
    //        m_EgisInterface = eqpEGiSInterfaces._BoeEGiSInterface;
    //        ALM_CRACKReportError = new Alarm("Loader Crack Report Timeout Alarm", AlarmLevel.L, AlarmCode.EquipmentStatusWarning);
    //        m_Use = false;
    //    }
    //    #endregion

    //    #region Methods
    //    public override int Do()
    //    {
    //        int nSeqNo = SeqNo;

    //        m_Use = m_EgisInterface.SetupCrackLoaderUse.GetValue<bool>();

    //        switch (nSeqNo)
    //        {
    //            case 0:
    //                if (m_Use)
    //                {
    //                    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Loader Crack Unit Use");
    //                    nSeqNo = 10;
    //                }
    //                else
    //                {
    //                    if (GlobalVar.CrackReportReq) GlobalVar.CrackReportReq = false; // 11.03.07 minhan
    //                    if (m_BoeInterface.mobCrack_NG_Request.GetState()) m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                }
    //                break;
    //            case 10:
    //                {
    //                    if (!m_Use)
    //                    {
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Loader Crack Unit No Use");
    //                        nSeqNo = 0;
    //                    }
    //                    else if (GlobalVar.CrackReportReq)
    //                    {
    //                        GlobalVar.CrackReportReq = false;

    //                        m_BoeInterface.mobCrack_NG_Request.SetState(true);
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Loader Crack Unit NG Request");

    //                        if (m_Simul.Melsec) m_BoeInterface.mibCrack_NG_Complete.SetState(true);
    //                        m_StartTicks = XFunc.GetTickCount();
    //                        nSeqNo = 20;
    //                    }
    //                    else
    //                    {
    //                        if (m_BoeInterface.mobCrack_NG_Request.GetState()) m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                    }
    //                }
    //                break;
    //            case 20: // 이미 보고한 상태라면 no use는 보지 않는다.
    //                {
    //                    if (!GlobalVar.LoaderReady)
    //                    {
    //                        m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Skip( case 20 Loader Ready Off)");
    //                        nSeqNo = 0;
    //                    }
    //                    else if (m_BoeInterface.mibCrack_NG_Complete.GetState())
    //                    {
    //                        m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                        if (m_Simul.Melsec) m_BoeInterface.mibCrack_NG_Complete.SetState(false);

    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Complete ON");
    //                        m_StartTicks = XFunc.GetTickCount();
    //                        nSeqNo = 30;
    //                    }
    //                    else if (GetElapsedTicks() > 4000)
    //                    {
    //                        AlarmId = ALM_CRACKReportError.Id;
    //                        m_EqpManager.SetAlarm(AlarmId);
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Time Out(case 20)");
    //                        nSeqNo = 1000;
    //                    }
    //                    else
    //                    {
    //                        if (!m_BoeInterface.mobCrack_NG_Request.GetState()) m_BoeInterface.mobCrack_NG_Request.SetState(true);
    //                    }
    //                }
    //                break;
    //            case 30:
    //                {
    //                    if (!GlobalVar.LoaderReady)
    //                    {
    //                        m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Skip( case 30 Loader Ready Off)");
    //                        nSeqNo = 0;
    //                    }
    //                    else if (!m_BoeInterface.mibCrack_NG_Complete.GetState())
    //                    {
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Complete OFF");
    //                        nSeqNo = 0;
    //                    }
    //                    else if (GetElapsedTicks() > 4000)
    //                    {
    //                        AlarmId = ALM_CRACKReportError.Id;
    //                        m_EqpManager.SetAlarm(AlarmId);
    //                        m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Crack Report Time Out(case 30)");
    //                        nSeqNo = 1000;
    //                    }
    //                    else
    //                    {
    //                        if (m_BoeInterface.mobCrack_NG_Request.GetState()) m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                    }
    //                }
    //                break;
    //            case 1000:
    //                if (m_EqpManager.AlarmResetSwitchPushed)
    //                {
    //                    m_BoeInterface.mobCrack_NG_Request.SetState(false);
    //                    m_EqpManager.ResetAlarm(AlarmId);
    //                    AlarmId = 0;
    //                    m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Alarm Release");
    //                    nSeqNo = 0;
    //                }
    //                break;
    //        }
    //        this.SeqNo = nSeqNo;

    //        return -1;
    //    }
    //    #endregion
    //}
}

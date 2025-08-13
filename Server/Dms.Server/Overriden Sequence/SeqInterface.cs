using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Ctl;
using System.Windows.Forms;
using Dms.Data;
using Dms.Device;
using Dms.Sequence;

namespace Dms.Server
{
    public class ThreadInterface : XSequence
    {
        #region Fields
        protected static ServerManager m_Server; 
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            RegisterSequence(new SeqRecv("Recv1", m_Server.RootNode.IfFromUp1, m_Server.RootNode.IfToUp1, eqpInterfaceSteps._RecvStep));
            RegisterSequence(new SeqSend("Send1", m_Server.RootNode.IfFromDn1, m_Server.RootNode.IfToDn1, eqpInterfaceSteps._SendStep));
        } 
        #endregion

        #region Constructor
        public ThreadInterface(int scanTime, ServerManager server)
            : base(scanTime)
        {
            m_Server = server;

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
        #endregion
    }

    public class SeqRecv : XSeqFunction
    {
        #region Enum
        private enum GLS_CHECK
        {
            NONE, OK, RECIPE_NG, DATA_NG
        }
        #endregion

        #region Fields
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManger;
        private Simul m_Simul;
        private IfSigFromUp m_IfFromUp;
        private IfSigToUp m_IfToUp;
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private GLS_CHECK m_Result = GLS_CHECK.NONE;
        private InterfaceStep m_Step;
        #endregion

        #region Constructor
        public SeqRecv(string seqName, IfSigFromUp sigFromUp, IfSigToUp sigToUp, InterfaceStep step)
        {
            SeqFunName = seqName;
            
            m_Server = ServerManager.Instance;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;

            m_IfFromUp = sigFromUp;
            m_IfToUp = sigToUp;
            m_Step = step;
        }
        #endregion

        #region Methods
        private GLS_CHECK ReadGlassData()
        {
            GLS_CHECK nRv = GLS_CHECK.NONE;

            if (!m_Simul.Melsec)
            {
                m_Server.RecvData.UpdateData(m_IfFromUp.MiwTransferData.GetValues());
            }

            if(m_Server.RecvData.GlassNumberCode.Code == (ushort)0xFFFE)
            {
                nRv = GLS_CHECK.OK;
            }
            else
            {
                if (m_Server.RecvData.GlassNumberCode.LotNo != 0 &&
                    m_Server.RecvData.GlassNumberCode.SlotNo != 0)
                {
                    //if (true == TagRecipe.IsAvailableRecipeId(Convert.ToInt32(m_Server.RecvData.HostRecipe1)))
                    if(m_Server.RecvData.HostRecipe1 != null)
                    {
                        nRv = GLS_CHECK.OK;
                    }
                    else
                    {
                        nRv = GLS_CHECK.RECIPE_NG;
                    }
                }
                else
                {
                    nRv = GLS_CHECK.DATA_NG;
                }
            }

            return nRv;
        }

        public override int Do()
        {
            if (!m_Server.GenInfos.EqpInitComp) return -1;

            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        nSeqNo = 5;
                        m_Step.StepNo = 0;
                    }
                    break;
                case 5 :
                    if (m_IfFromUp.MibUpstreamReady.GetStatus())
                    {
                        m_PortNo = 0;
                        m_SlotNo = 0;
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Up Ready : ON"); 
                        nSeqNo = 10;
                        m_Step.StepNo++;
                    }
                    break;
                case 10 :
                    if (m_IfToUp.MobDownstreamReady.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Ready : ON");
                        nSeqNo = 20;
                    }
                    break;
                case 20 :
                    if (m_IfToUp.MobReceivePossible.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Receive Possible : ON");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 30;
                        m_Step.StepNo++;
                    }
                    break;
                case 30 :
                    if (m_IfFromUp.MibUnloadRequest.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Upstream Unload Request : ON");
                        m_Step.StepNo++;

                        m_Result = ReadGlassData();

                        if (m_Result == GLS_CHECK.OK)
                        {
                            m_IfToUp.MobReceiveReady.SetStatus(true);
                            m_PortNo = (int)m_Server.RecvData.GlassNumberCode.LotNo;
                            m_SlotNo = (int)m_Server.RecvData.GlassNumberCode.SlotNo;
                            m_Server.SeqFlag.GlassDataCheckOk = true;
                            Log(SeqFunName, m_PortNo, m_SlotNo, "Glass Data Check : OK");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 100;
                            m_Step.StepNo++;
                        }
                        else if (m_Result == GLS_CHECK.RECIPE_NG)
                        {
                            Log(SeqFunName, m_PortNo, m_SlotNo, "Glass Data Check : Recipe NG");
                            nSeqNo = 1000;
                        }
                        else if (m_Result == GLS_CHECK.DATA_NG)
                        {
                            Log(SeqFunName, m_PortNo, m_SlotNo, "Glass Data Check : Data NG");
                            nSeqNo = 1000;
                        }
                    }
                    else if (m_Simul.Melsec)
                    {
                        m_IfFromUp.MibUnloadRequest.SetStatus(true);
                        m_IfFromUp.MibInterlock1.SetStatus(true);
                    }
                    else if (GetElapsedTicks() > m_Server.SetupIfTimeoutU1.GetValue<int>())
                    {
                        m_AlarmId = m_Server.ALM_IfTimeoutU1.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        Log(SeqFunName, m_PortNo, m_SlotNo, "U1 Timeout : Set");
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 2000;
                    }
                    break;
                case 100 :
                    if (m_IfFromUp.MibUnloadComplete.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Unload Complete : ON");
                        nSeqNo = 110;
                        m_Step.StepNo++;
                    }
                    else if (m_Simul.Melsec)
                    {
                        m_IfFromUp.MibUnloadComplete.SetStatus(true);
                    }
                    else if (GetElapsedTicks() > m_Server.SetupIfTimeoutU5.GetValue<int>())
                    {
                        m_AlarmId = m_Server.ALM_IfTimeoutU5.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        Log(SeqFunName, m_PortNo, m_SlotNo, "U5 Timeout : Set");
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 3000;
                    }
                    break;
                case 110 :
                    if(m_IfToUp.MobReceiveComplete.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Receive Complete : ON");
                        StartTicks = XFunc.GetTickCount();
                        nSeqNo = 120;
                        m_Step.StepNo++;
                    }
                    break;
                case 120:
                    if (!m_IfFromUp.MibUnloadRequest.GetStatus() &&
                        !m_IfFromUp.MibUnloadComplete.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Upstream singal : OFF");
                        nSeqNo = 130;
                    }
                    else if(m_Simul.Melsec)
                    {
                        m_IfFromUp.MibUnloadRequest.SetStatus(false);
                        m_IfFromUp.MibUnloadComplete.SetStatus(false);
                    }
                    else if (GetElapsedTicks() > m_Server.SetupIfTimeoutU6.GetValue<int>())
                    {
                        m_AlarmId = m_Server.ALM_IfTimeoutU6.Id;
                        m_EqpManger.SetAlarm(m_AlarmId);
                        Log(SeqFunName, m_PortNo, m_SlotNo, "U6 Timeout : Set");
                        m_ReturnSeqNo = nSeqNo;
                        nSeqNo = 4000;
                    }
                    break;
                case 130 :
                    {
                        m_IfToUp.MobReceiveComplete.SetStatus(false);
                        m_IfToUp.MobReceiveReady.SetStatus(false);
                        m_IfToUp.MobReceivePossible.SetStatus(false);

                        m_Server.SeqFlag.Recv1Complete = true;

                        Log(SeqFunName, m_PortNo, m_SlotNo, "Glass Receive Complete(Finish)");

                        nSeqNo = 0;
                        m_Step.StepNo = 0;
                    }
                    break;
            }

            this.SeqNo = nSeqNo;

            return -1;
        }

        private void Log(string seqName, int portNo, int slotNo, string message)
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

            log = string.Format("Interface  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_Server.GenInfos.EqpLog = log;
        }
        #endregion
    }

    public class SeqSend : XSeqFunction
    {
        #region Fields
        protected static IEqpManager m_EqpManger;
        protected static ServerManager m_Server;
        private Simul m_Simul;
        private IfSigToDn m_IfToDn;
        private IfSigFromDn m_IfFromDn;
        private TagGlassData m_SendData = new TagGlassData();
        private int m_PortNo = 0;
        private int m_SlotNo = 0;
        private int UL;
        private InterfaceStep m_Step;
        #endregion

        #region Constructor
        public SeqSend(string seqName, IfSigFromDn sigFromDn, IfSigToDn sigToDn, InterfaceStep step)
        {
            SeqFunName = seqName;

            m_Server = ServerManager.Instance;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_IfFromDn = sigFromDn;
            m_IfToDn = sigToDn;
            UL = ThreadCvControl.Units.Count - 1;
            m_Step = step;
        }
        #endregion

        #region Methods
        private void WriteGlassData()
        {
            if (m_Server.GlassData.IsExist(UL*2))
            {
                m_Server.GlassData.GetData(UL * 2, ref m_SendData);
                Log(SeqFunName, m_PortNo, m_SlotNo, "Send OUT BUT2 IN Glass Data");
            }
            else if(m_Server.GlassData.IsExist(UL*2+1))
            {
                m_Server.GlassData.GetData(UL * 2 + 1, ref m_SendData);
                Log(SeqFunName, m_PortNo, m_SlotNo, "Send OUT BUT2 OUT Glass Data");
            }

            //if (!m_Simul.Melsec)
            {
                m_IfToDn.MowTransferData.SetValues(m_SendData.Item.Stream);
            }

            m_PortNo = (int)m_SendData.Item.GlassNumberCode.LotNo;
            m_SlotNo = (int)m_SendData.Item.GlassNumberCode.SlotNo;
        }

        public override int Do()
        {
            if (!m_Server.GenInfos.EqpInitComp) return -1;
            
            bool run = true;
            run &= m_Server.GenInfos.AutoMode;
            run &= !m_Server.GenInfos.Pause;

            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        nSeqNo = 1;
                        m_Step.StepNo = 0;
                    }
                    break;
                case 1:
                    if (m_IfFromDn.MibDownstreamReady.GetStatus())
                    {
                        nSeqNo = 5;
                        m_Step.StepNo++;
                    }
                    break;
                case 5:
                    if (m_IfFromDn.MibDownstreamReady.GetStatus() && 
                        m_IfFromDn.MibReceivePossible.GetStatus() &&
                        m_IfToDn.MobUpstreamReady.GetStatus() &&
                        m_Server.SeqFlag.Send1Request)
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Send Request");
                        m_Server.SeqFlag.Send1Request = false;
                        WriteGlassData();
                        nSeqNo = 10;
                        m_Step.StepNo++;
                    }
                    else if (m_Simul.Melsec)
                    {
                        m_IfFromDn.MibReceivePossible.SetStatus(true);
                    }
                    break;
                case 10 :
                    //if (m_IfToDn.MobExchangeRequest.GetStatus())
                    if(m_IfToDn.MobUnloadRequest.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Unload Request : ON");
                        m_IfToDn.MobInterlock1.SetStatus(true);
                        nSeqNo = 20;
                        m_Step.StepNo++;
                    }
                    break;
                case 20:
                    if (m_IfFromDn.MibReceiveReady.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Receive Ready : ON");
                        nSeqNo = 30;
                        m_Step.StepNo++;
                    }
                    else if (m_Simul.Melsec)
                    {
                        m_IfFromDn.MibReceiveReady.SetStatus(true);
                    }
                    break;
                case 30 :
                    //if (m_IfToDn.MobUnloadReady.GetStatus())
                    {
                        m_IfToDn.MobUnloadReady.SetStatus(true);
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Unload Ready : ON");
                        this.StartTicks = XFunc.GetTickCount();
                        nSeqNo = 100;
                    }
                    break;
                case 100 :
                    if (m_IfFromDn.MibReceiveComplete.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Receive Complete : ON");
                        nSeqNo = 110;
                        m_Step.StepNo++;
                    }
                    else if (m_Simul.Melsec && GetElapsedTicks() > 6000)
                    {
                        m_IfFromDn.MibReceiveComplete.SetStatus(true);
                    }
                    break;
                case 110 :
                    if (!m_IfToDn.MobGlassOnEq.GetStatus() &&
                        m_IfToDn.MobUnloadComplete.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Unload Complete : ON");
                        nSeqNo = 120;
                        m_Step.StepNo++;
                    }
                    break;
                case 120 :
                    if (!m_IfFromDn.MibReceiveComplete.GetStatus() &&
                        !m_IfFromDn.MibReceiveReady.GetStatus() &&
                        !m_IfFromDn.MibReceivePossible.GetStatus())
                    {
                        Log(SeqFunName, m_PortNo, m_SlotNo, "Receive Complete : OFF");
                        nSeqNo = 130;
                    }
                    else if (m_Simul.Melsec)
                    {
                        m_IfFromDn.MibReceiveComplete.SetStatus(false);
                        m_IfFromDn.MibReceiveReady.SetStatus(false);
                        m_IfFromDn.MibReceivePossible.SetStatus(false);
                    }
                    break;
                case 130:
                    //if (!m_IfToDn.MobUnloadComplete.GetStatus())
                    {
                        m_IfToDn.MobUnloadComplete.SetStatus(false);
                        m_IfToDn.MobUnloadReady.SetStatus(false);
                        m_IfToDn.MobUnloadRequest.SetStatus(false);

                        Log(SeqFunName, m_PortNo, m_SlotNo, "Glass Send Finish");

                        m_Server.SeqFlag.Send1Complete = true;

                        nSeqNo = 0;
                        m_Step.StepNo = 0;
                    }
                    break;
            }

            this.SeqNo = nSeqNo;

            return -1;
        }

        private void Log(string seqName, int portNo, int slotNo, string message)
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

            log = string.Format("Interface  \t{0}\t{1}\t{2}\t{3}", seqName, portName, slotName, message);

            m_Server.EqpLog.TextOut(log);
            log = string.Format("{0} - {1}", seqName, message);
            m_Server.GenInfos.EqpLog = log;
        }
        #endregion
    }

    public class SeqForceRecvCompFromUp : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManger;
        private Simul m_Simul;
        private IfSigFromUp m_IfFromUp;
        private IfSigToUp m_IfToUp;
        private ThreadCvControl m_CvControl;
        #endregion

        #region Constructor
        public SeqForceRecvCompFromUp(string seqName, IfSigFromUp sigFromUp, IfSigToUp sigToUp)
        {
            SeqFunName = seqName;
            
            m_Server = ServerManager.Instance;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_CvControl = m_Server.ThreadHandler.CvControl;

            m_IfFromUp = sigFromUp;
            m_IfToUp = sigToUp;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_IfFromUp.MibForceRecvCompReq.GetStatus() && !ThreadCvControl.Units[0].GlsInSensor.IsDetected())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Glass is not exist in LD. Force Recv Comp is impossible");
                            nSeqNo = 5;
                        }
                        else if (m_IfFromUp.MibForceRecvCompReq.GetStatus())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Forcce Recv Comp Request from Up: ON");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 5:
                    {
                        if (ThreadCvControl.Units[0].GlsInSensor.IsDetected() || !m_IfFromUp.MibForceRecvCompReq.GetStatus())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Force Recv Comp Request from Up: Cancel");
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 10:
                    {
                        if (m_IfFromUp.MibForceRecvCompReq.GetStatus() && (GetElapsedTicks() > 500))
                        {
                            m_IfToUp.MobReceiveComplete.SetStatus(true);
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Recv Comp: ON");
                            nSeqNo = 20;
                        }
                        else if (!m_IfFromUp.MibForceRecvCompReq.GetStatus())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Force Recv Comp Request from Up: Cancel");
                            nSeqNo = 0;
                        }
                    }
                    break;
                case 20:
                    {
                        if (!m_IfFromUp.MibForceRecvCompReq.GetStatus() && !m_IfFromUp.IsOn(Logic.AND))
                        {
                            m_IfToUp.ResetAll();
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Force Recv Comp Request from Up: Finish");
                            nSeqNo = 0;
                        }
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqForceRecvCompReqToDn : XSeqFunction
    {
        #region Fields
        protected static ServerManager m_Server;
        protected static IEqpManager m_EqpManger;
        private Simul m_Simul;
        private IfSigFromDn m_IfFromDn;
        private IfSigToDn m_IfToDn;
        private ThreadCvControl m_CvControl;
        #endregion

        #region Constructor
        public SeqForceRecvCompReqToDn(string seqName, IfSigFromDn sigFromDn, IfSigToDn sigToDn)
        {
            SeqFunName = seqName;
            
            m_Server = ServerManager.Instance;
            m_EqpManger = m_Server.EqpStateManager;
            m_Simul = m_Server.Simul;
            m_CvControl = m_Server.ThreadHandler.CvControl;

            m_IfFromDn = sigFromDn;
            m_IfToDn = sigToDn;
        }
        #endregion

        #region Methods
        public override int Do()
        {
            int nSeqNo = this.SeqNo;

            switch (nSeqNo)
            {
                case 0:
                    {
                        if (m_Server.GenInfos.ForceCompReqToDn && ThreadCvControl.Units[ThreadCvControl.Units.Count - 1].GlsOutSensor.IsDetected())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Glass exist in UL. Force Recv Comp Req is impossible");
                            m_Server.GenInfos.ForceCompReqToDn = false;
                            nSeqNo = 0;
                        }
                        else if (m_Server.GenInfos.ForceCompReqToDn)
                        {
                            m_IfToDn.MobForceRecvCompReq.SetStatus(true);
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Forcce Recv Comp Request to Dn: ON");
                            StartTicks = XFunc.GetTickCount();
                            nSeqNo = 10;
                        }
                    }
                    break;
                case 10:
                    {
                        if (m_IfFromDn.MibForceReceiveComplete.GetStatus())
                        {
                            m_Server.SetInterfaceLog(SeqFunName, 0, 0, "Forcce Recv Comp: ON");
                            m_IfToDn.MobForceRecvCompReq.SetStatus(false);
                            nSeqNo = 20;
                        }
                    }
                    break;
            }
            this.SeqNo = nSeqNo;

            return -1;
        }
        #endregion
    }

    public class SeqForceHSInitReqToUp : XSeqFunction
    { 
    }

    public class SeqForceHSInitFromUp : XSeqFunction
    { 
    }

    public class SeqForceHSInitReqToDn : XSeqFunction
    { 
    }

    public class SeqForceHSInitFromDn : XSeqFunction
    { 
    }
}

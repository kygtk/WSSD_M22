using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Data;
using SEComEnabler.SEComPlugIn;
using SEComEnabler.SEComStructure;
using Dms.Common;
using Dms.Data;

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.05
// Author       : Kim Youngsik
// Description  : HSMS Client for SECom
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

namespace Dms.Ctl
{
    public class SecomHsmsClient
    {
        #region Fields
        private SEComEnabler.SEComPlugIn.CSEComPlugIn m_SecomPlugIn = null;
        private SEComError.SEComPlugIn m_ErrPlugIn = 0;
        private HsmsInfo m_HsmsInfo;
        private string NEWLINE = Environment.NewLine;
        private HsmsMessageQueue m_MsgQueue;
        private SetupHsmsEqpInfoProvider m_Provider;
        private string EqpName = "";

        public ManualResetEvent PrimaryOut = new ManualResetEvent(true);
        public AutoResetEvent SecondaryIn = new AutoResetEvent(false);

        static private XLog m_HsmsLog = new XLog("HsmsLog", XLog.LogStampType.UseStamp);
        #endregion

        #region properties
        public SEComEnabler.SEComPlugIn.CSEComPlugIn SEComPlugIn
        {
            get { return m_SecomPlugIn; }
            set { m_SecomPlugIn = value; }
        }
        public SEComError.SEComPlugIn ErrorPlugIn
        {
            get { return m_ErrPlugIn; }
            set { m_ErrPlugIn = value; }
        }
        public HsmsInfo HsmsInfo
        {
            get { return m_HsmsInfo; }
            set { m_HsmsInfo = value; }
        }
        #endregion

        #region Singleton
        public static readonly SecomHsmsClient Instance = new SecomHsmsClient();
        #endregion

        #region Constructor
        public SecomHsmsClient()
        {
            //m_HsmsInfo = HsmsInfo.Instance;
            m_HsmsInfo = new HsmsInfo();
            m_MsgQueue = HsmsMessageQueue.Instance;
            m_Provider = SetupHsmsEqpInfoProvider.Instance;
            InitSecom();
        }
        #endregion

        #region Virtual Methods
        public virtual void OnSecsInvalidReceived(string EqpId, XmlDocument Xml)
        {
            SetLog("InvalidMessage For " + EqpId + NEWLINE + Xml.OuterXml);

            //SecondaryIn.Set();
            m_MsgQueue.EnqueueError("Invalid");
        }

        public virtual void OnSecsAbortMessage(string EqpId, XmlDocument Xml)
        {
            SEComData sd = new SEComData();
            sd.XmlToSECSData(Xml);

            int stream = sd.HeaderItems.Stream;
            int function = sd.HeaderItems.Function;

            string msg = string.Format("S{0}F{1}", stream, function);

            SetLog("AbortMessage For " + EqpId + NEWLINE + Xml.OuterXml);

            m_MsgQueue.EnqueueError("Abort-"+ msg);
        }

        public virtual void OnSecsUnknownMessage(string EqpId, XmlDocument Xml)
        {
            SEComData sd = new SEComData();
            sd.XmlToSECSData(Xml); // (sd.HeaderItems use)

            SetLog("UnknownMessage For " + EqpId + NEWLINE + Xml.OuterXml);

            //SecondaryIn.Set();
            m_MsgQueue.EnqueueError("Unknown");
        }

        public virtual void OnSecsDisconnected(string EqpId, XmlDocument Xml)
        {
            m_HsmsInfo.HsmsConnect = false;
            //m_EcsInfo.ModeChanging = false;

            SetLog("Disconnected is" + EqpId + NEWLINE + Xml.OuterXml);
        }

        public virtual void OnSecsConnected(string EqpId, XmlDocument Xml)
        {
            m_HsmsInfo.HsmsConnect = true;

            SetLog("Connected is " + EqpId + NEWLINE + Xml.OuterXml);
        }

        public virtual void OnSecs2Log(string EqpId, string Direction, string Secs2)
        {
            //            SetLog(aSECS2);

            string[] sLog = Secs2.Split(new char[] { '\r' });

            SetLog(sLog[0]);
        }

        public virtual void OnSecsTimeOut(string aEquipmentID, XmlDocument aXML, string aTimeOut)
        {
            string timeout = "";

            // XML PATH USE
            XmlNode nodeStream = aXML.SelectSingleNode("/SECOM_MSG/RelatedHeader/Stream");
            XmlNode nodeFunction = aXML.SelectSingleNode("/SECOM_MSG/RelatedHeader/Function");

            switch (aTimeOut)
            {
                case "1003":
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        {
                            XmlNode nodeMessageName = aXML.SelectSingleNode("/SECOM_MSG/RelatedHeader/MessageName");
                            timeout = string.Format("T3-S{0}F{1}-{2}", nodeStream.InnerText, nodeFunction.InnerText, nodeMessageName.InnerText);
                        }
                    }
                    break;

                case "1005":
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        timeout = string.Format("T5-S{0}F{1}", nodeStream.InnerText, nodeFunction.InnerText);
                    }
                    break;

                case "1006":
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        timeout = string.Format("T6-S{0}F{1}", nodeStream.InnerText, nodeFunction.InnerText);
                    }
                    break;

                case "1007":
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        timeout = string.Format("T7-S{0}F{1}", nodeStream.InnerText, nodeFunction.InnerText);
                    }
                    break;

                case "1008":
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        timeout = string.Format("T8-S{0}F{1}", nodeStream.InnerText, nodeFunction.InnerText);
                    }
                    break;

                default:
                    {
                        //2009.08.25 Youngsik timeout format변경.
                        if (nodeStream != null)
                        timeout = string.Format("SECS1-S{0}F{1}", nodeStream.InnerText, nodeFunction.InnerText);
                    }
                    break; 
            }

            m_MsgQueue.EnqueueTimeout(timeout);
            SetLog("Timeout : " + timeout + " For " + aEquipmentID + NEWLINE + aXML.OuterXml);

        }

        public virtual void OnSecsReceived(string EqpId, string HsmsMsgName, XmlDocument Xml)
        {
            SEComData sd = new SEComData();
            sd.XmlToSECSData(Xml);

            int stream = sd.HeaderItems.Stream;
            int function = sd.HeaderItems.Function;
            int odd = function % 2;

            m_MsgQueue.EnqueueRecvMsg(sd);
        }

        public virtual SEComError.SEComPlugIn Send(SXTransaction sxTrx)
        {
            int function = sxTrx.Function;
            int odd = function % 2;

            DataRowCollection rows = m_Provider.Adapter.Table.Rows;

            for (int i = 0; i < rows.Count; i++)
            {
                DataSetSetupItem.SetupHsmsEqpInfoRow row = (DataSetSetupItem.SetupHsmsEqpInfoRow)rows[i];

                if (row.Name == "SECom Driver Name")
                    EqpName = row.ItemValue.ToString();
            }

            try
            {
                if (function != 0)
                {
                    switch (odd)
                    {
                        case 0: //case of reply
                            {
                                ErrorPlugIn = SEComPlugIn.Reply(EqpName, sxTrx, sxTrx.SystemBytes);
                            }
                            break;
                        case 1: //case of primary
                            {
                                ErrorPlugIn = SEComPlugIn.Request(EqpName, sxTrx);
                            }
                            break;
                    }
                }
                else
                {
                    ErrorPlugIn = SEComPlugIn.Request(EqpName, sxTrx);
                }
                

                if (ErrorPlugIn != 0)
                    SetLog("Send Fail For " + EqpName + " : " + ErrorPlugIn.ToString());
            }
            catch
            {
                return ErrorPlugIn;
            }

            return ErrorPlugIn;
        }
        #endregion

        #region Methods
        public void Initialize(HsmsInfo info)
        {
            m_HsmsInfo = info;
        }

        public void InitSecom()
        {
            DataRowCollection rows = m_Provider.Adapter.Table.Rows;

            for (int i = 0; i < rows.Count; i++)
            {
                DataSetSetupItem.SetupHsmsEqpInfoRow row = (DataSetSetupItem.SetupHsmsEqpInfoRow)rows[i];

                if (row.Name == "SECom Driver Name")
                    EqpName = row.ItemValue.ToString();
            }

            OpenSECom();

            RegisterEvent();
        }

        public void Uninitialize()
        {
            try
            {
                CloseSECom();

                UnRegisterEvent();

                this.m_SecomPlugIn.Dispose();
                this.m_SecomPlugIn = null;
            }
            catch (Exception ex)
            {
                SetLog(ex.ToString());
            }
        }

        public bool OpenSECom()
        {
            try
            {
                this.m_SecomPlugIn = new SEComEnabler.SEComPlugIn.CSEComPlugIn();

                this.m_ErrPlugIn = this.m_SecomPlugIn.Initialize(EqpName
                    , true //Receive SECS Log
                    , true //Use Dispatcher
                    );

                if (this.m_ErrPlugIn != 0)
                {
                    SetLog("Initialize Fail : " + this.m_ErrPlugIn.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.ToString());
                return false;
            }

            return true;
        }

        public bool CloseSECom()
        {
            try
            {
                m_SecomPlugIn.Terminate(EqpName);

                m_HsmsInfo.HsmsConnect = false;        //HOST와 연결해제되었음

                return true;
            }
            catch (Exception ex)
            {
                SetLog(ex.ToString());

                return false;
            }
        }

        public void ReloadProFile()
        {
            m_SecomPlugIn.ReloadProFile(EqpName);
        }

        public void ReloadSMD()
        {
            m_SecomPlugIn.ReloadSMD(EqpName);
        }

        public void RegisterEvent()
        {
            SEComPlugIn.OnSECS2Log += new CSEComPlugIn.DLGSECS2Log(OnSecs2Log);
            SEComPlugIn.OnSECSConnected += new CSEComPlugIn.DLGSECSConnected(OnSecsConnected);
            SEComPlugIn.OnSECSDisConnected += new CSEComPlugIn.DLGSECSDisConnected(OnSecsDisconnected);
            SEComPlugIn.OnSECSUnknownMessage += new CSEComPlugIn.DLGSECSUnknownMessage(OnSecsUnknownMessage);
            SEComPlugIn.OnSECSInvalidReceived += new CSEComPlugIn.DLGSECSInvalidReceived(OnSecsInvalidReceived);
            SEComPlugIn.OnSECSAbortMessage += new CSEComPlugIn.DLGSECSAbortMessage(OnSecsAbortMessage);
            SEComPlugIn.OnSECSTimeOut += new CSEComPlugIn.DLGSECSTimeOut(OnSecsTimeOut);

            SEComPlugIn.OnSECSReceived += new CSEComPlugIn.DLGSECSReceived(OnSecsReceived);
        }

        public void UnRegisterEvent()
        {
            SEComPlugIn.OnSECS2Log -= new CSEComPlugIn.DLGSECS2Log(OnSecs2Log);
            SEComPlugIn.OnSECSConnected -= new CSEComPlugIn.DLGSECSConnected(OnSecsConnected);
            SEComPlugIn.OnSECSDisConnected -= new CSEComPlugIn.DLGSECSDisConnected(OnSecsDisconnected);
            SEComPlugIn.OnSECSUnknownMessage -= new CSEComPlugIn.DLGSECSUnknownMessage(OnSecsUnknownMessage);
            SEComPlugIn.OnSECSInvalidReceived -= new CSEComPlugIn.DLGSECSInvalidReceived(OnSecsInvalidReceived);
            SEComPlugIn.OnSECSAbortMessage -= new CSEComPlugIn.DLGSECSAbortMessage(OnSecsAbortMessage);
            SEComPlugIn.OnSECSTimeOut -= new CSEComPlugIn.DLGSECSTimeOut(OnSecsTimeOut);

            SEComPlugIn.OnSECSReceived -= new CSEComPlugIn.DLGSECSReceived(OnSecsReceived);
        }


        public void SetLog(string message)
        {
            m_HsmsLog.TextOut(message);

            //m_DataQueue.SetLogListQueue(message);
        }
        #endregion
    }
}

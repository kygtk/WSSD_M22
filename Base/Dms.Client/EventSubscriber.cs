///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : EventSubscriber
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Windows.Forms;
using System.Threading;
using Dms.Device;
using Dms.Remoting;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Client
{
    public class EventSubscriber
    {
        #region Fields
        private static object m_LockKey = new object();
        private bool m_Initialized = false;
        private ClientManager m_Client = null;
        private ServerMode m_Mode;
        private IDMSComm m_IDmsComm = null;
        private MessageBroadcaster m_Broadcaster = null;
        private XLog m_EventLog = new XLog("EventRecvLog", XLog.LogStampType.UseStamp);
        Queue<object> m_DataQueue = new Queue<object>();
        private Thread m_DataUpdateMainThread = null;
        private GenInfoHandler m_GenInfoHandler = null;
        private ApdItemsHandler m_ApdItemsHandler = null;
        private PartsItemsHandler m_PartsItemsHandler = null;
        private HpmjItemsHandler m_HpmjItemsHandler = null;
        private IServerManager m_Server = null;

        //  Data
        private DataProvider m_DataProvider = null;

        //  Server
        #endregion

        #region Properties
        public IServerManager IServerManager
        {
            get { return m_Server; }
        }
        #endregion

        #region Contstructor
        //public EventSubscriber()
        //{
        //    m_Client = ClientManager.Instance;
        //    m_Mode = AppConfig.Instance.ServerMode;

        //    InitEventSubscriber();
        //}

        //public EventSubscriber(ClientManager client, ServerMode mode)
        //{
        //    m_Client = client;
        //    m_Mode = mode;

        //    InitEventSubscriber();
        //}

        public EventSubscriber(ClientManager client, ServerMode mode, IServerManager server)
        {
            m_Client = client;
            m_Mode = mode;
            m_Server = server;

            InitEventSubscriber();
        }
        #endregion

        #region Methods
        private bool InitEventSubscriber()
        {
            try
            {
                if (m_Mode == ServerMode.Local)
                {
                    //m_Server = ServerManager.Instance;
                    m_IDmsComm = new DMSCommImpl(m_Server);
                    m_DataProvider = m_Server.DataProvider;// new DataProvider(m_Server.DataProvider);

                    m_GenInfoHandler = GenInfoHandler.Instance;
                    m_ApdItemsHandler = ApdItemsHandler.Instance;
                    m_PartsItemsHandler = PartsItemsHandler.Instance;
                    m_HpmjItemsHandler = HpmjItemsHandler.Instance;
                }
                else if (m_Mode == ServerMode.Remoting)
                {
                    m_IDmsComm = (IDMSComm)RemotingHelper.GetObject(typeof(IDMSComm));
                    m_Broadcaster = new MessageBroadcaster();
                    m_Broadcaster.MessageArrived += new MessageArrivedHandler(HandleMessage);
                    m_DataProvider = new DataProvider();
                    m_DataProvider.TagContainer.ReadXml();
                    m_GenInfoHandler = GenInfoHandler.Instance;
                    m_GenInfoHandler.SyncTags(m_DataProvider.TagContainer);
                    m_ApdItemsHandler = ApdItemsHandler.Instance;
                    m_ApdItemsHandler.ReadXml();
                    m_ApdItemsHandler.SyncTags(m_DataProvider.TagContainer);
                    m_PartsItemsHandler = PartsItemsHandler.Instance;
                    m_PartsItemsHandler.ReadXml();
                    m_HpmjItemsHandler = HpmjItemsHandler.Instance;
                    m_HpmjItemsHandler.ReadXml();
                }

                m_Client.DMSComm = m_IDmsComm;
                m_Client.DataProvider = m_DataProvider;
                m_Client.GenInfos = m_GenInfoHandler;
                m_Client.ApdItemsHandler = m_ApdItemsHandler;
                m_Client.PartsItemsHandler = m_PartsItemsHandler;
                m_Client.HpmjItemsHandler = m_HpmjItemsHandler;

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        public void WriteEventLog(string log)
        {
            m_EventLog.TextOut(log);
        }

        public DmsErrors Initialize()
        {
            if (m_Initialized)
            {
                return DmsErrors.Success;
            }

            bool ok = true;
            if (m_Mode == ServerMode.Local)
            {
                if (AppConfig.Instance.AutoStart)
                {
                    ok &= m_Server.Created;

                    if (ok)
                    {
                        ok &= m_Server.Initialize() == DmsErrors.Success;
                    }

                    if (ok)
                    {
                        m_Server.Start();
                    }
                }
            }

            m_Initialized = ok;

            if (m_Initialized)
            {
                return DmsErrors.Success;
            }
            else
            {
                return DmsErrors.InternalError;
            }
        }

        public DmsErrors Uninitialize()
        {
            if (m_Mode == ServerMode.Local)
            {
                if (AppConfig.Instance.AutoStart)
                {
                    m_Server.Uninitialize();
                }
            }

            m_Initialized = false;

            return DmsErrors.Success;
        }

        private void UpdateDataIn()
        {
            try
            {
                while (true)
                {
                    object inObj = "";
                    if (m_DataQueue.Count == 0)
                    {
                        return;
                    }
                    lock (m_LockKey)
                    {
                        inObj = m_DataQueue.Dequeue();
                        DeviceTag tagReceived = inObj as DeviceTag;
                        DeviceTag tagOriginal = m_DataProvider.TagContainer[tagReceived.DeviceName];

                        if (tagReceived != null && tagOriginal != null)
                        {
                            tagOriginal.Clone(tagReceived);
                        }
                    }
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                m_Client.WriteExceptionLog("Event Handle Failed\n" + err.Message);
            }
        }

        public DmsErrors UpdateLocalDataFromEvent(object inObj)
        {
            if (!m_Initialized) return DmsErrors.NotInitialized;

            m_DataQueue.Enqueue(inObj);
            m_DataUpdateMainThread = new Thread(delegate () { UpdateDataIn(); });
            if (!m_DataUpdateMainThread.IsAlive)
            {
                m_DataUpdateMainThread.Start();
            }
            return DmsErrors.Success;
        }

        private void HandleMessage(object broadCastData)
        {
            if (m_Initialized == false) return;

            m_Client.Log("Entered : HandleMessage()");

            // Update local store...
            UpdateLocalDataFromEvent(broadCastData);
        }
        #endregion
    }
}

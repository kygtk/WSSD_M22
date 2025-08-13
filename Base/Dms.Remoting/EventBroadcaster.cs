using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Dms.Common;
using System.Threading;
using System.Runtime.Remoting;
using Dms.Device;

namespace Dms.Remoting
{
    [Serializable()]
    public class EventBroadcaster
    {
        #region Fields
        private static object m_LockKey = new object();
        private IServerManager m_Server;
        private BroadcasterImpl m_Broadcaster;
        private ServerMode m_ServerMode;
        private XLog m_EventLog = new XLog("EventFireLog", XLog.LogStampType.UseStamp);
        private bool m_Initialized = false;
        Queue<object> m_DataQueue = new Queue<object>();
        Queue<string> m_MsgQueue = new Queue<string>();
        private Thread m_DataUpdaterMainThread = null;
        private Thread m_InitialDataSendThread = null;

        #endregion   

        #region Properties
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        public BroadcasterImpl Broadcaster
        {
            get { return m_Broadcaster; }
        }
        #endregion

        #region Constructor
        public EventBroadcaster(IServerManager server, ServerMode mode)
        {
            m_Server = server;
            m_ServerMode = mode;
        }
        #endregion

        #region Methods
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

            try
            {
                if (m_ServerMode == ServerMode.Local)
                {
                    m_Broadcaster = new BroadcasterImpl();
                }
                else if (m_ServerMode == ServerMode.Remoting)
                {
                    InitRemoting();

                    m_Broadcaster = (BroadcasterImpl)RemoteSingletonObjectsList.Instance.GetRemoteSingletonObject(typeof(BroadcasterImpl));
                }

                m_Initialized = true;
                return DmsErrors.Success;
            }
            catch (Exception err)
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                m_Initialized = false;
                return DmsErrors.InternalError;
            }
        }

        public DmsErrors Uninitialize()
        {
            m_Initialized = false;

            return DmsErrors.Success;
        }

        private static void InitRemoting()
        {
            string fileName = "Dms.Server.exe.config";

            RemotingConfiguration.Configure(fileName, false);
            foreach (WellKnownServiceTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                MarshalByRefObject pxy = (MarshalByRefObject)Activator.GetObject(entry.ObjectType, "tcp://localhost:16784/Dms.Server/" + entry.ObjectUri);
                pxy.CreateObjRef(entry.ObjectType);
                RemoteSingletonObjectsList.Instance.RegisterRemoteSingletonObject(pxy);
            }
        }

        public DmsErrors SendInitialDataToClient()
        {
            m_InitialDataSendThread = new Thread(new ThreadStart(StartSendingInitialDataToClient));
            m_InitialDataSendThread.Priority = ThreadPriority.BelowNormal;
            m_InitialDataSendThread.Start();

            return DmsErrors.Success;
        }

        private void StartSendingInitialDataToClient()
        {
            //Send all the data one by one from here
            //FireEventToClient(m_GenInfo, "Initial Data");
            //TODO Send all initial data to client
            //for(int Count = 0; Count < 

        }

        public DmsErrors FireEvent(object broadCastData, string msg)
        {
            if (!m_Initialized) return DmsErrors.NotInitialized;
            if (m_Broadcaster.IsThereSubscribers == false)
            {
                WriteEventLog("Event Cancel : " + msg);
                return DmsErrors.Success;
            }

            m_DataQueue.Enqueue(broadCastData);
            m_MsgQueue.Enqueue(msg);
            m_DataUpdaterMainThread = new Thread(delegate () { SendDataToClient(); });

            if (!m_DataUpdaterMainThread.IsAlive)
            {
                m_DataUpdaterMainThread.Start();
            }

            return DmsErrors.Success;
        }

        private void SendDataToClient()
        {
            try
            {
                while (true)
                {
                    object dataObj = "";
                    string msg = "";
                    if (m_DataQueue.Count == 0)
                    {
                        return;
                    }
                    lock (m_LockKey)
                    {
                        dataObj = m_DataQueue.Dequeue();
                        msg = m_MsgQueue.Dequeue();

                        WriteEventLog("Entered : SendDataToClient");
                        WriteEventLog(msg);

                        m_Broadcaster.BroadcastMessage(dataObj);

                        WriteEventLog("Leaving : SendDataToClient");
                    }
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
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Dms.Common;
using System.Threading;
using Dms.Data;
using Dms.Device;
using Dms.Remoting;
using Dms.Sequence;

namespace Dms.Server
{
    public delegate void StateChanged();

    public sealed partial class ServerManager : IServerManager
    {
        #region Fields
        private bool m_Initialized = false;
        private ActiveState m_ServerState = ActiveState.Uninitialized;
        private ServerMode m_ServerMode = ServerMode.Local;
        private XLog m_EqpLog = new XLog("EqpLog", XLog.LogStampType.UseStamp);
        //private XLog m_ExceptionLog = new XLog("ServerExceptionLog", XLog.LogStampType.UseStamp);
        private EventBroadcaster m_EventBroadcaster;
        public event UninitializeDelegate UninitializeDel;
        public StateChanged ServerStateChanged;
        public ThreadHandler ThreadHandler;
        private static FormInitStatus m_InitForm = new FormInitStatus();
        private bool m_Created = false;
        #endregion

        #region Singleton code...
        public static readonly ServerManager Instance = new ServerManager();
        #endregion

        #region Properties
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        public ActiveState State
        {
            get { return m_ServerState; }
            set
            {
                m_ServerState = value;
                if (ServerStateChanged != null)
                {
                    ServerStateChanged();
                }
            }
        }
        public ServerMode ServerMode
        {
            get { return m_ServerMode; }
            set { m_ServerMode = value; }
        }
        public XLog EqpLog
        {
            get { return m_EqpLog; }
        }
        public FileSelect SecomFile
        {
            get { return AppConfig.Instance.SecomXmlFile; }
        }
        public EventBroadcaster EventBroadcaster
        {
            get { return m_EventBroadcaster; }
        }

        public bool Created { get { return m_Created; } }

        //public XLog ExceptionLog
        //{
        //    get { return m_ExceptionLog; }
        //}
        //public FilePath SecomPath   //2009.07.01 Youngsik
        //{
        //    get { return m_AppConfig.SecomXmlPath; }
        //}
        #endregion

        #region Constructor
        private ServerManager()
        {
            m_Created = (InitializeServerData() == DmsErrors.Success);

            if (!m_Created)
            {
                MessageBox.Show("Server fail to initialize properly. Program will be terminated.");
            }
        }
        #endregion

        #region Methods
        public void Log(string log)
        {
            string msg = string.Format("Server  \t{0}", log);
            m_EqpLog.TextOut(msg);
            if (m_GenInfos != null)
            {
                m_GenInfos.EqpLog = log;
            }
        }

        public DmsErrors Initialize()
        {
            if (m_Initialized)
            {
                return DmsErrors.Success;
            }

            try
            {
                Log("DMS Server : Initialize : Start");

                bool ok = true;

                ////////////////////////////////////////////////////////////////////
                // Initialize EventBroadcaster for .Net Remoting
                if (ok)
                {
                    m_EventBroadcaster = new EventBroadcaster(this, m_ServerMode);
                    ok &= m_EventBroadcaster.Initialize() == DmsErrors.Success;
                }

                ////////////////////////////////////////////////////////////////////
                // Initialize Device and DmsComponet objects
                if (ok)
                {
                    ok &= InitializeServerDevice() == DmsErrors.Success;
                    ok &= InitializeServerObjects() == DmsErrors.Success;
                }

                ////////////////////////////////////////////////////////////////////
                // Initialize Data Provider
                if (ok)
                {
                    m_DataProvider.DeleteGarbage();
                    m_GlassData.DeleteGarbage();
                    ok &= m_ApdItemsHandler.CreateHandler(this);
                    ok &= m_PartsItemsHandler.CreateHandler(this);
                }

                ////////////////////////////////////////////////////////////////////
                // Initialize Thread Handler
                if (ok)
                {
                    ThreadHandler = new ThreadHandler();
                    ThreadHandler.Initialize();
                    //ThreadHandler.Start(); // 11.02.19 minhan
                }

                ////////////////////////////////////////////////////////////////////
                // Set Server state
                if (ok)
                {
                    m_Initialized = ok;
                    m_ServerState = ActiveState.Initialized;
                    Log("DMS Server : Initialize : End");
                }
                else
                {
                    Log("DMS Server : Initialize : Fail");
                }

                return ok ? DmsErrors.Success : DmsErrors.InternalError;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
                //Application.Exit();

                Log("DMS Server : Initialize : Fail");

                return DmsErrors.InternalError;
            }
        }

        public void Uninitialize()
        {
            try
            {
                m_Initialized = false;

                Stop();

                ThreadHandler?.Pause();

                UninitializeServerObjects();

                m_ServerState = ActiveState.Uninitialized;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        public void UninitializeByException()
        {
            try
            {
                m_Initialized = false;

                Stop();

                UninitializeServerObjects();

                m_ServerState = ActiveState.Uninitialized;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        public void Start()
        {
            m_ServerState = ActiveState.Run;
        }

        public void Stop()
        {
            m_ServerState = ActiveState.Stop;
        }

        public void WriteExceptionLog(string log)
        {
            ExceptionLog.WriteLog(log);
        }

        public void FireEvent(object data, string msg)
        {
            //  m_EventBroadcaster.FireEvent(data, msg);
        }
        #endregion

        #region Method for Interface
        public void AddSeqInitFunction(XSeqInitFunction func)
        {
            ThreadHandler.MainControl.AddSeqInitFunction(func);
        }
        #endregion
    }
}

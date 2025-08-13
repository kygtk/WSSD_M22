///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : ClientManager
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Dms.Common;
using Dms.Device;

namespace Dms.Client
{
    public partial class ClientManager
    {
        #region Fields
        private string m_ClientId = "Client";
        private bool m_Initialized = false;
        //private XLog m_ClientLog = new XLog("ClientLog", XLog.LogStampType.UseStamp);
        //private XLog m_ExceptionLog = new XLog("ClientExceptionLog", XLog.LogStampType.UseStamp);
        private EventSubscriber m_EventSubscriber = null;
        private IDMSComm m_IDmsComm = null;
        private ServerMode m_ServerMode = ServerMode.Local;
        #endregion

        #region Singleton Code
        public static readonly ClientManager Instance = new ClientManager();
        #endregion

        #region Properties
        //public ServerManager Server
        //{
        //    get { return m_Server; }
        //    set { m_Server = value; }
        //}
        //public DeviceTags DeviceTagContainer
        //{
        //    get { return m_DeviceTagContainer; }
        //    set { m_DeviceTagContainer = value; }
        //}
        public EventSubscriber EventSubscriber
        {
            get { return m_EventSubscriber; }
        }
        public ServerMode ServerMode
        {
            get { return m_ServerMode; }
            set { m_ServerMode = value; }
        }
        public IDMSComm DMSComm
        {
            set { m_IDmsComm = value; }
        }
        #endregion

        #region Constructor
        private ClientManager()
        {

        }
        #endregion

        #region Methods
        public void Log(string log)
        {
            //string msg = string.Format("Client  \t{0}", log);
            //m_ClientLog.TextOut(msg);
            HmiLog.WriteLog(log);
        }

        public void WriteExceptionLog(string log)
        {
            //m_ExceptionLog.TextOut(log);
            ExceptionLog.WriteLog(log);
        }

        public DmsErrors Initialize(IServerManager server)
        {
            if (m_Initialized)
            {
                return DmsErrors.Success;
            }

            bool ok = true;
            m_ServerMode = AppConfig.Instance.ServerMode;
            m_EventSubscriber = new EventSubscriber(this, m_ServerMode, server);
            ok &= m_EventSubscriber.Initialize() == DmsErrors.Success;

            if (ok)
            {
                InitData();
            }
            else
            {
                m_EventSubscriber.Uninitialize();
            }

            m_Initialized = ok;

            return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
        }

        public DmsErrors Uninitialize()
        {
            /// Do final cleanup here....
            if (!m_Initialized) return DmsErrors.NotInitialized;

            m_EventSubscriber.Uninitialize();

            m_Initialized = false;
            return DmsErrors.Success;
        }

        public DmsErrors GetInitialData()
        {
            m_IDmsComm.GetInitialData();
            return DmsErrors.Success;
        }

        public DmsErrors SendCommand(Command cmd, params Object[] obj)
        {
            bool ok = m_IDmsComm.SendCommand(m_ClientId, cmd, obj);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //jemoon : Log
            string log = string.Format("Command : {0} - {1}", m_CurrentUserAccount.UserID, cmd.ToString());
            string para = "";
            int i = 0;
            foreach (object p in obj)
            {
                para += p.ToString();
                i++;
                if (i < obj.Length) para += " - ";
            }
            if (para != "")
            {
                log += (" - " + para);
            }

            this.Log(log);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (ok)
            {
                return DmsErrors.Success;
            }
            else
            {
                return DmsErrors.UnAuthorisedClient;
            }
        }
        #endregion
    }
}

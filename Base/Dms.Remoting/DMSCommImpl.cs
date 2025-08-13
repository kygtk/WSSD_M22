using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.Remoting
{
    public class DMSCommImpl : MarshalByRefObject, IDMSComm
    {
        private static object m_LockKey = new object();
        private IServerManager m_Server = null;

        public DMSCommImpl(IServerManager server)
        {
            m_Server = server;
        }

        public bool SendCommand(String clientId, Command cmd, params Object[] obj)
        {
            //if (m_Server.HMIMonitorController.IsController(clientId))
            {
                lock(m_LockKey)
                {
                    m_Server.CommandProc(cmd, obj);
                }

                return true;
            }
            //else
            //{
            //    return false;
            //}
        }

        public void GetInitialData()
        {
            //m_Server.EventBroadcaster.SendInitialDataToClient();
        }
    }
}

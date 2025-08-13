using Dms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMX3ApiCLR;
using wmx3Api;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        #region Const
        private const string c_WMXPath = "C:\\Program Files\\SoftServo\\WMX3";
        private const string c_DeviceName = "WSSD Sample Device";
        #endregion

        #region Fields
        private static WMX3Api m_Wmx = null;
        private static bool m_Initialized = false;
        private static bool m_Simualate = false;

        #endregion

        #region Properties
        public static bool Initialized
        {
            get { return m_Initialized; }
        }
        public static bool Simulate
        {
            get { return m_Simualate; }
            set { m_Simualate = value; }
        }
        #endregion

        #region Methods
        public static void Initialize()
        {
            if (m_Initialized) return;

            //  API Initialize
            {
                m_Wmx = new WMX3Api();
                m_Wmx.CreateDevice(c_WMXPath, DeviceType.DeviceTypeNormal, uint.MaxValue);
                m_Wmx.SetDeviceName(c_DeviceName);
            }

            //  하위 항목 Initialize
            {
                EngineCtl.Initialize();
                ServoCtl.Initialize();
                IoCtl.Initialize();
                EcCtl.Initialize();
            }

            m_Initialized = EngineCtl.Initialized
                         && ServoCtl.Initialized
                         && IoCtl.Initialized
                         && EcCtl.Initialized;
        }

        public static void Uninitialize()
        {
            //  하위 항목 Uninitialize
            {
                EngineCtl.Uninitialize();
                ServoCtl.Uninitialize();
                IoCtl.Uninitialize();
                EcCtl.Uninitialize();
            }

            m_Initialized = EngineCtl.Initialized
                         && ServoCtl.Initialized
                         && IoCtl.Initialized
                         && EcCtl.Initialized;

            //  API Uninitialize
            {
                m_Wmx.CloseDevice();
                m_Wmx.Dispose();
            }
        }
        #endregion
    }
}

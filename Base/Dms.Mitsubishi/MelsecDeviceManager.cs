using System;
using System.Collections.Generic;
using System.Text;
using Dms.Ctl;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public class MelsecDeviceManager
    {
        #region Fields
        private MelsecDeviceContainer m_Container = null;
        #endregion

        #region Properties
        public MelsecDeviceContainer MelContainer
        {
            get { return m_Container; }
            set { m_Container = value; }
        }
        #endregion
        
        #region Singleton
        public static readonly MelsecDeviceManager Instance = new MelsecDeviceManager();
        #endregion

        #region Constructor
        private MelsecDeviceManager()
        {
            m_Container = new MelsecDeviceContainer();
            m_Container.LoadDevicesFromDisk();
        }
        #endregion
    }
}

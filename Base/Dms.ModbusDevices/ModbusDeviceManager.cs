using System;
using System.Collections.Generic;
using System.Text;
using Dms.Ctl;

namespace Dms.ModbusDevices
{
    public class ModbusDeviceManager
    {
        #region Fields
        private ModbusDeviceContainer m_Container = null;
        #endregion

        #region Properties
        public ModbusDeviceContainer ModbusContainer
        {
            get { return m_Container; }
            set { m_Container = value; }
        }
        #endregion
        
        #region Singleton
        public static readonly ModbusDeviceManager Instance = new ModbusDeviceManager();
        #endregion

        #region Constructor
        private ModbusDeviceManager()
        {
            m_Container = new ModbusDeviceContainer();
            m_Container.LoadDevicesFromDisk();
        }
        #endregion
    }
}

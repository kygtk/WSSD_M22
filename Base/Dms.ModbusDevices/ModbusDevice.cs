using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Dms.Ctl;
using Dms.DeviceLibrary;

namespace Dms.ModbusDevices
{
    public enum mdevTYPE
    {
        mdevB = 0x1,
        mdevW = 0x2,
    }

    public struct ModbusDeviceInfo
    {
        public mdevTYPE m_DevType;
        public string m_StartAddress;
        public byte m_Size;
        public string m_Name;

        public ModbusDeviceInfo(mdevTYPE devType, string startAddr, byte size, string name)
        {
            m_DevType = devType;
            m_StartAddress = startAddr;
            m_Size = size;
            m_Name = name;
        }
    }

    public class ModbusDevice
    {
        #region Fields
        protected ModbusCommDevice m_Modbus = null;
        protected ModbusDeviceInfo m_DevInfo;
        private ushort m_StartAddress = 0;
        private byte m_UnitID = 0;
        #endregion

        #region Properties
        public byte UnitID
        {
            get { return m_UnitID; }
            set { m_UnitID = value; }
        }
        public mdevTYPE DevType
        {
            get { return m_DevInfo.m_DevType; }
            set { m_DevInfo.m_DevType = value; }
        }
        public string StartAddress
        {
            get { return m_DevInfo.m_StartAddress; }
            set
            {
                try
                {
                    m_StartAddress = Convert.ToUInt16(value, 16);
                    m_DevInfo.m_StartAddress = value;
                }
                catch 
                {
                    
                }
            }
        }
        public byte Size
        {
            get { return m_DevInfo.m_Size; }
            set { m_DevInfo.m_Size = value; }
        }
        public string Name
        {
            get { return m_DevInfo.m_Name; }
            set { m_DevInfo.m_Name = value; }
        }
        #endregion

        #region Constructor
        public ModbusDevice()
        {
            m_Modbus = ModbusCommDevice.Instance;
        }
        #endregion

        #region Methods
        public ushort GetStartAddress()
        {
            return m_StartAddress;
        }
        //public void SetModbus()
        //{
        //    m_Modbus = ModbusCommDevice.Instance;
        //}
        #endregion

        #region Override methods
        public override string ToString()
        {
            if (this.Name == "" || this.Name == null)
            {
                return "Not Defined";
            }
            else
            {
                string val;
                val = string.Format("{0}, {1}, {2}, {3}, {4}",
                    UnitID,
                    m_DevInfo.m_DevType.ToString(),
                    m_DevInfo.m_StartAddress,
                    m_DevInfo.m_Size,
                    m_DevInfo.m_Name);
                return val;
            }
        }
        #endregion
    }
}

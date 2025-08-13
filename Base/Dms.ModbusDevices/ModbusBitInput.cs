using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using Dms.DeviceLibrary;
using Dms.Ctl;

namespace Dms.ModbusDevices
{
    [Editor(typeof(UIEditorModbusDevice), typeof(UITypeEditor))]
    public class ModbusBitInput : ModbusDevice
    {
        #region Fields
        #endregion 

        #region Constructor
        public ModbusBitInput(byte unitID, byte size, string address, string name)
        {
            UnitID = unitID;
            StartAddress = address;

            m_DevInfo.m_DevType = mdevTYPE.mdevB;
            m_DevInfo.m_Size = size;
            m_DevInfo.m_StartAddress = address;
            m_DevInfo.m_Name = name;
        }

        public ModbusBitInput()
        {
            m_DevInfo.m_DevType = mdevTYPE.mdevB;
            m_DevInfo.m_Size = 1;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }
        #endregion

        #region Methods
        public bool GetStatus()
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_Name.ToUpper() == "NONE" || m_DevInfo.m_StartAddress.Contains("_") == true)
                {
                    return false;
                }
                else
                {
                    return m_Modbus.GetInputBit(UnitID, GetStartAddress());
                }
            }
            else
            {
                return false;
            }
        }

        public void SetStatus(bool val)
        {
            if (m_DevInfo.m_StartAddress.Contains("_") == false)
            {
                short sval = (short)(val ? 1 : 0);
                SetStatus(sval);
            }
        }
        
        public void SetStatus(short val)
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    m_Modbus.SetInputBit(UnitID, GetStartAddress(), Convert.ToBoolean(val));
                }
            }
            else
            {
                throw new Exception("m_Modbus is null");
            }
        }
        #endregion
    }
}

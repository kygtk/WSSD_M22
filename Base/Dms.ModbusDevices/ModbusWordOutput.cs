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
    public class ModbusWordOutput : ModbusDevice
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ModbusWordOutput(byte unitID, byte size, string address, string name)
        {
            UnitID = unitID;
            StartAddress = address;

            m_DevInfo.m_DevType = mdevTYPE.mdevW;
            m_DevInfo.m_Size = size;
            m_DevInfo.m_StartAddress = address;
            m_DevInfo.m_Name = name;
        }

        public ModbusWordOutput()
        {
            m_DevInfo.m_DevType = mdevTYPE.mdevW;
            m_DevInfo.m_Size = 1;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }

        public ModbusWordOutput(byte size) 
        {
            m_DevInfo.m_DevType = mdevTYPE.mdevW;
            m_DevInfo.m_Size = size;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }
        #endregion

        #region Methods
        public short GetValue()
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    return m_Modbus.GetOutputWord(UnitID, GetStartAddress());
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public void SetValue(short val)
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    m_Modbus.SetOutputWord(UnitID, GetStartAddress(), val);
                }
            }
            else
            {
            }
        }

        public short[] GetValues()
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    if (m_DevInfo.m_Size < 1)
                    {
                        throw new Exception("The Word size is not 1 : GetValues()");
                    }
                    else
                    {
                        short[] data = new short[m_DevInfo.m_Size];
                        data = m_Modbus.GetOutputWords(UnitID, GetStartAddress(), m_DevInfo.m_Size);
                        return data;
                    }
                }
                else
                {
                    short[] data = new short[m_DevInfo.m_Size];
                    return data;
                }
            }
            else
            {
                return null;
            }
        }
        public void SetValues(short[] val)
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    short[] temp;
                    if (this.Size == val.Length)
                    {
                        temp = val;
                    }
                    else
                    {
                        temp = new short[this.Size];
                        for (int i = 0; i < this.Size; i++)
                        {
                            temp[i] = val[i];
                        }
                    }

                    m_Modbus.SetOutputWords(UnitID, GetStartAddress(), this.Size, temp);
                }
            }
            else
            {

            }
        }

        public string GetString()
        {
            if (m_DevInfo.m_StartAddress.Contains("_") == false)
            {
                if (m_Modbus != null)
                {
                    if (m_DevInfo.m_Size != 1) return m_Modbus.GetOutputString(UnitID, GetStartAddress(), m_DevInfo.m_Size);
                    else throw new Exception("The Word size is 1 : GetString()");
                }
                else
                {
                    //throw new Exception("m_MelsecControlBoard is null");
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public void SetString(string val)
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    //jemoon : size가 같지 않은경우
                    string temp;
                    if ((this.Size*2) >= val.Length)
                    {
                        temp = val;
                    }
                    else
                    {
                        temp = val.Substring(0, (this.Size*2));
                    }

                    m_Modbus.SetOutputString(UnitID, GetStartAddress(), this.Size, temp);
                    //m_MelsecControlBoard.WriteString(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), temp, (short)temp.Length);
                }
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
            }
        }
        #endregion
    }
}

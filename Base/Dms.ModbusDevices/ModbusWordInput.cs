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
    public class ModbusWordInput : ModbusDevice
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ModbusWordInput(byte unitID, byte size, string address, string name)
        {
            UnitID = unitID;
            StartAddress = address;

            m_DevInfo.m_DevType = mdevTYPE.mdevW;
            m_DevInfo.m_Size = size;
            m_DevInfo.m_StartAddress = address;
            m_DevInfo.m_Name = name;
        }
        public ModbusWordInput()
        {
            m_DevInfo.m_DevType = mdevTYPE.mdevW;
            m_DevInfo.m_Size = 1;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }
        public ModbusWordInput(byte size)
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
                    if (m_DevInfo.m_Size == 1)
                    {
                        if (m_DevInfo.m_Name == "None" || m_DevInfo.m_StartAddress.ToUpper() == "0X____")
                        {
                            return 0;
                        }
                        else
                        {
                            return m_Modbus.GetInputWord(UnitID, GetStartAddress());
                        }
                    }
                    else throw new Exception("The Word size is not 1 : GetValue()");
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new Exception("m_Modbus is null");
            }
        }

        public string GetString()
        {
            if (m_Modbus != null)
            {
                if (m_DevInfo.m_StartAddress.Contains("_") == false)
                {
                    if (m_DevInfo.m_Size != 1)
                    {
                        return m_Modbus.GetInputString(UnitID, GetStartAddress(), m_DevInfo.m_Size);
                    }
                    else
                    {
                        throw new Exception("The Word size is 1 : GetString()");
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new Exception("m_Modbus is null");
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
                        data = m_Modbus.GetInputWords(UnitID, GetStartAddress(), m_DevInfo.m_Size);
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
                //throw new Exception("m_MelsecControlBoard is null");
            }        
        }

        //public void SetValue(short val)
        //{
        //    if (m_MelsecControlBoard != null)
        //    {
        //        if (m_MelsecControlBoard.Simulate)
        //        {
        //            m_MelsecControlBoard.SendWord(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), val);
        //        }
        //    }
        //    else
        //    {
        //        //throw new Exception("m_MelsecControlBoard is null");
        //    }
        //}

        //public void SetValues(short[] val)
        //{
        //    if (m_MelsecControlBoard != null)
        //    {
        //        if (m_MelsecControlBoard.Simulate)
        //        {
        //            //jemoon : size가 같지 않은경우
        //            short[] temp;
        //            if (this.Size == val.Length)
        //            {
        //                temp = val;
        //            }
        //            else
        //            {
        //                temp = new short[this.Size];
        //                for (int i = 0; i < this.Size; i++)
        //                {
        //                    temp[i] = val[i];
        //                }
        //            }

        //            m_MelsecControlBoard.SendWords(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), temp);
        //        }
        //    }
        //    else
        //    {
        //        //throw new Exception("m_MelsecControlBoard is null");
        //    }
        //}

        //public void SetString(string val)
        //{
        //    if (m_MelsecControlBoard != null)
        //    {
        //        if (!m_MelsecControlBoard.Simulate) throw new Exception("m_MelsecControlBoard is not simuate mode");

        //        //jemoon : size가 같지 않은경우
        //        string temp;
        //        if (this.Size >= val.Length)
        //        {
        //            temp = val;
        //        }
        //        else
        //        {
        //            temp = val.Substring(0, this.Size);
        //        }

        //        m_MelsecControlBoard.WriteString(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), temp, (short)temp.Length);
        //    }
        //    else
        //    {
        //        //throw new Exception("m_MelsecControlBoard is null");
        //    }
        //}
        #endregion
    }
}

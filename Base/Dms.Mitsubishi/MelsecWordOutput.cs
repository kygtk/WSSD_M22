using System;
using System.Collections.Generic;
using System.Text;
using Dms.DeviceLibrary;
using Dms.Ctl;
using System.Drawing.Design;
using System.ComponentModel;
using Dms.Common;

namespace Dms.Mitsubishi
{
    [Editor(typeof(UIEditorMelDevice), typeof(UITypeEditor))]
    public class MelsecWordOutput : MelsecDevice
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public MelsecWordOutput(Melsec mel)
        {
            m_MelsecControlBoard = mel;
            m_DevInfo.m_DevType = devTYPE.devW;
            m_DevInfo.m_Size = 1;
        }

        public MelsecWordOutput()
        {
            m_DevInfo.m_DevType = devTYPE.devW;
            m_DevInfo.m_Size = 1;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }

        public MelsecWordOutput(short size) 
        {
            m_DevInfo.m_DevType = devTYPE.devW;
            m_DevInfo.m_Size = size;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }
        #endregion

        #region Methods
        public short GetValue()
        {
            if (m_MelsecControlBoard != null)
                return m_MelsecControlBoard.ReceiveWord(GetStartAddress());
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
                return 0;
            }
        }

        public void SetValue(short val)
        {
            if (m_MelsecControlBoard != null)
                m_MelsecControlBoard.SendWord(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), val);
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
            }
        }

        public short[] GetValues()
        {
            if (m_MelsecControlBoard != null)
            {
                if (m_DevInfo.m_Size < 1)
                {
                    throw new Exception("The Word size is not 1 : GetValues()");
                }
                else
                {
                    short[] data = new short[m_DevInfo.m_Size];
                    data = m_MelsecControlBoard.ReceiveWords(GetStartAddress(), m_DevInfo.m_Size);
                    return data;
                }
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
                return null;
            }
        }

        public string GetString()
        {
            if (m_MelsecControlBoard != null)
            {
                if (m_DevInfo.m_Size != 1) return m_MelsecControlBoard.ReadString(GetStartAddress(), m_DevInfo.m_Size);
                else throw new Exception("The Word size is 1 : GetString()");
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
                return "";
            }
        }

        public void SetValues(short[] val)
        {
            if (m_MelsecControlBoard != null)
            {
                //jemoon : size가 같지 않은경우
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

                m_MelsecControlBoard.SendWords(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), temp);
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
            }
        }

        public void SetString(string val)
        {
            if (m_MelsecControlBoard != null)
            {
                //jemoon : size가 같지 않은경우
                string temp;
                if (this.Size >= val.Length)
                {
                    temp = val;
                }
                else
                {
                    temp = val.Substring(0, this.Size);
                }

                m_MelsecControlBoard.WriteString(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), temp, (short)temp.Length);
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
            }        
        }
        #endregion
    }
}

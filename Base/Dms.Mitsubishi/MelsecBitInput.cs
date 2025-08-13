using System;
using System.Collections.Generic;
using System.Text;
using Dms.Ctl;
using System.Drawing.Design;
using System.ComponentModel;
using Dms.Common;

namespace Dms.Mitsubishi
{
    [Editor(typeof(UIEditorMelDevice), typeof(UITypeEditor))]
    public class MelsecBitInput : MelsecDevice
    {
        #region Fields
        #endregion 

        #region Constructor
        public MelsecBitInput(Melsec mel)
        {
            m_MelsecControlBoard = mel;
            m_DevInfo.m_DevType = devTYPE.devB;
            m_DevInfo.m_Size = 1;
        }

        public MelsecBitInput()
        {
            m_DevInfo.m_DevType = devTYPE.devB;
            m_DevInfo.m_Size = 1;
            m_DevInfo.m_Name = "None";
            m_DevInfo.m_StartAddress = "0X____";
        }
        #endregion

        #region Methods
        public bool GetStatus()
        {
            if (m_MelsecControlBoard != null)
            {
                if (m_DevInfo.m_Name == "None" || m_DevInfo.m_StartAddress.ToUpper() == "0X____")
                {
                    return false;
                }
                else
                {
                    return m_MelsecControlBoard.ReceiveBit(GetStartAddress());
                }
            }
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
                return false;
            }
        }

        public void SetStatus(bool val)
        {
            short sval = (short)(val ? 1 : 0);
            SetStatus(sval);
        }

        public void SetStatus(short val)
        {
            if (m_MelsecControlBoard != null)
            {
                if (m_MelsecControlBoard.Simulate)
                {
                    m_MelsecControlBoard.SendBit(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), val);
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

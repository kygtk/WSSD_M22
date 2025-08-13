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
    public class MelsecBitOutput : MelsecDevice
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public MelsecBitOutput(Melsec mel)
        {
            m_MelsecControlBoard = mel;
            m_DevInfo.m_DevType = devTYPE.devB;
            m_DevInfo.m_Size = 1;
        }
        public MelsecBitOutput()
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
                return m_MelsecControlBoard.ReceiveBit(GetStartAddress());
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
                return false;
            }
        }

        public void SetStatus(short val)
        {
            if (m_MelsecControlBoard != null)
                m_MelsecControlBoard.SendBit(m_MelsecControlBoard.NetworkNo, m_MelsecControlBoard.StationNo, GetStartAddress(), val);
            else
            {
                //throw new Exception("m_MelsecControlBoard is null");
            }
        }

        public void SetStatus(bool val)
        {
            short sval = (short)(val ? 1 : 0);
            SetStatus(sval);
        }
        #endregion
    }
}

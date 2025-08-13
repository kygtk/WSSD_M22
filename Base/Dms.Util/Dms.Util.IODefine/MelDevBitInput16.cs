///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.23
// Author       : jemoon
// Description  : Melsec Device B input 
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class MelDevBitInput16 : MelsecTerminal
    {
        // MelsecInputDevB : DI 16 channel
        #region Constructor
        public MelDevBitInput16()
        {
			m_DevType = devTYPE.devB;
			m_IoType = Dms.Common.IoType.DI;
			m_ChannelCount = 16;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Digital in 16 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecBIn16";
            m_ProductId = 0;
        } 
        #endregion
    }
}

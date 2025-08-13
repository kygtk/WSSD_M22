///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.23
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
    public class MelDevBitInput08 : MelsecTerminal
    {
        // MelsecInputDevB : DI 8 channel
        #region Constructor
        public MelDevBitInput08()
        {
			m_DevType = devTYPE.devB;
			m_IoType = Dms.Common.IoType.DI;
			m_ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Digital in 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecBIn8";
            m_ProductId = 0;
        } 
        #endregion
    }
}

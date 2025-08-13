///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.23
// Author       : jemoon
// Description  : Melsec Device B output 
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
    public class MelDevBitOutput08 : MelsecTerminal
    {
        // MelsecOutputDevB : DO 8 channel
        #region Constructor
        public MelDevBitOutput08()
        {
			m_DevType = devTYPE.devB;
			m_IoType = Dms.Common.IoType.DO;
			m_ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Digital out 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecBOut8";
            m_ProductId = 0;
        } 
        #endregion
    }
}

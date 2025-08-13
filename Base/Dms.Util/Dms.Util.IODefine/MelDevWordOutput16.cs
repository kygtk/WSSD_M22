///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.23
// Author       : jemoon
// Description  : Melsec Device W output 
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
    public class MelDevWordOutput16 : MelsecTerminal
    {
        // MelsecOutputDevW : AO 16 channel
        #region Constructor
        public MelDevWordOutput16()
        {
			m_DevType = devTYPE.devW;
			m_IoType = Dms.Common.IoType.AO;
			m_ChannelCount = 16;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog out 16 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecWOut16";
            m_ProductId = 0;
        } 
        #endregion
    }
}

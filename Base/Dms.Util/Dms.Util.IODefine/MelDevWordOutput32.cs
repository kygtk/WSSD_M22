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
    public class MelDevWordOutput32 : MelsecTerminal
    {
        // MelsecOutputDevW : AO 32 channel
        #region Constructor
        public MelDevWordOutput32()
        {
			m_DevType = devTYPE.devW;
			m_IoType = Dms.Common.IoType.AO;
			m_ChannelCount = 32;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog out 32 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecWOut32";
            m_ProductId = 0;
        } 
        #endregion
    }
}

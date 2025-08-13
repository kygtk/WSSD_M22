///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Null 8pts)
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    [Serializable()]
	public class CCNull08 : CClinkStation
    {
        // CC8Null : Null 8 channel
        #region Constructor
        public CCNull08()
        {
            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Null 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 8 Null";
            m_ProductId = 0;
        } 
        #endregion
    }
}

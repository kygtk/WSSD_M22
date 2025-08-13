///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Null 16pts)
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
    public class CCNull16 : IoTerminal
    {
        // CC16Null : Null 16 channel
        #region Constructor
        public CCNull16()
        {
            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 16;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Null 16 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 16 Null";
            m_ProductId = 0;
        } 
        #endregion
    }
}

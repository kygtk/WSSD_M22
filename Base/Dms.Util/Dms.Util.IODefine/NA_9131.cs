///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Digital input 16pts)
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
    public class NA_9131 : CClinkStation
    {
        // NA_9131 : DI 32 channel
        #region Constructor
        public NA_9131()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteDevice;
			m_Info.TerminalType = CclinkTerminalType.NA;
			m_Info.Points = 0;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 0;

            //Parts Info
            m_PartCode = "";
            m_PartName = "NA-9131";
            m_PartSpec = "";
            m_PartDescription = "Network Card";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link NA-9131 Network Card";
            m_ProductId = 0;
        } 
        #endregion
    }
}

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
    public class ST_1228 : CClinkStation
    {
        // ST-1228 : DI 8 channel
        #region Constructor
        public ST_1228()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteIo;
			m_Info.TerminalType = CclinkTerminalType.DI;
			m_Info.Points = 8;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "ST-1228";
            m_PartSpec = "";
            m_PartDescription = "Digital in 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link ST-1228 Digital Input 8pts";
            m_ProductId = 0;
        } 
        #endregion
    }
}

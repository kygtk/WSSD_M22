///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Digital output 16pts)
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
	public class ST_2318 : CClinkStation
    {
        // ST-2318 : DO 8 channel
        #region Constructor
        public ST_2318()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteDevice;
			m_Info.TerminalType = CclinkTerminalType.DO;
			m_Info.Points = 8;

            this.IoType = Dms.Common.IoType.DO;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "ST-2318";
            m_PartSpec = "";
            m_PartDescription = "Digital out 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link ST-2318 Digital Output 8 pts";
            m_ProductId = 0;
        } 
        #endregion
    }
}

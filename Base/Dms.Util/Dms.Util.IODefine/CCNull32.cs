///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Null 32pts)
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
	public class CCNull32 : CClinkStation
    {
        // CC32Null : Null 32 channel
        #region Constructor
        public CCNull32()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.Reserved;
			m_Info.Points = 32;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 32;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Null 32 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;//.MitsubishiCCLink;20130530
            m_ProductName = "CC Link 32 Null";
            m_ProductId = 0;
        } 
        #endregion
    }
}

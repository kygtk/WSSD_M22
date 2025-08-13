///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.13
// Author       : jemoon
// Description  : CC Link(Reserved 32pts)
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
	public class CCReserved : CClinkStation
    {
        // CCReserved : Reserved Station
        #region Constructor
		public CCReserved()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.Reserved;
			m_Info.Points = 0;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 0;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
			m_PartDescription = "Reserved Station";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link Reserved Station";
            m_ProductId = 0;
        } 
        #endregion
    }
}

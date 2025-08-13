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
    public class AT2_R321 : CClinkStation
    {
        // AT2-R321 : DO 32 channel
        #region Constructor
        public AT2_R321()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteDevice;
			m_Info.TerminalType = CclinkTerminalType.DO;
			m_Info.Points = 32;

            this.IoType = Dms.Common.IoType.DO;
            this.ChannelCount = 32;

            //Parts Info
            m_PartCode = "";
            m_PartName = "AT2-R321";
            m_PartSpec = "";
            m_PartDescription = "Digital Output 32 pts Adapter";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link AT2-R321 DO 32 Adapter";
            m_ProductId = 0;
        } 
        #endregion
    }
}

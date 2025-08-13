///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Digital input 32pts)
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
    public class CC32D : CClinkStation
    {
        // CC32D : DI 32 channel
        #region Constructor
        public CC32D()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteIo;
			m_Info.TerminalType = CclinkTerminalType.DI;
			m_Info.Points = 32;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 32;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Digital in 32 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 32D";
            m_ProductId = 0;
        } 
        #endregion
    }
}

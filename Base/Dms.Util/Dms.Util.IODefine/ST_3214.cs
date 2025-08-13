///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Analog input 4pts)
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
    public class ST_3214 : CClinkStation
    {
        // ST_3214 : AI 4 channel
        #region Constructor
        public ST_3214()
        {
            //Station Info
            m_Info.StationOccupies = 1;
            m_Info.StationType = CclinkStationType.RemoteDevice;
            m_Info.TerminalType = CclinkTerminalType.AI;
            m_Info.Points = 4;//jemoon : 2011.03.21, 64 -> 4

            this.IoType = Dms.Common.IoType.AI;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "";
            m_PartName = "ST-3214";
            m_PartSpec = "";
            m_PartDescription = "Analog Input 4 pts 4 ~ 20mA";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link ST-3214 Analog Input 4 pts";
            m_ProductId = 0;
        }
        #endregion
    }
}

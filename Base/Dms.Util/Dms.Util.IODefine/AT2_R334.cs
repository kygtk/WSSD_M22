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
    public class AT2_R334 : CCSpecialDeivce
    {
        // AT2-R334 : DO 32 channel
        #region Constructor
        public AT2_R334()
        {
            //Station Info
            m_Info.StationOccupies = 1;
            m_Info.StationType = CclinkStationType.RemoteDevice;
            m_Info.TerminalType = CclinkTerminalType.DIO;
            m_Info.Points = 16;
            m_Info.AddressAllocType = AddressAllocType.Inclusive;

            m_IoDataType = Dms.Common.IoDataType.Digital;

            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = m_Info.Points * 2;;

            //Parts Info
            m_PartCode = "";
            m_PartName = "AT2-R334";
            m_PartSpec = "";
            m_PartDescription = "Digital Input Output 32 pts Adapter";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "CC Link AT2-R334 DiO 32 Adapter";
            m_ProductId = 0;
        }
        #endregion
    }
}

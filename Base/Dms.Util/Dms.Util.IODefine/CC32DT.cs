///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.12
// Author       : jemoon
// Description  : CC Link(Digital input 16pts and output 16pts)
//-------------------------------------------------------------------------
// Revison History
// * jemoon : 100503 - m_AddressAllocType 제거, m_Info.AddressAllocType 로 통일 by 동훈
//
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class CC32DT : CCSpecialDeivce
    {
        // CC32DT : DI 16 + DO 16 = 32 channel
        #region Constructor
        public CC32DT()
        {
            //Station Info
            m_Info.StationOccupies = 1;
            m_Info.StationType = CclinkStationType.RemoteIo;
            m_Info.TerminalType = CclinkTerminalType.DIO;
            m_Info.AddressAllocType = AddressAllocType.Exclusive;
            m_Info.Points = 16;

            m_IoDataType = Dms.Common.IoDataType.Digital;

            m_IoType = Dms.Common.IoType.DI;

            m_ChannelCount = m_Info.Points * 2;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Digital in 16 + out 16 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 32DT";
            m_ProductId = 0;
        }
        #endregion
    }
}

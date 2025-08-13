///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.26
// Author       : jemoon
// Description  : CC Link(CCIntelligentDeivce 32 Point)
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
	public class CCIntelligentDeivce32DT : CCSpecialDeivce
    {
		// CCIntelligentDeivce32 : DI 32 + DO 32 = 64 channel
        #region Constructor
		public CCIntelligentDeivce32DT()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteIo;
			m_Info.TerminalType = CclinkTerminalType.DIO;
			m_Info.AddressAllocType = AddressAllocType.Inclusive;
			m_Info.Points = 32;

			m_IoDataType = Dms.Common.IoDataType.Digital;

            m_IoType = Dms.Common.IoType.DI;

			m_ChannelCount = m_Info.Points * 2;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
			m_PartDescription = "IntelligentDeivce 32 Point";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
			m_ProductName = "CC Link IntelligentDeivce32DT";
            m_ProductId = 0;
        } 
        #endregion
    }
}

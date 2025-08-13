///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.12
// Author       : jemoon
// Description  : CC Link(Analog output 2pts)
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
    public class CC62DA : CClinkStation
    {
		#region Fields
		protected CclinkRangeType[] m_RangeType = new CclinkRangeType[] { 
			CclinkRangeType._0_5V, 
			CclinkRangeType._0_5V };
		protected CclinkOutputType[] m_OutputType = new CclinkOutputType[] { 
			CclinkOutputType.Hold, 
			CclinkOutputType.Hold };
		#endregion

		#region Properties
		[Category("CC Link Station Special Info")]
		public CclinkRangeType[] RangeType
		{
			get { return m_RangeType; }
			set { m_RangeType = value; }
		}
		[Category("CC Link Station Special Info")]
		public CclinkOutputType[] OutputType
		{
			get { return m_OutputType; }
			set { m_OutputType = value; }
		} 
		#endregion

		// CC62DA : AO 2 channel
        #region Constructor
		public CC62DA()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteDevice;
			m_Info.TerminalType = CclinkTerminalType.AO;
			m_Info.Points = 2;

            this.IoType = Dms.Common.IoType.AO;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog out 2 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 62DA";
            m_ProductId = 0;
        } 
        #endregion

		#region Override
		public override CclinkRangeType[] GetRangeTypeConfig()
		{
			return m_RangeType;
		}

		public override CclinkOutputType[] GetOuptTypeConfig()
		{
			return m_OutputType;
		}
		#endregion
	}
}

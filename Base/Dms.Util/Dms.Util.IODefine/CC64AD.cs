///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.12
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
    public class CC64AD : CClinkStation
    {
		#region Fields
		protected CclinkRangeType[] m_RangeType = new CclinkRangeType[] { 
			CclinkRangeType._4_20mA,
			CclinkRangeType._4_20mA, 
			CclinkRangeType._4_20mA,
			CclinkRangeType._4_20mA };
		protected CclinkMovingAvgType[] m_MovingAvgType = new CclinkMovingAvgType[] { 
			CclinkMovingAvgType._4Times, 
			CclinkMovingAvgType._4Times, 
			CclinkMovingAvgType._4Times, 
			CclinkMovingAvgType._4Times };
		#endregion

		#region Properties
		[Category("CC Link Station Special Info")]
		public CclinkRangeType[] RangeType
		{
			get { return m_RangeType; }
			set { m_RangeType = value; }
		}
		[Category("CC Link Station Special Info")]
		public CclinkMovingAvgType[] MovingAvgType
		{
			get { return m_MovingAvgType; }
			set { m_MovingAvgType = value; }
		}
		#endregion

		// CC64AD : AI 4 channel
        #region Constructor
		public CC64AD()
        {
			//Station Info
			m_Info.StationOccupies = 1;
			m_Info.StationType = CclinkStationType.RemoteDevice;
			m_Info.TerminalType = CclinkTerminalType.AI;
			m_Info.Points = 4;

            this.IoType = Dms.Common.IoType.AI;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog in 4 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link 64AD";
            m_ProductId = 0;
        } 
        #endregion

		#region Override
		public override CclinkRangeType[] GetRangeTypeConfig()
		{
			return m_RangeType;
		}
		public override CclinkMovingAvgType[] GetMovingAvgTypeConfig()
		{
			return m_MovingAvgType;
		}
		#endregion
	}
}

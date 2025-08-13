///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.12
// Author       : jemoon
// Description  : CC Link Station
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
	public class CClinkStaionInfo
	{
		public int MasterNo = 1;
		public int StationNo = 1;
		public int StationId = 0;
		public int StationOccupies = 1;
		public CclinkStationType StationType; //RemoteIO, RemoteDevice
		public CclinkTerminalType TerminalType; //DI, DO, AI, AO, DIO
		public AddressAllocType AddressAllocType = AddressAllocType.Exclusive;
		public int Points = 32;
		public int AddressRx;
		public int AddressRy;
		public int AddressRWr;
		public int AddressRWw;
		public int IndexDiBuf;
		public int IndexDoBuf;
		public int IndexAiBuf;
		public int IndexAoBuf;
		public UInt16 Usage;
		public UInt16 RangeType;
		public UInt16 MovingAvg;
		public bool HasTrouble;
		public bool InitComp;
	}

    [Serializable()]
    abstract public class CClinkStation : IoTerminal
	{
		#region Fields
		protected CClinkStaionInfo m_Info = new CClinkStaionInfo();
        protected int m_StationNo = 0;
		#endregion

		#region Properties
		[Browsable(false)]
		public CClinkStaionInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
        [Browsable(false)]
        public int StationNo
        {
            get { return m_StationNo; }
            set { m_StationNo = value; }
        }      

		#endregion

		// CClinkStation
        #region Constructor
		public CClinkStation()
        {
            //Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;
            m_ProductName = "CC Link Station";
            m_ProductId = 0;
        } 
        #endregion

		#region Methods
		public virtual CclinkRangeType[] GetRangeTypeConfig()
		{
			return null;
		}

		public virtual CclinkOutputType[] GetOuptTypeConfig()
		{
			return null;
		}

		public virtual CclinkMovingAvgType[] GetMovingAvgTypeConfig()
		{
			return null;
		}
        #endregion

        #region Overrides
        public override string ToString()
		{
			return this.GetType().Name + "." + m_Info.TerminalType;
		}
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.23
// Author       : jemoon
// Description  : Melsec Device B input 
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class MelsecTerminal : IoTerminal
    {
        // MelsecNetTerminal
        #region Fields
        protected short m_NetworkNo = 0;
		protected short m_StationNo = 255;
		protected devTYPE m_DevType = devTYPE.devB;
		protected int m_OffsetDec = 0;
		protected string m_OffsetHex = "0x0000";
        protected LinkMemoryDivision m_LinkMemoryDivision = LinkMemoryDivision.NONE;
		public const short _MaxBitSizeBlockRW = 15360;
		public const short _MaxWordSizeBlockRW = 960;
		public const short _MaxBitSizeRandomRead = 192;
		public const short _MaxWordSizeRandomRead = 12;
		public const short _MaxBitSizeRandomWrite = 188;
		public const short _MaxWordSizeRandomWrite = 120;
        #endregion

        #region Properties
        [Category("Connection Info")]
        public short NetworkNo
        {
            get { return m_NetworkNo; }
            set { m_NetworkNo = value; }
        }
        [Category("Connection Info")]
        public short StationNo
        {
            get { return m_StationNo; }
            set { m_StationNo = value; }
        }
        [Category("Address")]
        public devTYPE DeviceType
        {
            get { return m_DevType; }
			set { m_DevType = value; }
        }
        [Category("Address")]
        public string OffsetHex
        {
            get { return m_OffsetHex; }
            set
            {
                m_OffsetHex = value;
                m_OffsetDec = Convert.ToInt32(m_OffsetHex, 16);
            }
        }
        [Category("Address")]
		public int OffsetDec
        {
            get { return m_OffsetDec; }
            set 
			{ 
				m_OffsetDec = value;
				m_OffsetHex = string.Format("0x{0:X4}", m_OffsetDec);
			}
        }
		[Category("Address")]
		public AddressingType AddressType
		{
			get { return GetAddressingType(m_DevType); }
		}

        [Category("Address")]
        public LinkMemoryDivision LinkMemoryDivision
        {
            get { return m_LinkMemoryDivision; }
            set { m_LinkMemoryDivision = value; }
        }

        #endregion

        #region Constructor
        public MelsecTerminal()
        {
        } 
        #endregion

		#region Methods
		public static string GetDevTypeCategoryName(devTYPE dev)
		{
			string name = dev.ToString();
			name = name.Replace("dev", "");

			return name;
		}

		public enum AddressingType
		{ 
			HEX,
			DEC
		}

		public static AddressingType GetAddressingType(devTYPE dev)
		{
			AddressingType addr = AddressingType.DEC;
			switch (dev)
			{
				case devTYPE.devX:
				case devTYPE.devY:
				case devTYPE.devB:
				case devTYPE.devW:
					addr = AddressingType.HEX;
					break;
				case devTYPE.devL:
				case devTYPE.devM:
				case devTYPE.devF:
				case devTYPE.devD:
				case devTYPE.devR:
				case devTYPE.devZR:
					addr = AddressingType.DEC;
					break;
			}

			return addr;
		}

		public static IoType GetIoType(IoInOutType inoutType, devTYPE dev)
		{
			IoType ioType = IoType.DI;
			switch (dev)
			{
				case devTYPE.devX:
				case devTYPE.devY:
				case devTYPE.devL:
				case devTYPE.devM:
				case devTYPE.devF:
				case devTYPE.devB:
					ioType = inoutType == IoInOutType.In ? IoType.DI : IoType.DO;
					break;
				case devTYPE.devD:
				case devTYPE.devR:
				case devTYPE.devW:
				case devTYPE.devZR:
					ioType = inoutType == IoInOutType.In ? IoType.AI : IoType.AO;
					break;
			}
			return ioType;
		}

		public static IoDataType GetIoDataType(devTYPE dev)
		{
			IoDataType ioDataType = IoDataType.Digital;
			switch (dev)
			{
				case devTYPE.devX:
				case devTYPE.devY:
				case devTYPE.devL:
				case devTYPE.devM:
				case devTYPE.devF:
				case devTYPE.devB:
					ioDataType = IoDataType.Digital;
					break;
				case devTYPE.devD:
				case devTYPE.devR:
				case devTYPE.devW:
				case devTYPE.devZR:
					ioDataType = IoDataType.Analog;
					break;
			}
			return ioDataType;
		}

		private static Array m_DevTypes = Enum.GetValues(typeof(devTYPE));
		public static int GetDeviceTypeId(devTYPE dev)
		{
			int id = 0;
			int count = m_DevTypes.Length;
			foreach(object obj in m_DevTypes)			
			{
				if ((devTYPE)obj == dev)
				{
					break;
				}
				id++;
			}

			return id;
		}

		public MelsecTerminal Clone()
		{
			MelsecTerminal terminal = this.CreateObject() as MelsecTerminal;
			terminal.Id = m_Id;
			terminal.DeviceType = m_DevType;
			terminal.IoType = m_IoType;
			terminal.NetworkNo = m_NetworkNo;
			terminal.StationNo = m_StationNo;
			terminal.OffsetDec = m_OffsetDec;
            terminal.LinkMemoryDivision = m_LinkMemoryDivision;
			terminal.ChannelCount = m_ChannelCount;

			for (int i = 0; i < m_ChannelCount; i++)
			{
				terminal.Channels.Add(m_Channels[i].Clone());
			}

			return terminal;			
		}
		#endregion

		#region Override
		public override string ToString()
		{
			return this.GetType().Name + "." + /*this.IoType + "." +*/ GetDevTypeCategoryName(m_DevType);
		}
		#endregion
	}
}

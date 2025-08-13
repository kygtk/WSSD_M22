using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;
using System.Net.Sockets;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class IoNodeMelsec : IoNode
    {
		//public enum PollingMethod
		//{ 
		//    Block,
		//    Random
		//}

        #region Fields
        private channel m_MelsecChannel = channel.melsecnet10_Slot1;        
        private string m_IpAddress = "192.168.3.1";
        private ushort m_PortNo = 1000;
		private ProtocolType m_EthenetProtocol = ProtocolType.Tcp;
		private bool m_ProcessWorkingSetActivate = false;
		private ushort m_ProcessMinWorkingSetSize = 1;	//1MB
		private ushort m_ProcessMaxWorkingSetSize = 3;	//3MB
        //private PollingMethod m_PollMethod = PollingMethod.Block;
		private CClinkMasterInfo m_CcLinkMasterInfo = new CClinkMasterInfo();
        #endregion

        #region Properties
        [Category("Basic Info")]
        public channel ChannelNo
        {
            get { return m_MelsecChannel; }
            set { m_MelsecChannel = value; }
        }
        [Category("Connection Info : Only for Ethernet type")]
        public string EtherNetIpAddress
        {
            get { return m_IpAddress; }
            set { m_IpAddress = value; }
        }
		[Category("Connection Info : Only for Ethernet type")]
        public ushort EtherNetPortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
		[Category("Connection Info : Only for Ethernet type")]
		public ProtocolType EthenetProtocol
		{
			get { return m_EthenetProtocol; }
			set { m_EthenetProtocol = value; }
		}
		[Category("!Working Set"), Description("Set true when an error(code 77) occurs due to MD function execution")]
		public bool ProcessWorkingSetActivate
		{
			get { return m_ProcessWorkingSetActivate; }
			set { m_ProcessWorkingSetActivate = value; }
		}
		[Category("!Working Set"), Description("Minimum Size (MB)")]
		public ushort ProcessMinWorkingSetSize
		{
			get { return m_ProcessMinWorkingSetSize; }
			set { m_ProcessMinWorkingSetSize = value; }
		}
		[Category("!Working Set"), Description("Maximum Size (MB)")]
		public ushort ProcessMaxWorkingSetSize
		{
			get { return m_ProcessMaxWorkingSetSize; }
			set { m_ProcessMaxWorkingSetSize = value; }
		}
		//[Category("Connection Info")]
		//public PollingMethod PollMethod
		//{
		//    get { return m_PollMethod; }
		//    set { m_PollMethod = value; }
		//}
		[Category("Connection Info : Only for CC Link type")]
		[Browsable(false), XmlIgnore()]
		public CClinkMasterInfo CcLinkMasterInfo
		{
			get { return m_CcLinkMasterInfo; }
			set { m_CcLinkMasterInfo = value; }
		}
		[Category("Connection Info : Only for CC Link type")]
		public devTYPE DevTypeRx
		{
			get { return m_CcLinkMasterInfo.DevTypeRx; }
			set { m_CcLinkMasterInfo.DevTypeRx = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public devTYPE DevTypeRy
		{
			get { return m_CcLinkMasterInfo.DevTypeRy; }
			set { m_CcLinkMasterInfo.DevTypeRy = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public devTYPE DevTypeRWr
		{
			get { return m_CcLinkMasterInfo.DevTypeRWr; }
			set { m_CcLinkMasterInfo.DevTypeRWr = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public devTYPE DevTypeRWw
		{
			get { return m_CcLinkMasterInfo.DevTypeRWw; }
			set { m_CcLinkMasterInfo.DevTypeRWw = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		protected devTYPE DevTypeSb
		{
			get { return m_CcLinkMasterInfo.DevTypeSb; }
			set { m_CcLinkMasterInfo.DevTypeSb = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public devTYPE DevTypeSw
		{
			get { return m_CcLinkMasterInfo.DevTypeSw; }
			set { m_CcLinkMasterInfo.DevTypeSw = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressBase
		{
			get { return m_CcLinkMasterInfo.AddressBase; }
			set { m_CcLinkMasterInfo.AddressBase = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressRx
		{
			get { return m_CcLinkMasterInfo.AddressRx; }
			set { m_CcLinkMasterInfo.AddressRx = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressRy
		{
			get { return m_CcLinkMasterInfo.AddressRy; }
			set { m_CcLinkMasterInfo.AddressRy = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressRWr
		{
			get { return m_CcLinkMasterInfo.AddressRWr; }
			set { m_CcLinkMasterInfo.AddressRWr = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressRWw
		{
			get { return m_CcLinkMasterInfo.AddressRWw; }
			set { m_CcLinkMasterInfo.AddressRWw = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressSb
		{
			get { return m_CcLinkMasterInfo.AddressSb; }
			set { m_CcLinkMasterInfo.AddressSb = value; }
		}

		[Category("Connection Info : Only for CC Link type")]
		public int AddressSw
		{
			get { return m_CcLinkMasterInfo.AddressSw; }
			set { m_CcLinkMasterInfo.AddressSw = value; }
		}
        [Category("Connection Info : Only for CC Link type")]
        public int[] ReadyBit // 11.02.24 minhan
        {
            get { return m_CcLinkMasterInfo.ReadyBit; }
            set { m_CcLinkMasterInfo.ReadyBit = value; }
        }
        [Category("Connection Info : Only for CC Link type")]
        public bool ReadyBitUse // 11.03.07 minhan
        {
            get { return m_CcLinkMasterInfo.ReadyBitUse; }
            set { m_CcLinkMasterInfo.ReadyBitUse = value; }
        }
        #endregion

        #region Constructor
        public IoNodeMelsec()
        {
        }
        public IoNodeMelsec(FieldBusType busType)
        {
            MakeNodeModule(busType);

            // for default value
            switch (busType)
            {
                case FieldBusType.MitsubishiCClink:
                    {
                        m_MelsecChannel = channel.cclink_Slot1;                       
                    }
                    break;
                case FieldBusType.CrevisCClink:
                    {
                        m_MelsecChannel = channel.cclink_Slot1;                        
                    }
                    break;

                case FieldBusType.MitsubishiMelsecNet:
                    {
                        m_MelsecChannel = channel.melsecnet10_Slot1;                        
                    }
                    break;
				case FieldBusType.MitsubishiMelsecEtherNet:
					{
						m_MelsecChannel = channel.melsecEthernet;                        
					}
					break;
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Id + " : " + "Melsec Master";
        } 
        #endregion
    }
}

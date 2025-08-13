///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.02.13
// Author       : byeongmin
// Description  : Movensys IO Slave 
//-------------------------------------------------------------------------
// Revison History
// * 2025.04.7 : Create
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class DMS_AP : EcSlave_AP
    {
        //  DMS AP : AP 16 channel
        #region Constructor
        public DMS_AP()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.AP;

            //  Product Info
            m_VendorId = 0x00000F1C;
            m_VendorName = "DMS Co,. Ltd.";
            m_ProductCode = 0x10000100;
            m_ProductName = "EtherCAT_Slave_AP";
            m_Description = "Wireless AP 16";

            //  Station Info
            m_ChannelCount = 16;
        }
        #endregion

        #region Overrides
        public override EcSlave Copy()
        {
            DMS_AP slave = new DMS_AP();

            slave.m_SlaveNo = this.m_SlaveNo;
            slave.m_AliasNo = this.m_AliasNo;
            slave.m_SlaveType = this.m_SlaveType;

            slave.m_VendorId = this.m_VendorId;
            slave.m_VendorName = this.m_VendorName;
            slave.m_ProductCode = this.m_ProductCode;
            slave.m_ProductName = this.m_ProductName;
            slave.m_Description = this.m_Description;

            slave.m_ChannelCount = this.m_ChannelCount;

            return slave;
        }
        #endregion
    }
}

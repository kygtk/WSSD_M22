///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.02.14
// Author       : byeongmin
// Description  : Movensys IO Slave 
//-------------------------------------------------------------------------
// Revison History
// * 2025.02.14 : Create
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class BHY_IO2_EC_DI32NT : EcSlave_DI
    {
        //  BHY-IO2-EC-DI16NT : DI 32 channel
        #region Constructor
        public BHY_IO2_EC_DI32NT()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.DI;

            //  Product Info
            m_VendorId = 0x00009555;
            m_VendorName = "MOVENSYS";
            m_ProductCode = 0x10000002;
            m_ProductName = "BHY-IO2-EC-DI32NT";
            m_Description = "Digital Input 32";

            //  Station Info
            this.m_InChannelCount = 32;
        }
        #endregion

        #region Override
        public override EcSlave Copy()
        {
            BHY_IO2_EC_DI32NT slave = new BHY_IO2_EC_DI32NT();

            slave.m_SlaveNo = this.m_SlaveNo;
            slave.m_AliasNo = this.m_AliasNo;
            slave.m_SlaveType = this.m_SlaveType;

            slave.m_VendorId = this.m_VendorId;
            slave.m_VendorName = this.m_VendorName;
            slave.m_ProductCode = this.m_ProductCode;
            slave.m_ProductName = this.m_ProductName;
            slave.m_Description = this.m_Description;

            slave.m_InChannelCount = this.m_InChannelCount;

            return slave;
        }
        #endregion
    }
}

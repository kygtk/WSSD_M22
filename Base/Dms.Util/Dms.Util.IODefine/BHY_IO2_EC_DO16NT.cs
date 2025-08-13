///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.02.13
// Author       : byeongmin
// Description  : Movensys IO Slave 
//-------------------------------------------------------------------------
// Revison History
// * 2025.02.13 : Create
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class BHY_IO2_EC_DO16NT : EcSlave_DO
    {
        //  BHY-IO2-EC-DO16NT : DO 16 channel
        #region Constructor
        public BHY_IO2_EC_DO16NT()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.DO;

            //  Product Info
            m_VendorId = 0x00009555;
            m_VendorName = "MOVENSYS";
            m_ProductCode = 0x10000005;
            m_ProductName = "BHY-IO2-EC-DO16NT";
            m_Description = "Digital Output 16";

            //  Station Info
            this.m_OutChannelCount = 16;
        }
        #endregion

        #region Override
        public override EcSlave Copy()
        {
            BHY_IO2_EC_DO16NT slave = new BHY_IO2_EC_DO16NT();

            slave.m_SlaveNo = this.m_SlaveNo;
            slave.m_AliasNo = this.m_AliasNo;
            slave.m_SlaveType = this.m_SlaveType;

            slave.m_VendorId = this.m_VendorId;
            slave.m_VendorName = this.m_VendorName;
            slave.m_ProductCode = this.m_ProductCode;
            slave.m_ProductName = this.m_ProductName;
            slave.m_Description = this.m_Description;

            slave.m_OutChannelCount = this.m_OutChannelCount;

            return slave;
        }
        #endregion
    }
}

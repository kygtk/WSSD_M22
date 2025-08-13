///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.02.14
// Author       : byeongmin
// Description  : Fastech IO Slave 
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
    public class Ezi_IO_EC_AD08_T : EcSlave_AI
    {
        //  Ezi-IO-EC-AD08-T : AI 8 channel
        #region Constructor
        public Ezi_IO_EC_AD08_T()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.AI;

            //  Product Info
            m_VendorId = 0;
            m_VendorName = "";
            m_ProductCode = 0;
            m_ProductName = "Ezi-IO-EC-AD08-T";
            m_Description = "Analog Input 8";

            //  Station Info
            this.m_InChannelCount = 8;
        }
        #endregion

        #region Override
        public override EcSlave Copy()
        {
            Ezi_IO_EC_AD08_T slave = new Ezi_IO_EC_AD08_T();

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

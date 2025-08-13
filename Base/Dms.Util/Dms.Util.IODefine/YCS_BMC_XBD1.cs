using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class YCS_BMC_XBD1 : EcSlave_BLDC
    {
        //  YCS-BMC-XBD1 : YDIIT BLDC Motor Control
        #region Constructor
        public YCS_BMC_XBD1()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.BLDC;

            //  Product Info
            m_VendorId = 0x00000080;
            m_VendorName = "YDIIT";
            m_ProductCode = 0x00000003;
            m_ProductName = "YCS-BMC-XBD1";
            m_Description = "YDIIT BLDC Motor Control";

            //  Station Info
            m_BLDCCount = 1;
        }
        #endregion

        #region Overrides
        public override EcSlave Copy()
        {
            YCS_BMC_XBD1 slave = new YCS_BMC_XBD1();

            slave.m_SlaveNo = this.m_SlaveNo;
            slave.m_AliasNo = this.m_AliasNo;
            slave.m_SlaveType = this.m_SlaveType;

            slave.m_VendorId = this.m_VendorId;
            slave.m_VendorName = this.m_VendorName;
            slave.m_ProductCode = this.m_ProductCode;
            slave.m_ProductName = this.m_ProductName;
            slave.m_Description = this.m_Description;

            return slave;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class ESD_EC_120_C : EcSlave_BLDC
    {
        //  ESD-EC-120-C : Fastech BLDC
        #region Constructor
        public ESD_EC_120_C()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.BLDC;

            //  Product Info
            m_VendorId = 0x0FA00000;
            m_VendorName = "Fastech";
            m_ProductCode = 0x00000FA4;
            m_ProductName = "ESD-EC-120-C";
            m_Description = "Fastech BLDC";

            //  Station Info
            m_BLDCCount = 1;
        }
        #endregion

        #region Overrides
        public override EcSlave Copy()
        {
            ESD_EC_120_C slave = new ESD_EC_120_C();

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

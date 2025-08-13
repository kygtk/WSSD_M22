using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class MADLN05BE : EcSlave_Servo
    {
        //  MADLN05BE : Panasonic Servo
        #region Constructor
        public MADLN05BE()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_AxisIndex = 0;
            m_SlaveType = Common.SlaveType.Servo;

            //  Product Info
            m_VendorId = 0x0000066F;
            m_VendorName = "Panasonic Industry Co., Ltd.";
            m_ProductCode = 0x60380004;
            m_ProductName = "MADLN05BE";
            m_Description = "Panasonic Servo Motor";

            //  Station Info
            m_ServoCount = 1;
        }
        #endregion

        #region Overrides
        public override EcSlave Copy()
        {
            MADLN05BE slave = new MADLN05BE();

            slave.m_SlaveNo = this.m_SlaveNo;
            slave.m_AliasNo = this.m_AliasNo;
            slave.m_AxisIndex = this.m_AxisIndex;
            slave.m_SlaveType = this.m_SlaveType;

            slave.m_VendorId = this.m_VendorId;
            slave.m_VendorName = this.m_VendorName;
            slave.m_ProductCode = this.m_ProductCode;
            slave.m_ProductName = this.m_ProductName;
            slave.m_Description = this.m_Description;

            slave.m_ServoCount = this.m_ServoCount;

            return slave;
        }
        #endregion
    }
}

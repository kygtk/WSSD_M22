using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class S100 : EcSlave_Inverter
    {
        //  S100 : LS Inverter
        #region Constructor
        public S100()
        {
            //  Parts Info
            m_SlaveNo = -1;
            m_AliasNo = 0;
            m_SlaveType = Common.SlaveType.Inverter;

            //  Product Info
            m_VendorId = 0x000005E1;
            m_VendorName = "LSIS (LS ELECTRIC)";
            m_ProductCode = 0x64150034;
            m_ProductName = "S100";
            m_Description = "LS Inverter";

            //  Station Info
            m_InverterCount = 1;
        }
        #endregion

        #region Overrides
        public override EcSlave Copy()
        {
            S100 slave = new S100();

            slave.m_SlaveNo = m_SlaveNo;
            slave.m_AliasNo = m_AliasNo;
            slave.m_SlaveType = m_SlaveType;

            slave.m_VendorId = m_VendorId;
            slave.m_VendorName = m_VendorName;
            slave.m_ProductCode = m_ProductCode;
            slave.m_ProductName = m_ProductName;
            slave.m_Description = m_Description;

            slave.m_InverterCount = m_InverterCount;

            return slave;
        }
        #endregion
    }
}

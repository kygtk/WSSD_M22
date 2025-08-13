///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.04
// Author       : jemoon
// Description  : X20BC0087(Bus coupler for B&R ModbusTCP)
//-------------------------------------------------------------------------
// Revison History
// * jemoon : 2009.08.30 - Add Product Info 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class X20BC0087 : IoPart
    {
        #region Constructor
        public X20BC0087()
        {
            //Parts Info
            m_PartCode = "C1111-X058";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20BC0087(Bus Controller, ModbusTcp)";
            m_PartDescription = "Modbus TCP Bus Coupler for B&R Bus Terminal";
            m_PartRemark = "B&R Modbus TCP";
            m_PartPrice = 140000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20BC0087";
            m_ProductId = 8828;
        } 
        #endregion
    }
}

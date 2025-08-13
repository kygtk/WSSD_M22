///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.04
// Author       : jemoon
// Description  : X20PS9400(Power Suppy for B&R Bus Coupler)
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
    public class X20PS9400 : IoPart
    {
        #region Constructor
        public X20PS9400()
        {
            //Parts Info
            m_PartCode = "C1111-X060";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20PS9400(supply module)";
            m_PartDescription = "Power supply for X20BC0087";
            m_PartRemark = "B&R Modbus TCP";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20PS9400";
            m_ProductId = 8076;
        } 
        #endregion
    }
}

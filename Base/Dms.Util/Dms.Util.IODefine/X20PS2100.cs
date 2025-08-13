///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.04
// Author       : jemoon
// Description  : X20PS2100(Sub Power Suppy for B&R Io Bus)
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
    public class X20PS2100 : IoPart
    {
        #region Constructor
        public X20PS2100()
        {
            //Parts Info
            m_PartCode = "C1111-X060";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20PS2100(supply module for power bus)";
            m_PartDescription = "Power supply for bus terminal";
            m_PartRemark = "B&R Modbus TCP";
            m_PartPrice = 20000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20PS2100";
            m_ProductId = 7103;
        } 
        #endregion
    }
}

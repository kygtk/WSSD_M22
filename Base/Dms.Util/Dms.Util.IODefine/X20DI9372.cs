///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.27
// Author       : wschoi
// Description  : X20DI9372(Digital input 12pts)
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
    public class X20DI9372 : IoTerminal 
    {
        // X20DI9372 : DI 12 channel / negative
        #region Constructor
        public X20DI9372()
        {
            this.IoType = Dms.Common.IoType.DI;
            this.ChannelCount = 12;
            
            //Parts Info
            m_PartCode = "C1111-X061";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20DI9372(Digital input 12pts)";
            m_PartDescription = "Digital input 12pts";
            m_PartPrice = 75000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20DI9372";
            m_ProductId = 7464;
        } 
        #endregion
    }
}

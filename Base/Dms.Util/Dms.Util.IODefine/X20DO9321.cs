///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.27
// Author       : wschoi
// Description  : X20DO9321(Digital out 12pts)
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
    public class X20DO9321 : IoTerminal 
    {
        // X20DO9321 : DO 12 channel / negative
        #region Constructor
        public X20DO9321()
        {
            this.IoType = Dms.Common.IoType.DO;
            this.ChannelCount = 12;

            //Parts Info
            m_PartCode = "C1111-X062";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20DO9321(Digital out 12pts)";
            m_PartDescription = "Digital out 12pts";
            m_PartPrice = 85000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20DO9321";
            m_ProductId = 7067;
        } 
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.27
// Author       : wschoi
// Description  : X20AI4622(Analog input 4pts)
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
    public class X20AI4622 : IoTerminal 
    {
        // X20AI4622 : AI 4 channel / negative
        #region Constructor
        public X20AI4622()
        {
            this.IoType = Dms.Common.IoType.AI;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "C1111-X062";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20AI4622(Analog in 4pts)";
            m_PartDescription = "Analog in 4pts";
            m_PartPrice = 170000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20AI4622";
            m_ProductId = 7082;
        } 
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.27
// Author       : wschoi
// Description  : X20AO2622(Analog output 2pts)
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
    public class X20AO2622 : IoTerminal 
    {
        // X20AO2622 : AO 2 channel / negative
        #region Constructor
        public X20AO2622()
        {
            this.IoType = Dms.Common.IoType.AO;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X071";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20AO2622(Analog out 2pts)";
            m_PartDescription = "Analog out 2pts";
            m_PartPrice = 140000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20AO2622";
            m_ProductId = 7074;
        } 
        #endregion
    }
}

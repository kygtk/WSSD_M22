///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.08.27
// Author       : wschoi
// Description  : X20AI2622(Analog input 2pts)
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
    public class X20AI2622 : IoTerminal
    {
        // X20AI2622 : AI 2 channel / negative
        #region Constructor
        public X20AI2622()
        {
            this.IoType = Dms.Common.IoType.AI;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X070";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "X20AI2622(Analog in 2pts)";
            m_PartDescription = "Analog in 2pts";
            m_PartPrice = 140000;

            //Product Info
            m_ProductMaker = Maker.BR;
            m_ProductName = "X20AI2622";
            m_ProductId = 7070;
        } 
        #endregion
    }
}

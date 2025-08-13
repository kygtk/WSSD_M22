///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL1184
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.10 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class KL1184 : IoTerminal
    {   
        // KL1184 : DI 4 channel / negative

        public KL1184()
        {
            this.IoType = IoType.DI;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "C1111-X017";
            m_PartName = "DEVICENET SYSTEM(27)";
            m_PartSpec = "KL1184(4-CH 24V DI)";
            m_PartDescription = "4-ch digital input terminals 24 V DC, switching to negative potential";
            m_PartRemark = "0:18~30v, 1:0~7v";
            m_PartPrice = 38000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL1184";
        }
    }
}
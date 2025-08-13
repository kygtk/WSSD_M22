///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL1488
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
    public class KL1488 : IoTerminal
    {
        // KL1488 : DI 8 channel / negative

        public KL1488()
        {
            this.IoType = IoType.DI;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "C1111-X024";
            m_PartName = "DEVICENET SYSTEM(34)";
            m_PartSpec = "KL1488(8-CH 24V DIGITAL INPUT)";
            m_PartDescription = "8-ch digital input terminals 24 V DC, switching to negative potential";
            m_PartRemark = "0:18~30v, 1:0~7v";
            m_PartPrice = 60000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL1488";
        }
    }
}
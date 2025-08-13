///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL2488
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.10 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class KL2488 : IoTerminal
    {
        // KL2488 : DO 8 channel / negative

        public KL2488()
        {
            this.IoType = IoType.DO;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "C1111-X025";
            m_PartName = "DEVICENET SYSTEM(35)";
            m_PartSpec = "KL2488(8-CH 24V DIGITAL OUTPUT)";
            m_PartDescription = "8-ch digital output terminal 24 V DC, switching to negative potential";
            m_PartPrice = 65000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL2488";
        }
    }
}
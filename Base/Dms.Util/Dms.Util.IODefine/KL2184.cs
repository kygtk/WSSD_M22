///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL2184
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
    public class KL2184 : IoTerminal
    {
        // KL2184 : DO 4 channel / negative

        public KL2184()
        {
            this.IoType = IoType.DO;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "C1111-X018";
            m_PartName = "DEVICENET SYSTEM(28)";
            m_PartSpec = "KL2184(4-CH 24V DO)";
            m_PartDescription = "4-ch digital output terminal 24 V DC, switching to negative potential";
            m_PartPrice = 51000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL2184";
        }
    }
}
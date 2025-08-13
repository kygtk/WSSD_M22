///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL4001
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
    public class KL4001 : IoTerminal
    {
        // KL4001 : AO 1 channel / 0 ~ 10V

        public KL4001()
        {
            this.IoType = IoType.AO;
            this.ChannelCount = 1;

            //Parts Info
            m_PartCode = "C1111-X021";
            m_PartName = "DEVICENET SYSTEM(31)";
            m_PartSpec = "KL4001(1-CH ANALOG OUTPUT)";
            m_PartDescription = "1-channel analog output terminals";
            m_PartRemark = "0 ~ 10 v";
            m_PartPrice = 210000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL4001";
        }
    }
}
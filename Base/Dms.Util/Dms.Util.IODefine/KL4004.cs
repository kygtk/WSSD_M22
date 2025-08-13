///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL4004
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
    public class KL4004 : IoTerminal
    {
        // KL4004 : AO 4 channel / 0 ~ 10V

        public KL4004()
        {
            this.IoType = IoType.AO;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "C1111-X036";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "KL4004(4-CH ANALOG OUTPUT MODULE)";
            m_PartDescription = "4-channel analog output terminals";
            m_PartRemark = "0 ~ 10 v";
            m_PartPrice = 320000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL4004";
        }
    }
}
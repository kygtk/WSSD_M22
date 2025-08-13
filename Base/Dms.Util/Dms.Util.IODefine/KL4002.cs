///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL4002
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
    public class KL4002 : IoTerminal
    {
        // KL4002 : AO 2 channel / 0 ~ 10V

        public KL4002()
        {
            this.IoType = IoType.AO;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X020";
            m_PartName = "DEVICENET SYSTEM(30)";
            m_PartSpec = "KL4002(2-CH ANALOG OUTPUT)";
            m_PartDescription = "2-channel analog output terminals";
            m_PartRemark = "0 ~ 10 v";
            m_PartPrice = 210000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL4002";
        }
    }
}
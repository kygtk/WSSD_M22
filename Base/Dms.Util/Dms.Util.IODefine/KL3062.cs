///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL3062
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
    public class KL3062 : IoTerminal
    {
        // KL3062 : AI 2 channel / 0 ~ 10V

        public KL3062()
        {
            this.IoType = IoType.AI;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X022";
            m_PartName = "DEVICENET SYSTEM(32)";
            m_PartSpec = "KL3062(ANALOG INPUT)";
            m_PartDescription = "2-channel analog input terminals";
            m_PartRemark = "0~10 v";
            m_PartPrice = 210000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL3062";
        }
    }
}
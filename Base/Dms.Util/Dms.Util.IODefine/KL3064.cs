///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL3064
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
    public class KL3064 : IoTerminal
    {
        // KL3064 : AI 4 channel / 0 ~ 10V

        public KL3064()
        {
            this.IoType = IoType.AI;
            this.ChannelCount = 4;

            //Parts Info
            m_PartCode = "C1111-X019";
            m_PartName = "DEVICENET SYSTEM(29)";
            m_PartSpec = "KL3064(4-CH ANALOG INPUT)";
            m_PartDescription = "4-channel analog input terminals";
            m_PartRemark = "0~10 v";
            m_PartPrice = 240000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL3064";
        }
    }
}
///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL3102
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
    public class KL3102 : IoTerminal
    {
        // KL3102 : AI 2 channel / -10/0 ~ +10V

        public KL3102()
        {
            this.IoType = IoType.AI;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X026";
            m_PartName = "DEVICENET SYSTEM(36)";
            m_PartSpec = "KL3102(2-CH ANALOG INPUT)";
            m_PartDescription = "4-channel analog input terminals";
            m_PartRemark = "-10 ~ +10 v";
            m_PartPrice = 270000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL3102";
        }
    }
}
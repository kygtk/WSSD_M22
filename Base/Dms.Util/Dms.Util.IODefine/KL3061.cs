///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : KL3061
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
    public class KL3061 : IoTerminal
    {
        // KL3061 : AI 1 channel / 0 ~ 10V

        public KL3061()
        {
            this.IoType = IoType.AI;
            this.ChannelCount = 1;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL3061";
        }
    }
}
///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.06
// Author       : eun
// Description  : KL4022
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class KL4022 : IoTerminal
    {
        // KL4022 : AO 2 channel / 4 ~ 20mA

        public KL4022()
        {
            this.IoType = IoType.AO;
            this.ChannelCount = 2;

            //Parts Info
            m_PartCode = "C1111-X033";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "KL4022(2-ch Analog Output, 4~20mA)";
            m_PartDescription = "2-channel analog output terminals 0/4…20 mA";
            m_PartRemark = "0/4…20 mA";
            m_PartPrice = 240000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL4022";
        }
    }
}

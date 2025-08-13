///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.04
// Author       : jemoon
// Description  : BK1120(Bus coupler)
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class BK1120 : IoPart
    {
        #region Constructor
        public BK1120()
        {
            //Parts Info
            m_PartCode = "C1111-X028";
            m_PartName = "DEVICENET SYSTEM(37)";
            m_PartSpec = "BK1120(BUS COUPLER)";
            m_PartDescription = "EtherCAT Bus Coupler for Beckhoff Bus Terminal";
            m_PartRemark = "Beckhoff EtherCAT";
            m_PartPrice = 380000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "BK1120";
        } 
        #endregion
    }
}

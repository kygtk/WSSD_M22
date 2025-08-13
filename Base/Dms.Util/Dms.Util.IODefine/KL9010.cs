///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.03.04
// Author       : jemoon
// Description  : KL9010(End module)
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
    public class KL9010 : IoPart
    {
        #region Constructor
        public KL9010()
        {
            m_PartCode = "C1111-X016";
            m_PartName = "DEVICENET SYSTEM(26)";
            m_PartSpec = "KL9010(BUS END MODULE)";
            m_PartDescription = "End Bus Terminal";
            m_PartPrice = 13000;

            //Product Info
            m_ProductMaker = Maker.BeckHoff;
            m_ProductName = "KL9010";
        } 
        #endregion
    }
}

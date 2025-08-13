///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.23
// Author       : jemoon
// Description  : Melsec Device W inputs 
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class MelDevWordInputs : MelDevs
    {
        // MelDevBitInputs : AI n channel
        #region Constructor
        public MelDevWordInputs()
        {
            this.DeviceType = devTYPE.devW;
            this.IoType = Dms.Common.IoType.AI;
            this.ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog in n pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelDevWordInputs";
            m_ProductId = 0;
        }
        #endregion
    }
}

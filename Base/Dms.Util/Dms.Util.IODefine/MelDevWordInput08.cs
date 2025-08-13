///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.23
// Author       : jemoon
// Description  : Melsec Device W output 
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
    public class MelDevWordInput08 : MelsecTerminal
    {
        // MelsecInputDevW : AI 8 channel
        #region Constructor
        public MelDevWordInput08()
        {
			m_DevType = devTYPE.devW;
			m_IoType = Dms.Common.IoType.AI;
			m_ChannelCount = 8;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Analog in 8 pts";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.MitsubishiMelsecNet;
            m_ProductName = "MelsecWIn8";
            m_ProductId = 0;
        } 
        #endregion
    }
}

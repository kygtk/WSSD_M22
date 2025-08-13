///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.26
// Author       : jemoon
// Description  : CC Link(Dummy Terminal)
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
    public class CCDummy : IoTerminal
    {
        #region Constructor
		public CCDummy()
        {
        } 
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.11
// Author       : jemoon
// Description  : Exhaust UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class Exhaust : DmsUserControl
    {
        public Exhaust()
        {
            InitializeComponent();
            
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
    }
}

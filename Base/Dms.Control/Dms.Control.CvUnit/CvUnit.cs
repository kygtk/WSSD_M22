///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : CvUnit UserControl
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
using Dms.Control;
using Dms.Common;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(CvUnit), "CvUnitAni.bmp")]
    public partial class CvUnit : DmsUserControl
    {
        [Category("DMS : Device")]
        public CvMotor CvMotorR
        {
            get { return cvMotorR; }
            set { cvMotorR = value; }
        }

        [Category("DMS : Device")]
        public CvMotor CvMotorL
        {
            get { return cvMotorL; }
            set { cvMotorL = value; }
        }

        public CvUnit()
        {
            InitializeComponent();
        }
    }
}

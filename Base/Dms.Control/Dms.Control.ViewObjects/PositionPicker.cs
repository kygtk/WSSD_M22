using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class PositionPicker : UserControl
    {
        [Category("DMS : Setting")]
        public Point Position
        {
            get { return this.Location; }
        }

        public PositionPicker()
        {
            InitializeComponent();

            this.Visible = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Server;

namespace Dms.HMI
{
    public partial class JobsTabInterlockCtrl : UserControl
    {
        public JobsTabInterlockCtrl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            
        }

        public void Initialize()
        {
            interlockListView.Initialize();
        }
    }
}

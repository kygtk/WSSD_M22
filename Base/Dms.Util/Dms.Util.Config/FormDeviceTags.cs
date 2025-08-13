using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Util
{
    public partial class FormDeviceTags : Form
    {
        private DeviceTags m_Tags = null;

        public FormDeviceTags()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void Initialize(DeviceTags tags)
        {
            m_Tags = tags;
            this.viewTagContainer1.Initialize(m_Tags);
        }

        private void buttonClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
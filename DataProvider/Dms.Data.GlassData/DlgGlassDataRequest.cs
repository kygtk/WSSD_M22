using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Data
{
    public partial class DlgGlassDataRequest : Form
    {
        private string option = "C";

        public string Option
        {
            get { return option; }
        }

        public DlgGlassDataRequest()
        {
            InitializeComponent();
        }

        private void DlgGlassDataRequest_Load(object sender, EventArgs e)
        {
            chkOption1.Checked = true;
            chkOption2.Checked = false;
            chkOption3.Checked = false;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkOption1_Click(object sender, EventArgs e)
        {
            option = "C";	//by glass code
            chkOption1.Checked = true;
            chkOption2.Checked = false;
            chkOption3.Checked = false;
        }

        private void chkOption2_Click(object sender, EventArgs e)
        {
            option = "I";	//by glass id
            chkOption1.Checked = false;
            chkOption2.Checked = true;
            chkOption3.Checked = false;
        }

        private void chkOption3_Click(object sender, EventArgs e)
        {
            option = "A";	//by code and glass id
            chkOption1.Checked = false;
            chkOption2.Checked = false;
            chkOption3.Checked = true;
        }
    }
}
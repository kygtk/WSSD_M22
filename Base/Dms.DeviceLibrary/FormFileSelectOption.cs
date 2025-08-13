using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.DeviceLibrary
{
    public partial class FormFileSelectOption : Form
    {
        public FormFileSelectOption(string filename)
        {
            InitializeComponent();
            label1.Text = string.Format("Missing File: {0}\n\nWill you select the altanative file?", filename);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSelectWithoutQuestion_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
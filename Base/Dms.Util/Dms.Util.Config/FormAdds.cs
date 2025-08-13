using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Util
{
    public partial class FormAdds : Form
    {
        private int m_Count;
        public int Count
        {
            get { return m_Count; }
        }

        public FormAdds()
        {
            InitializeComponent();
            this.comboBoxCount.SelectedIndex = 0;

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            m_Count = Convert.ToInt32(this.comboBoxCount.Text);
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
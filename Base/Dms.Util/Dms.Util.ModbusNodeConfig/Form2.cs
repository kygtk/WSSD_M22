using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Util
{
    public partial class Form2 : Form
    {
        private string m_EqpName = "";

        public string EqpName
        {
            get { return m_EqpName; }
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_EqpName = txtEqpName.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_EqpName = "";
            this.Close();
        }

        private void txtEqpName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                m_EqpName = txtEqpName.Text;
                this.Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                m_EqpName = "";
                this.Close();
            }
        }
    }
}
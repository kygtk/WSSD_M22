using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class LoginA : UserControl
    {
        private LoginDialog m_Logindlg = new LoginDialog();
        public LoginA()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!m_Logindlg.IsHandleCreated)
            {
                m_Logindlg = new LoginDialog();
                m_Logindlg.Show();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms;
using Dms.Client;
using Dms.Common;
using Dms.Server;
using Dms.Data;

namespace Dms.HMI
{
    public partial class DlgScreenLock : Form
    {
        public DlgScreenLock()
        {
            InitializeComponent();

            tbPassword.Text = "";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Length > 0)
            {
                GlobalVar.ScreenLockPassword = tbPassword.Text;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalVar.ScreenLockPassword.Length > 0)
                GlobalVar.ScreenLockPassword.Remove(0);

            this.Close();
        }

        private void DlgScreenLock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
        }
    }
}
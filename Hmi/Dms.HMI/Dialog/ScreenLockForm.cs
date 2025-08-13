using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;
using Dms.Server;

namespace Dms.HMI
{
    public partial class ScreenLockForm : Form
    {
        #region Fields
        MainForm mainForm = new MainForm();//zhangliang 13.08.17
        #endregion 

        #region Constructor
        public ScreenLockForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if(AppConfig.Instance.Resolution == Resolution.R1280_1024)
            {
                this.Size = new Size(1280, 1024);
                panelResize1.Size = new Size(288, 121);
                panelResize2.Size = new Size(288, 121);
            }
        }
        #endregion

        #region Methods
        private void ScreenLockForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            if(e.Modifiers == Keys.Control)
            {
                e.Handled = true;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            bool passwordOk = tbPassword.Text.CompareTo(GlobalVar.ScreenLockPassword) == 0;
            passwordOk &= tbConfirm.Text.CompareTo(GlobalVar.ScreenLockPassword) == 0;

            if(passwordOk)
                this.Close();
            else
            {
                MessageBox.Show("Please Input Correct Password to Unlock the Screen", "Incorrect Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();//zhangliang 13.08.17
            mainForm.InitializeClientManager();//zhangliang 13.08.17
            mainForm.buttonExit_Click(sender, e);//zhangliang 13.08.17
            Application.Exit();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;

namespace Dms.Control
{
    public partial class PasswordChangeDialog : Form
    {
        #region Fields
        private UserAccountProvider m_AccountProvider; 
        #endregion

        #region Properties
        public string SelectedUserID
        {
            set { txtUserID.Text = value; }
        }
        #endregion

        #region Constructor
        public PasswordChangeDialog(UserAccountProvider provider)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_AccountProvider = provider;
        } 
        #endregion

        #region Methods
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_AccountProvider.ChangePassword(txtUserID.Text, txtCurPassword.Text, txtPassword1.Text, txtPassword2.Text))
            {
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Can't change password. Please check again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        } 
        #endregion
    }
}
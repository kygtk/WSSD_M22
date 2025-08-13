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
    public partial class AccountCreateDialog : Form
    {
        #region Fields
        private UserAccountProvider m_AccountProvider;
        #endregion

        #region Constructor
        public AccountCreateDialog(UserAccountProvider provider)
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
            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                MessageBox.Show("User ID can't be blank. Please enter the ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (txtPassword1.Text == txtPassword2.Text)
            {
                TagUserAccount account = new TagUserAccount();
                account.Password = txtPassword1.Text;
                account.UserID = txtUserID.Text;
                account.UserLevel = (UserLevels)cbLevel.SelectedItem;
                if (m_AccountProvider.CreateUser(account))
                {
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Can't add user. Please check exist of same user id", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Wrong Password!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void AccountCreateDialog_Load(object sender, EventArgs e)
        {
            this.cbLevel.DataSource = Enum.GetValues(typeof(UserLevels));
        }
        #endregion
    }
}
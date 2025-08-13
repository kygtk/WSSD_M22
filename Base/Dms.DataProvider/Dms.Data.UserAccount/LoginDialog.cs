using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Common;

namespace Dms.Control
{
    public partial class LoginDialog : Form
    {
        #region Fields
        private UserAccountProvider m_AccountProvider;
        private TagUserAccount m_ParentUserAccount;
        private TextBox m_txtParentUserID;
        private static bool m_IsFirst = false;
        #endregion

        #region Properties
        public UserAccountProvider AccountProvider
        {
            get { return m_AccountProvider; }
            set { m_AccountProvider = value; }
        }
        #endregion

        #region Constructor
        public LoginDialog(TagUserAccount parentAccount)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_ParentUserAccount = parentAccount;
        }
        #endregion

        #region Methods
        public void Initialize(TextBox box)
        {
            m_txtParentUserID = box;
            InitGrid();
        }

        public void InitGrid()
        {
            dataGridUserAccount.AutoGenerateColumns = false;
            this.dataGridUserAccount.DataSource = m_AccountProvider.Adapter.Table;

            DataGridViewTextBoxColumn colUserLevel = new DataGridViewTextBoxColumn();
            colUserLevel.DataPropertyName = "UserLevel";
            colUserLevel.HeaderText = "Level";
            colUserLevel.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridUserAccount.Columns.Add(colUserLevel);

            DataGridViewTextBoxColumn colUserID = new DataGridViewTextBoxColumn();
            colUserID.DataPropertyName = "UserID";
            colUserID.HeaderText = "User ID";
            colUserID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridUserAccount.Columns.Add(colUserID);
        }

        private void EnableButtons(UserLevels level)
        {
            bool on = false;
            if (level == UserLevels.Administrator) on = true;
            else on = false;

            btnCreate.Enabled = on;
            btnPasswordChange.Enabled = on;
            btnRemove.Enabled = on;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            AccountCreateDialog dlgNewAccount = new AccountCreateDialog(m_AccountProvider);
            dlgNewAccount.ShowDialog();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (m_ParentUserAccount.UserID != lblUserID.Text &&
                lblUserID.Text.Length > 0)
            {
                if (MessageBox.Show("Do you really want to remove the account?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                if (m_AccountProvider.RemoveUser(lblUserID.Text))
                    lblUserID.Text = "";
            }
            else
            {
                MessageBox.Show("Can't remove current login ID", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnPasswordChange_Click(object sender, EventArgs e)
        {
            if (lblUserID.Text.Length > 0)
            {
                PasswordChangeDialog dlgChangePassword = new PasswordChangeDialog(m_AccountProvider);
                dlgChangePassword.SelectedUserID = lblUserID.Text;
                dlgChangePassword.ShowDialog();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (lblUserID.Text.Length > 0)
            {
                TagUserAccount account = new TagUserAccount();
                account.UserID = lblUserID.Text;
                account.Password = txtPassword.Text;
                if (m_AccountProvider.Login(account))
                {
                    m_ParentUserAccount.Clone(account);
                    m_txtParentUserID.Text = account.UserID;
                    EnableButtons(m_ParentUserAccount.UserLevel);
                }
                else
                {
                    MessageBox.Show("The passwords you typed do not match.",
                        "Exclamation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }

        }

        private void dataGridUserAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int RowNo = e.RowIndex;
            if (RowNo < 0) return;

            lblUserID.Text = this.dataGridUserAccount.Rows[RowNo].Cells[1].Value.ToString();
            txtPassword.Clear();
            txtPassword.Focus(); ;
        }

        private void LoginDialog_Load(object sender, EventArgs e)
        {
            EnableButtons(m_ParentUserAccount.UserLevel);
            if (m_IsFirst) btnExit.Enabled = false;
            if (m_IsFirst == false) m_IsFirst = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (m_ParentUserAccount.UserID == null)//.Length == 0)
            {
                MessageBox.Show("Please login first.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to shutdown this program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion
    }
}
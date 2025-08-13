using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Data
{
    public partial class DlgRecipeConfirm : Form
    {
        #region Enum
        public enum RcpCmd
        {
            Creation,
            Deletion,
            Change
        }

        public enum Confirm
        {
            Secceed,
            Deny
        }

        public enum Direction
        {
            HOST,
            EQP,
        }
        #endregion

        #region Fields
        private string m_Cmd;
        //private string m_Result;
        private string m_Direction;
        private string m_Message;
        private string m_btnText;
        private int m_Count;
        #endregion

        #region Properites
        #endregion

        #region Constructors
        public DlgRecipeConfirm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        //public DlgRecipeConfirm(RcpCmd cmd, Confirm cfm, Direction dir, string rcpId)
        public DlgRecipeConfirm(params Object[] param)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            string[] cmd = (string[]) param;

            if (cmd[1] == "ADD")
            {
                m_Cmd = DlgRecipeConfirm.RcpCmd.Creation.ToString();
            }
            else if (cmd[1] == "CHANGE")
            {
                m_Cmd = DlgRecipeConfirm.RcpCmd.Change.ToString();
            }
            else if (cmd[1] == "DELETE")
            {
                m_Cmd = DlgRecipeConfirm.RcpCmd.Deletion.ToString();
            }

            if (cmd[3] == "EQP")
            {
                m_Direction = DlgRecipeConfirm.Direction.EQP.ToString();
            }
            else if (cmd[3] == "HOST")
            {
                m_Direction = DlgRecipeConfirm.Direction.HOST.ToString();
            }

            if (cmd[2] == "SUCCESS")
            {
                m_Message = string.Format("Recipe {0} was succeeded from {1}", m_Cmd, m_Direction);
                lblMessage.BackColor = Color.PaleGreen;
                lblMessage.ForeColor = Color.Blue;
                lblMessage.Text = m_Message;
            }
            else if(cmd[2] == "FAIL")
            {
                m_Message = string.Format("Recipe {0} was denied from {1} (ACK {2})", m_Cmd, m_Direction, cmd[4]);
                lblMessage.BackColor = Color.Yellow;
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = m_Message;
            }

            lblRecipeId.Text = (string)param[0];
        }
        #endregion

        #region Methods
        private void DlgRecipeConfirm_Load(object sender, EventArgs e)
        {
            m_Count = 5;
            m_btnText = string.Format("OK ( {0} sec )", m_Count);
            btnOK.Text = m_btnText;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmrRecipeConfirm_Tick(object sender, EventArgs e)
        {
            m_Count--;
            if (m_Count > 0)
            {
                m_btnText = string.Format("OK ( {0} sec )", m_Count);
                btnOK.Text = m_btnText;
            }
            else if (m_Count <= 0)
            {
                this.Close();
            }            
        }
        #endregion
    }
}
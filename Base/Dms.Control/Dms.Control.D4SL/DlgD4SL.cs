using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class DlgD4SL : Form
    {
        #region Tag Descriptor
        public static TagDescriptorD4SL tagDescriptor = new TagDescriptorD4SL();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();

        private ClientManager m_Client = ClientManager.Instance;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DlgD4SL()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public DlgD4SL(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Initialize(tag);
        }
        #endregion

        #region Methods
        public void Initialize(DeviceTag tag)
        {
            m_Tag = tag;
            m_TagOld.Clone(m_Tag);
        }

        private void UpdateState()
        {
            for (int idx = 0; idx < 8; idx++)
            {
                string sIdx = idx.ToString();
                Label lblState = gbIndividualOperation.Controls["lblState" + sIdx] as Label;
                Button btnLock = gbIndividualOperation.Controls["btnLock" + sIdx] as Button;
                Button btnUnlock = gbIndividualOperation.Controls["btnUnlock" + sIdx] as Button;

                string state = m_Tag[tagDescriptor.DOOR0_STATE - idx].Value;    //  TagDescriptor가 뒤집히므로 -idx 연산

                bool isNull = false;
                bool isOpened = false;

                lblState.Text = state;
                switch (state)
                {
                    case "OPENED":
                        lblState.BackColor = Color.HotPink;
                        lblState.ForeColor = Color.Red;
                        isOpened = true;
                        break;
                    case "CLOSED":
                        lblState.BackColor = Color.White;
                        lblState.ForeColor = SystemColors.ControlText;
                        break;
                    case "LOCKED":
                        lblState.BackColor = Color.IndianRed;
                        lblState.ForeColor = Color.White;
                        break;
                    default:
                        lblState.BackColor = Color.DarkGray;
                        lblState.ForeColor = Color.DimGray;
                        isNull = true;
                        break;
                }

                btnLock.Enabled = !isNull && !isOpened;
                btnUnlock.Enabled = !isNull && !isOpened;
            }
        }
        #endregion

        #region Event Handlers
        private void DlgD4SL_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            btnOk.Focus();
            UpdateState();
            timUpdateState.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            timUpdateState.Enabled = false;
            this.Close();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int index = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1));

            m_Client.SendCommand(Command.D4SLManual, m_Tag.DeviceName, D4SLAct.LockOne, index);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int index = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1));

            m_Client.SendCommand(Command.D4SLManual, m_Tag.DeviceName, D4SLAct.UnlockOne, index);
        }

        private void btnLockAll_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.D4SLManual, m_Tag.DeviceName, D4SLAct.LockAll);
        }

        private void btnUnlockAll_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.D4SLManual, m_Tag.DeviceName, D4SLAct.UnlockAll);
        }

        private void timUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }
        #endregion
    }
}

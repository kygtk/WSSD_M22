using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class DlgDoorLock : Form
    {
        #region Tag Descriptor
        public static TagDescriptorDoorLock tagDescriptor = new TagDescriptorDoorLock();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
        #endregion


        #region Properties
        #endregion

        #region Constructor
        public DlgDoorLock()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public DlgDoorLock(DeviceTag tag)
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

        #endregion

        #region Event Handlers
        private void DlgDoorLock_Load(object sender, EventArgs e)
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
            m_Client.SendCommand(Command.DoorLockManual, m_Tag.DeviceName, DoorLockAct.Lock);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.DoorLockManual, m_Tag.DeviceName, DoorLockAct.Unlock);
        }

        private void timUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void UpdateState()
        {
            //  BM : 실제 Lock과 Lock감지 동작이 일치하지 않으므로 (센서 오류 등) 주석처리 함
            //bool locked = m_Tag[tagDescriptor.LOCKED].Value == bool.TrueString
            //           || m_Tag[tagDescriptor.LOCKED].Value == "1";

            //this.btnLock.Enabled = !locked;
            //this.btnUnlock.Enabled = locked;
        }
        #endregion
    }
}

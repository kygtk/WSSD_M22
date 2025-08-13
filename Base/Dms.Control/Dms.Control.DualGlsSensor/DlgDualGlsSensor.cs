using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class DlgDualGlsSensor : Form
    {
        #region Tag Descriptor
        protected static TagDescriptorDualGlsSensor tagDescriptor = new TagDescriptorDualGlsSensor();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private ClientManager m_Client;
        private Color m_UseColor = Color.YellowGreen;
        private Color m_NoUseColor = Color.LightGray;
        #endregion

        #region Constructor
        public DlgDualGlsSensor()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        public void Initialize(DeviceTag tag)
        {
            m_Tag = tag;
            if (m_Client == null) m_Client = ClientManager.Instance;
        }

        private void UpdateState()
        {
            if (m_Tag[tagDescriptor.USE].Value == bool.TrueString || m_Tag[tagDescriptor.USE].Value == "1")
            {
                btnOop.Text = "USE";
                btnOop.BackColor = m_UseColor;
            }
            else
            {
                btnOop.Text = "NO USE";
                btnOop.BackColor = m_NoUseColor;
            }
            if (m_Tag[tagDescriptor.USEOP].Value == bool.TrueString || m_Tag[tagDescriptor.USEOP].Value == "1")
            {
                btnOp.Text = "USE";
                btnOp.BackColor = m_UseColor;
            }
            else
            {
                btnOp.Text = "NO USE";
                btnOp.BackColor = m_NoUseColor;
            }
        }

        private void DlgDualGlsSensor_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        private void btnOp_Click(object sender, EventArgs e)
        {
            bool value = (m_Tag[tagDescriptor.USEOP].Value == bool.TrueString || m_Tag[tagDescriptor.USEOP].Value == "1");
            m_Client.SendCommand(Command.DualGlsSensorManual, m_Tag.DeviceName, DualGlsSensorType.Op, !value);
        }

        private void btnOop_Click(object sender, EventArgs e)
        {
            bool value = (m_Tag[tagDescriptor.USE].Value == bool.TrueString || m_Tag[tagDescriptor.USE].Value == "1");
            m_Client.SendCommand(Command.DualGlsSensorManual, m_Tag.DeviceName, DualGlsSensorType.Oop, !value);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
            tmrUpdateState.Enabled = false;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }
        #endregion
    }
}
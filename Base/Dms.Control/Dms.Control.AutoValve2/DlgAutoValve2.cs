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
    public partial class DlgAutoValve2 : Form
    {
        #region Tag Descriptor
        public static TagDescriptorActuator tagDescriptor = new TagDescriptorActuator();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
        #endregion

        #region Constructor
        public DlgAutoValve2()
        {
            InitializeComponent();
        }

        public DlgAutoValve2(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_Tag = tag;
            m_TagOld.Clone(m_Tag);
        }
        #endregion

        private void btnOpen_CheckedChanged(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorManual, m_Tag.DeviceName, ActuatorAct.Pos);
        }

        private void btnClose_CheckedChanged(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorManual, m_Tag.DeviceName, ActuatorAct.Neg);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            tmrUpdateState.Enabled = false;
            this.Close();
            //this.Dispose();
        }

        private void DlgAutoValve2_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            btnOk.Focus();
            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if ((m_Tag[tagDescriptor.POS_SENSOR].Value == bool.TrueString || m_Tag[tagDescriptor.POS_SENSOR].Value == "1") &&
                (m_Tag[tagDescriptor.NEG_SENSOR].Value == bool.FalseString || m_Tag[tagDescriptor.NEG_SENSOR].Value == "0"))
            {
                this.btnOpen.Enabled = false;
                this.btnClose.Enabled = true;
            }

            else if ((m_Tag[tagDescriptor.POS_SENSOR].Value == bool.FalseString || m_Tag[tagDescriptor.POS_SENSOR].Value == "0") &&
                     (m_Tag[tagDescriptor.NEG_SENSOR].Value == bool.TrueString || m_Tag[tagDescriptor.NEG_SENSOR].Value == "1"))
            {
                this.btnOpen.Enabled = true;
                this.btnClose.Enabled = false;
            }
        }
        #region Methods

        #endregion
    }
}
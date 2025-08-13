using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class DlgRPS : Form
    {
        #region Tag Descriptor
        protected static TagDescriptorRPS tagDescriptor = new TagDescriptorRPS();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        //private ClientManager m_Client = ClientManager.Instance;
        #endregion

        public DlgRPS(DeviceTag tag)
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

        private void UpdateState()
        {
            chkReady.Checked = (m_Tag[tagDescriptor.READY].Value == bool.TrueString || m_Tag[tagDescriptor.READY].Value == "1");
            chkPlasmaOk.Checked = (m_Tag[tagDescriptor.PLASMAOK].Value == bool.TrueString || m_Tag[tagDescriptor.PLASMAOK].Value == "1");
            chkAcOk.Checked = (m_Tag[tagDescriptor.ACOK].Value == bool.TrueString || m_Tag[tagDescriptor.ACOK].Value == "1");
            
            btnOn.Enabled = (m_Tag[tagDescriptor.PLASMAON].Value == bool.FalseString || m_Tag[tagDescriptor.PLASMAON].Value == "0") && (chkReady.Checked);
            btnOff.Enabled = !btnOn.Enabled;
        }

        private void btnOn_Click(object sender, EventArgs e)
        {

        }

        private void btnOff_Click(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = false;
            this.Close();
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void DlgRPS_Load(object sender, EventArgs e)
        {
            this.Text = m_Tag.DeviceName;
            tmrUpdateState.Enabled = true;
            UpdateState();
        }
    }
}
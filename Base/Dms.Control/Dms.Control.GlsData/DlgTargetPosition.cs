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
    public partial class DlgTargetPosition : Form
    {
        private int m_CurrentPosition = 0;
        private DeviceTags m_GlsSensors = new DeviceTags();

        public int CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        public DlgTargetPosition(DeviceTags tag)
        {
            InitializeComponent();

            m_GlsSensors = tag;
        }

        private void DlgTargetPosition_Load(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_GlsSensors.Items)
            {
                cboPosition.Items.Add(tag.DeviceName);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cboPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = (string)(((ComboBox)sender).SelectedItem);
            m_CurrentPosition = m_GlsSensors[name].DeviceId;
        }
    }
}
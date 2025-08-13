///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.06.02
// Author       : L.Y.S
// Description  : Each Mfc Manual Operation UserControl
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Data;
using Dms.Device;

namespace Dms.Control
{
    public partial class MfcButtonOp : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMfc tagDescriptor = new TagDescriptorMfc();
        #endregion

        #region Fields
        private double m_GasFlowrate;
        #endregion

        public delegate void MfcClickEventHandler(DeviceTag tag, MFCAct act, double m_GasFlowrate);
        public event MfcClickEventHandler MfcClick;

        #region Constructor
        public MfcButtonOp(DeviceTags tags, DeviceTag tag)
        {
            InitializeComponent();

            m_Tags = tags;
            m_TagInfo = new DeviceTagInfo(tag);

            textBox1.Text = tag[tagDescriptor.SETVAL].Value;

        }
        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if ((TextBoxBase)sender == textBox1)
            {
                
            }
        }

        #region Methods
        private void MfcButtonOp_Load(object sender, EventArgs e)
        {
            bool ok = base.Initialize(m_Tags);
            if (ok)
            {
                lblName.Text = m_Tag.DeviceName;
                textBox1.Text = Convert.ToString(m_Tag[tagDescriptor.SETVAL].Value);

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                //UpdateState();
            }
        }
        #endregion

        private void btnON_Click(object sender, EventArgs e)
        {
            if (MfcClick != null)
            {
                MfcClick(this.m_Tag, MFCAct.ON, m_GasFlowrate);
            }
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            if (MfcClick != null)
            {
                MfcClick(this.m_Tag, MFCAct.OFF, m_GasFlowrate);
            }
        }

        private void btnSetPoint_Click(object sender, EventArgs e)
        {
            if (MfcClick != null)
            {
                m_GasFlowrate = Convert.ToDouble(textBox1.Text);
                MfcClick(this.m_Tag, MFCAct.SETPOINT, m_GasFlowrate);
            }
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            ClientManager client = ClientManager.Instance;

            IComponentContainer components = client.EventSubscriber.Server.ComponentContainer;
            Dms.Device.Mfc mfcUnit = components[this.m_Tag.DeviceName] as Dms.Device.Mfc;
            
            DlgCalibration dlg = new DlgCalibration(mfcUnit.Info);

            dlg.Show();
        }
    }
}

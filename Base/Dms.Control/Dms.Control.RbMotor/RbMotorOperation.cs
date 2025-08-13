///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.18
// Author       : eun
// Description  : RbMotor Manual Operation Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class RbMotorOperation : UserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        #endregion

        public delegate void RbMotorClickEventHandler(DeviceTag tag, RbMotorAct act);
        public event RbMotorClickEventHandler RbMotorClick;

        public RbMotorOperation(DeviceTag tag)
        {
            InitializeComponent();

            m_Tag = tag;
        }

        private void btnCw_CheckedChanged(object sender, EventArgs e)
        {
            if (RbMotorClick != null)
            {
                RbMotorClick(this.m_Tag, RbMotorAct.Cw);
            }
        }

        private void btnCcw_CheckedChanged(object sender, EventArgs e)
        {
            if (RbMotorClick != null)
            {
                RbMotorClick(this.m_Tag, RbMotorAct.Ccw);
            }
        }

        private void btnStop_CheckedChanged(object sender, EventArgs e)
        {
            if (RbMotorClick != null)
            {
                RbMotorClick(this.m_Tag, RbMotorAct.Stop);
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            chkAlarm.Checked = m_Tag[tagDescriptor.ALARM].Value == "1" || m_Tag[tagDescriptor.ALARM].Value == bool.TrueString;
            chkCpOff.Checked = m_Tag[tagDescriptor.CPON].Value != "1" && m_Tag[tagDescriptor.CPON].Value != bool.TrueString;

            if (m_Tag[tagDescriptor.STOP].Value == "1" || m_Tag[tagDescriptor.STOP].Value == bool.TrueString)
            {
                btnCw.Enabled = true;
                btnCcw.Enabled = true;
            }
            else if (m_Tag[tagDescriptor.CW].Value == "1" || m_Tag[tagDescriptor.CW].Value == bool.TrueString)
            {
                btnCcw.Enabled = false;
            }
            else if (m_Tag[tagDescriptor.CCW].Value == "1" || m_Tag[tagDescriptor.CCW].Value == bool.TrueString)
            {
                btnCw.Enabled = false;
            }
        }

        private void RbMotorOperation_Load(object sender, EventArgs e)
        {
            lblName.Text = m_Tag.DeviceName;
            tmrUpdateState.Enabled = true;
        }
    }
}

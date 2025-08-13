///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.08.24
// Author       : Hoon
// Description  : DBMotor Manual Operation Dialog
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
    public partial class DBMotorOperation : UserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        #endregion

        public delegate void DBMotorClickEventHandler(DeviceTag tag, DBMotorAct act);
        public event DBMotorClickEventHandler DBMotorClick;

        public DBMotorOperation(DeviceTag tag)
        {
            InitializeComponent();

            m_Tag = tag;
        }

        private void btnCw_CheckedChanged(object sender, EventArgs e)
        {
            if (DBMotorClick != null)
            {
                DBMotorClick(this.m_Tag, DBMotorAct.Cw);
            }
        }

        private void btnCcw_CheckedChanged(object sender, EventArgs e)
        {
            if (DBMotorClick != null)
            {
                DBMotorClick(this.m_Tag, DBMotorAct.Ccw);
            }
        }

        private void btnStop_CheckedChanged(object sender, EventArgs e)
        {
            if (DBMotorClick != null)
            {
                DBMotorClick(this.m_Tag, DBMotorAct.Stop);
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            chkAlarm.Checked = (m_Tag[tagDescriptor.ALARM].Value == "1" || m_Tag[tagDescriptor.ALARM].Value == bool.TrueString) ? true : false;
            chkCpOn.Checked = (m_Tag[tagDescriptor.CPON].Value == "1" || m_Tag[tagDescriptor.CPON].Value == bool.TrueString) ? true : false;

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

        private void DBMotorOperation_Load(object sender, EventArgs e)
        {
            lblName.Text = m_Tag.DeviceName;
            tmrUpdateState.Enabled = true;
        }
    }
}

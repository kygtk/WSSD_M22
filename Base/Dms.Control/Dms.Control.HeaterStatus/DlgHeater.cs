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
    public partial class DlgHeater : Form
    {
        #region Fields
        private DeviceTag m_Tag = null;
        private ClientManager m_Client = ClientManager.Instance;
        #endregion

        public DlgHeater(DeviceTag tag)
        {
            InitializeComponent();
            m_Tag = tag;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.Set, 1, tbTemp.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.On, m_Tag.DeviceName);
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.Off, m_Tag.DeviceName);
        }

        private void btnMcOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.McOn, m_Tag.DeviceName);
        }

        private void btnMcOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.McOff, m_Tag.DeviceName);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeaterManual, m_Tag.DeviceName, HeaterAct.Reset, m_Tag.DeviceName);
        }
    }
}
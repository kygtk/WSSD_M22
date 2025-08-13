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
    public partial class CylinderAllOperateForm : Form
    {
        private DeviceTags m_Tags = null;
        private ClientManager m_Client = null;
        private string m_Caption = "";
        private CylinderGroupBox.DirectionType m_Type;

        public string Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }

        public CylinderAllOperateForm(DeviceTags tags, CylinderGroupBox.DirectionType type)
        {
            InitializeComponent();
            m_Tags = tags;
            m_Type = type;
        }

        private void btnFW_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags)
            {
                m_Client.SendCommand(Command.ActuatorManual, tag.DeviceName, ActuatorAct.Pos);
            }
        }

        private void btnBW_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags)
            {
                m_Client.SendCommand(Command.ActuatorManual, tag.DeviceName, ActuatorAct.Neg);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CylinderAllOperateForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            this.Text = m_Caption;
            if (m_Type == CylinderGroupBox.DirectionType.UP_DOWN)
            {
                btnFW.Text = "UP";
                btnBW.Text = "DOWN";
            }
            else if (m_Type == CylinderGroupBox.DirectionType.LEFT_RIGHT)
            {
                btnFW.Text = "LEFT";
                btnBW.Text = "RIGHT";
            }
            else if (m_Type == CylinderGroupBox.DirectionType.FW_BW)
            {
                btnFW.Text = "FW";
                btnBW.Text = "BW";
            }
            else if (m_Type == CylinderGroupBox.DirectionType.OPEN_CLOSE)
            {
                btnFW.Text = "CLOSE";
                btnBW.Text = "OPEN";
            }
        }
    }
}
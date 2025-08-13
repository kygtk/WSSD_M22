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
    public partial class DlgActuatorTurnOperate : Form
    {
        #region Tag Descriptor
        public static TagDescriptorActuatorTurn tagDescriptor = new TagDescriptorActuatorTurn();
        #endregion

        private ClientManager m_Client;
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private string[] m_PositionList;

        public DlgActuatorTurnOperate()
        {
            InitializeComponent();
        }

        public void Initialize(ActuatorTurn control, bool autoMode)
        {
            m_Client = ClientManager.Instance;
            m_Tag = control.GetDeviceTag();
            m_TagOld.Clone(m_Tag);
            m_PositionList = control.PositionList;

            this.lblCurrentUnitName.Text = m_Tag.DeviceName;

            if (m_PositionList != null)
            {
                foreach (string pos in m_PositionList)
                {
                    if (pos != "") this.comboBoxPosList.Items.Add(pos);
                }
            }

            if (autoMode)
            {
                this.comboBoxPosList.Enabled = false;
                this.buttonAlarmReset.Enabled = false;
                this.buttonEStop.Enabled = false;
                this.buttonMove.Enabled = false;
            }

            UpdateState();

            this.tmrUpdateState.Enabled = true;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAlarmReset_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorTurnManual, m_Tag.DeviceName, ActuatorAct.AlarmReset);
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            int posId = this.comboBoxPosList.SelectedIndex;
            if (posId < 0)
            {
                MessageBox.Show("Select target position!");
            }
            else
            {
                m_Client.SendCommand(Command.ActuatorTurnManual, m_Tag.DeviceName, ActuatorAct.SetRefPosition, posId);
                m_Client.SendCommand(Command.ActuatorTurnManual, m_Tag.DeviceName, ActuatorAct.Pos);
            }
        }

        private void buttonEStop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ActuatorTurnManual, m_Tag.DeviceName, ActuatorAct.EStop);
        }

        private void comboBoxPosList_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            this.checkBoxAlarm.Checked = Convert.ToBoolean(m_Tag[tagDescriptor.ALARM].Value);

            double pos = Convert.ToDouble(m_Tag[tagDescriptor.CUR_POS].Value);
            int index = (int)pos;
            if (index >= 0 && index < m_PositionList.Length)
            {
                this.lblCurrentPosition.Text = m_PositionList[index];
            }
            else
            {
                this.lblCurrentPosition.Text = "UnKnown";
            }
        }
    }
}
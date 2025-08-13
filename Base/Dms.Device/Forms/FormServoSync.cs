using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Device
{
    public partial class FormServoSync : Form
    {
        private ServoUnit m_Unit;

        public FormServoSync()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        
        public void Initialize(ServoUnit unit)
        { 
            m_Unit = unit;

            foreach (ServoMotor item in m_Unit.Axis)
            {
                this.comboBoxMaster.Items.Add(item.Name);
                this.comboBoxSlave.Items.Add(item.Name);
            }

            if (m_Unit.Sync)
            {
                this.comboBoxMaster.SelectedItem = m_Unit.SyncInfo.Master.Name;
                this.comboBoxSlave.SelectedItem = m_Unit.SyncInfo.Slave.Name;
                this.checkBoxSync.Checked = m_Unit.Sync;
                UpdateComboList();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (this.checkBoxSync.Checked)
            {
                string master = (string)this.comboBoxMaster.SelectedItem;
                string slave = (string)this.comboBoxSlave.SelectedItem;

                m_Unit.SyncInfo.Master = m_Unit.Axis[master];
                m_Unit.SyncInfo.Slave = m_Unit.Axis[slave];
                m_Unit.SyncInfo.Sync = true;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxSync_CheckedChanged(object sender, EventArgs e)
        {
            bool set = ((CheckBox)sender).Checked;
            if (set)
            {
                if ((this.comboBoxMaster.SelectedItem == null) ||
                    (this.comboBoxSlave.SelectedItem == null))
                {
                    ((CheckBox)sender).Checked = false;
                }
            }
            else
            {
                this.comboBoxMaster.SelectedItem = null;
                this.comboBoxSlave.SelectedItem = null;

                m_Unit.SyncInfo.Sync = false;
                m_Unit.SyncInfo.Master = null;
                m_Unit.SyncInfo.Slave = null;
            }
        }

        private void comboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboList();
        }

        private void UpdateComboList()
        {
            //this.comboBoxMaster.Items.Clear();
            //this.comboBoxSlave.Items.Clear();
            //foreach (ServoMotor item in m_Unit.Axis)
            //{
            //    this.comboBoxMaster.Items.Add(item.Name);
            //    this.comboBoxSlave.Items.Add(item.Name);
            //}

            if (this.comboBoxMaster.SelectedItem != null)
            {
                this.comboBoxSlave.Items.Remove(this.comboBoxMaster.SelectedItem);
            }

            if (this.comboBoxSlave.SelectedItem != null)
            {
                this.comboBoxMaster.Items.Remove(this.comboBoxSlave.SelectedItem);
            }
        }
    }
}
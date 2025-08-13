///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Calibration Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using System.Xml.Serialization;
using Dms.Data;

namespace Dms.Control
{
    public partial class DlgCalibration : Form
    {
        #region Fields
        private ClientManager m_Client = ClientManager.Instance;
        private TagCalibrationInfo m_CalibrationInfo = new TagCalibrationInfo();
        private TagCalibrationInfo m_CalibrationInfoBackup = new TagCalibrationInfo();
        //private Gauge m_Gauge = null;
        private bool m_Changed = false;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public TagCalibrationInfo CalibrationInfo
        {
            get { return m_CalibrationInfo; }
            set { m_CalibrationInfo = value; }
        }
        #endregion

        #region Constructor
        public DlgCalibration(TagCalibrationInfo info)
        {
            InitializeComponent();

            Initialize(info);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);           
        } 
        #endregion

        #region Methods
        public void Initialize(TagCalibrationInfo info)
        {           
            m_CalibrationInfo.Clone(info);

            txtAdcMin.Text = m_CalibrationInfo.AdcMin.ToString();
            txtAdcMax.Text = m_CalibrationInfo.AdcMax.ToString();
            txtRealMin.Text = m_CalibrationInfo.RealMin.ToString();
            txtRealMax.Text = m_CalibrationInfo.RealMax.ToString();

            tmrUpdateState.Enabled = true;
        }

        private void DlgCalibration_Load(object sender, EventArgs e)
        {
            //m_Client = ClientManager.Instance;
            //m_CalibrationInfo = m_Gauge.CalibraionInfo;
            //m_CalibrationInfoBackup.Clone(m_CalibrationInfo);

            //this.Text += m_Gauge.DeviceTagInfo.DeviceName;
            //this.comboBoxScale.DataSource = Enum.GetValues(typeof(ScaleType));
            
            //tmrUpdateState.Enabled = true;
            
            //UpdateDataTagToControl();

            //bool permission = true;
            //permission &= !m_Client.GenInfos.AutoMode;
            //permission &= m_Client.CurrentUserAccount.UserLevel >= UserLevels.Technician;

            //this.groupBoxCalibraionParamters.Enabled = permission;
            //this.groupBoxButtons.Enabled = permission;
        }

        private void UpdateDataTagToControl()
        {
            txtAdcMin.Text = m_CalibrationInfo.AdcMin.ToString();
            txtAdcMax.Text = m_CalibrationInfo.AdcMax.ToString();
            txtRealMin.Text = m_CalibrationInfo.RealMin.ToString();
            txtRealMax.Text = m_CalibrationInfo.RealMax.ToString();
            //txtGain.Text = m_CalibrationInfo.FilterGain.ToString();
            //comboBoxScale.SelectedItem = m_CalibrationInfo.Scale;
        }

        private void UpdateDataControlToTag()
        {
            if (string.IsNullOrEmpty(txtAdcMax.Text) ||
                string.IsNullOrEmpty(txtAdcMin.Text) ||
                string.IsNullOrEmpty(txtRealMax.Text) ||
                string.IsNullOrEmpty(txtRealMin.Text))
            {
                MessageBox.Show("You have to key in value");
                return;
            }
            m_CalibrationInfo.AdcMin = Convert.ToInt16(txtAdcMin.Text);
            m_CalibrationInfo.AdcMax = Convert.ToInt16(txtAdcMax.Text);
            m_CalibrationInfo.RealMin = Convert.ToDouble(txtRealMin.Text);
            m_CalibrationInfo.RealMax = Convert.ToDouble(txtRealMax.Text);          
            
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
          
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            UpdateDataControlToTag();

            m_Client.SendCommand(Command.CalibrationSave, m_CalibrationInfo);

            m_Changed = true;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            
        }

        private void CancelChange()
        {
            //m_CalibrationInfo는 다시 원복
            if (m_Changed)
            {
                m_CalibrationInfo.Clone(m_CalibrationInfoBackup);
                m_Client.SendCommand(Command.CalibrationSave, m_CalibrationInfo);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //UpdateDataControlToTag();
            if (DialogResult.Yes == MessageBox.Show("Do you really want to Calibration?", "WSSD", MessageBoxButtons.YesNo))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                CancelChange();
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //m_CalibrationInfo는 다시 원복
            CancelChange();

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TextControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (((TextBox)sender).Name == txtGain.Name)
            //{
            //    if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || (e.KeyChar == '.')))
            //        e.Handled = true;
            //    else if ((e.KeyChar == '.') && txtGain.Text.Contains("."))
            //        e.Handled = true;
            //}
            //else
            //{
            //    if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
            //        e.Handled = true;
            //}
        }
        #endregion
    }
}
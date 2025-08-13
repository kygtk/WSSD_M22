using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;

namespace Dms.Control
{
    public partial class DlgRfUnit : Form
    {
        #region Fields
        private ClientManager m_Client = ClientManager.Instance;
        private DeviceTag m_TagRfUnit = null;

        private TagDescriptorRfUnit tagDescriptorRfUnit = new TagDescriptorRfUnit();
        #endregion

        #region Constructor
        public DlgRfUnit(DeviceTag tagRfUnit)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_TagRfUnit = tagRfUnit;
            
            tbPowerSet.Text = m_TagRfUnit[tagDescriptorRfUnit.SETRFPOWER].Value;
            tbLoadPreset.Text = m_TagRfUnit[tagDescriptorRfUnit.LOADPOS].Value;
            tbTunePreset.Text = m_TagRfUnit[tagDescriptorRfUnit.TUNEPOS].Value;
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRfPowerOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.PowerOn);
        }

        private void btnRfPowerOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.PowerOff);
        }

        private void btnRfInterlockSet_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.InterlockSet);
        }

        private void btnInterlockRelease_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.InterlockRelease);
        }
        private void btnPowerSet_Click(object sender, EventArgs e)
        {
            
        }
        private void tbTunePreset_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "100.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbTunePreset.Text);

            if (tbTunePreset.Text != curValue)
            {
                tbTunePreset.Text = curValue;

                m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.TunePresetSetting, tbTunePreset.Text);
            }
        }

        private void tbLoadPreset_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "100.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbLoadPreset.Text);

            if (tbLoadPreset.Text != curValue)
            {
                tbLoadPreset.Text = curValue;

                m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.LoadPresetSetting, tbLoadPreset.Text);
            }
        }

        private void btnPresetSet_Click(object sender, EventArgs e)
        {
            //
            //
        }           

        private void tbPowerSet_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "15.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbPowerSet.Text);
                        
            if (tbPowerSet.Text != curValue)
            {
                tbPowerSet.Text = curValue;

                m_Client.SendCommand(Command.RfUnitManual, m_TagRfUnit.DeviceName, RfAct.PowerSet, tbPowerSet.Text);
            }
        }
    }
}
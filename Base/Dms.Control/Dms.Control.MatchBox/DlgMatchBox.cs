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
    public partial class DlgMatchBox : Form
    {
        #region Fields
        private ClientManager m_Client = ClientManager.Instance;
        private DeviceTag m_TagRfg = null;
        private DeviceTag m_TagRfTuner = null;

        private TagDescriptorRfg tagDescriptorRfg = new TagDescriptorRfg();
        private TagDescriptorRfTuner tagDescriptorRfTuner = new TagDescriptorRfTuner();
        #endregion

        #region Constructor
        public DlgMatchBox(DeviceTag tagRfg, DeviceTag tagRfTuner)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_TagRfg = tagRfg;
            m_TagRfTuner = tagRfTuner;

            tbPowerSet.Text = m_TagRfg[tagDescriptorRfg.SETRFPOWER].Value;
            tbLoadPreset.Text = m_TagRfTuner[tagDescriptorRfTuner.LOADPOS].Value;
            tbTunePreset.Text = m_TagRfTuner[tagDescriptorRfTuner.TUNEPOS].Value;
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnRfPowerOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfgManual, m_TagRfg.DeviceName, RfAct.PowerOn);
        }

        private void btnRfPowerOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfgManual, m_TagRfg.DeviceName, RfAct.PowerOff);
        }

        private void btnRfInterlockSet_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfgManual, m_TagRfg.DeviceName, RfAct.InterlockSet);
        }
             
        private void btnInterlockRelease_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.RfgManual, m_TagRfg.DeviceName, RfAct.InterlockRelease);
        }

        //private void btnRfTunerInterlockSet_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.InterlockSet);
        //}

        //private void btnRfTunerInterlockRelease_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.InterlockRelease);
        //}


        //private void btnRfTunerInterlockSet_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.InterlockSet);
        //}

        //private void btnRfTunerInterlockRelease_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.InterlockRelease);
        //}


        //private void btnRfTunerAutoTunnigSet_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.AutoTunningSet);
        //}

        //private void btnRfTunerAutoTunningRelese_Click(object sender, EventArgs e)
        //{
        //    m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.AutoTunningRelease);
        //}
        
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

                m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.TunePresetSetting, tbTunePreset.Text);
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

                m_Client.SendCommand(Command.RfTunerManual, m_TagRfTuner.DeviceName, RfTunerAct.LoadPresetSetting, tbLoadPreset.Text);
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

                m_Client.SendCommand(Command.RfgManual, m_TagRfg.DeviceName, RfAct.PowerSet, tbPowerSet.Text);
            }
        }
    }
}
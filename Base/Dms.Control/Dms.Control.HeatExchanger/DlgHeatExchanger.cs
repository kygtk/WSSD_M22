using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;
using Dms.Server;

namespace Dms.Control
{
    public partial class DlgHeatExchanger : Form
    {
        #region Tag Descriptor
        public static TagDescriptorHeatExchanger tagDescriptor = new TagDescriptorHeatExchanger();
        #endregion

        #region Fields
        private ClientManager m_Client;
        protected DeviceTagInfo m_TagInfo = null;
        private DeviceTag m_TagHeatExchangerUnit = null;
        private Dms.Device.HeatExchanger m_HeatExchagerUnit;
              
        private Color m_OnColor = Color.Lime;
        private Color m_OffColor = Color.White;
        #endregion

        #region Properties
        [Category("DMS : UI"),
         Description("Select sensor on color")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }       
        [Category("DMS : UI"),
         Description("Select sensor off color")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        #endregion

        #region
        public DlgHeatExchanger()
        {
            InitializeComponent();

            m_Client = ClientManager.Instance;
           
            m_TagInfo = new DeviceTagInfo("HeatExchanger");            
        }
        #endregion

        public void Initialize(DeviceTag tagHeatExchanger)
        {
            m_TagHeatExchangerUnit = tagHeatExchanger;

            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;
            m_HeatExchagerUnit = components[m_TagHeatExchangerUnit.DeviceName] as Dms.Device.HeatExchanger;

            tmrUpdateState.Enabled = true;
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerRun);
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerStop);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tmrUpdateState.Stop();

            this.Close();
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_HeatExchagerUnit.IsEmo() == true) lblEmo.BackColor = Color.Red;
            else lblEmo.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsAlarm() == true) lblAlarm.BackColor = Color.Red;
            else lblAlarm.BackColor = Color.White;
            
            if (m_HeatExchagerUnit.IsWarning() == true) lblWarning.BackColor = Color.Yellow;
            else lblWarning.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsOn() == true) lblRunning.BackColor = Color.YellowGreen;
            else lblRunning.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsChamber1Ready() == true) lblCh1Ready.BackColor = Color.YellowGreen;
            else lblCh1Ready.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsChamber2Ready() == true) lblCh2Ready.BackColor = Color.YellowGreen;
            else lblCh2Ready.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsChamber3Ready() == true) lblCh3Ready.BackColor = Color.YellowGreen;
            else lblCh3Ready.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsPowerOnLed() == true) lblPowerOnLed.BackColor = Color.YellowGreen;
            else lblPowerOnLed.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsAlarmLed() == true) lblAlarmLed.BackColor = Color.YellowGreen;
            else lblAlarmLed.BackColor = Color.White;

            if (m_HeatExchagerUnit.IsReadyLed() == true) lblReadyLed.BackColor = Color.YellowGreen;
            else lblReadyLed.BackColor = Color.White;
            //lblEmo =
            //lblAlarm = 
            //lblRunning = 
            //lblWarning = 
            
            //lblAlarmLed = 
            //lblPowerOnLed = 
            //lblCh1Ready = 
            //lblCh2Ready = 
            //lblCh3Ready =
        }

        private void tbCh1Temp_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "100.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbCh1Temp.Text);

            if (tbCh1Temp.Text != curValue)
            {
                tbCh1Temp.Text = curValue;

                m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerSetTemp, 0, tbCh1Temp.Text);
            }
        }

        private void tbCh2Temp_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "100.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbCh2Temp.Text);

            if (tbCh2Temp.Text != curValue)
            {
                tbCh2Temp.Text = curValue;

                m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerSetTemp, 1, tbCh2Temp.Text);
            }
        }

        private void tbCh3Temp_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            string curValue;

            validation.Low = "0.0";
            validation.High = "100.0";
            validation.Format = OptionFormat.Float;

            curValue = validation.ShowEditDialog("", tbCh3Temp.Text);

            if (tbCh3Temp.Text != curValue)
            {
                tbCh3Temp.Text = curValue;

                m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerSetTemp, 2, tbCh3Temp.Text);
            }
        }

        private void btnTempSet_Click(object sender, EventArgs e)
        {
            
            //m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerSetTemp, 1, tbCh2Temp.Text);
            //m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerSetTemp, 2, tbCh3Temp.Text);
        }

        private void btnRemoteMode_CheckedChanged(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerRemoteMode, OpMode.Remote);
        }

        private void btnLocalMode_CheckedChanged(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.HeatExchangerManual, m_HeatExchagerUnit, HeatExchangerAct.HeatExchangerRemoteMode, OpMode.Local);
        }

        #region Override
        
        #endregion
    }
}
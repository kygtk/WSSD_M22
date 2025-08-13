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
    public partial class ViewShinkoDry : UserControl
    {
        private ClientManager m_Client;
        private Dms.Device.ShinkoDryCleaner m_Device = null;
        private bool m_Initialized = false;

        public ViewShinkoDry()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);    //깜박임 방지
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(Dms.Device.ShinkoDryCleaner device)
        {
            if (device == null) return;

            m_Device = device;
            m_Client = ClientManager.Instance;

            labelTitle.Text = " " + m_Device.Name;

            // Initialize Gauges
            if (device.GaugePressure1 == null)
            {
                this.gaugePressure1.Visible = false;
            }
            else
            {
                this.gaugePressure1.DeviceTagInfo = new DeviceTagInfo(device.GaugePressure1);
                this.gaugePressure1.Initialize(device.ServerManager.TagContainer);
            }

            if (device.GaugePressure2 == null)
            {
                this.gaugePressure2.Visible = false;
            }
            else
            {
                this.gaugePressure2.DeviceTagInfo = new DeviceTagInfo(device.GaugePressure2);
                this.gaugePressure2.Initialize(device.ServerManager.TagContainer);
            }

            if (device.GaugeVaccum1 == null)
            {
                this.gaugeVacuum1.Visible = false;
            }
            else
            {
                this.gaugeVacuum1.DeviceTagInfo = new DeviceTagInfo(device.GaugeVaccum1);
                this.gaugeVacuum1.Initialize(device.ServerManager.TagContainer);
            }

            if (device.GaugeVaccum2 == null)
            {
                this.gaugeVacuum2.Visible = false;
            }
            else
            {
                this.gaugeVacuum2.DeviceTagInfo = new DeviceTagInfo(device.GaugeVaccum2);
                this.gaugeVacuum2.Initialize(device.ServerManager.TagContainer);
            }

            if (device.GaugeTemperature == null)
            {
                this.gaugeTemperature.Visible = false;
            }
            else
            {
                this.gaugeTemperature.DeviceTagInfo = new DeviceTagInfo(device.GaugeTemperature);
                this.gaugeTemperature.Initialize(device.ServerManager.TagContainer);
            }

            m_Initialized = true;

            tmrUpdateState.Enabled = true;  //시작 상태 체크
            UpdateState();
        }

        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.PowerOn);
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.PowerOff);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.Run);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.Stop);
        }

        private void btnEmo_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.EStop);
        }

        private void btnEmoRelease_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShinkoDryCleanerManual, m_Device, ShinkoDryCleanerAct.EStopRelease);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();

            bool permission = true;
            permission &= !m_Client.GenInfos.AutoMode;
            permission &= m_Client.CurrentUserLevel >= UserLevels.Technician;

            this.Enabled = permission;       //Auto Mode일 경우 모든 Control을 Disable만듬.
        }

        private void UpdateState()
        {
            if (m_Initialized)
            {
                chkBlowRun.Checked = m_Device.DiBlowerRun.GetState();

                chkPreFilterError.Checked = m_Device.DiPreFilterError.GetState();

                chkHepaFilterError.Checked = m_Device.DiHepaFilterError.GetState();

                chkInverterError.Checked = m_Device.DiInverterError.GetState();

                chkWaterLeak.Checked = m_Device.DiWaterLeak.GetState();

                chkTempError.Checked = m_Device.DiTempError.GetState();

                chkPresError.Checked = m_Device.DiPresError.GetState();

                chkEmoIn.Checked = m_Device.DiEmo.GetState();

                chkPowerOn.Checked = m_Device.DoReady.GetState();

                chkRun.Checked = m_Device.DoRun.GetState();

                chkEmoOut.Checked = m_Device.DoEmo.GetState();

                // 다른 상태 갱신 추가
            }
        }
    }
}
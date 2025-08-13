using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;

namespace Dms.Control
{
    public partial class ViewSeAp : UserControl
    {
        #region Fields
        private Dms.Device.SeAp m_Ap = null;
        private ClientManager m_Client;
        private bool m_Initialized = false;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ViewSeAp()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        public void Initialize(Dms.Device.SeAp ap)
        {
            m_Ap = ap;
            m_Client = ClientManager.Instance;
            InitView();
            m_Initialized = true;
        }

        private void InitView()
        {

            SetTags();
            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        private void SetTags()
        {
            chkIntrOpen.Tag = (int)SeApAlarmIndex.plasmaAlmInterlockErr;
            chkInverterErr.Tag = (int)SeApAlarmIndex.plasmaAlmInverterErr;
            chkLowVoltageErr.Tag = (int)SeApAlarmIndex.plasmaAlmLowVoltageErr;
            chkHighVoltageErr.Tag = (int)SeApAlarmIndex.plasmaAlmHighVoltageErr;
            chkARCFault.Tag = (int)SeApAlarmIndex.plasmaAlmArcFault;
            chkLocalModeRunErr.Tag = (int)SeApAlarmIndex.plasmaAlmLocalModeRunErr;
            chkOnFault.Tag = (int)SeApAlarmIndex.plasmaAlmOnFaultErr;
            chkNoErr.Tag = (int)SeApAlarmIndex.plasmaNoAlm;

            chkPowerReady.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPowerReady;
            chkStatus0.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus0;
            chkStatus1.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus1;
            chkStatus2.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus2;

            chkN2PressLowLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiN2PressLowLimitAlarm;
            chkN2PressHighLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiN2PressHighLimitAlarm;
            chkCDAPressLowLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiCDAPressLowLimitAlarm;
            chkCDAPressHighLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiCDAPressHighLimitAlarm;
            chkPCWFlowLowLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowLowLimitAlarm;
            chkPCWFlowHighLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowHighLimitAlarm;
            chkPCWPressLowLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWPressLowLimitAlarm;
            chkPCWPressHighLimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWPressHighLimitAlarm;

            chkHouseClose.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiHouseClose;

            btnN2Set.Tag = Common.ApPlasmaAct.SetN2Flow;
            btnCDASet.Tag = Common.ApPlasmaAct.SetCDAFlow;
            btnVoltageSet.Tag = Common.ApPlasmaAct.SetVoltage;
            btnPowerOn.Tag = Common.ApPlasmaAct.PowerOnOff;
            btnCylUp.Tag = Common.ApPlasmaAct.HouseUp;
            btnCylDown.Tag = Common.ApPlasmaAct.HouseDown;
            btnN2Reset.Tag = Common.ApPlasmaAct.ResetN2Flow;
            btnCDAReset.Tag = Common.ApPlasmaAct.ResetCDAFlow;
            btnVoltageReset.Tag = Common.ApPlasmaAct.ResetVoltage;

            //txtSetN2.Text = m_Ap.SetupApN2FlowSet.GetValue<float>().ToString();
            //txtSetCDA.Text = m_Ap.SetupApCDAFlowSet.GetValue<float>().ToString();
            //txtSetVol.Text = m_Ap.SetupApPowerSet.GetValue<float>().ToString();
        }

        private void UpdateState()
        {
            txtCurN2.Text = m_Ap.MfcN2.Gauge.CurValue.ToString();
            txtCurCDA.Text = m_Ap.MfcCDA.Gauge.CurValue.ToString();
            //txtCurPCW.Text = m_Ap.GaugePCW.CurValue.ToString();
            txtCurVol.Text = m_Ap.MfcVoltage.Gauge.CurValue.ToString();

            //Update Ap House's Cylinder
            chkUp.Checked = m_Ap.ActuatorUnit.IsPositive();
            chkDown.Checked = m_Ap.ActuatorUnit.IsNegative();

            //Update Signals
            foreach (CheckBox box in gbSignal.Controls)
            {
                box.Checked = ((Dms.Device.IoDigitalInput)box.Tag).GetState();
            }

            //Update Alarm Status
            int value = m_Ap.GetStatusAlarm();
            foreach (CheckBox box in gbStatus.Controls)
            {
                box.Checked = (int)box.Tag == value;
            }

            //Update Power On/Off Button
            if (m_Ap.IsPowerOn())
            {
                btnPowerOn.Text = "Power Off";
                gbHouse.Enabled = false;
            }
            else
            {
                btnPowerOn.Text = "Power On";
            }
            chkPowerOn.Checked = m_Ap.IsPowerOn();
        }

        private void SetControls(bool enable)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (m_Ap.IsPowerOn())
                {
                    if (control.Name != gbHouse.Name) control.Enabled = enable;
                }
                else control.Enabled = enable;
            }

            //txtSetN2.Text = m_Ap.SetupApN2FlowSet.GetValue<float>().ToString();
            //txtSetCDA.Text = m_Ap.SetupApCDAFlowSet.GetValue<float>().ToString();
            //txtSetVol.Text = m_Ap.SetupApPowerSet.GetValue<float>().ToString();
        }


        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string value = "";
            if (button.Name == btnN2Set.Name)
            {
                value = txtSetN2.Text;
                if (value == "")
                {
                    MessageBox.Show("Key in the setting value first!!");
                    return;
                }
            }
            else if (button.Name == btnCDASet.Name)
            {
                value = txtSetCDA.Text;
                if (value == "")
                {
                    MessageBox.Show("Key in the setting value first!!");
                    return;
                }
            }
            else if (button.Name == btnVoltageSet.Name)
            {
                value = txtSetVol.Text;
                if (value == "")
                {
                    MessageBox.Show("Key in the setting value first!!");
                    return;
                }
            }

            m_Client.SendCommand(Dms.Common.Command.ApPlasmaManual, m_Ap, button.Tag, value);
        }

        private void btnCyl_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.ApPlasmaManual, m_Ap, button.Tag, bool.TrueString);
        }

        private void btnCyl_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            m_Client.SendCommand(Dms.Common.Command.ApPlasmaManual, m_Ap, button.Tag, bool.FalseString);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                SetControls(!m_Client.GenInfos.AutoMode);   //Auto Mode일 경우 모든 Control을 Disable만듬.
                UpdateState();
            }
        }
    }
}

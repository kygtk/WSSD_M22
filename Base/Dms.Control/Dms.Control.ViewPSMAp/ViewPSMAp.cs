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
    public partial class ViewPSMAp : UserControl
    {
        #region Fields
        private Dms.Device.PSMAp m_Ap = null;
        private ClientManager m_Client;
        private bool m_HasColorBoard;
        private bool m_Initialized = false;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ViewPSMAp()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        public void Initialize(Dms.Device.PSMAp ap)
        {
            m_Ap = ap;
            m_Client = ClientManager.Instance;
            InitView();
            m_Initialized = true;
        }

        private void InitView()
        {
            if (m_Ap.ColorBoard == null)
            {
                m_HasColorBoard = false;
            }
            else m_HasColorBoard = true;

            SetTags();
            UpdateState();
            tmrUpdateState.Enabled = true;
        }

        private void SetTags()
        {
            chkIntrOpen.Tag = (int)PSMApAlarmIndex.errIntrOpen;
            chkLowVol.Tag = (int)PSMApAlarmIndex.errLowVoltage;
            chkSystemError.Tag = (int)PSMApAlarmIndex.errSystem;
            chkOutputOpen.Tag = (int)PSMApAlarmIndex.errOutput;
            chkLocalMode.Tag = (int)PSMApAlarmIndex.errLocal;
            chkARCTrip.Tag = (int)PSMApAlarmIndex.errARC;
            chkNotError.Tag = (int)PSMApAlarmIndex.errNone;

            chkPowerReady.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPowerReady;
            chkStatus0.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus0;
            chkStatus1.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus1;
            chkStatus2.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus2;
            chkN2Limit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiN2FlowLimitAlarm;
            chkCDALimit.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiCDAFlowLimitAlarm;
            chkPCWLimit.Tag = ((Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowLimitAlarm != null) ? (Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowLimitAlarm : null;
            chkUpExhaustAlarm.Tag = ((Dms.Device.IoDigitalInput)m_Ap.DiUpExhaustAlarm != null) ? (Dms.Device.IoDigitalInput)m_Ap.DiUpExhaustAlarm : null;
            chkLowExhaustAlarm.Tag = ((Dms.Device.IoDigitalInput)m_Ap.DiLowExhaustAlarm != null) ? (Dms.Device.IoDigitalInput)m_Ap.DiLowExhaustAlarm : null;

            btnN2Set.Tag = Common.ApPlasmaAct.SetN2Flow;
            btnCDASet.Tag = Common.ApPlasmaAct.SetCDAFlow;
            btnVoltageSet.Tag = Common.ApPlasmaAct.SetVoltage;
            btnPowerOn.Tag = Common.ApPlasmaAct.PowerOnOff;
            btnCylUp.Tag = Common.ApPlasmaAct.HouseUp;
            btnCylDown.Tag = Common.ApPlasmaAct.HouseDown;
            btnN2Reset.Tag = Common.ApPlasmaAct.ResetN2Flow;
            btnCDAReset.Tag = Common.ApPlasmaAct.ResetCDAFlow;
            btnVoltageReset.Tag = Common.ApPlasmaAct.ResetVoltage;
        }

        private void UpdateState()
        {
            txtCurN2.Text = m_Ap.MfcN2.Gauge.CurValue.ToString();
            txtCurCDA.Text = m_Ap.MfcCDA.Gauge.CurValue.ToString();
            txtCurPCW.Text = m_Ap.GaugePCW.CurValue.ToString();
            txtCurVol.Text = m_Ap.MfcVoltage.Gauge.CurValue.ToString();

            //Update Ap House's Cylinder && Chamber Close State
            chkUp.Checked = m_Ap.ActuatorUnit.IsPositive();
            chkDown.Checked = m_Ap.ActuatorUnit.IsNegative();
            chkChamberClose.Checked = m_Ap.IsChamberClose();

            //Update Signals
            foreach (CheckBox box in gbSignal.Controls)
            {
                if (box.Tag != null)
                {
                    box.Checked = ((Dms.Device.IoDigitalInput)box.Tag).GetState();
                }
            }

            //Update Alarm Status
            int value = m_Ap.GetStatusAlarm();
            foreach (CheckBox box in gbStatus.Controls)
            {
                if (box.Tag != null)
                {
                    box.Checked = (int)box.Tag == value;
                }
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

            //Update ColorBoard
            if (m_Ap.ColorBoard != null)
            {
                txtCurRed.Text = m_Ap.ColorBoard.GaugeRed.CurValue.ToString();
                txtCurGreen.Text = m_Ap.ColorBoard.GaugeGreen.CurValue.ToString();
                txtCurBlue.Text = m_Ap.ColorBoard.GaugeBlue.CurValue.ToString();
            }
        }

        private void SetControls(bool enable)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                control.Enabled = enable;
            }

            if (enable && !m_HasColorBoard)
            {
                foreach (System.Windows.Forms.Control control in gbColorBoard.Controls)
                {
                    control.Enabled = false;
                }
            }

            if (chkPCWLimit.Enabled && chkPCWLimit.Tag == null) chkPCWLimit.Enabled = false;
            if (chkUpExhaustAlarm.Enabled && chkUpExhaustAlarm.Tag == null) chkUpExhaustAlarm.Enabled = false;
            if (chkLowExhaustAlarm.Enabled && chkLowExhaustAlarm.Tag == null) chkLowExhaustAlarm.Enabled = false;
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

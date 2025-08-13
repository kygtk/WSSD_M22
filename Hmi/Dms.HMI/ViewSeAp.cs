using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Client;
using Dms.Common;
using Dms.Device;
using Dms.Server;

namespace Dms.HMI
{
    public partial class ViewSeAp : UserControl
    {
        #region Fields
        private SeAp m_Ap = null;
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
            chkIntrOpen.Tag = (int)SeApAlarmIndex.plasmaAlmInterlockErr; // 10.12.21 minhan
            chkInverterErr.Tag = (int)SeApAlarmIndex.plasmaAlmInverterErr;
            chkLowVoltageErr.Tag = (int)SeApAlarmIndex.plasmaAlmLowVoltageErr;
            chkHighVoltageErr.Tag = (int)SeApAlarmIndex.plasmaAlmHighVoltageErr;
            chkARCFault.Tag = (int)SeApAlarmIndex.plasmaAlmArcFault;
            chkLocalModeRunErr.Tag = (int)SeApAlarmIndex.plasmaAlmLocalModeRunErr;
            chkOnFault.Tag = (int)SeApAlarmIndex.plasmaAlmOnFaultErr;
            chkNoErr.Tag = (int)SeApAlarmIndex.plasmaNoAlm;

            chkStatus0.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus0;
            chkStatus1.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus1;
            chkStatus2.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiAlarmStatus2;
            //chkN2Interlock.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiN2PressHighLimitAlarm; //일단 기존 SE AP Device 를 활용하자.... Mr.Kang
            //chkPCWInterlock.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowHighLimitAlarm;

            chkN2Interlock.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiN2FlowLowLimitAlarm; // 10.12.21 minhan
            chkPCWInterlock.Tag = (Dms.Device.IoDigitalInput)m_Ap.DiPCWFlowLowLimitAlarm;

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
            txtCurPCW.Text = eqpGauges._AP_Unit_PCW_Out_Gauge.CurValue.ToString();
            txtCurVol.Text = m_Ap.MfcVoltage.Gauge.CurValue.ToString();
            txtCurWATT.Text = eqpGauges._AP_Unit_Power_Gauge.CurValue.ToString();

            txtCurN2Press.Text = eqpGauges._AP_Unit_N2_Pressure_Gauge.CurValue.ToString();
            txtCurCDAPress.Text = eqpGauges._AP_Unit_CDA_Pressure_Gauge.CurValue.ToString();
            txtCurPCWPress.Text = eqpGauges._AP_Unit_PCW_Pressure_Gauge.CurValue.ToString();

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
                box.Checked = (int)box.Tag == value ? true : false;
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

            if (m_Ap.PcwInValve.IsOpen() == true) // 10.06.22 minhan
            {
                pcwopen_btn.BackColor = System.Drawing.Color.Red;
                pcwclose_btn.BackColor = System.Drawing.Color.CornflowerBlue; // 10.11.13 minhan
            }
            else
            {
                pcwopen_btn.BackColor = System.Drawing.Color.CornflowerBlue;
                pcwclose_btn.BackColor = System.Drawing.Color.Red; // 10.11.13 minhan
            }

            checkPCWFlow.Checked = eqpGauges._AP_Unit_PCW_Out_Gauge.IsAlarm; // 10.12.21 minhan
            checkPCW.Checked = eqpGauges._AP_Unit_PCW_Pressure_Gauge.IsAlarm;
            checkN2.Checked = eqpGauges._AP_Unit_N2_Pressure_Gauge.IsAlarm;
            checkCDA.Checked = eqpGauges._AP_Unit_CDA_Pressure_Gauge.IsAlarm;
            checkWatt.Checked = eqpGauges._AP_Unit_Power_Gauge.IsAlarm;
        }

        private void SetControls(bool enable)
        {  
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (m_Ap.IsPowerOn())
                {
                    if(control.Name != gbHouse.Name) control.Enabled = enable;
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
            else if (button.Name == btnCDAReset.Name) // 10.06.18 minhan
            {
                if (m_Ap.IsPowerOn()) 
                {
                    MessageBox.Show("Now is Plasma ON Please Plasma Off", "WSSD", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    txtSetCDA.Text = "0";
                }
            }
            else if (button.Name == btnN2Reset.Name) // 10.06.18 minhan
            {
                if (m_Ap.IsPowerOn())
                {
                    MessageBox.Show("Now is Plasma ON Please Plasma Off", "WSSD", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    txtSetN2.Text = "0";
                }
            }
            else if (button.Name == btnVoltageReset.Name) // 10.06.18 minhan
            {
                if (m_Ap.IsPowerOn())
                {
                    MessageBox.Show("Now is Plasma ON Please Plasma Off", "WSSD", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    txtSetVol.Text = "0";
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

        private void btn_PcwOpen(object sender, EventArgs e) // 10.06.17 minhan 
        {
            if (m_Client.GenInfos.AutoMode) return;
            //else if (eqpSeAps._SE_AP_Plasma_Unit.DiPCWFlowLowLimitAlarm.GetState() == false) // 10.06.18 minhan
            //{
            //    MessageBox.Show("SE PLASMA PCW FLOW Nomal Status.");
            //    return;
            //}
            else if (eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.IsOpen() == true)
            {
                MessageBox.Show("SE PLASMA PCW SOL VALVE IS OPEN.");
                return;
            }
            else if (eqpLeakSensors._AP_Unit_Leak_Sensor1.IsDetectedInterlock() || eqpLeakSensors._AP_Unit_Leak_Sensor2.IsDetectedInterlock() ||
                 eqpLeakSensors._AP_Unit_Leak_Sensor3.IsDetectedInterlock())
            {
                MessageBox.Show("AP UNIT LEAK ALARM");
                return;
            }
            else
            {
                eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.Open();
                pcwopen_btn.BackColor = System.Drawing.Color.Red;
                pcwclose_btn.BackColor = System.Drawing.Color.CornflowerBlue;
            }
        }

        private void btn_PcwClose(object sender, EventArgs e) // 10.12.21 minhan
        {
            if (m_Client.GenInfos.AutoMode) return;
            else if (eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.IsClose() == true)
            {
                MessageBox.Show("SE PLASMA PCW SOL VALVE IS CLOSE.");
                return;
            }
            else
            {
                eqpSeAps._SE_AP_Plasma_Unit.PcwInValve.Close();
                pcwopen_btn.BackColor = System.Drawing.Color.CornflowerBlue;
                pcwclose_btn.BackColor = System.Drawing.Color.Red;
            }
        }
    }
}

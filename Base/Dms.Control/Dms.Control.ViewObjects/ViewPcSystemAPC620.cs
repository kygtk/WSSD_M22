using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.PcSystemInfo;
using Dms.Common;

namespace Dms.Control
{
    public partial class ViewPcSystemAPC620 : UserControl
    {
        private BrPc m_Pc = null;

        public ViewPcSystemAPC620()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(_PcSystemInfo pcSystemInfo)
        {
            m_Pc = pcSystemInfo as BrPc;

            if (m_Pc != null)
            {
                SetTags();
                InitData();
                //SetEnable();
            }
        }

        public void SetMonitorTimer(bool enable)
        {
            if(m_Pc != null) this.tmrUpdateState.Enabled = enable;
        }

        public void Save()
        {
            if (m_Pc == null) return;

            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (control.GetType() == typeof(GroupBox))
                {
                    foreach (System.Windows.Forms.Control con in control.Controls)
                    {
                        object tag = con.Tag;
                        if (tag != null)
                        {
                            Type conType = con.GetType();
                            Type tagType = tag.GetType();
                            if (conType == typeof(ValidationTextBox))
                            {
                                ValidationTextBox txtBox = con as ValidationTextBox;
                                string text = txtBox.Text;
                                string refTagText = txtBox.ReferenceTag as string;
                                if (tagType == typeof(Function<string>))
                                {
                                    if (refTagText == _PcSystemInfo.entryMax) (tag as Function<string>).Max = text;
                                    else if (refTagText == _PcSystemInfo.entryMin) (tag as Function<string>).Min = text;
                                }
                                else if (tagType == typeof(Function<sbyte>))
                                {
                                    if (refTagText == _PcSystemInfo.entryMax) (tag as Function<sbyte>).Max = Convert.ToSByte(text);
                                    else if (refTagText == _PcSystemInfo.entryMin) (tag as Function<sbyte>).Min = Convert.ToSByte(text);
                                }
                                else if (tagType == typeof(Function<int>))
                                {
                                    if (refTagText == _PcSystemInfo.entryMax) (tag as Function<int>).Max = Convert.ToInt32(text);
                                    else if (refTagText == _PcSystemInfo.entryMin) (tag as Function<int>).Min = Convert.ToInt32(text);
                                }
                                else if (tagType == typeof(Function<ushort>))
                                {
                                    if (refTagText == _PcSystemInfo.entryMax) (tag as Function<ushort>).Max = Convert.ToUInt16(text);
                                    else if (refTagText == _PcSystemInfo.entryMin) (tag as Function<ushort>).Min = Convert.ToUInt16(text);
                                }
                                else if (tagType == typeof(Function<short>))
                                {
                                    if (refTagText == _PcSystemInfo.entryMax) (tag as Function<short>).Max = Convert.ToInt16(text);
                                    else if (refTagText == _PcSystemInfo.entryMin) (tag as Function<short>).Min = Convert.ToInt16(text);
                                }
                            }
                            else if (conType == typeof(CheckBox))
                            {
                                bool state = (con as CheckBox).Checked;
                                if (tagType == typeof(Function<string>))
                                {
                                    (tag as Function<string>).AlarmEnable = state;
                                }
                                else if (tagType == typeof(Function<sbyte>))
                                {
                                    (tag as Function<sbyte>).AlarmEnable = state;
                                }
                                else if (tagType == typeof(Function<int>))
                                {
                                    (tag as Function<int>).AlarmEnable = state;
                                }
                                else if (tagType == typeof(Function<ushort>))
                                {
                                    (tag as Function<ushort>).AlarmEnable = state;
                                }
                                else if (tagType == typeof(Function<short>))
                                {
                                    (tag as Function<short>).AlarmEnable = state;
                                }
                            }
                        }
                    }
                }
            }
            m_Pc.SaveAll();
        }

        private void SetTags()
        {
            Function<sbyte> sbyteFunc = m_Pc.funcGetCpuTemperature;

            txtTempCpu.Tag = sbyteFunc;
            txtTempCpuMax.Tag = sbyteFunc;
            txtTempCpuMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempCpu.Tag = sbyteFunc;

            sbyteFunc = m_Pc.funcGetCpuBoardTemerature;
            txtTempCpuBoard.Tag = sbyteFunc;
            txtTempCpuBoardMax.Tag = sbyteFunc;
            txtTempCpuBoardMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempCpuBoard.Tag = sbyteFunc;

            sbyteFunc = m_Pc.funcGetBoardIoTemperature;
            txtTempBoardIo.Tag = sbyteFunc;
            txtTempBoardIoMax.Tag = sbyteFunc;
            txtTempBoardIoMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempBoardIo.Tag = sbyteFunc;

            sbyteFunc = m_Pc.funcGetBoardPowerSupplyTemperature;
            txtTempPower.Tag = sbyteFunc;
            txtTempPowerMax.Tag = sbyteFunc;
            txtTempPowerMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempPower.Tag = sbyteFunc;

            sbyteFunc = m_Pc.funcGetBoardDrive1Temperature;
            txtTempDrive1.Tag = sbyteFunc;
            txtTempDrive1Max.Tag = sbyteFunc;
            txtTempDrive1Max.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempDrive1.Tag = sbyteFunc;

            sbyteFunc = m_Pc.funcGetBoardDrive2Temperature;
            txtTempDrive2.Tag = sbyteFunc;
            txtTempDrive2Max.Tag = sbyteFunc;
            txtTempDrive2Max.ReferenceTag = _PcSystemInfo.entryMax;
            chkTempDrive2.Tag = sbyteFunc;

            Function<int> intFunc = m_Pc.funcGetCpuVoltage;
            txtVoltageCpu.Tag = intFunc;
            txtVoltageCpuMax.Tag = intFunc;
            txtVoltageCpuMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageCpu.Tag = intFunc;
            txtVoltageCpuMin.Tag = intFunc;
            txtVoltageCpuMin.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetDCVoltage;
            txtVoltageDC.Tag = intFunc;
            txtVoltageDCMax.Tag = intFunc;
            txtVoltageDCMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageDC.Tag = intFunc;
            txtVoltageDCMin.Tag = intFunc;
            txtVoltageDCMin.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetStandbyVoltage;
            txtVoltageStandby.Tag = intFunc;
            txtVoltageStandbyMax.Tag = intFunc;
            txtVoltageStandbyMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageStandby.Tag = intFunc;
            txtVoltageStandbyMin.Tag = intFunc;
            txtVoltageStandbyMin.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetBatteryVoltage;
            txtVoltageBattery.Tag = intFunc;
            txtVoltageBatteryMax.Tag = intFunc;
            txtVoltageBatteryMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageBattery.Tag = intFunc;
            txtVoltageBatteryMin.Tag = intFunc;
            txtVoltageBatteryMin.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetCpu2Voltage;
            txtVoltageCpu2.Tag = intFunc;
            txtVoltageCpu2Max.Tag = intFunc;
            txtVoltageCpu2Max.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageCpu2.Tag = intFunc;
            txtVoltageCpu2Min.Tag = intFunc;
            txtVoltageCpu2Min.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetDC2Voltage;
            txtVoltageDC2.Tag = intFunc;
            txtVoltageDC2Max.Tag = intFunc;
            txtVoltageDC2Max.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageDC2.Tag = intFunc;
            txtVoltageDC2Min.Tag = intFunc;
            txtVoltageDC2Min.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetUpsBatteryVoltage;
            txtVoltageUPSBattery.Tag = intFunc;
            txtVoltageUPSBatteryMax.Tag = intFunc;
            txtVoltageUPSBatteryMax.ReferenceTag = _PcSystemInfo.entryMax;
            chkVoltageUPSBattery.Tag = intFunc;
            txtVoltageUPSBatteryMin.Tag = intFunc;
            txtVoltageUPSBatteryMin.ReferenceTag = _PcSystemInfo.entryMin;

            intFunc = m_Pc.funcGetCmosBatteryState;
            txtStatisCMOSBattery.Tag = intFunc;
            chkStatisCMOSBattery.Tag = intFunc;

            Function<short> shortFunc = m_Pc.funcGetCpuFanSpeed;
            txtFanCpu.Tag = shortFunc;
            txtFanCpuMin.Tag = shortFunc;
            chkFanCpu.Tag = shortFunc;
            txtFanCpuMin.ReferenceTag = _PcSystemInfo.entryMin;

            shortFunc = m_Pc.funcGetCaseFan1Speed;
            txtFan1.Tag = shortFunc;
            txtFan1Min.Tag = shortFunc;
            chkFan1.Tag = shortFunc;
            txtFan1Min.ReferenceTag = _PcSystemInfo.entryMin;

            shortFunc = m_Pc.funcGetCaseFan2Speed;
            txtFan2.Tag = shortFunc;
            txtFan2Min.Tag = shortFunc;
            chkFan2.Tag = shortFunc;
            txtFan2Min.ReferenceTag = _PcSystemInfo.entryMin;

            shortFunc = m_Pc.funcGetCaseFan3Speed;
            txtFan3.Tag = shortFunc;
            txtFan3Min.Tag = shortFunc;
            chkFan3.Tag = shortFunc;
            txtFan3Min.ReferenceTag = _PcSystemInfo.entryMin;

            Function<ushort> ushortFunc = m_Pc.funcGetPowerOnCycles;
            txtStaPowerOnCycle.Tag = ushortFunc;

            ushortFunc = m_Pc.funcGetPowerOnHours;
            txtStaPowerOnHour.Tag = ushortFunc;

            ushortFunc = m_Pc.funcGetFanOnHours;
            txtStaFanOnHour.Tag = ushortFunc;

            txtDeviceType.Tag = m_Pc.funcGetDeviceType;
            txtModelNo.Tag = m_Pc.funcGetModelNo;
            txtSerialNo.Tag = m_Pc.funcGetSerialNo;
        }

        //private void SetEnable()
        //{
        //    foreach (System.Windows.Forms.Control control in this.Controls)
        //    {
        //        if (control.GetType() == typeof(GroupBox))
        //        {
        //            foreach (System.Windows.Forms.Control con in control.Controls)
        //            {
        //                Type conType = con.GetType();
        //                if (conType == typeof(TextBox) || conType == typeof(CheckBox))
        //                {
        //                    object tag = con.Tag;
        //                    if (con.Tag != null)
        //                    {
        //                        Type tagType = tag.GetType();
        //                        //if (tagType == typeof(Function<string>))
        //                        //{
        //                        //    con.Enabled = (tag as Function<string>).MonitorEnable;
        //                        //}
        //                        if (tagType == typeof(Function<sbyte>))
        //                        {
        //                            con.Enabled = (tag as Function<sbyte>).MonitorEnable;
        //                        }
        //                        else if (tagType == typeof(Function<int>))
        //                        {
        //                            con.Enabled = (tag as Function<int>).MonitorEnable;
        //                        }
        //                        else if (tagType == typeof(Function<ushort>))
        //                        {
        //                            con.Enabled = (tag as Function<ushort>).MonitorEnable;
        //                        }
        //                        else if (tagType == typeof(Function<short>))
        //                        {
        //                            con.Enabled = (tag as Function<short>).MonitorEnable;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void UpdateCurrentValues()
        {
            txtTempCpu.Text = (txtTempCpu.Tag as Function<sbyte>).Value.ToString();
            txtTempCpuBoard.Text = (txtTempCpuBoard.Tag as Function<sbyte>).Value.ToString();
            txtTempBoardIo.Text = (txtTempBoardIo.Tag as Function<sbyte>).Value.ToString();
            txtTempPower.Text = (txtTempPower.Tag as Function<sbyte>).Value.ToString();
            txtTempDrive1.Text = (txtTempDrive1.Tag as Function<sbyte>).Value.ToString();
            txtTempDrive2.Text = (txtTempDrive2.Tag as Function<sbyte>).Value.ToString();

            txtVoltageCpu.Text = (txtVoltageCpu.Tag as Function<int>).Value.ToString();
            txtVoltageDC.Text = (txtVoltageDC.Tag as Function<int>).Value.ToString();
            txtVoltageStandby.Text = (txtVoltageStandby.Tag as Function<int>).Value.ToString();
            txtVoltageBattery.Text = (txtVoltageBattery.Tag as Function<int>).Value.ToString();
            txtVoltageCpu2.Text = (txtVoltageCpu2.Tag as Function<int>).Value.ToString();
            txtVoltageDC2.Text = (txtVoltageDC2.Tag as Function<int>).Value.ToString();
            txtVoltageUPSBattery.Text = (txtVoltageUPSBattery.Tag as Function<int>).Value.ToString();

            txtFanCpu.Text = (txtFanCpu.Tag as Function<short>).Value.ToString();
            txtFan1.Text = (txtFan1.Tag as Function<short>).Value.ToString();
            txtFan2.Text = (txtFan2.Tag as Function<short>).Value.ToString();
            txtFan3.Text = (txtFan3.Tag as Function<short>).Value.ToString();

            txtStaPowerOnCycle.Text = (txtStaPowerOnCycle.Tag as Function<ushort>).Value.ToString();
            txtStaPowerOnHour.Text = (txtStaPowerOnHour.Tag as Function<ushort>).Value.ToString();
            txtStaFanOnHour.Text = (txtStaFanOnHour.Tag as Function<ushort>).Value.ToString();
            txtStatisCMOSBattery.Text = (txtStatisCMOSBattery.Tag as Function<int>).Value.ToString();

            txtDeviceType.Text = (txtDeviceType.Tag as Function<string>).Value;
            txtModelNo.Text = (txtModelNo.Tag as Function<string>).Value;
            txtSerialNo.Text = (txtSerialNo.Tag as Function<string>).Value;
        }

        private void InitData()
        {
            UpdateCurrentValues();

            Function<sbyte> sbyteFunc = txtTempCpu.Tag as Function<sbyte>;
            txtTempCpuMax.Text = sbyteFunc.Max.ToString();
            chkTempCpu.Checked = sbyteFunc.AlarmEnable;

            sbyteFunc = txtTempCpuBoard.Tag as Function<sbyte>;
            txtTempCpuBoardMax.Text = sbyteFunc.Max.ToString();
            chkTempCpuBoard.Checked = sbyteFunc.AlarmEnable;

            sbyteFunc = txtTempBoardIo.Tag as Function<sbyte>;
            txtTempBoardIoMax.Text = sbyteFunc.Max.ToString();
            chkTempBoardIo.Checked = sbyteFunc.AlarmEnable;

            sbyteFunc = txtTempPower.Tag as Function<sbyte>;
            txtTempPowerMax.Text = sbyteFunc.Max.ToString();
            chkTempPower.Checked = sbyteFunc.AlarmEnable;

            sbyteFunc = txtTempDrive1.Tag as Function<sbyte>;
            txtTempDrive1Max.Text = sbyteFunc.Max.ToString();
            chkTempDrive1.Checked = sbyteFunc.AlarmEnable;

            sbyteFunc = txtTempDrive2.Tag as Function<sbyte>;
            txtTempDrive2Max.Text = sbyteFunc.Max.ToString();
            chkTempDrive2.Checked = sbyteFunc.AlarmEnable;

            Function<int> intFunc = txtVoltageCpu.Tag as Function<int>;
            txtVoltageCpuMax.Text = intFunc.Max.ToString();
            chkVoltageCpu.Checked = intFunc.AlarmEnable;
            txtVoltageCpuMin.Text = intFunc.Min.ToString();

            intFunc = txtVoltageDC.Tag as Function<int>;
            txtVoltageDCMax.Text = intFunc.Max.ToString();
            chkVoltageDC.Checked = intFunc.AlarmEnable;
            txtVoltageDCMin.Text = intFunc.Min.ToString();

            intFunc = txtVoltageStandby.Tag as Function<int>;
            txtVoltageStandbyMax.Text = intFunc.Max.ToString();
            chkVoltageStandby.Checked = intFunc.AlarmEnable;
            txtVoltageStandbyMin.Text = intFunc.Min.ToString();

            intFunc = txtVoltageBattery.Tag as Function<int>;
            txtVoltageBatteryMax.Text = intFunc.Max.ToString();
            chkVoltageBattery.Checked = intFunc.AlarmEnable;
            txtVoltageBatteryMin.Text = intFunc.Min.ToString();

            intFunc = txtVoltageCpu2.Tag as Function<int>;
            txtVoltageCpu2Max.Text = intFunc.Max.ToString();
            chkVoltageCpu2.Checked = intFunc.AlarmEnable;
            txtVoltageCpu2Min.Text = intFunc.Min.ToString();

            intFunc = txtVoltageDC2.Tag as Function<int>;
            txtVoltageDC2Max.Text = intFunc.Max.ToString();
            chkVoltageDC2.Checked = intFunc.AlarmEnable;
            txtVoltageDC2Min.Text = intFunc.Min.ToString();

            intFunc = txtVoltageUPSBattery.Tag as Function<int>;
            txtVoltageUPSBatteryMax.Text = intFunc.Max.ToString();
            chkVoltageUPSBattery.Checked = intFunc.AlarmEnable;
            txtVoltageUPSBatteryMin.Text = intFunc.Min.ToString();

            Function<short> shortFunc = txtFanCpu.Tag as Function<short>;
            txtFanCpuMin.Text = shortFunc.Min.ToString();
            chkFanCpu.Checked = shortFunc.AlarmEnable;

            shortFunc = txtFan1.Tag as Function<short>;
            txtFan1Min.Text = shortFunc.Min.ToString();
            chkFan1.Checked = shortFunc.AlarmEnable;

            shortFunc = txtFan2.Tag as Function<short>;
            txtFan2Min.Text = shortFunc.Min.ToString();
            chkFan2.Checked = shortFunc.AlarmEnable;

            shortFunc = txtFan3.Tag as Function<short>;
            txtFan3Min.Text = shortFunc.Min.ToString();
            chkFan3.Checked = shortFunc.AlarmEnable;

            intFunc = txtStatisCMOSBattery.Tag as Function<int>;
            chkStatisCMOSBattery.Checked = intFunc.AlarmEnable;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateCurrentValues();
        }
       
        //private void textBox_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox txtBox = sender as TextBox;
        //    object tag = txtBox.Tag;
        //    Type tagType = tag.GetType();
        //    string text = txtBox.Text;
        //    if (tagType == typeof(Function<string>))
        //    {
        //        (tag as Function<string>).InterlockValue = text;
        //        m_Pc.Save<string>((txtBox.Tag as Function<string>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<sbyte>))
        //    {
        //        (tag as Function<sbyte>).InterlockValue = Convert.ToSByte(text);
        //        m_Pc.Save<sbyte>((txtBox.Tag as Function<sbyte>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<int>))
        //    {
        //        (tag as Function<int>).InterlockValue = Convert.ToInt32(text);
        //        m_Pc.Save<int>((txtBox.Tag as Function<int>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<ushort>))
        //    {
        //        (tag as Function<ushort>).InterlockValue = Convert.ToUInt16(text);
        //        m_Pc.Save<ushort>((txtBox.Tag as Function<ushort>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<short>))
        //    {
        //        (tag as Function<short>).InterlockValue = Convert.ToInt16(text);
        //        m_Pc.Save<short>((txtBox.Tag as Function<short>), GeneralPc.entryInterlockValue);
        //    }
        //}

        //private void checkBox_CheckStateChanged(object sender, EventArgs e)
        //{
        //    CheckBox chkBox = sender as CheckBox;
        //    object tag = chkBox.Tag;
        //    Type tagType = tag.GetType();
        //    bool state = (chkBox.CheckState == CheckState.Checked);
        //    if (tagType == typeof(Function<string>))
        //    {
        //        (tag as Function<string>).AlarmEnable = state;
        //        m_Pc.Save<string>((chkBox.Tag as Function<string>), GeneralPc.entryAlarmEnable);
        //    }
        //    else if (tagType == typeof(Function<sbyte>))
        //    {
        //        (tag as Function<sbyte>).AlarmEnable = state;
        //        m_Pc.Save<sbyte>((chkBox.Tag as Function<sbyte>), GeneralPc.entryAlarmEnable);
        //    }
        //    else if (tagType == typeof(Function<int>))
        //    {
        //        (tag as Function<int>).AlarmEnable = state;
        //        m_Pc.Save<int>((chkBox.Tag as Function<int>), GeneralPc.entryAlarmEnable);
        //    }
        //    else if (tagType == typeof(Function<ushort>))
        //    {
        //        (tag as Function<ushort>).AlarmEnable = state;
        //        m_Pc.Save<ushort>((chkBox.Tag as Function<ushort>), GeneralPc.entryAlarmEnable);
        //    }
        //    else if (tagType == typeof(Function<short>))
        //    {
        //        (tag as Function<short>).AlarmEnable = state;
        //        m_Pc.Save<short>((chkBox.Tag as Function<short>), GeneralPc.entryAlarmEnable);
        //    }
        //}
    }
}

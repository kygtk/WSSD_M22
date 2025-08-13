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
    public partial class ViewPcSystemGeneral : UserControl
    {
        private GeneralPc m_Pc = null;

        public ViewPcSystemGeneral()
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
            m_Pc = pcSystemInfo as GeneralPc;

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
                                    else if(refTagText == _PcSystemInfo.entryMin) (tag as Function<string>).Min = text;
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

            //Function<short> shortFunc = m_Pc.funcGetCpuFan;

            //txtFanCpu.Tag = shortFunc;
            //txtFanCpuMax.Tag = shortFunc;
            //chkFanCpu.Tag = shortFunc;

            txtDeviceType.Tag = m_Pc.funcGetDeviceType;
            txtModelNo.Tag = m_Pc.funcGetModelNo;
            txtSerialNo.Tag = m_Pc.funcGetSerialNo;
            txtModelNo.Tag = m_Pc.funcGetModelNo;
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

            //txtFanCpu.Text = (txtFanCpu.Tag as Function<short>).Value.ToString();

            txtDeviceType.Text = (txtDeviceType.Tag as Function<string>).Value;
            txtModelNo.Text = (txtModelNo.Tag as Function<string>).Value;
            txtSerialNo.Text = (txtSerialNo.Tag as Function<string>).Value;
            txtModelNo.Text = (txtModelNo.Tag as Function<string>).Value;
        }

        private void InitData()
        {
            UpdateCurrentValues();

            Function<sbyte> sbyteFunc = txtTempCpu.Tag as Function<sbyte>;
            txtTempCpuMax.Text = sbyteFunc.Max.ToString();
            chkTempCpu.Checked = sbyteFunc.AlarmEnable;

            //Function<short> shortFunc = txtFanCpu.Tag as Function<short>;
            //txtFanCpuMax.Text = shortFunc.InterlockValue.ToString();
            //chkFanCpu.Checked = shortFunc.AlarmEnable;
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
        //        //m_Pc.Save<string>((txtBox.Tag as Function<string>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<sbyte>))
        //    {
        //        (tag as Function<sbyte>).InterlockValue = Convert.ToSByte(text);
        //        //m_Pc.Save<sbyte>((txtBox.Tag as Function<sbyte>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<int>))
        //    {
        //        (tag as Function<int>).InterlockValue = Convert.ToInt32(text);
        //        //m_Pc.Save<int>((txtBox.Tag as Function<int>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<ushort>))
        //    {
        //        (tag as Function<ushort>).InterlockValue = Convert.ToUInt16(text);
        //        //m_Pc.Save<ushort>((txtBox.Tag as Function<ushort>), GeneralPc.entryInterlockValue);
        //    }
        //    else if (tagType == typeof(Function<short>))
        //    {
        //        (tag as Function<short>).InterlockValue = Convert.ToInt16(text);
        //        //m_Pc.Save<short>((txtBox.Tag as Function<short>), GeneralPc.entryInterlockValue);
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

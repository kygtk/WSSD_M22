using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Mitsubishi;
using System.Collections;
using System.Reflection;
using Dms.DeviceLibrary;
using Dms.ModbusDevices;
using Dms.Common;

namespace Dms.Control
{
    public partial class ViewModbus : UserControl
    {
        private ArrayList m_List = new ArrayList();
        private Color onColor = Color.GreenYellow;
        private Color offColor = Color.White;
        private Type m_Type;
        private bool m_UseIoCheck = false;

        #region Properties
        [Category("DMS : UI")]
        public string TitleName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }
        [Category("DMS : Setting"),
        Description("Set true if you want to use this view for IO Check.")]
        public bool UseIoCheck
        {
            get { return m_UseIoCheck; }
            set { m_UseIoCheck = value; }
        }
        #endregion

        public ViewModbus()
        {
            InitializeComponent();

        }

        public void Initialize(DmsNode node, Type type)
        {
            m_List.Clear();
            m_Type = type;
            GetCollection(node.GetNode(), ref m_List, m_Type);
            InitData();

        }

        public void SetMonitorTimer(bool enable)
        {
            this.tmrUpdateState.Enabled = enable;
        }

        private void InitData()
        {
            int rowCount = 0;
            string name = "";

            int cnt = this.dataGridView.RowCount;

            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    this.dataGridView.Rows.RemoveAt(0);
                }
            }

            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            if (m_Type == typeof(ModbusBitInput) || m_Type == typeof(ModbusBitOutput))
            {
                dataGridView.ColumnCount = 2;

                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                dataGridView.ColumnCount = 4;

                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            dataGridView.ColumnHeadersVisible = false;

            int listCount = m_List.Count;
            for (int i = 0; i < listCount; i++)
            {
                rowCount += ((ModbusDevice)m_List[i]).Size;
            }

            
            dataGridView.RowCount = rowCount;
            ModbusDevice modDevice;
            if (listCount == rowCount)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    modDevice = (ModbusDevice)m_List[i];

                    if (modDevice.StartAddress.Contains("_"))
                    {
                        dataGridView[0, i].Value = modDevice.StartAddress;
                        dataGridView[1, i].Value = "Not Defined";
                    }
                    else
                    {
                        dataGridView[0, i].Value = modDevice.StartAddress;
                        name = modDevice.Name;
                        dataGridView[1, i].Value = name.Contains(":") ? name.Substring(name.IndexOf(':') + 5) : name.Substring(3);
                    }
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < listCount; i++)
                {
                    modDevice = (ModbusDevice)m_List[i];

                    int size = modDevice.Size;
                    for (int j = 0; j < size; j++)
                    {
                        if (modDevice.StartAddress.Contains("_"))
                        {
                            dataGridView[0, count].Value = "0X____";
                            dataGridView[1, count++].Value = "Not Defined";
                        }
                        else
                        {
                            dataGridView[0, count].Value = string.Format("0X{0:X4}", (Convert.ToInt32(modDevice.StartAddress, 16) + j));
                            name = modDevice.Name;
                            dataGridView[1, count++].Value = name.Contains(":") ? name.Substring(name.IndexOf(':') + 5) : name.Substring(3);
                        }
                    }
                }
            }

            tmrUpdateState.Enabled = true;
            UpdateState();
            dataGridView.ClearSelection();
        }

        private void UpdateState()
        {
            string value = "";
            int count = 0;

            //if (m_Type == typeof(MelsecBitInput) || m_Type == typeof(MelsecBitOutput))
            //{
            //    int rowCount = dataGridView.RowCount;
            //    for (int i = 0; i < rowCount; i++)
            //    {
            //        if (m_Type == typeof(MelsecBitInput))
            //        {
            //            value = ((MelsecBitInput)m_List[i]).GetStatus().ToString();
            //        }
            //        else value = ((MelsecBitOutput)m_List[i]).GetStatus().ToString();

            //        //if (value == bool.TrueString) dataGridView[2, i].Style.BackColor = onColor;
            //        //else dataGridView[2, i].Style.BackColor = offColor;

            //        if (value == bool.TrueString) dataGridView[1, i].Style.BackColor = onColor;
            //        else dataGridView[1, i].Style.BackColor = offColor;
            //    }
            //}
            //else
            //{
            //    short[] temp;
            //    int listCount = m_List.Count;
            //    for (int i = 0; i < listCount; i++)
            //    {
            //        if (m_Type == typeof(MelsecWordInput))
            //        {
            //            temp = ((MelsecWordInput)m_List[i]).GetValues();
            //        }
            //        else temp = ((MelsecWordOutput)m_List[i]).GetValues();

            //        int size = ((MelsecDevice)m_List[i]).Size;
            //        for (int j = 0; j < size; j++)
            //        {
            //            dataGridView[2, count++].Value = temp[j].ToString();
            //        }
            //    }
            //}
            if (m_Type == typeof(ModbusBitInput) || m_Type == typeof(ModbusBitOutput))
            {
                int rowCount = dataGridView.RowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    if (m_Type == typeof(ModbusBitInput))
                    {
                        if (((ModbusBitInput)m_List[i]).StartAddress.Contains("_") == false)
                        {
                            value = ((ModbusBitInput)m_List[i]).GetStatus().ToString();
                        }
                    }
                    else
                    {
                        if (((ModbusBitOutput)m_List[i]).StartAddress.Contains("_") == false)
                        {
                            value = ((ModbusBitOutput)m_List[i]).GetStatus().ToString();
                        }
                    }

                    //if (value == bool.TrueString) dataGridView[2, i].Style.BackColor = onColor;
                    //else dataGridView[2, i].Style.BackColor = offColor;

                    if (value == bool.TrueString) dataGridView[1, i].Style.BackColor = onColor;
                    else dataGridView[1, i].Style.BackColor = offColor;
                }
            }
            else
            {
                short[] temp;
                int listCount = m_List.Count;
                for (int i = 0; i < listCount; i++)
                {
                    if (m_Type == typeof(ModbusWordInput))
                    {
                        if (((ModbusWordInput)m_List[i]).StartAddress.Contains("_") == false)
                        {
                            temp = ((ModbusWordInput)m_List[i]).GetValues();
                        }
                        else
                        {
                            int length = ((ModbusWordInput)m_List[i]).Size;
                            temp = new short[length];
                            for (int j = 0; j < length; j++)
                            {
                                temp[j] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (((ModbusWordOutput)m_List[i]).StartAddress.Contains("_") == false)
                        {
                            temp = ((ModbusWordOutput)m_List[i]).GetValues();
                        }
                        else
                        {
                            int length = ((ModbusWordOutput)m_List[i]).Size;
                            temp = new short[length];
                            for (int j = 0; j < length; j++)
                            {
                                temp[j] = 0;
                            }
                        }
                    }

                    int size = ((ModbusDevice)m_List[i]).Size;
                    for (int j = 0; j < size; j++)
                    {
                        string val = string.Format("{0:X4}", temp[j]);
                        dataGridView[2, count].Value = val;
                        dataGridView[3, count++].Value = XFunc.ConvertToString(temp[j], ByteOrder.BigEndian);
                    }
                }
            }
        }

        private void GetCollection(TreeNode currentNode, ref ArrayList items, Type type)
        {
            object currentObject = currentNode.Tag;

            if (currentObject.GetType() == type)
            {
                items.Add(currentObject);
            }

            foreach (TreeNode childNode in currentNode.Nodes)
            {
                GetCollection(childNode, ref items, type);
            }

            PropertyInfo[] propertyInfos = currentObject.GetType().GetProperties();
            foreach (PropertyInfo info in propertyInfos)
            {
                if (type == info.PropertyType)
                {
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    ModbusDevice mod = getMethodInfo.Invoke(currentObject, null) as ModbusDevice;

                    ////if (m_Melsec.SimulateAddress)
                    ////{
                    ////    mel.Name = (currentObject as DmsNode).GetOriginalName() + " : " + info.Name;
                    ////}

                    items.Add(mod);
                }
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void ViewMelsec_Load(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
            
            if (m_UseIoCheck)
            {
                this.dataGridView.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(IoCheck);
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }

        private void IoCheck(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;

            if (row < 0) return;

            if (m_Type == typeof(ModbusBitOutput))
            {
                bool status = !((ModbusBitOutput)m_List[row]).GetStatus();
                ((ModbusBitOutput)m_List[row]).SetStatus(status);
            }
            else if (m_Type == typeof(ModbusBitInput))
            {
                bool status = !((ModbusBitInput)m_List[row]).GetStatus();
                ((ModbusBitInput)m_List[row]).SetStatus(status);
            }
        }
    }
}

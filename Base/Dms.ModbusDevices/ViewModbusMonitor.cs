using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.DeviceLibrary;
using Microsoft.VisualBasic;
using System.Reflection;
using Dms.Ctl;

namespace Dms.ModbusDevices
{
    public partial class ViewModbusMonitor : UserControl
    {
        private bool m_Initialized = false;
        private List<ModbusBitInput> m_ModbusBitInputs = null;
        private List<ModbusBitOutput> m_ModbusBitOutputs = null;
        private List<ModbusWordInput> m_ModbusWordInputs = null;
        private List<ModbusWordOutput> m_ModbusWordOutputs = null;
        //private Melsec m_Melsec = null;
        private ModbusCommDevice m_Modbus = null;
        //private List<int> WordViewIndexAddress = null;
        //private int m_SelectedWordIndex = -1;
        
        public Font BoldFont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        public Font NormalFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        public Color OnColor = Color.Red;
        public Color OffColor = Color.Silver;

        #region Constructor
        public ViewModbusMonitor()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }        
        #endregion

        //RootNode를 참조로 받아야 한다.
        public void Initialize(DmsNode rootNode)
        {
            // List를 초기화 합니다.
            m_ModbusBitInputs = new List<ModbusBitInput>();
            m_ModbusBitOutputs = new List<ModbusBitOutput>();
            m_ModbusWordInputs = new List<ModbusWordInput>();
            m_ModbusWordOutputs = new List<ModbusWordOutput>();
            
            // RootNode에서 Mesec Driver를 가져와서 Set 한다.
            bool find = false;
            TreeNode rootNodes = rootNode.GetNode();
            foreach (TreeNode node in rootNodes.Nodes)
            {
                if ((node.Tag.GetType() == typeof(ModbusCommDevice)))
                {
                    m_Modbus = node.Tag as ModbusCommDevice;
                    find = true;
                    break;
                }
            }

            // Melsec Driver instance가 있으면 OK
            m_Initialized = find;
            if (m_Initialized)
            {
                // Node에 등록된 객체들 중에서 melsec device를 읽어와서 device 정보를 확보한다.
                // MelsecBitInput
                ArrayList items1 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items1, typeof(ModbusBitInput));
                foreach (ModbusBitInput modbusDevice in items1)
                {
                    if( modbusDevice.Name != "None" ) m_ModbusBitInputs.Add(modbusDevice);
                }
                // MelsecBitOutput
                ArrayList items2 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items2, typeof(ModbusBitOutput));
                foreach (ModbusBitOutput modbusDevice in items2)
                {
                    if (modbusDevice.Name != "None") m_ModbusBitOutputs.Add(modbusDevice);
                }
                // MelsecWordInput
                ArrayList items3 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items3, typeof(ModbusWordInput));
                foreach (ModbusWordInput modbusDevice in items3)
                {
                    if (modbusDevice.Name != "None") m_ModbusWordInputs.Add(modbusDevice);
                }
                // MelsecWordOutput
                ArrayList items4 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items4, typeof(ModbusWordOutput));
                foreach (ModbusWordOutput modbusDevice in items4)
                {
                    if (modbusDevice.Name != "None") m_ModbusWordOutputs.Add(modbusDevice);
                }

                // GridView를 초기화 한다.
                InitUsedBitGridView();
                InitUsedWordGridView();
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
/*                    
                    if(m_Melsec.SimulateAddress)
                    {
                        mel.Name = (currentObject as DmsNode).GetOriginalName() + " : " + info.Name;
                    }
*/
                    items.Add(mod);
                }
            }
        }

        private void ViewModbus_Load(object sender, EventArgs e)
        {
            this.tmMonitor.Enabled = m_Initialized;
        }

        private void InitUsedBitGridView()
        {
            if (!m_Initialized) return;
            
            UsedBitDisplay();
        }

        private void UsedBitDisplay()
        {
            //Set gridview style
            dataGridViewUsedBit.Columns.Clear();
            dataGridViewUsedBit.AllowUserToAddRows = false;
            dataGridViewUsedBit.AllowUserToDeleteRows = false;
            dataGridViewUsedBit.AllowUserToResizeRows = false;
            dataGridViewUsedBit.MultiSelect = false;
            dataGridViewUsedBit.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewUsedBit.RowHeadersVisible = false;
            DataGridViewCellStyle cellStyle = dataGridViewUsedBit.RowsDefaultCellStyle;
            cellStyle.Font = NormalFont;
            
            int inputSize = m_ModbusBitInputs.Count;
            int outputSize = m_ModbusBitOutputs.Count;
            int maxColumnItems = 3;
            int maxColumns = maxColumnItems * 2; // input + output
            int maxRows = ((inputSize > outputSize) ? inputSize : outputSize);

            dataGridViewUsedBit.ColumnCount = maxColumns;
            dataGridViewUsedBit.RowCount = maxRows;

            // Set the column header names.
            for (int i = 0; i < maxColumns; i += maxColumnItems)
            {
                dataGridViewUsedBit.Columns[i].Name = "Address";
                dataGridViewUsedBit.Columns[i].Width = 60;
                dataGridViewUsedBit.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridViewUsedBit.Columns[i + 1].Name = "Group";
                dataGridViewUsedBit.Columns[i + 1].Width = 100;
                dataGridViewUsedBit.Columns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedBit.Columns[i + 1].ReadOnly = true;

                dataGridViewUsedBit.Columns[i + 2].Name = "Name";
                dataGridViewUsedBit.Columns[i + 2].Width = 200;
                dataGridViewUsedBit.Columns[i + 2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedBit.Columns[i + 2].ReadOnly = true;
            }

            for (int i = 0; i < maxRows; i++)
            {
                dataGridViewUsedBit.Rows[i].Height = 25;
                //dataGridViewWord.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
            }


            //string oldGroupName = "";
            string groupName = "";
            Color cellColor = Color.WhiteSmoke;
            ModbusBitInput modbusInput = null;
            for (int i = 0; i < inputSize; i++)
            {
                modbusInput = m_ModbusBitInputs[i];

                //groupName = mel.GetParentName();
                groupName = modbusInput.GetType().Name;
                //if (oldGroupName != groupName)
                //{
                //    oldGroupName = groupName;
                //    if (cellColor != Color.White) cellColor = Color.White;
                //    else cellColor = Color.WhiteSmoke;
                //}

                // bit Address column
                DataGridViewButtonCell button = new DataGridViewButtonCell();
                button.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                button.Style.BackColor = OffColor;
                button.Style.Font = NormalFont;

                dataGridViewUsedBit[0, i] = button;
                //dataGridViewUsedBit.Rows[i].Cells[0].Value = m_Melsec.GetAddress(modbusInput.DevType, modbusInput.GetStartAddress());
                dataGridViewUsedBit.Rows[i].Cells[0].Value = modbusInput.StartAddress;

                // bit Group column
                dataGridViewUsedBit.Rows[i].Cells[1].Value = groupName;
                dataGridViewUsedBit.Rows[i].Cells[1].Style.BackColor = cellColor;

                // bit Name
                dataGridViewUsedBit.Rows[i].Cells[2].Value = modbusInput.Name;//mel.GetOriginalName();
                dataGridViewUsedBit.Rows[i].Cells[2].Style.BackColor = cellColor;
            }

            cellColor = Color.White;
            ModbusBitOutput modbusOutput = null;
            for (int i = 0; i < outputSize; i++)
            {
                modbusOutput = m_ModbusBitOutputs[i];

                //groupName = mel.GetParentName();
                groupName = modbusOutput.GetType().Name;
                //if (oldGroupName != groupName)
                //{
                //    oldGroupName = groupName;
                //    if (cellColor != Color.White) cellColor = Color.White;
                //    else cellColor = Color.WhiteSmoke;
                //}
                
                // bit Address column
                DataGridViewButtonCell button = new DataGridViewButtonCell();
                button.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                button.Style.BackColor = OffColor;
                button.Style.Font = NormalFont;

                dataGridViewUsedBit[maxColumnItems + 0, i] = button;
                //dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 0].Value = m_Melsec.GetAddress(modbusOutput.DevType, modbusOutput.GetStartAddress());
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 0].Value = modbusOutput.StartAddress;

                // bit Group column
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 1].Value = groupName;
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 1].Style.BackColor = cellColor;
                
                // bit Name
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 2].Value = modbusOutput.Name;//mel.GetOriginalName();
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 2].Style.BackColor = cellColor;
            }

            dataGridViewUsedBit.ClearSelection();
        }

        private void InitUsedWordGridView()
        {
            if (!m_Initialized) return;
            
            UsedWordDisplay();
        }

        private void UsedWordDisplay()
        {
            //Set gridview style
            //dataGridViewWord.Columns.Clear();
            dataGridViewUsedWord.AllowUserToAddRows = false;
            dataGridViewUsedWord.AllowUserToDeleteRows = false;
            dataGridViewUsedWord.AllowUserToResizeRows = false;
            dataGridViewUsedWord.MultiSelect = false;
            dataGridViewUsedWord.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewUsedWord.RowHeadersVisible = false;
            DataGridViewCellStyle rowCellStyle = dataGridViewUsedWord.RowsDefaultCellStyle;
            rowCellStyle.Font = NormalFont;

            int inputSize = m_ModbusWordInputs.Count;
            int outputSize = m_ModbusWordOutputs.Count;
            int maxColumnItems = 7;
            int maxColumns = maxColumnItems;
            int maxRows = inputSize + outputSize;

            dataGridViewUsedWord.ColumnCount = maxColumns;
            dataGridViewUsedWord.RowCount = maxRows;
            
            // Set the column header names.
            for (int i = 0; i < maxColumns; i += maxColumnItems)
            {
                dataGridViewUsedWord.Columns[i].Name = "Address";
                dataGridViewUsedWord.Columns[i].Width = 60;
                dataGridViewUsedWord.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i].ReadOnly = true;
                DataGridViewCellStyle cellStyle = dataGridViewUsedWord.Columns[i].DefaultCellStyle;
                //cellStyle.Font = BoldFont;
                cellStyle.BackColor = Color.LightSteelBlue;
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridViewUsedWord.Columns[i + 1].Name = "Group";
                dataGridViewUsedWord.Columns[i + 1].Width = 150;
                dataGridViewUsedWord.Columns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 1].ReadOnly = true;

                dataGridViewUsedWord.Columns[i + 2].Name = "Name";
                dataGridViewUsedWord.Columns[i + 2].Width = 200;
                dataGridViewUsedWord.Columns[i + 2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 2].ReadOnly = true;

                dataGridViewUsedWord.Columns[i + 3].Name = "Decimal";
                dataGridViewUsedWord.Columns[i + 3].Width = 60;
                dataGridViewUsedWord.Columns[i + 3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 3].ReadOnly = true;

                dataGridViewUsedWord.Columns[i + 4].Name = "Ascii";
                dataGridViewUsedWord.Columns[i + 4].Width = 40;
                dataGridViewUsedWord.Columns[i + 4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 4].ReadOnly = true;

                dataGridViewUsedWord.Columns[i + 5].Name = "Hex";
                dataGridViewUsedWord.Columns[i + 5].Width = 40;
                dataGridViewUsedWord.Columns[i + 5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 5].ReadOnly = true;

                dataGridViewUsedWord.Columns[i + 6].Name = "Binary";
                dataGridViewUsedWord.Columns[i + 6].Width = 120;
                dataGridViewUsedWord.Columns[i + 6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewUsedWord.Columns[i + 6].ReadOnly = true;
            }

            for (int i = 0; i < maxRows; i++)
            {
                dataGridViewUsedWord.Rows[i].Height = 25;
            }

            string oldGroupName = "";
            string groupName = "";
            Color cellColor = Color.White;

            ModbusDevice modDev = null;
            for (int i = 0; i < maxRows; i++)
            {
                if (i < inputSize) modDev = m_ModbusWordInputs[i];
                else modDev = m_ModbusWordOutputs[i - inputSize];

                //groupName = mel.GetParentName();
                groupName = modDev.GetType().Name;
                if (oldGroupName != groupName)
                {
                    oldGroupName = groupName;
                    if (cellColor != Color.White) cellColor = Color.White;
                    else cellColor = Color.WhiteSmoke;
                }

                // 화면에 표시
                dataGridViewUsedWord.Rows[i].Cells[0].Tag = modDev;
                //dataGridViewUsedWord.Rows[i].Cells[0].Value = m_Melsec.GetAddress(modDev.DevType, modDev.GetStartAddress());
                dataGridViewUsedWord.Rows[i].Cells[0].Value = modDev.StartAddress;
                dataGridViewUsedWord.Rows[i].Cells[1].Value = groupName;
                dataGridViewUsedWord.Rows[i].Cells[1].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[2].Value = modDev.Name;//mel.GetOriginalName();
                dataGridViewUsedWord.Rows[i].Cells[2].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[3].Value = "0";
                dataGridViewUsedWord.Rows[i].Cells[3].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[4].Value = "";
                dataGridViewUsedWord.Rows[i].Cells[4].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[5].Value = "0000";
                dataGridViewUsedWord.Rows[i].Cells[5].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[6].Value = "0000000000000000";
                dataGridViewUsedWord.Rows[i].Cells[6].Style.BackColor = cellColor;
            }

            dataGridViewUsedWord.ClearSelection();        
        }

        private void tmMonitor_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                UpdateUsedBitView();
                UpdateUsedWordView();
           }
        }

        private void UpdateUsedBitView()
        {
            if (this.tabControl1.SelectedTab != this.tabPageUsedBits) return;

            bool bValue;
            DataGridViewButtonCell cell = null;

            for (int i = 0; i < dataGridViewUsedBit.Columns.Count; i++)
            {
                if (dataGridViewUsedBit.Columns[i].Name == "Address" )
                {
                    for (int n = 0; n < dataGridViewUsedBit.Rows.Count; n++)
                    {
                        cell = dataGridViewUsedBit[i, n] as DataGridViewButtonCell;
                        if (cell != null)
                        {
                            // Bit Input
                            if (i == 0) bValue = m_ModbusBitInputs[n].GetStatus();
                            // Bit Output
                            else bValue = m_ModbusBitOutputs[n].GetStatus();

                            if (bValue && cell.Style.BackColor != OnColor)
                            {
                                cell.Style.BackColor = OnColor;
                                cell.Style.Font = BoldFont;
                            }
                            else if (!bValue && cell.Style.BackColor != OffColor)
                            {
                                cell.Style.BackColor = OffColor;
                                cell.Style.Font = NormalFont;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateUsedWordView()
        {
            if (this.tabControl1.SelectedTab != this.tabPageUsedWords) return;

            tmMonitor.Enabled = false;

            int maxRows = dataGridViewUsedWord.RowCount;
            int maxColumnItems = dataGridViewUsedWord.ColumnCount;
            int inputSize = m_ModbusBitInputs.Count;
            
            short nValue;
            string sValue;
            ModbusDevice modDev;
            for (int i = 0; i < maxRows; i++)
            {
                modDev = dataGridViewUsedWord[0, i].Tag as ModbusDevice;
                //nValue = m_Melsec.ReceiveWord(modDev.GetStartAddress());
                nValue = (short)m_Modbus.GetInputWord(modDev.UnitID, modDev.GetStartAddress());
                if (nValue == -1) sValue = "65535";
                else sValue = nValue.ToString();

                //dataGridViewUsedWord[3,i].Value = sValue;
                //dataGridViewUsedWord[4,i].Value = m_Melsec.DecToAsc(sValue);
                //dataGridViewUsedWord[5,i].Value = m_Melsec.DecToHex(sValue);
                //dataGridViewUsedWord[6,i].Value = m_Melsec.DecToBin(sValue);
            }

            tmMonitor.Enabled = true;
        }

        private void dataGridViewUsedBit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;
            int nCol = e.ColumnIndex;
            int maxColumnItems = dataGridViewUsedBit.ColumnCount / 2;
            
            if (nRow < 0 || nCol < 0) return;

            // Address 영역이 아니면 Noop
            if (nCol % maxColumnItems != 0) return;

            // Bit Input
            if (nCol == 0)
            {
                bool state = m_ModbusBitInputs[nRow].GetStatus();
                m_ModbusBitInputs[nRow].SetStatus(!state);
            }
            // Bit Output
            else
            {
                bool state = m_ModbusBitOutputs[nRow].GetStatus();
                m_ModbusBitOutputs[nRow].SetStatus(!state);
            }

            dataGridViewUsedBit.ClearSelection();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.Ctl;
using Dms.DeviceLibrary;
using Microsoft.VisualBasic;
using System.Reflection;
using Dms.Common;

namespace Dms.Mitsubishi
{
    public partial class ViewMelsecNetMonitor : UserControl
    {
        private bool m_Initialized = false;
        private List<MelsecBitInput> m_MelsecBitInputs = null;
        private List<MelsecBitOutput> m_MelsecBitOutputs = null;
        private List<MelsecWordInput> m_MelsecWordInputs = null;
        private List<MelsecWordOutput> m_MelsecWordOutputs = null;
        private Melsec m_Melsec = null;
        private List<int> WordViewIndexAddress = null;
        private int m_SelectedWordIndex = -1;
        
        public Font BoldFont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        public Font NormalFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        public Color OnColor = Color.Red;
        public Color OffColor = Color.Silver;

        #region Constructor
        public ViewMelsecNetMonitor()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }        
        #endregion

        public void SetMonitorTimer(bool enable)
        {
            this.tmMonitor.Enabled = enable & m_Initialized;
        }

        //RootNode를 참조로 받아야 한다.
        public void Initialize(DmsNode rootNode)
        {
            // List를 초기화 합니다.
            m_MelsecBitInputs = new List<MelsecBitInput>();
            m_MelsecBitOutputs = new List<MelsecBitOutput>();
            m_MelsecWordInputs = new List<MelsecWordInput>();
            m_MelsecWordOutputs = new List<MelsecWordOutput>();
            
            // RootNode에서 Mesec Driver를 가져와서 Set 한다.
            bool find = false;
            TreeNode rootNodes = rootNode.GetNode();
            foreach (TreeNode node in rootNodes.Nodes)
            {
                if ((node.Tag.GetType() == typeof(Melsec)))
                {
                    m_Melsec = node.Tag as Melsec;
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
                GetCollection(rootNode.GetNode(), ref items1, typeof(MelsecBitInput));
                foreach (MelsecBitInput melDevice in items1)
                {
                    if( melDevice.Name != "None" ) m_MelsecBitInputs.Add(melDevice);
                }
                // MelsecBitOutput
                ArrayList items2 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items2, typeof(MelsecBitOutput));
                foreach (MelsecBitOutput melDevice in items2)
                {
                    if (melDevice.Name != "None") m_MelsecBitOutputs.Add(melDevice);
                }
                // MelsecWordInput
                ArrayList items3 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items3, typeof(MelsecWordInput));
                foreach (MelsecWordInput melDevice in items3)
                {
                    if (melDevice.Name != "None") m_MelsecWordInputs.Add(melDevice);
                }
                // MelsecWordOutput
                ArrayList items4 = new ArrayList();
                GetCollection(rootNode.GetNode(), ref items4, typeof(MelsecWordOutput));
                foreach (MelsecWordOutput melDevice in items4)
                {
                    if (melDevice.Name != "None") m_MelsecWordOutputs.Add(melDevice);
                }

                // GridView를 초기화 한다.
                InitUsedBitGridView();
                InitUsedWordGridView();
                InitWordGridView();
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
                    MelsecDevice mel = getMethodInfo.Invoke(currentObject, null) as MelsecDevice;
                    
                    if(m_Melsec.SimulateAddress)
                    {
                        mel.Name = (currentObject as DmsNode).GetOriginalName() + " : " + info.Name;
                    }

                    items.Add(mel);
                }
            }
        }

        private void ViewMelsecNet_Load(object sender, EventArgs e)
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
            
            int inputSize = m_MelsecBitInputs.Count;
            int outputSize = m_MelsecBitOutputs.Count;
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
            MelsecBitInput melInput = null;
            for (int i = 0; i < inputSize; i++)
            {
                melInput = m_MelsecBitInputs[i];

                //groupName = mel.GetParentName();
                groupName = melInput.GetType().Name;
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
                dataGridViewUsedBit.Rows[i].Cells[0].Value = m_Melsec.GetAddress(melInput.DevType, melInput.GetStartAddress());

                // bit Group column
                dataGridViewUsedBit.Rows[i].Cells[1].Value = groupName;
                dataGridViewUsedBit.Rows[i].Cells[1].Style.BackColor = cellColor;

                // bit Name
                dataGridViewUsedBit.Rows[i].Cells[2].Value = melInput.Name;//mel.GetOriginalName();
                dataGridViewUsedBit.Rows[i].Cells[2].Style.BackColor = cellColor;
            }

            cellColor = Color.White;
            MelsecBitOutput melOutput = null;
            for (int i = 0; i < outputSize; i++)
            {
                melOutput = m_MelsecBitOutputs[i];

                //groupName = mel.GetParentName();
                groupName = melOutput.GetType().Name;
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
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 0].Value = m_Melsec.GetAddress(melOutput.DevType, melOutput.GetStartAddress());

                // bit Group column
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 1].Value = groupName;
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 1].Style.BackColor = cellColor;
                
                // bit Name
                dataGridViewUsedBit.Rows[i].Cells[maxColumnItems + 2].Value = melOutput.Name;//mel.GetOriginalName();
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

            int inputSize = m_MelsecWordInputs.Count;
            int outputSize = m_MelsecWordOutputs.Count;
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

            MelsecDevice mel = null;
            for (int i = 0; i < maxRows; i++)
            {
                if (i < inputSize) mel = m_MelsecWordInputs[i];
                else mel = m_MelsecWordOutputs[i-inputSize];

                //groupName = mel.GetParentName();
                groupName = mel.GetType().Name;
                if (oldGroupName != groupName)
                {
                    oldGroupName = groupName;
                    if (cellColor != Color.White) cellColor = Color.White;
                    else cellColor = Color.WhiteSmoke;
                }

                // 화면에 표시
                dataGridViewUsedWord.Rows[i].Cells[0].Tag = mel;
                dataGridViewUsedWord.Rows[i].Cells[0].Value = m_Melsec.GetAddress(mel.DevType, mel.GetStartAddress());
                dataGridViewUsedWord.Rows[i].Cells[1].Value = groupName;
                dataGridViewUsedWord.Rows[i].Cells[1].Style.BackColor = cellColor;
                dataGridViewUsedWord.Rows[i].Cells[2].Value = mel.Name;//mel.GetOriginalName();
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

            dataGridViewWord.ClearSelection();        
        }

        private void InitWordGridView()
        {
            if (!m_Initialized) return;

            WordViewIndexAddress = new List<int>();
            StringBuilder sb = new StringBuilder();

            int monitorCount = m_Melsec.MonitorAddress.Count;
            int cbWordViewCount = cbWordView.Items.Count;
            if (monitorCount != 0)
            {
                tagADDR_INFO addrInfo;
                for (int i = 0; i < monitorCount; i++)
                {
                    addrInfo = m_Melsec.MonitorAddress[i];
                    if (addrInfo.Type == devTYPE.devW)
                    {
                        sb.Append(m_Melsec.GetAddress(devTYPE.devW, addrInfo.StartAddress));
                        sb.Append(" - ");
                        sb.Append(m_Melsec.GetAddress(devTYPE.devW, addrInfo.StartAddress + addrInfo.Size - 1));

                        cbWordView.Items.Add(sb.ToString());
                        WordViewIndexAddress.Add(i);

                        sb.Remove(0, sb.Length);
                    }
                }

                if (cbWordViewCount > 0) cbWordView.SelectedIndex = 0;
            }

            WordDisplay();
        }

        private void WordDisplay()
        {
            // 추가된 word monitor 영역이 하나도 없으면 그냥 리턴
            if (m_SelectedWordIndex == -1) return;

            List<string> AddressList = new List<string>();
            int index = WordViewIndexAddress[m_SelectedWordIndex];
            int addressSize = m_Melsec.MonitorAddress[index].Size;
            short startAddress = m_Melsec.MonitorAddress[index].StartAddress;
            for (int j = 0; j < addressSize; j++)
            {
                AddressList.Add(m_Melsec.GetAddress(devTYPE.devW, startAddress + j));
            }
            
            //Set gridview style
            //dataGridViewWord.Columns.Clear();
            dataGridViewWord.AllowUserToAddRows = false;
            dataGridViewWord.AllowUserToDeleteRows = false;
            dataGridViewWord.AllowUserToResizeRows = false;
            dataGridViewWord.MultiSelect = false;
            dataGridViewWord.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridViewWord.RowHeadersVisible = false;
            DataGridViewCellStyle rowCellStyle = dataGridViewWord.RowsDefaultCellStyle;
            rowCellStyle.Font = NormalFont;

            int maxColumns = 0;
            int maxRows = 16;
            int maxColumnItems = 5;
            maxColumns = addressSize / maxRows + ((addressSize % maxRows) > 0 ? 1: 0);// X축의 갯수
            maxColumns *= maxColumnItems;

            dataGridViewWord.ColumnCount = maxColumns;
            dataGridViewWord.RowCount = maxRows;

            //-----------------------'Word Map의 제목표시--------------------
            int wordColumnCount = dataGridViewWord.ColumnCount;
            DataGridViewColumnCollection columns = dataGridViewWord.Columns;
            DataGridViewColumn column;
            for (int i = 0; i < wordColumnCount; i += maxColumnItems)
            {
                column = columns[i];
                column.Name = "Address";
                column.Width = 60;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
                DataGridViewCellStyle cellStyle = column.DefaultCellStyle;
                //cellStyle.Font = BoldFont;
                cellStyle.BackColor = Color.LightSteelBlue;

                column = columns[i + 1];
                column.Name = "Decimal";
                column.Width = 60;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 2];
                column.Name = "Ascii";
                column.Width = 40;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 3];
                column.Name = "Hex";
                column.Width = 40;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 4];
                column.Name = "Binary";
                column.Width = 120;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < maxRows; i++)
            {
                dataGridViewWord.Rows[i].Height = 22;
                //dataGridViewWord.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
            }
            
            int indexX = 0;
            int indexY = 0;

            // -----------화면에 데이타를 표시하고 컬렉션에 저장한다----------------------------------
            DataGridViewCellCollection cells;
            for (int i = 0; i < addressSize; i++)
            {
                if (indexY >= maxRows)
                {
                    indexX = indexX + maxColumnItems;
                    indexY = 0;
                }

                // 화면에 표시
                cells = dataGridViewWord.Rows[indexY].Cells;
                cells[indexX].Value = AddressList[i];
                cells[indexX + 1].Value = "0";
                cells[indexX + 3].Value = "0000";
                cells[indexX + 4].Value = "0000000000000000";

                indexY += 1;
            }

            dataGridViewWord.ClearSelection();
        }

        private void tmMonitor_Tick(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                UpdateUsedBitView();
                UpdateUsedWordView();
                UpdateWordView();
            }
        }

        private void UpdateUsedBitView()
        {
            if (this.tabControl1.SelectedTab != this.tabPageUsedBits) return;

            bool bValue;
            DataGridViewButtonCell cell = null;
            int columnCount = dataGridViewUsedBit.Columns.Count;
            int rowCount = dataGridViewUsedBit.Rows.Count;
            for (int i = 0; i < columnCount; i++)
            {
                if (dataGridViewUsedBit.Columns[i].Name == "Address" )
                {
                    for (int n = 0; n < rowCount; n++)
                    {
                        cell = dataGridViewUsedBit[i, n] as DataGridViewButtonCell;
                        if (cell != null)
                        {
                            // Bit Input
                            if (i == 0) bValue = m_MelsecBitInputs[n].GetStatus();
                            // Bit Output
                            else bValue = m_MelsecBitOutputs[n].GetStatus();

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
            int inputSize = m_MelsecBitInputs.Count;
            
            short nValue;
            string sValue;
            MelsecDevice mel;
            for (int i = 0; i < maxRows; i++)
            {
                mel = dataGridViewUsedWord[0, i].Tag as MelsecDevice;
                nValue = m_Melsec.ReceiveWord(mel.GetStartAddress());
                if (nValue == -1) sValue = "65535";
                else sValue = nValue.ToString();

                dataGridViewUsedWord[3,i].Value = sValue;
                dataGridViewUsedWord[4,i].Value = m_Melsec.DecToAsc(sValue);
                dataGridViewUsedWord[5,i].Value = m_Melsec.DecToHex(sValue);
                dataGridViewUsedWord[6,i].Value = m_Melsec.DecToBin(sValue);
            }

            tmMonitor.Enabled = true;
        }

        private void UpdateWordView()
        {
            if (this.tabControl1.SelectedTab != this.tabPageWords) return;

            tmMonitor.Enabled = false;

            if (m_SelectedWordIndex == -1) return;
            int index = WordViewIndexAddress[m_SelectedWordIndex];
            int nStartValue = 0;

            for (int i = 0; i < index; i++)
            {
                if (m_Melsec.MonitorAddress[i].Type == devTYPE.devB) nStartValue += (m_Melsec.MonitorAddress[i].Size - 1) / 16 + 1;
                else if (m_Melsec.MonitorAddress[i].Type == devTYPE.devW) nStartValue += m_Melsec.MonitorAddress[i].Size;
            }

            int addressSize = m_Melsec.MonitorAddress[index].Size;
            int maxRows = dataGridViewWord.RowCount;
            int maxColumnItems = 5;

            int indexX = 0;
            int indexY = 0;

            // -----------화면에 데이타를 표시하고 컬렉션에 저장한다----------------------------------
            for (int i = 0; i < addressSize; i++)
            {
                if (indexY >= maxRows)
                {
                    indexX = indexX + maxColumnItems;
                    indexY = 0;
                }

                string sValue;

                // 화면에 표시
                if (m_Melsec.GetReadData(i + nStartValue) == -1)
                {
                    dataGridViewWord.Rows[indexY].Cells[indexX + 1].Value = 65535;
                    sValue = "65535";
                }
                else
                {
                    dataGridViewWord.Rows[indexY].Cells[indexX + 1].Value = m_Melsec.GetReadData(i + nStartValue);
                    sValue = m_Melsec.GetReadData(i + nStartValue).ToString();
                }

                dataGridViewWord.Rows[indexY].Cells[indexX + 2].Value = m_Melsec.DecToAsc(sValue);
                dataGridViewWord.Rows[indexY].Cells[indexX + 3].Value = m_Melsec.DecToHex(sValue);
                dataGridViewWord.Rows[indexY].Cells[indexX + 4].Value = m_Melsec.DecToBin(sValue);

                indexY += 1;
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
                bool state = m_MelsecBitInputs[nRow].GetStatus();
                m_MelsecBitInputs[nRow].SetStatus(!state);
            }
            // Bit Output
            else
            {
                bool state = m_MelsecBitOutputs[nRow].GetStatus();
                m_MelsecBitOutputs[nRow].SetStatus(!state);
            }

            dataGridViewUsedBit.ClearSelection();
        }

        private void dataGridViewWord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;
            int nCol = e.ColumnIndex;

            GridViewWordDataChange(nRow, nCol);
        }

        private void GridViewWordDataChange(int nRow, int nCol)
        {
            int nDivide = nCol % 5;
            string sValue;

            DataGridViewCell cell = dataGridViewWord.Rows[nRow].Cells[nCol];
            StringBuilder sbuild = new StringBuilder();
            string address = "";

            if (cell.Value == null)
            {
                if (nDivide == 1)
                {
                    dataGridViewWord.Rows[nRow].Cells[nCol].Value = "0";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = "";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 2].Value = "0000";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 3].Value = "0000000000000000";
                }
                else if (nDivide == 2)
                {
                    dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = "0";
                    dataGridViewWord.Rows[nRow].Cells[nCol].Value = "";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = "0000";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 2].Value = "0000000000000000";
                }
                else if (nDivide == 3)
                {
                    dataGridViewWord.Rows[nRow].Cells[nCol - 2].Value = "0";
                    dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = "";
                    dataGridViewWord.Rows[nRow].Cells[nCol].Value = "0000";
                    dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = "0000000000000000";

                }
                else if (nDivide == 4)
                {
                    dataGridViewWord.Rows[nRow].Cells[nCol - 3].Value = "0";
                    dataGridViewWord.Rows[nRow].Cells[nCol - 2].Value = "";
                    dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = "0000";
                    dataGridViewWord.Rows[nRow].Cells[nCol].Value = "0000000000000000";
                }
                return;
            }

            if (nDivide == 1)
            {
                try
                {
                    cell.Value = Strings.Left(cell.Value.ToString(), 5);
                    sValue = cell.Value.ToString();

                    if (Convert.ToInt32(sValue) > 65535)
                        sValue = "65535";

                    cell.Value = sValue;
                    dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = m_Melsec.DecToAsc(sValue);
                    dataGridViewWord.Rows[nRow].Cells[nCol + 2].Value = m_Melsec.DecToHex(sValue);
                    dataGridViewWord.Rows[nRow].Cells[nCol + 3].Value = m_Melsec.DecToBin(sValue);

                    address = dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value.ToString();

                    m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(sValue));
                }
                catch
                {
                    return;
                }

            }
            else if (nDivide == 2)
            {
                cell.Value = Strings.Left(cell.Value.ToString(), 2);
                sValue = Strings.UCase(cell.Value.ToString());

                cell.Value = sValue;
                dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = m_Melsec.AscToDec(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = m_Melsec.AscToHex(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol + 2].Value = m_Melsec.AscToBin(sValue);

                address = dataGridViewWord.Rows[nRow].Cells[nCol - 2].Value.ToString();

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.AscToDec(sValue)));
            }
            else if (nDivide == 3)
            {
                cell.Value = Strings.Left(cell.Value.ToString(), 4);
                sValue = Strings.UCase(cell.Value.ToString());

                if (sValue.Length <= 2)
                    sValue = "00" + sValue.PadLeft(2, '0');
                else
                    sValue = sValue.PadLeft(4, '0');

                cell.Value = sValue;
                dataGridViewWord.Rows[nRow].Cells[nCol - 2].Value = m_Melsec.HexToDec(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = m_Melsec.HexToAsc(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol + 1].Value = m_Melsec.HexToBin(sValue);

                address = dataGridViewWord.Rows[nRow].Cells[nCol - 3].Value.ToString();

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.HexToDec(sValue)));

            }
            else if (nDivide == 4)
            {
                cell.Value = Strings.Left(cell.Value.ToString(), 16);
                sValue = Strings.UCase(cell.Value.ToString());

                if (sValue.Length <= 8)
                    sValue = "00000000" + sValue.PadLeft(8, '0');
                else
                    sValue = sValue.PadLeft(16, '0');

                cell.Value = sValue;
                dataGridViewWord.Rows[nRow].Cells[nCol - 1].Value = m_Melsec.BinToHex(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol - 2].Value = m_Melsec.BinToAsc(sValue);
                dataGridViewWord.Rows[nRow].Cells[nCol - 3].Value = m_Melsec.BinToDec(sValue);

                address = dataGridViewWord.Rows[nRow].Cells[nCol - 4].Value.ToString();

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.BinToDec(sValue)));
            }
        }

        private void cbWordView_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SelectedWordIndex = this.cbWordView.SelectedIndex;
            WordDisplay();
        }
    }
}

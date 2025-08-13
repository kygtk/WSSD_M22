using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Dms.Common;

namespace Dms.Ctl
{
    public partial class Melsec_Debug_Form : Form
    {
        public Melsec m_Melsec;
        private Melsec_Debug_Process_Form Process_frm = null;
        public Font SelectFont;
        public Font NotSelectFont;
        private List<int> WordViewIndexAddress = null;
        private int WordViewIndex = 0;


        public Melsec_Debug_Form(Melsec melsec)
        {
            m_Melsec = melsec;

            SelectFont = new Font("Arial", 8, FontStyle.Bold);
            NotSelectFont = new Font("Arial", 8);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitWordGridView();

            BitDisplay();
            if (m_Melsec.MonitorAddress.Count != 0) WordDisplay();
        }

        private void InitWordGridView()
        {
            WordViewIndexAddress = new List<int>();
            lblWordView.Visible = false;
            cbWordView.Visible = false;

            StringBuilder sb = new StringBuilder();

            int count = m_Melsec.MonitorAddress.Count;
            if ( count != 0)
            {
                tagADDR_INFO addrInfo;
                for (int i = 0; i < count; i++)
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

                cbWordView.SelectedIndex = 0;
            }
        }

        private void BitDisplay()
        {
            int nintStatus, nintEventCnt;

            nintStatus = m_Melsec.GetStatusCount() / 25 + 1;    // Status Map의 X축의 갯수
            nintEventCnt = m_Melsec.GetReqCount() / 25 + 1;  // EventCnt Map의 X축의 갯수

            dataGridViewBit.ColumnCount = nintEventCnt * 4 + (nintStatus * 2);
            dataGridViewBit.RowCount = 25;

            // Set the column header names.
            DataGridViewColumnCollection columns = dataGridViewBit.Columns;
            DataGridViewColumn column; 
            int count = nintEventCnt * 4;
            for (int i = 0; i < count; i += 4)
            {
                column = columns[i];
                column.Name = "Req";
                column.Width = 50;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 1];
                column.Name = "Cmp";
                column.Width = 50;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 2];
                column.Name = "Commender";
                column.Width = 80;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 3];
                column.Name = "Define";
                column.Width = 200;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            count = dataGridViewBit.ColumnCount;
            for (int i = nintEventCnt * 4; i < count; i += 2)
            {
                column = columns[i];
                column.Name = "Status";
                column.Width = 50;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column = columns[i + 1];
                column.Name = "Define";
                column.Width = 200;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            DataGridViewRow row;
            for (int i = 0; i < 25; i++)
            {
                row = dataGridViewBit.Rows[i];
                row.Height = 22;
                row.HeaderCell.Value = Convert.ToString(i + 1);
            }

            int nintEventX = 0;
            int nintEventY = 0;
            int nintStatusX = nintEventCnt * 4;
            int nintStatusY = 0;

            foreach (clsAdressMapType address in m_Melsec.GetAddressTypeSortedList().Values)
            {
                if (address.Status != "STATUS")
                {
                    if (address.Status == "REQ")
                    {
                        if (nintEventY >= 25)
                        {
                            nintEventY = 0;
                            nintEventX = nintEventX + 4;
                        }

                        SetButtonCell(dataGridViewBit, nintEventX, nintEventY, address.ReqBit);
                        SetButtonCell(dataGridViewBit, nintEventX + 1, nintEventY, address.CompBit);

                        if (address.Commander == "P")
                            dataGridViewBit.Rows[nintEventY].Cells[nintEventX + 2].Value = "PLC";
                        else
                            dataGridViewBit.Rows[nintEventY].Cells[nintEventX + 2].Value = "CIM";

                        dataGridViewBit.Rows[nintEventY].Cells[nintEventX + 3].Value = address.EventDescription;

                        nintEventY += 1;
                    }
                }
                else if (address.Status == "STATUS" && (address.ReqBit.Substring(0, 1) != "W"))
                {
                    if (nintStatusY >= 25)
                    {
                        nintStatusY = 0;
                        nintStatusX = nintStatusX + 2;
                    }

                    SetButtonCell(dataGridViewBit, nintStatusX, nintStatusY, address.ReqBit);

                    dataGridViewBit.Rows[nintStatusY].Cells[nintStatusX + 1].Value = address.EventDescription;

                    nintStatusY += 1;
                }
            }

            dataGridViewBit.ClearSelection();
        }
        private void SetButtonCell(DataGridView GridView, int ColumnIndex, int RowIndex, string text)
        {
            DataGridViewButtonCell button = new DataGridViewButtonCell();

            button.Style.BackColor = Color.Silver;
            GridView[ColumnIndex, RowIndex] = button;
            GridView.Rows[RowIndex].Cells[ColumnIndex].Value = text;
       }

        private void WordDisplay()
        {
            tmMonitor.Enabled = false;

            int nintTempCnt = 0;
            int nintStatus = 0;
            List<string> AddressList = new List<string>();

            int index = WordViewIndexAddress[WordViewIndex];

            nintTempCnt += m_Melsec.MonitorAddress[index].Size;

            for (int j = 0; j < m_Melsec.MonitorAddress[index].Size; j++)
            {
                AddressList.Add(m_Melsec.GetAddress(devTYPE.devW, m_Melsec.MonitorAddress[index].StartAddress + j));
            }

            //dataGridViewWord.Columns.Clear();

            nintStatus = nintTempCnt / 25 + 1;                  // X축의 갯수
            dataGridViewWord.ColumnCount = nintStatus * 5;
            dataGridViewWord.RowCount = 25;

            //-----------------------'Word Map의 제목표시--------------------
            for (int i = 0; i < nintStatus * 5; i += 5)
            {
                dataGridViewWord.Columns[i].Name = "Address";
                dataGridViewWord.Columns[i].Width = 60;
                dataGridViewWord.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridViewWord.Columns[i + 1].Name = "Decimal";
                dataGridViewWord.Columns[i + 1].Width = 60;
                dataGridViewWord.Columns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridViewWord.Columns[i + 2].Name = "Ascii";
                dataGridViewWord.Columns[i + 2].Width = 40;
                dataGridViewWord.Columns[i + 2].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridViewWord.Columns[i + 3].Name = "Hex";
                dataGridViewWord.Columns[i + 3].Width = 40;
                dataGridViewWord.Columns[i + 3].SortMode = DataGridViewColumnSortMode.NotSortable;

                dataGridViewWord.Columns[i + 4].Name = "Binary";
                dataGridViewWord.Columns[i + 4].Width = 120;
                dataGridViewWord.Columns[i + 4].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < 25; i++)
            {
                dataGridViewWord.Rows[i].Height = 22;
                dataGridViewWord.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
            }

            int nintEventX = 0;
            int nintEventY = 0;

            // -----------화면에 데이타를 표시하고 컬렉션에 저장한다----------------------------------
            for (int i = 0; i < nintTempCnt; i++)
            {
                if (nintEventY >= 25)
                {
                    nintEventY = 0;
                    nintEventX = nintEventX + 5;
                }

                // 화면에 표시
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX].Value = AddressList[i];
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 1].Value = "0";
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 3].Value = "0000";
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 4].Value = "0000000000000000";

                nintEventY += 1;
            }

            tmMonitor.Enabled = true;
        }

        private void dataGridViewBit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = 0;
            int nCol = 0;

            nRow = e.RowIndex;
            nCol = e.ColumnIndex;

            if (nRow < 0 || nCol < 0) return;
            if (dataGridViewBit[nCol, nRow].Style.BackColor == Color.Empty) return;

            StringBuilder sb = new StringBuilder();

            string address = dataGridViewBit[nCol, nRow].Value.ToString();

            if (dataGridViewBit[nCol, nRow].Style.BackColor == Color.Silver)
            {
                if (m_Melsec.Simulate)
                {
                    m_Melsec.DebugSendBit(m_Melsec.GetAddressDec(address), 1);

                    if (m_Melsec.AddressMapType(address).Status == "STATUS" ||
                        (m_Melsec.AddressMapType(address).Status == "REQ" && m_Melsec.AddressMapType(address).Commander == "P") ||
                        (m_Melsec.AddressMapType(address).Status == "COMP" && m_Melsec.AddressMapType(address).Commander == "C") ||
                        (m_Melsec.AddressMapType(address).Status == "ABORT" && m_Melsec.AddressMapType(address).Commander == "C"))
                    {
                        sb.Append("ChangeBit," + address + "," + "1" + ",");
                        if (m_Melsec.AddressMapType(address).Status == "STATUS") sb.Append("1");
                        else sb.Append("0");
                        m_Melsec.GetScanEventQueue().Enqueue(sb.ToString());

                    }
                    else
                    {
                        sb.Append("BitWrite," + address + "," + "1");
                        m_Melsec.GetScanEventQueue().Enqueue(sb.ToString());
                    }
                }
                else
                {
                    m_Melsec.SendBit(0, 255, m_Melsec.GetAddressDec(address), 1, true);
                }
            }
            else
            {
                if (m_Melsec.Simulate)
                {
                    m_Melsec.DebugSendBit(m_Melsec.GetAddressDec(address), 0);

                    if (m_Melsec.AddressMapType(address).Status == "STATUS" ||
                        (m_Melsec.AddressMapType(address).Status == "REQ" && m_Melsec.AddressMapType(address).Commander == "P") ||
                        (m_Melsec.AddressMapType(address).Status == "COMP" && m_Melsec.AddressMapType(address).Commander == "C") ||
                        (m_Melsec.AddressMapType(address).Status == "ABORT" && m_Melsec.AddressMapType(address).Commander == "C"))
                    {
                        sb.Append("ChangeBit," + address + "," + "0" + ",");
                        if (m_Melsec.AddressMapType(address).Status == "STATUS") sb.Append("1");
                        else sb.Append("0");
                        m_Melsec.GetScanEventQueue().Enqueue(sb.ToString());
                    }
                    else
                    {
                        sb.Append("BitWrite," + address + "," + "1");
                        m_Melsec.GetScanEventQueue().Enqueue(sb.ToString());
                    }

                }
                else
                {
                    m_Melsec.SendBit(0, 255, m_Melsec.GetAddressDec(address), 0, true);
                }
            }

            dataGridViewBit.ClearSelection();
        }

        private void dataGridViewWord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int nRow = 0;
            int nCol = 0;

            nRow = e.RowIndex;
            nCol = e.ColumnIndex;

            GridViewWordDataChange(nRow, nCol);
        }

        private void GridViewWordDataChange(int nRow, int nCol)
        {
            int nDivide = nCol % 5;
            string sValue;

            DataGridViewCell cell;
            cell = dataGridViewWord.Rows[nRow].Cells[nCol];
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


                    if (m_Melsec.Simulate)
                    {
                        if (m_Melsec.AddressMapType(address) != null)
                        {
                            if (m_Melsec.AddressMapType(address).Status == "STATUS")
                            {
                                sbuild.Append("ChangeWord," + address + "," + sValue + ",1");
                                m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                            }
                        }
                    }

                    m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32( sValue ), false);
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

                if (m_Melsec.Simulate)
                {
                    if (m_Melsec.AddressMapType(address) != null)
                    {
                        if (m_Melsec.AddressMapType(address).Status == "STATUS")
                        {
                            sbuild.Append("ChangeWord," + address + "," + m_Melsec.AscToDec(sValue) + ",1");
                            m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                        }
                    }
                }

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.AscToDec(sValue)), false);
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

                if (m_Melsec.Simulate)
                {
                    if (m_Melsec.AddressMapType(address) != null)
                    {
                        if (m_Melsec.AddressMapType(address).Status == "STATUS")
                        {
                            sbuild.Append("ChangeWord," + address + "," + m_Melsec.HexToDec(sValue) + ",1");
                            m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                        }
                    }
                }

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.HexToDec(sValue)), false);

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

                if (m_Melsec.Simulate)
                {
                    if (m_Melsec.AddressMapType(address) != null)
                    {
                        if (m_Melsec.AddressMapType(address).Status == "STATUS")
                        {
                            sbuild.Append("ChangeWord," + address + "," + m_Melsec.BinToDec(sValue) + ",1");
                            m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                        }
                    }
                }

                m_Melsec.SendWord(0, 255, m_Melsec.GetAddressDec(address), (short)Convert.ToInt32(m_Melsec.BinToDec(sValue)), false);
            }
        }

        private void btn_Process_Click(object sender, EventArgs e)
        {
            if (m_Melsec.Simulate)
            {
                Process_frm = new Melsec_Debug_Process_Form(this);
                Process_frm.Show();
            }
            else
            {
                MessageBox.Show("Monitoring mode is not execute");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                lblWordView.Visible = true;
                cbWordView.Visible = true;
            }
            else
            {
                lblWordView.Visible = false;
                cbWordView.Visible = false;
            }

        }

        private void tmMonitor_Tick(object sender, EventArgs e)
        {            
            UpdateBitView();
            UpdateWordView();
        }

        private void cbWordView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (WordViewIndexAddress.Count > 0)
            {
                WordViewIndex = cbWordView.SelectedIndex;
                WordDisplay();
            }
        }

        private void UpdateBitView()
        {
            string sValue;
            bool bValue;
            int nCount = 0;

            int columnCount = dataGridViewBit.Columns.Count;
            int rowCount = dataGridViewBit.Rows.Count;
            string columnName;
            DataGridViewButtonCell cell;
            for (int i = 0; i < columnCount; i++)
            {
                columnName = dataGridViewBit.Columns[i].Name;
                if (columnName == "Req" ||
                    columnName == "Cmp" ||
                    columnName == "Status")
                {
                    for (int n = 0; n < rowCount; n++)
                    {
                        cell = dataGridViewBit[i, n] as DataGridViewButtonCell;
                        if (cell != null)
                        {
                            sValue = cell.Value.ToString();

                            bValue = m_Melsec.ReceiveBit(m_Melsec.GetAddressDec(sValue), false);

                            if (bValue && cell.Style.BackColor == Color.Silver)
                            {
                                cell.Style.BackColor = Color.Red;
                                cell.Style.Font = SelectFont;
                            }
                            else if (!bValue && cell.Style.BackColor == Color.Red)
                            {
                                cell.Style.BackColor = Color.Silver;
                                cell.Style.Font = NotSelectFont;
                            }

                            nCount += 1;
                        }
                    }
                }
            }
        }

        private void UpdateWordView()
        {
            int index = WordViewIndexAddress[WordViewIndex];
            int nintTempCnt = 0;
            int nintStatus = 0;
            int nStartValue = 0;

            for (int i = 0; i < index; i++)
            {
                if (m_Melsec.MonitorAddress[i].Type == devTYPE.devB) nStartValue += (m_Melsec.MonitorAddress[i].Size - 1) / 16 + 1;
                else if (m_Melsec.MonitorAddress[i].Type == devTYPE.devW) nStartValue += m_Melsec.MonitorAddress[i].Size;

            }

            nintTempCnt += m_Melsec.MonitorAddress[index].Size;

            nintStatus = nintTempCnt / 25 + 1;                  // X축의 갯수
            dataGridViewWord.ColumnCount = nintStatus * 5;
            dataGridViewWord.RowCount = 25;

            int nintEventX = 0;
            int nintEventY = 0;

            // -----------화면에 데이타를 표시하고 컬렉션에 저장한다----------------------------------
            for (int i = 0; i < nintTempCnt; i++)
            {
                if (nintEventY >= 25)
                {
                    nintEventY = 0;
                    nintEventX = nintEventX + 5;
                }

                string sValue;

                // 화면에 표시
                if (m_Melsec.GetReadData(i + nStartValue) == -1)
                {
                    dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 1].Value = 65535;
                    sValue = "65535";
                }
                else
                {
                    dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 1].Value = m_Melsec.GetReadData(i + nStartValue);
                    sValue = m_Melsec.GetReadData(i + nStartValue).ToString();
                }

                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 2].Value = m_Melsec.DecToAsc(sValue);
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 3].Value = m_Melsec.DecToHex(sValue);
                dataGridViewWord.Rows[nintEventY].Cells[nintEventX + 4].Value = m_Melsec.DecToBin(sValue);

                nintEventY += 1;
            }
        }

        private void dataGridViewWord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbWordView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
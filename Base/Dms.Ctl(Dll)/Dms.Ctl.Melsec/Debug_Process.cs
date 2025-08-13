using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace Dms.Ctl
{
    public partial class Melsec_Debug_Process_Form : Form
    {
        private Melsec_Debug_Form Debug_frm = null;
        public List<string> m_ProcessList = new List<string>();
        public Font SelectFont;
        public Font NotSelectFont;
        int nLine = 0;
        bool bPause = false;
        int nSelectRow = -1;

        public Melsec_Debug_Process_Form(Melsec_Debug_Form debug_form)
        {
            Debug_frm = debug_form;

            SelectFont = new Font("Arial", 9, FontStyle.Bold);
            NotSelectFont = new Font("Arial", 9);

            InitializeComponent();

            btnState(false);
        }

        private void btnState(bool bEnable)
        {
            btnDelete.Enabled = bEnable;
            btnInsert.Enabled = bEnable;
            btnSave.Enabled = bEnable;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            while (m_ProcessList.Count > 0)
            {
                m_ProcessList.Clear();
            }

            openProcessFileDlg.InitialDirectory = Application.StartupPath + @"\system\Scenario\";

            if (openProcessFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(openProcessFileDlg.FileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
                string input = "";

                while ((input = sr.ReadLine()) != null)
                {
                    m_ProcessList.Add(input);
                }

                sr.Close();

                InitDisplay();

                ProcessDisplay();
            }
        }

        private void ProcessDisplay()
        {
            dataGridViewProcess.RowCount = m_ProcessList.Count;

            int rowCount = dataGridViewProcess.RowCount;
            for (int i = 0; i < rowCount; i++)
            {
                dataGridViewProcess.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                dataGridViewProcess[2, i].Value = m_ProcessList[i];
            }

            btnState(true);
        }

        private void tmrProcess_Tick(object sender, EventArgs e)
        {
            string sProcessData = "";
            string[] sToken;
            string[] sTokenValue;

            if (bPause) return;

            if (nLine < m_ProcessList.Count)
            {
                dataGridViewProcess[1, nLine].Value = "ขั";

                dataGridViewProcess.CurrentCell = dataGridViewProcess.Rows[nLine].Cells[1];

                for (int i = 0; i <= nLine - 1; i++)
                {
                    dataGridViewProcess[1, i].Value = "";
                }

                sProcessData = m_ProcessList[nLine];

                if (Strings.UCase(sProcessData.Substring(0, 1)) == "B")
                {
                    sToken = sProcessData.Split(',');

                    if (sToken[0].Length != 5)
                    {
                        dataGridViewProcess[2, nLine].Style.ForeColor = Color.Red;
                        dataGridViewProcess[2, nLine].Style.Font = SelectFont;

                        nLine += 1;
                    }
                    else
                    {
                        Debug_frm.m_Melsec.DebugSendBit(sToken[0].Trim(), sToken[1].Trim(), false);

                        dataGridViewProcess[2, nLine].Style.ForeColor = Color.Blue;
                        dataGridViewProcess[2, nLine].Style.Font = SelectFont;

                        nLine += 1;
                    }
                }
                else if (Strings.UCase(sProcessData.Substring(0, 1)) == "W")
                {
                    sToken = sProcessData.Split(',');

                    if (sToken[0].Length != 5)
                    {
                        dataGridViewProcess[2, nLine].Style.ForeColor = Color.Red;
                        dataGridViewProcess[2, nLine].Style.Font = SelectFont;

                        nLine += 1;
                    }
                    else
                    {
                        if (sToken[2].Trim() == "A")
                        {
                            short nAddress = 0;
                            nAddress = Debug_frm.m_Melsec.GetAddressDec(sToken[0].Trim());

                            Debug_frm.m_Melsec.WriteString(0, 255, nAddress, sToken[1], (short)sToken[1].Trim().Length);
                        }

                        else if (sToken[2].Trim() == "I")
                        {
                            short nValue = 0;
                            short nAddress = 0;

                            nAddress = Debug_frm.m_Melsec.GetAddressDec(sToken[0].Trim());

                            if (sToken[1] != null)
                            {
                                nValue = Convert.ToInt16(sToken[1].Trim());
                            }

                            Debug_frm.m_Melsec.SendWord(0, 255, nAddress, nValue, true);

                            if (Debug_frm.m_Melsec.Simulate)
                            {
                                if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()) != null)
                                {
                                    if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()).Status == "STATUS")
                                    {
                                        StringBuilder sbuild = new StringBuilder();
                                        sbuild.Append("ChangeWord," + sToken[0].Trim() + "," + sToken[1].Trim() + ",1");
                                        Debug_frm.m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                                    }
                                }
                            }
                        }
                        else if (sToken[2].Trim() == "B")
                        {
                            short nValue = 0;
                            short nAddress = 0;

                            if (sToken[1] != null)
                            {
                                nValue = (short)Debug_frm.m_Melsec.BinToDec(sToken[1].Trim());
                            }

                            nAddress = Debug_frm.m_Melsec.GetAddressDec(sToken[0].Trim());
                            Debug_frm.m_Melsec.SendWord(0, 255, nAddress, nValue, true);

                            if (Debug_frm.m_Melsec.Simulate)
                            {
                                if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()) != null)
                                {
                                    if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()).Status == "STATUS")
                                    {
                                        StringBuilder sbuild = new StringBuilder();
                                        sbuild.Append("ChangeWord," + sToken[0].Trim() + "," + sToken[1].Trim() + ",1");
                                        Debug_frm.m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                                    }
                                }
                            }
                        }
                        else if (sToken[2].Trim() == "H")
                        {
                            short nValue = 0;
                            short nAddress = 0;

                            if (sToken[1] != null)
                            {
                                nValue = (short)Debug_frm.m_Melsec.HexToDec(sToken[1].Trim());
                            }

                            nAddress = Debug_frm.m_Melsec.GetAddressDec(sToken[0].Trim());
                            Debug_frm.m_Melsec.SendWord(0, 255, nAddress, nValue, true);

                            if (Debug_frm.m_Melsec.Simulate)
                            {
                                if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()) != null)
                                {
                                    if (Debug_frm.m_Melsec.AddressMapType(sToken[0].Trim()).Status == "STATUS")
                                    {
                                        StringBuilder sbuild = new StringBuilder();
                                        sbuild.Append("ChangeWord," + sToken[0].Trim() + "," + sToken[1].Trim() + ",1");
                                        Debug_frm.m_Melsec.GetScanEventQueue().Enqueue(sbuild.ToString());
                                    }
                                }
                            }
                        }
                        dataGridViewProcess[2, nLine].Style.ForeColor = Color.Blue;
                        dataGridViewProcess[2, nLine].Style.Font = SelectFont;

                        nLine += 1;
                    }
                }
                else if (Strings.UCase(sProcessData.Substring(0, 1)) == "'")
                {
                    dataGridViewProcess[2, nLine].Style.ForeColor = Color.LimeGreen;
                    dataGridViewProcess[2, nLine].Style.Font = SelectFont;

                    nLine += 1;
                }
                else if (Strings.UCase(sProcessData.Substring(0, 1)) == "I")
                {
                    sToken = sProcessData.Split(' ');
                    sTokenValue = sToken[1].Split('=');

                    if (Debug_frm.m_Melsec.DebugReceiveBit(sTokenValue[0]) == sTokenValue[1])
                    {
                        dataGridViewProcess[2, nLine].Style.ForeColor = Color.LimeGreen;
                        dataGridViewProcess[2, nLine].Style.Font = SelectFont;
                        nLine += 1;
                    }
                }
                else if (Strings.UCase(sProcessData.Substring(0, 1)) == "D")
                {
                    sToken = sProcessData.Split(',');
                    subDealyTime(Convert.ToInt32(sToken[1].Trim()));
                }
                else if (Strings.UCase(sProcessData.Substring(0, 1)) == " ")
                {
                    nLine += 1;
                }
            }
            else
            {
                tmrProcess.Enabled = false;
                btnProcess.Enabled = true;
            }
        }

        private void subDealyTime(int nTime)
        {
            int nDelayTime = nTime * 1000;

            tmr_Dealy.Interval = nDelayTime;
            tmr_Dealy.Enabled = true;
        }

        private void tmr_Dealy_Tick(object sender, EventArgs e)
        {
            dataGridViewProcess[2, nLine].Style.ForeColor = Color.Blue;
            dataGridViewProcess[2, nLine].Style.Font = SelectFont;

            nLine += 1;
            tmr_Dealy.Enabled = false;
        }

        private void btnTimerSet_Click(object sender, EventArgs e)
        {
            if (txtTimer.Text == null) return;

            tmrProcess.Interval = (int) (Convert.ToSingle(txtTimer.Text) * 1000);
        }

        private void InitDisplay()
        {
            int rowCount = dataGridViewProcess.RowCount;
            DataGridViewCell cell;
            for (int i = 0; i < rowCount; i++)
            {
                cell = dataGridViewProcess[2, i];
                cell.Style.ForeColor = Color.Black;
                cell.Style.BackColor = Color.White;
                cell.Style.Font = NotSelectFont;
                cell.Value = "";
            }

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if( !bPause ) nLine = 0;

            if (txtTimer.Text != null) 
                tmrProcess.Interval = (int) ( Convert.ToSingle(txtTimer.Text) * 1000);

            InitDisplay();

            dataGridViewProcess[2, nLine].Style.ForeColor = Color.Black;
            dataGridViewProcess[2, nLine].Style.BackColor = Color.White;

            tmrProcess.Enabled = true;
            btnProcess.Enabled = false;
            txtTimer.Enabled = false;
            btnTimerSet.Enabled = false;

            btnState(false);

            bPause = false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            tmrProcess.Enabled = false;
            btnProcess.Enabled = true;
            txtTimer.Enabled = true;
            btnTimerSet.Enabled = true;

            bPause = true;
        }

        private void btnInitial_Click(object sender, EventArgs e)
        {
            tmrProcess.Enabled = false;
            bPause = false;

            dataGridViewProcess.RowCount = 0;
            dataGridViewProcess.ColumnCount = 3;

            btnProcess.Enabled = true;
            txtTimer.Enabled = true;

            btnState(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            nSelectRow = dataGridViewProcess.CurrentCellAddress.Y;

            m_ProcessList.RemoveAt(nSelectRow);

            dataGridViewProcess.RowCount = m_ProcessList.Count;
            int rowCount = dataGridViewProcess.RowCount;
            for (int i = 0; i < rowCount; i++)
            {
                dataGridViewProcess.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                dataGridViewProcess[2, i].Value = m_ProcessList[i];
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int nRowCount = dataGridViewProcess.RowCount;
            int nCurRow = dataGridViewProcess.CurrentCellAddress.Y;

            if( nRowCount != nCurRow + 1)
                dataGridViewProcess.Rows.Insert(nCurRow, 1);
            else
                dataGridViewProcess.Rows.Add();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveProcessFileDlg.InitialDirectory = Application.StartupPath + @"\system\Scenario\";

            if (saveProcessFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(saveProcessFileDlg.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter sr = new StreamWriter(file, System.Text.Encoding.Default);
                string Output = "";

                m_ProcessList.Clear();

                DataGridViewCell cell;
                int rowCount = dataGridViewProcess.RowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    cell = dataGridViewProcess[2, i];

                    if (cell.Value == null)
                    {
                        cell.Value = " ";
                    }

                    Output = Strings.UCase(Convert.ToString(cell.Value));
                    dataGridViewProcess.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);

                    cell.Value = Output;
                    m_ProcessList.Add(Output);

                    sr.WriteLine(Output);
                }

                sr.Close();
            }
        }
    }
}
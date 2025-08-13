using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Server;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsTabGauge : UserControl // 10.12.21 minhan
    {
        #region Fields
        private static object m_LockKey = new object();
        private ServerManager m_Server;
        private string m_oldMsg = "";
        private string m_Msg = "";
        private string oldGaugeAlarmStatus = "";
        private string oldApGaugeAlarmStatus = "";
        private List<string> tempGaugeAlarmStatus;
        private List<string> tempApGaugeAlarmStatus;
        private int count = 0;
        private string msg;
        private string date;
        private string gaugename;
        private string curvalue;
        private string interlockvalue;
        private int m_Cnt = 0;
        #endregion

        public JobsTabGauge()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            m_Server = ServerManager.Instance;
            this.GaugeAlarmTimer.Enabled = true;
            this.alarmcount.ReadOnly = true;
            msg = "0 Count";
            this.alarmcount.Text = msg;
            this.Plasmaalarmcount.ReadOnly = true;
            msg = "0 Count";
            this.Plasmaalarmcount.Text = msg;

            DataGridViewTextBoxColumn colIntDate = new DataGridViewTextBoxColumn();
            colIntDate.HeaderText = "Date";
            //colIntDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.dataGridView.Columns.Add(colIntDate);

            DataGridViewTextBoxColumn colIntName = new DataGridViewTextBoxColumn();
            colIntName.HeaderText = "Gauge Name";
            //colIntName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colIntName);

            DataGridViewTextBoxColumn Curvalue = new DataGridViewTextBoxColumn();
            Curvalue.HeaderText = "Current value";
            //Curvalue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(Curvalue);

            DataGridViewTextBoxColumn Interlockvalue = new DataGridViewTextBoxColumn();
            Interlockvalue.HeaderText = "Interlock value";
            //Interlockvalue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(Interlockvalue);

            foreach (DataGridViewColumn column in this.dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // AP 11.03.20 minhan
            DataGridViewTextBoxColumn ApcolIntDate = new DataGridViewTextBoxColumn();
            ApcolIntDate.HeaderText = "Date";
            //ApcolIntDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewPlasma.Columns.Add(ApcolIntDate);

            DataGridViewTextBoxColumn ApcolIntName = new DataGridViewTextBoxColumn();
            ApcolIntName.HeaderText = "Gauge Name";
            //ApcolIntName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewPlasma.Columns.Add(ApcolIntName);

            DataGridViewTextBoxColumn ApCurvalue = new DataGridViewTextBoxColumn();
            ApCurvalue.HeaderText = "Current value";
            //ApCurvalue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewPlasma.Columns.Add(ApCurvalue);

            DataGridViewTextBoxColumn ApInterlockvalue = new DataGridViewTextBoxColumn();
            ApInterlockvalue.HeaderText = "Interlock value";
            //ApInterlockvalue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewPlasma.Columns.Add(ApInterlockvalue);

            foreach (DataGridViewColumn column in this.dataGridViewPlasma.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            lock (m_LockKey)
            {
                try
                {
                    tempGaugeAlarmStatus = GlobalVar.CurGaugeAlarm.GetRange(0, GlobalVar.CurGaugeAlarm.Count);
                    if (tempGaugeAlarmStatus.Count > 0 &&
                        oldGaugeAlarmStatus != tempGaugeAlarmStatus[tempGaugeAlarmStatus.Count - 1])
                    {
                        oldGaugeAlarmStatus = tempGaugeAlarmStatus[tempGaugeAlarmStatus.Count - 1];
                        count = 0;
                        this.dataGridView.Rows.Clear();

                        foreach (string list in tempGaugeAlarmStatus)
                        {
                            msg = list;

                            for (int i = 0; i < 4; i++)
                            {
                                if (i != 3)
                                {
                                    m_Cnt = msg.IndexOf('/');

                                    if (m_Cnt != -1)
                                    {
                                        if (i == 0) date = msg.Substring(0, m_Cnt);
                                        else if (i == 1) gaugename = msg.Substring(0, m_Cnt);
                                        else if (i == 2) curvalue = msg.Substring(0, m_Cnt);

                                        msg = msg.Substring(m_Cnt + 1);
                                    }
                                    else
                                    {
                                        date = "Error";
                                        gaugename = date;
                                        curvalue = date;
                                        interlockvalue = date;
                                        break;
                                    }
                                }
                                else
                                {
                                    if ((msg != null) && (msg != ""))
                                    {
                                        interlockvalue = msg;
                                    }
                                    else
                                    {
                                        date = "Error";
                                        gaugename = date;
                                        curvalue = date;
                                        interlockvalue = date;
                                        break;
                                    }
                                }
                            }

                            count++;
                            this.dataGridView.Rows.Add();

                            this.dataGridView.Rows[count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                            this.dataGridView.Rows[count - 1].SetValues(date, gaugename, curvalue, interlockvalue);
                        }

                        msg = string.Format("{0} Count", count);
                        this.alarmcount.Text = msg;

                        msg = "";
                        date = "";
                        gaugename = "";
                        curvalue = "";
                        interlockvalue = "";
                        m_Msg = "";
                        count = 0;
                    }

                    tempApGaugeAlarmStatus = GlobalVar.ApCurGaugeAlarm.GetRange(0, GlobalVar.ApCurGaugeAlarm.Count);
                    if (tempApGaugeAlarmStatus.Count > 0 &&
                        oldApGaugeAlarmStatus != tempApGaugeAlarmStatus[tempApGaugeAlarmStatus.Count - 1])
                    {
                        oldApGaugeAlarmStatus = tempApGaugeAlarmStatus[tempApGaugeAlarmStatus.Count - 1];
                        count = 0;
                        this.dataGridViewPlasma.Rows.Clear();

                        foreach (string list in tempApGaugeAlarmStatus)
                        {
                            msg = list;

                            for (int i = 0; i < 4; i++)
                            {
                                if (i != 3)
                                {
                                    m_Cnt = msg.IndexOf('/');

                                    if (m_Cnt != -1)
                                    {
                                        if (i == 0) date = msg.Substring(0, m_Cnt);
                                        else if (i == 1) gaugename = msg.Substring(0, m_Cnt);
                                        else if (i == 2) curvalue = msg.Substring(0, m_Cnt);

                                        msg = msg.Substring(m_Cnt + 1);
                                    }
                                    else
                                    {
                                        date = "Error";
                                        gaugename = date;
                                        curvalue = date;
                                        interlockvalue = date;
                                        break;
                                    }
                                }
                                else
                                {
                                    if ((msg != null) && (msg != ""))
                                    {
                                        interlockvalue = msg;
                                    }
                                    else
                                    {
                                        date = "Error";
                                        gaugename = date;
                                        curvalue = date;
                                        interlockvalue = date;
                                        break;
                                    }
                                }
                            }

                            count++;
                            this.dataGridViewPlasma.Rows.Add();

                            this.dataGridViewPlasma.Rows[count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                            this.dataGridViewPlasma.Rows[count - 1].SetValues(date, gaugename, curvalue, interlockvalue);
                        }

                        msg = string.Format("{0} Count", count);
                        this.Plasmaalarmcount.Text = msg;

                        msg = "";
                        date = "";
                        gaugename = "";
                        curvalue = "";
                        interlockvalue = "";
                        m_Msg = "";
                        count = 0;
                    }
                }
                catch (Exception err)
                {
                    m_Msg = err.ToString();

                    if (m_Msg != m_oldMsg)
                    {
                        msg = "";
                        date = "";
                        gaugename = "";
                        curvalue = "";
                        interlockvalue = "";
                        count = 0;

                        m_oldMsg = m_Msg;
                        m_Server.WriteExceptionLog(m_Msg);
                    }
                }
            }
        }

        private void GaugeAlarmClear(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else
            {
                GlobalVar.CurGaugeAlarm.Clear();
                tempGaugeAlarmStatus.Clear();
                this.dataGridView.Rows.Clear();
                count = 0;
                msg = string.Format("{0} Count", count);
                this.alarmcount.Text = msg;

                oldGaugeAlarmStatus = "";
                date = "";
                gaugename = "";
                curvalue = "";
                interlockvalue = "";
                msg = "";
                //count = 0;
            }
        }

        private void APGaugeAlarmClear(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else
            {
                GlobalVar.ApCurGaugeAlarm.Clear();
                tempApGaugeAlarmStatus.Clear();
                this.dataGridViewPlasma.Rows.Clear();
                count = 0;
                msg = string.Format("{0} Count", count);
                this.Plasmaalarmcount.Text = msg;

                oldApGaugeAlarmStatus = "";
                date = "";
                gaugename = "";
                curvalue = "";
                interlockvalue = "";
                msg = "";
                //count = 0;
            }
        }
    }
}

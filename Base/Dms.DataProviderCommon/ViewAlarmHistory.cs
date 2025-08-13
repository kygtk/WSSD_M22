using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Data
{
    public partial class ViewAlarmHistory : UserControl
    {
        private delegate void AddAlarmCallback(TagAlarmHistory alarm);

        private AlarmHistoryProvider m_Provider;
        private BindingSource m_BindSource = new BindingSource();
       
        public DataGridView GridView
        {
            get { return this.dataGridViewAlarmHistory; }
        }

        public DataGridViewSelectedRowCollection SelectedRows
        {
            get {return this.dataGridViewAlarmHistory.SelectedRows;}
        }

        public ViewAlarmHistory()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void InitGridView(AlarmHistoryProvider provider)
        {
            // Initialzie AlarmList
            m_Provider = provider;
            m_Provider.Viewer = this;

            //Biding to DataTable
            this.dataGridViewAlarmHistory.AutoGenerateColumns = false;
            this.dataGridViewAlarmHistory.DataSource = m_BindSource;
            m_BindSource.DataSource = m_Provider.Adapter.Table;

            DataGridViewTextBoxColumn colTime = new DataGridViewTextBoxColumn();
            colTime.DataPropertyName = "AlarmTime";
            colTime.HeaderText = "Time";
            colTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmHistory.Columns.Add(colTime);

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "AlarmId";
            colId.HeaderText = "Id";
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmHistory.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "AlarmName";
            colName.HeaderText = "Description";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewAlarmHistory.Columns.Add(colName);

            DataGridViewTextBoxColumn colLevel = new DataGridViewTextBoxColumn();
            colLevel.DataPropertyName = "AlarmLevel";
            colLevel.HeaderText = "Level";
            colLevel.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmHistory.Columns.Add(colLevel);

            this.dataGridViewAlarmHistory.Sort(colTime, ListSortDirection.Descending);
        }

        private void dataGridViewAlarmHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void dataGridViewAlarmHistory_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            m_Provider.UpdateToDB();
        }

        private void dataGridViewAlarmHistory_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //if (DialogResult.No == MessageBox.Show("Do you want to delete information ?", "WSSD Sever",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //{
            //    e.Cancel = true;
            //}
        }

        public void AddAlarm(TagAlarmHistory alarm)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewAlarmHistory.InvokeRequired)
            {
                AddAlarmCallback d = new AddAlarmCallback(AddAlarm);
                this.Invoke(d, new object[] { alarm });
            }
            else
            {
                m_Provider.InvokeAdd(alarm);
            }
        }

        private void dataGridViewAlarmHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetGridColor(sender as DataGridView);
        }

        private static readonly string _ALARM_L = AlarmLevel.L.ToString();
        private void SetGridColor(DataGridView gridView)
        {
            try
            {
                int count = gridView.Rows.Count;
                DataGridViewRowCollection rows = gridView.Rows;
                DataGridViewRow row;
                for (int i = 0; i < count; i++)
                {
                    row = rows[i];
                    if (row.Cells[3].Value.ToString() == _ALARM_L)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    }                    
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }
    }
}

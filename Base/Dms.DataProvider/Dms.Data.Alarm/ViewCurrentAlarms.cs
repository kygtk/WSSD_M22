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
    public partial class ViewCurrentAlarms : UserControl
    {
        private delegate void AlarmCallback(TagAlarm alarm);

        private CurrentAlarmsProvider m_Provider;
        private BindingSource m_BindSource = new BindingSource();

        [Category("DMS : UI")]
        public bool ColumnHeadersVisible
        {
            get { return this.dataGridViewCurrentAlarms.ColumnHeadersVisible; }
            set { this.dataGridViewCurrentAlarms.ColumnHeadersVisible = value; }
        }

        public ViewCurrentAlarms()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void InitGridView(CurrentAlarmsProvider provider)
        {
            m_Provider = provider;
            m_Provider.Viewer = this;
            
            //Biding to DataTable
            this.dataGridViewCurrentAlarms.AutoGenerateColumns = false;
            this.dataGridViewCurrentAlarms.DataSource = m_BindSource;
            m_BindSource.DataSource = m_Provider.Adapter.Table;

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "AlarmId";
            colId.HeaderText = "Id";
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCurrentAlarms.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "AlarmName";
            colName.HeaderText = "Description";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewCurrentAlarms.Columns.Add(colName);
        }

        private void dataGridViewCurrentAlarms_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        public void AddAlarm(TagAlarm alarm)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewCurrentAlarms.InvokeRequired)
            {
                AlarmCallback d = new AlarmCallback(AddAlarm);
                this.Invoke(d, new object[] { alarm });
            }
            else
            {
                m_Provider.InvokeAdd(alarm);
            }
        }

        public void RemoveAlarm(TagAlarm alarm)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewCurrentAlarms.InvokeRequired)
            {
                AlarmCallback d = new AlarmCallback(RemoveAlarm);
                this.Invoke(d, new object[] { alarm });
            }
            else
            {
                m_Provider.InvokeRemove(alarm);
            }
        }
    }
}

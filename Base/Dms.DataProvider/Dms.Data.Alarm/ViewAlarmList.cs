using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Data
{
    public partial class ViewAlarmList : UserControl
    {
        private AlarmListProvider m_Provider;
        private BindingSource m_BindSource = new BindingSource();

        public ViewAlarmList()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void InitGridView(AlarmListProvider provider)
        {
            // Initialzie AlarmList
            m_Provider = provider;

            this.dataGridViewAlarmList.AutoGenerateColumns = false;
            this.dataGridViewAlarmList.DataSource = m_BindSource;
            m_BindSource.DataSource = m_Provider.List;
            m_BindSource.DataMember = "Items";


            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "Id";
            colId.HeaderText = "Id";
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmList.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Description";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewAlarmList.Columns.Add(colName);

            DataGridViewTextBoxColumn colLevel = new DataGridViewTextBoxColumn();
            colLevel.DataPropertyName = "Level";
            colLevel.HeaderText = "Level";
            colLevel.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmList.Columns.Add(colLevel);

            DataGridViewTextBoxColumn colCode = new DataGridViewTextBoxColumn();
            colCode.DataPropertyName = "Code";
            colCode.HeaderText = "Code";
            colCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewAlarmList.Columns.Add(colCode);
        }

        private void dataGridViewAlarmList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewAlarmList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                    if (row.Cells[2].Value.ToString() == _ALARM_L)
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

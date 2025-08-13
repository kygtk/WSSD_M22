using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Data
{
    public partial class ViewCimGlassApdHistory : UserControl
    {
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete;
        private delegate void AddApdDataCallback(ApdData apddata);

        private GlassApdHistoryProvider m_Provider;
        private BindingSource m_BindSource = new BindingSource();
        private int m_HistoryCount = 200;

        public DataGridView GridView
        {
            get { return this.dataGridViewGlassApdHistory; }
        }

        [Category("DMS : UI")]
        public String TitleName
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        [Category("DMS : UI")]
        public bool RowHeaderVisible
        {
            get { return this.dataGridViewGlassApdHistory.RowHeadersVisible; }
            set { this.dataGridViewGlassApdHistory.RowHeadersVisible = value; }
        }

        [Category("DMS : UI")]
        public bool ColumnHeaderVisible
        {
            get { return this.dataGridViewGlassApdHistory.ColumnHeadersVisible; }
            set { this.dataGridViewGlassApdHistory.ColumnHeadersVisible = value; }
        }

        [Category("DMS : Setting"),
        DefaultValue(200)]
        public int HistoryCount
        {
            get { return m_HistoryCount; }
            set
            {
                m_HistoryCount = value;
            }
        }

        public ViewCimGlassApdHistory()
        {
            InitializeComponent();
        }

        public void InitGridView(GlassApdHistoryProvider provider)
        {
            // Initialize Apd List
            m_Provider = provider;
            m_Provider.Viewer = this;

            //Biding to DataTable
            this.dataGridViewGlassApdHistory.AutoGenerateColumns = false;
            this.dataGridViewGlassApdHistory.DataSource = m_BindSource;
            m_BindSource.DataSource = m_Provider.Adapter.Table;

            int count = provider.Adapter.GlassApdInfo.Count;
            int ColumnCount = 0;

            for(int i = 0 ; i < count; i++ )
            {
                TagGlassApdInfo info = provider.Adapter.GlassApdInfo.Items[i];
                if (info.UseData)
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                    //                  column.DataPropertyName = provider.Adapter.GlassApdInfo.Items[i].Name;
                    //                  column.HeaderText = provider.Adapter.GlassApdInfo.Items[i].Name;

                    column.DataPropertyName = provider.Adapter.Table.Columns[ColumnCount].ColumnName;
                    column.HeaderText = provider.Adapter.Table.Columns[ColumnCount].ColumnName;



                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                    if (info.Format.Length > info.Name.Length)
                    {
                        column.Width = info.Format.Length * 10;
                    }
                    else
                    {
                        column.Width = column.HeaderText.Length * 10;
                    }

                    column.SortMode = DataGridViewColumnSortMode.NotSortable;



                    this.dataGridViewGlassApdHistory.Columns.Add(column);

/*
                    if (info.ApdUnitName == "CIM")
                    {
                        this.dataGridViewGlassApdHistory.Columns[ColumnCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                    }
                    else
                    {
                        this.dataGridViewGlassApdHistory.Columns[ColumnCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        this.dataGridViewGlassApdHistory.Columns[ColumnCount].Width = provider.Adapter.GlassApdInfo.Items[i].Name.Length * 12;
                    }

                    this.dataGridViewGlassApdHistory.Columns[ColumnCount].SortMode = DataGridViewColumnSortMode.NotSortable;
*/
                    ColumnCount += 1;
                }
            }

            this.dataGridViewGlassApdHistory.RowHeadersWidth = 70;
        }

        private void dataGridViewGlassApdHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        public void AddApdData(ApdData apddata)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewGlassApdHistory.InvokeRequired)
            {
                AddApdDataCallback d = new AddApdDataCallback(AddApdData);
                this.Invoke(d, new object[] { apddata });
            }
            else
            {
                m_Provider.InvokeAdd(apddata);
            }
        }

        private void dataGridViewGlassApdHistory_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           dataGridViewGlassApdHistory.CurrentCell = dataGridViewGlassApdHistory.Rows[e.RowIndex].Cells[0];

           for (int i = 0; i < dataGridViewGlassApdHistory.Rows.Count; i++)
           {
               dataGridViewGlassApdHistory.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
           }

        }

        private void dataGridViewGlassApdHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataBindingComplete(sender, e);
        }
    }
}

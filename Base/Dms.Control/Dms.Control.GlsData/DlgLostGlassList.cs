using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;

namespace Dms.Control
{
    public partial class DlgLostGlassList : Form
    {
        #region Fields
        private LostGlassDataProvider m_Provider = null;
        private BindingSource m_BindSource = new BindingSource();
        private int m_SelectedNo = -1;
        #endregion

        #region Properties
        public int SelectedNo
        {
            get { return m_SelectedNo; }
            set { m_SelectedNo = value; }
        }
        #endregion

        #region Constructor
        public DlgLostGlassList()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public void Initialize(LostGlassDataProvider dataProvider)
        {
            m_Provider = dataProvider;
            InitGridView();
            this.gridView.ClearSelection();
        }

        private void InitGridView()
        {
            this.gridView.AutoGenerateColumns = false;
            //this.gridView.DataSource = m_BindSource;
            //m_BindSource.DataSource = m_Provider.Adapter.Table;

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.DataPropertyName = "No";
            colNo.HeaderText = "No";
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.gridView.Columns.Add(colNo);

            DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn();
            colData.DataPropertyName = "Word0";
            colData.HeaderText = "Glass Number Code";
            colData.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.gridView.Columns.Add(colData);

            SetData();

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void SetData()
        {
            try
            {
                int rowCount = m_Provider.Adapter.Table.Rows.Count;
                gridView.RowCount = rowCount;
                DataSetGlassData.LostGlassDataRow row;

                for (int i = 0; i < rowCount; i++)
                {
                    row = m_Provider.Adapter.Find(i + 1);
                    if (row != null)
                    {
                        gridView[0, i].Value = row.No.ToString();
                        gridView[1, i].Value = string.Format("{0:X4}", row.Word0);
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRecovery_Click(object sender, EventArgs e)
        {
            //m_SelectedNo = this.gridView.CurrentRow.Index + 1;
            if (this.gridView.CurrentRow != null)
            {   // jemoon : Row가 하나도 없거나 선택되지 않으면 return
                int index = this.gridView.CurrentRow.Index;
                if (index >= 0)
                {
                    m_SelectedNo = Convert.ToInt32(this.gridView[0, index].Value);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
        #endregion
    }
}
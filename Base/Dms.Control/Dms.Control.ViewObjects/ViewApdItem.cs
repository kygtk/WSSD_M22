using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;

namespace Dms.Control
{
    public partial class ViewApdItem : UserControl
    {
        #region Fields
        private Dms.Device.ApdItemsHandler m_ApdHandler = null;
        private Dms.Device.ApdItems m_ApdItems = null;
        #endregion
  
        #region Constructor
        public ViewApdItem()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        public void Initialize(Dms.Device.ApdItemsHandler handler)
        {
            m_ApdHandler = handler;
            //m_ApdHandler.OnApdValueChange += new ApdValueChangeEventHandler(UpdateValue);
            // -1 : History용 저장소
            m_ApdItems = new Dms.Device.ApdItems();

            if ( m_ApdHandler != null) m_ApdItems.Clone(m_ApdHandler.GetItems(-1));
            
            InitDataView();
        }
        private void InitDataView()
        {
            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.HeaderText = "No";
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colNo);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.HeaderText = "Name";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colName);

            DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
            colValue.HeaderText = "Value";
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colValue);

            DataGridViewTextBoxColumn colUnit = new DataGridViewTextBoxColumn();
            colUnit.HeaderText = "Unit";
            colUnit.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colUnit);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if(m_ApdItems.Count > 0)
                this.dataGridView.Rows.Add(m_ApdItems.Count);

            SetData();

            tmrUpdateState.Enabled = true;
        }

        private void SetData()
        {
            int count = 0;
            foreach (Dms.Device.ApdItem item in m_ApdItems.Items)
            {
                this.dataGridView.Rows[count].SetValues(item.Id+1, item.Name, item.Value, item.ItemUnit);
                count++;
            }
            dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void UpdateValue()
        {
            Dms.Device.ApdItems history = m_ApdHandler.GetItems(-1);
            if (history != null && m_ApdItems.IsChanged(history))
            {
                m_ApdItems.Clone(history);

                SetData();
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateValue();
        }
        #endregion
    }
}

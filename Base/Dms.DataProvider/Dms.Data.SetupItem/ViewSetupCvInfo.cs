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
    public partial class ViewSetupCvInfo : UserControl
    {
        #region Fields
        private bool m_ViewOnly = false;
        private bool m_Editable = true;
        private UserLevels m_EditableUserLevel = UserLevels.Engineer;
        private UserLevels m_CurUserLevel;
        #endregion

        public String TitleName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }
        [Category("DMS : Setting"),
        DefaultValue(UserLevels.Engineer)]
        public UserLevels EditableUserLevel
        {
            get { return m_EditableUserLevel; }
            set { m_EditableUserLevel = value; }
        }

        [Category("DMS : Setting"),
        DefaultValue(false)]
        public bool ViewOnly
        {
            get { return m_ViewOnly; }
            set 
            { 
                m_ViewOnly = value;
                this.dataGridView.ReadOnly = value;
            }
        }
        
        public ViewSetupCvInfo()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            //gridview폰트를 바꿀려면 아래처럼
            //DataGridViewCellStyle cellStyle = this.dataGridView.DefaultCellStyle;
            //cellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public void InitDataView(DataTable table)
        {
            //Biding to DataTable
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.DataSource = table;

            //DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            //colId.DataPropertyName = "ItemId";
            //colId.HeaderText = "Id";
            //colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridView.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colName);

            DataGridViewTextBoxColumn colVel = new DataGridViewTextBoxColumn();
            colVel.DataPropertyName = "VelRatio";
            colVel.HeaderText = "Velocity Ratio";
            colVel.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colVel);

            DataGridViewTextBoxColumn colGear = new DataGridViewTextBoxColumn();
            colGear.DataPropertyName = "GearRatio";
            colGear.HeaderText = "Gear Ratio";
            colGear.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colGear);

            DataGridViewTextBoxColumn colDia = new DataGridViewTextBoxColumn();
            colDia.DataPropertyName = "DiaMeter";
            colDia.HeaderText = "DiaMeter";
            colDia.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colDia);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void SetEditable(UserLevels curUserLevel)
        {
            m_CurUserLevel = curUserLevel;
        }

        private bool CheckEditable(int rowIndex, int colIndex)
        {
            if (m_ViewOnly)
            {
                m_Editable = false;
            }
            else
            {
                if (m_EditableUserLevel <= m_CurUserLevel)    //column에 따라 조건이 달라질 수 있는 경우 ||로 판단
                {
                    m_Editable = true;
                }
                else
                {
                    m_Editable = false;
                }
            }
            return m_Editable;
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // index == -1 : Header
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            // column index == 0 : Item Name
            if (e.ColumnIndex == 0) return;

            if (CheckEditable(e.RowIndex, e.ColumnIndex) == false)
            {
                return;
            }
            else
            {
                DataRowView rowView = dataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                DataTable table = rowView.DataView.ToTable();
                DataColumnCollection columns = table.Columns;
                DataGridViewCell cell = dataGridView[e.ColumnIndex, e.RowIndex];

                string oldValue = cell.Value.ToString();
                string curValue = "";
                string hiLimit = "";
                string lowLimit = "";
                string caption = "";
                OptionFormat optionFormat;
                KeyInValidation validation = new KeyInValidation();

                if (columns.Contains("HiTerm") && columns.Contains("LoTerm"))
                {
                    hiLimit = rowView["HiTerm"].ToString();
                    lowLimit = rowView["LoTerm"].ToString();
                }
                if (hiLimit == " ") hiLimit = hiLimit.Trim();
                if (lowLimit == " ") lowLimit = lowLimit.Trim();
                if (columns.Contains("Name"))
                {
                    caption = rowView["Name"].ToString();
                }

                validation.High = hiLimit;
                validation.Low = lowLimit;

                if (columns.Contains("OptionFormat") && columns.Contains("OptionType"))
                {
                    optionFormat = (OptionFormat)Enum.Parse(typeof(OptionFormat), rowView["OptionFormat"].ToString());
                    validation.Format = optionFormat;
                }
                else
                {
                    Type type = columns[e.ColumnIndex].DataType;
                    if (type == typeof(string)) validation.Format = OptionFormat.String;
                    else if (type == typeof(bool)) validation.Format = OptionFormat.Boolean;
                    else if (type == typeof(int)) validation.Format = OptionFormat.Digit;
                    else if (type == typeof(double) || type == typeof(float)) validation.Format = OptionFormat.Float;
                }

                curValue = validation.ShowEditDialog(caption, oldValue);
                if (oldValue != curValue)
                {
                    cell.Value = curValue;
                }
            }

        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }

    }
}

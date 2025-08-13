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
    public partial class ViewHsmsSetupInfo : UserControl
    {
        #region Fields
        private DataTable m_Table;
        private bool m_TitleVisible = true;
        private bool m_Editable = true;
        private int m_TitleHeight = 25;
        private UserLevels m_EditableUserLevel = UserLevels.Engineer;
        private bool m_ViewOnly = false;
        private UserLevels m_CurUserLevel;
        private int m_UnEditableColumnIndex = -1;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public String TitleName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }
        [Category("DMS : UI"),
        DefaultValue(true)]
        public bool TitleVisible
        {
            get { return m_TitleVisible; }
            set
            {
                m_TitleVisible = value;
                if (m_TitleVisible == false)
                {
                    m_TitleHeight = 0;
                }
                else
                {
                    if (m_TitleHeight == 0)
                    {
                        m_TitleHeight = 25;
                    }
                }
                splitContainer1.SplitterDistance = m_TitleHeight;
            }
        }
        [Category("DMS : UI"),
        DefaultValue(25)]
        public int TitleHeight
        {
            get { return m_TitleHeight; }
            set
            {
                m_TitleHeight = value;
                splitContainer1.SplitterDistance = m_TitleHeight;
            }
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

        [Browsable(false)]
        public DataTable Table
        {
            get { return m_Table; }
        }

        #endregion

        public ViewHsmsSetupInfo()
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
            m_Table = table;
            //Biding to DataTable
            this.dataGridView.AutoGenerateColumns = true;
            this.dataGridView.DataSource = table;

            bool sensorInterlockViewer = ((table as DataSetSetupItem.SetupSensorInterlockDataTable) != null);
            int columnCount = dataGridView.Columns.Count;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                if (sensorInterlockViewer)
                {   // jemoon : Sensor interlock viewer는 마지막 column이 sensor type을 나타내므로 숨기자
                    column.Visible = (column.Index != columnCount - 1);
                }
            }
        }

        public void InitDataView(DataTable table, bool unitVisible)
        {
            m_Table = table;

            //Biding to DataTable
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.DataSource = m_Table;

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "ID";
            colId.HeaderText = "ID";
            colId.Name = "ID";
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colName);

            DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
            colValue.DataPropertyName = "ItemValue";
            colValue.HeaderText = "Value";
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colValue);

            DataGridViewTextBoxColumn colMode = new DataGridViewTextBoxColumn();
            colMode.DataPropertyName = "Mode";
            colMode.HeaderText = "Mode";
            colMode.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            colMode.Visible = false;
            this.dataGridView.Columns.Add(colMode);

            if (unitVisible)
            {
                DataGridViewTextBoxColumn colUnit = new DataGridViewTextBoxColumn();
                colUnit.DataPropertyName = "UnitType";
                colUnit.HeaderText = "Unit";
                colUnit.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dataGridView.Columns.Add(colUnit);
            }

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            this.dataGridView.Sort(dataGridView.Columns["ID"], ListSortDirection.Ascending);
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
                if (m_EditableUserLevel > m_CurUserLevel ||
                   (m_UnEditableColumnIndex != -1 && m_UnEditableColumnIndex == colIndex))    //column에 따라 조건이 달라질 수 있는 경우 ||로 판단
                {
                    m_Editable = false;
                }
                else
                {
                    m_Editable = true;
                }
            }
            return m_Editable;
        }

        private void GetStyleByOptionFormat(string format, string value, ref Color backColor, ref Color foreColor)
        {
            Color back = new Color();
            Color fore = new Color();
            OptionFormatHelper.GetStyleByOptionFormat(format, value, ref back, ref fore);
            backColor = back;
            foreColor = fore;
        }

        private void GetStyleByUnitType(string type, ref Color backColor, ref Color foreColor)
        {
            Color back = new Color();
            Color fore = new Color();
            OptionFormatHelper.GetStyleByUnitType(type, ref back, ref fore);
            backColor = back;
            foreColor = fore;
        }

        public void SetStyle()
        {
            int rowCount = dataGridView.Rows.Count;
            int colCount = dataGridView.Columns.Count;
            DataRowView rowView;
            DataGridViewCell viewCell;
            Color backColor = new Color();
            Color foreColor = new Color();

            for (int row = 0; row < rowCount; row++)
            {
                rowView = dataGridView.Rows[row].DataBoundItem as DataRowView;

                if ( rowView["Mode"].ToString() != GridViewMode.OnlyRead.ToString())
                {
                    for (int column = 0; column < colCount; column++)
                    {
                        viewCell = dataGridView[column, row];
                        string dataPropertyName = dataGridView.Columns[column].DataPropertyName;
                        if (dataPropertyName == "ItemValue")
                        {
                            GetStyleByOptionFormat(rowView["OptionFormat"].ToString(), viewCell.Value.ToString(), ref backColor, ref foreColor);
                            viewCell.Style.BackColor = backColor;
                            viewCell.Style.ForeColor = foreColor;
                        }
                        else if (dataPropertyName == "UnitType")
                        {
                            GetStyleByUnitType(viewCell.Value.ToString(), ref backColor, ref foreColor);
                            viewCell.Style.BackColor = backColor;
                            viewCell.Style.ForeColor = foreColor;
                            m_UnEditableColumnIndex = column;
                        }
                        else if (dataPropertyName == "InterlockParam")
                        {
                            m_UnEditableColumnIndex = column;
                        }
                    }
                }
                else
                {
                    for (int column = 0; column < colCount; column++)
                    {
                        viewCell = dataGridView[column, row];
                        viewCell.Style.BackColor = Color.Silver;
                        viewCell.Style.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // TODO : Check data validation.
            // 변경된 셀에 대해 다시 표시
            //if (String.IsNullOrEmpty(dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString()))
            //{
            //    dataGridView.Rows[e.RowIndex].ErrorText = "Value must not be empty";
            //    dataGridView[e.ColumnIndex, e.RowIndex].ErrorText = "Value must not be empty";
            //}
            //dataGridView[e.ColumnIndex, e.RowIndex].ErrorText = String.Empty;
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetStyle();
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (/*e.FormattedValue == null && */String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                MessageBox.Show("Value must not be empty");
                e.Cancel = true;
                //dataGridView[e.ColumnIndex, e.RowIndex].ErrorText = "Value must not be empty";
                //dataGridView.Rows[e.RowIndex].ErrorText = "Value must not be empty";
            }
            else
            {
            }
            //if (String.IsNullOrEmpty(dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString()))
            //{
            //    dataGridView[e.ColumnIndex, e.RowIndex].ErrorText = "Value must not be empty";
            //    e.Cancel = true;
            //}
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // index == -1 : Header
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            // column index == 0 : Item Name
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) return;

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

                if (rowView["Mode"].ToString() == GridViewMode.OnlyRead.ToString()) return;

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
                    SetStyle();
                }
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }
    }
}

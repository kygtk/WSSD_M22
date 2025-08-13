using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Data
{
    public partial class ViewRecipe : UserControl
    {
        private delegate void RecipeAddCallback(TagRecipe recipe);
        private delegate void RecipeAddCallback2(DataSetRecipe.RecipeRow recipe);
        private delegate void RecipeDelCallback(string recipeId);

        private RecipeProvider m_Provider; // 10.12.25 minhan
        private DataTable m_Table;
        private _GenInfoHandler m_GenInfos;//2009.08.04 kimgun 10.12.25 minhan
        private bool m_TitleVisible = true;
        private bool m_Editable = true;
        private int m_TitleHeight = 25;
        private UserLevels m_EditableUserLevel; // 10.12.25 minhan
        private bool m_ReadOnly = false;
        private bool m_OnlyCurrent = false;
        private bool m_Vertical = false;
        private int m_SkipItems = 0;
        private UserLevels m_CurUserLevel;
        // static private int m_GlassSize = 0; // 10.12.25 minhan
        // static private bool m_AutoMode = false;// 10.12.25 minhan

        [Category("DMS : UI")]
        public String TitleName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }

        public DataGridView GridView
        {
            get { return this.dataGridView; }
        }

        public DataSetRecipe.RecipeRow SelectedRow
        {
            get
            {
                if (this.dataGridView.SelectedRows.Count == 0)
                {
                    return null;
                }
                else
                {
                    // 한개의 row만 선택가능토록 해야함.
                    return (DataSetRecipe.RecipeRow)(((DataRowView)(this.dataGridView.SelectedRows[0].DataBoundItem)).Row);
                }
            }
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
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;
                this.dataGridView.ReadOnly = value;
            }
        }
        [Browsable(false), XmlIgnore()]
        public bool Vertical
        {
            get { return m_Vertical; }
        }

        [Category("DMS : Setting"),
        Description("This value only for current recipe view")]
        public int SkipItems
        {
            get { return m_SkipItems; }
            set { m_SkipItems = value; }
        }
        //public int GlassSize // 10.12.25 minhan
        //{//2009.07.30 kimgun
        //    get { return m_GlassSize; }
        //    set { m_GlassSize = value; }
        //}
        //public bool AutoMode
        //{//2009.07.30 kimgun
        //    get { return m_AutoMode; }
        //    set { m_AutoMode = value; }
        //}
        public _GenInfoHandler GenInfos
        {//2009.08.04 kimgun
            get { return m_GenInfos; }
            set { m_GenInfos = value; }
        }

        public ViewRecipe()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_EditableUserLevel = UserLevels.Engineer; // 10.12.25 minhan
            //gridview폰트를 바꿀려면 아래처럼
            //DataGridViewCellStyle cellStyle = this.dataGridView.DefaultCellStyle;
            //cellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public void InitDataView(RecipeProvider provider, bool vertical, bool onlyCurrent)
        {
            try
            {
                m_Vertical = vertical;
                m_OnlyCurrent = onlyCurrent;
                m_Provider = RecipeProvider.Instance; // 10.12.25 minhan

                if (!m_OnlyCurrent)
                {   // Display all recipes
                    if (!m_Vertical)
                    {   // Display horizontal
                        // Biding to DataTable
                        m_Provider = provider;
                        m_Provider.Viewer.Add(this);
                        m_Table = m_Provider.Adapter.Table;
                        this.dataGridView.AutoGenerateColumns = true;
                        this.dataGridView.DataSource = m_Table;

                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            column.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        this.dataGridView.Columns[1].Frozen = true;
                        this.dataGridView.Columns[1].DividerWidth = 3;
                    }
                    else
                    {
                        // Display Vertical
                        // Can not bind data
                        // Not ready
                    }
                }
                else
                {   // Display Only current recipe
                    if (!m_Vertical)
                    {   // Display horizontal
                        // Can bind data
                        // Not ready
                    }
                    else
                    {   // Display Vertical
                        // Can not bind data
                        m_Provider = provider;
                        m_Provider.Viewer.Add(this);
                        m_Table = m_Provider.Adapter.Table;
                        this.dataGridView.AutoGenerateColumns = false;

                        DataGridViewCellStyle columnStyle = this.dataGridView.ColumnHeadersDefaultCellStyle;
                        columnStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                        colName.HeaderText = "Name";
                        colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dataGridView.Columns.Add(colName);

                        DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
                        colValue.HeaderText = "Value";
                        colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        this.dataGridView.Columns.Add(colValue);

                        // Vertical View 일경우에는 databinding이 되지 않으므로
                        SetCurrentData();

                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            column.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                //MessageBox.Show(err.Message.ToString());
            }
        }

        public void SetCurrentData()
        {
            if (m_Vertical)
            {
                int startColumnIndex = m_SkipItems;
                this.dataGridView.RowCount = m_Table.Columns.Count - startColumnIndex;

                if (!m_OnlyCurrent)
                {   // Display All recipes
                    // Not ready
                }
                else
                {   // Display Only Current Recipe
                    int curRecipeRowIndex = 0;
                    // Find Current recipe
                    int count = m_Table.Rows.Count;
                    DataRowCollection rows = m_Table.Rows;
                    for (int i = 0; i < count; i++)
                    {
                        DataSetRecipe.RecipeRow currentRow = (DataSetRecipe.RecipeRow)rows[i];
                        if (currentRow.V)
                        {
                            curRecipeRowIndex = i;
                            break;
                        }
                    }

                    int rowCount = dataGridView.RowCount;
                    for (int i = 0; i < rowCount; i++)
                    {
                        dataGridView[0, i].Value = m_Table.Columns[startColumnIndex].ColumnName;
                        dataGridView[1, i].Value = m_Table.Rows[curRecipeRowIndex][startColumnIndex++].ToString();
                    }

                }
            }
        }

        public void AddRecipe(TagRecipe item)
        {
            if (!m_OnlyCurrent && !m_Vertical)
            {
                if (this.dataGridView.InvokeRequired)
                {
                    RecipeAddCallback d = new RecipeAddCallback(AddRecipe);
                    this.Invoke(d, new object[] { item });
                }
                else
                {
                    m_Provider.InvokeAdd(item);
                }
            }
            else
            {
                m_Provider.InvokeAdd(item);
            }
        }

        public void AddRecipe(DataSetRecipe.RecipeRow recipe)
        {
            if (!m_OnlyCurrent && !m_Vertical)
            {
                if (this.dataGridView.InvokeRequired)
                {
                    RecipeAddCallback2 d = new RecipeAddCallback2(AddRecipe);
                    this.Invoke(d, new object[] { recipe });
                }
                else
                {
                    m_Provider.InvokeAdd(recipe);
                }
            }
            else
            {
                m_Provider.InvokeAdd(recipe);
            }
        }

        public void RemoveRecipe(string recipeId)
        {
            if (!m_OnlyCurrent && !m_Vertical)
            {
                if (this.dataGridView.InvokeRequired)
                {
                    RecipeDelCallback d = new RecipeDelCallback(RemoveRecipe);
                    this.Invoke(d, new object[] { recipeId });
                }
                else
                {
                    m_Provider.InvokeRemove(recipeId);
                }
            }
            else
            {
                m_Provider.InvokeRemove(recipeId);
            }
        }

        public void SetEditable(UserLevels curUserLevel)
        {
            m_CurUserLevel = curUserLevel;
        }

        private bool CheckEditable(int rowIndex, int colIndex)
        {
            if (m_ReadOnly)
            {
                m_Editable = false;
            }
            else
            {
                if (!m_Vertical) // 10.12.21 minhan
                {
                    DataRowCollection rows = m_Table.Rows;//2009.09.14 kimgun
                    DataSetRecipe.RecipeRow currentRow = (DataSetRecipe.RecipeRow)rows[rowIndex];//2009.09.14 kimgun

                    if ((m_EditableUserLevel <= m_CurUserLevel) &&
                        (/*currentRow.V == false || */m_GenInfos.AutoMode != true))//10.12.25 minhan 2009.09.14 kimgun    //column에 따라 조건이 달라질 수 있는 경우 ||로 판단
                    {
                        m_Editable = true;
                    }
                    else
                    {
                        m_Editable = false;
                    }
                }
                else
                {
                    if ((m_EditableUserLevel <= m_CurUserLevel) && (m_GenInfos.AutoMode != true))
                    {
                        m_Editable = true;
                    }
                    else
                    {
                        m_Editable = false;
                    }
                }
            }
            return m_Editable;
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // if (GenInfos.AutoMode) return;//2009.07.30 kimgun
            // index == -1 : Header
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            // case에 따라 index를 만들고
            int recipeIndex;
            int itemIndex;
            if (m_Vertical)
            {
                recipeIndex = e.ColumnIndex - 1;
                itemIndex = e.RowIndex;
            }
            else
            {
                recipeIndex = e.RowIndex;
                itemIndex = e.ColumnIndex;
            }

            RecipeItem item = (RecipeItem)itemIndex;

            // 수정하면 안되는 item들은 skip한다.
            bool skip = (CheckEditable(e.RowIndex, e.ColumnIndex) == false);
            if (m_Vertical)
            {
                // vertical일 경우 column 0 은 item name에 해당함 -> skip
                skip |= (e.ColumnIndex == 0);
            }

            skip |= (item == RecipeItem.V);
            skip |= (item == RecipeItem.Id);
            skip |= (item == RecipeItem.ChangedTime);

            // Popup KeyPad Dialog 
            if (skip)
            {
                return;
            }
            else
            {
                DataSetRecipe.RecipeRow row;

                if (m_Vertical) // 10.12.21 minhan 
                {
                    int curRecipeRowIndex = 0;

                    int count = m_Table.Rows.Count;
                    DataRowCollection rows = m_Table.Rows;
                    for (int i = 0; i < count; i++)
                    {
                        DataSetRecipe.RecipeRow currentRow = (DataSetRecipe.RecipeRow)rows[i];
                        if (currentRow.V)
                        {
                            curRecipeRowIndex = i;
                            break;
                        }
                    }
                    row = (DataSetRecipe.RecipeRow)m_Table.Rows[curRecipeRowIndex];
                }
                else row = (DataSetRecipe.RecipeRow)m_Table.Rows[recipeIndex];
                DataColumn col = ((DataSetRecipe.RecipeDataTable)m_Table).Columns[itemIndex];
                DataGridViewCell cell = this.dataGridView[e.ColumnIndex, e.RowIndex];
                string oldValue = cell.Value.ToString();
                string caption = col.ColumnName;
                KeyInValidation validation = new KeyInValidation();
                if (col.DataType == typeof(string)) validation.Format = OptionFormat.String;
                else if (col.DataType == typeof(bool)) validation.Format = OptionFormat.Boolean;
                else if (col.DataType == typeof(int)) validation.Format = OptionFormat.Digit;
                else if (col.DataType == typeof(double) || col.DataType == typeof(float)) validation.Format = OptionFormat.Float;

                string curValue = validation.ShowEditDialog(caption, oldValue);
                if (oldValue != curValue)
                {
                    bool checkValidation = TagRecipe.CheckValidation(itemIndex, curValue);

                    if (!checkValidation)
                    {
                        MessageBox.Show("Invalid data : " + curValue);
                    }
                    else
                    {
                        //cell.Value = curValue;

                        // 수정후 처리
                        //if (m_Vertical)
                        //{   //Vertical viewer일 경우에는 databinding이 되어 있지 않기 때문에.
                        row.BeginEdit();
                        row[itemIndex] = curValue;
                        row.EndEdit();
                        //}

                        //2009.06.17 Youngsik.
                        if (m_Provider.ChangedId.Contains(row.ID) == false)
                        {
                            m_Provider.ChangedId.Add(row.ID);
                        }
                        if (m_Vertical || (row.V)) m_Provider.SetCurrentRecipe(row.ID); // 10.12.21 minhan
                    }
                }
            }
        }


    }
}

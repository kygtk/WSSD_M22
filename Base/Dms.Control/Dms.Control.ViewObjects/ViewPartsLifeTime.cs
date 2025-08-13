using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using System.Collections;
using Dms.Device;

namespace Dms.Control
{
    public partial class ViewPartsLifeTime : UserControl
    {
        #region Enum
        private enum Header
        {
            No, Name, Cur_Glass, Max_Glass, Cur_Time, Max_Time
        }
        #endregion

        #region Fields
        private Dms.Device.PartsItemsHandler m_Handler = null;
        private Dms.Device.PartsItems m_Items = null;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public string TitleText
        {
            get { return labelTitle.Text; }
            set { labelTitle.Text = value; }
        }
        [Category("DMS : UI")]
        public Font GridViewFont
        {
            get { return dataGridView.Font; }
            set { dataGridView.Font = value; }
        }
        #endregion

        #region Constructor
        public ViewPartsLifeTime()
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
        public void Initialize(Dms.Device.PartsItemsHandler handler)
        {
            m_Handler = handler;
            //m_Handler.OnPartsLifeTimeChange += new PartsLifeTimeChangeEventHandler(UpdateValue);
            m_Items =  new Dms.Device.PartsItems();
            m_Items.Clone(m_Handler.GetItems());

            InitDataView();
        }
        private void InitDataView()
        {
            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.HeaderText = Header.No.ToString();
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colNo);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.HeaderText = Header.Name.ToString();
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colName);

            if (m_Handler.UseGlassCount)
            {
                DataGridViewTextBoxColumn colCurCount = new DataGridViewTextBoxColumn();
                colCurCount.HeaderText = Header.Cur_Glass.ToString() + " (EA)";
                colCurCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView.Columns.Add(colCurCount);

                DataGridViewTextBoxColumn colMaxCount = new DataGridViewTextBoxColumn();
                colMaxCount.HeaderText = Header.Max_Glass.ToString() + " (EA)";
                colMaxCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView.Columns.Add(colMaxCount);
            }

            DataGridViewTextBoxColumn colCurTime = new DataGridViewTextBoxColumn();
            colCurTime.HeaderText = Header.Cur_Time.ToString() + " (H)";
            colCurTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colCurTime);

            DataGridViewTextBoxColumn colMaxTime = new DataGridViewTextBoxColumn();
            colMaxTime.HeaderText = Header.Max_Time.ToString() + " (H)";
            colMaxTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colMaxTime);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            m_Items = m_Handler.GetItems();
            if(m_Items.Count > 0)
                this.dataGridView.Rows.Add(m_Items.Count);

            SetData();

            timerUpdateState.Enabled = true;
        }

        private void SetData()
        {
            int count = 0;
            foreach (Dms.Device.PartsItem item in m_Items.Items)
            {
                if (m_Handler.UseGlassCount)
                {
                    this.dataGridView.Rows[count].SetValues(item.Id + 1, item.Name,
                                                            item.CurGlsCount, item.MaxGlsCount,
                                                            item.CurUsedTime, item.MaxUsedTime);
                }
                else
                {
                    this.dataGridView.Rows[count].SetValues(item.Id + 1, item.Name,
                                                            item.CurUsedTime, item.MaxUsedTime);
                }
                count++;
            }
        }

        private void UpdateValue()
        {
            Dms.Device.PartsItems history = m_Handler.GetItems();
            if (m_Items.IsChanged(history))
            {
                m_Items.Clone(history);

                SetData();
            }
        }
        
        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateValue();
        }
        
        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.KeyCode < Keys.D0 && e.KeyCode > Keys.D9))
            {
                e.Handled = true;
            }
        }
        
        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        #endregion

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // index == -1 : Header
            if ((e.ColumnIndex == -1) || (e.RowIndex == -1)) return;

            int colIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            Header header = (Header)colIndex;

            if ((header == Header.No) || (header == Header.Name))
            {
                return;
            }
            else if((header == Header.Cur_Glass) || (header == Header.Cur_Time))
            {
                if (MessageBox.Show("Do you want to reset the current value?", "CURRENT VALUE RESET", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_Handler.Reset(rowIndex);
                }
            }
            else
            {
                KeyInValidation validatoin = new KeyInValidation();
                validatoin.Format = OptionFormat.Digit;
                string caption = (string)dataGridView[(int)Header.Name, e.RowIndex].Value;
                string curValue = dataGridView[colIndex, rowIndex].Value.ToString();
                string newValue = validatoin.ShowEditDialog(caption, curValue);
                
                if( curValue != newValue)
                {
                    dataGridView[e.ColumnIndex, e.RowIndex].Value = newValue;
                    int nValue = Convert.ToInt32(newValue);

                    //GlassCount를 사용하지 않을 경우에는 column index가 Header.Max_Glass이지만 SetMaxTime()함수를 호출해야 한다.
                    if (m_Handler.UseGlassCount == false)
                    {
                        header = Header.Max_Time;
                    }

                    switch (header)
                    {
                        case Header.Max_Glass:
                            {
                                m_Handler.SetMaxCount(rowIndex, nValue);
                            }
                            break;
                        case Header.Max_Time:
                            {
                                m_Handler.SetMaxTime(rowIndex, nValue);
                            }
                            break;
                    }
                }
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }
    }
}

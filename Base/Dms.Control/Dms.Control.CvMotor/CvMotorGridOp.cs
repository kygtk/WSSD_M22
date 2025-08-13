using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class CvMotorGridOp : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Enum
        private enum Items
        {
            name, stop, fw, bw, speed, alarm, cpon
        }
        #endregion

        #region Fields
        private int m_Interval = 0;
        private int m_CurrentSpeed = 0;
        private int m_CommandSpeed = 0;
        private int m_OldCurrentSpeed = 0;
        private int m_RecipeSpeed = 0;
        private int m_TagsCount = 0;
        private DeviceTags m_TagOlds = new DeviceTags();
        private DataGridViewCellStyle cellBoldStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle cellNormalStyle = new DataGridViewCellStyle();
        #endregion

        public delegate void CvMotorClickEventHandler(DeviceTag tag, CvMotorAct act, int speed);
        public event CvMotorClickEventHandler CvMotorClick;

        #region Constructor
        public CvMotorGridOp(DeviceTags tags, int recipeSpeed, int interval)
        {
            InitializeComponent();

            m_Tags = tags;
            m_TagsCount = m_Tags.Count;
            m_RecipeSpeed = recipeSpeed;
            m_Interval = interval;

            InitGridView();
        }
        #endregion

        #region Methods
        private void InitGridView()
        {
            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.HeaderText = "NAME";
            colName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridOperation.Columns.Add(colName);

            DataGridViewDisableButtonColumn colStop = new DataGridViewDisableButtonColumn();
            colStop.HeaderText = "STOP";
            colStop.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridOperation.Columns.Add(colStop);

            DataGridViewDisableButtonColumn colFw = new DataGridViewDisableButtonColumn();
            colFw.HeaderText = "FW";
            colFw.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridOperation.Columns.Add(colFw);

            DataGridViewDisableButtonColumn colBw = new DataGridViewDisableButtonColumn();
            colBw.HeaderText = "BW";
            colBw.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridOperation.Columns.Add(colBw);

            DataGridViewTextBoxColumn colSpeed = new DataGridViewTextBoxColumn();
            colSpeed.HeaderText = "SPEED";
            colSpeed.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridOperation.Columns.Add(colSpeed);

            DataGridViewCheckBoxColumn colAlarm = new DataGridViewCheckBoxColumn();
            colAlarm.HeaderText = "ALARM";
            colAlarm.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridOperation.Columns.Add(colAlarm);

            DataGridViewCheckBoxColumn colCpOn = new DataGridViewCheckBoxColumn();
            colCpOn.HeaderText = "CPON";
            colCpOn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridOperation.Columns.Add(colCpOn);

            for (int i = 0; i < (int)Items.cpon; i++)
                gridOperation.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            cellBoldStyle.Font = new Font("arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(129)));
            cellNormalStyle.Font = new Font("arial", 9F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(129)));
        }

        private void SetGridViewData()
        {
            this.gridOperation.RowCount = m_Tags.Count;
            int count = gridOperation.RowCount;
            for (int i = 0; i < count; i++)
            {
                gridOperation[(int)Items.name, i].Value = m_Tags.Items[i].DeviceName;
                gridOperation[(int)Items.stop, i].Value = "STOP";
                gridOperation[(int)Items.fw, i].Value = "FW";
                gridOperation[(int)Items.bw, i].Value = "BW";
                //((DataGridViewDisableButtonCell)gridOperation[(int)Items.stop, i]).FlatStyle = FlatStyle.Flat;
                gridOperation[(int)Items.stop, i].Style.BackColor = Color.Pink;
                //gridOperation[(int)Items.stop, i].Style.SelectionBackColor = Color.HotPink;
                //((DataGridViewDisableButtonCell)gridOperation[(int)Items.fw, i]).FlatStyle = FlatStyle.Flat;
                gridOperation[(int)Items.fw, i].Style.BackColor = Color.LightCyan;
                //gridOperation[(int)Items.fw, i].Style.SelectionBackColor = Color.DarkCyan;
                //((DataGridViewDisableButtonCell)gridOperation[(int)Items.bw, i]).FlatStyle = FlatStyle.Flat;
                gridOperation[(int)Items.bw, i].Style.BackColor = Color.LemonChiffon;
                //gridOperation[(int)Items.bw, i].Style.SelectionBackColor = Color.Gold;
            }
        }
        #endregion

        private void gridOperation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            CvMotorAct act = CvMotorAct.Noop;

            bool cancelCond = false;
            cancelCond |= (colIndex < (int)Items.stop);
            cancelCond |= (colIndex > (int)Items.bw);
            cancelCond |= rowIndex < 0;
            cancelCond |= rowIndex > m_Tags.Count;
            if (cancelCond) return;

            if (!((DataGridViewDisableButtonCell)gridOperation[colIndex, rowIndex]).Enabled) return;

            switch (colIndex)
            {
                case (int)Items.stop:
                    {
                        act = CvMotorAct.Stop;
                    }
                    break;
                case (int)Items.fw:
                    {
                        act = CvMotorAct.Fw;
                    }
                    break;
                case (int)Items.bw:
                    {
                        act = CvMotorAct.Bw;
                    }
                    break;
            }

            if (CvMotorClick != null)
            {
                CvMotorClick(m_Tags.Items[rowIndex], act, m_CommandSpeed);
            }
        }

        private void CvMotorGridOp_Load(object sender, EventArgs e)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            for (int i = 0; i < m_TagsCount; i++)
            {
                DeviceTag tag = new DeviceTag();
                tag.Clone(m_Tags.Items[i]);
                m_TagOlds.Add(tag);
            }

            SetGridViewData();

            m_Initialized = true;

            // Timer를 설정한다.
            tmrUpdateState = new System.Windows.Forms.Timer();
            tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);
            tmrUpdateState.Enabled = true;

            for (int i = 0; i < m_TagsCount; i++)
            {
                UpdateState(i);
            }
        }

        private void UpdateState(int i)
        {
            {
                gridOperation[(int)Items.alarm, i].Value =
                    (m_Tags.Items[i][tagDescriptor.ALARM].Value == "1") || (m_Tags.Items[i][tagDescriptor.ALARM].Value == bool.TrueString);
                gridOperation[(int)Items.cpon, i].Value =
                    (m_Tags.Items[i][tagDescriptor.CPON].Value == "1") || (m_Tags.Items[i][tagDescriptor.CPON].Value == bool.TrueString);

                if (m_Tags.Items[i][tagDescriptor.STOP].Value == "1" || m_Tags.Items[i][tagDescriptor.STOP].Value == bool.TrueString)
                {
                    gridOperation[(int)Items.stop, i].Style.Font = cellBoldStyle.Font;
                    gridOperation[(int)Items.fw, i].Style.Font = cellNormalStyle.Font;
                    gridOperation[(int)Items.bw, i].Style.Font = cellNormalStyle.Font;
                    ((DataGridViewDisableButtonCell)gridOperation[(int)Items.fw, i]).Enabled = true;
                    ((DataGridViewDisableButtonCell)gridOperation[(int)Items.bw, i]).Enabled = true;

                }
                else if (m_Tags.Items[i][tagDescriptor.FW].Value == "1" || m_Tags.Items[i][tagDescriptor.FW].Value == bool.TrueString)
                {
                    gridOperation[(int)Items.fw, i].Style.Font = cellBoldStyle.Font;
                    gridOperation[(int)Items.stop, i].Style.Font = cellNormalStyle.Font;
                    gridOperation[(int)Items.bw, i].Style.Font = cellNormalStyle.Font;
                    ((DataGridViewDisableButtonCell)gridOperation[(int)Items.bw, i]).Enabled = false;
                }
                else if (m_Tags.Items[i][tagDescriptor.BW].Value == "1" || m_Tags.Items[i][tagDescriptor.BW].Value == bool.TrueString)
                {
                    gridOperation[(int)Items.bw, i].Style.Font = cellBoldStyle.Font;
                    gridOperation[(int)Items.stop, i].Style.Font = cellNormalStyle.Font;
                    gridOperation[(int)Items.fw, i].Style.Font = cellNormalStyle.Font;
                    ((DataGridViewDisableButtonCell)gridOperation[(int)Items.fw, i]).Enabled = false;
                }

                m_CurrentSpeed = Convert.ToInt32(m_Tags.Items[i][tagDescriptor.SPEED].Value);
                //if (m_OldCurrentSpeed != m_CurrentSpeed)
                {
                    gridOperation[(int)Items.speed, i].Value = m_CurrentSpeed.ToString();
                    m_OldCurrentSpeed = m_CurrentSpeed;
                }
            }
            gridOperation.InvalidateRow(i);
            //gridOperation.InvalidateCell((int)Items.fw, i);
            //gridOperation.InvalidateCell((int)Items.bw, i);


            //this.gridOperation.Refresh();
        }

        #region Override
        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;
            DeviceTag oldTag;
            DeviceTag curTag;
            for (int i = 0; i < m_TagsCount; i++)
            {
                oldTag = m_TagOlds.Items[i];
                curTag = m_Tags.Items[i];
                if (oldTag.IsChanged(curTag))
                {
                    oldTag.Clone(m_Tags.Items[i]);
                    UpdateState(i);
                }
            }
        }
        #endregion
    }
}

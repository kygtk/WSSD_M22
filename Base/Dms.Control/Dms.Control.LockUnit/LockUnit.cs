using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Control;
using Dms.Common;

namespace Dms.Control
{
    public partial class LockUnit : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorLockUnit tagDescriptor = new TagDescriptorLockUnit();
        #endregion

        [Category("DMS : Basic Info"),
        Description("Select Title Text")]
        public string TitleText
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }
        [Category("DMS : Basic Info"),
                Description("Select Title Back Color")]
        public Color TitleBackColor
        {
            get { return this.lblTitle.BackColor; }
            set { this.lblTitle.BackColor = value; }
        }

        #region Constructor
        public LockUnit()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        public void InitDataView()
        {
            try
            {
                this.dataGridView.AutoGenerateColumns = false;

                DataGridViewCellStyle columnStyle = this.dataGridView.ColumnHeadersDefaultCellStyle;
                columnStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                colName.HeaderText = "Item";
                colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dataGridView.Columns.Add(colName);


                DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
                colValue.HeaderText = "Value";
                colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView.Columns.Add(colValue);

                this.dataGridView.RowCount = 3;

                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView[1, 0].Style = style;
                dataGridView[1, 1].Style = style;
                dataGridView[1, 2].Style = style;

                dataGridView[0, 0].Value = "Tray Exist";
                dataGridView[0, 1].Value = "Pressure";
                dataGridView[0, 2].Value = "Shower Head Pos";

                SetCurrentData();

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (true)//m_Tag[tagDescriptor.TRAYEXIST].Value == bool.TrueString || m_Tag[tagDescriptor.TRAYEXIST].Value == "1")
            {
                style.BackColor = Color.LawnGreen;
                dataGridView[1, 0].Style = style;
            }
            //else
            //{
            //    style.BackColor = Color.White;
            //    dataGridView[1, 0].Style = style;
            //}

            dataGridView[1, 1].Value = "Pressure";
            dataGridView[1, 2].Value = string.Format("{0:2F}",m_Tag[tagDescriptor.HANDPOSITION].Value);
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;

                InitDataView();

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            //if (!m_Initialized) return;

            ////this.lblPressure.Text = " ";

            //if (m_Tag[tagDescriptor.TRAYEXIST].Value == bool.TrueString || m_Tag[tagDescriptor.TRAYEXIST].Value == "1")
            //    this.lblTrayExist.BackColor = Color.LawnGreen;
            //else this.lblTrayExist.BackColor = Color.White;

            //if (m_Tag[tagDescriptor.HANDPOSITION].Value == "HOME")
            //    this.lblHome.BackColor = Color.LawnGreen;
            //else this.lblHome.BackColor = Color.White;

            //if (m_Tag[tagDescriptor.HANDPOSITION].Value == "RECV")
            //    this.lblRecv.BackColor = Color.LawnGreen;
            //else this.lblRecv.BackColor = Color.White;

            //if (m_Tag[tagDescriptor.HANDPOSITION].Value == "SEND")
            //    this.lblSend.BackColor = Color.LawnGreen;
            //else this.lblSend.BackColor = Color.White;

            //this.lblRunState.Text = m_Tag[tagDescriptor.RUNSTATE].Value;
            //this.progressBar1.Value = Convert.ToInt32(m_Tag[tagDescriptor.PROCESS].Value);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class IfSteps : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorInterfaceStep tagDescriptor = new TagDescriptorInterfaceStep();
        #endregion

        #region Enum
        public enum Direction
        {
            Left,
            Right
        }
        #endregion

        #region Fields
        private int m_StepNo;
        private string[] m_Names;
        private IfSteps.Direction[] m_Direction;
        private int m_DataCellHeight = 35;
        private DataGridViewContentAlignment m_GridContentAlignment = DataGridViewContentAlignment.MiddleCenter;
        #endregion

        #region Properties
        [Category("DMS : Setting"),
        Description("Max Step Number")]
        public int StepMaxNo
        {
            get { return m_StepNo; }
            set { m_StepNo = value; }
        }

        [Category("DMS : Setting"),
        Description("Step Name")]
        public string[] StepName
        {
            get { return m_Names; }
            set { m_Names = value; }
        }

        [Category("DMS : Setting"),
        Description("Step Direction")]
        public IfSteps.Direction[] StepDriection
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        [Category("DMS : UI")]
        public int DataCellHeight
        {
            get { return m_DataCellHeight; }
            set { m_DataCellHeight = value; }
        }
        [Category("DMS : UI")]
        public DataGridViewContentAlignment GridContentAlignment
        {
            get { return m_GridContentAlignment; }
            set { m_GridContentAlignment = value; }
        }

        [Category("DMS : Text"),
        Description("Title")]
        public string Title
        {
            get { return lblTopText.Text; }
            set { lblTopText.Text = value; }
        }

        [Category("DMS : Text"),
        Description("Upstream")]
        public string UpstreamName
        {
            get { return lblUpName.Text; }
            set { lblUpName.Text = value; }
        }

        [Category("DMS : Text"),
        Description("Downstream")]
        public string DownstreamName
        {
            get { return lblDnName.Text; }
            set { lblDnName.Text = value; }
        }
        #endregion

        #region Constructor
        public IfSteps()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Method
        private void InitDataView()
        {
            DataGridViewColumn colName = new DataGridViewColumn(new DataGridViewImageTextCell());
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.Columns.Add(colName);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (this.StepMaxNo > 0)
                this.dataGridView.Rows.Add(this.StepMaxNo);

            SetData();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                //row.Height = 35;
                row.Height = m_DataCellHeight;
            }

            tmrUpdateState.Enabled = true;
        }

        private void SetData()
        {
            int currentStepNo = Convert.ToInt32(m_Tag[tagDescriptor.STEP].Value);
            for (int i = 0; i < m_StepNo; i++)
            {
                if (i < currentStepNo)
                {
                    this.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    if (StepDriection[i] == Direction.Left)
                    {
                        this.dataGridView.Rows[i].Cells[0].Value = Properties.Resources.LeftDone;
                        this.dataGridView.Rows[i].Cells[0].Style.Alignment = m_GridContentAlignment;
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).Text = StepName[i];
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).TextColor = Color.Red;
                    }
                    else if (StepDriection[i] == Direction.Right)
                    {
                        this.dataGridView.Rows[i].Cells[0].Value = Properties.Resources.RightDone;
                        this.dataGridView.Rows[i].Cells[0].Style.Alignment = m_GridContentAlignment;
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).Text = StepName[i];
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).TextColor = Color.Red;
                    }                
                }
                else
                {
                    this.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    if (StepDriection[i] == Direction.Left)
                    {
                        this.dataGridView.Rows[i].Cells[0].Value = Properties.Resources.Left;
                        this.dataGridView.Rows[i].Cells[0].Style.Alignment = m_GridContentAlignment;
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).Text = StepName[i];
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).TextColor = Color.Black;
                    }
                    else if (StepDriection[i] == Direction.Right)
                    {
                        this.dataGridView.Rows[i].Cells[0].Value = Properties.Resources.Right;
                        this.dataGridView.Rows[i].Cells[0].Style.Alignment = m_GridContentAlignment;
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).Text = StepName[i];
                        (this.dataGridView.Rows[i].Cells[0] as DataGridViewImageTextCell).TextColor = Color.Black;
                    }                
                }
            }
        }
        #endregion

        #region Override
        /// <summary>
        /// It should be called by HMI - eun 20080110
        /// </summary>
        /// <param name="tags"></param>
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);
            
            InitDataView();

            if (ok)
            {
                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            SetData();
        }
        #endregion

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }
    }
}

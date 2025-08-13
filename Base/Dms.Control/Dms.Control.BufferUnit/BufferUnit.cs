using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;

namespace Dms.Control
{
    public partial class BufferUnit : DmsUserControl
    {
        #region Tag Descriptor
        private TagDescriptorBufferUnit tagDescriptor = new TagDescriptorBufferUnit();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        private ServoUnit m_ServoUnit;
        private DeviceTagInfo m_TagServoUnitInfo = null;
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
        [Category("ServoUnit Setting")]
        public DeviceTagInfo DeviceTagServoUnitInfo
        {
            get { return m_TagServoUnitInfo; }
            set { m_TagServoUnitInfo = value; }
        }

        public BufferUnit()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("ServoUnit");
        }

        #region Methods
        private bool Initialize()
        {
            m_Client = ClientManager.Instance;
            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;

            m_ServoUnit = components[m_TagServoUnitInfo.DeviceName] as ServoUnit;

            if (m_ServoUnit == null) return false;

            return true;
        }
        #endregion

        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                SetDoubleBuffer();

                if (!Initialize()) return false;

                InitDataView();

                m_Initialized = true;
                tmrUpdateState.Enabled = true;
            }

            return m_Initialized;
        }

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


                this.dataGridView.RowCount = 5;

                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView[1, 0].Style = style;
                dataGridView[1, 1].Style = style;
                dataGridView[1, 2].Style = style;
                dataGridView[1, 3].Style = style;
                dataGridView[1, 4].Style = style;

                dataGridView[0, 0].Value = "Tray Exist";
                dataGridView[0, 1].Value = "Hand Pos";
                dataGridView[0, 2].Value = "Teaching Pos";
                dataGridView[0, 3].Value = "Hand Cur Pos";
                dataGridView[0, 4].Value = "Hand Sensor";


                SetCurrentData();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                //MessageBox.Show(err.Message.ToString());
            }
        }

        public void SetCurrentData()
        {
            DataGridViewCellStyle trayExistStyle = new DataGridViewCellStyle();
            trayExistStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (m_Tag[tagDescriptor.TRAYEXIST].Value == "1" || m_Tag[tagDescriptor.TRAYEXIST].Value == bool.TrueString)
            {
                trayExistStyle.BackColor = Color.GreenYellow;
                dataGridView[1, 0].Style = trayExistStyle;
            }
            else
            {
                trayExistStyle.BackColor = Color.White;
                dataGridView[1, 0].Style = trayExistStyle;
            }

            int curPointId = m_ServoUnit.GetCurPointId();

            if (curPointId >= 0 && curPointId < m_ServoUnit.TeachPointName.Length)
            {
                dataGridView[1, 1].Value = m_ServoUnit.TeachPointName[curPointId];
            }
            else dataGridView[1, 1].Value = "None";

            string temp = "";
            for (int i = 0; i < m_ServoUnit.TeachPoints; i++)
            {
                temp += "[" + m_ServoUnit.TeachPointName[i][0] + ":" + string.Format("{0:F1}",m_ServoUnit.TeachPoint[i].Pos[0]) + "] ";
            }
            dataGridView[1, 2].Value = temp;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView[1, 3].Value = string.Format("{0:F2}",m_ServoUnit.Axis[0].GetPosition());

            if (m_Client.GenInfos.EqpInitComp)
            {
                if (m_Client.GenInfos.AutoMode && !m_Client.GenInfos.CycleStop)
                {
                    this.BackColor = Color.White;
                    this.lblTitle.BackColor = Color.GreenYellow;
                }
                else if (m_Client.GenInfos.AutoMode && m_Client.GenInfos.CycleStop)
                {
                    this.BackColor = Color.White;
                    this.lblTitle.BackColor = Color.Yellow;
                }
                else if (!m_Client.GenInfos.AutoMode)
                {
                    this.BackColor = Color.Gainsboro;
                    this.lblTitle.BackColor = Color.Gainsboro;
                }
            }

            if (m_ServoUnit.IsDetectHomeSwitch(0) == true)
            {
                dataGridView[1, 4].Value = "HOME";
            }
            else
            {
                dataGridView[1, 4].Value = "Unknown";
            }
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (!m_Initialized) return;

            SetCurrentData();
        }
        
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.Client;
using Dms.Server;

namespace Dms.Control
{
    public partial class PmChamber_HWCVD : DmsUserControl
    {
        #region Tag Descriptor
        private TagDescriptorPmChamber_HWCVD tagDescriptor = new TagDescriptorPmChamber_HWCVD();
        #endregion

        #region
        private DeviceTagInfo m_TagServoUnitInfo = null;
        private ServoUnit m_ServoUnit;
        private ClientManager m_Client = null;
        #endregion
        
        #region Properties
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
        [Category("DMS : TrayTransfer ServoUnit")]
        public DeviceTagInfo DeviceTagServoUnitInfo
        {
            get { return m_TagServoUnitInfo; }
            set { m_TagServoUnitInfo = value; }
        }
        #endregion

        public PmChamber_HWCVD()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagServoUnitInfo = new DeviceTagInfo("ServoUnit");
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
                

                dataGridView[0, 0].Value = "Process";
                dataGridView[0, 1].Value = "Run Status";
                dataGridView[0, 2].Value = "Pressure";
                dataGridView[0, 3].Value = "Tray Exist";
                dataGridView[0, 4].Value = "Tray Transfer Pos";
                
                
                dataGridView.Rows[0].Cells[1].ValueType = typeof(DataGridViewProgressCell);
                dataGridView.Rows[0].Cells[1] = new DataGridViewProgressCell();

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

        public bool InitializeTag()
        {
            // RecvSensor 
            //if (m_TagRecvSensorInfo == null || string.IsNullOrEmpty(m_TagRecvSensorInfo.DeviceName))
            //{
            //    string msg = string.Format("TagInfo of {0} is not created", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //DeviceTag tag = m_Tags[m_TagRecvSensorInfo.DeviceName];

            //if (tag == null)
            //{
            //    string msg = string.Format("Tag of {0} does not exist.", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //else
            //{
            //    m_TagRecvSensor = tag;
            //    m_TagRecvSensorOld.Clone(m_TagRecvSensor);
            //}


            //// WaitSensor 
            //if (m_TagWaitSensorInfo == null || string.IsNullOrEmpty(m_TagWaitSensorInfo.DeviceName))
            //{
            //    string msg = string.Format("TagInfo of {0} is not created", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //tag = m_Tags[m_TagWaitSensorInfo.DeviceName];

            //if (tag == null)
            //{
            //    string msg = string.Format("Tag of {0} does not exist.", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //else
            //{
            //    m_TagWaitSensor = tag;
            //    m_TagWaitSensorOld.Clone(m_TagRecvSensor);
            //}


            //// SendSensor 
            //if (m_TagSendSensorInfo == null || string.IsNullOrEmpty(m_TagSendSensorInfo.DeviceName))
            //{
            //    string msg = string.Format("TagInfo of {0} is not created", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //tag = m_Tags[m_TagSendSensorInfo.DeviceName];

            //if (tag == null)
            //{
            //    string msg = string.Format("Tag of {0} does not exist.", this.Name);
            //    MessageBox.Show(msg);
            //    return false;
            //}
            //else
            //{
            //    m_TagSendSensor = tag;
            //    m_TagSendSensorOld.Clone(m_TagRecvSensor);
            //}

            return true;
        }

        public void SetCurrentData()
        {
            DataGridViewCellStyle trayExistCellStyle = new DataGridViewCellStyle();
            trayExistCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewCellStyle rfPowerOnStyle = new DataGridViewCellStyle();
            rfPowerOnStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView[1, 0].Value = Convert.ToSingle(m_Tag[tagDescriptor.PROCESS].Value);
            dataGridView[1, 1].Value = m_Tag[tagDescriptor.RUNSTATE].Value;
            dataGridView[1, 2].Value = m_Tag[tagDescriptor.PRESSURE].Value + " Torr";

            if (m_Tag[tagDescriptor.TRAYEXIST].Value == bool.TrueString || m_Tag[tagDescriptor.TRAYEXIST].Value == "1")
            {
                trayExistCellStyle.BackColor = Color.GreenYellow;
                dataGridView[1, 3].Style = trayExistCellStyle;
            }
            else
            {
                trayExistCellStyle.BackColor = Color.White;
                dataGridView[1, 3].Style = trayExistCellStyle;
            }

            int curPointId = m_ServoUnit.GetCurPointId();
            if (curPointId >= 0 && curPointId < m_ServoUnit.TeachPointName.Length)
            {
                string curPointName = m_ServoUnit.TeachPointName[curPointId];

                if (curPointName == "HOME") dataGridView[1, 4].Value = "HOME";
                else if (curPointName == "IN") dataGridView[1, 4].Value = "IN";
                else if (curPointName == "OUT") dataGridView[1, 4].Value = "OUT";
                else dataGridView[1, 4].Value = "UNKNOWN";
            }
            else dataGridView[1, 4].Value = "UNKNOWN";
        }

        public bool Initialize()
        {
            m_Client = ClientManager.Instance;
            IComponentContainer components = m_Client.EventSubscriber.Server.ComponentContainer;

            m_ServoUnit = components[m_TagServoUnitInfo.DeviceName] as ServoUnit;

            if (m_ServoUnit == null) return false;

            return true;
        }

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);
            ok &= InitializeTag();
            ok &= Initialize();

            if (ok)
            {
                InitDataView();
                
                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_Tag == null) return;

                if (m_TagOld.IsChanged(m_Tag))
                {
                    m_TagOld.Clone(m_Tag);
                }
                UpdateState();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            SetCurrentData();            
        }
        #endregion

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }
    }
}

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
    public partial class PmChamberState : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorPmChamber tagDescriptor = new TagDescriptorPmChamber();
        TagDescriptorRfg tagDescriptorRfg = new TagDescriptorRfg();
        TagDescriptorRfTuner tagDescriptorRfTuner = new TagDescriptorRfTuner();
        TagDescriptorMfc tagDescriptorMfc = new TagDescriptorMfc();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        protected DeviceTag m_TagRfg= null;
        protected DeviceTag m_TagRfgOld = new DeviceTag();
        protected DeviceTagInfo m_TagRfgInfo = null;

        protected DeviceTag m_TagRfTuner = null;
        protected DeviceTag m_TagRfTunerOld = new DeviceTag();
        protected DeviceTagInfo m_TagRfTunerInfo = null;

        //protected DeviceTag m_TagPN2Mfc = null;
        //protected DeviceTag m_TagPN2MfcOld = new DeviceTag();
        //protected DeviceTagInfo m_TagPN2MfcInfo = null;

        protected DeviceTag m_TagSF6Mfc = null;
        protected DeviceTag m_TagSF6MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagSF6MfcInfo = null;

        protected DeviceTag m_TagCL2Mfc = null;
        protected DeviceTag m_TagCL2MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagCL2MfcInfo = null;

        protected DeviceTag m_TagO2Mfc = null;
        protected DeviceTag m_TagO2MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagO2MfcInfo = null;
        #endregion

        #region Properties
        public DeviceTagInfo DeviceTagRfgInfo
        {
            get { return m_TagRfgInfo; }
            set { m_TagRfgInfo = value; }
        }
        public DeviceTagInfo DeviceTagRfTunerInfo
        {
            get { return m_TagRfTunerInfo; }
            set { m_TagRfTunerInfo = value; }
        }
        //public DeviceTagInfo DeviceTagPN2MfcInfo
        //{
        //    get { return m_TagPN2MfcInfo; }
        //    set { m_TagPN2MfcInfo = value; }
        //}
        public DeviceTagInfo DeviceTagSF6MfcInfo
        {
            get { return m_TagSF6MfcInfo; }
            set { m_TagSF6MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagCL2MfcInfo
        {
            get { return m_TagCL2MfcInfo; }
            set { m_TagCL2MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagO2MfcInfo
        {
            get { return m_TagO2MfcInfo; }
            set { m_TagO2MfcInfo = value; }
        }
        #endregion

        public PmChamberState()
        {
            InitializeComponent();
             
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagRfgInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagRfTunerInfo = new DeviceTagInfo(this.GetType().Name);
            //m_TagPN2MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagSF6MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagCL2MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagO2MfcInfo = new DeviceTagInfo(this.GetType().Name);
        }

        #region Method
        public void SetCurrentData()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dataGridView[0, 0].Value = m_Client.GenInfos.CurRecipeId;
            dataGridView[1, 0].Value = m_TagRfg[tagDescriptorRfg.FORWARDPOWER].Value;
            dataGridView[2, 0].Value = m_TagRfg[tagDescriptorRfg.REFLECTEDPOWER].Value;
            dataGridView[3, 0].Value = m_TagRfTuner[tagDescriptorRfTuner.TUNEPOS].Value;
            dataGridView[4, 0].Value = m_TagRfTuner[tagDescriptorRfTuner.LOADPOS].Value;
            dataGridView[5, 0].Value = m_Tag[tagDescriptor.PROCESSPRESSURE].Value;
            dataGridView[6, 0].Value = m_TagSF6Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[7, 0].Value = m_TagCL2Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[8, 0].Value = m_TagO2Mfc[tagDescriptorMfc.CURVAL].Value;
            //dataGridView[9, 0].Value = m_TagRfTuner[tagDescriptorRfTuner.DCBIAS].Value;
        }

        public void InitDataView()
        {
            try
            {
                this.dataGridView.AutoGenerateColumns = false;

                DataGridViewCellStyle columnStyle = this.dataGridView.ColumnHeadersDefaultCellStyle;
                columnStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                
                dataGridView.ColumnCount = 9;
                dataGridView.RowCount = 1;

                dataGridView.Columns[0].HeaderText = "Recipe";
                dataGridView.Columns[1].HeaderText = "Forward (Watt)";
                dataGridView.Columns[2].HeaderText = "Reflected (Watt)";                
                dataGridView.Columns[3].HeaderText = "Tune Position(%)";
                dataGridView.Columns[4].HeaderText = "Load Position(%)";
                dataGridView.Columns[5].HeaderText = "공정압력";
                dataGridView.Columns[6].HeaderText = "SF6 유량 (sccm)";
                dataGridView.Columns[7].HeaderText = "CL2 유량 (sccm)";
                dataGridView.Columns[8].HeaderText = "O2 유량 (sccm)";
                //dataGridView.Columns[9].HeaderText = "DC Bias (Volt)";

                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }



                //DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                //colName.HeaderText = "Item";
                //colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //this.dataGridView.Columns.Add(colName);

                //DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
                //colValue.HeaderText = "Value";
                //colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //this.dataGridView.Columns.Add(colValue);




                // Vertical View 일경우에는 databinding이 되지 않으므로

                //this.dataGridView.RowCount = 10;

                //DataGridViewCellStyle style = new DataGridViewCellStyle();
                //style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridView[1, 0].Style = style;
                //dataGridView[1, 1].Style = style;
                //dataGridView[1, 2].Style = style;
                //dataGridView[1, 3].Style = style;
                //dataGridView[1, 4].Style = style;
                //dataGridView[1, 5].Style = style;
                //dataGridView[1, 6].Style = style;
                //dataGridView[1, 7].Style = style;
                //dataGridView[1, 8].Style = style;
                //dataGridView[1, 9].Style = style;
                
                //dataGridView[0, 0].Value = "Recipe";
                //dataGridView[0, 1].Value = "Forward(Watt)";
                //dataGridView[0, 2].Value = "Reflected(Watt)";
                //dataGridView[0, 3].Value = "DC Bias(Volt)";
                //dataGridView[0, 4].Value = "Tune 위치(%)";
                //dataGridView[0, 5].Value = "Load 위치(%)";
                //dataGridView[0, 6].Value = "공정압력";
                //dataGridView[0, 7].Value = "SF6 유량(sccm)";
                //dataGridView[0, 8].Value = "CL2 유량(sccm)";
                //dataGridView[0, 9].Value = "O2 유량(sccm)";

                SetCurrentData();

                //foreach (DataGridViewColumn column in dataGridView.Columns)
                //{
                //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                //}

            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                //MessageBox.Show(err.Message.ToString());
            }
        }

        public bool InitializeTag()
        {
            // Rfg
            if (m_TagRfgInfo == null || string.IsNullOrEmpty(m_TagRfgInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            
            DeviceTag tag = m_Tags[m_TagRfgInfo.DeviceName];
            
            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else 
            {
                m_TagRfg = tag;
                m_TagRfgOld.Clone(m_TagRfg);
            }

            // RfTuner
            if (m_TagRfTunerInfo == null || string.IsNullOrEmpty(m_TagRfTunerInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagRfTunerInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagRfTuner = tag;
                m_TagRfTunerOld.Clone(m_TagRfTuner);
            }

            // MfcSF6
            if (m_TagSF6MfcInfo == null || string.IsNullOrEmpty(m_TagSF6MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagSF6MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagSF6Mfc = tag;
                m_TagSF6MfcOld.Clone(m_TagSF6Mfc);
            }

            // MfcCL2
            if (m_TagCL2MfcInfo == null || string.IsNullOrEmpty(m_TagCL2MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagCL2MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagCL2Mfc = tag;
                m_TagCL2MfcOld.Clone(m_TagCL2Mfc);
            }

            // MfcO2
            if (m_TagO2MfcInfo == null || string.IsNullOrEmpty(m_TagO2MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagO2MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagO2Mfc = tag;
                m_TagO2MfcOld.Clone(m_TagO2Mfc);
            }

            return true;
        }
        #endregion
        
        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            ok &= InitializeTag();

            if (ok)
            {
                m_Client = ClientManager.Instance;
                
                InitDataView();

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            SetCurrentData();   
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_Tag == null) return;

                if (m_TagOld.IsChanged(m_Tag) || 
                    m_TagCL2MfcOld.IsChanged(m_TagCL2Mfc) ||
                    m_TagO2MfcOld.IsChanged(m_TagO2Mfc) ||
                    m_TagRfgOld.IsChanged(m_TagRfg) ||
                    m_TagRfTunerOld.IsChanged(m_TagRfTuner) ||
                    m_TagSF6MfcOld.IsChanged(m_TagSF6Mfc))
                {
                    m_TagOld.Clone(m_Tag);
                    UpdateState();
                }
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }
    }
}

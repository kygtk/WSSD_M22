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
    public partial class PmChamberState_HWCVD : DmsUserControl
    {
        #region Tag Descriptor
        TagDescriptorPmChamber_HWCVD tagDescriptor = new TagDescriptorPmChamber_HWCVD();
        TagDescriptorMfc tagDescriptorMfc = new TagDescriptorMfc();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        protected DeviceTag m_TagPN2Mfc = null;
        protected DeviceTag m_TagPN2MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagPN2MfcInfo = null;

        protected DeviceTag m_TagSiH4Mfc = null;
        protected DeviceTag m_TagSiH4MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagSiH4MfcInfo = null;

        protected DeviceTag m_TagNF3Mfc = null;
        protected DeviceTag m_TagNF3MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagNF3MfcInfo = null;

        protected DeviceTag m_TagCH4Mfc = null;
        protected DeviceTag m_TagCH4MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagCH4MfcInfo = null;

        protected DeviceTag m_TagPH3Mfc = null;
        protected DeviceTag m_TagPH3MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagPH3MfcInfo = null;

        protected DeviceTag m_TagB2H6Mfc = null;
        protected DeviceTag m_TagB2H6MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagB2H6MfcInfo = null;

        protected DeviceTag m_TagNH3Mfc = null;
        protected DeviceTag m_TagNH3MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagNH3MfcInfo = null;

        protected DeviceTag m_TagH2Mfc = null;
        protected DeviceTag m_TagH2MfcOld = new DeviceTag();
        protected DeviceTagInfo m_TagH2MfcInfo = null;       
        #endregion

        #region Properties
        public DeviceTagInfo DeviceTagPN2MfcInfo
        {
            get { return m_TagPN2MfcInfo; }
            set { m_TagPN2MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagSiH4MfcInfo
        {
            get { return m_TagSiH4MfcInfo; }
            set { m_TagSiH4MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagNF3MfcInfo
        {
            get { return m_TagNF3MfcInfo; }
            set { m_TagNF3MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagCH4MfcInfo
        {
            get { return m_TagCH4MfcInfo; }
            set { m_TagCH4MfcInfo = value; }
        }

        public DeviceTagInfo DeviceTagPH3MfcInfo
        {
            get { return m_TagPH3MfcInfo; }
            set { m_TagPH3MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagB2H6MfcInfo
        {
            get { return m_TagB2H6MfcInfo; }
            set { m_TagB2H6MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagNH3MfcInfo
        {
            get { return m_TagNH3MfcInfo; }
            set { m_TagNH3MfcInfo = value; }
        }
        public DeviceTagInfo DeviceTagH2MfcInfo
        {
            get { return m_TagH2MfcInfo; }
            set { m_TagH2MfcInfo = value; }
        }

        #endregion

        public PmChamberState_HWCVD()
        {
            InitializeComponent();
             
            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            
            m_TagPN2MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagSiH4MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagNF3MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagCH4MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagPH3MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagB2H6MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagNH3MfcInfo = new DeviceTagInfo(this.GetType().Name);
            m_TagH2MfcInfo = new DeviceTagInfo(this.GetType().Name);

        }

        #region Method
        public void SetCurrentData()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dataGridView[0, 0].Value = m_Client.GenInfos.CurRecipeId;            
            dataGridView[1, 0].Value = m_Tag[tagDescriptor.PROCESSPRESSURE].Value;
            dataGridView[2, 0].Value = m_TagPN2Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[3, 0].Value = m_TagSiH4Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[4, 0].Value = m_TagNF3Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[5, 0].Value = m_TagCH4Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[6, 0].Value = m_TagPH3Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[7, 0].Value = m_TagB2H6Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[8, 0].Value = m_TagNH3Mfc[tagDescriptorMfc.CURVAL].Value;
            dataGridView[9, 0].Value = m_TagH2Mfc[tagDescriptorMfc.CURVAL].Value;
        }

        public void InitDataView()
        {
            try
            {
                this.dataGridView.AutoGenerateColumns = false;

                DataGridViewCellStyle columnStyle = this.dataGridView.ColumnHeadersDefaultCellStyle;
                columnStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                
                dataGridView.ColumnCount = 10;
                dataGridView.RowCount = 1;

                dataGridView.Columns[0].HeaderText = "Recipe";
                dataGridView.Columns[1].HeaderText = "공정압력";
                dataGridView.Columns[2].HeaderText = "PN2 (ccm)";
                dataGridView.Columns[3].HeaderText = "SiH4 (ccm)";
                dataGridView.Columns[4].HeaderText = "NF3 (ccm)";
                dataGridView.Columns[5].HeaderText = "CH4 (ccm)";
                dataGridView.Columns[6].HeaderText = "PH3 (ccm)";
                dataGridView.Columns[7].HeaderText = "B2H6 (ccm)";
                dataGridView.Columns[8].HeaderText = "NH3 (ccm)";
                dataGridView.Columns[9].HeaderText = "H2 (ccm)";

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
            // MfcPN2
            if (m_TagPN2MfcInfo == null || string.IsNullOrEmpty(m_TagPN2MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            DeviceTag tag = m_Tags[m_TagPN2MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagPN2Mfc = tag;
                m_TagPN2MfcOld.Clone(m_TagPN2Mfc);
            }
            
            // MfcSiH4
            if (m_TagSiH4MfcInfo == null || string.IsNullOrEmpty(m_TagSiH4MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagSiH4MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagSiH4Mfc = tag;
                m_TagSiH4MfcOld.Clone(m_TagSiH4Mfc);
            }

            // MfcNF3
            if (m_TagNF3MfcInfo == null || string.IsNullOrEmpty(m_TagNF3MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagNF3MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagNF3Mfc = tag;
                m_TagNF3MfcOld.Clone(m_TagNF3Mfc);
            }

            // MfcCH4
            if (m_TagCH4MfcInfo == null || string.IsNullOrEmpty(m_TagCH4MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagCH4MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagCH4Mfc = tag;
                m_TagCH4MfcOld.Clone(m_TagCH4Mfc);
            }

            // MfcPH3
            if (m_TagPH3MfcInfo == null || string.IsNullOrEmpty(m_TagPH3MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagPH3MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagPH3Mfc = tag;
                m_TagPH3MfcOld.Clone(m_TagPH3Mfc);
            }

            // MfcB2H6
            if (m_TagB2H6MfcInfo == null || string.IsNullOrEmpty(m_TagB2H6MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagB2H6MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagB2H6Mfc = tag;
                m_TagB2H6MfcOld.Clone(m_TagB2H6Mfc);
            }


            // MfcNH3
            if (m_TagNH3MfcInfo == null || string.IsNullOrEmpty(m_TagNH3MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagNH3MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagNH3Mfc = tag;
                m_TagNH3MfcOld.Clone(m_TagNH3Mfc);
            }

            // MfcH2
            if (m_TagH2MfcInfo == null || string.IsNullOrEmpty(m_TagH2MfcInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            tag = m_Tags[m_TagH2MfcInfo.DeviceName];

            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else
            {
                m_TagH2Mfc = tag;
                m_TagH2MfcOld.Clone(m_TagH2Mfc);
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
                    m_TagPN2MfcOld.IsChanged(m_TagPN2Mfc) ||
                    m_TagSiH4MfcOld.IsChanged(m_TagSiH4Mfc) ||
                    m_TagNF3MfcOld.IsChanged(m_TagNF3Mfc) ||
                    m_TagCH4MfcOld.IsChanged(m_TagCH4Mfc) ||
                    m_TagPH3MfcOld.IsChanged(m_TagPH3Mfc) ||
                    m_TagB2H6MfcOld.IsChanged(m_TagB2H6Mfc) ||     
                    m_TagNH3MfcOld.IsChanged(m_TagNH3Mfc) ||                 
                    m_TagH2MfcOld.IsChanged(m_TagH2Mfc))
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

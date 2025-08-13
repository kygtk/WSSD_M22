using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Data;
using Dms.Device;

namespace Dms.Control
{
    public partial class DlgGlassInfo : Form
    {
        private ClientManager m_Client;
        private int m_PositionId = 0;
        private TagGlassData m_Data = new TagGlassData();
        private Dictionary<string, string> m_DisplayedItem = new Dictionary<string, string>();
        private bool cimEnable = false;
        private bool modifyEnable = false;
        private static bool m_Editable = false;

        public bool CimEnable
        {
            get { return cimEnable; }
            set { cimEnable = value; }
        }

        public bool ModifyEnable
        {
            get { return modifyEnable; }
            set { modifyEnable = value; }
        }

        public bool Editable
        {
            get { return m_Editable; }
            set { m_Editable = value; }
        }

        public DlgGlassInfo(int positionId)
        {
            InitializeComponent();

            m_PositionId = positionId;
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            DeviceTags glassSensors = m_Client.TagContainer[typeof(GlsSensor)];

            DlgTargetPosition dlg = new DlgTargetPosition(glassSensors);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                int nNewPosId = dlg.CurrentPosition;
                m_Client.SendCommand(Command.GlassDataMove, m_PositionId, nNewPosId);
                this.Close();
            }
            //jemoon : 110607
            //ShowDialog를 사용하여 폼을 표시한 경우, Dispose를 호출하여 폼의 모든 컨트롤을 가비지 수집처리.	 
            dlg.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            m_Client.SendCommand(Command.GlassDataDelete, m_PositionId);
            this.Close();
        }

        //2009.06.29 add by Youngsik... for Glass Data Request to CIM
        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to request glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            int rows = gridView1.RowCount;

            Dictionary<string, string> editedData = new Dictionary<string, string>();

            for (int i = 0; i < rows; i++)
            {
                editedData.Add(gridView1[0, i].Value.ToString(), gridView1[1, i].Value.ToString());
            }

            m_Client.SendCommand(Command.GlassDataRequest, m_PositionId, editedData);
            this.Close();
        }

        //2009.06.29 add by Youngsik... for Glass Data Report to CIM
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to report glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            int rows = gridView1.RowCount;

            Dictionary<string, string> editedData = new Dictionary<string, string>();

            for (int i = 0; i < rows; i++)
            {
                editedData.Add(gridView1[0, i].Value.ToString(), gridView1[1, i].Value.ToString());
            }

            m_Client.SendCommand(Command.GlassDataReport, m_PositionId, editedData);
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DlgGlassInfo_Load(object sender, EventArgs e)
        {
            //Initialize();

            //2009.06.29 Youngsik...
            if (cimEnable == false)
            {
                groupBoxCim.Hide();
                btnRequest.Hide();
                btnReport.Hide();
            }
            else
            {
                groupBoxCim.Show();
                btnRequest.Show();
                btnReport.Show();
            }

            if (m_Editable == false)
            {
                btnEdit.Hide();
                this.groupBox1.Height = this.groupBox1.Height - btnEdit.Height;
            }
            else
            {
                btnEdit.Show();
            }
        }

        public void Initialize()
        {
            m_Client = ClientManager.Instance;
            if (m_Client.GenInfos.AutoMode)
            {
                this.btnDelete.Enabled = false;
                this.btnMove.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            if (m_Client.GlassDataProvider.GetData(m_PositionId, ref m_Data))
            {
                m_DisplayedItem = m_Data.Item.GetDisplayedItem();
            }

            InitGridView();
        }

        private void InitGridView()
        {
            DataGridViewTextBoxColumn colItem = new DataGridViewTextBoxColumn();
            colItem.HeaderText = "Item";
            colItem.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.gridView1.Columns.Add(colItem);

            DataGridViewTextBoxColumn colContents = new DataGridViewTextBoxColumn();
            colContents.HeaderText = "Contents";
            colContents.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.gridView1.Columns.Add(colContents);

            if (m_DisplayedItem.Count > 0)
                this.gridView1.Rows.Add(m_DisplayedItem.Count);

            SetData();
        }

        private void SetData()
        {
            int count = 0;
            foreach (KeyValuePair<string, string> each in m_DisplayedItem)
            {
                this.gridView1[0, count].Value = each.Key;
                this.gridView1[1, count].Value = each.Value;
                count++;
            }
        }

        private void gridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (modifyEnable == false && m_Editable == false) return;

            int rowIndex = e.RowIndex;

            KeyInValidation validation = new KeyInValidation();
            DataGridViewCell cell = gridView1[e.ColumnIndex, e.RowIndex];
            string caption = gridView1[0, rowIndex].Value.ToString();
            string oldVal = gridView1[1, rowIndex].Value.ToString();
            string curVal = oldVal;

            validation.Format = OptionFormat.String;

            curVal = validation.ShowEditDialog(caption, oldVal);

            if (curVal != oldVal)
            {
                cell.Value = curVal;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to edit glass data?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            int rows = gridView1.RowCount;

            Dictionary<string, string> editedData = new Dictionary<string, string>();

            for (int i = 0; i < rows; i++)
            {
                editedData.Add(gridView1[0, i].Value.ToString(), gridView1[1, i].Value.ToString());
            }

            m_Client.SendCommand(Command.GlassDataEdit, m_PositionId, editedData);
            this.Close();
        }
    }
}

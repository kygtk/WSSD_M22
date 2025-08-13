using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Loader;
using Dms.Common;

namespace Dms.Control
{
    public partial class DlgInputID : Form
    {
        CassettePort m_Port = null;
        Cassette m_Cassette = null;
        CassetteType m_CassetteType = CassetteType.A;

        public Cassette Cassette
        {
            get { return m_Cassette; }
        }

        public DlgInputID()
        {
            InitializeComponent();
        }

        public void Initialize(CassettePort port)
        {
            m_Port = port;

            InitData(m_Port);
            InitGridView(m_Cassette);

            cbThickness.SelectedIndex = 0;
            cbCassettetype.SelectedIndex = 0;
        }

        public void InitData(CassettePort port)
        {
            m_Cassette = port.Cassette;

            lblLotID.Text = m_Cassette.GetLotID();
            lblCstID.Text = m_Cassette.GetCassetteID();
            txtPortID.Text = "PORT" + port.PortNo.ToString();

            lblRecipeID.Text = "001-001-001-001";
        }

        public void InitGridView(Cassette port)
        {
            dataGridViewInput.AutoGenerateColumns = false;
            dataGridViewInput.Columns.Clear();
            dataGridViewInput.DataSource = null;
            dataGridViewInput.DataSource = m_Cassette.Stages;

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.DataPropertyName = "SlotNo";
            colNo.HeaderText = "Slot";
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewInput.Columns.Add(colNo);

            DataGridViewCheckBoxColumn colExist = new DataGridViewCheckBoxColumn();
            colExist.DataPropertyName = "GlassExist";
            colExist.HeaderText = "Glass";
            colExist.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            colExist.ReadOnly = true;
            colExist.DefaultCellStyle.BackColor = Color.Gray;
            this.dataGridViewInput.Columns.Add(colExist);

            DataGridViewCheckBoxColumn colSelect = new DataGridViewCheckBoxColumn();
            colSelect.DataPropertyName = "GlassSelected";
            colSelect.HeaderText = "Select";
            colSelect.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewInput.Columns.Add(colSelect);

            DataGridViewTextBoxColumn colSheetID = new DataGridViewTextBoxColumn();
            colSheetID.DataPropertyName = "SheetID";
            colSheetID.HeaderText = "Sheet ID";
            colSheetID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewInput.Columns.Add(colSheetID);

            DataGridViewTextBoxColumn colRecipeID = new DataGridViewTextBoxColumn();
            colRecipeID.DataPropertyName = "RecipeID";
            colRecipeID.HeaderText = "Recipe ID";
            colRecipeID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewInput.Columns.Add(colRecipeID);

            DataGridViewTextBoxColumn colThickness = new DataGridViewTextBoxColumn();
            colThickness.DataPropertyName = "Thickness";
            colThickness.HeaderText = "Thickness";
            colThickness.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colThickness.ReadOnly = true;
            colThickness.DefaultCellStyle.BackColor = Color.Gray;
            this.dataGridViewInput.Columns.Add(colThickness);

            //          dataGridViewInput.Sort(dataGridViewInput.Columns[0], ListSortDirection.Ascending);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsValueCheck())
            {
                char[] token = { ' ', ',' , '-'};
                string[] sRecipes;

                int count = m_Cassette.Stages.Count;

                for (int i = 0; i < count; i++)
                {
                    sRecipes = m_Cassette.Stages[i].RecipeID.Trim().Split(token);

                    if (m_Cassette.Stages[i].GlassExist)
                    {
                        m_Cassette.Stages[i].Glass.DMSRecipe = sRecipes[0];
                        m_Cassette.Stages[i].Glass.EXPRecipe = sRecipes[1];
                        m_Cassette.Stages[i].Glass.AOIRecipe = sRecipes[3];
                    }
                }

                m_Cassette.SetRecipeID(lblRecipeID.Text);
                m_Cassette.SetLotID(lblLotID.Text);
                m_Cassette.SetCassetteID(lblCstID.Text);

                int index = cbCassettetype.SelectedIndex;

                if (index == 0) m_CassetteType = CassetteType.A;
                else if (index == 1) m_CassetteType = CassetteType.B;
                else if (index == 2) m_CassetteType = CassetteType.C;
                else if (index == 3) m_CassetteType = CassetteType.G;

                m_Cassette.SetCstType(m_CassetteType);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Input the Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsValueCheck()
        {
            bool bRv = true;

            if (string.IsNullOrEmpty(lblLotID.Text)) return false;
            if (string.IsNullOrEmpty(lblCstID.Text)) return false;
            if (string.IsNullOrEmpty(lblRecipeID.Text)) return false;

//            int count = this.dataGridViewInput.Rows.Count;
            int checkcount = m_Cassette.SlotCount;
            int GlassSelectCount = 0;

            for (int i = 0; i < checkcount; i++)
            {
                DataGridViewRow row = dataGridViewInput.Rows[i];

                if (string.IsNullOrEmpty(row.Cells[5].Value.ToString()) ||
                    string.IsNullOrEmpty(row.Cells[4].Value.ToString()) ||
                    string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                {
                    bRv = false;
                    break;
                }

                if (row.Cells[2].Value.ToString() == "True") GlassSelectCount += 1;
            }

            if (GlassSelectCount <= 0) bRv = false;


            return bRv;
        }

        private void btnAllSlotSet_Click(object sender, EventArgs e)
        {
            int count = dataGridViewInput.Rows.Count;

            string sValue = (string)cbThickness.SelectedItem;
            string sSheetID = lblSheetID.Text;
            string sCount = "";

            for (int i = 0; i < count; i++)
            {
                if (sValue != "")
                {
                    dataGridViewInput.Rows[i].Cells[5].Value = sValue;
                }

                if (sSheetID != "")
                {
                    sCount = (Convert.ToString(i + 1)).PadLeft(2, '0');
                    dataGridViewInput.Rows[i].Cells[3].Value = sSheetID + sCount;
                }
            }
        }

        private void btnAllSlotCopy_Click(object sender, EventArgs e)
        {
            if (lblRecipeID.Text.Length == 15 && lblRecipeID.Text != "")
            {
                int count = dataGridViewInput.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    dataGridViewInput.Rows[i].Cells[4].Value = lblRecipeID.Text;
                }
            }
        }

        private void btnCopyGreen_Click(object sender, EventArgs e)
        {
            int rowcount = dataGridViewInput.SelectedRows.Count;

            if (rowcount <= 0) return;

            if (lblRecipeID.Text.Length == 15 && lblRecipeID.Text != "")
            {
                for (int i = 0; i < rowcount; i++)
                {
                    DataGridViewCell cell = dataGridViewInput.SelectedRows[i].Cells[4];

                    cell.Value = lblRecipeID.Text;
                } 
            }
        }

        private void btnAllSlotSelect_Click(object sender, EventArgs e)
        {
            int count = dataGridViewInput.Rows.Count;

            for( int i = 0; i < count; i++)
            {
                DataGridViewRow row = dataGridViewInput.Rows[i];
                row.Selected = true;
            }
        }

        private void lblCstID_Click(object sender, EventArgs e)
        {
            KeyInValidation keyinvalidation = new KeyInValidation();
            string curValue = "";
            string oldValue = lblCstID.Text;
            string caption = "Cassette ID INPUT";

            keyinvalidation.Format = OptionFormat.String;

            curValue = keyinvalidation.ShowEditDialog(caption, oldValue);

            if (oldValue != curValue)
            {
                lblCstID.Text = curValue;
            }
        }

        private void lblLotID_Click(object sender, EventArgs e)
        {
            KeyInValidation keyinvalidation = new KeyInValidation();
            string curValue = "";
            string oldValue = lblLotID.Text;
            string caption = "LOT ID INPUT";

            keyinvalidation.Format = OptionFormat.String;

            curValue = keyinvalidation.ShowEditDialog(caption, oldValue);

            if (oldValue != curValue)
            {
                lblLotID.Text = curValue;
            }
        }

        private void lblSheetID_Click(object sender, EventArgs e)
        {
            KeyInValidation keyinvalidation = new KeyInValidation();
            string curValue = "";
            string oldValue = lblSheetID.Text;
            string caption = "SHEET ID INPUT";

            keyinvalidation.Format = OptionFormat.String;

            curValue = keyinvalidation.ShowEditDialog(caption, oldValue);

            if (oldValue != curValue)
            {
                lblSheetID.Text = curValue;
            }
        }

        private void lblRecipeID_Click(object sender, EventArgs e)
        {
            KeyInValidation keyinvalidation = new KeyInValidation();
            bool ok = false;
            string curValue = "";
            string oldValue = lblRecipeID.Text;
            string caption = "RECIPE ID INPUT";

            keyinvalidation.Format = OptionFormat.String;

            curValue = keyinvalidation.ShowEditDialog(caption, oldValue);

            char[] token = { ' ', ',', '-' };
            string[] temp = curValue.Trim().Split(token);

            if (temp == null || temp.Length < 4)
            {
                ok = false;
            }
            else if (temp.Length == 4)
            {
                if (temp[0].Length == 3 && temp[1].Length == 3 &&
                    temp[2].Length == 3 && temp[3].Length == 3)
                {
                    ok = true;
                }
                else ok = false;
            }

            if (ok)
            {
                if (oldValue != curValue)
                {
                    lblRecipeID.Text = curValue;
                }
            }
            else
            {
                MessageBox.Show("Wrong Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbThickness_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            return;
        }

        private void cbThickness_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
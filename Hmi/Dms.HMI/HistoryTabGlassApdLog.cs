using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Common;
using System.Globalization;

namespace Dms.HMI
{
    public partial class HistoryTabGlassApdLog : UserControl
    {
        private GlassApdHistoryProvider Provider;
        private GlassApdInfo GlassApdInfo;
        private string m_ProcessResult = "ALL";
        private List<string> m_SearchList = new List<string>();

        #region Properties
        public ViewCimGlassApdHistory ViewGlassApdHistory
        {
            get { return this.viewGlassApdHistory1; }
        }
        #endregion

        public HistoryTabGlassApdLog()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

           // m_CimManager = CimManager.Instance;
        }

        #region Methods
        public void Initialize(GlassApdHistoryProvider provider)
        {
            Provider = provider;

            viewGlassApdHistory1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(viewGlassApdHistory1_DataBindingComplete);
            this.viewGlassApdHistory1.InitGridView(Provider);

            provider.HistoryCount = viewGlassApdHistory1.HistoryCount;

            GlassApdInfo = Provider.Adapter.GlassApdInfo;

        }

        void viewGlassApdHistory1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            cbPageSetting(false);
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_SearchList.Clear();

            if (checkGlassId.Checked)
                m_SearchList.Add(txtGlassId.Text);
            else
                m_SearchList.Add("ALL");

            if (checkRecipeId.Checked)
                m_SearchList.Add(txtRecipeId.Text);
            else
                m_SearchList.Add("ALL");

            if (checkCassetteId.Checked)
                m_SearchList.Add(txtCassetteId.Text);
            else
                m_SearchList.Add("ALL");

            if (checkLotId.Checked)
                m_SearchList.Add(txtLotId.Text);
            else
                m_SearchList.Add("ALL");


            if (MessageBox.Show("Do you want to search glass apd history", "GLASS APD HISTORY", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Provider.Search(m_SearchList, dateTimePickerDate.Value);

//                m_CimManager.SetCommandLog(Command.GlassApdHistory, dateTimePickerDate.Value.ToShortDateString(), m_SearchList[0], m_SearchList[1], m_SearchList[2], m_SearchList[3], m_SearchList[4]);

                cbPageSetting(true);
            }
        }

        private void cbPageList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int SelectValue = Convert.ToInt32(cbPageList.SelectedIndex);

            m_SearchList.Clear();

            if (checkCassetteId.Checked)
                m_SearchList.Add(txtCassetteId.Text);
            else
                m_SearchList.Add("ALL");

            if (checkLotId.Checked)
                m_SearchList.Add(txtLotId.Text);
            else
                m_SearchList.Add("ALL");

            if (checkRecipeId.Checked)
                m_SearchList.Add(txtRecipeId.Text);
            else
                m_SearchList.Add("ALL");

            m_SearchList.Add(m_ProcessResult);

            if (checkGlassId.Checked)
                m_SearchList.Add(txtGlassId.Text);
            else
                m_SearchList.Add("ALL");

            Provider.SelectDispaly(SelectValue, m_SearchList, dateTimePickerDate.Value);
        }

        private void cbPageSetting(bool Check)
        {
            int Quiotent = Provider.TotalCount / viewGlassApdHistory1.HistoryCount;
            int RestCount = Provider.TotalCount % viewGlassApdHistory1.HistoryCount;
            int PageCount = 0;

            if (RestCount > 0) PageCount = Quiotent + 1;
            else PageCount = Quiotent;

            if (PageCount != cbPageList.Items.Count || Check)
            {
                cbPageList.Items.Clear();

                for (int i = 0; i < PageCount; i++)
                {
                    cbPageList.Items.Add(i + 1);
                }

                lblPageCount.Text = PageCount.ToString();

                if (Provider.Adapter.TotalCount > 0) cbPageList.Text = Provider.Adapter.PageNo.ToString();
                else cbPageList.Text = "";
            }
        }

        private void dateTimePickerDate_ValueChanged(object sender, EventArgs e)
        {
            Provider.LoadFromLog(dateTimePickerDate.Value );

            cbPageSetting(true);
        }

        private void viewGlassApdHistory1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioProcesstAll_CheckedChanged(object sender, EventArgs e)
        {
            m_ProcessResult = "ALL";
        }

        private void radioProcessTrue_CheckedChanged(object sender, EventArgs e)
        {
            m_ProcessResult = "T";
        }

        private void radioProcessFault_CheckedChanged(object sender, EventArgs e)
        {
            m_ProcessResult = "F";
        }

    }
}

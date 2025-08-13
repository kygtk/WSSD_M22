using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Client;
using Dms.Common;
using Dms.Control;

namespace Dms.HMI
{
    public partial class AlarmForm : Form
    {
        #region Fields
        private AlarmHistoryProvider m_AlarmHistoryProvider;
        private AlarmTabHistory m_AlarmTabHistory = new AlarmTabHistory();
        private AlarmTabList m_AlarmTabList = new AlarmTabList();
        private ClientManager m_Client;
        #endregion

        #region Constructor
        public AlarmForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        } 
        #endregion

        #region Methods
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string tabName = this.tabControl1.SelectedTab.Text;

            if (tabName == this.tabAlarmHistory.Text)
            {
                if (DialogResult.Yes == MessageBox.Show("Do you want to delete information ?", "WSSD Sever",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //m_AlarmTabHistory.ViewHistory.GridView.Focus();
                    //SendKeys.SendWait("{DEL}");

                    m_AlarmHistoryProvider.Remove(m_AlarmTabHistory.ViewHistory.SelectedRows);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string tabName = this.tabControl1.SelectedTab.Text;
            if (tabName == this.tabAlarmHistory.Text)
            {
                m_AlarmTabHistory.Save();
            }
            else if (tabName == this.tabAlarmList.Text)
            {
                m_AlarmTabList.Save();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tabName = this.tabControl1.SelectedTab.Text;
            if (tabName == this.tabAlarmHistory.Text)
            {
                this.btnDelete.Visible = true;
            }
            else if(tabName == this.tabAlarmList.Text)
            {
                this.btnDelete.Visible = false;
            }
        }

        private void AlarmForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;

            m_AlarmHistoryProvider = m_Client.DataProvider.AlarmHistory;
            m_AlarmTabHistory.Initialize(m_AlarmHistoryProvider);
            this.tabAlarmHistory.Controls.Add(m_AlarmTabHistory);

            m_AlarmTabList.Initialize(m_Client.DataProvider.AlarmList);
            this.tabAlarmList.Controls.Add(m_AlarmTabList);
            //tmrUpdateState.Enabled = true;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool enable = true;
            //enable &= (m_Client.GenInfos.UserLevel > (int)UserLevels.Technician);
            enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Technician);
            enable &= !m_Client.GenInfos.AutoMode;
            
            if (this.btnDelete.Enabled != enable)
            {
                this.btnDelete.Enabled = enable;
            }
        }

        private void AlarmForm_Activated(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
        }

        private void AlarmForm_Deactivate(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = false;
        }
        #endregion
    }
}
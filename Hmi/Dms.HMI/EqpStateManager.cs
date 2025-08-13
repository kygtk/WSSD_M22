using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Server;

namespace Dms.HMI
{
    public partial class EqpStateManager : Form
    {
        #region Fields
        private Size m_OrgSize;
        private ClientManager m_ClientManager = ClientManager.Instance;
        private ServerManager m_ServerManager = ServerManager.Instance;
        #endregion

        #region Constructor
        public EqpStateManager()
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
        private void switchButton_AlarmResetButtonClick(object sender, EventArgs e)
        {
            // AlarmReset 버튼은 서버가 로컬모드이고 디바이스 시물레이션 모드일때만 활성화됨
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.AlarmResetSwitchPushed = true;
            }
        }

        private void switchButtonAlarmReset_ButtonPushed(object sender, EventArgs e)
        {
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.AlarmResetSwitchPushed = true;
            }
        }

        private void switchButtonAlarmReset_ButtonReleased(object sender, EventArgs e)
        {
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.AlarmResetSwitchPushed = false;
            }
        }

        private void switchButton_BuzzerOffButtonClick(object sender, EventArgs e)
        {
            // Buzzer Off 버튼은 서버가 로컬모드이고 디바이스 시물레이션 모드일때만 활성화됨
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.BuzzerOffSwitchPushed = true;
            }
        }

        private void switchButtonBuzzerOff_ButtonPushed(object sender, EventArgs e)
        {
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.BuzzerOffSwitchPushed = true;
            }
        }

        private void switchButtonBuzzerOff_ButtonReleased(object sender, EventArgs e)
        {
            if (m_ServerManager.Initialized)
            {
                m_ServerManager.EqpStateManager.BuzzerOffSwitchPushed = false;
            }
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Hide")
            {
                ((Button)sender).Text = "Show";
                this.ClientSize = new Size(m_OrgSize.Width, 35);
            }
            else
            {
                ((Button)sender).Text = "Hide";
                this.ClientSize = m_OrgSize;
            }
        }

        private void EqpStateManager_Load(object sender, EventArgs e)
        {
            m_OrgSize = this.ClientSize;

            DeviceTags tagContainer = m_ClientManager.DataProvider.TagContainer;

            AppConfig app = AppConfig.Instance;
            ServerMode serverMode = ClientManager.Instance.ServerMode;
            if (app.AutoStart || (app.UserControlBuild && (serverMode == ServerMode.Remoting)))
            {
                this.switchButtonAlarmReset.Initialize(tagContainer);
                this.switchButtonBuzzerOff.Initialize(tagContainer);
                this.buzzer1.Initialize(tagContainer);
                this.signalTower1.Initialize(tagContainer);
            }
        }
        #endregion
    }
}
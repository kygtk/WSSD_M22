using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Server;
using Dms.Data;

namespace Dms.HMI
{
    public partial class JobsToolbar : UserControl
    {
        private ClientManager m_Client = null;
        
        public JobsToolbar()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool enable = true;

            //Manual Button
            enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Operator);
            if (btnManual.Enabled != enable)
            {
                btnManual.Enabled = enable;
            }

            //Ready Button
            enable = true;
            enable &= m_Client.GenInfos.AutoMode;
            enable &= !m_Client.GenInfos.EqpInitReq;
            enable &= !m_Client.GenInfos.EqpInitComp;
            if (btnReady.Enabled != enable)
            {
                btnReady.Enabled = enable;
            }

            //Cycle Start Button
            enable = true;
            enable &= m_Client.GenInfos.EqpInitComp;
            enable &= !m_Client.GenInfos.CleanOut;
            //enable &= GlobalVar.EqpCtlMode == '0';//2009.07.31 kimgun
            //enable &= GlobalVar.LoaderReady; // 11.05.02 minhan
            if (btnCycleStart.Enabled != enable)
            {
                btnCycleStart.Enabled = enable;
            }
            //Cycle Start Button
            enable = true;//2009.07.31 kimgun
            //enable &= GlobalVar.EqpCtlMode == '0';
            //enable &= GlobalVar.LoaderReady; // 11.05.02 minhan
            if (btnCycleStop.Enabled != enable)
            {
                btnCycleStop.Enabled = enable;
            }

            //Pause Button
            enable = true;
            enable &= (m_Client.GenInfos.AutoMode || m_Client.GenInfos.EqpInitComp);
            if (btnPause.Enabled != enable)
            {
                btnPause.Enabled = enable;
            }

            enable = true;
            enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Engineer);
            enable &= !m_Client.GenInfos.AutoMode;
            if (Maint.Enabled != enable)
            {
                Maint.Enabled = enable;
            }
            //if (btnOnline.Enabled != enable) // 10.12.21 minhan
            //{
            //    btnOnline.Enabled = enable;
            //}
        }

        private void JobsToolbar_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            tmrUpdateState.Enabled = true;
        }
        private void Maint_Click(object sender, EventArgs e)
        {
            if(GlobalVar.ScreenLockPassword.Length > 0)
                GlobalVar.ScreenLockPassword.Remove(0);

            DlgScreenLock dlgScreenLock = new DlgScreenLock();
            dlgScreenLock.ShowDialog();
            dlgScreenLock.Dispose();

            if(GlobalVar.ScreenLockPassword.Length > 0)
            {
                ScreenLockForm formScreenLock = new ScreenLockForm();
                formScreenLock.ShowDialog();
                formScreenLock.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using Dms.Client;
using System.Windows.Forms;

namespace Dms.HMI
{
    public partial class InterfaceToolbar : UserControl
    {
        private ClientManager m_Client = null;
        public InterfaceToolbar()
        {
            InitializeComponent();
        }
        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            //Clean Out
            if (Dms.Data.GlobalVar.EqpCtlMode == '0')//online일 경우
            {
                if (m_Client.GenInfos.CleanOut)
                    btnNormal.Enabled = false;
                else
                    btnNormal.Enabled = true;
                btnParticle.Enabled = true;
                btnRecovery.Enabled = true;
                
            }
            else
            {
                btnNormal.Enabled = false;
                btnParticle.Enabled = false;
                btnRecovery.Enabled = false;
            }
            if (m_Client.GenInfos.AutoMode)
            {
                btnManualIFRecv.Enabled = false;
                btnManualIFSend.Enabled = false;
            }
            else
            {
                btnManualIFRecv.Enabled = true;
                btnManualIFSend.Enabled = true;
            }
            
        }
        private void InterfaceToolbar_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            tmrUpdateState.Enabled = true;
        }
    }
}

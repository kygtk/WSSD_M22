using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Server
{
    public partial class CheckUnitCondForm : Form
    {
        #region Fields
        private static object m_LockKey = new object(); // 11.02.09 minhan
        protected _GenericCollection<TransferUnit> m_TransferUnits;
        private ThreadCvBOE_G8_DHDC m_control;
        private string[][] OldCheckCond;
        private ServerManager m_Server;
        private Size m_OrgSize; // 11.02.01 minhan 
        private TransferUnit m_LdCv; // 11.02.09 minhan
        private TransferUnit m_EuvCv; // 11.02.09 minhan
        private TransferUnit m_RbCv; // 11.02.09 minhan
        private static int m_Count; // 11.02.09 minhan 
        #endregion
        public CheckUnitCondForm()
        {
            InitializeComponent();
        }
        public void Initialize(ThreadCvBOE_G8_DHDC control)
        {
            m_Server = ServerManager.Instance;
            m_control = control;
            m_LdCv = eqpTransferUnits._LD_CvUnit; // 11.02.09 minhan
            m_EuvCv = eqpTransferUnits._EUV_CvUnit; // 11.02.09 minhan
            m_RbCv = eqpTransferUnits._RB_CvUnit; // 11.02.09 minhan
            m_Count = (int)ThreadCvBOE_G8_DHDC.CheckCond.CheckCondCnt; // 11.02.09 minhan
            m_TransferUnits = DmsComponents.Instance.ComponentContainer.GetCollection<TransferUnit>();
            OldCheckCond = new string[m_TransferUnits.Count][];
            foreach (TransferUnit device in m_TransferUnits)
            {
                OldCheckCond[device.Id] = new string[m_Count];
            }
        }
        public void UpdateCond()
        {
            lock (m_LockKey) // 09.11.26 minhan
            {
                try
                {
                    this.TopLevel = true;
                    foreach (TransferUnit device in m_TransferUnits)
                    {
                        if ((device.Id != m_LdCv.Id) &&
                            (device.Id != m_EuvCv.Id) &&
                            (device.Id != m_RbCv.Id)) // 11.02.09 minhan
                        {
                            continue;
                        }

                        for (int i = 0; i < m_Count; i++)
                        {

                            if (m_control.CheckUnitCond[device.Id][i] != null)
                            {
                                OldCheckCond[device.Id][i] = device.Name + ":" + m_control.CheckUnitCond[device.Id][i];
                                if (!UnitCondList.Items.Contains(OldCheckCond[device.Id][i]))
                                    UnitCondList.Items.Add(OldCheckCond[device.Id][i]);
                            }
                            else if (OldCheckCond[device.Id][i] != null)
                            {
                                UnitCondList.Items.Remove(OldCheckCond[device.Id][i]);
                                OldCheckCond[device.Id][i] = null;
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    m_Server.WriteExceptionLog(err.ToString());
                }
            }

        }

        private void tmrCheckUnitcondTick(object sender, EventArgs e)
        {
            UpdateCond();
            CheckDestory();
        }

        private void CheckDestory()
        {
            bool deleteCond = true;
            foreach (TransferUnit device in m_TransferUnits)
            {
                if ((device.Id != m_LdCv.Id) &&
                    (device.Id != m_EuvCv.Id) &&
                    (device.Id != m_RbCv.Id)) // 11.02.09 minhan
                {
                    continue;
                }

                for (int i = 0; i < m_Count; i++) // 11.02.09 minhan
                {
                    if (OldCheckCond[device.Id][i] != null)
                    {
                        deleteCond = false;
                        break;
                    }
                }

            }
            if (this.Visible && deleteCond)
            {
                this.Hide();
            }
            else if (!this.Visible && !deleteCond)
            {
                this.Show();
            }
        }

        private void Show_Click(object sender, EventArgs e) // 11.02.01 minhan
        {
            if (((Button)sender).Text == "Hide")
            {
                ((Button)sender).Text = "Show";
                this.ClientSize = new Size(m_OrgSize.Width, 24);
            }
            else
            {
                ((Button)sender).Text = "Hide";
                this.ClientSize = m_OrgSize;
            }
        }

        private void FormInitStatus_Load(object sender, EventArgs e) // 11.02.01 minhan
        {
            m_OrgSize = this.ClientSize;
        }
    }
}
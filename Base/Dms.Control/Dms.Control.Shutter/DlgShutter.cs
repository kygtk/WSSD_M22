using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class DlgShutter : Form
    {
        #region Tag Descriptor
        public static TagDescriptorShutter tagDescriptor = new TagDescriptorShutter();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTag m_TagOld = new DeviceTag();
        private ClientManager m_Client = ClientManager.Instance;
        #endregion


        public DlgShutter()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public DlgShutter(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_Tag = tag;
            m_TagOld.Clone(m_Tag);
        }

        private void DlgShutter_Load(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
            Point point = new Point(0, 768 - this.Height);
            this.Location = point;

            //if (m_Tag[tagDescriptor.SingleAct].Value == true.ToString() || m_Tag[tagDescriptor.SingleAct].Value == "1")
            //{
            //    btnPUSH.Enabled = false;
            //    btnPULL.Enabled = false;
            //    checkPUSH.Enabled = false;
            //    checkPULL.Enabled = false;
            //}

            UpdateState();
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_TagOld.IsChanged(m_Tag))
            {
                m_TagOld.Clone(m_Tag);
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (m_Tag[tagDescriptor.Open].Value == bool.TrueString || m_Tag[tagDescriptor.Open].Value == "1")
            {
                this.checkOPEN.Checked = true;
            }
            else
            {
                this.checkOPEN.Checked = false;
            }

            if (m_Tag[tagDescriptor.Close].Value == bool.TrueString || m_Tag[tagDescriptor.Close].Value == "1")
            {
                this.checkCLOSE.Checked = true;
            }
            else
            {
                this.checkCLOSE.Checked = false;
            }          
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUP_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Up);
        }

        private void btnDOWN_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Down);
        }

        private void btnPUSH_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Push);
        }
        private void btnPULL_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Pull);
        }

        private void btnOPEN_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Open);
        }

        private void btnCLOSE_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ShutterManual, m_Tag.DeviceName, ShutterAct.Close);
        }
    }
}
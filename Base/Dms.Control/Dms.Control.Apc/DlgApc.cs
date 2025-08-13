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
    public partial class DlgApc : Form
    {
        #region Tag Descriptor
        public static TagDescriptorApc tagDescriptor = new TagDescriptorApc();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private ClientManager m_Client = ClientManager.Instance;
        private int m_SelectedPoint = -1;
        #endregion

        #region Properties
        public int SelectedPoint
        {
            get { return m_SelectedPoint; }
            set { m_SelectedPoint = value; }
        }
        #endregion
        
        public DlgApc(DeviceTag tag)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_Tag = tag;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            ApcAct act = ApcAct.Noop;

            if (m_SelectedPoint == 0) act = ApcAct.SetPointAPressure;
            else if (m_SelectedPoint == 1) act = ApcAct.SetPointBPressure;
            else if (m_SelectedPoint == 2) act = ApcAct.SetPointCPressure;
            else if (m_SelectedPoint == 3) act = ApcAct.SetPointDPressure;
            else if (m_SelectedPoint == 4) act = ApcAct.SetPointEPressure;
            else
            {
                MessageBox.Show("Please Select Point.");
                return;
            }
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, act, tbValue.Text);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.Open);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.Close);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.Hold);
        }
        
        private void DlgApc_Load(object sender, EventArgs e)
        {
            tbValue.Text = "0.0";

            if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_A")
            {
                btnPointA.Checked = true;
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_B")
            {
                btnPointB.Checked = true;
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_C")
            {
                btnPointC.Checked = true;
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_D")
            {
                btnPointD.Checked = true;
            }
            else if (m_Tag[tagDescriptor.VALUE].Value == "SELECTED_POINT_E")
            {
                btnPointE.Checked = true;
            }
        }

        private void btnPointA_Click(object sender, EventArgs e)
         {
            m_SelectedPoint = 0;
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.SelectPointA);
        }

        private void btnPointB_Click(object sender, EventArgs e)
        {
            m_SelectedPoint = 1;
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.SelectPointB);
        }

        private void btnPointC_Click(object sender, EventArgs e)
         {
            m_SelectedPoint = 2;
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.SelectPointC);
        }

        private void btnPointD_Click(object sender, EventArgs e)
        {
            m_SelectedPoint = 3;
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.SelectPointD);
        }

        private void btnPointE_Click(object sender, EventArgs e)
       {
            m_SelectedPoint = 4;
            m_Client.SendCommand(Command.ApcManual, m_Tag.DeviceName, ApcAct.SelectPointE);
        }
    }
}
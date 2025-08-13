using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Cim.Common
{
    public partial class DlgConfirmGlassInfo : Form
    {
        private int m_PortCount = 0;
        private int m_SlotCount = 0;
        private int m_nPortNo = -1;
        private int m_nSlotNo = -1;

        public int PortNo
        {
            get { return m_nPortNo; }
        }

        public int SlotNo
        {
            get { return m_nSlotNo; }
        }

        public DlgConfirmGlassInfo(int portcount, int slotcount)
        {
            InitializeComponent();

            m_PortCount = portcount;
            m_SlotCount = slotcount;

            for (int i = 1; i <= m_PortCount; i++)
            {
                cbPortNo.Items.Add(string.Format("Port {0}", i));
            }

            for (int i = 1; i <= m_SlotCount; i++)
            {
                cbSlotNo.Items.Add(string.Format("Slot {0}", i));
            }

            cbPortNo.SelectedIndex = 0;
            cbSlotNo.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_nPortNo = cbPortNo.SelectedIndex;
            m_nSlotNo = cbSlotNo.SelectedIndex;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
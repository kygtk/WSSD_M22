using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class ViewCstSlotSeparator : UserControl
    {
        private string m_SlotNo = "01";
        public string No
        {
            get { return m_SlotNo; }
            set 
            { 
                m_SlotNo = value;
                this.labelSlotNo.Text = m_SlotNo;
            }
        }

        public ViewCstSlotSeparator()
        {
            InitializeComponent();

            this.No = "01";
        }
    }
}

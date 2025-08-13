using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class HeatExchanger : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorHeatExchanger tagDescriptor = new TagDescriptorHeatExchanger();
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public string DescriptionText
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }
        #endregion

        #region
        public HeatExchanger()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;
                
                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            
        }
        #endregion

        private void label1_Click(object sender, EventArgs e)
        {
            DlgHeatExchanger dlg = new DlgHeatExchanger();

            dlg.Initialize(m_Tag);

            dlg.ShowDialog();
        }
    }
}

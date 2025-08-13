using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control
{
    public partial class GasControl : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGasControl tagDescriptor = new TagDescriptorGasControl();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        #endregion

        [Category("Setting : Text")]
        public string Description
        {
            get { return lblGasControl.Text; }
            set { lblGasControl.Text = value; }
        }
    
        public GasControl()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }


        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags); 

            if (ok)
            {
                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                //UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
           
        }
        #endregion

        private void lblGasControl_Click(object sender, EventArgs e)
        {
            DlgGasControl dlg = new DlgGasControl(m_Tag);

            dlg.Show();
        }
    }
}

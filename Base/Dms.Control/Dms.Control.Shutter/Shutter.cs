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
    public partial class Shutter : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorShutter tagDescriptor = new TagDescriptorShutter();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        #endregion Fields

        #region Constructor
        public Shutter()
        {
            InitializeComponent();
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                m_Initialized = ok;

                m_Client = ClientManager.Instance;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.Open].Value == bool.TrueString)
            {
                pbImage.Image = Properties.Resources.ShutterOpen;

            }
            else 
            {
                pbImage.Image = Properties.Resources.ShutterClose;
            }
        }
        #endregion

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (m_Client.GenInfos.AutoMode) return;
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                //if (m_Client.GenInfos.AutoMode) return;
                DlgShutter dlg = new DlgShutter(m_Tag);
                dlg.ShowDialog();
            }
            else MessageBox.Show("Tag is not selected.");
        }
    }
}

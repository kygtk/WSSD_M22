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
    public partial class BaratronGauge : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorApc tagDescriptor = new TagDescriptorApc();
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        #endregion Fields

        #region Constructor
        public BaratronGauge()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color BoxColor
        {
            get { return lblBackColor.BackColor; }
            set { lblBackColor.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color DescriptionColor
        {
            get { return lblDescription.BackColor; }
            set { lblDescription.BackColor = value; }
        }
        #endregion


        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_Tag[tagDescriptor.VALUE] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.VALUE].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }

                m_Client = ClientManager.Instance;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            lblValue.Text = m_Tag[tagDescriptor.PRESSURE].Value + " Torr";
        }
        #endregion

    }
}

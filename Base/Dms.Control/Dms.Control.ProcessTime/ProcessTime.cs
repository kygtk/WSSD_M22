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
    public partial class ProcessTime : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorProcessTime tagDescriptor = new TagDescriptorProcessTime();
        #endregion

        #region Fields
        //TagUnit m_Unit;
        #endregion

        public ProcessTime()
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
                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            lblProcessTime.Text = m_Tag[tagDescriptor.PROCESSTIME].Value + "sec";
        }
        #endregion

    }
}

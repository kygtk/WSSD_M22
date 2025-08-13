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
    public partial class EuvLamp : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorLamp tagDescriptor = new TagDescriptorLamp();
        #endregion

        public EuvLamp()
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

            if (m_Tag[tagDescriptor.NOUSE].Value == bool.TrueString || m_Tag[tagDescriptor.NOUSE].Value == "1")
            {
                pictureBox1.Image = Dms.Control.Properties.Resources.LampNoUse;
            }
            else if (m_Tag[tagDescriptor.ON].Value == bool.TrueString || m_Tag[tagDescriptor.ON].Value == "1")
            {
                pictureBox1.Image = Dms.Control.Properties.Resources.LampOn;
            }
            else
            {
                pictureBox1.Image = Dms.Control.Properties.Resources.LampOff;
            }
        }
        #endregion
    }
}

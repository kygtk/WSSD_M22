using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
//using Dms.Client;
//using Dms.Data;
using Dms.Device;
//using Dms.Server;
using System.IO;

namespace Dms.Control
{
    public partial class CvMotorSpeed : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        private string m_OldSpeed = null;

        public CvMotorSpeed()
        {
            InitializeComponent();
            
            m_TagInfo = new DeviceTagInfo("CvMotor");
        }

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



        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_OldSpeed != m_Tag[tagDescriptor.SPEED].Value)
            {
                m_OldSpeed = m_Tag[tagDescriptor.SPEED].Value;
                UpdateState();
            }
        }

        protected override void UpdateState()
        {
            lblCvSpeed.Text = m_OldSpeed;
        }
    }
}

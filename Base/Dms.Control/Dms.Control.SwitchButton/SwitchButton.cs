///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : SwitchButton UserControl
//-------------------------------------------------------------------------
// Revison History
// *

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
    public partial class SwitchButton : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorLampSwitch tagDescriptor = new TagDescriptorLampSwitch();
        #endregion

        #region Fields
        private Color m_LampOnColor = Color.Gold;
        private Color m_LampOffColor = Color.Tan;
        #endregion

        #region Properties
        [Category("DMS : UI"), Description("Button Name")]
        public string ButtonText
        {
            get { return this.buttonSwitch.Text; }
            set { this.buttonSwitch.Text = value; }
        }
        [Category("DMS : UI"), Description("Lamp On Color")]
        public Color LampOnColor
        {
            get { return m_LampOnColor; }
            set { m_LampOnColor = value; }
        }
        [Category("DMS : UI"), Description("Lamp Off Color")]
        public Color LampOffColor
        {
            get { return m_LampOffColor; }
            set { m_LampOffColor = value; }
        }
        [Category("DMS : UI"), Description("Text Font")]
        public Font TextFont
        {
            get { return this.buttonSwitch.Font; }
            set { this.buttonSwitch.Font = value; }
        }
        [Category("DMS : UI"), Description("Tag")]
        public object ButtonTag
        {
            get { return this.buttonSwitch.Tag; }
            set { this.buttonSwitch.Tag = value; }
        }

        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("Button Click Event")]
        public event EventHandler ButtonClick;
        [Category("DMS : EVENT"), Description("Button Pushed Event")]
        public event EventHandler ButtonPushed;
        [Category("DMS : EVENT"), Description("Button Released Event")]
        public event EventHandler ButtonReleased;
        #endregion

        #region Constructor
        public SwitchButton()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }        
        #endregion

        #region Methods
        private void buttonSwitch_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }

        private void buttonSwitch_MouseDown(object sender, MouseEventArgs e)
        {
            if (ButtonPushed != null)
            {
                ButtonPushed(sender, e);
            }
        }

        private void buttonSwitch_MouseUp(object sender, MouseEventArgs e)
        {
            if (ButtonReleased != null)
            {
                ButtonReleased(sender, e);
            }
        }

        private void SwitchButton_Load(object sender, EventArgs e)
        {
            this.buttonSwitch.BackColor = m_LampOffColor;
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

            if (m_Tag[tagDescriptor.LAMP].Value == bool.TrueString || m_Tag[tagDescriptor.LAMP].Value == "1")
            {
                this.buttonSwitch.BackColor = m_LampOnColor;
            }
            else
            {
                this.buttonSwitch.BackColor = m_LampOffColor;
            }
        }
        #endregion
    }
}

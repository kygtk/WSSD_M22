///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : SignalTower UserControl
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
    public partial class SignalTower : DmsUserControl
    {        
        #region Tag Descriptor
        public static TagDescriptorSignalTower tagDescriptor = new TagDescriptorSignalTower();
        #endregion

        #region Fields
        private Color m_Lamp1OnColor = Color.DeepPink;
        private Color m_Lamp1OffColor = Color.Maroon;
        private Color m_Lamp2OnColor = Color.Yellow;
        private Color m_Lamp2OffColor = Color.Olive;
        private Color m_Lamp3OnColor = Color.Lime;
        private Color m_Lamp3OffColor = Color.Green;
        private Color m_Lamp4OnColor = Color.Cyan;
        private Color m_Lamp4OffColor = Color.DarkBlue;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color Lamp1OffColor
        {
            get { return m_Lamp1OffColor; }
            set { m_Lamp1OffColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp1OnColor
        {
            get { return m_Lamp1OnColor; }
            set { m_Lamp1OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp2OffColor
        {
            get { return m_Lamp2OffColor; }
            set { m_Lamp2OffColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp2OnColor
        {
            get { return m_Lamp2OnColor; }
            set { m_Lamp2OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp3OffColor
        {
            get { return m_Lamp3OffColor; }
            set { m_Lamp3OffColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp3OnColor
        {
            get { return m_Lamp3OnColor; }
            set { m_Lamp3OnColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp4OffColor
        {
            get { return m_Lamp4OffColor; }
            set { m_Lamp4OffColor = value; }
        }
        [Category("DMS : UI")]
        public Color Lamp4OnColor
        {
            get { return m_Lamp4OnColor; }
            set { m_Lamp4OnColor = value; }
        }
        #endregion

        #region Constructor
        public SignalTower()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        } 
        #endregion

        #region Methods
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

            if (m_Tag[tagDescriptor.LAMP1].Value == bool.TrueString || m_Tag[tagDescriptor.LAMP1].Value == "1")
            {
                this.button1.BackColor = m_Lamp1OnColor;
                this.button1.Text = "ON";
            }
            else
            {
                this.button1.BackColor = m_Lamp1OffColor;
                this.button1.Text = "OFF";
            }

            if (m_Tag[tagDescriptor.LAMP2].Value == bool.TrueString || m_Tag[tagDescriptor.LAMP2].Value == "1")
            {
                this.button2.BackColor = m_Lamp2OnColor;
                this.button2.Text = "ON";
            }
            else
            {
                this.button2.BackColor = m_Lamp2OffColor;
                this.button2.Text = "OFF";
            }

            if (m_Tag[tagDescriptor.LAMP3].Value == bool.TrueString || m_Tag[tagDescriptor.LAMP3].Value == "1")
            {
                this.button3.BackColor = m_Lamp3OnColor;
                this.button3.Text = "ON";
            }
            else
            {
                this.button3.BackColor = m_Lamp3OffColor;
                this.button3.Text = "OFF";
            }
            
            if (m_Tag[tagDescriptor.LAMP4].Value == bool.TrueString || m_Tag[tagDescriptor.LAMP4].Value == "1")
            {
                this.button4.BackColor = m_Lamp4OnColor;
                this.button4.Text = "ON";
            }
            else
            {
                this.button4.BackColor = m_Lamp4OffColor;
                this.button4.Text = "OFF";
            }
        }
        #endregion
    }
}

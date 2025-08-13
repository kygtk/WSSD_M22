///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : Buzzer UserControl
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
    public partial class Buzzer : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorBuzzer tagDescriptor = new TagDescriptorBuzzer();
        #endregion

        #region Fields
        private Bitmap m_SoundOn;
        private Bitmap m_SoundOff; 
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public Buzzer()
        {
            InitializeComponent();
            
            m_SoundOn = Dms.Control.Properties.Resources.soundon;
            m_SoundOff = Dms.Control.Properties.Resources.soundoff;
            
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

            if (m_Tag[tagDescriptor.MELODY1].Value == bool.TrueString || m_Tag[tagDescriptor.MELODY1].Value == "1")
            {
                this.pictureBox1.Image = m_SoundOn;
            }
            else
            {
                this.pictureBox1.Image = m_SoundOff;
            }

            if (m_Tag[tagDescriptor.MELODY2].Value == bool.TrueString || m_Tag[tagDescriptor.MELODY2].Value == "1")
            {
                this.pictureBox2.Image = m_SoundOn;
            }
            else
            {
                this.pictureBox2.Image = m_SoundOff;
            }

            if (m_Tag[tagDescriptor.MELODY3].Value == bool.TrueString || m_Tag[tagDescriptor.MELODY3].Value == "1")
            {
                this.pictureBox3.Image = m_SoundOn;
            }
            else
            {
                this.pictureBox3.Image = m_SoundOff;
            }

            if (m_Tag[tagDescriptor.MELODY4].Value == bool.TrueString || m_Tag[tagDescriptor.MELODY4].Value == "1")
            {
                this.pictureBox4.Image = m_SoundOn;
            }
            else
            {
                this.pictureBox4.Image = m_SoundOff;
            }
        }
        #endregion
    }
}

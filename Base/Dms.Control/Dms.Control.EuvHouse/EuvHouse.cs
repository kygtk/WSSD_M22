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
    public partial class EuvHouse : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorEuvUnit tagDescriptor = new TagDescriptorEuvUnit();
        #endregion
        #region Events
        [Category("DMS : EVENT"), Description("Euv Click Event")]
        public event EventHandler EuvClick;
        #endregion
        public EuvHouse()
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

            if (m_Tag[tagDescriptor.WARNING].Value == bool.TrueString || m_Tag[tagDescriptor.WARNING].Value == "1")
            {
                pbImageHouse.Image = Dms.Control.Properties.Resources.EuvAlarm1;
            }
            else if (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.ALARM].Value == "1")
            {
                pbImageHouse.Image = Dms.Control.Properties.Resources.EuvAlarm2;
            }
            else if (m_Tag[tagDescriptor.USE].Value == bool.TrueString || m_Tag[tagDescriptor.USE].Value == "1")
            {
                pbImageHouse.Image = Dms.Control.Properties.Resources.EuvHouse;
            }
            else
            {
                pbImageHouse.Image = Dms.Control.Properties.Resources.HouseNoUse;
            }
        }
        #endregion

        private void pbImageHouse_Click(object sender, EventArgs e)
        { 
            if (EuvClick != null)
            {
                EuvClick(sender, e);
            }
        }

        
    }
}

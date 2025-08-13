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
    public partial class Ap : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorPlasma tagDescriptor = new TagDescriptorPlasma();
        #endregion


        #region Enum
        public enum Frames
        {
            NoUse, Alarm, On, Off
        }
        #endregion

        #region Fields
        private List<Bitmap> m_BitmapHouse = new List<Bitmap>();
        private List<Bitmap> m_BitmapLamp = new List<Bitmap>();
        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("AP Click Event")]
        public event EventHandler ApClick;
        #endregion

        #region Constructor
        public Ap()
        {
            InitializeComponent();

            m_BitmapHouse.Add(Dms.Control.Properties.Resources.Plasma_NoUse);
            m_BitmapHouse.Add(Dms.Control.Properties.Resources.Plasma_Alarm);
            m_BitmapHouse.Add(Dms.Control.Properties.Resources.Plasma_On);
            m_BitmapHouse.Add(Dms.Control.Properties.Resources.Plasma_Off);
            

            m_BitmapLamp.Add(Dms.Control.Properties.Resources.Plasma_Lamp_Nouse);
            m_BitmapLamp.Add(Dms.Control.Properties.Resources.Plasma_Lamp_Alarm);
            m_BitmapLamp.Add(Dms.Control.Properties.Resources.Plasma_LampOn);
            m_BitmapLamp.Add(Dms.Control.Properties.Resources.Plasma_LampOff);
            

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
                if (m_Tag[tagDescriptor.ON] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.ON].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }
                if (m_Tag[tagDescriptor.ALARM] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.ALARM].Key);
                    MessageBox.Show(msg);
                    ok = false;
                }

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }

            return m_Initialized;
        }
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.USE].Value == bool.TrueString || m_Tag[tagDescriptor.USE].Value == "1")
            {
                if (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.ALARM].Value == "1")
                {
                    pbImageHouse.Image = m_BitmapHouse[(int)Frames.Alarm];
                    pbImageLamp.Image = m_BitmapLamp[(int)Frames.Alarm];
                }
                else if (m_Tag[tagDescriptor.ON].Value == bool.TrueString || m_Tag[tagDescriptor.ON].Value == "1")
                {
                    pbImageHouse.Image = m_BitmapHouse[(int)Frames.On];
                    pbImageLamp.Image = m_BitmapLamp[(int)Frames.On];
                }
                else
                {
                    pbImageHouse.Image = m_BitmapHouse[(int)Frames.Off];
                    pbImageLamp.Image = m_BitmapLamp[(int)Frames.Off];
                }
            }
            else
            {
                pbImageHouse.Image = m_BitmapHouse[(int)Frames.NoUse];
                pbImageLamp.Image = m_BitmapLamp[(int)Frames.NoUse];
            }
        }
        #endregion

        private void Ap_Click(object sender, EventArgs e)
        {
            if (ApClick != null)
            {
                ApClick(sender, e);
            }
        }
    }
}

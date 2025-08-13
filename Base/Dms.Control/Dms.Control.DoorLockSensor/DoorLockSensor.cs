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
    [ToolboxBitmap(typeof(DoorLockSensor), "DoorLockSensorAni.bmp")]
    public partial class DoorLockSensor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorDoorLock tagDescriptor = new TagDescriptorDoorLock();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        private Color m_OnColor = Color.HotPink;
        private Color m_OffColor = SystemColors.Control;

        private Color m_LockColor = Color.IndianRed;
        #endregion

        #region Properties
        [Category("DMS : UI"), Description("Select sensor on color")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        [Category("DMS : UI"), Description("Select sensor off color")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI"), Description("Select door lock color")]
        public Color LockColor
        {
            get { return m_LockColor; }
            set { m_LockColor = value; }
        }
        #endregion

        #region Constructor
        public DoorLockSensor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(GetType().Name);
        }
        #endregion

        #region Overrides
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            if (ok)
            {
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

            bool detected = m_Tag[tagDescriptor.DETECT].Value == bool.TrueString
                         || m_Tag[tagDescriptor.DETECT].Value == "1";
            bool locked = m_Tag[tagDescriptor.LOCKED].Value == bool.TrueString
                       || m_Tag[tagDescriptor.LOCKED].Value == "1";

            picSensor.BackColor = detected ? m_OnColor
                                           : locked ? m_LockColor
                                                    : m_OffColor;

        }
        #endregion

        #region EventHandler

        private void picSensor_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;

                DlgDoorLock dlg = new DlgDoorLock(m_Tag);
                dlg.ShowDialog();

                dlg.Dispose();
            }
            else
                MessageBox.Show("Tag is Not Selected.");
        }
        #endregion
    }
}

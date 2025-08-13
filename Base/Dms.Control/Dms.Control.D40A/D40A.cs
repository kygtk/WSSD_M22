using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using System.Xml.Serialization;

namespace Dms.Control
{
    [ToolboxBitmap(typeof(D40A), "D40AAni.bmp")]
    public partial class D40A : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorD40A tagDescriptor = new TagDescriptorD40A();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        private Color m_PairedColor = Color.LightGreen;
        private Color m_UnpairedColor = Color.LightGray;

        private Color m_DoorNullColor = Color.DarkGray;
        private Color m_DoorOpenColor = Color.HotPink;
        private Color m_DoorCloseColor = SystemColors.Control;
        #endregion

        #region Properties
        [Category("DMS : UI"), Description("Select sensor paired color")]
        public Color PairedColor
        {
            get { return m_PairedColor; }
            set { m_PairedColor = value; }
        }
        [Category("DMS : UI"), Description("Select sensor unpaired color")]
        public Color UnpairedColor
        {
            get { return m_UnpairedColor; }
            set { m_UnpairedColor = value; }
        }
        [Category("DMS : UI"), Description("Select empty door color")]
        public Color DoorNullColor
        {
            get { return m_DoorNullColor; }
            set { m_DoorNullColor = value; }
        }
        [Category("DMS : UI"), Description("Select opened door color")]
        public Color DoorOpenColor
        {
            get { return m_DoorOpenColor; }
            set { m_DoorOpenColor = value; }
        }
        [Category("DMS : UI"), Description("Select closed door color")]
        public Color DoorCloseColor
        {
            get { return m_DoorCloseColor; }
            set { m_DoorCloseColor = value; }
        }
        #endregion

        #region Constructor
        public D40A()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void SetColor(PictureBox pic, string state)
        {
            switch (state)
            {
                case "OPENED":
                    pic.BackColor = m_DoorOpenColor;
                    break;
                case "CLOSED":
                    pic.BackColor = m_DoorCloseColor;
                    break;

                default:
                    pic.BackColor = m_DoorNullColor;
                    break;
            }
        }
        #endregion

        #region Overrides
        public override bool Initialize(DeviceTags tagContainer)
        {
            bool ok = base.Initialize(tagContainer);

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

            picPairing.BackColor = m_Tag[tagDescriptor.PAIRING].Value == bool.TrueString ? m_PairedColor : m_UnpairedColor;

            SetColor(picDoor0, m_Tag[tagDescriptor.DOOR0_STATE].Value);
            SetColor(picDoor1, m_Tag[tagDescriptor.DOOR1_STATE].Value);
            SetColor(picDoor2, m_Tag[tagDescriptor.DOOR2_STATE].Value);
            SetColor(picDoor3, m_Tag[tagDescriptor.DOOR3_STATE].Value);
            SetColor(picDoor4, m_Tag[tagDescriptor.DOOR4_STATE].Value);
            SetColor(picDoor5, m_Tag[tagDescriptor.DOOR5_STATE].Value);
        }
        #endregion
    }
}

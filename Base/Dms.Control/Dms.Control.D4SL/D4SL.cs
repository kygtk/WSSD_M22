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
    [ToolboxBitmap(typeof(D4SL), "D4SLAni.bmp")]
    public partial class D4SL : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorD4SL tagDescriptor = new TagDescriptorD4SL();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        private Color m_PairedColor = Color.LightGreen;
        private Color m_UnpairedColor = Color.LightGray;

        private Color m_DoorNullColor = Color.DarkGray;
        private Color m_DoorOpenColor = Color.HotPink;
        private Color m_DoorCloseColor = SystemColors.Control;
        private Color m_DoorLockColor = Color.IndianRed;
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
        [Category("DMS : UI"), Description("Select locked door color")]
        public Color DoorLockColor
        {
            get { return m_DoorLockColor; }
            set { m_DoorLockColor = value; }
        }
        #endregion

        #region Constructor
        public D4SL()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        private void SetDisplay(Label lbl, string state)
        {
            lbl.Text = state;
            switch (state)
            {
                case "OPENED":
                    lbl.BackColor = m_DoorOpenColor;
                    break;
                case "CLOSED":
                    lbl.BackColor = m_DoorCloseColor;
                    break;
                case "LOCKED":
                    lbl.BackColor = m_DoorLockColor;
                    break;
                case "NULL":
                default:
                    lbl.BackColor = m_DoorNullColor;
                    break;
            }
            lbl.ForeColor = lbl.Text == "NULL" ? Color.DimGray
                                               : lbl.BackColor.GetBrightness() > 0.7f ? Color.Black
                                                                                      : Color.White;
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

            SetDisplay(lblDoor0, m_Tag[tagDescriptor.DOOR0_STATE].Value);
            SetDisplay(lblDoor1, m_Tag[tagDescriptor.DOOR1_STATE].Value);
            SetDisplay(lblDoor2, m_Tag[tagDescriptor.DOOR2_STATE].Value);
            SetDisplay(lblDoor3, m_Tag[tagDescriptor.DOOR3_STATE].Value);
            SetDisplay(lblDoor4, m_Tag[tagDescriptor.DOOR4_STATE].Value);
            SetDisplay(lblDoor5, m_Tag[tagDescriptor.DOOR5_STATE].Value);
            SetDisplay(lblDoor6, m_Tag[tagDescriptor.DOOR6_STATE].Value);
            SetDisplay(lblDoor7, m_Tag[tagDescriptor.DOOR7_STATE].Value);
        }
        #endregion

        #region EventHandler
        private void lblDoor_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;

                DlgD4SL dlg = new DlgD4SL(m_Tag);
                dlg.ShowDialog();

                dlg.Dispose();
            }
            else
                MessageBox.Show("Tag is Not Selected.");
        }
        #endregion
    }
}

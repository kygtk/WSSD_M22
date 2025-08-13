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
    [ToolboxBitmap(typeof(SmartDamper), "SmartDamperAni.bmp")]
    public partial class SmartDamper : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorSmartDamper tagDescriptor = new TagDescriptorSmartDamper();
        #endregion

        #region Fields
        private ClientManager m_Client = null;

        private Color m_PairedColor = Color.LightGreen;
        private Color m_UnpairedColor = Color.LightGray;
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
        #endregion

        #region Constructor
        public SmartDamper()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
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
            lblValue0.Text = m_Tag[tagDescriptor.PV_MODE].Value;
            lblValue1.Text = "SV : " + m_Tag[tagDescriptor.PV_TGT_PRESSURE].Value + " Pa";
            lblValue2.Text = "Hys : " + m_Tag[tagDescriptor.PV_TGT_HYSTERESIS].Value + " Pa";

            string alarmcode = m_Tag[tagDescriptor.PV_ALARMCODE].Value;
            lblValue3.Text = alarmcode == "0" ? "OK" : "Alarm : " + alarmcode;
            lblValue3.BackColor = alarmcode == "0" ? Color.White : Color.Red;

            lblValue4.Text = "PV : " + m_Tag[tagDescriptor.PV_CUR_PRESSURE].Value + " Pa";
            lblValue5.Text = "VA : " + m_Tag[tagDescriptor.PV_CUR_VALVEANGLE].Value + " ˚";
        }
        #endregion
    }
}

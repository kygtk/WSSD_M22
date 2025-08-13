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
    [ToolboxBitmap(typeof(Flowmeter), "FlowmeterAni.bmp")]
    public partial class Flowmeter : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorFlowMeter tagDescriptor = new TagDescriptorFlowMeter();
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
        public Flowmeter()
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
            lblFlow.Text = m_Tag[tagDescriptor.PV_FLOW].Value + " " + "lpm";
            lblPress.Text = m_Tag[tagDescriptor.PV_PRESS].Value + " " + "Pa";
        }
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : Glass Detect Sensor UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

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
    [ToolboxBitmap(typeof(GlsSensor), "GlsSensorAni.bmp")]
    public partial class GlsSensor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorGlsSensor tagDescriptor = new TagDescriptorGlsSensor();
        #endregion

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_SickColor = Color.HotPink;
        private Color m_OnSickColor = Color.IndianRed;//2010.01.27 kimgun
        private Color m_OffColor = Color.White;

        private ClientManager m_Client;
        #endregion

        #region Properties
        /// <summary>
        /// Select sensor on color - eun 20080118
        /// </summary>
        [Category("DMS : UI"),
         Description("Select sensor on color")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }
        /// <summary>
        /// Select sensor on color - eun 20080118
        /// </summary>
        [Category("DMS : UI"),
         Description("Select sensor off color")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI"),
         Description("Select sensor off sick color")]
        public Color SickColor
        {
            get { return m_SickColor; }
            set { m_SickColor = value; }
        }
        /// <summary>
        /// Select sensor Sick on color - 2010.01.27 kimgun
        /// </summary>
        [Category("DMS : UI"),
         Description("Select sensor on sick color")]
        public Color OnSickColor
        {
            get { return m_OnSickColor; }
            set { m_OnSickColor = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter GlsSensor contstructor - eun 20080110
        /// </summary>
        public GlsSensor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
            if (m_Client == null) m_Client = ClientManager.Instance;
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
                if (m_Tag[tagDescriptor.SICK] == null)
                {
                    string msg = string.Format("Tag of {0}'s key : {1} does not exist.", this.Name, tagDescriptor[tagDescriptor.SICK].Key);
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

            //if (m_Tag[tagDescriptor.SICK].Value == bool.TrueString || m_Tag[tagDescriptor.SICK].Value == "1")
            //{
            //    lblSensor.BackColor = m_SickColor;
            //}
            //else if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
            //{
            //    lblSensor.BackColor = m_OnColor;
            //}
            //else
            //{
            //    lblSensor.BackColor = m_OffColor;
            //}

            if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
            {
                if (m_Tag[tagDescriptor.SICK].Value == bool.TrueString || m_Tag[tagDescriptor.SICK].Value == "1")
                {//2010.01.27 kimgun
                    lblSensor.BackColor = m_OnSickColor;
                }
                else
                    lblSensor.BackColor = m_OnColor;
            }
            else
            {
                if (m_Tag[tagDescriptor.SICK].Value == bool.TrueString || m_Tag[tagDescriptor.SICK].Value == "1")
                {//2010.01.27 kimgun
                    lblSensor.BackColor = m_SickColor;
                }
                else
                    lblSensor.BackColor = m_OffColor;
            }
        }
        #endregion

        private void lblSensor_Click(object sender, EventArgs e)
        {//2010.01.27 kimgun
            bool SickStatus = false;
            SickStatus |= m_Tag[tagDescriptor.SICK].Value == bool.TrueString;
            SickStatus |= m_Tag[tagDescriptor.SICK].Value == "1";
            m_Client.SendCommand(Command.GlsSensorManual, m_Tag.DeviceId, SickStatus);
        }
    }
}

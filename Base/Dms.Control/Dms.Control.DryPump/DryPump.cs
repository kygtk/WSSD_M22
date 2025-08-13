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
    public partial class DryPump : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorDryPump tagDescriptor = new TagDescriptorDryPump();
        #endregion

        #region Enum
        public enum DeviceType
        {
            Normal, Inverter
        }
        #endregion

        #region Fields
        private ClientManager m_Client = null;
        private DeviceType m_Type = DeviceType.Normal;
        #endregion

        #region Properties
        /// <summary>
        /// Pump animation update time interval - eun 20080111
        /// </summary>
        //[Category("DMS : UI"),
        //Description("Pump animation update time interval")]
        //public int AnimationUpdateInterval
        //{
        //    get { return tmrUpdateAnimation.Interval; }
        //    set { tmrUpdateAnimation.Interval = value; }
        [Category("DMS : UI"), Description("Pump Type")]
        public DeviceType PumpType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        //}
        [Browsable(false), XmlIgnore()]
        public bool AlarmState
        {
            get { return m_Tag[tagDescriptor.ALARM].Value == bool.TrueString ? true : false; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Pump contstructor - eun 20080111
        /// </summary>
        public DryPump()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods


        /// <summary>
        /// Eventhandler for Pump operation dialog - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pump_Click(object sender, EventArgs e)
        {
            if (m_Tag == null) return;
            if (m_Tag.Items.Count != 0)
            {
                if (m_Client.GenInfos.AutoMode) return;
                DlgDryPump dlg = new DlgDryPump(m_Tag);
                dlg.ShowDialog();
            }
            else MessageBox.Show("Tag is not selected.");
        }
        #endregion

        #region Override
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

            if (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.ALARM].Value == "1")
            {
                axPump1.SetAlarm();
            }
            else if (m_Tag[tagDescriptor.STOP].Value == bool.TrueString || m_Tag[tagDescriptor.STOP].Value == "1")
            {
                axPump1.SetStop();
            }
            else if (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1")
            {
                axPump1.SetStart();
            }
        }
        #endregion

    }
}
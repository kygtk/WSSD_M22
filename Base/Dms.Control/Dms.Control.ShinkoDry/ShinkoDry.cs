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
    public partial class ShinkoDry : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorShinkoDryCleaner tagDescriptor = new TagDescriptorShinkoDryCleaner();
        #endregion

        #region Fields
        private Color m_PowerOnColor = Color.DarkOrange;
        private Color m_PowerOffColor = Color.MediumAquamarine;
        private Color m_RunColor = Color.Lime;
        private Color m_StopColor = Color.LightCyan;
        private Color m_EStopColor = Color.Red;
        private Color m_AlarmColor = Color.Red;
        private Color m_AlarmReleaseColor = Color.Khaki;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color PowerOnColor
        {
            get { return m_PowerOnColor; }
            set { m_PowerOnColor = value; }
        }

        [Category("DMS : UI")]
        public Color PowerOffColor
        {
            get { return m_PowerOffColor; }
            set { m_PowerOffColor = value; }
        }

        [Category("DMS : UI")]
        public Color RunColor
        {
            get { return m_RunColor; }
            set { m_RunColor = value; }
        }

        [Category("DMS : UI")]
        public Color StopColor
        {
            get { return m_StopColor; }
            set { m_StopColor = value; }
        }

        [Category("DMS : UI")]
        public Color EStopColor
        {
            get { return m_EStopColor; }
            set { m_EStopColor = value; }
        }

        [Category("DMS : UI")]
        public Color AlarmColor
        {
            get { return m_AlarmColor; }
            set { m_AlarmColor = value; }
        }

        [Category("DMS : UI")]
        public Color AlarmReleaseColor
        {
            get { return m_AlarmReleaseColor; }
            set { m_AlarmReleaseColor = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Sensor contstructor - eun 20080110
        /// </summary>
        public ShinkoDry()
        {
            InitializeComponent();

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
                labelName.Text = m_Tag.DeviceName;

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;
                
                UpdateState();
            }

            return m_Initialized;
        }
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if ((m_Tag[tagDescriptor.POWERON].Value == bool.TrueString || m_Tag[tagDescriptor.POWERON].Value == "1")
                  && (m_Tag[tagDescriptor.RUN].Value == bool.TrueString || m_Tag[tagDescriptor.RUN].Value == "1"))
            {
                labelRunStatus.Text = "Sycle Run";
                labelRunStatus.BackColor = m_RunColor;
            }
            else if ((m_Tag[tagDescriptor.POWERON].Value == bool.TrueString || m_Tag[tagDescriptor.POWERON].Value == "1")
                  && (m_Tag[tagDescriptor.STOP].Value == bool.TrueString || m_Tag[tagDescriptor.STOP].Value == "1"))
            {
                labelRunStatus.Text = "Sycle Stop";
                labelRunStatus.BackColor = m_PowerOffColor;
            }
            else if (m_Tag[tagDescriptor.POWERON].Value == bool.TrueString || m_Tag[tagDescriptor.POWERON].Value == "1")
            {
                labelRunStatus.Text = "Power On";
                labelRunStatus.BackColor = m_PowerOnColor;
            }
            else if (m_Tag[tagDescriptor.POWEROFF].Value == bool.TrueString || m_Tag[tagDescriptor.POWEROFF].Value == "1")
            {
                labelRunStatus.Text = "Power Off";
                labelRunStatus.BackColor = m_PowerOffColor;
            }
            else if (m_Tag[tagDescriptor.ESTOP].Value == bool.TrueString || m_Tag[tagDescriptor.ESTOP].Value == "1")
            {
                labelRunStatus.Text = "EMO";
                labelRunStatus.BackColor = m_EStopColor;
            }
            
            if (m_Tag[tagDescriptor.PREFILTER_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.PREFILTER_ALARM].Value == "1")
            {
                labelAlarm.Text = "Pre Filter";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.HEPAFILTER_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.HEPAFILTER_ALARM].Value == "1")
            {
                labelAlarm.Text = "Hepa Filter";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.INVERTER_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.INVERTER_ALARM].Value == "1")
            {
                labelAlarm.Text = "Inverter";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.WATERLEAK_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.WATERLEAK_ALARM].Value == "1")
            {
                labelAlarm.Text = "Water Leak";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.TEMP_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.TEMP_ALARM].Value == "1")
            {
                labelAlarm.Text = "Temperature";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.PRES_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.PRES_ALARM].Value == "1")
            {
                labelAlarm.Text = "Pressure";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else if (m_Tag[tagDescriptor.EMO_ALARM].Value == bool.TrueString || m_Tag[tagDescriptor.EMO_ALARM].Value == "1")
            {
                labelAlarm.Text = "EMO";
                labelAlarm.BackColor = m_AlarmColor;
            }
            else
            {
                labelAlarm.Text = "No Alarm";
                labelAlarm.BackColor = m_AlarmReleaseColor;
            }
        }
        #endregion
    }
}
///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : General Sensor UserControl
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

namespace Dms.Control
{
    [ToolboxBitmap(typeof(Sensor), "SensorAni.bmp")]
    public partial class Sensor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorSensor tagDescriptor = new TagDescriptorSensor();
        #endregion

        public enum ImageType
        {
            Default, New
        }

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_OffColor = Color.White;
        private ImageType m_SensorImageType = ImageType.Default;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color OnColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }

        [Category("DMS : UI")]
        public Color OffColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }
        [Category("DMS : UI")]
        public string Description
        {
            get { return lblSensor.Text; }
            set { lblSensor.Text = value; }
        }
        [Category("DMS : UI")]
        public Color SensorBackColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("DMS : UI")]
        public ImageType SensorImageType
        {
            get { return m_SensorImageType; }
            set 
            { 
                m_SensorImageType = value;
                SetSensorType();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Sensor contstructor - eun 20080110
        /// </summary>
        public Sensor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);

            SetSensorType();
        }
        #endregion

        #region Methods
        public void SetSensorType()
        {
			if (m_SensorImageType == ImageType.Default)
			{
				this.Padding = new Padding(0);
				this.BorderStyle = BorderStyle.FixedSingle;
				this.lblSensor.Dock = DockStyle.Fill;
				this.lblSensor.BackColor = OffColor;
				this.lblSensor.BorderStyle = BorderStyle.None;
			}
			else if (m_SensorImageType == ImageType.New)
			{
				this.Padding = new Padding(3);
				this.BorderStyle = BorderStyle.None;
				this.lblSensor.Dock = DockStyle.Fill;
				this.lblSensor.BackColor = OffColor;
				this.lblSensor.BorderStyle = BorderStyle.None;
			}
        }


        #endregion

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

            if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
            {
                lblSensor.BackColor = m_OnColor;
            }
            else
            {
                lblSensor.BackColor = m_OffColor;
            }
        }
        #endregion
    }
}

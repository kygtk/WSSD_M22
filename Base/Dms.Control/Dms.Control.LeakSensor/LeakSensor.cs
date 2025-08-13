///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : LeakSensor UserControl
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
    [ToolboxBitmap(typeof(LeakSensor), "LeakSensorAni.bmp")]
    public partial class LeakSensor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorSensor tagDescriptor = new TagDescriptorSensor();
        #endregion

        #region Fields
        private Color m_OnColor = Color.Cyan;
        private Color m_OffColor = SystemColors.Control;
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
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter LeakSensor contstructor - eun 20080110
        /// </summary>
        public LeakSensor()
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

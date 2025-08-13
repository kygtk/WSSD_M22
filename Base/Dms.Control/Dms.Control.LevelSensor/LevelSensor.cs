///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : LevelSensor UserControl
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
    [ToolboxBitmap(typeof(LevelSensor), "LevelSensorAni.bmp")]
    public partial class LevelSensor : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorLevelSensor tagDescriptor = new TagDescriptorLevelSensor();
        #endregion

        #region Fields
        private Color m_ConfirmColor = Color.Lime;
        private Color m_OnColor = Color.YellowGreen;
        private Color m_OffColor = SystemColors.Control;
        #endregion

        #region Properties
        /// <summary>
        /// Select sensor on color - eun 20080118
        /// </summary>
        [Category("DMS : UI"),
         Description("Select sensor confirm color")]
        public Color ConfirmColor
        {
            get { return m_ConfirmColor; }
            set { m_ConfirmColor = value; }
        }
        
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
        /// No parameter LevelSensor contstructor - eun 20080111
        /// </summary>
        public LevelSensor()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods

        #endregion

        #region Override
        /// <summary>
        /// It should be called by HMI - eun 20080111
        /// </summary>
        /// <param name="tags"></param>
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

            if (m_Tag[tagDescriptor.CONFIRM].Value == bool.TrueString || m_Tag[tagDescriptor.CONFIRM].Value == "1")
            {
                lblSensor.BackColor = m_ConfirmColor;
            
            }
            else if (m_Tag[tagDescriptor.DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.DETECT].Value == "1")
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

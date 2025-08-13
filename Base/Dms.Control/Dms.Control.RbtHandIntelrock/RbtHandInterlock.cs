///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.07.28
// Author       : HooLi
// Description  : Robot Hand Exist Sensor
//-------------------------------------------------------------------------
// Revison History
// * 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;


namespace Dms.Control.RbtHandIntelrock
{
    //[ToolboxBitmap(typeof(Sensor), "RbtHandInterlockAni.bmp")]
    public partial class RbtHandInterlock : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptiorRbtHandInterlock tagDescriptor = new TagDescriptiorRbtHandInterlock();
        #endregion

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_OffColor = Color.White;
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
            get { return RbtHandSensor.Text; }
            set { RbtHandSensor.Text = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Sensor contstructor - eun 20080110
        /// </summary>
        public RbtHandInterlock()
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
                RbtHandSensor.BackColor = m_OnColor;
            }
            else
            {
                RbtHandSensor.BackColor = m_OffColor;
            }
        }
        #endregion
    }
}

///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02. 14
// Author       : eun
// Description  : BrokenSensor Fixed Type UserControl
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
    [ToolboxBitmap(typeof(BrokenSensorFixedType), "BrokenSensorFixedType.ico")]
    public partial class BrokenSensorFixedType : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorBrokenFixed tagDescriptor = new TagDescriptorBrokenFixed();
        #endregion

        #region Enum
        public enum EqpTypes
        {
            L_Type, R_Type
        }
        #endregion

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_OffColor = Color.White;
        private EqpTypes m_Type = EqpTypes.L_Type;
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
        public EqpTypes EqpType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        public BrokenSensorFixedType()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }

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

            if (m_Type == EqpTypes.L_Type)
            {
                if (m_Tag[tagDescriptor.FRONT_1_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.FRONT_1_DETECT].Value == "1") lblSensorFront_L.BackColor = m_OnColor;
                else lblSensorFront_L.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.FRONT_2_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.FRONT_2_DETECT].Value == "1") lblSensorFront_R.BackColor = m_OnColor;
                else lblSensorFront_R.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.REAR_1_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.REAR_1_DETECT].Value == "1") lblSensorRear_L.BackColor = m_OnColor;
                else lblSensorRear_L.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.REAR_2_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.REAR_2_DETECT].Value == "1") lblSensorRear_R.BackColor = m_OnColor;
                else lblSensorRear_R.BackColor = m_OffColor;
            }
            else if (m_Type == EqpTypes.R_Type)
            {
                if (m_Tag[tagDescriptor.FRONT_1_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.FRONT_1_DETECT].Value == "1") lblSensorFront_R.BackColor = m_OnColor;
                else lblSensorFront_R.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.FRONT_2_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.FRONT_2_DETECT].Value == "1") lblSensorFront_L.BackColor = m_OnColor;
                else lblSensorFront_L.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.REAR_1_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.REAR_1_DETECT].Value == "1") lblSensorRear_R.BackColor = m_OnColor;
                else lblSensorRear_R.BackColor = m_OffColor;

                if (m_Tag[tagDescriptor.REAR_2_DETECT].Value == bool.TrueString || m_Tag[tagDescriptor.REAR_2_DETECT].Value == "1") lblSensorRear_L.BackColor = m_OnColor;
                else lblSensorRear_L.BackColor = m_OffColor;
            }
        }
        #endregion 
    }
}

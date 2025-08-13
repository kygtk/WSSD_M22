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
    public partial class PmChamberPressureState : DmsUserControl
    {
        public enum PressureType
        {
            Base, Process
        }


        #region Tag Descriptor
        TagDescriptorPmChamber tagDescriptor = new TagDescriptorPmChamber();
        #endregion

        #region Fields
        private Color m_OnColor = Color.Lime;
        private Color m_OffColor = Color.White;
        private PressureType m_PressureType = PressureType.Base;
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
        public PressureType CheckPressureType
        {
            get { return m_PressureType; }
            set { m_PressureType = value; }
        }           
        #endregion

        #region Costructor
        public PmChamberPressureState()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("PmChamber");
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
                 
            if (((PressureType.Base == m_PressureType) && (m_Tag[tagDescriptor.ISBASEPRESSURE].Value == bool.TrueString || m_Tag[tagDescriptor.ISBASEPRESSURE].Value == "1")) ||
                ((PressureType.Process == m_PressureType) && (m_Tag[tagDescriptor.ISPROCESSPRESSURE].Value == bool.TrueString || m_Tag[tagDescriptor.ISPROCESSPRESSURE].Value == "1")))
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Cim.Common;
using Dms.Data;
using Dms.Common;

namespace Dms.Control
{
    public partial class CimEqpSolarSubUnitState : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;

        private EqpInfo m_EqpInfo = new EqpInfo();

        private int m_nUnitNo = 0;
        private int m_nSubUnitNo = 0;
        private int m_nState = -1;

        private Color m_NonScheduledStateColor = Color.Red;
        private Color m_UnScheduledDowntimeStateColor = Color.Red;
        private Color m_ScheduledDowntimeStateColor = Color.Red;
        private Color m_EngineeringStateColor = Color.GreenYellow;
        private Color m_StandbyStateColor = Color.Yellow;
        private Color m_ProductiveStateColor = Color.GreenYellow;
        #endregion

        #region Properties
        [Category("DMS : EQP Info"),
        Description("Set Unit Number")]
        public int UnitNo
        {
            get { return m_nUnitNo; }
            set { m_nUnitNo = value; }
        }

        [Category("DMS : EQP Info"),
        Description("Set SubUnit Number")]
        public int SubUnitNo
        {
            get { return m_nSubUnitNo; }
            set { m_nSubUnitNo = value; }
        }


        [Category("DMS : UI"),
        Description("Set Title Text")]
        public string UnitText
        {
            get { return this.lblUnit.Text; }
            set { this.lblUnit.Text = value; }
        }

        [Category("DMS : UI"),
        Description("Set Title Text")]
        public Color UnitBackColor
        {
            get { return this.lblUnit.BackColor ; }
            set { this.lblUnit.BackColor = value; }
        }

        [Category("DMS : UI"),
        Description("Set Title Font")]
        public Font UnitFont
        {
            get { return this.lblUnit.Font; }
            set { this.lblUnit.Font = value; }
        }

        [Category("DMS : UI")]
        public Color NonScheduledStateColor
        {
            get { return m_NonScheduledStateColor; }
            set { m_NonScheduledStateColor = value; }
        }

        [Category("DMS : UI")]
        public Color UnScheduledDowntimeStateColor
        {
            get { return m_UnScheduledDowntimeStateColor; }
            set { m_UnScheduledDowntimeStateColor = value; }
        }

        [Category("DMS : UI")]
        public Color ScheduledDowntimeStateColor
        {
            get { return m_ScheduledDowntimeStateColor; }
            set { m_ScheduledDowntimeStateColor = value; }
        }

        [Category("DMS : UI")]
        public Color EngineeringStateColor
        {
            get { return m_EngineeringStateColor; }
            set { m_EngineeringStateColor = value; }
        }

        [Category("DMS : UI")]
        public Color StandbyStateColor
        {
            get { return m_StandbyStateColor; }
            set { m_StandbyStateColor = value; }
        }

        [Category("DMS : UI")]
        public Color ProductiveStateColor
        {
            get { return m_ProductiveStateColor; }
            set { m_ProductiveStateColor = value; }
        }

        [Browsable(false)]        
        public bool TimerUpdateEnable
        {
            get
            { 
                return m_TimerUpdateEnable; 
            }
            set
            {
                m_TimerUpdateEnable = value;
                this.tmrUpdateState.Enabled = value;
            }
        }
        /// <summary>
        /// If tag is null, this is false - eun 20080110
        /// </summary>
        [Browsable(false)]
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        #region Method
        public void Initilize( ref EqpInfos EqpInfos)
        {
            m_EqpInfo = EqpInfos.GetEqpInfo(m_nUnitNo);

            UpdateState(false);

            if (m_EqpInfo == null || UnitNo == 0 || m_EqpInfo.SolarUnitState.Count <= SubUnitNo)
            {
                MessageBox.Show("Unit Number and SubUnit Number is not set(EqpSolarSubUnitState), Program is Modify!");
                return;
            }

            m_Initialized = true;
        }

        /// <summary>
        /// Update text and color of GlsData - eun 20080110
        /// </summary>
        private void UpdateState(bool bChange)
        {
        }

        /// <summary>
        /// Update GlsData state - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (Initialized)
            {
                if( m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarNonScheduled && m_nState != 1)
                {
                    lblUnit.BackColor = m_NonScheduledStateColor;
                    m_nState = 1;
                }
                else if (m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarUnSheduledDowntimeState && m_nState != 2)
                {
                    lblUnit.BackColor = m_UnScheduledDowntimeStateColor;
                    m_nState = 2;
                }
                else if (m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarScheduledDowntimeState && m_nState != 3)
                {
                    lblUnit.BackColor = m_ScheduledDowntimeStateColor;
                    m_nState = 3;
                }
                else if (m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarEngineeringState && m_nState != 4)
                {
                    lblUnit.BackColor = m_EngineeringStateColor;
                    m_nState = 4;
                }
                else if (m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarStandbyState && m_nState != 5)
                {
                    lblUnit.BackColor = m_StandbyStateColor;
                    m_nState = 5;
                }
                else if (m_EqpInfo.SolarUnitState[SubUnitNo] == eqpSolarStatus.eqpSolarProductiveState && m_nState != 6)
                {
                    lblUnit.BackColor = m_ProductiveStateColor;
                    m_nState = 6;
                }

            }
        }
        #endregion

        public CimEqpSolarSubUnitState()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();
        }
    }
}

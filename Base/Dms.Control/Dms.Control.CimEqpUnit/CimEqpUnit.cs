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
    public partial class CimEqpUnit : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;

        private EqpInfo m_EqpInfo = new EqpInfo();

        private int m_nUnitNo = 0;
        private int m_nMode = -1;

        private Color m_RunColor = Color.GreenYellow;
        private Color m_IdleColor = Color.Yellow;
        private Color m_DownColor = Color.Red;
        private Color m_MaintColor = Color.LightGray;
        #endregion

        #region Properties
        [Category("DMS : EQP Info"),
        Description("Set Unit Number")]
        public int UnitNo
        {
            get { return m_nUnitNo; }
            set { m_nUnitNo = value; }
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
        public Color RunState
        {
            get { return m_RunColor; }
            set { m_RunColor = value; }
        }

        [Category("DMS : UI")]
        public Color IdleState
        {
            get { return m_IdleColor; }
            set { m_IdleColor = value; }
        }

        [Category("DMS : UI")]
        public Color DownState
        {
            get { return m_DownColor; }
            set { m_DownColor = value; }
        }

        [Category("DMS : UI")]
        public Color MaintColor
        {
            get { return m_MaintColor; }
            set { m_MaintColor = value; }
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

            if ( m_EqpInfo == null || UnitNo == 0 )
            {
                MessageBox.Show("Unit Number is not set(EqpUnit), Program is Modify!");
                return;
            }

            m_Initialized = true;
        }

        /// <summary>
        /// Update text and color of GlsData - eun 20080110
        /// </summary>
        private void UpdateState(bool bChange)
        {
/*
            if (bChange == false)
            {
                lblGlsData.Text = "";
                lblGlsData.BackColor = Color.White;
            }
            else
            {
                lblGlsData.Text = m_EqpInfo.m_SubUnit[SubUnitNo - 1].PortNo.ToString() + "-" + (m_EqpInfo.m_SubUnit[SubUnitNo - 1].SlotNo.ToString()).PadLeft(2, '0');
                lblGlsData.BackColor = Color.Yellow;
            }
 */ 
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
                if (m_EqpInfo.UnitStatus == eqpSTATUS.eqpDown && m_nMode != 3)
                {
                    lblUnit.BackColor = m_DownColor;
                    m_nMode = 3;
                }
                else if (m_EqpInfo.UnitStatus == eqpSTATUS.eqpRun && m_nMode != 1)
                {
                    lblUnit.BackColor = m_RunColor;
                    m_nMode = 1;
                }
                else if (m_EqpInfo.UnitStatus == eqpSTATUS.eqpIdle && m_nMode != 2)
                {
                    lblUnit.BackColor = m_IdleColor;
                    m_nMode = 2;
                }
                else if (m_EqpInfo.UnitStatus == eqpSTATUS.eqpMaint && m_nMode != 4)
                {
                    lblUnit.BackColor = m_MaintColor;
                    m_nMode = 4;
                }
            }
        }
        #endregion

        public CimEqpUnit()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();
        }
    }
}

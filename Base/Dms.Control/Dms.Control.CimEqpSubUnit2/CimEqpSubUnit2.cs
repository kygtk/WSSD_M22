using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Cim;
using Dms.Cim.Common;
using Dms.Data;

namespace Dms.Control
{
    public partial class CimEqpSubUnit2 : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;

        private EqpInfo m_EqpInfo = new EqpInfo();

        private int m_nUnitNo = 0;
        private int m_nSubUnitNo = 0;

        private string m_CarrierID = "";
        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("Unit Click Event")]
        public event EventHandler UnitClick;
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
        Description("Set Sub Unit Number")]
        public int SubUnitNo
        {
            get { return m_nSubUnitNo; }
            set { m_nSubUnitNo = value; }
        }

        [Category("DMS : EQP Info"),
        Description("Set Text")]
        public string SubUnitText
        {
            get { return this.lblSubUnit.Text; }
            set { this.lblSubUnit.Text = value; }
        }

        [Category("DMS : EQP Info"),
        Description("Set Font")]
        public Font SubUnitFont
        {
            get { return this.lblSubUnit.Font; }
            set { this.lblSubUnit.Font = value; }
        }

        [Category("DMS : EQP Info"),
        Description("Set GlassData BackColor")]
        public Color GlassDataBackColor
        {
            get { return this.lblSubUnit.BackColor; }
            set { this.lblSubUnit.BackColor = value; }
        }

        [Browsable(false)]
        public bool Initialized
        {
            get { return m_Initialized; }
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

        [Browsable(false)]
        public string CarrierID
        {
            get { return m_CarrierID; }
        }
        #endregion

        #region Method
        public void Initilize(ref EqpInfos EqpInfos)
        {
            m_EqpInfo = EqpInfos.GetEqpInfo(m_nUnitNo);

            UpdateState(false);

            if (m_EqpInfo == null || UnitNo == 0 || m_EqpInfo.SubUnitGlassData.Count < SubUnitNo)
            {
                MessageBox.Show("Unit Number and SubUnit Number is not set(EqpSubUnit2), Program is Modify!");
                return;
            }

            m_Initialized = true;
        }
        #endregion

        /// <summary>
        /// Update text and color of GlsData - eun 20080110
        /// </summary>
        private void UpdateState(bool bChange)
        {
            if (bChange == false)
            {
                lblSubUnit.Text = "";
                lblSubUnit.BackColor = Color.White;
            }
            else
            {
                lblSubUnit.Text = m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].CarrierId.ToString();

                lblSubUnit.BackColor = Color.Yellow;
            }
        }

        public CimEqpSubUnit2()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            if (Initialized)
            {
                if (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].CarrierId != "" &&
                    m_EqpInfo.SubUnitGlassData[SubUnitNo-1].CarrierId != null )
                {
                    m_CarrierID = m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].CarrierId;

                    UpdateState(true);
                }
                else
                {
                    if (lblSubUnit.Text != "") UpdateState(false);
                }
            }
        }

        private void lblSubUnit_Click(object sender, EventArgs e)
        {
            Unit_Click(this, e);
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            if (UnitClick != null)
            {
                UnitClick(this, e);
            }
        }
    }
}

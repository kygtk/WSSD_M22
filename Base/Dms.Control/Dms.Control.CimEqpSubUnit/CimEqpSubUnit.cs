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
    public partial class CimEqpSubUnit : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;

        private EqpInfo m_EqpInfo = new EqpInfo();
//        private LotInfos m_LotInfos = null;

        private int m_nUnitNo = 0;
        private int m_nSubUnitNo = 0;

        private int m_nPortNo = 0;
        private int m_nSlotNo = 0;

        //private bool m_IsValid = false;
        private bool m_IsClick = false;
        //private string m_TooltipMessage = "";
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

        /// <summary>
        /// If tag is null, this is false - eun 20080110
        /// </summary>
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
        public int PortNo
        {
            get { return m_nPortNo; }
        }

        [Browsable(false)]
        public int SlotNo
        {
            get { return m_nSlotNo; }
        }
/*
        public LotInfos LotInfos
        {
            get { return m_LotInfos; }
            set { m_LotInfos = value; }
        }
*/
        #endregion

        #region Method
        public void Initilize( ref EqpInfos EqpInfos)
        {
            m_EqpInfo = EqpInfos.GetEqpInfo(m_nUnitNo);

            UpdateState(false);

            if ( m_EqpInfo == null || UnitNo == 0 ||  m_EqpInfo.SubUnitGlassData.Count < SubUnitNo )
            {
                MessageBox.Show("Unit Number and SubUnit Number is not set(EqpSubUnit), Program is Modify!");
                return;
            }

            m_Initialized = true;
        }

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
                lblSubUnit.Text = m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo.ToString() + "-" + (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo.ToString()).PadLeft(2, '0');

                lblSubUnit.BackColor = LotColorInfo.GetColor(m_nPortNo);
            }
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
                if ((m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo >= 0 &&
                      m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo >= 0) &&
                    (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo != 0 &&
                      m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo != 0))
                {
                    m_nPortNo = m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo;
                    m_nSlotNo = m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo;

                    //m_IsValid = true;

                    UpdateState(true);
                    SetTooltip();
                }
                else if (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo == 0 ||
                          m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo == 0)
                {
                    //m_IsValid = false;

                    if (lblSubUnit.Text != "") UpdateState(false);
                }
            }
        }
        #endregion

        public CimEqpSubUnit()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();
        }

        private void SetTooltip()
        {
/*
            if (m_LotInfos == null) return;

            if (m_IsValid)
            {
                LotInfo lotinfo = m_LotInfos.GetLotInfo(m_nPortNo);

                if (lotinfo != null)
                {
                    m_TooltipMessage = "LOT NO \t: " + lotinfo.LotNo + "\n" +
                                       "CASSETTE ID \t: " + lotinfo.CSTID + "\n" +
                                       "LOT ID \t\t: " + lotinfo.LOTID + "\n" +
                                       "RECIPE ID \t: " + lotinfo.RecipeId;
                }
                else
                {
                    m_TooltipMessage = "LOT NO \t: " + "------" + "\n" +
                                       "CASSETTE ID \t: " + "------" + "\n" +
                                       "LOT ID \t\t: " + "------" + "\n" +
                                       "RECIPE ID \t: " + "------";
                }
            }
 */ 
        }

        private void lblSubUnit_Click(object sender, EventArgs e)
        {
/*
            if (m_LotInfos == null) return;

            if (m_IsValid)
            {
                toolTip.Active = true;
                toolTip.Hide(lblSubUnit);
                toolTip.Show(m_TooltipMessage, lblSubUnit);
                m_IsClick = true;
            }
*/
            Unit_Click(this, e);
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            if (UnitClick != null)
            {
                UnitClick(this, e);
            }
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            if (m_IsClick)
            {
                m_IsClick = false;
            }
            else
            {
                toolTip.Active = false;
                e.Cancel = true;
            }
        }
    }
}
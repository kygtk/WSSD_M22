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
    public partial class CimEqpSubUnitGlassView : UserControl
    {
        #region Fields
        private bool m_Initialized = false;
        private bool m_TimerUpdateEnable = false;

        private EqpInfo m_EqpInfo = new EqpInfo();

        private int m_nUnitNo = 0;
        private int m_nSubUnitNo = 0;
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
                this.timerUpdateState.Enabled = value;
            }
        }
        #endregion

        #region Methods
        public CimEqpSubUnitGlassView()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.CacheText, true);

            InitializeComponent();
        }
        
        public void Initilize( ref EqpInfos EqpInfos)
        {
            m_EqpInfo = EqpInfos.GetEqpInfo(m_nUnitNo);

            UpdateState(false);

            if ( m_EqpInfo == null || UnitNo == 0 ||  m_EqpInfo.SubUnitGlassData.Count < SubUnitNo )
            {
                MessageBox.Show("Unit Number and SubUnit Number is not set(EqpSubUnitGlassView), Program is Modify!");
                return;
            }

            m_Initialized = true;
        }

        private void timerUpdateState_Tick(object sender, EventArgs e)
        {
            if (m_Initialized == true)
            {
                if ((m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo >= 0 &&
                      m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo >= 0) &&
                    (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo != 0 &&
                      m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo != 0))
                {
                    UpdateState(true);
                }
                else if (m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].PortNo == 0 ||
                          m_EqpInfo.SubUnitGlassData[SubUnitNo - 1].SlotNo == 0)
                {
                    UpdateState(false);
                }
            }
        }

        private void UpdateState(bool bChange)
        {
            if (bChange == false)
            {
                this.lblGlass.BackColor = Color.Transparent;
            }
            else
            {
                this.lblGlass.BackColor = Color.DodgerBlue;
            }
        }

        public void BorderEnable(bool enable)
        {
            if (enable == true)
            {
                this.lblGlass.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                this.lblGlass.BorderStyle = BorderStyle.None;
            }
        }
        #endregion
    }
}

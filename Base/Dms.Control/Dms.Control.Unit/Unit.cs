///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : Cv Unit's unit UserControl
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
    [ToolboxBitmap(typeof(Unit), "UnitAni.bmp")]
    public partial class Unit : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorEqpUnit tagDescriptor = new TagDescriptorEqpUnit();
        #endregion


        #region Fields
        private Color m_NormalColor = Color.GreenYellow;
        private Color m_IdleColor = Color.Yellow;
        private Color m_FaultColor = Color.Red;
        private Color m_UnKnownColor = SystemColors.GradientInactiveCaption;
        private Color m_PMColor = Color.Orange;
        //private bool m_bTrigger = false;
        #endregion

        #region Properties
        [Category("DMS : Basic Info"),
         Description("Name shown on the view")]
        public string UnitName  //TODO:나중에 tag랑 연결되면 서버에서 만들어진 UnitName과 연결이 가능하도록?
        {
            get { return lblUnit.Text; }
            set { lblUnit.Text = value; }
        }

        [Category("DMS : UI")]
        public Color NormalState
        {
            get { return m_NormalColor; }
            set { m_NormalColor = value; }
        }

        [Category("DMS : UI")]
        public Color IdleState
        {
            get { return m_IdleColor; }
            set { m_IdleColor = value; }
        }

        [Category("DMS : UI")]
        public Color FaultState
        {
            get { return m_FaultColor; }
            set { m_FaultColor = value; }
        }

        [Category("DMS : UI")]
        public Color UnknownState
        {
            get { return m_UnKnownColor; }
            set { m_UnKnownColor = value; }
        }

        [Category("DMS : UI")]
        public Color PMState
        {
            get { return m_PMColor; }
            set { m_PMColor = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter Unit contstructor - eun 20080110
        /// </summary>
        public Unit()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update blink color of Unit - eun 20080110
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateAnimation_Tick(object sender, EventArgs e)
        {
            //m_bTrigger = !m_bTrigger;
            //if (m_Tag[tagDescriptor.CYCLESTOP].Value == bool.TrueString)
            //{
            //    if (m_bTrigger) lblUnit.BackColor = Color.Orange;
            //    else lblUnit.BackColor = Color.Lime;
            //}
            //else if (m_Tag[tagDescriptor.CLEANOUT].Value == bool.TrueString)
            //{
            //    if (m_bTrigger) lblUnit.BackColor = Color.Orange;
            //    else lblUnit.BackColor = Color.Cyan;
            //}
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

        /// <summary>
        /// Update color of Unit - eun 20080110
        /// </summary>
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            if (m_Tag[tagDescriptor.EQP_STATE].Value == EqpState.Fault.ToString())
            {
                lblUnit.BackColor = m_FaultColor;
            }
            else if (m_Tag[tagDescriptor.EQP_STATE].Value == EqpState.Normal.ToString())
            {
                if (m_Tag[tagDescriptor.PROCESS_STATE].Value == ProcessState.Idle.ToString())
                {
                    lblUnit.BackColor = m_IdleColor;
                }
                else lblUnit.BackColor = m_NormalColor;
            }
            else if (m_Tag[tagDescriptor.EQP_STATE].Value == EqpState.UnKnown.ToString())
            {
                lblUnit.BackColor = m_UnKnownColor;
            }
            else if (m_Tag[tagDescriptor.EQP_STATE].Value == EqpState.PM.ToString())
            {
                lblUnit.BackColor = m_PMColor;
            }
        }
        #endregion
    }
}

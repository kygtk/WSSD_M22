///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.11
// Author       : eun
// Description  : AutoValve Open/Close Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.Control.Hmi
{
    public partial class DlgAutoValve2 : Form
    {
        #region Fields
		private Dms.Device.HmiAutoValve m_Valve = null;
        #endregion

        #region Delegates
        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// No Parameter Constructor - eun 200870111
        /// </summary>
        public DlgAutoValve2()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    
        /// <summary>
        /// Having name parameter constructor - eun 20080111
        /// </summary>
        /// <param name="name"></param>
        public DlgAutoValve2(Dms.Device.HmiAutoValve valve)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			m_Valve = valve;
        }
        #endregion

        #region Methods
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            
			tmrUpdateState.Enabled = false;
            
			this.Close();
        }

		private void DlgAutoValve_Load(object sender, EventArgs e)
        {
			this.Text = m_Valve.Name;

			btnOpen.DoSwitch = m_Valve.DoOpenSwitch;
			btnOpen.Initialize(false);

			btnClose.DoSwitch = m_Valve.DoCloseSwitch;
			btnClose.Initialize(false);

			m_Valve.AoSelectedValveNo.SetState(m_Valve.ValveNo);

			btnOk.Focus();
            
			UpdateState();
            
			tmrUpdateState.Enabled = true;
        } 

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
			UpdateState();
        }

        private void UpdateState()
        {
			//bool valveOpen = m_Valve.DiValveOpenStatus.GetState();
			//if (valveOpen)
			//{
			//    this.btnOpen.Enabled = false;
			//    this.btnClose.Enabled = true;
			//}
			//else 
			//{
			//    this.btnOpen.Enabled = true;
			//    this.btnClose.Enabled = false;
			//}
        }
        #endregion
	}
}
///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : eun
// Description  : Default Gauge Select Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data;

namespace Dms.Control
{
    public partial class DlgSelGauge : Form
    {
        #region Fields
        private TagCalibrationInfo m_SelectedInfo = null;
        private string m_Name = "";
        #endregion

        #region Properties
        /// <summary>
        /// DlgCalibration watch it - eun 20080115
        /// </summary>
        public TagCalibrationInfo SelectedInfo
        {
            get { return m_SelectedInfo; }
        } 
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter DlgSelGauge contstructor - eun 20080115
        /// </summary>
        public DlgSelGauge()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        } 
        #endregion

        #region Methods

        public void Initialize(string name)
        {
            m_Name = name;
        }
        /// <summary>
        /// Make new TagCalibrationInfo by selected GaugeType - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SelectedInfo = new TagCalibrationInfo((GaugeType)(((ListBox)sender).SelectedItem), m_Name);
        }

        /// <summary>
        /// Close the form - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Initialize Listbox - eun 20080115
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgSelGauge_Load(object sender, EventArgs e)
        {
            lbList.DataSource = Enum.GetValues(typeof(GaugeType));
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormTagSelect : Form
    {
        #region Fields
        private DeviceTag m_SelectedTag = null;
        #endregion

        #region Properties
        public DeviceTag SelectedTag
        {
            get { return m_SelectedTag; }
        }
        #endregion

        #region Constructor
        public FormTagSelect()
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
        public void Initialize(DeviceTag deviceTag)
        {
            this.viewTagSelect.InitializeCurrentTagTree(deviceTag);

            this.viewTagSelect.InitializeTagsTreeByFilter(deviceTag);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.m_SelectedTag = viewTagSelect.CurTag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
    }
}
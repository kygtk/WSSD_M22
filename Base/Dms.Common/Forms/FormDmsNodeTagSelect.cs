using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormDmsNodeTagSelect : Form
    {
        #region Fields
        private DmsNodeTag m_SelectedTag = null;
        #endregion

        #region Properties
        public DmsNodeTag SelectedTag
        {
            get { return m_SelectedTag; }
        }
        #endregion

        #region Constructor
        public FormDmsNodeTagSelect()
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
        public void Initialize(DmsNodeTag deviceTag)
        {
            this.viewDmsNodeTagSelect.InitializeCurrentTagTree(deviceTag);

            this.viewDmsNodeTagSelect.InitializeTagsTreeByFilter(deviceTag);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.m_SelectedTag = viewDmsNodeTagSelect.CurTag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
    }
}
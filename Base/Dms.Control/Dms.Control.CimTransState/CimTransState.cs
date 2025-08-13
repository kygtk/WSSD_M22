///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : jemoon
// Description  : General Server Data UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경
// * 2009.09.30 - cim에서 사용할 수 있도록 수정

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
    public partial class CimTransState : UserControl
    {
        #region Fields
        #endregion

        #region Properties
        [Category("DMS : Basic Info"),
        Description("Select Title Text")]
        public string TitleText
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title BackColor")]
        public Color TitleBackColor
        {
            get { return this.lblTitle.BackColor; }
            set { this.lblTitle.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title ForeColor")]
        public Color TitleForeColor
        {
            get { return this.lblTitle.ForeColor; }
            set { this.lblTitle.ForeColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title Text Font")]
        public Font TitleTextFont
        {
            get { return this.lblTitle.Font; }
            set { this.lblTitle.Font = value; }
        }
        [Category("DMS : UI"),
        Description("Adjust Title panel size")]
        public int TitlePanelSize
        {
            get { return this.splitContainer1.SplitterDistance; }
            set { this.splitContainer1.SplitterDistance = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title panel Border Style")]
        public BorderStyle TitlePanelBorderStyle
        {
            get { return lblTitle.BorderStyle; }
            set { lblTitle.BorderStyle = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title panel text align")]
        public ContentAlignment TitlePanelTextAlign
        {
            get { return lblTitle.TextAlign; }
            set { lblTitle.TextAlign = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value BackColor")]
        public Color ValueBackColor
        {
            get { return this.lblValue.BackColor; }
            set { this.lblValue.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value ForeColor")]
        public Color ValueForeColor
        {
            get { return this.lblValue.ForeColor; }
            set { this.lblValue.ForeColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value Text Font")]
        public Font ValueTextFont
        {
            get { return this.lblValue.Font; }
            set { this.lblValue.Font = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value Border Style")]
        public BorderStyle ValueBorderStyle
        {
            get { return lblValue.BorderStyle; }
            set { lblValue.BorderStyle = value; }
        }
        [Category("DMS : UI"),
        Description("Select Value text align")]
        public ContentAlignment ValueTextAlign
        {
            get { return lblValue.TextAlign; }
            set { lblValue.TextAlign = value; }
        }
        [Category("DMS : UI"),
        Description("Adjust distance between Title panel and Value")]
        public int Distance
        {
            get { return splitContainer1.SplitterWidth; }
            set { splitContainer1.SplitterWidth = value; }
        }
        #endregion

        #region Events
        [Category("DMS : EVENT"), Description("Click Event")]
        public event EventHandler ValueClick;
        #endregion

        #region Constructor
        public CimTransState()
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
        private void lblValue_Click(object sender, EventArgs e)
        {
            if (ValueClick != null)
            {
                ValueClick(sender, e);
            }
        }

        public void SetText(string text)
        {
            lblValue.Text = text;
        }
        #endregion
    }
}

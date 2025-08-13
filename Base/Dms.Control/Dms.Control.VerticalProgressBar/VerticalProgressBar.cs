///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.14
// Author       : eun
// Description  : Vertical Progess Bar UserControl
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class VerticalProgressBar : UserControl
    {
        #region Fields
        private Color m_Color = Color.Blue;
        private int m_Maximum = 100;
        private int m_Minimum = 0;
        private int m_Value = 50;
        #endregion

        #region Properties
        [Category("DMS : UI")]
        public Color FillColor
        {
            get { return m_Color; }
            set 
            { 
                m_Color = value;
                Invalidate();
            }
        }

        [Category("DMS : UI")]
        public int Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                Invalidate();
            }
        }

        [Category("DMS : UI")]
        public int Maximum
        {
            get { return m_Maximum; }
            set
            {
                m_Maximum = value;
                Invalidate();
            }
        }
        #endregion

        #region Constructor
        public VerticalProgressBar()
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
        private void DrawBar(Graphics dc)
        {
            if (m_Maximum == m_Minimum || m_Value == 0) return;

            int height;		// the bar height

            height = m_Value * this.Height / (m_Maximum - m_Minimum); // the bar height

            dc.FillRectangle(new SolidBrush(m_Color), 0, this.Height - height, this.Width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            DrawBar(dc);

            base.OnPaint(e);
        }
        #endregion
    }
}

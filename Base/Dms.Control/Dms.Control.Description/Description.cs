using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class Description : UserControl
    {
        #region Fields
        private int m_BorderWidth = 3;
        #endregion


        #region Properties
        [Category("DMS : UI")]
        public Color DescriptionBackColor
        {
            get { return lblDescription.BackColor; }
            set { lblDescription.BackColor = value; }
        }
        [Category("DMS : UI")]
        public Color DescriptionForeColor
        {
            get { return lblDescription.ForeColor; }
            set { lblDescription.ForeColor = value; }
        }
        [Category("DMS : UI")]
        public string DescriptionText
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }
        [Category("DMS : UI")]
        public Font DescriptionFont
        {
            get { return lblDescription.Font; }
            set { lblDescription.Font = value; }
        }
        [Category("DMS : UI")]
        public int BorderWidth
        {
            get { return m_BorderWidth; }
            set 
            { 
                m_BorderWidth = value;
                SetBorderWidth();
            }
        }
        #endregion

        public Description()
        {
            InitializeComponent();
        }

        public void SetBorderWidth()
        {
            this.lblDescription.Size = new Size(this.Size.Width - m_BorderWidth * 2, this.Size.Height - m_BorderWidth * 2);
            this.lblDescription.Location = new Point(m_BorderWidth, m_BorderWidth);
        }
    }
}

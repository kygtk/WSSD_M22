using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control.UnderlineLabel
{
    public partial class UnderlineLabel : UserControl
    {
        #region Fields
        private string m_LabelText = "Label";
        private Color m_TextColor = Color.FromArgb(0,65,153);
        private Color m_BGColor = Color.Transparent;
        private Color m_LineColor = Color.FromArgb(0,65,153);

        private string m_OldText = "Label";
        private Color m_OldTextColor = Color.FromArgb(0, 65, 153);
        private Color m_OldBGColor = Color.Transparent;
        private Color m_OldLineColor = Color.FromArgb(0, 65, 153);
        #endregion
        #region Properties
        [Category("DMS : UI")]
        public string LabelText
        {
            get { return m_LabelText; }
            set { m_LabelText = value; }
        }
        [Category("DMS : UI")]
        public Color TextColor
        {
            get { return m_TextColor; }
            set { m_TextColor = value; }
        }
        [Category("DMS : UI")]
        public Color BGColor
        {
            get { return m_BGColor; }
            set { m_BGColor = value; }
        }
        [Category("DMS : UI")]
        public Color LineColor
        {
            get { return m_LineColor; }
            set { m_LineColor = value; }
        }
        #endregion


        #region Constructor
        public UnderlineLabel()
        {
            InitializeComponent();

            this.lblText.Text = m_LabelText;
            this.lblText.ForeColor = m_TextColor;
            this.lblText.BackColor = m_BGColor;
            this.lblUnderline.BackColor = m_LineColor;

            timerUpdate.Enabled = true;
        }
        #endregion

        #region Methods
        public void SetText(string text)
        {
            m_LabelText = text;
            this.lblText.Text = text;
        }

        public void SetTextColor(Color color)
        {
            m_TextColor = color;
            this.lblText.ForeColor = color;
        }

        public void SetBackColor(Color color)
        {
            m_BGColor = color;
            this.lblText.BackColor = color;
        }

        public void SetLineColor(Color color)
        {
            m_LineColor = color;
            this.lblUnderline.BackColor = color;
        }
        #endregion

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (m_LabelText != m_OldText)
            {
                m_OldText = m_LabelText;
                this.lblText.Text = m_OldText;
            }

            if (m_TextColor != m_OldTextColor)
            {
                m_OldTextColor = m_TextColor;
                this.lblText.ForeColor = m_OldTextColor;
            }

            if (m_BGColor != m_OldBGColor)
            {
                m_OldBGColor = m_BGColor;
                this.lblText.BackColor = m_OldBGColor;
            }

            if (m_LineColor != m_OldLineColor)
            {
                m_OldLineColor = m_LineColor;
                this.lblUnderline.BackColor = m_OldLineColor;
            }
        }
    }
}
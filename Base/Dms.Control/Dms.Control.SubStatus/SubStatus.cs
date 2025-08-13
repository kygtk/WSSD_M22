using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class SubStatus : UserControl
    {
        #region Properties
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName1BackColor
        {
            get { return lblStatusName1.BackColor; }
            set { lblStatusName1.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName1ForeColor
        {
            get { return lblStatusName1.ForeColor; }
            set { lblStatusName1.ForeColor = value; }
        }

        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName2BackColor
        {
            get { return lblStatusName2.BackColor; }
            set { lblStatusName2.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName2ForeColor
        {
            get { return lblStatusName2.ForeColor; }
            set { lblStatusName2.ForeColor = value; }
        }

        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName3BackColor
        {
            get { return lblStatusName3.BackColor; }
            set { lblStatusName3.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatusName3ForeColor
        {
            get { return lblStatusName3.ForeColor; }
            set { lblStatusName3.ForeColor = value; }
        }



        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus1BackColor
        {
            get { return lblStatus1.BackColor; }
            set { lblStatus1.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus1ForeColor
        {
            get { return lblStatus1.ForeColor; }
            set { lblStatus1.ForeColor = value; }
        }

        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus2BackColor
        {
            get { return lblStatus2.BackColor; }
            set { lblStatus2.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus2ForeColor
        {
            get { return lblStatus2.ForeColor; }
            set { lblStatus2.ForeColor = value; }
        }

        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus3BackColor
        {
            get { return lblStatus3.BackColor; }
            set { lblStatus3.BackColor = value; }
        }
        [Category("Color"), Description("Label color Setting")]
        public Color SubStatus3ForeColor
        {
            get { return lblStatus3.ForeColor; }
            set { lblStatus3.ForeColor = value; }
        } 
        [Category("Text"), Description("Status Name")]
        public string StatusName1Text
        {
            get{ return lblStatusName1.Text; }
            set{ lblStatusName1.Text = value; }
        }
        [Category("Text"), Description("Status Name")]
        public string StatusName2Text
        {
            get{ return lblStatusName2.Text; }
            set{ lblStatusName2.Text = value; }
        }
        [Category("Text"), Description("Status Name")]
        public string StatusName3Text
        {
            get{ return lblStatusName3.Text; }
            set{ lblStatusName3.Text = value; }
        }
        #endregion

        #region Methods

        public void SetStatus1Text(string text)
        {
            lblStatus1.Text = text;
        }
        public void SetStatus2Text(string text)
        {
            lblStatus2.Text = text;
        }
        public void SetStatus3Text(string text)
        {
            lblStatus3.Text = text;
        }
        #endregion

        public SubStatus()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }
    }
}
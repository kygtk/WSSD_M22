using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace Dms.Common
{
    public partial class SplashScreen : Form
    {
        public SplashScreen(Image backGroundImage)
        {
            InitializeComponent();

            if (backGroundImage != null)
            {
                this.BackgroundImage = backGroundImage;
                this.ClientSize = this.BackgroundImage.Size;
            }
        }

        public SplashScreen(Image backGroundImage, int duration)
        {
            InitializeComponent();

            if (backGroundImage != null)
            {
                this.BackgroundImage = backGroundImage;
                this.ClientSize = this.BackgroundImage.Size;
            }

            timer1.Interval = duration;
            timer1.Start();
        }

        public void Exit()
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Exit();
        }
    }
}
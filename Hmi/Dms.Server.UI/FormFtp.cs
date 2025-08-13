using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Ctl;

namespace Dms.Server
{
    public partial class FormFtp : Form
    {
        private string m_FileNameShort = "";
        public FormFtp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.textFileName.Text = dlg.FileName;
                //m_FileNameShort = dlg.SafeFileName;
            }

        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            //XFtp ftp = new XFtp();
            //ftp.FtpUpload(this.textIpAddress.Text,
            //    textPort.Text, textTargetDir.Text, 
            //    textUserID.Text, textPassword.Text,
            //    textFileName.Text, m_FileNameShort);
            FtpUploadInfo info = new FtpUploadInfo(
                this.textIpAddress.Text,
                this.textPort.Text,
                this.textTargetDir.Text,
                this.textFileName.Text,
                this.m_FileNameShort,
                this.textUserID.Text,
                this.textPassword.Text);
            FtpUploadFileQueue.Instance.Add(info);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Server;
using Dms.Common;
using Dms.Control;

namespace TestHMI
{
    public partial class Form1 : Form
    {
        private ClientManager m_ClientManager = null;

        public Form1()
        {
            m_ClientManager = ClientManager.Instance;
            m_ClientManager.Initialize();
            InitializeComponent();
            if (panel1.Controls.Count != 0) propertyGrid1.SelectedObject = panel1.Controls[0];

            //DmsUserControl ucon;
            //foreach (Control control in panel1.Controls)
            //{
            //    ucon = control as DmsUserControl;
            //    if (ucon != null) ucon.Initialize(m_ClientManager.DataProvider.TagContainer);
            //    if (control.GetType() == typeof(TagButton)) ((TagButton)control).Initialize(m_ClientManager.DataProvider.TagContainer);
            //    if (control.GetType() == typeof(CylinderGroupBox)) Initialize(control as CylinderGroupBox);
            //}

            AppConfig app = new AppConfig();
            ServerMode serverMode = ClientManager.Instance.ServerMode;
            if (app.AutoStart || (app.UserControlBuild && (serverMode == ServerMode.Remoting)))
            {
                DmsUserControl.InitializeAll(this, m_ClientManager.DataProvider.TagContainer);
            }

            timer1.Enabled = true;
        }

        //private void Initialize(CylinderGroupBox groupBox)
        //{
        //    DmsUserControl ucon;
        //    foreach (Control control in groupBox.Controls)
        //    {
        //        ucon = control as DmsUserControl;
        //        if (ucon != null) ucon.Initialize(m_ClientManager.DataProvider.TagContainer);
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            m_ClientManager.Uninitialize();
            this.Close();
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            FormServer form = new FormServer();
            form.Initialize(false);
            form.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool enable = true;
            enable &= m_ClientManager.GenInfos.AutoMode;
            enable &= !m_ClientManager.GenInfos.EqpInitReq;
            enable &= !m_ClientManager.GenInfos.EqpInitComp;
            //tagButton5.Enabled = enable;
        }
    }
}
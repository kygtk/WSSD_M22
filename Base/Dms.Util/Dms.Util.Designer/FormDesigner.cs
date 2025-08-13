using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Design;
using System.Management;
using Dms.Util.IODefine;
using Dms.ServerCommon;
using Dms.Common;
using Dms.Device;

namespace Dms.Util
{
    public partial class FormDesigner : Form
    {
        private IoDefines m_IoDefines = new IoDefines();
        private DmsComponents m_Components = DmsComponents.Instance;
        private DeviceTags m_DeviceTagContainer = new DeviceTags();
        private GenInfoHandler m_GenInfoHandler = GenInfoHandler.Instance;// to generate items             

        public FormDesigner()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void Initialize(IServerManager server, DesignerMode designMode)
        {
            this.viewEqpConfig1.DesignerMode = designMode;
            this.viewIOConfig1.DesignerMode = designMode;

            m_IoDefines = server.IoDefines;
            m_DeviceTagContainer = server.TagContainer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.viewEqpConfig1.Initialize(m_Components);
            this.viewIOConfig1.Initialize(m_IoDefines);
            this.viewIOConfig1.EnableBomMaker = true;
            this.viewIOEdit1.Initialize(m_IoDefines, ViewIOEdit.OpMode.Config);
            this.viewSlaveEdit1.Initialize(m_IoDefines, ViewSlaveEdit.OpMode.Config);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Exit?", "WSSD Designer", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabIoDefine)
            {
                if (this.viewIOConfig1.Initialized)
                {
                    m_IoDefines = this.viewIOConfig1.SelectedDefines;
                }

                this.viewIOConfig1.Initialize(m_IoDefines);
            }
            else if (this.tabControl1.SelectedTab == this.tabIoEdit)
            {
                m_IoDefines = this.viewIOConfig1.SelectedDefines;
                m_IoDefines.InitializeIOCollection();

                this.viewIOEdit1.Initialize(m_IoDefines, ViewIOEdit.OpMode.Config);
            }
            else if (this.tabControl1.SelectedTab == this.tabSlaveEdit)
            {
                m_IoDefines = this.viewIOConfig1.SelectedDefines;
                m_IoDefines.InitializeIOCollection();

                this.viewSlaveEdit1.Initialize(m_IoDefines, ViewSlaveEdit.OpMode.Config);
            }
        }

        private void buttonAppConfig_Click(object sender, EventArgs e)
        {
            FormAppConfigHelper form = new FormAppConfigHelper();
            form.ShowDialog();
        }

        private void buttonAppConfigLoadFrom_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Read From XML file";
            dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                FormAppConfigHelper form = new FormAppConfigHelper();
                form.LoadFromPath = dlg.FileName;
                form.ShowDialog();
            }
        }
    }
}
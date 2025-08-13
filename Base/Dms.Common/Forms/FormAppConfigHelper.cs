using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormAppConfigHelper : Form
    {
        private AppConfig m_AppConfig = AppConfig.Instance;
        public string LoadFromPath = ""; 

        public FormAppConfigHelper()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_AppConfig.WriteXml();
            }
        }

        private void FormAppConfigHelper_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LoadFromPath))
            {
                m_AppConfig.ReadXml();
            }
            else
            {
                m_AppConfig.ReadXml(LoadFromPath);
            }
            this.propertyGrid1.SelectedObject = m_AppConfig;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save As...";
                dlg.CreatePrompt = true;
                dlg.OverwritePrompt = true;
                dlg.FileName = m_AppConfig.GetType().Name + ".xml";
                dlg.DefaultExt = "xml";
                dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (DialogResult.OK == dlg.ShowDialog())
                {
                    m_AppConfig.WriteXml(dlg.FileName);
                }
            }
        }
    }
}
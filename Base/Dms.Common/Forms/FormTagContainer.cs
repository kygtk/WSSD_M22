using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormTagContainer : Form
    {
        private DeviceTags m_Tags = null;

        public FormTagContainer()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(DeviceTags tags)
        {
            m_Tags = tags;
            this.viewTagContainer1.Initialize(m_Tags);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.Close();
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = m_Tags.GetType().Name + ".xml";
            dlg.DefaultExt = "xml";
            //dlg.InitialDirectory = m_Tags.FileName;
            dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    m_Tags.WriteXml(dlg.FileName);
                }
                catch (Exception ex)    //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.viewTagContainer1.UpdateTree();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = checkBox1.Checked;
        }
    }
}
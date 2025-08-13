using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Util.IODefine
{
    public partial class FormBomAnalysis : Form
    {
        private IoDefines m_IoDefines;

        public FormBomAnalysis()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormBomAnalysis_Load(object sender, EventArgs e)
        {
            this.viewBomAnalysis1.Initialize(m_IoDefines);
        }

        public void Initialize(IoDefines iodefines)
        {
            m_IoDefines = iodefines;
        }
    }
}
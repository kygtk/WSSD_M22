using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class FormSimulationConfigHelper : Form
    {
        public FormSimulationConfigHelper()
        {
            InitializeComponent();
        }

        private void FormSimulationConfigHelper_Load(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = AppConfig.Instance.Simul;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
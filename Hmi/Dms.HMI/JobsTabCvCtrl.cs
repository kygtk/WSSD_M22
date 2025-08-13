using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Data;

namespace Dms.HMI
{
    public partial class JobsTabCvCtrl : UserControl
    {
        public JobsTabCvCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
        }

        #region Methods
        public void Initialize(SetupGenInfoProvider provider)
        {
            provider.Viewer.Add(this.viewSetupGeneral);
            this.viewSetupGeneral.InitDataView(provider.Adapter.Table, false);
        }

        public void Initialize(RecipeProvider provider)
        {
            this.viewRecipe.InitDataView(provider, true, true);
        }

        private void dataCurGlsCount_ValueClick(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to reset the history", "WSSD", MessageBoxButtons.YesNo))
            {
                ClientManager client = ClientManager.Instance;
                client.SendCommand(Command.CurGlassCountReset);
            }
        }
        #endregion
    }
}

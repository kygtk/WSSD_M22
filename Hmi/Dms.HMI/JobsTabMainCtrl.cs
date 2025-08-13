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
using Dms.Device;//12.01.17 sungyong

namespace Dms.HMI
{
    public partial class JobsTabMainCtrl : UserControl
    {
        #region Fields
        private ClientManager m_Client = ClientManager.Instance;
        #endregion

        #region Constructor
        public JobsTabMainCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

        }
        #endregion

        #region Methods
        public void Initialize(SetupGenInfoProvider provider)
        {
            provider.Viewer.Add(this.viewSetupGeneral);
            this.viewSetupGeneral.InitDataView(provider.Adapter.Table, false);
            timer1.Enabled = true; // 09.12.28 minhan


        }

        public void Initialize(RecipeProvider provider)
        {
            this.viewRecipe.InitDataView(provider, true, true);
            //this.viewRecipe.GlassSize = m_Client.EventSubscriber.Server.SetupGlassSize.GetValue<int>();//2009.07.30 kimgun 10.12.25 minhan
            this.viewRecipe.GenInfos = m_Client.GenInfos;//2009.08.17 kimgun
            timer1.Enabled = true; // 09.12.28 minhan
        }

        private void dataCurGlsCount_ValueClick(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to reset the history", "WSSD", MessageBoxButtons.YesNo))
            {
                //ClientManager client = ClientManager.Instance;
                //client.SendCommand(Command.CurGlassCountReset);
                m_Client.SendCommand(Command.CurGlassCountReset);
            }
        }

        private void Update_Cylinder(object sender, EventArgs e) // 09.12.28 minhan
        {
            TagInterlockCondition DoorCondition = DoorSensor.InterlockCondition;//12.01.17 sungyong

            if (m_Client.GenInfos.AutoMode || DoorCondition.IsAlarm)//12.01.17 sungyong
            {
                cylinder1.Enabled = false;
                cylinder2.Enabled = false;
                cylinder3.Enabled = false;
                cylinder4.Enabled = false;
                cylinder5.Enabled = false;
                cylinder6.Enabled = false;
                //cylinder7.Enabled = false;
                //cylinder8.Enabled = false;
                cylinder9.Enabled = false;
                //cylinder10.Enabled = false;
                //cylinder11.Enabled = false;
                cylinder12.Enabled = false;
                cylinder13.Enabled = false;
                cylinder14.Enabled = false;
            }
            else
            {
                cylinder1.Enabled = true;
                cylinder2.Enabled = true;
                cylinder3.Enabled = true;
                cylinder4.Enabled = true;
                cylinder5.Enabled = true;
                cylinder6.Enabled = true;
                //cylinder7.Enabled = true;
                //cylinder8.Enabled = true;
                cylinder9.Enabled = true;
                //cylinder10.Enabled = true;
                //cylinder11.Enabled = true;
                cylinder12.Enabled = true;
                cylinder13.Enabled = true;
                cylinder14.Enabled = true;
            }
        }
        #endregion

        private void glsSensor8_Load(object sender, EventArgs e)
        {

        }

        private void groupBoxEqpInfo_Enter(object sender, EventArgs e)
        {

        }

        private void data1_Load(object sender, EventArgs e)
        {

        }
    }
}

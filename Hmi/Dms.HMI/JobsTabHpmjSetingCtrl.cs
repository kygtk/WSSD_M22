using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Server;
using Dms.Device;

namespace Dms.HMI
{
    public partial class JobsTabHpmjSettingCtr : UserControl // 10.12.21 minhan
    {
        #region Fields
        private ServerManager m_Server;
        private HpmjInterface m_Hpmj;
        private string m_Msg;
        private string m_oldMsg;
        #endregion

        public JobsTabHpmjSettingCtr()
        {

            InitializeComponent();

            m_Server = ServerManager.Instance;
 //           m_Hpmj = eqpHpmjInterfaces._HpmjInterface;//LKL 150924
        }

        public void Initialize()
        {
            tmrUpdateState.Enabled = true;
            m_Msg = "";
            m_oldMsg = "";
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (GlobalVar.HpmjOnline)
                {
                    this.textBoxRunning_Mode.Text = string.Format("{0}", m_Hpmj.miwRunning_Mode.GetState());
                    this.textBoxFrequency_Set.Text = string.Format("{0}", m_Hpmj.miwFrequency_Set.GetState() / 10); // 11.02.09 minhan
                    this.textBoxPressure_Set.Text = string.Format("{0}", (double)m_Hpmj.miwPressure_Set.GetState() / 10);

                    this.textBoxShower_Flow_Set.Text = string.Format("{0}", (double)m_Hpmj.miwShower_Flow_Set.GetState() / 100);
                    this.textBoxShower_Flow_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwShower_Flow_Lower_Error.GetState() / 100);
                    this.textBoxShower_Flow_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwShower_Flow_Lower_Warning.GetState() / 100);
                    this.textBoxShower_Flow_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwShower_Flow_Upper_Error.GetState() / 100);
                    this.textBoxShower_Flow_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwShower_Flow_Upper_Warning.GetState() / 100);

                    this.textBoxFilter_In_Press_Set.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_In_Press_Set.GetState() / 10);
                    this.textBoxFilter_In_Press_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_In_Press_Lower_Error.GetState() / 10);
                    this.textBoxFilter_In_Press_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_In_Press_Lower_Warning.GetState() / 10);
                    this.textBoxFilter_In_Press_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_In_Press_Upper_Error.GetState() / 10);
                    this.textBoxFilter_In_Press_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_In_Press_Upper_Warning.GetState() / 10);

                    this.textBoxFilter_Out_Press_Set.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Out_Press_Set.GetState() / 10);
                    this.textBoxFilter_Out_Press_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Out_Press_Lower_Error.GetState() / 10);
                    this.textBoxFilter_Out_Press_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Out_Press_Lower_Warning.GetState() / 10);
                    this.textBoxFilter_Out_Press_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Out_Press_Upper_Error.GetState() / 10);
                    this.textBoxFilter_Out_Press_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Out_Press_Upper_Warning.GetState() / 10);

                    this.textBoxResistivity_Set.Text = string.Format("{0}", (double)m_Hpmj.miwResistivity_Set.GetState() / 100);
                    this.textBoxResistivity_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwResistivity_Lower_Error.GetState() / 100);
                    this.textBoxResistivity_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwResistivity_Lower_Warning.GetState() / 100);
                    this.textBoxResistivity_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwResistivity_Upper_Error.GetState() / 100);
                    this.textBoxResistivity_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwResistivity_Upper_Warning.GetState() / 100);

                    this.textBoxMain_CO2_Press_Set.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Press_Set.GetState() / 1000);
                    this.textBoxMain_CO2_Press_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Press_Lower_Error.GetState() / 1000);
                    this.textBoxMain_CO2_Press_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Press_Lower_Warning.GetState() / 1000);
                    this.textBoxMain_CO2_Press_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Press_Upper_Error.GetState() / 1000);
                    this.textBoxMain_CO2_Press_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Press_Upper_Warning.GetState() / 1000);

                    this.textBoxMain_DI_Press_Set.Text = string.Format("{0}", (double)m_Hpmj.miwDI_Press_Set.GetState() / 1000);
                    this.textBoxMain_DI_Press_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwDI_Press_Lower_Error.GetState() / 1000);
                    this.textBoxMain_DI_Press_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwDI_Press_Lower_Warning.GetState() / 1000);
                    this.textBoxMain_DI_Press_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwDI_Press_Upper_Error.GetState() / 1000);
                    this.textBoxMain_DI_Press_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwDI_Press_Upper_Warning.GetState() / 1000);

                    this.textBoxFilter_Difference_Press_Set.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Difference_Press_Set.GetState() / 10);
                    this.textBoxFilter_Difference_Press_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Difference_Press_Upper_Error.GetState() / 10);
                    this.textBoxFilter_Difference_Press_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwFilter_Difference_Press_Upper_Warning.GetState() / 10);

                    this.textBoxInverter_Load_Current_Set.Text = string.Format("{0}", (double)m_Hpmj.miwInverter_Load_Current_Set.GetState() / 10);
                    this.textBoxInverterCurrentError.Text = string.Format("{0}", (double)m_Hpmj.miwInveter_Load_Current_Upper_Error.GetState() / 10);
                    this.textBoxCurrentWarning.Text = string.Format("{0}", (double)m_Hpmj.miwInveter_Load_Current_Upper_Warning.GetState() / 10);

                    //this.textBoxMain_CO2_Flow_Set.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Flow_Set.GetState() / 10); // 11.05.26 minhan
                    //this.textBoxMain_CO2_Flow_Lower_Error.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Flow_Lower_Error.GetState() / 10);
                    //this.textBoxMain_CO2_Flow_Lower_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Flow_Lower_Warning.GetState() / 10);
                    //this.textBoxMain_CO2_Flow_Upper_Error.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Flow_Upper_Error.GetState() / 10);
                    //this.textBoxMain_CO2_Flow_Upper_Warninig.Text = string.Format("{0}", (double)m_Hpmj.miwMain_CO2_Flow_Upper_Warning.GetState() / 10);
                    m_Msg = "";
                    m_oldMsg = "";
                    this.labeHpmjstatus.Text = "ONLINE";
                }
                else
                {
                    this.labeHpmjstatus.Text = "OFFLINE";
                }
            }
            catch (Exception err)
            {
                m_Msg = err.ToString();

                if (m_Msg != m_oldMsg)
                {
                    m_oldMsg = m_Msg;
                    m_Server.WriteExceptionLog(m_Msg);
                    this.labeHpmjstatus.Text = "ERROR";
                }
            }

        }
    }
}

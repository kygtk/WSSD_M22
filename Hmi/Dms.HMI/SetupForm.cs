using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Control;
using Dms.Device; // 10.12.21 minhan
using Dms.Server; // 10.12.21 minhan
using Dms.Data; // 10.12.21 minhan
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class SetupForm : Form
    {
        #region Fields
        private ClientManager m_Client = null;
        private SetupTabCvDistance m_SetupTabCvDistance = new SetupTabCvDistance();
        private SetupTabCvMotors m_SetupTabCvMotors = new SetupTabCvMotors();
        private SetupTabGaugeInterlock m_SetupTabGaugeInterlock = new SetupTabGaugeInterlock();
        private SetupTabGeneral m_SetupTabGeneral = new SetupTabGeneral();
        private SetupTabSensorInterlock m_SetupTabSensorInterlock = new SetupTabSensorInterlock();
        private SetupTabInterface m_SetupTabInterface = new SetupTabInterface();
        //private SetupTabHsms m_SetupTabHsms = new SetupTabHsms(); // 11.02.01 minhan
        //private SetupTabFtp m_SetupTabFtp = new SetupTabFtp();
        private SetupTabHpmj m_SetupTabHpmj = new SetupTabHpmj();
        private _GenericCollection<Dms.Device.Gauge> m_Gauges; // 10.12.21 minhan
        private double m_Curvalue; // 11.02.01 minhan
        #endregion

        #region Constructor
        public SetupForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want to save information ?", "WSSD Sever",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                m_Client.SendCommand(Command.SetupSave);
            }
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            m_Gauges = DmsComponents.Instance.ComponentContainer.GetCollection<Dms.Device.Gauge>();
            m_SetupTabCvDistance.Initialize(m_Client.DataProvider.SetupCvDistance);
            m_SetupTabCvDistance.Initialize(m_Client.DataProvider.SetupSensorTimeout);
            m_SetupTabCvMotors.Initialize(m_Client.DataProvider.SetupCvInfo);
            m_SetupTabGaugeInterlock.Initialize(m_Client.DataProvider.SetupGaugeInterlock);
            m_SetupTabGeneral.Initialize(m_Client.DataProvider.SetupGenInfo);
            m_SetupTabGeneral.Initialize(m_Client.DataProvider.SetupIdleInfo);
            m_SetupTabGeneral.Initialize(m_Client.DataProvider.SetupTankLevel);
            m_SetupTabSensorInterlock.Initialize(m_Client.DataProvider.SetupSensorIntr);
            m_SetupTabInterface.Initialize(m_Client.DataProvider.SetupInterface);
            //m_SetupTabHsms.Initialize(m_Client.DataProvider.SetupHsmsInfo); // 11.02.01 minhan
            //m_SetupTabHsms.Initialize(m_Client.DataProvider.SetupHsmsEqpInfo);
            //m_SetupTabFtp.Initialize(m_Client.DataProvider.SetupFtpInfo);
            m_SetupTabHpmj.Initialize(m_Client.DataProvider.SetupHpmjInfo);

            this.tabCvDistance.Controls.Add(m_SetupTabCvDistance);
            this.tabCvMotor.Controls.Add(m_SetupTabCvMotors);
            this.tabGaugeInterlock.Controls.Add(m_SetupTabGaugeInterlock);
            this.tabGenInfo.Controls.Add(m_SetupTabGeneral);
            this.tabSensorInterlock.Controls.Add(m_SetupTabSensorInterlock);
            this.tabInterface.Controls.Add(m_SetupTabInterface);
            //this.tabHsms.Controls.Add(m_SetupTabHsms); // 10.12.21 minhan
            //this.tabFtp.Controls.Add(m_SetupTabFtp);
            this.tabHpmj.Controls.Add(m_SetupTabHpmj);
            //tmrUpdateState.Enabled = true;
            this.GaugeBox.Hide();
            this.GaugeName.ReadOnly = true;
            this.GaugeCurval.ReadOnly = true;
            m_Curvalue = 0;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool enable = true;
            //enable &= (m_Client.GenInfos.UserLevel > (int)UserLevels.Technician);
            enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Technician);
            //enable &= !m_Client.GenInfos.AutoMode;
            string gaugeName = "";
            if (this.btnSave.Enabled != enable)
            {
                this.btnSave.Enabled = enable;
            }

            if (this.tabControl1.SelectedTab.Name == this.tabGaugeInterlock.Name)  // 10.12.21 minhan
            {
                if (!GaugeBox.Visible)
                {
                    GaugeBox.Show();
                }

                if (m_Client.DataProvider.SetupGaugeInterlock.Viewer != null)
                {
                    foreach (ViewSetupInfo view in m_Client.DataProvider.SetupGaugeInterlock.Viewer)
                    {
                        gaugeName = view.SelectedCellName;
                    }
                }

                for (int i = 0; i < m_Gauges.Count; i++) // 11.02.01 minhan
                {
                    if (m_Gauges[i].Name == gaugeName)
                    {
                        this.GaugeName.Text = m_Gauges[i].Name;

                        if ((gaugeName == eqpGauges._FR_Unit_HPMJ_CO2_Pressure_Gauge_Name) ||
                            (gaugeName == eqpGauges._FR_Unit_HPMJ_Main_DI_Pressure_Gauge_Name))
                        {
                            m_Curvalue = (double)m_Gauges[i].CurAdc / 1000;
                            this.GaugeCurval.Text = m_Curvalue.ToString();
                        }
                        else if (gaugeName == eqpGauges._FR_Unit_HPMJ_DI_Resistance_Gauge_Name)
                        {
                            m_Curvalue = (double)m_Gauges[i].CurAdc / 100;
                            this.GaugeCurval.Text = m_Curvalue.ToString();
                        }
                        /*else if ((gaugeName == eqpGauges._FR_Unit_HPMJ_Diffrence_Press_Gauge_Name)) // 11.05.03 minhan
                        {
                            m_Curvalue = (double)m_Gauges[i].CurAdc / 10;
                            this.GaugeCurval.Text = m_Curvalue.ToString();
                        }*/
                        else
                        {
                            m_Curvalue = 0;
                            this.GaugeCurval.Text = m_Gauges[i].CurValue.ToString();
                        }
                    }
                }
            }
            else
            {
                if (GaugeBox.Visible)
                {
                    GaugeBox.Hide();
                }
            }
        }

        private void SetupForm_Deactivate(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = false;

            if (m_Client.DataProvider.IsSetupInfoChanged())
            {
                //if ((m_Client.GenInfos.UserLevel > (int)UserLevels.Technician))
                if ((m_Client.CurrentUserAccount.UserLevel > UserLevels.Technician))
                {
                    if (MessageBox.Show("There is some change of setup value. \r\nDo you want to save it anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_Client.SendCommand(Command.SetupSave);
                    }
                    else m_Client.DataProvider.RejectChangeSetupInfos();
                }
            }
        }

        private void SetupForm_Activated(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Util.IODefine;
using Dms.Common;
using Dms.Client;
using Dms.Data;
using Dms.Mitsubishi;
using Dms.Control;
using Dms.PcSystemInfo;
using Dms.Server;
using Dms.Device;

namespace Dms.HMI
{
    public partial class SystemForm : Form
    {
        #region Fields
        private List<SystemTabIOCtrl> m_SystemTabIoCtrls = new List<SystemTabIOCtrl>();
        private List<SystemTabSlaveCtrl> m_SystemTabSlaveCtrls = new List<SystemTabSlaveCtrl>();
        private List<IoDefines> m_IoList = new List<IoDefines>(); // 10.12.21 minhan
        private ClientManager m_Client;
        private ViewMelsecNetMonitor m_MelsecMonitorView = null;
        private _PcSystemInfo m_PcSystemInfo = null;
        private ViewPcSystemAPC620 m_PcSystemViewApc620 = null;
        private ViewPcSystemGeneral m_PcSystemViewGeneral = null;
        private const string _TabNameSystemStatus = "System Status";
        #endregion

        #region Constructor
        public SystemForm()
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
            string tabName = this.tabControlSystem.SelectedTab.Text;
            if (tabName != _TabNameSystemStatus)
            {
                foreach (IoDefines io in m_IoList)
                {
                    io.WriteText();
                }
            }
            else if (tabName == _TabNameSystemStatus && m_PcSystemInfo != null) //m_PcSystemInfo.SaveAll();
            {
                if (m_PcSystemViewGeneral != null) m_PcSystemViewGeneral.Save();
                else if (m_PcSystemViewApc620 != null) m_PcSystemViewApc620.Save();
            }
        }

        private void SystemForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;

            m_IoList = Controllers.Instance.IoDefineList;

            int index = 0;
            foreach (IoDefines io in m_IoList)
            {
                if (!io.IsDefined) continue;

                switch (io.BusType)
                {
                    case FieldBusType.MitsubishiMelsecEtherNet:
                        break;
                    case FieldBusType.BeckhoffEtherCAT:
                    case FieldBusType.BrModbusTcp:
                    case FieldBusType.CrevisModbusTcp:
                    case FieldBusType.MitsubishiCClink:
                    case FieldBusType.CrevisCClink:
                    case FieldBusType.MitsubishiMelsecNet:
                    case FieldBusType.TwinCATPlc:
                        {
                            SystemTabIOCtrl tabView = new SystemTabIOCtrl();

                            tabView.Initialize(io, Controllers.Instance.IoControllers[(int)io.BusType]);
                            m_SystemTabIoCtrls.Add(tabView);
                            TabPage tabPageBus = new TabPage(io.BusType.ToString());
                            tabPageBus.UseVisualStyleBackColor = true;
                            tabPageBus.Controls.Add(tabView);
                            this.tabControlSystem.TabPages.Insert(index, tabPageBus);

                            index++;
                        }
                        break;
                    case FieldBusType.MovensysEtherCAT:
                        {
                            SystemTabSlaveCtrl tabView = new SystemTabSlaveCtrl();

                            tabView.Initialize(io, Controllers.Instance.EcControllers[(int)io.BusType]);
                            m_SystemTabSlaveCtrls.Add(tabView);
                            TabPage tabPageBus = new TabPage(io.BusType.ToString());
                            tabPageBus.UseVisualStyleBackColor = true;
                            tabPageBus.Controls.Add(tabView);
                            this.tabControlSystem.TabPages.Insert(index, tabPageBus);

                            index++;
                        }
                        break;
                }
            }

            // jemoon : RootNode Melsec을 사용할 경우
            //if (m_Client.EventSubscriber.Server.RootNode != null) //10.12.21 minhan
            //{
            //    // tabPageMelsec
            //    // 
            //    System.Windows.Forms.TabPage m_TabPageMelsec = new TabPage();
            //    m_TabPageMelsec.Location = new System.Drawing.Point(4, 24);
            //    m_TabPageMelsec.Name = "tabPageMelsec";
            //    m_TabPageMelsec.Padding = new System.Windows.Forms.Padding(3);
            //    m_TabPageMelsec.Size = new System.Drawing.Size(901, 537);
            //    m_TabPageMelsec.TabIndex = 0;
            //    m_TabPageMelsec.Text = "Melsec Net";
            //    m_TabPageMelsec.UseVisualStyleBackColor = true;
            //    // ViewMelsecNetMonitor
            //    m_MelsecMonitorView = new ViewMelsecNetMonitor();
            //    m_MelsecMonitorView.Dock = DockStyle.Fill;
            //    m_MelsecMonitorView.Initialize(m_Client.EventSubscriber.Server.RootNode);
            //    m_TabPageMelsec.Controls.Add(m_MelsecMonitorView);
            //    this.tabControlSystem.TabPages.Add(m_TabPageMelsec);
            //}

            m_PcSystemInfo = PcSystemFactory.Instance.GetSystem(AppConfig.Instance.PcType);

            if (m_PcSystemInfo != null)
            {
                if (m_PcSystemInfo.GetType() == typeof(BrPc))
                {
                    m_PcSystemViewApc620 = new ViewPcSystemAPC620();
                    m_PcSystemViewApc620.Dock = DockStyle.Fill;
                    m_PcSystemViewApc620.Initialize(m_PcSystemInfo);

                    TabPage tabPageSystemStatus = new TabPage("System Status");
                    tabPageSystemStatus.UseVisualStyleBackColor = true;
                    tabPageSystemStatus.Controls.Add(m_PcSystemViewApc620);
                    this.tabControlSystem.Controls.Add(tabPageSystemStatus);
                }
                else if (m_PcSystemInfo.GetType() == typeof(GeneralPc))
                {
                    m_PcSystemViewGeneral = new ViewPcSystemGeneral();
                    m_PcSystemViewGeneral.Dock = DockStyle.Fill;
                    m_PcSystemViewGeneral.Initialize(m_PcSystemInfo);

                    TabPage tabPageSystemStatus = new TabPage("System Status");
                    tabPageSystemStatus.UseVisualStyleBackColor = true;
                    tabPageSystemStatus.Controls.Add(m_PcSystemViewGeneral);
                    this.tabControlSystem.Controls.Add(tabPageSystemStatus);
                }
            }

            this.tabControlSystem.SelectedIndex = 0;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool enable = true;
            //enable &= !m_Client.GenInfos.AutoMode;
            //enable &= (m_Client.GenInfos.UserLevel > (int)UserLevels.Operator);
            //enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Operator);

            //if (this.tabControlSystem.Enabled != enable)
            //{
            this.tabControlSystem.Enabled = enable;
            //}

            //if (this.btnSave.Enabled != enable)
            //{
            //    this.btnSave.Enabled = enable;
            //}
            //SetPermission();
        }

        private void SystemForm_Activated(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
            foreach (SystemTabIOCtrl view in m_SystemTabIoCtrls)
            {
                view.ViewIOEdit.TimerStateUpdateEnabled = true;
            }
            foreach (SystemTabSlaveCtrl view in m_SystemTabSlaveCtrls)
            {
                view.ViewSlaveEdit.TimerStateUpdateEnabled = true;
            }

            if (m_MelsecMonitorView != null) m_MelsecMonitorView.SetMonitorTimer(true);
            if (m_PcSystemViewApc620 != null) m_PcSystemViewApc620.SetMonitorTimer(true);
            if (m_PcSystemViewGeneral != null) m_PcSystemViewGeneral.SetMonitorTimer(true);
        }

        private void SystemForm_Deactivate(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = false;
            foreach (SystemTabIOCtrl view in m_SystemTabIoCtrls)
            {
                view.ViewIOEdit.TimerStateUpdateEnabled = false;
            }
            foreach (SystemTabSlaveCtrl view in m_SystemTabSlaveCtrls)
            {
                view.ViewSlaveEdit.TimerStateUpdateEnabled = false;
            }

            if (m_MelsecMonitorView != null) m_MelsecMonitorView.SetMonitorTimer(false);
            if (m_PcSystemViewApc620 != null) m_PcSystemViewApc620.SetMonitorTimer(false);
            if (m_PcSystemViewGeneral != null) m_PcSystemViewGeneral.SetMonitorTimer(false);
        }

        private void SetPermission()
        {
            bool permission = true;
            permission &= !m_Client.GenInfos.AutoMode;
            permission &= (m_Client.CurrentUserAccount.UserLevel >= UserLevels.Technician);

            this.tabControlSystem.Enabled = permission;
            this.btnSave.Enabled = permission;
        }
        #endregion
    }
}
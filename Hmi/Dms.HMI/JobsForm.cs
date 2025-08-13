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
using Dms.Data;
using Dms.Server;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsForm : Form
    {
        #region Fields
        private JobsTabMainCtrl jobsTabMain;
        private JobsTabMainCtrl_R jobsTabMain_R;
        private JobsTabPipeCtrl jobsTabPipe;
        private JobsTabPipeCtrl_R jobsTabPipe_R;
        private JobsTabProcessDataCtrl jobsTabProcessData;
        private JobsTabHpmjCtrl jobsTabHpmj;
        //        private JobsTabHpmjSettingCtr jobTabHpmjSettingCtr; // 11.02.07 minhan
        private JobsTabFfuCtr jobsTabFfuCtr; // 11.03.07 minhan
        //private JobsTabInterlockCtrl jobstabInterlock;//2009.09.23 kimgun // 10.12.17 minhan
        private JobsToolbar m_JobsToolbar;
        private JobsTabGauge jobsTabGauge; // 10.12.21 minhan
        private JobsTabGlassData jobsTabGlassData; // 11.02.09 minhan
        private JobsTabSendGlassData jobsTabSendGlassData; // 11.06.07 minhan
        private MaintToolbar m_MaintToolbar;
        //private InterfaceToolbar m_InterfaceToolbar; // 10.12.21 minhan
        private _GenericCollection<UshioEuvUnit> m_UshioEuvUnits; // 10.12.21 minhan
                                                                  //private _GenericCollection<SeAp> m_SeApUnits;
                                                                  //private _GenericCollection<PSMAp> m_PsmApUnits;

        //private JobsTabApCtrl jobsTabAp;
        private JobsTabEuvCtrl jobsTabEuv; // 10.12.25 minhan
        private JobsTabInterfaceCtrl jobsTabInterface;
        private ClientManager m_Client = null;
        //private AppConfig m_AppConfig;
        private int m_OldIsNormal = -1;
        #endregion

        #region Constructor
        public JobsForm()
        {
            InitializeComponent();
            m_JobsToolbar = new JobsToolbar();
            m_MaintToolbar = new MaintToolbar();
            //m_InterfaceToolbar = new InterfaceToolbar();
            m_JobsToolbar.Location = new Point(this.Width - m_JobsToolbar.Width, 0);
            m_MaintToolbar.Location = new Point(this.Width - m_MaintToolbar.Width, 0);
            //m_InterfaceToolbar.Location = new Point(this.Width - m_InterfaceToolbar.Width, 0);
            this.Controls.Add(m_JobsToolbar);
            this.Controls.Add(m_MaintToolbar);
            //this.Controls.Add(m_InterfaceToolbar);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentTabText = ((TabControl)sender).SelectedTab.Text;

            //if (m_AppConfig.AutoStart)
            {
                if (m_Client.GenInfos.AutoMode && currentTabText != tabInterface.Text)
                {
                    SetToolbarByTab(1);
                }
                else if (currentTabText == tabInterface.Text)
                {
                    SetToolbarByTab(2);
                }
                else
                {
                    if (currentTabText == tabPiping.Text)
                    {
                        SetToolbarByTab(0);
                    }
                    else
                    {
                        SetToolbarByTab(1);
                    }
                }
            }
        }

        private void SetToolbarByTab(int isNormal)
        {
            if ((m_OldIsNormal != 1) && (isNormal == 1))
            {
                m_OldIsNormal = 1;

                m_MaintToolbar.Visible = false;
                m_JobsToolbar.Visible = true;
                //m_InterfaceToolbar.Visible = false;

            }
            else if ((m_OldIsNormal != 2) && (isNormal == 2))
            {
                m_OldIsNormal = 2;

                m_JobsToolbar.Visible = false;
                m_MaintToolbar.Visible = false;
                //m_InterfaceToolbar.Visible = true;
            }
            else if ((m_OldIsNormal != 0) && (isNormal == 0))
            {
                m_OldIsNormal = 0;

                m_JobsToolbar.Visible = false;
                m_MaintToolbar.Visible = true;
                //m_InterfaceToolbar.Visible = false;
            }
        }

        private void tmrUpdateToolbar_Tick(object sender, EventArgs e)
        {
            //SetToolBarState
            if (!m_Client.GenInfos.AutoMode && (this.tabControl.SelectedTab.Text == tabPiping.Text))
            {
                SetToolbarByTab(0);
            }
            if (GlobalVar.RcvHostMsg)
            {//2009.10.14 kimgun host msg전송시 interface view로 이동
                GlobalVar.RcvHostMsg = false;
                if (tabControl.SelectedTab != tabInterface)
                {
                    tabControl.SelectedTab = tabInterface;
                    tabControl_SelectedIndexChanged(this.tabControl, null);
                }
            }

        }

        private void JobsForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            DeviceTags tagContainer = m_Client.DataProvider.TagContainer;
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;
            m_UshioEuvUnits = components.GetCollection<UshioEuvUnit>();
            //m_SeApUnits = components.GetCollection<SeAp>();
            //m_PsmApUnits = components.GetCollection<PSMAp>(); // 10.12.21 minhan

            //jobTabMain1 = new JobsTabCvCtrl();
            //jobTabMain1.Initialize(m_Client.DataProvider.SetupGenInfo);
            //jobTabMain1.Initialize(m_Client.DataProvider.RecipeProvider);
            //this.tabMain1.Controls.Add(jobTabMain1);

            if (AppConfig.Instance.Simul.LType)
            {
                jobsTabMain = new JobsTabMainCtrl();
                jobsTabMain.Initialize(m_Client.DataProvider.SetupGenInfo);
                jobsTabMain.Initialize(m_Client.DataProvider.RecipeProvider);
                this.tabMain.Controls.Add(jobsTabMain);
            }
            else
            {
                jobsTabMain_R = new JobsTabMainCtrl_R();
                jobsTabMain_R.Initialize(m_Client.DataProvider.SetupGenInfo);
                jobsTabMain_R.Initialize(m_Client.DataProvider.RecipeProvider);
                this.tabMain.Controls.Add(jobsTabMain_R);
            }

            if (AppConfig.Instance.Simul.LType)
            {
                jobsTabPipe = new JobsTabPipeCtrl();
                this.tabPiping.Controls.Add(jobsTabPipe);
            }
            else
            {
                jobsTabPipe_R = new JobsTabPipeCtrl_R();
                this.tabPiping.Controls.Add(jobsTabPipe_R);
            }

            jobsTabHpmj = new JobsTabHpmjCtrl();
            jobsTabHpmj.Initialize();
            this.tabHpmj.Controls.Add(jobsTabHpmj);

            //jobTabHpmjSettingCtr = new JobsTabHpmjSettingCtr(); // 11.02.07 minhan;//lkl 150929
            //jobTabHpmjSettingCtr.Initialize();
            //this.tabPageHPMJSettingPara.Controls.Add(jobTabHpmjSettingCtr);

            jobsTabFfuCtr = new JobsTabFfuCtr(); // 11.03.07 minhan
            jobsTabFfuCtr.Initialize();
            this.tabFfuCon.Controls.Add(jobsTabFfuCtr);

            jobsTabProcessData = new JobsTabProcessDataCtrl();
            jobsTabProcessData.Initialize();
            this.tabProcessData.Controls.Add(jobsTabProcessData);

            if (m_UshioEuvUnits != null && m_UshioEuvUnits.Count != 0)
            {
                jobsTabEuv = new JobsTabEuvCtrl();
                jobsTabEuv.Initialize();
                this.tabEUV.Text = "EUV";
                this.tabEUV.Controls.Add(jobsTabEuv);

                if (AppConfig.Instance.Simul.LType)
                {
                    this.jobsTabMain.euvHouse1.EuvClick += new EventHandler(euv1_EuvClick);
                    this.jobsTabPipe.euvHouse1.EuvClick += new EventHandler(euv1_EuvClick);
                }
                else
                {
                    this.jobsTabMain_R.euvHouse.EuvClick += new EventHandler(euv1_EuvClick);
                    this.jobsTabPipe_R.euvHouse1.EuvClick += new EventHandler(euv1_EuvClick);
                }
            }
            //if ((m_SeApUnits != null) && (m_SeApUnits.Count != 0)) // 10.12.21 minhan
            //{
            //    jobsTabAp = new JobsTabApCtrl();
            //    jobsTabAp.Initialize();
            //    this.tabEUV.Text = "AP";
            //    this.tabEUV.Controls.Add(jobsTabAp);

            //    if (m_Client.EventSubscriber.Server.Simul.LType)
            //    {
            //        this.jobsTabMain.ap1.ApClick += new EventHandler(ap1_ApClick);
            //        this.jobsTabPipe.ap1.ApClick += new EventHandler(ap1_ApClick);
            //    }
            //    else
            //    {
            //        this.jobsTabMain_R.ap1.ApClick += new EventHandler(ap1_ApClick);
            //        this.jobsTabPipe_R.ap1.ApClick += new EventHandler(ap1_ApClick);
            //    }
            //}

            jobsTabInterface = new JobsTabInterfaceCtrl();
            jobsTabInterface.Initialize();
            this.tabInterface.Controls.Add(jobsTabInterface);

            this.viewCurrentAlarms1.InitGridView(m_Client.DataProvider.CurrentAlarms);
            //jobstabInterlock = new JobsTabInterlockCtrl(); // 10.12.21 minhan
            //jobstabInterlock.Initialize();
            //this.tabInterlock.Controls.Add(jobstabInterlock);

            jobsTabGauge = new JobsTabGauge(); // 10.12.15 minhan
            jobsTabGauge.Initialize();
            this.tabGauge.Controls.Add(jobsTabGauge);

            jobsTabGlassData = new JobsTabGlassData(); // 11.02.09 minhan
            jobsTabGlassData.Initialize();
            this.tabRecvGlass.Controls.Add(jobsTabGlassData);

            jobsTabSendGlassData = new JobsTabSendGlassData(); // 11.06.07 minhan
            jobsTabSendGlassData.Initialize();
            this.tabSendGlass.Controls.Add(jobsTabSendGlassData);

            checkSeqLog.Tag = this.SeqlogList;
            checkSeqLog.Checked = SeqlogList.CheckEnable;
            checkCommLog.Tag = this.CommLogList;
            checkCommLog.Checked = CommLogList.CheckEnable;

            SetToolbarByTab(1);
            //tmrUpdateToolbar.Enabled = true;
        }

        void ap1_ApClick(object sender, EventArgs e)
        {
            //if (!m_Client.GenInfos.AutoMode)
            {
                this.tabControl.SelectedTab = tabEUV;
            }
        }
        void euv1_EuvClick(object sender, EventArgs e)
        {
            //if (!m_Client.GenInfos.AutoMode)
            {
                this.tabControl.SelectedTab = tabEUV;
            }
        }

        private void checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            bool checkState = checkbox.Checked;
            ((LogList)(checkbox.Tag)).CheckEnable = checkState;
        }

        private void JobsForm_Activated(object sender, EventArgs e)
        {
            tmrUpdateToolbar.Enabled = true;
        }

        private void JobsForm_Deactivate(object sender, EventArgs e)
        {
            tmrUpdateToolbar.Enabled = false;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Common;
using Dms.Data;
using System.Reflection;
using Dms.Control;
using Dms.Util;
using Dms.Server;
using Dms.Device;
using Dms.Ctl;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class MainForm : Form
    {
        #region Fields
        //Forms
        private JobsForm jobsForm = null;
        private SystemForm systemForm = null;
        private ServoForm servoForm = null;
        private RecipeForm recipeForm = null;
        private SetupForm setupForm = null;
        private AlarmForm alarmForm = null;
        private HistoryForm historyForm = null;

        //For Client
        private ClientManager m_ClientManager = null;
        private ServerManager m_ServerManager = null;
        private EqpStateManager m_EqpStateManger = null;

        //General Data
        private AppConfig m_AppConfig = AppConfig.Instance;
        private GenInfoHandler m_GenInfo = GenInfoHandler.Instance;
        private UserAccountProvider m_UserAccountProvider;
        private TagUserAccount m_CurUserAccount = new TagUserAccount();
        private TagUserAccount m_OldUserAccount = new TagUserAccount();
        private Point m_LoginDialogPosition = new Point(400, 400);
        private SplashScreen splashScreen;
        private Point m_FixedPoint = new Point(0, 0);
        private RecipeProvider m_Provider;
        private DlgRecipeConfirm m_Dlg;
        private DlgGlassInfo m_GlsInfoDlg;
        //private ServoUnitMp2300s m_Mp2300s = null;
        private ServoUnits m_ServoUnits;
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods for Client Components
        public bool InitializeClientManager() //
        {
            bool ok = true;

            m_ServerManager = ServerManager.Instance;
            ok &= m_ServerManager.Initialize() == DmsErrors.Success;

            m_ClientManager = ClientManager.Instance;
            ok &= m_ClientManager.Initialize(m_ServerManager) == DmsErrors.Success;

            return ok;
        }

        private bool UninitializeClientManager()
        {
            if (m_ClientManager != null)
            {
                m_ClientManager.Uninitialize();
            }

            return true;
        }
        #endregion 

        #region Methods
        private void InitializeDmsUserControl()
        {
            UserControlBuilder.InitializeAll(this, m_ClientManager.DataProvider.TagContainer);
        }

        private void InitializeEqpStateManager()
        {
            // Eqp Manager Dialog는 서버가 로컬모드이고 디바이스 시물레이션 모드일때만 활성화됨
            if (m_ClientManager.ServerMode == ServerMode.Local &&
                m_AppConfig.Simul.Device)
            {
                if (m_EqpStateManger == null)
                {
                    m_EqpStateManger = new EqpStateManager();
                    m_EqpStateManger.Show();
                }
            }
        }

        private void UninitializeEqpStateManager()
        {
            if (m_EqpStateManger != null) m_EqpStateManger.Close();
        }

        private void InitializeMainFormTitleText()
        {
            if (m_AppConfig.AutoStart)
            {
                this.Text += " - " + m_ServerManager.EqpStateManager.EqpUnit.Name;
                this.Text += " Ver " + m_ServerManager.EqpStateManager.EqpUnit.version;//2009.10.06 kimgun
                this.Text += " (" + m_ClientManager.ServerMode.ToString() + ")";
            }
            else
            {
                this.Text += "Server is not started";
            }
        }

        private void InitializeUserAccountProvider()
        {
            m_UserAccountProvider = UserAccountProvider.Instance;
        }

        private void SetCurrentUserAccount()
        {
            m_UserAccountProvider.CurrentUserAccount = m_CurUserAccount;
            m_ClientManager.CurrentUserAccount = m_CurUserAccount;
        }

        private void SetEditPermission()
        {
            m_ClientManager.DataProvider.SetEditPermission(m_CurUserAccount.UserLevel);
        }

        private void LoadDefaultButtonColor()
        {
            Color color;
            color = Color.Transparent;

            this.buttonJobs.BackColor = color;
            this.buttonSystem.BackColor = color;
            this.buttonServo.BackColor = color;
            this.buttonRecipe.BackColor = color;
            this.buttonSetup.BackColor = color;
            this.buttonAlarm.BackColor = color;
            this.buttonDataLog.BackColor = color; // 11.02.09 minhan
        }

        private void NaviButton_Click(object sender, EventArgs e)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //2009.07.31 kimgun Servo가 동작 중에는 jobview 이동 불가
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;
            m_ServoUnits = components.GetCollection<ServoUnit>() as ServoUnits;
            foreach (ServoUnit servo in m_ServoUnits)
                if (servo.ManualMoving) return;
            //////////////////////////////////////////////////////////////////////////////////////////////
            LoadDefaultButtonColor();
            ((Button)sender).BackColor = Color.LightSteelBlue;
            ((Form)(((Button)sender).Tag)).BringToFront();
            ((Form)(((Button)sender).Tag)).Focus();

            lblViewName.Text = ((Button)sender).Text.ToUpper();
        }

        public void buttonExit_Click(object sender, EventArgs e) //
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really close the application", "WSSD", MessageBoxButtons.YesNo))
            {
                splashScreen = new SplashScreen(Properties.Resources.splash_end_New, 2000);
                splashScreen.Show();

                if (m_IoSimulator != null)
                {
                    m_IoSimulator.Uninitialize();
                }

                // Uninitialize 하기전에 모든 UI Timer를 중지 할 수 있으면 좋을텐데...
                // jemoon : 구현완료 함
                KillUiUpdateTimer();

                UninitializeEqpStateManager();

                UninitializeClientManager();

                splashScreen.Close();
                this.Close();
            }
        }

        //Form에 등록된 모든 form timer kill
        private void KillUiUpdateTimer()
        {
            DmsUserControl.UninitializeUpdateTimerAll(this);
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {

        }

        private void NaviButtonStyle(System.Windows.Forms.Button btn, bool bEnable)
        {
            btn.Enabled = bEnable;
        }

        private void UpdateDateTime()
        {
            lblDate.Text = DateTime.Now.ToShortDateString().Replace('-', '/');
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateDateTime();
                SetNavigationButtonPermission();

                if (m_Provider.RecipeDialogInfo.Count > 0)
                {
                    string managementPara = m_Provider.RecipeDialogInfo[0];
                    m_Provider.RecipeDialogInfo.RemoveAt(0);

                    string[] split = managementPara.Split(new char[] { ',' });

                    m_Dlg = new DlgRecipeConfirm(split);
                    m_Dlg.Show();
                }

                if (GlobalVar.UnloadGlassDataRequest == true)
                {
                    GlobalVar.UnloadGlassDataRequest = false;

                    m_GlsInfoDlg = new DlgGlassInfo(0);
                    m_GlsInfoDlg.CimEnable = true;
                    m_GlsInfoDlg.ModifyEnable = true;
                    m_GlsInfoDlg.Initialize();
                    //m_GlsInfoDlg.TopMost = true;
                    //m_GlsInfoDlg.ShowDialog(this);
                    m_GlsInfoDlg.Show(this);
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }

        private void SetNavigationButtonPermission()
        {
            bool permission = true;
            permission &= !m_ClientManager.GenInfos.AutoMode;
            permission &= (m_CurUserAccount.UserLevel >= UserLevels.Technician);

            this.buttonSystem.Enabled = permission;

            permission &= (this.servoForm != null);
            this.buttonServo.Enabled = permission;

            //Salience View
            Dms.Device.IEqpManager eqpManager = m_ServerManager.EqpStateManager;
            bool isWarning = eqpManager.IsAlarmState;
            bool isAlarm = eqpManager.IsAlarmState;
            SalienceState alarmSalienceState = SalienceState.None;
            if (isAlarm)
            {
                alarmSalienceState = SalienceState.Alarm;
            }
            else if (isWarning)
            {
                alarmSalienceState = SalienceState.Caution;
            }
            this.viewSalienceAlarm.SetState(alarmSalienceState);

        }

        private void lblHostConnection_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ShowLoginDialog();
        }

        private bool ShowLoginDialog()
        {
            LoginDialog LoginDialog;

            LoginDialog = new LoginDialog(m_CurUserAccount);
            LoginDialog.AccountProvider = m_UserAccountProvider;
            //LoginDialog.Location = m_LoginDialogPosition;
            LoginDialog.Initialize(txtUserId);
            LoginDialog.ShowDialog();
            bool result = true;
            if (LoginDialog.DialogResult == DialogResult.OK)
            {
                m_LoginDialogPosition = LoginDialog.Location;
                UpdateLoginInfo();
            }
            else
            {
                result = false;
            }

            //jemoon : 110607
            //ShowDialog甫 荤侩窍咯 汽阑 钎矫茄 版快, Dispose甫 龋免窍咯 汽狼 葛电 牧飘费阑 啊厚瘤 荐笼贸府.	 
            LoginDialog.Dispose();
            return result;
        }

        private void UpdateLoginInfo()
        {
            SetCurrentUserAccount();

            txtUserId.Text = m_CurUserAccount.UserID + ":" + m_CurUserAccount.UserLevel.ToString();
            if (m_OldUserAccount != m_CurUserAccount)
            {
                m_OldUserAccount.Clone(m_CurUserAccount);
                SetEditPermission();
            }

            string log = string.Format("Current Login User ID : {0}, Level : {1}", m_CurUserAccount.UserID, m_CurUserAccount.UserLevel.ToString());
            HmiLog.WriteLog(log);
        }

        private bool SetDefaultLoginUserInfo()
        {
            bool isDefaultLogin = true;

            isDefaultLogin &= m_ClientManager.ServerMode == ServerMode.Local;
            isDefaultLogin &= m_AppConfig.Simul.Device;

            if (isDefaultLogin)
            {
                m_CurUserAccount = m_UserAccountProvider.LoginDefaultUser();
                UpdateLoginInfo();
            }

            return isDefaultLogin;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                bool ok = true;

                splashScreen = new SplashScreen(Properties.Resources.splash_start_New);
                splashScreen.Show();

                ok &= InitializeClientManager();

                if (!ok)
                {
                    splashScreen.Exit();
                    MessageBox.Show("Server has internal error!");
                    //Application.Exit();
                    this.Dispose();
                    this.Close();
                }
                else
                {
                    m_Provider = RecipeProvider.Instance;   //2009.06.19 Yougnsik... for test...

                    InitializeUserAccountProvider();

                    bool isDefaultLogin = SetDefaultLoginUserInfo();
                    if (isDefaultLogin == false && ShowLoginDialog() == false)
                    {
                        UninitializeClientManager();
                        this.Dispose();
                    }
                    else
                    {
                        InitializeMainFormComponents();
                        InitializeMainFormTitleText();
                        InitializeDmsUserControl();
                        InitializeEqpStateManager();
                        SetEditPermission();

                        this.LocationChanged += new EventHandler(MainForm_LocationChanged);
                        timerDateTime.Enabled = true;
                    }

                    ServerManager.Instance.ThreadHandler.Start(); // 11.02.19 minhan

                    splashScreen.Exit();
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();

                ExceptionLog.WriteLog(msg);

                UninitializeClientManager();

                MessageBox.Show(msg);
                this.Dispose();
            }
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (m_AppConfig.Simul.Device) return;

            this.Location = m_FixedPoint;
        }

        private void InitializeMainFormComponents()
        {
            this.systemForm = new SystemForm();
            this.systemForm.MdiParent = this;
            this.systemForm.Show();
            this.buttonSystem.Tag = this.systemForm;

            this.servoForm = new ServoForm();
            this.servoForm.MdiParent = this;
            this.servoForm.Show();
            this.buttonServo.Tag = this.servoForm;
            this.buttonServo.Enabled = true;

            this.recipeForm = new RecipeForm();
            this.recipeForm.MdiParent = this;
            this.recipeForm.Show();
            this.buttonRecipe.Tag = this.recipeForm;

            this.historyForm = new HistoryForm();
            this.historyForm.MdiParent = this;
            this.historyForm.Show();
            this.buttonDataLog.Tag = this.historyForm;

            this.setupForm = new SetupForm();
            this.setupForm.MdiParent = this;
            this.setupForm.Show();
            this.buttonSetup.Tag = this.setupForm;

            this.alarmForm = new AlarmForm();
            this.alarmForm.MdiParent = this;
            this.alarmForm.Show();
            this.buttonAlarm.Tag = this.alarmForm;

            this.jobsForm = new JobsForm();
            this.jobsForm.MdiParent = this;
            this.jobsForm.Show();
            this.buttonJobs.Tag = this.jobsForm;

            this.buttonHelp.Enabled = false;

            //Simulator button 활성화
            this.buttonSimulator.Visible = m_AppConfig.Simul.Device;

            //this.btnExchange.Visible = m_AppConfig.Simul.Device; // 11.06.10 minhan
            this.btnNosubstrate.Visible = m_AppConfig.Simul.Device; // 11.06.10 minhan
            this.btnOnline.Visible = m_AppConfig.Simul.Device; // 11.06.10 minhan
        }

        private void buttonBuzzer_Click(object sender, EventArgs e)
        {
            m_ServerManager.EqpStateManager.BuzzerOffSwitchPushed = true;
            //GlobalVar.BuzzerOff = true;
        }
        #endregion

        #region Simulator
        private FormIoSimulator m_IoSimulator = null;
        private void buttonSimulator_Click(object sender, EventArgs e)
        {
            bool created = ((m_IoSimulator != null) && !m_IoSimulator.IsDisposed);
            if (created)
            {   //이미 생성되어 있으면 front로 가져오기만 한다.
                m_IoSimulator.BringToFront();
            }
            else
            {
                //Simulator form 생성
                m_IoSimulator = new FormIoSimulator();
                ServerManager serverManager = ServerManager.Instance;
                m_IoSimulator.SetIoController(serverManager.IoController);
                m_IoSimulator.SetIoDefine(serverManager.IoDefines);

                //m_IoSimulator.SetIoController(MelsecNet.Instance);
                //m_IoSimulator.SetIoDefine(IoControllers.Instance.IoDefineList[(int)Dms.Util.IODefine.FieldBusType.MitsubishiMelsecNet]);

                //아래부분은 장비마다 달라짐, 필요한 device를 등록

                //m_IoSimulator.AddDevice(eqpIfSignalFromCims._ECS_Signal);
                //m_IoSimulator.AddDevice(eqpIfSignalToCims._EQP_Signal);

                //필요한 내용을 다 등록했으면 초기화
                m_IoSimulator.Initialize();
                m_IoSimulator.Show();
            }
        }
        //private void buttonExchange_Click(object sender, EventArgs e) // 11.06.10 minhan 
        //{
        //    if (!m_AppConfig.Simul.Device) return;

        //    if (!GlobalVar.SimulExReq)
        //    {
        //        GlobalVar.SimulExReq = true;
        //        eqpBOELoaderInterfaces._LoaderInterface.mibExchange_Req.SetState(true);
        //        this.btnExchange.BackColor = System.Drawing.Color.Red;
        //    }
        //    else
        //    {
        //        GlobalVar.SimulExReq = false;
        //        eqpBOELoaderInterfaces._LoaderInterface.mibExchange_Req.SetState(false);
        //        this.btnExchange.BackColor = System.Drawing.Color.Blue;
        //    }
        //}
        private void buttonNosubstrate_Click(object sender, EventArgs e) // 11.06.10 minhan 
        {
            if (!m_AppConfig.Simul.Device) return;

            if (!eqpBOELoaderInterfaces._LoaderInterface.mibNoSubstarate_to_Cleaner.GetState())
            {
                eqpBOELoaderInterfaces._LoaderInterface.mibNoSubstarate_to_Cleaner.SetState(true);
                this.btnNosubstrate.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                eqpBOELoaderInterfaces._LoaderInterface.mibNoSubstarate_to_Cleaner.SetState(false);
                this.btnNosubstrate.BackColor = System.Drawing.Color.Blue;
            }
        }
        private void buttonOnline_Click(object sender, EventArgs e) // 11.06.10 minhan 
        {
            if (!m_AppConfig.Simul.Device) return;

            if (!eqpBOELoaderInterfaces._LoaderInterface.mibLoader_Power_ON.GetState())
            {
                eqpBOELoaderInterfaces._LoaderInterface.mibLoader_Power_ON.SetState(true);
                this.btnOnline.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                eqpBOELoaderInterfaces._LoaderInterface.mibLoader_Power_ON.SetState(false);
                this.btnOnline.BackColor = System.Drawing.Color.Blue;
            }
        }
        #endregion


    }
}
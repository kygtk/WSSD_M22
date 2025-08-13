namespace Dms.HMI
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NavigationPanel = new System.Windows.Forms.Panel();
            this.btnOnline = new System.Windows.Forms.Button();
            this.btnNosubstrate = new System.Windows.Forms.Button();
            this.buttonDataLog = new System.Windows.Forms.Button();
            this.buttonSimulator = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonAlarm = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.buttonRecipe = new System.Windows.Forms.Button();
            this.buttonServo = new System.Windows.Forms.Button();
            this.buttonSystem = new System.Windows.Forms.Button();
            this.buttonJobs = new System.Windows.Forms.Button();
            this.buttonBuzzer = new System.Windows.Forms.Button();
            this.viewSalienceAlarm = new Dms.Control.ViewSalience();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);
            this.lblViewName = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.genInfo3 = new Dms.Control.GenInfo();
            this.genInfo2 = new Dms.Control.GenInfo();
            this.genInfo1 = new Dms.Control.GenInfo();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mainStatusType1 = new Dms.Control.MainStatusType1();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.MessagelogList = new Dms.Control.LogList();
            this.NavigationPanel.SuspendLayout();
            this.TitlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.NavigationPanel.Controls.Add(this.btnOnline);
            this.NavigationPanel.Controls.Add(this.btnNosubstrate);
            this.NavigationPanel.Controls.Add(this.buttonDataLog);
            this.NavigationPanel.Controls.Add(this.buttonSimulator);
            this.NavigationPanel.Controls.Add(this.buttonHelp);
            this.NavigationPanel.Controls.Add(this.buttonAlarm);
            this.NavigationPanel.Controls.Add(this.buttonExit);
            this.NavigationPanel.Controls.Add(this.buttonSetup);
            this.NavigationPanel.Controls.Add(this.buttonRecipe);
            this.NavigationPanel.Controls.Add(this.buttonServo);
            this.NavigationPanel.Controls.Add(this.buttonSystem);
            this.NavigationPanel.Controls.Add(this.buttonJobs);
            this.NavigationPanel.Controls.Add(this.buttonBuzzer);
            this.NavigationPanel.Controls.Add(this.viewSalienceAlarm);
            this.NavigationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NavigationPanel.Location = new System.Drawing.Point(0, 666);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(1018, 70);
            this.NavigationPanel.TabIndex = 1;
            // 
            // btnOnline
            // 
            this.btnOnline.Location = new System.Drawing.Point(724, 3);
            this.btnOnline.Name = "btnOnline";
            this.btnOnline.Size = new System.Drawing.Size(41, 64);
            this.btnOnline.TabIndex = 7;
            this.btnOnline.Text = "Online";
            this.btnOnline.UseVisualStyleBackColor = true;
            this.btnOnline.Visible = false;
            this.btnOnline.Click += new System.EventHandler(this.buttonOnline_Click);
            // 
            // btnNosubstrate
            // 
            this.btnNosubstrate.Location = new System.Drawing.Point(681, 3);
            this.btnNosubstrate.Name = "btnNosubstrate";
            this.btnNosubstrate.Size = new System.Drawing.Size(41, 64);
            this.btnNosubstrate.TabIndex = 6;
            this.btnNosubstrate.Text = "Nosubstrate";
            this.btnNosubstrate.UseVisualStyleBackColor = true;
            this.btnNosubstrate.Visible = false;
            this.btnNosubstrate.Click += new System.EventHandler(this.buttonNosubstrate_Click);
            // 
            // buttonDataLog
            // 
            this.buttonDataLog.BackColor = System.Drawing.Color.Transparent;
            this.buttonDataLog.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDataLog.Image = global::Dms.HMI.Properties.Resources.DataLog;
            this.buttonDataLog.Location = new System.Drawing.Point(387, 3);
            this.buttonDataLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDataLog.Name = "buttonDataLog";
            this.buttonDataLog.Size = new System.Drawing.Size(69, 64);
            this.buttonDataLog.TabIndex = 1;
            this.buttonDataLog.Text = "HISTORY";
            this.buttonDataLog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDataLog.UseVisualStyleBackColor = false;
            this.buttonDataLog.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonSimulator
            // 
            this.buttonSimulator.Location = new System.Drawing.Point(487, 3);
            this.buttonSimulator.Name = "buttonSimulator";
            this.buttonSimulator.Size = new System.Drawing.Size(69, 64);
            this.buttonSimulator.TabIndex = 4;
            this.buttonSimulator.Text = "Simulate";
            this.buttonSimulator.UseVisualStyleBackColor = true;
            this.buttonSimulator.Visible = false;
            this.buttonSimulator.Click += new System.EventHandler(this.buttonSimulator_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.BackColor = System.Drawing.Color.Transparent;
            this.buttonHelp.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelp.Image = global::Dms.HMI.Properties.Resources.Help;
            this.buttonHelp.Location = new System.Drawing.Point(933, 3);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(69, 64);
            this.buttonHelp.TabIndex = 0;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonHelp.UseVisualStyleBackColor = false;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonAlarm
            // 
            this.buttonAlarm.BackColor = System.Drawing.Color.Transparent;
            this.buttonAlarm.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAlarm.Image = global::Dms.HMI.Properties.Resources.alarms;
            this.buttonAlarm.Location = new System.Drawing.Point(770, 3);
            this.buttonAlarm.Name = "buttonAlarm";
            this.buttonAlarm.Size = new System.Drawing.Size(69, 64);
            this.buttonAlarm.TabIndex = 0;
            this.buttonAlarm.Text = "Alarms";
            this.buttonAlarm.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAlarm.UseVisualStyleBackColor = false;
            this.buttonAlarm.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.Transparent;
            this.buttonExit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Image = global::Dms.HMI.Properties.Resources.Exit;
            this.buttonExit.Location = new System.Drawing.Point(562, 3);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(69, 64);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "Exit";
            this.buttonExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonSetup
            // 
            this.buttonSetup.BackColor = System.Drawing.Color.Transparent;
            this.buttonSetup.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetup.Image = global::Dms.HMI.Properties.Resources.SetUp;
            this.buttonSetup.Location = new System.Drawing.Point(311, 3);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(69, 64);
            this.buttonSetup.TabIndex = 0;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSetup.UseVisualStyleBackColor = false;
            this.buttonSetup.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonRecipe
            // 
            this.buttonRecipe.BackColor = System.Drawing.Color.Transparent;
            this.buttonRecipe.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecipe.Image = global::Dms.HMI.Properties.Resources.Recipes;
            this.buttonRecipe.Location = new System.Drawing.Point(234, 3);
            this.buttonRecipe.Name = "buttonRecipe";
            this.buttonRecipe.Size = new System.Drawing.Size(69, 64);
            this.buttonRecipe.TabIndex = 0;
            this.buttonRecipe.Text = "Recipes";
            this.buttonRecipe.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRecipe.UseVisualStyleBackColor = false;
            this.buttonRecipe.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonServo
            // 
            this.buttonServo.BackColor = System.Drawing.Color.Transparent;
            this.buttonServo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonServo.Image = global::Dms.HMI.Properties.Resources.Servo;
            this.buttonServo.Location = new System.Drawing.Point(157, 3);
            this.buttonServo.Name = "buttonServo";
            this.buttonServo.Size = new System.Drawing.Size(69, 64);
            this.buttonServo.TabIndex = 0;
            this.buttonServo.Text = "Servo";
            this.buttonServo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonServo.UseVisualStyleBackColor = false;
            this.buttonServo.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonSystem
            // 
            this.buttonSystem.BackColor = System.Drawing.Color.Transparent;
            this.buttonSystem.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSystem.Image = global::Dms.HMI.Properties.Resources.System;
            this.buttonSystem.Location = new System.Drawing.Point(80, 3);
            this.buttonSystem.Name = "buttonSystem";
            this.buttonSystem.Size = new System.Drawing.Size(69, 64);
            this.buttonSystem.TabIndex = 0;
            this.buttonSystem.Text = "System";
            this.buttonSystem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSystem.UseVisualStyleBackColor = false;
            this.buttonSystem.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonJobs
            // 
            this.buttonJobs.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonJobs.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonJobs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonJobs.Image = global::Dms.HMI.Properties.Resources.Jobs;
            this.buttonJobs.Location = new System.Drawing.Point(3, 3);
            this.buttonJobs.Name = "buttonJobs";
            this.buttonJobs.Size = new System.Drawing.Size(69, 64);
            this.buttonJobs.TabIndex = 0;
            this.buttonJobs.Text = "Jobs";
            this.buttonJobs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonJobs.UseVisualStyleBackColor = false;
            this.buttonJobs.Click += new System.EventHandler(this.NaviButton_Click);
            // 
            // buttonBuzzer
            // 
            this.buttonBuzzer.Image = global::Dms.HMI.Properties.Resources.BuzzerOff;
            this.buttonBuzzer.Location = new System.Drawing.Point(849, 3);
            this.buttonBuzzer.Name = "buttonBuzzer";
            this.buttonBuzzer.Size = new System.Drawing.Size(69, 64);
            this.buttonBuzzer.TabIndex = 1;
            this.buttonBuzzer.Text = "Buzzer";
            this.buttonBuzzer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonBuzzer.UseVisualStyleBackColor = true;
            this.buttonBuzzer.Click += new System.EventHandler(this.buttonBuzzer_Click);
            // 
            // viewSalienceAlarm
            // 
            this.viewSalienceAlarm.BackColor = System.Drawing.Color.Transparent;
            this.viewSalienceAlarm.ColorAlarm = System.Drawing.Color.LightCoral;
            this.viewSalienceAlarm.ColorAttention = System.Drawing.Color.Chartreuse;
            this.viewSalienceAlarm.ColorCaution = System.Drawing.Color.Yellow;
            this.viewSalienceAlarm.ColorProcess = System.Drawing.Color.SkyBlue;
            this.viewSalienceAlarm.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewSalienceAlarm.DeviceTagInfo")));
            this.viewSalienceAlarm.Location = new System.Drawing.Point(766, 0);
            this.viewSalienceAlarm.Name = "viewSalienceAlarm";
            this.viewSalienceAlarm.Size = new System.Drawing.Size(77, 70);
            this.viewSalienceAlarm.TabIndex = 1;
            // 
            // timerDateTime
            // 
            this.timerDateTime.Interval = 500;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            // 
            // lblViewName
            // 
            this.lblViewName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblViewName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblViewName.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblViewName.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewName.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblViewName.Location = new System.Drawing.Point(264, 4);
            this.lblViewName.Name = "lblViewName";
            this.lblViewName.Size = new System.Drawing.Size(130, 35);
            this.lblViewName.TabIndex = 7;
            this.lblViewName.Text = "JOBS";
            this.lblViewName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDate.Location = new System.Drawing.Point(400, 3);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(111, 19);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "2999/12/31";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTime.Location = new System.Drawing.Point(400, 22);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(111, 18);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "PM 4:34:56";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.TitlePanel.Controls.Add(this.genInfo3);
            this.TitlePanel.Controls.Add(this.genInfo2);
            this.TitlePanel.Controls.Add(this.genInfo1);
            this.TitlePanel.Controls.Add(this.txtUserId);
            this.TitlePanel.Controls.Add(this.btnLogin);
            this.TitlePanel.Controls.Add(this.label4);
            this.TitlePanel.Controls.Add(this.mainStatusType1);
            this.TitlePanel.Controls.Add(this.pictureBoxLogo);
            this.TitlePanel.Controls.Add(this.lblTime);
            this.TitlePanel.Controls.Add(this.lblDate);
            this.TitlePanel.Controls.Add(this.lblViewName);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(1018, 65);
            this.TitlePanel.TabIndex = 2;
            // 
            // genInfo3
            // 
            this.genInfo3.AutoHide = false;
            this.genInfo3.AutoHideLogic = true;
            this.genInfo3.BackColor = System.Drawing.Color.Transparent;
            this.genInfo3.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo3.DeviceTagInfo")));
            this.genInfo3.Distance = 1;
            this.genInfo3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo3.Location = new System.Drawing.Point(774, 44);
            this.genInfo3.Name = "genInfo3";
            this.genInfo3.ReferenceTagDescriptor = null;
            this.genInfo3.Size = new System.Drawing.Size(239, 20);
            this.genInfo3.TabIndex = 22;
            this.genInfo3.TitleBackColor = System.Drawing.SystemColors.InactiveCaption;
            this.genInfo3.TitleForeColor = System.Drawing.Color.White;
            this.genInfo3.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo3.TitlePanelSize = 101;
            this.genInfo3.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo3.TitleText = "DownStream";
            this.genInfo3.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo3.ValueBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.genInfo3.ValueBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo3.ValueForeColor = System.Drawing.Color.Blue;
            this.genInfo3.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo3.ValueTextFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo3.ValueTextUnit = Dms.Common.UnitType.None;
            // 
            // genInfo2
            // 
            this.genInfo2.AutoHide = false;
            this.genInfo2.AutoHideLogic = true;
            this.genInfo2.BackColor = System.Drawing.Color.Transparent;
            this.genInfo2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo2.DeviceTagInfo")));
            this.genInfo2.Distance = 1;
            this.genInfo2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.Location = new System.Drawing.Point(774, 23);
            this.genInfo2.Name = "genInfo2";
            this.genInfo2.ReferenceTagDescriptor = null;
            this.genInfo2.Size = new System.Drawing.Size(239, 20);
            this.genInfo2.TabIndex = 22;
            this.genInfo2.TitleBackColor = System.Drawing.SystemColors.InactiveCaption;
            this.genInfo2.TitleForeColor = System.Drawing.Color.White;
            this.genInfo2.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo2.TitlePanelSize = 101;
            this.genInfo2.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo2.TitleText = "UpStream";
            this.genInfo2.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.ValueBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.genInfo2.ValueBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo2.ValueForeColor = System.Drawing.Color.Blue;
            this.genInfo2.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo2.ValueTextFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.ValueTextUnit = Dms.Common.UnitType.None;
            // 
            // genInfo1
            // 
            this.genInfo1.AutoHide = false;
            this.genInfo1.AutoHideLogic = true;
            this.genInfo1.BackColor = System.Drawing.Color.Transparent;
            this.genInfo1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo1.DeviceTagInfo")));
            this.genInfo1.Distance = 1;
            this.genInfo1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.Location = new System.Drawing.Point(774, 3);
            this.genInfo1.Name = "genInfo1";
            this.genInfo1.ReferenceTagDescriptor = null;
            this.genInfo1.Size = new System.Drawing.Size(239, 20);
            this.genInfo1.TabIndex = 20;
            this.genInfo1.TitleBackColor = System.Drawing.SystemColors.InactiveCaption;
            this.genInfo1.TitleForeColor = System.Drawing.Color.White;
            this.genInfo1.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo1.TitlePanelSize = 101;
            this.genInfo1.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo1.TitleText = "I/O Controller";
            this.genInfo1.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueBackColor = System.Drawing.SystemColors.InactiveBorder;
            this.genInfo1.ValueBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.genInfo1.ValueForeColor = System.Drawing.Color.Blue;
            this.genInfo1.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo1.ValueTextFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueTextUnit = Dms.Common.UnitType.None;
            // 
            // txtUserId
            // 
            this.txtUserId.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtUserId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserId.Location = new System.Drawing.Point(620, 18);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.ReadOnly = true;
            this.txtUserId.Size = new System.Drawing.Size(100, 21);
            this.txtUserId.TabIndex = 17;
            this.txtUserId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.SystemColors.Window;
            this.btnLogin.Location = new System.Drawing.Point(724, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(44, 36);
            this.btnLogin.TabIndex = 19;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(627, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "OPERATOR ID";
            // 
            // mainStatusType1
            // 
            this.mainStatusType1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.mainStatusType1.ControlAutoBackColor = System.Drawing.Color.DarkGray;
            this.mainStatusType1.ControlAutoForeColor = System.Drawing.Color.Gold;
            this.mainStatusType1.ControlManualBackColor = System.Drawing.Color.Silver;
            this.mainStatusType1.ControlManualForeColor = System.Drawing.Color.Gold;
            this.mainStatusType1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("mainStatusType1.DeviceTagInfo")));
            this.mainStatusType1.EqpFaultBackColor = System.Drawing.Color.White;
            this.mainStatusType1.EqpFaultForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.EqpNormalBackColor = System.Drawing.Color.White;
            this.mainStatusType1.EqpNormalForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.EqpPMBackColor = System.Drawing.Color.White;
            this.mainStatusType1.EqpPMForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.EqpUnitDeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("mainStatusType1.EqpUnitDeviceTagInfo")));
            this.mainStatusType1.HostDeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("mainStatusType1.HostDeviceTagInfo")));
            this.mainStatusType1.Location = new System.Drawing.Point(3, 3);
            this.mainStatusType1.Name = "mainStatusType1";
            this.mainStatusType1.ProcExeBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcExeForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.ProcIdleBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcIdleForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.ProcInitBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcInitForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.ProcPauseBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcPauseForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.ProcReadyBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcReadyForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.ProcSetupBackColor = System.Drawing.Color.White;
            this.mainStatusType1.ProcSetupForeColor = System.Drawing.Color.Black;
            this.mainStatusType1.RemoteControlBackColor = System.Drawing.Color.LightGreen;
            this.mainStatusType1.RemoteControlForeColor = System.Drawing.Color.Blue;
            this.mainStatusType1.RemoteMonitorBackColor = System.Drawing.Color.Yellow;
            this.mainStatusType1.RemoteMonitorForeColor = System.Drawing.Color.Red;
            this.mainStatusType1.RemoteOfflineBackColor = System.Drawing.Color.White;
            this.mainStatusType1.RemoteOfflineForeColor = System.Drawing.Color.Red;
            this.mainStatusType1.Size = new System.Drawing.Size(257, 59);
            this.mainStatusType1.TabIndex = 15;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxLogo.Image = global::Dms.HMI.Properties.Resources.logo_new;
            this.pictureBoxLogo.Location = new System.Drawing.Point(517, 3);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(97, 37);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLogo.TabIndex = 12;
            this.pictureBoxLogo.TabStop = false;
            // 
            // MessagelogList
            // 
            this.MessagelogList.BackColor = System.Drawing.Color.Transparent;
            this.MessagelogList.CheckEnable = true;
            this.MessagelogList.ClearSelection = false;
            this.MessagelogList.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("MessagelogList.DeviceTagInfo")));
            this.MessagelogList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessagelogList.Location = new System.Drawing.Point(264, 43);
            this.MessagelogList.Name = "MessagelogList";
            this.MessagelogList.Size = new System.Drawing.Size(504, 19);
            this.MessagelogList.TabIndex = 23;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1018, 736);
            this.ControlBox = false;
            this.Controls.Add(this.MessagelogList);
            this.Controls.Add(this.TitlePanel);
            this.Controls.Add(this.NavigationPanel);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WSSD";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.NavigationPanel.ResumeLayout(false);
            this.TitlePanel.ResumeLayout(false);
            this.TitlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavigationPanel;
        private System.Windows.Forms.Button buttonSystem;
        private System.Windows.Forms.Button buttonJobs;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonRecipe;
        private System.Windows.Forms.Button buttonServo;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Timer timerDateTime;
        private System.Windows.Forms.Label lblViewName;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private Dms.Control.MainStatusType1 mainStatusType1;
        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonAlarm;
        private Dms.Control.GenInfo genInfo1;
        private Dms.Control.GenInfo genInfo3;
        private Dms.Control.GenInfo genInfo2;
        private Dms.Control.LogList MessagelogList;
        private Dms.Control.ViewSalience viewSalienceAlarm;
        private System.Windows.Forms.Button buttonSimulator;
        private System.Windows.Forms.Button buttonBuzzer;
        private System.Windows.Forms.Button buttonDataLog;
        private System.Windows.Forms.Button btnNosubstrate;
        private System.Windows.Forms.Button btnOnline;
    }
}


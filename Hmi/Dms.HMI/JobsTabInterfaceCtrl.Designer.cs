namespace Dms.HMI
{
    partial class JobsTabInterfaceCtrl
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobsTabInterfaceCtrl));
            this.gbIFInterlock = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnloadOn = new System.Windows.Forms.Button();
            this.btnunloadOff = new System.Windows.Forms.Button();
            this.btnunloadOn = new System.Windows.Forms.Button();
            this.btnloadOff = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkNosub = new System.Windows.Forms.CheckBox();
            this.chkUlDataReadcomp = new System.Windows.Forms.CheckBox();
            this.chkLdDataReadReq = new System.Windows.Forms.CheckBox();
            this.ckbULRobotAceess = new System.Windows.Forms.CheckBox();
            this.ckbULReady = new System.Windows.Forms.CheckBox();
            this.ckbLDRobotAceess = new System.Windows.Forms.CheckBox();
            this.ckbLDReady = new System.Windows.Forms.CheckBox();
            this.ckbLoaderPower = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCleanOut = new System.Windows.Forms.CheckBox();
            this.ckbUnloadInterlock = new System.Windows.Forms.CheckBox();
            this.ckbUnloadWait = new System.Windows.Forms.CheckBox();
            this.chkUlDataReadReq = new System.Windows.Forms.CheckBox();
            this.chkUlSubstratePresent = new System.Windows.Forms.CheckBox();
            this.chkLdDataReadcomp = new System.Windows.Forms.CheckBox();
            this.chkLdSubstratePresent = new System.Windows.Forms.CheckBox();
            this.ckbLDAccessPossible = new System.Windows.Forms.CheckBox();
            this.ckbULAccessPossible = new System.Windows.Forms.CheckBox();
            this.ckbLoadReq = new System.Windows.Forms.CheckBox();
            this.ckbUnloadReq = new System.Windows.Forms.CheckBox();
            this.ckbHDCAvaliable = new System.Windows.Forms.CheckBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.ifStepsSend = new Dms.Control.IfSteps();
            this.ifStepsRecv = new Dms.Control.IfSteps();
            this.gbIFInterlock.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbIFInterlock
            // 
            this.gbIFInterlock.Controls.Add(this.groupBox6);
            this.gbIFInterlock.Controls.Add(this.groupBox2);
            this.gbIFInterlock.Controls.Add(this.groupBox1);
            this.gbIFInterlock.Location = new System.Drawing.Point(515, 21);
            this.gbIFInterlock.Name = "gbIFInterlock";
            this.gbIFInterlock.Size = new System.Drawing.Size(383, 415);
            this.gbIFInterlock.TabIndex = 0;
            this.gbIFInterlock.TabStop = false;
            this.gbIFInterlock.Text = "Signal Display";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnloadOn);
            this.groupBox6.Controls.Add(this.btnunloadOff);
            this.groupBox6.Controls.Add(this.btnunloadOn);
            this.groupBox6.Controls.Add(this.btnloadOff);
            this.groupBox6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(195, 293);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(184, 117);
            this.groupBox6.TabIndex = 438;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Signal Control";
            // 
            // btnloadOn
            // 
            this.btnloadOn.BackColor = System.Drawing.Color.Red;
            this.btnloadOn.Location = new System.Drawing.Point(6, 22);
            this.btnloadOn.Name = "btnloadOn";
            this.btnloadOn.Size = new System.Drawing.Size(82, 38);
            this.btnloadOn.TabIndex = 125;
            this.btnloadOn.Text = "Load Enable ON";
            this.btnloadOn.UseVisualStyleBackColor = false;
            this.btnloadOn.Click += new System.EventHandler(this.LoadEnableOn_Click);
            // 
            // btnunloadOff
            // 
            this.btnunloadOff.BackColor = System.Drawing.Color.Red;
            this.btnunloadOff.Location = new System.Drawing.Point(94, 66);
            this.btnunloadOff.Name = "btnunloadOff";
            this.btnunloadOff.Size = new System.Drawing.Size(82, 38);
            this.btnunloadOff.TabIndex = 128;
            this.btnunloadOff.Text = "Unload Enable Off";
            this.btnunloadOff.UseVisualStyleBackColor = false;
            this.btnunloadOff.Click += new System.EventHandler(this.UnloadEnableOff_Click);
            // 
            // btnunloadOn
            // 
            this.btnunloadOn.BackColor = System.Drawing.Color.Red;
            this.btnunloadOn.Location = new System.Drawing.Point(6, 66);
            this.btnunloadOn.Name = "btnunloadOn";
            this.btnunloadOn.Size = new System.Drawing.Size(82, 38);
            this.btnunloadOn.TabIndex = 126;
            this.btnunloadOn.Text = "Unload Enable ON";
            this.btnunloadOn.UseVisualStyleBackColor = false;
            this.btnunloadOn.Click += new System.EventHandler(this.UnloadEnableOn_Click);
            // 
            // btnloadOff
            // 
            this.btnloadOff.BackColor = System.Drawing.Color.Red;
            this.btnloadOff.Location = new System.Drawing.Point(94, 22);
            this.btnloadOff.Name = "btnloadOff";
            this.btnloadOff.Size = new System.Drawing.Size(82, 38);
            this.btnloadOff.TabIndex = 127;
            this.btnloadOff.Text = "Load Enable Off";
            this.btnloadOff.UseVisualStyleBackColor = false;
            this.btnloadOff.Click += new System.EventHandler(this.LoadEnableOff_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkNosub);
            this.groupBox2.Controls.Add(this.chkUlDataReadcomp);
            this.groupBox2.Controls.Add(this.chkLdDataReadReq);
            this.groupBox2.Controls.Add(this.ckbULRobotAceess);
            this.groupBox2.Controls.Add(this.ckbULReady);
            this.groupBox2.Controls.Add(this.ckbLDRobotAceess);
            this.groupBox2.Controls.Add(this.ckbLDReady);
            this.groupBox2.Controls.Add(this.ckbLoaderPower);
            this.groupBox2.Location = new System.Drawing.Point(195, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(175, 273);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Loader";
            // 
            // chkNosub
            // 
            this.chkNosub.AutoSize = true;
            this.chkNosub.BackColor = System.Drawing.Color.Transparent;
            this.chkNosub.Location = new System.Drawing.Point(6, 229);
            this.chkNosub.Name = "chkNosub";
            this.chkNosub.Size = new System.Drawing.Size(102, 19);
            this.chkNosub.TabIndex = 439;
            this.chkNosub.Text = "NoSubstarate";
            this.chkNosub.UseVisualStyleBackColor = false;
            // 
            // chkUlDataReadcomp
            // 
            this.chkUlDataReadcomp.AutoSize = true;
            this.chkUlDataReadcomp.BackColor = System.Drawing.Color.Transparent;
            this.chkUlDataReadcomp.Location = new System.Drawing.Point(6, 204);
            this.chkUlDataReadcomp.Name = "chkUlDataReadcomp";
            this.chkUlDataReadcomp.Size = new System.Drawing.Size(156, 19);
            this.chkUlDataReadcomp.TabIndex = 15;
            this.chkUlDataReadcomp.Text = "Unload Data completed";
            this.chkUlDataReadcomp.UseVisualStyleBackColor = false;
            // 
            // chkLdDataReadReq
            // 
            this.chkLdDataReadReq.AutoSize = true;
            this.chkLdDataReadReq.BackColor = System.Drawing.Color.Transparent;
            this.chkLdDataReadReq.Location = new System.Drawing.Point(6, 128);
            this.chkLdDataReadReq.Name = "chkLdDataReadReq";
            this.chkLdDataReadReq.Size = new System.Drawing.Size(166, 19);
            this.chkLdDataReadReq.TabIndex = 14;
            this.chkLdDataReadReq.Text = "Load Data Read Request";
            this.chkLdDataReadReq.UseVisualStyleBackColor = false;
            // 
            // ckbULRobotAceess
            // 
            this.ckbULRobotAceess.AutoSize = true;
            this.ckbULRobotAceess.Location = new System.Drawing.Point(6, 179);
            this.ckbULRobotAceess.Name = "ckbULRobotAceess";
            this.ckbULRobotAceess.Size = new System.Drawing.Size(162, 19);
            this.ckbULRobotAceess.TabIndex = 10;
            this.ckbULRobotAceess.Text = "Unload Robot Accessing";
            this.ckbULRobotAceess.UseVisualStyleBackColor = true;
            // 
            // ckbULReady
            // 
            this.ckbULReady.AutoSize = true;
            this.ckbULReady.Location = new System.Drawing.Point(6, 154);
            this.ckbULReady.Name = "ckbULReady";
            this.ckbULReady.Size = new System.Drawing.Size(153, 19);
            this.ckbULReady.TabIndex = 9;
            this.ckbULReady.Text = "Unload Transfer Ready";
            this.ckbULReady.UseVisualStyleBackColor = true;
            // 
            // ckbLDRobotAceess
            // 
            this.ckbLDRobotAceess.AutoSize = true;
            this.ckbLDRobotAceess.Location = new System.Drawing.Point(6, 78);
            this.ckbLDRobotAceess.Name = "ckbLDRobotAceess";
            this.ckbLDRobotAceess.Size = new System.Drawing.Size(150, 19);
            this.ckbLDRobotAceess.TabIndex = 6;
            this.ckbLDRobotAceess.Text = "Load Robot Accessing";
            this.ckbLDRobotAceess.UseVisualStyleBackColor = true;
            // 
            // ckbLDReady
            // 
            this.ckbLDReady.AutoSize = true;
            this.ckbLDReady.Location = new System.Drawing.Point(6, 53);
            this.ckbLDReady.Name = "ckbLDReady";
            this.ckbLDReady.Size = new System.Drawing.Size(141, 19);
            this.ckbLDReady.TabIndex = 1;
            this.ckbLDReady.Text = "Load Transfer Ready";
            this.ckbLDReady.UseVisualStyleBackColor = true;
            // 
            // ckbLoaderPower
            // 
            this.ckbLoaderPower.AutoSize = true;
            this.ckbLoaderPower.Location = new System.Drawing.Point(6, 20);
            this.ckbLoaderPower.Name = "ckbLoaderPower";
            this.ckbLoaderPower.Size = new System.Drawing.Size(124, 19);
            this.ckbLoaderPower.TabIndex = 1;
            this.ckbLoaderPower.Text = "Loader Power ON";
            this.ckbLoaderPower.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkCleanOut);
            this.groupBox1.Controls.Add(this.ckbUnloadInterlock);
            this.groupBox1.Controls.Add(this.ckbUnloadWait);
            this.groupBox1.Controls.Add(this.chkUlDataReadReq);
            this.groupBox1.Controls.Add(this.chkUlSubstratePresent);
            this.groupBox1.Controls.Add(this.chkLdDataReadcomp);
            this.groupBox1.Controls.Add(this.chkLdSubstratePresent);
            this.groupBox1.Controls.Add(this.ckbLDAccessPossible);
            this.groupBox1.Controls.Add(this.ckbULAccessPossible);
            this.groupBox1.Controls.Add(this.ckbLoadReq);
            this.groupBox1.Controls.Add(this.ckbUnloadReq);
            this.groupBox1.Controls.Add(this.ckbHDCAvaliable);
            this.groupBox1.Location = new System.Drawing.Point(6, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 333);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HDC";
            // 
            // chkCleanOut
            // 
            this.chkCleanOut.AutoSize = true;
            this.chkCleanOut.BackColor = System.Drawing.Color.Transparent;
            this.chkCleanOut.Location = new System.Drawing.Point(6, 304);
            this.chkCleanOut.Name = "chkCleanOut";
            this.chkCleanOut.Size = new System.Drawing.Size(114, 19);
            this.chkCleanOut.TabIndex = 18;
            this.chkCleanOut.Text = "Clean Out Mode";
            this.chkCleanOut.UseVisualStyleBackColor = false;
            // 
            // ckbUnloadInterlock
            // 
            this.ckbUnloadInterlock.AutoSize = true;
            this.ckbUnloadInterlock.Location = new System.Drawing.Point(6, 154);
            this.ckbUnloadInterlock.Name = "ckbUnloadInterlock";
            this.ckbUnloadInterlock.Size = new System.Drawing.Size(119, 19);
            this.ckbUnloadInterlock.TabIndex = 17;
            this.ckbUnloadInterlock.Text = "Unload InterLock";
            this.ckbUnloadInterlock.UseVisualStyleBackColor = true;
            // 
            // ckbUnloadWait
            // 
            this.ckbUnloadWait.AutoSize = true;
            this.ckbUnloadWait.Location = new System.Drawing.Point(6, 179);
            this.ckbUnloadWait.Name = "ckbUnloadWait";
            this.ckbUnloadWait.Size = new System.Drawing.Size(93, 19);
            this.ckbUnloadWait.TabIndex = 16;
            this.ckbUnloadWait.Text = "Unload Wait";
            this.ckbUnloadWait.UseVisualStyleBackColor = true;
            // 
            // chkUlDataReadReq
            // 
            this.chkUlDataReadReq.AutoSize = true;
            this.chkUlDataReadReq.BackColor = System.Drawing.Color.Transparent;
            this.chkUlDataReadReq.Location = new System.Drawing.Point(6, 279);
            this.chkUlDataReadReq.Name = "chkUlDataReadReq";
            this.chkUlDataReadReq.Size = new System.Drawing.Size(178, 19);
            this.chkUlDataReadReq.TabIndex = 15;
            this.chkUlDataReadReq.Text = "Unload Data Read Request";
            this.chkUlDataReadReq.UseVisualStyleBackColor = false;
            // 
            // chkUlSubstratePresent
            // 
            this.chkUlSubstratePresent.AutoSize = true;
            this.chkUlSubstratePresent.BackColor = System.Drawing.Color.Transparent;
            this.chkUlSubstratePresent.Location = new System.Drawing.Point(6, 254);
            this.chkUlSubstratePresent.Name = "chkUlSubstratePresent";
            this.chkUlSubstratePresent.Size = new System.Drawing.Size(172, 19);
            this.chkUlSubstratePresent.TabIndex = 14;
            this.chkUlSubstratePresent.Text = "Unload Present at Cleaner";
            this.chkUlSubstratePresent.UseVisualStyleBackColor = false;
            // 
            // chkLdDataReadcomp
            // 
            this.chkLdDataReadcomp.AutoSize = true;
            this.chkLdDataReadcomp.BackColor = System.Drawing.Color.Transparent;
            this.chkLdDataReadcomp.Location = new System.Drawing.Point(6, 128);
            this.chkLdDataReadcomp.Name = "chkLdDataReadcomp";
            this.chkLdDataReadcomp.Size = new System.Drawing.Size(144, 19);
            this.chkLdDataReadcomp.TabIndex = 13;
            this.chkLdDataReadcomp.Text = "Load Data completed";
            this.chkLdDataReadcomp.UseVisualStyleBackColor = false;
            // 
            // chkLdSubstratePresent
            // 
            this.chkLdSubstratePresent.AutoSize = true;
            this.chkLdSubstratePresent.BackColor = System.Drawing.Color.Transparent;
            this.chkLdSubstratePresent.Location = new System.Drawing.Point(6, 103);
            this.chkLdSubstratePresent.Name = "chkLdSubstratePresent";
            this.chkLdSubstratePresent.Size = new System.Drawing.Size(160, 19);
            this.chkLdSubstratePresent.TabIndex = 12;
            this.chkLdSubstratePresent.Text = "Load Present at Cleaner";
            this.chkLdSubstratePresent.UseVisualStyleBackColor = false;
            // 
            // ckbLDAccessPossible
            // 
            this.ckbLDAccessPossible.AutoSize = true;
            this.ckbLDAccessPossible.BackColor = System.Drawing.Color.Transparent;
            this.ckbLDAccessPossible.Location = new System.Drawing.Point(6, 78);
            this.ckbLDAccessPossible.Name = "ckbLDAccessPossible";
            this.ckbLDAccessPossible.Size = new System.Drawing.Size(149, 19);
            this.ckbLDAccessPossible.TabIndex = 11;
            this.ckbLDAccessPossible.Text = "Load Access Possible";
            this.ckbLDAccessPossible.UseVisualStyleBackColor = false;
            // 
            // ckbULAccessPossible
            // 
            this.ckbULAccessPossible.AutoSize = true;
            this.ckbULAccessPossible.Location = new System.Drawing.Point(6, 229);
            this.ckbULAccessPossible.Name = "ckbULAccessPossible";
            this.ckbULAccessPossible.Size = new System.Drawing.Size(161, 19);
            this.ckbULAccessPossible.TabIndex = 10;
            this.ckbULAccessPossible.Text = "Unload Access Possible";
            this.ckbULAccessPossible.UseVisualStyleBackColor = true;
            // 
            // ckbLoadReq
            // 
            this.ckbLoadReq.AutoSize = true;
            this.ckbLoadReq.BackColor = System.Drawing.Color.Transparent;
            this.ckbLoadReq.Location = new System.Drawing.Point(6, 53);
            this.ckbLoadReq.Name = "ckbLoadReq";
            this.ckbLoadReq.Size = new System.Drawing.Size(104, 19);
            this.ckbLoadReq.TabIndex = 2;
            this.ckbLoadReq.Text = "Load Request";
            this.ckbLoadReq.UseVisualStyleBackColor = false;
            // 
            // ckbUnloadReq
            // 
            this.ckbUnloadReq.AutoSize = true;
            this.ckbUnloadReq.Location = new System.Drawing.Point(6, 204);
            this.ckbUnloadReq.Name = "ckbUnloadReq";
            this.ckbUnloadReq.Size = new System.Drawing.Size(116, 19);
            this.ckbUnloadReq.TabIndex = 7;
            this.ckbUnloadReq.Text = "Unload Request";
            this.ckbUnloadReq.UseVisualStyleBackColor = true;
            // 
            // ckbHDCAvaliable
            // 
            this.ckbHDCAvaliable.AutoSize = true;
            this.ckbHDCAvaliable.Location = new System.Drawing.Point(6, 20);
            this.ckbHDCAvaliable.Name = "ckbHDCAvaliable";
            this.ckbHDCAvaliable.Size = new System.Drawing.Size(135, 19);
            this.ckbHDCAvaliable.TabIndex = 0;
            this.ckbHDCAvaliable.Text = "Cleaner is Available";
            this.ckbHDCAvaliable.UseVisualStyleBackColor = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // ifStepsSend
            // 
            this.ifStepsSend.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsSend.DataCellHeight = 35;
            this.ifStepsSend.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsSend.DeviceTagInfo")));
            this.ifStepsSend.DownstreamName = "Cleaner";
            this.ifStepsSend.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsSend.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ifStepsSend.Location = new System.Drawing.Point(259, 21);
            this.ifStepsSend.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsSend.Name = "ifStepsSend";
            this.ifStepsSend.Size = new System.Drawing.Size(218, 415);
            this.ifStepsSend.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsSend.StepMaxNo = 7;
            this.ifStepsSend.StepName = new string[] {
        "UnLoad Request",
        "UnLoad Ready to Cleaner",
        "UnLoad Access Possible",
        "UnLoad Accessing",
        "Send Glass Check",
        "Send Glass Data Check",
        "Interface Complete"};
            this.ifStepsSend.TabIndex = 2;
            this.ifStepsSend.Title = "Glass Send";
            this.ifStepsSend.UpstreamName = "Loader";
            // 
            // ifStepsRecv
            // 
            this.ifStepsRecv.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsRecv.DataCellHeight = 35;
            this.ifStepsRecv.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsRecv.DeviceTagInfo")));
            this.ifStepsRecv.DownstreamName = "Cleaner";
            this.ifStepsRecv.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsRecv.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ifStepsRecv.Location = new System.Drawing.Point(37, 21);
            this.ifStepsRecv.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsRecv.Name = "ifStepsRecv";
            this.ifStepsRecv.Size = new System.Drawing.Size(216, 415);
            this.ifStepsRecv.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsRecv.StepMaxNo = 7;
            this.ifStepsRecv.StepName = new string[] {
        "Load Request",
        "Load Ready to Cleaner",
        "Load Access Possible",
        "Load Accessing",
        "Recv Glass Check",
        "Recv Glass Data Check",
        "Interface Complete"};
            this.ifStepsRecv.TabIndex = 1;
            this.ifStepsRecv.Title = "Glass Recv";
            this.ifStepsRecv.UpstreamName = "Loader";
            // 
            // JobsTabInterfaceCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ifStepsSend);
            this.Controls.Add(this.ifStepsRecv);
            this.Controls.Add(this.gbIFInterlock);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "JobsTabInterfaceCtrl";
            this.Size = new System.Drawing.Size(901, 465);
            this.gbIFInterlock.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbIFInterlock;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbHDCAvaliable;
        private System.Windows.Forms.CheckBox ckbLDReady;
        private System.Windows.Forms.CheckBox ckbLoaderPower;
        private System.Windows.Forms.CheckBox ckbLoadReq;
        private System.Windows.Forms.CheckBox ckbLDRobotAceess;
        private System.Windows.Forms.CheckBox ckbUnloadReq;
        private System.Windows.Forms.CheckBox ckbULAccessPossible;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.CheckBox ckbLDAccessPossible;
        private System.Windows.Forms.CheckBox ckbULRobotAceess;
        private System.Windows.Forms.CheckBox ckbULReady;
        private Dms.Control.IfSteps ifStepsRecv;
        private Dms.Control.IfSteps ifStepsSend;
        private System.Windows.Forms.CheckBox chkLdSubstratePresent;
        private System.Windows.Forms.CheckBox chkLdDataReadcomp;
        private System.Windows.Forms.CheckBox chkLdDataReadReq;
        private System.Windows.Forms.CheckBox chkUlDataReadcomp;
        private System.Windows.Forms.CheckBox chkUlDataReadReq;
        private System.Windows.Forms.CheckBox chkUlSubstratePresent;
        private System.Windows.Forms.CheckBox ckbUnloadInterlock;
        private System.Windows.Forms.CheckBox ckbUnloadWait;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnloadOn;
        private System.Windows.Forms.Button btnunloadOff;
        private System.Windows.Forms.Button btnunloadOn;
        private System.Windows.Forms.Button btnloadOff;
        private System.Windows.Forms.CheckBox chkNosub;
        private System.Windows.Forms.CheckBox chkCleanOut;

    }
}

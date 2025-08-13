namespace Dms.Control
{
	partial class ViewYaskawaNX100
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
			if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewYaskawaNX100));
            this.groupBoxExternalCommand = new System.Windows.Forms.GroupBox();
            this.btnRobotHome = new System.Windows.Forms.CheckBox();
            this.btnAlarmReset = new System.Windows.Forms.CheckBox();
            this.btnRobotHold = new System.Windows.Forms.CheckBox();
            this.btnServoOff = new System.Windows.Forms.CheckBox();
            this.btnServoOn = new System.Windows.Forms.CheckBox();
            this.btnMotionStrobe = new System.Windows.Forms.CheckBox();
            this.groupBoxMotionCommand = new System.Windows.Forms.GroupBox();
            this.cboSelectCube = new System.Windows.Forms.ComboBox();
            this.cboOperation = new System.Windows.Forms.ComboBox();
            this.cboSelectHand = new System.Windows.Forms.ComboBox();
            this.cboSelectSlot = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelIoStatusPlc = new System.Windows.Forms.Panel();
            this.viewDeviceIoPlc = new Dms.Control.ViewDeviceIo();
            this.panelIfSteps = new System.Windows.Forms.Panel();
            this.ifStepsExchange = new Dms.Control.IfSteps();
            this.ifStepsHome = new Dms.Control.IfSteps();
            this.ifStepsPutPin = new Dms.Control.IfSteps();
            this.ifStepsPut = new Dms.Control.IfSteps();
            this.ifStepsGetPin = new Dms.Control.IfSteps();
            this.ifStepsGet = new Dms.Control.IfSteps();
            this.panelGroupGrid = new System.Windows.Forms.Panel();
            this.chkCubePassable = new System.Windows.Forms.CheckBox();
            this.chkCubeProhibition = new System.Windows.Forms.CheckBox();
            this.chkCubeInterfere = new System.Windows.Forms.CheckBox();
            this.panelIoStatusNx = new System.Windows.Forms.Panel();
            this.viewDeviceIoNx = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoStatus3 = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoStatus2 = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoStatus1 = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoAlarm3 = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoAlarm2 = new Dms.Control.ViewDeviceIo();
            this.viewDeviceIoAlarm1 = new Dms.Control.ViewDeviceIo();
            this.btnInitCommand = new System.Windows.Forms.CheckBox();
            this.groupBoxExternalCommand.SuspendLayout();
            this.groupBoxMotionCommand.SuspendLayout();
            this.panelIoStatusPlc.SuspendLayout();
            this.panelIfSteps.SuspendLayout();
            this.panelGroupGrid.SuspendLayout();
            this.panelIoStatusNx.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxExternalCommand
            // 
            this.groupBoxExternalCommand.Controls.Add(this.btnRobotHome);
            this.groupBoxExternalCommand.Controls.Add(this.btnInitCommand);
            this.groupBoxExternalCommand.Controls.Add(this.btnAlarmReset);
            this.groupBoxExternalCommand.Controls.Add(this.btnRobotHold);
            this.groupBoxExternalCommand.Controls.Add(this.btnServoOff);
            this.groupBoxExternalCommand.Controls.Add(this.btnServoOn);
            this.groupBoxExternalCommand.Location = new System.Drawing.Point(2, 221);
            this.groupBoxExternalCommand.Name = "groupBoxExternalCommand";
            this.groupBoxExternalCommand.Size = new System.Drawing.Size(193, 239);
            this.groupBoxExternalCommand.TabIndex = 0;
            this.groupBoxExternalCommand.TabStop = false;
            this.groupBoxExternalCommand.Text = "Robot External Command";
            // 
            // btnRobotHome
            // 
            this.btnRobotHome.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRobotHome.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRobotHome.Location = new System.Drawing.Point(6, 69);
            this.btnRobotHome.Name = "btnRobotHome";
            this.btnRobotHome.Size = new System.Drawing.Size(89, 43);
            this.btnRobotHome.TabIndex = 5;
            this.btnRobotHome.Text = "ROBOT HOME";
            this.btnRobotHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRobotHome.UseVisualStyleBackColor = true;
            this.btnRobotHome.Click += new System.EventHandler(this.btnRobotHome_Click);
            // 
            // btnAlarmReset
            // 
            this.btnAlarmReset.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAlarmReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAlarmReset.Location = new System.Drawing.Point(6, 118);
            this.btnAlarmReset.Name = "btnAlarmReset";
            this.btnAlarmReset.Size = new System.Drawing.Size(89, 43);
            this.btnAlarmReset.TabIndex = 5;
            this.btnAlarmReset.Text = "ALARM RESET";
            this.btnAlarmReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAlarmReset.UseVisualStyleBackColor = true;
            // 
            // btnRobotHold
            // 
            this.btnRobotHold.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRobotHold.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRobotHold.Location = new System.Drawing.Point(97, 69);
            this.btnRobotHold.Name = "btnRobotHold";
            this.btnRobotHold.Size = new System.Drawing.Size(89, 43);
            this.btnRobotHold.TabIndex = 5;
            this.btnRobotHold.Text = "ROBOT HOLD";
            this.btnRobotHold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRobotHold.UseVisualStyleBackColor = true;
            this.btnRobotHold.Click += new System.EventHandler(this.btnRobotHold_Click);
            // 
            // btnServoOff
            // 
            this.btnServoOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnServoOff.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnServoOff.Location = new System.Drawing.Point(97, 20);
            this.btnServoOff.Name = "btnServoOff";
            this.btnServoOff.Size = new System.Drawing.Size(89, 43);
            this.btnServoOff.TabIndex = 5;
            this.btnServoOff.Text = "SERVO OFF";
            this.btnServoOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnServoOff.UseVisualStyleBackColor = true;
            this.btnServoOff.Click += new System.EventHandler(this.btnServoOff_Click);
            // 
            // btnServoOn
            // 
            this.btnServoOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnServoOn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnServoOn.Location = new System.Drawing.Point(6, 20);
            this.btnServoOn.Name = "btnServoOn";
            this.btnServoOn.Size = new System.Drawing.Size(89, 43);
            this.btnServoOn.TabIndex = 5;
            this.btnServoOn.Text = "SERVO ON\r\n EX-START";
            this.btnServoOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnServoOn.UseVisualStyleBackColor = true;
            this.btnServoOn.Click += new System.EventHandler(this.btnServoOn_Click);
            // 
            // btnMotionStrobe
            // 
            this.btnMotionStrobe.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnMotionStrobe.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMotionStrobe.Location = new System.Drawing.Point(204, 368);
            this.btnMotionStrobe.Name = "btnMotionStrobe";
            this.btnMotionStrobe.Size = new System.Drawing.Size(187, 57);
            this.btnMotionStrobe.TabIndex = 5;
            this.btnMotionStrobe.Text = "MOTION PATTERN STROBE";
            this.btnMotionStrobe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMotionStrobe.UseVisualStyleBackColor = true;
            this.btnMotionStrobe.Click += new System.EventHandler(this.btnMotionStrobe_Click);
            // 
            // groupBoxMotionCommand
            // 
            this.groupBoxMotionCommand.Controls.Add(this.cboSelectCube);
            this.groupBoxMotionCommand.Controls.Add(this.cboOperation);
            this.groupBoxMotionCommand.Controls.Add(this.cboSelectHand);
            this.groupBoxMotionCommand.Controls.Add(this.cboSelectSlot);
            this.groupBoxMotionCommand.Controls.Add(this.label2);
            this.groupBoxMotionCommand.Controls.Add(this.label3);
            this.groupBoxMotionCommand.Controls.Add(this.label1);
            this.groupBoxMotionCommand.Controls.Add(this.label5);
            this.groupBoxMotionCommand.Location = new System.Drawing.Point(197, 221);
            this.groupBoxMotionCommand.Name = "groupBoxMotionCommand";
            this.groupBoxMotionCommand.Size = new System.Drawing.Size(201, 141);
            this.groupBoxMotionCommand.TabIndex = 1;
            this.groupBoxMotionCommand.TabStop = false;
            this.groupBoxMotionCommand.Text = "Robot Motion Pattern Selection";
            // 
            // cboSelectCube
            // 
            this.cboSelectCube.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectCube.FormattingEnabled = true;
            this.cboSelectCube.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboSelectCube.Location = new System.Drawing.Point(81, 20);
            this.cboSelectCube.Name = "cboSelectCube";
            this.cboSelectCube.Size = new System.Drawing.Size(112, 23);
            this.cboSelectCube.TabIndex = 0;
            this.cboSelectCube.TabStop = false;
            this.cboSelectCube.SelectionChangeCommitted += new System.EventHandler(this.SelectionChangeCommitted);
            // 
            // cboOperation
            // 
            this.cboOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOperation.FormattingEnabled = true;
            this.cboOperation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboOperation.Location = new System.Drawing.Point(81, 112);
            this.cboOperation.Name = "cboOperation";
            this.cboOperation.Size = new System.Drawing.Size(112, 23);
            this.cboOperation.TabIndex = 0;
            this.cboOperation.TabStop = false;
            this.cboOperation.SelectionChangeCommitted += new System.EventHandler(this.SelectionChangeCommitted);
            // 
            // cboSelectHand
            // 
            this.cboSelectHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectHand.FormattingEnabled = true;
            this.cboSelectHand.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboSelectHand.Location = new System.Drawing.Point(81, 81);
            this.cboSelectHand.Name = "cboSelectHand";
            this.cboSelectHand.Size = new System.Drawing.Size(112, 23);
            this.cboSelectHand.TabIndex = 0;
            this.cboSelectHand.TabStop = false;
            this.cboSelectHand.SelectionChangeCommitted += new System.EventHandler(this.SelectionChangeCommitted);
            // 
            // cboSelectSlot
            // 
            this.cboSelectSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectSlot.FormattingEnabled = true;
            this.cboSelectSlot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboSelectSlot.Location = new System.Drawing.Point(81, 51);
            this.cboSelectSlot.Name = "cboSelectSlot";
            this.cboSelectSlot.Size = new System.Drawing.Size(112, 23);
            this.cboSelectSlot.TabIndex = 0;
            this.cboSelectSlot.TabStop = false;
            this.cboSelectSlot.Tag = "";
            this.cboSelectSlot.SelectionChangeCommitted += new System.EventHandler(this.SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "CUBE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(6, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "OPERATION";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(6, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "SLOT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(6, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(188, 23);
            this.label5.TabIndex = 1;
            this.label5.Text = "HAND";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 1000;
            this.toolTip1.ReshowDelay = 1000;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // panelIoStatusPlc
            // 
            this.panelIoStatusPlc.Controls.Add(this.viewDeviceIoPlc);
            this.panelIoStatusPlc.Location = new System.Drawing.Point(338, 0);
            this.panelIoStatusPlc.Name = "panelIoStatusPlc";
            this.panelIoStatusPlc.Size = new System.Drawing.Size(160, 411);
            this.panelIoStatusPlc.TabIndex = 16;
            // 
            // viewDeviceIoPlc
            // 
            this.viewDeviceIoPlc.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoPlc.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoPlc.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoPlc.DeviceTagInfo")));
            this.viewDeviceIoPlc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewDeviceIoPlc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoPlc.GridBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewDeviceIoPlc.IdVisible = false;
            this.viewDeviceIoPlc.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoPlc.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoPlc.Location = new System.Drawing.Point(0, 0);
            this.viewDeviceIoPlc.Name = "viewDeviceIoPlc";
            this.viewDeviceIoPlc.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoPlc.OnColor = System.Drawing.Color.GreenYellow;
            this.viewDeviceIoPlc.Size = new System.Drawing.Size(160, 411);
            this.viewDeviceIoPlc.TabIndex = 15;
            this.viewDeviceIoPlc.Title = "I/O STATUS [PLC]";
            this.viewDeviceIoPlc.TitleTextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.viewDeviceIoPlc.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoPlc.TitleVisible = true;
            // 
            // panelIfSteps
            // 
            this.panelIfSteps.Controls.Add(this.ifStepsExchange);
            this.panelIfSteps.Controls.Add(this.ifStepsHome);
            this.panelIfSteps.Controls.Add(this.ifStepsPutPin);
            this.panelIfSteps.Controls.Add(this.ifStepsPut);
            this.panelIfSteps.Controls.Add(this.ifStepsGetPin);
            this.panelIfSteps.Controls.Add(this.ifStepsGet);
            this.panelIfSteps.Location = new System.Drawing.Point(162, 0);
            this.panelIfSteps.Name = "panelIfSteps";
            this.panelIfSteps.Size = new System.Drawing.Size(175, 459);
            this.panelIfSteps.TabIndex = 17;
            // 
            // ifStepsExchange
            // 
            this.ifStepsExchange.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsExchange.DataCellHeight = 29;
            this.ifStepsExchange.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsExchange.DeviceTagInfo")));
            this.ifStepsExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsExchange.DownstreamName = "PLC";
            this.ifStepsExchange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsExchange.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsExchange.Location = new System.Drawing.Point(0, 0);
            this.ifStepsExchange.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsExchange.Name = "ifStepsExchange";
            this.ifStepsExchange.Size = new System.Drawing.Size(175, 459);
            this.ifStepsExchange.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsExchange.StepMaxNo = 14;
            this.ifStepsExchange.StepName = new string[] {
        "Motion Pattern Strobe",
        "Motion Pattern Recept Comp",
        "Pattern Check Comp",
        "Cube Enterance Prohibition",
        "Cube Interference",
        "Exchange Begin",
        "GET Operation Begin",
        "GET Operation Comp",
        "GET Operation Comp Recept",
        "PUT Operation Begin",
        "PUT Operation Comp",
        "Exchange Comp",
        "PUT Operation Comp Recept",
        "Exchange Comp Recept"};
            this.ifStepsExchange.TabIndex = 9;
            this.ifStepsExchange.Title = "EXCHANGE";
            this.ifStepsExchange.UpstreamName = "NX100";
            // 
            // ifStepsHome
            // 
            this.ifStepsHome.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsHome.DataCellHeight = 35;
            this.ifStepsHome.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsHome.DeviceTagInfo")));
            this.ifStepsHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsHome.DownstreamName = "PLC";
            this.ifStepsHome.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsHome.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsHome.Location = new System.Drawing.Point(0, 0);
            this.ifStepsHome.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsHome.Name = "ifStepsHome";
            this.ifStepsHome.Size = new System.Drawing.Size(175, 459);
            this.ifStepsHome.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left};
            this.ifStepsHome.StepMaxNo = 9;
            this.ifStepsHome.StepName = new string[] {
        "External Servo On",
        "Servo is On",
        "Call Master Job",
        "Home Return Request",
        "Top of Master Job",
        "External Start",
        "Running",
        "Moving Home Begin",
        "Moving Home Comp"};
            this.ifStepsHome.TabIndex = 9;
            this.ifStepsHome.Title = "HOME";
            this.ifStepsHome.UpstreamName = "NX100";
            this.ifStepsHome.Visible = false;
            // 
            // ifStepsPutPin
            // 
            this.ifStepsPutPin.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsPutPin.DataCellHeight = 35;
            this.ifStepsPutPin.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsPutPin.DeviceTagInfo")));
            this.ifStepsPutPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsPutPin.DownstreamName = "PLC";
            this.ifStepsPutPin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsPutPin.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsPutPin.Location = new System.Drawing.Point(0, 0);
            this.ifStepsPutPin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsPutPin.Name = "ifStepsPutPin";
            this.ifStepsPutPin.Size = new System.Drawing.Size(175, 459);
            this.ifStepsPutPin.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsPutPin.StepMaxNo = 10;
            this.ifStepsPutPin.StepName = new string[] {
        "Motion Pattern Strobe",
        "Motion Pattern Recept Comp",
        "Pattern Check Comp",
        "Cube Enterance Prohibition",
        "Cube Interference",
        "PUT Operation Begin",
        "Delivery Prepare Comp",
        "Delivery Completion",
        "PUT Operation Comp",
        "PUT Operation Comp Recept"};
            this.ifStepsPutPin.TabIndex = 9;
            this.ifStepsPutPin.Title = "PUT (PIN Up/Down)";
            this.ifStepsPutPin.UpstreamName = "NX100";
            this.ifStepsPutPin.Visible = false;
            // 
            // ifStepsPut
            // 
            this.ifStepsPut.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsPut.DataCellHeight = 35;
            this.ifStepsPut.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsPut.DeviceTagInfo")));
            this.ifStepsPut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsPut.DownstreamName = "PLC";
            this.ifStepsPut.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsPut.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsPut.Location = new System.Drawing.Point(0, 0);
            this.ifStepsPut.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsPut.Name = "ifStepsPut";
            this.ifStepsPut.Size = new System.Drawing.Size(175, 459);
            this.ifStepsPut.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsPut.StepMaxNo = 8;
            this.ifStepsPut.StepName = new string[] {
        "Motion Pattern Strobe",
        "Motion Pattern Recept Comp",
        "Pattern Check Comp",
        "Cube Enterance Prohibition",
        "Cube Interference",
        "PUT Operation Begin",
        "PUT Operation Comp",
        "PUT Operation Comp Recept"};
            this.ifStepsPut.TabIndex = 9;
            this.ifStepsPut.Title = "PUT";
            this.ifStepsPut.UpstreamName = "NX100";
            this.ifStepsPut.Visible = false;
            // 
            // ifStepsGetPin
            // 
            this.ifStepsGetPin.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsGetPin.DataCellHeight = 35;
            this.ifStepsGetPin.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsGetPin.DeviceTagInfo")));
            this.ifStepsGetPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsGetPin.DownstreamName = "PLC";
            this.ifStepsGetPin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsGetPin.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsGetPin.Location = new System.Drawing.Point(0, 0);
            this.ifStepsGetPin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsGetPin.Name = "ifStepsGetPin";
            this.ifStepsGetPin.Size = new System.Drawing.Size(175, 459);
            this.ifStepsGetPin.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsGetPin.StepMaxNo = 10;
            this.ifStepsGetPin.StepName = new string[] {
        "Motion Pattern Strobe",
        "Motion Pattern Recept Comp",
        "Pattern Check Comp",
        "Cube Enterance Prohibition",
        "Cube Interference",
        "GET Operation Begin",
        "Delivery Prepare Comp",
        "Delivery Completion",
        "GET Operation Comp",
        "GET Operation Comp Recept"};
            this.ifStepsGetPin.TabIndex = 9;
            this.ifStepsGetPin.Title = "GET (PIN Up/Down)";
            this.ifStepsGetPin.UpstreamName = "NX100";
            this.ifStepsGetPin.Visible = false;
            // 
            // ifStepsGet
            // 
            this.ifStepsGet.BackColor = System.Drawing.Color.Transparent;
            this.ifStepsGet.DataCellHeight = 35;
            this.ifStepsGet.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("ifStepsGet.DeviceTagInfo")));
            this.ifStepsGet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ifStepsGet.DownstreamName = "PLC";
            this.ifStepsGet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ifStepsGet.GridContentAlignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            this.ifStepsGet.Location = new System.Drawing.Point(0, 0);
            this.ifStepsGet.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ifStepsGet.Name = "ifStepsGet";
            this.ifStepsGet.Size = new System.Drawing.Size(175, 459);
            this.ifStepsGet.StepDriection = new Dms.Control.IfSteps.Direction[] {
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Right,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Left,
        Dms.Control.IfSteps.Direction.Right};
            this.ifStepsGet.StepMaxNo = 8;
            this.ifStepsGet.StepName = new string[] {
        "Motion Pattern Strobe",
        "Motion Pattern Recept Comp",
        "Pattern Check Comp",
        "Cube Enterance Prohibition",
        "Cube Interference",
        "GET Operation Begin",
        "GET Operation Comp",
        "GET Operation Comp Recept"};
            this.ifStepsGet.TabIndex = 9;
            this.ifStepsGet.Title = "GET";
            this.ifStepsGet.UpstreamName = "NX100";
            this.ifStepsGet.Visible = false;
            // 
            // panelGroupGrid
            // 
            this.panelGroupGrid.Controls.Add(this.chkCubePassable);
            this.panelGroupGrid.Controls.Add(this.chkCubeProhibition);
            this.panelGroupGrid.Controls.Add(this.chkCubeInterfere);
            this.panelGroupGrid.Controls.Add(this.panelIfSteps);
            this.panelGroupGrid.Controls.Add(this.panelIoStatusNx);
            this.panelGroupGrid.Controls.Add(this.panelIoStatusPlc);
            this.panelGroupGrid.Location = new System.Drawing.Point(401, 3);
            this.panelGroupGrid.Name = "panelGroupGrid";
            this.panelGroupGrid.Size = new System.Drawing.Size(501, 462);
            this.panelGroupGrid.TabIndex = 18;
            // 
            // chkCubePassable
            // 
            this.chkCubePassable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCubePassable.BackColor = System.Drawing.Color.White;
            this.chkCubePassable.Enabled = false;
            this.chkCubePassable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkCubePassable.Location = new System.Drawing.Point(338, 436);
            this.chkCubePassable.Name = "chkCubePassable";
            this.chkCubePassable.Size = new System.Drawing.Size(160, 23);
            this.chkCubePassable.TabIndex = 23;
            this.chkCubePassable.Text = "Passable CUBE";
            this.chkCubePassable.UseVisualStyleBackColor = false;
            // 
            // chkCubeProhibition
            // 
            this.chkCubeProhibition.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCubeProhibition.BackColor = System.Drawing.Color.White;
            this.chkCubeProhibition.Enabled = false;
            this.chkCubeProhibition.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkCubeProhibition.Location = new System.Drawing.Point(338, 412);
            this.chkCubeProhibition.Name = "chkCubeProhibition";
            this.chkCubeProhibition.Size = new System.Drawing.Size(160, 23);
            this.chkCubeProhibition.TabIndex = 22;
            this.chkCubeProhibition.Text = "Prohibition CUBE";
            this.chkCubeProhibition.UseVisualStyleBackColor = false;
            // 
            // chkCubeInterfere
            // 
            this.chkCubeInterfere.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCubeInterfere.BackColor = System.Drawing.Color.White;
            this.chkCubeInterfere.Enabled = false;
            this.chkCubeInterfere.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkCubeInterfere.Location = new System.Drawing.Point(1, 436);
            this.chkCubeInterfere.Name = "chkCubeInterfere";
            this.chkCubeInterfere.Size = new System.Drawing.Size(160, 23);
            this.chkCubeInterfere.TabIndex = 21;
            this.chkCubeInterfere.Text = "Interference CUBE";
            this.chkCubeInterfere.UseVisualStyleBackColor = false;
            // 
            // panelIoStatusNx
            // 
            this.panelIoStatusNx.Controls.Add(this.viewDeviceIoNx);
            this.panelIoStatusNx.Location = new System.Drawing.Point(1, 0);
            this.panelIoStatusNx.Name = "panelIoStatusNx";
            this.panelIoStatusNx.Size = new System.Drawing.Size(160, 435);
            this.panelIoStatusNx.TabIndex = 15;
            // 
            // viewDeviceIoNx
            // 
            this.viewDeviceIoNx.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoNx.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoNx.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoNx.DeviceTagInfo")));
            this.viewDeviceIoNx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewDeviceIoNx.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoNx.GridBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewDeviceIoNx.IdVisible = false;
            this.viewDeviceIoNx.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoNx.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoNx.Location = new System.Drawing.Point(0, 0);
            this.viewDeviceIoNx.Name = "viewDeviceIoNx";
            this.viewDeviceIoNx.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoNx.OnColor = System.Drawing.Color.GreenYellow;
            this.viewDeviceIoNx.Size = new System.Drawing.Size(160, 435);
            this.viewDeviceIoNx.TabIndex = 15;
            this.viewDeviceIoNx.Title = "I/O STATUS [NX]";
            this.viewDeviceIoNx.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoNx.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoNx.TitleVisible = true;
            // 
            // viewDeviceIoStatus3
            // 
            this.viewDeviceIoStatus3.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoStatus3.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus3.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoStatus3.DeviceTagInfo")));
            this.viewDeviceIoStatus3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus3.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoStatus3.IdVisible = false;
            this.viewDeviceIoStatus3.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoStatus3.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoStatus3.Location = new System.Drawing.Point(266, 3);
            this.viewDeviceIoStatus3.Name = "viewDeviceIoStatus3";
            this.viewDeviceIoStatus3.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoStatus3.OnColor = System.Drawing.Color.GreenYellow;
            this.viewDeviceIoStatus3.Size = new System.Drawing.Size(132, 121);
            this.viewDeviceIoStatus3.TabIndex = 0;
            this.viewDeviceIoStatus3.Title = "";
            this.viewDeviceIoStatus3.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoStatus3.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus3.TitleVisible = true;
            // 
            // viewDeviceIoStatus2
            // 
            this.viewDeviceIoStatus2.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoStatus2.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoStatus2.DeviceTagInfo")));
            this.viewDeviceIoStatus2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus2.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoStatus2.IdVisible = false;
            this.viewDeviceIoStatus2.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoStatus2.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoStatus2.Location = new System.Drawing.Point(134, 3);
            this.viewDeviceIoStatus2.Name = "viewDeviceIoStatus2";
            this.viewDeviceIoStatus2.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoStatus2.OnColor = System.Drawing.Color.GreenYellow;
            this.viewDeviceIoStatus2.Size = new System.Drawing.Size(132, 121);
            this.viewDeviceIoStatus2.TabIndex = 0;
            this.viewDeviceIoStatus2.Title = "[UNKNOWN]";
            this.viewDeviceIoStatus2.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoStatus2.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus2.TitleVisible = true;
            // 
            // viewDeviceIoStatus1
            // 
            this.viewDeviceIoStatus1.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoStatus1.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoStatus1.DeviceTagInfo")));
            this.viewDeviceIoStatus1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus1.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoStatus1.IdVisible = false;
            this.viewDeviceIoStatus1.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoStatus1.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoStatus1.Location = new System.Drawing.Point(2, 3);
            this.viewDeviceIoStatus1.Name = "viewDeviceIoStatus1";
            this.viewDeviceIoStatus1.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoStatus1.OnColor = System.Drawing.Color.GreenYellow;
            this.viewDeviceIoStatus1.Size = new System.Drawing.Size(132, 121);
            this.viewDeviceIoStatus1.TabIndex = 0;
            this.viewDeviceIoStatus1.Title = "ROBOT STATUS";
            this.viewDeviceIoStatus1.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoStatus1.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoStatus1.TitleVisible = true;
            // 
            // viewDeviceIoAlarm3
            // 
            this.viewDeviceIoAlarm3.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoAlarm3.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm3.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoAlarm3.DeviceTagInfo")));
            this.viewDeviceIoAlarm3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm3.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoAlarm3.IdVisible = false;
            this.viewDeviceIoAlarm3.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoAlarm3.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoAlarm3.Location = new System.Drawing.Point(266, 121);
            this.viewDeviceIoAlarm3.Name = "viewDeviceIoAlarm3";
            this.viewDeviceIoAlarm3.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoAlarm3.OnColor = System.Drawing.Color.Red;
            this.viewDeviceIoAlarm3.Size = new System.Drawing.Size(132, 96);
            this.viewDeviceIoAlarm3.TabIndex = 0;
            this.viewDeviceIoAlarm3.Title = "ROBOT STATUS";
            this.viewDeviceIoAlarm3.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoAlarm3.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm3.TitleVisible = false;
            // 
            // viewDeviceIoAlarm2
            // 
            this.viewDeviceIoAlarm2.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoAlarm2.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoAlarm2.DeviceTagInfo")));
            this.viewDeviceIoAlarm2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm2.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoAlarm2.IdVisible = false;
            this.viewDeviceIoAlarm2.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoAlarm2.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoAlarm2.Location = new System.Drawing.Point(134, 121);
            this.viewDeviceIoAlarm2.Name = "viewDeviceIoAlarm2";
            this.viewDeviceIoAlarm2.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoAlarm2.OnColor = System.Drawing.Color.Red;
            this.viewDeviceIoAlarm2.Size = new System.Drawing.Size(132, 96);
            this.viewDeviceIoAlarm2.TabIndex = 0;
            this.viewDeviceIoAlarm2.Title = "ROBOT STATUS";
            this.viewDeviceIoAlarm2.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoAlarm2.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm2.TitleVisible = false;
            // 
            // viewDeviceIoAlarm1
            // 
            this.viewDeviceIoAlarm1.BackColor = System.Drawing.Color.Transparent;
            this.viewDeviceIoAlarm1.ContentsFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("viewDeviceIoAlarm1.DeviceTagInfo")));
            this.viewDeviceIoAlarm1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm1.GridBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewDeviceIoAlarm1.IdVisible = false;
            this.viewDeviceIoAlarm1.IoBind = Dms.Control.ViewDeviceIo.IoBindingMode.UserDefine;
            this.viewDeviceIoAlarm1.IoType = Dms.Common.IoType.DI;
            this.viewDeviceIoAlarm1.Location = new System.Drawing.Point(2, 121);
            this.viewDeviceIoAlarm1.Name = "viewDeviceIoAlarm1";
            this.viewDeviceIoAlarm1.OffColor = System.Drawing.Color.White;
            this.viewDeviceIoAlarm1.OnColor = System.Drawing.Color.Red;
            this.viewDeviceIoAlarm1.Size = new System.Drawing.Size(132, 96);
            this.viewDeviceIoAlarm1.TabIndex = 0;
            this.viewDeviceIoAlarm1.Title = "ROBOT STATUS";
            this.viewDeviceIoAlarm1.TitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewDeviceIoAlarm1.TitleTextFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewDeviceIoAlarm1.TitleVisible = false;
            // 
            // btnInitCommand
            // 
            this.btnInitCommand.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnInitCommand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInitCommand.Location = new System.Drawing.Point(97, 118);
            this.btnInitCommand.Name = "btnInitCommand";
            this.btnInitCommand.Size = new System.Drawing.Size(89, 43);
            this.btnInitCommand.TabIndex = 5;
            this.btnInitCommand.Text = "COMMAND INITIAL";
            this.btnInitCommand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnInitCommand.UseVisualStyleBackColor = true;
            this.btnInitCommand.CheckedChanged += new System.EventHandler(this.btnInitCommand_CheckedChanged);
            // 
            // ViewYaskawaNX100
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewDeviceIoStatus3);
            this.Controls.Add(this.btnMotionStrobe);
            this.Controls.Add(this.viewDeviceIoStatus2);
            this.Controls.Add(this.viewDeviceIoStatus1);
            this.Controls.Add(this.viewDeviceIoAlarm3);
            this.Controls.Add(this.viewDeviceIoAlarm2);
            this.Controls.Add(this.viewDeviceIoAlarm1);
            this.Controls.Add(this.groupBoxMotionCommand);
            this.Controls.Add(this.groupBoxExternalCommand);
            this.Controls.Add(this.panelGroupGrid);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewYaskawaNX100";
            this.Size = new System.Drawing.Size(903, 465);
            this.groupBoxExternalCommand.ResumeLayout(false);
            this.groupBoxMotionCommand.ResumeLayout(false);
            this.panelIoStatusPlc.ResumeLayout(false);
            this.panelIfSteps.ResumeLayout(false);
            this.panelGroupGrid.ResumeLayout(false);
            this.panelIoStatusNx.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBoxExternalCommand;
		private System.Windows.Forms.GroupBox groupBoxMotionCommand;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboSelectSlot;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboSelectHand;
		private System.Windows.Forms.Timer tmrUpdateState;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboSelectCube;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboOperation;
        private IfSteps ifStepsExchange;
        private IfSteps ifStepsPutPin;
        private IfSteps ifStepsGetPin;
        private IfSteps ifStepsPut;
        private IfSteps ifStepsGet;
        private IfSteps ifStepsHome;
        private System.Windows.Forms.Panel panelIoStatusPlc;
        private System.Windows.Forms.Panel panelIfSteps;
        private ViewDeviceIo viewDeviceIoAlarm1;
        private ViewDeviceIo viewDeviceIoAlarm2;
        private ViewDeviceIo viewDeviceIoAlarm3;
        private ViewDeviceIo viewDeviceIoStatus1;
        private ViewDeviceIo viewDeviceIoStatus2;
        private ViewDeviceIo viewDeviceIoStatus3;
        private System.Windows.Forms.Panel panelGroupGrid;
        private System.Windows.Forms.CheckBox chkCubePassable;
        private System.Windows.Forms.CheckBox chkCubeProhibition;
        private System.Windows.Forms.CheckBox chkCubeInterfere;
        private System.Windows.Forms.Panel panelIoStatusNx;
        private ViewDeviceIo viewDeviceIoNx;
        private ViewDeviceIo viewDeviceIoPlc;
        private System.Windows.Forms.CheckBox btnRobotHome;
        private System.Windows.Forms.CheckBox btnAlarmReset;
        private System.Windows.Forms.CheckBox btnRobotHold;
        private System.Windows.Forms.CheckBox btnServoOn;
        private System.Windows.Forms.CheckBox btnMotionStrobe;
        private System.Windows.Forms.CheckBox btnServoOff;
        private System.Windows.Forms.CheckBox btnInitCommand;
	}
}

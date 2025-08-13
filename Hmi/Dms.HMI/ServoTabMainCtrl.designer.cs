namespace Dms.HMI
{
    partial class ServoTabMainCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServoTabMainCtrl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewServo = new System.Windows.Forms.TreeView();
            this.txtRepeatWaitTime = new Dms.Common.ValidationTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonAllEstop = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.gbServoControl = new System.Windows.Forms.GroupBox();
            this.lblCommand = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxServoHome = new System.Windows.Forms.CheckBox();
            this.checkBoxServoOn = new System.Windows.Forms.CheckBox();
            this.checkBoxServoEstop = new System.Windows.Forms.CheckBox();
            this.btnEstop = new System.Windows.Forms.Button();
            this.btnServoOn = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtVelRatio = new Dms.Common.ValidationTextBox();
            this.trackBarVelRatio = new System.Windows.Forms.TrackBar();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentUnitName = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblCurrentPos = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.listTeachPoint = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbAxesControl = new System.Windows.Forms.GroupBox();
            this.btnRbpara = new System.Windows.Forms.Button();
            this.textBoxRadiusReq = new System.Windows.Forms.TextBox();
            this.textBoxHomeReq = new System.Windows.Forms.TextBox();
            this.labelRadius = new System.Windows.Forms.Label();
            this.labelHomeangle = new System.Windows.Forms.Label();
            this.textBoxHomeAngle = new System.Windows.Forms.TextBox();
            this.textBoxRadius = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtJogVel = new Dms.Common.ValidationTextBox();
            this.btnJogMinus = new System.Windows.Forms.Button();
            this.btnJogPlus = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarJogVel = new System.Windows.Forms.TrackBar();
            this.gridViewAxes = new Dms.Control.DoubleBufferedGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurVel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSelectedAxis = new System.Windows.Forms.Label();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupRadian = new System.Windows.Forms.GroupBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbServoControl.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelRatio)).BeginInit();
            this.gbAxesControl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJogVel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAxes)).BeginInit();
            this.groupRadian.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewServo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.splitContainer1.Panel2.Controls.Add(this.groupRadian);
            this.splitContainer1.Panel2.Controls.Add(this.txtRepeatWaitTime);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.buttonAllEstop);
            this.splitContainer1.Panel2.Controls.Add(this.lblMessage);
            this.splitContainer1.Panel2.Controls.Add(this.gbServoControl);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.gbAxesControl);
            this.splitContainer1.Size = new System.Drawing.Size(901, 537);
            this.splitContainer1.SplitterDistance = 123;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewServo
            // 
            this.treeViewServo.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewServo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewServo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewServo.Location = new System.Drawing.Point(0, 0);
            this.treeViewServo.Name = "treeViewServo";
            this.treeViewServo.Size = new System.Drawing.Size(123, 537);
            this.treeViewServo.TabIndex = 0;
            this.treeViewServo.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewServo_NodeMouseClick);
            // 
            // txtRepeatWaitTime
            // 
            this.txtRepeatWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRepeatWaitTime.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtRepeatWaitTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRepeatWaitTime.ForeColor = System.Drawing.Color.Blue;
            this.txtRepeatWaitTime.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtRepeatWaitTime.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtRepeatWaitTime.KeyPadInfo")));
            this.txtRepeatWaitTime.LimitHigh = "20";
            this.txtRepeatWaitTime.LimitLow = "0.5";
            this.txtRepeatWaitTime.Location = new System.Drawing.Point(675, 289);
            this.txtRepeatWaitTime.Name = "txtRepeatWaitTime";
            this.txtRepeatWaitTime.ReferenceTag = null;
            this.txtRepeatWaitTime.Size = new System.Drawing.Size(36, 21);
            this.txtRepeatWaitTime.TabIndex = 7;
            this.txtRepeatWaitTime.Text = "1.0";
            this.txtRepeatWaitTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRepeatWaitTime.UsedInKeyPad = false;
            this.txtRepeatWaitTime.TextChanged += new System.EventHandler(this.txtRepeatWaitTime_TextChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(8, 459);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 23);
            this.label6.TabIndex = 20;
            this.label6.Text = "Message";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonAllEstop
            // 
            this.buttonAllEstop.BackColor = System.Drawing.Color.Crimson;
            this.buttonAllEstop.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAllEstop.ForeColor = System.Drawing.Color.Yellow;
            this.buttonAllEstop.Location = new System.Drawing.Point(508, 463);
            this.buttonAllEstop.Name = "buttonAllEstop";
            this.buttonAllEstop.Size = new System.Drawing.Size(235, 66);
            this.buttonAllEstop.TabIndex = 11;
            this.buttonAllEstop.Text = "E-STOP  ALL";
            this.buttonAllEstop.UseVisualStyleBackColor = false;
            this.buttonAllEstop.Click += new System.EventHandler(this.buttonAllEstop_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(11, 483);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(483, 32);
            this.lblMessage.TabIndex = 19;
            this.lblMessage.Text = " Message from servo";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbServoControl
            // 
            this.gbServoControl.Controls.Add(this.lblCommand);
            this.gbServoControl.Controls.Add(this.label4);
            this.gbServoControl.Controls.Add(this.label2);
            this.gbServoControl.Controls.Add(this.groupBox2);
            this.gbServoControl.Controls.Add(this.groupBox1);
            this.gbServoControl.Controls.Add(this.lblCurrentUnitName);
            this.gbServoControl.Controls.Add(this.btnRead);
            this.gbServoControl.Controls.Add(this.btnSave);
            this.gbServoControl.Controls.Add(this.lblCurrentPos);
            this.gbServoControl.Controls.Add(this.btnSend);
            this.gbServoControl.Controls.Add(this.listTeachPoint);
            this.gbServoControl.Location = new System.Drawing.Point(4, 4);
            this.gbServoControl.Margin = new System.Windows.Forms.Padding(0);
            this.gbServoControl.Name = "gbServoControl";
            this.gbServoControl.Size = new System.Drawing.Size(752, 289);
            this.gbServoControl.TabIndex = 0;
            this.gbServoControl.TabStop = false;
            this.gbServoControl.Text = "ServoUnit Control";
            // 
            // lblCommand
            // 
            this.lblCommand.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCommand.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommand.ForeColor = System.Drawing.Color.Blue;
            this.lblCommand.Location = new System.Drawing.Point(308, 20);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(278, 27);
            this.lblCommand.TabIndex = 18;
            this.lblCommand.Text = " Command";
            this.lblCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(187, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "Manual Command";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(187, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Current Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxServoHome);
            this.groupBox2.Controls.Add(this.checkBoxServoOn);
            this.groupBox2.Controls.Add(this.checkBoxServoEstop);
            this.groupBox2.Controls.Add(this.btnEstop);
            this.groupBox2.Controls.Add(this.btnServoOn);
            this.groupBox2.Controls.Add(this.btnHome);
            this.groupBox2.Location = new System.Drawing.Point(597, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 141);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            // 
            // checkBoxServoHome
            // 
            this.checkBoxServoHome.AutoCheck = false;
            this.checkBoxServoHome.AutoSize = true;
            this.checkBoxServoHome.Location = new System.Drawing.Point(9, 108);
            this.checkBoxServoHome.Name = "checkBoxServoHome";
            this.checkBoxServoHome.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoHome.TabIndex = 13;
            this.checkBoxServoHome.UseVisualStyleBackColor = true;
            // 
            // checkBoxServoOn
            // 
            this.checkBoxServoOn.AutoCheck = false;
            this.checkBoxServoOn.AutoSize = true;
            this.checkBoxServoOn.Location = new System.Drawing.Point(9, 66);
            this.checkBoxServoOn.Name = "checkBoxServoOn";
            this.checkBoxServoOn.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoOn.TabIndex = 12;
            this.checkBoxServoOn.UseVisualStyleBackColor = true;
            // 
            // checkBoxServoEstop
            // 
            this.checkBoxServoEstop.AutoCheck = false;
            this.checkBoxServoEstop.AutoSize = true;
            this.checkBoxServoEstop.Location = new System.Drawing.Point(9, 24);
            this.checkBoxServoEstop.Name = "checkBoxServoEstop";
            this.checkBoxServoEstop.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoEstop.TabIndex = 11;
            this.checkBoxServoEstop.UseVisualStyleBackColor = true;
            // 
            // btnEstop
            // 
            this.btnEstop.BackColor = System.Drawing.Color.Pink;
            this.btnEstop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstop.Location = new System.Drawing.Point(25, 12);
            this.btnEstop.Name = "btnEstop";
            this.btnEstop.Size = new System.Drawing.Size(116, 38);
            this.btnEstop.TabIndex = 10;
            this.btnEstop.Text = "E-STOP";
            this.btnEstop.UseVisualStyleBackColor = false;
            this.btnEstop.Click += new System.EventHandler(this.btnEstop_Click);
            // 
            // btnServoOn
            // 
            this.btnServoOn.BackColor = System.Drawing.Color.LightCyan;
            this.btnServoOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServoOn.Location = new System.Drawing.Point(25, 54);
            this.btnServoOn.Name = "btnServoOn";
            this.btnServoOn.Size = new System.Drawing.Size(116, 38);
            this.btnServoOn.TabIndex = 8;
            this.btnServoOn.Text = "SERVO ON";
            this.btnServoOn.UseVisualStyleBackColor = false;
            this.btnServoOn.Click += new System.EventHandler(this.btnServoOn_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnHome.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(25, 96);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(116, 38);
            this.btnHome.TabIndex = 9;
            this.btnHome.Text = "HOME";
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVelRatio);
            this.groupBox1.Controls.Add(this.trackBarVelRatio);
            this.groupBox1.Controls.Add(this.btnMove);
            this.groupBox1.Controls.Add(this.btnRepeat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(597, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 135);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // txtVelRatio
            // 
            this.txtVelRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVelRatio.DataFormat = Dms.Common.OptionFormat.Digit;
            this.txtVelRatio.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVelRatio.ForeColor = System.Drawing.Color.Blue;
            this.txtVelRatio.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtVelRatio.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtVelRatio.KeyPadInfo")));
            this.txtVelRatio.LimitHigh = "100";
            this.txtVelRatio.LimitLow = "10";
            this.txtVelRatio.Location = new System.Drawing.Point(7, 12);
            this.txtVelRatio.Name = "txtVelRatio";
            this.txtVelRatio.ReferenceTag = null;
            this.txtVelRatio.Size = new System.Drawing.Size(36, 21);
            this.txtVelRatio.TabIndex = 3;
            this.txtVelRatio.Text = "100";
            this.txtVelRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelRatio.UsedInKeyPad = false;
            this.txtVelRatio.TextChanged += new System.EventHandler(this.txtVelRatio_TextChanged);
            // 
            // trackBarVelRatio
            // 
            this.trackBarVelRatio.AutoSize = false;
            this.trackBarVelRatio.LargeChange = 1;
            this.trackBarVelRatio.Location = new System.Drawing.Point(14, 36);
            this.trackBarVelRatio.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarVelRatio.Maximum = 100;
            this.trackBarVelRatio.Minimum = 10;
            this.trackBarVelRatio.Name = "trackBarVelRatio";
            this.trackBarVelRatio.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarVelRatio.Size = new System.Drawing.Size(24, 92);
            this.trackBarVelRatio.TabIndex = 2;
            this.trackBarVelRatio.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarVelRatio.Value = 100;
            this.trackBarVelRatio.Scroll += new System.EventHandler(this.trackBarVelRatio_Scroll);
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.LightCyan;
            this.btnMove.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMove.Location = new System.Drawing.Point(47, 37);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(95, 44);
            this.btnMove.TabIndex = 4;
            this.btnMove.Text = "MOVE";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnRepeat
            // 
            this.btnRepeat.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnRepeat.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepeat.Location = new System.Drawing.Point(47, 85);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(95, 44);
            this.btnRepeat.TabIndex = 5;
            this.btnRepeat.Text = "REPEAT";
            this.btnRepeat.UseVisualStyleBackColor = false;
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(47, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "Vel Ratio(%)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentUnitName
            // 
            this.lblCurrentUnitName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCurrentUnitName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentUnitName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentUnitName.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrentUnitName.Location = new System.Drawing.Point(6, 20);
            this.lblCurrentUnitName.Name = "lblCurrentUnitName";
            this.lblCurrentUnitName.Size = new System.Drawing.Size(177, 56);
            this.lblCurrentUnitName.TabIndex = 0;
            this.lblCurrentUnitName.Text = "ServoUnit Name";
            this.lblCurrentUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnRead.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRead.Location = new System.Drawing.Point(527, 88);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(53, 30);
            this.btnRead.TabIndex = 12;
            this.btnRead.Text = "READ";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightCyan;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(470, 88);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCurrentPos
            // 
            this.lblCurrentPos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCurrentPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentPos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPos.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrentPos.Location = new System.Drawing.Point(308, 50);
            this.lblCurrentPos.Name = "lblCurrentPos";
            this.lblCurrentPos.Size = new System.Drawing.Size(278, 26);
            this.lblCurrentPos.TabIndex = 13;
            this.lblCurrentPos.Text = " Current Position";
            this.lblCurrentPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.Pink;
            this.btnSend.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(413, 88);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(53, 30);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "SET";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listTeachPoint
            // 
            this.listTeachPoint.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listTeachPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listTeachPoint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTeachPoint.ForeColor = System.Drawing.Color.Blue;
            this.listTeachPoint.FullRowSelect = true;
            this.listTeachPoint.GridLines = true;
            this.listTeachPoint.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listTeachPoint.Location = new System.Drawing.Point(6, 84);
            this.listTeachPoint.MultiSelect = false;
            this.listTeachPoint.Name = "listTeachPoint";
            this.listTeachPoint.Size = new System.Drawing.Size(580, 199);
            this.listTeachPoint.TabIndex = 1;
            this.listTeachPoint.UseCompatibleStateImageBehavior = false;
            this.listTeachPoint.View = System.Windows.Forms.View.Details;
            this.listTeachPoint.Click += new System.EventHandler(this.listTeachPoint_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(639, 293);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Wait";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(717, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Sec";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbAxesControl
            // 
            this.gbAxesControl.Controls.Add(this.groupBox3);
            this.gbAxesControl.Controls.Add(this.gridViewAxes);
            this.gbAxesControl.Controls.Add(this.lblSelectedAxis);
            this.gbAxesControl.Location = new System.Drawing.Point(4, 302);
            this.gbAxesControl.Margin = new System.Windows.Forms.Padding(0);
            this.gbAxesControl.Name = "gbAxesControl";
            this.gbAxesControl.Size = new System.Drawing.Size(752, 154);
            this.gbAxesControl.TabIndex = 1;
            this.gbAxesControl.TabStop = false;
            this.gbAxesControl.Text = "Axes Control";
            // 
            // btnRbpara
            // 
            this.btnRbpara.BackColor = System.Drawing.Color.Pink;
            this.btnRbpara.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRbpara.Location = new System.Drawing.Point(282, 16);
            this.btnRbpara.Name = "btnRbpara";
            this.btnRbpara.Size = new System.Drawing.Size(42, 30);
            this.btnRbpara.TabIndex = 19;
            this.btnRbpara.Text = "SET";
            this.btnRbpara.UseVisualStyleBackColor = false;
            this.btnRbpara.Click += new System.EventHandler(this.HomeAngle_Radius_Set_click);
            // 
            // textBoxRadiusReq
            // 
            this.textBoxRadiusReq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRadiusReq.ForeColor = System.Drawing.Color.Blue;
            this.textBoxRadiusReq.Location = new System.Drawing.Point(194, 33);
            this.textBoxRadiusReq.Name = "textBoxRadiusReq";
            this.textBoxRadiusReq.Size = new System.Drawing.Size(82, 21);
            this.textBoxRadiusReq.TabIndex = 26;
            // 
            // textBoxHomeReq
            // 
            this.textBoxHomeReq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHomeReq.ForeColor = System.Drawing.Color.Blue;
            this.textBoxHomeReq.Location = new System.Drawing.Point(46, 33);
            this.textBoxHomeReq.Name = "textBoxHomeReq";
            this.textBoxHomeReq.Size = new System.Drawing.Size(82, 21);
            this.textBoxHomeReq.TabIndex = 25;
            // 
            // labelRadius
            // 
            this.labelRadius.BackColor = System.Drawing.Color.Transparent;
            this.labelRadius.Location = new System.Drawing.Point(134, 23);
            this.labelRadius.Name = "labelRadius";
            this.labelRadius.Size = new System.Drawing.Size(50, 23);
            this.labelRadius.TabIndex = 24;
            this.labelRadius.Text = "Radius";
            this.labelRadius.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHomeangle
            // 
            this.labelHomeangle.BackColor = System.Drawing.Color.Transparent;
            this.labelHomeangle.Location = new System.Drawing.Point(6, 23);
            this.labelHomeangle.Name = "labelHomeangle";
            this.labelHomeangle.Size = new System.Drawing.Size(39, 23);
            this.labelHomeangle.TabIndex = 23;
            this.labelHomeangle.Text = "Angle";
            this.labelHomeangle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHomeAngle
            // 
            this.textBoxHomeAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHomeAngle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHomeAngle.ForeColor = System.Drawing.Color.Black;
            this.textBoxHomeAngle.Location = new System.Drawing.Point(46, 13);
            this.textBoxHomeAngle.Name = "textBoxHomeAngle";
            this.textBoxHomeAngle.Size = new System.Drawing.Size(82, 21);
            this.textBoxHomeAngle.TabIndex = 22;
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRadius.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRadius.ForeColor = System.Drawing.Color.Black;
            this.textBoxRadius.Location = new System.Drawing.Point(194, 13);
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.Size = new System.Drawing.Size(82, 21);
            this.textBoxRadius.TabIndex = 21;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtJogVel);
            this.groupBox3.Controls.Add(this.btnJogMinus);
            this.groupBox3.Controls.Add(this.btnJogPlus);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.trackBarJogVel);
            this.groupBox3.Location = new System.Drawing.Point(597, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(149, 135);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            // 
            // txtJogVel
            // 
            this.txtJogVel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtJogVel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJogVel.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtJogVel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJogVel.ForeColor = System.Drawing.Color.Blue;
            this.txtJogVel.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtJogVel.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtJogVel.KeyPadInfo")));
            this.txtJogVel.LimitHigh = "100.0";
            this.txtJogVel.LimitLow = "0.1";
            this.txtJogVel.Location = new System.Drawing.Point(6, 12);
            this.txtJogVel.Name = "txtJogVel";
            this.txtJogVel.ReferenceTag = null;
            this.txtJogVel.Size = new System.Drawing.Size(36, 21);
            this.txtJogVel.TabIndex = 15;
            this.txtJogVel.Text = "1.0";
            this.txtJogVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtJogVel.UsedInKeyPad = false;
            this.txtJogVel.TextChanged += new System.EventHandler(this.txtJogVel_TextChanged);
            // 
            // btnJogMinus
            // 
            this.btnJogMinus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnJogMinus.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnJogMinus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJogMinus.Location = new System.Drawing.Point(45, 86);
            this.btnJogMinus.Name = "btnJogMinus";
            this.btnJogMinus.Size = new System.Drawing.Size(95, 44);
            this.btnJogMinus.TabIndex = 17;
            this.btnJogMinus.Text = "JOG (-)";
            this.btnJogMinus.UseVisualStyleBackColor = false;
            this.btnJogMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseDown);
            this.btnJogMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseUp);
            // 
            // btnJogPlus
            // 
            this.btnJogPlus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnJogPlus.BackColor = System.Drawing.Color.LightCyan;
            this.btnJogPlus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJogPlus.Location = new System.Drawing.Point(45, 36);
            this.btnJogPlus.Name = "btnJogPlus";
            this.btnJogPlus.Size = new System.Drawing.Size(95, 44);
            this.btnJogPlus.TabIndex = 16;
            this.btnJogPlus.Text = "JOG (+)";
            this.btnJogPlus.UseVisualStyleBackColor = false;
            this.btnJogPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseDown);
            this.btnJogPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseUp);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(45, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 21);
            this.label3.TabIndex = 18;
            this.label3.Text = "Vel (mm/s)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarJogVel
            // 
            this.trackBarJogVel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trackBarJogVel.AutoSize = false;
            this.trackBarJogVel.LargeChange = 1;
            this.trackBarJogVel.Location = new System.Drawing.Point(12, 37);
            this.trackBarJogVel.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarJogVel.Maximum = 100;
            this.trackBarJogVel.Minimum = 1;
            this.trackBarJogVel.Name = "trackBarJogVel";
            this.trackBarJogVel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarJogVel.Size = new System.Drawing.Size(24, 92);
            this.trackBarJogVel.TabIndex = 14;
            this.trackBarJogVel.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarJogVel.Value = 1;
            this.trackBarJogVel.Scroll += new System.EventHandler(this.trackBarJogVel_Scroll);
            // 
            // gridViewAxes
            // 
            this.gridViewAxes.AllowUserToAddRows = false;
            this.gridViewAxes.AllowUserToDeleteRows = false;
            this.gridViewAxes.AllowUserToResizeRows = false;
            this.gridViewAxes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridViewAxes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.gridViewAxes.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViewAxes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridViewAxes.ColumnHeadersHeight = 35;
            this.gridViewAxes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridViewAxes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colName,
            this.colSetPos,
            this.colCurPos,
            this.colCurVel,
            this.colCurAcc,
            this.colNeg,
            this.colHome,
            this.colPos,
            this.colEvent,
            this.colSource});
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViewAxes.DefaultCellStyle = dataGridViewCellStyle13;
            this.gridViewAxes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.gridViewAxes.Location = new System.Drawing.Point(7, 54);
            this.gridViewAxes.Name = "gridViewAxes";
            this.gridViewAxes.RowHeadersVisible = false;
            this.gridViewAxes.RowTemplate.Height = 23;
            this.gridViewAxes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridViewAxes.Size = new System.Drawing.Size(579, 92);
            this.gridViewAxes.TabIndex = 14;
            this.gridViewAxes.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridViewAxes_CellBeginEdit);
            this.gridViewAxes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViewAxes_CellDoubleClick);
            this.gridViewAxes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViewAxes_CellEndEdit);
            this.gridViewAxes.SelectionChanged += new System.EventHandler(this.gridViewAxes_SelectionChanged);
            // 
            // colNo
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNo.FillWeight = 42.09531F;
            this.colNo.HeaderText = "No";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colName.FillWeight = 129.3955F;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colSetPos
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSetPos.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSetPos.FillWeight = 129.3955F;
            this.colSetPos.HeaderText = "Set Pos";
            this.colSetPos.Name = "colSetPos";
            this.colSetPos.ReadOnly = true;
            this.colSetPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCurPos
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCurPos.DefaultCellStyle = dataGridViewCellStyle5;
            this.colCurPos.FillWeight = 129.3955F;
            this.colCurPos.HeaderText = "Cur Pos";
            this.colCurPos.Name = "colCurPos";
            this.colCurPos.ReadOnly = true;
            this.colCurPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCurVel
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCurVel.DefaultCellStyle = dataGridViewCellStyle6;
            this.colCurVel.FillWeight = 76.7246F;
            this.colCurVel.HeaderText = "Cur Vel";
            this.colCurVel.Name = "colCurVel";
            this.colCurVel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCurAcc
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCurAcc.DefaultCellStyle = dataGridViewCellStyle7;
            this.colCurAcc.FillWeight = 77.00943F;
            this.colCurAcc.HeaderText = "Cur Acc";
            this.colCurAcc.Name = "colCurAcc";
            this.colCurAcc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colNeg
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNeg.DefaultCellStyle = dataGridViewCellStyle8;
            this.colNeg.FillWeight = 50.56443F;
            this.colNeg.HeaderText = "-";
            this.colNeg.Name = "colNeg";
            this.colNeg.ReadOnly = true;
            this.colNeg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colHome
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHome.DefaultCellStyle = dataGridViewCellStyle9;
            this.colHome.FillWeight = 50.4857F;
            this.colHome.HeaderText = "H";
            this.colHome.Name = "colHome";
            this.colHome.ReadOnly = true;
            this.colHome.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPos
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPos.DefaultCellStyle = dataGridViewCellStyle10;
            this.colPos.FillWeight = 50.71113F;
            this.colPos.HeaderText = "+";
            this.colPos.Name = "colPos";
            this.colPos.ReadOnly = true;
            this.colPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEvent
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEvent.DefaultCellStyle = dataGridViewCellStyle11;
            this.colEvent.FillWeight = 129.3955F;
            this.colEvent.HeaderText = "Event";
            this.colEvent.Name = "colEvent";
            this.colEvent.ReadOnly = true;
            this.colEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colSource
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSource.DefaultCellStyle = dataGridViewCellStyle12;
            this.colSource.FillWeight = 129.3955F;
            this.colSource.HeaderText = "Source";
            this.colSource.Name = "colSource";
            this.colSource.ReadOnly = true;
            this.colSource.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lblSelectedAxis
            // 
            this.lblSelectedAxis.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSelectedAxis.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSelectedAxis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedAxis.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedAxis.Location = new System.Drawing.Point(7, 18);
            this.lblSelectedAxis.Name = "lblSelectedAxis";
            this.lblSelectedAxis.Size = new System.Drawing.Size(177, 31);
            this.lblSelectedAxis.TabIndex = 19;
            this.lblSelectedAxis.Text = "Current Axis";
            this.lblSelectedAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 500;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // groupRadian
            // 
            this.groupRadian.Controls.Add(this.btnRbpara);
            this.groupRadian.Controls.Add(this.textBoxHomeAngle);
            this.groupRadian.Controls.Add(this.textBoxRadiusReq);
            this.groupRadian.Controls.Add(this.textBoxHomeReq);
            this.groupRadian.Controls.Add(this.textBoxRadius);
            this.groupRadian.Controls.Add(this.labelRadius);
            this.groupRadian.Controls.Add(this.labelHomeangle);
            this.groupRadian.Location = new System.Drawing.Point(243, 294);
            this.groupRadian.Name = "groupRadian";
            this.groupRadian.Size = new System.Drawing.Size(347, 60);
            this.groupRadian.TabIndex = 19;
            this.groupRadian.TabStop = false;
            this.groupRadian.Text = "Radian Setting";
            // 
            // ServoTabMainCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServoTabMainCtrl";
            this.Size = new System.Drawing.Size(901, 537);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.gbServoControl.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelRatio)).EndInit();
            this.gbAxesControl.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJogVel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAxes)).EndInit();
            this.groupRadian.ResumeLayout(false);
            this.groupRadian.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewServo;
        private System.Windows.Forms.GroupBox gbServoControl;
        private System.Windows.Forms.Label lblCurrentUnitName;
        private System.Windows.Forms.ListView listTeachPoint;
        private System.Windows.Forms.TrackBar trackBarVelRatio;
        private Dms.Common.ValidationTextBox txtVelRatio;
        private System.Windows.Forms.Button btnRepeat;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEstop;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnServoOn;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblCurrentPos;
        private System.Windows.Forms.GroupBox gbAxesControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnJogMinus;
        private System.Windows.Forms.Button btnJogPlus;
        private Dms.Common.ValidationTextBox txtJogVel;
        private System.Windows.Forms.TrackBar trackBarJogVel;
        private System.Windows.Forms.Label lblSelectedAxis;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurVel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHome;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Dms.Control.DoubleBufferedGridView gridViewAxes;
        private System.Windows.Forms.CheckBox checkBoxServoEstop;
        private System.Windows.Forms.CheckBox checkBoxServoHome;
        private System.Windows.Forms.CheckBox checkBoxServoOn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAllEstop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMessage;
        private Dms.Common.ValidationTextBox txtRepeatWaitTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxHomeAngle;
        private System.Windows.Forms.TextBox textBoxRadius;
        private System.Windows.Forms.Label labelRadius;
        private System.Windows.Forms.Label labelHomeangle;
        private System.Windows.Forms.TextBox textBoxRadiusReq;
        private System.Windows.Forms.TextBox textBoxHomeReq;
        private System.Windows.Forms.Button btnRbpara;
        private System.Windows.Forms.GroupBox groupRadian;
    }
}

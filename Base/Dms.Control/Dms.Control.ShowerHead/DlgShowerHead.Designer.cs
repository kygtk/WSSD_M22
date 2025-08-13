namespace Dms.Control
{
    partial class DlgShowerHead
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgShowerHead));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxServoHome = new System.Windows.Forms.CheckBox();
            this.btnEstop = new System.Windows.Forms.Button();
            this.checkBoxServoEstop = new System.Windows.Forms.CheckBox();
            this.btnServoOn = new System.Windows.Forms.Button();
            this.checkBoxServoOn = new System.Windows.Forms.CheckBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.txtVelRatio = new Dms.Common.ValidationTextBox();
            this.trackBarVelRatio = new System.Windows.Forms.TrackBar();
            this.btnMove = new System.Windows.Forms.Button();
            this.lblVelRatio = new System.Windows.Forms.Label();
            this.lblCommand = new System.Windows.Forms.Label();
            this.lblCurPos = new System.Windows.Forms.Label();
            this.lblEvent = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSetPos = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPosSwitch = new System.Windows.Forms.Label();
            this.lblHomeSwitch = new System.Windows.Forms.Label();
            this.lblNegSwitch = new System.Windows.Forms.Label();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.listTeachPoint = new System.Windows.Forms.ListView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtJogVel = new Dms.Common.ValidationTextBox();
            this.btnJogMinus = new System.Windows.Forms.Button();
            this.btnJogPlus = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarJogVel = new System.Windows.Forms.TrackBar();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelRatio)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJogVel)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxServoHome);
            this.groupBox2.Controls.Add(this.btnEstop);
            this.groupBox2.Controls.Add(this.checkBoxServoEstop);
            this.groupBox2.Controls.Add(this.btnServoOn);
            this.groupBox2.Controls.Add(this.checkBoxServoOn);
            this.groupBox2.Controls.Add(this.btnHome);
            this.groupBox2.Location = new System.Drawing.Point(3, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(366, 62);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // checkBoxServoHome
            // 
            this.checkBoxServoHome.AutoCheck = false;
            this.checkBoxServoHome.AutoSize = true;
            this.checkBoxServoHome.Location = new System.Drawing.Point(244, 26);
            this.checkBoxServoHome.Name = "checkBoxServoHome";
            this.checkBoxServoHome.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoHome.TabIndex = 13;
            this.checkBoxServoHome.UseVisualStyleBackColor = true;
            // 
            // btnEstop
            // 
            this.btnEstop.BackColor = System.Drawing.Color.Pink;
            this.btnEstop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstop.Location = new System.Drawing.Point(25, 12);
            this.btnEstop.Name = "btnEstop";
            this.btnEstop.Size = new System.Drawing.Size(95, 44);
            this.btnEstop.TabIndex = 10;
            this.btnEstop.Text = "E-STOP";
            this.btnEstop.UseVisualStyleBackColor = false;
            this.btnEstop.Click += new System.EventHandler(this.btnEstop_Click);
            // 
            // checkBoxServoEstop
            // 
            this.checkBoxServoEstop.AutoCheck = false;
            this.checkBoxServoEstop.AutoSize = true;
            this.checkBoxServoEstop.Location = new System.Drawing.Point(6, 26);
            this.checkBoxServoEstop.Name = "checkBoxServoEstop";
            this.checkBoxServoEstop.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoEstop.TabIndex = 11;
            this.checkBoxServoEstop.UseVisualStyleBackColor = true;
            // 
            // btnServoOn
            // 
            this.btnServoOn.BackColor = System.Drawing.Color.LightCyan;
            this.btnServoOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServoOn.Location = new System.Drawing.Point(144, 12);
            this.btnServoOn.Name = "btnServoOn";
            this.btnServoOn.Size = new System.Drawing.Size(95, 44);
            this.btnServoOn.TabIndex = 8;
            this.btnServoOn.Text = "SERVO ON";
            this.btnServoOn.UseVisualStyleBackColor = false;
            this.btnServoOn.Click += new System.EventHandler(this.btnServoOn_Click);
            // 
            // checkBoxServoOn
            // 
            this.checkBoxServoOn.AutoCheck = false;
            this.checkBoxServoOn.AutoSize = true;
            this.checkBoxServoOn.Location = new System.Drawing.Point(125, 26);
            this.checkBoxServoOn.Name = "checkBoxServoOn";
            this.checkBoxServoOn.Size = new System.Drawing.Size(15, 14);
            this.checkBoxServoOn.TabIndex = 12;
            this.checkBoxServoOn.UseVisualStyleBackColor = true;
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.DarkKhaki;
            this.btnHome.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(265, 12);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(95, 44);
            this.btnHome.TabIndex = 9;
            this.btnHome.Text = "HOME";
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRepeat);
            this.groupBox1.Controls.Add(this.txtVelRatio);
            this.groupBox1.Controls.Add(this.trackBarVelRatio);
            this.groupBox1.Controls.Add(this.btnMove);
            this.groupBox1.Controls.Add(this.lblVelRatio);
            this.groupBox1.Location = new System.Drawing.Point(223, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 135);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // btnRepeat
            // 
            this.btnRepeat.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnRepeat.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepeat.Location = new System.Drawing.Point(45, 85);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(95, 44);
            this.btnRepeat.TabIndex = 26;
            this.btnRepeat.Text = "REPEAT";
            this.btnRepeat.UseVisualStyleBackColor = false;
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            // 
            // txtVelRatio
            // 
            this.txtVelRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVelRatio.DataFormat = Dms.Common.OptionFormat.None;
            this.txtVelRatio.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVelRatio.ForeColor = System.Drawing.Color.Blue;
            this.txtVelRatio.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtVelRatio.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtVelRatio.KeyPadInfo")));
            this.txtVelRatio.LimitHigh = "100";
            this.txtVelRatio.LimitLow = "10";
            this.txtVelRatio.Location = new System.Drawing.Point(6, 12);
            this.txtVelRatio.Name = "txtVelRatio";
            this.txtVelRatio.Size = new System.Drawing.Size(36, 21);
            this.txtVelRatio.TabIndex = 18;
            this.txtVelRatio.Text = "100";
            this.txtVelRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelRatio.UsedInKeyPad = false;
            // 
            // trackBarVelRatio
            // 
            this.trackBarVelRatio.AutoSize = false;
            this.trackBarVelRatio.LargeChange = 1;
            this.trackBarVelRatio.Location = new System.Drawing.Point(12, 37);
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
            this.btnMove.Location = new System.Drawing.Point(45, 36);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(95, 44);
            this.btnMove.TabIndex = 4;
            this.btnMove.Text = "MOVE";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // lblVelRatio
            // 
            this.lblVelRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVelRatio.Location = new System.Drawing.Point(45, 12);
            this.lblVelRatio.Name = "lblVelRatio";
            this.lblVelRatio.Size = new System.Drawing.Size(95, 21);
            this.lblVelRatio.TabIndex = 6;
            this.lblVelRatio.Text = "Vel Ratio(%)";
            this.lblVelRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCommand
            // 
            this.lblCommand.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCommand.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommand.ForeColor = System.Drawing.Color.Blue;
            this.lblCommand.Location = new System.Drawing.Point(70, 57);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(142, 20);
            this.lblCommand.TabIndex = 20;
            this.lblCommand.Text = " Command";
            this.lblCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurPos
            // 
            this.lblCurPos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCurPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurPos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurPos.ForeColor = System.Drawing.Color.Blue;
            this.lblCurPos.Location = new System.Drawing.Point(70, 33);
            this.lblCurPos.Name = "lblCurPos";
            this.lblCurPos.Size = new System.Drawing.Size(142, 19);
            this.lblCurPos.TabIndex = 19;
            this.lblCurPos.Text = " Cur Pos";
            this.lblCurPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEvent
            // 
            this.lblEvent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblEvent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEvent.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEvent.ForeColor = System.Drawing.Color.Blue;
            this.lblEvent.Location = new System.Drawing.Point(70, 11);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(142, 20);
            this.lblEvent.TabIndex = 20;
            this.lblEvent.Text = " Event";
            this.lblEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSource
            // 
            this.lblSource.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSource.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.ForeColor = System.Drawing.Color.Blue;
            this.lblSource.Location = new System.Drawing.Point(70, 34);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(142, 20);
            this.lblSource.TabIndex = 20;
            this.lblSource.Text = " Source";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lblSetPos);
            this.groupBox3.Controls.Add(this.lblCurPos);
            this.groupBox3.Location = new System.Drawing.Point(3, -2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 58);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "Set Pos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "Cur Pos";
            // 
            // lblSetPos
            // 
            this.lblSetPos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSetPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSetPos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSetPos.ForeColor = System.Drawing.Color.Blue;
            this.lblSetPos.Location = new System.Drawing.Point(70, 11);
            this.lblSetPos.Name = "lblSetPos";
            this.lblSetPos.Size = new System.Drawing.Size(142, 19);
            this.lblSetPos.TabIndex = 19;
            this.lblSetPos.Text = " Set Pos";
            this.lblSetPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.lblPosSwitch);
            this.groupBox4.Controls.Add(this.lblHomeSwitch);
            this.groupBox4.Controls.Add(this.lblNegSwitch);
            this.groupBox4.Controls.Add(this.lblCommand);
            this.groupBox4.Controls.Add(this.lblEvent);
            this.groupBox4.Controls.Add(this.lblSource);
            this.groupBox4.Location = new System.Drawing.Point(3, 158);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(218, 106);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "Sensor";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Command";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "Source";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "Event";
            // 
            // lblPosSwitch
            // 
            this.lblPosSwitch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblPosSwitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPosSwitch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosSwitch.ForeColor = System.Drawing.Color.Blue;
            this.lblPosSwitch.Location = new System.Drawing.Point(176, 80);
            this.lblPosSwitch.Name = "lblPosSwitch";
            this.lblPosSwitch.Size = new System.Drawing.Size(36, 20);
            this.lblPosSwitch.TabIndex = 20;
            this.lblPosSwitch.Text = "+";
            this.lblPosSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHomeSwitch
            // 
            this.lblHomeSwitch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblHomeSwitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHomeSwitch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomeSwitch.ForeColor = System.Drawing.Color.Blue;
            this.lblHomeSwitch.Location = new System.Drawing.Point(109, 80);
            this.lblHomeSwitch.Name = "lblHomeSwitch";
            this.lblHomeSwitch.Size = new System.Drawing.Size(64, 20);
            this.lblHomeSwitch.TabIndex = 20;
            this.lblHomeSwitch.Text = "HOME";
            this.lblHomeSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNegSwitch
            // 
            this.lblNegSwitch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblNegSwitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNegSwitch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNegSwitch.ForeColor = System.Drawing.Color.Blue;
            this.lblNegSwitch.Location = new System.Drawing.Point(70, 80);
            this.lblNegSwitch.Name = "lblNegSwitch";
            this.lblNegSwitch.Size = new System.Drawing.Size(36, 20);
            this.lblNegSwitch.TabIndex = 20;
            this.lblNegSwitch.Text = "-";
            this.lblNegSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
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
            this.listTeachPoint.Location = new System.Drawing.Point(3, 58);
            this.listTeachPoint.MultiSelect = false;
            this.listTeachPoint.Name = "listTeachPoint";
            this.listTeachPoint.Size = new System.Drawing.Size(217, 103);
            this.listTeachPoint.TabIndex = 24;
            this.listTeachPoint.UseCompatibleStateImageBehavior = false;
            this.listTeachPoint.View = System.Windows.Forms.View.Details;
            this.listTeachPoint.Click += new System.EventHandler(this.listTeachPoint_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtJogVel);
            this.groupBox5.Controls.Add(this.btnJogMinus);
            this.groupBox5.Controls.Add(this.btnJogPlus);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.trackBarJogVel);
            this.groupBox5.Location = new System.Drawing.Point(223, 129);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(146, 135);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
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
            this.txtJogVel.Size = new System.Drawing.Size(36, 21);
            this.txtJogVel.TabIndex = 15;
            this.txtJogVel.Text = "1.0";
            this.txtJogVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtJogVel.UsedInKeyPad = false;
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
            // DlgShowerHead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 324);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.listTeachPoint);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Name = "DlgShowerHead";
            this.Text = "ShowerHead Servo Motor";
            this.Load += new System.EventHandler(this.DlgShowerHead_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVelRatio)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJogVel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxServoHome;
        private System.Windows.Forms.CheckBox checkBoxServoOn;
        private System.Windows.Forms.CheckBox checkBoxServoEstop;
        private System.Windows.Forms.Button btnEstop;
        private System.Windows.Forms.Button btnServoOn;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBarVelRatio;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Label lblVelRatio;
        private Dms.Common.ValidationTextBox txtVelRatio;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.Label lblCurPos;
        private System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.ListView listTeachPoint;
        private System.Windows.Forms.GroupBox groupBox5;
        private Dms.Common.ValidationTextBox txtJogVel;
        private System.Windows.Forms.Button btnJogMinus;
        private System.Windows.Forms.Button btnJogPlus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarJogVel;
        private System.Windows.Forms.Button btnRepeat;
        private System.Windows.Forms.Label lblSetPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPosSwitch;
        private System.Windows.Forms.Label lblHomeSwitch;
        private System.Windows.Forms.Label lblNegSwitch;

    }
}
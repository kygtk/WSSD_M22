namespace Dms.Control
{
    partial class ViewSeAp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewSeAp));
            this.chkStatus2 = new System.Windows.Forms.CheckBox();
            this.chkStatus1 = new System.Windows.Forms.CheckBox();
            this.chkStatus0 = new System.Windows.Forms.CheckBox();
            this.chkPowerReady = new System.Windows.Forms.CheckBox();
            this.gbSignal = new System.Windows.Forms.GroupBox();
            this.chkPCWPressLowLimit = new System.Windows.Forms.CheckBox();
            this.chkPCWPressHighLimit = new System.Windows.Forms.CheckBox();
            this.chkPCWFlowLowLimit = new System.Windows.Forms.CheckBox();
            this.chkPCWFlowHighLimit = new System.Windows.Forms.CheckBox();
            this.chkCDAPressHighLimit = new System.Windows.Forms.CheckBox();
            this.chkN2PressHighLimit = new System.Windows.Forms.CheckBox();
            this.chkHouseClose = new System.Windows.Forms.CheckBox();
            this.chkCDAPressLowLimit = new System.Windows.Forms.CheckBox();
            this.chkN2PressLowLimit = new System.Windows.Forms.CheckBox();
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.btnVoltageSet = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurVol = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gbPowerSupply = new System.Windows.Forms.GroupBox();
            this.txtSetVol = new Dms.Common.ValidationTextBox();
            this.btnVoltageReset = new System.Windows.Forms.Button();
            this.chkPowerOn = new System.Windows.Forms.CheckBox();
            this.btnCylDown = new System.Windows.Forms.Button();
            this.chkDown = new System.Windows.Forms.CheckBox();
            this.gbHouse = new System.Windows.Forms.GroupBox();
            this.btnCylUp = new System.Windows.Forms.Button();
            this.chkUp = new System.Windows.Forms.CheckBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbFlowMeter = new System.Windows.Forms.GroupBox();
            this.txtSetCDA = new Dms.Common.ValidationTextBox();
            this.txtSetN2 = new Dms.Common.ValidationTextBox();
            this.btnCDAReset = new System.Windows.Forms.Button();
            this.btnN2Reset = new System.Windows.Forms.Button();
            this.btnCDASet = new System.Windows.Forms.Button();
            this.btnN2Set = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurN2 = new System.Windows.Forms.TextBox();
            this.txtCurCDA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkLowVoltageErr = new System.Windows.Forms.CheckBox();
            this.chkInverterErr = new System.Windows.Forms.CheckBox();
            this.chkIntrOpen = new System.Windows.Forms.CheckBox();
            this.chkHighVoltageErr = new System.Windows.Forms.CheckBox();
            this.chkOnFault = new System.Windows.Forms.CheckBox();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.chkNoErr = new System.Windows.Forms.CheckBox();
            this.chkLocalModeRunErr = new System.Windows.Forms.CheckBox();
            this.chkARCFault = new System.Windows.Forms.CheckBox();
            this.gbSignal.SuspendLayout();
            this.gbPowerSupply.SuspendLayout();
            this.gbHouse.SuspendLayout();
            this.gbFlowMeter.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkStatus2
            // 
            this.chkStatus2.AutoCheck = false;
            this.chkStatus2.AutoSize = true;
            this.chkStatus2.Location = new System.Drawing.Point(16, 115);
            this.chkStatus2.Name = "chkStatus2";
            this.chkStatus2.Size = new System.Drawing.Size(146, 19);
            this.chkStatus2.TabIndex = 3;
            this.chkStatus2.Text = "Power Supply Status2";
            this.chkStatus2.UseVisualStyleBackColor = true;
            // 
            // chkStatus1
            // 
            this.chkStatus1.AutoCheck = false;
            this.chkStatus1.AutoSize = true;
            this.chkStatus1.Location = new System.Drawing.Point(16, 88);
            this.chkStatus1.Name = "chkStatus1";
            this.chkStatus1.Size = new System.Drawing.Size(146, 19);
            this.chkStatus1.TabIndex = 2;
            this.chkStatus1.Text = "Power Supply Status1";
            this.chkStatus1.UseVisualStyleBackColor = true;
            // 
            // chkStatus0
            // 
            this.chkStatus0.AutoCheck = false;
            this.chkStatus0.AutoSize = true;
            this.chkStatus0.Location = new System.Drawing.Point(16, 63);
            this.chkStatus0.Name = "chkStatus0";
            this.chkStatus0.Size = new System.Drawing.Size(146, 19);
            this.chkStatus0.TabIndex = 1;
            this.chkStatus0.Text = "Power Supply Status0";
            this.chkStatus0.UseVisualStyleBackColor = true;
            // 
            // chkPowerReady
            // 
            this.chkPowerReady.AutoCheck = false;
            this.chkPowerReady.AutoSize = true;
            this.chkPowerReady.Location = new System.Drawing.Point(16, 38);
            this.chkPowerReady.Name = "chkPowerReady";
            this.chkPowerReady.Size = new System.Drawing.Size(99, 19);
            this.chkPowerReady.TabIndex = 0;
            this.chkPowerReady.Text = "Power Ready";
            this.chkPowerReady.UseVisualStyleBackColor = true;
            // 
            // gbSignal
            // 
            this.gbSignal.Controls.Add(this.chkPCWPressLowLimit);
            this.gbSignal.Controls.Add(this.chkPCWPressHighLimit);
            this.gbSignal.Controls.Add(this.chkPCWFlowLowLimit);
            this.gbSignal.Controls.Add(this.chkPCWFlowHighLimit);
            this.gbSignal.Controls.Add(this.chkCDAPressHighLimit);
            this.gbSignal.Controls.Add(this.chkN2PressHighLimit);
            this.gbSignal.Controls.Add(this.chkHouseClose);
            this.gbSignal.Controls.Add(this.chkCDAPressLowLimit);
            this.gbSignal.Controls.Add(this.chkN2PressLowLimit);
            this.gbSignal.Controls.Add(this.chkStatus2);
            this.gbSignal.Controls.Add(this.chkStatus1);
            this.gbSignal.Controls.Add(this.chkStatus0);
            this.gbSignal.Controls.Add(this.chkPowerReady);
            this.gbSignal.Location = new System.Drawing.Point(4, 217);
            this.gbSignal.Name = "gbSignal";
            this.gbSignal.Size = new System.Drawing.Size(592, 184);
            this.gbSignal.TabIndex = 18;
            this.gbSignal.TabStop = false;
            this.gbSignal.Text = "AP Plasma Signal";
            // 
            // chkPCWPressLowLimit
            // 
            this.chkPCWPressLowLimit.AutoCheck = false;
            this.chkPCWPressLowLimit.AutoSize = true;
            this.chkPCWPressLowLimit.Location = new System.Drawing.Point(444, 88);
            this.chkPCWPressLowLimit.Name = "chkPCWPressLowLimit";
            this.chkPCWPressLowLimit.Size = new System.Drawing.Size(116, 19);
            this.chkPCWPressLowLimit.TabIndex = 15;
            this.chkPCWPressLowLimit.Text = "PCW Press Low";
            this.chkPCWPressLowLimit.UseVisualStyleBackColor = true;
            // 
            // chkPCWPressHighLimit
            // 
            this.chkPCWPressHighLimit.AutoCheck = false;
            this.chkPCWPressHighLimit.AutoSize = true;
            this.chkPCWPressHighLimit.Location = new System.Drawing.Point(219, 88);
            this.chkPCWPressHighLimit.Name = "chkPCWPressHighLimit";
            this.chkPCWPressHighLimit.Size = new System.Drawing.Size(119, 19);
            this.chkPCWPressHighLimit.TabIndex = 14;
            this.chkPCWPressHighLimit.Text = "PCW Press High";
            this.chkPCWPressHighLimit.UseVisualStyleBackColor = true;
            // 
            // chkPCWFlowLowLimit
            // 
            this.chkPCWFlowLowLimit.AutoCheck = false;
            this.chkPCWFlowLowLimit.AutoSize = true;
            this.chkPCWFlowLowLimit.Location = new System.Drawing.Point(444, 115);
            this.chkPCWFlowLowLimit.Name = "chkPCWFlowLowLimit";
            this.chkPCWFlowLowLimit.Size = new System.Drawing.Size(109, 19);
            this.chkPCWFlowLowLimit.TabIndex = 13;
            this.chkPCWFlowLowLimit.Text = "PCW Flow Low";
            this.chkPCWFlowLowLimit.UseVisualStyleBackColor = true;
            // 
            // chkPCWFlowHighLimit
            // 
            this.chkPCWFlowHighLimit.AutoCheck = false;
            this.chkPCWFlowHighLimit.AutoSize = true;
            this.chkPCWFlowHighLimit.Location = new System.Drawing.Point(219, 115);
            this.chkPCWFlowHighLimit.Name = "chkPCWFlowHighLimit";
            this.chkPCWFlowHighLimit.Size = new System.Drawing.Size(112, 19);
            this.chkPCWFlowHighLimit.TabIndex = 12;
            this.chkPCWFlowHighLimit.Text = "PCW Flow High";
            this.chkPCWFlowHighLimit.UseVisualStyleBackColor = true;
            // 
            // chkCDAPressHighLimit
            // 
            this.chkCDAPressHighLimit.AutoCheck = false;
            this.chkCDAPressHighLimit.AutoSize = true;
            this.chkCDAPressHighLimit.Location = new System.Drawing.Point(219, 63);
            this.chkCDAPressHighLimit.Name = "chkCDAPressHighLimit";
            this.chkCDAPressHighLimit.Size = new System.Drawing.Size(144, 19);
            this.chkCDAPressHighLimit.TabIndex = 11;
            this.chkCDAPressHighLimit.Text = "MFC CDA Press High";
            this.chkCDAPressHighLimit.UseVisualStyleBackColor = true;
            // 
            // chkN2PressHighLimit
            // 
            this.chkN2PressHighLimit.AutoCheck = false;
            this.chkN2PressHighLimit.AutoSize = true;
            this.chkN2PressHighLimit.Location = new System.Drawing.Point(219, 38);
            this.chkN2PressHighLimit.Name = "chkN2PressHighLimit";
            this.chkN2PressHighLimit.Size = new System.Drawing.Size(135, 19);
            this.chkN2PressHighLimit.TabIndex = 10;
            this.chkN2PressHighLimit.Text = "MFC N2 Press High";
            this.chkN2PressHighLimit.UseVisualStyleBackColor = true;
            // 
            // chkHouseClose
            // 
            this.chkHouseClose.AutoCheck = false;
            this.chkHouseClose.AutoSize = true;
            this.chkHouseClose.Location = new System.Drawing.Point(16, 140);
            this.chkHouseClose.Name = "chkHouseClose";
            this.chkHouseClose.Size = new System.Drawing.Size(117, 19);
            this.chkHouseClose.TabIndex = 9;
            this.chkHouseClose.Text = "AP House Close";
            this.chkHouseClose.UseVisualStyleBackColor = true;
            // 
            // chkCDAPressLowLimit
            // 
            this.chkCDAPressLowLimit.AutoCheck = false;
            this.chkCDAPressLowLimit.AutoSize = true;
            this.chkCDAPressLowLimit.Location = new System.Drawing.Point(444, 63);
            this.chkCDAPressLowLimit.Name = "chkCDAPressLowLimit";
            this.chkCDAPressLowLimit.Size = new System.Drawing.Size(141, 19);
            this.chkCDAPressLowLimit.TabIndex = 8;
            this.chkCDAPressLowLimit.Text = "MFC CDA Press Low";
            this.chkCDAPressLowLimit.UseVisualStyleBackColor = true;
            // 
            // chkN2PressLowLimit
            // 
            this.chkN2PressLowLimit.AutoCheck = false;
            this.chkN2PressLowLimit.AutoSize = true;
            this.chkN2PressLowLimit.Location = new System.Drawing.Point(444, 38);
            this.chkN2PressLowLimit.Name = "chkN2PressLowLimit";
            this.chkN2PressLowLimit.Size = new System.Drawing.Size(132, 19);
            this.chkN2PressLowLimit.TabIndex = 7;
            this.chkN2PressLowLimit.Text = "MFC N2 Press Low";
            this.chkN2PressLowLimit.UseVisualStyleBackColor = true;
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.BackColor = System.Drawing.Color.Honeydew;
            this.btnPowerOn.Location = new System.Drawing.Point(39, 128);
            this.btnPowerOn.Name = "btnPowerOn";
            this.btnPowerOn.Size = new System.Drawing.Size(222, 57);
            this.btnPowerOn.TabIndex = 17;
            this.btnPowerOn.Text = "Power On";
            this.btnPowerOn.UseVisualStyleBackColor = false;
            this.btnPowerOn.Click += new System.EventHandler(this.button_Click);
            // 
            // btnVoltageSet
            // 
            this.btnVoltageSet.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnVoltageSet.Location = new System.Drawing.Point(160, 28);
            this.btnVoltageSet.Name = "btnVoltageSet";
            this.btnVoltageSet.Size = new System.Drawing.Size(61, 48);
            this.btnVoltageSet.TabIndex = 16;
            this.btnVoltageSet.Text = "SET";
            this.btnVoltageSet.UseVisualStyleBackColor = false;
            this.btnVoltageSet.Click += new System.EventHandler(this.button_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 30);
            this.label7.TabIndex = 12;
            this.label7.Text = "Voltage : (kV)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCurVol
            // 
            this.txtCurVol.Location = new System.Drawing.Point(66, 28);
            this.txtCurVol.Name = "txtCurVol";
            this.txtCurVol.ReadOnly = true;
            this.txtCurVol.Size = new System.Drawing.Size(88, 21);
            this.txtCurVol.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(20, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(278, 1);
            this.label6.TabIndex = 15;
            // 
            // gbPowerSupply
            // 
            this.gbPowerSupply.Controls.Add(this.txtSetVol);
            this.gbPowerSupply.Controls.Add(this.btnVoltageReset);
            this.gbPowerSupply.Controls.Add(this.chkPowerOn);
            this.gbPowerSupply.Controls.Add(this.btnPowerOn);
            this.gbPowerSupply.Controls.Add(this.btnVoltageSet);
            this.gbPowerSupply.Controls.Add(this.label7);
            this.gbPowerSupply.Controls.Add(this.label6);
            this.gbPowerSupply.Controls.Add(this.txtCurVol);
            this.gbPowerSupply.Location = new System.Drawing.Point(602, 6);
            this.gbPowerSupply.Name = "gbPowerSupply";
            this.gbPowerSupply.Size = new System.Drawing.Size(293, 205);
            this.gbPowerSupply.TabIndex = 17;
            this.gbPowerSupply.TabStop = false;
            this.gbPowerSupply.Text = "Power Supply";
            // 
            // txtSetVol
            // 
            this.txtSetVol.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetVol.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetVol.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetVol.KeyPadInfo")));
            this.txtSetVol.LimitHigh = "15";
            this.txtSetVol.LimitLow = "8";
            this.txtSetVol.Location = new System.Drawing.Point(66, 55);
            this.txtSetVol.Name = "txtSetVol";
            this.txtSetVol.Size = new System.Drawing.Size(88, 21);
            this.txtSetVol.TabIndex = 18;
            this.txtSetVol.UsedInKeyPad = false;
            // 
            // btnVoltageReset
            // 
            this.btnVoltageReset.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnVoltageReset.Location = new System.Drawing.Point(226, 28);
            this.btnVoltageReset.Name = "btnVoltageReset";
            this.btnVoltageReset.Size = new System.Drawing.Size(61, 48);
            this.btnVoltageReset.TabIndex = 19;
            this.btnVoltageReset.Text = "RESET";
            this.btnVoltageReset.UseVisualStyleBackColor = false;
            this.btnVoltageReset.Click += new System.EventHandler(this.button_Click);
            // 
            // chkPowerOn
            // 
            this.chkPowerOn.AutoCheck = false;
            this.chkPowerOn.AutoSize = true;
            this.chkPowerOn.Location = new System.Drawing.Point(41, 103);
            this.chkPowerOn.Name = "chkPowerOn";
            this.chkPowerOn.Size = new System.Drawing.Size(80, 19);
            this.chkPowerOn.TabIndex = 18;
            this.chkPowerOn.Text = "Power On";
            this.chkPowerOn.UseVisualStyleBackColor = true;
            // 
            // btnCylDown
            // 
            this.btnCylDown.BackColor = System.Drawing.Color.LightCyan;
            this.btnCylDown.Location = new System.Drawing.Point(152, 73);
            this.btnCylDown.Name = "btnCylDown";
            this.btnCylDown.Size = new System.Drawing.Size(132, 101);
            this.btnCylDown.TabIndex = 2;
            this.btnCylDown.Text = "DOWN";
            this.btnCylDown.UseVisualStyleBackColor = false;
            this.btnCylDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCyl_MouseDown);
            this.btnCylDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCyl_MouseUp);
            // 
            // chkDown
            // 
            this.chkDown.AutoCheck = false;
            this.chkDown.AutoSize = true;
            this.chkDown.Location = new System.Drawing.Point(157, 38);
            this.chkDown.Name = "chkDown";
            this.chkDown.Size = new System.Drawing.Size(106, 19);
            this.chkDown.TabIndex = 3;
            this.chkDown.Text = "Down Position";
            this.chkDown.UseVisualStyleBackColor = true;
            // 
            // gbHouse
            // 
            this.gbHouse.Controls.Add(this.chkDown);
            this.gbHouse.Controls.Add(this.btnCylDown);
            this.gbHouse.Controls.Add(this.btnCylUp);
            this.gbHouse.Controls.Add(this.chkUp);
            this.gbHouse.Location = new System.Drawing.Point(602, 217);
            this.gbHouse.Name = "gbHouse";
            this.gbHouse.Size = new System.Drawing.Size(293, 184);
            this.gbHouse.TabIndex = 19;
            this.gbHouse.TabStop = false;
            this.gbHouse.Text = "AP House";
            // 
            // btnCylUp
            // 
            this.btnCylUp.BackColor = System.Drawing.Color.Linen;
            this.btnCylUp.Location = new System.Drawing.Point(10, 73);
            this.btnCylUp.Name = "btnCylUp";
            this.btnCylUp.Size = new System.Drawing.Size(132, 101);
            this.btnCylUp.TabIndex = 1;
            this.btnCylUp.Text = "UP";
            this.btnCylUp.UseVisualStyleBackColor = false;
            this.btnCylUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCyl_MouseDown);
            this.btnCylUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCyl_MouseUp);
            // 
            // chkUp
            // 
            this.chkUp.AutoCheck = false;
            this.chkUp.AutoSize = true;
            this.chkUp.Location = new System.Drawing.Point(15, 38);
            this.chkUp.Name = "chkUp";
            this.chkUp.Size = new System.Drawing.Size(90, 19);
            this.chkUp.TabIndex = 0;
            this.chkUp.Text = "Up Position";
            this.chkUp.UseVisualStyleBackColor = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // gbFlowMeter
            // 
            this.gbFlowMeter.Controls.Add(this.txtSetCDA);
            this.gbFlowMeter.Controls.Add(this.txtSetN2);
            this.gbFlowMeter.Controls.Add(this.btnCDAReset);
            this.gbFlowMeter.Controls.Add(this.btnN2Reset);
            this.gbFlowMeter.Controls.Add(this.btnCDASet);
            this.gbFlowMeter.Controls.Add(this.btnN2Set);
            this.gbFlowMeter.Controls.Add(this.label5);
            this.gbFlowMeter.Controls.Add(this.label4);
            this.gbFlowMeter.Controls.Add(this.txtCurN2);
            this.gbFlowMeter.Controls.Add(this.txtCurCDA);
            this.gbFlowMeter.Controls.Add(this.label1);
            this.gbFlowMeter.Controls.Add(this.label2);
            this.gbFlowMeter.Location = new System.Drawing.Point(303, 6);
            this.gbFlowMeter.Name = "gbFlowMeter";
            this.gbFlowMeter.Size = new System.Drawing.Size(293, 205);
            this.gbFlowMeter.TabIndex = 15;
            this.gbFlowMeter.TabStop = false;
            this.gbFlowMeter.Text = "Flow Meters";
            // 
            // txtSetCDA
            // 
            this.txtSetCDA.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetCDA.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetCDA.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetCDA.KeyPadInfo")));
            this.txtSetCDA.LimitHigh = "20";
            this.txtSetCDA.LimitLow = "0";
            this.txtSetCDA.Location = new System.Drawing.Point(54, 114);
            this.txtSetCDA.Name = "txtSetCDA";
            this.txtSetCDA.Size = new System.Drawing.Size(100, 21);
            this.txtSetCDA.TabIndex = 17;
            this.txtSetCDA.UsedInKeyPad = false;
            // 
            // txtSetN2
            // 
            this.txtSetN2.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetN2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetN2.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetN2.KeyPadInfo")));
            this.txtSetN2.LimitHigh = "2000";
            this.txtSetN2.LimitLow = "600";
            this.txtSetN2.Location = new System.Drawing.Point(54, 54);
            this.txtSetN2.Name = "txtSetN2";
            this.txtSetN2.Size = new System.Drawing.Size(100, 21);
            this.txtSetN2.TabIndex = 16;
            this.txtSetN2.UsedInKeyPad = false;
            // 
            // btnCDAReset
            // 
            this.btnCDAReset.BackColor = System.Drawing.Color.LightCyan;
            this.btnCDAReset.Location = new System.Drawing.Point(226, 87);
            this.btnCDAReset.Name = "btnCDAReset";
            this.btnCDAReset.Size = new System.Drawing.Size(61, 48);
            this.btnCDAReset.TabIndex = 15;
            this.btnCDAReset.Text = "RESET";
            this.btnCDAReset.UseVisualStyleBackColor = false;
            this.btnCDAReset.Click += new System.EventHandler(this.button_Click);
            // 
            // btnN2Reset
            // 
            this.btnN2Reset.BackColor = System.Drawing.Color.LightCyan;
            this.btnN2Reset.Location = new System.Drawing.Point(226, 28);
            this.btnN2Reset.Name = "btnN2Reset";
            this.btnN2Reset.Size = new System.Drawing.Size(61, 48);
            this.btnN2Reset.TabIndex = 14;
            this.btnN2Reset.Text = "RESET";
            this.btnN2Reset.UseVisualStyleBackColor = false;
            this.btnN2Reset.Click += new System.EventHandler(this.button_Click);
            // 
            // btnCDASet
            // 
            this.btnCDASet.BackColor = System.Drawing.Color.Pink;
            this.btnCDASet.Location = new System.Drawing.Point(161, 87);
            this.btnCDASet.Name = "btnCDASet";
            this.btnCDASet.Size = new System.Drawing.Size(61, 48);
            this.btnCDASet.TabIndex = 11;
            this.btnCDASet.Text = "SET";
            this.btnCDASet.UseVisualStyleBackColor = false;
            this.btnCDASet.Click += new System.EventHandler(this.button_Click);
            // 
            // btnN2Set
            // 
            this.btnN2Set.BackColor = System.Drawing.Color.Pink;
            this.btnN2Set.Location = new System.Drawing.Point(161, 28);
            this.btnN2Set.Name = "btnN2Set";
            this.btnN2Set.Size = new System.Drawing.Size(61, 48);
            this.btnN2Set.TabIndex = 10;
            this.btnN2Set.Text = "SET";
            this.btnN2Set.UseVisualStyleBackColor = false;
            this.btnN2Set.Click += new System.EventHandler(this.button_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(9, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(278, 1);
            this.label5.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(9, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 1);
            this.label4.TabIndex = 8;
            // 
            // txtCurN2
            // 
            this.txtCurN2.Location = new System.Drawing.Point(54, 28);
            this.txtCurN2.Name = "txtCurN2";
            this.txtCurN2.ReadOnly = true;
            this.txtCurN2.Size = new System.Drawing.Size(100, 21);
            this.txtCurN2.TabIndex = 3;
            // 
            // txtCurCDA
            // 
            this.txtCurCDA.Location = new System.Drawing.Point(54, 87);
            this.txtCurCDA.Name = "txtCurCDA";
            this.txtCurCDA.ReadOnly = true;
            this.txtCurCDA.Size = new System.Drawing.Size(100, 21);
            this.txtCurCDA.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "N2 : (lpm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 33);
            this.label2.TabIndex = 1;
            this.label2.Text = "CDA : (lpm)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkLowVoltageErr
            // 
            this.chkLowVoltageErr.AutoCheck = false;
            this.chkLowVoltageErr.AutoSize = true;
            this.chkLowVoltageErr.Location = new System.Drawing.Point(16, 63);
            this.chkLowVoltageErr.Name = "chkLowVoltageErr";
            this.chkLowVoltageErr.Size = new System.Drawing.Size(158, 19);
            this.chkLowVoltageErr.TabIndex = 2;
            this.chkLowVoltageErr.Text = "Inverter DC Low-Voltage";
            this.chkLowVoltageErr.UseVisualStyleBackColor = true;
            // 
            // chkInverterErr
            // 
            this.chkInverterErr.AutoCheck = false;
            this.chkInverterErr.AutoSize = true;
            this.chkInverterErr.Location = new System.Drawing.Point(16, 39);
            this.chkInverterErr.Name = "chkInverterErr";
            this.chkInverterErr.Size = new System.Drawing.Size(96, 19);
            this.chkInverterErr.TabIndex = 1;
            this.chkInverterErr.Text = "Inverter Fault";
            this.chkInverterErr.UseVisualStyleBackColor = true;
            // 
            // chkIntrOpen
            // 
            this.chkIntrOpen.AutoCheck = false;
            this.chkIntrOpen.AutoSize = true;
            this.chkIntrOpen.Location = new System.Drawing.Point(16, 15);
            this.chkIntrOpen.Name = "chkIntrOpen";
            this.chkIntrOpen.Size = new System.Drawing.Size(140, 19);
            this.chkIntrOpen.TabIndex = 0;
            this.chkIntrOpen.Text = "Interlock Open Alarm";
            this.chkIntrOpen.UseVisualStyleBackColor = true;
            // 
            // chkHighVoltageErr
            // 
            this.chkHighVoltageErr.AutoCheck = false;
            this.chkHighVoltageErr.AutoSize = true;
            this.chkHighVoltageErr.Location = new System.Drawing.Point(16, 87);
            this.chkHighVoltageErr.Name = "chkHighVoltageErr";
            this.chkHighVoltageErr.Size = new System.Drawing.Size(135, 19);
            this.chkHighVoltageErr.TabIndex = 3;
            this.chkHighVoltageErr.Text = "Output Over-Voltage";
            this.chkHighVoltageErr.UseVisualStyleBackColor = true;
            // 
            // chkOnFault
            // 
            this.chkOnFault.AutoCheck = false;
            this.chkOnFault.AutoSize = true;
            this.chkOnFault.Location = new System.Drawing.Point(16, 159);
            this.chkOnFault.Name = "chkOnFault";
            this.chkOnFault.Size = new System.Drawing.Size(118, 19);
            this.chkOnFault.TabIndex = 6;
            this.chkOnFault.Text = "Plasma On Fault";
            this.chkOnFault.UseVisualStyleBackColor = true;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.chkNoErr);
            this.gbStatus.Controls.Add(this.chkOnFault);
            this.gbStatus.Controls.Add(this.chkLocalModeRunErr);
            this.gbStatus.Controls.Add(this.chkARCFault);
            this.gbStatus.Controls.Add(this.chkHighVoltageErr);
            this.gbStatus.Controls.Add(this.chkLowVoltageErr);
            this.gbStatus.Controls.Add(this.chkInverterErr);
            this.gbStatus.Controls.Add(this.chkIntrOpen);
            this.gbStatus.Location = new System.Drawing.Point(4, 6);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(293, 205);
            this.gbStatus.TabIndex = 14;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "AP Plasma Status";
            // 
            // chkNoErr
            // 
            this.chkNoErr.AutoCheck = false;
            this.chkNoErr.AutoSize = true;
            this.chkNoErr.Location = new System.Drawing.Point(16, 182);
            this.chkNoErr.Name = "chkNoErr";
            this.chkNoErr.Size = new System.Drawing.Size(72, 19);
            this.chkNoErr.TabIndex = 7;
            this.chkNoErr.Text = "No Error";
            this.chkNoErr.UseVisualStyleBackColor = true;
            // 
            // chkLocalModeRunErr
            // 
            this.chkLocalModeRunErr.AutoCheck = false;
            this.chkLocalModeRunErr.AutoSize = true;
            this.chkLocalModeRunErr.Location = new System.Drawing.Point(16, 135);
            this.chkLocalModeRunErr.Name = "chkLocalModeRunErr";
            this.chkLocalModeRunErr.Size = new System.Drawing.Size(145, 19);
            this.chkLocalModeRunErr.TabIndex = 5;
            this.chkLocalModeRunErr.Text = "Local Mode Run Error";
            this.chkLocalModeRunErr.UseVisualStyleBackColor = true;
            // 
            // chkARCFault
            // 
            this.chkARCFault.AutoCheck = false;
            this.chkARCFault.AutoSize = true;
            this.chkARCFault.Location = new System.Drawing.Point(16, 111);
            this.chkARCFault.Name = "chkARCFault";
            this.chkARCFault.Size = new System.Drawing.Size(81, 19);
            this.chkARCFault.TabIndex = 4;
            this.chkARCFault.Text = "ARC Fault";
            this.chkARCFault.UseVisualStyleBackColor = true;
            // 
            // ViewSeAp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gbSignal);
            this.Controls.Add(this.gbPowerSupply);
            this.Controls.Add(this.gbHouse);
            this.Controls.Add(this.gbFlowMeter);
            this.Controls.Add(this.gbStatus);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewSeAp";
            this.Size = new System.Drawing.Size(899, 407);
            this.gbSignal.ResumeLayout(false);
            this.gbSignal.PerformLayout();
            this.gbPowerSupply.ResumeLayout(false);
            this.gbPowerSupply.PerformLayout();
            this.gbHouse.ResumeLayout(false);
            this.gbHouse.PerformLayout();
            this.gbFlowMeter.ResumeLayout(false);
            this.gbFlowMeter.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkStatus2;
        private System.Windows.Forms.CheckBox chkStatus1;
        private System.Windows.Forms.CheckBox chkStatus0;
        private System.Windows.Forms.CheckBox chkPowerReady;
        private System.Windows.Forms.GroupBox gbSignal;
        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.Button btnVoltageSet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCurVol;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox gbPowerSupply;
        private System.Windows.Forms.Button btnVoltageReset;
        private System.Windows.Forms.CheckBox chkPowerOn;
        private System.Windows.Forms.Button btnCylDown;
        private System.Windows.Forms.CheckBox chkDown;
        private System.Windows.Forms.GroupBox gbHouse;
        private System.Windows.Forms.Button btnCylUp;
        private System.Windows.Forms.CheckBox chkUp;
        private System.Windows.Forms.GroupBox gbFlowMeter;
        private System.Windows.Forms.Button btnCDAReset;
        private System.Windows.Forms.Button btnN2Reset;
        private System.Windows.Forms.Button btnCDASet;
        private System.Windows.Forms.Button btnN2Set;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurN2;
        private System.Windows.Forms.TextBox txtCurCDA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkLowVoltageErr;
        private System.Windows.Forms.CheckBox chkInverterErr;
        private System.Windows.Forms.CheckBox chkIntrOpen;
        private System.Windows.Forms.CheckBox chkHighVoltageErr;
        private System.Windows.Forms.CheckBox chkOnFault;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.CheckBox chkLocalModeRunErr;
        private System.Windows.Forms.CheckBox chkARCFault;
        public Dms.Common.ValidationTextBox txtSetVol;
        public Dms.Common.ValidationTextBox txtSetCDA;
        public Dms.Common.ValidationTextBox txtSetN2;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.CheckBox chkNoErr;
        private System.Windows.Forms.CheckBox chkHouseClose;
        private System.Windows.Forms.CheckBox chkCDAPressLowLimit;
        private System.Windows.Forms.CheckBox chkN2PressLowLimit;
        private System.Windows.Forms.CheckBox chkN2PressHighLimit;
        private System.Windows.Forms.CheckBox chkPCWPressLowLimit;
        private System.Windows.Forms.CheckBox chkPCWPressHighLimit;
        private System.Windows.Forms.CheckBox chkPCWFlowLowLimit;
        private System.Windows.Forms.CheckBox chkPCWFlowHighLimit;
        private System.Windows.Forms.CheckBox chkCDAPressHighLimit;


    }
}

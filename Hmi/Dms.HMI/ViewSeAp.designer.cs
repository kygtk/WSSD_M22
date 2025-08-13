namespace Dms.HMI
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
            this.gbSignal = new System.Windows.Forms.GroupBox();
            this.chkN2Interlock = new System.Windows.Forms.CheckBox();
            this.chkPCWInterlock = new System.Windows.Forms.CheckBox();
            this.chkHouseClose = new System.Windows.Forms.CheckBox();
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.btnVoltageSet = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurVol = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gbPowerSupply = new System.Windows.Forms.GroupBox();
            this.checkWatt = new System.Windows.Forms.CheckBox();
            this.txtSetVol = new Dms.Common.ValidationTextBox();
            this.txtCurWATT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnVoltageReset = new System.Windows.Forms.Button();
            this.chkPowerOn = new System.Windows.Forms.CheckBox();
            this.btnCylDown = new System.Windows.Forms.Button();
            this.chkDown = new System.Windows.Forms.CheckBox();
            this.gbHouse = new System.Windows.Forms.GroupBox();
            this.btnCylUp = new System.Windows.Forms.Button();
            this.chkUp = new System.Windows.Forms.CheckBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbFlowMeter = new System.Windows.Forms.GroupBox();
            this.checkPCWFlow = new System.Windows.Forms.CheckBox();
            this.pcwclose_btn = new System.Windows.Forms.Button();
            this.txtCurPCW = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pcwopen_btn = new System.Windows.Forms.Button();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkPCW = new System.Windows.Forms.CheckBox();
            this.checkCDA = new System.Windows.Forms.CheckBox();
            this.checkN2 = new System.Windows.Forms.CheckBox();
            this.txtCurPCWPress = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCurN2Press = new System.Windows.Forms.TextBox();
            this.txtCurCDAPress = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gbSignal.SuspendLayout();
            this.gbPowerSupply.SuspendLayout();
            this.gbHouse.SuspendLayout();
            this.gbFlowMeter.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkStatus2
            // 
            this.chkStatus2.AutoCheck = false;
            this.chkStatus2.AutoSize = true;
            this.chkStatus2.Location = new System.Drawing.Point(16, 76);
            this.chkStatus2.Name = "chkStatus2";
            this.chkStatus2.Size = new System.Drawing.Size(164, 19);
            this.chkStatus2.TabIndex = 3;
            this.chkStatus2.Text = "Power Generator Status2";
            this.chkStatus2.UseVisualStyleBackColor = true;
            // 
            // chkStatus1
            // 
            this.chkStatus1.AutoCheck = false;
            this.chkStatus1.AutoSize = true;
            this.chkStatus1.Location = new System.Drawing.Point(16, 49);
            this.chkStatus1.Name = "chkStatus1";
            this.chkStatus1.Size = new System.Drawing.Size(164, 19);
            this.chkStatus1.TabIndex = 2;
            this.chkStatus1.Text = "Power Generator Status1";
            this.chkStatus1.UseVisualStyleBackColor = true;
            // 
            // chkStatus0
            // 
            this.chkStatus0.AutoCheck = false;
            this.chkStatus0.AutoSize = true;
            this.chkStatus0.Location = new System.Drawing.Point(16, 24);
            this.chkStatus0.Name = "chkStatus0";
            this.chkStatus0.Size = new System.Drawing.Size(164, 19);
            this.chkStatus0.TabIndex = 1;
            this.chkStatus0.Text = "Power Generator Status0";
            this.chkStatus0.UseVisualStyleBackColor = true;
            // 
            // gbSignal
            // 
            this.gbSignal.Controls.Add(this.chkN2Interlock);
            this.gbSignal.Controls.Add(this.chkPCWInterlock);
            this.gbSignal.Controls.Add(this.chkHouseClose);
            this.gbSignal.Controls.Add(this.chkStatus2);
            this.gbSignal.Controls.Add(this.chkStatus1);
            this.gbSignal.Controls.Add(this.chkStatus0);
            this.gbSignal.Location = new System.Drawing.Point(4, 217);
            this.gbSignal.Name = "gbSignal";
            this.gbSignal.Size = new System.Drawing.Size(293, 184);
            this.gbSignal.TabIndex = 18;
            this.gbSignal.TabStop = false;
            this.gbSignal.Text = "AP Plasma Signal";
            // 
            // chkN2Interlock
            // 
            this.chkN2Interlock.AutoCheck = false;
            this.chkN2Interlock.AutoSize = true;
            this.chkN2Interlock.Location = new System.Drawing.Point(16, 126);
            this.chkN2Interlock.Name = "chkN2Interlock";
            this.chkN2Interlock.Size = new System.Drawing.Size(120, 19);
            this.chkN2Interlock.TabIndex = 11;
            this.chkN2Interlock.Text = "N2 Flow Interlock";
            this.chkN2Interlock.UseVisualStyleBackColor = true;
            // 
            // chkPCWInterlock
            // 
            this.chkPCWInterlock.AutoCheck = false;
            this.chkPCWInterlock.AutoSize = true;
            this.chkPCWInterlock.Location = new System.Drawing.Point(16, 101);
            this.chkPCWInterlock.Name = "chkPCWInterlock";
            this.chkPCWInterlock.Size = new System.Drawing.Size(132, 19);
            this.chkPCWInterlock.TabIndex = 10;
            this.chkPCWInterlock.Text = "PCW Flow Interlock";
            this.chkPCWInterlock.UseVisualStyleBackColor = true;
            // 
            // chkHouseClose
            // 
            this.chkHouseClose.AutoCheck = false;
            this.chkHouseClose.AutoSize = true;
            this.chkHouseClose.Location = new System.Drawing.Point(16, 151);
            this.chkHouseClose.Name = "chkHouseClose";
            this.chkHouseClose.Size = new System.Drawing.Size(117, 19);
            this.chkHouseClose.TabIndex = 9;
            this.chkHouseClose.Text = "AP House Close";
            this.chkHouseClose.UseVisualStyleBackColor = true;
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.BackColor = System.Drawing.Color.Honeydew;
            this.btnPowerOn.Location = new System.Drawing.Point(39, 144);
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
            this.txtCurVol.Location = new System.Drawing.Point(66, 25);
            this.txtCurVol.Name = "txtCurVol";
            this.txtCurVol.ReadOnly = true;
            this.txtCurVol.Size = new System.Drawing.Size(88, 21);
            this.txtCurVol.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(9, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(278, 1);
            this.label6.TabIndex = 15;
            // 
            // gbPowerSupply
            // 
            this.gbPowerSupply.Controls.Add(this.checkWatt);
            this.gbPowerSupply.Controls.Add(this.txtSetVol);
            this.gbPowerSupply.Controls.Add(this.txtCurWATT);
            this.gbPowerSupply.Controls.Add(this.label3);
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
            // checkWatt
            // 
            this.checkWatt.AutoCheck = false;
            this.checkWatt.AutoSize = true;
            this.checkWatt.Location = new System.Drawing.Point(160, 87);
            this.checkWatt.Name = "checkWatt";
            this.checkWatt.Size = new System.Drawing.Size(85, 19);
            this.checkWatt.TabIndex = 20;
            this.checkWatt.Text = "Watt Alarm";
            this.checkWatt.UseVisualStyleBackColor = true;
            // 
            // txtSetVol
            // 
            this.txtSetVol.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetVol.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetVol.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetVol.KeyPadInfo")));
            this.txtSetVol.LimitHigh = "15";
            this.txtSetVol.LimitLow = "7";
            this.txtSetVol.Location = new System.Drawing.Point(66, 51);
            this.txtSetVol.Name = "txtSetVol";
            this.txtSetVol.ReferenceTag = null;
            this.txtSetVol.Size = new System.Drawing.Size(88, 21);
            this.txtSetVol.TabIndex = 18;
            this.txtSetVol.UsedInKeyPad = false;
            // 
            // txtCurWATT
            // 
            this.txtCurWATT.Location = new System.Drawing.Point(66, 87);
            this.txtCurWATT.Name = "txtCurWATT";
            this.txtCurWATT.ReadOnly = true;
            this.txtCurWATT.Size = new System.Drawing.Size(88, 21);
            this.txtCurWATT.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 33);
            this.label3.TabIndex = 8;
            this.label3.Text = "Watt : (Kw)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.chkPowerOn.Location = new System.Drawing.Point(41, 122);
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
            this.gbFlowMeter.Controls.Add(this.checkPCWFlow);
            this.gbFlowMeter.Controls.Add(this.pcwclose_btn);
            this.gbFlowMeter.Controls.Add(this.txtCurPCW);
            this.gbFlowMeter.Controls.Add(this.label12);
            this.gbFlowMeter.Controls.Add(this.pcwopen_btn);
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
            // checkPCWFlow
            // 
            this.checkPCWFlow.AutoCheck = false;
            this.checkPCWFlow.AutoSize = true;
            this.checkPCWFlow.Location = new System.Drawing.Point(12, 180);
            this.checkPCWFlow.Name = "checkPCWFlow";
            this.checkPCWFlow.Size = new System.Drawing.Size(118, 19);
            this.checkPCWFlow.TabIndex = 21;
            this.checkPCWFlow.Text = "PCW Flow Alarm";
            this.checkPCWFlow.UseVisualStyleBackColor = true;
            // 
            // pcwclose_btn
            // 
            this.pcwclose_btn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pcwclose_btn.Location = new System.Drawing.Point(226, 144);
            this.pcwclose_btn.Name = "pcwclose_btn";
            this.pcwclose_btn.Size = new System.Drawing.Size(61, 57);
            this.pcwclose_btn.TabIndex = 22;
            this.pcwclose_btn.Text = "PCW CLOSE";
            this.pcwclose_btn.UseVisualStyleBackColor = false;
            this.pcwclose_btn.Click += new System.EventHandler(this.btn_PcwClose);
            // 
            // txtCurPCW
            // 
            this.txtCurPCW.Location = new System.Drawing.Point(54, 148);
            this.txtCurPCW.Name = "txtCurPCW";
            this.txtCurPCW.ReadOnly = true;
            this.txtCurPCW.Size = new System.Drawing.Size(100, 21);
            this.txtCurPCW.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(2, 145);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 33);
            this.label12.TabIndex = 20;
            this.label12.Text = "PCW : (lpm)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pcwopen_btn
            // 
            this.pcwopen_btn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pcwopen_btn.Location = new System.Drawing.Point(161, 144);
            this.pcwopen_btn.Name = "pcwopen_btn";
            this.pcwopen_btn.Size = new System.Drawing.Size(61, 56);
            this.pcwopen_btn.TabIndex = 19;
            this.pcwopen_btn.Text = "PCW OPEN";
            this.pcwopen_btn.UseVisualStyleBackColor = false;
            this.pcwopen_btn.Click += new System.EventHandler(this.btn_PcwOpen);
            // 
            // txtSetCDA
            // 
            this.txtSetCDA.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetCDA.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetCDA.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetCDA.KeyPadInfo")));
            this.txtSetCDA.LimitHigh = "15";
            this.txtSetCDA.LimitLow = "0.9";
            this.txtSetCDA.Location = new System.Drawing.Point(54, 114);
            this.txtSetCDA.Name = "txtSetCDA";
            this.txtSetCDA.ReferenceTag = null;
            this.txtSetCDA.Size = new System.Drawing.Size(100, 21);
            this.txtSetCDA.TabIndex = 17;
            this.txtSetCDA.UsedInKeyPad = false;
            // 
            // txtSetN2
            // 
            this.txtSetN2.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtSetN2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtSetN2.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetN2.KeyPadInfo")));
            this.txtSetN2.LimitHigh = "1000";
            this.txtSetN2.LimitLow = "520";
            this.txtSetN2.Location = new System.Drawing.Point(54, 54);
            this.txtSetN2.Name = "txtSetN2";
            this.txtSetN2.ReferenceTag = null;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkPCW);
            this.groupBox1.Controls.Add(this.checkCDA);
            this.groupBox1.Controls.Add(this.checkN2);
            this.groupBox1.Controls.Add(this.txtCurPCWPress);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtCurN2Press);
            this.groupBox1.Controls.Add(this.txtCurCDAPress);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(303, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 185);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pressure Meters";
            // 
            // checkPCW
            // 
            this.checkPCW.AutoCheck = false;
            this.checkPCW.AutoSize = true;
            this.checkPCW.Location = new System.Drawing.Point(162, 138);
            this.checkPCW.Name = "checkPCW";
            this.checkPCW.Size = new System.Drawing.Size(89, 19);
            this.checkPCW.TabIndex = 14;
            this.checkPCW.Text = "PCW Alarm";
            this.checkPCW.UseVisualStyleBackColor = true;
            // 
            // checkCDA
            // 
            this.checkCDA.AutoCheck = false;
            this.checkCDA.AutoSize = true;
            this.checkCDA.Location = new System.Drawing.Point(162, 82);
            this.checkCDA.Name = "checkCDA";
            this.checkCDA.Size = new System.Drawing.Size(86, 19);
            this.checkCDA.TabIndex = 13;
            this.checkCDA.Text = "CDA Alarm";
            this.checkCDA.UseVisualStyleBackColor = true;
            // 
            // checkN2
            // 
            this.checkN2.AutoCheck = false;
            this.checkN2.AutoSize = true;
            this.checkN2.Location = new System.Drawing.Point(162, 30);
            this.checkN2.Name = "checkN2";
            this.checkN2.Size = new System.Drawing.Size(77, 19);
            this.checkN2.TabIndex = 12;
            this.checkN2.Text = "N2 Alarm";
            this.checkN2.UseVisualStyleBackColor = true;
            // 
            // txtCurPCWPress
            // 
            this.txtCurPCWPress.Location = new System.Drawing.Point(56, 138);
            this.txtCurPCWPress.Name = "txtCurPCWPress";
            this.txtCurPCWPress.ReadOnly = true;
            this.txtCurPCWPress.Size = new System.Drawing.Size(100, 21);
            this.txtCurPCWPress.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(4, 135);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 33);
            this.label13.TabIndex = 10;
            this.label13.Text = "PCW : (kPa)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.Location = new System.Drawing.Point(9, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(278, 1);
            this.label8.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightGray;
            this.label9.Location = new System.Drawing.Point(9, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(278, 1);
            this.label9.TabIndex = 8;
            // 
            // txtCurN2Press
            // 
            this.txtCurN2Press.Location = new System.Drawing.Point(56, 28);
            this.txtCurN2Press.Name = "txtCurN2Press";
            this.txtCurN2Press.ReadOnly = true;
            this.txtCurN2Press.Size = new System.Drawing.Size(100, 21);
            this.txtCurN2Press.TabIndex = 3;
            // 
            // txtCurCDAPress
            // 
            this.txtCurCDAPress.Location = new System.Drawing.Point(56, 80);
            this.txtCurCDAPress.Name = "txtCurCDAPress";
            this.txtCurCDAPress.ReadOnly = true;
            this.txtCurCDAPress.Size = new System.Drawing.Size(100, 21);
            this.txtCurCDAPress.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(4, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 30);
            this.label10.TabIndex = 0;
            this.label10.Text = "N2 : (kPa)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(4, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 33);
            this.label11.TabIndex = 1;
            this.label11.Text = "CDA : (kPa)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ViewSeAp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkStatus2;
        private System.Windows.Forms.CheckBox chkStatus1;
        private System.Windows.Forms.CheckBox chkStatus0;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCurWATT;
        private System.Windows.Forms.Button pcwopen_btn;
        private System.Windows.Forms.CheckBox chkN2Interlock;
        private System.Windows.Forms.CheckBox chkPCWInterlock;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCurN2Press;
        private System.Windows.Forms.TextBox txtCurCDAPress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCurPCWPress;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCurPCW;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button pcwclose_btn;
        private System.Windows.Forms.CheckBox checkWatt;
        private System.Windows.Forms.CheckBox checkPCWFlow;
        private System.Windows.Forms.CheckBox checkPCW;
        private System.Windows.Forms.CheckBox checkCDA;
        private System.Windows.Forms.CheckBox checkN2;


    }
}

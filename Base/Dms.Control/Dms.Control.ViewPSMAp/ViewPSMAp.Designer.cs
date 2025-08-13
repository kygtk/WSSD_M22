namespace Dms.Control
{
    partial class ViewPSMAp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewPSMAp));
			this.gbStatus = new System.Windows.Forms.GroupBox();
			this.chkNotError = new System.Windows.Forms.CheckBox();
			this.chkARCTrip = new System.Windows.Forms.CheckBox();
			this.chkLocalMode = new System.Windows.Forms.CheckBox();
			this.chkOutputOpen = new System.Windows.Forms.CheckBox();
			this.chkSystemError = new System.Windows.Forms.CheckBox();
			this.chkLowVol = new System.Windows.Forms.CheckBox();
			this.chkIntrOpen = new System.Windows.Forms.CheckBox();
			this.gbFlowMeter = new System.Windows.Forms.GroupBox();
			this.btnCDAReset = new System.Windows.Forms.Button();
			this.btnN2Reset = new System.Windows.Forms.Button();
			this.txtSetCDA = new Dms.Common.ValidationTextBox();
			this.txtSetN2 = new Dms.Common.ValidationTextBox();
			this.btnCDASet = new System.Windows.Forms.Button();
			this.btnN2Set = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCurPCW = new System.Windows.Forms.TextBox();
			this.txtCurN2 = new System.Windows.Forms.TextBox();
			this.txtCurCDA = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.gbPowerSupply = new System.Windows.Forms.GroupBox();
			this.btnVoltageReset = new System.Windows.Forms.Button();
			this.txtSetVol = new Dms.Common.ValidationTextBox();
			this.chkPowerOn = new System.Windows.Forms.CheckBox();
			this.btnPowerOn = new System.Windows.Forms.Button();
			this.btnVoltageSet = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCurVol = new System.Windows.Forms.TextBox();
			this.gbSignal = new System.Windows.Forms.GroupBox();
			this.chkCDALimit = new System.Windows.Forms.CheckBox();
			this.chkN2Limit = new System.Windows.Forms.CheckBox();
			this.chkStatus2 = new System.Windows.Forms.CheckBox();
			this.chkStatus1 = new System.Windows.Forms.CheckBox();
			this.chkStatus0 = new System.Windows.Forms.CheckBox();
			this.chkPowerReady = new System.Windows.Forms.CheckBox();
			this.gbColorBoard = new System.Windows.Forms.GroupBox();
			this.txtCurBlue = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtCurGreen = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtCurRed = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.gbHouse = new System.Windows.Forms.GroupBox();
			this.chkDown = new System.Windows.Forms.CheckBox();
			this.btnCylDown = new System.Windows.Forms.Button();
			this.btnCylUp = new System.Windows.Forms.Button();
			this.chkUp = new System.Windows.Forms.CheckBox();
			this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
			this.chkLowExhaustAlarm = new System.Windows.Forms.CheckBox();
			this.chkUpExhaustAlarm = new System.Windows.Forms.CheckBox();
			this.chkPCWLimit = new System.Windows.Forms.CheckBox();
			this.chkChamberClose = new System.Windows.Forms.CheckBox();
			this.gbStatus.SuspendLayout();
			this.gbFlowMeter.SuspendLayout();
			this.gbPowerSupply.SuspendLayout();
			this.gbSignal.SuspendLayout();
			this.gbColorBoard.SuspendLayout();
			this.gbHouse.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbStatus
			// 
			this.gbStatus.Controls.Add(this.chkNotError);
			this.gbStatus.Controls.Add(this.chkARCTrip);
			this.gbStatus.Controls.Add(this.chkLocalMode);
			this.gbStatus.Controls.Add(this.chkOutputOpen);
			this.gbStatus.Controls.Add(this.chkSystemError);
			this.gbStatus.Controls.Add(this.chkLowVol);
			this.gbStatus.Controls.Add(this.chkIntrOpen);
			this.gbStatus.Location = new System.Drawing.Point(3, 3);
			this.gbStatus.Name = "gbStatus";
			this.gbStatus.Size = new System.Drawing.Size(293, 205);
			this.gbStatus.TabIndex = 1;
			this.gbStatus.TabStop = false;
			this.gbStatus.Text = "AP Plasma Status";
			// 
			// chkNotError
			// 
			this.chkNotError.AutoCheck = false;
			this.chkNotError.AutoSize = true;
			this.chkNotError.Location = new System.Drawing.Point(16, 176);
			this.chkNotError.Name = "chkNotError";
			this.chkNotError.Size = new System.Drawing.Size(75, 19);
			this.chkNotError.TabIndex = 6;
			this.chkNotError.Text = "Not Error";
			this.chkNotError.UseVisualStyleBackColor = true;
			// 
			// chkARCTrip
			// 
			this.chkARCTrip.AutoCheck = false;
			this.chkARCTrip.AutoSize = true;
			this.chkARCTrip.Location = new System.Drawing.Point(16, 152);
			this.chkARCTrip.Name = "chkARCTrip";
			this.chkARCTrip.Size = new System.Drawing.Size(75, 19);
			this.chkARCTrip.TabIndex = 5;
			this.chkARCTrip.Text = "ARC Trip";
			this.chkARCTrip.UseVisualStyleBackColor = true;
			// 
			// chkLocalMode
			// 
			this.chkLocalMode.AutoCheck = false;
			this.chkLocalMode.AutoSize = true;
			this.chkLocalMode.Location = new System.Drawing.Point(16, 128);
			this.chkLocalMode.Name = "chkLocalMode";
			this.chkLocalMode.Size = new System.Drawing.Size(167, 19);
			this.chkLocalMode.TabIndex = 4;
			this.chkLocalMode.Text = "Power Supply Local Mode";
			this.chkLocalMode.UseVisualStyleBackColor = true;
			// 
			// chkOutputOpen
			// 
			this.chkOutputOpen.AutoCheck = false;
			this.chkOutputOpen.AutoSize = true;
			this.chkOutputOpen.Location = new System.Drawing.Point(16, 104);
			this.chkOutputOpen.Name = "chkOutputOpen";
			this.chkOutputOpen.Size = new System.Drawing.Size(95, 19);
			this.chkOutputOpen.TabIndex = 3;
			this.chkOutputOpen.Text = "Output Open";
			this.chkOutputOpen.UseVisualStyleBackColor = true;
			// 
			// chkSystemError
			// 
			this.chkSystemError.AutoCheck = false;
			this.chkSystemError.AutoSize = true;
			this.chkSystemError.Location = new System.Drawing.Point(16, 80);
			this.chkSystemError.Name = "chkSystemError";
			this.chkSystemError.Size = new System.Drawing.Size(246, 19);
			this.chkSystemError.TabIndex = 2;
			this.chkSystemError.Text = "Driver Board or System Abnormal Status";
			this.chkSystemError.UseVisualStyleBackColor = true;
			// 
			// chkLowVol
			// 
			this.chkLowVol.AutoCheck = false;
			this.chkLowVol.AutoSize = true;
			this.chkLowVol.Location = new System.Drawing.Point(16, 56);
			this.chkLowVol.Name = "chkLowVol";
			this.chkLowVol.Size = new System.Drawing.Size(114, 19);
			this.chkLowVol.TabIndex = 1;
			this.chkLowVol.Text = "DC Low Voltage";
			this.chkLowVol.UseVisualStyleBackColor = true;
			// 
			// chkIntrOpen
			// 
			this.chkIntrOpen.AutoCheck = false;
			this.chkIntrOpen.AutoSize = true;
			this.chkIntrOpen.Location = new System.Drawing.Point(16, 32);
			this.chkIntrOpen.Name = "chkIntrOpen";
			this.chkIntrOpen.Size = new System.Drawing.Size(105, 19);
			this.chkIntrOpen.TabIndex = 0;
			this.chkIntrOpen.Text = "Interlock Open";
			this.chkIntrOpen.UseVisualStyleBackColor = true;
			// 
			// gbFlowMeter
			// 
			this.gbFlowMeter.Controls.Add(this.btnCDAReset);
			this.gbFlowMeter.Controls.Add(this.btnN2Reset);
			this.gbFlowMeter.Controls.Add(this.txtSetCDA);
			this.gbFlowMeter.Controls.Add(this.txtSetN2);
			this.gbFlowMeter.Controls.Add(this.btnCDASet);
			this.gbFlowMeter.Controls.Add(this.btnN2Set);
			this.gbFlowMeter.Controls.Add(this.label5);
			this.gbFlowMeter.Controls.Add(this.label4);
			this.gbFlowMeter.Controls.Add(this.txtCurPCW);
			this.gbFlowMeter.Controls.Add(this.txtCurN2);
			this.gbFlowMeter.Controls.Add(this.txtCurCDA);
			this.gbFlowMeter.Controls.Add(this.label1);
			this.gbFlowMeter.Controls.Add(this.label2);
			this.gbFlowMeter.Controls.Add(this.label3);
			this.gbFlowMeter.Location = new System.Drawing.Point(302, 3);
			this.gbFlowMeter.Name = "gbFlowMeter";
			this.gbFlowMeter.Size = new System.Drawing.Size(293, 205);
			this.gbFlowMeter.TabIndex = 2;
			this.gbFlowMeter.TabStop = false;
			this.gbFlowMeter.Text = "Flow Meters";
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
			// txtSetCDA
			// 
			this.txtSetCDA.DataFormat = Dms.Common.OptionFormat.Float;
			this.txtSetCDA.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.txtSetCDA.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetCDA.KeyPadInfo")));
			this.txtSetCDA.LimitHigh = "3";
			this.txtSetCDA.LimitLow = "0";
			this.txtSetCDA.Location = new System.Drawing.Point(54, 114);
			this.txtSetCDA.Name = "txtSetCDA";
			this.txtSetCDA.ReferenceTag = null;
			this.txtSetCDA.Size = new System.Drawing.Size(100, 21);
			this.txtSetCDA.TabIndex = 13;
			this.txtSetCDA.UsedInKeyPad = false;
			// 
			// txtSetN2
			// 
			this.txtSetN2.DataFormat = Dms.Common.OptionFormat.Float;
			this.txtSetN2.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.txtSetN2.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetN2.KeyPadInfo")));
			this.txtSetN2.LimitHigh = "1000";
			this.txtSetN2.LimitLow = "500";
			this.txtSetN2.Location = new System.Drawing.Point(54, 54);
			this.txtSetN2.Name = "txtSetN2";
			this.txtSetN2.ReferenceTag = null;
			this.txtSetN2.Size = new System.Drawing.Size(100, 21);
			this.txtSetN2.TabIndex = 12;
			this.txtSetN2.UsedInKeyPad = false;
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
			// txtCurPCW
			// 
			this.txtCurPCW.Location = new System.Drawing.Point(54, 146);
			this.txtCurPCW.Name = "txtCurPCW";
			this.txtCurPCW.ReadOnly = true;
			this.txtCurPCW.Size = new System.Drawing.Size(100, 21);
			this.txtCurPCW.TabIndex = 5;
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
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 149);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 36);
			this.label3.TabIndex = 2;
			this.label3.Text = "PCW : (lpm)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// gbPowerSupply
			// 
			this.gbPowerSupply.Controls.Add(this.btnVoltageReset);
			this.gbPowerSupply.Controls.Add(this.txtSetVol);
			this.gbPowerSupply.Controls.Add(this.chkPowerOn);
			this.gbPowerSupply.Controls.Add(this.btnPowerOn);
			this.gbPowerSupply.Controls.Add(this.btnVoltageSet);
			this.gbPowerSupply.Controls.Add(this.label7);
			this.gbPowerSupply.Controls.Add(this.label6);
			this.gbPowerSupply.Controls.Add(this.txtCurVol);
			this.gbPowerSupply.Location = new System.Drawing.Point(601, 3);
			this.gbPowerSupply.Name = "gbPowerSupply";
			this.gbPowerSupply.Size = new System.Drawing.Size(293, 205);
			this.gbPowerSupply.TabIndex = 12;
			this.gbPowerSupply.TabStop = false;
			this.gbPowerSupply.Text = "Power Supply";
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
			// txtSetVol
			// 
			this.txtSetVol.DataFormat = Dms.Common.OptionFormat.Float;
			this.txtSetVol.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.txtSetVol.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtSetVol.KeyPadInfo")));
			this.txtSetVol.LimitHigh = "12";
			this.txtSetVol.LimitLow = "10";
			this.txtSetVol.Location = new System.Drawing.Point(66, 54);
			this.txtSetVol.Name = "txtSetVol";
			this.txtSetVol.ReferenceTag = null;
			this.txtSetVol.Size = new System.Drawing.Size(88, 21);
			this.txtSetVol.TabIndex = 14;
			this.txtSetVol.UsedInKeyPad = false;
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
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.LightGray;
			this.label6.Location = new System.Drawing.Point(20, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(278, 1);
			this.label6.TabIndex = 15;
			// 
			// txtCurVol
			// 
			this.txtCurVol.Location = new System.Drawing.Point(66, 28);
			this.txtCurVol.Name = "txtCurVol";
			this.txtCurVol.ReadOnly = true;
			this.txtCurVol.Size = new System.Drawing.Size(88, 21);
			this.txtCurVol.TabIndex = 13;
			// 
			// gbSignal
			// 
			this.gbSignal.Controls.Add(this.chkLowExhaustAlarm);
			this.gbSignal.Controls.Add(this.chkUpExhaustAlarm);
			this.gbSignal.Controls.Add(this.chkPCWLimit);
			this.gbSignal.Controls.Add(this.chkCDALimit);
			this.gbSignal.Controls.Add(this.chkN2Limit);
			this.gbSignal.Controls.Add(this.chkStatus2);
			this.gbSignal.Controls.Add(this.chkStatus1);
			this.gbSignal.Controls.Add(this.chkStatus0);
			this.gbSignal.Controls.Add(this.chkPowerReady);
			this.gbSignal.Location = new System.Drawing.Point(3, 214);
			this.gbSignal.Name = "gbSignal";
			this.gbSignal.Size = new System.Drawing.Size(293, 240);
			this.gbSignal.TabIndex = 13;
			this.gbSignal.TabStop = false;
			this.gbSignal.Text = "AP Plasma Signal";
			// 
			// chkCDALimit
			// 
			this.chkCDALimit.AutoCheck = false;
			this.chkCDALimit.AutoSize = true;
			this.chkCDALimit.Location = new System.Drawing.Point(16, 145);
			this.chkCDALimit.Name = "chkCDALimit";
			this.chkCDALimit.Size = new System.Drawing.Size(145, 19);
			this.chkCDALimit.TabIndex = 5;
			this.chkCDALimit.Text = "CDA Flow Limit Alarm";
			this.chkCDALimit.UseVisualStyleBackColor = true;
			// 
			// chkN2Limit
			// 
			this.chkN2Limit.AutoCheck = false;
			this.chkN2Limit.AutoSize = true;
			this.chkN2Limit.Location = new System.Drawing.Point(16, 121);
			this.chkN2Limit.Name = "chkN2Limit";
			this.chkN2Limit.Size = new System.Drawing.Size(136, 19);
			this.chkN2Limit.TabIndex = 4;
			this.chkN2Limit.Text = "N2 Flow Limit Alarm";
			this.chkN2Limit.UseVisualStyleBackColor = true;
			// 
			// chkStatus2
			// 
			this.chkStatus2.AutoCheck = false;
			this.chkStatus2.AutoSize = true;
			this.chkStatus2.Location = new System.Drawing.Point(16, 97);
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
			this.chkStatus1.Location = new System.Drawing.Point(16, 73);
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
			this.chkStatus0.Location = new System.Drawing.Point(16, 49);
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
			this.chkPowerReady.Location = new System.Drawing.Point(16, 25);
			this.chkPowerReady.Name = "chkPowerReady";
			this.chkPowerReady.Size = new System.Drawing.Size(99, 19);
			this.chkPowerReady.TabIndex = 0;
			this.chkPowerReady.Text = "Power Ready";
			this.chkPowerReady.UseVisualStyleBackColor = true;
			// 
			// gbColorBoard
			// 
			this.gbColorBoard.Controls.Add(this.txtCurBlue);
			this.gbColorBoard.Controls.Add(this.label10);
			this.gbColorBoard.Controls.Add(this.txtCurGreen);
			this.gbColorBoard.Controls.Add(this.label9);
			this.gbColorBoard.Controls.Add(this.txtCurRed);
			this.gbColorBoard.Controls.Add(this.label8);
			this.gbColorBoard.Location = new System.Drawing.Point(302, 214);
			this.gbColorBoard.Name = "gbColorBoard";
			this.gbColorBoard.Size = new System.Drawing.Size(293, 240);
			this.gbColorBoard.TabIndex = 6;
			this.gbColorBoard.TabStop = false;
			this.gbColorBoard.Text = "Color Board";
			// 
			// txtCurBlue
			// 
			this.txtCurBlue.Location = new System.Drawing.Point(54, 110);
			this.txtCurBlue.Name = "txtCurBlue";
			this.txtCurBlue.ReadOnly = true;
			this.txtCurBlue.Size = new System.Drawing.Size(100, 21);
			this.txtCurBlue.TabIndex = 9;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(-2, 113);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(50, 12);
			this.label10.TabIndex = 8;
			this.label10.Text = "Blue :";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtCurGreen
			// 
			this.txtCurGreen.Location = new System.Drawing.Point(54, 83);
			this.txtCurGreen.Name = "txtCurGreen";
			this.txtCurGreen.ReadOnly = true;
			this.txtCurGreen.Size = new System.Drawing.Size(100, 21);
			this.txtCurGreen.TabIndex = 7;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(-2, 86);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(50, 12);
			this.label9.TabIndex = 6;
			this.label9.Text = "Green :";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtCurRed
			// 
			this.txtCurRed.Location = new System.Drawing.Point(54, 56);
			this.txtCurRed.Name = "txtCurRed";
			this.txtCurRed.ReadOnly = true;
			this.txtCurRed.Size = new System.Drawing.Size(100, 21);
			this.txtCurRed.TabIndex = 5;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(-2, 59);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(50, 12);
			this.label8.TabIndex = 4;
			this.label8.Text = "Red :";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// gbHouse
			// 
			this.gbHouse.Controls.Add(this.chkChamberClose);
			this.gbHouse.Controls.Add(this.chkDown);
			this.gbHouse.Controls.Add(this.btnCylDown);
			this.gbHouse.Controls.Add(this.btnCylUp);
			this.gbHouse.Controls.Add(this.chkUp);
			this.gbHouse.Location = new System.Drawing.Point(601, 214);
			this.gbHouse.Name = "gbHouse";
			this.gbHouse.Size = new System.Drawing.Size(293, 240);
			this.gbHouse.TabIndex = 13;
			this.gbHouse.TabStop = false;
			this.gbHouse.Text = "AP House";
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
			// chkLowExhaustAlarm
			// 
			this.chkLowExhaustAlarm.AutoCheck = false;
			this.chkLowExhaustAlarm.AutoSize = true;
			this.chkLowExhaustAlarm.Location = new System.Drawing.Point(16, 217);
			this.chkLowExhaustAlarm.Name = "chkLowExhaustAlarm";
			this.chkLowExhaustAlarm.Size = new System.Drawing.Size(131, 19);
			this.chkLowExhaustAlarm.TabIndex = 8;
			this.chkLowExhaustAlarm.Text = "Low Exhaust Alarm";
			this.chkLowExhaustAlarm.UseVisualStyleBackColor = true;
			// 
			// chkUpExhaustAlarm
			// 
			this.chkUpExhaustAlarm.AutoCheck = false;
			this.chkUpExhaustAlarm.AutoSize = true;
			this.chkUpExhaustAlarm.Location = new System.Drawing.Point(16, 193);
			this.chkUpExhaustAlarm.Name = "chkUpExhaustAlarm";
			this.chkUpExhaustAlarm.Size = new System.Drawing.Size(124, 19);
			this.chkUpExhaustAlarm.TabIndex = 7;
			this.chkUpExhaustAlarm.Text = "Up Exhaust Alarm";
			this.chkUpExhaustAlarm.UseVisualStyleBackColor = true;
			// 
			// chkPCWLimit
			// 
			this.chkPCWLimit.AutoCheck = false;
			this.chkPCWLimit.AutoSize = true;
			this.chkPCWLimit.Location = new System.Drawing.Point(16, 169);
			this.chkPCWLimit.Name = "chkPCWLimit";
			this.chkPCWLimit.Size = new System.Drawing.Size(148, 19);
			this.chkPCWLimit.TabIndex = 6;
			this.chkPCWLimit.Text = "PCW Flow Limit Alarm";
			this.chkPCWLimit.UseVisualStyleBackColor = true;
			// 
			// chkChamberClose
			// 
			this.chkChamberClose.AutoCheck = false;
			this.chkChamberClose.AutoSize = true;
			this.chkChamberClose.Location = new System.Drawing.Point(15, 198);
			this.chkChamberClose.Name = "chkChamberClose";
			this.chkChamberClose.Size = new System.Drawing.Size(114, 19);
			this.chkChamberClose.TabIndex = 4;
			this.chkChamberClose.Text = "Chamber Close";
			this.chkChamberClose.UseVisualStyleBackColor = true;
			// 
			// ViewPSMAp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.gbHouse);
			this.Controls.Add(this.gbColorBoard);
			this.Controls.Add(this.gbSignal);
			this.Controls.Add(this.gbPowerSupply);
			this.Controls.Add(this.gbFlowMeter);
			this.Controls.Add(this.gbStatus);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ViewPSMAp";
			this.Size = new System.Drawing.Size(899, 457);
			this.gbStatus.ResumeLayout(false);
			this.gbStatus.PerformLayout();
			this.gbFlowMeter.ResumeLayout(false);
			this.gbFlowMeter.PerformLayout();
			this.gbPowerSupply.ResumeLayout(false);
			this.gbPowerSupply.PerformLayout();
			this.gbSignal.ResumeLayout(false);
			this.gbSignal.PerformLayout();
			this.gbColorBoard.ResumeLayout(false);
			this.gbColorBoard.PerformLayout();
			this.gbHouse.ResumeLayout(false);
			this.gbHouse.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.CheckBox chkNotError;
        private System.Windows.Forms.CheckBox chkARCTrip;
        private System.Windows.Forms.CheckBox chkLocalMode;
        private System.Windows.Forms.CheckBox chkOutputOpen;
        private System.Windows.Forms.CheckBox chkSystemError;
        private System.Windows.Forms.CheckBox chkLowVol;
        private System.Windows.Forms.CheckBox chkIntrOpen;
        private System.Windows.Forms.GroupBox gbFlowMeter;
        private System.Windows.Forms.Button btnCDASet;
        private System.Windows.Forms.Button btnN2Set;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurPCW;
        private System.Windows.Forms.TextBox txtCurN2;
        private System.Windows.Forms.TextBox txtCurCDA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbPowerSupply;
        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.Button btnVoltageSet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCurVol;
        private System.Windows.Forms.GroupBox gbSignal;
        private System.Windows.Forms.CheckBox chkCDALimit;
        private System.Windows.Forms.CheckBox chkN2Limit;
        private System.Windows.Forms.CheckBox chkStatus2;
        private System.Windows.Forms.CheckBox chkStatus1;
        private System.Windows.Forms.CheckBox chkStatus0;
        private System.Windows.Forms.CheckBox chkPowerReady;
        private System.Windows.Forms.GroupBox gbColorBoard;
        private System.Windows.Forms.TextBox txtCurBlue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCurGreen;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCurRed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbHouse;
        private System.Windows.Forms.CheckBox chkDown;
        private System.Windows.Forms.Button btnCylDown;
        private System.Windows.Forms.Button btnCylUp;
        private System.Windows.Forms.CheckBox chkUp;
        private System.Windows.Forms.CheckBox chkPowerOn;
        private System.Windows.Forms.Timer tmrUpdateState;
        public Dms.Common.ValidationTextBox txtSetN2;
        public Dms.Common.ValidationTextBox txtSetCDA;
        public Dms.Common.ValidationTextBox txtSetVol;
        private System.Windows.Forms.Button btnCDAReset;
        private System.Windows.Forms.Button btnN2Reset;
        private System.Windows.Forms.Button btnVoltageReset;
		private System.Windows.Forms.CheckBox chkLowExhaustAlarm;
		private System.Windows.Forms.CheckBox chkUpExhaustAlarm;
		private System.Windows.Forms.CheckBox chkPCWLimit;
		private System.Windows.Forms.CheckBox chkChamberClose;
    }
}

namespace Dms.Control
{
    partial class ViewShinkoDry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewShinkoDry));
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.chkEmoIn = new System.Windows.Forms.CheckBox();
            this.chkBlowRun = new System.Windows.Forms.CheckBox();
            this.chkPresError = new System.Windows.Forms.CheckBox();
            this.chkTempError = new System.Windows.Forms.CheckBox();
            this.chkWaterLeak = new System.Windows.Forms.CheckBox();
            this.chkInverterError = new System.Windows.Forms.CheckBox();
            this.chkHepaFilterError = new System.Windows.Forms.CheckBox();
            this.chkPreFilterError = new System.Windows.Forms.CheckBox();
            this.gbPressureMeter = new System.Windows.Forms.GroupBox();
            this.gaugeTemperature = new Dms.Control.Gauge();
            this.gaugeVacuum2 = new Dms.Control.Gauge();
            this.gaugeVacuum1 = new Dms.Control.Gauge();
            this.gaugePressure2 = new Dms.Control.Gauge();
            this.gaugePressure1 = new Dms.Control.Gauge();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.labelPressure1 = new System.Windows.Forms.Label();
            this.labelPressure2 = new System.Windows.Forms.Label();
            this.labelVacuum1 = new System.Windows.Forms.Label();
            this.labelVacuum2 = new System.Windows.Forms.Label();
            this.chkPowerOn = new System.Windows.Forms.CheckBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnEmo = new System.Windows.Forms.Button();
            this.chkEmoOut = new System.Windows.Forms.CheckBox();
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.btnEmoRelease = new System.Windows.Forms.Button();
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.gbStatus.SuspendLayout();
            this.gbPressureMeter.SuspendLayout();
            this.gbOutput.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.chkEmoIn);
            this.gbStatus.Controls.Add(this.chkBlowRun);
            this.gbStatus.Location = new System.Drawing.Point(12, 54);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(209, 94);
            this.gbStatus.TabIndex = 1;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Dry Cleaner Status";
            // 
            // chkEmoIn
            // 
            this.chkEmoIn.AutoCheck = false;
            this.chkEmoIn.AutoSize = true;
            this.chkEmoIn.Location = new System.Drawing.Point(16, 56);
            this.chkEmoIn.Name = "chkEmoIn";
            this.chkEmoIn.Size = new System.Drawing.Size(116, 19);
            this.chkEmoIn.TabIndex = 7;
            this.chkEmoIn.Text = "Emergency Stop";
            this.chkEmoIn.UseVisualStyleBackColor = true;
            // 
            // chkBlowRun
            // 
            this.chkBlowRun.AutoCheck = false;
            this.chkBlowRun.AutoSize = true;
            this.chkBlowRun.Location = new System.Drawing.Point(16, 32);
            this.chkBlowRun.Name = "chkBlowRun";
            this.chkBlowRun.Size = new System.Drawing.Size(90, 19);
            this.chkBlowRun.TabIndex = 0;
            this.chkBlowRun.Text = "Blower Run";
            this.chkBlowRun.UseVisualStyleBackColor = true;
            // 
            // chkPresError
            // 
            this.chkPresError.AutoCheck = false;
            this.chkPresError.AutoSize = true;
            this.chkPresError.Location = new System.Drawing.Point(16, 148);
            this.chkPresError.Name = "chkPresError";
            this.chkPresError.Size = new System.Drawing.Size(107, 19);
            this.chkPresError.TabIndex = 6;
            this.chkPresError.Text = "Pressure Error";
            this.chkPresError.UseVisualStyleBackColor = true;
            // 
            // chkTempError
            // 
            this.chkTempError.AutoCheck = false;
            this.chkTempError.AutoSize = true;
            this.chkTempError.Location = new System.Drawing.Point(16, 124);
            this.chkTempError.Name = "chkTempError";
            this.chkTempError.Size = new System.Drawing.Size(127, 19);
            this.chkTempError.TabIndex = 5;
            this.chkTempError.Text = "Temperature Error";
            this.chkTempError.UseVisualStyleBackColor = true;
            // 
            // chkWaterLeak
            // 
            this.chkWaterLeak.AutoCheck = false;
            this.chkWaterLeak.AutoSize = true;
            this.chkWaterLeak.Location = new System.Drawing.Point(16, 100);
            this.chkWaterLeak.Name = "chkWaterLeak";
            this.chkWaterLeak.Size = new System.Drawing.Size(88, 19);
            this.chkWaterLeak.TabIndex = 4;
            this.chkWaterLeak.Text = "Water Leak";
            this.chkWaterLeak.UseVisualStyleBackColor = true;
            // 
            // chkInverterError
            // 
            this.chkInverterError.AutoCheck = false;
            this.chkInverterError.AutoSize = true;
            this.chkInverterError.Location = new System.Drawing.Point(16, 76);
            this.chkInverterError.Name = "chkInverterError";
            this.chkInverterError.Size = new System.Drawing.Size(96, 19);
            this.chkInverterError.TabIndex = 3;
            this.chkInverterError.Text = "Inverter Error";
            this.chkInverterError.UseVisualStyleBackColor = true;
            // 
            // chkHepaFilterError
            // 
            this.chkHepaFilterError.AutoCheck = false;
            this.chkHepaFilterError.AutoSize = true;
            this.chkHepaFilterError.Location = new System.Drawing.Point(16, 52);
            this.chkHepaFilterError.Name = "chkHepaFilterError";
            this.chkHepaFilterError.Size = new System.Drawing.Size(116, 19);
            this.chkHepaFilterError.TabIndex = 2;
            this.chkHepaFilterError.Text = "Hepa Filter Error";
            this.chkHepaFilterError.UseVisualStyleBackColor = true;
            // 
            // chkPreFilterError
            // 
            this.chkPreFilterError.AutoCheck = false;
            this.chkPreFilterError.AutoSize = true;
            this.chkPreFilterError.Location = new System.Drawing.Point(16, 28);
            this.chkPreFilterError.Name = "chkPreFilterError";
            this.chkPreFilterError.Size = new System.Drawing.Size(109, 19);
            this.chkPreFilterError.TabIndex = 1;
            this.chkPreFilterError.Text = "Pre Finter Error";
            this.chkPreFilterError.UseVisualStyleBackColor = true;
            // 
            // gbPressureMeter
            // 
            this.gbPressureMeter.Controls.Add(this.gaugeTemperature);
            this.gbPressureMeter.Controls.Add(this.gaugeVacuum2);
            this.gbPressureMeter.Controls.Add(this.gaugeVacuum1);
            this.gbPressureMeter.Controls.Add(this.gaugePressure2);
            this.gbPressureMeter.Controls.Add(this.gaugePressure1);
            this.gbPressureMeter.Controls.Add(this.label9);
            this.gbPressureMeter.Controls.Add(this.label4);
            this.gbPressureMeter.Controls.Add(this.labelTemperature);
            this.gbPressureMeter.Controls.Add(this.labelPressure1);
            this.gbPressureMeter.Controls.Add(this.labelPressure2);
            this.gbPressureMeter.Controls.Add(this.labelVacuum1);
            this.gbPressureMeter.Controls.Add(this.labelVacuum2);
            this.gbPressureMeter.Location = new System.Drawing.Point(236, 54);
            this.gbPressureMeter.Name = "gbPressureMeter";
            this.gbPressureMeter.Size = new System.Drawing.Size(262, 327);
            this.gbPressureMeter.TabIndex = 9;
            this.gbPressureMeter.TabStop = false;
            this.gbPressureMeter.Text = "Gauges";
            // 
            // gaugeTemperature
            // 
            this.gaugeTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeTemperature.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("gaugeTemperature.DeviceTagInfo")));
            this.gaugeTemperature.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gaugeTemperature.Location = new System.Drawing.Point(130, 242);
            this.gaugeTemperature.Name = "gaugeTemperature";
            this.gaugeTemperature.Size = new System.Drawing.Size(100, 21);
            this.gaugeTemperature.TabIndex = 24;
            // 
            // gaugeVacuum2
            // 
            this.gaugeVacuum2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeVacuum2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("gaugeVacuum2.DeviceTagInfo")));
            this.gaugeVacuum2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gaugeVacuum2.Location = new System.Drawing.Point(130, 177);
            this.gaugeVacuum2.Name = "gaugeVacuum2";
            this.gaugeVacuum2.Size = new System.Drawing.Size(100, 21);
            this.gaugeVacuum2.TabIndex = 23;
            // 
            // gaugeVacuum1
            // 
            this.gaugeVacuum1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeVacuum1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("gaugeVacuum1.DeviceTagInfo")));
            this.gaugeVacuum1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gaugeVacuum1.Location = new System.Drawing.Point(130, 131);
            this.gaugeVacuum1.Name = "gaugeVacuum1";
            this.gaugeVacuum1.Size = new System.Drawing.Size(100, 21);
            this.gaugeVacuum1.TabIndex = 22;
            // 
            // gaugePressure2
            // 
            this.gaugePressure2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugePressure2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("gaugePressure2.DeviceTagInfo")));
            this.gaugePressure2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gaugePressure2.Location = new System.Drawing.Point(130, 68);
            this.gaugePressure2.Name = "gaugePressure2";
            this.gaugePressure2.Size = new System.Drawing.Size(100, 21);
            this.gaugePressure2.TabIndex = 22;
            // 
            // gaugePressure1
            // 
            this.gaugePressure1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugePressure1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("gaugePressure1.DeviceTagInfo")));
            this.gaugePressure1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gaugePressure1.Location = new System.Drawing.Point(130, 32);
            this.gaugePressure1.Name = "gaugePressure1";
            this.gaugePressure1.Size = new System.Drawing.Size(100, 21);
            this.gaugePressure1.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightGray;
            this.label9.Location = new System.Drawing.Point(8, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(245, 1);
            this.label9.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(8, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(245, 1);
            this.label4.TabIndex = 8;
            // 
            // labelTemperature
            // 
            this.labelTemperature.Location = new System.Drawing.Point(7, 242);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(110, 21);
            this.labelTemperature.TabIndex = 5;
            this.labelTemperature.Text = "Temperature(PV) :";
            this.labelTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPressure1
            // 
            this.labelPressure1.Location = new System.Drawing.Point(7, 34);
            this.labelPressure1.Name = "labelPressure1";
            this.labelPressure1.Size = new System.Drawing.Size(110, 21);
            this.labelPressure1.TabIndex = 0;
            this.labelPressure1.Text = "Pressure 1(PV) :";
            this.labelPressure1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPressure2
            // 
            this.labelPressure2.Location = new System.Drawing.Point(7, 68);
            this.labelPressure2.Name = "labelPressure2";
            this.labelPressure2.Size = new System.Drawing.Size(110, 21);
            this.labelPressure2.TabIndex = 1;
            this.labelPressure2.Text = "Pressure 2(PV) :";
            this.labelPressure2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelVacuum1
            // 
            this.labelVacuum1.Location = new System.Drawing.Point(7, 134);
            this.labelVacuum1.Name = "labelVacuum1";
            this.labelVacuum1.Size = new System.Drawing.Size(110, 21);
            this.labelVacuum1.TabIndex = 0;
            this.labelVacuum1.Text = "Vacuum 1(PV) :";
            this.labelVacuum1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelVacuum2
            // 
            this.labelVacuum2.Location = new System.Drawing.Point(7, 177);
            this.labelVacuum2.Name = "labelVacuum2";
            this.labelVacuum2.Size = new System.Drawing.Size(110, 21);
            this.labelVacuum2.TabIndex = 1;
            this.labelVacuum2.Text = "Vacuum 2(PV) :";
            this.labelVacuum2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPowerOn
            // 
            this.chkPowerOn.AutoCheck = false;
            this.chkPowerOn.AutoSize = true;
            this.chkPowerOn.Location = new System.Drawing.Point(26, 25);
            this.chkPowerOn.Name = "chkPowerOn";
            this.chkPowerOn.Size = new System.Drawing.Size(80, 19);
            this.chkPowerOn.TabIndex = 21;
            this.chkPowerOn.Text = "Power On";
            this.chkPowerOn.UseVisualStyleBackColor = true;
            // 
            // chkRun
            // 
            this.chkRun.AutoCheck = false;
            this.chkRun.AutoSize = true;
            this.chkRun.Location = new System.Drawing.Point(26, 125);
            this.chkRun.Name = "chkRun";
            this.chkRun.Size = new System.Drawing.Size(68, 19);
            this.chkRun.TabIndex = 3;
            this.chkRun.Text = "Run On";
            this.chkRun.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.Lime;
            this.btnRun.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(30, 150);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(107, 57);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnEmo
            // 
            this.btnEmo.BackColor = System.Drawing.Color.Tomato;
            this.btnEmo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmo.Location = new System.Drawing.Point(30, 250);
            this.btnEmo.Name = "btnEmo";
            this.btnEmo.Size = new System.Drawing.Size(107, 57);
            this.btnEmo.TabIndex = 1;
            this.btnEmo.Text = "EMO";
            this.btnEmo.UseVisualStyleBackColor = false;
            this.btnEmo.Click += new System.EventHandler(this.btnEmo_Click);
            // 
            // chkEmoOut
            // 
            this.chkEmoOut.AutoCheck = false;
            this.chkEmoOut.AutoSize = true;
            this.chkEmoOut.Location = new System.Drawing.Point(26, 225);
            this.chkEmoOut.Name = "chkEmoOut";
            this.chkEmoOut.Size = new System.Drawing.Size(52, 19);
            this.chkEmoOut.TabIndex = 0;
            this.chkEmoOut.Text = "EMO";
            this.chkEmoOut.UseVisualStyleBackColor = true;
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.BackColor = System.Drawing.Color.DarkOrange;
            this.btnPowerOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPowerOn.Location = new System.Drawing.Point(30, 50);
            this.btnPowerOn.Name = "btnPowerOn";
            this.btnPowerOn.Size = new System.Drawing.Size(107, 57);
            this.btnPowerOn.TabIndex = 20;
            this.btnPowerOn.Text = "Power On";
            this.btnPowerOn.UseVisualStyleBackColor = false;
            this.btnPowerOn.Click += new System.EventHandler(this.btnPowerOn_Click);
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.btnEmoRelease);
            this.gbOutput.Controls.Add(this.btnPowerOff);
            this.gbOutput.Controls.Add(this.btnStop);
            this.gbOutput.Controls.Add(this.label8);
            this.gbOutput.Controls.Add(this.label7);
            this.gbOutput.Controls.Add(this.chkPowerOn);
            this.gbOutput.Controls.Add(this.chkRun);
            this.gbOutput.Controls.Add(this.btnPowerOn);
            this.gbOutput.Controls.Add(this.btnRun);
            this.gbOutput.Controls.Add(this.btnEmo);
            this.gbOutput.Controls.Add(this.chkEmoOut);
            this.gbOutput.Location = new System.Drawing.Point(514, 54);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(293, 327);
            this.gbOutput.TabIndex = 19;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Manual Operation";
            // 
            // btnEmoRelease
            // 
            this.btnEmoRelease.BackColor = System.Drawing.Color.Khaki;
            this.btnEmoRelease.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmoRelease.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnEmoRelease.Location = new System.Drawing.Point(145, 250);
            this.btnEmoRelease.Name = "btnEmoRelease";
            this.btnEmoRelease.Size = new System.Drawing.Size(107, 57);
            this.btnEmoRelease.TabIndex = 24;
            this.btnEmoRelease.Text = "EMO Release";
            this.btnEmoRelease.UseVisualStyleBackColor = false;
            this.btnEmoRelease.Click += new System.EventHandler(this.btnEmoRelease_Click);
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.BackColor = System.Drawing.Color.MediumAquamarine;
            this.btnPowerOff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPowerOff.Location = new System.Drawing.Point(145, 50);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(107, 57);
            this.btnPowerOff.TabIndex = 20;
            this.btnPowerOff.Text = "Power Off";
            this.btnPowerOff.UseVisualStyleBackColor = false;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCyan;
            this.btnStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(145, 150);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(107, 57);
            this.btnStop.TabIndex = 23;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.Location = new System.Drawing.Point(8, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(260, 1);
            this.label8.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(8, 219);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(260, 1);
            this.label7.TabIndex = 20;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPresError);
            this.groupBox1.Controls.Add(this.chkHepaFilterError);
            this.groupBox1.Controls.Add(this.chkPreFilterError);
            this.groupBox1.Controls.Add(this.chkInverterError);
            this.groupBox1.Controls.Add(this.chkTempError);
            this.groupBox1.Controls.Add(this.chkWaterLeak);
            this.groupBox1.Location = new System.Drawing.Point(12, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 223);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Error Condition";
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTitle.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTitle.Location = new System.Drawing.Point(12, 14);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(486, 26);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Text = " Device Name";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ViewShinkoDry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.gbPressureMeter);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "ViewShinkoDry";
            this.Size = new System.Drawing.Size(899, 407);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbPressureMeter.ResumeLayout(false);
            this.gbOutput.ResumeLayout(false);
            this.gbOutput.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.CheckBox chkInverterError;
        private System.Windows.Forms.CheckBox chkHepaFilterError;
        private System.Windows.Forms.CheckBox chkPreFilterError;
        private System.Windows.Forms.CheckBox chkBlowRun;
        private System.Windows.Forms.CheckBox chkPresError;
        private System.Windows.Forms.CheckBox chkTempError;
        private System.Windows.Forms.CheckBox chkWaterLeak;
        private System.Windows.Forms.CheckBox chkEmoIn;
        private System.Windows.Forms.GroupBox gbPressureMeter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelPressure1;
        private System.Windows.Forms.Label labelPressure2;
        private System.Windows.Forms.Label labelVacuum2;
        private System.Windows.Forms.Label labelVacuum1;
        private System.Windows.Forms.CheckBox chkPowerOn;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnEmo;
        private System.Windows.Forms.CheckBox chkEmoOut;
        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.GroupBox gbOutput;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Button btnEmoRelease;
        private System.Windows.Forms.GroupBox groupBox1;
        private Gauge gaugePressure1;
        private System.Windows.Forms.Label labelTitle;
        private Gauge gaugePressure2;
        private Gauge gaugeVacuum2;
        private Gauge gaugeVacuum1;
        private Gauge gaugeTemperature;
    }
}

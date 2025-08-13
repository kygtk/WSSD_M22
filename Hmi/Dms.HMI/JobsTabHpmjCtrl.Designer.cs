namespace Dms.HMI
{
    partial class JobsTabHpmjCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobsTabHpmjCtrl));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.genInfo1 = new Dms.Control.GenInfo();
            this.labelInverter = new Dms.Control.GenInfo();
            this.hpmjCurrent = new Dms.Control.Gauge();
            this.hpmjShowerFlow = new Dms.Control.Gauge();
            this.hpmjShower = new Dms.Control.Shower();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkRemote = new System.Windows.Forms.CheckBox();
            this.hpmjFilterInPress = new Dms.Control.Gauge();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.chkReady = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.hpmjPump = new Dms.Control.Pump();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.hpmjFilterOutPress = new Dms.Control.Gauge();
            this.hpmjBubblerOutPress = new Dms.Control.Gauge();
            this.hpmjCo2VentValve = new Dms.Control.AutoValve();
            this.hpmjCo2Press = new Dms.Control.Gauge();
            this.chkPing = new System.Windows.Forms.CheckBox();
            this.hpmjCo2InValve = new Dms.Control.AutoValve();
            this.sensor1 = new Dms.Control.Sensor();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hpmjCO2Flow = new Dms.Control.Gauge();
            this.hpmjCo2Press2 = new Dms.Control.Gauge();
            this.genInfo2 = new Dms.Control.GenInfo();
            this.hpmjMainDiFlow = new Dms.Control.Gauge();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.viewSetupHpmj = new Dms.Data.ViewSetupInfo();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.genInfo1);
            this.groupBox2.Controls.Add(this.labelInverter);
            this.groupBox2.Controls.Add(this.hpmjCurrent);
            this.groupBox2.Controls.Add(this.hpmjShowerFlow);
            this.groupBox2.Controls.Add(this.hpmjShower);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(255, 92);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EQP";
            // 
            // genInfo1
            // 
            this.genInfo1.AutoHide = false;
            this.genInfo1.AutoHideLogic = true;
            this.genInfo1.BackColor = System.Drawing.Color.Transparent;
            this.genInfo1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo1.DeviceTagInfo")));
            this.genInfo1.Distance = 1;
            this.genInfo1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.Location = new System.Drawing.Point(138, 65);
            this.genInfo1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.genInfo1.Name = "genInfo1";
            this.genInfo1.ReferenceTagDescriptor = ((Dms.Common.TagDescriptor)(resources.GetObject("genInfo1.ReferenceTagDescriptor")));
            this.genInfo1.Size = new System.Drawing.Size(107, 20);
            this.genInfo1.TabIndex = 119;
            this.genInfo1.TitleBackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.genInfo1.TitleForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo1.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo1.TitlePanelSize = 40;
            this.genInfo1.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo1.TitleText = "Target";
            this.genInfo1.TitleTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueBackColor = System.Drawing.Color.Cyan;
            this.genInfo1.ValueBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo1.ValueForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo1.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo1.ValueTextFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueTextUnit = Dms.Common.UnitType.lpm;
            // 
            // labelInverter
            // 
            this.labelInverter.AutoHide = false;
            this.labelInverter.AutoHideLogic = true;
            this.labelInverter.BackColor = System.Drawing.Color.Transparent;
            this.labelInverter.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("labelInverter.DeviceTagInfo")));
            this.labelInverter.Distance = 4;
            this.labelInverter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInverter.Location = new System.Drawing.Point(4, 65);
            this.labelInverter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelInverter.Name = "labelInverter";
            this.labelInverter.ReferenceTagDescriptor = ((Dms.Common.TagDescriptor)(resources.GetObject("labelInverter.ReferenceTagDescriptor")));
            this.labelInverter.Size = new System.Drawing.Size(78, 20);
            this.labelInverter.TabIndex = 118;
            this.labelInverter.TitleBackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.labelInverter.TitleForeColor = System.Drawing.SystemColors.ControlText;
            this.labelInverter.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInverter.TitlePanelSize = 0;
            this.labelInverter.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelInverter.TitleText = "Title";
            this.labelInverter.TitleTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInverter.ValueBackColor = System.Drawing.Color.Transparent;
            this.labelInverter.ValueBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInverter.ValueForeColor = System.Drawing.SystemColors.ControlText;
            this.labelInverter.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelInverter.ValueTextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInverter.ValueTextUnit = Dms.Common.UnitType.Hz;
            // 
            // hpmjCurrent
            // 
            this.hpmjCurrent.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjCurrent.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCurrent.DeviceTagInfo")));
            this.hpmjCurrent.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCurrent.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCurrent.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjCurrent.Location = new System.Drawing.Point(8, 39);
            this.hpmjCurrent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCurrent.Name = "hpmjCurrent";
            this.hpmjCurrent.Size = new System.Drawing.Size(74, 20);
            this.hpmjCurrent.TabIndex = 3;
            this.hpmjCurrent.Visible = false;
            // 
            // hpmjShowerFlow
            // 
            this.hpmjShowerFlow.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjShowerFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjShowerFlow.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjShowerFlow.DeviceTagInfo")));
            this.hpmjShowerFlow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjShowerFlow.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjShowerFlow.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjShowerFlow.Location = new System.Drawing.Point(171, 42);
            this.hpmjShowerFlow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjShowerFlow.Name = "hpmjShowerFlow";
            this.hpmjShowerFlow.Size = new System.Drawing.Size(74, 20);
            this.hpmjShowerFlow.TabIndex = 2;
            // 
            // hpmjShower
            // 
            this.hpmjShower.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjShower.DeviceTagInfo")));
            this.hpmjShower.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjShower.Location = new System.Drawing.Point(117, 41);
            this.hpmjShower.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjShower.Name = "hpmjShower";
            this.hpmjShower.ShowerType = Dms.Control.Shower.Types.UP;
            this.hpmjShower.Size = new System.Drawing.Size(25, 21);
            this.hpmjShower.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label17.Location = new System.Drawing.Point(127, 19);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(5, 36);
            this.label17.TabIndex = 0;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label16.Location = new System.Drawing.Point(206, 21);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(5, 68);
            this.label16.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label4.Location = new System.Drawing.Point(129, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 6);
            this.label4.TabIndex = 0;
            // 
            // chkRemote
            // 
            this.chkRemote.AutoSize = true;
            this.chkRemote.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chkRemote.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkRemote.Location = new System.Drawing.Point(108, 68);
            this.chkRemote.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRemote.Name = "chkRemote";
            this.chkRemote.Size = new System.Drawing.Size(73, 16);
            this.chkRemote.TabIndex = 5;
            this.chkRemote.Text = "Remote";
            this.chkRemote.UseVisualStyleBackColor = true;
            // 
            // hpmjFilterInPress
            // 
            this.hpmjFilterInPress.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjFilterInPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjFilterInPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjFilterInPress.DeviceTagInfo")));
            this.hpmjFilterInPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjFilterInPress.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjFilterInPress.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjFilterInPress.Location = new System.Drawing.Point(171, 163);
            this.hpmjFilterInPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjFilterInPress.Name = "hpmjFilterInPress";
            this.hpmjFilterInPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjFilterInPress.TabIndex = 16;
            // 
            // chkRun
            // 
            this.chkRun.AutoSize = true;
            this.chkRun.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkRun.Location = new System.Drawing.Point(108, 44);
            this.chkRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRun.Name = "chkRun";
            this.chkRun.Size = new System.Drawing.Size(49, 16);
            this.chkRun.TabIndex = 3;
            this.chkRun.Text = "Run";
            this.chkRun.UseVisualStyleBackColor = true;
            // 
            // chkReady
            // 
            this.chkReady.AutoSize = true;
            this.chkReady.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkReady.Location = new System.Drawing.Point(108, 22);
            this.chkReady.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkReady.Name = "chkReady";
            this.chkReady.Size = new System.Drawing.Size(65, 16);
            this.chkReady.TabIndex = 1;
            this.chkReady.Text = "Ready";
            this.chkReady.UseVisualStyleBackColor = true;
            // 
            // chkWarning
            // 
            this.chkWarning.AutoSize = true;
            this.chkWarning.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.chkWarning.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkWarning.Location = new System.Drawing.Point(15, 69);
            this.chkWarning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(76, 16);
            this.chkWarning.TabIndex = 4;
            this.chkWarning.Text = "Warning";
            this.chkWarning.UseVisualStyleBackColor = true;
            // 
            // hpmjPump
            // 
            this.hpmjPump.BackColor = System.Drawing.Color.Transparent;
            this.hpmjPump.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjPump.DeviceTagInfo")));
            this.hpmjPump.Enabled = false;
            this.hpmjPump.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjPump.Location = new System.Drawing.Point(183, 191);
            this.hpmjPump.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjPump.Name = "hpmjPump";
            this.hpmjPump.PumpType = Dms.Control.Pump.DeviceType.Inverter;
            this.hpmjPump.Size = new System.Drawing.Size(50, 29);
            this.hpmjPump.TabIndex = 15;
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAlarm.Location = new System.Drawing.Point(15, 45);
            this.chkAlarm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(62, 16);
            this.chkAlarm.TabIndex = 2;
            this.chkAlarm.Text = "Alarm";
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // hpmjFilterOutPress
            // 
            this.hpmjFilterOutPress.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjFilterOutPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjFilterOutPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjFilterOutPress.DeviceTagInfo")));
            this.hpmjFilterOutPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjFilterOutPress.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjFilterOutPress.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjFilterOutPress.Location = new System.Drawing.Point(171, 77);
            this.hpmjFilterOutPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjFilterOutPress.Name = "hpmjFilterOutPress";
            this.hpmjFilterOutPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjFilterOutPress.TabIndex = 17;
            // 
            // hpmjBubblerOutPress
            // 
            this.hpmjBubblerOutPress.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjBubblerOutPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjBubblerOutPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjBubblerOutPress.DeviceTagInfo")));
            this.hpmjBubblerOutPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjBubblerOutPress.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjBubblerOutPress.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjBubblerOutPress.Location = new System.Drawing.Point(119, 264);
            this.hpmjBubblerOutPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjBubblerOutPress.Name = "hpmjBubblerOutPress";
            this.hpmjBubblerOutPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjBubblerOutPress.TabIndex = 14;
            // 
            // hpmjCo2VentValve
            // 
            this.hpmjCo2VentValve.BackColor = System.Drawing.Color.Transparent;
            this.hpmjCo2VentValve.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2VentValve.DeviceTagInfo")));
            this.hpmjCo2VentValve.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2VentValve.Location = new System.Drawing.Point(45, 30);
            this.hpmjCo2VentValve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2VentValve.Name = "hpmjCo2VentValve";
            this.hpmjCo2VentValve.Size = new System.Drawing.Size(21, 28);
            this.hpmjCo2VentValve.TabIndex = 13;
            this.hpmjCo2VentValve.ValveImageType = Dms.Control.AutoValve.ImageType.Default;
            this.hpmjCo2VentValve.ValveType = Dms.Control.AutoValve.DeviceType.Horizon;
            // 
            // hpmjCo2Press
            // 
            this.hpmjCo2Press.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCo2Press.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjCo2Press.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2Press.DeviceTagInfo")));
            this.hpmjCo2Press.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2Press.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCo2Press.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjCo2Press.Location = new System.Drawing.Point(8, 232);
            this.hpmjCo2Press.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2Press.Name = "hpmjCo2Press";
            this.hpmjCo2Press.Size = new System.Drawing.Size(74, 20);
            this.hpmjCo2Press.TabIndex = 11;
            // 
            // chkPing
            // 
            this.chkPing.AutoSize = true;
            this.chkPing.Enabled = false;
            this.chkPing.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkPing.Location = new System.Drawing.Point(15, 22);
            this.chkPing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkPing.Name = "chkPing";
            this.chkPing.Size = new System.Drawing.Size(57, 16);
            this.chkPing.TabIndex = 0;
            this.chkPing.Text = "PING";
            this.chkPing.UseVisualStyleBackColor = true;
            // 
            // hpmjCo2InValve
            // 
            this.hpmjCo2InValve.BackColor = System.Drawing.Color.Transparent;
            this.hpmjCo2InValve.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2InValve.DeviceTagInfo")));
            this.hpmjCo2InValve.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2InValve.Location = new System.Drawing.Point(28, 262);
            this.hpmjCo2InValve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2InValve.Name = "hpmjCo2InValve";
            this.hpmjCo2InValve.Size = new System.Drawing.Size(28, 25);
            this.hpmjCo2InValve.TabIndex = 12;
            this.hpmjCo2InValve.ValveImageType = Dms.Control.AutoValve.ImageType.Default;
            this.hpmjCo2InValve.ValveType = Dms.Control.AutoValve.DeviceType.Vertical;
            // 
            // sensor1
            // 
            this.sensor1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.sensor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sensor1.Description = "";
            this.sensor1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("sensor1.DeviceTagInfo")));
            this.sensor1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensor1.Location = new System.Drawing.Point(154, 330);
            this.sensor1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.sensor1.Name = "sensor1";
            this.sensor1.OffColor = System.Drawing.Color.White;
            this.sensor1.OnColor = System.Drawing.Color.Lime;
            this.sensor1.SensorBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.sensor1.SensorImageType = Dms.Control.Sensor.ImageType.Default;
            this.sensor1.Size = new System.Drawing.Size(67, 12);
            this.sensor1.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(171, 105);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 49);
            this.label14.TabIndex = 9;
            this.label14.Text = "FILTER";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(8, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 49);
            this.label3.TabIndex = 3;
            this.label3.Text = "CO2 Bubbler";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Lime;
            this.btnStart.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.Location = new System.Drawing.Point(10, 33);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(168, 130);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnStop);
            this.groupBox4.Controls.Add(this.btnStart);
            this.groupBox4.Location = new System.Drawing.Point(274, 103);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(190, 352);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Operation";
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCyan;
            this.btnStop.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStop.Location = new System.Drawing.Point(10, 209);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(168, 130);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkRemote);
            this.groupBox3.Controls.Add(this.chkRun);
            this.groupBox3.Controls.Add(this.chkReady);
            this.groupBox3.Controls.Add(this.chkWarning);
            this.groupBox3.Controls.Add(this.chkAlarm);
            this.groupBox3.Controls.Add(this.chkPing);
            this.groupBox3.Location = new System.Drawing.Point(274, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(190, 92);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hpmjCO2Flow);
            this.groupBox1.Controls.Add(this.hpmjCo2Press2);
            this.groupBox1.Controls.Add(this.genInfo2);
            this.groupBox1.Controls.Add(this.hpmjMainDiFlow);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.hpmjFilterOutPress);
            this.groupBox1.Controls.Add(this.hpmjFilterInPress);
            this.groupBox1.Controls.Add(this.hpmjPump);
            this.groupBox1.Controls.Add(this.hpmjBubblerOutPress);
            this.groupBox1.Controls.Add(this.hpmjCo2VentValve);
            this.groupBox1.Controls.Add(this.hpmjCo2InValve);
            this.groupBox1.Controls.Add(this.hpmjCo2Press);
            this.groupBox1.Controls.Add(this.sensor1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 103);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(255, 352);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sub-FAB";
            // 
            // hpmjCO2Flow
            // 
            this.hpmjCO2Flow.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCO2Flow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjCO2Flow.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCO2Flow.DeviceTagInfo")));
            this.hpmjCO2Flow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCO2Flow.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCO2Flow.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjCO2Flow.Location = new System.Drawing.Point(9, 163);
            this.hpmjCO2Flow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCO2Flow.Name = "hpmjCO2Flow";
            this.hpmjCO2Flow.Size = new System.Drawing.Size(74, 20);
            this.hpmjCO2Flow.TabIndex = 256;
            // 
            // hpmjCo2Press2
            // 
            this.hpmjCo2Press2.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCo2Press2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjCo2Press2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2Press2.DeviceTagInfo")));
            this.hpmjCo2Press2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2Press2.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjCo2Press2.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjCo2Press2.Location = new System.Drawing.Point(9, 200);
            this.hpmjCo2Press2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2Press2.Name = "hpmjCo2Press2";
            this.hpmjCo2Press2.Size = new System.Drawing.Size(74, 20);
            this.hpmjCo2Press2.TabIndex = 255;
            // 
            // genInfo2
            // 
            this.genInfo2.AutoHide = false;
            this.genInfo2.AutoHideLogic = true;
            this.genInfo2.BackColor = System.Drawing.Color.Transparent;
            this.genInfo2.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo2.DeviceTagInfo")));
            this.genInfo2.Distance = 1;
            this.genInfo2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.Location = new System.Drawing.Point(138, 56);
            this.genInfo2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.genInfo2.Name = "genInfo2";
            this.genInfo2.ReferenceTagDescriptor = ((Dms.Common.TagDescriptor)(resources.GetObject("genInfo2.ReferenceTagDescriptor")));
            this.genInfo2.Size = new System.Drawing.Size(107, 20);
            this.genInfo2.TabIndex = 254;
            this.genInfo2.TitleBackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.genInfo2.TitleForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo2.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo2.TitlePanelSize = 40;
            this.genInfo2.TitlePanelTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo2.TitleText = "Target";
            this.genInfo2.TitleTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.ValueBackColor = System.Drawing.Color.Cyan;
            this.genInfo2.ValueBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo2.ValueForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo2.ValueTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genInfo2.ValueTextFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo2.ValueTextUnit = Dms.Common.UnitType.Bar;
            // 
            // hpmjMainDiFlow
            // 
            this.hpmjMainDiFlow.BackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjMainDiFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjMainDiFlow.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjMainDiFlow.DeviceTagInfo")));
            this.hpmjMainDiFlow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjMainDiFlow.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.hpmjMainDiFlow.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.hpmjMainDiFlow.Location = new System.Drawing.Point(171, 228);
            this.hpmjMainDiFlow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjMainDiFlow.Name = "hpmjMainDiFlow";
            this.hpmjMainDiFlow.Size = new System.Drawing.Size(74, 20);
            this.hpmjMainDiFlow.TabIndex = 253;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Dms.HMI.Properties.Resources.ARROW_UP;
            this.pictureBox4.Location = new System.Drawing.Point(202, 295);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(13, 24);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 252;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Dms.HMI.Properties.Resources.ARROW_R;
            this.pictureBox2.Location = new System.Drawing.Point(0, 41);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 13);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 251;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Dms.HMI.Properties.Resources.ARROW_UP;
            this.pictureBox1.Location = new System.Drawing.Point(84, 295);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(13, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 250;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Dms.HMI.Properties.Resources.ARROW_UP;
            this.pictureBox3.Location = new System.Drawing.Point(38, 295);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(13, 24);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 249;
            this.pictureBox3.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(83, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "DI";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = "Vent";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "CO2";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(1, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 6);
            this.label8.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label15.Location = new System.Drawing.Point(154, 319);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 6);
            this.label15.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label11.Location = new System.Drawing.Point(92, 200);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 6);
            this.label11.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label10.Location = new System.Drawing.Point(127, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 6);
            this.label10.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label13.Location = new System.Drawing.Point(206, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(5, 309);
            this.label13.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label12.Location = new System.Drawing.Point(154, 127);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(5, 192);
            this.label12.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(89, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(5, 176);
            this.label5.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(127, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(5, 61);
            this.label7.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(43, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 189);
            this.label1.TabIndex = 0;
            // 
            // viewSetupHpmj
            // 
            this.viewSetupHpmj.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupHpmj.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupHpmj.Location = new System.Drawing.Point(473, 10);
            this.viewSetupHpmj.Name = "viewSetupHpmj";
            this.viewSetupHpmj.Size = new System.Drawing.Size(416, 445);
            this.viewSetupHpmj.TabIndex = 14;
            this.viewSetupHpmj.TitleName = "HPMJ Parameters";
            this.viewSetupHpmj.ViewOnly = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 200;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // JobsTabHpmjCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.viewSetupHpmj);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "JobsTabHpmjCtrl";
            this.Size = new System.Drawing.Size(901, 465);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private Dms.Control.Gauge hpmjShowerFlow;
        private Dms.Control.Shower hpmjShower;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkRemote;
        private Dms.Control.Gauge hpmjFilterInPress;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.CheckBox chkReady;
        private System.Windows.Forms.CheckBox chkWarning;
        private Dms.Control.Pump hpmjPump;
        private System.Windows.Forms.CheckBox chkAlarm;
        private Dms.Control.Gauge hpmjFilterOutPress;
        private Dms.Control.Gauge hpmjBubblerOutPress;
        private Dms.Control.AutoValve hpmjCo2VentValve;
        private Dms.Control.Gauge hpmjCo2Press;
        private System.Windows.Forms.CheckBox chkPing;
        private Dms.Control.AutoValve hpmjCo2InValve;
        private Dms.Control.Sensor sensor1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private Dms.Data.ViewSetupInfo viewSetupHpmj;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Timer tmrUpdateState;
        private Dms.Control.Gauge hpmjCurrent;
        private Dms.Control.GenInfo labelInverter;
        private Dms.Control.Gauge hpmjMainDiFlow;
        private Dms.Control.GenInfo genInfo1;
        private Dms.Control.GenInfo genInfo2;
        private Dms.Control.Gauge hpmjCo2Press2;
        private Dms.Control.Gauge hpmjCO2Flow;

    }
}

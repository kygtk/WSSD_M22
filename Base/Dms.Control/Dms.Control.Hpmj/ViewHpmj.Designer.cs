namespace Dms.Control
{
    partial class ViewHpmj
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewHpmj));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hpmjShowerFlow = new Dms.Control.Gauge();
            this.hpmjShower = new Dms.Control.Shower();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelInverter = new System.Windows.Forms.Label();
            this.hpmjFilterOutPress = new Dms.Control.Gauge();
            this.hpmjFilterInPress = new Dms.Control.Gauge();
            this.hpmjPump = new Dms.Control.Pump();
            this.hpmjBubblerOutPress = new Dms.Control.Gauge();
            this.hpmjCo2VentValve = new Dms.Control.AutoValve();
            this.hpmjCo2InValve = new Dms.Control.AutoValve();
            this.hpmjCo2InPress = new Dms.Control.Gauge();
            this.sensor1 = new Dms.Control.Sensor();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkRemote = new System.Windows.Forms.CheckBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.chkReady = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.chkPing = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnLocal = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnRemote = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.dataStateGridView = new Dms.Control.DoubleBufferedGridView();
            this.label18 = new System.Windows.Forms.Label();
            this.viewSetupHpmj = new Dms.Data.ViewSetupInfo();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStateGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.hpmjShowerFlow);
            this.groupBox2.Controls.Add(this.hpmjShower);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 8);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(255, 92);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EQP";
            // 
            // hpmjShowerFlow
            // 
            this.hpmjShowerFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjShowerFlow.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjShowerFlow.DeviceTagInfo")));
            this.hpmjShowerFlow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjShowerFlow.Location = new System.Drawing.Point(171, 39);
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
            this.label16.Size = new System.Drawing.Size(5, 71);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(43, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 189);
            this.label1.TabIndex = 0;
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Dms.Control.Properties.Resources.ARROW_UP;
            this.pictureBox1.Location = new System.Drawing.Point(39, 301);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(13, 25);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelInverter);
            this.groupBox1.Controls.Add(this.hpmjFilterOutPress);
            this.groupBox1.Controls.Add(this.hpmjFilterInPress);
            this.groupBox1.Controls.Add(this.hpmjPump);
            this.groupBox1.Controls.Add(this.hpmjBubblerOutPress);
            this.groupBox1.Controls.Add(this.hpmjCo2VentValve);
            this.groupBox1.Controls.Add(this.hpmjCo2InValve);
            this.groupBox1.Controls.Add(this.hpmjCo2InPress);
            this.groupBox1.Controls.Add(this.sensor1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.pictureBox5);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pictureBox1);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 101);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(255, 352);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sub-FAB";
            // 
            // labelInverter
            // 
            this.labelInverter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInverter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInverter.Location = new System.Drawing.Point(171, 224);
            this.labelInverter.Name = "labelInverter";
            this.labelInverter.Size = new System.Drawing.Size(74, 20);
            this.labelInverter.TabIndex = 18;
            this.labelInverter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hpmjFilterOutPress
            // 
            this.hpmjFilterOutPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjFilterOutPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjFilterOutPress.DeviceTagInfo")));
            this.hpmjFilterOutPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjFilterOutPress.Location = new System.Drawing.Point(171, 77);
            this.hpmjFilterOutPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjFilterOutPress.Name = "hpmjFilterOutPress";
            this.hpmjFilterOutPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjFilterOutPress.TabIndex = 17;
            // 
            // hpmjFilterInPress
            // 
            this.hpmjFilterInPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjFilterInPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjFilterInPress.DeviceTagInfo")));
            this.hpmjFilterInPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjFilterInPress.Location = new System.Drawing.Point(171, 163);
            this.hpmjFilterInPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjFilterInPress.Name = "hpmjFilterInPress";
            this.hpmjFilterInPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjFilterInPress.TabIndex = 16;
            // 
            // hpmjPump
            // 
            this.hpmjPump.BackColor = System.Drawing.Color.Transparent;
            this.hpmjPump.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjPump.DeviceTagInfo")));
            this.hpmjPump.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjPump.Location = new System.Drawing.Point(183, 191);
            this.hpmjPump.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjPump.Name = "hpmjPump";
            this.hpmjPump.PumpType = Dms.Control.Pump.DeviceType.Inverter;
            this.hpmjPump.Size = new System.Drawing.Size(50, 29);
            this.hpmjPump.TabIndex = 15;
            // 
            // hpmjBubblerOutPress
            // 
            this.hpmjBubblerOutPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjBubblerOutPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjBubblerOutPress.DeviceTagInfo")));
            this.hpmjBubblerOutPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.hpmjCo2VentValve.ValveType = Dms.Control.AutoValve.DeviceType.Horizon;
            // 
            // hpmjCo2InValve
            // 
            this.hpmjCo2InValve.BackColor = System.Drawing.Color.Transparent;
            this.hpmjCo2InValve.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2InValve.DeviceTagInfo")));
            this.hpmjCo2InValve.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2InValve.Location = new System.Drawing.Point(28, 259);
            this.hpmjCo2InValve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2InValve.Name = "hpmjCo2InValve";
            this.hpmjCo2InValve.Size = new System.Drawing.Size(28, 20);
            this.hpmjCo2InValve.TabIndex = 12;
            this.hpmjCo2InValve.ValveType = Dms.Control.AutoValve.DeviceType.Vertical;
            // 
            // hpmjCo2InPress
            // 
            this.hpmjCo2InPress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpmjCo2InPress.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("hpmjCo2InPress.DeviceTagInfo")));
            this.hpmjCo2InPress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpmjCo2InPress.Location = new System.Drawing.Point(8, 209);
            this.hpmjCo2InPress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hpmjCo2InPress.Name = "hpmjCo2InPress";
            this.hpmjCo2InPress.Size = new System.Drawing.Size(74, 20);
            this.hpmjCo2InPress.TabIndex = 11;
            // 
            // sensor1
            // 
            this.sensor1.Description = "";
            this.sensor1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("sensor1.DeviceTagInfo")));
            this.sensor1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensor1.Location = new System.Drawing.Point(154, 330);
            this.sensor1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.sensor1.Name = "sensor1";
            this.sensor1.OffColor = System.Drawing.Color.White;
            this.sensor1.OnColor = System.Drawing.Color.Lime;
            this.sensor1.Size = new System.Drawing.Size(67, 12);
            this.sensor1.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(8, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 49);
            this.label3.TabIndex = 3;
            this.label3.Text = "CO2 Bubbler";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Dms.Control.Properties.Resources.ARROW_UP;
            this.pictureBox3.Location = new System.Drawing.Point(202, 20);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(13, 25);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Dms.Control.Properties.Resources.ARROW_L;
            this.pictureBox4.Location = new System.Drawing.Point(7, 41);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(25, 13);
            this.pictureBox4.TabIndex = 2;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Dms.Control.Properties.Resources.ARROW_DN;
            this.pictureBox5.Location = new System.Drawing.Point(150, 290);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(13, 25);
            this.pictureBox5.TabIndex = 2;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Dms.Control.Properties.Resources.ARROW_UP;
            this.pictureBox2.Location = new System.Drawing.Point(85, 301);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(13, 25);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
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
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Silver;
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
            this.label7.BackColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(127, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(5, 61);
            this.label7.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkRemote);
            this.groupBox3.Controls.Add(this.chkRun);
            this.groupBox3.Controls.Add(this.chkReady);
            this.groupBox3.Controls.Add(this.chkWarning);
            this.groupBox3.Controls.Add(this.chkAlarm);
            this.groupBox3.Controls.Add(this.chkPing);
            this.groupBox3.Location = new System.Drawing.Point(274, 8);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(190, 92);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // chkRemote
            // 
            this.chkRemote.AutoSize = true;
            this.chkRemote.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chkRemote.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkRemote.Location = new System.Drawing.Point(108, 68);
            this.chkRemote.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRemote.Name = "chkRemote";
            this.chkRemote.Size = new System.Drawing.Size(73, 16);
            this.chkRemote.TabIndex = 5;
            this.chkRemote.Text = "Remote";
            this.chkRemote.UseVisualStyleBackColor = true;
            // 
            // chkRun
            // 
            this.chkRun.AutoSize = true;
            this.chkRun.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.chkReady.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.chkWarning.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkWarning.Location = new System.Drawing.Point(15, 69);
            this.chkWarning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(76, 16);
            this.chkWarning.TabIndex = 4;
            this.chkWarning.Text = "Warning";
            this.chkWarning.UseVisualStyleBackColor = true;
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAlarm.Location = new System.Drawing.Point(15, 45);
            this.chkAlarm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(62, 16);
            this.chkAlarm.TabIndex = 2;
            this.chkAlarm.Text = "Alarm";
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // chkPing
            // 
            this.chkPing.AutoSize = true;
            this.chkPing.Enabled = false;
            this.chkPing.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkPing.Location = new System.Drawing.Point(15, 22);
            this.chkPing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkPing.Name = "chkPing";
            this.chkPing.Size = new System.Drawing.Size(57, 16);
            this.chkPing.TabIndex = 0;
            this.chkPing.Text = "PING";
            this.chkPing.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnStop);
            this.groupBox4.Controls.Add(this.btnLocal);
            this.groupBox4.Controls.Add(this.btnStart);
            this.groupBox4.Controls.Add(this.btnRemote);
            this.groupBox4.Location = new System.Drawing.Point(274, 101);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(190, 352);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Operation";
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCyan;
            this.btnStop.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStop.Location = new System.Drawing.Point(10, 266);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(168, 73);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnLocal
            // 
            this.btnLocal.BackColor = System.Drawing.Color.MediumAquamarine;
            this.btnLocal.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLocal.Location = new System.Drawing.Point(10, 104);
            this.btnLocal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLocal.Name = "btnLocal";
            this.btnLocal.Size = new System.Drawing.Size(168, 73);
            this.btnLocal.TabIndex = 1;
            this.btnLocal.Text = "Local";
            this.btnLocal.UseVisualStyleBackColor = false;
            this.btnLocal.Click += new System.EventHandler(this.btnLocal_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Lime;
            this.btnStart.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.Location = new System.Drawing.Point(10, 185);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(168, 73);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnRemote
            // 
            this.btnRemote.BackColor = System.Drawing.Color.DarkOrange;
            this.btnRemote.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRemote.Location = new System.Drawing.Point(10, 23);
            this.btnRemote.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.Size = new System.Drawing.Size(168, 73);
            this.btnRemote.TabIndex = 0;
            this.btnRemote.Text = "Remote";
            this.btnRemote.UseVisualStyleBackColor = false;
            this.btnRemote.Click += new System.EventHandler(this.btnRemote_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 200;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // dataStateGridView
            // 
            this.dataStateGridView.AllowUserToAddRows = false;
            this.dataStateGridView.AllowUserToDeleteRows = false;
            this.dataStateGridView.AllowUserToResizeRows = false;
            this.dataStateGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataStateGridView.Location = new System.Drawing.Point(473, 42);
            this.dataStateGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataStateGridView.MultiSelect = false;
            this.dataStateGridView.Name = "dataStateGridView";
            this.dataStateGridView.RowHeadersVisible = false;
            this.dataStateGridView.RowTemplate.Height = 23;
            this.dataStateGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataStateGridView.Size = new System.Drawing.Size(416, 150);
            this.dataStateGridView.TabIndex = 5;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(473, 15);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(416, 24);
            this.label18.TabIndex = 6;
            this.label18.Text = "HPMJ Parts State";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // viewSetupHpmj
            // 
            this.viewSetupHpmj.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupHpmj.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupHpmj.Location = new System.Drawing.Point(473, 199);
            this.viewSetupHpmj.Name = "viewSetupHpmj";
            this.viewSetupHpmj.Size = new System.Drawing.Size(416, 254);
            this.viewSetupHpmj.TabIndex = 7;
            this.viewSetupHpmj.TitleName = "HPMJ Parameters";
            // 
            // ViewHpmj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewSetupHpmj);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.dataStateGridView);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ViewHpmj";
            this.Size = new System.Drawing.Size(903, 467);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataStateGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label4;
        private Gauge hpmjFilterOutPress;
        private Gauge hpmjFilterInPress;
        private Pump hpmjPump;
        private Gauge hpmjBubblerOutPress;
        private AutoValve hpmjCo2VentValve;
        private AutoValve hpmjCo2InValve;
        private Gauge hpmjCo2InPress;
        private Sensor sensor1;
        private Gauge hpmjShowerFlow;
        private Shower hpmjShower;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkRemote;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.CheckBox chkReady;
        private System.Windows.Forms.CheckBox chkWarning;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.CheckBox chkPing;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnLocal;
        private System.Windows.Forms.Button btnRemote;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer tmrUpdateState;
        private DoubleBufferedGridView dataStateGridView;
        private System.Windows.Forms.Label label18;
        private Dms.Data.ViewSetupInfo viewSetupHpmj;
        private System.Windows.Forms.Label labelInverter;
    }
}
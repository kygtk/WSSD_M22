namespace Dms.Control
{
    partial class ViewEyeEuv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewEyeEuv));
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbEuvStatus = new System.Windows.Forms.GroupBox();
            this.checkLamp2OkIn = new System.Windows.Forms.CheckBox();
            this.checkLamp1OkIn = new System.Windows.Forms.CheckBox();
            this.checkReadyIn = new System.Windows.Forms.CheckBox();
            this.checkRemoteIn = new System.Windows.Forms.CheckBox();
            this.checkTrouble2 = new System.Windows.Forms.CheckBox();
            this.checkTrouble1 = new System.Windows.Forms.CheckBox();
            this.checkLamp2TimeOverIn = new System.Windows.Forms.CheckBox();
            this.checkLamp1TimeOverIn = new System.Windows.Forms.CheckBox();
            this.gbManualOperation = new System.Windows.Forms.GroupBox();
            this.checkReset = new System.Windows.Forms.CheckBox();
            this.checkEmergencyStop = new System.Windows.Forms.CheckBox();
            this.checkLampCoolingCdaOk = new System.Windows.Forms.CheckBox();
            this.checkStanby = new System.Windows.Forms.CheckBox();
            this.checkRemoteOut = new System.Windows.Forms.CheckBox();
            this.checkWaterLeakOk = new System.Windows.Forms.CheckBox();
            this.checkWaterFlowOk = new System.Windows.Forms.CheckBox();
            this.checkLamp2On = new System.Windows.Forms.CheckBox();
            this.checkLamp1On = new System.Windows.Forms.CheckBox();
            this.chkDown = new System.Windows.Forms.CheckBox();
            this.gbHouse = new System.Windows.Forms.GroupBox();
            this.btnCylDown = new System.Windows.Forms.Button();
            this.btnCylUp = new System.Windows.Forms.Button();
            this.chkUp = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pbLamp2Status = new System.Windows.Forms.PictureBox();
            this.pbLamp1Status = new System.Windows.Forms.PictureBox();
            this.gbAlarmStatus = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkPcwOutValve = new System.Windows.Forms.CheckBox();
            this.checkPcwInValve = new System.Windows.Forms.CheckBox();
            this.checkN2Valve = new System.Windows.Forms.CheckBox();
            this.checkCDAValve = new System.Windows.Forms.CheckBox();
            this.N2Pressgauge = new Dms.Control.Gauge();
            this.PCWInPressgauge = new Dms.Control.Gauge();
            this.PCWOutPressgauge = new Dms.Control.Gauge();
            this.gbEuvStatus.SuspendLayout();
            this.gbManualOperation.SuspendLayout();
            this.gbHouse.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp2Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp1Status)).BeginInit();
            this.gbAlarmStatus.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // gbEuvStatus
            // 
            this.gbEuvStatus.Controls.Add(this.checkLamp2OkIn);
            this.gbEuvStatus.Controls.Add(this.checkLamp1OkIn);
            this.gbEuvStatus.Controls.Add(this.checkReadyIn);
            this.gbEuvStatus.Controls.Add(this.checkRemoteIn);
            this.gbEuvStatus.Location = new System.Drawing.Point(647, 3);
            this.gbEuvStatus.Name = "gbEuvStatus";
            this.gbEuvStatus.Size = new System.Drawing.Size(229, 123);
            this.gbEuvStatus.TabIndex = 0;
            this.gbEuvStatus.TabStop = false;
            this.gbEuvStatus.Text = "Euv State";
            // 
            // checkLamp2OkIn
            // 
            this.checkLamp2OkIn.AutoSize = true;
            this.checkLamp2OkIn.Location = new System.Drawing.Point(12, 98);
            this.checkLamp2OkIn.Name = "checkLamp2OkIn";
            this.checkLamp2OkIn.Size = new System.Drawing.Size(107, 16);
            this.checkLamp2OkIn.TabIndex = 3;
            this.checkLamp2OkIn.Text = "Lamp2 OK/NG";
            this.checkLamp2OkIn.UseVisualStyleBackColor = true;
            // 
            // checkLamp1OkIn
            // 
            this.checkLamp1OkIn.AutoSize = true;
            this.checkLamp1OkIn.Location = new System.Drawing.Point(12, 76);
            this.checkLamp1OkIn.Name = "checkLamp1OkIn";
            this.checkLamp1OkIn.Size = new System.Drawing.Size(107, 16);
            this.checkLamp1OkIn.TabIndex = 2;
            this.checkLamp1OkIn.Text = "Lamp1 OK/NG";
            this.checkLamp1OkIn.UseVisualStyleBackColor = true;
            // 
            // checkReadyIn
            // 
            this.checkReadyIn.AutoSize = true;
            this.checkReadyIn.Location = new System.Drawing.Point(12, 54);
            this.checkReadyIn.Name = "checkReadyIn";
            this.checkReadyIn.Size = new System.Drawing.Size(121, 16);
            this.checkReadyIn.TabIndex = 1;
            this.checkReadyIn.Text = "Ready/NotReady";
            this.checkReadyIn.UseVisualStyleBackColor = true;
            // 
            // checkRemoteIn
            // 
            this.checkRemoteIn.AutoSize = true;
            this.checkRemoteIn.Location = new System.Drawing.Point(12, 32);
            this.checkRemoteIn.Name = "checkRemoteIn";
            this.checkRemoteIn.Size = new System.Drawing.Size(104, 16);
            this.checkRemoteIn.TabIndex = 0;
            this.checkRemoteIn.Text = "Remote/Local";
            this.checkRemoteIn.UseVisualStyleBackColor = true;
            // 
            // checkTrouble2
            // 
            this.checkTrouble2.AutoSize = true;
            this.checkTrouble2.Location = new System.Drawing.Point(12, 88);
            this.checkTrouble2.Name = "checkTrouble2";
            this.checkTrouble2.Size = new System.Drawing.Size(131, 16);
            this.checkTrouble2.TabIndex = 7;
            this.checkTrouble2.Text = "Trouble2 (OutSide)";
            this.checkTrouble2.UseVisualStyleBackColor = true;
            // 
            // checkTrouble1
            // 
            this.checkTrouble1.AutoSize = true;
            this.checkTrouble1.Location = new System.Drawing.Point(12, 66);
            this.checkTrouble1.Name = "checkTrouble1";
            this.checkTrouble1.Size = new System.Drawing.Size(121, 16);
            this.checkTrouble1.TabIndex = 6;
            this.checkTrouble1.Text = "Trouble1 (Inside)";
            this.checkTrouble1.UseVisualStyleBackColor = true;
            // 
            // checkLamp2TimeOverIn
            // 
            this.checkLamp2TimeOverIn.AutoSize = true;
            this.checkLamp2TimeOverIn.Location = new System.Drawing.Point(12, 44);
            this.checkLamp2TimeOverIn.Name = "checkLamp2TimeOverIn";
            this.checkLamp2TimeOverIn.Size = new System.Drawing.Size(121, 16);
            this.checkLamp2TimeOverIn.TabIndex = 5;
            this.checkLamp2TimeOverIn.Text = "Lamp2 TimeOver";
            this.checkLamp2TimeOverIn.UseVisualStyleBackColor = true;
            // 
            // checkLamp1TimeOverIn
            // 
            this.checkLamp1TimeOverIn.AutoSize = true;
            this.checkLamp1TimeOverIn.Location = new System.Drawing.Point(12, 22);
            this.checkLamp1TimeOverIn.Name = "checkLamp1TimeOverIn";
            this.checkLamp1TimeOverIn.Size = new System.Drawing.Size(121, 16);
            this.checkLamp1TimeOverIn.TabIndex = 4;
            this.checkLamp1TimeOverIn.Text = "Lamp1 TimeOver";
            this.checkLamp1TimeOverIn.UseVisualStyleBackColor = true;
            // 
            // gbManualOperation
            // 
            this.gbManualOperation.Controls.Add(this.checkReset);
            this.gbManualOperation.Controls.Add(this.checkEmergencyStop);
            this.gbManualOperation.Controls.Add(this.checkLampCoolingCdaOk);
            this.gbManualOperation.Controls.Add(this.checkStanby);
            this.gbManualOperation.Controls.Add(this.checkRemoteOut);
            this.gbManualOperation.Controls.Add(this.checkWaterLeakOk);
            this.gbManualOperation.Controls.Add(this.checkWaterFlowOk);
            this.gbManualOperation.Location = new System.Drawing.Point(392, 0);
            this.gbManualOperation.Name = "gbManualOperation";
            this.gbManualOperation.Size = new System.Drawing.Size(249, 409);
            this.gbManualOperation.TabIndex = 1;
            this.gbManualOperation.TabStop = false;
            this.gbManualOperation.Text = "EUV Manual Operation";
            // 
            // checkReset
            // 
            this.checkReset.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkReset.BackColor = System.Drawing.Color.Honeydew;
            this.checkReset.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkReset.Location = new System.Drawing.Point(27, 353);
            this.checkReset.Name = "checkReset";
            this.checkReset.Size = new System.Drawing.Size(200, 50);
            this.checkReset.TabIndex = 16;
            this.checkReset.Text = "Reset";
            this.checkReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkReset.UseVisualStyleBackColor = false;
            this.checkReset.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkEmergencyStop
            // 
            this.checkEmergencyStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkEmergencyStop.BackColor = System.Drawing.Color.Honeydew;
            this.checkEmergencyStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkEmergencyStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkEmergencyStop.Location = new System.Drawing.Point(27, 297);
            this.checkEmergencyStop.Name = "checkEmergencyStop";
            this.checkEmergencyStop.Size = new System.Drawing.Size(200, 50);
            this.checkEmergencyStop.TabIndex = 15;
            this.checkEmergencyStop.Text = "Emergency Stop";
            this.checkEmergencyStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkEmergencyStop.UseVisualStyleBackColor = false;
            this.checkEmergencyStop.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkLampCoolingCdaOk
            // 
            this.checkLampCoolingCdaOk.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkLampCoolingCdaOk.BackColor = System.Drawing.Color.Honeydew;
            this.checkLampCoolingCdaOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkLampCoolingCdaOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkLampCoolingCdaOk.Location = new System.Drawing.Point(27, 241);
            this.checkLampCoolingCdaOk.Name = "checkLampCoolingCdaOk";
            this.checkLampCoolingCdaOk.Size = new System.Drawing.Size(200, 50);
            this.checkLampCoolingCdaOk.TabIndex = 14;
            this.checkLampCoolingCdaOk.Text = "Lamp Cooling CDA OK";
            this.checkLampCoolingCdaOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkLampCoolingCdaOk.UseVisualStyleBackColor = false;
            this.checkLampCoolingCdaOk.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkStanby
            // 
            this.checkStanby.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkStanby.BackColor = System.Drawing.Color.Honeydew;
            this.checkStanby.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkStanby.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkStanby.Location = new System.Drawing.Point(27, 73);
            this.checkStanby.Name = "checkStanby";
            this.checkStanby.Size = new System.Drawing.Size(200, 50);
            this.checkStanby.TabIndex = 9;
            this.checkStanby.Text = "Stanby";
            this.checkStanby.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkStanby.UseVisualStyleBackColor = false;
            this.checkStanby.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkRemoteOut
            // 
            this.checkRemoteOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkRemoteOut.BackColor = System.Drawing.Color.Honeydew;
            this.checkRemoteOut.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkRemoteOut.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkRemoteOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkRemoteOut.Location = new System.Drawing.Point(27, 17);
            this.checkRemoteOut.Name = "checkRemoteOut";
            this.checkRemoteOut.Size = new System.Drawing.Size(200, 50);
            this.checkRemoteOut.TabIndex = 8;
            this.checkRemoteOut.Text = "Remote/Local";
            this.checkRemoteOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkRemoteOut.UseVisualStyleBackColor = false;
            this.checkRemoteOut.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkWaterLeakOk
            // 
            this.checkWaterLeakOk.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkWaterLeakOk.BackColor = System.Drawing.Color.Honeydew;
            this.checkWaterLeakOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkWaterLeakOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkWaterLeakOk.Location = new System.Drawing.Point(27, 129);
            this.checkWaterLeakOk.Name = "checkWaterLeakOk";
            this.checkWaterLeakOk.Size = new System.Drawing.Size(200, 50);
            this.checkWaterLeakOk.TabIndex = 13;
            this.checkWaterLeakOk.Text = "Water Leak OK";
            this.checkWaterLeakOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkWaterLeakOk.UseVisualStyleBackColor = false;
            this.checkWaterLeakOk.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkWaterFlowOk
            // 
            this.checkWaterFlowOk.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkWaterFlowOk.BackColor = System.Drawing.Color.Honeydew;
            this.checkWaterFlowOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkWaterFlowOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkWaterFlowOk.Location = new System.Drawing.Point(27, 185);
            this.checkWaterFlowOk.Name = "checkWaterFlowOk";
            this.checkWaterFlowOk.Size = new System.Drawing.Size(200, 50);
            this.checkWaterFlowOk.TabIndex = 12;
            this.checkWaterFlowOk.Text = "Water Flow OK";
            this.checkWaterFlowOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkWaterFlowOk.UseVisualStyleBackColor = false;
            this.checkWaterFlowOk.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkLamp2On
            // 
            this.checkLamp2On.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkLamp2On.BackColor = System.Drawing.Color.YellowGreen;
            this.checkLamp2On.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkLamp2On.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkLamp2On.Location = new System.Drawing.Point(35, 80);
            this.checkLamp2On.Name = "checkLamp2On";
            this.checkLamp2On.Size = new System.Drawing.Size(130, 50);
            this.checkLamp2On.TabIndex = 11;
            this.checkLamp2On.Text = "Lamp2 ON";
            this.checkLamp2On.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkLamp2On.UseVisualStyleBackColor = false;
            this.checkLamp2On.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkLamp1On
            // 
            this.checkLamp1On.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkLamp1On.BackColor = System.Drawing.Color.YellowGreen;
            this.checkLamp1On.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkLamp1On.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkLamp1On.Location = new System.Drawing.Point(35, 20);
            this.checkLamp1On.Name = "checkLamp1On";
            this.checkLamp1On.Size = new System.Drawing.Size(130, 50);
            this.checkLamp1On.TabIndex = 10;
            this.checkLamp1On.Text = "Lamp1 ON";
            this.checkLamp1On.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkLamp1On.UseVisualStyleBackColor = false;
            this.checkLamp1On.Click += new System.EventHandler(this.Check_Click);
            // 
            // chkDown
            // 
            this.chkDown.AutoCheck = false;
            this.chkDown.AutoSize = true;
            this.chkDown.Location = new System.Drawing.Point(10, 115);
            this.chkDown.Name = "chkDown";
            this.chkDown.Size = new System.Drawing.Size(105, 16);
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
            this.gbHouse.Location = new System.Drawing.Point(0, 195);
            this.gbHouse.Name = "gbHouse";
            this.gbHouse.Size = new System.Drawing.Size(174, 208);
            this.gbHouse.TabIndex = 20;
            this.gbHouse.TabStop = false;
            this.gbHouse.Text = "Euv House";
            // 
            // btnCylDown
            // 
            this.btnCylDown.BackColor = System.Drawing.Color.LightCyan;
            this.btnCylDown.Location = new System.Drawing.Point(10, 137);
            this.btnCylDown.Name = "btnCylDown";
            this.btnCylDown.Size = new System.Drawing.Size(147, 58);
            this.btnCylDown.TabIndex = 2;
            this.btnCylDown.Text = "DOWN";
            this.btnCylDown.UseVisualStyleBackColor = false;
            this.btnCylDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnClick);
            this.btnCylDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnRelease);
            // 
            // btnCylUp
            // 
            this.btnCylUp.BackColor = System.Drawing.Color.Linen;
            this.btnCylUp.Location = new System.Drawing.Point(10, 42);
            this.btnCylUp.Name = "btnCylUp";
            this.btnCylUp.Size = new System.Drawing.Size(147, 58);
            this.btnCylUp.TabIndex = 1;
            this.btnCylUp.Text = "UP";
            this.btnCylUp.UseVisualStyleBackColor = false;
            this.btnCylUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnClick);
            this.btnCylUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnRelease);
            // 
            // chkUp
            // 
            this.chkUp.AutoCheck = false;
            this.chkUp.AutoSize = true;
            this.chkUp.Location = new System.Drawing.Point(10, 20);
            this.chkUp.Name = "chkUp";
            this.chkUp.Size = new System.Drawing.Size(88, 16);
            this.chkUp.TabIndex = 0;
            this.chkUp.Text = "Up Position";
            this.chkUp.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pbLamp2Status);
            this.groupBox4.Controls.Add(this.pbLamp1Status);
            this.groupBox4.Controls.Add(this.checkLamp1On);
            this.groupBox4.Controls.Add(this.checkLamp2On);
            this.groupBox4.Location = new System.Drawing.Point(215, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(171, 149);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Lamp On";
            // 
            // pbLamp2Status
            // 
            this.pbLamp2Status.BackColor = System.Drawing.Color.DarkGray;
            this.pbLamp2Status.Location = new System.Drawing.Point(9, 97);
            this.pbLamp2Status.Name = "pbLamp2Status";
            this.pbLamp2Status.Size = new System.Drawing.Size(20, 17);
            this.pbLamp2Status.TabIndex = 36;
            this.pbLamp2Status.TabStop = false;
            // 
            // pbLamp1Status
            // 
            this.pbLamp1Status.BackColor = System.Drawing.Color.DarkGray;
            this.pbLamp1Status.Location = new System.Drawing.Point(9, 32);
            this.pbLamp1Status.Name = "pbLamp1Status";
            this.pbLamp1Status.Size = new System.Drawing.Size(20, 17);
            this.pbLamp1Status.TabIndex = 35;
            this.pbLamp1Status.TabStop = false;
            // 
            // gbAlarmStatus
            // 
            this.gbAlarmStatus.Controls.Add(this.checkTrouble2);
            this.gbAlarmStatus.Controls.Add(this.checkTrouble1);
            this.gbAlarmStatus.Controls.Add(this.checkLamp1TimeOverIn);
            this.gbAlarmStatus.Controls.Add(this.checkLamp2TimeOverIn);
            this.gbAlarmStatus.Location = new System.Drawing.Point(647, 142);
            this.gbAlarmStatus.Name = "gbAlarmStatus";
            this.gbAlarmStatus.Size = new System.Drawing.Size(229, 113);
            this.gbAlarmStatus.TabIndex = 8;
            this.gbAlarmStatus.TabStop = false;
            this.gbAlarmStatus.Text = "Alarm Status";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.N2Pressgauge);
            this.groupBox6.Controls.Add(this.PCWInPressgauge);
            this.groupBox6.Controls.Add(this.PCWOutPressgauge);
            this.groupBox6.Location = new System.Drawing.Point(3, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(171, 167);
            this.groupBox6.TabIndex = 37;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Util Monitor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(5, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "Pcw Out Pressure";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "Pcw In Pressure";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 12);
            this.label1.TabIndex = 38;
            this.label1.Text = "N2 Pressure";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkPcwOutValve);
            this.groupBox7.Controls.Add(this.checkPcwInValve);
            this.groupBox7.Controls.Add(this.checkN2Valve);
            this.groupBox7.Controls.Add(this.checkCDAValve);
            this.groupBox7.Location = new System.Drawing.Point(215, 164);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(171, 240);
            this.groupBox7.TabIndex = 37;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Util Valve";
            // 
            // checkPcwOutValve
            // 
            this.checkPcwOutValve.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkPcwOutValve.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkPcwOutValve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkPcwOutValve.Location = new System.Drawing.Point(19, 189);
            this.checkPcwOutValve.Name = "checkPcwOutValve";
            this.checkPcwOutValve.Size = new System.Drawing.Size(130, 50);
            this.checkPcwOutValve.TabIndex = 13;
            this.checkPcwOutValve.Text = "PCW Out Valve";
            this.checkPcwOutValve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkPcwOutValve.UseVisualStyleBackColor = false;
            this.checkPcwOutValve.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkPcwInValve
            // 
            this.checkPcwInValve.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkPcwInValve.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkPcwInValve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkPcwInValve.Location = new System.Drawing.Point(19, 133);
            this.checkPcwInValve.Name = "checkPcwInValve";
            this.checkPcwInValve.Size = new System.Drawing.Size(130, 50);
            this.checkPcwInValve.TabIndex = 12;
            this.checkPcwInValve.Text = "PCW In Valve";
            this.checkPcwInValve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkPcwInValve.UseVisualStyleBackColor = false;
            this.checkPcwInValve.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkN2Valve
            // 
            this.checkN2Valve.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkN2Valve.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkN2Valve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkN2Valve.Location = new System.Drawing.Point(19, 20);
            this.checkN2Valve.Name = "checkN2Valve";
            this.checkN2Valve.Size = new System.Drawing.Size(130, 50);
            this.checkN2Valve.TabIndex = 10;
            this.checkN2Valve.Text = "N2 In Valve";
            this.checkN2Valve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkN2Valve.UseVisualStyleBackColor = false;
            this.checkN2Valve.Click += new System.EventHandler(this.Check_Click);
            // 
            // checkCDAValve
            // 
            this.checkCDAValve.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkCDAValve.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkCDAValve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkCDAValve.Location = new System.Drawing.Point(19, 77);
            this.checkCDAValve.Name = "checkCDAValve";
            this.checkCDAValve.Size = new System.Drawing.Size(130, 50);
            this.checkCDAValve.TabIndex = 11;
            this.checkCDAValve.Text = "CDA In Valve";
            this.checkCDAValve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkCDAValve.UseVisualStyleBackColor = false;
            this.checkCDAValve.Click += new System.EventHandler(this.Check_Click);
            // 
            // N2Pressgauge
            // 
            this.N2Pressgauge.BackColor = System.Drawing.SystemColors.GrayText;
            this.N2Pressgauge.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("N2Pressgauge.DeviceTagInfo")));
            this.N2Pressgauge.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.N2Pressgauge.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.N2Pressgauge.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.N2Pressgauge.Location = new System.Drawing.Point(6, 32);
            this.N2Pressgauge.Name = "N2Pressgauge";
            this.N2Pressgauge.Size = new System.Drawing.Size(76, 20);
            this.N2Pressgauge.TabIndex = 0;
            // 
            // PCWInPressgauge
            // 
            this.PCWInPressgauge.BackColor = System.Drawing.SystemColors.GrayText;
            this.PCWInPressgauge.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("PCWInPressgauge.DeviceTagInfo")));
            this.PCWInPressgauge.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PCWInPressgauge.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.PCWInPressgauge.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.PCWInPressgauge.Location = new System.Drawing.Point(6, 79);
            this.PCWInPressgauge.Name = "PCWInPressgauge";
            this.PCWInPressgauge.Size = new System.Drawing.Size(76, 20);
            this.PCWInPressgauge.TabIndex = 1;
            // 
            // PCWOutPressgauge
            // 
            this.PCWOutPressgauge.BackColor = System.Drawing.SystemColors.GrayText;
            this.PCWOutPressgauge.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("PCWOutPressgauge.DeviceTagInfo")));
            this.PCWOutPressgauge.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PCWOutPressgauge.GaugeBackColor = System.Drawing.SystemColors.GrayText;
            this.PCWOutPressgauge.GaugeImageType = Dms.Control.Gauge.ImageType.Default;
            this.PCWOutPressgauge.Location = new System.Drawing.Point(6, 129);
            this.PCWOutPressgauge.Name = "PCWOutPressgauge";
            this.PCWOutPressgauge.Size = new System.Drawing.Size(76, 20);
            this.PCWOutPressgauge.TabIndex = 2;
            // 
            // ViewEyeEuv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.gbAlarmStatus);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbHouse);
            this.Controls.Add(this.gbManualOperation);
            this.Controls.Add(this.gbEuvStatus);
            this.Name = "ViewEyeEuv";
            this.Size = new System.Drawing.Size(899, 407);
            this.gbEuvStatus.ResumeLayout(false);
            this.gbEuvStatus.PerformLayout();
            this.gbManualOperation.ResumeLayout(false);
            this.gbHouse.ResumeLayout(false);
            this.gbHouse.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp2Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp1Status)).EndInit();
            this.gbAlarmStatus.ResumeLayout(false);
            this.gbAlarmStatus.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.GroupBox gbEuvStatus;
        private System.Windows.Forms.GroupBox gbManualOperation;
        private System.Windows.Forms.CheckBox chkDown;
        private System.Windows.Forms.GroupBox gbHouse;
        private System.Windows.Forms.Button btnCylDown;
        private System.Windows.Forms.Button btnCylUp;
        private System.Windows.Forms.CheckBox chkUp;
        private System.Windows.Forms.CheckBox checkTrouble2;
        private System.Windows.Forms.CheckBox checkTrouble1;
        private System.Windows.Forms.CheckBox checkLamp2TimeOverIn;
        private System.Windows.Forms.CheckBox checkLamp1TimeOverIn;
        private System.Windows.Forms.CheckBox checkLamp2OkIn;
        private System.Windows.Forms.CheckBox checkLamp1OkIn;
        private System.Windows.Forms.CheckBox checkReadyIn;
        private System.Windows.Forms.CheckBox checkRemoteIn;
        private System.Windows.Forms.CheckBox checkReset;
        private System.Windows.Forms.CheckBox checkEmergencyStop;
        private System.Windows.Forms.CheckBox checkLampCoolingCdaOk;
        private System.Windows.Forms.CheckBox checkWaterLeakOk;
        private System.Windows.Forms.CheckBox checkWaterFlowOk;
        private System.Windows.Forms.CheckBox checkLamp2On;
        private System.Windows.Forms.CheckBox checkLamp1On;
        private System.Windows.Forms.CheckBox checkStanby;
        private System.Windows.Forms.CheckBox checkRemoteOut;
        private Gauge PCWOutPressgauge;
        private Gauge PCWInPressgauge;
        private Gauge N2Pressgauge;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox pbLamp2Status;
        private System.Windows.Forms.PictureBox pbLamp1Status;
        private System.Windows.Forms.GroupBox gbAlarmStatus;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox checkN2Valve;
        private System.Windows.Forms.CheckBox checkCDAValve;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkPcwOutValve;
        private System.Windows.Forms.CheckBox checkPcwInValve;
    }
}

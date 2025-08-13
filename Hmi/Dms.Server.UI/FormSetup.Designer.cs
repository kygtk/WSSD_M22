namespace Dms.Server
{
    partial class FormSetup
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGen = new System.Windows.Forms.TabPage();
            this.viewSetupGenInfo = new Dms.Data.ViewSetupInfo();
            this.buttonSaveGen = new System.Windows.Forms.Button();
            this.tabPageIdle = new System.Windows.Forms.TabPage();
            this.viewSetupIdleInfo = new Dms.Data.ViewSetupInfo();
            this.buttonSaveIdle = new System.Windows.Forms.Button();
            this.tabPageTankLevel = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.viewSetupTankLevel = new Dms.Data.ViewSetupInfo();
            this.tabPageGaugeInterlock = new System.Windows.Forms.TabPage();
            this.buttonSaveGaugeInterlock = new System.Windows.Forms.Button();
            this.viewSetupGaugeInterlock = new Dms.Data.ViewSetupInfo();
            this.tabPageSensor = new System.Windows.Forms.TabPage();
            this.buttonSaveSenIntr = new System.Windows.Forms.Button();
            this.viewSetupSensorInterlock = new Dms.Data.ViewSetupInfo();
            this.tabPageCvDistance = new System.Windows.Forms.TabPage();
            this.viewSetupSensorTimeout = new Dms.Data.ViewSetupInfo();
            this.buttonSaveDistance = new System.Windows.Forms.Button();
            this.viewSetupCvDistance = new Dms.Data.ViewSetupInfo();
            this.tabPageCvMotor = new System.Windows.Forms.TabPage();
            this.buttonSaveCv = new System.Windows.Forms.Button();
            this.viewSetupCvInfo = new Dms.Data.ViewSetupCvInfo();
            this.tabPageCal = new System.Windows.Forms.TabPage();
            this.buttonSaveCal = new System.Windows.Forms.Button();
            this.viewCalibration = new Dms.Data.ViewSetupInfo();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageGen.SuspendLayout();
            this.tabPageIdle.SuspendLayout();
            this.tabPageTankLevel.SuspendLayout();
            this.tabPageGaugeInterlock.SuspendLayout();
            this.tabPageSensor.SuspendLayout();
            this.tabPageCvDistance.SuspendLayout();
            this.tabPageCvMotor.SuspendLayout();
            this.tabPageCal.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGen);
            this.tabControl1.Controls.Add(this.tabPageIdle);
            this.tabControl1.Controls.Add(this.tabPageTankLevel);
            this.tabControl1.Controls.Add(this.tabPageGaugeInterlock);
            this.tabControl1.Controls.Add(this.tabPageSensor);
            this.tabControl1.Controls.Add(this.tabPageCvDistance);
            this.tabControl1.Controls.Add(this.tabPageCvMotor);
            this.tabControl1.Controls.Add(this.tabPageCal);
            this.tabControl1.Location = new System.Drawing.Point(3, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(766, 430);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGen
            // 
            this.tabPageGen.Controls.Add(this.viewSetupGenInfo);
            this.tabPageGen.Controls.Add(this.buttonSaveGen);
            this.tabPageGen.Location = new System.Drawing.Point(4, 24);
            this.tabPageGen.Name = "tabPageGen";
            this.tabPageGen.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGen.Size = new System.Drawing.Size(758, 402);
            this.tabPageGen.TabIndex = 0;
            this.tabPageGen.Text = "General";
            this.tabPageGen.UseVisualStyleBackColor = true;
            // 
            // viewSetupGenInfo
            // 
            this.viewSetupGenInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupGenInfo.Location = new System.Drawing.Point(6, 15);
            this.viewSetupGenInfo.Name = "viewSetupGenInfo";
            this.viewSetupGenInfo.Size = new System.Drawing.Size(478, 359);
            this.viewSetupGenInfo.TabIndex = 1;
            this.viewSetupGenInfo.TitleName = "General Setup Info";
            // 
            // buttonSaveGen
            // 
            this.buttonSaveGen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveGen.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveGen.Name = "buttonSaveGen";
            this.buttonSaveGen.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveGen.TabIndex = 0;
            this.buttonSaveGen.Text = "Save";
            this.buttonSaveGen.UseVisualStyleBackColor = true;
            this.buttonSaveGen.Click += new System.EventHandler(this.buttonSaveGen_Click);
            // 
            // tabPageIdle
            // 
            this.tabPageIdle.Controls.Add(this.viewSetupIdleInfo);
            this.tabPageIdle.Controls.Add(this.buttonSaveIdle);
            this.tabPageIdle.Location = new System.Drawing.Point(4, 21);
            this.tabPageIdle.Name = "tabPageIdle";
            this.tabPageIdle.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIdle.Size = new System.Drawing.Size(758, 405);
            this.tabPageIdle.TabIndex = 1;
            this.tabPageIdle.Text = "Idle Run";
            this.tabPageIdle.UseVisualStyleBackColor = true;
            // 
            // viewSetupIdleInfo
            // 
            this.viewSetupIdleInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupIdleInfo.Location = new System.Drawing.Point(6, 15);
            this.viewSetupIdleInfo.Name = "viewSetupIdleInfo";
            this.viewSetupIdleInfo.Size = new System.Drawing.Size(478, 362);
            this.viewSetupIdleInfo.TabIndex = 3;
            this.viewSetupIdleInfo.TitleName = "Idle Run Info";
            // 
            // buttonSaveIdle
            // 
            this.buttonSaveIdle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveIdle.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveIdle.Name = "buttonSaveIdle";
            this.buttonSaveIdle.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveIdle.TabIndex = 2;
            this.buttonSaveIdle.Text = "Save";
            this.buttonSaveIdle.UseVisualStyleBackColor = true;
            this.buttonSaveIdle.Click += new System.EventHandler(this.buttonSaveIdle_Click);
            // 
            // tabPageTankLevel
            // 
            this.tabPageTankLevel.Controls.Add(this.button1);
            this.tabPageTankLevel.Controls.Add(this.viewSetupTankLevel);
            this.tabPageTankLevel.Location = new System.Drawing.Point(4, 21);
            this.tabPageTankLevel.Name = "tabPageTankLevel";
            this.tabPageTankLevel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTankLevel.Size = new System.Drawing.Size(758, 405);
            this.tabPageTankLevel.TabIndex = 7;
            this.tabPageTankLevel.Text = "Tank Level";
            this.tabPageTankLevel.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(621, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 45);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonTankLevel_Click);
            // 
            // viewSetupTankLevel
            // 
            this.viewSetupTankLevel.Location = new System.Drawing.Point(6, 15);
            this.viewSetupTankLevel.Name = "viewSetupTankLevel";
            this.viewSetupTankLevel.Size = new System.Drawing.Size(478, 362);
            this.viewSetupTankLevel.TabIndex = 0;
            this.viewSetupTankLevel.TitleName = "Tank Level Info";
            // 
            // tabPageGaugeInterlock
            // 
            this.tabPageGaugeInterlock.Controls.Add(this.buttonSaveGaugeInterlock);
            this.tabPageGaugeInterlock.Controls.Add(this.viewSetupGaugeInterlock);
            this.tabPageGaugeInterlock.Location = new System.Drawing.Point(4, 21);
            this.tabPageGaugeInterlock.Name = "tabPageGaugeInterlock";
            this.tabPageGaugeInterlock.Size = new System.Drawing.Size(758, 405);
            this.tabPageGaugeInterlock.TabIndex = 6;
            this.tabPageGaugeInterlock.Text = "Gauge Interlock";
            this.tabPageGaugeInterlock.UseVisualStyleBackColor = true;
            // 
            // buttonSaveGaugeInterlock
            // 
            this.buttonSaveGaugeInterlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveGaugeInterlock.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveGaugeInterlock.Name = "buttonSaveGaugeInterlock";
            this.buttonSaveGaugeInterlock.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveGaugeInterlock.TabIndex = 1;
            this.buttonSaveGaugeInterlock.Text = "Save";
            this.buttonSaveGaugeInterlock.UseVisualStyleBackColor = true;
            this.buttonSaveGaugeInterlock.Click += new System.EventHandler(this.buttonSaveGaugeInterlock_Click);
            // 
            // viewSetupGaugeInterlock
            // 
            this.viewSetupGaugeInterlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupGaugeInterlock.Location = new System.Drawing.Point(6, 15);
            this.viewSetupGaugeInterlock.Name = "viewSetupGaugeInterlock";
            this.viewSetupGaugeInterlock.Size = new System.Drawing.Size(568, 362);
            this.viewSetupGaugeInterlock.TabIndex = 0;
            this.viewSetupGaugeInterlock.TitleName = "Gauge Interlock";
            // 
            // tabPageSensor
            // 
            this.tabPageSensor.Controls.Add(this.buttonSaveSenIntr);
            this.tabPageSensor.Controls.Add(this.viewSetupSensorInterlock);
            this.tabPageSensor.Location = new System.Drawing.Point(4, 21);
            this.tabPageSensor.Name = "tabPageSensor";
            this.tabPageSensor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSensor.Size = new System.Drawing.Size(758, 405);
            this.tabPageSensor.TabIndex = 4;
            this.tabPageSensor.Text = "Sensor Interlock";
            this.tabPageSensor.UseVisualStyleBackColor = true;
            // 
            // buttonSaveSenIntr
            // 
            this.buttonSaveSenIntr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSenIntr.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveSenIntr.Name = "buttonSaveSenIntr";
            this.buttonSaveSenIntr.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveSenIntr.TabIndex = 2;
            this.buttonSaveSenIntr.Text = "Save";
            this.buttonSaveSenIntr.UseVisualStyleBackColor = true;
            this.buttonSaveSenIntr.Click += new System.EventHandler(this.buttonSaveSenIntr_Click);
            // 
            // viewSetupSensorInterlock
            // 
            this.viewSetupSensorInterlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupSensorInterlock.Location = new System.Drawing.Point(6, 15);
            this.viewSetupSensorInterlock.Name = "viewSetupSensorInterlock";
            this.viewSetupSensorInterlock.Size = new System.Drawing.Size(478, 362);
            this.viewSetupSensorInterlock.TabIndex = 0;
            this.viewSetupSensorInterlock.TitleName = "Sensor Interlock";
            // 
            // tabPageCvDistance
            // 
            this.tabPageCvDistance.Controls.Add(this.viewSetupSensorTimeout);
            this.tabPageCvDistance.Controls.Add(this.buttonSaveDistance);
            this.tabPageCvDistance.Controls.Add(this.viewSetupCvDistance);
            this.tabPageCvDistance.Location = new System.Drawing.Point(4, 21);
            this.tabPageCvDistance.Name = "tabPageCvDistance";
            this.tabPageCvDistance.Size = new System.Drawing.Size(758, 405);
            this.tabPageCvDistance.TabIndex = 5;
            this.tabPageCvDistance.Text = "Cv Distance";
            this.tabPageCvDistance.UseVisualStyleBackColor = true;
            // 
            // viewSetupSensorTimeout
            // 
            this.viewSetupSensorTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupSensorTimeout.Location = new System.Drawing.Point(340, 15);
            this.viewSetupSensorTimeout.Name = "viewSetupSensorTimeout";
            this.viewSetupSensorTimeout.Size = new System.Drawing.Size(258, 362);
            this.viewSetupSensorTimeout.TabIndex = 2;
            this.viewSetupSensorTimeout.TitleName = "Sensor Off Tiemout Check";
            // 
            // buttonSaveDistance
            // 
            this.buttonSaveDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveDistance.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveDistance.Name = "buttonSaveDistance";
            this.buttonSaveDistance.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveDistance.TabIndex = 1;
            this.buttonSaveDistance.Text = "Save";
            this.buttonSaveDistance.UseVisualStyleBackColor = true;
            this.buttonSaveDistance.Click += new System.EventHandler(this.buttonSaveDistance_Click);
            // 
            // viewSetupCvDistance
            // 
            this.viewSetupCvDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupCvDistance.Location = new System.Drawing.Point(6, 15);
            this.viewSetupCvDistance.Name = "viewSetupCvDistance";
            this.viewSetupCvDistance.Size = new System.Drawing.Size(331, 362);
            this.viewSetupCvDistance.TabIndex = 0;
            this.viewSetupCvDistance.TitleName = "Conveyor Unit Distance";
            // 
            // tabPageCvMotor
            // 
            this.tabPageCvMotor.Controls.Add(this.buttonSaveCv);
            this.tabPageCvMotor.Controls.Add(this.viewSetupCvInfo);
            this.tabPageCvMotor.Location = new System.Drawing.Point(4, 21);
            this.tabPageCvMotor.Name = "tabPageCvMotor";
            this.tabPageCvMotor.Size = new System.Drawing.Size(758, 405);
            this.tabPageCvMotor.TabIndex = 2;
            this.tabPageCvMotor.Text = "Cv Motor";
            this.tabPageCvMotor.UseVisualStyleBackColor = true;
            // 
            // buttonSaveCv
            // 
            this.buttonSaveCv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveCv.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveCv.Name = "buttonSaveCv";
            this.buttonSaveCv.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveCv.TabIndex = 3;
            this.buttonSaveCv.Text = "Save";
            this.buttonSaveCv.UseVisualStyleBackColor = true;
            this.buttonSaveCv.Click += new System.EventHandler(this.buttonSaveCv_Click);
            // 
            // viewSetupCvInfo
            // 
            this.viewSetupCvInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupCvInfo.Location = new System.Drawing.Point(6, 15);
            this.viewSetupCvInfo.Name = "viewSetupCvInfo";
            this.viewSetupCvInfo.Size = new System.Drawing.Size(478, 362);
            this.viewSetupCvInfo.TabIndex = 0;
            this.viewSetupCvInfo.TitleName = "Conveyor Motor Parameters";
            // 
            // tabPageCal
            // 
            this.tabPageCal.Controls.Add(this.buttonSaveCal);
            this.tabPageCal.Controls.Add(this.viewCalibration);
            this.tabPageCal.Location = new System.Drawing.Point(4, 21);
            this.tabPageCal.Name = "tabPageCal";
            this.tabPageCal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCal.Size = new System.Drawing.Size(758, 405);
            this.tabPageCal.TabIndex = 3;
            this.tabPageCal.Text = "Gauge Calibration";
            this.tabPageCal.UseVisualStyleBackColor = true;
            // 
            // buttonSaveCal
            // 
            this.buttonSaveCal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveCal.Location = new System.Drawing.Point(621, 36);
            this.buttonSaveCal.Name = "buttonSaveCal";
            this.buttonSaveCal.Size = new System.Drawing.Size(121, 45);
            this.buttonSaveCal.TabIndex = 1;
            this.buttonSaveCal.Text = "Save";
            this.buttonSaveCal.UseVisualStyleBackColor = true;
            this.buttonSaveCal.Click += new System.EventHandler(this.buttonSaveCal_Click);
            // 
            // viewCalibration
            // 
            this.viewCalibration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewCalibration.Location = new System.Drawing.Point(6, 15);
            this.viewCalibration.Name = "viewCalibration";
            this.viewCalibration.Size = new System.Drawing.Size(569, 362);
            this.viewCalibration.TabIndex = 0;
            this.viewCalibration.TitleName = "Calibration";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(631, 450);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(138, 35);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(787, 498);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup";
            this.tabControl1.ResumeLayout(false);
            this.tabPageGen.ResumeLayout(false);
            this.tabPageIdle.ResumeLayout(false);
            this.tabPageTankLevel.ResumeLayout(false);
            this.tabPageGaugeInterlock.ResumeLayout(false);
            this.tabPageSensor.ResumeLayout(false);
            this.tabPageCvDistance.ResumeLayout(false);
            this.tabPageCvMotor.ResumeLayout(false);
            this.tabPageCal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGen;
        private System.Windows.Forms.Button buttonSaveGen;
        private Dms.Data.ViewSetupInfo viewSetupGenInfo;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabPage tabPageIdle;
        private System.Windows.Forms.TabPage tabPageCvMotor;
        private Dms.Data.ViewSetupInfo viewSetupIdleInfo;
        private System.Windows.Forms.Button buttonSaveIdle;
        private Dms.Data.ViewSetupCvInfo viewSetupCvInfo;
        private System.Windows.Forms.Button buttonSaveCv;
        private System.Windows.Forms.TabPage tabPageCal;
        private Dms.Data.ViewSetupInfo viewCalibration;
        private System.Windows.Forms.Button buttonSaveCal;
        private System.Windows.Forms.TabPage tabPageSensor;
        private Dms.Data.ViewSetupInfo viewSetupSensorInterlock;
        private System.Windows.Forms.Button buttonSaveSenIntr;
        private System.Windows.Forms.TabPage tabPageCvDistance;
        private Dms.Data.ViewSetupInfo viewSetupCvDistance;
        private System.Windows.Forms.Button buttonSaveDistance;
        private Dms.Data.ViewSetupInfo viewSetupSensorTimeout;
        private System.Windows.Forms.TabPage tabPageGaugeInterlock;
        private Dms.Data.ViewSetupInfo viewSetupGaugeInterlock;
        private System.Windows.Forms.Button buttonSaveGaugeInterlock;
        private System.Windows.Forms.TabPage tabPageTankLevel;
        private Dms.Data.ViewSetupInfo viewSetupTankLevel;
        private System.Windows.Forms.Button button1;
    }
}
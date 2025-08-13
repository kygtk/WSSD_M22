namespace Dms.Control
{
    partial class DlgHeatExchanger
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLocalMode = new System.Windows.Forms.RadioButton();
            this.btnRemoteMode = new System.Windows.Forms.RadioButton();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAlarmLed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblReadyLed = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblCh3Ready = new System.Windows.Forms.Label();
            this.lblPowerOnLed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblCh1Ready = new System.Windows.Forms.Label();
            this.lblCh2Ready = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.lblEmo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblRunning = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbCh3Temp = new System.Windows.Forms.TextBox();
            this.tbCh2Temp = new System.Windows.Forms.TextBox();
            this.tbCh1Temp = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLocalMode);
            this.groupBox1.Controls.Add(this.btnRemoteMode);
            this.groupBox1.Controls.Add(this.btnOff);
            this.groupBox1.Controls.Add(this.btnOn);
            this.groupBox1.Location = new System.Drawing.Point(6, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // btnLocalMode
            // 
            this.btnLocalMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnLocalMode.Location = new System.Drawing.Point(147, 15);
            this.btnLocalMode.Name = "btnLocalMode";
            this.btnLocalMode.Size = new System.Drawing.Size(140, 63);
            this.btnLocalMode.TabIndex = 1;
            this.btnLocalMode.TabStop = true;
            this.btnLocalMode.Text = "Local Mode";
            this.btnLocalMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLocalMode.UseVisualStyleBackColor = true;
            this.btnLocalMode.CheckedChanged += new System.EventHandler(this.btnLocalMode_CheckedChanged);
            // 
            // btnRemoteMode
            // 
            this.btnRemoteMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRemoteMode.Location = new System.Drawing.Point(4, 15);
            this.btnRemoteMode.Name = "btnRemoteMode";
            this.btnRemoteMode.Size = new System.Drawing.Size(140, 63);
            this.btnRemoteMode.TabIndex = 1;
            this.btnRemoteMode.TabStop = true;
            this.btnRemoteMode.Text = "Remote Mode";
            this.btnRemoteMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemoteMode.UseVisualStyleBackColor = true;
            this.btnRemoteMode.CheckedChanged += new System.EventHandler(this.btnRemoteMode_CheckedChanged);
            // 
            // btnOff
            // 
            this.btnOff.Location = new System.Drawing.Point(147, 81);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(140, 63);
            this.btnOff.TabIndex = 0;
            this.btnOff.Text = "OFF";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnOn
            // 
            this.btnOn.Location = new System.Drawing.Point(4, 81);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(140, 63);
            this.btnOn.TabIndex = 0;
            this.btnOn.Text = "ON";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.lblAlarmLed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblReadyLed);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.lblCh3Ready);
            this.groupBox2.Controls.Add(this.lblPowerOnLed);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lblWarning);
            this.groupBox2.Controls.Add(this.lblCh1Ready);
            this.groupBox2.Controls.Add(this.lblCh2Ready);
            this.groupBox2.Controls.Add(this.lblAlarm);
            this.groupBox2.Controls.Add(this.lblEmo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lblRunning);
            this.groupBox2.Location = new System.Drawing.Point(5, 269);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 110);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(218, 86);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 17);
            this.label20.TabIndex = 0;
            this.label20.Text = "Alarm LED";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(218, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "CH3 Ready";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(119, 86);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(81, 17);
            this.label19.TabIndex = 0;
            this.label19.Text = "Ready LED";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(119, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "EMO";
            // 
            // lblAlarmLed
            // 
            this.lblAlarmLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAlarmLed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarmLed.Location = new System.Drawing.Point(203, 86);
            this.lblAlarmLed.Name = "lblAlarmLed";
            this.lblAlarmLed.Size = new System.Drawing.Size(15, 15);
            this.lblAlarmLed.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(119, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "CH2 Ready";
            // 
            // lblReadyLed
            // 
            this.lblReadyLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReadyLed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReadyLed.Location = new System.Drawing.Point(104, 87);
            this.lblReadyLed.Name = "lblReadyLed";
            this.lblReadyLed.Size = new System.Drawing.Size(15, 15);
            this.lblReadyLed.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(20, 86);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(81, 17);
            this.label17.TabIndex = 0;
            this.label17.Text = "Power On LED";
            // 
            // lblCh3Ready
            // 
            this.lblCh3Ready.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCh3Ready.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCh3Ready.Location = new System.Drawing.Point(203, 64);
            this.lblCh3Ready.Name = "lblCh3Ready";
            this.lblCh3Ready.Size = new System.Drawing.Size(15, 15);
            this.lblCh3Ready.TabIndex = 0;
            // 
            // lblPowerOnLed
            // 
            this.lblPowerOnLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPowerOnLed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerOnLed.Location = new System.Drawing.Point(5, 85);
            this.lblPowerOnLed.Name = "lblPowerOnLed";
            this.lblPowerOnLed.Size = new System.Drawing.Size(15, 15);
            this.lblPowerOnLed.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "CH1 Ready";
            // 
            // lblWarning
            // 
            this.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWarning.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(104, 42);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(15, 15);
            this.lblWarning.TabIndex = 0;
            // 
            // lblCh1Ready
            // 
            this.lblCh1Ready.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCh1Ready.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCh1Ready.Location = new System.Drawing.Point(5, 64);
            this.lblCh1Ready.Name = "lblCh1Ready";
            this.lblCh1Ready.Size = new System.Drawing.Size(15, 15);
            this.lblCh1Ready.TabIndex = 0;
            // 
            // lblCh2Ready
            // 
            this.lblCh2Ready.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCh2Ready.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCh2Ready.Location = new System.Drawing.Point(104, 64);
            this.lblCh2Ready.Name = "lblCh2Ready";
            this.lblCh2Ready.Size = new System.Drawing.Size(15, 15);
            this.lblCh2Ready.TabIndex = 0;
            // 
            // lblAlarm
            // 
            this.lblAlarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAlarm.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarm.Location = new System.Drawing.Point(5, 41);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(15, 15);
            this.lblAlarm.TabIndex = 0;
            // 
            // lblEmo
            // 
            this.lblEmo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEmo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmo.Location = new System.Drawing.Point(104, 20);
            this.lblEmo.Name = "lblEmo";
            this.lblEmo.Size = new System.Drawing.Size(15, 15);
            this.lblEmo.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "운전중";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(20, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 17);
            this.label15.TabIndex = 0;
            this.label15.Text = "중고장 Alarm";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(119, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "경고장 Alarm";
            // 
            // lblRunning
            // 
            this.lblRunning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRunning.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunning.Location = new System.Drawing.Point(5, 19);
            this.lblRunning.Name = "lblRunning";
            this.lblRunning.Size = new System.Drawing.Size(15, 15);
            this.lblRunning.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbCh3Temp);
            this.groupBox3.Controls.Add(this.tbCh2Temp);
            this.groupBox3.Controls.Add(this.tbCh1Temp);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Location = new System.Drawing.Point(6, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(292, 110);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Temp Setting";
            // 
            // tbCh3Temp
            // 
            this.tbCh3Temp.Location = new System.Drawing.Point(175, 85);
            this.tbCh3Temp.Name = "tbCh3Temp";
            this.tbCh3Temp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbCh3Temp.Size = new System.Drawing.Size(90, 20);
            this.tbCh3Temp.TabIndex = 0;
            this.tbCh3Temp.Click += new System.EventHandler(this.tbCh3Temp_Click);
            // 
            // tbCh2Temp
            // 
            this.tbCh2Temp.Location = new System.Drawing.Point(175, 54);
            this.tbCh2Temp.Name = "tbCh2Temp";
            this.tbCh2Temp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbCh2Temp.Size = new System.Drawing.Size(90, 20);
            this.tbCh2Temp.TabIndex = 0;
            this.tbCh2Temp.Click += new System.EventHandler(this.tbCh2Temp_Click);
            // 
            // tbCh1Temp
            // 
            this.tbCh1Temp.Location = new System.Drawing.Point(175, 20);
            this.tbCh1Temp.Name = "tbCh1Temp";
            this.tbCh1Temp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbCh1Temp.Size = new System.Drawing.Size(90, 20);
            this.tbCh1Temp.TabIndex = 0;
            this.tbCh1Temp.Click += new System.EventHandler(this.tbCh1Temp_Click);
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(7, 86);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(162, 17);
            this.label23.TabIndex = 0;
            this.label23.Text = "Chamber3 Temperature";
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(7, 55);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(162, 17);
            this.label22.TabIndex = 0;
            this.label22.Text = "Chamber2 Temperature";
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(266, 87);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(25, 17);
            this.label26.TabIndex = 0;
            this.label26.Text = "℃";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(266, 56);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(25, 17);
            this.label25.TabIndex = 0;
            this.label25.Text = "℃";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(266, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(25, 17);
            this.label24.TabIndex = 0;
            this.label24.Text = "℃";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(7, 22);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(162, 17);
            this.label21.TabIndex = 0;
            this.label21.Text = "Chamber1 Temperature";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(153, 385);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(146, 60);
            this.button3.TabIndex = 0;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgHeatExchanger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DlgHeatExchanger";
            this.Text = "DlgHeatExchanger";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblAlarmLed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblEmo;
        private System.Windows.Forms.Label lblReadyLed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblCh3Ready;
        private System.Windows.Forms.Label lblPowerOnLed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblCh2Ready;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblCh1Ready;
        private System.Windows.Forms.Label lblRunning;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbCh2Temp;
        private System.Windows.Forms.TextBox tbCh1Temp;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.TextBox tbCh3Temp;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.RadioButton btnLocalMode;
        private System.Windows.Forms.RadioButton btnRemoteMode;
    }
}
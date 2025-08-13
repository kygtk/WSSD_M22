namespace Dms.Control
{
    partial class CvMotorButtonOp
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
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaxSpeed = new System.Windows.Forms.Label();
            this.lblMinSpeed = new System.Windows.Forms.Label();
            this.cboSpeed = new System.Windows.Forms.ComboBox();
            this.btnStop = new System.Windows.Forms.RadioButton();
            this.btnBw = new System.Windows.Forms.RadioButton();
            this.btnFw = new System.Windows.Forms.RadioButton();
            this.chkCpOff = new System.Windows.Forms.CheckBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(548, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "~";
            // 
            // lblMaxSpeed
            // 
            this.lblMaxSpeed.Location = new System.Drawing.Point(561, 27);
            this.lblMaxSpeed.Name = "lblMaxSpeed";
            this.lblMaxSpeed.Size = new System.Drawing.Size(45, 12);
            this.lblMaxSpeed.TabIndex = 21;
            this.lblMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblMinSpeed
            // 
            this.lblMinSpeed.Location = new System.Drawing.Point(504, 27);
            this.lblMinSpeed.Name = "lblMinSpeed";
            this.lblMinSpeed.Size = new System.Drawing.Size(45, 12);
            this.lblMinSpeed.TabIndex = 20;
            this.lblMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboSpeed
            // 
            this.cboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed.FormattingEnabled = true;
            this.cboSpeed.Location = new System.Drawing.Point(504, 2);
            this.cboSpeed.Name = "cboSpeed";
            this.cboSpeed.Size = new System.Drawing.Size(102, 20);
            this.cboSpeed.TabIndex = 19;
            this.cboSpeed.SelectionChangeCommitted += new System.EventHandler(this.cboSpeed_SelectionChangeCommitted);
            // 
            // btnStop
            // 
            this.btnStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnStop.BackColor = System.Drawing.Color.Pink;
            this.btnStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(338, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 40);
            this.btnStop.TabIndex = 18;
            this.btnStop.TabStop = true;
            this.btnStop.Text = "STOP";
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnBw
            // 
            this.btnBw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnBw.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnBw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBw.Location = new System.Drawing.Point(248, 1);
            this.btnBw.Name = "btnBw";
            this.btnBw.Size = new System.Drawing.Size(84, 40);
            this.btnBw.TabIndex = 17;
            this.btnBw.TabStop = true;
            this.btnBw.Text = "BW";
            this.btnBw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBw.UseVisualStyleBackColor = false;
            this.btnBw.Click += new System.EventHandler(this.btnBw_Click);
            // 
            // btnFw
            // 
            this.btnFw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnFw.BackColor = System.Drawing.Color.LightCyan;
            this.btnFw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFw.Location = new System.Drawing.Point(158, 1);
            this.btnFw.Name = "btnFw";
            this.btnFw.Size = new System.Drawing.Size(84, 40);
            this.btnFw.TabIndex = 16;
            this.btnFw.TabStop = true;
            this.btnFw.Text = "FW";
            this.btnFw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFw.UseVisualStyleBackColor = false;
            this.btnFw.Click += new System.EventHandler(this.btnFw_Click);
            // 
            // chkCpOn
            // 
            this.chkCpOff.AutoCheck = false;
            this.chkCpOff.Location = new System.Drawing.Point(428, 23);
            this.chkCpOff.Name = "chkCpOn";
            this.chkCpOff.Size = new System.Drawing.Size(78, 16);
            this.chkCpOff.TabIndex = 15;
            this.chkCpOff.Text = "C/P OFF";
            this.chkCpOff.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkCpOff.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Thistle;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(5, 1);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(147, 39);
            this.lblName.TabIndex = 23;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoCheck = false;
            this.chkAlarm.Location = new System.Drawing.Point(428, 3);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(70, 16);
            this.chkAlarm.TabIndex = 14;
            this.chkAlarm.Text = "ALARM";
            this.chkAlarm.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // CvMotorButtonOp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblMinSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMaxSpeed);
            this.Controls.Add(this.cboSpeed);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnBw);
            this.Controls.Add(this.btnFw);
            this.Controls.Add(this.chkCpOff);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.chkAlarm);
            this.Name = "CvMotorButtonOp";
            this.Size = new System.Drawing.Size(609, 41);
            this.Load += new System.EventHandler(this.CvMotorButtonOp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaxSpeed;
        private System.Windows.Forms.Label lblMinSpeed;
        private System.Windows.Forms.ComboBox cboSpeed;
        private System.Windows.Forms.RadioButton btnStop;
        private System.Windows.Forms.RadioButton btnBw;
        private System.Windows.Forms.RadioButton btnFw;
        private System.Windows.Forms.CheckBox chkCpOff;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkAlarm;
    }
}

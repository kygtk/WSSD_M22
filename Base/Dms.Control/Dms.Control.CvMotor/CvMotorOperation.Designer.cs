namespace Dms.Control
{
    partial class CvMotorOperation
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
			this.lblName = new System.Windows.Forms.Label();
			this.chkAlarm = new System.Windows.Forms.CheckBox();
			this.chkCpOff = new System.Windows.Forms.CheckBox();
			this.btnFw = new System.Windows.Forms.RadioButton();
			this.btnBw = new System.Windows.Forms.RadioButton();
			this.btnStop = new System.Windows.Forms.RadioButton();
			this.cboSpeed = new System.Windows.Forms.ComboBox();
			this.lblMinSpeed = new System.Windows.Forms.Label();
			this.lblMaxSpeed = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblName.BackColor = System.Drawing.Color.Thistle;
			this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(3, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(84, 39);
			this.lblName.TabIndex = 0;
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkAlarm
			// 
			this.chkAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkAlarm.AutoCheck = false;
			this.chkAlarm.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.chkAlarm.Location = new System.Drawing.Point(6, 177);
			this.chkAlarm.Name = "chkAlarm";
			this.chkAlarm.Size = new System.Drawing.Size(70, 16);
			this.chkAlarm.TabIndex = 4;
			this.chkAlarm.Text = "ALARM";
			this.chkAlarm.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkAlarm.UseVisualStyleBackColor = true;
			// 
			// chkCpOff
			// 
			this.chkCpOff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkCpOff.AutoCheck = false;
			this.chkCpOff.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.chkCpOff.Location = new System.Drawing.Point(6, 197);
			this.chkCpOff.Name = "chkCpOff";
			this.chkCpOff.Size = new System.Drawing.Size(73, 16);
			this.chkCpOff.TabIndex = 5;
			this.chkCpOff.Text = "C/P OFF";
			this.chkCpOff.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkCpOff.UseVisualStyleBackColor = true;
			// 
			// btnFw
			// 
			this.btnFw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnFw.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnFw.BackColor = System.Drawing.Color.LightCyan;
			this.btnFw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnFw.Location = new System.Drawing.Point(3, 42);
			this.btnFw.Name = "btnFw";
			this.btnFw.Size = new System.Drawing.Size(84, 40);
			this.btnFw.TabIndex = 6;
			this.btnFw.TabStop = true;
			this.btnFw.Text = "FW";
			this.btnFw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnFw.UseVisualStyleBackColor = false;
			this.btnFw.Click += new System.EventHandler(this.btnFw_Click);
			// 
			// btnBw
			// 
			this.btnBw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnBw.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnBw.BackColor = System.Drawing.Color.LemonChiffon;
			this.btnBw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBw.Location = new System.Drawing.Point(3, 87);
			this.btnBw.Name = "btnBw";
			this.btnBw.Size = new System.Drawing.Size(84, 40);
			this.btnBw.TabIndex = 7;
			this.btnBw.TabStop = true;
			this.btnBw.Text = "BW";
			this.btnBw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnBw.UseVisualStyleBackColor = false;
			this.btnBw.Click += new System.EventHandler(this.btnBw_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnStop.BackColor = System.Drawing.Color.Pink;
			this.btnStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStop.Location = new System.Drawing.Point(3, 132);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(84, 40);
			this.btnStop.TabIndex = 8;
			this.btnStop.TabStop = true;
			this.btnStop.Text = "STOP";
			this.btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// cboSpeed
			// 
			this.cboSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSpeed.FormattingEnabled = true;
			this.cboSpeed.Location = new System.Drawing.Point(2, 218);
			this.cboSpeed.Name = "cboSpeed";
			this.cboSpeed.Size = new System.Drawing.Size(86, 23);
			this.cboSpeed.TabIndex = 9;
			this.cboSpeed.SelectionChangeCommitted += new System.EventHandler(this.cboSpeed_SelectionChangeCommitted);
			// 
			// lblMinSpeed
			// 
			this.lblMinSpeed.Location = new System.Drawing.Point(0, 243);
			this.lblMinSpeed.Name = "lblMinSpeed";
			this.lblMinSpeed.Size = new System.Drawing.Size(42, 12);
			this.lblMinSpeed.TabIndex = 10;
			this.lblMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblMaxSpeed
			// 
			this.lblMaxSpeed.Location = new System.Drawing.Point(48, 243);
			this.lblMaxSpeed.Name = "lblMaxSpeed";
			this.lblMaxSpeed.Size = new System.Drawing.Size(42, 12);
			this.lblMaxSpeed.TabIndex = 11;
			this.lblMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(38, 243);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 15);
			this.label3.TabIndex = 12;
			this.label3.Text = "~";
			// 
			// CvMotorOperation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblMaxSpeed);
			this.Controls.Add(this.lblMinSpeed);
			this.Controls.Add(this.cboSpeed);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnBw);
			this.Controls.Add(this.btnFw);
			this.Controls.Add(this.chkCpOff);
			this.Controls.Add(this.chkAlarm);
			this.Controls.Add(this.lblName);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CvMotorOperation";
			this.Size = new System.Drawing.Size(90, 260);
			this.Load += new System.EventHandler(this.CvMotorOperation_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.CheckBox chkCpOff;
        private System.Windows.Forms.RadioButton btnFw;
        private System.Windows.Forms.RadioButton btnBw;
        private System.Windows.Forms.RadioButton btnStop;
        private System.Windows.Forms.ComboBox cboSpeed;
        private System.Windows.Forms.Label lblMinSpeed;
        private System.Windows.Forms.Label lblMaxSpeed;
        private System.Windows.Forms.Label label3;
    }
}

namespace Dms.Control
{
    partial class MfcButtonOp
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
            this.btnON = new System.Windows.Forms.RadioButton();
            this.btnOFF = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSetPoint = new System.Windows.Forms.Button();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnON
            // 
            this.btnON.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnON.BackColor = System.Drawing.Color.LightCyan;
            this.btnON.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnON.Location = new System.Drawing.Point(111, 3);
            this.btnON.Name = "btnON";
            this.btnON.Size = new System.Drawing.Size(85, 34);
            this.btnON.TabIndex = 1;
            this.btnON.Text = "ON";
            this.btnON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnON.UseVisualStyleBackColor = false;
            this.btnON.Click += new System.EventHandler(this.btnON_Click);
            // 
            // btnOFF
            // 
            this.btnOFF.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnOFF.BackColor = System.Drawing.Color.Pink;
            this.btnOFF.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOFF.Location = new System.Drawing.Point(199, 3);
            this.btnOFF.Name = "btnOFF";
            this.btnOFF.Size = new System.Drawing.Size(85, 34);
            this.btnOFF.TabIndex = 2;
            this.btnOFF.Text = "OFF";
            this.btnOFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOFF.UseVisualStyleBackColor = false;
            this.btnOFF.Click += new System.EventHandler(this.btnOFF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ccm";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(289, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(66, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Thistle;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(5, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(101, 30);
            this.lblName.TabIndex = 0;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSetPoint
            // 
            this.btnSetPoint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetPoint.Location = new System.Drawing.Point(387, 3);
            this.btnSetPoint.Name = "btnSetPoint";
            this.btnSetPoint.Size = new System.Drawing.Size(85, 34);
            this.btnSetPoint.TabIndex = 3;
            this.btnSetPoint.Text = "Set Point";
            this.btnSetPoint.UseVisualStyleBackColor = true;
            this.btnSetPoint.Click += new System.EventHandler(this.btnSetPoint_Click);
            // 
            // btnCalibration
            // 
            this.btnCalibration.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalibration.Location = new System.Drawing.Point(475, 3);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(85, 34);
            this.btnCalibration.TabIndex = 3;
            this.btnCalibration.Text = "Calibration";
            this.btnCalibration.UseVisualStyleBackColor = true;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // MfcButtonOp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.btnSetPoint);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOFF);
            this.Controls.Add(this.btnON);
            this.Name = "MfcButtonOp";
            this.Size = new System.Drawing.Size(563, 40);
            this.Load += new System.EventHandler(this.MfcButtonOp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton btnON;
        private System.Windows.Forms.RadioButton btnOFF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSetPoint;
        private System.Windows.Forms.Button btnCalibration;
    }
}

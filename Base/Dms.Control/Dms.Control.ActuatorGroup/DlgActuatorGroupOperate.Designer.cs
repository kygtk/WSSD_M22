namespace Dms.Control
{
    partial class DlgActuatorGroupOperate
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBW = new System.Windows.Forms.Button();
            this.btnFW = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBW);
            this.groupBox1.Controls.Add(this.btnFW);
            this.groupBox1.Location = new System.Drawing.Point(10, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(216, 89);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actuator Group Action";
            // 
            // btnBW
            // 
            this.btnBW.BackColor = System.Drawing.Color.Pink;
            this.btnBW.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBW.Location = new System.Drawing.Point(113, 23);
            this.btnBW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBW.Name = "btnBW";
            this.btnBW.Size = new System.Drawing.Size(90, 51);
            this.btnBW.TabIndex = 0;
            this.btnBW.Text = "BW";
            this.btnBW.UseVisualStyleBackColor = false;
            this.btnBW.Click += new System.EventHandler(this.btnBW_Click);
            // 
            // btnFW
            // 
            this.btnFW.BackColor = System.Drawing.Color.LightCyan;
            this.btnFW.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFW.Location = new System.Drawing.Point(14, 23);
            this.btnFW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFW.Name = "btnFW";
            this.btnFW.Size = new System.Drawing.Size(90, 51);
            this.btnFW.TabIndex = 0;
            this.btnFW.Text = "FW";
            this.btnFW.UseVisualStyleBackColor = false;
            this.btnFW.Click += new System.EventHandler(this.btnFW_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Linen;
            this.btnOK.Location = new System.Drawing.Point(136, 111);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 51);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DlgActuatorGroupOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(237, 173);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgActuatorGroupOperate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acutoator";
            this.Load += new System.EventHandler(this.DlgActuatorGroupOperateForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBW;
        private System.Windows.Forms.Button btnFW;
        private System.Windows.Forms.Button btnOK;

    }
}
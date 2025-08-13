namespace Dms.Control
{
    partial class CylinderOperateForm
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
            this.btnBW = new System.Windows.Forms.Button();
            this.btnFW = new System.Windows.Forms.Button();
            this.checkFW = new System.Windows.Forms.CheckBox();
            this.checkBW = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timerUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // checkFW
            // 
            this.checkFW.AutoCheck = false;
            this.checkFW.AutoSize = true;
            this.checkFW.Location = new System.Drawing.Point(14, 27);
            this.checkFW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkFW.Name = "checkFW";
            this.checkFW.Size = new System.Drawing.Size(87, 19);
            this.checkFW.TabIndex = 1;
            this.checkFW.Text = "FW Sensor";
            this.checkFW.UseVisualStyleBackColor = true;
            // 
            // checkBW
            // 
            this.checkBW.AutoCheck = false;
            this.checkBW.AutoSize = true;
            this.checkBW.Location = new System.Drawing.Point(113, 27);
            this.checkBW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBW.Name = "checkBW";
            this.checkBW.Size = new System.Drawing.Size(88, 19);
            this.checkBW.TabIndex = 1;
            this.checkBW.Text = "BW Sensor";
            this.checkBW.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Linen;
            this.btnOK.Location = new System.Drawing.Point(136, 171);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 51);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBW);
            this.groupBox1.Controls.Add(this.btnFW);
            this.groupBox1.Location = new System.Drawing.Point(10, 67);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(216, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBW);
            this.groupBox2.Controls.Add(this.checkFW);
            this.groupBox2.Location = new System.Drawing.Point(10, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(215, 54);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensor State";
            // 
            // timerUpdateState
            // 
            this.timerUpdateState.Enabled = true;
            this.timerUpdateState.Interval = 200;
            this.timerUpdateState.Tick += new System.EventHandler(this.timerUpdateState_Tick);
            // 
            // CylinderOperateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(237, 231);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CylinderOperateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cylinder";
            this.Load += new System.EventHandler(this.CylinderOperateForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBW;
        private System.Windows.Forms.Button btnFW;
        private System.Windows.Forms.CheckBox checkFW;
        private System.Windows.Forms.CheckBox checkBW;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timerUpdateState;
    }
}
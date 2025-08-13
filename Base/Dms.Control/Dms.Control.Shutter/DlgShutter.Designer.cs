namespace Dms.Control
{
    partial class DlgShutter
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
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.checkCLOSE = new System.Windows.Forms.CheckBox();
            this.checkOPEN = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnOPEN = new System.Windows.Forms.Button();
            this.btnCLOSE = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Enabled = true;
            this.tmrUpdateState.Interval = 200;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // checkCLOSE
            // 
            this.checkCLOSE.AutoCheck = false;
            this.checkCLOSE.AutoSize = true;
            this.checkCLOSE.Location = new System.Drawing.Point(133, 21);
            this.checkCLOSE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkCLOSE.Name = "checkCLOSE";
            this.checkCLOSE.Size = new System.Drawing.Size(57, 16);
            this.checkCLOSE.TabIndex = 1;
            this.checkCLOSE.Text = "Close";
            this.checkCLOSE.UseVisualStyleBackColor = true;
            // 
            // checkOPEN
            // 
            this.checkOPEN.AutoCheck = false;
            this.checkOPEN.AutoSize = true;
            this.checkOPEN.Location = new System.Drawing.Point(8, 21);
            this.checkOPEN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkOPEN.Name = "checkOPEN";
            this.checkOPEN.Size = new System.Drawing.Size(54, 16);
            this.checkOPEN.TabIndex = 1;
            this.checkOPEN.Text = "Open";
            this.checkOPEN.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkCLOSE);
            this.groupBox2.Controls.Add(this.checkOPEN);
            this.groupBox2.Location = new System.Drawing.Point(12, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(257, 44);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensor State";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Linen;
            this.btnOK.Location = new System.Drawing.Point(12, 151);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(257, 59);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnOPEN
            // 
            this.btnOPEN.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnOPEN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOPEN.Location = new System.Drawing.Point(6, 19);
            this.btnOPEN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOPEN.Name = "btnOPEN";
            this.btnOPEN.Size = new System.Drawing.Size(119, 51);
            this.btnOPEN.TabIndex = 6;
            this.btnOPEN.Text = "OPEN";
            this.btnOPEN.UseVisualStyleBackColor = false;
            this.btnOPEN.Click += new System.EventHandler(this.btnOPEN_Click);
            // 
            // btnCLOSE
            // 
            this.btnCLOSE.BackColor = System.Drawing.Color.HotPink;
            this.btnCLOSE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCLOSE.Location = new System.Drawing.Point(131, 19);
            this.btnCLOSE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCLOSE.Name = "btnCLOSE";
            this.btnCLOSE.Size = new System.Drawing.Size(119, 51);
            this.btnCLOSE.TabIndex = 7;
            this.btnCLOSE.Text = "CLOSE";
            this.btnCLOSE.UseVisualStyleBackColor = false;
            this.btnCLOSE.Click += new System.EventHandler(this.btnCLOSE_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnOPEN);
            this.groupBox4.Controls.Add(this.btnCLOSE);
            this.groupBox4.Location = new System.Drawing.Point(12, 65);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(257, 78);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Operation";
            // 
            // DlgShutter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 224);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Name = "DlgShutter";
            this.Text = "DlgShutter";
            this.Load += new System.EventHandler(this.DlgShutter_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.CheckBox checkCLOSE;
        private System.Windows.Forms.CheckBox checkOPEN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnOPEN;
        private System.Windows.Forms.Button btnCLOSE;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}
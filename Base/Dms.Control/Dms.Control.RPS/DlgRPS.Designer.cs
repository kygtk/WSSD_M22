namespace Dms.Control
{
    partial class DlgRPS
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
            this.chkReady = new System.Windows.Forms.CheckBox();
            this.chkPlasmaOk = new System.Windows.Forms.CheckBox();
            this.chkAcOk = new System.Windows.Forms.CheckBox();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAcOk);
            this.groupBox1.Controls.Add(this.chkPlasmaOk);
            this.groupBox1.Controls.Add(this.chkReady);
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // chkReady
            // 
            this.chkReady.AutoSize = true;
            this.chkReady.Location = new System.Drawing.Point(8, 21);
            this.chkReady.Name = "chkReady";
            this.chkReady.Size = new System.Drawing.Size(66, 19);
            this.chkReady.TabIndex = 0;
            this.chkReady.Text = "READY";
            this.chkReady.UseVisualStyleBackColor = true;
            // 
            // chkPlasmaOk
            // 
            this.chkPlasmaOk.AutoSize = true;
            this.chkPlasmaOk.Location = new System.Drawing.Point(80, 21);
            this.chkPlasmaOk.Name = "chkPlasmaOk";
            this.chkPlasmaOk.Size = new System.Drawing.Size(92, 19);
            this.chkPlasmaOk.TabIndex = 1;
            this.chkPlasmaOk.Text = "PLASMA OK";
            this.chkPlasmaOk.UseVisualStyleBackColor = true;
            // 
            // chkAcOk
            // 
            this.chkAcOk.AutoSize = true;
            this.chkAcOk.Location = new System.Drawing.Point(178, 21);
            this.chkAcOk.Name = "chkAcOk";
            this.chkAcOk.Size = new System.Drawing.Size(62, 19);
            this.chkAcOk.TabIndex = 2;
            this.chkAcOk.Text = "AC OK";
            this.chkAcOk.UseVisualStyleBackColor = true;
            // 
            // btnOn
            // 
            this.btnOn.BackColor = System.Drawing.Color.Pink;
            this.btnOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOn.Location = new System.Drawing.Point(8, 61);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(118, 65);
            this.btnOn.TabIndex = 1;
            this.btnOn.Text = "PLASMA ON";
            this.btnOn.UseVisualStyleBackColor = false;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnOff
            // 
            this.btnOff.BackColor = System.Drawing.Color.LightCyan;
            this.btnOff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOff.Location = new System.Drawing.Point(136, 61);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(118, 65);
            this.btnOff.TabIndex = 2;
            this.btnOff.Text = "PLASMA OFF";
            this.btnOff.UseVisualStyleBackColor = false;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(163, 132);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(91, 51);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgRPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(263, 189);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnOff);
            this.Controls.Add(this.btnOn);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgRPS";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DlgRPS";
            this.Load += new System.EventHandler(this.DlgRPS_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAcOk;
        private System.Windows.Forms.CheckBox chkPlasmaOk;
        private System.Windows.Forms.CheckBox chkReady;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
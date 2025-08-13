namespace Dms.Control
{
    partial class DlgRotaryPump
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
            this.btnOk = new System.Windows.Forms.Button();
            this.gbPump = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblALARM = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRUNNING = new System.Windows.Forms.Label();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbPump.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(253, 38);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 60);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gbPump
            // 
            this.gbPump.Controls.Add(this.label6);
            this.gbPump.Controls.Add(this.lblALARM);
            this.gbPump.Controls.Add(this.label2);
            this.gbPump.Controls.Add(this.lblRUNNING);
            this.gbPump.Controls.Add(this.btnOff);
            this.gbPump.Controls.Add(this.btnStart);
            this.gbPump.Location = new System.Drawing.Point(5, 4);
            this.gbPump.Name = "gbPump";
            this.gbPump.Size = new System.Drawing.Size(242, 94);
            this.gbPump.TabIndex = 2;
            this.gbPump.TabStop = false;
            this.gbPump.Text = "PUMP";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(22, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "ALARM";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblALARM
            // 
            this.lblALARM.BackColor = System.Drawing.Color.White;
            this.lblALARM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblALARM.Location = new System.Drawing.Point(6, 16);
            this.lblALARM.Name = "lblALARM";
            this.lblALARM.Size = new System.Drawing.Size(15, 15);
            this.lblALARM.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(140, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "RUNNING";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblRUNNING
            // 
            this.lblRUNNING.BackColor = System.Drawing.Color.White;
            this.lblRUNNING.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRUNNING.Location = new System.Drawing.Point(124, 17);
            this.lblRUNNING.Name = "lblRUNNING";
            this.lblRUNNING.Size = new System.Drawing.Size(15, 15);
            this.lblRUNNING.TabIndex = 4;
            // 
            // btnOff
            // 
            this.btnOff.BackColor = System.Drawing.Color.Pink;
            this.btnOff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOff.Location = new System.Drawing.Point(123, 34);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(112, 54);
            this.btnOff.TabIndex = 3;
            this.btnOff.Text = "OFF";
            this.btnOff.UseVisualStyleBackColor = false;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightCyan;
            this.btnStart.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(6, 34);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 54);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgRotaryPump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(334, 104);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbPump);
            this.Name = "DlgRotaryPump";
            this.Text = "DlgDryPump";
            this.Load += new System.EventHandler(this.DlgPump_Load);
            this.gbPump.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbPump;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRUNNING;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblALARM;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
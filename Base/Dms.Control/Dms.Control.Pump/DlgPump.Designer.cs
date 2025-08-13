namespace Dms.Control
{
    partial class DlgPump
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
            this.gbPump = new System.Windows.Forms.GroupBox();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.optAlarm = new System.Windows.Forms.RadioButton();
            this.optNoAlarm = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetFrequency = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.gbPump.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPump
            // 
            this.gbPump.Controls.Add(this.btnOff);
            this.gbPump.Controls.Add(this.btnStart);
            this.gbPump.Controls.Add(this.optAlarm);
            this.gbPump.Controls.Add(this.optNoAlarm);
            this.gbPump.Location = new System.Drawing.Point(8, 2);
            this.gbPump.Name = "gbPump";
            this.gbPump.Size = new System.Drawing.Size(200, 100);
            this.gbPump.TabIndex = 0;
            this.gbPump.TabStop = false;
            this.gbPump.Text = "PUMP";
            // 
            // btnOff
            // 
            this.btnOff.BackColor = System.Drawing.Color.Pink;
            this.btnOff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOff.Location = new System.Drawing.Point(103, 57);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(89, 37);
            this.btnOff.TabIndex = 3;
            this.btnOff.Text = "OFF";
            this.btnOff.UseVisualStyleBackColor = false;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightCyan;
            this.btnStart.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(103, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(89, 37);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // optAlarm
            // 
            this.optAlarm.AutoCheck = false;
            this.optAlarm.AutoSize = true;
            this.optAlarm.Location = new System.Drawing.Point(9, 57);
            this.optAlarm.Name = "optAlarm";
            this.optAlarm.Size = new System.Drawing.Size(64, 19);
            this.optAlarm.TabIndex = 1;
            this.optAlarm.TabStop = true;
            this.optAlarm.Text = "ALARM";
            this.optAlarm.UseVisualStyleBackColor = true;
            // 
            // optNoAlarm
            // 
            this.optNoAlarm.AutoCheck = false;
            this.optNoAlarm.AutoSize = true;
            this.optNoAlarm.Location = new System.Drawing.Point(9, 33);
            this.optNoAlarm.Name = "optNoAlarm";
            this.optNoAlarm.Size = new System.Drawing.Size(85, 19);
            this.optNoAlarm.TabIndex = 0;
            this.optNoAlarm.TabStop = true;
            this.optNoAlarm.Text = "NO ALARM";
            this.optNoAlarm.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(214, 202);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 37);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetFrequency);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFrequency);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.chkAlarm);
            this.groupBox1.Location = new System.Drawing.Point(8, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 131);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "INVERTER";
            // 
            // btnSetFrequency
            // 
            this.btnSetFrequency.BackColor = System.Drawing.Color.LightCyan;
            this.btnSetFrequency.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetFrequency.Location = new System.Drawing.Point(103, 74);
            this.btnSetFrequency.Name = "btnSetFrequency";
            this.btnSetFrequency.Size = new System.Drawing.Size(89, 48);
            this.btnSetFrequency.TabIndex = 5;
            this.btnSetFrequency.Text = "SET FREQ.";
            this.btnSetFrequency.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Frequency";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hz";
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(9, 98);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(46, 21);
            this.txtFrequency.TabIndex = 2;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Pink;
            this.btnReset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(103, 17);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(89, 48);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "ALARM RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Location = new System.Drawing.Point(9, 35);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(65, 19);
            this.chkAlarm.TabIndex = 0;
            this.chkAlarm.Text = "ALARM";
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // DlgPump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(299, 247);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbPump);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgPump";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pump Manual Operation";
            this.Load += new System.EventHandler(this.DlgPump_Load);
            this.gbPump.ResumeLayout(false);
            this.gbPump.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPump;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RadioButton optAlarm;
        private System.Windows.Forms.RadioButton optNoAlarm;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetFrequency;
    }
}
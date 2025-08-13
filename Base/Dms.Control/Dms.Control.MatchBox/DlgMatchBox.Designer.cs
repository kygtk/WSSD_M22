namespace Dms.Control
{
    partial class DlgMatchBox
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
            this.btnRfPowerOn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInterlockRelease = new System.Windows.Forms.Button();
            this.btnRfInterlockSet = new System.Windows.Forms.Button();
            this.btnRfPowerOff = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPowerSet = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tbLoadPreset = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbTunePreset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRfPowerOn
            // 
            this.btnRfPowerOn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRfPowerOn.Location = new System.Drawing.Point(4, 85);
            this.btnRfPowerOn.Name = "btnRfPowerOn";
            this.btnRfPowerOn.Size = new System.Drawing.Size(107, 65);
            this.btnRfPowerOn.TabIndex = 0;
            this.btnRfPowerOn.Text = "POWER ON";
            this.btnRfPowerOn.UseVisualStyleBackColor = true;
            this.btnRfPowerOn.Click += new System.EventHandler(this.btnRfPowerOn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInterlockRelease);
            this.groupBox1.Controls.Add(this.btnRfInterlockSet);
            this.groupBox1.Controls.Add(this.btnRfPowerOff);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnRfPowerOn);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 203);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RFG";
            // 
            // btnInterlockRelease
            // 
            this.btnInterlockRelease.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInterlockRelease.Location = new System.Drawing.Point(114, 16);
            this.btnInterlockRelease.Name = "btnInterlockRelease";
            this.btnInterlockRelease.Size = new System.Drawing.Size(107, 65);
            this.btnInterlockRelease.TabIndex = 0;
            this.btnInterlockRelease.Text = "INTERLOCK 해제";
            this.btnInterlockRelease.UseVisualStyleBackColor = true;
            this.btnInterlockRelease.Click += new System.EventHandler(this.btnInterlockRelease_Click);
            // 
            // btnRfInterlockSet
            // 
            this.btnRfInterlockSet.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRfInterlockSet.Location = new System.Drawing.Point(4, 16);
            this.btnRfInterlockSet.Name = "btnRfInterlockSet";
            this.btnRfInterlockSet.Size = new System.Drawing.Size(107, 65);
            this.btnRfInterlockSet.TabIndex = 0;
            this.btnRfInterlockSet.Text = "INTERLOCK 설정";
            this.btnRfInterlockSet.UseVisualStyleBackColor = true;
            this.btnRfInterlockSet.Click += new System.EventHandler(this.btnRfInterlockSet_Click);
            // 
            // btnRfPowerOff
            // 
            this.btnRfPowerOff.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRfPowerOff.Location = new System.Drawing.Point(114, 85);
            this.btnRfPowerOff.Name = "btnRfPowerOff";
            this.btnRfPowerOff.Size = new System.Drawing.Size(107, 65);
            this.btnRfPowerOff.TabIndex = 0;
            this.btnRfPowerOff.Text = "POWER OFF";
            this.btnRfPowerOff.UseVisualStyleBackColor = true;
            this.btnRfPowerOff.Click += new System.EventHandler(this.btnRfPowerOff_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tbPowerSet);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(5, 150);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(216, 47);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Power Set";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(192, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "KW";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbPowerSet
            // 
            this.tbPowerSet.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPowerSet.Location = new System.Drawing.Point(109, 18);
            this.tbPowerSet.Name = "tbPowerSet";
            this.tbPowerSet.Size = new System.Drawing.Size(79, 20);
            this.tbPowerSet.TabIndex = 0;
            this.tbPowerSet.Click += new System.EventHandler(this.tbPowerSet_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.RosyBrown;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 22);
            this.label10.TabIndex = 1;
            this.label10.Text = "Power Set";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(4, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 94);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RF TUNER";
            // 
            // groupBox6
            // tbPowerSet
            // 
            this.tbPowerSet.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPowerSet.Location = new System.Drawing.Point(127, 17);
            this.tbPowerSet.Name = "tbPowerSet";
            this.tbPowerSet.Size = new System.Drawing.Size(92, 20);
            this.tbPowerSet.TabIndex = 0;
            this.tbPowerSet.Click += new System.EventHandler(this.tbPowerSet_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.RosyBrown;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 20);
            this.label10.TabIndex = 1;
            this.label10.Text = "Power Set";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 87);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RF TUNER";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tbLoadPreset);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.tbTunePreset);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(4, 14);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(216, 75);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Preset";
            // 
            // tbLoadPreset
            // 
            this.tbLoadPreset.BackColor = System.Drawing.Color.White;
            this.tbLoadPreset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLoadPreset.Location = new System.Drawing.Point(110, 46);
            this.tbLoadPreset.Name = "tbLoadPreset";
            this.tbLoadPreset.ReadOnly = true;
            this.tbLoadPreset.Size = new System.Drawing.Size(79, 20);
            this.tbLoadPreset.TabIndex = 0;
            this.tbLoadPreset.Click += new System.EventHandler(this.tbLoadPreset_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 22);
            this.label7.TabIndex = 1;
            this.label7.Text = "Load Preset";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbTunePreset
            // 
            this.tbTunePreset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTunePreset.Location = new System.Drawing.Point(110, 17);
            this.tbTunePreset.Name = "tbTunePreset";
            this.tbTunePreset.Size = new System.Drawing.Size(79, 20);
            this.tbTunePreset.TabIndex = 0;
            this.tbTunePreset.Click += new System.EventHandler(this.tbTunePreset_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(193, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 22);
            this.label5.TabIndex = 1;
            this.label5.Text = "%";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 22);
            this.label6.TabIndex = 1;
            this.label6.Text = "Tune Preset";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(193, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 22);
            this.label4.TabIndex = 1;
            this.label4.Text = "%";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.groupBox6.Controls.Add(this.tbLoadPreset);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.tbTunePreset);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(5, 13);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(252, 69);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Preset";
            // 
            // tbLoadPreset
            // 
            this.tbLoadPreset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLoadPreset.Location = new System.Drawing.Point(128, 42);
            this.tbLoadPreset.Name = "tbLoadPreset";
            this.tbLoadPreset.Size = new System.Drawing.Size(92, 20);
            this.tbLoadPreset.TabIndex = 0;
            this.tbLoadPreset.Click += new System.EventHandler(this.tbLoadPreset_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "Load Preset";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbTunePreset
            // 
            this.tbTunePreset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTunePreset.Location = new System.Drawing.Point(128, 16);
            this.tbTunePreset.Name = "tbTunePreset";
            this.tbTunePreset.Size = new System.Drawing.Size(92, 20);
            this.tbTunePreset.TabIndex = 0;
            this.tbTunePreset.Click += new System.EventHandler(this.tbTunePreset_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(225, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "%";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "Tune Preset";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(225, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "%";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnOk.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOk.Location = new System.Drawing.Point(118, 307);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(113, 65);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // DlgMatchBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 375);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DlgMatchBox";
            this.Text = "DlgMatchBox";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRfPowerOn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnInterlockRelease;
        private System.Windows.Forms.Button btnRfInterlockSet;
        private System.Windows.Forms.Button btnRfPowerOff;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPowerSet;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTunePreset;
        private System.Windows.Forms.TextBox tbLoadPreset;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
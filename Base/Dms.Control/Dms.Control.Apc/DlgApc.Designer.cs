namespace Dms.Control
{
    partial class DlgApc
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPointE = new System.Windows.Forms.RadioButton();
            this.btnPointD = new System.Windows.Forms.RadioButton();
            this.btnPointC = new System.Windows.Forms.RadioButton();
            this.btnPointB = new System.Windows.Forms.RadioButton();
            this.btnPointA = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnHold = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbValue);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "APC Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "torr / %";
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(6, 26);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(115, 21);
            this.tbValue.TabIndex = 2;
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.Color.PapayaWhip;
            this.btnSet.Location = new System.Drawing.Point(193, 20);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 34);
            this.btnSet.TabIndex = 1;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPointE);
            this.groupBox2.Controls.Add(this.btnPointD);
            this.groupBox2.Controls.Add(this.btnPointC);
            this.groupBox2.Controls.Add(this.btnPointB);
            this.groupBox2.Controls.Add(this.btnPointA);
            this.groupBox2.Location = new System.Drawing.Point(6, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 47);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Point";
            // 
            // btnPointE
            // 
            this.btnPointE.AutoSize = true;
            this.btnPointE.Location = new System.Drawing.Point(224, 20);
            this.btnPointE.Name = "btnPointE";
            this.btnPointE.Size = new System.Drawing.Size(31, 16);
            this.btnPointE.TabIndex = 0;
            this.btnPointE.Text = "E";
            this.btnPointE.UseVisualStyleBackColor = true;
            this.btnPointE.Click += new System.EventHandler(this.btnPointE_Click);
            // 
            // btnPointD
            // 
            this.btnPointD.AutoSize = true;
            this.btnPointD.Location = new System.Drawing.Point(170, 20);
            this.btnPointD.Name = "btnPointD";
            this.btnPointD.Size = new System.Drawing.Size(31, 16);
            this.btnPointD.TabIndex = 0;
            this.btnPointD.Text = "D";
            this.btnPointD.UseVisualStyleBackColor = true;
            this.btnPointD.Click += new System.EventHandler(this.btnPointD_Click);
            // 
            // btnPointC
            // 
            this.btnPointC.AutoSize = true;
            this.btnPointC.Location = new System.Drawing.Point(115, 20);
            this.btnPointC.Name = "btnPointC";
            this.btnPointC.Size = new System.Drawing.Size(32, 16);
            this.btnPointC.TabIndex = 0;
            this.btnPointC.Text = "C";
            this.btnPointC.UseVisualStyleBackColor = true;
            this.btnPointC.Click += new System.EventHandler(this.btnPointC_Click);
            // 
            // btnPointB
            // 
            this.btnPointB.AutoSize = true;
            this.btnPointB.Location = new System.Drawing.Point(61, 20);
            this.btnPointB.Name = "btnPointB";
            this.btnPointB.Size = new System.Drawing.Size(31, 16);
            this.btnPointB.TabIndex = 0;
            this.btnPointB.Text = "B";
            this.btnPointB.UseVisualStyleBackColor = true;
            this.btnPointB.Click += new System.EventHandler(this.btnPointB_Click);
            // 
            // btnPointA
            // 
            this.btnPointA.AutoSize = true;
            this.btnPointA.Location = new System.Drawing.Point(7, 20);
            this.btnPointA.Name = "btnPointA";
            this.btnPointA.Size = new System.Drawing.Size(31, 16);
            this.btnPointA.TabIndex = 0;
            this.btnPointA.Text = "A";
            this.btnPointA.UseVisualStyleBackColor = true;
            this.btnPointA.Click += new System.EventHandler(this.btnPointA_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnHold);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnOpen);
            this.groupBox3.Location = new System.Drawing.Point(12, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 74);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Throttle Valve";
            // 
            // btnHold
            // 
            this.btnHold.Location = new System.Drawing.Point(184, 20);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(86, 48);
            this.btnHold.TabIndex = 0;
            this.btnHold.Text = "Stop";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(95, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 48);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(6, 20);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(86, 48);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // DlgApc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(303, 220);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "DlgApc";
            this.Text = "APC Manual Operation";
            this.Load += new System.EventHandler(this.DlgApc_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton btnPointC;
        private System.Windows.Forms.RadioButton btnPointB;
        private System.Windows.Forms.RadioButton btnPointA;
        private System.Windows.Forms.RadioButton btnPointE;
        private System.Windows.Forms.RadioButton btnPointD;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOpen;
    }
}
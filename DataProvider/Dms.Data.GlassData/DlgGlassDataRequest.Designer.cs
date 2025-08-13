namespace Dms.Data
{
    partial class DlgGlassDataRequest
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
            this.chkOption3 = new System.Windows.Forms.CheckBox();
            this.chkOption2 = new System.Windows.Forms.CheckBox();
            this.chkOption1 = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkOption3);
            this.groupBox1.Controls.Add(this.chkOption2);
            this.groupBox1.Controls.Add(this.chkOption1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request Option";
            // 
            // chkOption3
            // 
            this.chkOption3.Location = new System.Drawing.Point(6, 81);
            this.chkOption3.Name = "chkOption3";
            this.chkOption3.Size = new System.Drawing.Size(268, 24);
            this.chkOption3.TabIndex = 0;
            this.chkOption3.Text = "Glass Data Request by Glass Code and ID";
            this.chkOption3.UseVisualStyleBackColor = true;
            this.chkOption3.Click += new System.EventHandler(this.chkOption3_Click);
            // 
            // chkOption2
            // 
            this.chkOption2.Location = new System.Drawing.Point(6, 51);
            this.chkOption2.Name = "chkOption2";
            this.chkOption2.Size = new System.Drawing.Size(268, 24);
            this.chkOption2.TabIndex = 0;
            this.chkOption2.Text = "Glass Data Request by Glass ID";
            this.chkOption2.UseVisualStyleBackColor = true;
            this.chkOption2.Click += new System.EventHandler(this.chkOption2_Click);
            // 
            // chkOption1
            // 
            this.chkOption1.Location = new System.Drawing.Point(7, 21);
            this.chkOption1.Name = "chkOption1";
            this.chkOption1.Size = new System.Drawing.Size(268, 24);
            this.chkOption1.TabIndex = 0;
            this.chkOption1.Text = "Glass Data Request by Glass Code";
            this.chkOption1.UseVisualStyleBackColor = true;
            this.chkOption1.Click += new System.EventHandler(this.chkOption1_Click);
            // 
            // OK
            // 
            this.OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.OK.Location = new System.Drawing.Point(182, 132);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 43);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = false;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // DlgGlassDataRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 185);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.Name = "DlgGlassDataRequest";
            this.Text = "Data Request";
            this.Load += new System.EventHandler(this.DlgGlassDataRequest_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.CheckBox chkOption1;
        private System.Windows.Forms.CheckBox chkOption3;
        private System.Windows.Forms.CheckBox chkOption2;
    }
}
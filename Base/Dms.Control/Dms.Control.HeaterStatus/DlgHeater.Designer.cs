namespace Dms.Control
{
    partial class DlgHeater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgHeater));
            this.btnSet = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnResetOn = new System.Windows.Forms.Button();
            this.tbTemp = new Dms.Common.ValidationTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnSet.Location = new System.Drawing.Point(99, 19);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(118, 26);
            this.btnSet.TabIndex = 1;
            this.btnSet.Text = "Set Temp";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(135, 169);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 51);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOff);
            this.groupBox1.Controls.Add(this.btnResetOn);
            this.groupBox1.Controls.Add(this.btnOn);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.tbTemp);
            this.groupBox1.Location = new System.Drawing.Point(5, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 162);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnOff
            // 
            this.btnOff.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnOff.Location = new System.Drawing.Point(5, 106);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(88, 51);
            this.btnOff.TabIndex = 1;
            this.btnOff.Text = "HEATER Off";
            this.btnOff.UseVisualStyleBackColor = false;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnOn
            // 
            this.btnOn.BackColor = System.Drawing.Color.LightYellow;
            this.btnOn.Location = new System.Drawing.Point(5, 48);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(88, 51);
            this.btnOn.TabIndex = 1;
            this.btnOn.Text = "Heater On";
            this.btnOn.UseVisualStyleBackColor = false;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnResetOn
            // 
            this.btnResetOn.BackColor = System.Drawing.Color.LightYellow;
            this.btnResetOn.Location = new System.Drawing.Point(99, 48);
            this.btnResetOn.Name = "btnResetOn";
            this.btnResetOn.Size = new System.Drawing.Size(118, 109);
            this.btnResetOn.TabIndex = 1;
            this.btnResetOn.Text = "Reset";
            this.btnResetOn.UseVisualStyleBackColor = false;
            this.btnResetOn.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // tbTemp
            // 
            this.tbTemp.DataFormat = Dms.Common.OptionFormat.Digit;
            this.tbTemp.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbTemp.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("tbTemp.KeyPadInfo")));
            this.tbTemp.LimitHigh = "";
            this.tbTemp.LimitLow = "0";
            this.tbTemp.Location = new System.Drawing.Point(5, 22);
            this.tbTemp.Name = "tbTemp";
            this.tbTemp.ReferenceTag = null;
            this.tbTemp.Size = new System.Drawing.Size(88, 20);
            this.tbTemp.TabIndex = 0;
            this.tbTemp.Text = "0";
            this.tbTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbTemp.UsedInKeyPad = false;
            // 
            // DlgHeater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 225);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgHeater";
            this.Text = "DlgHeater";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Common.ValidationTextBox tbTemp;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Button btnResetOn;
    }
}
namespace Dms.Server
{
    partial class FormFtp
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textUserID = new System.Windows.Forms.TextBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textIpAddress = new System.Windows.Forms.TextBox();
            this.textFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textTargetDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "User ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "IP Address";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(102, 80);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(82, 21);
            this.textPassword.TabIndex = 24;
            // 
            // textUserID
            // 
            this.textUserID.Location = new System.Drawing.Point(102, 53);
            this.textUserID.Name = "textUserID";
            this.textUserID.Size = new System.Drawing.Size(82, 21);
            this.textUserID.TabIndex = 23;
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(261, 26);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(39, 21);
            this.textPort.TabIndex = 25;
            // 
            // textIpAddress
            // 
            this.textIpAddress.Location = new System.Drawing.Point(102, 26);
            this.textIpAddress.Name = "textIpAddress";
            this.textIpAddress.Size = new System.Drawing.Size(106, 21);
            this.textIpAddress.TabIndex = 26;
            // 
            // textFileName
            // 
            this.textFileName.Location = new System.Drawing.Point(102, 107);
            this.textFileName.Name = "textFileName";
            this.textFileName.Size = new System.Drawing.Size(327, 21);
            this.textFileName.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "Upload File";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(440, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 21);
            this.button1.TabIndex = 32;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Location = new System.Drawing.Point(395, 153);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(145, 81);
            this.buttonUpload.TabIndex = 32;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textFileName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.textTargetDir);
            this.groupBox1.Controls.Add(this.textUserID);
            this.groupBox1.Controls.Add(this.textPort);
            this.groupBox1.Controls.Add(this.textIpAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 142);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textTargetDir
            // 
            this.textTargetDir.Location = new System.Drawing.Point(261, 53);
            this.textTargetDir.Name = "textTargetDir";
            this.textTargetDir.Size = new System.Drawing.Size(168, 21);
            this.textTargetDir.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(193, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "Target Dir";
            // 
            // FormFtp
            // 
            this.ClientSize = new System.Drawing.Size(565, 246);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonUpload);
            this.Name = "FormFtp";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textUserID;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textIpAddress;
        private System.Windows.Forms.TextBox textFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textTargetDir;
    }
}
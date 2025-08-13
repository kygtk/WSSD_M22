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
            this.btnDN = new System.Windows.Forms.Button();
            this.btnUP = new System.Windows.Forms.Button();
            this.checkUP = new System.Windows.Forms.CheckBox();
            this.checkDN = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timerUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checUnLock = new System.Windows.Forms.CheckBox();
            this.checkLock = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDN
            // 
            this.btnDN.BackColor = System.Drawing.Color.Pink;
            this.btnDN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDN.Location = new System.Drawing.Point(113, 23);
            this.btnDN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDN.Name = "btnDN";
            this.btnDN.Size = new System.Drawing.Size(90, 51);
            this.btnDN.TabIndex = 0;
            this.btnDN.Text = "DN";
            this.btnDN.UseVisualStyleBackColor = false;
            this.btnDN.Click += new System.EventHandler(this.btnBW_Click);
            // 
            // btnUP
            // 
            this.btnUP.BackColor = System.Drawing.Color.LightCyan;
            this.btnUP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUP.Location = new System.Drawing.Point(14, 23);
            this.btnUP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUP.Name = "btnUP";
            this.btnUP.Size = new System.Drawing.Size(90, 51);
            this.btnUP.TabIndex = 0;
            this.btnUP.Text = "UP";
            this.btnUP.UseVisualStyleBackColor = false;
            this.btnUP.Click += new System.EventHandler(this.btnFW_Click);
            // 
            // checkUP
            // 
            this.checkUP.AutoCheck = false;
            this.checkUP.AutoSize = true;
            this.checkUP.Location = new System.Drawing.Point(14, 27);
            this.checkUP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkUP.Name = "checkUP";
            this.checkUP.Size = new System.Drawing.Size(86, 19);
            this.checkUP.TabIndex = 1;
            this.checkUP.Text = "UP Sensor";
            this.checkUP.UseVisualStyleBackColor = true;
            // 
            // checkDN
            // 
            this.checkDN.AutoCheck = false;
            this.checkDN.AutoSize = true;
            this.checkDN.Location = new System.Drawing.Point(113, 27);
            this.checkDN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkDN.Name = "checkDN";
            this.checkDN.Size = new System.Drawing.Size(87, 19);
            this.checkDN.TabIndex = 1;
            this.checkDN.Text = "DN Sensor";
            this.checkDN.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Linen;
            this.btnOK.Location = new System.Drawing.Point(365, 167);
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
            this.groupBox1.Controls.Add(this.btnDN);
            this.groupBox1.Controls.Add(this.btnUP);
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
            this.groupBox2.Controls.Add(this.checkDN);
            this.groupBox2.Controls.Add(this.checkUP);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checUnLock);
            this.groupBox3.Controls.Add(this.checkLock);
            this.groupBox3.Location = new System.Drawing.Point(239, 5);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(215, 54);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "OutPut State";
            // 
            // checUnLock
            // 
            this.checUnLock.AutoCheck = false;
            this.checUnLock.AutoSize = true;
            this.checUnLock.Location = new System.Drawing.Point(113, 27);
            this.checUnLock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checUnLock.Name = "checUnLock";
            this.checUnLock.Size = new System.Drawing.Size(68, 19);
            this.checUnLock.TabIndex = 1;
            this.checUnLock.Text = "UnLock";
            this.checUnLock.UseVisualStyleBackColor = true;
            // 
            // checkLock
            // 
            this.checkLock.AutoCheck = false;
            this.checkLock.AutoSize = true;
            this.checkLock.Location = new System.Drawing.Point(14, 27);
            this.checkLock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkLock.Name = "checkLock";
            this.checkLock.Size = new System.Drawing.Size(52, 19);
            this.checkLock.TabIndex = 1;
            this.checkLock.Text = "Lock";
            this.checkLock.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Location = new System.Drawing.Point(239, 67);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(216, 89);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Action";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Pink;
            this.button1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(113, 23);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "UnLock";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightCyan;
            this.button2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(14, 23);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 51);
            this.button2.TabIndex = 0;
            this.button2.Text = "Lock";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // CylinderOperateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(467, 231);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
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
            this.Text = "TR Chuck Cylinder";
            this.Load += new System.EventHandler(this.CylinderOperateForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDN;
        private System.Windows.Forms.Button btnUP;
        private System.Windows.Forms.CheckBox checkUP;
        private System.Windows.Forms.CheckBox checkDN;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timerUpdateState;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checUnLock;
        private System.Windows.Forms.CheckBox checkLock;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
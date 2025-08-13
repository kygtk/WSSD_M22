namespace Dms.Server
{
    partial class FormServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FireEvent = new System.Windows.Forms.Button();
            this.close2 = new System.Windows.Forms.Button();
            this.buttonAlarm = new System.Windows.Forms.Button();
            this.buttonLdIn = new System.Windows.Forms.Button();
            this.buttonAlarmList = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonRecipe = new System.Windows.Forms.Button();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.SeverLogHistory = new System.Windows.Forms.ListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonInit = new System.Windows.Forms.Button();
            this.labelServerState = new System.Windows.Forms.Label();
            this.buttonFTP = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // FireEvent
            // 
            this.FireEvent.Location = new System.Drawing.Point(201, 20);
            this.FireEvent.Name = "FireEvent";
            this.FireEvent.Size = new System.Drawing.Size(102, 23);
            this.FireEvent.TabIndex = 1;
            this.FireEvent.Text = "Fire Event";
            this.FireEvent.UseVisualStyleBackColor = true;
            this.FireEvent.Click += new System.EventHandler(this.FireEvent_Click);
            // 
            // close2
            // 
            this.close2.Location = new System.Drawing.Point(12, 334);
            this.close2.Name = "close2";
            this.close2.Size = new System.Drawing.Size(121, 34);
            this.close2.TabIndex = 2;
            this.close2.Text = "Close";
            this.close2.UseVisualStyleBackColor = true;
            this.close2.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // buttonAlarm
            // 
            this.buttonAlarm.BackColor = System.Drawing.Color.Moccasin;
            this.buttonAlarm.Location = new System.Drawing.Point(129, 20);
            this.buttonAlarm.Name = "buttonAlarm";
            this.buttonAlarm.Size = new System.Drawing.Size(102, 23);
            this.buttonAlarm.TabIndex = 3;
            this.buttonAlarm.Text = "Alarm Reset";
            this.buttonAlarm.UseVisualStyleBackColor = false;
            this.buttonAlarm.Click += new System.EventHandler(this.buttonAlarm_Click);
            // 
            // buttonLdIn
            // 
            this.buttonLdIn.Location = new System.Drawing.Point(12, 20);
            this.buttonLdIn.Name = "buttonLdIn";
            this.buttonLdIn.Size = new System.Drawing.Size(102, 23);
            this.buttonLdIn.TabIndex = 4;
            this.buttonLdIn.Text = "LD In On";
            this.buttonLdIn.UseVisualStyleBackColor = true;
            this.buttonLdIn.Click += new System.EventHandler(this.buttonLdIn_Click);
            // 
            // buttonAlarmList
            // 
            this.buttonAlarmList.Location = new System.Drawing.Point(21, 19);
            this.buttonAlarmList.Name = "buttonAlarmList";
            this.buttonAlarmList.Size = new System.Drawing.Size(102, 23);
            this.buttonAlarmList.TabIndex = 5;
            this.buttonAlarmList.Text = "Alarm List";
            this.buttonAlarmList.UseVisualStyleBackColor = true;
            this.buttonAlarmList.Click += new System.EventHandler(this.buttonAlarmList_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.buttonLdIn);
            this.groupBox2.Controls.Add(this.buttonAlarm);
            this.groupBox2.Location = new System.Drawing.Point(156, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 257);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(14, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(100, 123);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Sensor";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(14, 98);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(72, 19);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "LL Level";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(14, 79);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(65, 19);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "L Level";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(14, 60);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(67, 19);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "M Level";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(14, 41);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(67, 19);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "H Level";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoCheck = false;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(14, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "HH Level";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.HHLevel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonFTP);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.FireEvent);
            this.groupBox3.Controls.Add(this.buttonRecipe);
            this.groupBox3.Controls.Add(this.buttonSetup);
            this.groupBox3.Controls.Add(this.buttonAlarmList);
            this.groupBox3.Location = new System.Drawing.Point(481, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(319, 257);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Test";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "DeviceTags";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRecipe
            // 
            this.buttonRecipe.Location = new System.Drawing.Point(21, 48);
            this.buttonRecipe.Name = "buttonRecipe";
            this.buttonRecipe.Size = new System.Drawing.Size(102, 23);
            this.buttonRecipe.TabIndex = 16;
            this.buttonRecipe.Text = "Recipe";
            this.buttonRecipe.UseVisualStyleBackColor = true;
            this.buttonRecipe.Click += new System.EventHandler(this.buttonRecipe_Click);
            // 
            // buttonSetup
            // 
            this.buttonSetup.Location = new System.Drawing.Point(21, 77);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(102, 23);
            this.buttonSetup.TabIndex = 16;
            this.buttonSetup.Text = "Setup Item";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // SeverLogHistory
            // 
            this.SeverLogHistory.FormattingEnabled = true;
            this.SeverLogHistory.ItemHeight = 15;
            this.SeverLogHistory.Items.AddRange(new object[] {
            "Welcome to the WSSD Server !!",
            "======================================"});
            this.SeverLogHistory.Location = new System.Drawing.Point(481, 286);
            this.SeverLogHistory.Name = "SeverLogHistory";
            this.SeverLogHistory.ScrollAlwaysVisible = true;
            this.SeverLogHistory.Size = new System.Drawing.Size(319, 64);
            this.SeverLogHistory.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 100);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(121, 34);
            this.buttonStart.TabIndex = 16;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(12, 138);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(121, 34);
            this.buttonPause.TabIndex = 17;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(12, 62);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(121, 34);
            this.buttonInit.TabIndex = 18;
            this.buttonInit.Text = "Initialize";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // labelServerState
            // 
            this.labelServerState.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelServerState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelServerState.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerState.ForeColor = System.Drawing.Color.Blue;
            this.labelServerState.Location = new System.Drawing.Point(12, 16);
            this.labelServerState.Name = "labelServerState";
            this.labelServerState.Size = new System.Drawing.Size(121, 32);
            this.labelServerState.TabIndex = 19;
            this.labelServerState.Text = "Server State";
            this.labelServerState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonFTP
            // 
            this.buttonFTP.Location = new System.Drawing.Point(201, 52);
            this.buttonFTP.Name = "buttonFTP";
            this.buttonFTP.Size = new System.Drawing.Size(102, 23);
            this.buttonFTP.TabIndex = 23;
            this.buttonFTP.Text = "FTP Test";
            this.buttonFTP.UseVisualStyleBackColor = true;
            this.buttonFTP.Click += new System.EventHandler(this.buttonFTP_Click);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(822, 380);
            this.ControlBox = false;
            this.Controls.Add(this.labelServerState);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.close2);
            this.Controls.Add(this.SeverLogHistory);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remoting Server";
            this.Load += new System.EventHandler(this.RemServerForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FireEvent;
        private System.Windows.Forms.Button close2;
        private System.Windows.Forms.Button buttonAlarm;
        private System.Windows.Forms.Button buttonLdIn;
        private System.Windows.Forms.Button buttonAlarmList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonRecipe;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox SeverLogHistory;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Label labelServerState;
        private System.Windows.Forms.Button buttonFTP;
    }
}


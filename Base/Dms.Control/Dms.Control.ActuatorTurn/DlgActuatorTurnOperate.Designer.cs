namespace Dms.Control
{
    partial class DlgActuatorTurnOperate
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
            this.comboBoxPosList = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMove = new System.Windows.Forms.Button();
            this.lblCurrentUnitName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCurrentPosition = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEStop = new System.Windows.Forms.Button();
            this.checkBoxAlarm = new System.Windows.Forms.CheckBox();
            this.buttonAlarmReset = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxPosList
            // 
            this.comboBoxPosList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPosList.FormattingEnabled = true;
            this.comboBoxPosList.Location = new System.Drawing.Point(76, 16);
            this.comboBoxPosList.Name = "comboBoxPosList";
            this.comboBoxPosList.Size = new System.Drawing.Size(147, 20);
            this.comboBoxPosList.TabIndex = 0;
            this.comboBoxPosList.SelectedIndexChanged += new System.EventHandler(this.comboBoxPosList_SelectedIndexChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(138, 213);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(119, 47);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMove
            // 
            this.buttonMove.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttonMove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonMove.Location = new System.Drawing.Point(12, 147);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(117, 47);
            this.buttonMove.TabIndex = 2;
            this.buttonMove.Text = "Move";
            this.buttonMove.UseVisualStyleBackColor = false;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // lblCurrentUnitName
            // 
            this.lblCurrentUnitName.BackColor = System.Drawing.Color.White;
            this.lblCurrentUnitName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentUnitName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentUnitName.Location = new System.Drawing.Point(12, 9);
            this.lblCurrentUnitName.Name = "lblCurrentUnitName";
            this.lblCurrentUnitName.Size = new System.Drawing.Size(243, 27);
            this.lblCurrentUnitName.TabIndex = 3;
            this.lblCurrentUnitName.Text = "Unit Name";
            this.lblCurrentUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCurrentPosition);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxPosList);
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 77);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // lblCurrentPosition
            // 
            this.lblCurrentPosition.BackColor = System.Drawing.Color.White;
            this.lblCurrentPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentPosition.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPosition.Location = new System.Drawing.Point(76, 43);
            this.lblCurrentPosition.Name = "lblCurrentPosition";
            this.lblCurrentPosition.Size = new System.Drawing.Size(147, 21);
            this.lblCurrentPosition.TabIndex = 3;
            this.lblCurrentPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Current";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Target";
            // 
            // buttonEStop
            // 
            this.buttonEStop.BackColor = System.Drawing.Color.Pink;
            this.buttonEStop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonEStop.Location = new System.Drawing.Point(138, 147);
            this.buttonEStop.Name = "buttonEStop";
            this.buttonEStop.Size = new System.Drawing.Size(117, 47);
            this.buttonEStop.TabIndex = 2;
            this.buttonEStop.Text = "E-Stop";
            this.buttonEStop.UseVisualStyleBackColor = false;
            this.buttonEStop.Click += new System.EventHandler(this.buttonEStop_Click);
            // 
            // checkBoxAlarm
            // 
            this.checkBoxAlarm.AutoCheck = false;
            this.checkBoxAlarm.AutoSize = true;
            this.checkBoxAlarm.Location = new System.Drawing.Point(12, 229);
            this.checkBoxAlarm.Name = "checkBoxAlarm";
            this.checkBoxAlarm.Size = new System.Drawing.Size(57, 16);
            this.checkBoxAlarm.TabIndex = 5;
            this.checkBoxAlarm.Text = "Alarm";
            this.checkBoxAlarm.UseVisualStyleBackColor = true;
            // 
            // buttonAlarmReset
            // 
            this.buttonAlarmReset.BackColor = System.Drawing.Color.Wheat;
            this.buttonAlarmReset.Location = new System.Drawing.Point(70, 213);
            this.buttonAlarmReset.Name = "buttonAlarmReset";
            this.buttonAlarmReset.Size = new System.Drawing.Size(59, 47);
            this.buttonAlarmReset.TabIndex = 6;
            this.buttonAlarmReset.Text = "Reset";
            this.buttonAlarmReset.UseVisualStyleBackColor = false;
            this.buttonAlarmReset.Click += new System.EventHandler(this.buttonAlarmReset_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgActuatorTurnOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(268, 275);
            this.ControlBox = false;
            this.Controls.Add(this.buttonAlarmReset);
            this.Controls.Add(this.checkBoxAlarm);
            this.Controls.Add(this.lblCurrentUnitName);
            this.Controls.Add(this.buttonEStop);
            this.Controls.Add(this.buttonMove);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgActuatorTurnOperate";
            this.Text = "Operate";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPosList;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Label lblCurrentUnitName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrentPosition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEStop;
        private System.Windows.Forms.CheckBox checkBoxAlarm;
        private System.Windows.Forms.Button buttonAlarmReset;
        private System.Windows.Forms.Timer tmrUpdateState;

    }
}
namespace Dms.Server
{
    partial class FormAlarmList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlarmList));
            this.viewCurrentAlarms = new Dms.Data.ViewCurrentAlarms();
            this.viewAlarmList = new Dms.Data.ViewAlarmList();
            this.buttonAlarmReset = new System.Windows.Forms.Button();
            this.buttonAlarmSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textAlarmId = new Dms.Common.ValidationTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // viewCurrentAlarms
            // 
            this.viewCurrentAlarms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewCurrentAlarms.BackColor = System.Drawing.Color.Transparent;
            this.viewCurrentAlarms.ColumnHeadersVisible = true;
            this.viewCurrentAlarms.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewCurrentAlarms.Location = new System.Drawing.Point(461, 12);
            this.viewCurrentAlarms.Name = "viewCurrentAlarms";
            this.viewCurrentAlarms.Size = new System.Drawing.Size(298, 393);
            this.viewCurrentAlarms.TabIndex = 1;
            // 
            // viewAlarmList
            // 
            this.viewAlarmList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewAlarmList.BackColor = System.Drawing.Color.Transparent;
            this.viewAlarmList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewAlarmList.Location = new System.Drawing.Point(12, 12);
            this.viewAlarmList.Name = "viewAlarmList";
            this.viewAlarmList.Size = new System.Drawing.Size(443, 467);
            this.viewAlarmList.TabIndex = 0;
            // 
            // buttonAlarmReset
            // 
            this.buttonAlarmReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAlarmReset.Location = new System.Drawing.Point(628, 448);
            this.buttonAlarmReset.Name = "buttonAlarmReset";
            this.buttonAlarmReset.Size = new System.Drawing.Size(75, 23);
            this.buttonAlarmReset.TabIndex = 17;
            this.buttonAlarmReset.Text = "Reset";
            this.buttonAlarmReset.UseVisualStyleBackColor = true;
            this.buttonAlarmReset.Click += new System.EventHandler(this.buttonAlarmReset_Click);
            // 
            // buttonAlarmSet
            // 
            this.buttonAlarmSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAlarmSet.Location = new System.Drawing.Point(628, 419);
            this.buttonAlarmSet.Name = "buttonAlarmSet";
            this.buttonAlarmSet.Size = new System.Drawing.Size(75, 23);
            this.buttonAlarmSet.TabIndex = 16;
            this.buttonAlarmSet.Text = "Set";
            this.buttonAlarmSet.UseVisualStyleBackColor = true;
            this.buttonAlarmSet.Click += new System.EventHandler(this.buttonAlarmSet_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(469, 424);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "AlarmID";
            // 
            // textAlarmId
            // 
            this.textAlarmId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textAlarmId.DataFormat = Dms.Common.OptionFormat.Digit;
            this.textAlarmId.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textAlarmId.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("textAlarmId.KeyPadInfo")));
            this.textAlarmId.LimitHigh = "9999";
            this.textAlarmId.LimitLow = "1";
            this.textAlarmId.Location = new System.Drawing.Point(522, 420);
            this.textAlarmId.Name = "textAlarmId";
            this.textAlarmId.Size = new System.Drawing.Size(100, 21);
            this.textAlarmId.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 483);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(443, 48);
            this.button1.TabIndex = 18;
            this.button1.Text = "Update to DB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(466, 483);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(283, 48);
            this.buttonClose.TabIndex = 19;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormAlarmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(772, 543);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonAlarmReset);
            this.Controls.Add(this.buttonAlarmSet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textAlarmId);
            this.Controls.Add(this.viewCurrentAlarms);
            this.Controls.Add(this.viewAlarmList);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormAlarmList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAlarmList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Dms.Data.ViewAlarmList viewAlarmList;
        private Dms.Data.ViewCurrentAlarms viewCurrentAlarms;
        private System.Windows.Forms.Button buttonAlarmReset;
        private System.Windows.Forms.Button buttonAlarmSet;
        private System.Windows.Forms.Label label1;
        private Dms.Common.ValidationTextBox textAlarmId;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonClose;
    }
}
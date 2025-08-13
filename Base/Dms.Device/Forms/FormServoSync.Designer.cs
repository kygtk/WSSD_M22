namespace Dms.Device
{
    partial class FormServoSync
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMaster = new System.Windows.Forms.ComboBox();
            this.comboBoxSlave = new System.Windows.Forms.ComboBox();
            this.checkBoxSync = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.setupSensorInterlockTableAdapter = new Dms.Data.DataSetSetupItemTableAdapters.SetupSensorInterlockTableAdapter();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Master ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Slave ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMaster
            // 
            this.comboBoxMaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaster.FormattingEnabled = true;
            this.comboBoxMaster.Location = new System.Drawing.Point(90, 21);
            this.comboBoxMaster.Name = "comboBoxMaster";
            this.comboBoxMaster.Size = new System.Drawing.Size(121, 23);
            this.comboBoxMaster.TabIndex = 12;
            this.comboBoxMaster.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedIndexChanged);
            // 
            // comboBoxSlave
            // 
            this.comboBoxSlave.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSlave.FormattingEnabled = true;
            this.comboBoxSlave.Location = new System.Drawing.Point(90, 53);
            this.comboBoxSlave.Name = "comboBoxSlave";
            this.comboBoxSlave.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSlave.TabIndex = 13;
            this.comboBoxSlave.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedIndexChanged);
            // 
            // checkBoxSync
            // 
            this.checkBoxSync.AutoSize = true;
            this.checkBoxSync.Location = new System.Drawing.Point(12, 94);
            this.checkBoxSync.Name = "checkBoxSync";
            this.checkBoxSync.Size = new System.Drawing.Size(95, 19);
            this.checkBoxSync.TabIndex = 14;
            this.checkBoxSync.Text = "Sync Control";
            this.checkBoxSync.UseVisualStyleBackColor = true;
            this.checkBoxSync.CheckedChanged += new System.EventHandler(this.checkBoxSync_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(122, 127);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(41, 127);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 16;
            this.buttonSelect.Text = "Ok";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // setupSensorInterlockTableAdapter1
            // 
            this.setupSensorInterlockTableAdapter.ClearBeforeFill = true;
            // 
            // FormServoSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(230, 175);
            this.ControlBox = false;
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxSync);
            this.Controls.Add(this.comboBoxSlave);
            this.Controls.Add(this.comboBoxMaster);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormServoSync";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Sync Contol";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMaster;
        private System.Windows.Forms.ComboBox comboBoxSlave;
        private System.Windows.Forms.CheckBox checkBoxSync;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSelect;
        private Dms.Data.DataSetSetupItemTableAdapters.SetupSensorInterlockTableAdapter setupSensorInterlockTableAdapter;
    }
}
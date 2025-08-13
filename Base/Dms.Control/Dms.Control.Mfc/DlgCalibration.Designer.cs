namespace Dms.Control
{
    partial class DlgCalibration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCalibration));
            this.groupBoxCalibraionParamters = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtAdcCur = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdcMax = new Dms.Common.ValidationTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAdcMin = new Dms.Common.ValidationTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtRealCur = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRealMax = new Dms.Common.ValidationTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRealMin = new Dms.Common.ValidationTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBoxCalibraionParamters.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCalibraionParamters
            // 
            this.groupBoxCalibraionParamters.Controls.Add(this.groupBox3);
            this.groupBoxCalibraionParamters.Controls.Add(this.groupBox2);
            this.groupBoxCalibraionParamters.Location = new System.Drawing.Point(5, 2);
            this.groupBoxCalibraionParamters.Name = "groupBoxCalibraionParamters";
            this.groupBoxCalibraionParamters.Size = new System.Drawing.Size(238, 120);
            this.groupBoxCalibraionParamters.TabIndex = 0;
            this.groupBoxCalibraionParamters.TabStop = false;
            this.groupBoxCalibraionParamters.Text = "Calibration Parameter";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtAdcCur);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtAdcMax);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtAdcMin);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(121, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 93);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ADC";
            // 
            // txtAdcCur
            // 
            this.txtAdcCur.Location = new System.Drawing.Point(43, 67);
            this.txtAdcCur.Name = "txtAdcCur";
            this.txtAdcCur.ReadOnly = true;
            this.txtAdcCur.Size = new System.Drawing.Size(60, 21);
            this.txtAdcCur.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "CUR";
            // 
            // txtAdcMax
            // 
            this.txtAdcMax.DataFormat = Dms.Common.OptionFormat.Digit;
            this.txtAdcMax.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtAdcMax.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtAdcMax.KeyPadInfo")));
            this.txtAdcMax.LimitHigh = "32767";
            this.txtAdcMax.LimitLow = "1";
            this.txtAdcMax.Location = new System.Drawing.Point(43, 42);
            this.txtAdcMax.MaxLength = 5;
            this.txtAdcMax.Name = "txtAdcMax";
            this.txtAdcMax.ReferenceTag = null;
            this.txtAdcMax.Size = new System.Drawing.Size(60, 21);
            this.txtAdcMax.TabIndex = 3;
            this.txtAdcMax.UsedInKeyPad = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "MAX";
            // 
            // txtAdcMin
            // 
            this.txtAdcMin.DataFormat = Dms.Common.OptionFormat.Digit;
            this.txtAdcMin.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtAdcMin.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtAdcMin.KeyPadInfo")));
            this.txtAdcMin.LimitHigh = "32767";
            this.txtAdcMin.LimitLow = "1";
            this.txtAdcMin.Location = new System.Drawing.Point(43, 18);
            this.txtAdcMin.MaxLength = 5;
            this.txtAdcMin.Name = "txtAdcMin";
            this.txtAdcMin.ReferenceTag = null;
            this.txtAdcMin.Size = new System.Drawing.Size(60, 21);
            this.txtAdcMin.TabIndex = 1;
            this.txtAdcMin.UsedInKeyPad = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "MIN";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtRealCur);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtRealMax);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtRealMin);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(110, 93);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // txtRealCur
            // 
            this.txtRealCur.Location = new System.Drawing.Point(43, 67);
            this.txtRealCur.Name = "txtRealCur";
            this.txtRealCur.ReadOnly = true;
            this.txtRealCur.Size = new System.Drawing.Size(60, 21);
            this.txtRealCur.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "CUR";
            // 
            // txtRealMax
            // 
            this.txtRealMax.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtRealMax.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtRealMax.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtRealMax.KeyPadInfo")));
            this.txtRealMax.LimitHigh = "";
            this.txtRealMax.LimitLow = "";
            this.txtRealMax.Location = new System.Drawing.Point(43, 42);
            this.txtRealMax.MaxLength = 5;
            this.txtRealMax.Name = "txtRealMax";
            this.txtRealMax.ReferenceTag = null;
            this.txtRealMax.Size = new System.Drawing.Size(60, 21);
            this.txtRealMax.TabIndex = 3;
            this.txtRealMax.UsedInKeyPad = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "MAX";
            // 
            // txtRealMin
            // 
            this.txtRealMin.DataFormat = Dms.Common.OptionFormat.Float;
            this.txtRealMin.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtRealMin.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtRealMin.KeyPadInfo")));
            this.txtRealMin.LimitHigh = "";
            this.txtRealMin.LimitLow = "";
            this.txtRealMin.Location = new System.Drawing.Point(43, 18);
            this.txtRealMin.MaxLength = 5;
            this.txtRealMin.Name = "txtRealMin";
            this.txtRealMin.ReferenceTag = null;
            this.txtRealMin.Size = new System.Drawing.Size(60, 21);
            this.txtRealMin.TabIndex = 1;
            this.txtRealMin.UsedInKeyPad = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "MIN";
            // 
            // btnCalibration
            // 
            this.btnCalibration.BackColor = System.Drawing.Color.Honeydew;
            this.btnCalibration.Location = new System.Drawing.Point(5, 128);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(117, 50);
            this.btnCalibration.TabIndex = 1;
            this.btnCalibration.Text = "CALIBRATION";
            this.btnCalibration.UseVisualStyleBackColor = false;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(126, 128);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 50);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(252, 183);
            this.ControlBox = false;
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBoxCalibraionParamters);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgCalibration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calibration : ";
            this.Load += new System.EventHandler(this.DlgCalibration_Load);
            this.groupBoxCalibraionParamters.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCalibraionParamters;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRealCur;
        private System.Windows.Forms.Label label3;
        private Dms.Common.ValidationTextBox txtRealMax;
        private System.Windows.Forms.Label label2;
        private Dms.Common.ValidationTextBox txtRealMin;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtAdcCur;
        private System.Windows.Forms.Label label4;
        private Dms.Common.ValidationTextBox txtAdcMax;
        private System.Windows.Forms.Label label5;
        private Dms.Common.ValidationTextBox txtAdcMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCalibration;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
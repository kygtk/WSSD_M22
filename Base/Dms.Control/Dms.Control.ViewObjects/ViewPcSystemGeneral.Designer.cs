namespace Dms.Control
{
    partial class ViewPcSystemGeneral
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewPcSystemGeneral));
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTempCpuMax = new Dms.Common.ValidationTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTempCpu = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTempCpu = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtDeviceType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.txtModelNo = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTempCpuMax);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkTempCpu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTempCpu);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 127);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Temperatures";
            // 
            // txtTempCpuMax
            // 
            this.txtTempCpuMax.DataFormat = Dms.Common.OptionFormat.Digit;
            this.txtTempCpuMax.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtTempCpuMax.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("txtTempCpuMax.KeyPadInfo")));
            this.txtTempCpuMax.LimitHigh = "";
            this.txtTempCpuMax.LimitLow = "";
            this.txtTempCpuMax.Location = new System.Drawing.Point(285, 33);
            this.txtTempCpuMax.Name = "txtTempCpuMax";
            this.txtTempCpuMax.ReferenceTag = null;
            this.txtTempCpuMax.Size = new System.Drawing.Size(79, 21);
            this.txtTempCpuMax.TabIndex = 6;
            this.txtTempCpuMax.UsedInKeyPad = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(280, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "MAX";
            // 
            // chkTempCpu
            // 
            this.chkTempCpu.AutoSize = true;
            this.chkTempCpu.Location = new System.Drawing.Point(263, 37);
            this.chkTempCpu.Name = "chkTempCpu";
            this.chkTempCpu.Size = new System.Drawing.Size(15, 14);
            this.chkTempCpu.TabIndex = 3;
            this.chkTempCpu.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "\'C";
            // 
            // txtTempCpu
            // 
            this.txtTempCpu.Location = new System.Drawing.Point(152, 33);
            this.txtTempCpu.Name = "txtTempCpu";
            this.txtTempCpu.ReadOnly = true;
            this.txtTempCpu.Size = new System.Drawing.Size(79, 21);
            this.txtTempCpu.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtDeviceType);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtSerialNo);
            this.groupBox5.Controls.Add(this.label45);
            this.groupBox5.Controls.Add(this.txtModelNo);
            this.groupBox5.Controls.Add(this.label47);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(412, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(461, 127);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Factory Setting";
            // 
            // txtDeviceType
            // 
            this.txtDeviceType.Location = new System.Drawing.Point(152, 33);
            this.txtDeviceType.Name = "txtDeviceType";
            this.txtDeviceType.ReadOnly = true;
            this.txtDeviceType.Size = new System.Drawing.Size(212, 21);
            this.txtDeviceType.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "DeviceType :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Location = new System.Drawing.Point(152, 89);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.ReadOnly = true;
            this.txtSerialNo.Size = new System.Drawing.Size(212, 21);
            this.txtSerialNo.TabIndex = 12;
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(7, 89);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(139, 19);
            this.label45.TabIndex = 11;
            this.label45.Text = "Serial No :";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModelNo
            // 
            this.txtModelNo.Location = new System.Drawing.Point(152, 61);
            this.txtModelNo.Name = "txtModelNo";
            this.txtModelNo.ReadOnly = true;
            this.txtModelNo.Size = new System.Drawing.Size(212, 21);
            this.txtModelNo.TabIndex = 7;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(7, 61);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(139, 19);
            this.label47.TabIndex = 6;
            this.label47.Text = "Model No :";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.6895F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.3105F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.52783F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.47217F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 521);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // ViewPcSystemGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewPcSystemGeneral";
            this.Size = new System.Drawing.Size(899, 535);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkTempCpu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTempCpu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txtModelNo;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtDeviceType;
        private System.Windows.Forms.Label label4;
        private Dms.Common.ValidationTextBox txtTempCpuMax;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

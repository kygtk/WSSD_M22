namespace Dms.Control
{
    partial class DlgDBMotor
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
            this.gbSpeed = new System.Windows.Forms.GroupBox();
            this.cboSpeed = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbAllOp = new System.Windows.Forms.GroupBox();
            this.btnAllStop = new System.Windows.Forms.RadioButton();
            this.btnAllCcw = new System.Windows.Forms.RadioButton();
            this.btnAllCw = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbIndividualOp = new System.Windows.Forms.GroupBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbSpeed.SuspendLayout();
            this.gbAllOp.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSpeed
            // 
            this.gbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSpeed.Controls.Add(this.cboSpeed);
            this.gbSpeed.Controls.Add(this.label1);
            this.gbSpeed.Location = new System.Drawing.Point(7, 6);
            this.gbSpeed.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gbSpeed.Name = "gbSpeed";
            this.gbSpeed.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gbSpeed.Size = new System.Drawing.Size(116, 57);
            this.gbSpeed.TabIndex = 1;
            this.gbSpeed.TabStop = false;
            this.gbSpeed.Text = "R/B Speed";
            // 
            // cboSpeed
            // 
            this.cboSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed.Location = new System.Drawing.Point(8, 23);
            this.cboSpeed.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboSpeed.Name = "cboSpeed";
            this.cboSpeed.Size = new System.Drawing.Size(76, 23);
            this.cboSpeed.TabIndex = 0;
            this.cboSpeed.SelectionChangeCommitted += new System.EventHandler(this.cboSpeed_SelectionChangeCommitted);
            this.cboSpeed.SelectedIndexChanged += new System.EventHandler(this.cboSpeed_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(84, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "rpm";
            // 
            // gbAllOp
            // 
            this.gbAllOp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAllOp.Controls.Add(this.btnAllStop);
            this.gbAllOp.Controls.Add(this.btnAllCcw);
            this.gbAllOp.Controls.Add(this.btnAllCw);
            this.gbAllOp.Location = new System.Drawing.Point(7, 66);
            this.gbAllOp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbAllOp.Name = "gbAllOp";
            this.gbAllOp.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbAllOp.Size = new System.Drawing.Size(116, 180);
            this.gbAllOp.TabIndex = 2;
            this.gbAllOp.TabStop = false;
            this.gbAllOp.Text = "All Operation";
            // 
            // btnAllStop
            // 
            this.btnAllStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllStop.BackColor = System.Drawing.Color.Pink;
            this.btnAllStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllStop.Location = new System.Drawing.Point(13, 126);
            this.btnAllStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAllStop.Name = "btnAllStop";
            this.btnAllStop.Size = new System.Drawing.Size(90, 40);
            this.btnAllStop.TabIndex = 2;
            this.btnAllStop.TabStop = true;
            this.btnAllStop.Text = "ALL STOP";
            this.btnAllStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllStop.UseVisualStyleBackColor = false;
            this.btnAllStop.Click += new System.EventHandler(this.btnAllStop_Click);
            // 
            // btnAllCcw
            // 
            this.btnAllCcw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllCcw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllCcw.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnAllCcw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllCcw.Location = new System.Drawing.Point(13, 76);
            this.btnAllCcw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAllCcw.Name = "btnAllCcw";
            this.btnAllCcw.Size = new System.Drawing.Size(90, 40);
            this.btnAllCcw.TabIndex = 1;
            this.btnAllCcw.TabStop = true;
            this.btnAllCcw.Text = "ALL CCW";
            this.btnAllCcw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllCcw.UseVisualStyleBackColor = false;
            this.btnAllCcw.Click += new System.EventHandler(this.btnAllCcw_Click);
            // 
            // btnAllCw
            // 
            this.btnAllCw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllCw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllCw.BackColor = System.Drawing.Color.LightCyan;
            this.btnAllCw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllCw.Location = new System.Drawing.Point(13, 26);
            this.btnAllCw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAllCw.Name = "btnAllCw";
            this.btnAllCw.Size = new System.Drawing.Size(90, 40);
            this.btnAllCw.TabIndex = 0;
            this.btnAllCw.TabStop = true;
            this.btnAllCw.Text = "ALL CW";
            this.btnAllCw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllCw.UseVisualStyleBackColor = false;
            this.btnAllCw.Click += new System.EventHandler(this.btnAllCw_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(23, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 41);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gbIndividualOp
            // 
            this.gbIndividualOp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbIndividualOp.Location = new System.Drawing.Point(132, 6);
            this.gbIndividualOp.Name = "gbIndividualOp";
            this.gbIndividualOp.Size = new System.Drawing.Size(365, 240);
            this.gbIndividualOp.TabIndex = 5;
            this.gbIndividualOp.TabStop = false;
            this.gbIndividualOp.Text = "Individual Operation";
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgDBMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(509, 301);
            this.ControlBox = false;
            this.Controls.Add(this.gbIndividualOp);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbAllOp);
            this.Controls.Add(this.gbSpeed);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDBMotor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "D/B Motor Manual Operation";
            this.Load += new System.EventHandler(this.DlgDBMotor_Load);
            this.gbSpeed.ResumeLayout(false);
            this.gbAllOp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSpeed;
        private System.Windows.Forms.ComboBox cboSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbAllOp;
        private System.Windows.Forms.RadioButton btnAllCw;
        private System.Windows.Forms.RadioButton btnAllStop;
        private System.Windows.Forms.RadioButton btnAllCcw;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbIndividualOp;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
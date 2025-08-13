namespace Dms.Control
{
    partial class DlgCvMotor
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
            this.btnAllBw = new System.Windows.Forms.RadioButton();
            this.btnAllFw = new System.Windows.Forms.RadioButton();
            this.btnExtension = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbIndividualOp = new System.Windows.Forms.GroupBox();
            this.tbPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbSpeed.SuspendLayout();
            this.gbAllOp.SuspendLayout();
            this.gbIndividualOp.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSpeed
            // 
            this.gbSpeed.Controls.Add(this.cboSpeed);
            this.gbSpeed.Controls.Add(this.label1);
            this.gbSpeed.Location = new System.Drawing.Point(8, 7);
            this.gbSpeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbSpeed.Name = "gbSpeed";
            this.gbSpeed.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbSpeed.Size = new System.Drawing.Size(116, 73);
            this.gbSpeed.TabIndex = 0;
            this.gbSpeed.TabStop = false;
            this.gbSpeed.Text = "C/V Speed";
            // 
            // cboSpeed
            // 
            this.cboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeed.Location = new System.Drawing.Point(10, 29);
            this.cboSpeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSpeed.Name = "cboSpeed";
            this.cboSpeed.Size = new System.Drawing.Size(96, 23);
            this.cboSpeed.TabIndex = 0;
            this.cboSpeed.SelectionChangeCommitted += new System.EventHandler(this.cboSpeed_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(41, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "(mm/min)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbAllOp
            // 
            this.gbAllOp.Controls.Add(this.btnAllStop);
            this.gbAllOp.Controls.Add(this.btnAllBw);
            this.gbAllOp.Controls.Add(this.btnAllFw);
            this.gbAllOp.Location = new System.Drawing.Point(8, 88);
            this.gbAllOp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbAllOp.Name = "gbAllOp";
            this.gbAllOp.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbAllOp.Size = new System.Drawing.Size(196, 213);
            this.gbAllOp.TabIndex = 1;
            this.gbAllOp.TabStop = false;
            this.gbAllOp.Text = "All Operation";
            // 
            // btnAllStop
            // 
            this.btnAllStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllStop.BackColor = System.Drawing.Color.Pink;
            this.btnAllStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllStop.Location = new System.Drawing.Point(9, 148);
            this.btnAllStop.Name = "btnAllStop";
            this.btnAllStop.Size = new System.Drawing.Size(179, 50);
            this.btnAllStop.TabIndex = 2;
            this.btnAllStop.Text = "ALL STOP";
            this.btnAllStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllStop.UseVisualStyleBackColor = false;
            this.btnAllStop.Click += new System.EventHandler(this.btnAllStop_Click);
            // 
            // btnAllBw
            // 
            this.btnAllBw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllBw.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnAllBw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllBw.Location = new System.Drawing.Point(9, 87);
            this.btnAllBw.Name = "btnAllBw";
            this.btnAllBw.Size = new System.Drawing.Size(179, 50);
            this.btnAllBw.TabIndex = 1;
            this.btnAllBw.Text = "ALL BW";
            this.btnAllBw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllBw.UseVisualStyleBackColor = false;
            this.btnAllBw.Click += new System.EventHandler(this.btnAllBw_Click);
            // 
            // btnAllFw
            // 
            this.btnAllFw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAllFw.BackColor = System.Drawing.Color.LightCyan;
            this.btnAllFw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllFw.Location = new System.Drawing.Point(9, 26);
            this.btnAllFw.Name = "btnAllFw";
            this.btnAllFw.Size = new System.Drawing.Size(179, 50);
            this.btnAllFw.TabIndex = 0;
            this.btnAllFw.Text = "ALL FW";
            this.btnAllFw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAllFw.UseVisualStyleBackColor = false;
            this.btnAllFw.Click += new System.EventHandler(this.btnAllFw_Click);
            // 
            // btnExtension
            // 
            this.btnExtension.BackColor = System.Drawing.Color.Linen;
            this.btnExtension.Location = new System.Drawing.Point(133, 16);
            this.btnExtension.Name = "btnExtension";
            this.btnExtension.Size = new System.Drawing.Size(70, 60);
            this.btnExtension.TabIndex = 2;
            this.btnExtension.Text = ">>\r\nDETAIL";
            this.btnExtension.UseVisualStyleBackColor = false;
            this.btnExtension.Click += new System.EventHandler(this.btnExtension_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(61, 307);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 41);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gbIndividualOp
            // 
            this.gbIndividualOp.Controls.Add(this.tbPanel);
            this.gbIndividualOp.Location = new System.Drawing.Point(223, 7);
            this.gbIndividualOp.Name = "gbIndividualOp";
            this.gbIndividualOp.Size = new System.Drawing.Size(710, 294);
            this.gbIndividualOp.TabIndex = 4;
            this.gbIndividualOp.TabStop = false;
            this.gbIndividualOp.Text = "Individual Operation";
            // 
            // tbPanel
            // 
            this.tbPanel.AutoScroll = true;
            this.tbPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tbPanel.ColumnCount = 1;
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbPanel.Location = new System.Drawing.Point(7, 21);
            this.tbPanel.Name = "tbPanel";
            this.tbPanel.RowCount = 1;
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbPanel.Size = new System.Drawing.Size(697, 267);
            this.tbPanel.TabIndex = 0;
            this.tbPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbPanel_MouseUp);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // DlgCvMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            //this.ClientSize = new System.Drawing.Size(944, 360);
            this.Size = new System.Drawing.Size(952, 394);
            this.ControlBox = false;
            this.Controls.Add(this.gbIndividualOp);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnExtension);
            this.Controls.Add(this.gbAllOp);
            this.Controls.Add(this.gbSpeed);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgCvMotor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "C/V Motor Manual Operation";
            this.Load += new System.EventHandler(this.DlgCvMotor_Load);
            this.gbSpeed.ResumeLayout(false);
            this.gbAllOp.ResumeLayout(false);
            this.gbIndividualOp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSpeed;
        private System.Windows.Forms.GroupBox gbAllOp;
        private System.Windows.Forms.Button btnExtension;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbIndividualOp;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.RadioButton btnAllFw;
        private System.Windows.Forms.RadioButton btnAllStop;
        private System.Windows.Forms.RadioButton btnAllBw;
        private System.Windows.Forms.TableLayoutPanel tbPanel;
    }
}
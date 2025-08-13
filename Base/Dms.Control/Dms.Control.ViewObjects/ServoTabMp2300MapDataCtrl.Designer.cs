namespace Dms.Control
{
    partial class ServoTabMp2300MapDataCtrl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridViewInput = new Dms.Control.DoubleBufferedGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gridViewOutput = new Dms.Control.DoubleBufferedGridView();
            this.cboInput = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboOutput = new System.Windows.Forms.ComboBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // gridViewInput
            // 
            this.gridViewInput.AllowUserToAddRows = false;
            this.gridViewInput.AllowUserToDeleteRows = false;
            this.gridViewInput.AllowUserToResizeColumns = false;
            this.gridViewInput.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewInput.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.gridViewInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewInput.Location = new System.Drawing.Point(3, 30);
            this.gridViewInput.Name = "gridViewInput";
            this.gridViewInput.ReadOnly = true;
            this.gridViewInput.RowHeadersVisible = false;
            this.gridViewInput.RowTemplate.Height = 23;
            this.gridViewInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridViewInput.Size = new System.Drawing.Size(405, 426);
            this.gridViewInput.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(405, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mapping Data Input";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(414, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mapping Data Output";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridViewOutput
            // 
            this.gridViewOutput.AllowUserToAddRows = false;
            this.gridViewOutput.AllowUserToDeleteRows = false;
            this.gridViewOutput.AllowUserToResizeColumns = false;
            this.gridViewOutput.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewOutput.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridViewOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewOutput.Location = new System.Drawing.Point(414, 30);
            this.gridViewOutput.Name = "gridViewOutput";
            this.gridViewOutput.ReadOnly = true;
            this.gridViewOutput.RowHeadersVisible = false;
            this.gridViewOutput.RowTemplate.Height = 23;
            this.gridViewOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridViewOutput.Size = new System.Drawing.Size(450, 426);
            this.gridViewOutput.TabIndex = 2;
            // 
            // cboInput
            // 
            this.cboInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInput.FormattingEnabled = true;
            this.cboInput.Location = new System.Drawing.Point(6, 490);
            this.cboInput.Name = "cboInput";
            this.cboInput.Size = new System.Drawing.Size(175, 23);
            this.cboInput.TabIndex = 4;
            this.cboInput.SelectionChangeCommitted += new System.EventHandler(this.cboInput_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 472);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Select Input Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(411, 472);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Select Output Address";
            // 
            // cboOutput
            // 
            this.cboOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOutput.FormattingEnabled = true;
            this.cboOutput.Location = new System.Drawing.Point(414, 490);
            this.cboOutput.Name = "cboOutput";
            this.cboOutput.Size = new System.Drawing.Size(175, 23);
            this.cboOutput.TabIndex = 6;
            this.cboOutput.SelectionChangeCommitted += new System.EventHandler(this.cboOutput_SelectionChangeCommitted);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // ServoTabMp2300MapDataCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboOutput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gridViewOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridViewInput);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServoTabMp2300MapDataCtrl";
            this.Size = new System.Drawing.Size(901, 537);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedGridView gridViewInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DoubleBufferedGridView gridViewOutput;
        private System.Windows.Forms.ComboBox cboInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboOutput;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}

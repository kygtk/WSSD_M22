namespace Dms.Util.IODefine
{
    partial class ViewBomAnalysis
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTotalAmount = new System.Windows.Forms.Label();
            this.labelTotalAmountTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new Dms.Control.DoubleBufferedGridView();
            this.ColDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPartCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPartSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPartPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(687, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = " BOM Analysis";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 26);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTotalAmount);
            this.panel2.Controls.Add(this.labelTotalAmountTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 377);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(687, 36);
            this.panel2.TabIndex = 13;
            // 
            // labelTotalAmount
            // 
            this.labelTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalAmount.BackColor = System.Drawing.Color.White;
            this.labelTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalAmount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalAmount.ForeColor = System.Drawing.Color.Red;
            this.labelTotalAmount.Location = new System.Drawing.Point(479, 9);
            this.labelTotalAmount.Name = "labelTotalAmount";
            this.labelTotalAmount.Size = new System.Drawing.Size(208, 23);
            this.labelTotalAmount.TabIndex = 13;
            this.labelTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelTotalAmount.Visible = false;
            // 
            // labelTotalAmountTitle
            // 
            this.labelTotalAmountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalAmountTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelTotalAmountTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalAmountTitle.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalAmountTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTotalAmountTitle.Location = new System.Drawing.Point(314, 9);
            this.labelTotalAmountTitle.Name = "labelTotalAmountTitle";
            this.labelTotalAmountTitle.Size = new System.Drawing.Size(163, 23);
            this.labelTotalAmountTitle.TabIndex = 12;
            this.labelTotalAmountTitle.Text = "Total Amount  ";
            this.labelTotalAmountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelTotalAmountTitle.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDevice,
            this.ColPartCode,
            this.ColPartName,
            this.ColPartSpec,
            this.ColCount,
            this.ColPartPrice,
            this.ColAmount});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 26);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 18;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(687, 351);
            this.dataGridView1.TabIndex = 1;
            // 
            // ColDevice
            // 
            this.ColDevice.HeaderText = "Device";
            this.ColDevice.Name = "ColDevice";
            this.ColDevice.ReadOnly = true;
            this.ColDevice.Width = 68;
            // 
            // ColPartCode
            // 
            this.ColPartCode.HeaderText = "자재번호";
            this.ColPartCode.Name = "ColPartCode";
            this.ColPartCode.ReadOnly = true;
            this.ColPartCode.Width = 78;
            // 
            // ColPartName
            // 
            this.ColPartName.HeaderText = "자재명";
            this.ColPartName.Name = "ColPartName";
            this.ColPartName.ReadOnly = true;
            this.ColPartName.Width = 66;
            // 
            // ColPartSpec
            // 
            this.ColPartSpec.HeaderText = "규격";
            this.ColPartSpec.Name = "ColPartSpec";
            this.ColPartSpec.ReadOnly = true;
            this.ColPartSpec.Width = 54;
            // 
            // ColCount
            // 
            this.ColCount.HeaderText = "수량";
            this.ColCount.Name = "ColCount";
            this.ColCount.ReadOnly = true;
            this.ColCount.Width = 54;
            // 
            // ColPartPrice
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColPartPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColPartPrice.HeaderText = "단가";
            this.ColPartPrice.Name = "ColPartPrice";
            this.ColPartPrice.ReadOnly = true;
            this.ColPartPrice.Width = 54;
            // 
            // ColAmount
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColAmount.HeaderText = "비용";
            this.ColAmount.Name = "ColAmount";
            this.ColAmount.ReadOnly = true;
            this.ColAmount.Width = 54;
            // 
            // ViewBomAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ViewBomAnalysis";
            this.Size = new System.Drawing.Size(687, 413);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Dms.Control.DoubleBufferedGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTotalAmount;
        private System.Windows.Forms.Label labelTotalAmountTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPartCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPartSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPartPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAmount;

    }
}

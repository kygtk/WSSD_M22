namespace Dms.Control
{
    partial class CvMotorGridOp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridOperation = new Dms.Control.DoubleBufferedGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridOperation)).BeginInit();
            this.SuspendLayout();
            // 
            // gridOperation
            // 
            this.gridOperation.AllowUserToAddRows = false;
            this.gridOperation.AllowUserToDeleteRows = false;
            this.gridOperation.AllowUserToResizeColumns = false;
            this.gridOperation.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridOperation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridOperation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOperation.Location = new System.Drawing.Point(0, 0);
            this.gridOperation.MultiSelect = false;
            this.gridOperation.Name = "gridOperation";
            this.gridOperation.RowHeadersVisible = false;
            this.gridOperation.RowTemplate.Height = 35;
            this.gridOperation.RowTemplate.ReadOnly = true;
            this.gridOperation.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridOperation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridOperation.Size = new System.Drawing.Size(690, 212);
            this.gridOperation.TabIndex = 0;
            this.gridOperation.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridOperation_CellClick);
            // 
            // CvMotorGridOp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.gridOperation);
            this.Name = "CvMotorGridOp";
            this.Size = new System.Drawing.Size(690, 212);
            this.Load += new System.EventHandler(this.CvMotorGridOp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridOperation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedGridView gridOperation;
    }
}

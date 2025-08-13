namespace Dms.Data
{
    partial class ViewCurrentAlarms
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
            this.dataGridViewCurrentAlarms = new Dms.Control.DoubleBufferedGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrentAlarms)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCurrentAlarms
            // 
            this.dataGridViewCurrentAlarms.AllowUserToAddRows = false;
            this.dataGridViewCurrentAlarms.AllowUserToDeleteRows = false;
            this.dataGridViewCurrentAlarms.AllowUserToOrderColumns = true;
            this.dataGridViewCurrentAlarms.AllowUserToResizeRows = false;
            this.dataGridViewCurrentAlarms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCurrentAlarms.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dataGridViewCurrentAlarms.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewCurrentAlarms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCurrentAlarms.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCurrentAlarms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurrentAlarms.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCurrentAlarms.Name = "dataGridViewCurrentAlarms";
            this.dataGridViewCurrentAlarms.ReadOnly = true;
            this.dataGridViewCurrentAlarms.RowHeadersVisible = false;
            this.dataGridViewCurrentAlarms.RowTemplate.Height = 23;
            this.dataGridViewCurrentAlarms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCurrentAlarms.Size = new System.Drawing.Size(250, 167);
            this.dataGridViewCurrentAlarms.TabIndex = 5;
            this.dataGridViewCurrentAlarms.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewCurrentAlarms_DataError);
            // 
            // ViewCurrentAlarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dataGridViewCurrentAlarms);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewCurrentAlarms";
            this.Size = new System.Drawing.Size(250, 167);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrentAlarms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCurrentAlarms;

    }
}

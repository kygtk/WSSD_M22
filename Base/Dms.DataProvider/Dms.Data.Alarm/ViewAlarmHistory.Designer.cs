namespace Dms.Data
{
    partial class ViewAlarmHistory
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
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewAlarmHistory = new Dms.Control.DoubleBufferedGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlarmHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Alarm History";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewAlarmHistory
            // 
            this.dataGridViewAlarmHistory.AllowUserToAddRows = false;
            this.dataGridViewAlarmHistory.AllowUserToDeleteRows = false;
            this.dataGridViewAlarmHistory.AllowUserToResizeColumns = false;
            this.dataGridViewAlarmHistory.AllowUserToResizeRows = false;
            this.dataGridViewAlarmHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAlarmHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAlarmHistory.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewAlarmHistory.Location = new System.Drawing.Point(3, 29);
            this.dataGridViewAlarmHistory.Name = "dataGridViewAlarmHistory";
            this.dataGridViewAlarmHistory.ReadOnly = true;
            this.dataGridViewAlarmHistory.RowHeadersVisible = false;
            this.dataGridViewAlarmHistory.RowTemplate.Height = 23;
            this.dataGridViewAlarmHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAlarmHistory.Size = new System.Drawing.Size(287, 164);
            this.dataGridViewAlarmHistory.TabIndex = 5;
            this.dataGridViewAlarmHistory.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewAlarmHistory_UserDeletingRow);
            this.dataGridViewAlarmHistory.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewAlarmHistory_DataBindingComplete);
            this.dataGridViewAlarmHistory.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewAlarmHistory_UserDeletedRow);
            this.dataGridViewAlarmHistory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewAlarmHistory_DataError);
            // 
            // ViewAlarmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAlarmHistory);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewAlarmHistory";
            this.Size = new System.Drawing.Size(294, 197);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlarmHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewAlarmHistory;
    }
}

namespace Dms.Common
{
    partial class FormInitStatus
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
            this.dataGridViewInitItem = new System.Windows.Forms.DataGridView();
            this.ColumnItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerUpdateViewer = new System.Windows.Forms.Timer(this.components);
            this.buttonShow = new System.Windows.Forms.Button();
            this.timerFormCloser = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInitItem)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewInitItem
            // 
            this.dataGridViewInitItem.AllowUserToAddRows = false;
            this.dataGridViewInitItem.AllowUserToDeleteRows = false;
            this.dataGridViewInitItem.AllowUserToResizeRows = false;
            this.dataGridViewInitItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewInitItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInitItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnItem,
            this.ColumnStatus});
            this.dataGridViewInitItem.Location = new System.Drawing.Point(2, 41);
            this.dataGridViewInitItem.Name = "dataGridViewInitItem";
            this.dataGridViewInitItem.ReadOnly = true;
            this.dataGridViewInitItem.RowHeadersVisible = false;
            this.dataGridViewInitItem.RowTemplate.Height = 23;
            this.dataGridViewInitItem.Size = new System.Drawing.Size(300, 395);
            this.dataGridViewInitItem.TabIndex = 0;
            this.dataGridViewInitItem.SelectionChanged += new System.EventHandler(this.dataGridViewInitItem_SelectionChanged);
            // 
            // ColumnItem
            // 
            this.ColumnItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnItem.HeaderText = "Item";
            this.ColumnItem.Name = "ColumnItem";
            this.ColumnItem.ReadOnly = true;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Width = 65;
            // 
            // timerUpdateViewer
            // 
            this.timerUpdateViewer.Enabled = true;
            this.timerUpdateViewer.Interval = 300;
            this.timerUpdateViewer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonShow
            // 
            this.buttonShow.BackColor = System.Drawing.Color.Gold;
            this.buttonShow.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShow.ForeColor = System.Drawing.Color.Black;
            this.buttonShow.Location = new System.Drawing.Point(0, 0);
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(303, 35);
            this.buttonShow.TabIndex = 1;
            this.buttonShow.Text = "Hide";
            this.buttonShow.UseVisualStyleBackColor = false;
            this.buttonShow.Click += new System.EventHandler(this.buttonShow_Click);
            // 
            // timerFormCloser
            // 
            this.timerFormCloser.Interval = 3000;
            this.timerFormCloser.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // FormInitStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(303, 439);
            this.ControlBox = false;
            this.Controls.Add(this.buttonShow);
            this.Controls.Add(this.dataGridViewInitItem);
            this.Name = "FormInitStatus";
            this.Opacity = 0.9;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Initialize equipment...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormInitStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInitItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewInitItem;
        private System.Windows.Forms.Timer timerUpdateViewer;
        private System.Windows.Forms.Button buttonShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.Timer timerFormCloser;
    }
}
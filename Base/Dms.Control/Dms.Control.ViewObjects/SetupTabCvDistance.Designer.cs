namespace Dms.Control
{
    partial class SetupTabCvDistance
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
            this.viewSetupCvTimeout = new Dms.Data.ViewSetupInfo();
            this.viewSetupCvDistance = new Dms.Data.ViewSetupInfo();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewSetupCvTimeout
            // 
            this.viewSetupCvTimeout.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupCvTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupCvTimeout.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupCvTimeout.Location = new System.Drawing.Point(476, 3);
            this.viewSetupCvTimeout.Name = "viewSetupCvTimeout";
            this.viewSetupCvTimeout.Size = new System.Drawing.Size(381, 489);
            this.viewSetupCvTimeout.TabIndex = 1;
            this.viewSetupCvTimeout.TitleName = "Conveyor Timeout";
            // 
            // viewSetupCvDistance
            // 
            this.viewSetupCvDistance.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupCvDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupCvDistance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupCvDistance.Location = new System.Drawing.Point(3, 3);
            this.viewSetupCvDistance.Name = "viewSetupCvDistance";
            this.viewSetupCvDistance.Size = new System.Drawing.Size(467, 489);
            this.viewSetupCvDistance.TabIndex = 0;
            this.viewSetupCvDistance.TitleName = "Conveyor Distance";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Controls.Add(this.viewSetupCvTimeout, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewSetupCvDistance, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(860, 495);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // SetupTabCvDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetupTabCvDistance";
            this.Size = new System.Drawing.Size(903, 539);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewSetupInfo viewSetupCvDistance;
        private Dms.Data.ViewSetupInfo viewSetupCvTimeout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

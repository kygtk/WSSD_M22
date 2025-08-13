namespace Dms.Control
{
    partial class SetupTabHsms
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
            this.viewSetupHsms = new Dms.Data.ViewSetupInfo();
            this.viewSetupHsmsEqp = new Dms.Data.ViewSetupInfo();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewSetupHsms
            // 
            this.viewSetupHsms.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupHsms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupHsms.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupHsms.Location = new System.Drawing.Point(3, 3);
            this.viewSetupHsms.Name = "viewSetupHsms";
            this.viewSetupHsms.Size = new System.Drawing.Size(484, 489);
            this.viewSetupHsms.TabIndex = 2;
            this.viewSetupHsms.TitleName = "HSMS Client Parameters";
            // 
            // viewSetupHsmsEqp
            // 
            this.viewSetupHsmsEqp.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupHsmsEqp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupHsmsEqp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupHsmsEqp.Location = new System.Drawing.Point(493, 3);
            this.viewSetupHsmsEqp.Name = "viewSetupHsmsEqp";
            this.viewSetupHsmsEqp.Size = new System.Drawing.Size(365, 489);
            this.viewSetupHsmsEqp.TabIndex = 2;
            this.viewSetupHsmsEqp.TitleName = "EQP Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tableLayoutPanel1.Controls.Add(this.viewSetupHsms, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewSetupHsmsEqp, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(861, 495);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // SetupTabHsms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetupTabHsms";
            this.Size = new System.Drawing.Size(903, 539);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewSetupInfo viewSetupHsms;
        private Dms.Data.ViewSetupInfo viewSetupHsmsEqp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

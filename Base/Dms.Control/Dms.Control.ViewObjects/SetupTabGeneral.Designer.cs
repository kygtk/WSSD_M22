namespace Dms.Control
{
    partial class SetupTabGeneral
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
            this.viewSetupGeneral = new Dms.Data.ViewSetupInfo();
            this.viewSetupIdleRun = new Dms.Data.ViewSetupInfo();
            this.viewSetupTankLevel = new Dms.Data.ViewSetupInfo();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewSetupGeneral
            // 
            this.viewSetupGeneral.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupGeneral.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupGeneral.Location = new System.Drawing.Point(3, 3);
            this.viewSetupGeneral.Name = "viewSetupGeneral";
            this.tableLayoutPanel1.SetRowSpan(this.viewSetupGeneral, 2);
            this.viewSetupGeneral.Size = new System.Drawing.Size(493, 489);
            this.viewSetupGeneral.TabIndex = 0;
            this.viewSetupGeneral.TitleName = "General Setup Information";
            // 
            // viewSetupIdleRun
            // 
            this.viewSetupIdleRun.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupIdleRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupIdleRun.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupIdleRun.Location = new System.Drawing.Point(502, 3);
            this.viewSetupIdleRun.Name = "viewSetupIdleRun";
            this.viewSetupIdleRun.Size = new System.Drawing.Size(356, 241);
            this.viewSetupIdleRun.TabIndex = 1;
            this.viewSetupIdleRun.TitleName = "Idle Running";
            // 
            // viewSetupTankLevel
            // 
            this.viewSetupTankLevel.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupTankLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSetupTankLevel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupTankLevel.Location = new System.Drawing.Point(502, 250);
            this.viewSetupTankLevel.Name = "viewSetupTankLevel";
            this.viewSetupTankLevel.Size = new System.Drawing.Size(356, 242);
            this.viewSetupTankLevel.TabIndex = 2;
            this.viewSetupTankLevel.TitleName = "Tank Level";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tableLayoutPanel1.Controls.Add(this.viewSetupGeneral, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewSetupTankLevel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.viewSetupIdleRun, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(861, 495);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // SetupTabGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetupTabGeneral";
            this.Size = new System.Drawing.Size(903, 539);
            this.Load += new System.EventHandler(this.SetupTabGeneral_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewSetupInfo viewSetupGeneral;
        private Dms.Data.ViewSetupInfo viewSetupIdleRun;
        private Dms.Data.ViewSetupInfo viewSetupTankLevel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

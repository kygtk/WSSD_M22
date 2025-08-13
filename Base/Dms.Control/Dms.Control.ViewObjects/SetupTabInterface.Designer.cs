namespace Dms.Control
{
    partial class SetupTabInterface
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
            this.viewSetupInterface = new Dms.Data.ViewSetupInfo();
            this.SuspendLayout();
            // 
            // viewSetupInterface
            // 
            this.viewSetupInterface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupInterface.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupInterface.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupInterface.Location = new System.Drawing.Point(21, 21);
            this.viewSetupInterface.Name = "viewSetupInterface";
            this.viewSetupInterface.Size = new System.Drawing.Size(439, 495);
            this.viewSetupInterface.TabIndex = 0;
            this.viewSetupInterface.TitleName = "Interface Parameters";
            // 
            // SetupTabInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewSetupInterface);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetupTabInterface";
            this.Size = new System.Drawing.Size(903, 539);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewSetupInfo viewSetupInterface;
    }
}

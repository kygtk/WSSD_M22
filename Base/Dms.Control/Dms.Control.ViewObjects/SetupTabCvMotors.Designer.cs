namespace Dms.Control
{
    partial class SetupTabCvMotors
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
            this.viewSetupCvMotos = new Dms.Data.ViewSetupCvInfo();
            this.SuspendLayout();
            // 
            // viewSetupCvMotos
            // 
            this.viewSetupCvMotos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSetupCvMotos.BackColor = System.Drawing.Color.Transparent;
            this.viewSetupCvMotos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSetupCvMotos.Location = new System.Drawing.Point(21, 21);
            this.viewSetupCvMotos.Name = "viewSetupCvMotos";
            this.viewSetupCvMotos.Size = new System.Drawing.Size(680, 495);
            this.viewSetupCvMotos.TabIndex = 0;
            this.viewSetupCvMotos.TitleName = "Conveyor Motor Information";
            // 
            // SetupTabCvMotors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewSetupCvMotos);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetupTabCvMotors";
            this.Size = new System.Drawing.Size(903, 539);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewSetupCvInfo viewSetupCvMotos;
    }
}

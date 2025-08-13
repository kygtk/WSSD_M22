namespace Dms.Control
{
    partial class SystemTabIOCtrl
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
            this.viewIOEdit1 = new Dms.Util.IODefine.ViewIOEdit();
            this.SuspendLayout();
            // 
            // viewIOEdit1
            // 
            this.viewIOEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewIOEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewIOEdit1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOEdit1.Location = new System.Drawing.Point(9, 4);
            this.viewIOEdit1.Name = "viewIOEdit1";
            this.viewIOEdit1.OperateMode = Dms.Util.IODefine.ViewIOEdit.OpMode.Config;
            this.viewIOEdit1.Size = new System.Drawing.Size(886, 530);
            this.viewIOEdit1.TabIndex = 0;
            this.viewIOEdit1.TimerStateUpdateEnabled = false;
            // 
            // SystemTabIOCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewIOEdit1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SystemTabIOCtrl";
            this.Size = new System.Drawing.Size(903, 539);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Util.IODefine.ViewIOEdit viewIOEdit1;
    }
}

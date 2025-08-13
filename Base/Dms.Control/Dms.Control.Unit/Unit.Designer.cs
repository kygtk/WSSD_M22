namespace Dms.Control
{
    partial class Unit
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
            this.lblUnit = new System.Windows.Forms.Label();
            this.tmrUpdateAnimation = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblUnit
            // 
            this.lblUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUnit.Location = new System.Drawing.Point(0, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(96, 32);
            this.lblUnit.TabIndex = 0;
            this.lblUnit.Text = "__ UNIT";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdateAnimation
            // 
            this.tmrUpdateAnimation.Tick += new System.EventHandler(this.tmrUpdateAnimation_Tick);
            // 
            // Unit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblUnit);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Unit";
            this.Size = new System.Drawing.Size(96, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Timer tmrUpdateAnimation;
    }
}

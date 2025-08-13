namespace Dms.Control
{
    partial class Apc
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
            this.lblBackColor = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBackColor
            // 
            this.lblBackColor.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackColor.Location = new System.Drawing.Point(0, 0);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(70, 23);
            this.lblBackColor.TabIndex = 0;
            // 
            // lblState
            // 
            this.lblState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblState.BackColor = System.Drawing.Color.Azure;
            this.lblState.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(3, 3);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(64, 17);
            this.lblState.TabIndex = 1;
            this.lblState.Text = "APC";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblState.Click += new System.EventHandler(this.ApcClick);
            // 
            // Apc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblBackColor);
            this.Name = "Apc";
            this.Size = new System.Drawing.Size(70, 23);
            this.Click += new System.EventHandler(this.ApcClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Label lblState;

    }
}

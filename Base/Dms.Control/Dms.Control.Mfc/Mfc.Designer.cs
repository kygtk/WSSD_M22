namespace Dms.Control
{
    partial class Mfc
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
            this.lblCurFlow = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblMfc = new System.Windows.Forms.Label();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCurFlow
            // 
            this.lblCurFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurFlow.BackColor = System.Drawing.Color.White;
            this.lblCurFlow.Location = new System.Drawing.Point(3, 3);
            this.lblCurFlow.Name = "lblCurFlow";
            this.lblCurFlow.Size = new System.Drawing.Size(62, 14);
            this.lblCurFlow.TabIndex = 0;
            this.lblCurFlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblName.Location = new System.Drawing.Point(3, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(62, 14);
            this.lblName.TabIndex = 1;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblName.Click += new System.EventHandler(this.Mfc_Click);
            // 
            // lblMfc
            // 
            this.lblMfc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMfc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblMfc.Location = new System.Drawing.Point(3, 32);
            this.lblMfc.Name = "lblMfc";
            this.lblMfc.Size = new System.Drawing.Size(62, 14);
            this.lblMfc.TabIndex = 1;
            this.lblMfc.Text = "MFC";
            this.lblMfc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblMfc.Click += new System.EventHandler(this.Mfc_Click);
            // 
            // lblBackColor
            // 
            this.lblBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackColor.Location = new System.Drawing.Point(0, 0);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(68, 49);
            this.lblBackColor.TabIndex = 2;
            this.lblBackColor.Text = "label1";
            // 
            // Mfc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblMfc);
            this.Controls.Add(this.lblCurFlow);
            this.Controls.Add(this.lblBackColor);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Mfc";
            this.Size = new System.Drawing.Size(68, 49);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCurFlow;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblMfc;
        private System.Windows.Forms.Label lblBackColor;

    }
}

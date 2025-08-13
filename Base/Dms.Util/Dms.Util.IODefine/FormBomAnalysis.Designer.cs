namespace Dms.Util.IODefine
{
    partial class FormBomAnalysis
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.viewBomAnalysis1 = new Dms.Util.IODefine.ViewBomAnalysis();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(806, 21);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(66, 430);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // viewBomAnalysis1
            // 
            this.viewBomAnalysis1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewBomAnalysis1.BackColor = System.Drawing.Color.Transparent;
            this.viewBomAnalysis1.Location = new System.Drawing.Point(12, 21);
            this.viewBomAnalysis1.Name = "viewBomAnalysis1";
            this.viewBomAnalysis1.Size = new System.Drawing.Size(785, 465);
            this.viewBomAnalysis1.TabIndex = 0;
            // 
            // FormBomAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(881, 498);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.viewBomAnalysis1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormBomAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " BOM";
            this.Load += new System.EventHandler(this.FormBomAnalysis_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewBomAnalysis viewBomAnalysis1;
        private System.Windows.Forms.Button buttonClose;
    }
}
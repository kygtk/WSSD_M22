namespace Dms.Control
{
    partial class ViewPortUnit
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
            this.labelPortName = new System.Windows.Forms.Label();
            this.labelCstId = new System.Windows.Forms.Label();
            this.panelCstFrame = new System.Windows.Forms.Panel();
            this.labelPortStatus = new System.Windows.Forms.Label();
            this.panelCst = new System.Windows.Forms.Panel();
            this.panelCstFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPortName
            // 
            this.labelPortName.BackColor = System.Drawing.SystemColors.Highlight;
            this.labelPortName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelPortName.Location = new System.Drawing.Point(0, 0);
            this.labelPortName.Name = "labelPortName";
            this.labelPortName.Size = new System.Drawing.Size(158, 20);
            this.labelPortName.TabIndex = 12;
            this.labelPortName.Text = "PORT #1";
            this.labelPortName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPortName.Click += new System.EventHandler(this.labelPortName_Click);
            // 
            // labelCstId
            // 
            this.labelCstId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCstId.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.labelCstId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCstId.ForeColor = System.Drawing.Color.Black;
            this.labelCstId.Location = new System.Drawing.Point(3, 22);
            this.labelCstId.Name = "labelCstId";
            this.labelCstId.Size = new System.Drawing.Size(142, 17);
            this.labelCstId.TabIndex = 15;
            this.labelCstId.Text = "Cassette Id";
            this.labelCstId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCstId.Click += new System.EventHandler(this.labelCstId_Click);
            // 
            // panelCstFrame
            // 
            this.panelCstFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCstFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCstFrame.Controls.Add(this.labelPortStatus);
            this.panelCstFrame.Controls.Add(this.labelCstId);
            this.panelCstFrame.Controls.Add(this.panelCst);
            this.panelCstFrame.Location = new System.Drawing.Point(3, 23);
            this.panelCstFrame.Name = "panelCstFrame";
            this.panelCstFrame.Size = new System.Drawing.Size(150, 299);
            this.panelCstFrame.TabIndex = 14;
            // 
            // labelPortStatus
            // 
            this.labelPortStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPortStatus.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelPortStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortStatus.ForeColor = System.Drawing.Color.Black;
            this.labelPortStatus.Location = new System.Drawing.Point(3, 3);
            this.labelPortStatus.Name = "labelPortStatus";
            this.labelPortStatus.Size = new System.Drawing.Size(142, 17);
            this.labelPortStatus.TabIndex = 60;
            this.labelPortStatus.Text = "Port Status";
            this.labelPortStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCst
            // 
            this.panelCst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCst.Location = new System.Drawing.Point(6, 44);
            this.panelCst.Name = "panelCst";
            this.panelCst.Padding = new System.Windows.Forms.Padding(0, 3, 10, 3);
            this.panelCst.Size = new System.Drawing.Size(138, 249);
            this.panelCst.TabIndex = 59;
            // 
            // ViewPortUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelCstFrame);
            this.Controls.Add(this.labelPortName);
            this.Name = "ViewPortUnit";
            this.Size = new System.Drawing.Size(158, 327);
            this.Load += new System.EventHandler(this.ViewPortUnit_Load);
            this.panelCstFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPortName;
        private System.Windows.Forms.Label labelCstId;
        private System.Windows.Forms.Panel panelCstFrame;
        private System.Windows.Forms.Panel panelCst;
        private System.Windows.Forms.Label labelPortStatus;
    }
}

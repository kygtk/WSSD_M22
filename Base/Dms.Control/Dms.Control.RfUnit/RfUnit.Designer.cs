namespace Dms.Control
{
    partial class RfUnit
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
            this.lblMatchBox = new System.Windows.Forms.Label();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMatchBox
            // 
            this.lblMatchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMatchBox.BackColor = System.Drawing.Color.Honeydew;
            this.lblMatchBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMatchBox.Location = new System.Drawing.Point(3, 3);
            this.lblMatchBox.Name = "lblMatchBox";
            this.lblMatchBox.Size = new System.Drawing.Size(84, 29);
            this.lblMatchBox.TabIndex = 3;
            this.lblMatchBox.Text = "RF Unit";
            this.lblMatchBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMatchBox.Click += new System.EventHandler(this.lblMatchBox_Click);
            // 
            // lblBackColor
            // 
            this.lblBackColor.BackColor = System.Drawing.Color.Thistle;
            this.lblBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackColor.Location = new System.Drawing.Point(0, 0);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(90, 35);
            this.lblBackColor.TabIndex = 2;
            // 
            // RfUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblMatchBox);
            this.Controls.Add(this.lblBackColor);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RfUnit";
            this.Size = new System.Drawing.Size(90, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMatchBox;
        private System.Windows.Forms.Label lblBackColor;

    }
}

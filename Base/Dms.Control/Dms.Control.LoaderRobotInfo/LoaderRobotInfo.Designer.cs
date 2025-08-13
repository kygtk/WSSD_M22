namespace LoaderRobotInfo
{
    partial class LoaderRobotInfo
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
            if(disposing && (components != null))
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
            this.lblText1 = new System.Windows.Forms.Label();
            this.lblValue1 = new System.Windows.Forms.Label();
            this.lblText2 = new System.Windows.Forms.Label();
            this.lblValue2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblText1
            // 
            this.lblText1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblText1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblText1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblText1.Location = new System.Drawing.Point(0, 0);
            this.lblText1.Name = "lblText1";
            this.lblText1.Size = new System.Drawing.Size(82, 26);
            this.lblText1.TabIndex = 0;
            this.lblText1.Text = "Upper Hand";
            this.lblText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue1
            // 
            this.lblValue1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblValue1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValue1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblValue1.Location = new System.Drawing.Point(82, 0);
            this.lblValue1.Name = "lblValue1";
            this.lblValue1.Size = new System.Drawing.Size(56, 26);
            this.lblValue1.TabIndex = 0;
            this.lblValue1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblText2
            // 
            this.lblText2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblText2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblText2.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblText2.Location = new System.Drawing.Point(196, 0);
            this.lblText2.Name = "lblText2";
            this.lblText2.Size = new System.Drawing.Size(82, 26);
            this.lblText2.TabIndex = 0;
            this.lblText2.Text = "Lower Hand";
            this.lblText2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue2
            // 
            this.lblValue2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lblValue2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValue2.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblValue2.Location = new System.Drawing.Point(140, 0);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(56, 26);
            this.lblValue2.TabIndex = 0;
            this.lblValue2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoaderRobotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblValue2);
            this.Controls.Add(this.lblText2);
            this.Controls.Add(this.lblValue1);
            this.Controls.Add(this.lblText1);
            this.Name = "LoaderRobotInfo";
            this.Size = new System.Drawing.Size(278, 26);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblText1;
        private System.Windows.Forms.Label lblValue1;
        private System.Windows.Forms.Label lblText2;
        private System.Windows.Forms.Label lblValue2;
    }
}

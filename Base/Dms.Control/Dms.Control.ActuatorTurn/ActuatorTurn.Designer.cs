namespace Dms.Control
{
    partial class ActuatorTurn
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 20);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Turn";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblValue
            // 
            this.lblValue.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.Location = new System.Drawing.Point(0, 20);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(99, 24);
            this.lblValue.TabIndex = 3;
            this.lblValue.Text = "Value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblValue.Click += new System.EventHandler(this.lblValue_Click);
            // 
            // ActuatorTurn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblTitle);
            this.Name = "ActuatorTurn";
            this.Size = new System.Drawing.Size(99, 44);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblValue;
    }
}

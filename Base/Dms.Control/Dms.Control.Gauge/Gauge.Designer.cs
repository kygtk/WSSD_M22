namespace Dms.Control
{
    partial class Gauge
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
			this.lblValue = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblValue
			// 
			this.lblValue.BackColor = System.Drawing.Color.White;
			this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblValue.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblValue.Location = new System.Drawing.Point(0, 0);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(78, 22);
			this.lblValue.TabIndex = 0;
			this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblValue.Click += new System.EventHandler(this.lblValue_Click);
			// 
			// Gauge
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.GrayText;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblValue);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "Gauge";
			this.Size = new System.Drawing.Size(78, 22);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblValue;
    }
}

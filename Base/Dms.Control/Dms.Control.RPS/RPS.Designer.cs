namespace Dms.Control
{
    partial class RPS
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
            this.lblState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblState
            // 
            this.lblState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblState.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(3, 3);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(84, 29);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "RPS";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblState.Click += new System.EventHandler(this.RPS_Click);
            // 
            // RPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.lblState);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RPS";
            this.Size = new System.Drawing.Size(90, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblState;
    }
}

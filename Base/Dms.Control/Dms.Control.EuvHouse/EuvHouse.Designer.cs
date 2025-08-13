namespace Dms.Control
{
    partial class EuvHouse
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
            this.pbImageHouse = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageHouse)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImageHouse
            // 
            this.pbImageHouse.Image = global::Dms.Control.Properties.Resources.HouseNoUse;
            this.pbImageHouse.InitialImage = null;
            this.pbImageHouse.Location = new System.Drawing.Point(0, 0);
            this.pbImageHouse.Name = "pbImageHouse";
            this.pbImageHouse.Size = new System.Drawing.Size(80, 35);
            this.pbImageHouse.TabIndex = 0;
            this.pbImageHouse.TabStop = false;
            this.pbImageHouse.Click += new System.EventHandler(this.pbImageHouse_Click);
            // 
            // EuvHouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pbImageHouse);
            this.Name = "EuvHouse";
            this.Size = new System.Drawing.Size(82, 37);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageHouse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImageHouse;
    }
}

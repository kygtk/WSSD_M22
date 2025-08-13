namespace Dms.Control
{
    partial class DBMotor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBMotor));
            this.axDiskBrush1 = new AxDISKBRUSHLib.AxDiskBrush();
            this.tmrUpdateAnimation = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axDiskBrush1)).BeginInit();
            this.SuspendLayout();
            // 
            // axDiskBrush1
            // 
            this.axDiskBrush1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axDiskBrush1.Enabled = true;
            this.axDiskBrush1.Location = new System.Drawing.Point(0, 0);
            this.axDiskBrush1.Margin = new System.Windows.Forms.Padding(41, 24, 41, 24);
            this.axDiskBrush1.Name = "axDiskBrush1";
            this.axDiskBrush1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDiskBrush1.OcxState")));
            this.axDiskBrush1.Size = new System.Drawing.Size(25, 26);
            this.axDiskBrush1.TabIndex = 0;
            this.axDiskBrush1.ClickEvent += new System.EventHandler(this.DBMotor_Click);
            // 
            // DBMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.axDiskBrush1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(41, 24, 41, 24);
            this.Name = "DBMotor";
            this.Size = new System.Drawing.Size(25, 26);
            ((System.ComponentModel.ISupportInitialize)(this.axDiskBrush1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDISKBRUSHLib.AxDiskBrush axDiskBrush1;
        private System.Windows.Forms.Timer tmrUpdateAnimation;
    }
}

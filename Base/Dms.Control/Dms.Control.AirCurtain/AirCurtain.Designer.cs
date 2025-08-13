namespace Dms.Control
{
    partial class AirCurtain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AirCurtain));
            this.tmrUpdateAnimation = new System.Windows.Forms.Timer(this.components);
            this.axAirCurtain1 = new AxAIRCURTAINLib.AxAirCurtain();
            ((System.ComponentModel.ISupportInitialize)(this.axAirCurtain1)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrUpdateAnimation
            // 
            this.tmrUpdateAnimation.Tick += new System.EventHandler(this.tmrUpdateAnimation_Tick);
            // 
            // axAirCurtain1
            // 
            this.axAirCurtain1.Enabled = true;
            this.axAirCurtain1.Location = new System.Drawing.Point(0, 0);
            this.axAirCurtain1.Name = "axAirCurtain1";
            this.axAirCurtain1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAirCurtain1.OcxState")));
            this.axAirCurtain1.Size = new System.Drawing.Size(32, 31);
            this.axAirCurtain1.TabIndex = 0;
            // 
            // AirCurtain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.axAirCurtain1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AirCurtain";
            this.Size = new System.Drawing.Size(32, 31);
            ((System.ComponentModel.ISupportInitialize)(this.axAirCurtain1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdateAnimation;
        private AxAIRCURTAINLib.AxAirCurtain axAirCurtain1;
    }
}

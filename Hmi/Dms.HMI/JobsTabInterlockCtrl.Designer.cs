namespace Dms.HMI
{
    partial class JobsTabInterlockCtrl
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
            this.interlockListView = new Dms.HMI.InterlockListView();
            this.SuspendLayout();
            // 
            // interlockListView
            // 
            this.interlockListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.interlockListView.AutoSize = true;
            this.interlockListView.BackColor = System.Drawing.Color.Transparent;
            this.interlockListView.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.interlockListView.Location = new System.Drawing.Point(0, 16);
            this.interlockListView.Name = "interlockListView";
            this.interlockListView.Size = new System.Drawing.Size(709, 421);
            this.interlockListView.TabIndex = 0;
            // 
            // JobsTabInterlockCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.interlockListView);
            this.Name = "JobsTabInterlockCtrl";
            this.Size = new System.Drawing.Size(741, 497);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InterlockListView interlockListView;


    }
}

namespace Dms.Control
{
    partial class ViewCstSlot
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
            this.labelSlotState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSlotState
            // 
            this.labelSlotState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSlotState.BackColor = System.Drawing.Color.Silver;
            this.labelSlotState.Font = new System.Drawing.Font("굴림", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSlotState.Location = new System.Drawing.Point(21, 1);
            this.labelSlotState.Name = "labelSlotState";
            this.labelSlotState.Size = new System.Drawing.Size(70, 5);
            this.labelSlotState.TabIndex = 1;
            this.labelSlotState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewCstSlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.labelSlotState);
            this.Name = "ViewCstSlot";
            this.Size = new System.Drawing.Size(96, 7);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSlotState;
    }
}

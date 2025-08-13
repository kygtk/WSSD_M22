namespace Dms.Control
{
    partial class PmChamberPressureState
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
            this.lblSensor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSensor
            // 
            this.lblSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSensor.BackColor = System.Drawing.Color.White;
            this.lblSensor.Location = new System.Drawing.Point(3, 3);
            this.lblSensor.Name = "lblSensor";
            this.lblSensor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblSensor.Size = new System.Drawing.Size(9, 9);
            this.lblSensor.TabIndex = 0;
            this.lblSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PmChamberPressureState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.lblSensor);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PmChamberPressureState";
            this.Size = new System.Drawing.Size(15, 15);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSensor;
    }
}

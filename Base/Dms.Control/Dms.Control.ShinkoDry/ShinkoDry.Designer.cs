namespace Dms.Control
{
    partial class ShinkoDry
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
            this.labelName = new System.Windows.Forms.Label();
            this.labelRunStatus = new System.Windows.Forms.Label();
            this.labelAlarm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelName.BackColor = System.Drawing.Color.White;
            this.labelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(-2, -1);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(81, 17);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Device Name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRunStatus
            // 
            this.labelRunStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelRunStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRunStatus.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRunStatus.Location = new System.Drawing.Point(-2, 16);
            this.labelRunStatus.Name = "labelRunStatus";
            this.labelRunStatus.Size = new System.Drawing.Size(81, 17);
            this.labelRunStatus.TabIndex = 1;
            this.labelRunStatus.Text = "Run Status";
            this.labelRunStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAlarm
            // 
            this.labelAlarm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAlarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelAlarm.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarm.Location = new System.Drawing.Point(-2, 33);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(81, 17);
            this.labelAlarm.TabIndex = 2;
            this.labelAlarm.Text = "Device Alarm";
            this.labelAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShinkoDry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelAlarm);
            this.Controls.Add(this.labelRunStatus);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "ShinkoDry";
            this.Size = new System.Drawing.Size(79, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelRunStatus;
        private System.Windows.Forms.Label labelAlarm;
    }
}

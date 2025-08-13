namespace Dms.Control
{
    partial class MainStatusType2
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
            this.lblProcessStatus = new System.Windows.Forms.Label();
            this.lblEqpStatus = new System.Windows.Forms.Label();
            this.lblControlMode = new System.Windows.Forms.Label();
            this.lblHostConnection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProcessStatus
            // 
            this.lblProcessStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblProcessStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProcessStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblProcessStatus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessStatus.ForeColor = System.Drawing.Color.Black;
            this.lblProcessStatus.Location = new System.Drawing.Point(1, 38);
            this.lblProcessStatus.Name = "lblProcessStatus";
            this.lblProcessStatus.Size = new System.Drawing.Size(115, 20);
            this.lblProcessStatus.TabIndex = 18;
            this.lblProcessStatus.Text = "PROCESS";
            this.lblProcessStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEqpStatus
            // 
            this.lblEqpStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEqpStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEqpStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblEqpStatus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEqpStatus.ForeColor = System.Drawing.Color.Red;
            this.lblEqpStatus.Location = new System.Drawing.Point(1, 19);
            this.lblEqpStatus.Name = "lblEqpStatus";
            this.lblEqpStatus.Size = new System.Drawing.Size(115, 20);
            this.lblEqpStatus.TabIndex = 19;
            this.lblEqpStatus.Text = "IDLE";
            this.lblEqpStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblControlMode
            // 
            this.lblControlMode.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblControlMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblControlMode.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblControlMode.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblControlMode.ForeColor = System.Drawing.Color.Blue;
            this.lblControlMode.Location = new System.Drawing.Point(1, 0);
            this.lblControlMode.Name = "lblControlMode";
            this.lblControlMode.Size = new System.Drawing.Size(115, 20);
            this.lblControlMode.TabIndex = 16;
            this.lblControlMode.Text = "AUTO";
            this.lblControlMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHostConnection
            // 
            this.lblHostConnection.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblHostConnection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHostConnection.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblHostConnection.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHostConnection.ForeColor = System.Drawing.Color.Red;
            this.lblHostConnection.Location = new System.Drawing.Point(121, 0);
            this.lblHostConnection.Name = "lblHostConnection";
            this.lblHostConnection.Size = new System.Drawing.Size(135, 58);
            this.lblHostConnection.TabIndex = 17;
            this.lblHostConnection.Text = "OFFLINE";
            this.lblHostConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainStatusType1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblProcessStatus);
            this.Controls.Add(this.lblEqpStatus);
            this.Controls.Add(this.lblControlMode);
            this.Controls.Add(this.lblHostConnection);
            this.Name = "MainStatusType1";
            this.Size = new System.Drawing.Size(257, 59);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProcessStatus;
        private System.Windows.Forms.Label lblEqpStatus;
        private System.Windows.Forms.Label lblControlMode;
        private System.Windows.Forms.Label lblHostConnection;
    }
}

namespace Dms.Control
{
    partial class CimPortState
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
            this.lblPortName = new System.Windows.Forms.Label();
            this.lblPortStatus = new System.Windows.Forms.Label();
            this.lblCstID = new System.Windows.Forms.Label();
            this.lblPortCount = new System.Windows.Forms.Label();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.lblTrsMode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPortName
            // 
            this.lblPortName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPortName.BackColor = System.Drawing.Color.White;
            this.lblPortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPortName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortName.Location = new System.Drawing.Point(0, 0);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(72, 80);
            this.lblPortName.TabIndex = 0;
            this.lblPortName.Text = "PORT _";
            this.lblPortName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPortStatus
            // 
            this.lblPortStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPortStatus.BackColor = System.Drawing.Color.White;
            this.lblPortStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPortStatus.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortStatus.Location = new System.Drawing.Point(5, 15);
            this.lblPortStatus.Name = "lblPortStatus";
            this.lblPortStatus.Size = new System.Drawing.Size(62, 14);
            this.lblPortStatus.TabIndex = 1;
            this.lblPortStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCstID
            // 
            this.lblCstID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCstID.BackColor = System.Drawing.Color.White;
            this.lblCstID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCstID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblCstID.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCstID.Location = new System.Drawing.Point(5, 31);
            this.lblCstID.Name = "lblCstID";
            this.lblCstID.Size = new System.Drawing.Size(62, 14);
            this.lblCstID.TabIndex = 1;
            this.lblCstID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPortCount
            // 
            this.lblPortCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPortCount.BackColor = System.Drawing.Color.White;
            this.lblPortCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPortCount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblPortCount.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortCount.Location = new System.Drawing.Point(5, 47);
            this.lblPortCount.Name = "lblPortCount";
            this.lblPortCount.Size = new System.Drawing.Size(62, 14);
            this.lblPortCount.TabIndex = 1;
            this.lblPortCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 1000;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // lblTrsMode
            // 
            this.lblTrsMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTrsMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTrsMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblTrsMode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrsMode.Location = new System.Drawing.Point(5, 63);
            this.lblTrsMode.Name = "lblTrsMode";
            this.lblTrsMode.Size = new System.Drawing.Size(62, 14);
            this.lblTrsMode.TabIndex = 2;
            this.lblTrsMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CimPortState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblTrsMode);
            this.Controls.Add(this.lblPortCount);
            this.Controls.Add(this.lblCstID);
            this.Controls.Add(this.lblPortStatus);
            this.Controls.Add(this.lblPortName);
            this.Name = "CimPortState";
            this.Size = new System.Drawing.Size(72, 83);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.Label lblPortStatus;
        private System.Windows.Forms.Label lblCstID;
        private System.Windows.Forms.Label lblPortCount;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.Label lblTrsMode;
    }
}

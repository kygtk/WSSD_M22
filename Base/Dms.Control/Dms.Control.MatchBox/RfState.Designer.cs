namespace Dms.Control
{
    partial class RfState
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
            this.pbOnState = new System.Windows.Forms.PictureBox();
            this.pbOverTemp = new System.Windows.Forms.PictureBox();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblItem2 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBackColor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbOnState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOverTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // pbOnState
            // 
            this.pbOnState.BackColor = System.Drawing.Color.White;
            this.pbOnState.Location = new System.Drawing.Point(3, 20);
            this.pbOnState.Name = "pbOnState";
            this.pbOnState.Size = new System.Drawing.Size(14, 14);
            this.pbOnState.TabIndex = 35;
            this.pbOnState.TabStop = false;
            // 
            // pbOverTemp
            // 
            this.pbOverTemp.BackColor = System.Drawing.Color.White;
            this.pbOverTemp.Location = new System.Drawing.Point(3, 37);
            this.pbOverTemp.Name = "pbOverTemp";
            this.pbOverTemp.Size = new System.Drawing.Size(14, 14);
            this.pbOverTemp.TabIndex = 35;
            this.pbOverTemp.TabStop = false;
            // 
            // lblItem1
            // 
            this.lblItem1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblItem1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.lblItem1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem1.ForeColor = System.Drawing.Color.Black;
            this.lblItem1.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblItem1.Location = new System.Drawing.Point(20, 20);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(115, 14);
            this.lblItem1.TabIndex = 34;
            this.lblItem1.Text = "RF ON STATE";
            this.lblItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblItem2
            // 
            this.lblItem2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblItem2.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.lblItem2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblItem2.Location = new System.Drawing.Point(20, 37);
            this.lblItem2.Name = "lblItem2";
            this.lblItem2.Size = new System.Drawing.Size(115, 14);
            this.lblItem2.TabIndex = 34;
            this.lblItem2.Text = "RFG OVER TEMP";
            this.lblItem2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.DarkKhaki;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(132, 14);
            this.lblTitle.TabIndex = 37;
            this.lblTitle.Text = " RF Generator";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBackColor
            // 
            this.lblBackColor.BackColor = System.Drawing.Color.Olive;
            this.lblBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackColor.Location = new System.Drawing.Point(0, 0);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(138, 54);
            this.lblBackColor.TabIndex = 38;
            // 
            // RfState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.pbOverTemp);
            this.Controls.Add(this.pbOnState);
            this.Controls.Add(this.lblItem2);
            this.Controls.Add(this.lblItem1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblBackColor);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RfState";
            this.Size = new System.Drawing.Size(138, 54);
            ((System.ComponentModel.ISupportInitialize)(this.pbOnState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOverTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbOverTemp;
        private System.Windows.Forms.PictureBox pbOnState;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.Label lblItem2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBackColor;
    }
}

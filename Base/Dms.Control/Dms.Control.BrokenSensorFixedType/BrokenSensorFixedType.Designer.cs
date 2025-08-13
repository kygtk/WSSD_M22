namespace Dms.Control
{
    partial class BrokenSensorFixedType
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
            this.lblSensorRear_L = new System.Windows.Forms.Label();
            this.lblSensorFront_L = new System.Windows.Forms.Label();
            this.lblSensorRear_R = new System.Windows.Forms.Label();
            this.lblSensorFront_R = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSensorRear_L
            // 
            this.lblSensorRear_L.BackColor = System.Drawing.Color.White;
            this.lblSensorRear_L.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSensorRear_L.Location = new System.Drawing.Point(5, 5);
            this.lblSensorRear_L.Name = "lblSensorRear_L";
            this.lblSensorRear_L.Size = new System.Drawing.Size(15, 15);
            this.lblSensorRear_L.TabIndex = 0;
            this.lblSensorRear_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSensorFront_L
            // 
            this.lblSensorFront_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSensorFront_L.BackColor = System.Drawing.Color.White;
            this.lblSensorFront_L.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSensorFront_L.Location = new System.Drawing.Point(5, 26);
            this.lblSensorFront_L.Name = "lblSensorFront_L";
            this.lblSensorFront_L.Size = new System.Drawing.Size(15, 15);
            this.lblSensorFront_L.TabIndex = 1;
            this.lblSensorFront_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSensorRear_R
            // 
            this.lblSensorRear_R.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSensorRear_R.BackColor = System.Drawing.Color.White;
            this.lblSensorRear_R.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSensorRear_R.Location = new System.Drawing.Point(80, 5);
            this.lblSensorRear_R.Name = "lblSensorRear_R";
            this.lblSensorRear_R.Size = new System.Drawing.Size(15, 15);
            this.lblSensorRear_R.TabIndex = 2;
            this.lblSensorRear_R.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSensorFront_R
            // 
            this.lblSensorFront_R.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSensorFront_R.BackColor = System.Drawing.Color.White;
            this.lblSensorFront_R.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSensorFront_R.Location = new System.Drawing.Point(80, 26);
            this.lblSensorFront_R.Name = "lblSensorFront_R";
            this.lblSensorFront_R.Size = new System.Drawing.Size(15, 15);
            this.lblSensorFront_R.TabIndex = 3;
            this.lblSensorFront_R.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 44);
            this.label1.TabIndex = 4;
            // 
            // BrokenSensorFixedType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblSensorFront_R);
            this.Controls.Add(this.lblSensorRear_R);
            this.Controls.Add(this.lblSensorFront_L);
            this.Controls.Add(this.lblSensorRear_L);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BrokenSensorFixedType";
            this.Size = new System.Drawing.Size(100, 46);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSensorRear_L;
        private System.Windows.Forms.Label lblSensorFront_L;
        private System.Windows.Forms.Label lblSensorRear_R;
        private System.Windows.Forms.Label lblSensorFront_R;
        private System.Windows.Forms.Label label1;
    }
}

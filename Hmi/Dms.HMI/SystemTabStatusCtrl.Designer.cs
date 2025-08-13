namespace Dms.HMI
{
    partial class SystemTabStatusCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemTabStatusCtrl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.genInfo1 = new Dms.Control.GenInfo();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.genInfo1);
            this.groupBox1.Location = new System.Drawing.Point(9, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(881, 522);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // genInfo1
            // 
            this.genInfo1.BackColor = System.Drawing.Color.Transparent;
            this.genInfo1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("genInfo1.DeviceTagInfo")));
            this.genInfo1.Distance = 4;
            this.genInfo1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.Location = new System.Drawing.Point(19, 30);
            this.genInfo1.Name = "genInfo1";
            this.genInfo1.Size = new System.Drawing.Size(210, 20);
            this.genInfo1.TabIndex = 0;
            this.genInfo1.TitleBackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.genInfo1.TitleForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo1.TitlePanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo1.TitlePanelSize = 110;
            this.genInfo1.TitleText = "CPU Temperature";
            this.genInfo1.TitleTextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.genInfo1.ValueBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genInfo1.ValueForeColor = System.Drawing.SystemColors.ControlText;
            this.genInfo1.ValueTextFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genInfo1.ValueTextUnit = Dms.Common.UnitType.Celsius;
            // 
            // SystemTabStatusCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SystemTabStatusCtrl";
            this.Size = new System.Drawing.Size(903, 539);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Dms.Control.GenInfo genInfo1;


    }
}

namespace Dms.HMI
{
    partial class MaintToolbar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaintToolbar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDiStop = new Dms.Control.TagButton();
            this.btnDiRun = new Dms.Control.TagButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 595);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDiStop);
            this.groupBox1.Controls.Add(this.btnDiRun);
            this.groupBox1.Location = new System.Drawing.Point(4, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(84, 594);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnDiStop
            // 
            this.btnDiStop.BackColor = System.Drawing.Color.Transparent;
            this.btnDiStop.ButtonCheckedColor = System.Drawing.Color.Transparent;
            this.btnDiStop.ButtonCheckedType = Dms.Control.TagButton.Type.B_FalseCheck;
            this.btnDiStop.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDiStop.ButtonUnCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnDiStop.Command = Dms.Common.Command.DiStop;
            this.btnDiStop.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnDiStop.DeviceTagInfo")));
            this.btnDiStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDiStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDiStop.Image = global::Dms.HMI.Properties.Resources.DIStop;
            this.btnDiStop.Location = new System.Drawing.Point(3, 83);
            this.btnDiStop.Name = "btnDiStop";
            this.btnDiStop.Size = new System.Drawing.Size(78, 66);
            this.btnDiStop.TabIndex = 2;
            this.btnDiStop.Text = "DI Stop";
            this.btnDiStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDiStop.UseVisualStyleBackColor = true;
            // 
            // btnDiRun
            // 
            this.btnDiRun.BackColor = System.Drawing.Color.Transparent;
            this.btnDiRun.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnDiRun.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnDiRun.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDiRun.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnDiRun.Command = Dms.Common.Command.DiStart;
            this.btnDiRun.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnDiRun.DeviceTagInfo")));
            this.btnDiRun.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDiRun.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDiRun.Image = global::Dms.HMI.Properties.Resources.DIRun;
            this.btnDiRun.Location = new System.Drawing.Point(3, 17);
            this.btnDiRun.Name = "btnDiRun";
            this.btnDiRun.Size = new System.Drawing.Size(78, 66);
            this.btnDiRun.TabIndex = 3;
            this.btnDiRun.Text = "DI Run";
            this.btnDiRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDiRun.UseVisualStyleBackColor = true;
            // 
            // MaintToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MaintToolbar";
            this.Size = new System.Drawing.Size(92, 595);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        public Dms.Control.TagButton btnDiStop;
        public Dms.Control.TagButton btnDiRun;
    }
}

namespace Dms.Control
{
    partial class SwitchButton
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
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.buttonSwitch.BackColor = System.Drawing.Color.Tan;
            this.buttonSwitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSwitch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSwitch.Location = new System.Drawing.Point(0, 0);
            this.buttonSwitch.Name = "button1";
            this.buttonSwitch.Size = new System.Drawing.Size(78, 69);
            this.buttonSwitch.TabIndex = 0;
            this.buttonSwitch.Text = "Name";
            this.buttonSwitch.UseVisualStyleBackColor = false;
            this.buttonSwitch.Click += new System.EventHandler(this.buttonSwitch_Click);
            this.buttonSwitch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSwitch_MouseDown);
            this.buttonSwitch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSwitch_MouseUp);
            // 
            // SwitchButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.buttonSwitch);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SwitchButton";
            this.Size = new System.Drawing.Size(78, 69);
            this.Load += new System.EventHandler(this.SwitchButton_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSwitch;
    }
}

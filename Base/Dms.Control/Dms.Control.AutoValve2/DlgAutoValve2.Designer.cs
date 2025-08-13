namespace Dms.Control
{
    partial class DlgAutoValve2
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.btnOpen = new System.Windows.Forms.RadioButton();
            this.gbButton = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.RadioButton();
            this.gbButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(134, 100);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 40);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnOpen.BackColor = System.Drawing.Color.LightCyan;
            this.btnOpen.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOpen.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.Location = new System.Drawing.Point(14, 24);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(97, 50);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "OPEN";
            this.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.CheckedChanged += new System.EventHandler(this.btnOpen_CheckedChanged);
            // 
            // gbButton
            // 
            this.gbButton.Controls.Add(this.btnClose);
            this.gbButton.Controls.Add(this.btnOpen);
            this.gbButton.Location = new System.Drawing.Point(7, 4);
            this.gbButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbButton.Name = "gbButton";
            this.gbButton.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbButton.Size = new System.Drawing.Size(227, 88);
            this.gbButton.TabIndex = 2;
            this.gbButton.TabStop = false;
            this.gbButton.Text = "Valve On/Off";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnClose.BackColor = System.Drawing.Color.Pink;
            this.btnClose.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(114, 24);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 50);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "CLOSE";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.CheckedChanged += new System.EventHandler(this.btnClose_CheckedChanged);
            // 
            // DlgAutoValve2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(242, 148);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbButton);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgAutoValve2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.DlgAutoValve2_Load);
            this.gbButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.RadioButton btnOpen;
        private System.Windows.Forms.GroupBox gbButton;
        private System.Windows.Forms.RadioButton btnClose;
    }
}
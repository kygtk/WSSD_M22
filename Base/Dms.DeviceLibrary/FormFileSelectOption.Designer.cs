namespace Dms.DeviceLibrary
{
    partial class FormFileSelectOption
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonSelectWithoutQuestion = new System.Windows.Forms.Button();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 96);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // buttonSelect
            // 
            this.buttonSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSelect.Location = new System.Drawing.Point(10, 116);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(94, 35);
            this.buttonSelect.TabIndex = 1;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonSelectWithoutQuestion
            // 
            this.buttonSelectWithoutQuestion.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.buttonSelectWithoutQuestion.Location = new System.Drawing.Point(114, 116);
            this.buttonSelectWithoutQuestion.Name = "buttonSelectWithoutQuestion";
            this.buttonSelectWithoutQuestion.Size = new System.Drawing.Size(94, 35);
            this.buttonSelectWithoutQuestion.TabIndex = 1;
            this.buttonSelectWithoutQuestion.Text = "Select without this dialog";
            this.buttonSelectWithoutQuestion.UseVisualStyleBackColor = true;
            this.buttonSelectWithoutQuestion.Click += new System.EventHandler(this.buttonSelectWithoutQuestion_Click);
            // 
            // buttonSkip
            // 
            this.buttonSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.buttonSkip.Location = new System.Drawing.Point(218, 116);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(94, 35);
            this.buttonSkip.TabIndex = 1;
            this.buttonSkip.Text = "Skip all";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // FormFileSelectOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(322, 164);
            this.ControlBox = false;
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.buttonSelectWithoutQuestion);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormFileSelectOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileSelectOption";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonSelectWithoutQuestion;
        private System.Windows.Forms.Button buttonSkip;
    }
}
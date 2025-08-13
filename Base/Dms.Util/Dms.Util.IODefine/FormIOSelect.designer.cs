namespace Dms.Util.IODefine
{
    partial class FormIOSelect
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
            this.ButtonSelect = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.viewIOEdit1 = new Dms.Util.IODefine.ViewIOEdit();
            this.SuspendLayout();
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonSelect.Location = new System.Drawing.Point(567, 57);
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(93, 268);
            this.ButtonSelect.TabIndex = 1;
            this.ButtonSelect.Text = "Select";
            this.ButtonSelect.UseVisualStyleBackColor = true;
            this.ButtonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClose.Location = new System.Drawing.Point(567, 331);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(93, 46);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClear.Location = new System.Drawing.Point(567, 5);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(93, 46);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // viewIOEdit1
            // 
            this.viewIOEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewIOEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewIOEdit1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOEdit1.Location = new System.Drawing.Point(4, 4);
            this.viewIOEdit1.Name = "viewIOEdit1";
            this.viewIOEdit1.OperateMode = Dms.Util.IODefine.ViewIOEdit.OpMode.Config;
            this.viewIOEdit1.Size = new System.Drawing.Size(560, 382);
            this.viewIOEdit1.TabIndex = 2;
            this.viewIOEdit1.TimerStateUpdateEnabled = false;
            // 
            // FormIOSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(675, 389);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.viewIOEdit1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.ButtonSelect);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormIOSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormIOSelect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonSelect;
        private System.Windows.Forms.Button buttonClose;
        private Dms.Util.IODefine.ViewIOEdit viewIOEdit1;
        private System.Windows.Forms.Button buttonClear;

    }
}
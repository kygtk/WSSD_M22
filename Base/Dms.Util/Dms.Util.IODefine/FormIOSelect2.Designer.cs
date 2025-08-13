namespace Dms.Util.IODefine
{
	partial class FormIOSelect2
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
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.ButtonSelect = new System.Windows.Forms.Button();
            this.viewIOSelect21 = new Dms.Util.IODefine.ViewIOSelect2();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClear.Location = new System.Drawing.Point(378, 9);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(76, 46);
            this.buttonClear.TabIndex = 6;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClose.Location = new System.Drawing.Point(378, 328);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(76, 46);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonSelect.Location = new System.Drawing.Point(378, 61);
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(76, 261);
            this.ButtonSelect.TabIndex = 4;
            this.ButtonSelect.Text = "Select";
            this.ButtonSelect.UseVisualStyleBackColor = true;
            this.ButtonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // viewIOSelect21
            // 
            this.viewIOSelect21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewIOSelect21.BackColor = System.Drawing.Color.Transparent;
            this.viewIOSelect21.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOSelect21.Location = new System.Drawing.Point(6, 8);
            this.viewIOSelect21.Name = "viewIOSelect21";
            this.viewIOSelect21.Size = new System.Drawing.Size(368, 372);
            this.viewIOSelect21.TabIndex = 3;
            // 
            // FormIOSelect2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(461, 383);
            this.ControlBox = false;
            this.Controls.Add(this.viewIOSelect21);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.ButtonSelect);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormIOSelect2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormIOSelect2";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button ButtonSelect;
		private ViewIOSelect2 viewIOSelect21;
	}
}
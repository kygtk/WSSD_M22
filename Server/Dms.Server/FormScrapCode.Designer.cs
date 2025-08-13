namespace Dms.Server
{
	partial class FormScrapCode
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
			this.comboBoxCodeList = new System.Windows.Forms.ComboBox();
			this.labelTitle = new System.Windows.Forms.Label();
			this.buttonSelect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// comboBoxCodeList
			// 
			this.comboBoxCodeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCodeList.FormattingEnabled = true;
			this.comboBoxCodeList.Location = new System.Drawing.Point(8, 49);
			this.comboBoxCodeList.Name = "comboBoxCodeList";
			this.comboBoxCodeList.Size = new System.Drawing.Size(200, 20);
			this.comboBoxCodeList.TabIndex = 0;
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.labelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelTitle.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelTitle.Location = new System.Drawing.Point(8, 9);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(200, 24);
			this.labelTitle.TabIndex = 9;
			this.labelTitle.Text = "Select Scrap Code";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonSelect
			// 
			this.buttonSelect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.buttonSelect.Location = new System.Drawing.Point(8, 91);
			this.buttonSelect.Name = "buttonSelect";
			this.buttonSelect.Size = new System.Drawing.Size(200, 42);
			this.buttonSelect.TabIndex = 10;
			this.buttonSelect.Text = "Select";
			this.buttonSelect.UseVisualStyleBackColor = true;
			this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
			// 
			// FormScrapCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(220, 149);
			this.ControlBox = false;
			this.Controls.Add(this.buttonSelect);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.comboBoxCodeList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormScrapCode";
			this.Text = " ";
			this.Load += new System.EventHandler(this.FormScrapCode_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxCodeList;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Button buttonSelect;
	}
}
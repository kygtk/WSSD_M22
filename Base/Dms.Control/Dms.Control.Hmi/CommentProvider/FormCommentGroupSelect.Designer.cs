namespace Dms.Control.Hmi
{
	partial class FormCommentGroupSelect
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
			this.viewCommentGroupSelect1 = new Dms.Control.Hmi.ViewCommentGroupSelect();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// viewCommentGroupSelect1
			// 
			this.viewCommentGroupSelect1.BackColor = System.Drawing.Color.Transparent;
			this.viewCommentGroupSelect1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.viewCommentGroupSelect1.Location = new System.Drawing.Point(1, 55);
			this.viewCommentGroupSelect1.Name = "viewCommentGroupSelect1";
			this.viewCommentGroupSelect1.Size = new System.Drawing.Size(314, 293);
			this.viewCommentGroupSelect1.TabIndex = 0;
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(3, 348);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 51);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(235, 348);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 51);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(84, 348);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(145, 51);
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(3, 3);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(307, 46);
			this.btnEdit.TabIndex = 4;
			this.btnEdit.Text = "Edit Comment Group";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// FormCommentGroupSelect
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(313, 403);
			this.ControlBox = false;
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.viewCommentGroupSelect1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormCommentGroupSelect";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormCommentGroupSelect";
			this.ResumeLayout(false);

		}

		#endregion

		private ViewCommentGroupSelect viewCommentGroupSelect1;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnEdit;
	}
}
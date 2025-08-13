namespace Dms.Control.Hmi
{
	partial class FormCommentGroupConfig
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
			this.viewCommentGroupConfig1 = new Dms.Control.Hmi.ViewCommentGroupConfig();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// viewCommentGroupConfig1
			// 
			this.viewCommentGroupConfig1.BackColor = System.Drawing.Color.Transparent;
			this.viewCommentGroupConfig1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.viewCommentGroupConfig1.Location = new System.Drawing.Point(1, 2);
			this.viewCommentGroupConfig1.Name = "viewCommentGroupConfig1";
			this.viewCommentGroupConfig1.Size = new System.Drawing.Size(690, 427);
			this.viewCommentGroupConfig1.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(550, 433);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(142, 34);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// FormCommentGroupConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(693, 470);
			this.ControlBox = false;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.viewCommentGroupConfig1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormCommentGroupConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormCommentGroupConfig";
			this.ResumeLayout(false);

		}

		#endregion

		private ViewCommentGroupConfig viewCommentGroupConfig1;
		private System.Windows.Forms.Button btnClose;
	}
}
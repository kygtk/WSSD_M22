namespace Dms.Common
{
	partial class FormExceptionHandler
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
			this.listBoxException = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBoxException
			// 
			this.listBoxException.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxException.FormattingEnabled = true;
			this.listBoxException.ItemHeight = 12;
			this.listBoxException.Location = new System.Drawing.Point(5, 12);
			this.listBoxException.Name = "listBoxException";
			this.listBoxException.Size = new System.Drawing.Size(341, 280);
			this.listBoxException.TabIndex = 0;
			// 
			// FormExceptionHandler
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(351, 300);
			this.ControlBox = false;
			this.Controls.Add(this.listBoxException);
			this.Name = "FormExceptionHandler";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " WSSD - Exception";
			this.Load += new System.EventHandler(this.FormExceptionHandler_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxException;


	}
}
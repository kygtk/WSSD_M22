namespace Dms.Control.Hmi
{
	partial class HmiCommentBit
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
			this.lblComment = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblComment
			// 
			this.lblComment.BackColor = System.Drawing.Color.White;
			this.lblComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblComment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblComment.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblComment.Location = new System.Drawing.Point(0, 0);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(100, 20);
			this.lblComment.TabIndex = 3;
			this.lblComment.Text = "Comment";
			this.lblComment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// HmiCommentSimple
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.lblComment);
			this.Name = "HmiCommentSimple";
			this.Size = new System.Drawing.Size(100, 20);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblComment;



	}
}

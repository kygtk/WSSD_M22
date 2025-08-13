namespace Dms.Control.Hmi
{
	partial class HmiDateTime
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
			this.lblTime = new System.Windows.Forms.Label();
			this.lblDate = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTime
			// 
			this.lblTime.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblTime.Location = new System.Drawing.Point(0, 19);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(111, 18);
			this.lblTime.TabIndex = 7;
			this.lblTime.Text = "PM 00:00:00";
			this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDate
			// 
			this.lblDate.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblDate.Location = new System.Drawing.Point(0, 0);
			this.lblDate.Name = "lblDate";
			this.lblDate.Size = new System.Drawing.Size(111, 19);
			this.lblDate.TabIndex = 6;
			this.lblDate.Text = "0000/00/00";
			this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// HmiDateTime
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.lblTime);
			this.Controls.Add(this.lblDate);
			this.Name = "HmiDateTime";
			this.Size = new System.Drawing.Size(111, 37);
			this.UpdateTimerInterval = 1000;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Label lblDate;
	}
}

namespace Dms.Util.IODefine
{
	partial class FormMelsecDeviceEditor
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
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonCommit = new System.Windows.Forms.Button();
			this.viewMelsecDevices1 = new Dms.Util.IODefine.ViewMelsecDeviceEdit();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.buttonClose.Location = new System.Drawing.Point(538, 427);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(141, 41);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonCommit
			// 
			this.buttonCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCommit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.buttonCommit.Location = new System.Drawing.Point(12, 427);
			this.buttonCommit.Name = "buttonCommit";
			this.buttonCommit.Size = new System.Drawing.Size(520, 41);
			this.buttonCommit.TabIndex = 2;
			this.buttonCommit.Text = "Commit";
			this.buttonCommit.UseVisualStyleBackColor = true;
			this.buttonCommit.Click += new System.EventHandler(this.buttonCommit_Click);
			// 
			// viewMelsecDevices1
			// 
			this.viewMelsecDevices1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.viewMelsecDevices1.Location = new System.Drawing.Point(12, 12);
			this.viewMelsecDevices1.Name = "viewMelsecDevices1";
			this.viewMelsecDevices1.Padding = new System.Windows.Forms.Padding(1);
			this.viewMelsecDevices1.Size = new System.Drawing.Size(667, 406);
			this.viewMelsecDevices1.TabIndex = 3;
			// 
			// FormMelsecDeviceEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(691, 474);
			this.ControlBox = false;
			this.Controls.Add(this.viewMelsecDevices1);
			this.Controls.Add(this.buttonCommit);
			this.Controls.Add(this.buttonClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FormMelsecDeviceEditor";
			this.Text = " MelsecDeviceEdit";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonCommit;
		private ViewMelsecDeviceEdit viewMelsecDevices1;
	}
}
namespace Dms.Util.IODefine
{
	partial class ViewMelsecDeviceEdit
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridViewDevices = new Dms.Control.DoubleBufferedGridView();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevices)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridViewDevices
			// 
			this.dataGridViewDevices.AllowUserToAddRows = false;
			this.dataGridViewDevices.AllowUserToOrderColumns = true;
			this.dataGridViewDevices.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridViewDevices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridViewDevices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridViewDevices.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewDevices.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewDevices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewDevices.Location = new System.Drawing.Point(1, 1);
			this.dataGridViewDevices.Name = "dataGridViewDevices";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.NullValue = null;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewDevices.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewDevices.RowTemplate.Height = 18;
			this.dataGridViewDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewDevices.Size = new System.Drawing.Size(326, 252);
			this.dataGridViewDevices.TabIndex = 1;
			this.dataGridViewDevices.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewDevices_ColumnHeaderMouseClick);
			this.dataGridViewDevices.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewDevices_CellMouseDown);
			this.dataGridViewDevices.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewDevices_DataError);
			this.dataGridViewDevices.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewDevices_KeyDown);
			this.dataGridViewDevices.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewDevices_RowHeaderMouseClick);
			// 
			// ViewMelsecDevices
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.dataGridViewDevices);
			this.Name = "ViewMelsecDevices";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(328, 254);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevices)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Dms.Control.DoubleBufferedGridView dataGridViewDevices;
	}
}

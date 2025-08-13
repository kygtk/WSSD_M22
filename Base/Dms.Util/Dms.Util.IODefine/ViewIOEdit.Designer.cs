namespace Dms.Util.IODefine
{
    partial class ViewIOEdit
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridView1 = new Dms.Control.DoubleBufferedGridView();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dataGridView2 = new Dms.Control.DoubleBufferedGridView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.dataGridView3 = new Dms.Control.DoubleBufferedGridView();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.dataGridView4 = new Dms.Control.DoubleBufferedGridView();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.buttonFiltering = new System.Windows.Forms.Button();
			this.textBoxFilter = new System.Windows.Forms.TextBox();
			this.checkBoxFilterOn = new System.Windows.Forms.CheckBox();
			this.comboBoxDisplayFormat = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle10;
			this.dataGridView1.Location = new System.Drawing.Point(6, 6);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.Height = 18;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView1.Size = new System.Drawing.Size(676, 294);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
			this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
			this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
			this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
			this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
			this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Location = new System.Drawing.Point(3, 31);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(696, 334);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dataGridView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(688, 306);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Digital Inputs";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dataGridView2);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(688, 306);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Digital Output";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// dataGridView2
			// 
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.AllowUserToResizeRows = false;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
			this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle12;
			this.dataGridView2.Location = new System.Drawing.Point(6, 6);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.RowHeadersVisible = false;
			this.dataGridView2.RowTemplate.Height = 18;
			this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView2.Size = new System.Drawing.Size(676, 294);
			this.dataGridView2.TabIndex = 1;
			this.dataGridView2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
			this.dataGridView2.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
			this.dataGridView2.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
			this.dataGridView2.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
			this.dataGridView2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
			this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.dataGridView3);
			this.tabPage3.Location = new System.Drawing.Point(4, 24);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(688, 306);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Analog Input";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// dataGridView3
			// 
			this.dataGridView3.AllowUserToAddRows = false;
			this.dataGridView3.AllowUserToDeleteRows = false;
			this.dataGridView3.AllowUserToResizeRows = false;
			dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
			this.dataGridView3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView3.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle14;
			this.dataGridView3.Location = new System.Drawing.Point(6, 6);
			this.dataGridView3.Name = "dataGridView3";
			this.dataGridView3.RowHeadersVisible = false;
			this.dataGridView3.RowTemplate.Height = 18;
			this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView3.Size = new System.Drawing.Size(676, 294);
			this.dataGridView3.TabIndex = 1;
			this.dataGridView3.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
			this.dataGridView3.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
			this.dataGridView3.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
			this.dataGridView3.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
			this.dataGridView3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
			this.dataGridView3.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.dataGridView4);
			this.tabPage4.Location = new System.Drawing.Point(4, 24);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(688, 306);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Analog Output";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// dataGridView4
			// 
			this.dataGridView4.AllowUserToAddRows = false;
			this.dataGridView4.AllowUserToDeleteRows = false;
			this.dataGridView4.AllowUserToResizeRows = false;
			dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
			this.dataGridView4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
			this.dataGridView4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView4.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView4.DefaultCellStyle = dataGridViewCellStyle16;
			this.dataGridView4.Location = new System.Drawing.Point(6, 6);
			this.dataGridView4.Name = "dataGridView4";
			this.dataGridView4.RowHeadersVisible = false;
			this.dataGridView4.RowTemplate.Height = 18;
			this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView4.Size = new System.Drawing.Size(676, 294);
			this.dataGridView4.TabIndex = 1;
			this.dataGridView4.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
			this.dataGridView4.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
			this.dataGridView4.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
			this.dataGridView4.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
			this.dataGridView4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
			this.dataGridView4.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
			// 
			// timer1
			// 
			this.timer1.Interval = 300;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// buttonFiltering
			// 
			this.buttonFiltering.Location = new System.Drawing.Point(347, 3);
			this.buttonFiltering.Name = "buttonFiltering";
			this.buttonFiltering.Size = new System.Drawing.Size(38, 23);
			this.buttonFiltering.TabIndex = 21;
			this.buttonFiltering.Text = "Do";
			this.buttonFiltering.UseVisualStyleBackColor = true;
			this.buttonFiltering.Click += new System.EventHandler(this.buttonFiltering_Click);
			// 
			// textBoxFilter
			// 
			this.textBoxFilter.Location = new System.Drawing.Point(3, 4);
			this.textBoxFilter.Name = "textBoxFilter";
			this.textBoxFilter.Size = new System.Drawing.Size(338, 21);
			this.textBoxFilter.TabIndex = 19;
			this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
			this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
			// 
			// checkBoxFilterOn
			// 
			this.checkBoxFilterOn.AutoSize = true;
			this.checkBoxFilterOn.Location = new System.Drawing.Point(391, 6);
			this.checkBoxFilterOn.Name = "checkBoxFilterOn";
			this.checkBoxFilterOn.Size = new System.Drawing.Size(53, 19);
			this.checkBoxFilterOn.TabIndex = 20;
			this.checkBoxFilterOn.Text = "Filter";
			this.checkBoxFilterOn.UseVisualStyleBackColor = true;
			this.checkBoxFilterOn.CheckedChanged += new System.EventHandler(this.checkBoxFilterOn_CheckedChanged);
			// 
			// comboBoxDisplayFormat
			// 
			this.comboBoxDisplayFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxDisplayFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDisplayFormat.FormattingEnabled = true;
			this.comboBoxDisplayFormat.Location = new System.Drawing.Point(568, 3);
			this.comboBoxDisplayFormat.Name = "comboBoxDisplayFormat";
			this.comboBoxDisplayFormat.Size = new System.Drawing.Size(121, 23);
			this.comboBoxDisplayFormat.TabIndex = 22;
			this.comboBoxDisplayFormat.Visible = false;			
			// 
			// ViewIOEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.comboBoxDisplayFormat);
			this.Controls.Add(this.buttonFiltering);
			this.Controls.Add(this.textBoxFilter);
			this.Controls.Add(this.checkBoxFilterOn);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ViewIOEdit";
			this.Size = new System.Drawing.Size(702, 372);
			this.Load += new System.EventHandler(this.ViewIOEdit_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
			this.tabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonFiltering;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.CheckBox checkBoxFilterOn;
        private Dms.Control.DoubleBufferedGridView dataGridView1;
        private Dms.Control.DoubleBufferedGridView dataGridView2;
        private Dms.Control.DoubleBufferedGridView dataGridView3;
		private Dms.Control.DoubleBufferedGridView dataGridView4;
		private System.Windows.Forms.ComboBox comboBoxDisplayFormat;
    }
}

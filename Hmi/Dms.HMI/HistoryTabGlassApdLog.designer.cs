namespace Dms.HMI
{
    partial class HistoryTabGlassApdLog
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtLotId = new System.Windows.Forms.TextBox();
            this.txtGlassId = new System.Windows.Forms.TextBox();
            this.txtRecipeId = new System.Windows.Forms.TextBox();
            this.txtCassetteId = new System.Windows.Forms.TextBox();
            this.checkGlassId = new System.Windows.Forms.CheckBox();
            this.checkRecipeId = new System.Windows.Forms.CheckBox();
            this.checkLotId = new System.Windows.Forms.CheckBox();
            this.checkCassetteId = new System.Windows.Forms.CheckBox();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPageList = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioProcessFault = new System.Windows.Forms.RadioButton();
            this.radioProcessTrue = new System.Windows.Forms.RadioButton();
            this.radioProcesstAll = new System.Windows.Forms.RadioButton();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.viewGlassApdHistory1 = new Dms.Data.ViewCimGlassApdHistory();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.lblPageCount);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbPageList);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.dateTimePickerDate);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(899, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Get Data";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLotId);
            this.groupBox3.Controls.Add(this.txtGlassId);
            this.groupBox3.Controls.Add(this.txtRecipeId);
            this.groupBox3.Controls.Add(this.txtCassetteId);
            this.groupBox3.Controls.Add(this.checkGlassId);
            this.groupBox3.Controls.Add(this.checkRecipeId);
            this.groupBox3.Controls.Add(this.checkLotId);
            this.groupBox3.Controls.Add(this.checkCassetteId);
            this.groupBox3.Location = new System.Drawing.Point(250, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(463, 80);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Indicate ID";
            // 
            // txtLotId
            // 
            this.txtLotId.Location = new System.Drawing.Point(335, 49);
            this.txtLotId.Name = "txtLotId";
            this.txtLotId.Size = new System.Drawing.Size(122, 21);
            this.txtLotId.TabIndex = 4;
            this.txtLotId.Visible = false;
            this.txtLotId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtGlassId
            // 
            this.txtGlassId.Location = new System.Drawing.Point(101, 51);
            this.txtGlassId.Name = "txtGlassId";
            this.txtGlassId.Size = new System.Drawing.Size(122, 21);
            this.txtGlassId.TabIndex = 4;
            this.txtGlassId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtRecipeId
            // 
            this.txtRecipeId.Location = new System.Drawing.Point(101, 22);
            this.txtRecipeId.Name = "txtRecipeId";
            this.txtRecipeId.Size = new System.Drawing.Size(122, 21);
            this.txtRecipeId.TabIndex = 4;
            this.txtRecipeId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtCassetteId
            // 
            this.txtCassetteId.Location = new System.Drawing.Point(335, 20);
            this.txtCassetteId.Name = "txtCassetteId";
            this.txtCassetteId.Size = new System.Drawing.Size(122, 21);
            this.txtCassetteId.TabIndex = 4;
            this.txtCassetteId.Visible = false;
            this.txtCassetteId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // checkGlassId
            // 
            this.checkGlassId.AutoSize = true;
            this.checkGlassId.Location = new System.Drawing.Point(16, 52);
            this.checkGlassId.Name = "checkGlassId";
            this.checkGlassId.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkGlassId.Size = new System.Drawing.Size(72, 19);
            this.checkGlassId.TabIndex = 3;
            this.checkGlassId.Text = "Glass ID";
            this.checkGlassId.UseVisualStyleBackColor = true;
            // 
            // checkRecipeId
            // 
            this.checkRecipeId.AutoSize = true;
            this.checkRecipeId.Location = new System.Drawing.Point(16, 24);
            this.checkRecipeId.Name = "checkRecipeId";
            this.checkRecipeId.Size = new System.Drawing.Size(79, 19);
            this.checkRecipeId.TabIndex = 2;
            this.checkRecipeId.Text = "Recipe ID";
            this.checkRecipeId.UseVisualStyleBackColor = true;
            // 
            // checkLotId
            // 
            this.checkLotId.AutoSize = true;
            this.checkLotId.Location = new System.Drawing.Point(244, 50);
            this.checkLotId.Name = "checkLotId";
            this.checkLotId.Size = new System.Drawing.Size(58, 19);
            this.checkLotId.TabIndex = 1;
            this.checkLotId.Text = "Lot ID";
            this.checkLotId.UseVisualStyleBackColor = true;
            this.checkLotId.Visible = false;
            // 
            // checkCassetteId
            // 
            this.checkCassetteId.AutoSize = true;
            this.checkCassetteId.Location = new System.Drawing.Point(244, 22);
            this.checkCassetteId.Name = "checkCassetteId";
            this.checkCassetteId.Size = new System.Drawing.Size(91, 19);
            this.checkCassetteId.TabIndex = 0;
            this.checkCassetteId.Text = "Cassette ID";
            this.checkCassetteId.UseVisualStyleBackColor = true;
            this.checkCassetteId.Visible = false;
            // 
            // lblPageCount
            // 
            this.lblPageCount.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageCount.Location = new System.Drawing.Point(867, 15);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(29, 23);
            this.lblPageCount.TabIndex = 17;
            this.lblPageCount.Text = "1";
            this.lblPageCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(859, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 23);
            this.label3.TabIndex = 16;
            this.label3.Text = "/";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(756, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Page :";
            // 
            // cbPageList
            // 
            this.cbPageList.FormattingEnabled = true;
            this.cbPageList.Location = new System.Drawing.Point(809, 14);
            this.cbPageList.Name = "cbPageList";
            this.cbPageList.Size = new System.Drawing.Size(48, 23);
            this.cbPageList.TabIndex = 14;
            this.cbPageList.SelectionChangeCommitted += new System.EventHandler(this.cbPageList_SelectionChangeCommitted);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(731, 43);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(137, 51);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Execute";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioProcessFault);
            this.groupBox1.Controls.Add(this.radioProcessTrue);
            this.groupBox1.Controls.Add(this.radioProcesstAll);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 47);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Result";
            this.groupBox1.Visible = false;
            // 
            // radioProcessFault
            // 
            this.radioProcessFault.AutoSize = true;
            this.radioProcessFault.Enabled = false;
            this.radioProcessFault.Location = new System.Drawing.Point(161, 21);
            this.radioProcessFault.Name = "radioProcessFault";
            this.radioProcessFault.Size = new System.Drawing.Size(52, 19);
            this.radioProcessFault.TabIndex = 2;
            this.radioProcessFault.TabStop = true;
            this.radioProcessFault.Text = "Fault";
            this.radioProcessFault.UseVisualStyleBackColor = true;
            this.radioProcessFault.Visible = false;
            this.radioProcessFault.CheckedChanged += new System.EventHandler(this.radioProcessFault_CheckedChanged);
            // 
            // radioProcessTrue
            // 
            this.radioProcessTrue.AutoSize = true;
            this.radioProcessTrue.Enabled = false;
            this.radioProcessTrue.Location = new System.Drawing.Point(79, 21);
            this.radioProcessTrue.Name = "radioProcessTrue";
            this.radioProcessTrue.Size = new System.Drawing.Size(51, 19);
            this.radioProcessTrue.TabIndex = 1;
            this.radioProcessTrue.TabStop = true;
            this.radioProcessTrue.Text = "True";
            this.radioProcessTrue.UseVisualStyleBackColor = true;
            this.radioProcessTrue.Visible = false;
            this.radioProcessTrue.CheckedChanged += new System.EventHandler(this.radioProcessTrue_CheckedChanged);
            // 
            // radioProcesstAll
            // 
            this.radioProcesstAll.AutoSize = true;
            this.radioProcesstAll.Enabled = false;
            this.radioProcesstAll.Location = new System.Drawing.Point(17, 21);
            this.radioProcesstAll.Name = "radioProcesstAll";
            this.radioProcesstAll.Size = new System.Drawing.Size(39, 19);
            this.radioProcesstAll.TabIndex = 0;
            this.radioProcesstAll.TabStop = true;
            this.radioProcesstAll.Text = "All";
            this.radioProcesstAll.UseVisualStyleBackColor = true;
            this.radioProcesstAll.Visible = false;
            this.radioProcesstAll.CheckedChanged += new System.EventHandler(this.radioProcesstAll_CheckedChanged);
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDate.Location = new System.Drawing.Point(59, 21);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(185, 21);
            this.dateTimePickerDate.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Date :";
            // 
            // viewGlassApdHistory1
            // 
            this.viewGlassApdHistory1.BackColor = System.Drawing.Color.Transparent;
            this.viewGlassApdHistory1.ColumnHeaderVisible = true;
            this.viewGlassApdHistory1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.viewGlassApdHistory1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewGlassApdHistory1.HistoryCount = 20;
            this.viewGlassApdHistory1.Location = new System.Drawing.Point(0, 103);
            this.viewGlassApdHistory1.Name = "viewGlassApdHistory1";
            this.viewGlassApdHistory1.RowHeaderVisible = false;
            this.viewGlassApdHistory1.Size = new System.Drawing.Size(903, 436);
            this.viewGlassApdHistory1.TabIndex = 0;
            this.viewGlassApdHistory1.TitleName = "Glass Apd History";
            this.viewGlassApdHistory1.Load += new System.EventHandler(this.viewGlassApdHistory1_Load);
            // 
            // HistoryTabGlassApdLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.viewGlassApdHistory1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HistoryTabGlassApdLog";
            this.Size = new System.Drawing.Size(903, 539);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewCimGlassApdHistory viewGlassApdHistory1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkGlassId;
        private System.Windows.Forms.CheckBox checkRecipeId;
        private System.Windows.Forms.CheckBox checkLotId;
        private System.Windows.Forms.CheckBox checkCassetteId;
        private System.Windows.Forms.Label lblPageCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPageList;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCassetteId;
        private System.Windows.Forms.TextBox txtLotId;
        private System.Windows.Forms.TextBox txtRecipeId;
        private System.Windows.Forms.TextBox txtGlassId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioProcessFault;
        private System.Windows.Forms.RadioButton radioProcessTrue;
        private System.Windows.Forms.RadioButton radioProcesstAll;

    }
}

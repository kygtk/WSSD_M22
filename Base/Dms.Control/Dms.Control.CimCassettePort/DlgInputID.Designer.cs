namespace Dms.Control
{
    partial class DlgInputID
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPortID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLotID = new System.Windows.Forms.Label();
            this.lblCstID = new System.Windows.Forms.Label();
            this.cbCassettetype = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewInput = new System.Windows.Forms.DataGridView();
            this.btnAllSlotSelect = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSheetID = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAllSlotSet = new System.Windows.Forms.Button();
            this.cbThickness = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblRecipeID = new System.Windows.Forms.Label();
            this.btnCopyGreen = new System.Windows.Forms.Button();
            this.btnAllSlotCopy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInput)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port ID";
            // 
            // txtPortID
            // 
            this.txtPortID.Location = new System.Drawing.Point(115, 23);
            this.txtPortID.Name = "txtPortID";
            this.txtPortID.ReadOnly = true;
            this.txtPortID.Size = new System.Drawing.Size(169, 21);
            this.txtPortID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cassette ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblLotID);
            this.groupBox1.Controls.Add(this.lblCstID);
            this.groupBox1.Controls.Add(this.cbCassettetype);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 88);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lblLotID
            // 
            this.lblLotID.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblLotID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLotID.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLotID.Location = new System.Drawing.Point(402, 23);
            this.lblLotID.Name = "lblLotID";
            this.lblLotID.Size = new System.Drawing.Size(224, 23);
            this.lblLotID.TabIndex = 8;
            this.lblLotID.Click += new System.EventHandler(this.lblLotID_Click);
            // 
            // lblCstID
            // 
            this.lblCstID.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCstID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCstID.Location = new System.Drawing.Point(103, 49);
            this.lblCstID.Name = "lblCstID";
            this.lblCstID.Size = new System.Drawing.Size(169, 22);
            this.lblCstID.TabIndex = 7;
            this.lblCstID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCstID.Click += new System.EventHandler(this.lblCstID_Click);
            // 
            // cbCassettetype
            // 
            this.cbCassettetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCassettetype.FormattingEnabled = true;
            this.cbCassettetype.Items.AddRange(new object[] {
            "A : Slot(30) without Back Support",
            "B : Slot(30) with Back Support",
            "C : Slot(35) with Back Support",
            "G : Slot(39)"});
            this.cbCassettetype.Location = new System.Drawing.Point(402, 52);
            this.cbCassettetype.Name = "cbCassettetype";
            this.cbCassettetype.Size = new System.Drawing.Size(224, 23);
            this.cbCassettetype.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Cassette Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Lot ID";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(8, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(83, 45);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(175, 19);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(83, 45);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.btnOK);
            this.groupBox2.Location = new System.Drawing.Point(448, 537);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 74);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // dataGridViewInput
            // 
            this.dataGridViewInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewInput.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewInput.Location = new System.Drawing.Point(12, 161);
            this.dataGridViewInput.Name = "dataGridViewInput";
            this.dataGridViewInput.RowTemplate.Height = 23;
            this.dataGridViewInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInput.Size = new System.Drawing.Size(704, 370);
            this.dataGridViewInput.TabIndex = 7;
            // 
            // btnAllSlotSelect
            // 
            this.btnAllSlotSelect.Location = new System.Drawing.Point(10, 98);
            this.btnAllSlotSelect.Name = "btnAllSlotSelect";
            this.btnAllSlotSelect.Size = new System.Drawing.Size(75, 57);
            this.btnAllSlotSelect.TabIndex = 8;
            this.btnAllSlotSelect.Text = "All Slot Select";
            this.btnAllSlotSelect.UseVisualStyleBackColor = true;
            this.btnAllSlotSelect.Click += new System.EventHandler(this.btnAllSlotSelect_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblSheetID);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnAllSlotSet);
            this.groupBox3.Controls.Add(this.cbThickness);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(91, 91);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(315, 64);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // lblSheetID
            // 
            this.lblSheetID.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSheetID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSheetID.Location = new System.Drawing.Point(133, 30);
            this.lblSheetID.Name = "lblSheetID";
            this.lblSheetID.Size = new System.Drawing.Size(114, 24);
            this.lblSheetID.TabIndex = 12;
            this.lblSheetID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSheetID.Click += new System.EventHandler(this.lblSheetID_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Sheet ID";
            // 
            // btnAllSlotSet
            // 
            this.btnAllSlotSet.Location = new System.Drawing.Point(250, 14);
            this.btnAllSlotSet.Name = "btnAllSlotSet";
            this.btnAllSlotSet.Size = new System.Drawing.Size(59, 47);
            this.btnAllSlotSet.TabIndex = 10;
            this.btnAllSlotSet.Text = "All Slot Set";
            this.btnAllSlotSet.UseVisualStyleBackColor = true;
            this.btnAllSlotSet.Click += new System.EventHandler(this.btnAllSlotSet_Click);
            // 
            // cbThickness
            // 
            this.cbThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThickness.FormattingEnabled = true;
            this.cbThickness.Items.AddRange(new object[] {
            "1.1 mm",
            "0.7 mm",
            "0.6 mm",
            "0.5 mm"});
            this.cbThickness.Location = new System.Drawing.Point(8, 31);
            this.cbThickness.Name = "cbThickness";
            this.cbThickness.Size = new System.Drawing.Size(123, 23);
            this.cbThickness.TabIndex = 10;
            this.cbThickness.SelectedIndexChanged += new System.EventHandler(this.cbThickness_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Thickness";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblRecipeID);
            this.groupBox4.Controls.Add(this.btnCopyGreen);
            this.groupBox4.Controls.Add(this.btnAllSlotCopy);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(411, 91);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(305, 64);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            // 
            // lblRecipeID
            // 
            this.lblRecipeID.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblRecipeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRecipeID.Location = new System.Drawing.Point(5, 30);
            this.lblRecipeID.Name = "lblRecipeID";
            this.lblRecipeID.Size = new System.Drawing.Size(147, 24);
            this.lblRecipeID.TabIndex = 12;
            this.lblRecipeID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRecipeID.Click += new System.EventHandler(this.lblRecipeID_Click);
            // 
            // btnCopyGreen
            // 
            this.btnCopyGreen.Location = new System.Drawing.Point(218, 13);
            this.btnCopyGreen.Name = "btnCopyGreen";
            this.btnCopyGreen.Size = new System.Drawing.Size(82, 47);
            this.btnCopyGreen.TabIndex = 10;
            this.btnCopyGreen.Text = "Copy to Green Slot";
            this.btnCopyGreen.UseVisualStyleBackColor = true;
            this.btnCopyGreen.Click += new System.EventHandler(this.btnCopyGreen_Click);
            // 
            // btnAllSlotCopy
            // 
            this.btnAllSlotCopy.Location = new System.Drawing.Point(157, 13);
            this.btnAllSlotCopy.Name = "btnAllSlotCopy";
            this.btnAllSlotCopy.Size = new System.Drawing.Size(59, 47);
            this.btnAllSlotCopy.TabIndex = 10;
            this.btnAllSlotCopy.Text = "All Slot Copy";
            this.btnAllSlotCopy.UseVisualStyleBackColor = true;
            this.btnAllSlotCopy.Click += new System.EventHandler(this.btnAllSlotCopy_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Lot Recipe ID";
            // 
            // DlgInputID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(725, 617);
            this.ControlBox = false;
            this.Controls.Add(this.btnAllSlotSelect);
            this.Controls.Add(this.dataGridViewInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPortID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgInputID";
            this.Text = "ID Input";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInput)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPortID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewInput;
        private System.Windows.Forms.Button btnAllSlotSelect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbThickness;
        private System.Windows.Forms.Button btnAllSlotSet;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAllSlotCopy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCopyGreen;
        private System.Windows.Forms.ComboBox cbCassettetype;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCstID;
        private System.Windows.Forms.Label lblLotID;
        private System.Windows.Forms.Label lblSheetID;
        private System.Windows.Forms.Label lblRecipeID;
    }
}
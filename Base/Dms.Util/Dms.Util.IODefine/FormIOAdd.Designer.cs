namespace Dms.Util.IODefine
{
    partial class FormIOAdd
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
			this.listBoxTerminalTypes = new System.Windows.Forms.ListBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.comboBoxCount = new System.Windows.Forms.ComboBox();
			this.comboBoxIoType = new System.Windows.Forms.ComboBox();
			this.labelBusType = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listBoxTerminalTypes
			// 
			this.listBoxTerminalTypes.FormattingEnabled = true;
			this.listBoxTerminalTypes.ItemHeight = 15;
			this.listBoxTerminalTypes.Location = new System.Drawing.Point(12, 66);
			this.listBoxTerminalTypes.Name = "listBoxTerminalTypes";
			this.listBoxTerminalTypes.Size = new System.Drawing.Size(173, 409);
			this.listBoxTerminalTypes.Sorted = true;
			this.listBoxTerminalTypes.TabIndex = 0;
			this.listBoxTerminalTypes.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(279, 442);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(173, 33);
			this.buttonAdd.TabIndex = 1;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(458, 442);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(170, 33);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Enabled = false;
			this.propertyGrid1.Location = new System.Drawing.Point(191, 36);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(437, 400);
			this.propertyGrid1.TabIndex = 3;
			// 
			// comboBoxCount
			// 
			this.comboBoxCount.FormattingEnabled = true;
			this.comboBoxCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
			this.comboBoxCount.Location = new System.Drawing.Point(191, 448);
			this.comboBoxCount.Name = "comboBoxCount";
			this.comboBoxCount.Size = new System.Drawing.Size(82, 23);
			this.comboBoxCount.TabIndex = 4;
			// 
			// comboBoxIoType
			// 
			this.comboBoxIoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxIoType.FormattingEnabled = true;
			this.comboBoxIoType.Location = new System.Drawing.Point(12, 37);
			this.comboBoxIoType.Name = "comboBoxIoType";
			this.comboBoxIoType.Size = new System.Drawing.Size(173, 23);
			this.comboBoxIoType.TabIndex = 5;
			this.comboBoxIoType.SelectedIndexChanged += new System.EventHandler(this.comboBoxIoType_SelectedIndexChanged);
			// 
			// labelBusType
			// 
			this.labelBusType.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.labelBusType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelBusType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBusType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelBusType.Location = new System.Drawing.Point(12, 9);
			this.labelBusType.Name = "labelBusType";
			this.labelBusType.Size = new System.Drawing.Size(616, 23);
			this.labelBusType.TabIndex = 11;
			this.labelBusType.Text = " BusType";
			this.labelBusType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormIOAdd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(640, 484);
			this.ControlBox = false;
			this.Controls.Add(this.labelBusType);
			this.Controls.Add(this.comboBoxIoType);
			this.Controls.Add(this.comboBoxCount);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.listBoxTerminalTypes);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormIOAdd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormIOAdd";
			this.Load += new System.EventHandler(this.FormIOAdd_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTerminalTypes;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.ComboBox comboBoxIoType;
        private System.Windows.Forms.Label labelBusType;
    }
}
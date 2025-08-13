
namespace Dms.Util.IODefine
{
    partial class FormSlaveAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelBusType = new System.Windows.Forms.Label();
            this.comboBoxSlaveType = new System.Windows.Forms.ComboBox();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxSlaveTypes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
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
            this.labelBusType.TabIndex = 18;
            this.labelBusType.Text = " BusType";
            this.labelBusType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxSlaveType
            // 
            this.comboBoxSlaveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSlaveType.FormattingEnabled = true;
            this.comboBoxSlaveType.Location = new System.Drawing.Point(12, 37);
            this.comboBoxSlaveType.Name = "comboBoxSlaveType";
            this.comboBoxSlaveType.Size = new System.Drawing.Size(173, 23);
            this.comboBoxSlaveType.TabIndex = 17;
            this.comboBoxSlaveType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSlaveType_SelectedIndexChanged);
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
            this.comboBoxCount.TabIndex = 16;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Enabled = false;
            this.propertyGrid1.Location = new System.Drawing.Point(191, 36);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(437, 400);
            this.propertyGrid1.TabIndex = 15;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(458, 442);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(170, 33);
            this.buttonClose.TabIndex = 14;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(279, 442);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(173, 33);
            this.buttonAdd.TabIndex = 13;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxSlaveTypes
            // 
            this.listBoxSlaveTypes.FormattingEnabled = true;
            this.listBoxSlaveTypes.ItemHeight = 15;
            this.listBoxSlaveTypes.Location = new System.Drawing.Point(12, 66);
            this.listBoxSlaveTypes.Name = "listBoxSlaveTypes";
            this.listBoxSlaveTypes.Size = new System.Drawing.Size(173, 409);
            this.listBoxSlaveTypes.Sorted = true;
            this.listBoxSlaveTypes.TabIndex = 12;
            this.listBoxSlaveTypes.SelectedIndexChanged += new System.EventHandler(this.listBoxSlaveTypes_SelectedIndexChanged);
            // 
            // FormSlaveAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(640, 484);
            this.ControlBox = false;
            this.Controls.Add(this.labelBusType);
            this.Controls.Add(this.comboBoxSlaveType);
            this.Controls.Add(this.comboBoxCount);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxSlaveTypes);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSlaveAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSlaveAdd";
            this.Load += new System.EventHandler(this.FormSlaveAdd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBusType;
        private System.Windows.Forms.ComboBox comboBoxSlaveType;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxSlaveTypes;
    }
}

namespace Dms.Device
{
    partial class FormSlaveConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSlaveConfig));
            this.label3 = new System.Windows.Forms.Label();
            this.validationTextBoxMaxSimulateCount = new Dms.Common.ValidationTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewCurrentConfig = new System.Windows.Forms.TreeView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.viewSlaveEdit1 = new Dms.Util.IODefine.ViewSlaveEdit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 12);
            this.label3.TabIndex = 35;
            this.label3.Text = "Max Simulate Count";
            // 
            // validationTextBoxMaxSimulateCount
            // 
            this.validationTextBoxMaxSimulateCount.DataFormat = Dms.Common.OptionFormat.Digit;
            this.validationTextBoxMaxSimulateCount.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.validationTextBoxMaxSimulateCount.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("validationTextBoxMaxSimulateCount.KeyPadInfo")));
            this.validationTextBoxMaxSimulateCount.LimitHigh = "";
            this.validationTextBoxMaxSimulateCount.LimitLow = "";
            this.validationTextBoxMaxSimulateCount.Location = new System.Drawing.Point(138, 38);
            this.validationTextBoxMaxSimulateCount.Name = "validationTextBoxMaxSimulateCount";
            this.validationTextBoxMaxSimulateCount.ReferenceTag = null;
            this.validationTextBoxMaxSimulateCount.Size = new System.Drawing.Size(81, 21);
            this.validationTextBoxMaxSimulateCount.TabIndex = 34;
            this.validationTextBoxMaxSimulateCount.UsedInKeyPad = false;
            this.validationTextBoxMaxSimulateCount.TextChanged += new System.EventHandler(this.validationTextBoxMaxSimulateCount_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DarkGray;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(229, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 2);
            this.label2.TabIndex = 33;
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(238)))));
            this.buttonDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonDown.Image = global::Dms.Device.Properties.Resources.arrow_down;
            this.buttonDown.Location = new System.Drawing.Point(229, 243);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(38, 35);
            this.buttonDown.TabIndex = 32;
            this.buttonDown.UseVisualStyleBackColor = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(238)))));
            this.buttonUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonUp.Image = global::Dms.Device.Properties.Resources.arrow_up;
            this.buttonUp.Location = new System.Drawing.Point(229, 194);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(38, 35);
            this.buttonUp.TabIndex = 31;
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRemove.Location = new System.Drawing.Point(229, 130);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(38, 35);
            this.buttonRemove.TabIndex = 30;
            this.buttonRemove.Text = ">>";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 23);
            this.label1.TabIndex = 28;
            this.label1.Text = " Current Config";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewCurrentConfig
            // 
            this.treeViewCurrentConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewCurrentConfig.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewCurrentConfig.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewCurrentConfig.HideSelection = false;
            this.treeViewCurrentConfig.Location = new System.Drawing.Point(12, 65);
            this.treeViewCurrentConfig.Name = "treeViewCurrentConfig";
            this.treeViewCurrentConfig.Size = new System.Drawing.Size(207, 255);
            this.treeViewCurrentConfig.TabIndex = 27;
            this.treeViewCurrentConfig.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCurrentConfig_NodeMouseClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdd.Location = new System.Drawing.Point(229, 81);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 35);
            this.buttonAdd.TabIndex = 29;
            this.buttonAdd.Text = "<<";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(12, 330);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(69, 51);
            this.buttonClose.TabIndex = 26;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(87, 330);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(132, 51);
            this.buttonSelect.TabIndex = 25;
            this.buttonSelect.Text = "Confirm";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // viewSlaveEdit1
            // 
            this.viewSlaveEdit1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewSlaveEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewSlaveEdit1.Font = new System.Drawing.Font("Arial", 9F);
            this.viewSlaveEdit1.Location = new System.Drawing.Point(275, 12);
            this.viewSlaveEdit1.Name = "viewSlaveEdit1";
            this.viewSlaveEdit1.OperateMode = Dms.Util.IODefine.ViewSlaveEdit.OpMode.Config;
            this.viewSlaveEdit1.ShowNodeInfo = true;
            this.viewSlaveEdit1.Size = new System.Drawing.Size(485, 382);
            this.viewSlaveEdit1.TabIndex = 3;
            this.viewSlaveEdit1.TimerStateUpdateEnabled = false;
            // 
            // FormSlaveConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(772, 398);
            this.Controls.Add(this.viewSlaveEdit1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.validationTextBoxMaxSimulateCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewCurrentConfig);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSlaveConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSlaveConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private Common.ValidationTextBox validationTextBoxMaxSimulateCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewCurrentConfig;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSelect;
        private Util.IODefine.ViewSlaveEdit viewSlaveEdit1;
    }
}
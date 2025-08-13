
namespace Dms.Util.IODefine
{
    partial class FormSlaveSelect
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
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.viewSlaveEdit1 = new Dms.Util.IODefine.ViewSlaveEdit();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClear.Location = new System.Drawing.Point(572, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(93, 46);
            this.buttonClear.TabIndex = 7;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClose.Location = new System.Drawing.Point(572, 330);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(93, 46);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonSelect.Location = new System.Drawing.Point(572, 56);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(93, 268);
            this.buttonSelect.TabIndex = 5;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // viewSlaveEdit1
            // 
            this.viewSlaveEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSlaveEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewSlaveEdit1.Font = new System.Drawing.Font("Arial", 9F);
            this.viewSlaveEdit1.Location = new System.Drawing.Point(4, 4);
            this.viewSlaveEdit1.Name = "viewSlaveEdit1";
            this.viewSlaveEdit1.OperateMode = Dms.Util.IODefine.ViewSlaveEdit.OpMode.Config;
            this.viewSlaveEdit1.ShowNodeInfo = true;
            this.viewSlaveEdit1.Size = new System.Drawing.Size(560, 382);
            this.viewSlaveEdit1.TabIndex = 2;
            this.viewSlaveEdit1.TimerStateUpdateEnabled = false;
            // 
            // FormSlaveSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(675, 389);
            this.ControlBox = false;
            this.Controls.Add(this.viewSlaveEdit1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelect);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSlaveSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSlaveSelect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSelect;
        private ViewSlaveEdit viewSlaveEdit1;
    }
}
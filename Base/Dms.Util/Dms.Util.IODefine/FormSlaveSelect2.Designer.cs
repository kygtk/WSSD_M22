
namespace Dms.Util.IODefine
{
    partial class FormSlaveSelect2
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
            this.ButtonSelect = new System.Windows.Forms.Button();
            this.viewSlaveSelect21 = new Dms.Util.IODefine.ViewSlaveSelect2();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClear.Location = new System.Drawing.Point(378, 6);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(76, 46);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClose.Location = new System.Drawing.Point(378, 325);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(76, 46);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonSelect.Location = new System.Drawing.Point(378, 58);
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(76, 261);
            this.ButtonSelect.TabIndex = 8;
            this.ButtonSelect.Text = "Select";
            this.ButtonSelect.UseVisualStyleBackColor = true;
            this.ButtonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // viewSlaveSelect21
            // 
            this.viewSlaveSelect21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSlaveSelect21.BackColor = System.Drawing.Color.Transparent;
            this.viewSlaveSelect21.Font = new System.Drawing.Font("Arial", 9F);
            this.viewSlaveSelect21.Location = new System.Drawing.Point(6, 8);
            this.viewSlaveSelect21.Name = "viewSlaveSelect21";
            this.viewSlaveSelect21.Size = new System.Drawing.Size(368, 372);
            this.viewSlaveSelect21.TabIndex = 3;
            // 
            // FormSlaveSelect2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(461, 383);
            this.ControlBox = false;
            this.Controls.Add(this.viewSlaveSelect21);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.ButtonSelect);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormSlaveSelect2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSlaveSelect2";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button ButtonSelect;
        private ViewSlaveSelect2 viewSlaveSelect21;
    }
}
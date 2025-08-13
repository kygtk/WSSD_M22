
namespace Dms.Control
{
    partial class DlgDoorLock
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
            this.components = new System.ComponentModel.Container();
            this.gbButton = new System.Windows.Forms.GroupBox();
            this.btnLock = new System.Windows.Forms.RadioButton();
            this.btnUnlock = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.timUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbButton
            // 
            this.gbButton.Controls.Add(this.btnLock);
            this.gbButton.Controls.Add(this.btnUnlock);
            this.gbButton.Location = new System.Drawing.Point(7, 6);
            this.gbButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbButton.Name = "gbButton";
            this.gbButton.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbButton.Size = new System.Drawing.Size(227, 88);
            this.gbButton.TabIndex = 2;
            this.gbButton.TabStop = false;
            this.gbButton.Text = "Door Lock/Unlock";
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnLock.BackColor = System.Drawing.Color.Pink;
            this.btnLock.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLock.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLock.Location = new System.Drawing.Point(114, 24);
            this.btnLock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(97, 50);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "Lock";
            this.btnLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLock.UseVisualStyleBackColor = false;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnlock.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnUnlock.BackColor = System.Drawing.Color.LightCyan;
            this.btnUnlock.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUnlock.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnlock.Location = new System.Drawing.Point(14, 24);
            this.btnUnlock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(97, 50);
            this.btnUnlock.TabIndex = 0;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUnlock.UseVisualStyleBackColor = false;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(136, 102);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 40);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // timUpdateState
            // 
            this.timUpdateState.Enabled = true;
            this.timUpdateState.Interval = 300;
            this.timUpdateState.Tick += new System.EventHandler(this.timUpdateState_Tick);
            // 
            // DlgDoorLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(242, 148);
            this.ControlBox = false;
            this.Controls.Add(this.gbButton);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDoorLock";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.DlgDoorLock_Load);
            this.gbButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbButton;
        private System.Windows.Forms.RadioButton btnLock;
        private System.Windows.Forms.RadioButton btnUnlock;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Timer timUpdateState;
    }
}
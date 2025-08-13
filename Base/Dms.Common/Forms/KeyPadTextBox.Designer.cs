namespace Dms.Common
{
    partial class KeyPadTextBox
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
            this.lblOldVal = new System.Windows.Forms.Label();
            this.txtOldVal = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNewVal = new System.Windows.Forms.Label();
            this.txtNewVal = new Dms.Common.ValidationTextBox();
            this.lblForamtTitle = new System.Windows.Forms.Label();
            this.lblFormat = new System.Windows.Forms.Label();
            this.lblRange = new System.Windows.Forms.Label();
            this.lblRangeTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblOldVal
            // 
            this.lblOldVal.Location = new System.Drawing.Point(2, 39);
            this.lblOldVal.Name = "lblOldVal";
            this.lblOldVal.Size = new System.Drawing.Size(75, 20);
            this.lblOldVal.TabIndex = 0;
            this.lblOldVal.Text = "Old Value :";
            this.lblOldVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOldVal
            // 
            this.txtOldVal.Location = new System.Drawing.Point(83, 39);
            this.txtOldVal.Name = "txtOldVal";
            this.txtOldVal.ReadOnly = true;
            this.txtOldVal.Size = new System.Drawing.Size(100, 21);
            this.txtOldVal.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.LightCyan;
            this.btnOk.Location = new System.Drawing.Point(196, 28);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 36);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnCancel.Location = new System.Drawing.Point(196, 69);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblNewVal
            // 
            this.lblNewVal.Location = new System.Drawing.Point(2, 71);
            this.lblNewVal.Name = "lblNewVal";
            this.lblNewVal.Size = new System.Drawing.Size(75, 20);
            this.lblNewVal.TabIndex = 0;
            this.lblNewVal.Text = "New Value :";
            this.lblNewVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewVal
            // 
            this.txtNewVal.DataFormat = Dms.Common.OptionFormat.None;
            this.txtNewVal.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtNewVal.KeyPadInfo = null;
            this.txtNewVal.LimitHigh = "";
            this.txtNewVal.LimitLow = "";
            this.txtNewVal.Location = new System.Drawing.Point(83, 71);
            this.txtNewVal.Name = "txtNewVal";
            this.txtNewVal.Size = new System.Drawing.Size(100, 21);
            this.txtNewVal.TabIndex = 2;
            // 
            // lblForamtTitle
            // 
            this.lblForamtTitle.Location = new System.Drawing.Point(2, 9);
            this.lblForamtTitle.Name = "lblForamtTitle";
            this.lblForamtTitle.Size = new System.Drawing.Size(75, 20);
            this.lblForamtTitle.TabIndex = 5;
            this.lblForamtTitle.Text = "Foramt :";
            this.lblForamtTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFormat
            // 
            this.lblFormat.Location = new System.Drawing.Point(80, 9);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(196, 20);
            this.lblFormat.TabIndex = 5;
            this.lblFormat.Text = "Data Format";
            this.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRange
            // 
            this.lblRange.Location = new System.Drawing.Point(80, 100);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(196, 20);
            this.lblRange.TabIndex = 7;
            this.lblRange.Text = "Data Range";
            this.lblRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRange.Visible = false;
            // 
            // lblRangeTitle
            // 
            this.lblRangeTitle.Location = new System.Drawing.Point(2, 100);
            this.lblRangeTitle.Name = "lblRangeTitle";
            this.lblRangeTitle.Size = new System.Drawing.Size(75, 20);
            this.lblRangeTitle.TabIndex = 6;
            this.lblRangeTitle.Text = "Range :";
            this.lblRangeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRangeTitle.Visible = false;
            // 
            // KeyPadTextBox
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Caption = "Edit";
            this.ClientSize = new System.Drawing.Size(288, 128);
            this.ControlBox = false;
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.lblForamtTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtNewVal);
            this.Controls.Add(this.txtOldVal);
            this.Controls.Add(this.lblNewVal);
            this.Controls.Add(this.lblOldVal);
            this.Controls.Add(this.lblRange);
            this.Controls.Add(this.lblRangeTitle);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyPadTextBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit";
            this.Load += new System.EventHandler(this.KeyPadTextBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label lblOldVal;
        private System.Windows.Forms.TextBox txtOldVal;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNewVal;
        private Dms.Common.ValidationTextBox txtNewVal;
        private System.Windows.Forms.Label lblForamtTitle;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.Label lblRangeTitle;
    }
}
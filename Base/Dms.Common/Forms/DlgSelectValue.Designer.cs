namespace Dms.Common
{
    partial class DlgSelectValue
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
            this.lblFrom = new System.Windows.Forms.Label();
            this.txtCurValue = new Dms.Common.ValidationTextBox();
            this.cboNewValue = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(2, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(67, 20);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "FROM :";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCurValue
            // 
            this.txtCurValue.Location = new System.Drawing.Point(75, 17);
            this.txtCurValue.Name = "txtCurValue";
            this.txtCurValue.ReadOnly = true;
            this.txtCurValue.Size = new System.Drawing.Size(100, 21);
            this.txtCurValue.TabIndex = 2;
            // 
            // cboNewValue
            // 
            this.cboNewValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewValue.FormattingEnabled = true;
            this.cboNewValue.Location = new System.Drawing.Point(75, 58);
            this.cboNewValue.Name = "cboNewValue";
            this.cboNewValue.Size = new System.Drawing.Size(100, 23);
            this.cboNewValue.TabIndex = 1;
            this.cboNewValue.SelectedIndexChanged += new System.EventHandler(this.cboCurId_SelectedIndexChanged);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.LightCyan;
            this.btnOk.Location = new System.Drawing.Point(188, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 36);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnCancel.Location = new System.Drawing.Point(188, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(2, 61);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(67, 20);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "TO :";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DlgSelectValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(288, 106);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cboNewValue);
            this.Controls.Add(this.txtCurValue);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSelectValue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select";
            this.Load += new System.EventHandler(this.DlgRecipeId_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFrom;
        private Dms.Common.ValidationTextBox txtCurValue;
        private System.Windows.Forms.ComboBox cboNewValue;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTo;
    }
}
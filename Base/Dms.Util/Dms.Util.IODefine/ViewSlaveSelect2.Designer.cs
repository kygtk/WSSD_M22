
namespace Dms.Util.IODefine
{
    partial class ViewSlaveSelect2
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxAutoFind = new System.Windows.Forms.CheckBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkBoxAutoFind
            // 
            this.checkBoxAutoFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoFind.AutoSize = true;
            this.checkBoxAutoFind.Location = new System.Drawing.Point(437, 7);
            this.checkBoxAutoFind.Name = "checkBoxAutoFind";
            this.checkBoxAutoFind.Size = new System.Drawing.Size(50, 19);
            this.checkBoxAutoFind.TabIndex = 7;
            this.checkBoxAutoFind.Text = "Auto";
            this.checkBoxAutoFind.UseVisualStyleBackColor = true;
            this.checkBoxAutoFind.CheckedChanged += new System.EventHandler(this.checkBoxAutoFind_CheckedChanged);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(356, 4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 15;
            this.listBox.Location = new System.Drawing.Point(4, 31);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(497, 259);
            this.listBox.TabIndex = 5;
            this.listBox.SelectedValueChanged += new System.EventHandler(this.listBox_SelectedValueChanged);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(4, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(348, 21);
            this.txtName.TabIndex = 4;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // ViewSlaveSelect2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.checkBoxAutoFind);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "ViewSlaveSelect2";
            this.Size = new System.Drawing.Size(505, 295);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxAutoFind;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.TextBox txtName;
    }
}

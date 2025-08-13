namespace Dms.Cim.Common
{
    partial class DlgConfirmGlassInfo
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
            this.cbPortNo = new System.Windows.Forms.ComboBox();
            this.lblDisplay1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSlotNo = new System.Windows.Forms.ComboBox();
            this.lblDisplay2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbPortNo
            // 
            this.cbPortNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPortNo.FormattingEnabled = true;
            this.cbPortNo.Location = new System.Drawing.Point(100, 12);
            this.cbPortNo.Name = "cbPortNo";
            this.cbPortNo.Size = new System.Drawing.Size(121, 23);
            this.cbPortNo.TabIndex = 8;
            // 
            // lblDisplay1
            // 
            this.lblDisplay1.BackColor = System.Drawing.Color.Red;
            this.lblDisplay1.Location = new System.Drawing.Point(27, 78);
            this.lblDisplay1.Name = "lblDisplay1";
            this.lblDisplay1.Size = new System.Drawing.Size(220, 23);
            this.lblDisplay1.TabIndex = 7;
            this.lblDisplay1.Text = "Select Port No, Slot No Into Cassette";
            this.lblDisplay1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(80, 131);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(131, 53);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "PORT NO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(27, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "SLOT NO";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSlotNo
            // 
            this.cbSlotNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSlotNo.FormattingEnabled = true;
            this.cbSlotNo.Location = new System.Drawing.Point(100, 41);
            this.cbSlotNo.Name = "cbSlotNo";
            this.cbSlotNo.Size = new System.Drawing.Size(121, 23);
            this.cbSlotNo.TabIndex = 8;
            // 
            // lblDisplay2
            // 
            this.lblDisplay2.BackColor = System.Drawing.Color.Red;
            this.lblDisplay2.Location = new System.Drawing.Point(27, 101);
            this.lblDisplay2.Name = "lblDisplay2";
            this.lblDisplay2.Size = new System.Drawing.Size(220, 23);
            this.lblDisplay2.TabIndex = 7;
            this.lblDisplay2.Text = "Confirm the empty glass in cassette";
            this.lblDisplay2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DlgConfirmGlassInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(277, 192);
            this.Controls.Add(this.cbSlotNo);
            this.Controls.Add(this.cbPortNo);
            this.Controls.Add(this.lblDisplay2);
            this.Controls.Add(this.lblDisplay1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DlgConfirmGlassInfo";
            this.Text = "Glass Infomation Confirm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPortNo;
        private System.Windows.Forms.Label lblDisplay1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSlotNo;
        private System.Windows.Forms.Label lblDisplay2;
    }
}
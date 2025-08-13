namespace Dms.Control
{
    partial class DlgMfc
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
            this.tbPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gbIndividualOp = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbIndividualOp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPanel
            // 
            this.tbPanel.AutoScroll = true;
            this.tbPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tbPanel.ColumnCount = 1;
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbPanel.Location = new System.Drawing.Point(7, 21);
            this.tbPanel.Name = "tbPanel";
            this.tbPanel.RowCount = 1;
            this.tbPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbPanel.Size = new System.Drawing.Size(586, 229);
            this.tbPanel.TabIndex = 1;
            // 
            // gbIndividualOp
            // 
            this.gbIndividualOp.Controls.Add(this.tbPanel);
            this.gbIndividualOp.Location = new System.Drawing.Point(9, 5);
            this.gbIndividualOp.Name = "gbIndividualOp";
            this.gbIndividualOp.Size = new System.Drawing.Size(601, 258);
            this.gbIndividualOp.TabIndex = 5;
            this.gbIndividualOp.TabStop = false;
            this.gbIndividualOp.Text = "Individual Operation";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(519, 269);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(91, 47);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // DlgMfc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(615, 321);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gbIndividualOp);
            this.Name = "DlgMfc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MFC Manual Operation";
            this.gbIndividualOp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbPanel;
        private System.Windows.Forms.GroupBox gbIndividualOp;
        private System.Windows.Forms.Button btnOk;
    }
}
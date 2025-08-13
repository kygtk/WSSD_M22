
namespace Dms.Control
{
    partial class LCT
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
            this.picPairing = new System.Windows.Forms.PictureBox();
            this.lblLevel1 = new System.Windows.Forms.Label();
            this.lblLevel2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblConsistence = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPairing)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picPairing
            // 
            this.picPairing.BackColor = System.Drawing.Color.LightGreen;
            this.picPairing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPairing.Dock = System.Windows.Forms.DockStyle.Left;
            this.picPairing.Location = new System.Drawing.Point(0, 0);
            this.picPairing.Margin = new System.Windows.Forms.Padding(0);
            this.picPairing.Name = "picPairing";
            this.picPairing.Size = new System.Drawing.Size(10, 75);
            this.picPairing.TabIndex = 5;
            this.picPairing.TabStop = false;
            // 
            // lblLevel1
            // 
            this.lblLevel1.BackColor = System.Drawing.Color.White;
            this.lblLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLevel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel1.Location = new System.Drawing.Point(1, 1);
            this.lblLevel1.Margin = new System.Windows.Forms.Padding(1);
            this.lblLevel1.Name = "lblLevel1";
            this.lblLevel1.Size = new System.Drawing.Size(94, 22);
            this.lblLevel1.TabIndex = 1;
            this.lblLevel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel2
            // 
            this.lblLevel2.BackColor = System.Drawing.Color.White;
            this.lblLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLevel2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel2.Location = new System.Drawing.Point(1, 25);
            this.lblLevel2.Margin = new System.Windows.Forms.Padding(1);
            this.lblLevel2.Name = "lblLevel2";
            this.lblLevel2.Size = new System.Drawing.Size(94, 22);
            this.lblLevel2.TabIndex = 2;
            this.lblLevel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblConsistence, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblLevel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLevel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(96, 75);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lblConsistence
            // 
            this.lblConsistence.BackColor = System.Drawing.Color.White;
            this.lblConsistence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConsistence.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConsistence.Location = new System.Drawing.Point(1, 49);
            this.lblConsistence.Margin = new System.Windows.Forms.Padding(1);
            this.lblConsistence.Name = "lblConsistence";
            this.lblConsistence.Size = new System.Drawing.Size(94, 25);
            this.lblConsistence.TabIndex = 3;
            this.lblConsistence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LCT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.picPairing);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LCT";
            this.Size = new System.Drawing.Size(96, 75);
            ((System.ComponentModel.ISupportInitialize)(this.picPairing)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picPairing;
        private System.Windows.Forms.Label lblLevel1;
        private System.Windows.Forms.Label lblLevel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblConsistence;
    }
}

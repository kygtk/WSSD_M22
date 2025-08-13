
namespace Dms.Control
{
    partial class SmartDamper
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblValue3 = new System.Windows.Forms.Label();
            this.lblValue5 = new System.Windows.Forms.Label();
            this.lblValue2 = new System.Windows.Forms.Label();
            this.lblValue0 = new System.Windows.Forms.Label();
            this.lblValue4 = new System.Windows.Forms.Label();
            this.lblValue1 = new System.Windows.Forms.Label();
            this.picPairing = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPairing)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblValue3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblValue5, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblValue2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblValue0, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblValue4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblValue1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(138, 75);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblValue3
            // 
            this.lblValue3.BackColor = System.Drawing.Color.White;
            this.lblValue3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue3.Location = new System.Drawing.Point(70, 1);
            this.lblValue3.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue3.Name = "lblValue3";
            this.lblValue3.Size = new System.Drawing.Size(67, 23);
            this.lblValue3.TabIndex = 5;
            this.lblValue3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue5
            // 
            this.lblValue5.BackColor = System.Drawing.Color.White;
            this.lblValue5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue5.Location = new System.Drawing.Point(70, 51);
            this.lblValue5.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue5.Name = "lblValue5";
            this.lblValue5.Size = new System.Drawing.Size(67, 23);
            this.lblValue5.TabIndex = 5;
            this.lblValue5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue2
            // 
            this.lblValue2.BackColor = System.Drawing.Color.White;
            this.lblValue2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue2.Location = new System.Drawing.Point(1, 51);
            this.lblValue2.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(67, 23);
            this.lblValue2.TabIndex = 4;
            this.lblValue2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue0
            // 
            this.lblValue0.BackColor = System.Drawing.Color.White;
            this.lblValue0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue0.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue0.Location = new System.Drawing.Point(1, 1);
            this.lblValue0.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue0.Name = "lblValue0";
            this.lblValue0.Size = new System.Drawing.Size(67, 23);
            this.lblValue0.TabIndex = 3;
            this.lblValue0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue4
            // 
            this.lblValue4.BackColor = System.Drawing.Color.White;
            this.lblValue4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue4.Location = new System.Drawing.Point(70, 26);
            this.lblValue4.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue4.Name = "lblValue4";
            this.lblValue4.Size = new System.Drawing.Size(67, 23);
            this.lblValue4.TabIndex = 5;
            this.lblValue4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValue1
            // 
            this.lblValue1.BackColor = System.Drawing.Color.White;
            this.lblValue1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValue1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue1.Location = new System.Drawing.Point(1, 26);
            this.lblValue1.Margin = new System.Windows.Forms.Padding(1);
            this.lblValue1.Name = "lblValue1";
            this.lblValue1.Size = new System.Drawing.Size(67, 23);
            this.lblValue1.TabIndex = 5;
            this.lblValue1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picPairing
            // 
            this.picPairing.BackColor = System.Drawing.Color.LightGreen;
            this.picPairing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPairing.Dock = System.Windows.Forms.DockStyle.Left;
            this.picPairing.Location = new System.Drawing.Point(0, 0);
            this.picPairing.Name = "picPairing";
            this.picPairing.Size = new System.Drawing.Size(10, 75);
            this.picPairing.TabIndex = 4;
            this.picPairing.TabStop = false;
            // 
            // SmartDamper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picPairing);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "SmartDamper";
            this.Size = new System.Drawing.Size(148, 75);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPairing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox picPairing;
        private System.Windows.Forms.Label lblValue3;
        private System.Windows.Forms.Label lblValue5;
        private System.Windows.Forms.Label lblValue2;
        private System.Windows.Forms.Label lblValue0;
        private System.Windows.Forms.Label lblValue4;
        private System.Windows.Forms.Label lblValue1;
    }
}

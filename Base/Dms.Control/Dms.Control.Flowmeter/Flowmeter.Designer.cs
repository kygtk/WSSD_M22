
namespace Dms.Control
{
    partial class Flowmeter
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
            this.lblPress = new System.Windows.Forms.Label();
            this.lblFlow = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            this.picPairing.Size = new System.Drawing.Size(10, 52);
            this.picPairing.TabIndex = 3;
            this.picPairing.TabStop = false;
            // 
            // lblPress
            // 
            this.lblPress.BackColor = System.Drawing.Color.White;
            this.lblPress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPress.Location = new System.Drawing.Point(1, 27);
            this.lblPress.Margin = new System.Windows.Forms.Padding(1);
            this.lblPress.Name = "lblPress";
            this.lblPress.Size = new System.Drawing.Size(86, 24);
            this.lblPress.TabIndex = 2;
            this.lblPress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFlow
            // 
            this.lblFlow.BackColor = System.Drawing.Color.White;
            this.lblFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFlow.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlow.Location = new System.Drawing.Point(1, 1);
            this.lblFlow.Margin = new System.Windows.Forms.Padding(1);
            this.lblFlow.Name = "lblFlow";
            this.lblFlow.Size = new System.Drawing.Size(86, 24);
            this.lblFlow.TabIndex = 1;
            this.lblFlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblFlow, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPress, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(88, 52);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // Flowmeter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picPairing);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Flowmeter";
            this.Size = new System.Drawing.Size(98, 52);
            ((System.ComponentModel.ISupportInitialize)(this.picPairing)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picPairing;
        private System.Windows.Forms.Label lblPress;
        private System.Windows.Forms.Label lblFlow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

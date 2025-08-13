namespace Dms.Control
{
    partial class DlgGlassInfo
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridView1 = new Dms.Control.DoubleBufferedGridView();
            this.btnRequest = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.groupBoxCim = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.groupBoxCim.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCyan;
            this.btnDelete.Location = new System.Drawing.Point(14, 20);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(124, 39);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnMove);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Location = new System.Drawing.Point(316, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 156);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glass Data Function";
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnMove.Location = new System.Drawing.Point(14, 63);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(124, 39);
            this.btnMove.TabIndex = 0;
            this.btnMove.Text = "MOVE";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Linen;
            this.btnOk.Location = new System.Drawing.Point(393, 300);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 49);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridView1);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 336);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Glass Information";
            // 
            // gridView1
            // 
            this.gridView1.AllowUserToAddRows = false;
            this.gridView1.AllowUserToDeleteRows = false;
            this.gridView1.AllowUserToResizeColumns = false;
            this.gridView1.AllowUserToResizeRows = false;
            this.gridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView1.Location = new System.Drawing.Point(7, 20);
            this.gridView1.MultiSelect = false;
            this.gridView1.Name = "gridView1";
            this.gridView1.ReadOnly = true;
            this.gridView1.RowHeadersVisible = false;
            this.gridView1.RowTemplate.Height = 23;
            this.gridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView1.ShowEditingIcon = false;
            this.gridView1.Size = new System.Drawing.Size(278, 310);
            this.gridView1.TabIndex = 0;
            this.gridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridView1_CellMouseDoubleClick);
            // 
            // btnRequest
            // 
            this.btnRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnRequest.Location = new System.Drawing.Point(14, 23);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(124, 39);
            this.btnRequest.TabIndex = 1;
            this.btnRequest.Text = "DATA REQUEST";
            this.btnRequest.UseVisualStyleBackColor = false;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnReport.Location = new System.Drawing.Point(14, 66);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(124, 39);
            this.btnReport.TabIndex = 2;
            this.btnReport.Text = "DATA  REPORT";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // groupBoxCim
            // 
            this.groupBoxCim.Controls.Add(this.btnRequest);
            this.groupBoxCim.Controls.Add(this.btnReport);
            this.groupBoxCim.Location = new System.Drawing.Point(316, 174);
            this.groupBoxCim.Name = "groupBoxCim";
            this.groupBoxCim.Size = new System.Drawing.Size(153, 120);
            this.groupBoxCim.TabIndex = 4;
            this.groupBoxCim.TabStop = false;
            this.groupBoxCim.Text = "CIM Function";
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnEdit.Location = new System.Drawing.Point(14, 106);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(124, 39);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "EDIT";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // DlgGlassInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(480, 360);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxCim);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgGlassInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Glass Information";
            this.Load += new System.EventHandler(this.DlgGlassInfo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.groupBoxCim.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private DoubleBufferedGridView gridView1;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.GroupBox groupBoxCim;
        private System.Windows.Forms.Button btnEdit;
    }
}
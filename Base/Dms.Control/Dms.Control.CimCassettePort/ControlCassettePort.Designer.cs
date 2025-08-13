namespace Dms.Control
{
    partial class ControlCassettePort
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textCassetteID = new System.Windows.Forms.TextBox();
            this.textRecipeID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textProcID = new System.Windows.Forms.TextBox();
            this.textOpID = new System.Windows.Forms.TextBox();
            this.textWaitTime = new System.Windows.Forms.TextBox();
            this.textLotID = new System.Windows.Forms.TextBox();
            this.textMqcFlag = new System.Windows.Forms.TextBox();
            this.textCassetteType = new System.Windows.Forms.TextBox();
            this.buttonAbortCancel = new System.Windows.Forms.Button();
            this.buttonManualStart = new System.Windows.Forms.Button();
            this.checkSelectAll = new System.Windows.Forms.CheckBox();
            this.buttonInputID = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 95);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(721, 200);
            this.dataGridView1.TabIndex = 0;
            // 
            // textCassetteID
            // 
            this.textCassetteID.Location = new System.Drawing.Point(90, 9);
            this.textCassetteID.Name = "textCassetteID";
            this.textCassetteID.ReadOnly = true;
            this.textCassetteID.Size = new System.Drawing.Size(138, 21);
            this.textCassetteID.TabIndex = 1;
            // 
            // textRecipeID
            // 
            this.textRecipeID.Location = new System.Drawing.Point(304, 9);
            this.textRecipeID.Name = "textRecipeID";
            this.textRecipeID.ReadOnly = true;
            this.textRecipeID.Size = new System.Drawing.Size(138, 21);
            this.textRecipeID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cassette ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Recipe ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "OP ID";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(491, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Wait Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "PROC ID";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(515, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Lot ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(236, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "MQC Flag";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(464, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Cassette Type";
            // 
            // textProcID
            // 
            this.textProcID.Location = new System.Drawing.Point(90, 36);
            this.textProcID.Name = "textProcID";
            this.textProcID.ReadOnly = true;
            this.textProcID.Size = new System.Drawing.Size(138, 21);
            this.textProcID.TabIndex = 1;
            // 
            // textOpID
            // 
            this.textOpID.Location = new System.Drawing.Point(304, 36);
            this.textOpID.Name = "textOpID";
            this.textOpID.ReadOnly = true;
            this.textOpID.Size = new System.Drawing.Size(138, 21);
            this.textOpID.TabIndex = 1;
            // 
            // textWaitTime
            // 
            this.textWaitTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textWaitTime.Location = new System.Drawing.Point(559, 9);
            this.textWaitTime.Name = "textWaitTime";
            this.textWaitTime.ReadOnly = true;
            this.textWaitTime.Size = new System.Drawing.Size(178, 21);
            this.textWaitTime.TabIndex = 1;
            // 
            // textLotID
            // 
            this.textLotID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textLotID.Location = new System.Drawing.Point(559, 36);
            this.textLotID.Name = "textLotID";
            this.textLotID.ReadOnly = true;
            this.textLotID.Size = new System.Drawing.Size(178, 21);
            this.textLotID.TabIndex = 1;
            // 
            // textMqcFlag
            // 
            this.textMqcFlag.Location = new System.Drawing.Point(304, 63);
            this.textMqcFlag.Name = "textMqcFlag";
            this.textMqcFlag.ReadOnly = true;
            this.textMqcFlag.Size = new System.Drawing.Size(138, 21);
            this.textMqcFlag.TabIndex = 1;
            // 
            // textCassetteType
            // 
            this.textCassetteType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textCassetteType.Location = new System.Drawing.Point(559, 63);
            this.textCassetteType.Name = "textCassetteType";
            this.textCassetteType.ReadOnly = true;
            this.textCassetteType.Size = new System.Drawing.Size(178, 21);
            this.textCassetteType.TabIndex = 1;
            // 
            // buttonAbortCancel
            // 
            this.buttonAbortCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAbortCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAbortCancel.Location = new System.Drawing.Point(112, 300);
            this.buttonAbortCancel.Name = "buttonAbortCancel";
            this.buttonAbortCancel.Size = new System.Drawing.Size(92, 43);
            this.buttonAbortCancel.TabIndex = 3;
            this.buttonAbortCancel.Text = "Abort/Cancel";
            this.buttonAbortCancel.UseVisualStyleBackColor = true;
            this.buttonAbortCancel.Click += new System.EventHandler(this.buttonAbortCancel_Click);
            // 
            // buttonManualStart
            // 
            this.buttonManualStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonManualStart.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonManualStart.Location = new System.Drawing.Point(14, 300);
            this.buttonManualStart.Name = "buttonManualStart";
            this.buttonManualStart.Size = new System.Drawing.Size(92, 43);
            this.buttonManualStart.TabIndex = 3;
            this.buttonManualStart.Text = "Manual Process Start";
            this.buttonManualStart.UseVisualStyleBackColor = true;
            this.buttonManualStart.Click += new System.EventHandler(this.buttonManualStart_Click);
            // 
            // checkSelectAll
            // 
            this.checkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkSelectAll.AutoSize = true;
            this.checkSelectAll.Enabled = false;
            this.checkSelectAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkSelectAll.Location = new System.Drawing.Point(661, 299);
            this.checkSelectAll.Name = "checkSelectAll";
            this.checkSelectAll.Size = new System.Drawing.Size(76, 19);
            this.checkSelectAll.TabIndex = 4;
            this.checkSelectAll.Text = "Select All";
            this.checkSelectAll.UseVisualStyleBackColor = true;
            this.checkSelectAll.Visible = false;
            this.checkSelectAll.CheckedChanged += new System.EventHandler(this.checkSelectAll_CheckedChanged);
            // 
            // buttonInputID
            // 
            this.buttonInputID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInputID.Location = new System.Drawing.Point(210, 300);
            this.buttonInputID.Name = "buttonInputID";
            this.buttonInputID.Size = new System.Drawing.Size(73, 43);
            this.buttonInputID.TabIndex = 5;
            this.buttonInputID.Text = "Input ID";
            this.buttonInputID.UseVisualStyleBackColor = true;
            this.buttonInputID.Click += new System.EventHandler(this.buttonInputID_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // ControlCassettePort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.buttonInputID);
            this.Controls.Add(this.checkSelectAll);
            this.Controls.Add(this.buttonManualStart);
            this.Controls.Add(this.buttonAbortCancel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textMqcFlag);
            this.Controls.Add(this.textOpID);
            this.Controls.Add(this.textCassetteType);
            this.Controls.Add(this.textLotID);
            this.Controls.Add(this.textWaitTime);
            this.Controls.Add(this.textRecipeID);
            this.Controls.Add(this.textProcID);
            this.Controls.Add(this.textCassetteID);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlCassettePort";
            this.Size = new System.Drawing.Size(754, 346);
            this.Load += new System.EventHandler(this.ControlCassettePort_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textCassetteID;
        private System.Windows.Forms.TextBox textRecipeID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textProcID;
        private System.Windows.Forms.TextBox textOpID;
        private System.Windows.Forms.TextBox textWaitTime;
        private System.Windows.Forms.TextBox textLotID;
        private System.Windows.Forms.TextBox textMqcFlag;
        private System.Windows.Forms.TextBox textCassetteType;
        private System.Windows.Forms.Button buttonAbortCancel;
        private System.Windows.Forms.Button buttonManualStart;
        private System.Windows.Forms.CheckBox checkSelectAll;
        private System.Windows.Forms.Button buttonInputID;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}

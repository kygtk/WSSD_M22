namespace Dms.Ctl
{
    partial class Melsec_Debug_Process_Form
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewProcess = new Dms.Control.DoubleBufferedGridView();
            this.Col1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openProcessFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.btnRead = new System.Windows.Forms.Button();
            this.tmrProcess = new System.Windows.Forms.Timer(this.components);
            this.btnProcess = new System.Windows.Forms.Button();
            this.tmr_Dealy = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.btnTimerSet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnInitial = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveProcessFileDlg = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewProcess
            // 
            this.dataGridViewProcess.AllowUserToAddRows = false;
            this.dataGridViewProcess.AllowUserToDeleteRows = false;
            this.dataGridViewProcess.AllowUserToResizeColumns = false;
            this.dataGridViewProcess.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProcess.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2,
            this.Col3});
            this.dataGridViewProcess.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewProcess.Name = "dataGridViewProcess";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewProcess.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewProcess.RowHeadersWidth = 60;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewProcess.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewProcess.RowTemplate.Height = 23;
            this.dataGridViewProcess.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewProcess.Size = new System.Drawing.Size(552, 351);
            this.dataGridViewProcess.TabIndex = 0;
            // 
            // Col1
            // 
            this.Col1.HeaderText = "A";
            this.Col1.Name = "Col1";
            this.Col1.Width = 50;
            // 
            // Col2
            // 
            this.Col2.HeaderText = "B";
            this.Col2.Name = "Col2";
            this.Col2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Col2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col2.Width = 40;
            // 
            // Col3
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Col3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Col3.HeaderText = "CONTENTS";
            this.Col3.Name = "Col3";
            this.Col3.Width = 400;
            // 
            // openProcessFileDlg
            // 
            this.openProcessFileDlg.Filter = "\"Text Files (*.txt)|*.txt";
            this.openProcessFileDlg.Title = "Process 파일 열기";
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(199, 362);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(176, 27);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "READ";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // tmrProcess
            // 
            this.tmrProcess.Interval = 1000;
            this.tmrProcess.Tick += new System.EventHandler(this.tmrProcess_Tick);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(379, 362);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(176, 27);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "PROCESS";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // tmr_Dealy
            // 
            this.tmr_Dealy.Interval = 1000;
            this.tmr_Dealy.Tick += new System.EventHandler(this.tmr_Dealy_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 369);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Delay Time";
            // 
            // txtTimer
            // 
            this.txtTimer.Location = new System.Drawing.Point(84, 365);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.Size = new System.Drawing.Size(49, 21);
            this.txtTimer.TabIndex = 4;
            this.txtTimer.Text = "1";
            // 
            // btnTimerSet
            // 
            this.btnTimerSet.Location = new System.Drawing.Point(14, 396);
            this.btnTimerSet.Name = "btnTimerSet";
            this.btnTimerSet.Size = new System.Drawing.Size(151, 27);
            this.btnTimerSet.TabIndex = 5;
            this.btnTimerSet.Text = "Timer 설정";
            this.btnTimerSet.UseVisualStyleBackColor = true;
            this.btnTimerSet.Click += new System.EventHandler(this.btnTimerSet_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "sec";
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(199, 396);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(68, 27);
            this.btnPause.TabIndex = 6;
            this.btnPause.Text = "PAUSE";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnInitial
            // 
            this.btnInitial.Location = new System.Drawing.Point(271, 396);
            this.btnInitial.Name = "btnInitial";
            this.btnInitial.Size = new System.Drawing.Size(68, 27);
            this.btnInitial.TabIndex = 7;
            this.btnInitial.Text = "INITIAL";
            this.btnInitial.UseVisualStyleBackColor = true;
            this.btnInitial.Click += new System.EventHandler(this.btnInitial_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(343, 396);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 27);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(415, 396);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(68, 27);
            this.btnInsert.TabIndex = 9;
            this.btnInsert.Text = "INSERT";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(487, 395);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 27);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Melsec_Debug_Process_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 431);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnInitial);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnTimerSet);
            this.Controls.Add(this.txtTimer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.dataGridViewProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Melsec_Debug_Process_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug_Process_Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewProcess;
        private System.Windows.Forms.OpenFileDialog openProcessFileDlg;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Timer tmrProcess;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Timer tmr_Dealy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimer;
        private System.Windows.Forms.Button btnTimerSet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnInitial;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveProcessFileDlg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
    }
}
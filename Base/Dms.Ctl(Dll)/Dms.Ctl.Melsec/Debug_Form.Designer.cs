namespace Dms.Ctl
{
    partial class Melsec_Debug_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewBit = new Dms.Control.DoubleBufferedGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewWord = new Dms.Control.DoubleBufferedGridView();
            this.btn_Process = new System.Windows.Forms.Button();
            this.cbWordView = new System.Windows.Forms.ComboBox();
            this.lblWordView = new System.Windows.Forms.Label();
            this.tmMonitor = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBit)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWord)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(878, 627);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewBit);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(870, 602);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bit Address";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBit
            // 
            this.dataGridViewBit.AllowUserToAddRows = false;
            this.dataGridViewBit.AllowUserToDeleteRows = false;
            this.dataGridViewBit.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBit.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewBit.MultiSelect = false;
            this.dataGridViewBit.Name = "dataGridViewBit";
            this.dataGridViewBit.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBit.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBit.RowHeadersWidth = 50;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dataGridViewBit.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewBit.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBit.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewBit.RowTemplate.Height = 23;
            this.dataGridViewBit.Size = new System.Drawing.Size(864, 595);
            this.dataGridViewBit.TabIndex = 0;
            this.dataGridViewBit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBit_CellContentClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewWord);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(870, 602);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Word Address";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewWord
            // 
            this.dataGridViewWord.AllowUserToAddRows = false;
            this.dataGridViewWord.AllowUserToDeleteRows = false;
            this.dataGridViewWord.AllowUserToResizeColumns = false;
            this.dataGridViewWord.AllowUserToResizeRows = false;
            this.dataGridViewWord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWord.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewWord.MultiSelect = false;
            this.dataGridViewWord.Name = "dataGridViewWord";
            this.dataGridViewWord.RowHeadersWidth = 50;
            this.dataGridViewWord.RowTemplate.Height = 23;
            this.dataGridViewWord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewWord.Size = new System.Drawing.Size(864, 593);
            this.dataGridViewWord.TabIndex = 0;
            this.dataGridViewWord.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWord_CellEndEdit);
            this.dataGridViewWord.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWord_CellContentClick);
            // 
            // btn_Process
            // 
            this.btn_Process.Location = new System.Drawing.Point(12, 636);
            this.btn_Process.Name = "btn_Process";
            this.btn_Process.Size = new System.Drawing.Size(109, 26);
            this.btn_Process.TabIndex = 1;
            this.btn_Process.Text = "Process";
            this.btn_Process.UseVisualStyleBackColor = true;
            this.btn_Process.Click += new System.EventHandler(this.btn_Process_Click);
            // 
            // cbWordView
            // 
            this.cbWordView.FormattingEnabled = true;
            this.cbWordView.Location = new System.Drawing.Point(255, 641);
            this.cbWordView.Name = "cbWordView";
            this.cbWordView.Size = new System.Drawing.Size(118, 20);
            this.cbWordView.TabIndex = 2;
            this.cbWordView.SelectionChangeCommitted += new System.EventHandler(this.cbWordView_SelectionChangeCommitted);
            this.cbWordView.SelectedIndexChanged += new System.EventHandler(this.cbWordView_SelectedIndexChanged);
            // 
            // lblWordView
            // 
            this.lblWordView.AutoSize = true;
            this.lblWordView.Location = new System.Drawing.Point(137, 644);
            this.lblWordView.Name = "lblWordView";
            this.lblWordView.Size = new System.Drawing.Size(112, 12);
            this.lblWordView.TabIndex = 3;
            this.lblWordView.Text = "Select Word Range";
            // 
            // tmMonitor
            // 
            this.tmMonitor.Tick += new System.EventHandler(this.tmMonitor_Tick);
            // 
            // Melsec_Debug_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(881, 671);
            this.Controls.Add(this.lblWordView);
            this.Controls.Add(this.cbWordView);
            this.Controls.Add(this.btn_Process);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Melsec_Debug_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Melsec Debug";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBit)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridViewBit;
        private System.Windows.Forms.DataGridView dataGridViewWord;
        private System.Windows.Forms.Button btn_Process;
        private System.Windows.Forms.ComboBox cbWordView;
        private System.Windows.Forms.Label lblWordView;
        private System.Windows.Forms.Timer tmMonitor;
    }
}
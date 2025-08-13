namespace Dms.ModbusDevices
{
    partial class ViewModbusMonitor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageUsedBits = new System.Windows.Forms.TabPage();
            this.dataGridViewUsedBit = new Dms.Control.DoubleBufferedGridView();
            this.tabPageUsedWords = new System.Windows.Forms.TabPage();
            this.dataGridViewUsedWord = new Dms.Control.DoubleBufferedGridView();
            this.tmMonitor = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageUsedBits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsedBit)).BeginInit();
            this.tabPageUsedWords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsedWord)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageUsedBits);
            this.tabControl1.Controls.Add(this.tabPageUsedWords);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(545, 369);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageUsedBits
            // 
            this.tabPageUsedBits.Controls.Add(this.dataGridViewUsedBit);
            this.tabPageUsedBits.Location = new System.Drawing.Point(4, 21);
            this.tabPageUsedBits.Name = "tabPageUsedBits";
            this.tabPageUsedBits.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUsedBits.Size = new System.Drawing.Size(537, 344);
            this.tabPageUsedBits.TabIndex = 0;
            this.tabPageUsedBits.Text = "Used Bits";
            this.tabPageUsedBits.UseVisualStyleBackColor = true;
            // 
            // dataGridViewUsedBit
            // 
            this.dataGridViewUsedBit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsedBit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsedBit.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewUsedBit.Name = "dataGridViewUsedBit";
            this.dataGridViewUsedBit.RowTemplate.Height = 23;
            this.dataGridViewUsedBit.Size = new System.Drawing.Size(531, 338);
            this.dataGridViewUsedBit.TabIndex = 0;
            this.dataGridViewUsedBit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewUsedBit_CellContentClick);
            // 
            // tabPageUsedWords
            // 
            this.tabPageUsedWords.Controls.Add(this.dataGridViewUsedWord);
            this.tabPageUsedWords.Location = new System.Drawing.Point(4, 21);
            this.tabPageUsedWords.Name = "tabPageUsedWords";
            this.tabPageUsedWords.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUsedWords.Size = new System.Drawing.Size(537, 344);
            this.tabPageUsedWords.TabIndex = 2;
            this.tabPageUsedWords.Text = "Used Words";
            this.tabPageUsedWords.UseVisualStyleBackColor = true;
            // 
            // dataGridViewUsedWord
            // 
            this.dataGridViewUsedWord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsedWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsedWord.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewUsedWord.Name = "dataGridViewUsedWord";
            this.dataGridViewUsedWord.RowTemplate.Height = 23;
            this.dataGridViewUsedWord.Size = new System.Drawing.Size(531, 338);
            this.dataGridViewUsedWord.TabIndex = 0;
            // 
            // tmMonitor
            // 
            this.tmMonitor.Tick += new System.EventHandler(this.tmMonitor_Tick);
            // 
            // ViewMelsecNetMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabControl1);
            this.Name = "ViewMelsecNetMonitor";
            this.Size = new System.Drawing.Size(545, 369);
            this.Load += new System.EventHandler(this.ViewModbus_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageUsedBits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsedBit)).EndInit();
            this.tabPageUsedWords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsedWord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageUsedBits;
        private Dms.Control.DoubleBufferedGridView dataGridViewUsedBit;
        private System.Windows.Forms.Timer tmMonitor;
        private System.Windows.Forms.TabPage tabPageUsedWords;
        private Dms.Control.DoubleBufferedGridView dataGridViewUsedWord;
    }
}

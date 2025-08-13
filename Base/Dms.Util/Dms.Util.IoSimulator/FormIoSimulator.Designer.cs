namespace Dms.Util
{
    partial class FormIoSimulator
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewIoList = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.viewIOEdit = new Dms.Util.IODefine.ViewIOEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageIo = new System.Windows.Forms.TabPage();
            this.tabPageSequence = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnSeqPauseAll = new System.Windows.Forms.Button();
            this.btnSeqStartAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSeqPause = new System.Windows.Forms.Button();
            this.btnSeqStart = new System.Windows.Forms.Button();
            this.listBoxSimulSequence = new System.Windows.Forms.ListBox();
            this.listBoxSeqLog = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageIo.SuspendLayout();
            this.tabPageSequence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewIoList);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 3, 10);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.viewIOEdit);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3, 10, 10, 10);
            this.splitContainer1.Size = new System.Drawing.Size(866, 519);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewIoList
            // 
            this.treeViewIoList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewIoList.Location = new System.Drawing.Point(10, 40);
            this.treeViewIoList.Name = "treeViewIoList";
            this.treeViewIoList.Size = new System.Drawing.Size(204, 468);
            this.treeViewIoList.TabIndex = 10;
            this.treeViewIoList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewIoList_NodeMouseClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = " I/O Devices";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // viewIOEdit
            // 
            this.viewIOEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewIOEdit.BackColor = System.Drawing.Color.Transparent;
            this.viewIOEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOEdit.Location = new System.Drawing.Point(1, 40);
            this.viewIOEdit.Name = "viewIOEdit";
            this.viewIOEdit.OperateMode = Dms.Util.IODefine.ViewIOEdit.OpMode.Config;
            this.viewIOEdit.ShowNodeInfo = true;
            this.viewIOEdit.Size = new System.Drawing.Size(640, 477);
            this.viewIOEdit.TabIndex = 0;
            this.viewIOEdit.TimerStateUpdateEnabled = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(635, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = " I/O Control";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageIo);
            this.tabControl1.Controls.Add(this.tabPageSequence);
            this.tabControl1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl1.Location = new System.Drawing.Point(5, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(880, 551);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageIo
            // 
            this.tabPageIo.Controls.Add(this.splitContainer1);
            this.tabPageIo.Location = new System.Drawing.Point(4, 22);
            this.tabPageIo.Name = "tabPageIo";
            this.tabPageIo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIo.Size = new System.Drawing.Size(872, 525);
            this.tabPageIo.TabIndex = 0;
            this.tabPageIo.Text = "I/O List";
            this.tabPageIo.UseVisualStyleBackColor = true;
            // 
            // tabPageSequence
            // 
            this.tabPageSequence.Controls.Add(this.splitContainer2);
            this.tabPageSequence.Location = new System.Drawing.Point(4, 22);
            this.tabPageSequence.Name = "tabPageSequence";
            this.tabPageSequence.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSequence.Size = new System.Drawing.Size(872, 525);
            this.tabPageSequence.TabIndex = 1;
            this.tabPageSequence.Text = "Sequence";
            this.tabPageSequence.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnSeqPauseAll);
            this.splitContainer2.Panel1.Controls.Add(this.btnSeqStartAll);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.btnSeqPause);
            this.splitContainer2.Panel1.Controls.Add(this.btnSeqStart);
            this.splitContainer2.Panel1.Controls.Add(this.listBoxSimulSequence);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 3, 10);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBoxSeqLog);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(5, 10, 10, 10);
            this.splitContainer2.Size = new System.Drawing.Size(866, 519);
            this.splitContainer2.SplitterDistance = 248;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 5;
            // 
            // btnSeqPauseAll
            // 
            this.btnSeqPauseAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSeqPauseAll.Location = new System.Drawing.Point(129, 470);
            this.btnSeqPauseAll.Name = "btnSeqPauseAll";
            this.btnSeqPauseAll.Size = new System.Drawing.Size(112, 40);
            this.btnSeqPauseAll.TabIndex = 14;
            this.btnSeqPauseAll.Text = "Pause All";
            this.btnSeqPauseAll.UseVisualStyleBackColor = true;
            this.btnSeqPauseAll.Click += new System.EventHandler(this.btnSeqPauseAll_Click);
            // 
            // btnSeqStartAll
            // 
            this.btnSeqStartAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSeqStartAll.Location = new System.Drawing.Point(13, 470);
            this.btnSeqStartAll.Name = "btnSeqStartAll";
            this.btnSeqStartAll.Size = new System.Drawing.Size(112, 40);
            this.btnSeqStartAll.TabIndex = 13;
            this.btnSeqStartAll.Text = "Start All";
            this.btnSeqStartAll.UseVisualStyleBackColor = true;
            this.btnSeqStartAll.Click += new System.EventHandler(this.btnSeqStartAll_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = " Sequences";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSeqPause
            // 
            this.btnSeqPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSeqPause.Location = new System.Drawing.Point(129, 426);
            this.btnSeqPause.Name = "btnSeqPause";
            this.btnSeqPause.Size = new System.Drawing.Size(112, 40);
            this.btnSeqPause.TabIndex = 4;
            this.btnSeqPause.Text = "Pause";
            this.btnSeqPause.UseVisualStyleBackColor = true;
            this.btnSeqPause.Click += new System.EventHandler(this.btnSeqPause_Click);
            // 
            // btnSeqStart
            // 
            this.btnSeqStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSeqStart.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeqStart.Location = new System.Drawing.Point(13, 426);
            this.btnSeqStart.Name = "btnSeqStart";
            this.btnSeqStart.Size = new System.Drawing.Size(112, 40);
            this.btnSeqStart.TabIndex = 3;
            this.btnSeqStart.Text = "Start";
            this.btnSeqStart.UseVisualStyleBackColor = true;
            this.btnSeqStart.Click += new System.EventHandler(this.btnSeqStart_Click);
            // 
            // listBoxSimulSequence
            // 
            this.listBoxSimulSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSimulSequence.FormattingEnabled = true;
            this.listBoxSimulSequence.ItemHeight = 12;
            this.listBoxSimulSequence.Location = new System.Drawing.Point(10, 40);
            this.listBoxSimulSequence.Name = "listBoxSimulSequence";
            this.listBoxSimulSequence.Size = new System.Drawing.Size(235, 364);
            this.listBoxSimulSequence.TabIndex = 1;
            this.listBoxSimulSequence.SelectedIndexChanged += new System.EventHandler(this.listBoxSimulSequence_SelectedIndexChanged);
            // 
            // listBoxSeqLog
            // 
            this.listBoxSeqLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSeqLog.FormattingEnabled = true;
            this.listBoxSeqLog.ItemHeight = 12;
            this.listBoxSeqLog.Location = new System.Drawing.Point(5, 40);
            this.listBoxSeqLog.Name = "listBoxSeqLog";
            this.listBoxSeqLog.Size = new System.Drawing.Size(602, 460);
            this.listBoxSeqLog.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(602, 24);
            this.label4.TabIndex = 13;
            this.label4.Text = " Sequence Log";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormIoSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(892, 566);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormIoSimulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " * Simulator *";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIoSimulator_FormClosing);
            this.Load += new System.EventHandler(this.FormIoSimulator_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageIo.ResumeLayout(false);
            this.tabPageSequence.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewIoList;
        private System.Windows.Forms.Label label1;
        private Dms.Util.IODefine.ViewIOEdit viewIOEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageIo;
        private System.Windows.Forms.TabPage tabPageSequence;
        private System.Windows.Forms.ListBox listBoxSimulSequence;
        private System.Windows.Forms.Button btnSeqPause;
        private System.Windows.Forms.Button btnSeqStart;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxSeqLog;
        private System.Windows.Forms.Button btnSeqPauseAll;
        private System.Windows.Forms.Button btnSeqStartAll;
    }
}


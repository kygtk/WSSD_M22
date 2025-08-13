namespace Dms.HMI
{
    partial class HistoryForm
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
            this.tabHistory = new System.Windows.Forms.TabControl();
            this.tabPageGlassApdHistoryLog = new System.Windows.Forms.TabPage();
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.tabHistory.SuspendLayout();
            this.pnlToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.tabPageGlassApdHistoryLog);
            this.tabHistory.Location = new System.Drawing.Point(9, 6);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.SelectedIndex = 0;
            this.tabHistory.Size = new System.Drawing.Size(909, 565);
            this.tabHistory.TabIndex = 0;
            // 
            // tabPageGlassApdHistoryLog
            // 
            this.tabPageGlassApdHistoryLog.Location = new System.Drawing.Point(4, 24);
            this.tabPageGlassApdHistoryLog.Name = "tabPageGlassApdHistoryLog";
            this.tabPageGlassApdHistoryLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGlassApdHistoryLog.Size = new System.Drawing.Size(901, 537);
            this.tabPageGlassApdHistoryLog.TabIndex = 1;
            this.tabPageGlassApdHistoryLog.Text = "Processed Data History";
            this.tabPageGlassApdHistoryLog.UseVisualStyleBackColor = true;
            // 
            // pnlToolBar
            // 
            this.pnlToolBar.Controls.Add(this.gbToolbar);
            this.pnlToolBar.Location = new System.Drawing.Point(920, 0);
            this.pnlToolBar.Margin = new System.Windows.Forms.Padding(1);
            this.pnlToolBar.Name = "pnlToolBar";
            this.pnlToolBar.Size = new System.Drawing.Size(92, 595);
            this.pnlToolBar.TabIndex = 1;
            // 
            // gbToolbar
            // 
            this.gbToolbar.Location = new System.Drawing.Point(4, -4);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Size = new System.Drawing.Size(84, 578);
            this.gbToolbar.TabIndex = 0;
            this.gbToolbar.TabStop = false;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Interval = 1000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // CimHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.Controls.Add(this.pnlToolBar);
            this.Controls.Add(this.tabHistory);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CimHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HistoryForm";
            this.Activated += new System.EventHandler(this.HistoryForm_Activated);
            this.tabHistory.ResumeLayout(false);
            this.pnlToolBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabHistory;
        private System.Windows.Forms.TabPage tabPageGlassApdHistoryLog;
        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.GroupBox gbToolbar;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}
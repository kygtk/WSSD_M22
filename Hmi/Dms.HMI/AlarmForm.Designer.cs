namespace Dms.HMI
{
    partial class AlarmForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAlarmHistory = new System.Windows.Forms.TabPage();
            this.tabAlarmList = new System.Windows.Forms.TabPage();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.btnDelete = new Dms.Control.TagButton();
            this.btnSave = new Dms.Control.TagButton();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.gbToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAlarmHistory);
            this.tabControl1.Controls.Add(this.tabAlarmList);
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(9, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(909, 565);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabAlarmHistory
            // 
            this.tabAlarmHistory.Location = new System.Drawing.Point(4, 24);
            this.tabAlarmHistory.Name = "tabAlarmHistory";
            this.tabAlarmHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmHistory.Size = new System.Drawing.Size(901, 537);
            this.tabAlarmHistory.TabIndex = 0;
            this.tabAlarmHistory.Text = "Alarm History";
            this.tabAlarmHistory.UseVisualStyleBackColor = true;
            // 
            // tabAlarmList
            // 
            this.tabAlarmList.Location = new System.Drawing.Point(4, 24);
            this.tabAlarmList.Name = "tabAlarmList";
            this.tabAlarmList.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmList.Size = new System.Drawing.Size(901, 537);
            this.tabAlarmList.TabIndex = 1;
            this.tabAlarmList.Text = "Alarm List";
            this.tabAlarmList.UseVisualStyleBackColor = true;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.SystemColors.Control;
            this.pnlToolbar.Controls.Add(this.gbToolbar);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlToolbar.Location = new System.Drawing.Point(920, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(1);
            this.pnlToolbar.Size = new System.Drawing.Size(92, 595);
            this.pnlToolbar.TabIndex = 5;
            // 
            // gbToolbar
            // 
            this.gbToolbar.Controls.Add(this.btnDelete);
            this.gbToolbar.Controls.Add(this.btnSave);
            this.gbToolbar.Location = new System.Drawing.Point(4, -4);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Size = new System.Drawing.Size(84, 594);
            this.gbToolbar.TabIndex = 0;
            this.gbToolbar.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnDelete.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnDelete.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnDelete.Command = Dms.Common.Command.Noop;
            this.btnDelete.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnDelete.DeviceTagInfo")));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Image = global::Dms.HMI.Properties.Resources.Delete;
            this.btnDelete.Location = new System.Drawing.Point(3, 83);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 66);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnSave.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnSave.Command = Dms.Common.Command.Noop;
            this.btnSave.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnSave.DeviceTagInfo")));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::Dms.HMI.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(3, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 66);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.ControlBox = false;
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AlarmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SystemForm";
            this.Deactivate += new System.EventHandler(this.AlarmForm_Deactivate);
            this.Load += new System.EventHandler(this.AlarmForm_Load);
            this.Activated += new System.EventHandler(this.AlarmForm_Activated);
            this.tabControl1.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.gbToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAlarmHistory;
        private System.Windows.Forms.TabPage tabAlarmList;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.GroupBox gbToolbar;
        private Dms.Control.TagButton btnSave;
        private Dms.Control.TagButton btnDelete;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
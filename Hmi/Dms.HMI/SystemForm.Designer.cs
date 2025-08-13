namespace Dms.HMI
{
    partial class SystemForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemForm));
            this.tabControlSystem = new System.Windows.Forms.TabControl();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.btnSave = new Dms.Control.TagButton();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.pnlToolbar.SuspendLayout();
            this.gbToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSystem
            // 
            this.tabControlSystem.Location = new System.Drawing.Point(9, 6);
            this.tabControlSystem.Name = "tabControlSystem";
            this.tabControlSystem.SelectedIndex = 0;
            this.tabControlSystem.Size = new System.Drawing.Size(909, 565);
            this.tabControlSystem.TabIndex = 0;
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
            this.pnlToolbar.TabIndex = 4;
            // 
            // gbToolbar
            // 
            this.gbToolbar.Controls.Add(this.btnSave);
            this.gbToolbar.Location = new System.Drawing.Point(4, -4);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Size = new System.Drawing.Size(84, 594);
            this.gbToolbar.TabIndex = 0;
            this.gbToolbar.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnSave.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnSave.Command = Dms.Common.Command.Noop;
            this.btnSave.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnSave.DeviceTagInfo")));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::Dms.HMI.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(3, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 66);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 500;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // SystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.ControlBox = false;
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.tabControlSystem);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SystemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SystemForm";
            this.Deactivate += new System.EventHandler(this.SystemForm_Deactivate);
            this.Load += new System.EventHandler(this.SystemForm_Load);
            this.Activated += new System.EventHandler(this.SystemForm_Activated);
            this.pnlToolbar.ResumeLayout(false);
            this.gbToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSystem;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.GroupBox gbToolbar;
        private Dms.Control.TagButton btnSave;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}
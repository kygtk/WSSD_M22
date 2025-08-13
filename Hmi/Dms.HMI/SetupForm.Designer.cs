namespace Dms.HMI
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGenInfo = new System.Windows.Forms.TabPage();
            this.tabGaugeInterlock = new System.Windows.Forms.TabPage();
            this.tabSensorInterlock = new System.Windows.Forms.TabPage();
            this.tabCvDistance = new System.Windows.Forms.TabPage();
            this.tabCvMotor = new System.Windows.Forms.TabPage();
            this.tabHpmj = new System.Windows.Forms.TabPage();
            this.tabInterface = new System.Windows.Forms.TabPage();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.GaugeBox = new System.Windows.Forms.GroupBox();
            this.GaugeCurval = new System.Windows.Forms.TextBox();
            this.GaugeName = new System.Windows.Forms.TextBox();
            this.btnSave = new Dms.Control.TagButton();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.gbToolbar.SuspendLayout();
            this.GaugeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGenInfo);
            this.tabControl1.Controls.Add(this.tabGaugeInterlock);
            this.tabControl1.Controls.Add(this.tabSensorInterlock);
            this.tabControl1.Controls.Add(this.tabCvDistance);
            this.tabControl1.Controls.Add(this.tabCvMotor);
            this.tabControl1.Controls.Add(this.tabHpmj);
            this.tabControl1.Controls.Add(this.tabInterface);
            this.tabControl1.Location = new System.Drawing.Point(9, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(909, 565);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGenInfo
            // 
            this.tabGenInfo.Location = new System.Drawing.Point(4, 24);
            this.tabGenInfo.Name = "tabGenInfo";
            this.tabGenInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabGenInfo.Size = new System.Drawing.Size(901, 537);
            this.tabGenInfo.TabIndex = 0;
            this.tabGenInfo.Text = "General";
            this.tabGenInfo.UseVisualStyleBackColor = true;
            // 
            // tabGaugeInterlock
            // 
            this.tabGaugeInterlock.Location = new System.Drawing.Point(4, 24);
            this.tabGaugeInterlock.Name = "tabGaugeInterlock";
            this.tabGaugeInterlock.Size = new System.Drawing.Size(901, 537);
            this.tabGaugeInterlock.TabIndex = 3;
            this.tabGaugeInterlock.Text = "Gauge Interlock";
            this.tabGaugeInterlock.UseVisualStyleBackColor = true;
            // 
            // tabSensorInterlock
            // 
            this.tabSensorInterlock.Location = new System.Drawing.Point(4, 24);
            this.tabSensorInterlock.Name = "tabSensorInterlock";
            this.tabSensorInterlock.Size = new System.Drawing.Size(901, 537);
            this.tabSensorInterlock.TabIndex = 4;
            this.tabSensorInterlock.Text = "Sensor Interlock";
            this.tabSensorInterlock.UseVisualStyleBackColor = true;
            // 
            // tabCvDistance
            // 
            this.tabCvDistance.Location = new System.Drawing.Point(4, 24);
            this.tabCvDistance.Name = "tabCvDistance";
            this.tabCvDistance.Size = new System.Drawing.Size(901, 537);
            this.tabCvDistance.TabIndex = 5;
            this.tabCvDistance.Text = "Cv Distance";
            this.tabCvDistance.UseVisualStyleBackColor = true;
            // 
            // tabCvMotor
            // 
            this.tabCvMotor.Location = new System.Drawing.Point(4, 24);
            this.tabCvMotor.Name = "tabCvMotor";
            this.tabCvMotor.Size = new System.Drawing.Size(901, 537);
            this.tabCvMotor.TabIndex = 6;
            this.tabCvMotor.Text = "Cv Motors";
            this.tabCvMotor.UseVisualStyleBackColor = true;
            // 
            // tabHpmj
            // 
            this.tabHpmj.Location = new System.Drawing.Point(4, 24);
            this.tabHpmj.Name = "tabHpmj";
            this.tabHpmj.Size = new System.Drawing.Size(901, 537);
            this.tabHpmj.TabIndex = 10;
            this.tabHpmj.Text = "HPMJ";
            this.tabHpmj.UseVisualStyleBackColor = true;
            // 
            // tabInterface
            // 
            this.tabInterface.Location = new System.Drawing.Point(4, 24);
            this.tabInterface.Name = "tabInterface";
            this.tabInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabInterface.Size = new System.Drawing.Size(901, 537);
            this.tabInterface.TabIndex = 7;
            this.tabInterface.Text = "Interface Parameter";
            this.tabInterface.UseVisualStyleBackColor = true;
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
            this.gbToolbar.Controls.Add(this.GaugeBox);
            this.gbToolbar.Controls.Add(this.btnSave);
            this.gbToolbar.Location = new System.Drawing.Point(4, -4);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Size = new System.Drawing.Size(84, 594);
            this.gbToolbar.TabIndex = 0;
            this.gbToolbar.TabStop = false;
            // 
            // GaugeBox
            // 
            this.GaugeBox.Controls.Add(this.GaugeCurval);
            this.GaugeBox.Controls.Add(this.GaugeName);
            this.GaugeBox.Location = new System.Drawing.Point(3, 89);
            this.GaugeBox.Name = "GaugeBox";
            this.GaugeBox.Size = new System.Drawing.Size(77, 174);
            this.GaugeBox.TabIndex = 8;
            this.GaugeBox.TabStop = false;
            this.GaugeBox.Text = "Cur Val";
            // 
            // GaugeCurval
            // 
            this.GaugeCurval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GaugeCurval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GaugeCurval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.GaugeCurval.Location = new System.Drawing.Point(6, 132);
            this.GaugeCurval.Multiline = true;
            this.GaugeCurval.Name = "GaugeCurval";
            this.GaugeCurval.Size = new System.Drawing.Size(65, 33);
            this.GaugeCurval.TabIndex = 6;
            // 
            // GaugeName
            // 
            this.GaugeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GaugeName.Location = new System.Drawing.Point(6, 20);
            this.GaugeName.Multiline = true;
            this.GaugeName.Name = "GaugeName";
            this.GaugeName.Size = new System.Drawing.Size(65, 106);
            this.GaugeName.TabIndex = 0;
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
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.ControlBox = false;
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SystemForm";
            this.Deactivate += new System.EventHandler(this.SetupForm_Deactivate);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.Activated += new System.EventHandler(this.SetupForm_Activated);
            this.tabControl1.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.gbToolbar.ResumeLayout(false);
            this.GaugeBox.ResumeLayout(false);
            this.GaugeBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGenInfo;
        private System.Windows.Forms.TabPage tabGaugeInterlock;
        private System.Windows.Forms.TabPage tabSensorInterlock;
        private System.Windows.Forms.TabPage tabCvDistance;
        private System.Windows.Forms.TabPage tabCvMotor;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.GroupBox gbToolbar;
        private Dms.Control.TagButton btnSave;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.TabPage tabInterface;
        private System.Windows.Forms.TabPage tabHpmj;
        private System.Windows.Forms.GroupBox GaugeBox;
        private System.Windows.Forms.TextBox GaugeCurval;
        private System.Windows.Forms.TextBox GaugeName;
    }
}
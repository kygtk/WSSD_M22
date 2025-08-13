namespace Dms.HMI
{
    partial class JobsToolbar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobsToolbar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Maint = new System.Windows.Forms.Button();
            this.btnPause = new Dms.Control.TagButton();
            this.btnCycleStop = new Dms.Control.TagButton();
            this.btnCycleStart = new Dms.Control.TagButton();
            this.btnReady = new Dms.Control.TagButton();
            this.btnManual = new Dms.Control.TagButton();
            this.btnAuto = new Dms.Control.TagButton();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 595);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Maint);
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.btnCycleStop);
            this.groupBox1.Controls.Add(this.btnCycleStart);
            this.groupBox1.Controls.Add(this.btnReady);
            this.groupBox1.Controls.Add(this.btnManual);
            this.groupBox1.Controls.Add(this.btnAuto);
            this.groupBox1.Location = new System.Drawing.Point(4, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(84, 594);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // Maint
            // 
            this.Maint.BackColor = System.Drawing.Color.Transparent;
            this.Maint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Maint.BackgroundImage")));
            this.Maint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Maint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Maint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Maint.Location = new System.Drawing.Point(3, 525);
            this.Maint.Name = "Maint";
            this.Maint.Size = new System.Drawing.Size(78, 66);
            this.Maint.TabIndex = 11;
            this.Maint.Text = "Maint";
            this.Maint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Maint.UseVisualStyleBackColor = false;
            this.Maint.Click += new System.EventHandler(this.Maint_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPause.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnPause.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnPause.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPause.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnPause.Command = Dms.Common.Command.Pause;
            this.btnPause.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnPause.DeviceTagInfo")));
            this.btnPause.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPause.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.Image = global::Dms.HMI.Properties.Resources.Pause;
            this.btnPause.Location = new System.Drawing.Point(3, 347);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(78, 66);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "Pause";
            this.btnPause.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnCycleStop
            // 
            this.btnCycleStop.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleStop.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnCycleStop.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnCycleStop.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleStop.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnCycleStop.Command = Dms.Common.Command.CycleStop;
            this.btnCycleStop.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnCycleStop.DeviceTagInfo")));
            this.btnCycleStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCycleStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCycleStop.Image = global::Dms.HMI.Properties.Resources.CycleStop;
            this.btnCycleStop.Location = new System.Drawing.Point(3, 281);
            this.btnCycleStop.Name = "btnCycleStop";
            this.btnCycleStop.Size = new System.Drawing.Size(78, 66);
            this.btnCycleStop.TabIndex = 10;
            this.btnCycleStop.Text = "Cycle\r\nStop";
            this.btnCycleStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleStop.UseVisualStyleBackColor = true;
            // 
            // btnCycleStart
            // 
            this.btnCycleStart.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleStart.ButtonCheckedColor = System.Drawing.Color.Transparent;
            this.btnCycleStart.ButtonCheckedType = Dms.Control.TagButton.Type.B_FalseCheck;
            this.btnCycleStart.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleStart.ButtonUnCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnCycleStart.Command = Dms.Common.Command.CycleStart;
            this.btnCycleStart.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnCycleStart.DeviceTagInfo")));
            this.btnCycleStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCycleStart.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCycleStart.Image = global::Dms.HMI.Properties.Resources.CycleStart;
            this.btnCycleStart.Location = new System.Drawing.Point(3, 215);
            this.btnCycleStart.Name = "btnCycleStart";
            this.btnCycleStart.Size = new System.Drawing.Size(78, 66);
            this.btnCycleStart.TabIndex = 5;
            this.btnCycleStart.Text = "Cycle\r\nStart";
            this.btnCycleStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleStart.UseVisualStyleBackColor = true;
            // 
            // btnReady
            // 
            this.btnReady.BackColor = System.Drawing.Color.Transparent;
            this.btnReady.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnReady.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnReady.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReady.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnReady.Command = Dms.Common.Command.Ready;
            this.btnReady.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnReady.DeviceTagInfo")));
            this.btnReady.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReady.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnReady.Image = global::Dms.HMI.Properties.Resources.Ready;
            this.btnReady.Location = new System.Drawing.Point(3, 149);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(78, 66);
            this.btnReady.TabIndex = 9;
            this.btnReady.Text = "Ready";
            this.btnReady.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReady.UseVisualStyleBackColor = true;
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.Transparent;
            this.btnManual.ButtonCheckedColor = System.Drawing.Color.Transparent;
            this.btnManual.ButtonCheckedType = Dms.Control.TagButton.Type.B_FalseCheck;
            this.btnManual.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManual.ButtonUnCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnManual.Command = Dms.Common.Command.Manual;
            this.btnManual.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnManual.DeviceTagInfo")));
            this.btnManual.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnManual.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnManual.Image = global::Dms.HMI.Properties.Resources.Manual;
            this.btnManual.Location = new System.Drawing.Point(3, 83);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(78, 66);
            this.btnManual.TabIndex = 6;
            this.btnManual.Text = "Manual";
            this.btnManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManual.UseVisualStyleBackColor = true;
            // 
            // btnAuto
            // 
            this.btnAuto.BackColor = System.Drawing.Color.Transparent;
            this.btnAuto.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnAuto.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnAuto.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAuto.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnAuto.Command = Dms.Common.Command.Auto;
            this.btnAuto.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnAuto.DeviceTagInfo")));
            this.btnAuto.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAuto.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAuto.Image = global::Dms.HMI.Properties.Resources.auto;
            this.btnAuto.Location = new System.Drawing.Point(3, 17);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(78, 66);
            this.btnAuto.TabIndex = 8;
            this.btnAuto.Text = "Auto";
            this.btnAuto.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAuto.UseVisualStyleBackColor = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // JobsToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "JobsToolbar";
            this.Size = new System.Drawing.Size(92, 595);
            this.Load += new System.EventHandler(this.JobsToolbar_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Dms.Control.TagButton btnPause;
        private Dms.Control.TagButton btnCycleStop;
        private Dms.Control.TagButton btnCycleStart;
        private Dms.Control.TagButton btnReady;
        private Dms.Control.TagButton btnManual;
        private Dms.Control.TagButton btnAuto;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.Button Maint;
    }
}

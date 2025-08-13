using Dms.Util.IODefine;

namespace Dms.Util
{
    partial class FormDesigner
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabEqpConfig = new System.Windows.Forms.TabPage();
            this.tabIoDefine = new System.Windows.Forms.TabPage();
            this.viewIOConfig1 = new Dms.Util.IODefine.ViewIOConfig();
            this.tabIoEdit = new System.Windows.Forms.TabPage();
            this.viewIOEdit1 = new Dms.Util.IODefine.ViewIOEdit();
            this.tabSlaveEdit = new System.Windows.Forms.TabPage();
            this.viewSlaveEdit1 = new Dms.Util.IODefine.ViewSlaveEdit();
            this.buttonAppConfig = new System.Windows.Forms.Button();
            this.buttonAppConfigLoadFrom = new System.Windows.Forms.Button();
            this.viewEqpConfig1 = new Dms.Util.ViewEqpConfig();
            this.tabControl1.SuspendLayout();
            this.tabEqpConfig.SuspendLayout();
            this.tabIoDefine.SuspendLayout();
            this.tabIoEdit.SuspendLayout();
            this.tabSlaveEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(713, 696);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(168, 35);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabEqpConfig);
            this.tabControl1.Controls.Add(this.tabIoDefine);
            this.tabControl1.Controls.Add(this.tabIoEdit);
            this.tabControl1.Controls.Add(this.tabSlaveEdit);
            this.tabControl1.Location = new System.Drawing.Point(7, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(874, 674);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabEqpConfig
            // 
            this.tabEqpConfig.Controls.Add(this.viewEqpConfig1);
            this.tabEqpConfig.Location = new System.Drawing.Point(4, 24);
            this.tabEqpConfig.Name = "tabEqpConfig";
            this.tabEqpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabEqpConfig.Size = new System.Drawing.Size(866, 646);
            this.tabEqpConfig.TabIndex = 0;
            this.tabEqpConfig.Text = "Eqp Design";
            this.tabEqpConfig.UseVisualStyleBackColor = true;
            // 
            // tabIoDefine
            // 
            this.tabIoDefine.Controls.Add(this.viewIOConfig1);
            this.tabIoDefine.Location = new System.Drawing.Point(4, 24);
            this.tabIoDefine.Name = "tabIoDefine";
            this.tabIoDefine.Padding = new System.Windows.Forms.Padding(3);
            this.tabIoDefine.Size = new System.Drawing.Size(866, 646);
            this.tabIoDefine.TabIndex = 1;
            this.tabIoDefine.Text = "I/O Define";
            this.tabIoDefine.UseVisualStyleBackColor = true;
            // 
            // viewIOConfig1
            // 
            this.viewIOConfig1.BackColor = System.Drawing.Color.Transparent;
            this.viewIOConfig1.DesignerMode = Dms.Common.DesignerMode.Design;
            this.viewIOConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewIOConfig1.EnableBomMaker = false;
            this.viewIOConfig1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOConfig1.Location = new System.Drawing.Point(3, 3);
            this.viewIOConfig1.Name = "viewIOConfig1";
            this.viewIOConfig1.Size = new System.Drawing.Size(860, 640);
            this.viewIOConfig1.TabIndex = 0;
            // 
            // tabIoEdit
            // 
            this.tabIoEdit.Controls.Add(this.viewIOEdit1);
            this.tabIoEdit.Location = new System.Drawing.Point(4, 24);
            this.tabIoEdit.Name = "tabIoEdit";
            this.tabIoEdit.Size = new System.Drawing.Size(866, 646);
            this.tabIoEdit.TabIndex = 2;
            this.tabIoEdit.Text = "I/O Edit";
            this.tabIoEdit.UseVisualStyleBackColor = true;
            // 
            // viewIOEdit1
            // 
            this.viewIOEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewIOEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewIOEdit1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOEdit1.Location = new System.Drawing.Point(0, 0);
            this.viewIOEdit1.Name = "viewIOEdit1";
            this.viewIOEdit1.OperateMode = Dms.Util.IODefine.ViewIOEdit.OpMode.Config;
            this.viewIOEdit1.ShowNodeInfo = true;
            this.viewIOEdit1.Size = new System.Drawing.Size(866, 646);
            this.viewIOEdit1.TabIndex = 0;
            this.viewIOEdit1.TimerStateUpdateEnabled = false;
            // 
            // tabSlaveEdit
            // 
            this.tabSlaveEdit.Controls.Add(this.viewSlaveEdit1);
            this.tabSlaveEdit.Location = new System.Drawing.Point(4, 24);
            this.tabSlaveEdit.Name = "tabSlaveEdit";
            this.tabSlaveEdit.Size = new System.Drawing.Size(866, 646);
            this.tabSlaveEdit.TabIndex = 3;
            this.tabSlaveEdit.Text = "Slave Edit";
            this.tabSlaveEdit.UseVisualStyleBackColor = true;
            // 
            // viewSlaveEdit1
            // 
            this.viewSlaveEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewSlaveEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewSlaveEdit1.Font = new System.Drawing.Font("Arial", 9F);
            this.viewSlaveEdit1.Location = new System.Drawing.Point(0, 0);
            this.viewSlaveEdit1.Name = "viewSlaveEdit1";
            this.viewSlaveEdit1.OperateMode = Dms.Util.IODefine.ViewSlaveEdit.OpMode.Config;
            this.viewSlaveEdit1.ShowNodeInfo = true;
            this.viewSlaveEdit1.Size = new System.Drawing.Size(866, 646);
            this.viewSlaveEdit1.TabIndex = 0;
            this.viewSlaveEdit1.TimerStateUpdateEnabled = false;
            // 
            // buttonAppConfig
            // 
            this.buttonAppConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAppConfig.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonAppConfig.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAppConfig.Location = new System.Drawing.Point(7, 696);
            this.buttonAppConfig.Name = "buttonAppConfig";
            this.buttonAppConfig.Size = new System.Drawing.Size(168, 35);
            this.buttonAppConfig.TabIndex = 5;
            this.buttonAppConfig.Text = "APP Config";
            this.buttonAppConfig.UseVisualStyleBackColor = false;
            this.buttonAppConfig.Click += new System.EventHandler(this.buttonAppConfig_Click);
            // 
            // buttonAppConfigLoadFrom
            // 
            this.buttonAppConfigLoadFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAppConfigLoadFrom.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAppConfigLoadFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAppConfigLoadFrom.Location = new System.Drawing.Point(181, 696);
            this.buttonAppConfigLoadFrom.Name = "buttonAppConfigLoadFrom";
            this.buttonAppConfigLoadFrom.Size = new System.Drawing.Size(168, 35);
            this.buttonAppConfigLoadFrom.TabIndex = 6;
            this.buttonAppConfigLoadFrom.Text = "APP Config...";
            this.buttonAppConfigLoadFrom.UseVisualStyleBackColor = false;
            this.buttonAppConfigLoadFrom.Click += new System.EventHandler(this.buttonAppConfigLoadFrom_Click);
            // 
            // viewEqpConfig1
            // 
            this.viewEqpConfig1.BackColor = System.Drawing.Color.Transparent;
            this.viewEqpConfig1.DesignerMode = Dms.Common.DesignerMode.Design;
            this.viewEqpConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewEqpConfig1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewEqpConfig1.Location = new System.Drawing.Point(3, 3);
            this.viewEqpConfig1.Name = "viewEqpConfig1";
            this.viewEqpConfig1.Size = new System.Drawing.Size(860, 640);
            this.viewEqpConfig1.TabIndex = 0;
            // 
            // FormDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(893, 738);
            this.Controls.Add(this.buttonAppConfigLoadFrom);
            this.Controls.Add(this.buttonAppConfig);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonClose);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormDesigner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WSSD Designer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabEqpConfig.ResumeLayout(false);
            this.tabIoDefine.ResumeLayout(false);
            this.tabIoEdit.ResumeLayout(false);
            this.tabSlaveEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabEqpConfig;
        private System.Windows.Forms.TabPage tabIoDefine;
        private ViewEqpConfig viewEqpConfig1;
        private ViewIOConfig viewIOConfig1;
        private System.Windows.Forms.TabPage tabIoEdit;
        private Dms.Util.IODefine.ViewIOEdit viewIOEdit1;
        private System.Windows.Forms.Button buttonAppConfig;
        private System.Windows.Forms.Button buttonAppConfigLoadFrom;
        private System.Windows.Forms.TabPage tabSlaveEdit;
        private ViewSlaveEdit viewSlaveEdit1;
    }
}
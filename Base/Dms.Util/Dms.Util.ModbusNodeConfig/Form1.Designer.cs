namespace Dms.Util
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSimulateAddress = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.formDeviceAssy1 = new Dms.DeviceLibrary.FormDeviceAssy();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(938, 524);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSimulateAddress);
            this.tabPage1.Controls.Add(this.btnCreate);
            this.tabPage1.Controls.Add(this.formDeviceAssy1);
            this.tabPage1.Controls.Add(this.btnWrite);
            this.tabPage1.Controls.Add(this.btnLoad);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(930, 499);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Define";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSimulateAddress
            // 
            this.btnSimulateAddress.Location = new System.Drawing.Point(817, 154);
            this.btnSimulateAddress.Name = "btnSimulateAddress";
            this.btnSimulateAddress.Size = new System.Drawing.Size(100, 43);
            this.btnSimulateAddress.TabIndex = 6;
            this.btnSimulateAddress.Text = "Simulate Address";
            this.btnSimulateAddress.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(817, 7);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 43);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // formDeviceAssy1
            // 
            this.formDeviceAssy1.CurNode = null;
            this.formDeviceAssy1.Location = new System.Drawing.Point(4, 6);
            this.formDeviceAssy1.MenuEnabled = false;
            this.formDeviceAssy1.Name = "formDeviceAssy1";
            this.formDeviceAssy1.Size = new System.Drawing.Size(807, 451);
            this.formDeviceAssy1.TabIndex = 5;
            this.formDeviceAssy1.NodeDel += new Dms.DeviceLibrary.FormDeviceAssy.TreeNodeDelEvent(this.formDeviceAssy1_NodeDel);
            this.formDeviceAssy1.NodeAdd += new Dms.DeviceLibrary.FormDeviceAssy.TreeNodeAddEvent(this.formDeviceAssy1_NodeAdd);
            this.formDeviceAssy1.NodeClick += new Dms.DeviceLibrary.FormDeviceAssy.TreeNodeClickEvent(this.formDeviceAssy1_NodeClick);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(817, 105);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(100, 43);
            this.btnWrite.TabIndex = 4;
            this.btnWrite.Text = "Write to Xml";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(817, 56);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 43);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(938, 524);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Modbus Configurator";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnLoad;
        private Dms.DeviceLibrary.FormDeviceAssy formDeviceAssy1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnSimulateAddress;

    }
}


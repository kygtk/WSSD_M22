namespace Dms.HMI
{
    partial class JobsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.tabRecvGlass = new System.Windows.Forms.TabPage();
            this.tabPiping = new System.Windows.Forms.TabPage();
            this.tabEUV = new System.Windows.Forms.TabPage();
            this.tabProcessData = new System.Windows.Forms.TabPage();
            this.tabInterface = new System.Windows.Forms.TabPage();
            this.tabHpmj = new System.Windows.Forms.TabPage();
            this.tabGauge = new System.Windows.Forms.TabPage();
//            this.tabPageHPMJSettingPara = new System.Windows.Forms.TabPage();//lkl 150929
            this.tabFfuCon = new System.Windows.Forms.TabPage();
            this.tabSendGlass = new System.Windows.Forms.TabPage();
            this.checkCommLog = new System.Windows.Forms.CheckBox();
            this.checkSeqLog = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrUpdateToolbar = new System.Windows.Forms.Timer(this.components);
            this.SeqlogList = new Dms.Control.LogList();
            this.CommLogList = new Dms.Control.LogList();
            this.viewCurrentAlarms1 = new Dms.Data.ViewCurrentAlarms();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMain);
            this.tabControl.Controls.Add(this.tabPiping);
            this.tabControl.Controls.Add(this.tabEUV);
            this.tabControl.Controls.Add(this.tabHpmj);
//            this.tabControl.Controls.Add(this.tabPageHPMJSettingPara);//lkl 150929
            this.tabControl.Controls.Add(this.tabProcessData);
            this.tabControl.Controls.Add(this.tabGauge);
            this.tabControl.Controls.Add(this.tabFfuCon);
            this.tabControl.Controls.Add(this.tabInterface);
            this.tabControl.Controls.Add(this.tabRecvGlass);
            this.tabControl.Controls.Add(this.tabSendGlass);
            this.tabControl.Location = new System.Drawing.Point(9, 4);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(909, 494);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabMain
            // 
            this.tabMain.Location = new System.Drawing.Point(4, 24);
            this.tabMain.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabMain.Size = new System.Drawing.Size(901, 466);
            this.tabMain.TabIndex = 1;
            this.tabMain.Text = "MAIN";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // tabRecvGlass
            // 
            this.tabRecvGlass.Location = new System.Drawing.Point(4, 24);
            this.tabRecvGlass.Name = "tabRecvGlass";
            this.tabRecvGlass.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecvGlass.Size = new System.Drawing.Size(901, 466);
            this.tabRecvGlass.TabIndex = 10;
            this.tabRecvGlass.Text = "Recv GlassData";
            this.tabRecvGlass.UseVisualStyleBackColor = true;
            // 
            // tabPiping
            // 
            this.tabPiping.Location = new System.Drawing.Point(4, 24);
            this.tabPiping.Name = "tabPiping";
            this.tabPiping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPiping.Size = new System.Drawing.Size(901, 466);
            this.tabPiping.TabIndex = 2;
            this.tabPiping.Text = "PIPING";
            this.tabPiping.UseVisualStyleBackColor = true;
            // 
            // tabEUV
            // 
            this.tabEUV.AllowDrop = true;
            this.tabEUV.Location = new System.Drawing.Point(4, 24);
            this.tabEUV.Name = "tabEUV";
            this.tabEUV.Padding = new System.Windows.Forms.Padding(3);
            this.tabEUV.Size = new System.Drawing.Size(901, 466);
            this.tabEUV.TabIndex = 7;
            this.tabEUV.Text = "EUV";
            this.tabEUV.UseVisualStyleBackColor = true;
            // 
            // tabProcessData
            // 
            this.tabProcessData.Location = new System.Drawing.Point(4, 24);
            this.tabProcessData.Name = "tabProcessData";
            this.tabProcessData.Padding = new System.Windows.Forms.Padding(3);
            this.tabProcessData.Size = new System.Drawing.Size(901, 466);
            this.tabProcessData.TabIndex = 3;
            this.tabProcessData.Text = "PROCESS DATA";
            this.tabProcessData.UseVisualStyleBackColor = true;
            // 
            // tabInterface
            // 
            this.tabInterface.Location = new System.Drawing.Point(4, 24);
            this.tabInterface.Name = "tabInterface";
            this.tabInterface.Padding = new System.Windows.Forms.Padding(3);
            this.tabInterface.Size = new System.Drawing.Size(901, 466);
            this.tabInterface.TabIndex = 5;
            this.tabInterface.Text = "INTERFACE";
            this.tabInterface.UseVisualStyleBackColor = true;
            // 
            // tabHpmj
            // 
            this.tabHpmj.Location = new System.Drawing.Point(4, 24);
            this.tabHpmj.Name = "tabHpmj";
            this.tabHpmj.Padding = new System.Windows.Forms.Padding(3);
            this.tabHpmj.Size = new System.Drawing.Size(901, 466);
            this.tabHpmj.TabIndex = 6;
            this.tabHpmj.Text = "HPMJ";
            this.tabHpmj.UseVisualStyleBackColor = true;
            // 
            // tabGauge
            // 
            this.tabGauge.Location = new System.Drawing.Point(4, 24);
            this.tabGauge.Name = "tabGauge";
            this.tabGauge.Padding = new System.Windows.Forms.Padding(3);
            this.tabGauge.Size = new System.Drawing.Size(901, 466);
            this.tabGauge.TabIndex = 8;
            this.tabGauge.Text = "GAUGE";
            this.tabGauge.UseVisualStyleBackColor = true;
            // 
            // tabPageHPMJSettingPara
            // 
            //this.tabPageHPMJSettingPara.Location = new System.Drawing.Point(4, 24);
            //this.tabPageHPMJSettingPara.Name = "tabPageHPMJSettingPara";
            //this.tabPageHPMJSettingPara.Padding = new System.Windows.Forms.Padding(3);
            //this.tabPageHPMJSettingPara.Size = new System.Drawing.Size(901, 466);
            //this.tabPageHPMJSettingPara.TabIndex = 9;
            //this.tabPageHPMJSettingPara.Text = "HPMJSettingPara";
            //this.tabPageHPMJSettingPara.UseVisualStyleBackColor = true;//lkl 150929
            // 
            // tabFfuCon
            // 
            this.tabFfuCon.Location = new System.Drawing.Point(4, 24);
            this.tabFfuCon.Name = "tabFfuCon";
            this.tabFfuCon.Padding = new System.Windows.Forms.Padding(3);
            this.tabFfuCon.Size = new System.Drawing.Size(901, 466);
            this.tabFfuCon.TabIndex = 11;
            this.tabFfuCon.Text = "FFU Control";
            this.tabFfuCon.UseVisualStyleBackColor = true;
            // 
            // tabSendGlass
            // 
            this.tabSendGlass.Location = new System.Drawing.Point(4, 24);
            this.tabSendGlass.Name = "tabSendGlass";
            this.tabSendGlass.Padding = new System.Windows.Forms.Padding(3);
            this.tabSendGlass.Size = new System.Drawing.Size(901, 466);
            this.tabSendGlass.TabIndex = 12;
            this.tabSendGlass.Text = "Send GlassData";
            this.tabSendGlass.UseVisualStyleBackColor = true;
            // 
            // checkCommLog
            // 
            this.checkCommLog.AutoSize = true;
            this.checkCommLog.Location = new System.Drawing.Point(9, 506);
            this.checkCommLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkCommLog.Name = "checkCommLog";
            this.checkCommLog.Size = new System.Drawing.Size(90, 19);
            this.checkCommLog.TabIndex = 2;
            this.checkCommLog.Text = "Comm. Log";
            this.checkCommLog.UseVisualStyleBackColor = true;
            this.checkCommLog.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // checkSeqLog
            // 
            this.checkSeqLog.AutoSize = true;
            this.checkSeqLog.Location = new System.Drawing.Point(313, 506);
            this.checkSeqLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkSeqLog.Name = "checkSeqLog";
            this.checkSeqLog.Size = new System.Drawing.Size(107, 19);
            this.checkSeqLog.TabIndex = 2;
            this.checkSeqLog.Text = "Sequence Log";
            this.checkSeqLog.UseVisualStyleBackColor = true;
            this.checkSeqLog.CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(622, 507);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current Alarms";
            // 
            // tmrUpdateToolbar
            // 
            this.tmrUpdateToolbar.Tick += new System.EventHandler(this.tmrUpdateToolbar_Tick);
            // 
            // SeqlogList
            // 
            this.SeqlogList.BackColor = System.Drawing.Color.Transparent;
            this.SeqlogList.CheckEnable = true;
            this.SeqlogList.ClearSelection = false;
            this.SeqlogList.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("SeqlogList.DeviceTagInfo")));
            this.SeqlogList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SeqlogList.Location = new System.Drawing.Point(311, 524);
            this.SeqlogList.Name = "SeqlogList";
            this.SeqlogList.Size = new System.Drawing.Size(302, 66);
            this.SeqlogList.TabIndex = 5;
            // 
            // CommLogList
            // 
            this.CommLogList.BackColor = System.Drawing.Color.Transparent;
            this.CommLogList.CheckEnable = true;
            this.CommLogList.ClearSelection = false;
            this.CommLogList.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("CommLogList.DeviceTagInfo")));
            this.CommLogList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommLogList.Location = new System.Drawing.Point(8, 524);
            this.CommLogList.Name = "CommLogList";
            this.CommLogList.Size = new System.Drawing.Size(302, 66);
            this.CommLogList.TabIndex = 0;
            // 
            // viewCurrentAlarms1
            // 
            this.viewCurrentAlarms1.BackColor = System.Drawing.Color.Transparent;
            this.viewCurrentAlarms1.ColumnHeadersVisible = false;
            this.viewCurrentAlarms1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewCurrentAlarms1.Location = new System.Drawing.Point(616, 524);
            this.viewCurrentAlarms1.Name = "viewCurrentAlarms1";
            this.viewCurrentAlarms1.Size = new System.Drawing.Size(299, 64);
            this.viewCurrentAlarms1.TabIndex = 6;
            // 
            // JobsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.Controls.Add(this.viewCurrentAlarms1);
            this.Controls.Add(this.CommLogList);
            this.Controls.Add(this.SeqlogList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkSeqLog);
            this.Controls.Add(this.checkCommLog);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "JobsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "JobsForm";
            this.Deactivate += new System.EventHandler(this.JobsForm_Deactivate);
            this.Load += new System.EventHandler(this.JobsForm_Load);
            this.Activated += new System.EventHandler(this.JobsForm_Activated);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.CheckBox checkCommLog;
        private System.Windows.Forms.CheckBox checkSeqLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmrUpdateToolbar;
        private Dms.Control.LogList SeqlogList;
        private Dms.Control.LogList CommLogList;
        private Dms.Data.ViewCurrentAlarms viewCurrentAlarms1;
        private System.Windows.Forms.TabPage tabPiping;
        private System.Windows.Forms.TabPage tabProcessData;
        private System.Windows.Forms.TabPage tabInterface;
        private System.Windows.Forms.TabPage tabHpmj;
        private System.Windows.Forms.TabPage tabEUV;
        private System.Windows.Forms.TabPage tabGauge;
//        private System.Windows.Forms.TabPage tabPageHPMJSettingPara;//lkl 150929
        private System.Windows.Forms.TabPage tabRecvGlass;
        private System.Windows.Forms.TabPage tabFfuCon;
        private System.Windows.Forms.TabPage tabSendGlass;

    }
}
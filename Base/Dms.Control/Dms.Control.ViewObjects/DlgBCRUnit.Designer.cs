namespace Dms.Control
{
    partial class DlgBCRUnit
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelBcrName = new System.Windows.Forms.Label();
            this.panelDataFrame = new System.Windows.Forms.Panel();
            this.labelBcrData = new System.Windows.Forms.Label();
            this.timerUpdateState = new System.Windows.Forms.Timer(this.components);
            this.buttonRead = new System.Windows.Forms.Button();
            this.panelDataFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonClose.Location = new System.Drawing.Point(173, 102);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(70, 52);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelBcrName
            // 
            this.labelBcrName.BackColor = System.Drawing.Color.CornflowerBlue;
            this.labelBcrName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBcrName.ForeColor = System.Drawing.Color.White;
            this.labelBcrName.Location = new System.Drawing.Point(10, 16);
            this.labelBcrName.Name = "labelBcrName";
            this.labelBcrName.Size = new System.Drawing.Size(152, 31);
            this.labelBcrName.TabIndex = 16;
            this.labelBcrName.Text = "BL #";
            this.labelBcrName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDataFrame
            // 
            this.panelDataFrame.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelDataFrame.Controls.Add(this.labelBcrData);
            this.panelDataFrame.Location = new System.Drawing.Point(10, 59);
            this.panelDataFrame.Name = "panelDataFrame";
            this.panelDataFrame.Padding = new System.Windows.Forms.Padding(3);
            this.panelDataFrame.Size = new System.Drawing.Size(152, 34);
            this.panelDataFrame.TabIndex = 17;
            // 
            // labelBcrData
            // 
            this.labelBcrData.BackColor = System.Drawing.Color.White;
            this.labelBcrData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBcrData.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBcrData.ForeColor = System.Drawing.Color.Navy;
            this.labelBcrData.Location = new System.Drawing.Point(3, 3);
            this.labelBcrData.Name = "labelBcrData";
            this.labelBcrData.Size = new System.Drawing.Size(146, 28);
            this.labelBcrData.TabIndex = 18;
            this.labelBcrData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBcrData.Click += new System.EventHandler(this.labelCstId_Click);
            // 
            // timerUpdateState
            // 
            this.timerUpdateState.Interval = 300;
            this.timerUpdateState.Tick += new System.EventHandler(this.timerUpdateState_Tick);
            // 
            // buttonRead
            // 
            this.buttonRead.BackColor = System.Drawing.Color.Lavender;
            this.buttonRead.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRead.Location = new System.Drawing.Point(173, 16);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(70, 45);
            this.buttonRead.TabIndex = 0;
            this.buttonRead.Text = "READ";
            this.buttonRead.UseVisualStyleBackColor = false;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // DlgBCRUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(255, 166);
            this.ControlBox = false;
            this.Controls.Add(this.panelDataFrame);
            this.Controls.Add(this.labelBcrName);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgBCRUnit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "  BCR Unit";
            this.Load += new System.EventHandler(this.DlgMappingUnit_Load);
            this.panelDataFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelBcrName;
        private System.Windows.Forms.Panel panelDataFrame;
        private System.Windows.Forms.Timer timerUpdateState;
        private System.Windows.Forms.Label labelBcrData;
        private System.Windows.Forms.Button buttonRead;
    }
}
namespace Dms.Control
{
    partial class DlgMappingUnit
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
            this.buttonMapping = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelPortName = new System.Windows.Forms.Label();
            this.panelCstFrame = new System.Windows.Forms.Panel();
            this.panelCst = new System.Windows.Forms.Panel();
            this.timerUpdateState = new System.Windows.Forms.Timer(this.components);
            this.panelCstFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMapping
            // 
            this.buttonMapping.BackColor = System.Drawing.Color.Lavender;
            this.buttonMapping.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonMapping.Location = new System.Drawing.Point(151, 49);
            this.buttonMapping.Name = "buttonMapping";
            this.buttonMapping.Size = new System.Drawing.Size(79, 67);
            this.buttonMapping.TabIndex = 0;
            this.buttonMapping.Text = "Mapping";
            this.buttonMapping.UseVisualStyleBackColor = false;
            this.buttonMapping.Click += new System.EventHandler(this.buttonMapping_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonClose.Location = new System.Drawing.Point(151, 263);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(79, 52);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelPortName
            // 
            this.labelPortName.BackColor = System.Drawing.Color.CornflowerBlue;
            this.labelPortName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName.ForeColor = System.Drawing.Color.White;
            this.labelPortName.Location = new System.Drawing.Point(10, 12);
            this.labelPortName.Name = "labelPortName";
            this.labelPortName.Size = new System.Drawing.Size(132, 26);
            this.labelPortName.TabIndex = 16;
            this.labelPortName.Text = "Port #";
            this.labelPortName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCstFrame
            // 
            this.panelCstFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panelCstFrame.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelCstFrame.Controls.Add(this.panelCst);
            this.panelCstFrame.Location = new System.Drawing.Point(10, 44);
            this.panelCstFrame.Name = "panelCstFrame";
            this.panelCstFrame.Size = new System.Drawing.Size(132, 269);
            this.panelCstFrame.TabIndex = 17;
            // 
            // panelCst
            // 
            this.panelCst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCst.BackColor = System.Drawing.Color.White;
            this.panelCst.Location = new System.Drawing.Point(3, 4);
            this.panelCst.Name = "panelCst";
            this.panelCst.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panelCst.Size = new System.Drawing.Size(125, 261);
            this.panelCst.TabIndex = 18;
            // 
            // timerUpdateState
            // 
            this.timerUpdateState.Interval = 300;
            this.timerUpdateState.Tick += new System.EventHandler(this.timerUpdateState_Tick);
            // 
            // DlgMappingUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(239, 326);
            this.ControlBox = false;
            this.Controls.Add(this.panelCstFrame);
            this.Controls.Add(this.labelPortName);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonMapping);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgMappingUnit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mapping Unit";
            this.Load += new System.EventHandler(this.DlgMappingUnit_Load);
            this.panelCstFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonMapping;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelPortName;
        private System.Windows.Forms.Panel panelCstFrame;
        private System.Windows.Forms.Panel panelCst;
        private System.Windows.Forms.Timer timerUpdateState;
    }
}
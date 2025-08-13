namespace Dms.Control
{
    partial class RbMotorOperation
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
			this.lblName = new System.Windows.Forms.Label();
			this.btnCw = new System.Windows.Forms.RadioButton();
			this.btnCcw = new System.Windows.Forms.RadioButton();
			this.btnStop = new System.Windows.Forms.RadioButton();
			this.chkAlarm = new System.Windows.Forms.CheckBox();
			this.chkCpOff = new System.Windows.Forms.CheckBox();
			this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblName.BackColor = System.Drawing.Color.Thistle;
			this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblName.Location = new System.Drawing.Point(1, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(78, 36);
			this.lblName.TabIndex = 0;
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCw
			// 
			this.btnCw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnCw.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnCw.BackColor = System.Drawing.Color.LightCyan;
			this.btnCw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCw.Location = new System.Drawing.Point(1, 41);
			this.btnCw.Name = "btnCw";
			this.btnCw.Size = new System.Drawing.Size(78, 36);
			this.btnCw.TabIndex = 1;
			this.btnCw.TabStop = true;
			this.btnCw.Text = "CW";
			this.btnCw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnCw.UseVisualStyleBackColor = false;
			this.btnCw.CheckedChanged += new System.EventHandler(this.btnCw_CheckedChanged);
			// 
			// btnCcw
			// 
			this.btnCcw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnCcw.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnCcw.BackColor = System.Drawing.Color.LemonChiffon;
			this.btnCcw.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCcw.Location = new System.Drawing.Point(1, 83);
			this.btnCcw.Name = "btnCcw";
			this.btnCcw.Size = new System.Drawing.Size(78, 36);
			this.btnCcw.TabIndex = 2;
			this.btnCcw.TabStop = true;
			this.btnCcw.Text = "CCW";
			this.btnCcw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnCcw.UseVisualStyleBackColor = false;
			this.btnCcw.CheckedChanged += new System.EventHandler(this.btnCcw_CheckedChanged);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Appearance = System.Windows.Forms.Appearance.Button;
			this.btnStop.BackColor = System.Drawing.Color.Pink;
			this.btnStop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStop.Location = new System.Drawing.Point(1, 125);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(78, 36);
			this.btnStop.TabIndex = 3;
			this.btnStop.TabStop = true;
			this.btnStop.Text = "STOP";
			this.btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.CheckedChanged += new System.EventHandler(this.btnStop_CheckedChanged);
			// 
			// chkAlarm
			// 
			this.chkAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkAlarm.AutoCheck = false;
			this.chkAlarm.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.chkAlarm.Location = new System.Drawing.Point(4, 164);
			this.chkAlarm.Name = "chkAlarm";
			this.chkAlarm.Size = new System.Drawing.Size(73, 16);
			this.chkAlarm.TabIndex = 4;
			this.chkAlarm.Text = "ALARM";
			this.chkAlarm.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkAlarm.UseVisualStyleBackColor = true;
			// 
			// chkCpOff
			// 
			this.chkCpOff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkCpOff.AutoCheck = false;
			this.chkCpOff.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.chkCpOff.Location = new System.Drawing.Point(4, 181);
			this.chkCpOff.Name = "chkCpOff";
			this.chkCpOff.Size = new System.Drawing.Size(73, 16);
			this.chkCpOff.TabIndex = 5;
			this.chkCpOff.Text = "CP OFF";
			this.chkCpOff.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkCpOff.UseVisualStyleBackColor = true;
			// 
			// tmrUpdateState
			// 
			this.tmrUpdateState.Interval = 300;
			this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
			// 
			// RbMotorOperation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.chkCpOff);
			this.Controls.Add(this.chkAlarm);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnCcw);
			this.Controls.Add(this.btnCw);
			this.Controls.Add(this.lblName);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "RbMotorOperation";
			this.Size = new System.Drawing.Size(80, 200);
			this.Load += new System.EventHandler(this.RbMotorOperation_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.RadioButton btnCw;
        private System.Windows.Forms.RadioButton btnCcw;
        private System.Windows.Forms.RadioButton btnStop;
        private System.Windows.Forms.CheckBox chkAlarm;
		private System.Windows.Forms.CheckBox chkCpOff;
        private System.Windows.Forms.Timer tmrUpdateState;
    }
}

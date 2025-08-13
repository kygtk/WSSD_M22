namespace Dms.HMI
{
    partial class EqpStateManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EqpStateManager));
			this.switchButtonAlarmReset = new Dms.Control.SwitchButton();
			this.switchButtonBuzzerOff = new Dms.Control.SwitchButton();
			this.buzzer1 = new Dms.Control.Buzzer();
			this.signalTower1 = new Dms.Control.SignalTower();
			this.buttonShow = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// switchButtonAlarmReset
			// 
			this.switchButtonAlarmReset.BackColor = System.Drawing.Color.Transparent;
			this.switchButtonAlarmReset.ButtonTag = null;
			this.switchButtonAlarmReset.ButtonText = "Alarm Reset";
			this.switchButtonAlarmReset.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("switchButtonAlarmReset.DeviceTagInfo")));
			this.switchButtonAlarmReset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.switchButtonAlarmReset.LampOffColor = System.Drawing.Color.Tan;
			this.switchButtonAlarmReset.LampOnColor = System.Drawing.Color.Gold;
			this.switchButtonAlarmReset.Location = new System.Drawing.Point(8, 266);
			this.switchButtonAlarmReset.Name = "switchButtonAlarmReset";
			this.switchButtonAlarmReset.Size = new System.Drawing.Size(92, 69);
			this.switchButtonAlarmReset.TabIndex = 3;
			this.switchButtonAlarmReset.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.switchButtonAlarmReset.ButtonPushed += new System.EventHandler(this.switchButtonAlarmReset_ButtonPushed);
			this.switchButtonAlarmReset.ButtonReleased += new System.EventHandler(this.switchButtonAlarmReset_ButtonReleased);
			// 
			// switchButtonBuzzerOff
			// 
			this.switchButtonBuzzerOff.BackColor = System.Drawing.Color.Transparent;
			this.switchButtonBuzzerOff.ButtonTag = null;
			this.switchButtonBuzzerOff.ButtonText = "Buzzer Off";
			this.switchButtonBuzzerOff.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("switchButtonBuzzerOff.DeviceTagInfo")));
			this.switchButtonBuzzerOff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.switchButtonBuzzerOff.LampOffColor = System.Drawing.Color.Tan;
			this.switchButtonBuzzerOff.LampOnColor = System.Drawing.Color.Gold;
			this.switchButtonBuzzerOff.Location = new System.Drawing.Point(111, 266);
			this.switchButtonBuzzerOff.Name = "switchButtonBuzzerOff";
			this.switchButtonBuzzerOff.Size = new System.Drawing.Size(92, 69);
			this.switchButtonBuzzerOff.TabIndex = 4;
			this.switchButtonBuzzerOff.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.switchButtonBuzzerOff.ButtonPushed += new System.EventHandler(this.switchButtonBuzzerOff_ButtonPushed);
			this.switchButtonBuzzerOff.ButtonReleased += new System.EventHandler(this.switchButtonBuzzerOff_ButtonReleased);
			// 
			// buzzer1
			// 
			this.buzzer1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.buzzer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.buzzer1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("buzzer1.DeviceTagInfo")));
			this.buzzer1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buzzer1.Location = new System.Drawing.Point(111, 44);
			this.buzzer1.Name = "buzzer1";
			this.buzzer1.Size = new System.Drawing.Size(92, 214);
			this.buzzer1.TabIndex = 2;
			// 
			// signalTower1
			// 
			this.signalTower1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.signalTower1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.signalTower1.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("signalTower1.DeviceTagInfo")));
			this.signalTower1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.signalTower1.Lamp1OffColor = System.Drawing.Color.Maroon;
			this.signalTower1.Lamp1OnColor = System.Drawing.Color.Red;
			this.signalTower1.Lamp2OffColor = System.Drawing.Color.Olive;
			this.signalTower1.Lamp2OnColor = System.Drawing.Color.Yellow;
			this.signalTower1.Lamp3OffColor = System.Drawing.Color.Green;
			this.signalTower1.Lamp3OnColor = System.Drawing.Color.Lime;
			this.signalTower1.Lamp4OffColor = System.Drawing.Color.DarkBlue;
			this.signalTower1.Lamp4OnColor = System.Drawing.Color.Blue;
			this.signalTower1.Location = new System.Drawing.Point(8, 44);
			this.signalTower1.Name = "signalTower1";
			this.signalTower1.Size = new System.Drawing.Size(92, 214);
			this.signalTower1.TabIndex = 1;
			// 
			// buttonShow
			// 
			this.buttonShow.BackColor = System.Drawing.Color.Gold;
			this.buttonShow.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonShow.ForeColor = System.Drawing.Color.Black;
			this.buttonShow.Location = new System.Drawing.Point(0, 0);
			this.buttonShow.Name = "buttonShow";
			this.buttonShow.Size = new System.Drawing.Size(212, 35);
			this.buttonShow.TabIndex = 0;
			this.buttonShow.Text = "Hide";
			this.buttonShow.UseVisualStyleBackColor = false;
			this.buttonShow.Click += new System.EventHandler(this.buttonShow_Click);
			// 
			// EqpStateManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(212, 344);
			this.ControlBox = false;
			this.Controls.Add(this.buttonShow);
			this.Controls.Add(this.signalTower1);
			this.Controls.Add(this.buzzer1);
			this.Controls.Add(this.switchButtonBuzzerOff);
			this.Controls.Add(this.switchButtonAlarmReset);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "EqpStateManager";
			this.Opacity = 0.88;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EqpStateManager";
			this.Load += new System.EventHandler(this.EqpStateManager_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private Dms.Control.SwitchButton switchButtonAlarmReset;
        private Dms.Control.SwitchButton switchButtonBuzzerOff;
        private Dms.Control.Buzzer buzzer1;
        private Dms.Control.SignalTower signalTower1;
        private System.Windows.Forms.Button buttonShow;
    }
}
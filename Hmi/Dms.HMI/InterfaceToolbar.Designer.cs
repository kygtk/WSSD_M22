namespace Dms.HMI
{
    partial class InterfaceToolbar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InterfaceToolbar));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnManualIFSend = new Dms.Control.TagButton();
            this.btnNormal = new Dms.Control.TagButton();
            this.btnParticle = new Dms.Control.TagButton();
            this.btnRecovery = new Dms.Control.TagButton();
            this.btnLoopBack = new Dms.Control.TagButton();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.btnManualIFRecv = new Dms.Control.TagButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnManualIFSend);
            this.groupBox1.Controls.Add(this.btnManualIFRecv);
            this.groupBox1.Controls.Add(this.btnNormal);
            this.groupBox1.Controls.Add(this.btnParticle);
            this.groupBox1.Controls.Add(this.btnRecovery);
            this.groupBox1.Controls.Add(this.btnLoopBack);
            this.groupBox1.Location = new System.Drawing.Point(4, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(84, 594);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btnManualIFSend
            // 
            this.btnManualIFSend.BackColor = System.Drawing.Color.Transparent;
            this.btnManualIFSend.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnManualIFSend.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnManualIFSend.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManualIFSend.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnManualIFSend.Command = Dms.Common.Command.ManualIFsend;
            this.btnManualIFSend.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnManualIFSend.DeviceTagInfo")));
            this.btnManualIFSend.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnManualIFSend.Image = global::Dms.HMI.Properties.Resources.IFReset;
            this.btnManualIFSend.Location = new System.Drawing.Point(3, 408);
            this.btnManualIFSend.Name = "btnManualIFSend";
            this.btnManualIFSend.Size = new System.Drawing.Size(78, 66);
            this.btnManualIFSend.TabIndex = 9;
            this.btnManualIFSend.Text = "Manual IF Send";
            this.btnManualIFSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManualIFSend.UseVisualStyleBackColor = true;
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.Transparent;
            this.btnNormal.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnNormal.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnNormal.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNormal.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnNormal.Command = Dms.Common.Command.NormalMode;
            this.btnNormal.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnNormal.DeviceTagInfo")));
            this.btnNormal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnNormal.Image = global::Dms.HMI.Properties.Resources.Normal;
            this.btnNormal.Location = new System.Drawing.Point(3, 17);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(78, 66);
            this.btnNormal.TabIndex = 7;
            this.btnNormal.Text = "Normal";
            this.btnNormal.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNormal.UseVisualStyleBackColor = true;
            // 
            // btnParticle
            // 
            this.btnParticle.BackColor = System.Drawing.Color.Transparent;
            this.btnParticle.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnParticle.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnParticle.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnParticle.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnParticle.Command = Dms.Common.Command.ParticleMode;
            this.btnParticle.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnParticle.DeviceTagInfo")));
            this.btnParticle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnParticle.Image = global::Dms.HMI.Properties.Resources.Particle;
            this.btnParticle.Location = new System.Drawing.Point(3, 149);
            this.btnParticle.Name = "btnParticle";
            this.btnParticle.Size = new System.Drawing.Size(78, 66);
            this.btnParticle.TabIndex = 6;
            this.btnParticle.Text = "Particle";
            this.btnParticle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnParticle.UseVisualStyleBackColor = true;
            // 
            // btnRecovery
            // 
            this.btnRecovery.BackColor = System.Drawing.Color.Transparent;
            this.btnRecovery.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnRecovery.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnRecovery.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecovery.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnRecovery.Command = Dms.Common.Command.RecoveryMode;
            this.btnRecovery.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnRecovery.DeviceTagInfo")));
            this.btnRecovery.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRecovery.Image = global::Dms.HMI.Properties.Resources.Recovery;
            this.btnRecovery.Location = new System.Drawing.Point(3, 83);
            this.btnRecovery.Name = "btnRecovery";
            this.btnRecovery.Size = new System.Drawing.Size(78, 66);
            this.btnRecovery.TabIndex = 5;
            this.btnRecovery.Text = "Recovery";
            this.btnRecovery.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecovery.UseVisualStyleBackColor = true;
            // 
            // btnLoopBack
            // 
            this.btnLoopBack.BackColor = System.Drawing.Color.Transparent;
            this.btnLoopBack.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnLoopBack.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnLoopBack.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoopBack.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnLoopBack.Command = Dms.Common.Command.LoopBack;
            this.btnLoopBack.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnLoopBack.DeviceTagInfo")));
            this.btnLoopBack.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoopBack.Image = global::Dms.HMI.Properties.Resources.LoopBack4;
            this.btnLoopBack.Location = new System.Drawing.Point(3, 253);
            this.btnLoopBack.Name = "btnLoopBack";
            this.btnLoopBack.Size = new System.Drawing.Size(78, 66);
            this.btnLoopBack.TabIndex = 4;
            this.btnLoopBack.Text = "LoopBack";
            this.btnLoopBack.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoopBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLoopBack.UseVisualStyleBackColor = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // btnManualIFRecv
            // 
            this.btnManualIFRecv.BackColor = System.Drawing.Color.Transparent;
            this.btnManualIFRecv.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnManualIFRecv.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnManualIFRecv.ButtonTextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManualIFRecv.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnManualIFRecv.Command = Dms.Common.Command.ManualIFrecv;
            this.btnManualIFRecv.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnManualIFRecv.DeviceTagInfo")));
            this.btnManualIFRecv.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnManualIFRecv.Image = global::Dms.HMI.Properties.Resources.IFReset1;
            this.btnManualIFRecv.Location = new System.Drawing.Point(3, 336);
            this.btnManualIFRecv.Name = "btnManualIFRecv";
            this.btnManualIFRecv.Size = new System.Drawing.Size(78, 66);
            this.btnManualIFRecv.TabIndex = 8;
            this.btnManualIFRecv.Text = "Manual IF Recv";
            this.btnManualIFRecv.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManualIFRecv.UseVisualStyleBackColor = true;
            // 
            // InterfaceToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBox1);
            this.Name = "InterfaceToolbar";
            this.Size = new System.Drawing.Size(92, 595);
            this.Load += new System.EventHandler(this.InterfaceToolbar_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer tmrUpdateState;
        private Dms.Control.TagButton btnNormal;
        private Dms.Control.TagButton btnParticle;
        private Dms.Control.TagButton btnRecovery;
        private Dms.Control.TagButton btnLoopBack;
        private Dms.Control.TagButton btnManualIFSend;
        private Dms.Control.TagButton btnManualIFRecv;



    }
}

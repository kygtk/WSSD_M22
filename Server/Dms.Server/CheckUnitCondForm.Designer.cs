namespace Dms.Server
{
    partial class CheckUnitCondForm
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
            this.UnitCondList = new System.Windows.Forms.ListBox();
            this.tmrCheckUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonShow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UnitCondList
            // 
            this.UnitCondList.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.UnitCondList.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.UnitCondList.ForeColor = System.Drawing.Color.DarkRed;
            this.UnitCondList.FormattingEnabled = true;
            this.UnitCondList.Location = new System.Drawing.Point(-1, 27);
            this.UnitCondList.Name = "UnitCondList";
            this.UnitCondList.Size = new System.Drawing.Size(295, 238);
            this.UnitCondList.TabIndex = 0;
            // 
            // tmrCheckUpdate
            // 
            this.tmrCheckUpdate.Enabled = true;
            this.tmrCheckUpdate.Tick += new System.EventHandler(this.tmrCheckUnitcondTick);
            // 
            // buttonShow
            // 
            this.buttonShow.BackColor = System.Drawing.Color.Gold;
            this.buttonShow.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShow.ForeColor = System.Drawing.Color.Black;
            this.buttonShow.Location = new System.Drawing.Point(-1, -1);
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(295, 24);
            this.buttonShow.TabIndex = 2;
            this.buttonShow.Text = "Hide";
            this.buttonShow.UseVisualStyleBackColor = false;
            this.buttonShow.Click += new System.EventHandler(this.Show_Click);
            // 
            // CheckUnitCondForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(294, 266);
            this.ControlBox = false;
            this.Controls.Add(this.buttonShow);
            this.Controls.Add(this.UnitCondList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckUnitCondForm";
            this.Opacity = 0.8;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CheckUnitCondForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormInitStatus_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox UnitCondList;
        private System.Windows.Forms.Timer tmrCheckUpdate;
        private System.Windows.Forms.Button buttonShow;



    }
}
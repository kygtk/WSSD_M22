namespace Dms.HMI
{
    partial class JobsTabGauge
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
            this.GaugeAlarmTimer = new System.Windows.Forms.Timer(this.components);
            this.btngaugeAlarm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.alarmcount = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewPlasma = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Plasmaalarmcount = new System.Windows.Forms.TextBox();
            this.btnPlasmaAlarmReset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlasma)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GaugeAlarmTimer
            // 
            this.GaugeAlarmTimer.Interval = 300;
            this.GaugeAlarmTimer.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // btngaugeAlarm
            // 
            this.btngaugeAlarm.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btngaugeAlarm.Location = new System.Drawing.Point(28, 409);
            this.btngaugeAlarm.Name = "btngaugeAlarm";
            this.btngaugeAlarm.Size = new System.Drawing.Size(112, 38);
            this.btngaugeAlarm.TabIndex = 129;
            this.btngaugeAlarm.Text = "Gauge List Clear";
            this.btngaugeAlarm.UseVisualStyleBackColor = true;
            this.btngaugeAlarm.Click += new System.EventHandler(this.GaugeAlarmClear);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.alarmcount);
            this.groupBox1.Location = new System.Drawing.Point(146, 409);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 42);
            this.groupBox1.TabIndex = 130;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Count view";
            // 
            // alarmcount
            // 
            this.alarmcount.BackColor = System.Drawing.SystemColors.Window;
            this.alarmcount.Location = new System.Drawing.Point(6, 15);
            this.alarmcount.Name = "alarmcount";
            this.alarmcount.Size = new System.Drawing.Size(131, 21);
            this.alarmcount.TabIndex = 131;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(28, 53);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(499, 350);
            this.dataGridView.TabIndex = 131;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(28, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 38);
            this.label1.TabIndex = 132;
            this.label1.Text = "Gauge Alarm List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewPlasma
            // 
            this.dataGridViewPlasma.AllowUserToAddRows = false;
            this.dataGridViewPlasma.AllowUserToDeleteRows = false;
            this.dataGridViewPlasma.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPlasma.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlasma.Location = new System.Drawing.Point(529, 53);
            this.dataGridViewPlasma.MultiSelect = false;
            this.dataGridViewPlasma.Name = "dataGridViewPlasma";
            this.dataGridViewPlasma.ReadOnly = true;
            this.dataGridViewPlasma.RowHeadersVisible = false;
            this.dataGridViewPlasma.RowTemplate.Height = 23;
            this.dataGridViewPlasma.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewPlasma.Size = new System.Drawing.Size(368, 350);
            this.dataGridViewPlasma.TabIndex = 133;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(529, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 38);
            this.label2.TabIndex = 134;
            this.label2.Text = "Plasma Gauge Alarm List";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Plasmaalarmcount);
            this.groupBox2.Location = new System.Drawing.Point(651, 407);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(143, 42);
            this.groupBox2.TabIndex = 136;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Count view";
            // 
            // Plasmaalarmcount
            // 
            this.Plasmaalarmcount.BackColor = System.Drawing.SystemColors.Window;
            this.Plasmaalarmcount.Location = new System.Drawing.Point(6, 15);
            this.Plasmaalarmcount.Name = "Plasmaalarmcount";
            this.Plasmaalarmcount.Size = new System.Drawing.Size(131, 21);
            this.Plasmaalarmcount.TabIndex = 131;
            // 
            // btnPlasmaAlarmReset
            // 
            this.btnPlasmaAlarmReset.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlasmaAlarmReset.Location = new System.Drawing.Point(533, 407);
            this.btnPlasmaAlarmReset.Name = "btnPlasmaAlarmReset";
            this.btnPlasmaAlarmReset.Size = new System.Drawing.Size(112, 38);
            this.btnPlasmaAlarmReset.TabIndex = 135;
            this.btnPlasmaAlarmReset.Text = "Plasma List Clear";
            this.btnPlasmaAlarmReset.UseVisualStyleBackColor = true;
            this.btnPlasmaAlarmReset.Click += new System.EventHandler(this.APGaugeAlarmClear);
            // 
            // JobsTabGauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnPlasmaAlarmReset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewPlasma);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btngaugeAlarm);
            this.Name = "JobsTabGauge";
            this.Size = new System.Drawing.Size(901, 463);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlasma)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer GaugeAlarmTimer;
        private System.Windows.Forms.Button btngaugeAlarm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox alarmcount;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewPlasma;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox Plasmaalarmcount;
        private System.Windows.Forms.Button btnPlasmaAlarmReset;

    }
}

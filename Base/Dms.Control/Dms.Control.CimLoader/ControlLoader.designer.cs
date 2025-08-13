namespace Dms.Control
{
    partial class ControlLoader
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRechuck = new System.Windows.Forms.Button();
            this.btnUnloadRequest = new System.Windows.Forms.Button();
            this.btnLoadRequest = new System.Windows.Forms.Button();
            this.btnCstId = new System.Windows.Forms.Button();
            this.btnMapping = new System.Windows.Forms.Button();
            this.cbPortNo = new System.Windows.Forms.ComboBox();
            this.btnPort1Disable = new System.Windows.Forms.Button();
            this.btnPort1Enable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOperatorCall = new System.Windows.Forms.Button();
            this.btnMgv = new System.Windows.Forms.Button();
            this.btnAlarmReset = new System.Windows.Forms.Button();
            this.btnBuzzerOff = new System.Windows.Forms.Button();
            this.btnAgv = new System.Windows.Forms.Button();
            this.btnRemoteChange = new System.Windows.Forms.Button();
            this.btnDateandTimeSet = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRobotHome = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbSlotNo = new System.Windows.Forms.ComboBox();
            this.cbRobotHand = new System.Windows.Forms.ComboBox();
            this.cbRobotCommand = new System.Windows.Forms.ComboBox();
            this.cbStageNo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbThickness = new System.Windows.Forms.ComboBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.dataGridViewInfo = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblUpperHand = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRechuck);
            this.groupBox1.Controls.Add(this.btnUnloadRequest);
            this.groupBox1.Controls.Add(this.btnLoadRequest);
            this.groupBox1.Controls.Add(this.btnCstId);
            this.groupBox1.Controls.Add(this.btnMapping);
            this.groupBox1.Controls.Add(this.cbPortNo);
            this.groupBox1.Controls.Add(this.btnPort1Disable);
            this.groupBox1.Controls.Add(this.btnPort1Enable);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(622, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 204);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PORT ACTION";
            // 
            // btnRechuck
            // 
            this.btnRechuck.Location = new System.Drawing.Point(140, 126);
            this.btnRechuck.Name = "btnRechuck";
            this.btnRechuck.Size = new System.Drawing.Size(127, 32);
            this.btnRechuck.TabIndex = 10;
            this.btnRechuck.Text = "RECHUCK";
            this.btnRechuck.UseVisualStyleBackColor = true;
            this.btnRechuck.Click += new System.EventHandler(this.btnRechuck_Click);
            // 
            // btnUnloadRequest
            // 
            this.btnUnloadRequest.Location = new System.Drawing.Point(140, 90);
            this.btnUnloadRequest.Name = "btnUnloadRequest";
            this.btnUnloadRequest.Size = new System.Drawing.Size(127, 32);
            this.btnUnloadRequest.TabIndex = 9;
            this.btnUnloadRequest.Text = "UNLOAD REQUEST";
            this.btnUnloadRequest.UseVisualStyleBackColor = true;
            this.btnUnloadRequest.Click += new System.EventHandler(this.btnUnloadRequest_Click);
            // 
            // btnLoadRequest
            // 
            this.btnLoadRequest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadRequest.Location = new System.Drawing.Point(140, 52);
            this.btnLoadRequest.Name = "btnLoadRequest";
            this.btnLoadRequest.Size = new System.Drawing.Size(127, 32);
            this.btnLoadRequest.TabIndex = 8;
            this.btnLoadRequest.Text = "LOAD REQUEST";
            this.btnLoadRequest.UseVisualStyleBackColor = true;
            this.btnLoadRequest.Click += new System.EventHandler(this.btnLoadRequest_Click);
            // 
            // btnCstId
            // 
            this.btnCstId.Location = new System.Drawing.Point(8, 164);
            this.btnCstId.Name = "btnCstId";
            this.btnCstId.Size = new System.Drawing.Size(127, 32);
            this.btnCstId.TabIndex = 7;
            this.btnCstId.Text = "CST ID";
            this.btnCstId.UseVisualStyleBackColor = true;
            this.btnCstId.Click += new System.EventHandler(this.btnCstId_Click);
            // 
            // btnMapping
            // 
            this.btnMapping.Location = new System.Drawing.Point(8, 126);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(127, 32);
            this.btnMapping.TabIndex = 6;
            this.btnMapping.Text = "MAPPING";
            this.btnMapping.UseVisualStyleBackColor = true;
            this.btnMapping.Click += new System.EventHandler(this.btnMapping_Click);
            // 
            // cbPortNo
            // 
            this.cbPortNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPortNo.FormattingEnabled = true;
            this.cbPortNo.Location = new System.Drawing.Point(81, 23);
            this.cbPortNo.Name = "cbPortNo";
            this.cbPortNo.Size = new System.Drawing.Size(121, 23);
            this.cbPortNo.TabIndex = 5;
            this.cbPortNo.SelectedIndexChanged += new System.EventHandler(this.cbPortNo_SelectedIndexChanged);
            // 
            // btnPort1Disable
            // 
            this.btnPort1Disable.Location = new System.Drawing.Point(8, 90);
            this.btnPort1Disable.Name = "btnPort1Disable";
            this.btnPort1Disable.Size = new System.Drawing.Size(127, 32);
            this.btnPort1Disable.TabIndex = 4;
            this.btnPort1Disable.Text = "DISABLE";
            this.btnPort1Disable.UseVisualStyleBackColor = true;
            this.btnPort1Disable.Click += new System.EventHandler(this.btnPort1Disable_Click);
            // 
            // btnPort1Enable
            // 
            this.btnPort1Enable.Location = new System.Drawing.Point(8, 52);
            this.btnPort1Enable.Name = "btnPort1Enable";
            this.btnPort1Enable.Size = new System.Drawing.Size(127, 32);
            this.btnPort1Enable.TabIndex = 3;
            this.btnPort1Enable.Text = "ENABLE";
            this.btnPort1Enable.UseVisualStyleBackColor = true;
            this.btnPort1Enable.Click += new System.EventHandler(this.btnPort1Enable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "PORT NO : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOperatorCall);
            this.groupBox2.Controls.Add(this.btnMgv);
            this.groupBox2.Controls.Add(this.btnAlarmReset);
            this.groupBox2.Controls.Add(this.btnBuzzerOff);
            this.groupBox2.Controls.Add(this.btnAgv);
            this.groupBox2.Controls.Add(this.btnRemoteChange);
            this.groupBox2.Controls.Add(this.btnDateandTimeSet);
            this.groupBox2.Location = new System.Drawing.Point(622, 217);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(275, 146);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LOADER ACTION";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnOperatorCall
            // 
            this.btnOperatorCall.Location = new System.Drawing.Point(169, 96);
            this.btnOperatorCall.Name = "btnOperatorCall";
            this.btnOperatorCall.Size = new System.Drawing.Size(100, 32);
            this.btnOperatorCall.TabIndex = 14;
            this.btnOperatorCall.Text = "Operator Call";
            this.btnOperatorCall.UseVisualStyleBackColor = true;
            this.btnOperatorCall.Click += new System.EventHandler(this.btnOperatorCall_Click);
            // 
            // btnMgv
            // 
            this.btnMgv.Location = new System.Drawing.Point(87, 96);
            this.btnMgv.Name = "btnMgv";
            this.btnMgv.Size = new System.Drawing.Size(77, 32);
            this.btnMgv.TabIndex = 9;
            this.btnMgv.Text = "MGV";
            this.btnMgv.UseVisualStyleBackColor = true;
            this.btnMgv.Click += new System.EventHandler(this.btnMgv_Click);
            // 
            // btnAlarmReset
            // 
            this.btnAlarmReset.Location = new System.Drawing.Point(169, 58);
            this.btnAlarmReset.Name = "btnAlarmReset";
            this.btnAlarmReset.Size = new System.Drawing.Size(100, 32);
            this.btnAlarmReset.TabIndex = 13;
            this.btnAlarmReset.Text = "Alarm Reset";
            this.btnAlarmReset.UseVisualStyleBackColor = true;
            // 
            // btnBuzzerOff
            // 
            this.btnBuzzerOff.Location = new System.Drawing.Point(169, 20);
            this.btnBuzzerOff.Name = "btnBuzzerOff";
            this.btnBuzzerOff.Size = new System.Drawing.Size(100, 32);
            this.btnBuzzerOff.TabIndex = 12;
            this.btnBuzzerOff.Text = "Buzzer OFF";
            this.btnBuzzerOff.UseVisualStyleBackColor = true;
            this.btnBuzzerOff.Click += new System.EventHandler(this.btnBuzzerOff_Click);
            // 
            // btnAgv
            // 
            this.btnAgv.Location = new System.Drawing.Point(7, 97);
            this.btnAgv.Name = "btnAgv";
            this.btnAgv.Size = new System.Drawing.Size(76, 32);
            this.btnAgv.TabIndex = 8;
            this.btnAgv.Text = "AGV";
            this.btnAgv.UseVisualStyleBackColor = true;
            this.btnAgv.Click += new System.EventHandler(this.btnAgv_Click);
            // 
            // btnRemoteChange
            // 
            this.btnRemoteChange.Location = new System.Drawing.Point(7, 59);
            this.btnRemoteChange.Name = "btnRemoteChange";
            this.btnRemoteChange.Size = new System.Drawing.Size(157, 32);
            this.btnRemoteChange.TabIndex = 11;
            this.btnRemoteChange.Text = "Remote Mode Change";
            this.btnRemoteChange.UseVisualStyleBackColor = true;
            this.btnRemoteChange.Click += new System.EventHandler(this.btnRemoteChange_Click);
            // 
            // btnDateandTimeSet
            // 
            this.btnDateandTimeSet.Location = new System.Drawing.Point(7, 20);
            this.btnDateandTimeSet.Name = "btnDateandTimeSet";
            this.btnDateandTimeSet.Size = new System.Drawing.Size(157, 32);
            this.btnDateandTimeSet.TabIndex = 10;
            this.btnDateandTimeSet.Text = "DateTime Set Reqeust";
            this.btnDateandTimeSet.UseVisualStyleBackColor = true;
            this.btnDateandTimeSet.Click += new System.EventHandler(this.btnDateandTimeSet_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRobotHome);
            this.groupBox3.Location = new System.Drawing.Point(622, 378);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 67);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ROBOT ACTION";
            // 
            // btnRobotHome
            // 
            this.btnRobotHome.Location = new System.Drawing.Point(12, 20);
            this.btnRobotHome.Name = "btnRobotHome";
            this.btnRobotHome.Size = new System.Drawing.Size(156, 32);
            this.btnRobotHome.TabIndex = 0;
            this.btnRobotHome.Text = "HOME";
            this.btnRobotHome.UseVisualStyleBackColor = true;
            this.btnRobotHome.Click += new System.EventHandler(this.btnRobotHome_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.cbSlotNo);
            this.groupBox4.Controls.Add(this.cbRobotHand);
            this.groupBox4.Controls.Add(this.cbRobotCommand);
            this.groupBox4.Controls.Add(this.cbStageNo);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.cbThickness);
            this.groupBox4.Controls.Add(this.btnMove);
            this.groupBox4.Location = new System.Drawing.Point(363, 175);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 361);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ROBOT GLASS MOVING";
            // 
            // cbSlotNo
            // 
            this.cbSlotNo.FormattingEnabled = true;
            this.cbSlotNo.Location = new System.Drawing.Point(106, 113);
            this.cbSlotNo.Name = "cbSlotNo";
            this.cbSlotNo.Size = new System.Drawing.Size(121, 23);
            this.cbSlotNo.TabIndex = 6;
            // 
            // cbRobotHand
            // 
            this.cbRobotHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRobotHand.FormattingEnabled = true;
            this.cbRobotHand.Location = new System.Drawing.Point(106, 53);
            this.cbRobotHand.Name = "cbRobotHand";
            this.cbRobotHand.Size = new System.Drawing.Size(121, 23);
            this.cbRobotHand.TabIndex = 5;
            // 
            // cbRobotCommand
            // 
            this.cbRobotCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRobotCommand.FormattingEnabled = true;
            this.cbRobotCommand.Location = new System.Drawing.Point(106, 27);
            this.cbRobotCommand.Name = "cbRobotCommand";
            this.cbRobotCommand.Size = new System.Drawing.Size(121, 23);
            this.cbRobotCommand.TabIndex = 5;
            // 
            // cbStageNo
            // 
            this.cbStageNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStageNo.FormattingEnabled = true;
            this.cbStageNo.Location = new System.Drawing.Point(106, 86);
            this.cbStageNo.Name = "cbStageNo";
            this.cbStageNo.Size = new System.Drawing.Size(121, 23);
            this.cbStageNo.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Robot Hand :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Slot No :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Command :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stage No :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Thickness :";
            // 
            // cbThickness
            // 
            this.cbThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThickness.FormattingEnabled = true;
            this.cbThickness.Location = new System.Drawing.Point(106, 143);
            this.cbThickness.Name = "cbThickness";
            this.cbThickness.Size = new System.Drawing.Size(121, 23);
            this.cbThickness.TabIndex = 2;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(24, 193);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(200, 92);
            this.btnMove.TabIndex = 0;
            this.btnMove.Text = "MOVE";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // dataGridViewInfo
            // 
            this.dataGridViewInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewInfo.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewInfo.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewInfo.Name = "dataGridViewInfo";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewInfo.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewInfo.RowTemplate.Height = 23;
            this.dataGridViewInfo.Size = new System.Drawing.Size(347, 533);
            this.dataGridViewInfo.TabIndex = 11;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblUpperHand);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Location = new System.Drawing.Point(363, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(247, 164);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Robot Hand Infomation";
            // 
            // lblUpperHand
            // 
            this.lblUpperHand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUpperHand.Location = new System.Drawing.Point(20, 50);
            this.lblUpperHand.Name = "lblUpperHand";
            this.lblUpperHand.Size = new System.Drawing.Size(152, 25);
            this.lblUpperHand.TabIndex = 1;
            this.lblUpperHand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "UPPER HAND";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // ControlLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.dataGridViewInfo);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlLoader";
            this.Size = new System.Drawing.Size(903, 539);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPort1Disable;
        private System.Windows.Forms.Button btnPort1Enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPortNo;
        private System.Windows.Forms.Button btnUnloadRequest;
        private System.Windows.Forms.Button btnLoadRequest;
        private System.Windows.Forms.Button btnCstId;
        private System.Windows.Forms.Button btnMapping;
        private System.Windows.Forms.Button btnRechuck;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOperatorCall;
        private System.Windows.Forms.Button btnMgv;
        private System.Windows.Forms.Button btnAlarmReset;
        private System.Windows.Forms.Button btnBuzzerOff;
        private System.Windows.Forms.Button btnAgv;
        private System.Windows.Forms.Button btnRemoteChange;
        private System.Windows.Forms.Button btnDateandTimeSet;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRobotHome;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.ComboBox cbThickness;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbStageNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSlotNo;
        private System.Windows.Forms.ComboBox cbRobotCommand;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbRobotHand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridViewInfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblUpperHand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}

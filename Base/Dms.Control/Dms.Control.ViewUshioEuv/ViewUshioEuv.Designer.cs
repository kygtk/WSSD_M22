namespace Dms.Control
{
    partial class ViewUshioEuv
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.dataEuvUsedTimeGridView = new Dms.Control.DoubleBufferedGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataLampIntensityGridView = new Dms.Control.DoubleBufferedGridView();
            this.chkDown = new System.Windows.Forms.CheckBox();
            this.btnCylDown = new System.Windows.Forms.Button();
            this.btnCylUp = new System.Windows.Forms.Button();
            this.chkUp = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkLamp1Select = new System.Windows.Forms.CheckBox();
            this.chkLamp2Select = new System.Windows.Forms.CheckBox();
            this.gbLampPower = new System.Windows.Forms.GroupBox();
            this.checkULON = new System.Windows.Forms.CheckBox();
            this.pbLamp3Status = new System.Windows.Forms.PictureBox();
            this.pbLamp2Status = new System.Windows.Forms.PictureBox();
            this.pbLamp1Status = new System.Windows.Forms.PictureBox();
            this.chkLamp3Select = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkPCWOutOpen = new System.Windows.Forms.CheckBox();
            this.chkPCWInOpen = new System.Windows.Forms.CheckBox();
            this.chkCDAInOpen = new System.Windows.Forms.CheckBox();
            this.chkN2InOpen = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkULST = new System.Windows.Forms.CheckBox();
            this.checkUICR = new System.Windows.Forms.CheckBox();
            this.checkUERT = new System.Windows.Forms.CheckBox();
            this.checkUEMG = new System.Windows.Forms.CheckBox();
            this.checkUTRE = new System.Windows.Forms.CheckBox();
            this.checkUDLC = new System.Windows.Forms.CheckBox();
            this.gbEuvStatus = new System.Windows.Forms.GroupBox();
            this.chkURDY = new System.Windows.Forms.CheckBox();
            this.chkULAC = new System.Windows.Forms.CheckBox();
            this.chkUCRE = new System.Windows.Forms.CheckBox();
            this.chkUNRE = new System.Windows.Forms.CheckBox();
            this.gbAlmStatus = new System.Windows.Forms.GroupBox();
            this.chkEMO = new System.Windows.Forms.CheckBox();
            this.chkControlAnomaly = new System.Windows.Forms.CheckBox();
            this.chkElecCoverOpen = new System.Windows.Forms.CheckBox();
            this.chkN2PressDrop = new System.Windows.Forms.CheckBox();
            this.chkTransTemp = new System.Windows.Forms.CheckBox();
            this.chkLampOpen = new System.Windows.Forms.CheckBox();
            this.chkLampTemp = new System.Windows.Forms.CheckBox();
            this.chkN2PressSurplus = new System.Windows.Forms.CheckBox();
            this.chkCircuitAnomaly = new System.Windows.Forms.CheckBox();
            this.chkProtective = new System.Windows.Forms.CheckBox();
            this.chkLampFail = new System.Windows.Forms.CheckBox();
            this.chkLampTime = new System.Windows.Forms.CheckBox();
            this.tmrUpdateState = new System.Windows.Forms.Timer(this.components);
            this.gbStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataEuvUsedTimeGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLampIntensityGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbLampPower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp3Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp2Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp1Status)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gbEuvStatus.SuspendLayout();
            this.gbAlmStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTitle.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTitle.Location = new System.Drawing.Point(11, 32);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(490, 26);
            this.labelTitle.TabIndex = 26;
            this.labelTitle.Text = " EUV Manual Operation View";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.dataEuvUsedTimeGridView);
            this.gbStatus.Location = new System.Drawing.Point(14, 69);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(218, 100);
            this.gbStatus.TabIndex = 22;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "EUV Used Time";
            // 
            // dataEuvUsedTimeGridView
            // 
            this.dataEuvUsedTimeGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataEuvUsedTimeGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataEuvUsedTimeGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataEuvUsedTimeGridView.Location = new System.Drawing.Point(11, 18);
            this.dataEuvUsedTimeGridView.MultiSelect = false;
            this.dataEuvUsedTimeGridView.Name = "dataEuvUsedTimeGridView";
            this.dataEuvUsedTimeGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataEuvUsedTimeGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataEuvUsedTimeGridView.RowHeadersVisible = false;
            this.dataEuvUsedTimeGridView.RowHeadersWidth = 5;
            this.dataEuvUsedTimeGridView.RowTemplate.Height = 23;
            this.dataEuvUsedTimeGridView.Size = new System.Drawing.Size(196, 76);
            this.dataEuvUsedTimeGridView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataLampIntensityGridView);
            this.groupBox1.Location = new System.Drawing.Point(14, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 100);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lamp Intensity";
            // 
            // dataLampIntensityGridView
            // 
            this.dataLampIntensityGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLampIntensityGridView.Location = new System.Drawing.Point(11, 17);
            this.dataLampIntensityGridView.Name = "dataLampIntensityGridView";
            this.dataLampIntensityGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataLampIntensityGridView.RowHeadersVisible = false;
            this.dataLampIntensityGridView.RowHeadersWidth = 5;
            this.dataLampIntensityGridView.RowTemplate.Height = 23;
            this.dataLampIntensityGridView.Size = new System.Drawing.Size(196, 76);
            this.dataLampIntensityGridView.TabIndex = 1;
            // 
            // chkDown
            // 
            this.chkDown.AutoCheck = false;
            this.chkDown.AutoSize = true;
            this.chkDown.Location = new System.Drawing.Point(104, 26);
            this.chkDown.Name = "chkDown";
            this.chkDown.Size = new System.Drawing.Size(94, 17);
            this.chkDown.TabIndex = 3;
            this.chkDown.Text = "Down Position";
            this.chkDown.UseVisualStyleBackColor = true;
            // 
            // btnCylDown
            // 
            this.btnCylDown.BackColor = System.Drawing.Color.LightCyan;
            this.btnCylDown.Location = new System.Drawing.Point(112, 48);
            this.btnCylDown.Name = "btnCylDown";
            this.btnCylDown.Size = new System.Drawing.Size(85, 57);
            this.btnCylDown.TabIndex = 2;
            this.btnCylDown.Text = "DOWN";
            this.btnCylDown.UseVisualStyleBackColor = false;
            this.btnCylDown.Click += new System.EventHandler(this.btnCylDown_Click);
            this.btnCylDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnClick);
            this.btnCylDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnRelease);
            // 
            // btnCylUp
            // 
            this.btnCylUp.BackColor = System.Drawing.Color.Linen;
            this.btnCylUp.Location = new System.Drawing.Point(13, 48);
            this.btnCylUp.Name = "btnCylUp";
            this.btnCylUp.Size = new System.Drawing.Size(85, 57);
            this.btnCylUp.TabIndex = 1;
            this.btnCylUp.Text = "UP";
            this.btnCylUp.UseVisualStyleBackColor = false;
            this.btnCylUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnClick);
            this.btnCylUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HouseUpdnRelease);
            // 
            // chkUp
            // 
            this.chkUp.AutoCheck = false;
            this.chkUp.AutoSize = true;
            this.chkUp.Location = new System.Drawing.Point(13, 26);
            this.chkUp.Name = "chkUp";
            this.chkUp.Size = new System.Drawing.Size(80, 17);
            this.chkUp.TabIndex = 0;
            this.chkUp.Text = "Up Position";
            this.chkUp.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDown);
            this.groupBox2.Controls.Add(this.btnCylDown);
            this.groupBox2.Controls.Add(this.chkUp);
            this.groupBox2.Controls.Add(this.btnCylUp);
            this.groupBox2.Location = new System.Drawing.Point(14, 294);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 121);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EUV House";
            // 
            // chkLamp1Select
            // 
            this.chkLamp1Select.AutoSize = true;
            this.chkLamp1Select.Location = new System.Drawing.Point(48, 20);
            this.chkLamp1Select.Name = "chkLamp1Select";
            this.chkLamp1Select.Size = new System.Drawing.Size(91, 17);
            this.chkLamp1Select.TabIndex = 21;
            this.chkLamp1Select.Text = "Lamp1 Select";
            this.chkLamp1Select.UseVisualStyleBackColor = true;
            this.chkLamp1Select.CheckedChanged += new System.EventHandler(this.chkLamp1Select_CheckedChanged);
            // 
            // chkLamp2Select
            // 
            this.chkLamp2Select.AutoSize = true;
            this.chkLamp2Select.Location = new System.Drawing.Point(48, 41);
            this.chkLamp2Select.Name = "chkLamp2Select";
            this.chkLamp2Select.Size = new System.Drawing.Size(91, 17);
            this.chkLamp2Select.TabIndex = 28;
            this.chkLamp2Select.Text = "Lamp2 Select";
            this.chkLamp2Select.UseVisualStyleBackColor = true;
            this.chkLamp2Select.CheckedChanged += new System.EventHandler(this.chkLamp2Select_CheckedChanged);
            // 
            // gbLampPower
            // 
            this.gbLampPower.Controls.Add(this.checkULON);
            this.gbLampPower.Controls.Add(this.pbLamp3Status);
            this.gbLampPower.Controls.Add(this.pbLamp2Status);
            this.gbLampPower.Controls.Add(this.pbLamp1Status);
            this.gbLampPower.Controls.Add(this.chkLamp3Select);
            this.gbLampPower.Controls.Add(this.chkLamp1Select);
            this.gbLampPower.Controls.Add(this.chkLamp2Select);
            this.gbLampPower.Location = new System.Drawing.Point(244, 69);
            this.gbLampPower.Name = "gbLampPower";
            this.gbLampPower.Size = new System.Drawing.Size(166, 142);
            this.gbLampPower.TabIndex = 29;
            this.gbLampPower.TabStop = false;
            this.gbLampPower.Text = "Lamp Select";
            // 
            // checkULON
            // 
            this.checkULON.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkULON.BackColor = System.Drawing.Color.GreenYellow;
            this.checkULON.Checked = true;
            this.checkULON.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkULON.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkULON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkULON.Location = new System.Drawing.Point(21, 89);
            this.checkULON.Name = "checkULON";
            this.checkULON.Size = new System.Drawing.Size(128, 40);
            this.checkULON.TabIndex = 33;
            this.checkULON.Text = "LAMP ON";
            this.checkULON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkULON.UseVisualStyleBackColor = false;
            this.checkULON.Click += new System.EventHandler(this.check_click);
            // 
            // pbLamp3Status
            // 
            this.pbLamp3Status.BackColor = System.Drawing.Color.DarkGray;
            this.pbLamp3Status.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbLamp3Status.Location = new System.Drawing.Point(21, 63);
            this.pbLamp3Status.Name = "pbLamp3Status";
            this.pbLamp3Status.Size = new System.Drawing.Size(20, 17);
            this.pbLamp3Status.TabIndex = 34;
            this.pbLamp3Status.TabStop = false;
            // 
            // pbLamp2Status
            // 
            this.pbLamp2Status.BackColor = System.Drawing.Color.DarkGray;
            this.pbLamp2Status.Location = new System.Drawing.Point(21, 40);
            this.pbLamp2Status.Name = "pbLamp2Status";
            this.pbLamp2Status.Size = new System.Drawing.Size(20, 17);
            this.pbLamp2Status.TabIndex = 34;
            this.pbLamp2Status.TabStop = false;
            // 
            // pbLamp1Status
            // 
            this.pbLamp1Status.BackColor = System.Drawing.Color.DarkGray;
            this.pbLamp1Status.Location = new System.Drawing.Point(21, 19);
            this.pbLamp1Status.Name = "pbLamp1Status";
            this.pbLamp1Status.Size = new System.Drawing.Size(20, 17);
            this.pbLamp1Status.TabIndex = 33;
            this.pbLamp1Status.TabStop = false;
            // 
            // chkLamp3Select
            // 
            this.chkLamp3Select.AutoSize = true;
            this.chkLamp3Select.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkLamp3Select.Location = new System.Drawing.Point(48, 64);
            this.chkLamp3Select.Name = "chkLamp3Select";
            this.chkLamp3Select.Size = new System.Drawing.Size(91, 17);
            this.chkLamp3Select.TabIndex = 28;
            this.chkLamp3Select.Text = "Lamp3 Select";
            this.chkLamp3Select.UseVisualStyleBackColor = true;
            this.chkLamp3Select.CheckedChanged += new System.EventHandler(this.chkLamp3Select_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkPCWOutOpen);
            this.groupBox4.Controls.Add(this.chkPCWInOpen);
            this.groupBox4.Controls.Add(this.chkCDAInOpen);
            this.groupBox4.Controls.Add(this.chkN2InOpen);
            this.groupBox4.Location = new System.Drawing.Point(244, 217);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(166, 198);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "UT Valve Operation";
            // 
            // chkPCWOutOpen
            // 
            this.chkPCWOutOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPCWOutOpen.BackColor = System.Drawing.Color.LightSteelBlue;
            this.chkPCWOutOpen.Checked = true;
            this.chkPCWOutOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPCWOutOpen.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkPCWOutOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPCWOutOpen.Location = new System.Drawing.Point(13, 158);
            this.chkPCWOutOpen.Name = "chkPCWOutOpen";
            this.chkPCWOutOpen.Size = new System.Drawing.Size(140, 30);
            this.chkPCWOutOpen.TabIndex = 32;
            this.chkPCWOutOpen.Text = "PCW OUT Valve Open";
            this.chkPCWOutOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPCWOutOpen.UseVisualStyleBackColor = false;
            this.chkPCWOutOpen.Click += new System.EventHandler(this.check_click);
            // 
            // chkPCWInOpen
            // 
            this.chkPCWInOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPCWInOpen.BackColor = System.Drawing.Color.LightSteelBlue;
            this.chkPCWInOpen.Checked = true;
            this.chkPCWInOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPCWInOpen.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkPCWInOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPCWInOpen.Location = new System.Drawing.Point(13, 111);
            this.chkPCWInOpen.Name = "chkPCWInOpen";
            this.chkPCWInOpen.Size = new System.Drawing.Size(140, 30);
            this.chkPCWInOpen.TabIndex = 31;
            this.chkPCWInOpen.Text = "PCW IN Valve Open";
            this.chkPCWInOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPCWInOpen.UseVisualStyleBackColor = false;
            this.chkPCWInOpen.Click += new System.EventHandler(this.check_click);
            // 
            // chkCDAInOpen
            // 
            this.chkCDAInOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCDAInOpen.BackColor = System.Drawing.Color.LightSteelBlue;
            this.chkCDAInOpen.Checked = true;
            this.chkCDAInOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCDAInOpen.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkCDAInOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCDAInOpen.Location = new System.Drawing.Point(13, 69);
            this.chkCDAInOpen.Name = "chkCDAInOpen";
            this.chkCDAInOpen.Size = new System.Drawing.Size(140, 30);
            this.chkCDAInOpen.TabIndex = 30;
            this.chkCDAInOpen.Text = "CDA IN Valve Open";
            this.chkCDAInOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkCDAInOpen.UseVisualStyleBackColor = false;
            this.chkCDAInOpen.Click += new System.EventHandler(this.check_click);
            // 
            // chkN2InOpen
            // 
            this.chkN2InOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkN2InOpen.BackColor = System.Drawing.Color.LightSteelBlue;
            this.chkN2InOpen.Checked = true;
            this.chkN2InOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkN2InOpen.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkN2InOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkN2InOpen.Location = new System.Drawing.Point(13, 26);
            this.chkN2InOpen.Name = "chkN2InOpen";
            this.chkN2InOpen.Size = new System.Drawing.Size(140, 30);
            this.chkN2InOpen.TabIndex = 29;
            this.chkN2InOpen.Text = "N2 IN Valve Open";
            this.chkN2InOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkN2InOpen.UseVisualStyleBackColor = false;
            this.chkN2InOpen.Click += new System.EventHandler(this.check_click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkULST);
            this.groupBox5.Controls.Add(this.checkUICR);
            this.groupBox5.Controls.Add(this.checkUERT);
            this.groupBox5.Controls.Add(this.checkUEMG);
            this.groupBox5.Controls.Add(this.checkUTRE);
            this.groupBox5.Controls.Add(this.checkUDLC);
            this.groupBox5.Location = new System.Drawing.Point(423, 69);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(208, 346);
            this.groupBox5.TabIndex = 31;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "EUV Manual Operation";
            // 
            // checkULST
            // 
            this.checkULST.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkULST.BackColor = System.Drawing.Color.Honeydew;
            this.checkULST.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkULST.Checked = true;
            this.checkULST.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkULST.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkULST.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkULST.Location = new System.Drawing.Point(15, 290);
            this.checkULST.Name = "checkULST";
            this.checkULST.Size = new System.Drawing.Size(181, 47);
            this.checkULST.TabIndex = 29;
            this.checkULST.Text = "Lamp Use Time Reset(ULST)";
            this.checkULST.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkULST.UseVisualStyleBackColor = false;
            this.checkULST.Click += new System.EventHandler(this.check_click);
            // 
            // checkUICR
            // 
            this.checkUICR.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkUICR.BackColor = System.Drawing.Color.Honeydew;
            this.checkUICR.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUICR.Checked = true;
            this.checkUICR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUICR.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkUICR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUICR.Location = new System.Drawing.Point(15, 237);
            this.checkUICR.Name = "checkUICR";
            this.checkUICR.Size = new System.Drawing.Size(181, 47);
            this.checkUICR.TabIndex = 28;
            this.checkUICR.Text = "Counter Reset(UICR)";
            this.checkUICR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUICR.UseVisualStyleBackColor = false;
            this.checkUICR.Click += new System.EventHandler(this.check_click);
            // 
            // checkUERT
            // 
            this.checkUERT.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkUERT.BackColor = System.Drawing.Color.Honeydew;
            this.checkUERT.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUERT.Checked = true;
            this.checkUERT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUERT.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkUERT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUERT.Location = new System.Drawing.Point(15, 184);
            this.checkUERT.Name = "checkUERT";
            this.checkUERT.Size = new System.Drawing.Size(181, 47);
            this.checkUERT.TabIndex = 27;
            this.checkUERT.Text = "EMO Reset(UERT)";
            this.checkUERT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUERT.UseVisualStyleBackColor = false;
            this.checkUERT.Click += new System.EventHandler(this.check_click);
            // 
            // checkUEMG
            // 
            this.checkUEMG.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkUEMG.BackColor = System.Drawing.Color.Honeydew;
            this.checkUEMG.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUEMG.Checked = true;
            this.checkUEMG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUEMG.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkUEMG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUEMG.Location = new System.Drawing.Point(15, 131);
            this.checkUEMG.Name = "checkUEMG";
            this.checkUEMG.Size = new System.Drawing.Size(181, 47);
            this.checkUEMG.TabIndex = 26;
            this.checkUEMG.Text = "Emergency(UEMG)";
            this.checkUEMG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUEMG.UseVisualStyleBackColor = false;
            this.checkUEMG.Click += new System.EventHandler(this.check_click);
            // 
            // checkUTRE
            // 
            this.checkUTRE.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkUTRE.BackColor = System.Drawing.Color.Honeydew;
            this.checkUTRE.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUTRE.Checked = true;
            this.checkUTRE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUTRE.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkUTRE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUTRE.Location = new System.Drawing.Point(15, 76);
            this.checkUTRE.Name = "checkUTRE";
            this.checkUTRE.Size = new System.Drawing.Size(181, 47);
            this.checkUTRE.TabIndex = 25;
            this.checkUTRE.Text = "Remote Mode(UTRE)";
            this.checkUTRE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUTRE.UseVisualStyleBackColor = false;
            this.checkUTRE.Click += new System.EventHandler(this.check_click);
            // 
            // checkUDLC
            // 
            this.checkUDLC.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkUDLC.BackColor = System.Drawing.Color.Honeydew;
            this.checkUDLC.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUDLC.Checked = true;
            this.checkUDLC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUDLC.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkUDLC.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkUDLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUDLC.Location = new System.Drawing.Point(15, 23);
            this.checkUDLC.Name = "checkUDLC";
            this.checkUDLC.Size = new System.Drawing.Size(181, 47);
            this.checkUDLC.TabIndex = 24;
            this.checkUDLC.Text = "Local Mode Lock(UDLC)";
            this.checkUDLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkUDLC.UseVisualStyleBackColor = false;
            this.checkUDLC.Click += new System.EventHandler(this.check_click);
            // 
            // gbEuvStatus
            // 
            this.gbEuvStatus.Controls.Add(this.chkURDY);
            this.gbEuvStatus.Controls.Add(this.chkULAC);
            this.gbEuvStatus.Controls.Add(this.chkUCRE);
            this.gbEuvStatus.Controls.Add(this.chkUNRE);
            this.gbEuvStatus.Location = new System.Drawing.Point(648, 69);
            this.gbEuvStatus.Name = "gbEuvStatus";
            this.gbEuvStatus.Size = new System.Drawing.Size(234, 115);
            this.gbEuvStatus.TabIndex = 32;
            this.gbEuvStatus.TabStop = false;
            this.gbEuvStatus.Text = "EUV Status";
            // 
            // chkURDY
            // 
            this.chkURDY.AutoCheck = false;
            this.chkURDY.AutoSize = true;
            this.chkURDY.Location = new System.Drawing.Point(9, 89);
            this.chkURDY.Name = "chkURDY";
            this.chkURDY.Size = new System.Drawing.Size(94, 17);
            this.chkURDY.TabIndex = 25;
            this.chkURDY.Text = "Ready(URDY)";
            this.chkURDY.UseVisualStyleBackColor = true;
            // 
            // chkULAC
            // 
            this.chkULAC.AutoCheck = false;
            this.chkULAC.AutoSize = true;
            this.chkULAC.Location = new System.Drawing.Point(9, 67);
            this.chkULAC.Name = "chkULAC";
            this.chkULAC.Size = new System.Drawing.Size(83, 17);
            this.chkULAC.TabIndex = 24;
            this.chkULAC.Text = "Busy(ULAC)";
            this.chkULAC.UseVisualStyleBackColor = true;
            // 
            // chkUCRE
            // 
            this.chkUCRE.AutoCheck = false;
            this.chkUCRE.AutoSize = true;
            this.chkUCRE.Location = new System.Drawing.Point(9, 45);
            this.chkUCRE.Name = "chkUCRE";
            this.chkUCRE.Size = new System.Drawing.Size(171, 17);
            this.chkUCRE.TabIndex = 23;
            this.chkUCRE.Text = "Remote Mode Possible(UCRE)";
            this.chkUCRE.UseVisualStyleBackColor = true;
            // 
            // chkUNRE
            // 
            this.chkUNRE.AutoCheck = false;
            this.chkUNRE.AutoSize = true;
            this.chkUNRE.Location = new System.Drawing.Point(9, 23);
            this.chkUNRE.Name = "chkUNRE";
            this.chkUNRE.Size = new System.Drawing.Size(130, 17);
            this.chkUNRE.TabIndex = 22;
            this.chkUNRE.Text = "Remote Mode(UNRE)";
            this.chkUNRE.UseVisualStyleBackColor = true;
            // 
            // gbAlmStatus
            // 
            this.gbAlmStatus.Controls.Add(this.chkEMO);
            this.gbAlmStatus.Controls.Add(this.chkControlAnomaly);
            this.gbAlmStatus.Controls.Add(this.chkElecCoverOpen);
            this.gbAlmStatus.Controls.Add(this.chkN2PressDrop);
            this.gbAlmStatus.Controls.Add(this.chkTransTemp);
            this.gbAlmStatus.Controls.Add(this.chkLampOpen);
            this.gbAlmStatus.Controls.Add(this.chkLampTemp);
            this.gbAlmStatus.Controls.Add(this.chkN2PressSurplus);
            this.gbAlmStatus.Controls.Add(this.chkCircuitAnomaly);
            this.gbAlmStatus.Controls.Add(this.chkProtective);
            this.gbAlmStatus.Controls.Add(this.chkLampFail);
            this.gbAlmStatus.Controls.Add(this.chkLampTime);
            this.gbAlmStatus.Location = new System.Drawing.Point(648, 190);
            this.gbAlmStatus.Name = "gbAlmStatus";
            this.gbAlmStatus.Size = new System.Drawing.Size(234, 225);
            this.gbAlmStatus.TabIndex = 33;
            this.gbAlmStatus.TabStop = false;
            this.gbAlmStatus.Text = "Alarm Status";
            // 
            // chkEMO
            // 
            this.chkEMO.AutoCheck = false;
            this.chkEMO.AutoSize = true;
            this.chkEMO.Location = new System.Drawing.Point(9, 204);
            this.chkEMO.Name = "chkEMO";
            this.chkEMO.Size = new System.Drawing.Size(50, 17);
            this.chkEMO.TabIndex = 37;
            this.chkEMO.Text = "EMO";
            this.chkEMO.UseVisualStyleBackColor = true;
            // 
            // chkControlAnomaly
            // 
            this.chkControlAnomaly.AutoCheck = false;
            this.chkControlAnomaly.AutoSize = true;
            this.chkControlAnomaly.Location = new System.Drawing.Point(9, 188);
            this.chkControlAnomaly.Name = "chkControlAnomaly";
            this.chkControlAnomaly.Size = new System.Drawing.Size(139, 17);
            this.chkControlAnomaly.TabIndex = 36;
            this.chkControlAnomaly.Text = "Control System Anomaly";
            this.chkControlAnomaly.UseVisualStyleBackColor = true;
            // 
            // chkElecCoverOpen
            // 
            this.chkElecCoverOpen.AutoCheck = false;
            this.chkElecCoverOpen.AutoSize = true;
            this.chkElecCoverOpen.Location = new System.Drawing.Point(9, 172);
            this.chkElecCoverOpen.Name = "chkElecCoverOpen";
            this.chkElecCoverOpen.Size = new System.Drawing.Size(189, 17);
            this.chkElecCoverOpen.TabIndex = 35;
            this.chkElecCoverOpen.Text = "Electrical Component Cover Open ";
            this.chkElecCoverOpen.UseVisualStyleBackColor = true;
            // 
            // chkN2PressDrop
            // 
            this.chkN2PressDrop.AutoCheck = false;
            this.chkN2PressDrop.AutoSize = true;
            this.chkN2PressDrop.Location = new System.Drawing.Point(9, 155);
            this.chkN2PressDrop.Name = "chkN2PressDrop";
            this.chkN2PressDrop.Size = new System.Drawing.Size(110, 17);
            this.chkN2PressDrop.TabIndex = 34;
            this.chkN2PressDrop.Text = "N2 Pressure Drop";
            this.chkN2PressDrop.UseVisualStyleBackColor = true;
            // 
            // chkTransTemp
            // 
            this.chkTransTemp.AutoCheck = false;
            this.chkTransTemp.AutoSize = true;
            this.chkTransTemp.Location = new System.Drawing.Point(9, 138);
            this.chkTransTemp.Name = "chkTransTemp";
            this.chkTransTemp.Size = new System.Drawing.Size(189, 17);
            this.chkTransTemp.TabIndex = 33;
            this.chkTransTemp.Text = "Setup Transformer Temp. Anomaly";
            this.chkTransTemp.UseVisualStyleBackColor = true;
            // 
            // chkLampOpen
            // 
            this.chkLampOpen.AutoCheck = false;
            this.chkLampOpen.AutoSize = true;
            this.chkLampOpen.Location = new System.Drawing.Point(9, 121);
            this.chkLampOpen.Name = "chkLampOpen";
            this.chkLampOpen.Size = new System.Drawing.Size(115, 17);
            this.chkLampOpen.TabIndex = 32;
            this.chkLampOpen.Text = "Lamp House Open";
            this.chkLampOpen.UseVisualStyleBackColor = true;
            // 
            // chkLampTemp
            // 
            this.chkLampTemp.AutoCheck = false;
            this.chkLampTemp.AutoSize = true;
            this.chkLampTemp.Location = new System.Drawing.Point(9, 104);
            this.chkLampTemp.Name = "chkLampTemp";
            this.chkLampTemp.Size = new System.Drawing.Size(162, 17);
            this.chkLampTemp.TabIndex = 31;
            this.chkLampTemp.Text = "Lamp House Temp. Anomaly";
            this.chkLampTemp.UseVisualStyleBackColor = true;
            // 
            // chkN2PressSurplus
            // 
            this.chkN2PressSurplus.AutoCheck = false;
            this.chkN2PressSurplus.AutoSize = true;
            this.chkN2PressSurplus.Location = new System.Drawing.Point(9, 87);
            this.chkN2PressSurplus.Name = "chkN2PressSurplus";
            this.chkN2PressSurplus.Size = new System.Drawing.Size(121, 17);
            this.chkN2PressSurplus.TabIndex = 30;
            this.chkN2PressSurplus.Text = "N2 pressure Surplus";
            this.chkN2PressSurplus.UseVisualStyleBackColor = true;
            // 
            // chkCircuitAnomaly
            // 
            this.chkCircuitAnomaly.AutoCheck = false;
            this.chkCircuitAnomaly.AutoSize = true;
            this.chkCircuitAnomaly.Location = new System.Drawing.Point(9, 70);
            this.chkCircuitAnomaly.Name = "chkCircuitAnomaly";
            this.chkCircuitAnomaly.Size = new System.Drawing.Size(136, 17);
            this.chkCircuitAnomaly.TabIndex = 29;
            this.chkCircuitAnomaly.Text = "Electric Circuit Anomaly";
            this.chkCircuitAnomaly.UseVisualStyleBackColor = true;
            // 
            // chkProtective
            // 
            this.chkProtective.AutoCheck = false;
            this.chkProtective.AutoSize = true;
            this.chkProtective.Location = new System.Drawing.Point(9, 53);
            this.chkProtective.Name = "chkProtective";
            this.chkProtective.Size = new System.Drawing.Size(179, 17);
            this.chkProtective.TabIndex = 28;
            this.chkProtective.Text = "Protective Function Appearence";
            this.chkProtective.UseVisualStyleBackColor = true;
            // 
            // chkLampFail
            // 
            this.chkLampFail.AutoCheck = false;
            this.chkLampFail.AutoSize = true;
            this.chkLampFail.Location = new System.Drawing.Point(9, 36);
            this.chkLampFail.Name = "chkLampFail";
            this.chkLampFail.Size = new System.Drawing.Size(88, 17);
            this.chkLampFail.TabIndex = 27;
            this.chkLampFail.Text = "Lamp On Fail";
            this.chkLampFail.UseVisualStyleBackColor = true;
            // 
            // chkLampTime
            // 
            this.chkLampTime.AutoCheck = false;
            this.chkLampTime.AutoSize = true;
            this.chkLampTime.Location = new System.Drawing.Point(9, 20);
            this.chkLampTime.Name = "chkLampTime";
            this.chkLampTime.Size = new System.Drawing.Size(137, 17);
            this.chkLampTime.TabIndex = 26;
            this.chkLampTime.Text = "Lamp Use Time Excess";
            this.chkLampTime.UseVisualStyleBackColor = true;
            // 
            // tmrUpdateState
            // 
            this.tmrUpdateState.Interval = 300;
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            // 
            // ViewUshioEuv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gbAlmStatus);
            this.Controls.Add(this.gbEuvStatus);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gbLampPower);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.groupBox1);
            this.Name = "ViewUshioEuv";
            this.Size = new System.Drawing.Size(903, 465);
            this.gbStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataEuvUsedTimeGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLampIntensityGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbLampPower.ResumeLayout(false);
            this.gbLampPower.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp3Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp2Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp1Status)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.gbEuvStatus.ResumeLayout(false);
            this.gbEuvStatus.PerformLayout();
            this.gbAlmStatus.ResumeLayout(false);
            this.gbAlmStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCylDown;
        private System.Windows.Forms.Button btnCylUp;
        private System.Windows.Forms.CheckBox chkDown;
        private System.Windows.Forms.CheckBox chkUp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkLamp1Select;
        private System.Windows.Forms.CheckBox chkLamp2Select;
        private System.Windows.Forms.GroupBox gbLampPower;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox gbEuvStatus;
        private System.Windows.Forms.GroupBox gbAlmStatus;
        private System.Windows.Forms.CheckBox chkURDY;
        private System.Windows.Forms.CheckBox chkULAC;
        private System.Windows.Forms.CheckBox chkUCRE;
        private System.Windows.Forms.CheckBox chkUNRE;
        private System.Windows.Forms.CheckBox chkEMO;
        private System.Windows.Forms.CheckBox chkControlAnomaly;
        private System.Windows.Forms.CheckBox chkElecCoverOpen;
        private System.Windows.Forms.CheckBox chkN2PressDrop;
        private System.Windows.Forms.CheckBox chkTransTemp;
        private System.Windows.Forms.CheckBox chkLampOpen;
        private System.Windows.Forms.CheckBox chkLampTemp;
        private System.Windows.Forms.CheckBox chkN2PressSurplus;
        private System.Windows.Forms.CheckBox chkCircuitAnomaly;
        private System.Windows.Forms.CheckBox chkProtective;
        private System.Windows.Forms.CheckBox chkLampFail;
        private System.Windows.Forms.CheckBox chkLampTime;
        private DoubleBufferedGridView dataEuvUsedTimeGridView;
        private DoubleBufferedGridView dataLampIntensityGridView;
        private System.Windows.Forms.Timer tmrUpdateState;
        private System.Windows.Forms.CheckBox chkCDAInOpen;
        private System.Windows.Forms.CheckBox chkN2InOpen;
        private System.Windows.Forms.CheckBox chkPCWOutOpen;
        private System.Windows.Forms.CheckBox chkPCWInOpen;
        private System.Windows.Forms.PictureBox pbLamp2Status;
        private System.Windows.Forms.PictureBox pbLamp1Status;
        private System.Windows.Forms.CheckBox checkUDLC;
        private System.Windows.Forms.CheckBox checkUERT;
        private System.Windows.Forms.CheckBox checkUEMG;
        private System.Windows.Forms.CheckBox checkUTRE;
        private System.Windows.Forms.CheckBox checkULST;
        private System.Windows.Forms.CheckBox checkUICR;
        private System.Windows.Forms.CheckBox checkULON;
        private System.Windows.Forms.PictureBox pbLamp3Status;
        private System.Windows.Forms.CheckBox chkLamp3Select;
    }
}

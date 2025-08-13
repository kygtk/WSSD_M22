using Dms.Util.IODefine;
namespace Dms.Device
{
    partial class FormIOConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIOConfig));
            this.viewIOEdit1 = new Dms.Util.IODefine.ViewIOEdit();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewCurrentConfig = new System.Windows.Forms.TreeView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.validationTextBoxMaxSimulateCount = new Dms.Common.ValidationTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // viewIOEdit1
            // 
            this.viewIOEdit1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewIOEdit1.BackColor = System.Drawing.Color.Transparent;
            this.viewIOEdit1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewIOEdit1.Location = new System.Drawing.Point(275, 12);
            this.viewIOEdit1.Name = "viewIOEdit1";
            this.viewIOEdit1.OperateMode = Dms.Util.IODefine.ViewIOEdit.OpMode.Config;
            this.viewIOEdit1.Size = new System.Drawing.Size(485, 382);
            this.viewIOEdit1.TabIndex = 3;
            this.viewIOEdit1.TimerStateUpdateEnabled = false;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(12, 334);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(69, 51);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(87, 334);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(132, 51);
            this.buttonSelect.TabIndex = 4;
            this.buttonSelect.Text = "Confirm";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRemove.Location = new System.Drawing.Point(229, 134);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(38, 35);
            this.buttonRemove.TabIndex = 18;
            this.buttonRemove.Text = ">>";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = " Current Config";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewCurrentConfig
            // 
            this.treeViewCurrentConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewCurrentConfig.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewCurrentConfig.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewCurrentConfig.HideSelection = false;
            this.treeViewCurrentConfig.Location = new System.Drawing.Point(12, 69);
            this.treeViewCurrentConfig.Name = "treeViewCurrentConfig";
            this.treeViewCurrentConfig.Size = new System.Drawing.Size(207, 255);
            this.treeViewCurrentConfig.TabIndex = 15;
            this.treeViewCurrentConfig.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCurrentConfig_NodeMouseClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdd.Location = new System.Drawing.Point(229, 85);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 35);
            this.buttonAdd.TabIndex = 17;
            this.buttonAdd.Text = "<<";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DarkGray;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(229, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 2);
            this.label2.TabIndex = 21;
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(238)))));
            this.buttonDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonDown.Image = global::Dms.Device.Properties.Resources.arrow_down;
            this.buttonDown.Location = new System.Drawing.Point(229, 247);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(38, 35);
            this.buttonDown.TabIndex = 20;
            this.buttonDown.UseVisualStyleBackColor = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(238)))));
            this.buttonUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonUp.Image = global::Dms.Device.Properties.Resources.arrow_up;
            this.buttonUp.Location = new System.Drawing.Point(229, 198);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(38, 35);
            this.buttonUp.TabIndex = 19;
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // validationTextBoxMaxSimulateCount
            // 
            this.validationTextBoxMaxSimulateCount.DataFormat = Dms.Common.OptionFormat.Digit;
            this.validationTextBoxMaxSimulateCount.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.validationTextBoxMaxSimulateCount.KeyPadInfo = ((Dms.Common.KeyPadInfo)(resources.GetObject("validationTextBoxMaxSimulateCount.KeyPadInfo")));
            this.validationTextBoxMaxSimulateCount.LimitHigh = "";
            this.validationTextBoxMaxSimulateCount.LimitLow = "";
            this.validationTextBoxMaxSimulateCount.Location = new System.Drawing.Point(138, 42);
            this.validationTextBoxMaxSimulateCount.Name = "validationTextBoxMaxSimulateCount";
            this.validationTextBoxMaxSimulateCount.ReferenceTag = null;
            this.validationTextBoxMaxSimulateCount.Size = new System.Drawing.Size(81, 21);
            this.validationTextBoxMaxSimulateCount.TabIndex = 22;
            this.validationTextBoxMaxSimulateCount.UsedInKeyPad = false;
            this.validationTextBoxMaxSimulateCount.TextChanged += new System.EventHandler(this.validationTextBoxMaxSimulateCount_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "Max Simulate Count";
            // 
            // FormIOConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(772, 398);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.validationTextBoxMaxSimulateCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewCurrentConfig);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.viewIOEdit1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormIOConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormIOConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewIOEdit viewIOEdit1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewCurrentConfig;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Label label2;
        private Dms.Common.ValidationTextBox validationTextBoxMaxSimulateCount;
        private System.Windows.Forms.Label label3;
    }
}
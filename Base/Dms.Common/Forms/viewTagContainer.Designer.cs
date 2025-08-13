namespace Dms.Common
{
    partial class ViewTagContainer
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
            this.labelTags = new System.Windows.Forms.Label();
            this.treeViewTags = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // labelTags
            // 
            this.labelTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTags.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTags.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTags.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTags.Location = new System.Drawing.Point(4, 5);
            this.labelTags.Name = "labelTags";
            this.labelTags.Size = new System.Drawing.Size(475, 23);
            this.labelTags.TabIndex = 19;
            this.labelTags.Text = "DeviceTag Container";
            this.labelTags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewTags
            // 
            this.treeViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewTags.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewTags.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewTags.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewTags.HideSelection = false;
            this.treeViewTags.Location = new System.Drawing.Point(4, 32);
            this.treeViewTags.Name = "treeViewTags";
            this.treeViewTags.Size = new System.Drawing.Size(475, 272);
            this.treeViewTags.TabIndex = 18;
            this.treeViewTags.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewTags_NodeMouseClick);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleName = "";
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.BackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.CommandsBackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.Enabled = false;
            this.propertyGrid1.Location = new System.Drawing.Point(4, 310);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(475, 187);
            this.propertyGrid1.TabIndex = 15;
            // 
            // ViewTagContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.labelTags);
            this.Controls.Add(this.treeViewTags);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewTagContainer";
            this.Size = new System.Drawing.Size(482, 502);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTags;
        private System.Windows.Forms.TreeView treeViewTags;
        private System.Windows.Forms.PropertyGrid propertyGrid1;

    }
}

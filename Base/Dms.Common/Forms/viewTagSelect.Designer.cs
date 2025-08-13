namespace Dms.Common
{
    partial class ViewTagSelect
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.treeViewTags = new System.Windows.Forms.TreeView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewCurrentTag = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonFiltering = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.checkBoxFilterOn = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AllowDrop = true;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemDel,
            this.toolStripMenuItemSave,
            this.toolStripMenuItemLoad,
            this.saveAsToolStripMenuItem,
            this.loadFromToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 136);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemAdd.Tag = "Add";
            this.toolStripMenuItemAdd.Text = "&Add";
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemDel.Tag = "Del";
            this.toolStripMenuItemDel.Text = "&Del";
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemSave.Tag = "Save";
            this.toolStripMenuItemSave.Text = "&Save";
            // 
            // toolStripMenuItemLoad
            // 
            this.toolStripMenuItemLoad.Name = "toolStripMenuItemLoad";
            this.toolStripMenuItemLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemLoad.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemLoad.Tag = "Load";
            this.toolStripMenuItemLoad.Text = "&Load";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // loadFromToolStripMenuItem
            // 
            this.loadFromToolStripMenuItem.Name = "loadFromToolStripMenuItem";
            this.loadFromToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.loadFromToolStripMenuItem.Text = "Load From...";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRemove.Location = new System.Drawing.Point(223, 141);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(38, 35);
            this.buttonRemove.TabIndex = 21;
            this.buttonRemove.Text = ">>";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(272, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "Tags";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewTags
            // 
            this.treeViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewTags.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewTags.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewTags.HideSelection = false;
            this.treeViewTags.Location = new System.Drawing.Point(272, 58);
            this.treeViewTags.Name = "treeViewTags";
            this.treeViewTags.Size = new System.Drawing.Size(207, 199);
            this.treeViewTags.TabIndex = 18;
            this.treeViewTags.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewTags_NodeMouseClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdd.Location = new System.Drawing.Point(223, 92);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 35);
            this.buttonAdd.TabIndex = 20;
            this.buttonAdd.Text = "<<";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 23);
            this.label1.TabIndex = 17;
            this.label1.Text = " Current Tag";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewCurrentTag
            // 
            this.treeViewCurrentTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewCurrentTag.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewCurrentTag.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewCurrentTag.HideSelection = false;
            this.treeViewCurrentTag.Location = new System.Drawing.Point(4, 32);
            this.treeViewCurrentTag.Name = "treeViewCurrentTag";
            this.treeViewCurrentTag.Size = new System.Drawing.Size(207, 225);
            this.treeViewCurrentTag.TabIndex = 16;
            this.treeViewCurrentTag.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCurrentTag_NodeMouseClick);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleName = "";
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.BackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.CommandsBackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.Enabled = false;
            this.propertyGrid1.Location = new System.Drawing.Point(4, 263);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(475, 234);
            this.propertyGrid1.TabIndex = 15;
            // 
            // buttonFiltering
            // 
            this.buttonFiltering.Location = new System.Drawing.Point(441, 31);
            this.buttonFiltering.Name = "buttonFiltering";
            this.buttonFiltering.Size = new System.Drawing.Size(38, 23);
            this.buttonFiltering.TabIndex = 24;
            this.buttonFiltering.Text = "Do";
            this.buttonFiltering.UseVisualStyleBackColor = true;
            this.buttonFiltering.Click += new System.EventHandler(this.buttonFiltering_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(272, 32);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(167, 21);
            this.textBoxFilter.TabIndex = 22;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
            // 
            // checkBoxFilterOn
            // 
            this.checkBoxFilterOn.AutoSize = true;
            this.checkBoxFilterOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxFilterOn.Location = new System.Drawing.Point(217, 35);
            this.checkBoxFilterOn.Name = "checkBoxFilterOn";
            this.checkBoxFilterOn.Size = new System.Drawing.Size(51, 16);
            this.checkBoxFilterOn.TabIndex = 23;
            this.checkBoxFilterOn.Text = "Filter";
            this.checkBoxFilterOn.UseVisualStyleBackColor = true;
            this.checkBoxFilterOn.CheckedChanged += new System.EventHandler(this.checkBoxFilterOn_CheckedChanged);
            // 
            // ViewTagSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.buttonFiltering);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.checkBoxFilterOn);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.treeViewTags);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewCurrentTag);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "ViewTagSelect";
            this.Size = new System.Drawing.Size(482, 502);
            this.Load += new System.EventHandler(this.ViewTagSelect_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoad;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromToolStripMenuItem;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeViewTags;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewCurrentTag;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonFiltering;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.CheckBox checkBoxFilterOn;

    }
}

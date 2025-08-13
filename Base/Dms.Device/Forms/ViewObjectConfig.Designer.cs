namespace Dms.Device
{
    partial class ViewObjectConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.treeViewCurrentConfig = new System.Windows.Forms.TreeView();
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.treeViewComponents = new System.Windows.Forms.TreeView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.checkBoxFilterOn = new System.Windows.Forms.CheckBox();
            this.buttonFiltering = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = " Current Config";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleName = "";
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.BackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.CommandsBackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 305);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(475, 191);
            this.propertyGrid1.TabIndex = 8;
            this.propertyGrid1.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid1_SelectedGridItemChanged);
            this.propertyGrid1.SelectedObjectsChanged += new System.EventHandler(this.propertyGrid1_SelectedObjectsChanged);
            this.propertyGrid1.Leave += new System.EventHandler(this.propertyGrid1_Leave);
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // treeViewCurrentConfig
            // 
            this.treeViewCurrentConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewCurrentConfig.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewCurrentConfig.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewCurrentConfig.HideSelection = false;
            this.treeViewCurrentConfig.Location = new System.Drawing.Point(3, 31);
            this.treeViewCurrentConfig.Name = "treeViewCurrentConfig";
            this.treeViewCurrentConfig.Size = new System.Drawing.Size(207, 268);
            this.treeViewCurrentConfig.TabIndex = 9;
            this.treeViewCurrentConfig.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemAdd.Tag = "Add";
            this.toolStripMenuItemAdd.Text = "&Add";
            this.toolStripMenuItemAdd.Click += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemDel.Tag = "Del";
            this.toolStripMenuItemDel.Text = "&Del";
            this.toolStripMenuItemDel.Click += new System.EventHandler(this.toolStripMenuItemDel_Click);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemSave.Tag = "Save";
            this.toolStripMenuItemSave.Text = "&Save";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItemLoad
            // 
            this.toolStripMenuItemLoad.Name = "toolStripMenuItemLoad";
            this.toolStripMenuItemLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemLoad.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemLoad.Tag = "Load";
            this.toolStripMenuItemLoad.Text = "&Load";
            this.toolStripMenuItemLoad.Click += new System.EventHandler(this.toolStripMenuItemLoad_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadFromToolStripMenuItem
            // 
            this.loadFromToolStripMenuItem.Name = "loadFromToolStripMenuItem";
            this.loadFromToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.loadFromToolStripMenuItem.Text = "Load From...";
            this.loadFromToolStripMenuItem.Click += new System.EventHandler(this.loadFromToolStripMenuItem_Click);
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
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(271, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = " Components";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeViewComponents
            // 
            this.treeViewComponents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewComponents.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewComponents.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeViewComponents.HideSelection = false;
            this.treeViewComponents.Location = new System.Drawing.Point(271, 54);
            this.treeViewComponents.Name = "treeViewComponents";
            this.treeViewComponents.Size = new System.Drawing.Size(207, 245);
            this.treeViewComponents.TabIndex = 11;
            this.treeViewComponents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewComponents_NodeMouseClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdd.Location = new System.Drawing.Point(222, 91);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 35);
            this.buttonAdd.TabIndex = 13;
            this.buttonAdd.Text = "<<";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRemove.Location = new System.Drawing.Point(222, 140);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(38, 35);
            this.buttonRemove.TabIndex = 14;
            this.buttonRemove.Text = ">>";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(271, 30);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(167, 21);
            this.textBoxFilter.TabIndex = 15;
            this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // checkBoxFilterOn
            // 
            this.checkBoxFilterOn.AutoSize = true;
            this.checkBoxFilterOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxFilterOn.Location = new System.Drawing.Point(215, 32);
            this.checkBoxFilterOn.Name = "checkBoxFilterOn";
            this.checkBoxFilterOn.Size = new System.Drawing.Size(53, 19);
            this.checkBoxFilterOn.TabIndex = 17;
            this.checkBoxFilterOn.Text = "Filter";
            this.checkBoxFilterOn.UseVisualStyleBackColor = true;
            this.checkBoxFilterOn.CheckedChanged += new System.EventHandler(this.checkBoxFilterOn_CheckedChanged);
            // 
            // buttonFiltering
            // 
            this.buttonFiltering.Location = new System.Drawing.Point(440, 29);
            this.buttonFiltering.Name = "buttonFiltering";
            this.buttonFiltering.Size = new System.Drawing.Size(38, 23);
            this.buttonFiltering.TabIndex = 18;
            this.buttonFiltering.Text = "Do";
            this.buttonFiltering.UseVisualStyleBackColor = true;
            this.buttonFiltering.Click += new System.EventHandler(this.buttonFiltering_Click);
            // 
            // ViewObjectConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.buttonFiltering);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.checkBoxFilterOn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.treeViewComponents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewCurrentConfig);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.propertyGrid1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewObjectConfig";
            this.Size = new System.Drawing.Size(482, 502);
            this.Load += new System.EventHandler(this.ViewEqpConfig_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeViewCurrentConfig;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoad;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeViewComponents;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.CheckBox checkBoxFilterOn;
        private System.Windows.Forms.Button buttonFiltering;
    }
}

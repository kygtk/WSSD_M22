namespace Dms.Util.IODefine
{
    partial class ViewIOConfig
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUpdateAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.editMelsecDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.comboBoxFieldBusType = new System.Windows.Forms.ComboBox();
            this.buttonMakeBom = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = " I/O Configuraion";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleName = "";
            this.propertyGrid1.BackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.CommandsBackColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(422, 448);
            this.propertyGrid1.TabIndex = 8;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            this.propertyGrid1.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid1_SelectedGridItemChanged);
            this.propertyGrid1.Leave += new System.EventHandler(this.propertyGrid1_Leave);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AllowDrop = true;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemDel,
            this.toolStripSeparator2,
            this.toolStripMenuItemLoad,
            this.toolStripMenuItemLoadAll,
            this.toolStripMenuItemLoadFrom,
            this.toolStripSeparator1,
            this.toolStripMenuItemUpdate,
            this.toolStripMenuItemUpdateAll,
            this.toolStripSeparator3,
            this.toolStripMenuItemSave,
            this.toolStripMenuItemSaveAll,
            this.toolStripMenuItemSaveAs,
            this.toolStripSeparator5,
            this.editMelsecDeviceToolStripMenuItem,
            this.toolStripSeparator4});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 276);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemAdd.Tag = "Add";
            this.toolStripMenuItemAdd.Text = "&Add";
            this.toolStripMenuItemAdd.Click += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemDel.Tag = "Del";
            this.toolStripMenuItemDel.Text = "&Del";
            this.toolStripMenuItemDel.Click += new System.EventHandler(this.toolStripMenuItemDel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // toolStripMenuItemLoad
            // 
            this.toolStripMenuItemLoad.Name = "toolStripMenuItemLoad";
            this.toolStripMenuItemLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemLoad.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemLoad.Tag = "Load";
            this.toolStripMenuItemLoad.Text = "&Load";
            this.toolStripMenuItemLoad.Click += new System.EventHandler(this.toolStripMenuItemLoad_Click);
            // 
            // toolStripMenuItemLoadAll
            // 
            this.toolStripMenuItemLoadAll.Name = "toolStripMenuItemLoadAll";
            this.toolStripMenuItemLoadAll.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemLoadAll.Text = "Load All";
            this.toolStripMenuItemLoadAll.Click += new System.EventHandler(this.loadAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItemLoadFrom
            // 
            this.toolStripMenuItemLoadFrom.Name = "toolStripMenuItemLoadFrom";
            this.toolStripMenuItemLoadFrom.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemLoadFrom.Text = "Load From...";
            this.toolStripMenuItemLoadFrom.Click += new System.EventHandler(this.loadFromToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // toolStripMenuItemUpdate
            // 
            this.toolStripMenuItemUpdate.Name = "toolStripMenuItemUpdate";
            this.toolStripMenuItemUpdate.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemUpdate.Text = "Update";
            this.toolStripMenuItemUpdate.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // toolStripMenuItemUpdateAll
            // 
            this.toolStripMenuItemUpdateAll.Name = "toolStripMenuItemUpdateAll";
            this.toolStripMenuItemUpdateAll.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemUpdateAll.Text = "Update All";
            this.toolStripMenuItemUpdateAll.Click += new System.EventHandler(this.updateAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(172, 6);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemSave.Tag = "Save";
            this.toolStripMenuItemSave.Text = "&Save";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItemSaveAll
            // 
            this.toolStripMenuItemSaveAll.Name = "toolStripMenuItemSaveAll";
            this.toolStripMenuItemSaveAll.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemSaveAll.Text = "Save All";
            this.toolStripMenuItemSaveAll.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSaveAs
            // 
            this.toolStripMenuItemSaveAs.Name = "toolStripMenuItemSaveAs";
            this.toolStripMenuItemSaveAs.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemSaveAs.Text = "Save As...";
            this.toolStripMenuItemSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(172, 6);
            // 
            // editMelsecDeviceToolStripMenuItem
            // 
            this.editMelsecDeviceToolStripMenuItem.Name = "editMelsecDeviceToolStripMenuItem";
            this.editMelsecDeviceToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editMelsecDeviceToolStripMenuItem.Text = "Edit Melsec Device";
            this.editMelsecDeviceToolStripMenuItem.Click += new System.EventHandler(this.editMelsecDeviceToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(172, 6);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 21);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(389, 399);
            this.treeView1.TabIndex = 9;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxFieldBusType);
            this.splitContainer1.Panel1.Controls.Add(this.buttonMakeBom);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(815, 448);
            this.splitContainer1.SplitterDistance = 389;
            this.splitContainer1.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Controls.Add(this.textBoxFileName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(389, 420);
            this.panel1.TabIndex = 13;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxFileName.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFileName.Location = new System.Drawing.Point(0, 0);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(389, 21);
            this.textBoxFileName.TabIndex = 11;
            // 
            // comboBoxFieldBusType
            // 
            this.comboBoxFieldBusType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFieldBusType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFieldBusType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFieldBusType.FormattingEnabled = true;
            this.comboBoxFieldBusType.Location = new System.Drawing.Point(121, 3);
            this.comboBoxFieldBusType.Name = "comboBoxFieldBusType";
            this.comboBoxFieldBusType.Size = new System.Drawing.Size(198, 23);
            this.comboBoxFieldBusType.TabIndex = 12;
            // 
            // buttonMakeBom
            // 
            this.buttonMakeBom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMakeBom.Location = new System.Drawing.Point(327, 3);
            this.buttonMakeBom.Name = "buttonMakeBom";
            this.buttonMakeBom.Size = new System.Drawing.Size(59, 23);
            this.buttonMakeBom.TabIndex = 11;
            this.buttonMakeBom.Text = "BOM";
            this.buttonMakeBom.UseVisualStyleBackColor = true;
            this.buttonMakeBom.Visible = false;
            this.buttonMakeBom.Click += new System.EventHandler(this.buttonMakeBom_Click);
            // 
            // ViewIOConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewIOConfig";
            this.Size = new System.Drawing.Size(815, 448);
            this.Load += new System.EventHandler(this.ViewIOConfig_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoad;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadFrom;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonMakeBom;
        private System.Windows.Forms.ComboBox comboBoxFieldBusType;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdateAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxFileName;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem editMelsecDeviceToolStripMenuItem;
    }
}

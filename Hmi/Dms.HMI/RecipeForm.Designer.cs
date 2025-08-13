namespace Dms.HMI
{
    partial class RecipeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecipeForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageRecipes = new System.Windows.Forms.TabPage();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.btnSave = new Dms.Control.TagButton();
            this.btnDelete = new Dms.Control.TagButton();
            this.btnAdd = new Dms.Control.TagButton();
            this.btnCopy = new Dms.Control.TagButton();
            this.btnSelect = new Dms.Control.TagButton();
            this.tmrUpdateToolbar = new System.Windows.Forms.Timer(this.components);
            this.tabPageCurrentRecipe = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.gbToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageRecipes);
            this.tabControl1.Controls.Add(this.tabPageCurrentRecipe);
            this.tabControl1.Location = new System.Drawing.Point(9, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(909, 565);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageRecipes
            // 
            this.tabPageRecipes.Location = new System.Drawing.Point(4, 24);
            this.tabPageRecipes.Name = "tabPageRecipes";
            this.tabPageRecipes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRecipes.Size = new System.Drawing.Size(901, 537);
            this.tabPageRecipes.TabIndex = 0;
            this.tabPageRecipes.Text = "Recipes";
            this.tabPageRecipes.UseVisualStyleBackColor = true;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.SystemColors.Control;
            this.pnlToolbar.Controls.Add(this.gbToolbar);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlToolbar.Location = new System.Drawing.Point(920, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(1);
            this.pnlToolbar.Size = new System.Drawing.Size(92, 595);
            this.pnlToolbar.TabIndex = 4;
            // 
            // gbToolbar
            // 
            this.gbToolbar.Controls.Add(this.btnSave);
            this.gbToolbar.Controls.Add(this.btnDelete);
            this.gbToolbar.Controls.Add(this.btnAdd);
            this.gbToolbar.Controls.Add(this.btnCopy);
            this.gbToolbar.Controls.Add(this.btnSelect);
            this.gbToolbar.Location = new System.Drawing.Point(4, -4);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Size = new System.Drawing.Size(84, 594);
            this.gbToolbar.TabIndex = 0;
            this.gbToolbar.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnSave.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnSave.Command = Dms.Common.Command.Noop;
            this.btnSave.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnSave.DeviceTagInfo")));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = global::Dms.HMI.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(3, 281);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 66);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnDelete.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnDelete.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnDelete.Command = Dms.Common.Command.Noop;
            this.btnDelete.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnDelete.DeviceTagInfo")));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Image = global::Dms.HMI.Properties.Resources.Delete;
            this.btnDelete.Location = new System.Drawing.Point(3, 215);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 66);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnAdd.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnAdd.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnAdd.Command = Dms.Common.Command.Noop;
            this.btnAdd.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnAdd.DeviceTagInfo")));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Image = global::Dms.HMI.Properties.Resources.Add;
            this.btnAdd.Location = new System.Drawing.Point(3, 149);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(78, 66);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnCopy.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnCopy.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnCopy.Command = Dms.Common.Command.Noop;
            this.btnCopy.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnCopy.DeviceTagInfo")));
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCopy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCopy.Image = global::Dms.HMI.Properties.Resources.Copy;
            this.btnCopy.Location = new System.Drawing.Point(3, 83);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(78, 66);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.ButtonCheckedColor = System.Drawing.Color.LightSteelBlue;
            this.btnSelect.ButtonCheckedType = Dms.Control.TagButton.Type.A_TrueCheck;
            this.btnSelect.ButtonUnCheckedColor = System.Drawing.Color.Transparent;
            this.btnSelect.Command = Dms.Common.Command.Noop;
            this.btnSelect.DeviceTagInfo = ((Dms.Common.DeviceTagInfo)(resources.GetObject("btnSelect.DeviceTagInfo")));
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSelect.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelect.Image = global::Dms.HMI.Properties.Resources.Select;
            this.btnSelect.Location = new System.Drawing.Point(3, 17);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(78, 66);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // tmrUpdateToolbar
            // 
            this.tmrUpdateToolbar.Interval = 500;
            this.tmrUpdateToolbar.Tick += new System.EventHandler(this.tmrUpdateToolbar_Tick);
            // 
            // tabPageCurrentRecipe
            // 
            this.tabPageCurrentRecipe.Location = new System.Drawing.Point(4, 24);
            this.tabPageCurrentRecipe.Name = "tabPageCurrentRecipe";
            this.tabPageCurrentRecipe.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCurrentRecipe.Size = new System.Drawing.Size(901, 537);
            this.tabPageCurrentRecipe.TabIndex = 1;
            this.tabPageCurrentRecipe.Text = "CurrentRecipe";
            this.tabPageCurrentRecipe.UseVisualStyleBackColor = true;
            // 
            // RecipeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1012, 595);
            this.ControlBox = false;
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RecipeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SystemForm";
            this.Deactivate += new System.EventHandler(this.RecipeForm_Deactivate);
            this.Load += new System.EventHandler(this.RecipeForm_Load);
            this.Activated += new System.EventHandler(this.RecipeForm_Activate);
            this.tabControl1.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.gbToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageRecipes;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.GroupBox gbToolbar;
        private Dms.Control.TagButton btnDelete;
        private Dms.Control.TagButton btnAdd;
        private Dms.Control.TagButton btnSave;
        private Dms.Control.TagButton btnCopy;
        private Dms.Control.TagButton btnSelect;
        private System.Windows.Forms.Timer tmrUpdateToolbar;
        private System.Windows.Forms.TabPage tabPageCurrentRecipe;
    }
}
namespace Dms.Data
{
    partial class TankLevelGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clmTankLevelItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTankLevelValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridTankLevelInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTankLevelItems,
            this.clmTankLevelValue});
            this.Location = new System.Drawing.Point(0, 0);
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(340, 127);
            this.TabIndex = 13;
            // 
            // clmTankLevelItems
            // 
            this.clmTankLevelItems.HeaderText = "ITEMS";
            this.clmTankLevelItems.Name = "clmTankLevelItems";
            this.clmTankLevelItems.ReadOnly = true;
            this.clmTankLevelItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmTankLevelItems.Width = 210;
            // 
            // clmTankLevelValue
            // 
            this.clmTankLevelValue.HeaderText = "VALUE";
            this.clmTankLevelValue.Name = "clmTankLevelValue";
            this.clmTankLevelValue.ReadOnly = true;
            this.clmTankLevelValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmTankLevelValue.Width = 126;
            // 
            // TankLevelGrid
            // 

            this.Name = "TankLevelGrid";
            this.Size = new System.Drawing.Size(343, 129);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmTankLevelItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTankLevelValue;
    }
}

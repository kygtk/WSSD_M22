namespace Dms.Data
{
    partial class IdleRunInfoGrid
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
            this.clmIdleRunItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmIdleRunValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridIdleRunInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmIdleRunItems,
            this.clmIdleRunValue});
            this.Location = new System.Drawing.Point(1, 0);
            this.Name = "dataGridIdleRunInfo";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(340, 201);
            this.TabIndex = 12;
            // 
            // clmIdleRunItems
            // 
            this.clmIdleRunItems.HeaderText = "ITEMS";
            this.clmIdleRunItems.Name = "clmIdleRunItems";
            this.clmIdleRunItems.ReadOnly = true;
            this.clmIdleRunItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmIdleRunItems.Width = 210;
            // 
            // clmIdleRunValue
            // 
            this.clmIdleRunValue.HeaderText = "VALUE";
            this.clmIdleRunValue.Name = "clmIdleRunValue";
            this.clmIdleRunValue.ReadOnly = true;
            this.clmIdleRunValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmIdleRunValue.Width = 105;
            // 
            // IdleRunInfoGrid
            // 
            this.Name = "IdleRunInfoGrid";
            this.Size = new System.Drawing.Size(341, 202);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmIdleRunItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIdleRunValue;
    }
}

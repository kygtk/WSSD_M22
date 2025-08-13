namespace Dms.Data
{
    partial class CVGenInfo
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
            this.clmGeninfoItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGeninfoOption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGeninfoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridGenInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmGeninfoItems,
            this.clmGeninfoOption,
            this.clmGeninfoValue});
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "dataGridGenInfo";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(568, 517);
            this.TabIndex = 11;
            // 
            // clmGeninfoItems
            // 
            this.clmGeninfoItems.HeaderText = "ITEMS";
            this.clmGeninfoItems.Name = "clmGeninfoItems";
            this.clmGeninfoItems.ReadOnly = true;
            this.clmGeninfoItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGeninfoItems.Width = 265;
            // 
            // clmGeninfoOption
            // 
            this.clmGeninfoOption.HeaderText = "OPTION";
            this.clmGeninfoOption.Name = "clmGeninfoOption";
            this.clmGeninfoOption.ReadOnly = true;
            this.clmGeninfoOption.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGeninfoOption.Width = 150;
            // 
            // clmGeninfoValue
            // 
            this.clmGeninfoValue.HeaderText = "VALUE";
            this.clmGeninfoValue.Name = "clmGeninfoValue";
            this.clmGeninfoValue.ReadOnly = true;
            this.clmGeninfoValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGeninfoValue.Width = 150;
            // 
            // CVGenInfo
            // 
            this.Name = "CVGenInfo";
            this.Size = new System.Drawing.Size(569, 519);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmGeninfoItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGeninfoOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGeninfoValue;
    }
}

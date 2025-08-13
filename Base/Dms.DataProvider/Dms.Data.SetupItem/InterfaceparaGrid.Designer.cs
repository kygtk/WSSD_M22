namespace Dms.Data
{
    partial class InterfaceparaGrid
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
            this.clmInterfaceItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmInterfaceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridInterface
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmInterfaceItems,
            this.clmInterfaceValue});
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "dataGridInterface";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(553, 522);
            this.TabIndex = 12;
            // 
            // clmInterfaceItems
            // 
            this.clmInterfaceItems.HeaderText = "ITEMS";
            this.clmInterfaceItems.Name = "clmInterfaceItems";
            this.clmInterfaceItems.ReadOnly = true;
            this.clmInterfaceItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmInterfaceItems.Width = 400;
            // 
            // clmInterfaceValue
            // 
            this.clmInterfaceValue.HeaderText = "VALUE";
            this.clmInterfaceValue.Name = "clmInterfaceValue";
            this.clmInterfaceValue.ReadOnly = true;
            this.clmInterfaceValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmInterfaceValue.Width = 150;
            // 
            // InterfaceparaGrid
            // 
            this.Name = "InterfaceparaGrid";
            this.Size = new System.Drawing.Size(553, 522);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmInterfaceItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmInterfaceValue;
    }
}

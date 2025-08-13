namespace Dms.Data
{
    partial class CVDistanceGrid
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
            //this.dataGridCVDistance = new System.Windows.Forms.DataGridView();
            this.clmCVDistanceItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCVDistanceOldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCVDistanceNewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
           // ((System.ComponentModel.ISupportInitialize)(this.dataGridCVDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridCVDistance
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCVDistanceItems,
            this.clmCVDistanceOldValue,
            this.clmCVDistanceNewValue});
            this.Location = new System.Drawing.Point(2, 1);
            this.Name = "dataGridCVDistance";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(583, 488);
            this.TabIndex = 18;
            // 
            // clmCVDistanceItems
            // 
            this.clmCVDistanceItems.HeaderText = "ITEMS";
            this.clmCVDistanceItems.Name = "clmCVDistanceItems";
            this.clmCVDistanceItems.ReadOnly = true;
            this.clmCVDistanceItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVDistanceItems.Width = 300;
            // 
            // clmCVDistanceOldValue
            // 
            this.clmCVDistanceOldValue.HeaderText = "OLD VALUE";
            this.clmCVDistanceOldValue.Name = "clmCVDistanceOldValue";
            this.clmCVDistanceOldValue.ReadOnly = true;
            this.clmCVDistanceOldValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVDistanceOldValue.Width = 140;
            // 
            // clmCVDistanceNewValue
            // 
            this.clmCVDistanceNewValue.HeaderText = "NEW VALUE";
            this.clmCVDistanceNewValue.Name = "clmCVDistanceNewValue";
            this.clmCVDistanceNewValue.ReadOnly = true;
            this.clmCVDistanceNewValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVDistanceNewValue.Width = 140;
            // 
            // CVDistanceGrid
            // 
            //this.Controls.Add(this.dataGridCVDistance);
            this.Size = new System.Drawing.Size(587, 491);
            //((System.ComponentModel.ISupportInitialize)(this.dataGridCVDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.DataGridView dataGridCVDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVDistanceItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVDistanceOldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVDistanceNewValue;
    }
}

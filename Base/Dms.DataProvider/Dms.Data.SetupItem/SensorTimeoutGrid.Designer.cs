namespace Dms.Data
{
    partial class SensorTimeoutGrid
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
            this.clmInSensorItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmINSensorValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridInSensorTimeout
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmInSensorItems,
            this.clmINSensorValue});
            this.Location = new System.Drawing.Point(0, 0);
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(318, 183);
            this.TabIndex = 19;
            // 
            // clmInSensorItems
            // 
            this.clmInSensorItems.HeaderText = "ITEMS";
            this.clmInSensorItems.Name = "clmInSensorItems";
            this.clmInSensorItems.ReadOnly = true;
            this.clmInSensorItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmInSensorItems.Width = 210;
            // 
            // clmINSensorValue
            // 
            this.clmINSensorValue.HeaderText = "VALUE";
            this.clmINSensorValue.Name = "clmINSensorValue";
            this.clmINSensorValue.ReadOnly = true;
            this.clmINSensorValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmINSensorValue.Width = 105;
            // 
            // SensorTimeoutGrid
            // 
            this.Name = "SensorTimeoutGrid";
            this.Size = new System.Drawing.Size(319, 185);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmInSensorItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmINSensorValue;
    }
}

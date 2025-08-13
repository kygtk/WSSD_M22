namespace Dms.Data
{
    partial class CVMotorGearGrid
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
            this.clmCVMotorItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCVMotorVelocity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCVMotorGearRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCVMotorDiameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridCVInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCVMotorItems,
            this.clmCVMotorVelocity,
            this.clmCVMotorGearRatio,
            this.clmCVMotorDiameter});
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "dataGridCVInfo";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(753, 528);
            this.TabIndex = 13;
            // 
            // clmCVMotorItems
            // 
            this.clmCVMotorItems.HeaderText = "ITEMS";
            this.clmCVMotorItems.Name = "clmCVMotorItems";
            this.clmCVMotorItems.ReadOnly = true;
            this.clmCVMotorItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVMotorItems.Width = 300;
            // 
            // clmCVMotorVelocity
            // 
            this.clmCVMotorVelocity.HeaderText = "VELOCITY RATIO";
            this.clmCVMotorVelocity.Name = "clmCVMotorVelocity";
            this.clmCVMotorVelocity.ReadOnly = true;
            this.clmCVMotorVelocity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVMotorVelocity.Width = 150;
            // 
            // clmCVMotorGearRatio
            // 
            this.clmCVMotorGearRatio.HeaderText = "GEAR RATIO";
            this.clmCVMotorGearRatio.Name = "clmCVMotorGearRatio";
            this.clmCVMotorGearRatio.ReadOnly = true;
            this.clmCVMotorGearRatio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmCVMotorGearRatio.Width = 150;
            // 
            // clmCVMotorDiameter
            // 
            this.clmCVMotorDiameter.HeaderText = "DIAMETER";
            this.clmCVMotorDiameter.Name = "clmCVMotorDiameter";
            this.clmCVMotorDiameter.ReadOnly = true;
            this.clmCVMotorDiameter.Width = 150;
            // 
            // CVMotorGearGrid
            // 
            this.Name = "CVMotorGearGrid";
            this.Size = new System.Drawing.Size(755, 530);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVMotorItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVMotorVelocity;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVMotorGearRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCVMotorDiameter;
    }
}

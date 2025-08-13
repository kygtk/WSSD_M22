namespace Dms.Data
{
    partial class PartsLifeTimeGrid
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
            this.clmPartsLifeTimeInfoItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPartsLifeTimeInfoGlsNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPartsLifeTimeInfoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridPartsLifeTimeInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmPartsLifeTimeInfoItems,
            this.clmPartsLifeTimeInfoGlsNo,
            this.clmPartsLifeTimeInfoValue});
            this.Location = new System.Drawing.Point(0, 0);
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(340, 130);
            this.TabIndex = 14;
            // 
            // clmPartsLifeTimeInfoItems
            // 
            this.clmPartsLifeTimeInfoItems.HeaderText = "ITEMS";
            this.clmPartsLifeTimeInfoItems.Name = "clmPartsLifeTimeInfoItems";
            this.clmPartsLifeTimeInfoItems.ReadOnly = true;
            this.clmPartsLifeTimeInfoItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmPartsLifeTimeInfoItems.Width = 155;
            // 
            // clmPartsLifeTimeInfoGlsNo
            // 
            this.clmPartsLifeTimeInfoGlsNo.HeaderText = "GLS NO";
            this.clmPartsLifeTimeInfoGlsNo.Name = "clmPartsLifeTimeInfoGlsNo";
            this.clmPartsLifeTimeInfoGlsNo.ReadOnly = true;
            this.clmPartsLifeTimeInfoGlsNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmPartsLifeTimeInfoGlsNo.Width = 80;
            // 
            // clmPartsLifeTimeInfoValue
            // 
            this.clmPartsLifeTimeInfoValue.HeaderText = "VALUE";
            this.clmPartsLifeTimeInfoValue.Name = "clmPartsLifeTimeInfoValue";
            this.clmPartsLifeTimeInfoValue.ReadOnly = true;
            this.clmPartsLifeTimeInfoValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmPartsLifeTimeInfoValue.Width = 80;
            // 
            // PartsLifeTimeGrid
            // 
            this.Name = "PartsLifeTimeGrid";
            this.Size = new System.Drawing.Size(341, 132);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmPartsLifeTimeInfoItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPartsLifeTimeInfoGlsNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPartsLifeTimeInfoValue;
    }
}

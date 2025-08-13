namespace Dms.Data
{
    partial class GaugeLimitGrid
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
            this.clmGaugeLimitItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGaugeLimitOption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGaugeLimitMIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGaugeLimitW_MIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGaugeLimitW_MAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGaugeLimitMAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuspendLayout();
            // 
            // dataGridGaugeLimitInfo
            // 
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmGaugeLimitItems,
            this.clmGaugeLimitOption,
            this.clmGaugeLimitMIN,
            this.clmGaugeLimitW_MIN,
            this.clmGaugeLimitW_MAX,
            this.clmGaugeLimitMAX});
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "dataGridGaugeLimitInfo";
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.Size = new System.Drawing.Size(678, 521);
            this.TabIndex = 12;
            // 
            // clmGaugeLimitItems
            // 
            this.clmGaugeLimitItems.HeaderText = "ITEMS";
            this.clmGaugeLimitItems.Name = "clmGaugeLimitItems";
            this.clmGaugeLimitItems.ReadOnly = true;
            this.clmGaugeLimitItems.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGaugeLimitItems.Width = 265;
            // 
            // clmGaugeLimitOption
            // 
            this.clmGaugeLimitOption.HeaderText = "OPTION";
            this.clmGaugeLimitOption.Name = "clmGaugeLimitOption";
            this.clmGaugeLimitOption.ReadOnly = true;
            this.clmGaugeLimitOption.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGaugeLimitOption.Width = 90;
            // 
            // clmGaugeLimitMIN
            // 
            this.clmGaugeLimitMIN.HeaderText = "MIN";
            this.clmGaugeLimitMIN.Name = "clmGaugeLimitMIN";
            this.clmGaugeLimitMIN.ReadOnly = true;
            this.clmGaugeLimitMIN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmGaugeLimitMIN.Width = 80;
            // 
            // clmGaugeLimitW_MIN
            // 
            this.clmGaugeLimitW_MIN.HeaderText = "W_MIN";
            this.clmGaugeLimitW_MIN.Name = "clmGaugeLimitW_MIN";
            this.clmGaugeLimitW_MIN.ReadOnly = true;
            this.clmGaugeLimitW_MIN.Width = 80;
            // 
            // clmGaugeLimitW_MAX
            // 
            this.clmGaugeLimitW_MAX.HeaderText = "W_MAX";
            this.clmGaugeLimitW_MAX.Name = "clmGaugeLimitW_MAX";
            this.clmGaugeLimitW_MAX.ReadOnly = true;
            this.clmGaugeLimitW_MAX.Width = 80;
            // 
            // clmGaugeLimitMAX
            // 
            this.clmGaugeLimitMAX.HeaderText = "MAX";
            this.clmGaugeLimitMAX.Name = "clmGaugeLimitMAX";
            this.clmGaugeLimitMAX.ReadOnly = true;
            this.clmGaugeLimitMAX.Width = 80;
            // 
            // GaugeLimitGrid
            // 
            this.Name = "GaugeLimitGrid";
            this.Size = new System.Drawing.Size(680, 523);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitMIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitW_MIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitW_MAX;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGaugeLimitMAX;
    }
}

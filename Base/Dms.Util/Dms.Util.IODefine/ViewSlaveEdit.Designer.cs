
namespace Dms.Util.IODefine
{
    partial class ViewSlaveEdit
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageServo = new System.Windows.Forms.TabPage();
            this.dataGridViewServo = new Dms.Control.DoubleBufferedGridView();
            this.tabPageBLDC = new System.Windows.Forms.TabPage();
            this.dataGridViewBLDC = new Dms.Control.DoubleBufferedGridView();
            this.tabPageInverter = new System.Windows.Forms.TabPage();
            this.dataGridViewInverter = new Dms.Control.DoubleBufferedGridView();
            this.tabPageDI = new System.Windows.Forms.TabPage();
            this.dataGridViewDI = new Dms.Control.DoubleBufferedGridView();
            this.tabPageDO = new System.Windows.Forms.TabPage();
            this.dataGridViewDO = new Dms.Control.DoubleBufferedGridView();
            this.tabPageAI = new System.Windows.Forms.TabPage();
            this.dataGridViewAI = new Dms.Control.DoubleBufferedGridView();
            this.tabPageAO = new System.Windows.Forms.TabPage();
            this.dataGridViewAO = new Dms.Control.DoubleBufferedGridView();
            this.tabPageAP = new System.Windows.Forms.TabPage();
            this.dataGridViewAP = new Dms.Control.DoubleBufferedGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxDisplayFormat = new System.Windows.Forms.ComboBox();
            this.buttonFiltering = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.checkBoxFilterOn = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageServo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServo)).BeginInit();
            this.tabPageBLDC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBLDC)).BeginInit();
            this.tabPageInverter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInverter)).BeginInit();
            this.tabPageDI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDI)).BeginInit();
            this.tabPageDO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDO)).BeginInit();
            this.tabPageAI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAI)).BeginInit();
            this.tabPageAO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAO)).BeginInit();
            this.tabPageAP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAP)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageServo);
            this.tabControl1.Controls.Add(this.tabPageBLDC);
            this.tabControl1.Controls.Add(this.tabPageInverter);
            this.tabControl1.Controls.Add(this.tabPageDI);
            this.tabControl1.Controls.Add(this.tabPageDO);
            this.tabControl1.Controls.Add(this.tabPageAI);
            this.tabControl1.Controls.Add(this.tabPageAO);
            this.tabControl1.Controls.Add(this.tabPageAP);
            this.tabControl1.Location = new System.Drawing.Point(3, 31);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 334);
            this.tabControl1.TabIndex = 23;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageServo
            // 
            this.tabPageServo.Controls.Add(this.dataGridViewServo);
            this.tabPageServo.Location = new System.Drawing.Point(4, 24);
            this.tabPageServo.Name = "tabPageServo";
            this.tabPageServo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServo.Size = new System.Drawing.Size(688, 306);
            this.tabPageServo.TabIndex = 5;
            this.tabPageServo.Text = "Servo";
            this.tabPageServo.UseVisualStyleBackColor = true;
            // 
            // dataGridViewServo
            // 
            this.dataGridViewServo.AllowUserToAddRows = false;
            this.dataGridViewServo.AllowUserToDeleteRows = false;
            this.dataGridViewServo.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewServo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewServo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewServo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewServo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewServo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewServo.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewServo.Name = "dataGridViewServo";
            this.dataGridViewServo.RowHeadersVisible = false;
            this.dataGridViewServo.RowTemplate.Height = 18;
            this.dataGridViewServo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewServo.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewServo.TabIndex = 28;
            this.dataGridViewServo.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewServo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewServo.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewServo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewServo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewServo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageBLDC
            // 
            this.tabPageBLDC.Controls.Add(this.dataGridViewBLDC);
            this.tabPageBLDC.Location = new System.Drawing.Point(4, 24);
            this.tabPageBLDC.Name = "tabPageBLDC";
            this.tabPageBLDC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBLDC.Size = new System.Drawing.Size(688, 306);
            this.tabPageBLDC.TabIndex = 4;
            this.tabPageBLDC.Text = "BLDC";
            this.tabPageBLDC.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBLDC
            // 
            this.dataGridViewBLDC.AllowUserToAddRows = false;
            this.dataGridViewBLDC.AllowUserToDeleteRows = false;
            this.dataGridViewBLDC.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewBLDC.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewBLDC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBLDC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewBLDC.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewBLDC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBLDC.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewBLDC.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewBLDC.Name = "dataGridViewBLDC";
            this.dataGridViewBLDC.RowHeadersVisible = false;
            this.dataGridViewBLDC.RowTemplate.Height = 18;
            this.dataGridViewBLDC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewBLDC.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewBLDC.TabIndex = 0;
            this.dataGridViewBLDC.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewBLDC.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewBLDC.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewBLDC.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewBLDC.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewBLDC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageInverter
            // 
            this.tabPageInverter.Controls.Add(this.dataGridViewInverter);
            this.tabPageInverter.Location = new System.Drawing.Point(4, 24);
            this.tabPageInverter.Name = "tabPageInverter";
            this.tabPageInverter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInverter.Size = new System.Drawing.Size(688, 306);
            this.tabPageInverter.TabIndex = 7;
            this.tabPageInverter.Text = "Inverter";
            this.tabPageInverter.UseVisualStyleBackColor = true;
            // 
            // dataGridViewInverter
            // 
            this.dataGridViewInverter.AllowUserToAddRows = false;
            this.dataGridViewInverter.AllowUserToDeleteRows = false;
            this.dataGridViewInverter.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewInverter.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewInverter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewInverter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewInverter.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewInverter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewInverter.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewInverter.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewInverter.Name = "dataGridViewInverter";
            this.dataGridViewInverter.RowHeadersVisible = false;
            this.dataGridViewInverter.RowTemplate.Height = 18;
            this.dataGridViewInverter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewInverter.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewInverter.TabIndex = 28;
            this.dataGridViewInverter.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewInverter.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewInverter.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewInverter.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewInverter.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewInverter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageDI
            // 
            this.tabPageDI.Controls.Add(this.dataGridViewDI);
            this.tabPageDI.Location = new System.Drawing.Point(4, 24);
            this.tabPageDI.Name = "tabPageDI";
            this.tabPageDI.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDI.Size = new System.Drawing.Size(688, 306);
            this.tabPageDI.TabIndex = 0;
            this.tabPageDI.Text = "Digital Inputs";
            this.tabPageDI.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDI
            // 
            this.dataGridViewDI.AllowUserToAddRows = false;
            this.dataGridViewDI.AllowUserToDeleteRows = false;
            this.dataGridViewDI.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewDI.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewDI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDI.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewDI.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewDI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDI.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewDI.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewDI.Name = "dataGridViewDI";
            this.dataGridViewDI.RowHeadersVisible = false;
            this.dataGridViewDI.RowTemplate.Height = 18;
            this.dataGridViewDI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewDI.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewDI.TabIndex = 0;
            this.dataGridViewDI.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewDI.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewDI.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewDI.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewDI.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewDI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageDO
            // 
            this.tabPageDO.Controls.Add(this.dataGridViewDO);
            this.tabPageDO.Location = new System.Drawing.Point(4, 24);
            this.tabPageDO.Name = "tabPageDO";
            this.tabPageDO.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDO.Size = new System.Drawing.Size(688, 306);
            this.tabPageDO.TabIndex = 1;
            this.tabPageDO.Text = "Digital Output";
            this.tabPageDO.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDO
            // 
            this.dataGridViewDO.AllowUserToAddRows = false;
            this.dataGridViewDO.AllowUserToDeleteRows = false;
            this.dataGridViewDO.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewDO.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewDO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewDO.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewDO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDO.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewDO.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewDO.Name = "dataGridViewDO";
            this.dataGridViewDO.RowHeadersVisible = false;
            this.dataGridViewDO.RowTemplate.Height = 18;
            this.dataGridViewDO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewDO.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewDO.TabIndex = 1;
            this.dataGridViewDO.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewDO.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewDO.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewDO.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewDO.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewDO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageAI
            // 
            this.tabPageAI.Controls.Add(this.dataGridViewAI);
            this.tabPageAI.Location = new System.Drawing.Point(4, 24);
            this.tabPageAI.Name = "tabPageAI";
            this.tabPageAI.Size = new System.Drawing.Size(688, 306);
            this.tabPageAI.TabIndex = 2;
            this.tabPageAI.Text = "Analog Input";
            this.tabPageAI.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAI
            // 
            this.dataGridViewAI.AllowUserToAddRows = false;
            this.dataGridViewAI.AllowUserToDeleteRows = false;
            this.dataGridViewAI.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewAI.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewAI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAI.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewAI.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewAI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAI.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewAI.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewAI.Name = "dataGridViewAI";
            this.dataGridViewAI.RowHeadersVisible = false;
            this.dataGridViewAI.RowTemplate.Height = 18;
            this.dataGridViewAI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewAI.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewAI.TabIndex = 1;
            this.dataGridViewAI.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewAI.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewAI.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewAI.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewAI.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewAI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageAO
            // 
            this.tabPageAO.Controls.Add(this.dataGridViewAO);
            this.tabPageAO.Location = new System.Drawing.Point(4, 24);
            this.tabPageAO.Name = "tabPageAO";
            this.tabPageAO.Size = new System.Drawing.Size(688, 306);
            this.tabPageAO.TabIndex = 3;
            this.tabPageAO.Text = "Analog Output";
            this.tabPageAO.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAO
            // 
            this.dataGridViewAO.AllowUserToAddRows = false;
            this.dataGridViewAO.AllowUserToDeleteRows = false;
            this.dataGridViewAO.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewAO.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewAO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewAO.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewAO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAO.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewAO.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewAO.Name = "dataGridViewAO";
            this.dataGridViewAO.RowHeadersVisible = false;
            this.dataGridViewAO.RowTemplate.Height = 18;
            this.dataGridViewAO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewAO.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewAO.TabIndex = 1;
            this.dataGridViewAO.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewAO.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewAO.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewAO.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewAO.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewAO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // tabPageAP
            // 
            this.tabPageAP.Controls.Add(this.dataGridViewAP);
            this.tabPageAP.Location = new System.Drawing.Point(4, 24);
            this.tabPageAP.Name = "tabPageAP";
            this.tabPageAP.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAP.Size = new System.Drawing.Size(688, 306);
            this.tabPageAP.TabIndex = 6;
            this.tabPageAP.Text = "AP";
            this.tabPageAP.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAP
            // 
            this.dataGridViewAP.AllowUserToAddRows = false;
            this.dataGridViewAP.AllowUserToDeleteRows = false;
            this.dataGridViewAP.AllowUserToResizeRows = false;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            this.dataGridViewAP.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAP.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewAP.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewAP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAP.DefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridViewAP.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewAP.Name = "dataGridViewAP";
            this.dataGridViewAP.RowHeadersVisible = false;
            this.dataGridViewAP.RowTemplate.Height = 18;
            this.dataGridViewAP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewAP.Size = new System.Drawing.Size(676, 294);
            this.dataGridViewAP.TabIndex = 28;
            this.dataGridViewAP.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridViewAP.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridViewAP.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDoubleClick);
            this.dataGridViewAP.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridViewAP.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridViewAP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBoxDisplayFormat
            // 
            this.comboBoxDisplayFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDisplayFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayFormat.FormattingEnabled = true;
            this.comboBoxDisplayFormat.Location = new System.Drawing.Point(568, 3);
            this.comboBoxDisplayFormat.Name = "comboBoxDisplayFormat";
            this.comboBoxDisplayFormat.Size = new System.Drawing.Size(121, 23);
            this.comboBoxDisplayFormat.TabIndex = 27;
            this.comboBoxDisplayFormat.Visible = false;
            // 
            // buttonFiltering
            // 
            this.buttonFiltering.Location = new System.Drawing.Point(347, 3);
            this.buttonFiltering.Name = "buttonFiltering";
            this.buttonFiltering.Size = new System.Drawing.Size(38, 23);
            this.buttonFiltering.TabIndex = 26;
            this.buttonFiltering.Text = "Do";
            this.buttonFiltering.UseVisualStyleBackColor = true;
            this.buttonFiltering.Click += new System.EventHandler(this.buttonFiltering_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(3, 4);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(338, 21);
            this.textBoxFilter.TabIndex = 24;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
            // 
            // checkBoxFilterOn
            // 
            this.checkBoxFilterOn.AutoSize = true;
            this.checkBoxFilterOn.Location = new System.Drawing.Point(391, 6);
            this.checkBoxFilterOn.Name = "checkBoxFilterOn";
            this.checkBoxFilterOn.Size = new System.Drawing.Size(53, 19);
            this.checkBoxFilterOn.TabIndex = 25;
            this.checkBoxFilterOn.Text = "Filter";
            this.checkBoxFilterOn.UseVisualStyleBackColor = true;
            this.checkBoxFilterOn.CheckedChanged += new System.EventHandler(this.checkBoxFilterOn_CheckedChanged);
            // 
            // ViewSlaveEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.comboBoxDisplayFormat);
            this.Controls.Add(this.buttonFiltering);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.checkBoxFilterOn);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "ViewSlaveEdit";
            this.Size = new System.Drawing.Size(702, 372);
            this.Load += new System.EventHandler(this.ViewSlaveEdit_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageServo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServo)).EndInit();
            this.tabPageBLDC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBLDC)).EndInit();
            this.tabPageInverter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInverter)).EndInit();
            this.tabPageDI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDI)).EndInit();
            this.tabPageDO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDO)).EndInit();
            this.tabPageAI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAI)).EndInit();
            this.tabPageAO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAO)).EndInit();
            this.tabPageAP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDI;
        private Control.DoubleBufferedGridView dataGridViewDI;
        private System.Windows.Forms.TabPage tabPageDO;
        private Control.DoubleBufferedGridView dataGridViewDO;
        private System.Windows.Forms.TabPage tabPageAI;
        private Control.DoubleBufferedGridView dataGridViewAI;
        private System.Windows.Forms.TabPage tabPageAO;
        private Control.DoubleBufferedGridView dataGridViewAO;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxDisplayFormat;
        private System.Windows.Forms.Button buttonFiltering;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.CheckBox checkBoxFilterOn;
        private System.Windows.Forms.TabPage tabPageBLDC;
        private Control.DoubleBufferedGridView dataGridViewBLDC;
        private System.Windows.Forms.TabPage tabPageServo;
        private System.Windows.Forms.TabPage tabPageInverter;
        private System.Windows.Forms.TabPage tabPageAP;
        private Control.DoubleBufferedGridView dataGridViewServo;
        private Control.DoubleBufferedGridView dataGridViewInverter;
        private Control.DoubleBufferedGridView dataGridViewAP;
    }
}

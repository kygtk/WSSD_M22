using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    public partial class ViewIOEdit : UserControl
    {
        public enum OpMode
        {
            Config,
            Select,
            MultiSelect,
            System
        }

        #region Fields
        private IoDefines m_Pool = null;
        private OpMode m_Mode = OpMode.Config;
        private BindingSource[] m_BindSource;
        private List<DataGridView> m_DataGridView = new List<DataGridView>();
        private string m_SelectedName;
        private bool m_Initialized = false;
        private bool m_FormLoaded = false;
        [Category("DMS : Setting")]
        public event SetIoStateEventHandler OnSetIoState;
        protected ICtlDevice m_CtlDevice = null;
        private const string m_ColumHeaderNameWiringNo = "WireNo";
        private List<string> m_SelectedNames = new List<string>();
        private DataGridViewSelectedRowCollection m_SelectedRows;
        private bool m_ShowNodeInfo = true;

        private IoType m_SelectedIoType;
        private static string m_FilterString = "";
        private static bool m_FilterOn = false;
        private static Format m_SelectedDisplayFormat = Format.NUMBER;
        #endregion

        #region Properties
        [Browsable(false), XmlIgnore()]
        public string SelectedName
        {
            get { return m_SelectedName; }
        }

        [Browsable(false), XmlIgnore()]
        public OpMode OperateMode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool ShowNodeInfo
        {
            get { return m_ShowNodeInfo; }
            set { m_ShowNodeInfo = value; }
        }

        [Browsable(false), XmlIgnore()]
        public bool TimerStateUpdateEnabled
        {
            get { return this.timer1.Enabled; }
            set
            {
                if (value == false)
                {
                    this.timer1.Enabled = value;
                }
                else if (m_Mode == OpMode.System)
                {
                    this.timer1.Enabled = value;
                }
            }
        }

        [Browsable(false), XmlIgnore()]
        public List<string> SelectedNames
        {
            get
            {
                m_SelectedNames.Clear();
                int count = m_SelectedRows.Count;
                for (int i = (count - 1); i >= 0; i--)
                {
                    DataGridViewRow row = m_SelectedRows[i];
                    m_SelectedNames.Add(row.DataBoundItem.ToString());
                }
                return m_SelectedNames;
            }
        }
        #endregion

        #region Constructor
        public ViewIOEdit()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            MakeComboBoxDisplayFormat();
        }
        #endregion

        private void ViewIOEdit_Load(object sender, EventArgs e)
        {
            m_DataGridView.Add(this.dataGridView1);
            m_DataGridView.Add(this.dataGridView2);
            m_DataGridView.Add(this.dataGridView3);
            m_DataGridView.Add(this.dataGridView4);

            //this.textBoxFilter.Text = m_FilterString;
            //this.checkBoxFilterOn.Checked = m_FilterOn;

            m_FormLoaded = true;

            //if (m_Mode == OpMode.MultiSelect)
            //{
            //    this.dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            //}
        }

        public void Initialize(IoDefines pool, OpMode mode, ICtlDevice ctlDevice)
        {
            Initialize(pool, mode);

            AppConfig config = AppConfig.Instance;
            if (config.AutoStart && m_Mode == OpMode.System)
            {
                m_CtlDevice = ctlDevice;
                //m_CtlDevice.OnIoStateChange += new IoStateChangeEventHandler(HandleEvent);
            }
        }

        public void Initialize(IoDefines pool, OpMode mode)
        {
            m_Pool = pool;
            m_Mode = mode;

            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;

            if (m_FilterOn)
            {
                Filtering();
            }
            else
            {
                UpdateEditView(pool, mode);
            }
        }

        private void UpdateEditView(IoDefines pool, OpMode mode)
        {
            m_Mode = mode;

            m_BindSource = new BindingSource[4];

            int count = m_BindSource.Length;
            for (int i = 0; i < count; i++)
            {
                m_BindSource[i] = new BindingSource();
                m_BindSource[i].DataSource = pool;
            }

            Initialize(this.dataGridView1, m_BindSource[0], "DigitalInputs");   //  데이터 바인딩 방법 찾기
            Initialize(this.dataGridView2, m_BindSource[1], "DigitalOutputs");
            Initialize(this.dataGridView3, m_BindSource[2], "AnalogInputs");
            Initialize(this.dataGridView4, m_BindSource[3], "AnalogOutputs");

            UpdateComboBoxDisplayFormat();

            m_Initialized = true;
        }

        private void Initialize(DataGridView gridview, BindingSource bindSoruce, string member)
        {
            bindSoruce.DataMember = member;

            gridview.AutoGenerateColumns = false;
            gridview.DataSource = bindSoruce;

            if (!m_Initialized)
            {
                DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                col1.DataPropertyName = "Id";
                col1.HeaderText = "Id";
                gridview.Columns.Add(col1);

                bool showNodeInfo = m_Mode == OpMode.Config;
                showNodeInfo &= m_ShowNodeInfo;

                DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
                col2.DataPropertyName = "Node";
                col2.HeaderText = "Node";
                col2.Visible = showNodeInfo;
                gridview.Columns.Add(col2);

                DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
                col3.DataPropertyName = "Terminal";
                col3.HeaderText = "Term.";
                col3.Visible = showNodeInfo;
                gridview.Columns.Add(col3);

                DataGridViewTextBoxColumn col4 = new DataGridViewTextBoxColumn();
                col4.DataPropertyName = "Channel";
                col4.HeaderText = "Ch.";
                col4.Visible = showNodeInfo;
                col4.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                gridview.Columns.Add(col4);

                DataGridViewTextBoxColumn col5 = new DataGridViewTextBoxColumn();
                col5.DataPropertyName = "WiringNo";
                col5.HeaderText = m_ColumHeaderNameWiringNo;
                col5.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                gridview.Columns.Add(col5);

                DataGridViewTextBoxColumn col6 = new DataGridViewTextBoxColumn();
                col6.DataPropertyName = "Name";
                col6.HeaderText = "Name";
                col6.Name = "Name";
                col6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridview.Columns.Add(col6);

                //DataGridViewTextBoxColumn col7 = new DataGridViewTextBoxColumn();
                //col7.DataPropertyName = "Description";
                //col7.HeaderText = "Description";
                //gridview.Columns.Add(col7);

                if (member == "DigitalInputs")
                {
                    DataGridViewComboBoxColumn col8 = new DataGridViewComboBoxColumn();
                    col8.DataPropertyName = "ActiveType";
                    col8.DataSource = Enum.GetValues(typeof(ActiveType));
                    col8.HeaderText = "Active";
                    gridview.Columns.Add(col8);
                }

                if (m_Mode == OpMode.System)
                {
                    DataGridViewTextBoxColumn col9 = new DataGridViewTextBoxColumn();
                    col9.DataPropertyName = "State";
                    col9.HeaderText = "State";
                    col9.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    gridview.Columns.Add(col9);
                }
            }

            SetOpMode(gridview, m_Mode);
        }

        private void HandleEvent(object sender, IoStateEventArgs e)
        {
            //lock (this)
            //{
            //    int typeIndex = (int)e.Type;
            //    int itemIndex = e.Id;
            //    DataGridView view = m_DataGridView[typeIndex];
            //    int stateColumnIndex = view.ColumnCount - 1;
            //    //DataGridViewCell cell = view.Rows[itemIndex].Cells[stateColumnIndex];

            //    foreach (DataGridViewRow row in view.Rows)
            //    {
            //        //event에서 넘겨진 io id와 view의 id가 같으면
            //        if ((int)(row.Cells[0].Value) == itemIndex)
            //        {
            //            //view.Refresh();
            //            SetStateCellBackColor(row.Cells[stateColumnIndex]);
            //            //view.UpdateCellValue(stateColumnIndex, row.Index);
            //        }
            //    }

            //    //SetStateCellBackColor(cell);
            //}
        }

        private void SetStateCellBackColor(DataGridViewCell cell)
        {
            if (cell.Value != null)
            {
                Color onColor = Color.GreenYellow;
                Color offColor = Color.WhiteSmoke;

                string value = cell.Value.ToString();

                if (value == "ON")
                {
                    cell.Style.BackColor = onColor;
                }
                else if (value == "OFF")
                {
                    cell.Style.BackColor = offColor;
                }
                else
                {	//AI/AO값을 update 할려면 아래처럼 해줘야 한다. 이상하다.
                    cell.Style.BackColor = onColor;
                    cell.Style.BackColor = offColor;
                }
            }
        }

        private void SetOpMode(DataGridView view, OpMode mode)
        {
            if (mode != OpMode.Config)
            {
                view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                view.MultiSelect = (mode == OpMode.MultiSelect);
                view.ReadOnly = true;

                if (mode == OpMode.MultiSelect)
                {
                    view.SelectionChanged += new EventHandler(view_SelectionChanged);
                }

                //gridview폰트를 바꿀려면 아래처럼
                //DataGridViewCellStyle cellStyle = view.DefaultCellStyle;
                //cellStyle.Font = new System.Drawing.Font("arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
        }

        void view_SelectionChanged(object sender, EventArgs e)
        {
            m_SelectedRows = (sender as DataGridView).SelectedRows;
        }

        public void InitializeByFilter(IoDefines sourcePool, IoType ioType, params string[] keys)
        {
            //TODO:잘안되네 ~~ 그래서 일단 무식하게
            //m_BindSource[0].Filter = string.Format("Name like '{0}' ", keys[0]);
            //this.dataGridView1.DataSource = m_BindSource[0];

            //IoDefines poolFilter = new IoDefines();

            m_Pool = sourcePool;

            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;

            IoDefines poolFilter = Filtering(sourcePool, ioType, keys);
            Initialize(poolFilter, m_Mode);
        }

        private void UpdateEditViewByFilter(IoDefines sourcePool, IoType ioType, params string[] keys)
        {
            IoDefines poolFilter = Filtering(sourcePool, ioType, keys);
            UpdateEditView(poolFilter, m_Mode);
        }

        private IoDefines Filtering(IoDefines sourcePool, IoType ioType, params string[] keys)
        {
            m_SelectedIoType = ioType;

            IoDefines poolFilter = new IoDefines();
            poolFilter.Clone(m_Pool);

            if (keys.Length > 0)
            {
                if (ioType == IoType.DI)
                {
                    List<IoItemDI> filter = new List<IoItemDI>();
                    foreach (IoItemDI item in sourcePool.DigitalInputs)
                    {
                        bool matchAll = true;
                        foreach (string key in keys)
                        {
                            if (key == "__") continue;
                            matchAll &= item.Name.ToLower().Contains(key.ToLower());
                        }

                        if (matchAll)
                        {
                            filter.Add(item);
                        }
                    }

                    if (filter.Count > 0) poolFilter.DigitalInputs = filter;
                    //else poolFilter.DigitalInputs = sourcePool.DigitalInputs;
                }
                else
                {
                    List<IoItem> source = new List<IoItem>();
                    List<IoItem> filter = new List<IoItem>();

                    switch (ioType)
                    {
                        case IoType.DO:
                            source = sourcePool.DigitalOutputs;
                            break;
                        case IoType.AI:
                            source = sourcePool.AnalogInputs;
                            break;
                        case IoType.AO:
                            source = sourcePool.AnalogOutputs;
                            break;
                    }

                    foreach (IoItem item in source)
                    {
                        bool matchAll = true;
                        foreach (string key in keys)
                        {
                            if (key == "__") continue;
                            matchAll &= item.Name.ToLower().Contains(key.ToLower());
                        }

                        if (matchAll)
                        {
                            filter.Add(item);
                        }
                    }

                    switch (ioType)
                    {
                        case IoType.DO:
                            {
                                if (filter.Count > 0) poolFilter.DigitalOutputs = filter;
                                //else poolFilter.DigitalOutputs = source;
                            }
                            break;
                        case IoType.AI:
                            {
                                if (filter.Count > 0) poolFilter.AnalogInputs = filter;
                                //else poolFilter.AnalogInputs = source;
                            }
                            break;
                        case IoType.AO:
                            {
                                if (filter.Count > 0) poolFilter.AnalogOutputs = filter;
                                //else poolFilter.AnalogOutputs = source;
                            }
                            break;
                    }
                }
            }

            this.tabControl1.SelectedIndex = (int)ioType;

            return poolFilter;
        }

        public void SetGridColor(DataGridView gridView)
        {
            int oldNode = -1;
            int oldTerminal = -1;
            int terminalCount = -1;

            foreach (DataGridViewRow row in gridView.Rows)
            {
                int node = (int)row.Cells[1].Value;
                int terminal = (int)row.Cells[2].Value;

                if (oldNode != node || oldTerminal != terminal)
                {
                    oldNode = node;
                    oldTerminal = terminal;
                    terminalCount++;
                }

                Color nodeBackColor;
                if (node % 2 == 0)
                {
                    nodeBackColor = Color.LightBlue;
                }
                else
                {
                    nodeBackColor = Color.LightSteelBlue;
                }

                Color terminalBackColor;
                if (terminalCount % 2 == 0)
                {
                    terminalBackColor = Color.Lavender;
                }
                else
                {
                    terminalBackColor = Color.White;
                }

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = terminalBackColor;

                    if (cell.ColumnIndex == 0)
                    {
                        cell.Style.BackColor = Color.LightGray;
                    }
                    else if (cell.ColumnIndex < 2)
                    {
                        cell.Style.BackColor = nodeBackColor;
                    }
                    else if (cell.Value != null && cell.Value.GetType() == typeof(ActiveType))
                    {
                        if ((ActiveType)cell.Value == ActiveType.B)
                        {
                            cell.Style.BackColor = Color.LightPink;
                        }
                        else
                        {
                            cell.Style.BackColor = Color.White;
                        }
                    }
                    //else if (this.m_Mode == OpMode.System && 
                    //         cell.ColumnIndex == (gridView.ColumnCount-1))
                    //{
                    //    SetStateCellBackColor(cell);
                    //}
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView view = sender as DataGridView;
            SetGridColor(view);
            view.ClearSelection();
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            DataGridView view = sender as DataGridView;
            m_SelectedName = view.Rows[e.RowIndex].Cells["Name"].Value as string;
            if (m_SelectedNames.Contains(m_SelectedName))
            {
                m_SelectedNames.Remove(m_SelectedName);
            }
            else
            {
                m_SelectedNames.Add(m_SelectedName);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    if (MessageBox.Show("Do you want to paste contents from clipboard?", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                    Cursor.Current = Cursors.WaitCursor;

                    string s = Clipboard.GetText();
                    char[] splitter = { '\r', '\n' };
                    string[] lines = s.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    DataGridView view = sender as DataGridView;
                    DataGridViewCell cell;
                    int row = view.CurrentCell.RowIndex;
                    int col = view.CurrentCell.ColumnIndex;
                    int colCount = view.ColumnCount;
                    int lineCount = lines.Length;
                    string line;
                    for (int i = 0; i < lineCount; i++)
                    {
                        line = lines[i];
                        if (row < view.RowCount && line.Length > 0)
                        {
                            string[] cells = line.Split('\t');
                            int cellCount = cells.Length;
                            for (int j = 0; j < cellCount; j++)
                            {
                                if (col + j < colCount)
                                {
                                    cell = view[col + j, row];
                                    if (cell.ValueType == typeof(ActiveType))
                                    {
                                        cell.Value = cells[j] == "A" ? ActiveType.A : ActiveType.B;
                                    }
                                    else cell.Value = cells[j];
                                }
                                else break;
                            }
                            row++;
                        }
                        else break;
                    }

                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("Paste complete!");
                }
            }
            catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            if (m_Mode == OpMode.System)
            {
                DataGridView view = sender as DataGridView;
                BindingSource source = view.DataSource as BindingSource;
                //string curValue = view[e.ColumnIndex, e.RowIndex].Value.ToString();
                //string caption = source.Current.ToString();   //get current i/o name for dialog's caption
                //string[] items = { bool.TrueString, bool.FalseString }; //set selectable items

                if (m_CtlDevice != null)
                {
                    IoItem ioItem = source.Current as IoItem;
                    int id = ioItem.Id;
                    switch (ioItem.IoType)
                    {
                        case IoType.DI:
                            {
                                bool curValue = false;
                                if (m_Pool.BusType == FieldBusType.TwinCATPlc)
                                {
                                    curValue = (bool)m_CtlDevice.Read(id, IoType.DI, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                    m_CtlDevice.Write(id, !curValue, IoType.DI, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                }
                                else
                                {
                                    curValue = m_CtlDevice.ReadDiAsync(id);
                                    m_CtlDevice.WriteDiSync(id, !curValue);
                                }
                            }
                            break;
                        case IoType.DO:
                            {
                                bool curValue = false;
                                if (m_Pool.BusType == FieldBusType.TwinCATPlc)
                                {
                                    curValue = (bool)m_CtlDevice.Read(id, IoType.DO, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                    m_CtlDevice.Write(id, !curValue, IoType.DO, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                }
                                else
                                {
                                    curValue = m_CtlDevice.ReadDoAsync(id);
                                    m_CtlDevice.WriteDoAsync(id, !curValue);
                                }
                            }
                            break;
                        case IoType.AI:
                            {
                                KeyInValidation validation = new KeyInValidation();
                                string newValueString = "";

                                if (m_Pool.BusType == FieldBusType.TwinCATPlc)
                                {
                                    newValueString = validation.ShowEditDialog("Key In value", m_CtlDevice.Read(id, IoType.AI, ioItem.Channel, ioItem.Terminal, ioItem.Node).ToString());
                                    m_CtlDevice.Write(id, newValueString, IoType.AI, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                }
                                else
                                {
                                    ushort curValue = (ushort)(m_CtlDevice.ReadAiAsync(id));
                                    validation.Format = OptionFormat.Digit;
                                    validation.High = "65535";
                                    validation.Low = "0";
                                    newValueString = validation.ShowEditDialog("Key In value", curValue.ToString());
                                    ushort newValue = Convert.ToUInt16(newValueString);
                                    if (curValue != newValue)
                                    {
                                        m_CtlDevice.WriteAiSync(id, (short)newValue);
                                    }
                                }
                            }
                            break;
                        case IoType.AO:
                            {
                                KeyInValidation validation = new KeyInValidation();
                                string newValueString = "";

                                // TwinCATPlc의 경우, ioitem.Channel이 2이면 VarType.Out을 의미함.
                                // 이 경우, 변수의 타입에 따라 써주어야 하는 값의 타입이 달라지므로 일괄적으로 string형으로 써주는 함수에 값을 전달하도록 한다.
                                if (m_Pool.BusType == FieldBusType.TwinCATPlc/* && ioItem.Channel == (int)IoType.DO*/)
                                {
                                    newValueString = validation.ShowEditDialog("Key In value", m_CtlDevice.Read(id, IoType.AO, ioItem.Channel, ioItem.Terminal, ioItem.Node).ToString());
                                    m_CtlDevice.Write(id, newValueString, IoType.AO, ioItem.Channel, ioItem.Terminal, ioItem.Node);
                                }
                                else
                                {
                                    ushort curValue = m_CtlDevice.ReadAoAsync(id);
                                    validation.Format = OptionFormat.Digit;
                                    validation.High = "65535";
                                    validation.Low = "0";
                                    newValueString = validation.ShowEditDialog("Key In value", curValue.ToString());
                                    ushort newValue = Convert.ToUInt16(newValueString);
                                    if (curValue != newValue)
                                    {
                                        m_CtlDevice.WriteAoAsync(id, newValue);
                                    }
                                }
                            }
                            break;
                    }
                }

                //jemoon : Controller를 직접 이용하도록 수정
                //IoItem ioItem = source.Current as IoItem;
                SetIoStateEventHandler eHandle = OnSetIoState;
                if (eHandle != null)
                {
                    //    OnSetIoState(view, new IoStateEventArgs(ioItem.IoType, ioItem.Id));
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (m_Mode == OpMode.System && m_FormLoaded && m_Initialized)
            {
                if (m_CtlDevice.Uninitializing || !m_CtlDevice.Initialized) return;

                //jemoon : 현재상태 update
                int selectedIndex = this.tabControl1.SelectedIndex;
                IoItem ioitem;
                if (selectedIndex == (int)IoType.DI)
                {
                    int count = m_Pool.DigitalInputs.Count;
                    for (int i = 0; i < count; i++)
                    {
                        ioitem = m_Pool.DigitalInputs[i];
                        if (m_Pool.BusType == FieldBusType.TwinCATPlc)
                        {
                            object state = m_CtlDevice.Read(ioitem.Id, IoType.DI, ioitem.Channel, ioitem.Terminal, ioitem.Node);
                            ioitem.State = (state != null && (bool)state == true) ? "ON" : "OFF";
                        }
                        else ioitem.State = m_CtlDevice.ReadDiAsync(ioitem.Id) ? "ON" : "OFF";
                    }
                }
                else if (selectedIndex == (int)IoType.DO)
                {
                    int count = m_Pool.DigitalOutputs.Count;
                    for (int i = 0; i < count; i++)
                    {
                        ioitem = m_Pool.DigitalOutputs[i];
                        if (m_Pool.BusType == FieldBusType.TwinCATPlc)
                            ioitem.State = (bool)m_CtlDevice.Read(ioitem.Id, IoType.DO, ioitem.Channel, ioitem.Terminal, ioitem.Node) ? "ON" : "OFF";
                        else ioitem.State = m_CtlDevice.ReadDoAsync(ioitem.Id) ? "ON" : "OFF";
                    }
                }
                else if (selectedIndex == (int)IoType.AI)
                {
                    try
                    {
                        int count = m_Pool.AnalogInputs.Count;
                        for (int i = 0; i < count; i++)
                        {
                            ioitem = m_Pool.AnalogInputs[i];
                            //ioitem.State = m_CtlDevice.ReadAiAsync(ioitem.Id).ToString();
                            short value = 0;
                            string state = "";

                            // TwinCATPlc의 경우, ioitem.Channel이 1(IoType.DI)이면 VarType.In을 의미함.
                            // 이 경우, 변수의 타입에 따라 결과값의 타입이 달라지므로 일괄적으로 string형으로 화면에 보여주도록 한다.
                            if (m_Pool.BusType == FieldBusType.TwinCATPlc && ioitem.Channel == (int)VarType.In)
                                state = (string)m_CtlDevice.Read(ioitem.Id, IoType.AI, ioitem.Channel, ioitem.Terminal, ioitem.Node);
                            else value = m_CtlDevice.ReadAiAsync(ioitem.Id);

                            switch (m_SelectedDisplayFormat)
                            {
                                case Format.ASCII:
                                    if (state == "") state = XFunc.ConvertToString(value, ByteOrder.BigEndian);
                                    break;
                                case Format.BCD:
                                    if (state == "") state = string.Format("{0:X4}h", value);
                                    break;
                                case Format.HEX:
                                    if (state == "") state = string.Format("{0:X4}h", value);
                                    break;
                                default:
                                    if (state == "") state = value.ToString();
                                    break;
                            }
                            ioitem.State = state;
                        }
                    }
                    catch (Exception error)
                    {
                        Trace.WriteLine(error.ToString());
                        throw;
                    }
                }
                else if (selectedIndex == (int)IoType.AO)
                {
                    int count = m_Pool.AnalogOutputs.Count;
                    for (int i = 0; i < count; i++)
                    {
                        ioitem = m_Pool.AnalogOutputs[i];
                        //ioitem.State = m_CtlDevice.ReadAoAsync(ioitem.Id).ToString();
                        ushort value = 0;
                        string state = "";

                        if (m_Pool.BusType == FieldBusType.TwinCATPlc && ioitem.Channel == (int)VarType.Out)
                            state = (string)m_CtlDevice.Read(ioitem.Id, IoType.AO, ioitem.Channel, ioitem.Terminal, ioitem.Node);
                        else value = m_CtlDevice.ReadAoAsync(ioitem.Id);

                        switch (m_SelectedDisplayFormat)
                        {
                            case Format.ASCII:
                                if (state == "") state = XFunc.ConvertToString((short)value, ByteOrder.BigEndian);
                                break;
                            case Format.BCD:
                                if (state == "") state = string.Format("{0:X4}h", value);
                                break;
                            case Format.HEX:
                                if (state == "") state = string.Format("{0:X4}h", value);
                                break;
                            default:
                                if (state == "") state = value.ToString();
                                break;
                        }
                        ioitem.State = state;
                    }
                }


                DataGridView view = m_DataGridView[selectedIndex];
                int stateColumnIndex = view.ColumnCount - 1;
                int rows = view.RowCount;

                for (int i = 0; i < rows; i++)
                {
                    SetStateCellBackColor(view.Rows[i].Cells[stateColumnIndex]);
                }
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (m_Mode == OpMode.System)
            //{
            //    DataGridView view = sender as DataGridView;
            //    DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    string value = cell.Value.ToString();
            //    Color color;
            //    if (value == bool.TrueString)
            //    {
            //        color = Color.GreenYellow;
            //    }
            //    else if (value == bool.FalseString)
            //    {
            //        color = Color.WhiteSmoke;
            //    }
            //    else
            //    {
            //        color = Color.WhiteSmoke;
            //    }

            //    cell.Style.BackColor = color;
            //}
        }

        private void checkBoxFilterOn_CheckedChanged(object sender, EventArgs e)
        {
            m_FilterOn = (sender as CheckBox).Checked;

            if (!m_FilterOn)
            {
                this.textBoxFilter.Text = "";
            }

            Filtering();
        }

        private void buttonFiltering_Click(object sender, EventArgs e)
        {
            if (!m_FilterOn)
            {
                this.checkBoxFilterOn.Checked = true;
            }
            else
            {
                Filtering();
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            m_FilterString = (sender as TextBox).Text;
        }

        private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.buttonFiltering.Focus();
                SendKeys.SendWait("{ENTER}");
            }
        }

        private void Filtering()
        {
            string[] keys = m_FilterString.Split(' ');

            if (m_Mode == OpMode.Select || m_Mode == OpMode.MultiSelect)
            {
                UpdateEditViewByFilter(m_Pool, m_SelectedIoType, keys);
            }
            else
            {
                UpdateEditViewByFilter(m_Pool, (IoType)tabControl1.SelectedIndex, keys);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtering();

            UpdateComboBoxDisplayFormat();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (((DataGridView)sender).Columns[e.ColumnIndex].HeaderText == m_ColumHeaderNameWiringNo)
            {
                if (m_Pool.WireNumberMode != WireNumberingMode.Editable)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MakeComboBoxDisplayFormat()
        {
            Array format = Enum.GetValues(typeof(Format));
            this.comboBoxDisplayFormat.DataSource = format;
            UpdateComboBoxDisplayFormat();

            this.comboBoxDisplayFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplayType_SelectedIndexChanged);
        }

        private void UpdateComboBoxDisplayFormat()
        {
            IoType selectedIoType = (IoType)this.tabControl1.SelectedIndex;
            this.comboBoxDisplayFormat.Visible = m_Mode == OpMode.System && (selectedIoType == IoType.AI || selectedIoType == IoType.AO);
            this.comboBoxDisplayFormat.SelectedIndex = (int)(m_SelectedDisplayFormat);
        }

        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SelectedDisplayFormat = (Format)this.comboBoxDisplayFormat.SelectedIndex;
        }
    }
}

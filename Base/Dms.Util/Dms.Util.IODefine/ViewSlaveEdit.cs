using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using static Dms.Util.IODefine.EcSlaveItem;

namespace Dms.Util.IODefine
{
    public partial class ViewSlaveEdit : UserControl
    {
        #region Enums
        public enum OpMode
        {
            Config,
            Select,
            MultiSelect,
            System
        }
        #endregion

        #region Const
        const int TABINDEX_Servo = 0;
        const int TABINDEX_BLDC = 1;
        const int TABINDEX_Inverter = 2;
        const int TABINDEX_DI = 3;
        const int TABINDEX_DO = 4;
        const int TABINDEX_AI = 5;
        const int TABINDEX_AO = 6;
        const int TABINDEX_AP = 7;
        #endregion

        #region Fields
        private OpMode m_Mode = OpMode.Config;
        private static Format m_SelectedDisplayFormat = Format.NUMBER;

        private bool m_FormLoaded = false;
        private bool m_DataGridViewInitialized = false;
        protected ICtlDevice m_CtlDevice = null;
        protected ICtlDevice_Adv m_CtlDevice_Adv = null;
        private static string m_FilterString = "";
        private static bool m_FilterOn = false;
        private bool m_ShowNodeInfo = true;

        private List<DataGridView> m_DataGridView = new List<DataGridView>();
        private BindingSource[] m_BindSource;
        private IoDefines m_Pool = null;
        private EcSlaveItemType m_SelectedSlaveItemType;
        private DataGridViewSelectedRowCollection m_SelectedRows;
        private string m_SelectedName;
        private List<string> m_SelectedNames = new List<string>();

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
        public ViewSlaveEdit()
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

        #region Public Initialize
        public void Initialize(IoDefines pool, OpMode mode, ICtlDevice ctlDevice)
        {
            Initialize(pool, mode);

            AppConfig config = AppConfig.Instance;
            if (config.AutoStart && m_Mode == OpMode.System)
            {
                m_CtlDevice = ctlDevice;
                m_CtlDevice_Adv = ctlDevice as ICtlDevice_Adv;
            }
        }

        public void Initialize(IoDefines pool, OpMode mode)
        {
            m_Pool = pool;
            m_Mode = mode;

            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;

            if (m_FilterOn)
                Filtering();
            else
                UpdateEditView(pool, mode);
        }

        public void InitializeByFilter(IoDefines pool, EcSlaveItemType itemType, params string[] keys)
        {
            m_Pool = pool;

            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;

            IoDefines poolFilter = FilteredPool(pool, itemType, keys);
            Initialize(poolFilter, m_Mode);
        }
        #endregion

        #region Methods
        private void MakeComboBoxDisplayFormat()
        {
            Array format = Enum.GetValues(typeof(Format));
            this.comboBoxDisplayFormat.DataSource = format;
            UpdateComboBoxDisplayFormat();

            this.comboBoxDisplayFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplayType_SelectedIndexChanged);
        }

        private void UpdateComboBoxDisplayFormat()
        {
            //IoType selectedIoType = (IoType)this.tabControl1.SelectedIndex;
            //this.comboBoxDisplayFormat.Visible = m_Mode == OpMode.System && (selectedIoType == IoType.AI || selectedIoType == IoType.AO);
            this.comboBoxDisplayFormat.Visible = m_Mode == OpMode.System && (tabControl1.SelectedIndex == TABINDEX_AI || tabControl1.SelectedIndex == TABINDEX_AO);
            this.comboBoxDisplayFormat.SelectedIndex = (int)(m_SelectedDisplayFormat);
        }

        private void Filtering()
        {
            string[] keys = m_FilterString.Split(' ');

            EcSlaveItemType slaveType = m_Mode == OpMode.Select || m_Mode == OpMode.MultiSelect ?
                                        m_SelectedSlaveItemType :
                                        GetSlaveTypeBySelectedTabIndex(tabControl1.SelectedIndex);

            UpdateEditViewByFilter(m_Pool, slaveType, keys);
        }

        private EcSlaveItemType GetSlaveTypeBySelectedTabIndex(int index)
        {
            switch (index)
            {
                case TABINDEX_Servo:
                    return EcSlaveItemType.Servo;
                case TABINDEX_BLDC:
                    return EcSlaveItemType.BLDC;
                case TABINDEX_Inverter:
                    return EcSlaveItemType.Inverter;
                case TABINDEX_DI:
                    return EcSlaveItemType.DI;
                case TABINDEX_DO:
                    return EcSlaveItemType.DO;
                case TABINDEX_AI:
                    return EcSlaveItemType.AI;
                case TABINDEX_AO:
                    return EcSlaveItemType.AO;
                case TABINDEX_AP:
                    return EcSlaveItemType.AP;
                default:
                    return EcSlaveItemType.Null;
            }
        }

        private void UpdateEditViewByFilter(IoDefines sourcePool, EcSlaveItemType itemType, params string[] keys)
        {
            IoDefines poolFilter = FilteredPool(sourcePool, itemType, keys);
            UpdateEditView(poolFilter, m_Mode);
        }

        private IoDefines FilteredPool(IoDefines sourcePool, EcSlaveItemType itemType, params string[] keys)
        {
            m_SelectedSlaveItemType = itemType;
            int tabIndex = 0;

            IoDefines poolFilter = new IoDefines();
            poolFilter.Clone(m_Pool);

            if (keys.Length > 0)
            {
                switch (m_SelectedSlaveItemType)
                {
                    case EcSlaveItemType.Servo:
                        {
                            List<EcSlaveItem_Servo> filter = FilterMatch(sourcePool.SlaveServos, keys);
                            if (filter.Count > 0) poolFilter.SlaveServos = filter;
                            tabIndex = TABINDEX_Servo;
                        }
                        break;
                    case EcSlaveItemType.BLDC:
                        {
                            List<EcSlaveItem_BLDC> filter = FilterMatch(sourcePool.SlaveBLDCs, keys);
                            if (filter.Count > 0) poolFilter.SlaveBLDCs = filter;
                            tabIndex = TABINDEX_BLDC;
                        }
                        break;
                    case EcSlaveItemType.Inverter:
                        {
                            List<EcSlaveItem_Inverter> filter = FilterMatch(sourcePool.SlaveInverters, keys);
                            if (filter.Count > 0) poolFilter.SlaveInverters = filter;
                            tabIndex = TABINDEX_Inverter;
                        }
                        break;
                    case EcSlaveItemType.DI:
                        {
                            List<EcSlaveItem_DI> filter = FilterMatch(sourcePool.SlaveDigitalInputs, keys);
                            if (filter.Count > 0) poolFilter.SlaveDigitalInputs = filter;
                            tabIndex = TABINDEX_DI;
                        }
                        break;
                    case EcSlaveItemType.DO:
                        {
                            List<EcSlaveItem_DO> filter = FilterMatch(sourcePool.SlaveDigitalOutputs, keys);
                            if (filter.Count > 0) poolFilter.SlaveDigitalOutputs = filter;
                            tabIndex = TABINDEX_DO;
                        }
                        break;
                    case EcSlaveItemType.AI:
                        {
                            List<EcSlaveItem_AI> filter = FilterMatch(sourcePool.SlaveAnalogInputs, keys);
                            if (filter.Count > 0) poolFilter.SlaveAnalogInputs = filter;
                            tabIndex = TABINDEX_AI;
                        }
                        break;
                    case EcSlaveItemType.AO:
                        {
                            List<EcSlaveItem_AO> filter = FilterMatch(sourcePool.SlaveAnalogOutputs, keys);
                            if (filter.Count > 0) poolFilter.SlaveAnalogOutputs = filter;
                            tabIndex = TABINDEX_AO;
                        }
                        break;
                    case EcSlaveItemType.AP:
                        {
                            List<EcSlaveItem_AP> filter = FilterMatch(sourcePool.SlaveAPs, keys);
                            if (filter.Count > 0) poolFilter.SlaveAPs = filter;
                            tabIndex = TABINDEX_AP;
                        }
                        break;
                }
            }

            this.tabControl1.SelectedIndex = tabIndex;

            return poolFilter;
        }

        private List<EcSlaveItem_Servo> FilterMatch(List<EcSlaveItem_Servo> source, params string[] keys)
        {
            List<EcSlaveItem_Servo> filter = new List<EcSlaveItem_Servo>();

            foreach (EcSlaveItem_Servo item in source)
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

            return filter;
        }
        private List<EcSlaveItem_BLDC> FilterMatch(List<EcSlaveItem_BLDC> source, params string[] keys)
        {
            List<EcSlaveItem_BLDC> filter = new List<EcSlaveItem_BLDC>();

            foreach (EcSlaveItem_BLDC item in source)
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

            return filter;
        }
        private List<EcSlaveItem_Inverter> FilterMatch(List<EcSlaveItem_Inverter> source, params string[] keys)
        {
            List<EcSlaveItem_Inverter> filter = new List<EcSlaveItem_Inverter>();

            foreach (EcSlaveItem_Inverter item in source)
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

            return filter;
        }
        private List<EcSlaveItem_DI> FilterMatch(List<EcSlaveItem_DI> source, params string[] keys)
        {
            List<EcSlaveItem_DI> filter = new List<EcSlaveItem_DI>();

            foreach (EcSlaveItem_DI item in source)
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

            return filter;
        }
        private List<EcSlaveItem_DO> FilterMatch(List<EcSlaveItem_DO> source, params string[] keys)
        {
            List<EcSlaveItem_DO> filter = new List<EcSlaveItem_DO>();

            foreach (EcSlaveItem_DO item in source)
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

            return filter;
        }
        private List<EcSlaveItem_AI> FilterMatch(List<EcSlaveItem_AI> source, params string[] keys)
        {
            List<EcSlaveItem_AI> filter = new List<EcSlaveItem_AI>();

            foreach (EcSlaveItem_AI item in source)
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

            return filter;
        }
        private List<EcSlaveItem_AO> FilterMatch(List<EcSlaveItem_AO> source, params string[] keys)
        {
            List<EcSlaveItem_AO> filter = new List<EcSlaveItem_AO>();

            foreach (EcSlaveItem_AO item in source)
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

            return filter;
        }
        private List<EcSlaveItem_AP> FilterMatch(List<EcSlaveItem_AP> source, params string[] keys)
        {
            List<EcSlaveItem_AP> filter = new List<EcSlaveItem_AP>();

            foreach (EcSlaveItem_AP item in source)
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

            return filter;
        }

        private void UpdateEditView(IoDefines pool, OpMode mode)
        {
            m_Mode = mode;

            m_BindSource = new BindingSource[8];

            int count = m_BindSource.Length;
            for (int i = 0; i < count; i++)
            {
                m_BindSource[i] = new BindingSource();
                m_BindSource[i].DataSource = pool;
            }

            DataGridViewInitialize(this.dataGridViewServo, m_BindSource[0], "SlaveServos");
            DataGridViewInitialize(this.dataGridViewBLDC, m_BindSource[1], "SlaveBLDCs");
            DataGridViewInitialize(this.dataGridViewInverter, m_BindSource[2], "SlaveInverters");
            DataGridViewInitialize(this.dataGridViewDI, m_BindSource[3], "SlaveDigitalInputs");
            DataGridViewInitialize(this.dataGridViewDO, m_BindSource[4], "SlaveDigitalOutputs");
            DataGridViewInitialize(this.dataGridViewAI, m_BindSource[5], "SlaveAnalogInputs");
            DataGridViewInitialize(this.dataGridViewAO, m_BindSource[6], "SlaveAnalogOutputs");
            DataGridViewInitialize(this.dataGridViewAP, m_BindSource[7], "SlaveAPs");

            UpdateComboBoxDisplayFormat();

            m_DataGridViewInitialized = true;
        }

        private void DataGridViewInitialize(DataGridView gridview, BindingSource bindSoruce, string member)
        {
            bindSoruce.DataMember = member;

            gridview.AutoGenerateColumns = false;
            gridview.DataSource = bindSoruce;

            if (!m_DataGridViewInitialized)
            {
                bool showNodeInfo = m_Mode == OpMode.Config;
                showNodeInfo &= m_ShowNodeInfo;

                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.Name = "colId";
                colId.HeaderText = "ID.";
                gridview.Columns.Add(colId);

                DataGridViewTextBoxColumn colSNo = new DataGridViewTextBoxColumn();
                colSNo.DataPropertyName = "SlaveNo";
                colSNo.Name = "colSlaveNo";
                colSNo.HeaderText = "S.No.";
                colSNo.Visible = false; //  Slave No는 표시 할 필요 없음
                gridview.Columns.Add(colSNo);

                DataGridViewTextBoxColumn colANo = new DataGridViewTextBoxColumn();
                colANo.DataPropertyName = "AliasNo";
                colANo.Name = "colAliasNo";
                colANo.HeaderText = "A.No.";
                colANo.Visible = showNodeInfo;
                gridview.Columns.Add(colANo);

                DataGridViewTextBoxColumn colCh = new DataGridViewTextBoxColumn();
                colCh.DataPropertyName = "Channel";
                colCh.Name = "colChannel";
                colCh.HeaderText = "Ch.";
                colCh.Visible = showNodeInfo;
                gridview.Columns.Add(colCh);

                DataGridViewTextBoxColumn colAddr = new DataGridViewTextBoxColumn();
                colAddr.DataPropertyName = "Address";
                colAddr.Name = "colAddress";
                colAddr.HeaderText = "Addr.";
                colAddr.Visible = !showNodeInfo;
                gridview.Columns.Add(colAddr);

                DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                colName.DataPropertyName = "Name";
                colName.Name = "colName";
                colName.HeaderText = "Name";
                colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridview.Columns.Add(colName);

                if (member == "SlaveDigitalInputs")
                {
                    DataGridViewComboBoxColumn col5 = new DataGridViewComboBoxColumn();
                    col5.DataPropertyName = "ActiveType";
                    col5.Name = "colActiveType";
                    col5.DataSource = Enum.GetValues(typeof(ActiveType));
                    col5.HeaderText = "Active";
                    gridview.Columns.Add(col5);
                }

                if (member == "SlaveAPs")
                {
                    DataGridViewComboBoxColumn colPeerType = new DataGridViewComboBoxColumn();
                    colPeerType.DataPropertyName = "PeerType";
                    colPeerType.Name = "colPeerType";
                    colPeerType.DataSource = Enum.GetValues(typeof(PeerType));
                    colPeerType.HeaderText = "Type";
                    gridview.Columns.Add(colPeerType);

                    DataGridViewTextBoxColumn colPeerId = new DataGridViewTextBoxColumn();
                    colPeerId.DataPropertyName = "PeerId";
                    colPeerId.Name = "colPeerId";
                    colPeerId.HeaderText = "Peer Id";
                    gridview.Columns.Add(colPeerId);
                }

                if (m_Mode == OpMode.System)
                {
                    DataGridViewTextBoxColumn col6 = new DataGridViewTextBoxColumn();
                    col6.DataPropertyName = "State";
                    col6.HeaderText = "State";
                    col6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    gridview.Columns.Add(col6);
                }
            }

            SetOpMode(gridview, m_Mode);
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
            }
        }

        private void SetStateCellBackColor(DataGridViewCell cell)
        {
            if (cell.Value != null)
            {
                bool isExistDevice = (int)cell.OwningRow.Cells["colSlaveNo"].Value >= 0;

                Color onColor = Color.GreenYellow;
                Color offColor = Color.WhiteSmoke;

                string value = cell.Value.ToString();

                if (!isExistDevice)
                {

                }
                else if (value == "ON" || value == "Paired")
                {
                    cell.Style.BackColor = onColor;
                }
                else if (value == "OFF" || value == "Unpaired")
                {
                    cell.Style.BackColor = offColor;
                }
                else
                {   //AI/AO값을 update 할려면 아래처럼 해줘야 한다. 이상하다.
                    cell.Style.BackColor = onColor;
                    cell.Style.BackColor = offColor;
                }
            }
        }

        private void SetGridColor(DataGridView gridView)
        {
            int SlaveCount = -1;
            int oldChannel = -1;

            foreach (DataGridViewRow row in gridView.Rows)
            {
                int SlaveNo = (int)row.Cells["colSlaveNo"].Value;
                int Channel = (int)row.Cells["colChannel"].Value;

                if (Channel <= oldChannel) SlaveCount++;

                oldChannel = Channel;

                Color slaveBackColor;
                if (m_Mode == OpMode.System && SlaveNo < 0)
                {
                    slaveBackColor = Color.DimGray;
                }
                else if (SlaveCount % 2 == 0)
                {
                    slaveBackColor = Color.Lavender;
                }
                else
                {
                    slaveBackColor = Color.White;
                }

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = slaveBackColor;

                    if (cell.Value != null && cell.Value.GetType() == typeof(ActiveType))
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
                }
            }
        }
        #endregion

        #region EventHandler
        private void ViewSlaveEdit_Load(object sender, EventArgs e)
        {
            m_DataGridView.Add(this.dataGridViewServo);
            m_DataGridView.Add(this.dataGridViewBLDC);
            m_DataGridView.Add(this.dataGridViewInverter);
            m_DataGridView.Add(this.dataGridViewDI);
            m_DataGridView.Add(this.dataGridViewDO);
            m_DataGridView.Add(this.dataGridViewAI);
            m_DataGridView.Add(this.dataGridViewAO);
            m_DataGridView.Add(this.dataGridViewAP);

            m_FormLoaded = true;
        }

        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SelectedDisplayFormat = (Format)this.comboBoxDisplayFormat.SelectedIndex;
        }

        private void view_SelectionChanged(object sender, EventArgs e)
        {
            m_SelectedRows = (sender as DataGridView).SelectedRows;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Mode == OpMode.System && m_FormLoaded && m_DataGridViewInitialized)
            {
                if (m_CtlDevice.Uninitializing || !m_CtlDevice.Initialized) return;

                //jemoon : 현재상태 update
                int selectedIndex = this.tabControl1.SelectedIndex;
                EcSlaveItem slaveitem;
                switch (selectedIndex)
                {
                    case TABINDEX_Servo:
                        {
                            if (m_CtlDevice_Adv == null) break;
                            int count = m_Pool.SlaveServos.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveServos[i];
                                slaveitem.State = m_CtlDevice_Adv.ServoIsAlarm(slaveitem.Id) ? "Alarm" : "Normal";
                            }
                        }
                        break;
                    case TABINDEX_BLDC:
                        {
                            if (m_CtlDevice_Adv == null) break;
                            int count = m_Pool.SlaveBLDCs.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveBLDCs[i];
                                slaveitem.State = m_CtlDevice_Adv.BldcGetIsAlarm(slaveitem.Id) ? "Alarm" : "Normal";
                            }
                        }
                        break;
                    case TABINDEX_Inverter:
                        {
                            if (m_CtlDevice_Adv == null) break;
                            int count = m_Pool.SlaveInverters.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveInverters[i];
                                slaveitem.State = m_CtlDevice_Adv.InverterGetIsAlarm(slaveitem.Id) ? "Alarm" : "Normal";
                            }
                        }
                        break;
                    case TABINDEX_DI:
                        {
                            int count = m_Pool.SlaveDigitalInputs.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveDigitalInputs[i];
                                slaveitem.State = m_CtlDevice.ReadDiAsync(slaveitem.Id) ? "ON" : "OFF";
                            }
                        }
                        break;
                    case TABINDEX_DO:
                        {
                            int count = m_Pool.SlaveDigitalOutputs.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveDigitalOutputs[i];
                                slaveitem.State = m_CtlDevice.ReadDoAsync(slaveitem.Id) ? "ON" : "OFF";
                            }
                        }
                        break;
                    case TABINDEX_AI:
                        {
                            try
                            {
                                int count = m_Pool.SlaveAnalogInputs.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    slaveitem = m_Pool.SlaveAnalogInputs[i];

                                    short value = m_CtlDevice.ReadAiAsync(slaveitem.Id);
                                    string state = "";

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
                                    slaveitem.State = state;
                                }
                            }
                            catch (Exception error)
                            {
                                Trace.WriteLine(error.ToString());
                                throw;
                            }
                        }
                        break;
                    case TABINDEX_AO:
                        {
                            int count = m_Pool.SlaveAnalogOutputs.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveAnalogOutputs[i];

                                ushort value = m_CtlDevice.ReadAoAsync(slaveitem.Id);
                                string state = "";

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
                                slaveitem.State = state;
                            }
                        }
                        break;
                    case TABINDEX_AP:
                        {
                            if (m_CtlDevice_Adv == null) break;

                            int count = m_Pool.SlaveAPs.Count;
                            for (int i = 0; i < count; i++)
                            {
                                slaveitem = m_Pool.SlaveAPs[i];

                                PairingState state = m_CtlDevice_Adv.ApGetPairingState(slaveitem.Id);

                                slaveitem.State = state.ToString();
                            }
                        }
                        break;
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

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView view = sender as DataGridView;
            SetGridColor(view);
            view.ClearSelection();
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            DataGridView view = sender as DataGridView;
            m_SelectedName = view.Rows[e.RowIndex].Cells["colName"].Value as string;
            if (m_SelectedNames.Contains(m_SelectedName))
            {
                m_SelectedNames.Remove(m_SelectedName);
            }
            else
            {
                m_SelectedNames.Add(m_SelectedName);
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
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

                if (m_CtlDevice != null)
                {
                    EcSlaveItem slaveitem = source.Current as EcSlaveItem;
                    int id = slaveitem.Id;
                    switch (slaveitem.SlaveItemType)
                    {
                        case EcSlaveItemType.Servo:
                            break;
                        case EcSlaveItemType.BLDC:
                            break;
                        case EcSlaveItemType.Inverter:
                            break;
                        case EcSlaveItemType.DI:
                            {
                                bool curValue = m_CtlDevice.ReadDiAsync(id);
                                m_CtlDevice.WriteDiSync(id, !curValue);
                            }
                            break;
                        case EcSlaveItemType.DO:
                            {
                                bool curValue = m_CtlDevice.ReadDoAsync(id);
                                m_CtlDevice.WriteDoAsync(id, !curValue);
                            }
                            break;
                        case EcSlaveItemType.AI:
                            {
                                KeyInValidation validation = new KeyInValidation();

                                ushort curValue = (ushort)(m_CtlDevice.ReadAiAsync(id));
                                validation.Format = OptionFormat.Digit;
                                validation.High = "65535";
                                validation.Low = "0";
                                string newValueString = validation.ShowEditDialog("Key In value", curValue.ToString());
                                ushort newValue = Convert.ToUInt16(newValueString);
                                if (curValue != newValue)
                                {
                                    m_CtlDevice.WriteAiSync(id, (short)newValue);
                                }
                            }
                            break;
                        case EcSlaveItemType.AO:
                            {
                                KeyInValidation validation = new KeyInValidation();

                                ushort curValue = m_CtlDevice.ReadAoAsync(id);
                                validation.Format = OptionFormat.Digit;
                                validation.High = "65535";
                                validation.Low = "0";
                                string newValueString = validation.ShowEditDialog("Key In value", curValue.ToString());
                                ushort newValue = Convert.ToUInt16(newValueString);
                                if (curValue != newValue)
                                {
                                    m_CtlDevice.WriteAoAsync(id, newValue);
                                }
                            }
                            break;
                        case EcSlaveItemType.AP:    //  BM 작업 예정
                            {

                            }
                            break;
                    }
                }

                //jemoon : Controller를 직접 이용하도록 수정
                //IoItem ioItem = source.Current as IoItem;
                //SetIoStateEventHandler eHandle = OnSetIoState;
                //if (eHandle != null)
                //{
                //    //    OnSetIoState(view, new IoStateEventArgs(ioItem.IoType, ioItem.Id));
                //}
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (((DataGridView)sender).Columns[e.ColumnIndex].HeaderText == m_ColumHeaderNameWiringNo)
            //{
            //    if (m_Pool.WireNumberMode != WireNumberingMode.Editable)
            //    {
            //        e.Cancel = true;
            //    }
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtering();

            UpdateComboBoxDisplayFormat();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using System.Collections;
using Dms.Device;

namespace Dms.Control
{
    public partial class ViewHpmj : UserControl
    {
        #region Enum
        private enum StateGridHeader
        {
            No, Item, Value
        }
        #endregion 

        #region Fields
        private ClientManager m_Client;
        private Hpmj m_Device = null;
        private bool m_Initialized = false;
        private Dms.Device.HpmjItemsHandler m_Handler = null;
        private Dms.Device.HpmjItems m_Items = null;
        private Dms.Data.SetupHpmjInfoProvider m_HpmjProvider;
        #endregion

        #region Properties
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        public ViewHpmj()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);    //깜박임 방지
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(Hpmj device, HpmjItemsHandler handler, Dms.Data.SetupHpmjInfoProvider provider)
        {
            if (device == null) return;

            m_Device = device;
            m_Client = ClientManager.Instance;

            m_Handler = handler;
            m_Items = new Dms.Device.HpmjItems();
            m_Items.Clone(m_Handler.GetItems());

            m_HpmjProvider = provider;
            m_HpmjProvider.Viewer.Add(this.viewSetupHpmj);
            this.viewSetupHpmj.InitDataView(provider.Adapter.Table, true);

            m_Initialized = true;

            InitView();
            InitGrid();
        }

        private void InitView()
        {
            SetTag();
        }

        private void InitGrid()
        {
            this.dataStateGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.HeaderText = StateGridHeader.No.ToString();
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataStateGridView.Columns.Add(colNo);

            DataGridViewTextBoxColumn item = new DataGridViewTextBoxColumn();
            item.HeaderText = StateGridHeader.Item.ToString();
            item.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataStateGridView.Columns.Add(item);

            DataGridViewTextBoxColumn data = new DataGridViewTextBoxColumn();
            data.HeaderText = StateGridHeader.Value.ToString();
            data.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataStateGridView.Columns.Add(data);

            foreach (DataGridViewColumn column in dataStateGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            m_Items = m_Handler.GetItems();
            if (m_Items.Count > 0)
                this.dataStateGridView.Rows.Add(m_Items.Count);

            SetData();

            tmrUpdateState.Enabled = true;
        }

        private void SetData()
        {
            int count = 0;
            foreach (Dms.Device.HpmjItem item in m_Items.Items)
            {
                this.dataStateGridView.Rows[count].SetValues(item.Id + 1, item.Name,
                                                        item.CurUsedTime, item.MaxUsedTime);
                count++;
            }
        }

        private void SetTag()
        {
            btnStart.Tag = Common.PumpAct.Stop;
            btnStop.Tag = Common.PumpAct.Run;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            chkPing.Checked = false;
            //chkAlarm.Checked = m_Device.IsAlarm();
            //chkRun.Checked = m_Device.Pump.IsRun();
            //chkRemote.Checked = m_Device.doManual_On.GetState();

            labelInverter.Text = string.Format("{0:00.00} Hz", m_Device.IfFlag.CurrentHertz);

            if (m_Device.IfFlag.PIDError || m_Device.IfFlag.Alarm)
            {
                labelInverter.BackColor = System.Drawing.Color.DarkOrange;
            }
            else if (m_Device.IfFlag.Ready)
            {
                labelInverter.BackColor = System.Drawing.Color.Lime;
            }
            else
            {
                labelInverter.BackColor = System.Drawing.Color.Yellow;
            }

            btnStart.Enabled = !m_Device.IfFlag.PumpRun;
            btnStop.Enabled = m_Device.IfFlag.PumpRun;
            btnRemote.Enabled = !m_Device.IfFlag.Remote;
            btnLocal.Enabled = m_Device.IfFlag.Remote;

            chkReady.Checked = m_Device.IfFlag.Ready;
            chkRun.Checked = m_Device.IfFlag.PumpRun;
            chkRemote.Checked = m_Device.IfFlag.Remote;
            chkAlarm.Checked = m_Device.IfFlag.Alarm | m_Device.IfFlag.PIDError;
        }

        private void btnRemote_Click(object sender, EventArgs e)
        {
            //m_Client.SendCommand(Command.HpmjManual, m_Device, HpmjAct.Remote);
            if (m_Device.IfFlag.PumpRun)
            {
                m_Device.IfFlag.PumpRun = false;
            }
            m_Device.IfFlag.Remote = true;
        }

        private void btnLocal_Click(object sender, EventArgs e)
        {
            //m_Client.SendCommand(Command.HpmjManual, m_Device, HpmjAct.Local);
            m_Device.IfFlag.Remote = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //m_Client.SendCommand(Command.HpmjManual, m_Device, HpmjAct.PumpRun);
            m_Device.IfFlag.PumpRun = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //m_Client.SendCommand(Command.HpmjManual, m_Device, HpmjAct.PumpStop);
            m_Device.IfFlag.PumpRun = false;
        }
    }
}
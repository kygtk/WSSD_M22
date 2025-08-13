using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Client;
using Dms.Server;
using Dms.Device;
using Dms.Data;
using Dms.Common;
using System.Collections;


namespace Dms.HMI
{
    public partial class JobsTabHpmjCtrl : UserControl
    {
        #region Enum
        private enum StateGridHeader
        {
            No, Item, Value
        }
        #endregion

        #region Fields
        private ClientManager m_Client;
        private GenInfoHandler m_GenInfo; // 11.01.31 minhan
        private ServerManager m_Server; // 11.01.31 minhan
        private Dms.Device.HpmjInterface m_Device = null;
        private Dms.Device.Hpmj m_HpmjUnit; // 11.01.31 minhan
        private bool m_Initialized = false;
        private Dms.Device.HpmjItemsHandler m_Handler = null;
        private Dms.Device.HpmjItems m_Items = null;
        private SetupHpmjInfoProvider m_HpmjProvider;
        private short m_Buf; // 11.01.27 minhan
        private string m_Msg; // 11.01.31 minhan
        private string m_oldMsg; // 11.01.31 minhan
        #endregion

        #region Properties
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        public JobsTabHpmjCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);    //깜박임 방지
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize()
        {
            ClientManager client = ClientManager.Instance;
            DmsComponents components = client.EventSubscriber.Server.DmsComponents;
            _GenericCollection<HpmjInterface> hpmjUnits = components.ComponentContainer.GetCollection<HpmjInterface>();

            if (hpmjUnits == null || hpmjUnits.Count == 0) return;

            this.Initialize(hpmjUnits[0], client.HpmjItemsHandler, client.DataProvider.SetupHpmjInfo);

        }

        public void Initialize(Dms.Device.HpmjInterface device, Dms.Device.HpmjItemsHandler handler, Dms.Data.SetupHpmjInfoProvider provider)
        {
            if (device == null) return;

            m_Device = device;
            m_HpmjUnit = eqpHpmjs._HPMJ_Unit; // 11.01.31 minhan
            m_Client = ClientManager.Instance;
            m_GenInfo = GenInfoHandler.Instance; // 11.01.31 minhan
            m_Server = ServerManager.Instance;
            m_Handler = handler;
            m_Items = new Dms.Device.HpmjItems();
            m_Items.Clone(m_Handler.GetItems());

            m_HpmjProvider = provider;
            m_HpmjProvider.Viewer.Add(this.viewSetupHpmj);
            this.viewSetupHpmj.InitDataView(provider.Adapter.Table, true);
            
            m_Initialized = true;
            m_Buf = 0;
            m_Msg = "";
            m_oldMsg = "";

            //RunningTime.Enabled = false; // 11.01.31 minhan
            //PackingTime.Enabled = false;
            //FilterChangeTime.Enabled = false;
            //CO2BubblerChangeTime.Enabled = false;

            //PackingTimeSet.Enabled = false;
            //FilterChangeTimeSet.Enabled = false;
            //CO2BubblerChangeTimeSet.Enabled = false;

            InitView();
            InitGrid();
        }

        private void InitView()
        {
            SetTag();
        }

        private void InitGrid()
        {
            //this.dataStateGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.HeaderText = StateGridHeader.No.ToString();
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataStateGridView.Columns.Add(colNo);

            DataGridViewTextBoxColumn item = new DataGridViewTextBoxColumn();
            item.HeaderText = StateGridHeader.Item.ToString();
            item.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //this.dataStateGridView.Columns.Add(item);

            DataGridViewTextBoxColumn data = new DataGridViewTextBoxColumn();
            data.HeaderText = StateGridHeader.Value.ToString();
            data.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            tmrUpdateState.Enabled = true;
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

        private void UpdateState() // 11.01.27 minhan
        {
            try
            {
                // check
                chkReady.Checked = m_Device.diReady.GetState();
                chkRun.Checked = m_Device.diRun_Pump.GetState();
                chkRemote.Checked = m_Device.diRemote.GetState();
                chkAlarm.Checked = m_Device.diAlarmHeavy.GetState();
                chkWarning.Checked = m_Device.diAlarmLight.GetState();
                chkLocal.Checked = m_Device.diLocal.GetState();

                if (GlobalVar.HpmjOnline) // 11.01.27 minhan
                {
                    chkPing.Checked = true;
                    m_Buf = 0;
                    m_Buf = m_Device.miwTotal_Running_Time_Count.GetState();
                    m_GenInfo.HpmjRunningTime = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwPump_Packing_Change_Count.GetState();
                    m_GenInfo.HpmjPackingTime = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwFilter_Change_Count.GetState();
                    m_GenInfo.HpmjFilterTime = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwCO2_Bubbler_Change_Count.GetState();
                    m_GenInfo.HpmjCo2BubblerTime = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwPump_Packing_Change_Set.GetState();
                    m_GenInfo.HpmjPackingTimeSet = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwFilter_Change_Count_Set.GetState();
                    m_GenInfo.HpmjFilterTimeSet = m_Buf.ToString();
                    m_Buf = 0;
                    m_Buf = m_Device.miwCO2_Bubbler_Change_Set.GetState();
                    m_GenInfo.HpmjCo2BubblerTimeSet = m_Buf.ToString();

                    m_GenInfo.HpmjInHz = string.Format("{0}", (double)m_HpmjUnit.InverHertz.CurAdc / 10);
                    m_GenInfo.HpmjFlow = string.Format("{0}", (double)m_HpmjUnit.HpmjFlow.CurAdc / 100);
                    m_GenInfo.HpmjInPress = string.Format("{0}", (double)m_HpmjUnit.FilterInPress.CurAdc / 10);
                    m_GenInfo.HpmjOutPress = string.Format("{0}", (double)m_HpmjUnit.FilterOutPress.CurAdc / 10);
                    m_GenInfo.HpmjCurrent = string.Format("{0}", (double)m_HpmjUnit.InvertCurrent.CurAdc / 10);
                    m_GenInfo.HpmjCo2Press = string.Format("{0}", (double)m_HpmjUnit.CO2InPress.CurAdc / 1000);
                    m_GenInfo.HpmjResistivity = string.Format("{0}", (double)m_HpmjUnit.Resistivity.CurAdc / 100);
                    m_GenInfo.HpmjMainDI = string.Format("{0}", (double)m_HpmjUnit.MainDiPress.CurAdc / 1000);
                    m_GenInfo.HpmjCo2Flow = string.Format("{0}", (double)m_HpmjUnit.HpmjCo2Flow.CurAdc / 10); // 11.05.03 minhan
                }
                else
                {
                    chkPing.Checked = false;
                    m_Buf = 0;
                    m_GenInfo.HpmjRunningTime = "";
                    m_GenInfo.HpmjPackingTime = "";
                    m_GenInfo.HpmjFilterTime = "";
                    m_GenInfo.HpmjCo2BubblerTime = "";
                    m_GenInfo.HpmjPackingTimeSet = "";
                    m_GenInfo.HpmjFilterTimeSet = "";
                    m_GenInfo.HpmjCo2BubblerTimeSet = "";
                    m_GenInfo.HpmjInHz = "";
                    m_GenInfo.HpmjFlow = "";
                    m_GenInfo.HpmjInPress = "";
                    m_GenInfo.HpmjOutPress = "";
                    m_GenInfo.HpmjCurrent = "";
                    m_GenInfo.HpmjCo2Press = "";
                    m_GenInfo.HpmjResistivity = "";
                    m_GenInfo.HpmjMainDI = ""; // 11.05.03 minhan
                    m_GenInfo.HpmjCo2Flow = "";
                }

                // btn 11.01.31 minhan
                if (GlobalVar.HpmjOnline) // 11.01.27 minhan
                {
                    btnStart.Enabled = !m_Device.diRun_Pump.GetState() && !m_HpmjUnit.IfFlag.PumpRun && !m_Client.GenInfos.AutoMode;
                    btnStop.Enabled = (m_Device.diRun_Pump.GetState() || m_HpmjUnit.IfFlag.PumpRun) && !m_Client.GenInfos.AutoMode;
                    btnRemote.Enabled = !m_Client.GenInfos.AutoMode;
                    btnLocal.Enabled = !m_Client.GenInfos.AutoMode;
                    btnRunTimeReset.Enabled = !m_Client.GenInfos.AutoMode && !GlobalVar.HpmjRunningCountReset; // 11.02.09 minhan
                    btnFliterCountReset.Enabled = !m_Client.GenInfos.AutoMode && !GlobalVar.HpmjFilterCountReset;
                    btnPumpCountReset.Enabled = !m_Client.GenInfos.AutoMode && !GlobalVar.HpmjPackingCountReset;
                    btnInterlockParaSet.Enabled = !m_Client.GenInfos.AutoMode && !GlobalVar.HpmjInterlockParaSet;
                    btnBuzzerOff.Enabled = !m_Client.GenInfos.AutoMode;
                    btnAlarmReset.Enabled = !m_Client.GenInfos.AutoMode;
                }
                else
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = (m_Device.diRun_Pump.GetState() || m_HpmjUnit.IfFlag.PumpRun) && !m_Client.GenInfos.AutoMode; // stop은 진행.
                    btnRemote.Enabled = false;
                    btnLocal.Enabled = false;
                    btnRunTimeReset.Enabled = false;
                    btnFliterCountReset.Enabled = false;
                    btnPumpCountReset.Enabled = false;
                    btnInterlockParaSet.Enabled = false;
                    btnBuzzerOff.Enabled = false;
                    btnAlarmReset.Enabled = false;
                }
            }
            catch (Exception err)
            {
                m_Msg = err.ToString();

                if (m_Msg != m_oldMsg)
                {
                    m_oldMsg = m_Msg;
                    m_Server.WriteExceptionLog(m_Msg);
                }
            }
        }

        private void btnRemote_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (!GlobalVar.HpmjReady)
            {
                MessageBox.Show("HPMJ Unit No Ready", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (GlobalVar.HpmjRemote)
            {
                MessageBox.Show("Current Mode is Remote", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                m_Device.doRemote_Request.SetState(true);
                m_Device.doLocal_Request.SetState(false);
            }
        }

        private void btnLocal_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (!GlobalVar.HpmjReady)
            {
                MessageBox.Show("HPMJ Unit No Ready", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
            else if (m_Device.diLocal.GetState())
            {
                MessageBox.Show("Current Mode is Local", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                m_Device.doLocal_Request.SetState(true);
                m_Device.doRemote_Request.SetState(false);
            }
        }

        private void btnStart_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (eqpHpmjs._HPMJ_Unit.SetupHpmjUse.GetValue<bool>() &&
                GlobalVar.HpmjRemote &&
                GlobalVar.HpmjReady)
            {
                m_HpmjUnit.IfFlag.PumpRun = true;
            }
            else
            {
                if (!GlobalVar.HpmjRemote)
                {
                    MessageBox.Show("Please HPMJ Remote.", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (!GlobalVar.HpmjReady)
                {
                    MessageBox.Show("Checking HPMJ Status.", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    MessageBox.Show("Please HPMJ Setup is Use", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_HpmjUnit.IfFlag.PumpRun = false;
        }

        private void btnRunTimeReset_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (GlobalVar.HpmjRunningCountReset)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjRunningCountReset = true;
            }
        }

        private void btnPumpCountReset_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (GlobalVar.HpmjPackingCountReset)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjPackingCountReset = true;
            }
        }

        private void btnFliterCountReset_Click(object sender, EventArgs e) // 11.01.31 minhan ready도 봐야 하는지 모르겠네.
        {
            if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (GlobalVar.HpmjFilterCountReset)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjFilterCountReset = true;
            }
        }

        private void btnBuzzerOff_Click(object sender, EventArgs e) // 11.01.31 minhan
        {

            if (m_GenInfo.AutoMode)
            {
                return;
            }
            else if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            //else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            //{
            //    MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //    return;
            //}
            else if (GlobalVar.HpmjBuzzerOff)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjBuzzerOff = true;
            }
        }

        private void btnAlarmReset_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (m_GenInfo.AutoMode)
            {
                return;
            }
            else if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            //else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            //{
            //    MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //    return;
            //}
            else if (GlobalVar.HpmjAlarmReset)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjAlarmReset = true;
            }
        }

        private void btnInterlockParaSet_Click(object sender, EventArgs e) // 11.01.31 minhan
        {
            if (m_GenInfo.AutoMode)
            {
                return;
            }
            else if (!GlobalVar.HpmjOnline)
            {
                MessageBox.Show("Please HPMJ Interface Check!(Offline)", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if ((m_Device.diRun_Pump.GetState() == true) || m_HpmjUnit.IfFlag.PumpRun)
            {
                MessageBox.Show("Please HPMJ Pump Stop", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (!GlobalVar.HpmjReady)
            {
                MessageBox.Show("HPMJ Unit No Ready", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else if (GlobalVar.HpmjInterlockParaSet)
            {
                MessageBox.Show("Current Signal ON", "WSSD", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                GlobalVar.HpmjInterlockParaSet = true;
            }
        }

    }
}

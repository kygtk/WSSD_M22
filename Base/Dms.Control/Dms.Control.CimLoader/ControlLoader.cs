using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Cim;
using Dms.DeviceLibrary;

namespace Dms.Control
{
    public partial class ControlLoader : UserControl
    {
        private ILoader m_Loader = null;
        private IfFlagLoader m_IfLoader = null;
        private CassettePort m_Port = null;
        private List<CassettePort> m_Ports = null;
        private int m_PortIndex = -1;
        private string m_sItem;
        private IRobot m_Robot = null;
        private RobotStage OldRobotUpperHand = new RobotStage();
        private List<Glasses> oldPortGlasses = new List<Glasses>();

        public ControlLoader()
        {
            InitializeComponent();
        }

        public void Initialize(ILoader loader, IfFlagLoader ifloader)
        {
            m_Loader = loader;
            m_IfLoader = ifloader;
            m_Ports = loader.GetCassettePortsInfo();
            m_Robot = loader.GetRobotInfo();

            for (int i = 0; i < m_Loader.PortCount; i++)
            {
                m_sItem = string.Format("PORT {0}", i + 1);
                cbPortNo.Items.Add(m_sItem);

                cbStageNo.Items.Add(m_sItem);

                oldPortGlasses.Add(new Glasses());
            }
            cbPortNo.SelectedIndex = 0;

            cbRobotCommand.Items.Add("Get Stand By Request");
            cbRobotCommand.Items.Add("Get Request");
            cbRobotCommand.Items.Add("Put Stand By Request");
            cbRobotCommand.Items.Add("Put Request");
            cbRobotCommand.Items.Add("Y Alignment Request");
            cbRobotCommand.SelectedIndex = 1;

            cbThickness.Items.Add("1.1mm");
            cbThickness.Items.Add("0.7mm");
            cbThickness.Items.Add("0.6mm");
            cbThickness.Items.Add("0.5mm");
            cbThickness.SelectedIndex = 0;

            cbRobotHand.Items.Add("Upper Hand");
            cbRobotHand.SelectedIndex = 0;

            for (int i = 1; i <= 39; i++)
            {
                cbSlotNo.Items.Add(i.ToString().PadLeft(2, '0'));
            }
            cbSlotNo.SelectedIndex = 0;

            if (m_Loader.LoaderType == LoaderType.Loader)
            {
                cbStageNo.Items.Add("Loading Stage");
            }
            else if (m_Loader.LoaderType == LoaderType.Unloader)
            {
                cbStageNo.Items.Add("Unloading Stage");
            }
            else
            {
                cbStageNo.Items.Add("Loading Stage");
                cbStageNo.Items.Add("Unloading Stage");
            }

            cbStageNo.SelectedIndex = 0;

            if( m_Loader != null ) InitGridView(m_Loader);

        }

        public void InitGridView(ILoader loader)
        {
            dataGridViewInfo.AutoGenerateColumns = false;
            dataGridViewInfo.Columns.Clear();

            dataGridViewInfo.ColumnCount = loader.PortCount+1;
            dataGridViewInfo.RowCount = 39;

            dataGridViewInfo.RowHeadersVisible = false;

            for (int i = 0; i < loader.PortCount+1; i++)
            {
                DataGridViewColumn column = dataGridViewInfo.Columns[i];

                if (i == 0)
                {
                    column.DataPropertyName = "SLOTNO";
                    column.Name = "SLOTNO";
                    column.HeaderText = "SLOT NO";
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    string portname = string.Format("PORT{0}", i);
                    column.DataPropertyName = portname;
                    column.Name = portname;
                    column.HeaderText = portname;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }

            for (int i = 0; i < 39; i++)
            {
                DataGridViewRow row = dataGridViewInfo.Rows[i];

                row.Cells[0].Value = i + 1;
                row.Cells[1].Value = m_Ports[0].Cassette.Stages[i].LoaderGlassStatus;
                row.Cells[2].Value = m_Ports[1].Cassette.Stages[i].LoaderGlassStatus;
                row.Cells[3].Value = m_Ports[2].Cassette.Stages[i].LoaderGlassStatus;
            }
        }

        public void UpdateGridView()
        {
            LoaderGlassStatus status = LoaderGlassStatus.glsEmpty;

            for (int i = 0; i < 39; i++)
            {
                DataGridViewRow row = dataGridViewInfo.Rows[i];

                for (int j = 0; j < m_Loader.PortCount; j++)
                {
                    status = m_Ports[j].Cassette.Stages[i].LoaderGlassStatus;
                    row.Cells[j + 1].Value = status;

                    oldPortGlasses[j].List[i].SetStatus(status);

                    SetStateCellBackColor(row.Cells[j+1], status);
                }
            }
        }

        private void SetStateCellBackColor(DataGridViewCell cell, LoaderGlassStatus status)
        {
            if (cell.Value != null)
            {
                Color etcColor = Color.WhiteSmoke;
                Color existColor = Color.Yellow;
                Color selectColor = Color.GreenYellow;
                Color waitColor = Color.SkyBlue;
                Color processColor = Color.Lime;
                Color EndColor = Color.DarkCyan;
                Color cancelColor = Color.LightPink;

                if (status == LoaderGlassStatus.glsExist)
                {
                    cell.Style.BackColor = existColor;
                }
                else if (status == LoaderGlassStatus.glsSelected)
                {
                    cell.Style.BackColor = selectColor;
                }
                else if (status == LoaderGlassStatus.glsWait)
                {
                    cell.Style.BackColor = waitColor;
                }
                else if (status == LoaderGlassStatus.glsProcess)
                {
                    cell.Style.BackColor = processColor;

                }
                else if (status == LoaderGlassStatus.glsProcessEnd)
                {
                    cell.Style.BackColor = EndColor;

                }
                else if (status == LoaderGlassStatus.glsAborted || status == LoaderGlassStatus.glsCanceled)
                {
                    cell.Style.BackColor = cancelColor;

                }
                else
                {
                    cell.Style.BackColor = etcColor;
                }
            }
        }



        private void btnAgv_Click(object sender, EventArgs e)
        {
            if (m_Loader.TrsMode == TrsMode.MGV)
            {
                m_IfLoader.TrsMode = TrsMode.AGV;
                m_IfLoader.TrsModeChangeReq = true;
            }
        }

        private void btnMgv_Click(object sender, EventArgs e)
        {
            if (m_Loader.TrsMode == TrsMode.AGV)
            {
                m_IfLoader.TrsMode = TrsMode.MGV;
                m_IfLoader.TrsModeChangeReq = true;
            }
        }

        private void btnPort1Enable_Click(object sender, EventArgs e)
        {
            CassettePort port = m_Loader.GetPortInfo(m_PortIndex);

            if (port.IsPortEnable() == false)
            {
                port.SetCommandPortCommand(PortCommand.PortEnable);
            }
        }

        private void btnPort1Disable_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            if (m_Port.IsPortEnable() == true)
            {
                m_Port.SetCommandPortCommand(PortCommand.PortDisable);
            }
        }

        private void btnMapping_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            m_Port.SetCommandPortCommand(PortCommand.MappingDataRequest);

        }

        private void btnCstId_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            m_Port.SetCommandPortCommand(PortCommand.CassetteIDRequest);
        }

        private void btnLoadRequest_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            m_Port.SetCommandPortCommand(PortCommand.LoadRequest);
        }

        private void btnUnloadRequest_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            m_Port.SetCommandPortCommand(PortCommand.UnloadRequest);
        }

        private void btnRechuck_Click(object sender, EventArgs e)
        {
            m_Port = m_Loader.GetPortInfo(m_PortIndex);

            m_Port.SetCommandPortCommand(PortCommand.RechuckRequest);
        }

        private void btnDateandTimeSet_Click(object sender, EventArgs e)
        {
            m_IfLoader.TimeSetReq = true;
        }

        private void btnRemoteChange_Click(object sender, EventArgs e)
        {
            m_IfLoader.RemoteReq = true;
        }

        private void btnBuzzerOff_Click(object sender, EventArgs e)
        {
            m_IfLoader.BuzzerOffReq = true;
        }

        private void btnAlarmReset_Click(object sender, EventArgs e)
        {
            m_IfLoader.AlarmResetReq = true;
        }

        private void btnOperatorCall_Click(object sender, EventArgs e)
        {
            m_IfLoader.OperatorCallReq = true;
        }

        private void cbPortNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_PortIndex = cbPortNo.SelectedIndex;
        }

        private void btnRobotHome_Click(object sender, EventArgs e)
        {
            m_IfLoader.RobotHomeReq = true;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (!m_Robot.isRobotMoving() && m_Robot.IsRobotReady())
            {
                m_IfLoader.RobotCommandAction = cbRobotCommand.SelectedIndex + 1;
                m_IfLoader.RobotCommandHand = cbRobotHand.SelectedIndex + 1;

                if (cbStageNo.SelectedItem.ToString().IndexOf("PORT") != -1)
                {
                    m_IfLoader.RobotCommandPortNo = cbStageNo.SelectedIndex + 1;
                }
                else if (cbStageNo.SelectedItem.ToString().IndexOf("Loading Stage") != -1)
                {
                    m_IfLoader.RobotCommandPortNo = 9;
                }
                else
                {
                    m_IfLoader.RobotCommandPortNo = 10;
                }

                if (m_IfLoader.RobotCommandPortNo != 9 &&
                    m_IfLoader.RobotCommandPortNo != 10)
                {
                    m_IfLoader.RobotCommnadSlotNo = cbSlotNo.SelectedIndex + 1;
                }
                else m_IfLoader.RobotCommnadSlotNo = 1;

                m_IfLoader.RobotCommnadThickness = cbThickness.SelectedIndex + 1;

                m_IfLoader.RobotCommandReq = true;
            }
            else
            {
                MessageBox.Show("Robot Command is not Order", "Robot Command");
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {

            if (m_Robot.IsHandGlassExist(LoaderRobotHand.upperHand))
            {
                if ( m_Robot.UpperHand.Glass.OriginalPortNo != OldRobotUpperHand.Glass.OriginalPortNo ||
                     m_Robot.UpperHand.Glass.OriginalSlotNo != OldRobotUpperHand.Glass.OriginalSlotNo)

                {
                    lblUpperHand.Text = m_Robot.UpperHand.Glass.OriginalPortNo.ToString() + "-" + m_Robot.UpperHand.Glass.OriginalSlotNo.ToString().PadLeft(2, '0');
                    lblUpperHand.BackColor = Color.Yellow;

                    OldRobotUpperHand = m_Robot.UpperHand.Clone();
                }
                else
                {
                    lblUpperHand.Text = "";
                    lblUpperHand.BackColor = Color.White;

                    OldRobotUpperHand = m_Robot.UpperHand.Clone();
                }
            }

            bool IsGlassStatus = false;

            for (int i = 0; i < m_Loader.PortCount; i++)
            {
                for (int j = 0; j < 39; j++)
                {
                    if (oldPortGlasses[i].List[j].GetStatus() != m_Ports[i].Cassette.Stages[j].GetGlassStatus() )
                    {
                        IsGlassStatus = true;
                        break;
                    }
                }
            }

            if (IsGlassStatus) UpdateGridView();
        }
    }
}


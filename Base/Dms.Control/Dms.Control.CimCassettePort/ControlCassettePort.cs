using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Loader;
using Dms.Common;
using Dms.Cim.Common;

namespace Dms.Control
{
    public partial class ControlCassettePort : UserControl
    {
        #region Fields
//        private CimManager m_CimManager = CimManager.Instance;
        private Cassette m_Cassette;
        private CassettePort m_Port;
        private BindingSource m_BindSrc = new BindingSource();
        private ILoader m_Loader;
        private HostInfo m_HostInfo;
//        private IfFlagLoader m_IfLoader;
        private int m_PortNo = 0;
        private int m_OldIsButtonState = -1;
        private bool m_IsModify = false;
        #endregion

        public ControlCassettePort()
        {
            InitializeComponent();
        }

        public ControlCassettePort(ILoader loader, CassettePort port, HostInfo info)
        {
            InitializeComponent();

            m_HostInfo = info;

            m_Loader = loader;
            m_Port = port;
            Initialize(m_Port);
        }

        public ControlCassettePort(ILoader loader, int portNo, HostInfo info)
        {
            InitializeComponent();

            m_HostInfo = info;

            m_PortNo = portNo;
            m_Loader = loader;
            m_Port = m_Loader.GetPortInfo(portNo);
            Initialize(m_Port);
        }

        public void Initialize(CassettePort port)
        {
            InitData(port);
            InitGridView(port);

            if( m_Loader.LoaderType == LoaderType.Unloader )
            {
                buttonInputID.Visible = false;
                buttonInputID.Enabled = false;
            }

//            m_IfLoader = m_CimManager.RootNode.IfLoader;

            tmrUpdate.Enabled = true;
        }

        public void SetModify(bool enable)
        {
            m_IsModify = enable;
        }

        public void InitData(CassettePort port)
        {
            textCassetteID.Text = port.Cassette.GetCassetteID();
            textRecipeID.Text = port.Cassette.GetRecipeID();
            textLotID.Text = port.Cassette.GetLotID();

            if (port.Cassette.GetCstType() == CassetteType.A) textCassetteType.Text = "Slot(30) without Back Support";
            else if (port.Cassette.GetCstType() == CassetteType.B) textCassetteType.Text = "Slot(30) with Back Support";
            else if (port.Cassette.GetCstType() == CassetteType.C) textCassetteType.Text = "Slot(35) with Back Support";
            else if (port.Cassette.GetCstType() == CassetteType.G) textCassetteType.Text = "Slot(39)";
        }

        public void InitGridView(CassettePort port)
        {
            m_Cassette = port.Cassette;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            m_BindSrc.DataSource = m_Cassette.Stages;
            dataGridView1.DataSource = m_BindSrc;

//            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.DataPropertyName = "SlotNo";
            colNo.Name = "SlotNo";
            colNo.HeaderText = "Slot";
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns.Add(colNo);

            DataGridViewCheckBoxColumn colExist = new DataGridViewCheckBoxColumn();
            colExist.DataPropertyName = "GlassExist";
            colExist.Name = "GlassExist";
            colExist.HeaderText = "Glass";
            colExist.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns.Add(colExist);

            DataGridViewCheckBoxColumn colSelect = new DataGridViewCheckBoxColumn();
            colSelect.DataPropertyName = "GlassSelected";
            colSelect.Name = "GlassSelected";
            colSelect.HeaderText = "Select";
            colSelect.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns.Add(colSelect);

            DataGridViewTextBoxColumn colSheetID = new DataGridViewTextBoxColumn();
            colSheetID.DataPropertyName = "SheetID";
            colSheetID.Name = "SheetID";
            colSheetID.HeaderText = "Sheet ID";
            colSheetID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns.Add(colSheetID);

            DataGridViewTextBoxColumn colRecipeID = new DataGridViewTextBoxColumn();
            colRecipeID.DataPropertyName = "RecipeID";
            colRecipeID.Name = "RecipeID";
            colRecipeID.HeaderText = "Recipe ID";
            colRecipeID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns.Add(colRecipeID);

            DataGridViewTextBoxColumn colGlassStatus = new DataGridViewTextBoxColumn();
            colGlassStatus.DataPropertyName = "LoaderGlassStatus";
            colGlassStatus.Name = "LoaderGlassStatus";
            colGlassStatus.HeaderText = "Glass Status";
            colGlassStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns.Add(colGlassStatus);

            DataGridViewTextBoxColumn colThickness = new DataGridViewTextBoxColumn();
            colThickness.DataPropertyName = "Thickness";
            colThickness.Name = "Thickness";
            colThickness.HeaderText = "Thickness";
            colThickness.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns.Add(colThickness);

            this.dataGridView1.ReadOnly = true;

            SelectDisplay(port);

//          dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        public void SelectDisplay(CassettePort port)
        {
            if (port.CurPortStatus == PortStatus.LoadComplete)
            {
                int slotcount = port.Cassette.SlotCount;

                for (int i = 0; i < DefineConstants.MaxSlotNo; i++)
                {
                    if (i >= slotcount) dataGridView1.Rows[i].Visible = false;
                }
            }

/*
            int rowindex = 0;

            foreach (Stage stage in port.Cassette.Stages)
            {
                if (stage.GlassExist == false)
                {
                    DataGridViewRow row = dataGridView1.Rows[rowindex];

                    row.Visible = false;
                }

                rowindex += 1;
            }   
*/ 
        }

        private void buttonManualStart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to start the process ?", "Process Start", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (m_HostInfo.Mode == hostMODE.hostOffline)
                {
                    if (m_Loader != null && m_Port.LotStatus != LotStatus.Wait)
                    {
                        m_Loader.SetLotCommand(m_Cassette.PortNo, LotCommand.Start);

                        AddLotInfo(m_Port);
                    }
                }
                else
                {
                    LotInfo Lot = m_Loader.LotInfos.GetLotInfo( m_Port.LotNo );

                    if (m_HostInfo.Mode == hostMODE.hostOnlineMonitor)
                    {
//                        m_CimManager.DataQueue.SetHostCommand("S7F101", m_Port.LotNo);
                    }

                    if( Lot != null )
                    {
                        if (m_HostInfo.Mode == hostMODE.hostOnlineControl && Lot.RCode == "Y")
                        {
//                            m_CimManager.DataQueue.SetHostCommand("S7F101", m_Port.LotNo);
                        }
                    }
                }

//                m_CimManager.SetCommandLog(Command.ProcessStartManual, "PORT NO", m_PortNo + 1, LotCommand.Start);
            }
        }

        private void buttonAbortCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Abort/Cancel the process ?", "Process Abort/Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (m_Loader != null )
                {
                    m_Loader.SetLotCommand(m_Cassette.PortNo, LotCommand.Cancel);
                }

//                m_CimManager.SetCommandLog(Command.AbortCancelManual, "PORT NO", m_PortNo + 1, LotCommand.Cancel);
            }
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int slotCount = m_Cassette.Stages.Count;
            if (this.checkSelectAll.Checked)
            {
                for (int i = 0; i < slotCount; i++)
                {
                    if (m_Cassette.Stages[i].IsGlassExist())
                    {
                        m_Cassette.Stages[i].GlassSelected = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < slotCount; i++)
                {
                    if (m_Cassette.Stages[i].IsGlassExist())
                    {
                        m_Cassette.Stages[i].GlassSelected = false;
                    }
                }
            }
            dataGridView1.Invalidate();
        }

        private void ControlCassettePort_Load(object sender, EventArgs e)
        {

        }

        private void buttonInputID_Click(object sender, EventArgs e)
        {
//            m_CimManager.SetCommandLog(Command.InputIDManual, "PORTNO", m_PortNo + 1, "Input ID Button is Click");

            DlgInputID dlg = new DlgInputID();

            dlg.Initialize(m_Port.Clone());

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int count = m_Cassette.Stages.Count;

                Cassette cassette = dlg.Cassette;

                m_Cassette.SetLotID(cassette.GetLotID());
                m_Cassette.SetCstType(cassette.GetCstType());
                m_Cassette.SetCassetteID(cassette.GetCassetteID());

                for (int i = 0; i < count; i++)
                {
                    Stage stage = cassette.Stages[i];

                    m_Cassette.Stages[i].Thickness = stage.Thickness;
                    m_Cassette.Stages[i].RecipeID = stage.RecipeID;
                    m_Cassette.Stages[i].SheetID = stage.SheetID;
                    m_Cassette.Stages[i].Glass.LotID = cassette.GetLotID();
                    m_Cassette.Stages[i].GlassSelected = stage.GlassSelected;

                    m_Cassette.Stages[i].Glass.DMSRecipe = stage.Glass.DMSRecipe;
                    m_Cassette.Stages[i].Glass.AOIRecipe = stage.Glass.AOIRecipe;
                    m_Cassette.Stages[i].Glass.EXPRecipe = stage.Glass.EXPRecipe;

                    m_Cassette.Stages[i].PortNo = m_PortNo + 1;
                    m_Cassette.PortNo = m_PortNo + 1;

                    if (stage.GlassSelected)
                    {
                        m_Cassette.Stages[i].SetGlassStatus(LoaderGlassStatus.glsSelected);
                    }
                    else
                    {
                        if (stage.GlassExist)
                        {
                            m_Cassette.Stages[i].SetGlassStatus(LoaderGlassStatus.glsExist);
                        }
                        else
                        {
                            m_Cassette.Stages[i].SetGlassStatus(LoaderGlassStatus.glsEmpty);
                        }
                    }
                }

                LotInfo Lot = m_Loader.LotInfos.GetLotInfo(m_Port.LotNo );

                if (Lot != null)
                {
                    int lotslotcount = Lot.SlotInfos.Count;

                    if (m_HostInfo.Mode == hostMODE.hostOnlineMonitor ||
                        (m_HostInfo.Mode == hostMODE.hostOnlineControl && Lot.RCode == "Y"))
                    {
                        for (int i = 0; i < lotslotcount; i++)
                        {
                            Stage stage = m_Cassette.Stages[i];

                            Lot.SlotInfos[i].GlassRecipeId = stage.RecipeID;

                            if (stage.GlassSelected)
                            {
                                Lot.ProcessFlagMap[i] = true;
                            }
                            else
                            {
                                Lot.ProcessFlagMap[i] = false;
                            }
                        }
                    }
                }

                m_Port = m_Loader.GetPortInfo(m_PortNo);
                InitGridView(m_Port);
                InitData(m_Port);

//                m_CimManager.SetCommandLog(Command.InputIDManual, "PORTNO", m_PortNo + 1, "Input ID Dialog is OK");
            }
            else
            {
//                m_CimManager.SetCommandLog(Command.InputIDManual, "PORTNO", m_PortNo + 1, "Input ID Dialog is Exit");
            }
        }

        private void SetButton(bool enable)
        {
            if ((m_OldIsButtonState != 1) && (enable == true))
            {
                m_OldIsButtonState = 1;

                buttonManualStart.Enabled = true;
                buttonAbortCancel.Enabled = true;

                if (m_Loader.LoaderType == LoaderType.Loader) buttonInputID.Enabled = true;
            }
            else if ((m_OldIsButtonState != 0) && (enable == false))
            {
                m_OldIsButtonState = 0;

                buttonManualStart.Enabled = false;
                buttonAbortCancel.Enabled = false;
                buttonInputID.Enabled = false;
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (m_HostInfo.Mode != hostMODE.hostOnlineControl)
            {
                if (m_Port.CurPortStatus == PortStatus.LoadComplete)
                {
                    SetButton(true);
                }
                else
                {
                    SetButton(false);
                }
            }
            else 
            {
                if (m_IsModify && m_Port.CurPortStatus == PortStatus.LoadComplete )
                {
                    SetButton(true);
                }
                else
                {
                    SetButton(false);
                }
            }
        }

        private void AddLotInfo(CassettePort m_Port)
        {
            LotInfo info = new LotInfo();

            info.PortNo = m_Port.PortNo;
            info.CSTID = m_Port.Cassette.GetCassetteID();
            info.LOTID = m_Port.Cassette.GetLotID();
//          info.OPID = m_Port
            info.RecipeId = m_Port.Cassette.GetRecipeID();
            info.CSTTYPE = m_Port.Cassette.GetCstType().ToString();

            LotSlotInfos lotslotinfos = new LotSlotInfos();

            for (int i = 0; i < DefineConstants.MaxSlotNo; i++)
            {
                LotSlotInfo slotinfo = new LotSlotInfo();
                
                slotinfo.SlotId = Convert.ToString(i + 1).PadLeft(2,'0');
                slotinfo.GlassId = m_Port.Cassette.Stages[i].Glass.GlassID;

                if (m_Port.Cassette.Stages[i].GlassSelected)
                {
                    info.ProcessFlagMap[i] = true;
                    slotinfo.ProcessFlag = "1";
                }
                else
                {
                    info.ProcessFlagMap[i] = false;
                    slotinfo.ProcessFlag = "0";
                }

                slotinfo.GlassRecipeId = m_Port.Cassette.Stages[i].RecipeID;
                slotinfo.Thickness = m_Port.Cassette.Stages[i].Glass.Thickness;

                lotslotinfos.Add(slotinfo);
            }

            info.SlotInfos = lotslotinfos;

            short nLotNo = m_Loader.LotInfos.GetMaxLotNo();
            info.LotNo = nLotNo;

            m_Port.LotNo = info.LotNo;

//          info.WriteXml();
            m_Loader.LotInfos.Add(info);

            int count = m_Cassette.Stages.Count;

            for (int i = 0; i < count; i++)
            {
                m_Cassette.Stages[i].Glass.LotNo = info.LotNo;
                m_Cassette.Stages[i].Glass.OriginalPortNo = info.PortNo;
                m_Cassette.Stages[i].Glass.OriginalSlotNo = i + 1;
            }
        }
    }
}
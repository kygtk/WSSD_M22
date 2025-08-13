using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public partial class ViewIOConfig : UserControl
    {
        #region Enum
        enum ObjLevel
        {
            Top,
            Node,
            Terminal,
            Channel
        }
        enum ContextStripId
        {
            Add,
            Del,
            Separator1,
            Load,
            LoadAll,
            LoadFrom,
            Separator2,
            Update,
            UpdateAll,
            Separator3,
            Save,
            SaveAll,
            SaveAs,
            Separator4,
            EditMelsecDevice,
            Separator5
        }
        #endregion

        #region Fields
        private IoDefines m_SelectedDefine;
        private List<IoDefines> m_IoDefineList = new List<IoDefines>();
        private int m_SelectedIndex = 0;
        private TreeNode m_CurNode;
        private bool m_Initialized;
        private DesignerMode m_DesignerMode = DesignerMode.Design;
        private MelsecDeviceInfos m_MelsecDevices = new MelsecDeviceInfos();
        #endregion

        #region Properties
        public DesignerMode DesignerMode
        {
            get { return m_DesignerMode; }
            set
            {
                m_DesignerMode = value;
                this.propertyGrid1.Enabled = m_DesignerMode == DesignerMode.Design;
            }
        }
        public bool EnableBomMaker
        {
            get { return this.buttonMakeBom.Visible; }
            set { this.buttonMakeBom.Visible = value; }
        }
        public IoDefines SelectedDefines
        {
            get { return m_SelectedDefine; }
        }
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        public ViewIOConfig()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (System.Reflection.Assembly.GetEntryAssembly() != null)
                m_IoDefineList = IoDefines.CreateIoDefineList();
        }

        public void Initialize(IoDefines pool, DesignerMode mode)
        {
            m_DesignerMode = mode;

            Initialize(pool);
        }

        public void Initialize(IoDefines pool)
        {
            if (!m_Initialized)
            {
                m_SelectedIndex = (int)pool.BusType;
                m_IoDefineList[m_SelectedIndex] = pool;
            }

            m_SelectedDefine = pool;

            UpdateFileName();

            // Update Module Id
            m_SelectedDefine.UpdateModuleIndex();

            // Update Wiring Number
            m_SelectedDefine.UpdateWiringNo();

            // Update tree view
            UpdateTreeView(m_SelectedDefine);

            #region no use old version
            //Cursor.Current = Cursors.WaitCursor;
            //treeView1.BeginUpdate();
            //treeView1.Nodes.Clear();

            //TreeNode node1 = new TreeNode();
            //node1.Tag = m_SelectedDefine;
            //node1.Text = node1.Tag.ToString();

            //int diCount = 0;
            //int doCount = 0;
            //int aiCount = 0;
            //int aoCount = 0;

            //int nodeCount = m_SelectedDefine.Container.Count;
            //for (int nodeid = 0; nodeid < nodeCount; nodeid++)
            //{
            //    TreeNode node2 = new TreeNode();
            //    IoNode ioNode = m_SelectedDefine.Container[nodeid];

            //    if (m_Mode == ViewIOEdit.OpMode.Config)
            //    {
            //        ioNode.MakeNodeModule(m_SelectedDefine.BusType);
            //        ioNode.Id = nodeid;
            //    }

            //    node2.Tag = ioNode;
            //    node2.Text = ioNode.ToString();

            //    int diTerminalCount = 0;
            //    int doTerminalCount = 0;
            //    int aiTerminalCount = 0;
            //    int aoTerminalCount = 0;

            //    int diCountByNode = 0;
            //    int doCountByNode = 0;
            //    int aiCountByNode = 0;
            //    int aoCountByNode = 0;

            //    int terminalCount = ioNode.Terminals.Count;
            //    for (int terminalid = 0; terminalid < terminalCount; terminalid++)
            //    {
            //        TreeNode node3 = new TreeNode();
            //        IoTerminal terminal = ioNode.Terminals[terminalid];

            //        int ioTypeId = 0;
            //        if (terminal.IoType == IoType.DI) ioTypeId = diTerminalCount++;
            //        else if (terminal.IoType == IoType.DO) ioTypeId = doTerminalCount++;
            //        else if (terminal.IoType == IoType.AI) ioTypeId = aiTerminalCount++;
            //        else if (terminal.IoType == IoType.AO) ioTypeId = aoTerminalCount++;

            //        if (m_Mode == ViewIOEdit.OpMode.Config)
            //        {
            //            terminal.Id = terminalid;
            //            terminal.IdByIoType = ioTypeId;
            //        }

            //        node3.Tag = terminal;
            //        node3.Text = terminal.Id + " : " + terminal.ToString() + "." + terminal.IdByIoType;

            //        int channelCount = terminal.ChannelCount;
            //        for (int channelid = 0; channelid < channelCount; channelid++)
            //        {
            //            int ioId = 0;
            //            int ioIdByNode = 0;
            //            if (terminal.IoType == IoType.DI)
            //            {
            //                ioId = diCount++;
            //                ioIdByNode = diCountByNode++;
            //            }
            //            else if (terminal.IoType == IoType.DO)
            //            {
            //                ioId = doCount++;
            //                ioIdByNode = doCountByNode++;
            //            }
            //            else if (terminal.IoType == IoType.AI)
            //            {
            //                ioId = aiCount++;
            //                ioIdByNode = aiCountByNode++;
            //            }
            //            else if (terminal.IoType == IoType.AO)
            //            {
            //                ioId = aoCount++;
            //                ioIdByNode = aoCountByNode++;
            //            }

            //            TreeNode node4 = new TreeNode();
            //            IoItem item = terminal.Channels[channelid];
            //            if (m_Mode == ViewIOEdit.OpMode.Config)
            //            {
            //                item.Id = ioId;
            //                item.Node = nodeid;
            //                item.Terminal = ioTypeId;
            //                item.Channel = channelid;
            //                item.IoType = terminal.IoType;
            //            }

            //            node4.Tag = item;
            //            node4.Text = item.Id + " : " + item.Name;

            //            node3.Nodes.Add(node4);
            //        }
            //        //node3.Expand();
            //        node2.Nodes.Add(node3);
            //    }

            //    ioNode.UpdateIoCountByType();

            //    node2.Expand();
            //    node1.Nodes.Add(node2);
            //    node1.Expand();
            //}
            //treeView1.Nodes.Add(node1);
            //treeView1.EndUpdate(); 
            #endregion

            //m_Pool.InitializeIOCollection();

            m_Initialized = true;
        }

        private void UpdateFileName()
        {
            this.textBoxFileName.Text = "";
            this.textBoxFileName.AppendText(m_SelectedDefine.FileName);
        }

        private void UpdateTreeView(IoDefines ioDefine)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeView1.BeginUpdate();
            object topnodetag = (treeView1.TopNode != null) ? treeView1.TopNode.Tag : null;
            treeView1.Nodes.Clear();
            var scrolloffset = treeView1.AutoScrollOffset;
            TreeNode node1 = new TreeNode();
            node1.Tag = ioDefine;
            node1.Text = node1.Tag.ToString();

            // Update Node tree
            int nodeCount = ioDefine.Container.Count;
            for (int nodeid = 0; nodeid < nodeCount; nodeid++)
            {
                IoNode ioNode = ioDefine.Container[nodeid];

                TreeNode node2 = new TreeNode();
                node2.Tag = ioNode;
                node2.Text = ioNode.ToString();

                // Update Terminal Tree
                int terminalCount = ioNode.Terminals.Count;
                for (int terminalid = 0; terminalid < terminalCount; terminalid++)
                {
                    IoTerminal terminal = ioNode.Terminals[terminalid];

                    TreeNode node3 = new TreeNode();
                    node3.Tag = terminal;
                    node3.Text = terminal.Id + " : " + terminal.ToString() + "." + terminal.IdByIoType;

                    // Update Channel Tree
                    int channelCount = terminal.ChannelCount;
                    for (int channelid = 0; channelid < channelCount; channelid++)
                    {
                        IoItem item = terminal.Channels[channelid];

                        TreeNode node4 = new TreeNode();
                        node4.Tag = item;
                        node4.Text = channelid + " : " + item.Name;

                        node3.Nodes.Add(node4);
                    }

                    node2.Nodes.Add(node3);
                }

                // Update Slave Tree
                int slaveCount = ioNode.Slaves.Count;
                for (int slaveid = 0; slaveid < slaveCount; slaveid++)
                {
                    EcSlave slave = ioNode.Slaves[slaveid];

                    TreeNode node3 = new TreeNode();
                    node3.Tag = slave;
                    node3.Text = slaveid + " : " + slave.ToString() + "." + slave.AliasNo;

                    // Update Channel Tree
                    int channelCount;
                    switch (slave.SlaveType)
                    {
                        case SlaveType.Servo:
                            {
                                channelCount = ((EcSlave_Servo)slave).ServoCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_Servo)slave).Servos[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.BLDC:
                            {
                                channelCount = ((EcSlave_BLDC)slave).BLDCCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_BLDC)slave).BLDCs[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.Inverter:
                            {
                                channelCount = ((EcSlave_Inverter)slave).InverterCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_Inverter)slave).Inverters[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.DI:
                            {
                                channelCount = ((EcSlave_DI)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DI)slave).InChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.DIO:
                            {
                                channelCount = ((EcSlave_DIO)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DIO)slave).InChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }

                                channelCount = ((EcSlave_DIO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DIO)slave).OutChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.DO:
                            {
                                channelCount = ((EcSlave_DO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_DO)slave).OutChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.AI:
                            {
                                channelCount = ((EcSlave_AI)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AI)slave).InChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.AIO:
                            {
                                channelCount = ((EcSlave_AIO)slave).InChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AIO)slave).InChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }

                                channelCount = ((EcSlave_AIO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AIO)slave).OutChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.AO:
                            {
                                channelCount = ((EcSlave_AO)slave).OutChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem item = ((EcSlave_AO)slave).OutChannels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        case SlaveType.AP:
                            {
                                channelCount = ((EcSlave_AP)slave).ChannelCount;
                                for (int channelid = 0; channelid < channelCount; channelid++)
                                {
                                    EcSlaveItem_AP item = ((EcSlave_AP)slave).Channels[channelid];

                                    TreeNode node4 = new TreeNode();
                                    node4.Tag = item;
                                    node4.Text = channelid + " : " + item.Name;

                                    //TreeNode node5 =new TreeNode();
                                    //node5.Tag = item.Item;
                                    //node5.Text = item.Item.ToString();

                                    //node4.Nodes.Add(node5);
                                    node3.Nodes.Add(node4);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    node2.Nodes.Add(node3);
                }

                node2.Expand();
                node1.Nodes.Add(node2);
                node1.Expand();
            }

            treeView1.Nodes.Add(node1);
            treeView1.EndUpdate();

            TreeNode topnode = FindNode(treeView1, topnodetag);
            if (topnode != null) treeView1.TopNode = topnode;

            Cursor.Current = Cursors.Default;
        }

        private TreeNode FindNode(TreeView treeview, object tag)
        {
            for (int i = 0; i < treeview.Nodes.Count; i++)
            {
                TreeNode node = treeview.Nodes[i];

                if (node.Tag == tag) return node;
                else
                {
                    TreeNode subnode = FindNode(node, tag);
                    if (subnode != null) return subnode;
                }
            }

            return null;
        }

        private TreeNode FindNode(TreeNode treenode, object tag)
        {
            for (int i = 0; i < treenode.Nodes.Count; i++)
            {
                TreeNode node = treenode.Nodes[i];

                if (node.Tag == tag) return node;
                else
                {
                    TreeNode subnode = FindNode(node, tag);
                    if (subnode != null) return subnode;
                }
            }

            return null;
        }

        private void ViewIOConfig_Load(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Items[(int)ContextStripId.Add].Enabled = false;
            this.contextMenuStrip1.Items[(int)ContextStripId.Del].Enabled = false;

            MakeBusTypeCategory();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_CurNode = e.Node;
            //this.propertyGrid1.SelectedObject = m_CurNode.Tag;
            MouseButtons button = e.Button;

            if (button == MouseButtons.Right)
            {
                if (m_DesignerMode != DesignerMode.Design)
                {
                    m_CurNode.ContextMenuStrip = null;
                }
                else
                {
                    if (m_CurNode.ContextMenuStrip == null)
                    {
                        m_CurNode.ContextMenuStrip = this.contextMenuStrip1;
                    }

                    if (m_CurNode.Level == (int)ObjLevel.Top)
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = true;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.EditMelsecDevice].Enabled = false;
                    }
                    else if (m_CurNode.Level == (int)ObjLevel.Node)
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled =
                            m_SelectedDefine.BusType != FieldBusType.MitsubishiMelsecEtherNet;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = true;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.EditMelsecDevice].Enabled = false;
                        //m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.EditMelsecDevice].Enabled = 
                        //    m_SelectedDefine.BusType == FieldBusType.MitsubishiMelsecNet &&
                        //    ((IoNodeMelsec)m_CurNode.Tag).ChannelNo == channel.melsecEthernet;
                    }
                    else if (m_CurNode.Level == (int)ObjLevel.Terminal)
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = true;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.EditMelsecDevice].Enabled = false;
                    }
                    else
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.EditMelsecDevice].Enabled = false;
                    }
                }
            }
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {

        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            if (m_CurNode.Level == (int)ObjLevel.Top)
            {
                // Movensys EtherCAT Master는 한개까지만 생성 가능 
                if (m_SelectedDefine.BusType == FieldBusType.MovensysEtherCAT && m_SelectedDefine.Count >= 1)
                {
                    MessageBox.Show("No more Ethercat masters can be created");
                    return;
                }

                // Add Node
                FormNodeAdd dlgAdds = new FormNodeAdd();
                dlgAdds.ShowDialog();
                if (dlgAdds.DialogResult != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    int count = dlgAdds.Count;

                    // Movensys EtherCAT Master는 한개까지만 생성 가능 
                    if (m_SelectedDefine.BusType == FieldBusType.MovensysEtherCAT && count != 1)
                    {
                        MessageBox.Show("Only one EtherCAT master can be created.");
                        return;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        m_SelectedDefine.Container.Add(IoNode.CreateNewNode(m_SelectedDefine.BusType));
                    }

                    Initialize(m_SelectedDefine);
                }
            }
            else if (m_CurNode.Level == (int)ObjLevel.Node)
            {
                if (m_SelectedDefine.BusType == FieldBusType.MovensysEtherCAT)
                {
                    // Add Slave
                    FormSlaveAdd form = new FormSlaveAdd();
                    form.BusType = m_SelectedDefine.BusType;
                    form.ShowDialog();

                    if (form.DialogResult == DialogResult.OK)
                    {
                        int nodeId = m_CurNode.Index;
                        foreach (EcSlave item in form.NewSlaves)
                        {
                            m_SelectedDefine.Container[nodeId].Slaves.Add(item);
                        }
                    }
                }
                else
                {
                    // Add Termninal
                    FormIOAdd form = new FormIOAdd();
                    form.BusType = m_SelectedDefine.BusType;
                    form.ShowDialog();

                    if (form.DialogResult == DialogResult.OK)
                    {
                        int nodeId = m_CurNode.Index;
                        foreach (IoTerminal item in form.NewTerminals)
                        {
                            m_SelectedDefine.Container[nodeId].Terminals.Add(item);
                        }
                    }
                }

                Initialize(m_SelectedDefine);
            }
        }

        private void propertyGrid1_Leave(object sender, EventArgs e)
        {
            if (m_Initialized)
            {
                Initialize(m_SelectedDefine);
            }
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_SelectedDefine.WriteXml();

                UpdateFileName();
            }

        }

        private void toolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            m_SelectedDefine.ReadXml();
            Initialize(m_SelectedDefine);
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = m_SelectedDefine.GetDefaultFileName() + ".xml";
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    m_SelectedDefine.WriteXml(dlg.FileName);

                    UpdateFileName();
                }
                catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void loadFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Read From XML file";
            dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    m_SelectedDefine.ReadXml(dlg.FileName);
                    Initialize(m_SelectedDefine);
                }
                catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            if (m_CurNode.Level == (int)ObjLevel.Node)
            {
                int nodeId = m_CurNode.Index;
                m_SelectedDefine.Container.Remove(m_CurNode.Tag as IoNode);
                Initialize(m_SelectedDefine);
            }
            else if (m_CurNode.Level == (int)ObjLevel.Terminal)
            {
                int nodeId = m_CurNode.Parent.Index;
                if (m_CurNode.Tag as IoTerminal != null)
                    m_SelectedDefine.Container[nodeId].Terminals.Remove(m_CurNode.Tag as IoTerminal);
                if (m_CurNode.Tag as EcSlave != null)
                    m_SelectedDefine.Container[nodeId].Slaves.Remove(m_CurNode.Tag as EcSlave);
                Initialize(m_SelectedDefine);
            }

        }

        private void buttonMakeBom_Click(object sender, EventArgs e)
        {
            FormBomAnalysis form = new FormBomAnalysis();
            form.Initialize(m_SelectedDefine);
            form.ShowDialog();
        }

        private void MakeBusTypeCategory()
        {
            //jemoon : Make combo item list
            this.comboBoxFieldBusType.DataSource = Enum.GetValues(typeof(FieldBusType));
            this.comboBoxFieldBusType.SelectedIndex = m_SelectedIndex;

            //초기화 단계에서 select change event 발생 방지
            this.comboBoxFieldBusType.SelectedIndexChanged += new System.EventHandler(this.comboBoxFieldBusType_SelectedIndexChanged);
        }

        private void comboBoxFieldBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.comboBoxFieldBusType.SelectedIndex;
            m_SelectedIndex = index;
            m_SelectedDefine = m_IoDefineList[m_SelectedIndex];
            this.propertyGrid1.SelectedObject = m_SelectedDefine;

            Initialize(m_SelectedDefine);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save All IoConfiguraions ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (IoDefines ioDefines in m_IoDefineList)
                {
                    ioDefines.WriteXml();
                }

                Initialize(m_SelectedDefine);
            }
        }

        private void loadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Load All IoConfiguraions ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (IoDefines ioDefines in m_IoDefineList)
                {
                    ioDefines.ReadXml();
                }

                Initialize(m_SelectedDefine);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize(m_SelectedDefine);
        }

        private void updateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IoDefines ioDefines in m_IoDefineList)
            {
                Initialize(ioDefines);
            }

            //순서에따라 update하고 나면 최초 선택되어져 있던게 바뀌어 버리므로
            //선택 되어져 있던 define으로 다시 돌려놓기
            m_SelectedDefine = m_IoDefineList[m_SelectedIndex];
            Initialize(m_SelectedDefine);
        }

        // Only for MelsecNet etherNet mode
        private void editMelsecDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMelsecDeviceEditor form = new FormMelsecDeviceEditor();

            IoNode node = (m_CurNode.Tag as IoNode);
            m_MelsecDevices = MelsecDeviceInfos.GetMelsecDeviceInfos(node);
            form.Initialize(m_MelsecDevices);
            if (form.ShowDialog() == DialogResult.OK)
            {
                node.Terminals.Clear();

                node.Terminals = MelsecDeviceInfos.MakeTerminals(m_MelsecDevices);

                Initialize(m_SelectedDefine);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_CurNode = e.Node;
            this.propertyGrid1.SelectedObject = m_CurNode.Tag;
        }
    }
}

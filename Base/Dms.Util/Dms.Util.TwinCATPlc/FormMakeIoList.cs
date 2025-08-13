using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Ctl;
using Dms.Util.IODefine;
using TwinCAT.Ads;
using SYSSERVLib;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.IO;
using Dms.Device;

namespace Dms.Util
{
    public partial class FormMakeIoList : Form
    {
        #region Fields
        private string m_AmsNetId;
        private int m_Port;
        private int m_InOffset;
        private int m_OutOffset;
        private int m_DiOffset;
        private int m_DoOffset;
        private int m_AiOffset;
        private int m_AoOffset;
        private bool m_IsSaveIo;
        private int m_InSize;
        private int m_OutSize;
        private int m_DiSize;
        private int m_DoSize;
        private int m_AiSize;
        private int m_AoSize;
        private Dictionary<IoType, string> m_DicName = new Dictionary<IoType, string>();
        private Dictionary<VarType, int> m_DicOffset = new Dictionary<VarType, int>();
        private TcAdsClient m_Client;

        private IoNodeTwinCATPLC m_InNode = new IoNodeTwinCATPLC();
        private IoNodeTwinCATPLC m_OutNode = new IoNodeTwinCATPLC();
        private IoNodeTwinCATPLC m_DiNode = new IoNodeTwinCATPLC();
        private IoNodeTwinCATPLC m_DoNode = new IoNodeTwinCATPLC();
        private IoNodeTwinCATPLC m_AiNode = new IoNodeTwinCATPLC();
        private IoNodeTwinCATPLC m_AoNode = new IoNodeTwinCATPLC();
        private int m_InTerminalCount;
        private int m_OutTerminalCount;
        private int m_DiTerminalCount;
        private int m_DoTerminalCount;
        private int m_AiTerminalCount;
        private int m_AoTerminalCount;
        private int[] m_DiNodes;
        private int[] m_DoNodes;
        private int[] m_AiNodes;
        private int[] m_AoNodes;
        #endregion

        #region Constructor
        public FormMakeIoList()
        {
            InitializeComponent();

            if (m_DicName.Count == 0)
            {
                m_DicName.Add(IoType.DI, "di");
                m_DicName.Add(IoType.DO, "do");
                m_DicName.Add(IoType.AI, "ai");
                m_DicName.Add(IoType.AO, "ao");
            }

            m_InNode.Id = (int)VarType.In;
            m_OutNode.Id = (int)VarType.Out;
            m_DiNode.Id = (int)VarType.Di;
            m_DoNode.Id = (int)VarType.Do;
            m_AiNode.Id = (int)VarType.Ai;
            m_AoNode.Id = (int)VarType.Ao;
        }
        #endregion

        #region Methods
        private void FormMakeIoList_Load(object sender, EventArgs e)
        {
            IoDefines io = new IoDefines(FieldBusType.TwinCATPlc);
            if (io.ReadXml())
            {
                foreach (IoNodeTwinCATPLC node in io.Container)
                {
                    this.txtAddress.Text = node.AmsNetId;
                    this.txtPort.Text = node.PortNo.ToString();
                    switch (node.Id)
                    {
                        case (int)VarType.In:
                            {
                                this.txtInOffset.Text = node.Offset.ToString();
                                this.txtInSize.Text = node.Size.ToString();
                            }
                            break;
                        case (int)VarType.Out:
                            {
                                this.txtOutOffset.Text = node.Offset.ToString();
                                this.txtOutSize.Text = node.Size.ToString();
                            }
                            break;
                        case (int)VarType.Di:
                            {
                                this.txtDIOffset.Text = node.Offset.ToString();
                                this.txtDiSize.Text = node.Size.ToString();
                                this.txtDiNodeStartNo.Text = node.NodeStartNo;
                            }
                            break;
                        case (int)VarType.Do:
                            {
                                this.txtDOOffset.Text = node.Offset.ToString();
                                this.txtDoSize.Text = node.Size.ToString();
                                this.txtDoNodeStartNo.Text = node.NodeStartNo;
                            }
                            break;
                        case (int)VarType.Ai:
                            {
                                this.txtAIOffset.Text = node.Offset.ToString();
                                this.txtAiSize.Text = node.Size.ToString();
                                this.txtAiNodeStartNo.Text = node.NodeStartNo;
                            }
                            break;
                        case (int)VarType.Ao:
                            {
                                this.txtAOOffset.Text = node.Offset.ToString();
                                this.txtAoSize.Text = node.Size.ToString();
                                this.txtAoNodeStartNo.Text = node.NodeStartNo;
                            }
                            break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Control to Variable
                m_AmsNetId = this.txtAddress.Text;
                m_Port = Convert.ToInt32(this.txtPort.Text);
                m_InOffset = Convert.ToInt32(this.txtInOffset.Text);
                m_OutOffset = Convert.ToInt32(this.txtOutOffset.Text);
                m_DiOffset = Convert.ToInt32(this.txtDIOffset.Text) * 8;    //DI, DO는 Offset값이 현재값의 *8
                m_DoOffset = Convert.ToInt32(this.txtDOOffset.Text) * 8;
                m_AiOffset = Convert.ToInt32(this.txtAIOffset.Text);
                m_AoOffset = Convert.ToInt32(this.txtAOOffset.Text);
                m_IsSaveIo = chkSaveIo.Checked;
                m_InSize = Convert.ToInt32(this.txtInSize.Text);
                m_OutSize = Convert.ToInt32(this.txtOutSize.Text);
                m_DiSize = Convert.ToInt32(this.txtDiSize.Text);
                m_DoSize = Convert.ToInt32(this.txtDoSize.Text);
                m_AiSize = Convert.ToInt32(this.txtAiSize.Text);
                m_AoSize = Convert.ToInt32(this.txtAoSize.Text);

                m_DicOffset.Clear();
                m_DicOffset.Add(VarType.In, m_InOffset);
                m_DicOffset.Add(VarType.Out, m_OutOffset);
                m_DicOffset.Add(VarType.Di, m_DiOffset);
                m_DicOffset.Add(VarType.Do, m_DoOffset);
                m_DicOffset.Add(VarType.Ai, m_AiOffset);
                m_DicOffset.Add(VarType.Ao, m_AoOffset);

                m_InNode.AmsNetId = m_AmsNetId;
                m_InNode.Offset = m_InOffset;
                m_InNode.Size = m_InSize;

                m_OutNode.AmsNetId = m_AmsNetId;
                m_OutNode.Offset = m_OutOffset;
                m_OutNode.Size = m_OutSize;

                m_DiNode.AmsNetId = m_AmsNetId;
                m_DiNode.Offset = m_DiOffset / 8;
                m_DiNode.Size = m_DiSize;

                m_DoNode.AmsNetId = m_AmsNetId;
                m_DoNode.Offset = m_DoOffset / 8;
                m_DoNode.Size = m_DoSize;

                m_AiNode.AmsNetId = m_AmsNetId;
                m_AiNode.Offset = m_AiOffset;
                m_AiNode.Size = m_AiSize;

                m_AoNode.AmsNetId = m_AmsNetId;
                m_AoNode.Offset = m_AoOffset;
                m_AoNode.Size = m_AoSize;

                string[] nodes = txtDiNodeStartNo.Text.Split(',');
                m_DiNodes = new int[nodes.Length];
                for (int i = 0; i < m_DiNodes.Length; i++)
                {
                    m_DiNodes[i] = (nodes[i] != "") ? Convert.ToInt32(nodes[i]) : -1;
                }

                nodes = txtDoNodeStartNo.Text.Split(',');
                m_DoNodes = new int[nodes.Length];
                for (int i = 0; i < m_DoNodes.Length; i++)
                {
                    m_DoNodes[i] = (nodes[i] != "") ? Convert.ToInt32(nodes[i]) : -1;
                }

                nodes = txtAiNodeStartNo.Text.Split(',');
                m_AiNodes = new int[nodes.Length];
                for (int i = 0; i < m_AiNodes.Length; i++)
                {
                    m_AiNodes[i] = (nodes[i] != "") ? Convert.ToInt32(nodes[i]) : -1;
                }

                nodes = txtAoNodeStartNo.Text.Split(',');
                m_AoNodes = new int[nodes.Length];
                for (int i = 0; i < m_AoNodes.Length; i++)
                {
                    m_AoNodes[i] = (nodes[i] != "") ? Convert.ToInt32(nodes[i]) : -1;
                }
                #endregion

                bool ok = CreateController();
                if (ok)
                {
                    ok &= CreateIoList();
                    if (ok)
                    {
                        SortItem(m_InNode);
                        SortItem(m_OutNode);
                        SortItem(m_DiNode);
                        SortItem(m_DoNode);
                        SortItem(m_AiNode);
                        SortItem(m_AoNode);

                        m_DiNode.NodeStartNo = txtDiNodeStartNo.Text;
                        m_DoNode.NodeStartNo = txtDoNodeStartNo.Text;
                        m_AiNode.NodeStartNo = txtAiNodeStartNo.Text;
                        m_AoNode.NodeStartNo = txtAoNodeStartNo.Text;

                        IoDefines io = new IoDefines(FieldBusType.TwinCATPlc);
                        io.Container.Add(m_InNode);
                        io.Container.Add(m_OutNode);
                        if (m_IsSaveIo)
                        {
                            io.Container.Add(m_DiNode);
                            io.Container.Add(m_DoNode);
                            io.Container.Add(m_AiNode);
                            io.Container.Add(m_AoNode);
                        }
                       
                        io.WriteXml();
                        //m_Collection.WriteXml();
                        WirteCsharpCode(io.Container);
                        MessageBox.Show("Save IO Define success!!!");
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
         }

        private void SortItem(IoNodeTwinCATPLC ioNode)
        {
            foreach (IoTerminal terminal in ioNode.Terminals)
            {
                terminal.Channels.Sort(CompareById);
            }
        }

        private int CompareById(IoItem x, IoItem y)
        {
            return x.Id.CompareTo(y.Id);
        }

        private void Clear()
        {
            m_InTerminalCount = 0;
            m_OutTerminalCount = 0;
            m_DiTerminalCount = 0;
            m_DoTerminalCount = 0;
            m_AiTerminalCount = 0;
            m_AoTerminalCount = 0;

            m_InNode.Terminals.Clear();
            m_OutNode.Terminals.Clear();
            m_DiNode.Terminals.Clear();
            m_DoNode.Terminals.Clear();
            m_AiNode.Terminals.Clear();
            m_AoNode.Terminals.Clear();
        }

        private bool CreateController()
        {
            TcSystemServerClass server = new TcSystemServerClass();

            if (server.SystemState == (int)AdsState.Stop)
            {
                string msg = "TwinCAT Start failed";
                MessageBox.Show(msg);
                return false;
            }

            m_Client = new TcAdsClient();
            m_Client.Connect(m_AmsNetId, m_Port);
            if (m_Client.IsConnected == false)
            {
                string msg = "TwinCAT Connection failed!";
                MessageBox.Show(msg);

                return false;
            }
            return true;
        }

        private bool CreateIoList()
        {
            try
            {
                Clear();
                TcAdsSymbolInfoLoader loader = m_Client.CreateSymbolInfoLoader();
                foreach (TcAdsSymbolInfo info in loader)
                {
                    switch (info.IndexGroup)
                    {
                        case (long)AdsReservedIndexGroups.PlcRWMB:
                        case (long)AdsReservedIndexGroups.IOImageRWIX:
                        case (long)AdsReservedIndexGroups.IOImageRWOX:
                        case (long)AdsReservedIndexGroups.IOImageRWIB:
                        case (long)AdsReservedIndexGroups.IOImageRWOB:
                            {
                                VarType type = GetVarType(info);
                                if (type == VarType.None) continue;

                                MakeItems(info, type);

                                //TwinCATPlcType type = GetType(info);
                                //if (type == TwinCATPlcType.None) continue;

                                //MakeItemList(info, m_Collection, type);
                            }
                            break;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
            return true;
        }

        private int AddTerminalCount(VarType type)
        {
            int id = 0;
            switch (type)
            {
                case VarType.In:
                    id = m_InTerminalCount++;
                    break;
                case VarType.Out:
                    id = m_OutTerminalCount++;
                    break;
                case VarType.Di:
                    id = m_DiTerminalCount++;
                    break;
                case VarType.Do:
                    id = m_DoTerminalCount++;
                    break;
                case VarType.Ai:
                    id = m_AiTerminalCount++;
                    break;
                case VarType.Ao:
                    id = m_AoTerminalCount++;
                    break;
            }
            return id;
        }

        private void MakeItems(TcAdsSymbolInfo info, VarType type)
        {
            if (info.Parent == null)
            {
                if (info.SubSymbolCount == 0)
                {
                    //parent x, subsymbol x => terminal 1ea, io 1ea
                    TwinCATPlcTerminal terminal;// = new TwinCATPlcTerminal();
                    if (type == VarType.In || type == VarType.Out)
                    {
                        terminal = new TwinCATPlcTerminal();
                        terminal.Id = AddTerminalCount(type);
                    }
                    else
                    {
                        terminal = GetTerminal(type, 1) as TwinCATPlcTerminal;
                        if (terminal == null)
                        {
                            terminal = new TwinCATPlcTerminal();
                            terminal.Id = AddTerminalCount(type);
                        }
                    }

                    IoItem item = GetIoItem(type, info);
                    item.Id = (int)info.IndexOffset - m_DicOffset[type];
                    if (type == VarType.Ai || type == VarType.Ao) item.Id = item.Id / 2;
                    item.Node = GetNodeNo(type, info.Size, item.Id);
                    item.Channel = (int)type;                                   //channel로 VarType구분
                    item.Terminal = (int)info.Datatype;                         //변수 읽을때 DataType별로 형변환하기위해
                    item.WiringNo = GetWiringNo(item, type, info.IndexOffset);  //DI,DO,AI,AO의 정확한 WiringNo를 위해
                    item.Name = m_DicName[item.IoType] + info.Name;

                    terminal.IoType = item.IoType;
                    terminal.Channels.Add(item);
                    terminal.ChannelCount++;

                    AddTerminal(type, terminal);
                }
                else
                {
                    //parent x, subsymbol o => terminal 1ea
                    TwinCATPlcTerminal terminal = new TwinCATPlcTerminal();
                    terminal.Id = AddTerminalCount(type);

                    AddTerminal(type, terminal);
                }
            }
            else
            {
                if (info.SubSymbolCount == 0)
                {
                    //parent o, subsymbol x => item 1ea
                    //IoItem item = GetIoItem(type, info);
                    IoItem item = GetIoItem(type, info);
                    item.Id = (int)info.IndexOffset - m_DicOffset[type];
                    //item.Node = (int)type;
                    item.Node = GetNodeNo(type, info.Size, item.Id);
                    //item.Terminal = terminal.Id;
                    item.WiringNo = string.Format("{0:X4}", info.IndexOffset);
                    item.Name = m_DicName[item.IoType] + info.Name;

                    TwinCATPlcTerminal terminal = GetTerminal(type, 1) as TwinCATPlcTerminal;
                    if (terminal.IoType == item.IoType)
                    {
                        //item.Terminal = terminal.Id;
                        item.Terminal = (int)info.Datatype;
                        //item.Channel = (int)info.Datatype; //terminal.ChannelCount;
                        item.Channel = (int)type;

                        terminal.Channels.Add(item);
                        terminal.ChannelCount++;
                    }
                    else
                    {
                        if (terminal.ChannelCount == 0)
                        {
                            //terminal = new TwinCATPlcTerminal();
                            //terminal.Id = AddTerminalCount(type);
                            terminal.IoType = item.IoType;

                            //item.Terminal = terminal.Id;
                            item.Terminal = (int)info.Datatype;
                            //item.Channel = (int)info.Datatype; //terminal.ChannelCount;
                            item.Channel = (int)type;

                            terminal.Channels.Add(item);
                            terminal.ChannelCount++;

                            //AddTerminal(type, terminal);
                        }
                        else
                        {
                            terminal = GetTerminal(type, 2) as TwinCATPlcTerminal;
                            if (terminal == null || terminal.IoType != item.IoType)
                            {
                                terminal = new TwinCATPlcTerminal();
                                terminal.Id = AddTerminalCount(type);
                                terminal.IoType = item.IoType;

                                //item.Terminal = terminal.Id;
                                item.Terminal = (int)info.Datatype;
                                //item.Channel = (int)info.Datatype; //terminal.ChannelCount;
                                item.Channel = (int)type;

                                terminal.Channels.Add(item);
                                terminal.ChannelCount++;

                                AddTerminal(type, terminal);
                            }
                            else
                            {
                                //item.Terminal = terminal.Id;
                                item.Terminal = (int)info.Datatype;
                                //item.Channel = (int)info.Datatype;
                                item.Channel = (int)type;

                                terminal.Channels.Add(item);
                                terminal.ChannelCount++;
                            }
                        }
                    }
                }
                else
                {
                    //parent o, subsymbol o => nothing
                }
            }
        }

        private string GetWiringNo(IoItem item, VarType type, long offset)
        {
            if (type == VarType.In || type == VarType.Out) return string.Format("{0:X4}", offset);
            else
            {
                string wiringCategory = "";
                int no = item.Id;
                if (type == VarType.Di)
                {
                    wiringCategory = "X";
                    no = (m_DiNodes[item.Node - 1] > 0) ? item.Id - m_DiNodes[item.Node - 1] + 1 : item.Id;
                }
                else if (type == VarType.Do)
                {
                    wiringCategory = "Y";
                    no = (m_DoNodes[item.Node - 1] > 0) ? item.Id - m_DoNodes[item.Node - 1] + 1 : item.Id;
                }
                else if (type == VarType.Ai)
                {
                    wiringCategory = "AI";
                    no = (m_AiNodes[item.Node - 1] > 0) ? item.Id - m_AiNodes[item.Node - 1] + 1 : item.Id;
                }
                else if (type == VarType.Ao)
                {
                    wiringCategory = "AO";
                    no = (m_AoNodes[item.Node - 1] > 0) ? item.Id - m_AoNodes[item.Node - 1] + 1 : item.Id;
                }

                string wireNo = string.Format("{0:d3}", Convert.ToInt32(Convert.ToString(no, 8)));
                return item.Node.ToString() + wiringCategory + wireNo;
            }
        }

        private int GetNodeNo(VarType type, int size, int id)
        {
            if (type == VarType.In || type == VarType.Out) return size;
            else
            {
                switch (type)
                {
                    case VarType.Di:
                        {
                            for (int i = 0; i < m_DiNodes.Length; i++)
                            {
                                if ((i + 1) == m_DiNodes.Length) return (i + 1);
                                if ((id + 1) >= m_DiNodes[i] && (id + 1) < m_DiNodes[i + 1]) return (i + 1);
                            }
                        }
                        break;
                    case VarType.Do:
                        {
                            for (int i = 0; i < m_DoNodes.Length; i++)
                            {
                                if ((i + 1) == m_DoNodes.Length) return (i + 1);
                                if ((id + 1) >= m_DoNodes[i] && (id + 1) < m_DoNodes[i + 1]) return (i + 1);
                            }
                        }
                        break;
                    case VarType.Ai:
                        {
                            for (int i = 0; i < m_AiNodes.Length; i++)
                            {
                                if ((i + 1) == m_AiNodes.Length) return (i + 1);
                                if ((id + 1) >= m_AiNodes[i] && (id + 1) < m_AiNodes[i + 1]) return (i + 1);
                            }
                        }
                        break;
                    case VarType.Ao:
                        {
                            for (int i = 0; i < m_AoNodes.Length; i++)
                            {
                                if ((i + 1) == m_AoNodes.Length) return (i + 1);
                                if ((id + 1) >= m_AoNodes[i] && (id + 1) < m_AoNodes[i + 1]) return (i + 1);
                            }
                        }
                        break;
                }
                return 1;
            }
        }

        private void AddTerminal(VarType type, TwinCATPlcTerminal terminal)
        {
            switch (type)
            {
                case VarType.In:
                    m_InNode.Terminals.Add(terminal);
                    break;
                case VarType.Out:
                    m_OutNode.Terminals.Add(terminal);
                    break;
                case VarType.Di:
                    if(m_DiNode.Terminals.Count == 0) m_DiNode.Terminals.Add(terminal);
                    break;
                case VarType.Do:
                    if(m_DoNode.Terminals.Count == 0) m_DoNode.Terminals.Add(terminal);
                    break;
                case VarType.Ai:
                    if(m_AiNode.Terminals.Count == 0) m_AiNode.Terminals.Add(terminal);
                    break;
                case VarType.Ao:
                    if(m_AoNode.Terminals.Count == 0) m_AoNode.Terminals.Add(terminal);
                    break;
            }
        }

        private IoTerminal GetTerminal(VarType type, int index)
        {
            try
            {
                switch (type)
                {
                    case VarType.In:
                        return (index > m_InTerminalCount) ? null : m_InNode.Terminals[m_InTerminalCount - index];
                    case VarType.Out:
                        return (index > m_OutTerminalCount) ? null : m_OutNode.Terminals[m_OutTerminalCount - index];
                    case VarType.Di:
                        return (index > m_DiTerminalCount) ? null : m_DiNode.Terminals[m_DiTerminalCount - index];
                    case VarType.Do:
                        return (index > m_DoTerminalCount) ? null : m_DoNode.Terminals[m_DoTerminalCount - index];
                    case VarType.Ai:
                        return (index > m_AiTerminalCount) ? null : m_AiNode.Terminals[m_AiTerminalCount - index];
                    case VarType.Ao:
                        return (index > m_AoTerminalCount) ? null : m_AoNode.Terminals[m_AoTerminalCount - index];
                    default:
                        return null;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return null;
            }
        }

        private IoItem GetIoItem(VarType type, TcAdsSymbolInfo info)
        {
            IoItem item;
            if (type == VarType.Di || ((type == VarType.In) && (info.Type == "BOOL")))
            {
                item = new IoItemDI();
                item.IoType = IoType.DI;
            }
            else if (type == VarType.Do || ((type == VarType.Out) && (info.Type == "BOOL")))
            {
                item = new IoItem();
                item.IoType = IoType.DO;
            }
            else if ((type == VarType.In) || (type == VarType.Ai))
            {
                item = new IoItem();
                item.IoType = IoType.AI;
            }
            else
            {
                item = new IoItem();
                item.IoType = IoType.AO;
            }
            return item;
        }

        private VarType GetVarType(TcAdsSymbolInfo info)
        {
            int offset = (int)info.IndexOffset;
            long group = info.IndexGroup;
            if (m_IsSaveIo)
            {
                if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_InOffset && offset < (m_InOffset + m_InSize)) return VarType.In;
                else if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_OutOffset && offset < (m_OutOffset + m_OutSize)) return VarType.Out;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWIX && offset >= m_DiOffset && offset < (m_DiOffset + m_DiSize)) return VarType.Di;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWOX && offset >= m_DoOffset && offset < (m_DoOffset + m_DoSize)) return VarType.Do;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWIB && offset >= m_AiOffset && offset < (m_AiOffset + m_AiSize)) return VarType.Ai;
                else if (group == (long)AdsReservedIndexGroups.IOImageRWOB && offset >= m_AoOffset && offset < (m_AoOffset + m_AoSize)) return VarType.Ao;
                else return VarType.None;
            }
            else
            {
                if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_InOffset && offset < (m_InOffset + m_InSize)) return VarType.In;
                else if (group == (long)AdsReservedIndexGroups.PlcRWMB && offset >= m_OutOffset && offset < (m_OutOffset + m_OutSize)) return VarType.Out;
                else return VarType.None;
            }
        }

        public void WirteCsharpCode(List<IoNode> componentContainer)
        {
            GenerateCode(new CSharpCodeProvider(), BuildCode(componentContainer));
        }

        protected void GenerateCode(CodeDomProvider provider, CodeCompileUnit compileUnit)
        {
            string codeFile = AppConfig.Instance.IoDefinePathName + @"\TwinCATPlcIo." + provider.FileExtension;

            FileInfo file = new FileInfo(codeFile);
            if (file.Exists)
            {
                file.CopyTo(codeFile + ".old", true);
            }

            IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(codeFile, false, System.Text.Encoding.Default), "    ");
            CodeGeneratorOptions option = new CodeGeneratorOptions();
            option.BracingStyle = "C";
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, option);
            writer.Close();
        }

        private string _DITypeName = typeof(IoDigitalInput).Name;
        private string _DOTypeName = typeof(IoDigitalOutput).Name;
        private string _AITypeName = typeof(IoAnalogInput).Name;
        private string _AOTypeName = typeof(IoAnalogOutput).Name;
        protected CodeCompileUnit BuildCode(List<IoNode> componentContainer)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Namespace
            CodeNamespace nameSpace = new CodeNamespace("Dms.Server");
            compileUnit.Namespaces.Add(nameSpace);

            // using
            nameSpace.Imports.Add(new CodeNamespaceImport("Dms.Device"));

            // Build Class and Member
            foreach (IoNode node in componentContainer)
            {
                // Class
                string className = "io" + ((VarType)node.Id).ToString();
                CodeTypeDeclaration class1 = new CodeTypeDeclaration(className);
                nameSpace.Types.Add(class1);

                foreach (IoTerminal terminal in node.Terminals)
                {
                    foreach (IoItem item in terminal.Channels)
                    {
                        string ioName = XFunc.FilterigName(item.Name);
                        CodeTypeReference ioType = new CodeTypeReference();

                        // Declares a field
                        CodeMemberField field = new CodeMemberField();
                        field.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                        field.Name = "m_" + ioName;
                        CodeVariableReferenceExpression initExpression = new CodeVariableReferenceExpression();
                        switch(item.IoType)
                        {
                            case IoType.DI:
                                {
                                    initExpression.VariableName = "ServerManager.Instance.Digitalinputs[\"" + item.Name + "\"]" + " as " + _DITypeName;
                                    ioType = new CodeTypeReference(_DITypeName);
                                }
                                break;
                            case IoType.DO:
                                {
                                    initExpression.VariableName = "ServerManager.Instance.DigitalOutputs[\"" + item.Name + "\"]" + " as " + _DOTypeName;
                                    ioType = new CodeTypeReference(_DOTypeName);
                                }
                                break;
                            case IoType.AI:
                                {
                                    initExpression.VariableName = "ServerManager.Instance.AnalogInputs[\"" + item.Name + "\"]" + " as " + _AITypeName;
                                    ioType = new CodeTypeReference(_AITypeName);
                                } 
                                break;
                            case IoType.AO:
                                {
                                    initExpression.VariableName = "ServerManager.Instance.AnalogOutputs[\"" + item.Name + "\"]" + " as " + _AOTypeName;
                                    ioType = new CodeTypeReference(_AOTypeName);
                                }
                                break;
                        }
                        field.Type = ioType;
                        field.InitExpression = initExpression;
                        class1.Members.Add(field);

                        // Declares a property of type String named StringProperty.
                        CodeMemberProperty property1 = new CodeMemberProperty();
                        property1.Name = "_" + ioName + "_Name";
                        property1.Type = new CodeTypeReference("System.String");
                        property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                        property1.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(item.Name)));
                        class1.Members.Add(property1);

                        CodeMemberProperty property3 = new CodeMemberProperty();
                        property3.Name = "_" + ioName;
                        property3.Type = ioType;
                        property3.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                        CodeVariableReferenceExpression variableRef2 = new CodeVariableReferenceExpression();
                        variableRef2.VariableName = field.Name;
                        property3.GetStatements.Add(new CodeMethodReturnStatement(variableRef2));
                        class1.Members.Add(property3);
                    }
                }
            }
            return compileUnit;
        }
        #endregion
    }
}
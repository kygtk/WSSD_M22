using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.Util.IODefine;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace Dms.Util
{
    public partial class FormIoSimulator : Form
    {
        private List<_DeviceAsm> m_Devices = new List<_DeviceAsm>();
        private IoDefines m_Iolist = null;   //전체 I/O List
        private IoDefines m_SelectedDeviceIoList = new IoDefines();   // 선택된 Device의 I/O List
        private List<IoDefines> m_DeviceIoLists = new List<IoDefines>();
        private ICtlDevice m_IoController;

        //for sequence simulator
        private List<XSequence> m_SimulSeqList = new List<XSequence>();
        private XSequence m_SelectedSeq = null;
        private bool m_Initialzed = false;

        public FormIoSimulator()
        {
            InitializeComponent();
        }

        public void SetIoController(ICtlDevice ioController)
        {
            m_IoController = ioController;
        }

        public void SetIoDefine(IoDefines iodefine)
        {
            m_Iolist = iodefine;
        }

        public void AddDevice(_DeviceAsm device)
        {
            m_Devices.Add(device);
        }

        private void InitializeDeviceIoList()
        {
            int deviceCount = m_Devices.Count;
            for (int i = 0; i < deviceCount; i++)
            {
                IoDefines ioDefine = new IoDefines();
                ArrayList dis = m_Devices[i].GetAssociatedIoDevices(IoType.DI);
                foreach (object io in dis)
                {
                    ioDefine.DigitalInputs.Add(((IoDigitalInput)io).IoInfo);
                }
                ArrayList dos = m_Devices[i].GetAssociatedIoDevices(IoType.DO);
                foreach (object io in dos)
                {
                    ioDefine.DigitalOutputs.Add(((IoDigitalOutput)io).GetIoInfo());
                }
                ArrayList ais = m_Devices[i].GetAssociatedIoDevices(IoType.AI);
                foreach (object io in ais)
                {
                    ioDefine.AnalogInputs.Add(((IoAnalogInput)io).GetIoInfo());
                }
                ArrayList aos = m_Devices[i].GetAssociatedIoDevices(IoType.AO);
                foreach (object io in aos)
                {
                    ioDefine.AnalogOutputs.Add(((IoAnalogOutput)io).GetIoInfo());
                }

                m_DeviceIoLists.Add(ioDefine);
            }        
        }

        private void InitializeViewIoList()
        {
            treeViewIoList.BeginUpdate();
            treeViewIoList.Nodes.Clear();

            // 전체 I/O List에 대해 Node 하나 생성
            if (m_Iolist != null)
            {
                TreeNode node = new TreeNode();
                node.Tag = m_Iolist;
                node.Text = "ALL I/O List";
                treeViewIoList.Nodes.Add(node);
            }
            
            // 등록된 Device들의 I/O List Node 생성
            int deviceCount = m_Devices.Count;
            for (int i = 0; i < deviceCount; i++)
            { 
                TreeNode node = new TreeNode();
                node.Tag = m_DeviceIoLists[i];
                node.Text = m_Devices[i].Name;
                treeViewIoList.Nodes.Add(node);
            }

            treeViewIoList.ExpandAll();
            treeViewIoList.EndUpdate();

            //생성된것이 있다면 첫번째 껄로 초기화
            if (treeViewIoList.Nodes.Count > 0)
            {
                m_SelectedDeviceIoList = treeViewIoList.Nodes[0].Tag as IoDefines;
            }
        }

        private void InitializeViewIoEdit()
        {
            viewIOEdit.Initialize(m_SelectedDeviceIoList, ViewIOEdit.OpMode.System, m_IoController);
        }

        private void FormIoSimulator_Load(object sender, EventArgs e)
        {
            MakeSimulSeqList();
            MakeSimulSeqLogList();
            viewIOEdit.TimerStateUpdateEnabled = true;
        }

        private void MakeSimulSeqList()
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(AppConfig.GetAppRootPath() + "Assembly\\Dms.Simulator.dll");
                Type[] types = asm.GetTypes();

                foreach (Type type in types)
                {
                    if (XFunc.CheckTypeCompatibility(typeof(XSequence), type, Compatibility.Compatible))
                    {
                        if (!type.IsAbstract)
                        {
                            XSequence seq = Activator.CreateInstance(type) as XSequence;

                            m_SimulSeqList.Add(seq);
                        }
                    }
                }

                this.listBoxSimulSequence.DataSource = m_SimulSeqList;
            }
            catch
            {
                this.tabControl1.TabPages.Remove(this.tabPageSequence);
            }
        }

        private void MakeSimulSeqLogList()
        {
            XSimLog.LogList = this.listBoxSeqLog;
        }

        private void treeViewIoList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;
            m_SelectedDeviceIoList = curNode.Tag as IoDefines;

            viewIOEdit.Initialize(m_SelectedDeviceIoList, ViewIOEdit.OpMode.System, m_IoController);
            viewIOEdit.TimerStateUpdateEnabled = true;
        }

        public void Initialize()
        {
            InitializeDeviceIoList();
            InitializeViewIoList();
            InitializeViewIoEdit();

            m_Initialzed = true;
        }

        public void Uninitialize()
        {
            if(m_Initialzed)
            {
                m_Initialzed = false;

                //등록된 모든 Timer kill
                DmsUserControl.UninitializeUpdateTimer(this);
                
                //등록된 모든 Sequence stop
                StopAllSequence();
                
                Thread.Sleep(300);
            }
        }

        private void StopAllSequence()
        {
            foreach (XSequence seq in m_SimulSeqList)
            {
                seq.Pause();
            }        
        }

        private void StartAllSequence()
        {
            foreach (XSequence seq in m_SimulSeqList)
            {
                seq.Start();
            }        
        }

        private void FormIoSimulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Uninitialize();
        }

        private void listBoxSimulSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.listBoxSimulSequence.SelectedIndex;

            if (m_SimulSeqList.Count > 0) m_SelectedSeq = m_SimulSeqList[index];
        } 

        private void btnSeqStart_Click(object sender, EventArgs e)
        {
            if (m_SelectedSeq != null)
            {
                m_SelectedSeq.Start();
            }
        }

        private void btnSeqPause_Click(object sender, EventArgs e)
        {
            if (m_SelectedSeq != null)
            {
                m_SelectedSeq.Pause();
            }
        }

        private void btnSeqStartAll_Click(object sender, EventArgs e)
        {
            StartAllSequence();
        }

        private void btnSeqPauseAll_Click(object sender, EventArgs e)
        {
            StopAllSequence();
        }            
    }
}
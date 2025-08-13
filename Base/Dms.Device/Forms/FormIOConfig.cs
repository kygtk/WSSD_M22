using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Dms.Util.IODefine;

namespace Dms.Device
{
    public partial class FormIOConfig : Form
    {
        #region Enums
        //Current Config tree에서 선택된 level
        enum ObjLevel
        {
            Top,
            Item
        }
        #endregion

        #region Fields
        private object m_SelectedItemInCurConfig = null; //Current Config view에서 선택된 아이템
        private IoDefines m_IoDefines;  //IoList를 만들어야 함
        private IoType m_OriginalIoType; // Io List를 Sort 할 선택된 원본 Io의 Type
        private iIoCollection m_OriginalCollection;//선택된 원본 IoCollection;
        private iIoCollection m_SelectedCollection = null; //UIEditor에 반환될 결과
        #endregion

        #region Properties
        public iIoCollection SelectedCollection
        {
            get { return m_SelectedCollection; }
        }
        #endregion

        #region Constructor
        public FormIOConfig()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
        }
        #endregion

        public void Initialize(object currentConfig)
        {
            //Initialize Current selected view
            InitializeCurrentConfigTree(currentConfig);

            //Initialize Io List
            InitializeIoList();
        }

        private void InitializeCurrentConfigTree(object curConfig)
        {
            m_OriginalCollection = curConfig as iIoCollection;
            m_OriginalIoType = m_OriginalCollection.ContainedIoType;
            m_SelectedCollection = m_OriginalCollection.CreateCollection();
            this.validationTextBoxMaxSimulateCount.UsedInKeyPad = true;
            this.validationTextBoxMaxSimulateCount.Text = m_OriginalCollection.MaxSimulateCount.ToString();

            foreach (object obj in m_OriginalCollection)
            {
                m_SelectedCollection.Add(obj);
            }

            treeViewCurrentConfig.BeginUpdate();
            treeViewCurrentConfig.Nodes.Clear();
            TreeNode node1 = new TreeNode();
            node1.Tag = m_SelectedCollection;
            node1.Text = m_OriginalCollection.Name;

            foreach (object item in m_SelectedCollection)
            {
                TreeNode node2 = new TreeNode();
                node2.Tag = item;
                node2.Text = ((_Device)item).Name;
                node1.Nodes.Add(node2);
            }

            treeViewCurrentConfig.Nodes.Add(node1);
            treeViewCurrentConfig.ExpandAll();
            treeViewCurrentConfig.EndUpdate();
        }

        private void InitializeIoList()
        {
            List<IoDefines> ioDefineList = IoDefines.ReadIodefineList();

            m_IoDefines = new IoDefines();

            foreach (IoDefines ioList in ioDefineList)
            {
                foreach (IoItem item in ioList.DigitalInputs)
                {
                    m_IoDefines.AddIOCollection(item);
                }
                foreach (IoItem item in ioList.DigitalOutputs)
                {
                    m_IoDefines.AddIOCollection(item);
                }
                foreach (IoItem item in ioList.AnalogInputs)
                {
                    m_IoDefines.AddIOCollection(item);
                }
                foreach (IoItem item in ioList.AnalogOutputs)
                {
                    m_IoDefines.AddIOCollection(item);
                }
            }

            this.viewIOEdit1.OperateMode = ViewIOEdit.OpMode.MultiSelect;
            this.viewIOEdit1.InitializeByFilter(m_IoDefines, m_OriginalIoType);
        }

        public void UpdateCurrentConfigTree(iIoCollection curConfig)
        {
            treeViewCurrentConfig.BeginUpdate();
            treeViewCurrentConfig.Nodes[0].Nodes.Clear();

            foreach (object item in curConfig)
            {
                TreeNode node = new TreeNode();
                node.Tag = item;
                node.Text = ((_Device)item).Name;

                treeViewCurrentConfig.Nodes[0].Nodes.Add(node);
            }
            treeViewCurrentConfig.ExpandAll();
            treeViewCurrentConfig.EndUpdate();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string selectedIoName = this.viewIOEdit1.SelectedName;
            List<string> names = this.viewIOEdit1.SelectedNames;

            int count = names.Count;
            _DeviceIo io;

            //if (string.IsNullOrEmpty(selectedIoName))
            if (count == 0)
            {
                MessageBox.Show("Select Io item 1st!");
            }
            else
            {
                foreach (_Device item in m_SelectedCollection)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (item.Name == names[i])
                        {
                            MessageBox.Show(names[i] + " is alreay contained!");
                            return;
                        }
                    }
                }

                //_DeviceIo io = m_SelectedIoCollection.CreateNewItem();
                //io.Name = selectedIoName;

                for (int i = 0; i < count; i++)
                {
                    io = m_OriginalCollection.CreateNewItem();
                    io.Name = names[i];
                    m_SelectedCollection.Add(io);
                }
                UpdateCurrentConfigTree(m_SelectedCollection);
                treeViewCurrentConfig.SelectedNode = treeViewCurrentConfig.Nodes[0].Nodes[m_SelectedCollection.Count - 1];
                m_SelectedItemInCurConfig = treeViewCurrentConfig.SelectedNode.Tag;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (m_SelectedItemInCurConfig == null)
            {
                MessageBox.Show("Select Io item 1st!");
            }
            else
            {
                int index = m_SelectedCollection.IndexOf(m_SelectedItemInCurConfig);
                m_SelectedCollection.RemoveItem(index);
                UpdateCurrentConfigTree(m_SelectedCollection);
                if (m_SelectedCollection.Count > index)
                {
                    treeViewCurrentConfig.SelectedNode = treeViewCurrentConfig.Nodes[0].Nodes[index];
                    m_SelectedItemInCurConfig = treeViewCurrentConfig.SelectedNode.Tag;
                }
                else
                {
                    m_SelectedItemInCurConfig = null;
                }
            }
        }

        private void treeViewCurrentConfig_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;

            if (curNode.Level == (int)ObjLevel.Item)
            {
                m_SelectedItemInCurConfig = curNode.Tag;
                this.buttonUp.Enabled = true;
                this.buttonDown.Enabled = true;
            }
            else
            {
                this.buttonUp.Enabled = false;
                this.buttonDown.Enabled = false;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (m_SelectedItemInCurConfig == null)
            {
                MessageBox.Show("Select Io item first");
            }
            else
            {
                string name = ((_DeviceIo)m_SelectedItemInCurConfig).ToString();
                int index = m_SelectedCollection.IndexOf(m_SelectedItemInCurConfig);
                if (index == 0) return;
                m_SelectedCollection.RemoveItem(index);
                m_SelectedCollection.InsertItem(index - 1, m_SelectedItemInCurConfig);
                UpdateCurrentConfigTree(m_SelectedCollection);
                treeViewCurrentConfig.SelectedNode = treeViewCurrentConfig.Nodes[0].Nodes[index - 1];
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (m_SelectedItemInCurConfig == null)
            {
                MessageBox.Show("Select Io item first");
            }
            else
            {
                string name = ((_DeviceIo)m_SelectedItemInCurConfig).ToString();
                int index = m_SelectedCollection.IndexOf(m_SelectedItemInCurConfig);
                if (index == m_SelectedCollection.Count - 1) return;
                m_SelectedCollection.RemoveItem(index);
                m_SelectedCollection.InsertItem(index + 1, m_SelectedItemInCurConfig);
                UpdateCurrentConfigTree(m_SelectedCollection);
                treeViewCurrentConfig.SelectedNode = treeViewCurrentConfig.Nodes[0].Nodes[index + 1];
            }
        }

        private void validationTextBoxMaxSimulateCount_TextChanged(object sender, EventArgs e)
        {
            m_SelectedCollection.MaxSimulateCount = Convert.ToInt32(this.validationTextBoxMaxSimulateCount.Text);
        }
    }
}
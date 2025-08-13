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
    public partial class FormSlaveConfig : Form
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
        private EcSlaveItemType m_OriginalSlaveItemType; // Io List를 Sort 할 선택된 원본 Io의 Type
        private ISlaveCollection m_OriginalCollection;//선택된 원본 IoCollection;
        private ISlaveCollection m_SelectedCollection = null; //UIEditor에 반환될 결과
        #endregion

        #region Properties
        public ISlaveCollection SelectedCollection
        {
            get { return m_SelectedCollection; }
        }
        #endregion

        #region Constructor
        public FormSlaveConfig()
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

        #region Methods
        public void Initialize(object currentConfig)
        {
            InitializeCurrentConfigTree(currentConfig);

            InitializeSlaveList();
        }

        private void InitializeCurrentConfigTree(object curConfig)
        {
            m_OriginalCollection = curConfig as ISlaveCollection;
            m_OriginalSlaveItemType = m_OriginalCollection.ContainedSlaveItemType;
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

        private void InitializeSlaveList()
        {
            List<IoDefines> ioDefineList = IoDefines.ReadIodefineList();

            m_IoDefines = new IoDefines();

            foreach (IoDefines ioList in ioDefineList)
            {
                foreach (EcSlaveItem_Servo item in ioList.SlaveServos)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_BLDC item in ioList.SlaveBLDCs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_Inverter item in ioList.SlaveInverters)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_DI item in ioList.SlaveDigitalInputs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_DO item in ioList.SlaveDigitalOutputs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_AI item in ioList.SlaveAnalogInputs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_AO item in ioList.SlaveAnalogOutputs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
                foreach (EcSlaveItem_AP item in ioList.SlaveAPs)
                {
                    m_IoDefines.AddSlaveCollection(item);
                }
            }

            this.viewSlaveEdit1.OperateMode = ViewSlaveEdit.OpMode.MultiSelect;
            this.viewSlaveEdit1.InitializeByFilter(m_IoDefines, m_OriginalSlaveItemType);
        }

        public void UpdateCurrentConfigTree(ISlaveCollection curConfig)
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
        #endregion

        #region Event Handlers
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
            string selectedSlaveName = this.viewSlaveEdit1.SelectedName;
            List<string> names = this.viewSlaveEdit1.SelectedNames;

            int count = names.Count;
            _DeviceSlave slave;

            //if (string.IsNullOrEmpty(selectedIoName))
            if (count == 0)
            {
                MessageBox.Show("Select Slave item 1st!");
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

                for (int i = 0; i < count; i++)
                {
                    slave = m_OriginalCollection.CreateNewItem();
                    slave.Name = names[i];
                    m_SelectedCollection.Add(slave);
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
                MessageBox.Show("Select Slave item 1st!");
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

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (m_SelectedItemInCurConfig == null)
            {
                MessageBox.Show("Select Slave item first");
            }
            else
            {
                string name = ((_DeviceSlave)m_SelectedItemInCurConfig).ToString();
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
                MessageBox.Show("Select Slave item first");
            }
            else
            {
                string name = ((_DeviceSlave)m_SelectedItemInCurConfig).ToString();
                int index = m_SelectedCollection.IndexOf(m_SelectedItemInCurConfig);
                if (index == m_SelectedCollection.Count - 1) return;
                m_SelectedCollection.RemoveItem(index);
                m_SelectedCollection.InsertItem(index + 1, m_SelectedItemInCurConfig);
                UpdateCurrentConfigTree(m_SelectedCollection);
                treeViewCurrentConfig.SelectedNode = treeViewCurrentConfig.Nodes[0].Nodes[index + 1];
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

        private void validationTextBoxMaxSimulateCount_TextChanged(object sender, EventArgs e)
        {
            m_SelectedCollection.MaxSimulateCount = Convert.ToInt32(this.validationTextBoxMaxSimulateCount.Text);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using System.Collections;

namespace Dms.Device
{
    public partial class ViewObjectConfig : UserControl
    {
        enum ObjLevel
        {
            Top,
            Group,
            Item
        }
        enum ContextStripId
        {
            Add,
            Del,
            Save,
            Load
        }

        private TreeNode m_CurNode = null;
        private IComponentContainer m_Components = null;
        private ArrayList m_CurConfig = new ArrayList();
        private object m_SelectedComponent = null;
        private object m_SelectedCurConfig = null;

        private Type m_SelectedDeviceType = null;
        private static string m_FilterString = "";
        private static bool m_FilterOn = false;

        public ArrayList CurConfig
        {
            get { return m_CurConfig; }
        }

        public ViewObjectConfig()
        {
            InitializeComponent();
            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


        public void UpdateCurrentConfigTree(ArrayList curConfig)
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
        
        public void InitializeCurrentConfigTree(object curConfig)
        {
            IGenericCollection collection = curConfig as IGenericCollection;

            //m_CurConfig = collection.Items;
            foreach (object obj in collection)
            {
                m_CurConfig.Add(obj);
            }

            treeViewCurrentConfig.BeginUpdate();
            treeViewCurrentConfig.Nodes.Clear();
            TreeNode node1 = new TreeNode();
            node1.Tag = m_CurConfig;
            node1.Text = collection.Name;

            foreach (object item in m_CurConfig)
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


        private void InitializeComponentsTree(ArrayList components)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeViewComponents.BeginUpdate(); 

            treeViewComponents.Nodes.Clear();

            TreeNode node1 = new TreeNode();
            //node1.Tag = components;
            node1.Text = "Components";

            foreach (object item in components)
            {
                TreeNode node2 = new TreeNode();
                node2.Tag = item;
                node2.Text = ((_Device)item).Name;
                node1.Nodes.Add(node2);
            }

            treeViewComponents.Nodes.Add(node1);
            treeViewComponents.ExpandAll();
            treeViewComponents.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        public void InitializeComponentsTreeByFilter(IComponentContainer components, Type type, params string[] keys)
        {
            m_SelectedDeviceType = type;
            
            //DmsComponents components = DmsComponents.Instance;
            //ComponentContainer components = ComponentContainer.Instance;
            //components.ReadXml();
            
            //ArrayList source = components[type];
            if (m_Components == null)
            {
                m_Components = components;
            }
            
            ArrayList source = components.GetCollectionArray(type, Compatibility.Compatible);
            ArrayList target = new ArrayList();
            foreach (object item in source)
            {
                bool matchAll = true;
                foreach (string key in keys)
                {
                    matchAll &= ((_Device)item).Name.ToLower().Contains(key.ToLower());
                }

                if (matchAll)
                {
                    target.Add(item);
                }            
            }

            if (target.Count == 0)
            {
                target = source;
            }

            InitializeComponentsTree(target);
        }

        private void ViewEqpConfig_Load(object sender, EventArgs e)
        {
            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;

            if (curNode.Level == (int)ObjLevel.Top)
            {
                this.buttonRemove.Enabled = false;
            }
            else
            {
                this.propertyGrid1.SelectedObject = curNode.Tag;
                m_SelectedCurConfig = curNode.Tag;
                this.buttonRemove.Enabled = true;
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            Type type = m_CurNode.Tag as Type;
            TreeNode node = new TreeNode();
            node.Tag = Activator.CreateInstance(type);
            node.Text = type.Name;
            m_CurNode.Nodes.Add(node);
            m_CurNode.ExpandAll();
        }

        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            //m_Components.Container.Remove(m_CurNode.Tag);
            //m_CurNode.Remove();
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Save ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    m_Components.UpdateContainer(this.treeView1.Nodes);
            //    m_Components.WriteXml();
            //}           
        }

        private void toolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            //m_Components.ReadXml();
            //Initialize(m_Components);
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void propertyGrid1_Leave(object sender, EventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadFromToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            foreach (_Device item in m_CurConfig)
            {
                if (item.Name == ((_Device)m_SelectedComponent).Name)
                {
                    MessageBox.Show(m_SelectedComponent.ToString() + " is alreay contained!");
                    return;
                }
            }

            m_CurConfig.Add(m_SelectedComponent);
            UpdateCurrentConfigTree(m_CurConfig);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            m_CurConfig.Remove(m_SelectedCurConfig);
            UpdateCurrentConfigTree(m_CurConfig);

            m_SelectedCurConfig = null;
            this.buttonRemove.Enabled = false;
        }

        private void treeViewComponents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;

            if (curNode.Level == (int)ObjLevel.Top)
            {
                this.buttonAdd.Enabled = false;
            }
            else
            {
                this.propertyGrid1.SelectedObject = curNode.Tag;
                m_SelectedComponent = curNode.Tag;

                this.buttonAdd.Enabled = true;
            }
        }

        private void checkBoxFilterOn_CheckedChanged(object sender, EventArgs e)
        {
            m_FilterOn = (sender as CheckBox).Checked;

            if (m_FilterOn)
            {
                Filtering();
            }
            else
            {
                this.textBoxFilter.Text = "";
                Filtering();
            }
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

        private void Filtering()
        {
            string[] keys = m_FilterString.Split(' ');

            InitializeComponentsTreeByFilter(m_Components, m_SelectedDeviceType, keys);            
        }
    }
}

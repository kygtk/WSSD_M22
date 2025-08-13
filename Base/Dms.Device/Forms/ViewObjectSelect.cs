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
    public partial class ViewObjectSelect : UserControl
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
        private object m_SelectedObject = null;

        private Type m_SelectedDeviceType = null;
        private static string m_FilterString = "";
        private static bool m_FilterOn = false;


        public object SelectedObject
        {
            get { return m_SelectedObject; }
        }

        public string ObjectName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }

        public ViewObjectSelect()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(ArrayList components)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeView1.BeginUpdate(); 

            treeView1.Nodes.Clear();

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
            
            treeView1.Nodes.Add(node1);
            treeView1.ExpandAll();
            treeView1.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        public void InitializeByFilter(IComponentContainer components, Type type, params string[] keys)
        {
            m_SelectedDeviceType = type;

            //DmsComponents components = DmsComponents.Instance;
            //ComponentContainer components = ComponentContainer.Instance;
            //components.ReadXml();

            if (m_Components == null)
            {
                m_Components = components;
            }

            //ArrayList source = components[type];
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

            Initialize(target);
        }

        private void ViewEqpConfig_Load(object sender, EventArgs e)
        {
            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_CurNode = e.Node;

            if (m_CurNode.Level > (int)ObjLevel.Top)
            {
                this.propertyGrid1.SelectedObject = m_CurNode.Tag;
                m_SelectedObject = m_CurNode.Tag;
            }

            //MouseButtons button = e.Button;
            //if (button == MouseButtons.Right)
            //{

            //    m_CurNode.ContextMenuStrip = this.contextMenuStrip1;
            //    m_CurNode.ContextMenuStrip.Show();
            //    if (m_CurNode.Level == (int)ObjLevel.Top)
            //    {
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
            //    }
            //    else if (m_CurNode.Level == (int)ObjLevel.Group)
            //    {
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = true;
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
            //    }
            //    else if (m_CurNode.Level == (int)ObjLevel.Item)
            //    {
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
            //        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = true;
            //    }
            //}
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

            InitializeByFilter(m_Components, m_SelectedDeviceType, keys);
        }
    }
}

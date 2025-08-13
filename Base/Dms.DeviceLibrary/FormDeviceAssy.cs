using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.DeviceLibrary
{
    #region Enum
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
    }
    #endregion

    public partial class FormDeviceAssy : UserControl
    {
        protected TreeNode m_CurNode = null;
        protected TreeNode m_OldNode = null;
        protected bool m_MenuEnabled = false;

        public delegate void TreeNodeClickEvent(object sender, TreeNodeMouseClickEventArgs e);
        public delegate void PropertyValueChangedEvent(object s, PropertyValueChangedEventArgs e);
        public delegate void TreeNodeAddEvent(object sender, EventArgs e);
        public delegate void TreeNodeDelEvent(object sender, EventArgs e, TreeNode node);
        [Category("DMS")]
        public event TreeNodeClickEvent NodeClick;
        public event TreeNodeAddEvent NodeAdd;
        public event TreeNodeDelEvent NodeDel;

        //[Category("DMS")]
        //public event PropertyValueChangedEvent PropertyValueChanged;


        public FormDeviceAssy()
        {
            InitializeComponent();
        }

        public TreeView DeviceTreeView
        {
            get { return treeView1; }
        }

        public TreeNode CurNode
        {
            get { return m_CurNode; }
            set { m_CurNode = value; }
        }

        public bool MenuEnabled
        {
            get { return m_MenuEnabled; }
            set { m_MenuEnabled = value; }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_MenuEnabled == true)
            {
                m_CurNode = e.Node;
                this.propertyGrid1.SelectedObject = m_CurNode.Tag;

                MouseButtons button = e.Button;

                if ((button == MouseButtons.Right) && (m_CurNode.Level != (int)ObjLevel.Item))
                {
                    if (m_CurNode.ContextMenuStrip == null)
                    {
                        m_CurNode.ContextMenuStrip = this.contextMenuStrip1;
                    }

                    m_CurNode.ContextMenuStrip.BackColor = Color.LightCyan;
                    //m_CurNode.ContextMenuStrip.Enabled = (m_DesignerMode == DesignerMode.Design);

                    if (m_CurNode.Level == (int)ObjLevel.Top)
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = true;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
                    }
                    else if (m_CurNode.Level == (int)ObjLevel.Group)
                    {
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = true;
                        m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = true;
                    }
                }
            }

            if( m_OldNode != null ) this.m_OldNode.ForeColor = Color.Black;

            if (NodeClick != null)
            {
                this.m_CurNode = e.Node;

                NodeClick(sender, e);

                propertyGrid1.SelectedObject = m_CurNode.Tag;

                this.m_CurNode.ForeColor = Color.Red;
                this.m_OldNode = m_CurNode;
            }
        }

        public void UpdateTreeViewWith(DmsNode nd)
        {
            treeView1.Nodes.Clear();
            treeView1.BeginUpdate();
            treeView1.Nodes.Add(nd.GetNode());
            treeView1.ExpandAll();
            treeView1.EndUpdate();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                if (m_OldNode != null) this.m_OldNode.ForeColor = Color.Black;

                propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;

                this.m_CurNode = treeView1.SelectedNode;
                this.m_CurNode.ForeColor = Color.Red;
                this.m_OldNode = m_CurNode;
            }
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            NodeAdd(sender, e);
        }

        private void menuDel_Click(object sender, EventArgs e)
        {
            NodeDel(sender, e, m_CurNode);
        }

    }
}

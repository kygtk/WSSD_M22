using Dms.Common;
using Dms.Device;
using Dms.ServerCommon;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dms.Util
{
    public partial class ViewEqpConfig : UserControl
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
            Adds,
            AddFamily,
            Separator1,
            Del,
            Separator2,
            Load,
            LoadFrom,
            Separator3,
            Save,
            SaveAs,
            Separator4,
            DeviceTags
        }
        #endregion

        #region Fields
        private DesignerMode m_DesignerMode = DesignerMode.Design;
        private TreeNode m_CurNode = null;
        private TreeNode m_OldNode = null;
        private DmsComponents m_Components = null;
        private DeviceTags m_DeviceTagContainer = new DeviceTags();
        private TreeView m_TreeView = null;
        #endregion

        #region Properties
        public DesignerMode DesignerMode
        {
            get { return m_DesignerMode; }
            set { m_DesignerMode = value; }
        }
        #endregion

        public ViewEqpConfig()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize(DmsComponents components)
        {
            m_Components = components;

            UpdateFileName();

            Cursor.Current = Cursors.WaitCursor;

            treeView1.BeginUpdate();
            object topnodetag = (treeView1.TopNode != null) ? treeView1.TopNode.Tag : null;
            treeView1.Nodes.Clear();

            TreeNode node1 = new TreeNode();
            node1.Tag = components;
            node1.Text = "Atomic Device";//node1.Tag.GetType().Name;

            foreach (IGenericCollection collection in components.ContainedItems)
            {
                if (collection.ContainedItemModel != ModelType.Atomic) continue;

                TreeNode node2 = new TreeNode();
                node2.Tag = collection;
                node2.Text = collection.Name;
                if (collection.Count > 0)
                {
                    node2.Text += (" (" + collection.Count.ToString() + ")");
                }

                foreach (Object obj in collection)
                {
                    TreeNode node3 = new TreeNode();
                    node3.Tag = obj;
                    node3.Text = obj.ToString();
                    node2.Nodes.Add(node3);
                }
                node1.Nodes.Add(node2);

                collection.SetComponentContainer(components.ComponentContainer);
            }

            treeView1.Nodes.Add(node1);
            treeView1.Nodes[0].Expand();
            treeView1.EndUpdate();

            TreeNode topnode = FindNode(treeView1, topnodetag);
            if (topnode != null) treeView1.TopNode = topnode;

            treeView2.BeginUpdate();
            object topnodetag2 = (treeView2.TopNode != null) ? treeView2.TopNode.Tag : null;
            treeView2.Nodes.Clear();

            node1 = new TreeNode();
            node1.Tag = components;
            node1.Text = "Coupled Device";//node1.Tag.GetType().Name;

            foreach (IGenericCollection collection in components.ContainedItems)
            {
                if (collection.ContainedItemModel != ModelType.Coupled) continue;

                TreeNode node2 = new TreeNode();
                node2.Tag = collection;
                node2.Text = collection.Name;
                if (collection.Count > 0)
                {
                    node2.Text += (" (" + collection.Count.ToString() + ")");
                }

                foreach (Object obj in collection)
                {
                    TreeNode node3 = new TreeNode();
                    node3.Tag = obj;
                    node3.Text = obj.ToString();
                    node2.Nodes.Add(node3);
                }
                node1.Nodes.Add(node2);

                collection.SetComponentContainer(components.ComponentContainer);
            }

            treeView2.Nodes.Add(node1);
            treeView2.Nodes[0].Expand();
            treeView2.EndUpdate();

            TreeNode topnode2 = FindNode(treeView2, topnodetag);
            if (topnode2 != null) treeView2.TopNode = topnode2;

            if (m_DesignerMode == DesignerMode.Design)
            {
                UpdateTagConatiner();
            }

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

        private void UpdateFileName()
        {
            this.textBoxFileName.Text = "";
            this.textBoxFileName.AppendText(m_Components.ComponentContainer.FileName);
        }

        private void ViewEqpConfig_Load(object sender, EventArgs e)
        {
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_TreeView = sender as TreeView;
            m_CurNode = e.Node;

            MouseButtons button = e.Button;
            if (button == MouseButtons.Right)
            {
                if (m_CurNode.ContextMenuStrip == null)
                {
                    m_CurNode.ContextMenuStrip = this.contextMenuStrip1;
                }
                m_CurNode.ContextMenuStrip.Enabled = (m_DesignerMode == DesignerMode.Design);

                if (m_CurNode.Level == (int)ObjLevel.Top)
                {
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Adds].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.AddFamily].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
                }
                else if (m_CurNode.Level == (int)ObjLevel.Group)
                {
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = true;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Adds].Enabled = true;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.AddFamily].Enabled = true;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = false;
                }
                else if (m_CurNode.Level == (int)ObjLevel.Item)
                {
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Adds].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.AddFamily].Enabled = false;
                    m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Del].Enabled = true;
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Initialize(m_Components);


            if (m_CurNode.Parent.Text == m_TreeView.Nodes[0].Text)
            {
                m_TreeView.Nodes[0].Nodes[m_CurNode.Index].Expand();
            }
            else
            {
                m_TreeView.Nodes[0].Nodes[m_CurNode.Parent.Index].Expand();
            }

            if (m_OldNode != null)
            {
                if (m_OldNode.Level == (int)ObjLevel.Item)
                    m_OldNode = m_TreeView.Nodes[0].Nodes[m_OldNode.Parent.Index].Nodes[m_OldNode.Index];
                else if (m_OldNode.Level == (int)ObjLevel.Group)
                    m_OldNode = m_TreeView.Nodes[0].Nodes[m_OldNode.Index];
                else if (m_OldNode.Level == (int)ObjLevel.Top)
                    m_OldNode = m_TreeView.Nodes[0];
                this.m_OldNode.ForeColor = Color.Black;
                this.m_CurNode = m_OldNode;
                this.m_CurNode.ForeColor = Color.Red;
            }
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            ICollectionFactory factory = m_CurNode.Tag as ICollectionFactory;
            _Device item = factory.CreateObject();
            if (item == null)
            {
                //MessageBox.Show("Can not add new instance : abstract type, see the Add Family types!");
                toolStripMenuItemAddFamilyType.PerformClick();
                return;
            }

            factory.Add(item);

            TreeNode node = new TreeNode();
            node.Tag = item;
            m_CurNode.Nodes.Add(node);

            UpdateContainer(this.m_TreeView.Nodes);

            Initialize(m_Components);

            m_TreeView.Nodes[0].Nodes[m_CurNode.Index].Expand();
        }

        public void UpdateContainer(TreeNodeCollection nodes)
        {
            foreach (TreeNode node1 in nodes)
            {
                foreach (TreeNode node2 in node1.Nodes)
                {
                    int id = 0;
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        (node3.Tag as _Device).Id = id++;
                    }
                }
            }
        }

        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            (m_CurNode.Parent.Tag as IGenericCollection).Remove(m_CurNode.Tag);

            TreeNode parentNode = m_CurNode.Parent;
            m_CurNode.Remove();
            Initialize(m_Components);

            m_TreeView.Nodes[0].Nodes[parentNode.Index].Expand();
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save ?", "WSSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                UpdateContainer(this.treeView1.Nodes);
                UpdateContainer(this.treeView2.Nodes);
                if (m_Components.WriteXml())
                {
                    m_Components.WirteCsharpCode();
                    m_DeviceTagContainer.WriteXml();

                    UpdateFileName();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void toolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            if (m_Components.ReadXml())
            {
                Initialize(m_Components);
            }
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.ExpandAllGridItems();
        }

        private void propertyGrid1_Leave(object sender, EventArgs e)
        {
            //Initialize(m_Pool);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = m_Components.ComponentContainer.GetType().Name + ".xml";
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    UpdateContainer(this.treeView1.Nodes);
                    UpdateContainer(this.treeView2.Nodes);
                    if (m_Components.WriteXml(dlg.FileName))
                    {
                        m_Components.WirteCsharpCode();
                        m_DeviceTagContainer.WriteXml();

                        UpdateFileName();
                    }
                    Cursor.Current = Cursors.Default;

                }
                catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
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
                    m_Components.ReadXml(dlg.FileName);
                    Initialize(m_Components);
                }
                catch (Exception err)    //Don't Use XFunc.ExceptionHandler.Add(err);
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void deviceTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDeviceTags form = new FormDeviceTags();
            form.Initialize(m_DeviceTagContainer);
            form.ShowDialog();
        }

        public void UpdateTagConatiner()
        {
            m_DeviceTagContainer.Items.Clear();

            GenInfoCollection.BuildTags(m_DeviceTagContainer);

            foreach (IGenericCollection collection in this.m_Components.ContainedItems)
            {
                foreach (_Device device in collection)
                {
                    device.CreateTag(m_DeviceTagContainer);
                }
            }
        }

        private void ToolStripMenuItemAdds_Click(object sender, EventArgs e)
        {
            ICollectionFactory factory = m_CurNode.Tag as ICollectionFactory;

            FormAdds dlgAdds = new FormAdds();
            dlgAdds.ShowDialog();
            if (dlgAdds.DialogResult != DialogResult.OK)
            {
                return;
            }
            else
            {
                int count = dlgAdds.Count;
                for (int i = 0; i < count; i++)
                {
                    object item = factory.CreateObject();
                    if (item == null)
                    {
                        MessageBox.Show("Can not add new instance : abstract type, see the Add Family types!");
                        return;
                    }

                    factory.Add(item);

                    TreeNode node = new TreeNode();
                    node.Tag = item;
                    m_CurNode.Nodes.Add(node);
                }

                UpdateContainer(this.m_TreeView.Nodes);

                Initialize(m_Components);

                m_TreeView.Nodes[0].Nodes[m_CurNode.Index].Expand();
            }
        }

        private void addFamilyTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGenericCollection collection = m_CurNode.Tag as IGenericCollection;
            FormAddFamilyType form = new FormAddFamilyType();
            form.SourceType = collection.ContainedItemType;
            form.ShowDialog();
            if (form.DialogResult != DialogResult.OK)
            {
                return;
            }
            else
            {
                foreach (object obj in form.CreatedInstance)
                {
                    collection.Add(obj);
                    TreeNode node = new TreeNode();
                    node.Tag = obj;
                    m_CurNode.Nodes.Add(node);
                }

                UpdateContainer(this.m_TreeView.Nodes);

                Initialize(m_Components);

                m_TreeView.Nodes[0].Nodes[m_CurNode.Index].Expand();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_CurNode = e.Node;
            this.propertyGrid1.SelectedObject = m_CurNode.Tag;

            if (m_OldNode != null) this.m_OldNode.ForeColor = Color.Black;
            this.m_CurNode.ForeColor = Color.Red;
            this.m_OldNode = m_CurNode;
        }
    }
}

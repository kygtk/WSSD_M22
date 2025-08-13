using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class ViewDmsNodeTagSelect : UserControl
    {
        #region Enum
        enum ObjLevel
        {
            Top,
            Group,
            Item
        } 
        #endregion

        #region Fields
        private DmsNodeTags m_tags = null;
        private DmsNodeTag m_SelectedTag = null;
        private DmsNodeTag m_curTag = null;
        private DmsNodeTag m_TargetTag = null;
        private static string m_FilterString = "";
        private static bool m_FilterOn = false;
        #endregion

        #region Properties
        public DmsNodeTag CurTag
        {
            get { return m_curTag; }
            set { m_curTag = value; }
        } 
        #endregion

        #region Constructor
        public ViewDmsNodeTagSelect()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        #region Methods
        public void InitializeCurrentTagTree(DmsNodeTag tag)
        {
            if (tag != null)
            {
                m_curTag = tag;
            }

            treeViewCurrentTag.BeginUpdate();
            TreeNode node1 = new TreeNode();
            node1.Text = "Current Tag";
            treeViewCurrentTag.Nodes.Add(node1);
            treeViewCurrentTag.ExpandAll();
            treeViewCurrentTag.EndUpdate();

            UpdateCurrentTagTree(m_curTag);
        }

        private void InitializeTagsTree(DmsNodeTags tags)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeViewTags.BeginUpdate();

            m_tags = tags;

            treeViewTags.Nodes.Clear();

            TreeNode node1 = new TreeNode();
            node1.Text = "Tags";

            foreach (DmsNodeTag tag in m_tags.Items)
            {
                TreeNode node2 = new TreeNode();
                node2.Tag = (object)tag;
                node2.Text = tag.Name;
                node1.Nodes.Add(node2);
            }

            node1.Expand();

            treeViewTags.Nodes.Add(node1);
            //treeViewTags.ExpandAll();
            treeViewTags.EndUpdate();

            Cursor.Current = Cursors.Default;
        }

        public void InitializeTagsTreeByFilter(DmsNodeTag deviceTag, params string[] keys)
        {
            m_TargetTag = deviceTag;

            DmsNodeTags source = new DmsNodeTags();
            DmsNodeTags target = new DmsNodeTags();

            source.ReadXml();

            if (deviceTag == null)
            {
                target = source;
            }
            else
            {
                //source = source.GetTags(deviceTag.DeviceType);
                //source = source.GetFamilyTags(deviceTag.FamilyType);

                foreach (DmsNodeTag item in source.Items)
                {
                    bool matchAll = true;
                    foreach (string key in keys)
                    {
                        matchAll &= item.Name.ToLower().Contains(key.ToLower());
                    }

                    if (matchAll)
                    {
                        target.Add(item);
                    }
                }
            }

            if (target.Items.Count == 0)
            {
                target = source;
            }

            InitializeTagsTree(target);
        }

        private void treeViewTags_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;

            if (curNode.Level == (int)ObjLevel.Top)
            {
                this.buttonAdd.Enabled = false;
            }
            else if (curNode.Level == (int)ObjLevel.Group)
            {
                m_SelectedTag = (DmsNodeTag)curNode.Tag;
                propertyGrid1.SelectedObject = (object)m_SelectedTag;
                this.buttonAdd.Enabled = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (m_SelectedTag == null) return;

            if (m_curTag != null)
            {
                MessageBox.Show("Current tag is alreay contained! Remove current tag first.");
            }
            else
            {
                m_curTag = m_SelectedTag;
                UpdateCurrentTagTree(m_curTag);
            }
        }

        private void UpdateCurrentTagTree(DmsNodeTag curTag)
        {
            treeViewCurrentTag.BeginUpdate();
            treeViewCurrentTag.Nodes[0].Nodes.Clear();

            if (curTag != null)
            {
                TreeNode node = new TreeNode();
                node.Tag = (object)curTag;
                node.Text = curTag.Name;

                treeViewCurrentTag.Nodes[0].Nodes.Add(node);
            }

            treeViewCurrentTag.ExpandAll();
            treeViewCurrentTag.EndUpdate();
        }

        private void treeViewCurrentTag_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;

            if (curNode.Tag == null) return;

            if (curNode.Level == (int)ObjLevel.Top)
            {
                this.buttonRemove.Enabled = false;
            }
            else
            {
                this.propertyGrid1.SelectedObject = curNode.Tag;
                this.buttonRemove.Enabled = true;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            m_curTag = null;
            UpdateCurrentTagTree(m_curTag);
            this.buttonRemove.Enabled = false;
        }
        #endregion

        private void ViewTagSelect_Load(object sender, EventArgs e)
        {
            this.textBoxFilter.Text = m_FilterString;
            this.checkBoxFilterOn.Checked = m_FilterOn;
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

            InitializeTagsTreeByFilter(m_TargetTag, keys);
        }
    }
}

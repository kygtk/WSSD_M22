using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
    public partial class ViewTagContainer : UserControl
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
        private DeviceTags m_tags = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ViewTagContainer()
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
        public void Initialize(DeviceTags tags)
        {
            m_tags = tags;
            
            Cursor.Current = Cursors.WaitCursor;
            treeViewTags.BeginUpdate();    
        
            treeViewTags.Nodes.Clear();

            TreeNode node1 = new TreeNode();
            node1.Text = "DeviceTags";

            foreach (DeviceTag tag in m_tags.Items)
            {
                TreeNode node2 = new TreeNode();
                node2.Tag = (object)tag;
                node2.Text = tag.DeviceName;
                node1.Nodes.Add(node2);
                foreach (GenericTag genTag in tag.Items)
                {
                    TreeNode node3 = new TreeNode();
                    node3.Tag = (object)genTag;
                    node3.Text = genTag.Key + " : " + genTag.Value;
                    node2.Nodes.Add(node3);
                }
            }

            treeViewTags.Nodes.Add(node1);
            treeViewTags.ExpandAll();
            treeViewTags.EndUpdate();

            Cursor.Current = Cursors.Default;
        }

        public void UpdateTree()
        {
            Cursor.Current = Cursors.WaitCursor;
            treeViewTags.BeginUpdate();    

            foreach (TreeNode node1 in this.treeViewTags.Nodes)
            {
                foreach(TreeNode node2 in node1.Nodes)
                {
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        GenericTag tag = node3.Tag as GenericTag;
                        node3.Text = tag.Key + " : " + tag.Value;
                    }
                }
            }

            treeViewTags.EndUpdate();
            Cursor.Current = Cursors.Default;
        }


        private void treeViewTags_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode= e.Node;

            propertyGrid1.SelectedObject = curNode.Tag;

            //if (curNode.Level == (int)ObjLevel.Top)
            //{
            //}
            //else if(curNode.Level == (int)ObjLevel.Group)
            //{
            //    propertyGrid1.SelectedObject = curNode.Tag;
            //}
        }
        #endregion
    }
}

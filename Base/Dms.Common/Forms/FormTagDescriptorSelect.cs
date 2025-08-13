using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Dms.Common
{
    public partial class FormTagDescriptorSelect : Form
    {
        #region Fields
        //private static List<TagDescriptors> m_Types;
        private TagDescriptor m_SelectedTagDescriptor;
        private TagDescriptors m_TagDescriptors;
        private DeviceTagInfo m_DeviceTagInfo;
        private DeviceTag m_DeviceTag;
        private DeviceTags m_DeviceTagContainer;
        #endregion

        #region Properties
        public TagDescriptor SelectedTagDescriptor
        {
            get { return m_SelectedTagDescriptor; }
            set { m_SelectedTagDescriptor = value; }
        }
        public DeviceTagInfo DeviceTagInfo
        {
            get { return m_DeviceTagInfo; }
            set { m_DeviceTagInfo = value; }        
        }
        #endregion
        
        public FormTagDescriptorSelect()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void FormTagDescriptorSelect_Load(object sender, EventArgs e)
        {
            //if (m_Types == null)
            //{
            //    m_Types = new List<TagDescriptors>();
            //    Assembly asm = Assembly.Load("Dms.Common");
            //    Type[] types = asm.GetTypes();
            //    Type sourceType = typeof(TagDescriptors);
            //    foreach (Type type in types)
            //    {
            //        if (XFunc.CheckTypeCompatibility(sourceType, type, Compatibility.Compatible))
            //        {
            //            if (!type.IsAbstract && type != sourceType)
            //            {
            //                m_Types.Add(Activator.CreateInstance(type) as TagDescriptors);
            //            }
            //        }
            //    }
            //}

            if (m_DeviceTagInfo != null)
            {
                m_DeviceTagContainer = new DeviceTags();
                m_DeviceTagContainer.ReadXml();
                m_DeviceTag = m_DeviceTagContainer[m_DeviceTagInfo.DeviceName];

                m_TagDescriptors = new TagDescriptors();
                int count = m_DeviceTag.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    m_TagDescriptors.Items.Add(new TagDescriptor(i, m_DeviceTag.Items[i].Key));
                }

                Initialize();
            }
        }

        private void Initialize()
        {
            //Cursor.Current = Cursors.WaitCursor;
            //treeView1.BeginUpdate();

            //treeView1.Nodes.Clear();

            //TreeNode node1 = new TreeNode();
            //node1.Tag = m_Types;
            //node1.Text = "Tag Descriptors";

            //foreach (TagDescriptors tagDescriptors in m_Types)
            //{
            //    TreeNode node2 = new TreeNode();
            //    node2.Tag = tagDescriptors;
            //    node2.Text = tagDescriptors.GetType().Name.Replace("TagDescriptor", "");

            //    foreach (TagDescriptor tagDescriptor in tagDescriptors)
            //    {
            //        TreeNode node3 = new TreeNode();
            //        node3.Tag = tagDescriptor;
            //        node3.Text = tagDescriptor.ToString();
            //        node2.Nodes.Add(node3);
            //    }
            //    node1.Nodes.Add(node2);
            //}

            //treeView1.Nodes.Add(node1);
            //treeView1.Nodes[0].Expand();
            //treeView1.EndUpdate();

            //Cursor.Current = Cursors.Default;

            Cursor.Current = Cursors.WaitCursor;
            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();

            TreeNode node1 = new TreeNode();
            node1.Tag = m_TagDescriptors;
            node1.Text = m_DeviceTag.DeviceName;

            foreach (TagDescriptor tagDescriptor in m_TagDescriptors)
            {
                TreeNode node2 = new TreeNode();
                node2.Tag = tagDescriptor;
                node2.Text = tagDescriptor.ToString();
                node1.Nodes.Add(node2);
            }

            treeView1.Nodes.Add(node1);
            treeView1.Nodes[0].Expand();
            treeView1.EndUpdate();

            Cursor.Current = Cursors.Default;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode curNode = e.Node;
            TagDescriptor sel = curNode.Tag as TagDescriptor;

            if (sel != null)
            {
                m_SelectedTagDescriptor = sel;
                string parentName = curNode.Parent.Text;
                this.label1.Text = " Selected : " + parentName + " - " + sel.Key;
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (m_SelectedTagDescriptor == null)
            {
                MessageBox.Show("Select TagDescriptor!");
                return;
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            m_SelectedTagDescriptor = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
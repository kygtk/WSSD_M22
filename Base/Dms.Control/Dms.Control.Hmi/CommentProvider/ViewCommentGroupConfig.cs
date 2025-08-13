using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control.Hmi
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
		Separator1,
		Edit,
		Separator2,
		Load,
		Separator3,
		Save,
	}
	#endregion

	public partial class ViewCommentGroupConfig : UserControl
	{
		#region Fields
		private DesignerMode m_DesignerMode = DesignerMode.Design;
		private TreeNode m_CurNode = null;
		private TreeNode m_OldNode = null;
		private TreeView m_TreeView = null;
		private HmiCommentProvider m_CommentProvider = null;
		#endregion

		public ViewCommentGroupConfig()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.CacheText, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		public void Initialize(HmiCommentProvider provider)
		{
			HmiCommentProvider.SortByGroupNo(provider);

			m_CommentProvider = provider;

			UpdateFileName();

			Cursor.Current = Cursors.WaitCursor;

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();

			TreeNode node1 = new TreeNode();
			node1.Tag = provider;
			node1.Text = "Comment Groups";

			foreach (HmiCommentGroup group in provider.CommentGroups)
			{
				TreeNode node2 = new TreeNode();
				node2.Tag = group;
				node2.Text = group.ToString();
				node1.Nodes.Add(node2);
			}

			treeView1.Nodes.Add(node1);
			treeView1.Nodes[0].Expand();
			treeView1.EndUpdate();

			Cursor.Current = Cursors.Default;
		}

		private void UpdateFileName()
		{
			this.textBoxFileName.Text = "";
			this.textBoxFileName.AppendText(m_CommentProvider.FileName);
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			m_TreeView = sender as TreeView;
			m_CurNode = e.Node;

			this.propertyGrid1.SelectedObject = m_CurNode.Tag;

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
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Edit].Enabled = false;
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Load].Enabled = true;
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Save].Enabled = true;
				}
				else if (m_CurNode.Level == (int)ObjLevel.Group)
				{
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Add].Enabled = false;
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Edit].Enabled = false;
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Load].Enabled = false;
					m_CurNode.ContextMenuStrip.Items[(int)ContextStripId.Save].Enabled = false;
				}
			}

			if (m_OldNode != null) this.m_OldNode.ForeColor = Color.Black;
			this.m_CurNode.ForeColor = Color.Red;
			this.m_OldNode = m_CurNode;
		}

		private void addToolStripMenuItem_Click(object sender, EventArgs e)
		{
			

		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DialogResult.Yes == MessageBox.Show("Do you really want to load?", "Load from File", MessageBoxButtons.YesNo))
			{
				if (m_CommentProvider.ReadFromStorage())
				{
					Initialize(m_CommentProvider);
					MessageBox.Show("Load Complete");
				}
				else
				{
					MessageBox.Show("Load Fail");
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DialogResult.Yes == MessageBox.Show("Do you really want to save?", "Save to File", MessageBoxButtons.YesNo))
			{
				if (m_CommentProvider.WriteToStorage())
				{
					MessageBox.Show("Save Complete");
				}
				else
				{
					MessageBox.Show("Save Fail");
				}
			}
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			Initialize(m_CommentProvider);

			m_TreeView.Nodes[0].Expand();

			if (m_OldNode != null)
			{
				if (m_OldNode.Level == (int)ObjLevel.Group)
					m_OldNode = m_TreeView.Nodes[0].Nodes[m_OldNode.Index];
				else if (m_OldNode.Level == (int)ObjLevel.Top)
					m_OldNode = m_TreeView.Nodes[0];
				this.m_OldNode.ForeColor = Color.Black;
				this.m_CurNode = m_OldNode;
				this.m_CurNode.ForeColor = Color.Red;
			}
		}

		private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
		{
			this.propertyGrid1.ExpandAllGridItems();
		}
	}
}

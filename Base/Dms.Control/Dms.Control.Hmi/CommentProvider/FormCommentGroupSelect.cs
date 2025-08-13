using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control.Hmi
{
	public partial class FormCommentGroupSelect : Form
	{
		#region Fields
		private HmiCommentGroup m_SelectedGroup = null;
		private HmiCommentProvider m_Provider = HmiCommentProvider.Instance;
		private List<HmiCommentGroup> m_CommentGroupList = null;
		private string m_OwnerName = "";
		private List<string> m_Names;
		private readonly string m_Splitter = "\t";
		#endregion

		public HmiCommentGroup SelectedGroup
		{
			get { return m_SelectedGroup; }
		}

		public string OwnerName
		{
			get { return m_OwnerName; }
			set { m_OwnerName = value; }
		}

		#region Constructor
		public FormCommentGroupSelect()
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
		public void Initialize(HmiCommentGroup group)
		{
			//this.Text = " * " + m_OwnerName + " *";

			m_Provider.ReadFromStorage();
			List<HmiCommentGroup> commentGroupList = m_Provider.CommentGroups;
			m_CommentGroupList = new List<HmiCommentGroup>();
			m_Names = new List<string>();

			//if (m_SelectedGroup == null) return;

			foreach (HmiCommentGroup comment in commentGroupList)
			{
				m_CommentGroupList.Add(comment);
				m_Names.Add(string.Format("{0}{1}{2}", comment.GroupNo.ToString(), m_Splitter, comment.GroupName));
			}

			this.viewCommentGroupSelect1.Initialize(m_Names);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			m_SelectedGroup.GroupNo = 0;
			m_SelectedGroup.GroupName = "";
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			string name = this.viewCommentGroupSelect1.SelectedName;
			if (name != null)
			{
				m_SelectedGroup = new HmiCommentGroup();
				m_SelectedGroup.GroupNo = Convert.ToUInt16(name.Substring(0, name.IndexOf(m_Splitter)));
				m_SelectedGroup.GroupName = name.Substring(name.IndexOf(m_Splitter) + 1);
				this.DialogResult = DialogResult.OK;
			}
			this.Close();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			FormCommentGroupConfig form = new FormCommentGroupConfig();
			form.Initialize(m_Provider);
			if (form.ShowDialog() == DialogResult.OK)
			{
				Initialize(m_SelectedGroup);
			}
		}
		#endregion
	}
}
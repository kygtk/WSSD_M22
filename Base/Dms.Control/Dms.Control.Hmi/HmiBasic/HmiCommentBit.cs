///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : Bit type Comment UserControl for HMI
//-------------------------------------------------------------------------
// Revison History
// * 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.Control.Hmi
{
	public partial class HmiCommentBit : DmsHmiComponent
	{
		#region Fields
		//On
		private string m_OnComment = "";
		private Font m_OnFont = HmiDefaultFont;
		private Color m_OnForeColor = Color.Black;
		private Color m_OnBackColor = Color.White;
		//Off
		private string m_OffComment = "";
		private Font m_OffFont = HmiDefaultFont;
		private Color m_OffForeColor = Color.Black;
		private Color m_OffBackColor = Color.White;
		//Data
		private CommentType m_CommentType = CommentType.Group;
		private IoDigitalInput m_DiCondition;
		private HmiCommentGroupInfo m_CommentGroupInfo = null;
		private HmiCommentGroup m_CommentGroup;
		private bool m_ChangeCommentStyle = false;
		#endregion

		#region Properties
		//UI
		[Category("DMS : !UI"), Description("Text : 직접입력, CommentGroup : From data group")]
		public CommentType CommentType
		{
			get { return m_CommentType; }
			set { m_CommentType = value; }
		}
		[Category("DMS : !UI"), Description("set border style")]
		public BorderStyle CommentBorderStyle
		{
			get { return this.lblComment.BorderStyle; }
			set { this.lblComment.BorderStyle = value; }
		}
		[Category("DMS : !UI"), Description("set text alignment")]
		public ContentAlignment CommentTextAlign
		{
			get { return this.lblComment.TextAlign; }
			set { this.lblComment.TextAlign = value; }
		}
		//Comment Text : For ON status
		[Category("DMS : Comment Text - ON"), Description("write comment")]
		public string OnComment
		{
			get { return m_OnComment; }
			set { m_OnComment = value; }
		}
		[Category("DMS : Comment Text - ON"), Description("Set font")]
		public Font OnFont
		{
			get { return m_OnFont; }
			set { m_OnFont = value; }
		}
		[Category("DMS : Comment Text - ON"), Description("set text color")]
		public Color OnForeColor
		{
			get { return m_OnForeColor; }
			set { m_OnForeColor = value; }
		}
		[Category("DMS : Comment Text - ON"), Description("set background color")]
		public Color OnBackColor
		{
			get { return m_OnBackColor; }
			set { m_OnBackColor = value; }
		}
		//Comment Text : For OFF status
		[Category("DMS : Comment Text - OFF"), Description("write comment")]
		public string OffComment
		{
			get { return m_OffComment; }
			set { m_OffComment = value; }
		}
		[Category("DMS : Comment Text - OFF"), Description("Set font")]
		public Font OffFont
		{
			get { return m_OffFont; }
			set { m_OffFont = value; }
		}
		[Category("DMS : Comment Text - OFF"), Description("set text color")]
		public Color OffForeColor
		{
			get { return m_OffForeColor; }
			set { m_OffForeColor = value; }
		}
		[Category("DMS : Comment Text - OFF"), Description("set background color")]
		public Color OffBackColor
		{
			get { return m_OffBackColor; }
			set { m_OffBackColor = value; }
		}
		//Comment Group
		[Category("DMS : Comment Group"), Description("Change Comment style : for Comment group")]
		public bool ChangeCommentStyle
		{
			get { return m_ChangeCommentStyle; }
			set { m_ChangeCommentStyle = value; }
		}
		[Category("DMS : Comment Group"), Description("Select Comment group")]
		public HmiCommentGroupInfo CommentGroupInfo
		{
			get { return m_CommentGroupInfo; }
			set { m_CommentGroupInfo = value; }
		}
		//DataPoint
		[Category("DMS : Data Point"), Description("Select Device for Condition Number")]
		public IoDigitalInput DiCondition
		{
			get { return m_DiCondition; }
			set { m_DiCondition = value; }
		}
		#endregion

		#region Constructor
		public HmiCommentBit()
		{
			InitializeComponent();
		}
		#endregion

		#region Methods
		private int m_OldConditionId = -1;
		private void UpdateComment()
		{
			int conditionId = m_DiCondition.GetState() ? 1 : 0;
			if (conditionId < 0) conditionId = 0;

			if (m_OldConditionId != conditionId)
			{
				m_OldConditionId = conditionId;

				if (m_CommentType == CommentType.Text)
				{
					UpdateComment(conditionId);
				}
				else
				{
					HmiComment comment = m_CommentGroup[conditionId];
					UpdateComment(comment);
				}
			}
		}

		private void UpdateComment(int conditionId)
		{
			if (conditionId == 0)
			{
				this.lblComment.Text = m_OffComment;
				this.lblComment.Font = m_OffFont;
				this.lblComment.ForeColor = m_OffForeColor;
				this.lblComment.BackColor = m_OffBackColor;
			}
			else
			{
				this.lblComment.Text = m_OnComment;
				this.lblComment.Font = m_OnFont;
				this.lblComment.ForeColor = m_OnForeColor;
				this.lblComment.BackColor = m_OnBackColor;
			}
		}

		private void UpdateComment(HmiComment comment)
		{
			this.lblComment.Text = comment.Text;
			if (!m_ChangeCommentStyle)
			{
				this.lblComment.Font = comment.CommentFont;
				this.lblComment.ForeColor = comment.CommentColor;
				this.lblComment.BackColor = comment.CommentBackColor;
			}
		}
		#endregion

		#region Override
		public override bool Initialize()
		{
			bool ok = base.Initialize();

			//Find Data point
			ok &= (m_DiCondition != null);
			if (!ok)
			{
				string msg = string.Format("Data point of {0} does not exist in {1}!", this.Name, this.Parent.Name);
				MessageBox.Show(msg);
				ok = false;
			}

			if (ok)
			{
				if (m_CommentType == CommentType.Group)
				{
					//CommentGroupInfo가 꼭 설정 되어있어야 한다.
					ok &= (m_CommentGroupInfo != null);
					if (ok)
					{
						//Find Comment Group
						m_CommentGroup = HmiCommentProvider.Instance[m_CommentGroupInfo.GroupName];
						ok &= m_CommentGroup != null;
					}

					if(!ok)
					{
						string msg = string.Format("Comment group of {0} does not defined in {1}!", this.Name, this.Parent.Name);
						MessageBox.Show(msg);
						ok = false;
					}
				}
			}

			if (ok)
			{
				m_Initialized = ok;

				tmrUpdateState.Enabled = m_Initialized;
			}

			return m_Initialized;
		}

		protected override void UpdateState()
		{
			if (!m_Initialized) return;

			UpdateComment();
		}
		#endregion
	}
}

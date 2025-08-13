///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : Simple Comment UserControl for HMI
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
	public partial class HmiCommentSimple : DmsHmiComponent
	{
		#region Fields
		private CommentType m_CommentType = CommentType.Text;
		private bool m_ChangeCommentStyle = false;
		private HmiCommentInfo m_CommentInfo;
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
		//Comment Text
		[Category("DMS : Comment Text"), Description("Set text")]
		public string CommentText
		{
			get { return this.lblComment.Text; }
			set { this.lblComment.Text = value; }
		}
		[Category("DMS : Comment Text"), Description("Set font")]
		public Font CommentFont
		{
			get { return this.lblComment.Font; }
			set { this.lblComment.Font = value; }
		}
		[Category("DMS : Comment Text"), Description("set text color")]
		public Color CommentForeColor
		{
			get { return this.lblComment.ForeColor; }
			set { this.lblComment.ForeColor = value; }
		}
		[Category("DMS : Comment Text"), Description("set background color")]
		public Color CommentBackColor
		{
			get { return this.lblComment.BackColor; }
			set { this.lblComment.BackColor = value; }
		}
		//Comment Group
		[Category("DMS : Comment Group"), Description("Change Comment style : for Comment group")]
		public bool ChangeCommentStyle
		{
			get { return m_ChangeCommentStyle; }
			set { m_ChangeCommentStyle = value; }
		}
		[Category("DMS : Comment Group"), Description("set comment group")]
		public HmiCommentInfo CommentInfo
		{
			get { return m_CommentInfo; }
			set { m_CommentInfo = value; }
		}
		#endregion

		#region Constructor
		public HmiCommentSimple()
		{
			InitializeComponent();
		}
		#endregion

		#region Methods
		private void UpdateComment(HmiComment comment)
		{
			this.CommentText = comment.Text;
			if (!m_ChangeCommentStyle)
			{
				this.CommentFont = comment.CommentFont;
				this.CommentForeColor = comment.CommentColor;
				this.CommentBackColor = comment.CommentBackColor;
			}
		}
		#endregion

		#region Override
		public override bool Initialize()
		{
			bool ok = base.Initialize();

			//Comment group에서 읽어와야 하는 경우에만
			if (m_CommentType == CommentType.Group)
			{
				//CommentInfo가 꼭 설정 되어있어야 한다.
				ok &= (m_CommentInfo != null);
				
				if (ok)
				{
					//Find Comment Group
					HmiCommentGroup commentGroup = HmiCommentProvider.Instance[m_CommentInfo.GroupInfo.GroupName];
					if (commentGroup == null)
					{
						ok = false;
					}
					else
					{
						//Find Comment
						HmiComment comment = commentGroup[m_CommentInfo.Comment.CommentNo - 1];

						//Update UI
						UpdateComment(comment);
					}
				}

				if(!ok)
				{
					string msg = string.Format("Comment group of {0} does not defined in {1}!", this.Name, this.Parent.Name);
					MessageBox.Show(msg);
					ok = false;
				}
			}

			if (ok)
			{
				m_Initialized = ok;
				
				//Timer 돌릴 필요가 없다
				tmrUpdateState.Enabled = false;
			}

			return m_Initialized;
		}
		#endregion
	}
}

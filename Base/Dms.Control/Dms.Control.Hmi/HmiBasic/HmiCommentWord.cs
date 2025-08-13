///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : Word type Comment UserControl for HMI
//-------------------------------------------------------------------------
// Revison History
// * 2010.01.14 : jemoon - Text 타입 지원하지 않음

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
	public partial class HmiCommentWord : DmsHmiComponent
	{
		#region Fields
		private CommentType m_CommentType = CommentType.Group;
		private IoAnalogInput m_AiConditionNo;
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
		
		//Comment Text
		//Todo

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
		[Category("DMS : Data Point"), Description("Select Device for comment Number")]
		public IoAnalogInput AiConditionNo
		{
			get { return m_AiConditionNo; }
			set { m_AiConditionNo = value; }
		}
		#endregion

		#region Constructor
		public HmiCommentWord()
		{
			InitializeComponent();
		}
		#endregion

		#region Methods
		private int m_OldConditionId = -1;
		private void UpdateComment()
		{
			int conditionId = (int)m_AiConditionNo.GetState() - 1;
			if (conditionId < 0) conditionId = 0;

			if (m_OldConditionId != conditionId)
			{
				m_OldConditionId = conditionId;

				if (m_CommentType == CommentType.Text)
				{
					SetComment(conditionId);
				}
				else
				{
					HmiComment comment = m_CommentGroup[conditionId];
					SetComment(comment);
				}
			}
		}

		private void SetComment(int conditionId)
		{ 
		}

		private void SetComment(HmiComment comment)
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
			ok &= (m_AiConditionNo != null);
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

					if (!ok)
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

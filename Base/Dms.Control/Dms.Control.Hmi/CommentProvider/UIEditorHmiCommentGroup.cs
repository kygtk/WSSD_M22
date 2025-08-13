using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel.Design;
//using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Dms.Control.Hmi
{
	public class HmiCommentGroupsEditor : CollectionEditor
	{
		private Type m_Type;
		private ushort m_No = 0;
		private string m_Name = "Group";

		#region Contstructor
		public HmiCommentGroupsEditor(Type itemType)
			: base(itemType)
		{
		}
		#endregion

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			m_Type = ((HmiCommentProvider)context.Instance).ContainedType;

			return base.EditValue(context, provider, value);
		}

		protected override object CreateInstance(Type itemType)
		{
			m_No = (ushort)(((HmiCommentProvider)Context.Instance).GetMaxGroupNo() + 1);
			object instance = Activator.CreateInstance(m_Type, m_No, m_Name + m_No.ToString());
			//m_No++;
			return instance;
		}
	}

	public class UIEditorHmiCommentGroup : UITypeEditor
	{
		#region Constructor
		public UIEditorHmiCommentGroup()
		{ 
		}
		#endregion

		private HmiCommentGroupsEditor _editor = new HmiCommentGroupsEditor(typeof(List<HmiCommentGroup>)); //change ArrayList to your type

		#region Override
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
		{
			if (value != null)
			{
				value = this._editor.EditValue(context, sp, value);

				List<HmiCommentGroup> list = value as List<HmiCommentGroup>;
				List<HmiCommentGroup> list2 = list.GetRange(0, list.Count);

				return list2;
			}

			return base.EditValue(context, sp, value);
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return this._editor.GetEditStyle(context);
		}
		#endregion
	}

	public class HmiCommentsEditor : CollectionEditor
	{
		private Type m_Type;
		private static int m_Count = (int)HmiLanguage.Count;

		public HmiCommentsEditor(Type itemType)
			: base(itemType)
		{ 
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			m_Type = ((HmiComment)context.Instance).ContainedType;

			return base.EditValue(context, provider, value);
		}

		protected override object CreateInstance(Type itemType)
		{
			if (((HmiComment)Context.Instance).Comments.Length < m_Count)
			{
				return base.CreateInstance(m_Type);
			}
			else return null;
		}
	}

	public class UIEditorHmiComments : UITypeEditor
	{
		public UIEditorHmiComments()
		{ 
		}

		private HmiCommentsEditor _editor = new HmiCommentsEditor(typeof(List<string>));

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (value != null)
			{
				value = this._editor.EditValue(context, provider, value);

				List<string> list = value as List<string>;
				List<string> list2 = list.GetRange(0, list.Count);

				return list2;
			}
			return base.EditValue(context, provider, value);
		}
	}

	public class UIEditorHmiCommentGroupInfoSelect : UITypeEditor
	{
		#region Constructor
		public UIEditorHmiCommentGroupInfoSelect()
		{
		}
		#endregion

		#region Override
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
		{
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc == null) return value;

			HmiCommentGroupInfo groupInfo = (HmiCommentGroupInfo)value;

			if (groupInfo == null) groupInfo = new HmiCommentGroupInfo();

			HmiCommentGroup group = new HmiCommentGroup();
			group.GroupNo = groupInfo.GroupNo;
			group.GroupName = groupInfo.GroupName;

			FormCommentGroupSelect ui = new FormCommentGroupSelect();
			ui.Initialize(group);
			edSvc.ShowDialog(ui);

			if (ui.DialogResult == DialogResult.OK)
			{
				groupInfo = new HmiCommentGroupInfo();

				if (ui.SelectedGroup != null)
				{
					groupInfo.GroupNo = ui.SelectedGroup.GroupNo;
					groupInfo.GroupName = ui.SelectedGroup.GroupName;
					value = groupInfo;
				}
				else
				{
					groupInfo.GroupName = "";
					groupInfo.GroupNo = 0;
					value = groupInfo;

				}
			}

			return value;
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
		#endregion
	}
}
